#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.Controls.Compatibility;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using Wisej.Design;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using DevExpress.Web.Internal.XmlProcessor;
using Microsoft.IdentityModel.Tokens;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FIXSSNFORM : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private PrivilegeEntity _privlagesEnty = new PrivilegeEntity();
        #endregion
        public FIXSSNFORM(BaseForm baseForm, PrivilegeEntity privileges)
        {
            propBaseForm = baseForm;
            InitializeComponent();
            this.Size = new Size(1060, 533/*588*/);
            this.Text = privileges.PrivilegeName;
            _privlagesEnty = privileges;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            ToolTip exceltool = new ToolTip();
            exceltool.SetToolTip(PbExcel, "Generate Excel");

            txtMaxim.Validator = TextBoxValidation.IntegerValidator;
            txtYear.Validator = TextBoxValidation.IntegerValidator;
            //rbClientId_CheckedChanged()
            cmbButtonsName.Items.Clear();
            rbClientId.Focus();
            #region //**Added by Vikash**\\
            lblButtonName.Text = string.Empty;
            gvwMain.Visible = false; gvwSub.Visible = false;
            pnlgvwMain.Visible = false; pnlgvwSub.Visible = false;
            btn_Masssmash.Visible = true;
            lblSSN.Visible = false; cmbSSNNumber.Visible = false; btnSubmit.Visible = false;
            chkbClientID.Visible = false; cmbClientID.Visible = false;
            btnSelecFixClientIds.Enabled = false;
            btnSelectNewClientId.Enabled = false;
            btn_Masssmash.Enabled = false;
            gvtRkey.Visible = false;
            gvcchksel.Visible = false;
            gvtSnpkey.HeaderText = "Client ID";
            gvcchksel.Visible = false;
            gvtClientId.Visible = false;
            pnlSSN.Visible = false;

            #endregion

            // cmbButtonsName.Items.Add(new ListItem("", ""));
            cmbButtonsName.Items.Add(new ListItem("Client ID with different First Name & DOB", "8"));
            cmbButtonsName.Items.Add(new ListItem("First Name & DOB with diff Client ID", "7"));
            cmbButtonsName.Items.Add(new ListItem("First Name & DOB with Empty Client ID", "10"));
            if (baseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
            {
                cmbButtonsName.Items.Add(new ListItem("ERAP duplication check", "9"));
            }
            cmbButtonsName.SelectedIndex = 0;
            pnlExcel.Visible = false;

            strBaseAgency = string.Empty;
            if (propBaseForm.BaseAdminAgency != "**")
                strBaseAgency = propBaseForm.BaseAdminAgency;

            #region //**Vikash Added**\\
            pnlSubmit.Visible = true;
            gvCLIDMain.Visible = true; pnlgvCLIDMain.Visible = true;
            gvClientSub.Visible = true; pnlgvClientSub.Visible = true;
            lblMaxRecs.Visible = false; txtMaxim.Visible = false;
            chkbClientID.Visible = false; cmbClientID.Visible = false;
            rdoDateYear.Checked = true;
            //rdoAddDate_Click(sender, e);
            gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
            gvCLIDMain.Rows.Clear(); gvClientSub.Rows.Clear(); lblCount.Text = string.Empty; chkbClientID.Checked = true;
            gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
            #endregion

            if (propBaseForm.BaseAgencyControlDetails.ClidSmash == "Y")
            {
                txtYear.Text = propBaseForm.BaseAgencyControlDetails.ClidYear;
                if (propBaseForm.BaseAgencyControlDetails.ClidFrom != string.Empty)
                {
                    dtaddFrom.Value = Convert.ToDateTime(propBaseForm.BaseAgencyControlDetails.ClidFrom);
                    if (propBaseForm.BaseAgencyControlDetails.ClidTo != string.Empty)
                        dtaddTo.Value = Convert.ToDateTime(propBaseForm.BaseAgencyControlDetails.ClidTo);
                    else
                        dtaddTo.Checked = false;
                }
                if (propBaseForm.BaseAgencyControlDetails.ClidSSN != "Y")
                    rbSSN.Visible = false;
                if (propBaseForm.BaseAgencyControlDetails.ClidClid != "Y")
                    rbClientId.Visible = false;

                if (propBaseForm.BaseAgencyControlDetails.ClidSSN == "Y" && propBaseForm.BaseAgencyControlDetails.ClidClid != "Y")
                {
                    rbSSN.Checked = true;
                    rbClientId_CheckedChanged(rbSSN, new EventArgs());
                }
                else
                {
                    if (propBaseForm.BaseAgencyControlDetails.ClidSSN != "Y" && propBaseForm.BaseAgencyControlDetails.ClidClid != "Y")
                    {

                        cmbButtonsName.Items.Clear();
                        cmbButtonsName.Items.Add(new ListItem("", ""));
                        cmbButtonsName.SelectedIndex = 0;
                        btnAllSearch.Visible = false;

                    }
                }

                if (propBaseForm.BaseAgencyControlDetails.ClidYear == string.Empty)
                {
                    rdoDateYear.Visible = false;
                    rdoAddDate.Checked = true;
                    pnladddate.Visible = true;
                    pnladdYear.Visible = false;

                }
                if (propBaseForm.BaseAgencyControlDetails.ClidFrom == string.Empty)
                {
                    rdoAddDate.Visible = false;
                }

                txtYear.Enabled = false;
                dtaddTo.Enabled = false;
                dtaddFrom.Enabled = false;
            }



        }

        private BaseForm propBaseForm { get; set; }

        private string strBaseAgency { get; set; }

        List<CaseSnpEntity> snpdetails { get; set; }


        bool ISClientID = false;
        private void gvwMain_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwMain.Rows.Count > 0)
            {
                if (gvwMain.SelectedRows[0].Selected)
                {
                    lblClientId.Text = "";
                    string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid;
                    CaseSnpEntity casesnpdata = (gvwMain.SelectedRows[0].Tag as CaseSnpEntity);
                    if (casesnpdata != null)
                    {

                        ssnno = string.Empty;
                        strclientid = string.Empty;
                        if (casesnpdata.NameixLast == "SSNNUMBER")
                        {
                            ssnno = casesnpdata.Ssno;
                            Firstname = string.Empty;
                            lastname = string.Empty;
                            dateofbirth = string.Empty;
                            cmbClientID.Visible = false;
                            chkbClientID.Visible = false;
                        }
                        else
                        {
                            Firstname = casesnpdata.NameixFi;
                            lastname = casesnpdata.NameixLast;
                            dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                        }


                        List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, ssnno, string.Empty, Firstname, lastname, dateofbirth, string.Empty, "SSNALL", string.Empty, string.Empty, strBaseAgency);
                        if (propBaseForm.BaseAdminAgency != "**")
                            snpdetails = snpdetails.FindAll(u => u.Agency == propBaseForm.BaseAdminAgency);


                        gvwSub.Rows.Clear();

                        cmbSSNNumber.Items.Clear();
                        if (casesnpdata.NameixLast != "SSNNUMBER")
                        {
                            // modified by murali 19/Dec/20188 start
                            if (snpdetails.FindAll(u => u.Ssno.Substring(3, 2) != "00").Count > 0)
                            {
                                var groupsssn = snpdetails.GroupBy(n => n.Ssno).Select(n => new { Ssno = n.Key, SSNCount = n.Count() }).Where(u => u.Ssno.Substring(3, 2) != "00").OrderByDescending(n => n.SSNCount);
                                foreach (var item in groupsssn)
                                {
                                    cmbSSNNumber.Items.Add(new ListItem(item.Ssno + "         (" + item.SSNCount + ")", item.Ssno));
                                }
                                var groups = snpdetails.GroupBy(n => n.Ssno).Select(n => new { Ssno = n.Key, SSNCount = n.Count() }).Where(u => u.Ssno.Substring(3, 2) == "00").OrderByDescending(n => n.SSNCount);
                                foreach (var item in groups)
                                {
                                    cmbSSNNumber.Items.Add(new ListItem(item.Ssno + "         (" + item.SSNCount + ")", item.Ssno));
                                }
                            }
                            else
                            {
                                var groups = snpdetails.GroupBy(n => n.Ssno).Select(n => new { Ssno = n.Key, SSNCount = n.Count() }).OrderByDescending(n => n.SSNCount);
                                foreach (var item in groups)
                                {
                                    cmbSSNNumber.Items.Add(new ListItem(item.Ssno + "         (" + item.SSNCount + ")", item.Ssno));
                                }
                            }
                            // modified by murali 19/Dec/2018 start
                            if (snpdetails.Count > 0)
                                cmbSSNNumber.SelectedIndex = 0;

                            //added by Sudheer on 01/23/2018 for Client Ids filling....
                            string CLID = "0";
                            if (cmbSSNNumber.Items.Count > 0)
                                CLID = snpdetails.Find(u => u.Ssno.Equals(((ListItem)cmbSSNNumber.SelectedItem).Value.ToString())).ClientId.ToString();
                            this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                            cmbClientID.Items.Clear();
                            this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                            int Sel_Ind = 0; int Cnt = 1;
                            var groupsClientId = snpdetails.GroupBy(n => n.ClientId).Select(n => new { ClientID = n.Key, CIDCount = n.Count() }).OrderByDescending(n => n.CIDCount);

                            cmbClientID.Items.Insert(0, new ListItem("New ClientID", "NEW", "Y", Color.White, "0"));
                            cmbClientID.ColorMember = "FavoriteColor"; //string strkey = "*********";
                            foreach (var item in groupsClientId)
                            {
                                List<CaseSnpEntity> snpSSN = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, string.Empty, item.ClientID, string.Empty, string.Empty, string.Empty, "*********", "Sub", string.Empty, string.Empty, strBaseAgency);
                                if (snpSSN.Count > 0)
                                {
                                    var distSSN = snpSSN.Select(u => u.Ssno.Trim()).Distinct().ToList();
                                    if (distSSN.Count > 1) { ISClientID = false; } else ISClientID = true;
                                }
                                cmbClientID.Items.Add(new ListItem(item.ClientID + "         (" + item.CIDCount + ")", item.ClientID, ISClientID == true ? "Y" : "N", ISClientID == true ? Color.Green : Color.Red, item.CIDCount.ToString()));

                                if (item.ClientID.ToString() == CLID) { if (ISClientID) Sel_Ind = Cnt; else Sel_Ind = 0; }
                                Cnt++;
                            }
                            if (snpdetails.Count > 0)
                            {
                                if (Sel_Ind == 0)
                                {
                                    int maxValue = cmbClientID.Items.Cast<ListItem>().Where(item => item.ID.Equals("Y")).Select(item => int.Parse(item.DefaultValue)).Max();
                                    SetComboBoxValue(cmbClientID, maxValue.ToString());
                                }
                                else
                                    cmbClientID.SelectedIndex = Sel_Ind;
                            }


                            //List<CaseSnpEntity> SelSnpSSn = snpdetails.FindAll(u => u.Ssno.Equals(snpdetails[0].Ssno));
                            var distClientID = snpdetails.Select(u => u.ClientId.Trim()).Distinct().ToList();
                            if (distClientID.Count > 1)
                            {
                                ISClientID = true; chkbClientID.Visible = true;
                                if (chkbClientID.Checked)
                                {
                                    cmbClientID.Visible = true;
                                    btnSubmit.Text = "Fi&x SSN# && ClientID";
                                }
                            }
                            else
                            {
                                chkbClientID.Visible = false; cmbClientID.Visible = false;
                                this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                                cmbClientID.Items.Clear();
                                this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                            }
                            //chkbClientID.Checked = false;
                        }
                        foreach (CaseSnpEntity item in snpdetails)
                        {
                            int index = gvwSub.Rows.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), LookupDataAccess.Getdate(item.AltBdate), item.ClientId, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, item.AddOperator, LookupDataAccess.Getdate(item.DateAdd), item.LstcOperator, LookupDataAccess.Getdate(item.DateLstc), item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App + " " + item.FamilySeq);
                            if (item.Status.Trim() != "A")
                                gvwSub.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                            gvwSub.Rows[index].Tag = item;
                        }
                    }

                    if (gvwSub.Rows.Count > 0)
                    {
                        gvwSub.Rows[0].Selected = true;
                        cmbClientID.Enabled = chkbClientID.Enabled = cmbSSNNumber.Enabled = btnSubmit.Enabled = true;
                    }
                    else
                    {
                        cmbClientID.Enabled = chkbClientID.Enabled = cmbSSNNumber.Enabled = btnSubmit.Enabled = false;
                        this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                        cmbClientID.Items.Clear();
                        this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                        cmbSSNNumber.Items.Clear();
                    }


                }

            }
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (gvwMain.Rows.Count > 0)
            {
                if (gvwMain.SelectedRows[0].Selected)
                {
                    bool ISfalse = true;
                    string strkey = "*********";
                    //List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, string.Empty, ((ListItem)cmbClientID.SelectedItem).Value.ToString(), string.Empty, string.Empty, string.Empty, strkey, "Sub");
                    //if (snpdetails.Count > 0)
                    //{
                    //    var distSSN = snpdetails.Select(u => u.Ssno.Trim()).Distinct().ToList();
                    //    if (distSSN.Count > 1) { ISfalse = false; MessageBox.Show("Duplicate SSN# found with this Client ID " + ((ListItem)cmbClientID.SelectedItem).Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxHandlerLaunchClientID, true); }
                    //}

                    //if (ISfalse)
                    //{
                    if (cmbSSNNumber.Items.Count > 0)
                    {
                        MessageBox.Show("Are You Sure Want Update SSN# Selected Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                    }
                    //}
                }
            }
        }

        private void MessageBoxHandlerLaunchClientID(object sender, EventArgs e)
        {
            Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;
            if (senderForm != null)
            {
                if (senderForm.DialogResult.ToString() == "OK")
                {
                    rbClientId.Checked = true;
                    fillCliendIdGrid(((ListItem)cmbClientID.SelectedItem).Value.ToString());

                }
            }
        }

        private void fillCliendIdGrid(string ClientID)
        {
            btn_Masssmash.Enabled = false;
            btnSelecFixClientIds.Enabled = false;
            btnSelectNewClientId.Enabled = true;
            strFillLoad = "ClientId";
            _errorProvider.SetError(txtMaxim, null);
            gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
            gvCLIDMain.Rows.Clear();
            gvClientSub.Rows.Clear();
            string strYear = string.Empty;
            if (txtYear.Text != string.Empty)
            {
                strYear = txtYear.Text;
            }
            string strFromDate = string.Empty; string strToDate = string.Empty;
            if (rdoAddDate.Checked)
            {
                strFromDate = dtaddFrom.Value.ToShortDateString();
                if (dtaddTo.Checked)
                    strToDate = dtaddTo.Value.ToShortDateString();
            }

            snpdetails = _model.CaseMstData.GetSnpFixclinetId(strYear, txtMaxim.Text.ToString(), string.Empty, ClientID, string.Empty, string.Empty, string.Empty, string.Empty, "Client", strFromDate, strToDate, strBaseAgency);
            foreach (CaseSnpEntity item in snpdetails)
            {
                int index = gvCLIDMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.ClientId, item.Ssno, string.Empty, LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno);
                if (item.Status.Trim() != "A")
                    gvCLIDMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                gvCLIDMain.Rows[index].Tag = item;
            }
            lblCount.Text = gvCLIDMain.Rows.Count.ToString();
            if (gvCLIDMain.Rows.Count > 0)
            {
                gvCLIDMainSelectedIndexID = 0;
                gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
            }
            else
            {
                AlertBox.Show("No Records found", MessageBoxIcon.Warning);
            }
            gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
            gvCLIDMain_SelectionChanged(gvCLIDMain, new EventArgs());
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {

                CaseSnpEntity casesnpdetails = gvwMain.SelectedRows[0].Tag as CaseSnpEntity;
                if (casesnpdetails != null)
                {
                    string LastName = string.Empty; string Mode = "Single";
                    if (!string.IsNullOrEmpty(casesnpdetails.NameixLast.Trim()))
                        LastName = casesnpdetails.NameixLast;
                    string ClientID = string.Empty;
                    if (cmbClientID.Visible)
                    {
                        if (chkbClientID.Checked && ((ListItem)cmbClientID.SelectedItem).Value.ToString().Trim() == "NEW")
                        { ClientID = "0"; Mode = "NEW"; }
                        else if (chkbClientID.Checked)
                            ClientID = ((ListItem)cmbClientID.SelectedItem).Value.ToString().Trim();
                    }
                    else
                    {
                        if (gvwSub.Rows.Count > 0)
                        {
                            ClientID = gvwSub.Rows[0].Cells["gvtClientId2"].Value == null ? string.Empty : gvwSub.Rows[0].Cells["gvtClientId2"].Value.ToString();
                        }

                    }

                    if (_model.CaseMstData.UpdateSNPClientId(((ListItem)cmbSSNNumber.SelectedItem).Value.ToString().Trim(), ClientID, casesnpdetails.NameixFi, LastName, LookupDataAccess.Getdate(casesnpdetails.AltBdate), "UPDATESSN", Mode, propBaseForm.UserID))
                    {


                        if (_model.CaseMstData.INSERTUPDATEFIXSNPAUDIT(casesnpdetails.Agency, casesnpdetails.Dept, casesnpdetails.Program, casesnpdetails.Year, casesnpdetails.App, casesnpdetails.FamilySeq, "S", casesnpdetails.Ssno, ((ListItem)cmbSSNNumber.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty, casesnpdetails.ClientId, ClientID, string.Empty, string.Empty, string.Empty, propBaseForm.UserID, "SSNSWITCH"))
                        {

                        }
                        snpdetails.Remove(casesnpdetails);
                        ISClientID = false;
                        gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                        gvwMain.Rows.Clear();
                        foreach (CaseSnpEntity item in snpdetails)
                        {
                            int index = gvwMain.Rows.Add(item.NameixFi.ToString(), item.NameixLast.ToString(), LookupDataAccess.Getdate(item.AltBdate));
                            gvwMain.Rows[index].Tag = item;
                        }
                        lblCount.Text = gvwMain.Rows.Count.ToString();
                        if (gvwMain.Rows.Count > 0)
                        {
                            gvwMain.Rows[0].Selected = true;
                        }
                        gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);

                    }
                    else
                    {
                        AlertBox.Show("Error Process, please try again", MessageBoxIcon.Warning);
                    }


                }
                gvwMain_SelectionChanged(gvwMain, EventArgs.Empty);//(sender, e);


            }
            //}
        }

        private void chkbClientID_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbClientID.Checked)
            {
                cmbClientID.Visible = true;
                btnSubmit.Text = "Fix SSN# && ClientID";
            }
            else
            {
                cmbClientID.Visible = false;
                btnSubmit.Text = "Fix Selected SSN#";
            }
        }

        string strFillLoad = string.Empty;

        private void btnAllIds_Click(object sender, EventArgs e)
        {
            if (gvCLIDMain.Rows.Count > 0)
            {
                List<DataGridViewRow> SelectedgvRows = (from c in gvCLIDMain.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvcchksel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();

                if (SelectedgvRows.Count > 1)
                {

                    if (gvCLIDMain.SelectedRows[0].Selected)
                    {
                        MessageBox.Show("Are You Sure to Mass Update all selected rows in top grid", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerMultiple);
                    }
                }
                else
                {
                    AlertBox.Show("You must select 2 or more rows in top grid", MessageBoxIcon.Warning);
                }
            }
        }

        private void MessageBoxHandlerMultiple(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                string ClientID = string.Empty;
                string strFirstName = string.Empty;
                string strLastName = string.Empty;
                string strDOB = string.Empty;
                //  bool boolsnpswitch = true;
                foreach (DataGridViewRow item in gvCLIDMain.Rows)
                {
                    if (item.Cells["gvcchksel"].Value.ToString().ToUpper() == "TRUE")
                    {

                        CaseSnpEntity casesnpdetails = item.Tag as CaseSnpEntity;
                        if (casesnpdetails != null)
                        {
                            //if (casesnpdetails.Ssno.Substring(3, 2) == "00")
                            //{
                            //    if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, casesnpdetails.ClaimSsno, casesnpdetails.NameixFi, casesnpdetails.NameixLast, casesnpdetails.AltBdate, "00", "Single", propBaseForm.UserID))
                            //    { }
                            //    else
                            //    {
                            //        CommonFunctions.MessageBoxDisplay("Error Process try again..");
                            //    }

                            //}
                            //else
                            //{
                            //    if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, casesnpdetails.ClaimSsno, string.Empty, string.Empty, string.Empty, "nn", "Single", propBaseForm.UserID))
                            //    { }
                            //    else
                            //    {
                            //        CommonFunctions.MessageBoxDisplay("Error Process try again..");
                            //    }
                            //}

                            strFirstName = casesnpdetails.NameixFi;
                            strLastName = string.Empty; //casesnpdata.NameixLast;
                            strDOB = LookupDataAccess.Getdate(casesnpdetails.AltBdate);


                            List<CaseSnpEntity> snpmassupdatedetails = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, string.Empty, string.Empty, strFirstName, strLastName, strDOB, string.Empty, "SSNALL", string.Empty, string.Empty, strBaseAgency);

                            if (snpmassupdatedetails.Count > 0)
                            {
                                CaseSnpEntity snpMassupdatetopdata = snpmassupdatedetails[0];
                                if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "7")
                                {
                                    string Mode = "MASS";
                                    if (_model.CaseMstData.UpdateSNPClientId(string.Empty, snpMassupdatetopdata.ClientId, snpMassupdatetopdata.NameixFi, string.Empty, LookupDataAccess.Getdate(snpMassupdatetopdata.AltBdate), "UPDCLIID", Mode, propBaseForm.UserID))
                                    {
                                        snpdetails.Remove(casesnpdetails);
                                    }
                                }
                            }

                        }
                    }
                }

                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                gvCLIDMain.Rows.Clear();
                foreach (CaseSnpEntity item in snpdetails)
                {
                    if (item.AltBdate != string.Empty)
                    {
                        int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                        gvCLIDMain.Rows[index].Tag = item;
                    }
                }
                lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                if (gvCLIDMain.Rows.Count > 0)
                {
                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                }
                chkb50rec.Checked = false;
                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                gvCLIDMain_SelectionChanged(gvCLIDMain, EventArgs.Empty);//(sender, e);

            }
            //}
        }

        private void btnSelecIds_Click(object sender, EventArgs e)
        {
            if (gvClientSub.Rows.Count > 0)
            {
                if (gvClientSub.SelectedRows[0].Selected)
                {
                    if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "10")
                    {
                        CaseSnpEntity casesnpdetails = gvClientSub.SelectedRows[0].Tag as CaseSnpEntity;
                        if (casesnpdetails != null)
                        {
                            if (casesnpdetails.ClientId != string.Empty)
                            {
                                MessageBox.Show("Are You Sure Want Update Client ID Selected Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerSelIDs);

                            }
                            else
                            {
                                AlertBox.Show("You should not Select blank Client ID", MessageBoxIcon.Warning);

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Are You Sure Want Update Client ID Selected Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerSelIDs);
                    }
                }
            }
        }

        private void MessageBoxHandlerSelIDs(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {

                CaseSnpEntity casesnpdetails = gvClientSub.SelectedRows[0].Tag as CaseSnpEntity;
                if (casesnpdetails != null)
                {
                    string ClientID = string.Empty;
                    string strFirstName = string.Empty;
                    string strLastName = string.Empty;
                    string strDOB = string.Empty;
                    if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "7" || ((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "10")
                    {
                        string Mode = string.Empty;
                        if (_model.CaseMstData.UpdateSNPClientId(string.Empty, casesnpdetails.ClientId, casesnpdetails.NameixFi, string.Empty, LookupDataAccess.Getdate(casesnpdetails.AltBdate), "UPDCLIID", Mode, propBaseForm.UserID))
                        {
                            CaseSnpEntity casesnpdetailsMain = gvCLIDMain.SelectedRows[0].Tag as CaseSnpEntity;
                            if (casesnpdetailsMain != null)
                                snpdetails.Remove(casesnpdetailsMain);
                            gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                            gvCLIDMain.Rows.Clear();
                            foreach (CaseSnpEntity item in snpdetails)
                            {
                                if (item.AltBdate != string.Empty)
                                {
                                    int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                                    gvCLIDMain.Rows[index].Tag = item;
                                }
                            }
                            lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                            if (gvCLIDMain.Rows.Count > 0)
                            {
                                if (gvCLIDMain.Rows.Count >= gvCLIDMainSelectedIndexID && gvCLIDMainSelectedIndexID > 0)
                                {
                                    gvCLIDMainSelectedIndexID = gvCLIDMainSelectedIndexID - 1;
                                }

                                gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;

                            }
                            gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                            gvCLIDMain_SelectionChanged(gvCLIDMain, EventArgs.Empty);//(sender, e);

                        }
                    }



                }
                if (gvCLIDMain.Rows.Count > 0)
                {
                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                }
                //if (casesnpdetails != null)
                //{

                //    string ClientID = string.Empty;

                //    string Mode = string.Empty;
                //    if (((ListItem)cmbClientID.SelectedItem).Value.ToString().Trim() == "NEW")
                //    { ClientID = "0"; Mode = "NEW"; }
                //    else
                //        ClientID = ((ListItem)cmbClientID.SelectedItem).Value.ToString().Trim();


                //    if (casesnpdetails.Ssno.Substring(3, 2) == "00")
                //    {
                //        //if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, casesnpdetails.ClaimSsno, casesnpdetails.NameixFi, casesnpdetails.NameixLast, casesnpdetails.AltBdate, "00", "Single"))
                //        if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, ClientID, casesnpdetails.NameixFi, casesnpdetails.NameixLast, casesnpdetails.AltBdate, "00", "Single", propBaseForm.UserID))
                //        { }
                //        else
                //        {
                //            CommonFunctions.MessageBoxDisplay("Error Process try again..");
                //        }

                //    }
                //    else
                //    {
                //        //  if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, casesnpdetails.ClaimSsno, string.Empty, string.Empty, string.Empty, "nn", "Single"))
                //        if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, ClientID, string.Empty, string.Empty, string.Empty, "nn", "Single", propBaseForm.UserID))
                //        { }
                //        else
                //        {
                //            CommonFunctions.MessageBoxDisplay("Error Process try again..");
                //        }
                //    }
                //}
                gvCLIDMain_SelectionChanged(gvCLIDMain, EventArgs.Empty);//(sender, e);
            }
            //}
        }

        private void btnSelectSSN_Click(object sender, EventArgs e)
        {
            if (gvClientSub.Rows.Count > 0)
            {
                if (gvClientSub.SelectedRows[0].Selected)
                {
                    MessageBox.Show("Are You Sure Want Generate New Client ID", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxSSNHandler);
                }
            }
        }

        private void MessageBoxSSNHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            //    // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {

                CaseSnpEntity casesnpdetails = gvClientSub.SelectedRows[0].Tag as CaseSnpEntity;
                if (casesnpdetails != null)
                {
                    if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "7" || ((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "10")
                    {
                        string Mode = string.Empty;
                        if (_model.CaseMstData.UpdateSNPClientId(string.Empty, casesnpdetails.ClientId, casesnpdetails.NameixFi, string.Empty, LookupDataAccess.Getdate(casesnpdetails.AltBdate), "UPDCLIID", "NEW", propBaseForm.UserID))
                        {
                            CaseSnpEntity casesnpdetailsMain = gvCLIDMain.SelectedRows[0].Tag as CaseSnpEntity;
                            if (casesnpdetailsMain != null)
                                snpdetails.Remove(casesnpdetailsMain);
                            gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                            gvCLIDMain.Rows.Clear();
                            foreach (CaseSnpEntity item in snpdetails)
                            {
                                if (item.AltBdate != string.Empty)
                                {
                                    int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                                    gvCLIDMain.Rows[index].Tag = item;
                                }
                            }
                            lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                            if (gvCLIDMain.Rows.Count > 0)
                            {
                                gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                            }
                            gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                            gvCLIDMain_SelectionChanged(gvCLIDMain, EventArgs.Empty);//(sender, e);

                        }
                    }


                    //if (!_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, casesnpdetails.ClientId, string.Empty, string.Empty, string.Empty, "SSN", "Single", propBaseForm.UserID))
                    //{
                    //    CommonFunctions.MessageBoxDisplay("Error Process try again..");
                    //}

                }
                if (gvCLIDMain.Rows.Count > 0)
                {
                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                }
                gvCLIDMain_SelectionChanged(gvCLIDMain, EventArgs.Empty); //(sender, e);
            }
            //}
        }

        int gvCLIDMainSelectedIndexID = 0;
        private void gvCLIDMain_SelectionChanged(object sender, EventArgs e)
        {
            if (gvCLIDMain.Rows.Count > 0)
            {
                if (gvCLIDMain.SelectedRows[0].Selected)
                {
                    gvCLIDMainSelectedIndexID = gvCLIDMain.SelectedRows[0].Index;
                    string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid;
                    CaseSnpEntity casesnpdata = (gvCLIDMain.SelectedRows[0].Tag as CaseSnpEntity);
                    if (casesnpdata != null)
                    {
                        if (casesnpdata.Ssno.ToUpper() == "CLIENTID")
                        {
                            cmbClientID.Visible = false;
                            //btnSelecFixClientIds.Enabled = false;
                            lblClientId.Text = "";
                            //chkbClientID.Enabled = false;
                            // btnSelectNewClientId.Enabled = false;
                            ssnno = string.Empty;
                            strclientid = string.Empty;
                            if (casesnpdata.NameixLast.ToUpper() == "CLIENTID")
                            {
                                strclientid = casesnpdata.ClientId;
                                Firstname = string.Empty;
                                lastname = string.Empty;
                                dateofbirth = string.Empty;
                                cmbClientID.Visible = false;
                                chkbClientID.Visible = false;
                            }
                            else
                            {
                                Firstname = casesnpdata.NameixFi;
                                lastname = string.Empty; //casesnpdata.NameixLast;
                                dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                            }

                            gvClientSub.Rows.Clear();
                            string strFilterType = "SSNALL";
                            if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "9")
                                strFilterType = "SSNALLERAP";
                            List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, string.Empty, strclientid, Firstname, lastname, dateofbirth, string.Empty, strFilterType, string.Empty, string.Empty, strBaseAgency);
                            if (propBaseForm.BaseAdminAgency != "**")
                                snpdetails = snpdetails.FindAll(u => u.Agency.Equals(propBaseForm.BaseAdminAgency));
                            foreach (CaseSnpEntity item in snpdetails)
                            {
                                int index = gvClientSub.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.ClientId, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), string.Empty, item.AddOperator, LookupDataAccess.Getdate(item.DateAdd), item.LstcOperator, LookupDataAccess.Getdate(item.DateLstc));
                                if (item.Status.Trim() != "A")
                                    gvClientSub.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                gvClientSub.Rows[index].Tag = item;
                            }
                            if (gvClientSub.Rows.Count > 0)
                            {
                                gvClientSub.Rows[0].Selected = true;
                                if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "7")
                                {
                                    btnSelecFixClientIds.Enabled = true;
                                    btnSelectNewClientId.Enabled = true;
                                    btn_Masssmash.Enabled = true;
                                    lblButtonName.Location = new Point(260, 11);
                                }
                                if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "10")
                                {
                                    btnSelecFixClientIds.Enabled = true;
                                    btnSelectNewClientId.Enabled = true;
                                }
                            }
                            else
                            {
                                btnSelecFixClientIds.Enabled = false;
                                btnSelectNewClientId.Enabled = false;
                                btn_Masssmash.Enabled = false;
                            }


                        }
                        else
                        {
                            ssnno = string.Empty;
                            strclientid = string.Empty;
                            ssnno = casesnpdata.Ssno.Substring(3, 2);
                            strkey = "*********";//casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                            Firstname = string.Empty; lastname = string.Empty; dateofbirth = string.Empty; ;
                            if (ssnno == "00")
                            {
                                ssnno = string.Empty;
                                Firstname = casesnpdata.NameixFi;
                                lastname = casesnpdata.NameixLast;
                                dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                            }
                            else
                            {
                                ssnno = casesnpdata.Ssno;
                            }
                            if (strFillLoad != string.Empty)
                            {
                                strclientid = casesnpdata.ClientId;
                                ssnno = string.Empty;
                                Firstname = string.Empty;
                                lastname = string.Empty;
                                dateofbirth = string.Empty;
                                strkey = "*********";
                            }
                            List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, ssnno, strclientid, Firstname, lastname, dateofbirth, strkey, "Sub", string.Empty, string.Empty, strBaseAgency);
                            gvClientSub.Rows.Clear();
                            this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                            cmbClientID.Items.Clear();
                            this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                            int Sel_Ind = 0; int Cnt = 1;
                            var groupsClientId = snpdetails.GroupBy(n => n.ClientId).Select(n => new { ClientID = n.Key, CIDCount = n.Count() }).OrderByDescending(n => n.CIDCount);
                            lblClientId.Text = "Client ID";
                            //cmbClientID.Items.Insert(0, new ListItem("New ClientID", "NEW", "Y", Color.White, "0"));

                            cmbClientID.Visible = true;
                            cmbClientID.ColorMember = "FavoriteColor"; //string strkey = "*********";
                            foreach (var item in groupsClientId)
                            {
                                ISClientID = true;

                                cmbClientID.Items.Add(new ListItem(item.ClientID + "         (" + item.CIDCount + ")", item.ClientID, ISClientID == true ? "Y" : "N", ISClientID == true ? Color.Green : Color.Red, item.CIDCount.ToString()));

                            }
                            if (snpdetails.Count > 0)
                            {
                                if (Sel_Ind == 0)
                                {
                                    int maxValue = cmbClientID.Items.Cast<ListItem>().Where(item => item.ID.Equals("Y")).Select(item => int.Parse(item.DefaultValue)).Max();
                                    SetComboBoxValue(cmbClientID, maxValue.ToString());
                                }
                                else
                                    cmbClientID.SelectedIndex = Sel_Ind;
                            }


                            foreach (CaseSnpEntity item in snpdetails)
                            {
                                int index = gvClientSub.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.ClientId, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), string.Empty, item.AddOperator, LookupDataAccess.Getdate(item.DateAdd), LookupDataAccess.Getdate(item.DateLstc));
                                if (item.Status.Trim() != "A")
                                    gvClientSub.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                gvClientSub.Rows[index].Tag = item;
                            }

                            if (strFillLoad != string.Empty)
                            {
                                if (gvClientSub.Rows.Count > 0)
                                    gvClientSub.Rows[0].Selected = true;
                            }
                        }
                    }
                }

            }
            else
            {
                cmbClientID.Visible = false;
            }
        }

        private void rbClientId_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSSN.Checked)
            {
                this.Size = new Size(1060, 533/*588*/);
                lblSSN.Location = new Point(15, 13);
                cmbSSNNumber.Location = new Point(90, 8);
                btnSubmit.Location = new Point(256, 8);
                pnlSSN.Visible = true;
                pnlButtons.Visible = false;
                lblButtonName.Text = string.Empty;
                pnlExcel.Visible = false;
                btnFixDob.Visible = false;
                btnFixFName.Visible = false;
                //pnlClient.Visible = false;
                pnlSubmit.Visible = false;
                pnlUpdClientID.Visible = false;
                gvCLIDMain.Visible = false; pnlgvCLIDMain.Visible = false;
                gvClientSub.Visible = false; pnlgvClientSub.Visible = false;
                lblMaxRecs.Visible = false; txtMaxim.Visible = false;
                pnlName.Visible = true;
                gvwMain.Visible = true; gvwSub.Visible = true;
                pnlgvwMain.Visible = true; pnlgvwSub.Visible = true;
                btn_Masssmash.Visible = false;
                //  btnSearch.Visible = true; btnSearchSSN.Visible = true; btnFnameSearch.Visible = true;
                lblSSN.Visible = true; cmbSSNNumber.Visible = true; btnSubmit.Visible = true;
                cmbClientID.Enabled = chkbClientID.Enabled = cmbSSNNumber.Enabled = btnSubmit.Enabled = false;
                gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                cmbButtonsName.Items.Clear();
                //  cmbButtonsName.Items.Add(new ListItem("", ""));
                //cmbButtonsName.Items.Add(new ListItem("Last Name & DOB with Diff SS#", "1"));// Brain is asked small changes doc we have removed lastname & dob option 02/10/2021 
                cmbButtonsName.Items.Add(new ListItem("First Name & DOB with Diff SS#", "2"));
                cmbButtonsName.Items.Add(new ListItem("Same SS# with diff name or DOB", "3"));
                cmbButtonsName.SelectedIndex = 0;

                cmbSSNNumber.Items.Clear();
                this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                cmbClientID.Items.Clear();
                this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                gvwMain.Rows.Clear(); gvwSub.Rows.Clear(); lblCount.Text = string.Empty; chkbClientID.Checked = true;
                //if (chkbClientID.Checked)
                //{
                //    cmbClientID.Visible = true;
                //    btnSubmit.Text = "Fix SSN# && ClientID";
                //}
                rdoDateYear.Checked = true;
                rdoAddDate_Click(rdoDateYear, e);
                txtFName.Text = string.Empty; txtLName.Text = string.Empty;
                gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);

            }
            else
            {
                this.Size = new Size(1060, 533/*588*/);
                lblButtonName.Text = string.Empty;
                pnlExcel.Visible = false;
                //lblFName.Visible = true; txtFName.Visible = false;
                //lblLName.Visible = false; txtLName.Visible = false;
                //lblDob.Visible = false; dtpFrmDate.Visible = false;
                gvwMain.Visible = false; gvwSub.Visible = false;
                pnlgvwMain.Visible = false; pnlgvwSub.Visible = false;
                btn_Masssmash.Visible = true;
                //    btnSearch.Visible = false; btnSearchSSN.Visible = false; btnFnameSearch.Visible = false;
                lblSSN.Visible = false; cmbSSNNumber.Visible = false; btnSubmit.Visible = false;
                chkbClientID.Visible = false; cmbClientID.Visible = false;
                btnSelecFixClientIds.Enabled = false;
                btnSelectNewClientId.Enabled = false;
                btn_Masssmash.Enabled = false;
                gvtRkey.Visible = false;
                gvcchksel.Visible = false;
                gvtSnpkey.HeaderText = "Client ID";
                gvcchksel.Visible = false;
                gvtClientId.Visible = false;
                cmbButtonsName.Items.Clear();
                pnlSSN.Visible = false;
                pnlButtons.Visible = true;

                // cmbButtonsName.Items.Add(new ListItem("", ""));                
                //  cmbButtonsName.Items.Add(new ListItem("SS# With Diff Client IDs", "4"));
                // cmbButtonsName.Items.Add(new ListItem("Client ID With Diff SS#", "5"));
                cmbButtonsName.Items.Add(new ListItem("Client ID with different First Name & DOB", "8"));
                cmbButtonsName.Items.Add(new ListItem("First Name & DOB with diff Client ID", "7"));
                cmbButtonsName.Items.Add(new ListItem("First Name & DOB with Empty Client ID", "10"));
                if (propBaseForm.BaseAgencyControlDetails.PaymentCategorieService == "Y")
                {
                    cmbButtonsName.Items.Add(new ListItem("ERAP duplication check", "9"));
                }
                cmbButtonsName.SelectedIndex = 0;
                //pnlClient.Visible = true;
                //pnlClient.Location = new System.Drawing.Point(2, 235);
                pnlSubmit.Visible = true;
                // pnlSubmit.Location = new System.Drawing.Point(257, 503);//4,457--257
                gvCLIDMain.Visible = true; pnlgvCLIDMain.Visible = true;
                gvClientSub.Visible = true; pnlgvClientSub.Visible = true;
                lblMaxRecs.Visible = false; txtMaxim.Visible = false;
                chkbClientID.Visible = false; cmbClientID.Visible = false;
                rdoDateYear.Checked = true;
                rdoAddDate_Click(rdoDateYear, e);
                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                gvCLIDMain.Rows.Clear(); gvClientSub.Rows.Clear(); lblCount.Text = string.Empty; chkbClientID.Checked = true;

                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
            }
            if (propBaseForm.BaseAgencyControlDetails.ClidSmash == "Y")
            {
                txtYear.Text = propBaseForm.BaseAgencyControlDetails.ClidYear;
                if (propBaseForm.BaseAgencyControlDetails.ClidFrom != string.Empty)
                {
                    dtaddFrom.Value = Convert.ToDateTime(propBaseForm.BaseAgencyControlDetails.ClidFrom);
                    if (propBaseForm.BaseAgencyControlDetails.ClidTo != string.Empty)
                        dtaddTo.Value = Convert.ToDateTime(propBaseForm.BaseAgencyControlDetails.ClidTo);
                }
                if (propBaseForm.BaseAgencyControlDetails.ClidSSN != "Y")
                    rbSSN.Visible = false;
                if (propBaseForm.BaseAgencyControlDetails.ClidClid != "Y")
                    rbClientId.Visible = false;

                if (propBaseForm.BaseAgencyControlDetails.ClidYear == string.Empty)
                {
                    rdoDateYear.Visible = false;
                    rdoAddDate.Checked = true;
                    pnladddate.Visible = true;
                    pnladdYear.Visible = false;

                }
                if (propBaseForm.BaseAgencyControlDetails.ClidFrom == string.Empty)
                {
                    rdoAddDate.Visible = false;
                }

                txtYear.Enabled = false;
                dtaddTo.Enabled = false;
                dtaddFrom.Enabled = false;
            }
            //if (rbClientId.Checked) { pnlUpdClientID.Visible = true;pnlUpdClientID.Location = new System.Drawing.Point(5, 471); }
        }

        private void cmbClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbClientID.SelectedItem).ID.ToString() == "N")
            {
                int maxValue = cmbClientID.Items.Cast<ListItem>().Where(item => item.ID.Equals("Y")).Select(item => int.Parse(item.DefaultValue)).Max();
                SetComboBoxValue(cmbClientID, maxValue.ToString());
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if ((li.DefaultValue.Equals(value) && li.ID.Equals("Y")))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void rdoAddDate_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtaddFrom, null);
            _errorProvider.SetError(dtaddTo, null);
            RadioButton rb = (RadioButton)sender;
            string rbname = rb.Name;
            if (propBaseForm.BaseAgencyControlDetails != null)
                if (propBaseForm.BaseAgencyControlDetails.ClidSmash == "Y")
                {
                    if (rbname == "rdoAddDate")
                    {
                        pnladddate.Visible = true;
                        pnladdYear.Visible = false;
                        rdoDateYear.Checked = false;
                        rdoAddDate.Checked = false;
                        pnlYear.Size = new Size(150, 43);
                    }
                    if (rbname == "rdoDateYear")
                    {
                        pnladddate.Visible = false;
                        pnladdYear.Visible = true;
                        rdoDateYear.Checked = false;
                        pnlYear.Size = new Size(198, 43);
                    }
                }
                else
                {
                    if (rbname == "rdoDateYear")
                    {
                        dtaddFrom.Checked = false;
                        dtaddTo.Checked = false;
                        pnladddate.Visible = false;
                        pnladdYear.Visible = true;
                        rdoAddDate.Checked = false;
                        pnlYear.Size = new Size(198, 43);
                    }
                    if (rbname == "rdoAddDate")
                    {
                        dtaddFrom.Value = DateTime.Now.AddDays(-30);
                        dtaddFrom.Checked = true;
                        // dtaddTo.Value = DateTime.Now;
                        dtaddTo.Checked = true;
                        pnladddate.Visible = true;
                        pnladdYear.Visible = false;
                        txtYear.Text = string.Empty;
                        rdoDateYear.Checked = false;
                        pnlYear.Size = new Size(150, 43);
                    }
                }
        }
        //private void rdoAddDate_Click(object sender, EventArgs e)
        //{
        //    if (propBaseForm.BaseAgencyControlDetails != null)
        //        if (propBaseForm.BaseAgencyControlDetails.ClidSmash == "Y")
        //        {
        //            if (rdoAddDate.Checked)
        //            {
        //                pnladddate.Visible = true;
        //                pnladdYear.Visible = false;
        //                rdoDateYear.Checked = false;
        //                pnlYear.Size = new Size(150, 43);
        //            }
        //            else
        //            {
        //                pnladddate.Visible = false;
        //                pnladdYear.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            if (rdoAddDate.Checked)
        //            {
        //                dtaddFrom.Value = DateTime.Now.AddDays(-30);
        //                dtaddFrom.Checked = true;
        //                dtaddTo.Value = DateTime.Now;
        //                dtaddTo.Checked = true;
        //                pnladddate.Visible = true;
        //                pnladdYear.Visible = false;
        //                txtYear.Text = string.Empty;
        //                rdoDateYear.Checked = false;
        //                pnlYear.Size = new Size(150, 43);
        //            }
        //            else
        //            {
        //                dtaddFrom.Checked = false;
        //                dtaddTo.Checked = false;
        //                pnladddate.Visible = false;
        //                pnladdYear.Visible = true;

        //            }
        //        }
        //}

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelreportData("FixSSNData");
        }

        private void ExcelreportData(string ExcelName)
        {


            StringBuilder strMstApplUpdate = new StringBuilder();
            // string PdfName = "Pdf File";
            //PdfName = form.GetFileName();
            string propReportPath = _model.lookupDataAccess.GetReportPath();

            string Random_Filename = string.Empty;

            ExcelName = propReportPath + propBaseForm.UserID + "\\" + ExcelName;
            try
            {
                if (!Directory.Exists(propReportPath + propBaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + propBaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }


            try
            {
                string Tmpstr = ExcelName + ".xls";
                if (System.IO.File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = ExcelName + newFileName.Substring(0, length) + ".xls";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                ExcelName = Random_Filename;
            else
                ExcelName += ".xls";

            try
            {
                Workbook book = new Workbook();



                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Calibri";
                styleb.Font.Size = 9;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                styleb.Alignment.Vertical = StyleVerticalAlignment.Center;
                //styleb.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //styleb.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //styleb.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //styleb.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Calibri";
                style.Font.Size = 13;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Alignment.Vertical = StyleVerticalAlignment.Center;
                //style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);





                WorksheetStyle HeaderStyleSmallblue = book.Styles.Add("HeaderStyleSmallblue");
                HeaderStyleSmallblue.Font.FontName = "Calibri";
                HeaderStyleSmallblue.Font.Size = 11;
                HeaderStyleSmallblue.Font.Bold = true;
                HeaderStyleSmallblue.Font.Color = "#0000FF";
                HeaderStyleSmallblue.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleSmallblue.Alignment.Vertical = StyleVerticalAlignment.Center;
                HeaderStyleSmallblue.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleSmallblue.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleSmallblue.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleSmallblue.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle stylesmall = book.Styles.Add("HeaderStyleSmall");
                stylesmall.Font.FontName = "Calibri";
                stylesmall.Font.Size = 11;
                stylesmall.Font.Bold = true;
                stylesmall.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                stylesmall.Alignment.Vertical = StyleVerticalAlignment.Center;
                stylesmall.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                stylesmall.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                stylesmall.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                stylesmall.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle HeaderStyleSmallApcent = book.Styles.Add("HeaderStyleSmallApcent");
                HeaderStyleSmallApcent.Font.FontName = "Calibri";
                HeaderStyleSmallApcent.Font.Size = 11;
                HeaderStyleSmallApcent.Font.Bold = true;
                HeaderStyleSmallApcent.Interior.Color = "#FFCDD2";
                HeaderStyleSmallApcent.Interior.Pattern = StyleInteriorPattern.Solid;
                HeaderStyleSmallApcent.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleSmallApcent.Alignment.Vertical = StyleVerticalAlignment.Center;
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle HeaderStyleSmallClosed = book.Styles.Add("HeaderStyleSmallClosed");
                HeaderStyleSmallClosed.Font.FontName = "Calibri";
                HeaderStyleSmallClosed.Font.Size = 11;
                HeaderStyleSmallClosed.Font.Bold = true;
                HeaderStyleSmallClosed.Interior.Color = "#E1DDDC";
                HeaderStyleSmallClosed.Interior.Pattern = StyleInteriorPattern.Solid;
                HeaderStyleSmallClosed.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleSmallClosed.Alignment.Vertical = StyleVerticalAlignment.Center;
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Calibri";
                style1.Font.Size = 11;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style1.Alignment.WrapText = true;
                style1.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style1.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style1.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style1.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle HeaderStyleRotate = book.Styles.Add("HeaderStyleRotate");
                HeaderStyleRotate.Font.FontName = "Calibri";
                HeaderStyleRotate.Font.Size = 11;
                HeaderStyleRotate.Font.Bold = true;
                HeaderStyleRotate.Alignment.Rotate = 90;
                HeaderStyleRotate.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleRotate.Alignment.WrapText = true;
                HeaderStyleRotate.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleRotate.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleRotate.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleRotate.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);



                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Calibri";
                style2.Font.Size = 11;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle stylesunday = book.Styles.Add("CellStylesunday");
                style2.Font.FontName = "Calibri";
                style2.Font.Size = 11;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle ResponseStyle = book.Styles.Add("ResponseStyle");
                ResponseStyle.Font.FontName = "Calibri";
                ResponseStyle.Font.Size = 10;
                ResponseStyle.Font.Bold = true;
                ResponseStyle.Interior.Color = "#000000";
                ResponseStyle.Interior.Pattern = StyleInteriorPattern.Solid;
                ResponseStyle.Alignment.Vertical = StyleVerticalAlignment.Center;
                ResponseStyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                ResponseStyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                ResponseStyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                ResponseStyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                ResponseStyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle ResponseStylegreen = book.Styles.Add("ResponseStylegreen");
                ResponseStylegreen.Font.FontName = "Calibri";
                ResponseStylegreen.Font.Size = 11;
                ResponseStylegreen.Font.Bold = true;
                ResponseStylegreen.Interior.Color = "#00FF00";
                ResponseStylegreen.Interior.Pattern = StyleInteriorPattern.Solid;
                ResponseStylegreen.Alignment.Vertical = StyleVerticalAlignment.Center;
                ResponseStylegreen.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                ResponseStylegreen.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                ResponseStylegreen.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                ResponseStylegreen.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                ResponseStylegreen.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                style = book.Styles.Add("Default");
                style.Font.FontName = "Calibri";
                style.Font.Size = 9;
                style.Font.Bold = true;

                WorksheetStyle Default12 = book.Styles.Add("Default12");
                Default12.Font.FontName = "Calibri";
                Default12.Font.Size = 9;
                //Default12.Font.Bold = false;
                Default12.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                Default12.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                Default12.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                Default12.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);




                Worksheet sheet = book.Worksheets.Add("Sheet1");

                WorksheetCell cell;
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                //sheet.Table.Columns.Add(new WorksheetColumn(100));
                //sheet.Table.Columns.Add(new WorksheetColumn(100));



                WorksheetRow row = sheet.Table.Rows.Add();


                if (rbSSN.Checked)
                {
                    if (gvwMain.Rows.Count > 0)
                    {
                        row = sheet.Table.Rows.Add();
                        if (gvtFirstName.HeaderText == "SS#")
                        {
                            row.Cells.Add(new WorksheetCell("SS#", "HeaderStyle1"));

                        }
                        else
                        {
                            if (gvtLastName.Visible == false)
                            {
                                row.Cells.Add(new WorksheetCell("First Name", "HeaderStyle1"));
                                row.Cells.Add(new WorksheetCell("DOB", "HeaderStyle1"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell("First Name", "HeaderStyle1"));
                                row.Cells.Add(new WorksheetCell("Last Name", "HeaderStyle1"));
                                row.Cells.Add(new WorksheetCell("DOB", "HeaderStyle1"));
                            }
                        }

                        row.Cells.Add(new WorksheetCell("Name", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("DOB", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Client Id", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("SSN", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Added By", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Date Added", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Last Changed", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("SNP Key", "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell("Good Client ID", "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell("Good SS#", "HeaderStyle1"));



                        if (chkExcelAllrows.Checked == false)
                        {
                            row = sheet.Table.Rows.Add();
                            if (gvwMain.SelectedRows[0].Selected)
                            {
                                if (gvtFirstName.HeaderText == "SS#")
                                {
                                    row.Cells.Add(new WorksheetCell(gvwMain.SelectedRows[0].Cells[0].Value.ToString(), "Default12"));

                                }
                                else
                                {
                                    if (gvtLastName.Visible == false)
                                    {
                                        row.Cells.Add(new WorksheetCell(gvwMain.SelectedRows[0].Cells[0].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvwMain.SelectedRows[0].Cells[2].Value.ToString(), "Default12"));
                                    }
                                    else
                                    {
                                        row.Cells.Add(new WorksheetCell(gvwMain.SelectedRows[0].Cells[0].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvwMain.SelectedRows[0].Cells[1].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvwMain.SelectedRows[0].Cells[2].Value.ToString(), "Default12"));
                                    }
                                }

                                int intfirstrow = 0;
                                foreach (DataGridViewRow item in gvwSub.Rows)
                                {

                                    if (intfirstrow == 0)
                                    {
                                        intfirstrow = 1;
                                        row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[3].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[6].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[8].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[9].Value.ToString().ToString(), "Default12"));
                                        //row.Cells.Add(new WorksheetCell(((ListItem)cmbClientID.SelectedItem).Value.ToString(), "Default12"));
                                        //row.Cells.Add(new WorksheetCell(((ListItem)cmbSSNNumber.SelectedItem).Value.ToString(), "Default12"));
                                    }
                                    else
                                    {

                                        row = sheet.Table.Rows.Add();
                                        if (gvtFirstName.HeaderText == "SS#")
                                        {
                                            row.Cells.Add(new WorksheetCell("", "Default12"));

                                        }
                                        else
                                        {
                                            if (gvtLastName.Visible == false)
                                            {
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                            }
                                            else
                                            {
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                            }
                                        }
                                        row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[3].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[6].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[8].Value.ToString().ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[9].Value.ToString().ToString(), "Default12"));
                                    }

                                }

                            }
                        }

                        else
                        {

                            foreach (DataGridViewRow item in gvwMain.Rows)
                            {
                                row = sheet.Table.Rows.Add();

                                if (gvtFirstName.HeaderText == "SS#")
                                {
                                    row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString(), "Default12"));

                                }
                                else
                                {
                                    if (gvtLastName.Visible == false)
                                    {
                                        row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString(), "Default12"));
                                    }
                                    else
                                    {
                                        row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString(), "Default12"));
                                    }
                                }

                                string ssnno, Firstname, lastname, dateofbirth;
                                CaseSnpEntity casesnpdata = (item.Tag as CaseSnpEntity);
                                if (casesnpdata != null)
                                {

                                    ssnno = string.Empty;
                                    if (casesnpdata.NameixLast == "SSNNUMBER")
                                    {
                                        ssnno = casesnpdata.Ssno;
                                        Firstname = string.Empty;
                                        lastname = string.Empty;
                                        dateofbirth = string.Empty;

                                    }
                                    else
                                    {
                                        Firstname = casesnpdata.NameixFi;
                                        lastname = casesnpdata.NameixLast;
                                        dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                                    }
                                    string strFilterType = "SSNALL";
                                    if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "9")
                                        strFilterType = "SSNALLERAP";
                                    List<CaseSnpEntity> reportsnpdetails = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, ssnno, string.Empty, Firstname, lastname, dateofbirth, string.Empty, strFilterType, string.Empty, string.Empty, strBaseAgency);


                                    int intfirstrow = 0;
                                    foreach (CaseSnpEntity snpsubitems in reportsnpdetails)
                                    {

                                        if (intfirstrow == 0)
                                        {
                                            intfirstrow = 1;

                                            //string strSSNnum = string.Empty, strClientId = string.Empty;
                                            //var groups = snpdetails.GroupBy(n => n.Ssno).Select(n => new { Ssno = n.Key, SSNCount = n.Count() }).OrderByDescending(n => n.SSNCount);
                                            //if (groups.ToList().Count > 0)
                                            //    strSSNnum = groups.FirstOrDefault().Ssno.ToString();


                                            ////added by Sudheer on 01/23/2018 for Client Ids filling....


                                            //var groupsClientId = snpdetails.GroupBy(n => n.ClientId).Select(n => new { ClientID = n.Key, CIDCount = n.Count() }).OrderByDescending(n => n.CIDCount);

                                            //if (groupsClientId.ToList().Count > 0)
                                            //    strClientId = groupsClientId.FirstOrDefault().ClientID.ToString();

                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(snpsubitems.NameixFi, snpsubitems.NameixMi, snpsubitems.NameixLast, propBaseForm.BaseHierarchyCnFormat), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.AltBdate).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.ClientId.ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetPhoneSsnNoFormat(snpsubitems.Ssno.ToString().ToString()), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.AddOperator.ToString().ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateAdd).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateLstc).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.Agency + snpsubitems.Dept + snpsubitems.Program + " " + snpsubitems.Year + " " + snpsubitems.AltApp, "Default12"));
                                            //row.Cells.Add(new WorksheetCell(strClientId, "Default12"));
                                            //row.Cells.Add(new WorksheetCell(strSSNnum, "Default12"));



                                        }
                                        else
                                        {

                                            row = sheet.Table.Rows.Add();
                                            if (gvtFirstName.HeaderText == "SS#")
                                            {
                                                row.Cells.Add(new WorksheetCell("", "Default12"));

                                            }
                                            else
                                            {
                                                if (gvtLastName.Visible == false)
                                                {
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                }
                                                else
                                                {
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                }
                                            }
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(snpsubitems.NameixFi, snpsubitems.NameixMi, snpsubitems.NameixLast, propBaseForm.BaseHierarchyCnFormat), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.AltBdate).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.ClientId.ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetPhoneSsnNoFormat(snpsubitems.Ssno.ToString().ToString()), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.AddOperator.ToString().ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateAdd).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateLstc).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.Agency + snpsubitems.Dept + snpsubitems.Program + " " + snpsubitems.Year + " " + snpsubitems.AltApp, "Default12"));
                                        }

                                    }


                                }
                            }
                        }
                    }

                }
                else
                {
                    if (gvCLIDMain.Rows.Count > 0)
                    {

                        row = sheet.Table.Rows.Add();
                        if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                        {
                            row.Cells.Add(new WorksheetCell("Client ID", "HeaderStyle1"));

                        }
                        else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                        {
                            row.Cells.Add(new WorksheetCell("First Name", "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("DOB", "HeaderStyle1"));


                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell("SNP Key", "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("Client Id", "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("SSN", "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("DOB", "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("Name", "HeaderStyle1"));
                        }
                        // row.Cells.Add(new WorksheetCell("New Client Id", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("SNP Key", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Client Id", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("SSN", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("DOB", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Name", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Added By", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Date Added", "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("Last Changed", "HeaderStyle1"));

                        if (chkExcelAllrows.Checked == false)
                        {
                            List<DataGridViewRow> SelectedgvRows = (from c in gvCLIDMain.Rows.Cast<DataGridViewRow>().ToList()
                                                                    where (((DataGridViewCheckBoxCell)c.Cells["gvcchksel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                                    select c).ToList();

                            if (SelectedgvRows.Count <= 1)
                            {
                                if (gvCLIDMain.SelectedRows[0].Selected)
                                {

                                    row = sheet.Table.Rows.Add();
                                    if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                                    {
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[1].Value.ToString(), "Default12"));

                                    }
                                    else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                                    {
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[1].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[5].Value.ToString(), "Default12"));

                                    }
                                    else
                                    {

                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[1].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[2].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[3].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[5].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[6].Value.ToString(), "Default12"));
                                    }
                                    // row.Cells.Add(new WorksheetCell(gvCLIDMain.SelectedRows[0].Cells[6].Value.ToString(), "Default12"));



                                    CaseSnpEntity casesnpdata = (gvCLIDMain.SelectedRows[0].Tag as CaseSnpEntity);
                                    if (casesnpdata != null)
                                    {

                                        int intfirstrow = 0;
                                        foreach (DataGridViewRow item in gvClientSub.Rows)
                                        {
                                            if (intfirstrow == 0)
                                            {
                                                intfirstrow = 1;
                                                row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[4].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[7].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[8].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[9].Value.ToString().ToString(), "Default12"));
                                            }
                                            else
                                            {
                                                row = sheet.Table.Rows.Add();
                                                if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                                                {
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                }
                                                else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                                                {
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));

                                                }
                                                else
                                                {

                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                }
                                                row.Cells.Add(new WorksheetCell(item.Cells[0].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[4].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[7].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[8].Value.ToString().ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(item.Cells[9].Value.ToString().ToString(), "Default12"));
                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {

                                foreach (DataGridViewRow item in SelectedgvRows)
                                {

                                    row = sheet.Table.Rows.Add();
                                    if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                                    {
                                        row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                    }
                                    else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                                    {
                                        row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString(), "Default12"));

                                    }
                                    else
                                    {
                                        row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[3].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString(), "Default12"));
                                        row.Cells.Add(new WorksheetCell(item.Cells[6].Value.ToString(), "Default12"));
                                    }
                                    string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid;
                                    List<CaseSnpEntity> snpreportexcellist = new List<CaseSnpEntity>();
                                    CaseSnpEntity casesnpdata = (item.Tag as CaseSnpEntity);
                                    if (casesnpdata != null)
                                    {
                                        if (casesnpdata.Ssno.ToUpper() == "CLIENTID")
                                        {
                                            lblClientId.Text = "";
                                            // chkbClientID.Enabled = false;
                                            //btnSelectNewClientId.Enabled = false;
                                            ssnno = string.Empty;
                                            strclientid = string.Empty;
                                            if (casesnpdata.NameixLast.ToUpper() == "CLIENTID")
                                            {
                                                strclientid = casesnpdata.ClientId;
                                                Firstname = string.Empty;
                                                lastname = string.Empty;
                                                dateofbirth = string.Empty;
                                                cmbClientID.Visible = false;
                                                chkbClientID.Visible = false;
                                            }
                                            else
                                            {
                                                Firstname = casesnpdata.NameixFi;
                                                lastname = string.Empty; //casesnpdata.NameixLast;
                                                dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                                            }

                                            string strFilterType = "SSNALL";
                                            if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "9")
                                                strFilterType = "SSNALLERAP";
                                            snpreportexcellist = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, string.Empty, strclientid, Firstname, lastname, dateofbirth, string.Empty, strFilterType, string.Empty, string.Empty, strBaseAgency);
                                        }
                                        else
                                        {
                                            ssnno = string.Empty;
                                            strclientid = string.Empty;
                                            ssnno = casesnpdata.Ssno.Substring(3, 2);
                                            strkey = "*********";//casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                                            Firstname = string.Empty; lastname = string.Empty; dateofbirth = string.Empty; ;
                                            if (ssnno == "00")
                                            {
                                                ssnno = string.Empty;
                                                Firstname = casesnpdata.NameixFi;
                                                lastname = casesnpdata.NameixLast;
                                                dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                                            }
                                            else
                                            {
                                                ssnno = casesnpdata.Ssno;
                                            }
                                            if (strFillLoad != string.Empty)
                                            {
                                                strclientid = casesnpdata.ClientId;
                                                ssnno = string.Empty;
                                                Firstname = string.Empty;
                                                lastname = string.Empty;
                                                dateofbirth = string.Empty;
                                                strkey = "*********";
                                            }
                                            snpreportexcellist = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, ssnno, strclientid, Firstname, lastname, dateofbirth, strkey, "Sub", string.Empty, string.Empty, strBaseAgency);

                                        }

                                        int intfirstrow = 0;
                                        foreach (CaseSnpEntity snpsubitems in snpreportexcellist)
                                        {
                                            if (intfirstrow == 0)
                                            {
                                                intfirstrow = 1;

                                                row.Cells.Add(new WorksheetCell(snpsubitems.Agency + snpsubitems.Dept + snpsubitems.Program + " " + snpsubitems.Year + " " + snpsubitems.AltApp, "Default12"));
                                                row.Cells.Add(new WorksheetCell(snpsubitems.ClientId, "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.GetPhoneSsnNoFormat(snpsubitems.Ssno.ToString()), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.AltBdate).ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(snpsubitems.NameixFi, snpsubitems.NameixMi, snpsubitems.NameixLast, propBaseForm.BaseHierarchyCnFormat).ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(snpsubitems.AddOperator.ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateAdd), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateLstc), "Default12"));

                                            }
                                            else
                                            {
                                                row = sheet.Table.Rows.Add();
                                                if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                                                {
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                }
                                                else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                                                {
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));

                                                }
                                                else
                                                {

                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                                }

                                                row.Cells.Add(new WorksheetCell(snpsubitems.Agency + snpsubitems.Dept + snpsubitems.Program + " " + snpsubitems.Year + " " + snpsubitems.AltApp, "Default12"));
                                                row.Cells.Add(new WorksheetCell(snpsubitems.ClientId, "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.GetPhoneSsnNoFormat(snpsubitems.Ssno.ToString()), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.AltBdate).ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(snpsubitems.NameixFi, snpsubitems.NameixMi, snpsubitems.NameixLast, propBaseForm.BaseHierarchyCnFormat).ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(snpsubitems.AddOperator.ToString(), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateAdd), "Default12"));
                                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateLstc), "Default12"));
                                            }

                                        }


                                    }

                                }
                            }
                        }
                        else
                        {
                            foreach (DataGridViewRow item in gvCLIDMain.Rows)
                            {

                                row = sheet.Table.Rows.Add();
                                if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                                {
                                    row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                }
                                else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                                {
                                    row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                    row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString(), "Default12"));

                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell(item.Cells[1].Value.ToString(), "Default12"));
                                    row.Cells.Add(new WorksheetCell(item.Cells[2].Value.ToString(), "Default12"));
                                    row.Cells.Add(new WorksheetCell(item.Cells[3].Value.ToString(), "Default12"));
                                    row.Cells.Add(new WorksheetCell(item.Cells[5].Value.ToString(), "Default12"));
                                    row.Cells.Add(new WorksheetCell(item.Cells[6].Value.ToString(), "Default12"));
                                }
                                string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid;
                                List<CaseSnpEntity> snpreportexcellist = new List<CaseSnpEntity>();
                                CaseSnpEntity casesnpdata = (item.Tag as CaseSnpEntity);
                                if (casesnpdata != null)
                                {
                                    if (casesnpdata.Ssno.ToUpper() == "CLIENTID")
                                    {
                                        lblClientId.Text = "";
                                        // chkbClientID.Enabled = false;
                                        //btnSelectNewClientId.Enabled = false;
                                        ssnno = string.Empty;
                                        strclientid = string.Empty;
                                        if (casesnpdata.NameixLast.ToUpper() == "CLIENTID")
                                        {
                                            strclientid = casesnpdata.ClientId;
                                            Firstname = string.Empty;
                                            lastname = string.Empty;
                                            dateofbirth = string.Empty;
                                            cmbClientID.Visible = false;
                                            chkbClientID.Visible = false;
                                        }
                                        else
                                        {
                                            Firstname = casesnpdata.NameixFi;
                                            lastname = string.Empty; //casesnpdata.NameixLast;
                                            dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                                        }

                                        string strFilterType = "SSNALL";
                                        if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString() == "9")
                                            strFilterType = "SSNALLERAP";
                                        snpreportexcellist = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, string.Empty, strclientid, Firstname, lastname, dateofbirth, string.Empty, strFilterType, string.Empty, string.Empty, strBaseAgency);
                                    }
                                    else
                                    {
                                        ssnno = string.Empty;
                                        strclientid = string.Empty;
                                        ssnno = casesnpdata.Ssno.Substring(3, 2);
                                        strkey = "*********";//casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                                        Firstname = string.Empty; lastname = string.Empty; dateofbirth = string.Empty; ;
                                        if (ssnno == "00")
                                        {
                                            ssnno = string.Empty;
                                            Firstname = casesnpdata.NameixFi;
                                            lastname = casesnpdata.NameixLast;
                                            dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                                        }
                                        else
                                        {
                                            ssnno = casesnpdata.Ssno;
                                        }
                                        if (strFillLoad != string.Empty)
                                        {
                                            strclientid = casesnpdata.ClientId;
                                            ssnno = string.Empty;
                                            Firstname = string.Empty;
                                            lastname = string.Empty;
                                            dateofbirth = string.Empty;
                                            strkey = "*********";
                                        }
                                        snpreportexcellist = _model.CaseMstData.GetSnpFixclinetIdAddDate(string.Empty, string.Empty, ssnno, strclientid, Firstname, lastname, dateofbirth, strkey, "Sub", string.Empty, string.Empty, strBaseAgency);

                                    }

                                    int intfirstrow = 0;
                                    foreach (CaseSnpEntity snpsubitems in snpreportexcellist)
                                    {
                                        if (intfirstrow == 0)
                                        {
                                            intfirstrow = 1;

                                            row.Cells.Add(new WorksheetCell(snpsubitems.Agency + snpsubitems.Dept + snpsubitems.Program + " " + snpsubitems.Year + " " + snpsubitems.AltApp, "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.ClientId, "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetPhoneSsnNoFormat(snpsubitems.Ssno.ToString()), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.AltBdate).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(snpsubitems.NameixFi, snpsubitems.NameixMi, snpsubitems.NameixLast, propBaseForm.BaseHierarchyCnFormat).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.AddOperator.ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateAdd), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateLstc), "Default12"));

                                        }
                                        else
                                        {
                                            row = sheet.Table.Rows.Add();
                                            if (gvtSnpkey.HeaderText.ToUpper() == "CLIENT ID")
                                            {
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                            }
                                            else if (gvtSnpkey.HeaderText.ToUpper() == "FIRST NAME")
                                            {
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));

                                            }
                                            else
                                            {

                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                                row.Cells.Add(new WorksheetCell("", "Default12"));
                                            }

                                            row.Cells.Add(new WorksheetCell(snpsubitems.Agency + snpsubitems.Dept + snpsubitems.Program + " " + snpsubitems.Year + " " + snpsubitems.AltApp, "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.ClientId, "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetPhoneSsnNoFormat(snpsubitems.Ssno.ToString()), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.AltBdate).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(snpsubitems.NameixFi, snpsubitems.NameixMi, snpsubitems.NameixLast, propBaseForm.BaseHierarchyCnFormat).ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(snpsubitems.AddOperator.ToString(), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateAdd), "Default12"));
                                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(snpsubitems.DateLstc), "Default12"));
                                        }

                                    }


                                }

                            }
                        }
                    }

                }



                FileStream stream = new FileStream(ExcelName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "FIXSSNDATA.xls";

                //// downloadGateway.Version = file.Version;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), ExcelName);

                FileInfo fiDownload = new FileInfo(ExcelName);
                /// Need to check for file exists, is local file, is allow to read, etc...
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private bool ValidateForm()
        {
            bool isValid = true;
            if (rdoAddDate.Checked == true)
            {
                if (dtaddFrom.Checked == false)
                {
                    _errorProvider.SetError(dtaddFrom, "Please select 'From Date'");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtaddFrom, null);
                }
                if (dtaddTo.Checked == false)
                {
                    _errorProvider.SetError(dtaddTo, "Please select 'To Date'");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtaddTo, null);
                }
            }
            if (rdoAddDate.Checked == true)
            {
                if (dtaddFrom.Checked.Equals(true) && dtaddTo.Checked.Equals(true))
                {
                    if (string.IsNullOrWhiteSpace(dtaddFrom.Text))
                    {
                        _errorProvider.SetError(dtaddFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtaddFrom, null);
                    }
                    if (string.IsNullOrWhiteSpace(dtaddTo.Text))
                    {
                        _errorProvider.SetError(dtaddTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtaddTo, null);
                    }
                }
            }
            if (rdoAddDate.Checked == true)
            {
                if (dtaddFrom.Checked.Equals(true) && dtaddTo.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(dtaddFrom.Text) && (!string.IsNullOrEmpty(dtaddTo.Text)))
                    {
                        if (Convert.ToDateTime(dtaddFrom.Text) > Convert.ToDateTime(dtaddTo.Text))
                        {
                            _errorProvider.SetError(dtaddFrom, "'From Date' should be less than or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtaddFrom, null);
                        }
                    }
                }
            }
            if (dtpFrmDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpFrmDate.Text))
                {
                    _errorProvider.SetError(dtpFrmDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDob.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFrmDate, null);
                }
            }
            return isValid;
        }
        private void btnAllSearch_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtaddFrom, null);
            _errorProvider.SetError(dtaddTo, null);
            if (ValidateForm())
            {
                this.Size = new Size(1060, 533/*588*/);
                bool boolvalidation = true;
                _errorProvider.SetError(txtYear, null);
                if (txtYear.Text != string.Empty)
                {
                    if (Convert.ToInt32(txtYear.Text) < 2000)
                    {
                        boolvalidation = false;
                        _errorProvider.SetError(txtYear, "Please fill year 2000 above");

                    }
                }
                if (boolvalidation)
                {
                    if (cmbButtonsName.Text != string.Empty)
                    {
                        _errorProvider.SetError(cmbButtonsName, null);
                        pnlExcel.Visible = false;
                        btnDupseudossn.Visible = false;
                        btnFixDob.Visible = false;
                        btnFixFName.Visible = false;
                        chkb50rec.Visible = false;
                        pnlUpdClientID.Visible = false;
                        lblButtonName.Text = cmbButtonsName.Text;
                        lblButtonName.Location = new Point(15, 11);
                        switch (((ListItem)cmbButtonsName.SelectedItem).Value.ToString())
                        {

                            case "1":
                                string strYear = string.Empty, strFname = string.Empty, strLName = string.Empty, strdob = string.Empty;
                                gvtDob.Visible = true;
                                gvtFirstName.HeaderText = "First Name";
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                string strFromDate = string.Empty; string strToDate = string.Empty;
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                btnSubmit.Visible = true;
                                //btnSubmit.Enabled = true; cmbSSNNumber.Enabled = true;
                                cmbSSNNumber.Enabled = btnSubmit.Enabled = false;
                                this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                                cmbClientID.Items.Clear();
                                this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                                cmbSSNNumber.Items.Clear();
                                lblSSN.Visible = true; cmbSSNNumber.Visible = true;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                                gvwMain.Rows.Clear();
                                this.gvtLastName.Visible = true;
                                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "SSNMainALL", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails)
                                {
                                    int index = gvwMain.Rows.Add(item.NameixFi.ToString(), item.NameixLast.ToString(), LookupDataAccess.Getdate(item.AltBdate));
                                    gvwMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvwMain.Rows.Count.ToString();
                                if (gvwMain.Rows.Count > 0)
                                {
                                    gvwMain.Rows[0].Selected = true;
                                    pnlExcel.Visible = true;
                                }
                                else
                                {
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                    gvwSub.Rows.Clear();
                                    cmbSSNNumber.Enabled = btnSubmit.Enabled = chkbClientID.Enabled = cmbClientID.Enabled = false;
                                }
                                gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                                gvwMain_SelectionChanged(sender, e);
                                break;

                            case "2":   //ss#Integrity - First Name & DOB with different SS#
                                this.Size = new Size(1060, 570);
                                pnlButtons.Visible = true;
                                pnlSSN.Visible = true;
                                pnlExcel.AppearanceKey = null;
                                gvtFirstName.HeaderText = "First Name";
                                gvtDob.Visible = true;
                                strYear = strFname = strLName = strdob = string.Empty;
                                pnlSubmit.Visible = false; pnlUpdClientID.Visible = false; pnlFixBtns.Visible = false; //Vikash
                                lblSSN.Location = new Point(256, 13); cmbSSNNumber.Location = new Point(299, 8); btnSubmit.Location = new Point(455, 8);//Vikash
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = strToDate = string.Empty;
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                btnSubmit.Visible = true;
                                //btnSubmit.Enabled = true; cmbSSNNumber.Enabled = true;
                                cmbSSNNumber.Enabled = btnSubmit.Enabled = false;

                                lblSSN.Visible = true; cmbSSNNumber.Visible = true;
                                this.cmbClientID.SelectedIndexChanged -= new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                                cmbClientID.Items.Clear();
                                this.cmbClientID.SelectedIndexChanged += new System.EventHandler(this.cmbClientID_SelectedIndexChanged);//Vikash
                                cmbSSNNumber.Items.Clear();
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();

                                gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                                gvwMain.Rows.Clear();
                                this.gvtLastName.Visible = false;
                                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "SSNFName", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails)
                                {
                                    int index = gvwMain.Rows.Add(item.NameixFi.ToString(), string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                                    gvwMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvwMain.Rows.Count.ToString();
                                if (gvwMain.Rows.Count > 0)
                                {
                                    gvwMain.Rows[0].Selected = true;
                                    pnlExcel.Visible = true;

                                }
                                else
                                {
                                    this.Size = new Size(1060, 533);// Vikash
                                    pnlButtons.Visible = false;
                                    lblSSN.Location = new Point(15, 13);
                                    cmbSSNNumber.Location = new Point(90, 8);
                                    btnSubmit.Location = new Point(256, 8);
                                    pnlSSN.Visible = true;
                                    gvwSub.Rows.Clear();
                                    chkbClientID.Visible = cmbClientID.Visible = false;
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                    cmbSSNNumber.Enabled = btnSubmit.Enabled = chkbClientID.Enabled = cmbClientID.Enabled = false;
                                }
                                gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                                gvwMain_SelectionChanged(sender, e);

                                break;

                            case "3":    //ss#Integrity - Same SS# with different name or DOB
                                this.Size = new Size(1060, 533);
                                pnlSSN.Visible = false;
                                pnlButtons.Visible = true;
                                pnlExcel.AppearanceKey = "panel-grdo";
                                gvtLastName.Visible = false;
                                gvtDob.Visible = false;
                                gvtFirstName.HeaderText = "SS#";
                                //   "First Name";
                                strYear = strFname = strLName = strdob = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = strToDate = string.Empty;
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                btnSubmit.Visible = false; cmbSSNNumber.Enabled = false;
                                lblSSN.Visible = false; cmbSSNNumber.Visible = false;

                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();

                                gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                                gvwMain.Rows.Clear();
                                //this.gvtLastName.Visible = true;
                                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "OnlySSN", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails)
                                {
                                    int index = gvwMain.Rows.Add(item.Ssno.ToString(), item.NameixLast.ToString(), LookupDataAccess.Getdate(item.AltBdate));
                                    gvwMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvwMain.Rows.Count.ToString();
                                if (gvwMain.Rows.Count > 0)
                                {
                                    gvwMain.Rows[0].Selected = true;
                                    ToolTip tip = new ToolTip();
                                    tip.SetToolTip(btnDupseudossn, "Get Pseudo from CODEGEN");
                                    pnlFixBtns.Size = new Size(95, 34); pnlFixBtns.Visible = true;//Vikash
                                    btnDupseudossn.Visible = true;
                                    pnlExcel.Visible = true;
                                }
                                else
                                {
                                    gvwSub.Rows.Clear();
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                                gvwMain_SelectionChanged(sender, e);
                                break;
                            case "4":
                                gvtSnpkey.HeaderText = "CASE SNP";

                                gvtClientId.Visible = gvtSSN.Visible = gvtNo.Visible = gvtRkey.Visible = gvtName.Visible = gvtmaxClientId.Visible = true;

                                btn_Masssmash.Enabled = true;
                                lblButtonName.Location = new Point(260, 11);
                                btnSelecFixClientIds.Enabled = true;
                                btnSelectNewClientId.Enabled = false;
                                strFillLoad = string.Empty;
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                snpdetails = _model.CaseMstData.GetSnpFixclinetId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "Main", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails)
                                {
                                    int index = gvCLIDMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.ClientId, item.Ssno, string.Empty, LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno);
                                    if (item.Status.Trim() != "A")
                                        gvCLIDMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                    gvCLIDMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                if (gvCLIDMain.Rows.Count > 0)
                                {
                                    gvCLIDMainSelectedIndexID = 0;
                                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                                    pnlExcel.Visible = true;
                                }
                                else
                                {
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);

                                break;
                            case "5":
                                gvcchksel.Visible = false;
                                gvtSnpkey.HeaderText = "CASE SNP";

                                gvtClientId.Visible = gvtSSN.Visible = gvtNo.Visible = gvtRkey.Visible = gvtName.Visible = gvtmaxClientId.Visible = true;



                                btn_Masssmash.Enabled = false;
                                btnSelecFixClientIds.Enabled = false;
                                btnSelectNewClientId.Enabled = true;
                                strFillLoad = "ClientId";
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                List<CaseSnpEntity> snpdetails5 = _model.CaseMstData.GetSnpFixclinetId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "Client", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails5)
                                {
                                    int index = gvCLIDMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.ClientId, item.Ssno, string.Empty, LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno);
                                    if (item.Status.Trim() != "A")
                                        gvCLIDMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                    gvCLIDMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                if (gvCLIDMain.Rows.Count > 0)
                                {
                                    gvCLIDMainSelectedIndexID = 0;
                                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                                    pnlExcel.Visible = true;
                                }
                                else
                                {
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);

                                break;
                            case "6":
                                gvcchksel.Visible = false;
                                btn_Masssmash.Enabled = true;
                                btnSelecFixClientIds.Enabled = true;
                                btnSelectNewClientId.Enabled = false;
                                strFillLoad = string.Empty;
                                lblButtonName.Location = new Point(260, 11);
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                List<CaseSnpEntity> snpdetails4 = _model.CaseMstData.GetSnpFixclinetId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "Main00", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails4)
                                {
                                    int index = gvCLIDMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.AltApp, item.ClientId, item.Ssno, string.Empty, LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno);
                                    if (item.Status.Trim() != "A")
                                        gvCLIDMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                    gvCLIDMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                if (gvCLIDMain.Rows.Count > 0)
                                {
                                    gvCLIDMainSelectedIndexID = 0;
                                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                                    pnlExcel.Visible = true;
                                }
                                else
                                {
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);
                                break;
                            case "7":  //Client ID Integrity - First Name and DOB with different Client ID
                                Size = new Size(1060, 533);
                                gvtSnpkey.HeaderText = "First Name";
                                chkb50rec.Visible = true;
                                gvtClientId.Visible = false;
                                gvcchksel.Visible = true;
                                gvtSSN.Visible = false;
                                gvtNo.Visible = false;
                                gvtRkey.Visible = true;
                                gvtName.Visible = false;
                                gvtmaxClientId.Visible = false;
                                btn_Masssmash.Enabled = false;
                                btnSelecFixClientIds.Enabled = false;
                                btnSelectNewClientId.Enabled = false;
                                btn_Masssmash.Visible = true;
                                lblButtonName.Location = new Point(455, 8);//(260, 11);
                                btnSelecFixClientIds.Visible = true;
                                btnSelectNewClientId.Visible = true;
                                strFillLoad = string.Empty;
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "CLIENTFNameDOBALL", strFromDate, strToDate, strBaseAgency);
                                if (snpdetails.Count > 0)
                                {
                                    snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "CLIENTFNameDOB", strFromDate, strToDate, strBaseAgency);
                                    foreach (CaseSnpEntity item in snpdetails)
                                    {
                                        if (item.AltBdate != string.Empty)
                                        {
                                            int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                                            gvCLIDMain.Rows[index].Tag = item;
                                        }
                                    }
                                    lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                    if (gvCLIDMain.Rows.Count > 0)
                                    {
                                        gvCLIDMainSelectedIndexID = 0;
                                        gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                        gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                                        pnlSubmit.Visible = true;
                                        pnlExcel.Visible = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("No Mismatch Records Found \n Do you want to see all good records anyway?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerGooddata);

                                    }
                                }
                                else
                                {
                                    this.Size = new Size(1060, 533);// Vikash
                                    pnlButtons.Visible = true;
                                    pnlSSN.Visible = false;
                                    pnlSubmit.Visible = true;
                                    btnSelecFixClientIds.Enabled = btnSelectNewClientId.Enabled = false;
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);


                                break;
                            case "8": //Client ID Integrity - FirstClient ID with different First Name & DOB
                                Size = new Size(1060, 533);
                                pnlButtons.Visible = true;
                                pnlSSN.Visible = false;
                                pnlFixBtns.Visible = true;
                                pnlExcel.AppearanceKey = "panel-grdo";
                                gvtSnpkey.HeaderText = "Client ID";
                                gvcchksel.Visible = false;
                                gvtClientId.Visible = false;
                                gvtSSN.Visible = false;
                                gvtNo.Visible = false;
                                gvtRkey.Visible = false;
                                gvtName.Visible = false;
                                gvtmaxClientId.Visible = false;
                                //btnAllIds.Enabled = false;
                                btnSelecFixClientIds.Enabled = false;
                                btnSelectNewClientId.Enabled = false;
                                btn_Masssmash.Enabled = false;
                                btn_Masssmash.Visible = false;
                                pnlSubmit.Visible = false;
                                //btnSelecFixClientIds.Visible = false;
                                //btnSelectNewClientId.Visible = false;
                                btnFixFName.Visible = false;
                                btnFixDob.Visible = false;
                                strFillLoad = "ClientId";
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "OnlyClientId", strFromDate, strToDate, strBaseAgency);
                                foreach (CaseSnpEntity item in snpdetails)
                                {
                                    int index = gvCLIDMain.Rows.Add(false, item.ClientId);
                                    gvCLIDMain.Rows[index].Tag = item;
                                }
                                lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                if (gvCLIDMain.Rows.Count > 0)
                                {
                                    gvCLIDMainSelectedIndexID = 0;
                                    //gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                                    pnlExcel.Visible = true;
                                    // murali commented this condition on 10/18/2021
                                    //if (propBaseForm.UserID == "JAKE")
                                    //{
                                    pnlFixBtns.Padding = new Padding(3, 5, 5, 5);
                                    spacer3.Visible = false; pnlFixBtns.Size = new Size(220, 34);//Vikash
                                    btnFixFName.Visible = true;
                                    btnFixDob.Visible = true;
                                    pnlUpdClientID.Visible = true;
                                    //pnlUpdClientID.Location = new System.Drawing.Point(5, 471);
                                    //}
                                }
                                else
                                {
                                    this.Size = new Size(1060, 533);// Vikash
                                    pnlButtons.Visible = true;
                                    pnlSSN.Visible = false;
                                    pnlSubmit.Visible = true;
                                    btnSelecFixClientIds.Enabled = btnSelectNewClientId.Enabled = false;
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);


                                break;
                            case "9":
                                gvtSnpkey.HeaderText = "First Name";
                                chkb50rec.Visible = true;
                                lblButtonName.Location = new Point(260, 11);
                                gvtClientId.Visible = false;
                                gvcchksel.Visible = true;
                                gvtSSN.Visible = false;
                                gvtNo.Visible = false;
                                gvtRkey.Visible = true;
                                gvtName.Visible = false;
                                gvtmaxClientId.Visible = false;
                                btn_Masssmash.Enabled = false;
                                btnSelecFixClientIds.Enabled = false;
                                btnSelectNewClientId.Enabled = false;
                                btn_Masssmash.Visible = true;
                                btnSelecFixClientIds.Visible = false;
                                btnSelectNewClientId.Visible = false;
                                strFillLoad = string.Empty;
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                List<ProgramDefinitionEntity> programEntity = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty);
                                programEntity = programEntity.FindAll(u => u.DepSerpostPAYCAT != string.Empty);
                                if (programEntity.Count > 0)
                                {
                                    snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "ERAP", strFromDate, strToDate, strBaseAgency);
                                    if (snpdetails.Count > 0)
                                    {
                                        // snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "CLIENTFNameDOB", strFromDate, strToDate);
                                        foreach (CaseSnpEntity item in snpdetails)
                                        {
                                            if (item.AltBdate != string.Empty)
                                            {
                                                int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                                                gvCLIDMain.Rows[index].Tag = item;
                                            }
                                        }
                                        lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                        if (gvCLIDMain.Rows.Count > 0)
                                        {
                                            gvCLIDMainSelectedIndexID = 0;
                                            gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                                            gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                                            pnlExcel.Visible = true;
                                        }
                                        //else
                                        //{
                                        //    MessageBox.Show("No Mismatch Records Found \n Do you want to see all good records anyway?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandlerGooddata, true);

                                        //}
                                    }

                                    else
                                    {
                                        AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    AlertBox.Show("No Program is set for Payment Category in PROG Definition", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);


                                break;
                            case "10":  //Client ID Integrity - FirstFirst Name & DOB with Empty Client ID
                                Size = new Size(1060, 570);
                                pnlButtons.Visible = true;
                                pnlSSN.Visible = false;
                                pnlExcel.AppearanceKey = "panel-grdo";
                                gvtSnpkey.HeaderText = "First Name";
                                chkb50rec.Visible = true;
                                lblButtonName.Location = new Point(260, 11);
                                gvtClientId.Visible = false;
                                gvcchksel.Visible = true;
                                gvtSSN.Visible = false;
                                gvtNo.Visible = false;
                                gvtRkey.Visible = true;
                                gvtName.Visible = false;
                                gvtmaxClientId.Visible = false;
                                btn_Masssmash.Enabled = false;
                                btnSelecFixClientIds.Enabled = false;
                                btnSelectNewClientId.Enabled = false;
                                btn_Masssmash.Visible = true;
                                btnSelecFixClientIds.Visible = true;
                                btnSelectNewClientId.Visible = true;
                                strFillLoad = string.Empty;
                                _errorProvider.SetError(txtMaxim, null);
                                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain.Rows.Clear();
                                gvClientSub.Rows.Clear();
                                strYear = string.Empty;
                                if (rdoDateYear.Checked)
                                {
                                    if (txtYear.Text != string.Empty)
                                    {
                                        strYear = txtYear.Text;
                                    }
                                }
                                strFromDate = string.Empty; strToDate = strFname = strLName = strdob = string.Empty;
                                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                                if (rdoAddDate.Checked)
                                {
                                    strFromDate = dtaddFrom.Value.ToShortDateString();
                                    if (dtaddTo.Checked == true)
                                        strToDate = dtaddTo.Value.ToShortDateString();
                                }
                                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "FNameDOBALLCLIENTEMPTY", strFromDate, strToDate, strBaseAgency);
                                if (snpdetails.Count > 0)
                                {
                                    // snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "CLIENTEMPTYFNameDOB", strFromDate, strToDate);
                                    foreach (CaseSnpEntity item in snpdetails)
                                    {
                                        if (item.AltBdate != string.Empty)
                                        {
                                            int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                                            gvCLIDMain.Rows[index].Tag = item;
                                        }
                                    }
                                    lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                                    //if (gvCLIDMain.Rows.Count > 0)
                                    //{
                                    //    gvCLIDMain.Rows[0].Selected = true;
                                    //    pnlExcel.Visible = true;
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("No Mismatch Records Found \n Do you want to see all good records anyway?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandlerGooddata, true);

                                    //}
                                }
                                else
                                {
                                    this.Size = new Size(1060, 533);// Vikash
                                    pnlButtons.Visible = true;
                                    pnlSSN.Visible = false;
                                    pnlSubmit.Visible = true;
                                    btnSelecFixClientIds.Enabled = btnSelectNewClientId.Enabled = false;
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                                gvCLIDMain_SelectionChanged(sender, e);


                                break;
                        }
                    }
                    //else
                    //{
                    //    _errorProvider.SetError(cmbButtonsName, "Please select combobox option");

                    //}
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void pnlSubmit_Click(object sender, EventArgs e)
        {

        }

        private void PbExcel_Click(object sender, EventArgs e)
        {
            ExcelreportData("FixSSNData");
        }


        private void MessageBoxHandlerGooddata(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                gvCLIDMain.SelectionChanged -= new EventHandler(gvCLIDMain_SelectionChanged);
                gvCLIDMain.Rows.Clear();
                gvClientSub.Rows.Clear();
                string strYear = string.Empty, strFname = string.Empty, strLName = string.Empty, strdob = string.Empty;
                string strFromDate = string.Empty; string strToDate = string.Empty;

                if (rdoDateYear.Checked)
                {
                    if (txtYear.Text != string.Empty)
                    {
                        strYear = txtYear.Text;
                    }
                }

                if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                if (rdoAddDate.Checked)
                {
                    strFromDate = dtaddFrom.Value.ToShortDateString();
                    if (dtaddTo.Checked == true)
                        strToDate = dtaddTo.Value.ToShortDateString();
                }
                snpdetails = _model.CaseMstData.GetSnpFixSSN(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "CLIENTFNameDOBALL", strFromDate, strToDate, strBaseAgency);
                foreach (CaseSnpEntity item in snpdetails)
                {
                    if (item.AltBdate != string.Empty)
                    {
                        int index = gvCLIDMain.Rows.Add(false, item.NameixFi, string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(item.AltBdate));
                        gvCLIDMain.Rows[index].Tag = item;
                    }
                }
                lblCount.Text = gvCLIDMain.Rows.Count.ToString();
                if (gvCLIDMain.Rows.Count > 0)
                {
                    gvCLIDMainSelectedIndexID = 0;
                    gvCLIDMain.CurrentCell = gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Cells[0];
                    gvCLIDMain.Rows[gvCLIDMainSelectedIndexID].Selected = true;
                    pnlExcel.Visible = true;
                }
                else
                {
                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);

                }
                gvCLIDMain.SelectionChanged += new EventHandler(gvCLIDMain_SelectionChanged);
                gvCLIDMain_SelectionChanged(gvCLIDMain, EventArgs.Empty);//(sender, e);

            }
            //}
        }

        private void chkb50rec_CheckedChanged(object sender, EventArgs e)
        {
            if (gvCLIDMain.Rows.Count > 0)
            {
                if (chkb50rec.Checked)
                {
                    int Count = 0;
                    foreach (DataGridViewRow dr in gvCLIDMain.Rows)
                    {
                        dr.Cells["gvcchksel"].Value = true;
                        Count++;

                        if (Count == 50)
                            break;
                    }
                }
                else
                {
                    foreach (DataGridViewRow dr in gvCLIDMain.Rows)
                    {
                        dr.Cells["gvcchksel"].Value = false;

                    }
                }
            }
        }

        private void btnDupseudossn_Click(object sender, EventArgs e)
        {
            if (gvwMain.Rows.Count > 0)
            {
                if (gvwMain.SelectedRows[0].Selected)
                {
                    if (gvwSub.Rows.Count > 0)
                    {
                        strFixType = "DUPFIXPSUDOSSN";
                        MessageBox.Show("Are You Sure Want Update SSN# Selected Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerPSeudoSSN);
                    }
                }
            }
        }

        private void MessageBoxHandlerPSeudoSSN(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                string Mode = "";
                if (strFixType == "DUPFIXPSUDOSSN")
                {

                    CaseSnpEntity casesnpdetails = gvwSub.SelectedRows[0].Tag as CaseSnpEntity;
                    if (casesnpdetails != null)
                    {




                        string ClientID = string.Empty;

                        if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, ClientID, casesnpdetails.NameixFi, casesnpdetails.NameixLast, LookupDataAccess.Getdate(casesnpdetails.AltBdate), "DUPFIXPSUDOSSN", Mode, propBaseForm.UserID))
                        {
                            //if (_model.CaseMstData.INSERTUPDATEFIXSNPAUDIT(casesnpdetails.Agency, casesnpdetails.Dept, casesnpdetails.Program, casesnpdetails.Year, casesnpdetails.App, casesnpdetails.FamilySeq, "S", casesnpdetails.Ssno, ((ListItem)cmbSSNNumber.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty, casesnpdetails.ClientId, ClientID, string.Empty, string.Empty, string.Empty, propBaseForm.UserID, "SSNSWITCH"))
                            //{

                            //}
                        }
                        else
                        {
                            AlertBox.Show("Error Process, please try again", MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    CaseSnpEntity casesnpdetails = gvClientSub.SelectedRows[0].Tag as CaseSnpEntity;
                    if (_model.CaseMstData.UpdateSNPClientId(casesnpdetails.Ssno, casesnpdetails.ClientId, casesnpdetails.NameixFi, casesnpdetails.NameixLast, LookupDataAccess.Getdate(casesnpdetails.AltBdate), strFixType, Mode, propBaseForm.UserID))
                    {
                        btnAllSearch_Click(btnAllSearch, EventArgs.Empty); //(sender,e);
                    }
                    else
                    {
                        AlertBox.Show("Error Process, please try again", MessageBoxIcon.Warning);
                    }

                }


            }
            gvwMain_SelectionChanged(gvwMain, EventArgs.Empty);//(sender, e);


            //}
        }

        string strFixType = string.Empty;
        private void btnFixDob_Click(object sender, EventArgs e)
        {
            if (gvCLIDMain.Rows.Count > 0)
            {
                if (gvClientSub.SelectedRows[0].Selected)
                {
                    if (gvClientSub.Rows.Count > 0)
                    {
                        strFixType = "FIXDOB";
                        MessageBox.Show("Are You Sure Want Update DOB Selected Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerPSeudoSSN);
                    }
                }
            }
        }

        private void btnFixFName_Click(object sender, EventArgs e)
        {
            if (gvCLIDMain.Rows.Count > 0)
            {
                if (gvClientSub.SelectedRows[0].Selected)
                {
                    if (gvClientSub.Rows.Count > 0)
                    {
                        strFixType = "FIXFNAME";
                        MessageBox.Show("Are You Sure Want Update First Name Selected Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerPSeudoSSN);
                    }
                }
            }
        }

        private void btnNewClientID_Click(object sender, EventArgs e)
        {
            if (gvCLIDMain.Rows.Count > 0)
            {
                if (gvClientSub.Rows.Count > 0)
                {
                    strFixType = "FIXClientIDs";
                    MessageBox.Show("Are You Sure Want to Assign New Client IDs from CODEGEN", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerUpdateClientID);
                }
            }
        }

        private void MessageBoxHandlerUpdateClientID(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                string Mode = "";


                if (strFixType == "FIXClientIDs")
                {
                    string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid;
                    CaseSnpEntity casesnpdata = (gvCLIDMain.SelectedRows[0].Tag as CaseSnpEntity);
                    if (casesnpdata != null)
                    {
                        ssnno = string.Empty;
                        strclientid = string.Empty;
                        if (casesnpdata.NameixLast.ToUpper() == "CLIENTID")
                        {
                            strclientid = casesnpdata.ClientId;
                            Firstname = string.Empty;
                            lastname = string.Empty;
                            dateofbirth = string.Empty;
                            cmbClientID.Visible = false;
                            chkbClientID.Visible = false;
                        }
                        else
                        {
                            Firstname = casesnpdata.NameixFi;
                            lastname = string.Empty; //casesnpdata.NameixLast;
                            dateofbirth = LookupDataAccess.Getdate(casesnpdata.AltBdate);
                        }

                        if (_model.CaseMstData.UpdateClientIds(string.Empty, string.Empty, string.Empty, strclientid, Firstname, lastname, dateofbirth, string.Empty, "SSNALL", string.Empty, string.Empty, propBaseForm.UserID))
                        {
                            btnAllSearch_Click(btnAllSearch, EventArgs.Empty); //(sender, e);
                        }
                        else
                        {
                            AlertBox.Show("Error Process, please try again", MessageBoxIcon.Warning);
                        }
                    }

                    //    string ClientID = string.Empty;
                    //string strYear = string.Empty;
                    //if (rdoDateYear.Checked)
                    //{
                    //    if (txtYear.Text != string.Empty)
                    //    {
                    //        strYear = txtYear.Text;
                    //    }
                    //}
                    //string strFromDate = string.Empty; string strToDate = string.Empty; string strFname = string.Empty; string strLName = string.Empty; string strdob = string.Empty;
                    //if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                    //if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();
                    //if (rdoAddDate.Checked)
                    //{
                    //    strFromDate = dtaddFrom.Value.ToShortDateString();
                    //    if (dtaddTo.Checked == true)
                    //        strToDate = dtaddTo.Value.ToShortDateString();
                    //}

                }
                gvwMain_SelectionChanged(gvwMain, EventArgs.Empty);//(sender, e);
            }
            //}
        }

        private void cmbButtonsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (((ListItem)cmbButtonsName.SelectedItem).Value.ToString().Trim() == "8")
            //{ pnlUpdClientID.Visible = true; pnlUpdClientID.Location = new System.Drawing.Point(5, 471); }
            //else pnlUpdClientID.Visible = false;
        }

        private void FIXSSNFORM_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_HELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(_privlagesEnty.Program, 0, propBaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }

        private void rbClientId_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void rbClientId_Leave(object sender, EventArgs e)
        {
            rbSSN.Focus();
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {
            rdoAddDate.Focus();
        }




        //private void btnFixClientID_Click(object sender, EventArgs e)
        //{
        //    FIXCLIENTForm Ref_Form = new FIXCLIENTForm(((ListItem)cmbSSNNumber.SelectedItem).Value.ToString().Trim());
        //    Ref_Form.ShowDialog();
        //}


    }
}