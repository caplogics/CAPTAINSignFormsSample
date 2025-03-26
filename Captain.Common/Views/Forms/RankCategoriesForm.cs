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
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RankCategoriesForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public RankCategoriesForm(BaseForm baseform,string Mode,string Type,string Agy, string Rank_code,string Sub_code, PrivilegeEntity Priviliges)
        {
            InitializeComponent();
            propFromType = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();
            Baseform = baseform;
            mode = Mode;
            type = Type; Agency = Agy;
            RankCode = Rank_code;
            SubCode = Sub_code;
            priviliges = Priviliges;
            this.Text = "Ranking Categories";
            HeadStartCombo();
            switch (mode)
            {
                case "Edit":
                    
                    if (type == "Rank")
                    {
                        this.Text = "Ranking Categories"/*priviliges.Program*/ + " - Rank Edit";
                        fillRankCntrols();
                        this.Size = new System.Drawing.Size(527, 169);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                        txtFrom.Visible = false;
                        txtTo.Visible = false;
                        lblReqPR.Visible = false;
                        lblFrom.Visible = false;
                        lblTo.Visible = false;
                        lblReqCode.Visible = false;
                        lblPointRng.Visible = false;
                        //this.Text = "Ranking Categories";
                        txtCode.Validator = TextBoxValidation.IntegerValidator;
                        txtCode.Enabled = false;
                    }
                    else
                    {
                        this.Text = "Point Range Categories"/*priviliges.Program*/ + " - Rank Point Category Edit";
                        fillPointrangeControls();
                        //this.Text = "Point Range Categories";
                        lblHeadstart.Visible = false;
                        cmbRankCat.Visible = false;
                        lblReqPR.Visible = true;
                        this.Size = new System.Drawing.Size(527, 169);
                        this.lblPointRng.Location = new System.Drawing.Point(15,47);
                        this.lblReqPR.Location = new System.Drawing.Point(89, 44);
                        this.lblFrom.Location = new System.Drawing.Point(147, 47);
                        this.txtFrom.Location = new System.Drawing.Point(191, 43);
                        this.lblTo.Location = new System.Drawing.Point(266, 47);
                        this.txtTo.Location = new System.Drawing.Point(289, 43);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Left;
                        txtFrom.Visible = true;
                        txtTo.Visible = true;
                        lblReqCode.Visible = true;
                        lblFrom.Visible = true;
                        lblTo.Visible = true;
                        lblPointRng.Visible = true;
                        txtCode.Enabled = false;
                        txtFrom.Validator = TextBoxValidation.IntegerValidator;
                        txtTo.Validator = TextBoxValidation.IntegerValidator;
                        
                    }
                    break;
                case "Add":
                    
                    if (type == "Rank")
                    {
                        this.Text = "Ranking Categories"/*priviliges.Program*/ + " - Rank Add";
                        this.Size = new Size(527, 169);
                        //this.cmbRankCat.Location = new System.Drawing.Point(181, 37);
                       // this.lblHeadstart.Location = new System.Drawing.Point(5, 40);
                        this.txtCode.TextAlign = HorizontalAlignment.Right;
                        //this.Text = "Ranking Categories";
                        txtFrom.Visible = false;
                        txtTo.Visible = false;
                        lblFrom.Visible = false;
                        lblReqPR.Visible = false;
                        lblTo.Visible = false;
                        lblReqCode.Visible = false;
                        lblPointRng.Visible = false;
                        txtCode.Enabled = false;
                        txtCode.Validator = TextBoxValidation.IntegerValidator;
                        List<RankCatgEntity> Ranks = _model.SPAdminData.Browse_RankCtg();
                        if (Ranks.Count > 0)
                        {
                            string Rank = string.Empty;
                            foreach (RankCatgEntity Entity in Ranks)
                            {
                                if (Entity.Agency == Agency && string.IsNullOrEmpty(Entity.SubCode.Trim()))
                                {
                                    Rank = Entity.Code.Trim();
                                }
                            }
                            if (string.IsNullOrEmpty(Rank.Trim()))
                                txtCode.Text = "1";
                            else
                                txtCode.Text = (int.Parse(Rank.Trim()) + 1).ToString();

                        }
                        else
                            txtCode.Text = "1";
                    }
                    else
                    {
                        this.Text = /*priviliges.Program*/"Point Range Categories" + " - Rank Point Category Add";
                        lblHeadstart.Visible = false;
                        cmbRankCat.Visible = false; lblReqPR.Visible = true;
                        //this.Text = "Point Range Categories";
                        this.Size = new System.Drawing.Size(527, 169);
                        this.lblPointRng.Location = new System.Drawing.Point(15, 47);
                        this.lblReqPR.Location = new System.Drawing.Point(89, 44);
                        this.lblFrom.Location = new System.Drawing.Point(147, 47);
                        this.txtFrom.Location = new System.Drawing.Point(191, 43);
                        this.lblTo.Location = new System.Drawing.Point(266, 47);
                        this.txtTo.Location = new System.Drawing.Point(289, 43);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Left;
                        txtFrom.Visible = true;
                        txtTo.Visible = true;
                        lblReqCode.Visible = true;
                        lblFrom.Visible = true;
                        lblTo.Visible = true;
                        lblPointRng.Visible = true;
                        txtFrom.Validator = TextBoxValidation.IntegerValidator;
                        txtTo.Validator = TextBoxValidation.IntegerValidator;
                        
                    }
                    break;
            }

        }


        public RankCategoriesForm(BaseForm baseform, string Mode, string Type, string Agy, string Rank_code, string Sub_code, PrivilegeEntity Priviliges,string strFromType)
        {
            InitializeComponent();
            propFromType = "Groups";
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();
            Baseform = baseform;
            mode = Mode;
            type = Type; Agency = Agy;
            RankCode = Rank_code;
            SubCode = Sub_code;
            priviliges = Priviliges;
            this.Text = "Preass Groups";
            cmbRankCat.Visible = false;
            lblHeadstart.Visible = false;
            HeadStartCombo();
            switch (mode)
            {
                case "Edit":

                    if (type == "PREASSGROUPS")
                    {
                        this.Text = "Preass Groups"/*priviliges.Program*/ + " - Edit";
                        fillPreassGroupCntrols();
                        this.Size = new System.Drawing.Size(527, 135);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                        txtFrom.Visible = false;
                        txtTo.Visible = false;
                        lblReqPR.Visible = false;
                        lblFrom.Visible = false;
                        lblTo.Visible = false;
                        lblReqCode.Visible = false;
                        lblPointRng.Visible = false;
                        //this.Text = "Preass Groups";
                        txtCode.Validator = TextBoxValidation.IntegerValidator;
                        txtCode.Enabled = false;
                        fillPreassGroupCntrols();
                    }
                    else
                    {
                        this.Text = "Preass Group Points"/*priviliges.Program */+ " - Edit";
                        fillPreassGroupPointsControls();
                       // this.Text = "Preass Group Points";
                        lblHeadstart.Visible = false;
                        cmbRankCat.Visible = false;
                        lblReqPR.Visible = true;
                        this.Size = new System.Drawing.Size(527, 169);
                        this.lblPointRng.Location = new System.Drawing.Point(15, 47);
                        this.lblReqPR.Location = new System.Drawing.Point(89, 44);
                        this.lblFrom.Location = new System.Drawing.Point(147, 47);
                        this.txtFrom.Location = new System.Drawing.Point(191, 43);
                        this.lblTo.Location = new System.Drawing.Point(266, 47);
                        this.txtTo.Location = new System.Drawing.Point(289, 43);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Left;
                        txtFrom.Visible = true;
                        txtTo.Visible = true;
                        lblReqCode.Visible = true;
                        lblFrom.Visible = true;
                        lblTo.Visible = true;
                        lblPointRng.Visible = true;
                        txtCode.Enabled = false;
                        txtFrom.Validator = TextBoxValidation.FloatValidator;
                        txtTo.Validator = TextBoxValidation.FloatValidator;

                        fillPreassGroupPointsControls();
                    }
                    break;
                case "Add":

                    if (type == "PREASSGROUPS")
                    {
                        this.Text = "Preass Groups"/*priviliges.Program*/ + " - Add";
                        this.Size = new System.Drawing.Size(527, 135);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                       // this.Text = "Preass Groups";
                        txtFrom.Visible = false;
                        txtTo.Visible = false;
                        lblFrom.Visible = false;
                        lblReqPR.Visible = false;
                        lblTo.Visible = false;
                        lblReqCode.Visible = false;
                        lblPointRng.Visible = false;
                        txtCode.Enabled = false;
                        txtCode.Validator = TextBoxValidation.IntegerValidator;
                        List<RankCatgEntity> Ranks = _model.SPAdminData.Browse_PreassGroups();
                        if (Ranks.Count > 0)
                        {
                            string Rank = string.Empty;
                            foreach (RankCatgEntity Entity in Ranks)
                            {
                                if (string.IsNullOrEmpty(Entity.SubCode.Trim()))
                                {
                                    Rank = Entity.Code.Trim();
                                }
                            }
                            if (string.IsNullOrEmpty(Rank.Trim()))
                                txtCode.Text = "1";
                            else
                                txtCode.Text = (int.Parse(Rank.Trim()) + 1).ToString();

                        }
                        else
                            txtCode.Text = "1";
                    }
                    else
                    {
                        this.Text = "Preass Group Points"/*priviliges.Program*/ + " - Add";
                        lblHeadstart.Visible = false;
                        cmbRankCat.Visible = false; lblReqPR.Visible = true;
                       // this.Text = "Preass Group Points";
                        this.Size = new System.Drawing.Size(527, 169);
                        this.lblPointRng.Location = new System.Drawing.Point(15, 47);
                        this.lblReqPR.Location = new System.Drawing.Point(89, 44);
                        this.lblFrom.Location = new System.Drawing.Point(147, 47);
                        this.txtFrom.Location = new System.Drawing.Point(191, 43);
                        this.lblTo.Location = new System.Drawing.Point(266, 47);
                        this.txtTo.Location = new System.Drawing.Point(289, 43);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Left;
                        txtFrom.Visible = true;
                        txtTo.Visible = true;
                        lblReqCode.Visible = true;
                        lblFrom.Visible = true;
                        lblTo.Visible = true;
                        lblPointRng.Visible = true;
                        txtFrom.Validator = TextBoxValidation.FloatValidator;
                        txtTo.Validator = TextBoxValidation.FloatValidator;

                    }
                    break;
            }

        }


        #region properties

        public BaseForm Baseform { get; set; }

        public PrivilegeEntity priviliges { get; set; }

        public string mode { get; set; }

        public string type { get; set; }

        public string Agency { get; set; }

        public string RankCode { get; set; }

        public string SubCode { get; set; }

        public bool IsSaveValid { get; set; }

        public string propFromType { get; set; }

        #endregion


        private void HeadStartCombo()
        {
            cmbRankCat.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("None", "N"));
            listItem.Add(new ListItem("Primary", "P"));
            listItem.Add(new ListItem("Secondary", "S"));
            listItem.Add(new ListItem("Additional", "A"));
            cmbRankCat.Items.AddRange(listItem.ToArray());
            cmbRankCat.SelectedIndex = 0;
        }

        private void fillRankCntrols()
        {
            List<RankCatgEntity> RnkCntrls;
            RnkCntrls = _model.SPAdminData.Browse_RankCtg();

            if (RnkCntrls.Count > 0)
            {
                foreach (RankCatgEntity drRanks in RnkCntrls)
                {
                    if (drRanks.Code.ToString() == RankCode.ToString() && string.IsNullOrWhiteSpace(drRanks.SubCode) && drRanks.Agency==Agency)
                    {
                        txtCode.Text = drRanks.Code.ToString();
                        txtDesc.Text = drRanks.Desc.ToString();
                        if (cmbRankCat != null && cmbRankCat.Items.Count > 0)
                        {
                            foreach (ListItem list in cmbRankCat.Items)
                            {
                                if (list.Value.Equals(drRanks.HeadStrt.ToString()) || list.Text.Equals(drRanks.HeadStrt.ToString()))
                                {
                                    cmbRankCat.SelectedItem = list;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void fillPointrangeControls()
        {
            List<RankCatgEntity> PointCntrls;
            PointCntrls = _model.SPAdminData.Browse_RankCtg();

            if (PointCntrls.Count > 0)
            {
                foreach (RankCatgEntity drPoint in PointCntrls)
                {
                    if (drPoint.Code.ToString() == RankCode.ToString() && drPoint.SubCode.ToString() == SubCode.ToString() && drPoint.Agency==Agency)
                    {
                        txtCode.Text = drPoint.SubCode.ToString();
                        txtDesc.Text = drPoint.Desc.ToString();
                        txtFrom.Text = drPoint.PointsLow.ToString();
                        txtTo.Text = drPoint.PointsHigh.ToString();
                    }
                }
            }
        }


        private void fillPreassGroupCntrols()
        {
            List<RankCatgEntity> RnkCntrls;
            RnkCntrls = _model.SPAdminData.Browse_PreassGroups();

            if (RnkCntrls.Count > 0)
            {
                foreach (RankCatgEntity drRanks in RnkCntrls)
                {
                    if (drRanks.Code.ToString() == RankCode.ToString() && string.IsNullOrWhiteSpace(drRanks.SubCode))
                    {
                        txtCode.Text = drRanks.Code.ToString();
                        txtDesc.Text = drRanks.Desc.ToString();                        
                    }
                }
            }
        }

        private void fillPreassGroupPointsControls()
        {
            List<RankCatgEntity> PointCntrls;
            PointCntrls = _model.SPAdminData.Browse_PreassGroups();

            if (PointCntrls.Count > 0)
            {
                foreach (RankCatgEntity drPoint in PointCntrls)
                {
                    if (drPoint.Code.ToString() == RankCode.ToString() && drPoint.SubCode.ToString() == SubCode.ToString())
                    {
                        txtCode.Text = drPoint.SubCode.ToString();
                        txtDesc.Text = drPoint.Desc.ToString();
                        txtFrom.Text = drPoint.PointsLow.ToString();
                        txtTo.Text = drPoint.PointsHigh.ToString();
                    }
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (propFromType == "Groups")
            {
                string stroutmsg = string.Empty;

                if (type == "PREASSGROUPS")
                {

                    try
                    {

                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            RankCatgEntity RankEntity = new RankCatgEntity();
                           
                            RankEntity.Code = txtCode.Text;
                            RankEntity.Desc = txtDesc.Text;                           
                            RankEntity.PointsLow = "0";
                            RankEntity.PointsHigh = "0";
                            RankEntity.addoperator = Baseform.UserID;
                            RankEntity.lstcOperator = Baseform.UserID;
                            RankEntity.Mode = mode;

                            if (_model.SPAdminData.InsertUpdatePreassGroup(RankEntity, out stroutmsg))
                            {
                            }
                            //this.Close();

                            if (mode == "Add")
                            {
                                //MessageBox.Show("Ranking Association Details Inserted Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                //MessageBox.Show("Preass Group Updated Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }

                            mode = "View";
                            //CriticalActivity RankControl = Baseform.GetBaseUserControl() as CriticalActivity;
                            //if (RankControl != null)
                            //{
                            //    RankControl.RefreshRankGrid();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
                else
                {
                    try
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            RankCatgEntity pointEntity = new RankCatgEntity();

                            pointEntity.Agency = Agency.Trim();
                            pointEntity.Code = RankCode.ToString();
                            pointEntity.SubCode = txtCode.Text;
                            pointEntity.Desc = txtDesc.Text;
                            pointEntity.PointsLow = txtFrom.Text;
                            pointEntity.PointsHigh = txtTo.Text;
                            pointEntity.addoperator = Baseform.UserID;
                            pointEntity.lstcOperator = Baseform.UserID;
                            pointEntity.Mode = mode;

                            if (_model.SPAdminData.InsertUpdatePreassGroup(pointEntity, out stroutmsg))
                            {
                            }
                            //this.Close();

                            if (mode == "Add")
                            {
                                //MessageBox.Show("Preass Group Point Saved Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                               // MessageBox.Show("Preass Group Point Updated Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }

                            mode = "View";
                            //CriticalActivity RankControl = Baseform.GetBaseUserControl() as CriticalActivity;
                            //if (RankControl != null)
                            //{
                            //    RankControl.RefreshPointGrid();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            else
            {
                if (type == "Rank")
                {

                    try
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            RankCatgEntity RankEntity = new RankCatgEntity();

                            RankEntity.Agency = Agency.Trim();
                            RankEntity.Code = txtCode.Text;
                            RankEntity.Desc = txtDesc.Text;
                            RankEntity.HeadStrt = ((ListItem)cmbRankCat.SelectedItem).Value.ToString();
                            RankEntity.PointsLow = "0";
                            RankEntity.PointsHigh = "0";
                            RankEntity.addoperator = Baseform.UserID;
                            RankEntity.lstcOperator = Baseform.UserID;
                            RankEntity.Mode = mode;

                            if (_model.SPAdminData.InsertRankPoints(RankEntity))
                            {
                            }
                            //this.Close();

                            if (mode == "Add")
                            {
                                //MessageBox.Show("Ranking Association Details Inserted Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                //MessageBox.Show("Ranking Association Details Updated Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }

                            mode = "View";
                            //CriticalActivity RankControl = Baseform.GetBaseUserControl() as CriticalActivity;
                            //if (RankControl != null)
                            //{
                            //    RankControl.RefreshRankGrid();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
                else
                {
                    try
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            RankCatgEntity pointEntity = new RankCatgEntity();

                            pointEntity.Agency = Agency.Trim();
                            pointEntity.Code = RankCode.ToString();
                            pointEntity.SubCode = txtCode.Text;
                            pointEntity.Desc = txtDesc.Text;
                            pointEntity.PointsLow = txtFrom.Text;
                            pointEntity.PointsHigh = txtTo.Text;
                            pointEntity.addoperator = Baseform.UserID;
                            pointEntity.lstcOperator = Baseform.UserID;
                            pointEntity.Mode = mode;

                            if (_model.SPAdminData.InsertRankPoints(pointEntity))
                            {
                            }
                            //this.Close();

                            if (mode == "Add")
                            {
                               // MessageBox.Show("Point Range Association Details Saved Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                //MessageBox.Show("Point Range Association Details Updated Successfully", "CAPTAIN");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }

                            mode = "View";
                            //CriticalActivity RankControl = Baseform.GetBaseUserControl() as CriticalActivity;
                            //if (RankControl != null)
                            //{
                            //    RankControl.RefreshPointGrid();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        public bool ValidateForm()
        {
            bool isValid = true;

             if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrWhiteSpace(txtCode.Text))
             {
                 _errorProvider.SetError(txtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                 isValid = false;
             }
             else
             {
                 if (isCodeExists(txtCode.Text))
                 {
                    _errorProvider.SetError(txtCode, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblCode.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                 }
                 else if (type == "Rank")
                 {
                     if (int.Parse(txtCode.Text.Trim()) < 1)
                     {
                         _errorProvider.SetError(txtCode, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                         isValid = false;
                     }
                 }
                 else
                 {
                     _errorProvider.SetError(txtCode, null);
                     
                 }
             }

             if (type == "Rank")
             {
                 

                 if (isRankCatExists(((ListItem)cmbRankCat.SelectedItem).Value.ToString()))
                 {
                     _errorProvider.SetError(cmbRankCat, string.Format("There are 2 or more Ranking Categories with the same Head Start Ranking Category".Replace(Consts.Common.Colon, string.Empty)));
                     isValid = false;
                 }
                 else
                     _errorProvider.SetError(cmbRankCat, null);
             }
 
             if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
             {
                    _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lbldesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
             }
             else
             {
                _errorProvider.SetError(txtDesc, null);
             }

             if (type == "PointRange")
             {
                 //if(

                 if (string.IsNullOrEmpty(txtTo.Text) || string.IsNullOrWhiteSpace(txtTo.Text) || string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrWhiteSpace(txtFrom.Text))
                 {
                     _errorProvider.SetError(txtTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFrom.Text + " and " + lblTo.Text.Replace(Consts.Common.Colon, string.Empty)));
                     isValid = false;
                 }
                 else if (decimal.Parse(txtTo.Text) < decimal.Parse(txtFrom.Text))
                 {
                     _errorProvider.SetError(txtTo, string.Format(lblFrom.Text + " value Should not be greater than or equal " + lblTo.Text + " value".Replace(Consts.Common.Colon, string.Empty)));
                     isValid = false;
                 }
                 else
                 {
                     _errorProvider.SetError(txtTo, null);
                 }
             }             

             
            
             IsSaveValid = isValid;

             return (isValid);
        }


        private bool isCodeExists(string Code)
        {
            bool isExists = false;
            if (propFromType == "Groups")
            {
                if (mode.Equals("Add"))
                {
                    if (type == "Rank")
                    {
                        DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_PreassGroups();
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["PREASSGRP_CODE"].ToString().Trim().TrimStart('0') == Code.ToString().Trim().TrimStart('0'))
                                {
                                    isExists = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        DataSet dsTb = Captain.DatabaseLayer.SPAdminDB.Browse_PreassGroups();
                        DataTable dtTb = dsTb.Tables[0];
                        if (dtTb.Rows.Count > 0)
                        {
                            foreach (DataRow drTb in dtTb.Rows)
                            {
                                string SubCode = drTb["PREASSGRP_SUBCODE"].ToString().Trim();
                                if (SubCode.Trim() == Code.ToString().Trim() && drTb["PREASSGRP_CODE"].ToString().Trim() == RankCode.Trim())
                                    isExists = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (mode.Equals("Add"))
                {
                    if (type == "Rank")
                    {
                        DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_RnkCatg();
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["RANKCTG_GRPCTG_CODE"].ToString().Trim().TrimStart('0') == Code.ToString().Trim().TrimStart('0') && dr["RANKCTG_AGENCY"].ToString().Trim() == Agency)
                                {
                                    isExists = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        DataSet dsTb = Captain.DatabaseLayer.SPAdminDB.Browse_RnkCatg();
                        DataTable dtTb = dsTb.Tables[0];
                        if (dtTb.Rows.Count > 0)
                        {
                            foreach (DataRow drTb in dtTb.Rows)
                            {
                                string SubCode = drTb["RANKCTG_SUBCTG_CODE"].ToString().Trim();
                                if (SubCode.Trim() == Code.ToString().Trim() && drTb["RANKCTG_GRPCTG_CODE"].ToString().Trim() == RankCode.Trim() && drTb["RANKCTG_AGENCY"].ToString().Trim() == Agency)
                                    isExists = true;
                            }
                        }
                    }
                }
            }
            return isExists;
        }

        public bool isRankCatExists(string Cat)
        {
            bool isExists = false;
            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_RnkCatg();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Cat.ToString().Trim() != "N")
                    {
                        if (dr["RANKCTG_HS_CTG"].ToString().Trim() == Cat.ToString().Trim() && txtCode.Text.Trim().TrimStart('0') != dr["RANKCTG_GRPCTG_CODE"].ToString().Trim().TrimStart('0') && dr["RANKCTG_AGENCY"].ToString().Trim() == Agency)
                        {
                            isExists = true;
                        }
                    }
                }

            }
            return isExists;
        }

        public string[] GetSelected_Rank_Code()
        {
            string[] Added_Edited_RankCode = new string[3];
            Added_Edited_RankCode[0] = txtCode.Text;
            Added_Edited_RankCode[1] = mode;
            Added_Edited_RankCode[2] = Agency;
            return Added_Edited_RankCode;
        }

        public string[] GetSelected_RankSubCatg_Code()
        {
            string[] Added_Edited_SubCode = new string[3];
            Added_Edited_SubCode[0] = RankCode;
            Added_Edited_SubCode[1] = txtCode.Text;
            Added_Edited_SubCode[2] = mode;
            return Added_Edited_SubCode;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCode_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                //if (type == "Rank")
                //{
                //    if (int.Parse(txtCode.Text.Trim()) < 1)
                //        _errorProvider.SetError(txtCode, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                //    else
                //        _errorProvider.SetError(txtCode, null);
                //}
                //else
                    _errorProvider.SetError(txtCode, null);
            }
        }

        private void txtDesc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDesc.Text))
                _errorProvider.SetError(txtDesc, null);
        }

        private void txtTo_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTo.Text))
                _errorProvider.SetError(txtTo, null);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Ranking Categories Form");
        }

    }
}