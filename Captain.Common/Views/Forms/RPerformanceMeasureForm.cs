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
using DevExpress.CodeParser;
using DevExpress.XtraRichEdit.Model;
using static NPOI.HSSF.Util.HSSFColor;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RPerformanceMeasureForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        public string _TabType = "";
        #endregion

        public RPerformanceMeasureForm(BaseForm baseform, string mode, string grid, string GrpCd, string TblCd, string RefFdate, string RefTdate, string BtnMode, PrivilegeEntity privileage, string TabType, string pprsw)
        {
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();
            _TabType = TabType;
            BaseForm = baseform;
            Privileage = privileage;
            Mode = mode;
            GridType = grid;
            refFdate = RefFdate;
            refTdate = RefTdate;
            groupCd = GrpCd;
            tablecd = TblCd;
            btn_mode = BtnMode;
            PPR_SW = pprsw;
            //if(GridType=="Group")
            //    txtCode.Validator = TextBoxValidation.IntegerValidator;

            txtAgeFrm.Validator = TextBoxValidation.IntegerValidator;
            txtAgeTo.Validator = TextBoxValidation.IntegerValidator;
            txtExpAch.Validator = TextBoxValidation.IntegerValidator;

            txtBenFResnDomain.Validator = TextBoxValidation.IntegerValidator;

            FillCmbHie();
            FillAchieveResCombo();
            fillCntIndicator();
            pnlCopyTargets.Visible = false;
            switch (Mode)
            {
                case "Edit":

                    if (GridType == "Code")
                    {
                        this.Text = "Outcome Indicators Definition " + "- Edit";
                        if (_TabType == "RServices")
                            this.Text = "Service Measures Definition - Edit";
                        FillCodeControls();
                        //this.Size = new System.Drawing.Size(600, 270);//250
                        pnlGVGroup.Visible = false;
                        pnlDates.Visible = true;
                        chkbCopy.Visible = false;
                        cmbRefPer.Visible = false;
                        lblRefPer.Visible = false;
                        pnlDates.Size = new Size(577, 40);
                        this.pnlCodeDesc.Size = new System.Drawing.Size(577, 101);//(683, 86);
                        //this.Height = this.Height - (pnlTable.Height + pnlGVGroup.Height + 30);
                        this.Size = new Size(577, 235);
                        this.chkbActive.Location = new System.Drawing.Point(461, 11);
                        this.chkbActive.Visible = true;

                        //this.pnlGVGroup.Size = new System.Drawing.Size(598, 62);
                        //this.btnGSave.Location = new System.Drawing.Point(429, 215);
                        //this.button3.Location = new System.Drawing.Point(507, 215);
                        //this.pnlDates.Location = new System.Drawing.Point(2, 140);
                        //this.pnlSave.Location = new System.Drawing.Point(2, 202);

                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                        this.txtCode.MaxLength = 80;
                        txtCode.Visible = false; lblCode.Visible = false; lblReqCode.Visible = false;
                        BtnResult.Visible = false;
                        pnlTable.Visible = false;
                        cmbCntInd.Visible = false;
                        lblCntInd.Visible = false;
                        cmbHie.Visible = true; cmbHie.Enabled = false;
                        lblAgency.Visible = true; lblAgency.Text = "Agency";
                        this.lblAgency.Location = new System.Drawing.Point(15, 15);//(25, 12); //55
                        this.cmbHie.Location = new System.Drawing.Point(100, 11);// (124, 9); //134
                        lblReqDesc.Location = new Point(81, 60);
                        txtDesc.Location = new Point(100, 43);
                        txtDesc.Size = new Size(444, 56);
                        dtpFrom.Location = new Point(100, 5); lblToDt.Location = new Point(242, 9); dtpTo.Location = new Point(297, 5);
                        lblCode.Text = "Code";
                        lblDesc.Text = "Description";
                        //LblHeader.Text = "Definition";
                        //if (btn_mode == "Copy")
                        //{
                        //    this.Text = privileage.Program + " - Group Add";
                        //    txtCode.Enabled = true;
                        //    BtnResult.Visible = false;
                        //    Mode = "Add";
                        //}
                        //else
                        //{
                        txtCode.Enabled = false;
                        Mode = "Edit";

                        if (_TabType == "RServices")
                        {

                            SGoalEntity = _model.SPAdminData.Browse_RNGSRGA(refFdate, refTdate, "", "", null);
                            if (SGoalEntity.Count > 0)
                            {
                                chkbPPR.Enabled = false;
                                chkbPPR.ForeColor = System.Drawing.Color.Silver;
                            }
                            else
                            {
                                chkbPPR.Enabled = true;
                                chkbPPR.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        else if (_TabType == "RPerfMeasures")
                        {

                            GoalEntity = _model.SPAdminData.Browse_RNGGA(refFdate, refTdate, "", "", null);
                            if (GoalEntity.Count > 0)
                            {
                                chkbPPR.Enabled = false;
                                chkbPPR.ForeColor = System.Drawing.Color.Silver;
                            }
                            else
                            {
                                chkbPPR.Enabled = true;
                                chkbPPR.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        //}
                        chkbCSBG.Visible = false;
                        lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;
                    }
                    else if (GridType == "Group")
                    {
                        this.Text = "Outcome Indicators Domain Definition - Edit";
                        FillGroupControls();
                        //this.Size = new System.Drawing.Size(600, 270);//250
                        pnlGVGroup.Visible = true; pnlDates.Visible = false;
                        //this.pnlGVGroup.Size = new System.Drawing.Size(598, 62);
                        //**this.pnlCodeDesc.Size = new System.Drawing.Size(687, 128);//(687, 103);//(683, 86);
                        //this.Height = this.Height - (pnlTable.Height + pnlDates.Height+30);
                        //**this.Size = new Size(687, 290);//(687, 265);

                        ////this.pnlSave.Location = new System.Drawing.Point(2, 205);
                        //this.BtnResult.Location = new Point(510, 11);//(460, 6);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                        txtCode.Visible = true; lblCode.Visible = true; lblReqCode.Visible = true;
                        BtnResult.Visible = true;
                        pnlTable.Visible = false;
                        cmbCntInd.Visible = false;
                        lblCntInd.Visible = false;
                        //cmbHie.Visible = false;
                        //lblAgency.Visible = false;
                        if (_TabType == "RServices")
                        {
                            if (PPR_SW == "Y")
                            {
                                chkbCSBG.Visible = true;
                                lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = true;

                                if (chkbCSBG.Checked)
                                {
                                    lblBenfResnDomain_Req.Visible = true;
                                    txtBenFResnDomain.Enabled = true;
                                }
                                else
                                {
                                    lblBenfResnDomain_Req.Visible = false;
                                    txtBenFResnDomain.Enabled = false;
                                    txtBenFResnDomain.Text = string.Empty;
                                }

                                this.pnlCodeDesc.Size = new System.Drawing.Size(687, 130);//(687, 128);
                                this.Size = new Size(687, 292);//(687, 290);
                            }
                            else
                            {
                                chkbCSBG.Visible = false;
                                lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;

                                this.pnlCodeDesc.Size = new System.Drawing.Size(687, 103);
                                this.Size = new Size(687, 265);
                            }

                            if (PPR_SW != "Y")
                            {
                                lblAgency.Visible = false; cmbHie.Visible = false;
                            }
                            else
                            {
                                lblAgency.Visible = true; cmbHie.Visible = true; lblAgency.Text = "Hierarchy";
                                this.lblAgency.Location = new Point(355, 15);//(25, 12); //55
                                this.cmbHie.Location = new Point(421, 11);
                            }
                        }
                        else
                        {
                            this.pnlCodeDesc.Size = new System.Drawing.Size(687, 103);
                            this.Size = new Size(687, 265);

                            chkbCSBG.Visible = false;
                            lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;

                            if (PPR_SW != "Y")
                            {
                                lblAgency.Visible = false; cmbHie.Visible = false;
                            }
                            else
                            {
                                lblAgency.Visible = true; cmbHie.Visible = true; lblAgency.Text = "Hierarchy";
                                this.lblAgency.Location = new Point(355, 15);//(25, 12); //55
                                this.cmbHie.Location = new Point(421, 11);
                            }
                        }
                        cmbHie.TabIndex = 1; txtDesc.TabIndex = 2;
                        chkb1.TabIndex = 3; txtnum1.TabIndex = 4; chkb2.TabIndex = 5; txtnum2.TabIndex = 6; chkb3.TabIndex = 7; txtnum3.TabIndex = 8; chkb4.TabIndex = 9; txtnum4.TabIndex = 10;
                        chkb5.TabIndex = 11; txtnum5.TabIndex = 12; cmbAchvResCol.TabIndex = 13; BtnResult.TabIndex = 14; btnGSave.TabIndex = 15; btnGCancel.TabIndex = 16;


                        //if (_TabType== "RServices")
                        //{
                        //    lblCode.Text = "Group Code";
                        //    lblReqCode.Location = new Point(83, 12);
                        //    lblDesc.Text = "Group Description";
                        //    lblReqDesc.Location = new Point(119, 60);
                        //}
                        //else
                        //{
                        //    lblCode.Text = "Domain Code";
                        //    lblDesc.Text = "Domain Description";
                        //}
                        //LblHeader.Text = "Group Definition";
                        if (btn_mode == "Copy")
                        {
                            this.Text = "Outcome Indicators Domain Definition - Add";
                            txtCode.Enabled = true;
                            if (_TabType == "RServices")
                            {
                                this.Text = "Service Measures Group Definition - Add";
                                BtnResult.Visible = false;
                                pnlGVGroup.Visible = false;
                                //this.Height = this.Height - (pnlGVGroup.Height);
                                this.Size = new Size(692, 260);

                            }
                            else pnlGVGroup.Visible = true;
                            BtnResult.Visible = false;
                            Mode = "Add";
                        }
                        else
                        {
                            if (_TabType == "RServices")
                            {
                                this.Text = "Domain Definition – Edit";//"Service Measures Group Definition - Edit";
                                //this.pnlSave.Location = new System.Drawing.Point(2, 144);
                                //this.Size = new System.Drawing.Size(600, 175);
                                pnlGVGroup.Visible = false;
                                BtnResult.Visible = false;
                                this.Height = this.Height - (pnlGVGroup.Height);
                            }
                            txtCode.Enabled = false;
                            Mode = "Edit";
                        }

                    }
                    else if (GridType == "Table")
                    {
                        chkIndSwitch.Checked = false;
                        this.Text = "Outcome Indicators Group Definition - Edit";
                        if (_TabType == "RServices")
                            this.Text = "Service Measures Group Definition - Edit";//"Service Measures Table Definition - Edit";
                        fillTableControls();
                        //this.Size = new System.Drawing.Size(600, 498);
                        pnlGVGroup.Visible = false; pnlDates.Visible = false;

                        this.pnlCodeDesc.Size = new System.Drawing.Size(687, 103);
                        //this.Height = this.Height - (pnlGVGroup.Height + pnlDates.Height + 30);
                        this.Size = new Size(687, 660);



                        //this.pnlGvTable.Size = new System.Drawing.Size(598, 312);
                        //this.pnlGvTable.Location = new System.Drawing.Point(2, 144);
                        //this.btnGSave.Location = new System.Drawing.Point(429, 218);
                        //this.button3.Location = new System.Drawing.Point(507, 218);
                        //this.pnlSave.Location = new System.Drawing.Point(2, 455);f
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Left;
                        txtCode.Visible = true; lblCode.Visible = true; lblReqCode.Visible = true;
                        pnlTable.Visible = true;
                        cmbCntInd.Visible = true;
                        cmbHie.Visible = false;
                        lblAgency.Visible = false;
                        BtnResult.Visible = false;
                        lblCntInd.Visible = true;
                        txtCode.Enabled = false;
                        lblReqDesc.Location = new Point(115, 60);
                        lblReqCode.Location = new Point(79, 11);

                        //if (_TabType == "RServices")
                        //{
                        //    //lblCode.Text = "Table Code";
                        //    //lblDesc.Text = "Table Description";

                        //}
                        //else
                        {
                            lblCode.Text = "Group Code";
                            lblReqCode.Location = new Point(83, 12);
                            lblDesc.Text = "Group Description";
                            lblReqDesc.Location = new Point(119, 60);

                            pnlCopyTargets.Visible = true;
                            this.pnlCopyTargets.Size = new System.Drawing.Size(404, 32);
                            this.pnlCopyTargets.Location = new System.Drawing.Point(11, 3);

                        }

                        //LblHeader.Text = "Table Definition";
                        if (_TabType == "RServices") BtnGoal.Text = "Service &Associations"; else BtnGoal.Text = "&Outcome Associations";

                        if (_TabType == "RServices")
                        {
                            Services = "CA";
                        }
                        else Services = "MS";

                        //BtnResult.Visible = false;
                        gvGoals.Enabled = true;
                        fillGvGoals();
                        ////if (chkbPrograms.Checked) FillProgramsGrid();
                        //if(PPR_SW!="Y")
                        //    FillGrid();

                        chkbCSBG.Visible = false;
                        lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;
                    }
                    break;
                case "Add":

                    if (GridType == "Code")
                    {
                        this.Text = "Outcome Indicators Definition - Add";
                        if (_TabType == "RServices")
                            this.Text = "Service Measures Definition - Add";
                        //this.Size = new System.Drawing.Size(600, 270);//250
                        pnlGVGroup.Visible = false; pnlDates.Visible = true;
                        this.pnlCodeDesc.Size = new System.Drawing.Size(577, 101);//(683, 86);
                        // this.Height = this.Height - (pnlTable.Height + pnlGVGroup.Height+30);
                        this.Size = new Size(577, 266);
                        this.chkbActive.Location = new System.Drawing.Point(461, 11);
                        this.chkbActive.Visible = true;
                        //this.pnlGVGroup.Size = new System.Drawing.Size(598, 62);

                        //this.pnlDates.Location = new System.Drawing.Point(2, 140);
                        //this.pnlSave.Location = new System.Drawing.Point(2, 202);
                        //this.pnlSave.Location = new System.Drawing.Point(2, 144);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                        txtCode.Visible = false; lblCode.Visible = false; lblReqCode.Visible = false;
                        pnlTable.Visible = false;
                        BtnResult.Visible = false;
                        cmbCntInd.Visible = false;
                        lblCntInd.Visible = false;
                        cmbHie.Visible = true; cmbHie.Enabled = true;
                        lblAgency.Visible = true; lblAgency.Text = "Agency";
                        this.lblAgency.Location = new Point(15, 15);//(25, 12); //55
                        this.cmbHie.Location = new Point(100, 11);//(124, 9);
                        cmbHie.TabIndex = 1; chkbActive.TabIndex = 2; txtDesc.TabIndex = 3;
                        dtpFrom.TabIndex = 4; dtpTo.TabIndex = 5; chkbCopy.TabIndex = 6; cmbRefPer.TabIndex = 7;
                        btnGSave.TabIndex = 8; btnGCancel.TabIndex = 9;
                        lblReqDesc.Location = new Point(81, 60);
                        txtDesc.Location = new Point(100, 43);
                        txtDesc.Size = new Size(444, 56);
                        cmbRefPer.Size = new Size(319, 25);
                        dtpFrom.Location = new Point(100, 5); lblToDt.Location = new Point(242, 9); dtpTo.Location = new Point(297, 5);
                        lblCode.Text = "Code";
                        lblDesc.Text = "Description";
                        //LblHeader.Text = "Definition";
                        //BtnResult.Visible = false;
                        txtCode.Validator = TextBoxValidation.IntegerValidator;

                        chkbCSBG.Visible = false;
                        lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;
                    }

                    else if (GridType == "Group")
                    {
                        this.Text = "Outcome Indicators Domain Definition - Add";
                        //this.Size = new System.Drawing.Size(600, 270); //250
                        pnlGVGroup.Visible = true; pnlDates.Visible = false;
                        //**this.pnlCodeDesc.Size = new System.Drawing.Size(687, 128);//(687, 103);
                        //this.Height = this.Height - (pnlTable.Height + pnlDates.Height+30);
                        //**this.Size = new Size(687, 290);//(687, 265);

                        //this.pnlGVGroup.Size = new System.Drawing.Size(598, 62);
                        //this.btnGSave.Location = new System.Drawing.Point(429, 215);
                        //this.button3.Location = new System.Drawing.Point(507, 215);
                        //this.pnlSave.Location = new System.Drawing.Point(2, 205);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Right;
                        txtCode.Visible = true; lblCode.Visible = true; lblReqCode.Visible = true;

                        if (_TabType == "RServices")
                        {
                            this.Text = "Domain Definition – Add";//"Service Measures Group Definition - Add";
                            //this.pnlSave.Location = new System.Drawing.Point(2, 144);
                            //this.Size = new System.Drawing.Size(600, 175);
                            BtnResult.Visible = false; pnlGVGroup.Visible = false;
                            //this.Height = this.Height - (pnlGVGroup.Height);

                            if (PPR_SW == "Y")
                            {
                                chkbCSBG.Visible = true;
                                lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = true;

                                if (chkbCSBG.Checked)
                                {
                                    lblBenfResnDomain_Req.Visible = true;
                                    txtBenFResnDomain.Enabled = true;
                                }
                                else
                                {
                                    lblBenfResnDomain_Req.Visible = false;
                                    txtBenFResnDomain.Enabled = false;
                                    txtBenFResnDomain.Text = string.Empty;
                                }

                                this.pnlCodeDesc.Size = new System.Drawing.Size(687, 130);//(687, 128);
                                this.Size = new Size(687, 290);
                            }
                            else
                            {
                                chkbCSBG.Visible = false;
                                lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;

                                this.pnlCodeDesc.Size = new System.Drawing.Size(687, 103);
                                this.Size = new Size(687, 265);
                            }

                            this.Height = this.Height - (pnlGVGroup.Height);//this.Size = new Size(692, 200);
                        }
                        else
                        {
                            this.pnlCodeDesc.Size = new System.Drawing.Size(687, 103);
                            this.Size = new Size(687, 265);

                            chkb1.Checked = true; txtnum1.Text = "Achieved";
                            chkb2.Checked = true; txtnum2.Text = "Progressing";
                            chkb3.Checked = true; txtnum3.Text = "Terminated";
                            chkb4.Checked = true; txtnum4.Text = "Withdrew";

                            chkbCSBG.Visible = false;
                            lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;
                        }
                        pnlTable.Visible = false;
                        BtnResult.Visible = false;
                        cmbCntInd.Visible = false;
                        lblCntInd.Visible = false;
                        //cmbHie.Visible = false;
                        //lblAgency.Visible = false;
                        if (_TabType == "RServices")
                        {
                            if (PPR_SW != "Y")
                            {
                                lblAgency.Visible = false; cmbHie.Visible = false;
                            }
                            else
                            {
                                lblAgency.Visible = true; cmbHie.Visible = true; lblAgency.Text = "Hierarchy";
                                this.lblAgency.Location = new Point(355, 15);//(25, 12); //55
                                this.cmbHie.Location = new Point(421, 11);
                            }
                        }
                        else
                        {
                            if (PPR_SW != "Y")
                            {
                                lblAgency.Visible = false; cmbHie.Visible = false;
                            }
                            else
                            {
                                lblAgency.Visible = true; cmbHie.Visible = true; lblAgency.Text = "Hierarchy";
                                this.lblAgency.Location = new Point(355, 15);//(25, 12); //55
                                this.cmbHie.Location = new Point(421, 11);
                            }
                        }

                        txtCode.TabIndex = 1; cmbHie.TabIndex = 2; txtDesc.TabIndex = 3;
                        chkb1.TabIndex = 4; txtnum1.TabIndex = 5; chkb2.TabIndex = 6; txtnum2.TabIndex = 7; chkb3.TabIndex = 8; txtnum3.TabIndex = 9; chkb4.TabIndex = 10; txtnum4.TabIndex = 11;
                        chkb5.TabIndex = 12; txtnum5.TabIndex = 13; cmbAchvResCol.TabIndex = 14; btnGSave.TabIndex = 15; btnGCancel.TabIndex = 16;

                        if (_TabType == "RServices")
                        {
                            lblCode.Text = "Domain Code";
                            lblReqCode.Location = new Point(92, 12);
                            lblDesc.Text = "Domain Description";
                            lblReqDesc.Location = new Point(127, 60);
                        }
                        else
                        {
                            lblCode.Text = "Domain Code";
                            lblDesc.Text = "Domain Description";
                            lblReqCode.Location = new Point(92, 12);
                            lblReqDesc.Location = new Point(127, 60);
                        }
                        //LblHeader.Text = "Group Definition";
                        //BtnResult.Visible = false;
                    }
                    else if (GridType == "Table")
                    {
                        this.Text = "Outcome Indicators Group Definition - Add";
                        if (_TabType == "RServices")
                            this.Text = "Service Measures Group Definition - Add";//"Service Measures Table Definition - Add";
                        //this.Size = new System.Drawing.Size(600, 270);//250
                        pnlGVGroup.Visible = false; pnlDates.Visible = false;
                        this.pnlCodeDesc.Size = new System.Drawing.Size(687, 103);

                        this.pnlTable.Height = this.pnlTable.Height - pnlGvTable.Height;
                        //this.Height = this.Height - (pnlGVGroup.Height + pnlGvTable.Height+pnlDates.Height + 30);
                        this.Size = new Size(687, 233);

                        //this.pnlGvTable.Location = new System.Drawing.Point(2, 144);
                        //this.pnlGvTable.Size = new System.Drawing.Size(598, 62);

                        //this.pnlSave.Location = new System.Drawing.Point(2, 205);
                        this.txtCode.TextAlign = Wisej.Web.HorizontalAlignment.Left;
                        txtCode.Visible = true; lblCode.Visible = true; lblReqCode.Visible = true;
                        pnlTable.Visible = true; pnlTableField.Visible = true; pnlGvTable.Visible = false;
                        BtnGoal.Visible = false; chkbPrograms.Visible = false;
                        BtnResult.Visible = false;
                        cmbCntInd.Visible = true;
                        lblCntInd.Visible = true;
                        // chkIndSwitch.Visible = true;
                        pnlgvwHie.Visible = false;
                        cmbHie.Visible = false;
                        lblAgency.Visible = false;
                        lblReqDesc.Location = new Point(114, 60);
                        lblReqCode.Location = new Point(80, 12);
                        if (_TabType == "RServices")
                        {
                            lblCode.Text = "Group Code";
                            lblDesc.Text = "Group Description";
                            lblReqCode.Location = new Point(83, 12);
                            lblReqDesc.Location = new Point(119, 60);
                        }
                        else
                        {
                            lblCode.Text = "Group Code";
                            lblReqCode.Location = new Point(83, 12);
                            lblDesc.Text = "Group Description";
                            lblReqDesc.Location = new Point(119, 60);
                        }
                        //LblHeader.Text = "Table Definition";
                        gvGoals.Enabled = false;
                        //this.lblAgeFrm.Location = new System.Drawing.Point(54, 40);
                        //this.txtAgeFrm.Location = new System.Drawing.Point(116, 37);
                        //this.lblAgeTo.Location = new System.Drawing.Point(149, 68);
                        //this.txtAgeTo.Location = new System.Drawing.Point(192, 37);

                        chkbCSBG.Visible = false;
                        lblBenfResnDomain.Visible = txtBenFResnDomain.Visible = false;
                    }
                    break;
                case "Delete":
                    this.Text = Privileage.Program + " - Delete";
                    break;
            }

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

        public string btn_mode { get; set; }

        public string ReferDesc { get; set; }

        public bool IsSaveValid { get; set; }

        public string PPR_SW { get; set; }

        #endregion

        private void FillAchieveResCombo()
        {
            cmbAchvResCol.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("*", "All"));
            listItem.Add(new ListItem("1", "1"));
            listItem.Add(new ListItem("2", "2"));
            listItem.Add(new ListItem("3", "3"));
            listItem.Add(new ListItem("4", "4"));
            listItem.Add(new ListItem("5", "5"));
            cmbAchvResCol.Items.AddRange(listItem.ToArray());
            this.cmbAchvResCol.SelectedIndexChanged -= new System.EventHandler(this.cmbAchvResCol_SelectedIndexChanged);
            if (Mode == "Add")
                cmbAchvResCol.SelectedIndex = 1;
            this.cmbAchvResCol.SelectedIndexChanged += new System.EventHandler(this.cmbAchvResCol_SelectedIndexChanged);
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

        private void SetRepComboBoxValue(ComboBox comboBox, string value)
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



        private void fillCntIndicator()
        {
            cmbCntInd.Items.Clear();
            List<ListItem> listcnt = new List<ListItem>();
            listcnt.Add(new ListItem("Applicant", "A"));
            listcnt.Add(new ListItem("Individual Households", "I"));
            listcnt.Add(new ListItem("All Household Members", "H"));
            listcnt.Add(new ListItem("Selected Household Members", "S"));
            cmbCntInd.Items.AddRange(listcnt.ToArray());
            cmbCntInd.SelectedIndex = 3;
        }

        private void FillCmbHie()
        {
            string UserAgency = string.Empty;
            if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            {
                HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******" && u.UsedFlag == "N");
                if (SelHie != null)
                    UserAgency = "**";
            }

            if (GridType == "Code")
            {
                DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once
                DataTable dt = ds.Tables[0];

                if (BaseForm.BaseAdminAgency != "**")
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "AGY= '" + BaseForm.BaseAdminAgency + "'";
                        dt = dv.ToTable();
                    }

                }




                List<RNKCRIT1Entity> RankHieList;
                //if(((RepListItem)cmbRnk.SelectedItem).Value.ToString()=="*")
                //RankHieList = _model.SPAdminData.GetRNKCRIT(null);
                //else
                //    RankHieList = _model.SPAdminData.GetRNKCRIT(((RepListItem)cmbRnk.SelectedItem).Value.ToString());
                cmbHie.Items.Clear();
                List<RepListItem> ListHie = new List<RepListItem>();
                if (dt.Rows.Count > 0)
                {
                    if (BaseForm.BaseAdminAgency == "**" || UserAgency == "**")
                        cmbHie.Items.Add(new RepListItem("**" + " - " + "All Agencies", "**"));
                    //foreach (RNKCRIT1Entity drRankHie in RankHieList)
                    //{
                    string Hierarchy = string.Empty;
                    foreach (DataRow dr in dt.Rows)//dr["Agy"] + " - " + dr["Name"], dr["Agy"]
                    {
                        Hierarchy = dr["Agy"] + " - " + dr["Name"].ToString();
                        cmbHie.Items.Add(new RepListItem(Hierarchy, dr["Agy"].ToString()));
                        //if (dr["Agy"].ToString().Trim() == drRankHie.Agency.Trim())
                        //{
                        //    Hierac = drRankHie.Agency.ToString();
                        //    Hierarchy = dr["Agy"] + " - " + dr["Name"].ToString();
                        //    break;
                        //}
                        //else if (drRankHie.Agency.Trim() == "**")
                        //{
                        //    Hierac = drRankHie.Agency.ToString();
                        //    Hierarchy = "**" + " - " + "All Agencies";
                        //    break;
                        //}
                    }
                    //cmbHie.Items.Add(new RepListItem(Hierarchy, Hierac));
                    //}
                }
                cmbHie.SelectedIndex = 0;
            }
            else if (GridType == "Group")
            {
                cmbHie.Items.Clear();
                int rowCnt = 0;
                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);

                List<RepListItem> ListHie = new List<RepListItem>();
                if (userHierarchy.Count > 0) userHierarchy = userHierarchy.FindAll(u => u.UsedFlag == "N");
                if (caseHierarchy.Count > 0)
                {
                    caseHierarchy = caseHierarchy.FindAll(u => u.Prog.Trim().Equals(""));
                    cmbHie.Items.Add(new RepListItem("", ""));


                    if (UserAgency == "**")
                        cmbHie.Items.Add(new RepListItem("**-**-**" + " - " + "All Hierarchies", "**-**-**"));

                    if (BaseForm.BaseAdminAgency != "**")
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Equals(BaseForm.BaseAdminAgency));
                    else
                        cmbHie.Items.Add(new RepListItem("**-**-**" + " - " + "All Hierarchies", "**-**-**"));



                    //if(userHierarchy.Count>0)
                    //{
                    //    foreach(HierarchyEntity entity in userHierarchy)
                    //    {
                    //        List<HierarchyEntity> SelectedHies = new List<HierarchyEntity>();

                    //        if (entity.Agency != "**" && entity.Dept != "**" && (entity.Prog == "**" || entity.Prog!="**"))
                    //            SelectedHies = caseHierarchy.FindAll(u => u.Agency == entity.Agency && u.Dept == entity.Dept);
                    //        else if (entity.Agency != "**" && entity.Dept == "**")
                    //            SelectedHies = caseHierarchy.FindAll(u => u.Agency == entity.Agency);
                    //        else if (entity.Agency == "**")
                    //            SelectedHies = caseHierarchy;

                    //        //if (entity.Agency!="**")
                    //        //    SelectedHies=caseHierarchy.FindAll(u=>u.Agency==entity.Agency && (u.Dept==entity.Dept || u.Dept==""));

                    //        if (SelectedHies.Count>0)
                    //        {
                    //            foreach (HierarchyEntity hierarchyEntity in SelectedHies)
                    //            {
                    //                string strAgency = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency;
                    //                string strDept = hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept;
                    //                string strProgram = hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                    //                string code = strAgency + "-" + strDept + "-" + strProgram;

                    //                cmbHie.Items.Add(new RepListItem(code + " - " + hierarchyEntity.HirarchyName, code));
                    //                rowCnt++;
                    //            }
                    //        }
                    //    }
                    //}

                    foreach (HierarchyEntity hierarchyEntity in caseHierarchy)
                    {
                        string strAgency = hierarchyEntity.Agency == string.Empty ? "**" : hierarchyEntity.Agency;
                        string strDept = hierarchyEntity.Dept == string.Empty ? "**" : hierarchyEntity.Dept;
                        string strProgram = hierarchyEntity.Prog == string.Empty ? "**" : hierarchyEntity.Prog;
                        string code = strAgency + "-" + strDept + "-" + strProgram;

                        cmbHie.Items.Add(new RepListItem(code + " - " + hierarchyEntity.HirarchyName, code));
                        rowCnt++;
                    }

                    if (rowCnt > 0)
                        cmbHie.SelectedIndex = 0;
                }
            }

        }


        private void FillCodeControls()
        {
            List<RCsb14GroupEntity> grpCntrls;
            List<SRCsb14GroupEntity> SgrpCntrls;

            string UserAgency = string.Empty;
            if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            {
                HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                if (SelHie != null)
                    UserAgency = "**";
            }

            if (_TabType == "RPerfMeasures")
            {
                grpCntrls = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, null, null, null, BaseForm.UserID, string.Empty);
                //if (grpCntrls.Count > 0)
                //{
                //    grpCntrls = grpCntrls.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                //}

                if (grpCntrls.Count > 0)
                {
                    foreach (RCsb14GroupEntity drg in grpCntrls)
                    {
                        if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && string.IsNullOrWhiteSpace(drg.GrpCode.ToString().Trim()) && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                        {
                            //if (btn_mode == "Copy")
                            //    txtCode.Text = string.Empty;
                            //else
                            txtCode.Text = drg.Code.ToString();
                            txtDesc.Text = drg.GrpDesc.ToString();
                            dtpFrom.Text = drg.OFdate.ToString();
                            dtpTo.Text = drg.OTdate.ToString();
                            SetRepComboBoxValue(cmbHie, drg.Agency.ToString());
                            if (drg.Active.Equals("Y"))
                            {
                                chkbActive.Checked = true;
                            }
                            else
                            {
                                chkbActive.Checked = false;
                            }

                            if (drg.PPR_SW.Equals("Y"))
                            {
                                chkbPPR.Checked = true;
                            }
                            else
                            {
                                chkbPPR.Checked = false;
                            }


                            //txtnum1.Text = drg.Hrd1.ToString();
                            //if (drg.Incld1.ToString() == "True")
                            //    chkb1.Checked = true;
                            //else
                            //    chkb1.Checked = false;
                            //txtnum2.Text = drg.Hrd2.ToString();
                            //if (drg.Incld2.ToString() == "True")
                            //    chkb2.Checked = true;
                            //else
                            //    chkb2.Checked = false;
                            //txtnum3.Text = drg.Hrd3.ToString();
                            //if (drg.Incld3.ToString() == "True")
                            //    chkb3.Checked = true;
                            //else
                            //    chkb3.Checked = false;
                            //txtnum4.Text = drg.Hrd4.ToString();
                            //if (drg.Incld4.ToString() == "True")
                            //    chkb4.Checked = true;
                            //else
                            //    chkb4.Checked = false;
                            //txtnum5.Text = drg.Hrd5.ToString();
                            //if (drg.Incld5.ToString() == "True")
                            //    chkb5.Checked = true;
                            //else
                            //    chkb5.Checked = false;
                            //cmbAchvResCol.Text = drg.ExAchev.ToString();

                            //if (Mode == "Edit")
                            //{
                            //    if (drg.ExAchev.ToString() == "0")
                            //        SetComboBoxValue(cmbAchvResCol, "*");
                            //    else
                            //        SetComboBoxValue(cmbAchvResCol, drg.ExAchev.ToString());
                            //}
                        }
                    }
                }
            }
            else if (_TabType == "RServices")
            {
                SgrpCntrls = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, null, null, null, BaseForm.UserID, string.Empty);

                //if (SgrpCntrls.Count > 0)
                //{
                //    SgrpCntrls = SgrpCntrls.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                //}
                if (SgrpCntrls.Count > 0)
                {
                    foreach (SRCsb14GroupEntity drg in SgrpCntrls)
                    {
                        if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && string.IsNullOrWhiteSpace(drg.GrpCode.ToString().Trim()) && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                        {
                            txtCode.Text = drg.Code.ToString();
                            txtDesc.Text = drg.GrpDesc.ToString();
                            dtpFrom.Text = drg.OFdate.ToString();
                            dtpTo.Text = drg.OTdate.ToString();

                            SetRepComboBoxValue(cmbHie, drg.Agency.ToString());
                            if (drg.Active.Equals("Y"))
                            {
                                chkbActive.Checked = true;
                            }
                            else
                            {
                                chkbActive.Checked = false;
                            }

                            if (drg.PPR_SW.Equals("Y"))
                            {
                                chkbPPR.Checked = true;
                            }
                            else
                            {
                                chkbPPR.Checked = false;
                            }

                        }
                    }
                }
            }


        }

        private void FillGroupControls()
        {
            List<RCsb14GroupEntity> grpCntrls;
            List<SRCsb14GroupEntity> SgrpCntrls;
            string UserAgency = string.Empty;
            if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            {
                HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                if (SelHie != null)
                    UserAgency = "**";
            }

            if (_TabType == "RPerfMeasures")
            {
                grpCntrls = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, null, null, null, BaseForm.UserID, string.Empty);

                //if (grpCntrls.Count > 0)
                //{


                //    grpCntrls = grpCntrls.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                //}

                if (grpCntrls.Count > 0)
                {
                    foreach (RCsb14GroupEntity drg in grpCntrls)
                    {
                        if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && string.IsNullOrWhiteSpace(drg.GrpCode.ToString()) && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                            ReferDesc = drg.GrpDesc.Trim();
                        if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && drg.GrpCode.ToString() == groupCd && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                        {
                            if (btn_mode == "Copy")
                                txtCode.Text = string.Empty;
                            else
                                txtCode.Text = drg.GrpCode.ToString();
                            txtDesc.Text = drg.GrpDesc.ToString();
                            txtnum1.Text = drg.Hrd1.ToString();
                            if (drg.Incld1.ToString() == "True")
                                chkb1.Checked = true;
                            else
                                chkb1.Checked = false;
                            txtnum2.Text = drg.Hrd2.ToString();
                            if (drg.Incld2.ToString() == "True")
                                chkb2.Checked = true;
                            else
                                chkb2.Checked = false;
                            txtnum3.Text = drg.Hrd3.ToString();
                            if (drg.Incld3.ToString() == "True")
                                chkb3.Checked = true;
                            else
                                chkb3.Checked = false;
                            txtnum4.Text = drg.Hrd4.ToString();
                            if (drg.Incld4.ToString() == "True")
                                chkb4.Checked = true;
                            else
                                chkb4.Checked = false;
                            txtnum5.Text = drg.Hrd5.ToString();
                            if (drg.Incld5.ToString() == "True")
                                chkb5.Checked = true;
                            else
                                chkb5.Checked = false;
                            cmbAchvResCol.Text = drg.ExAchev.ToString();

                            if (Mode == "Edit")
                            {
                                if (drg.ExAchev.ToString() == "0")
                                    SetComboBoxValue(cmbAchvResCol, "*");
                                else
                                    SetComboBoxValue(cmbAchvResCol, drg.ExAchev.ToString());

                                if (!string.IsNullOrEmpty(drg.DomainHie.Trim()))
                                    SetRepComboBoxValue(cmbHie, drg.DomainHie.ToString());
                                else
                                    SetRepComboBoxValue(cmbHie, "");
                            }
                        }
                    }
                }
            }
            else if (_TabType == "RServices")
            {
                SgrpCntrls = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, null, null, null, BaseForm.UserID, string.Empty);

                //if (SgrpCntrls.Count > 0)
                //{


                //    SgrpCntrls = SgrpCntrls.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                //}

                if (SgrpCntrls.Count > 0)
                {
                    foreach (SRCsb14GroupEntity drg in SgrpCntrls)
                    {
                        if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && string.IsNullOrWhiteSpace(drg.GrpCode.ToString()) && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                            ReferDesc = drg.GrpDesc.Trim();
                        if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && drg.GrpCode.ToString() == groupCd && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                        {
                            if (btn_mode == "Copy")
                                txtCode.Text = string.Empty;
                            else
                                txtCode.Text = drg.GrpCode.ToString();
                            txtDesc.Text = drg.GrpDesc.ToString();
                            txtnum1.Text = drg.Hrd1.ToString();
                            if (drg.Incld1.ToString() == "True")
                                chkb1.Checked = true;
                            else
                                chkb1.Checked = false;
                            txtnum2.Text = drg.Hrd2.ToString();
                            if (drg.Incld2.ToString() == "True")
                                chkb2.Checked = true;
                            else
                                chkb2.Checked = false;
                            txtnum3.Text = drg.Hrd3.ToString();
                            if (drg.Incld3.ToString() == "True")
                                chkb3.Checked = true;
                            else
                                chkb3.Checked = false;
                            txtnum4.Text = drg.Hrd4.ToString();
                            if (drg.Incld4.ToString() == "True")
                                chkb4.Checked = true;
                            else
                                chkb4.Checked = false;
                            txtnum5.Text = drg.Hrd5.ToString();
                            if (drg.Incld5.ToString() == "True")
                                chkb5.Checked = true;
                            else
                                chkb5.Checked = false;
                            cmbAchvResCol.Text = drg.ExAchev.ToString();

                            if (drg.CSBG_Domain.ToString() == "Y")
                                chkbCSBG.Checked = true;
                            else
                                chkbCSBG.Checked = false;

                            txtBenFResnDomain.Text = drg.BenRsnDomainField.ToString();

                            if (Mode == "Edit")
                            {
                                if (drg.ExAchev.ToString() == "0")
                                    SetComboBoxValue(cmbAchvResCol, "*");
                                else
                                    SetComboBoxValue(cmbAchvResCol, drg.ExAchev.ToString());

                                if (!string.IsNullOrEmpty(drg.DomainHie.Trim()))
                                    SetRepComboBoxValue(cmbHie, drg.DomainHie.ToString());
                                else
                                    SetRepComboBoxValue(cmbHie, "00");
                            }
                        }
                    }
                }
            }


        }

        private void fillTableControls()
        {
            List<RCsb14GroupEntity> TblCntrl;
            List<SRCsb14GroupEntity> STblCntrl;

            string UserAgency = string.Empty;
            if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            {
                HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                if (SelHie != null)
                    UserAgency = "**";
            }
            if (_TabType == "RPerfMeasures")
            {
                TblCntrl = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, null, null, null, BaseForm.UserID, string.Empty);
                //if (TblCntrl.Count > 0)
                //{


                //    TblCntrl = TblCntrl.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                //}

                if (TblCntrl.Count > 0)
                {
                    foreach (RCsb14GroupEntity dr in TblCntrl)
                    {
                        if (dr.Code.ToString() == refFdate && dr.Agency.ToString() == refTdate && string.IsNullOrWhiteSpace(dr.GrpCode.ToString()) && string.IsNullOrWhiteSpace(dr.TblCode.ToString()))
                            ReferDesc = dr.GrpDesc.Trim();

                        if (dr.Code.ToString() == refFdate && dr.Agency.ToString() == refTdate && dr.GrpCode.ToString() == groupCd && dr.TblCode.ToString() == tablecd)
                        {
                            txtCode.Text = dr.TblCode.ToString();
                            txtDesc.Text = dr.GrpDesc.ToString();
                            txtAgeFrm.Text = dr.AFrom.ToString();
                            if (dr.Ato.ToString().Trim() == "0")
                                txtAgeTo.Text = "";
                            else
                                txtAgeTo.Text = dr.Ato.ToString();
                            txtExpAch.Text = dr.ExAchev.ToString();
                            if (dr.IndSwitch.ToString().ToUpper() == "Y")
                                chkIndSwitch.Checked = true;
                            if (dr.CalCost.ToString() == "1")
                                chkbCalcCosts.Checked = true;
                            else
                                chkbCalcCosts.Checked = false;
                            if (dr.UseSer.ToString() == "True")
                                chkbUseSer.Checked = true;
                            else
                                chkbUseSer.Checked = false;
                            if (dr.Duplicate.ToString() == "True")
                                chkbDupl.Checked = true;
                            else
                                chkbDupl.Checked = false;
                            if (dr.Disable.ToString() == "True")
                                chkbDisabled.Checked = true;
                            else
                                chkbDisabled.Checked = false;

                            this.chkbPrograms.CheckedChanged -= new System.EventHandler(this.chkbPrograms_CheckedChanged);
                            if (dr.Prog_Switch.ToString() == "Y")
                            {
                                chkbPrograms.Checked = true;
                                //this.Budget.ReadOnly = true;
                                this.Budget.ReadOnly = false;
                            }
                            else
                            {
                                chkbPrograms.Checked = false;
                                pnlgvwHie.Visible = false;
                                this.pnlGvTable.Size = new System.Drawing.Size(689, 362);
                                this.Budget.ReadOnly = false;
                            }
                            this.chkbPrograms.CheckedChanged += new System.EventHandler(this.chkbPrograms_CheckedChanged);

                            if (cmbCntInd != null && cmbCntInd.Items.Count > 0)
                            {
                                foreach (ListItem list in cmbCntInd.Items)
                                {
                                    if (list.Value.Equals(dr.CntIndic.ToString()) || list.Text.Equals(dr.CntIndic.ToString()))
                                    {
                                        cmbCntInd.SelectedItem = list;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (_TabType == "RServices")
            {
                STblCntrl = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, null, null, null, BaseForm.UserID, string.Empty);

                //if (STblCntrl.Count > 0)
                //{


                //    STblCntrl = STblCntrl.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                //}
                if (STblCntrl.Count > 0)
                {
                    foreach (SRCsb14GroupEntity dr in STblCntrl)
                    {
                        if (dr.Code.ToString() == refFdate && dr.Agency.ToString() == refTdate && string.IsNullOrWhiteSpace(dr.GrpCode.ToString()) && string.IsNullOrWhiteSpace(dr.TblCode.ToString()))
                            ReferDesc = dr.GrpDesc.Trim();
                        if (dr.Code.ToString() == refFdate && dr.Agency.ToString() == refTdate && dr.GrpCode.ToString() == groupCd && dr.TblCode.ToString() == tablecd)
                        {
                            txtCode.Text = dr.TblCode.ToString();
                            txtDesc.Text = dr.GrpDesc.ToString();
                            txtAgeFrm.Text = dr.AFrom.ToString();
                            if (dr.Ato.ToString().Trim() == "0")
                                txtAgeTo.Text = "";
                            else
                                txtAgeTo.Text = dr.Ato.ToString();
                            txtExpAch.Text = dr.ExAchev.ToString();
                            if (dr.CalCost.ToString() == "1")
                                chkbCalcCosts.Checked = true;
                            else
                                chkbCalcCosts.Checked = false;
                            if (dr.UseSer.ToString() == "True")
                                chkbUseSer.Checked = true;
                            else
                                chkbUseSer.Checked = false;
                            if (dr.Duplicate.ToString() == "True")
                                chkbDupl.Checked = true;
                            else
                                chkbDupl.Checked = false;
                            if (dr.Disable.ToString() == "True")
                                chkbDisabled.Checked = true;
                            else
                                chkbDisabled.Checked = false;
                            if (cmbCntInd != null && cmbCntInd.Items.Count > 0)
                            {
                                foreach (ListItem list in cmbCntInd.Items)
                                {
                                    if (list.Value.Equals(dr.CntIndic.ToString()) || list.Text.Equals(dr.CntIndic.ToString()))
                                    {
                                        cmbCntInd.SelectedItem = list;
                                        break;
                                    }
                                }
                            }

                            this.chkbPrograms.CheckedChanged -= new System.EventHandler(this.chkbPrograms_CheckedChanged);
                            if (dr.Prog_Switch.ToString() == "Y")
                            {
                                chkbPrograms.Checked = true;
                                //this.Budget.ReadOnly = true;
                                this.Budget.ReadOnly = false;
                            }
                            else
                            {
                                chkbPrograms.Checked = false;
                                pnlgvwHie.Visible = false;
                                this.pnlGvTable.Size = new System.Drawing.Size(689, 362);
                                this.Budget.ReadOnly = false;
                            }
                            this.chkbPrograms.CheckedChanged += new System.EventHandler(this.chkbPrograms_CheckedChanged);
                        }
                    }
                }
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Performance");
            ////else
            ////    Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Performance");
        }

        private void PerformanceMeasureForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGSave_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpTo, null);
            _errorProvider.SetError(dtpFrom, null);
            bool IsInserted = false;
            if (GridType == "Code")
            {
                try
                {
                    Getmaincode();
                    if (ValidateForm())
                    {
                        if (_TabType == "RPerfMeasures")
                        {
                            if (chkbCopy.Checked.Equals(true) && cmbRefPer.Enabled == true)
                                MessageBox.Show("Are you sure want to copy all Group Definitions for the selected date range?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: SaveForm);
                            else
                            {
                                CaptainModel model = new CaptainModel();
                                RCsb14GroupEntity GrpEntity = new RCsb14GroupEntity();



                                GrpEntity.Agency = ((RepListItem)cmbHie.SelectedItem).Value.ToString();
                                GrpEntity.Code = txtCode.Text;
                                //GrpEntity.GrpCode = txtCode.Text;
                                GrpEntity.GrpDesc = txtDesc.Text;
                                GrpEntity.OFdate = LookupDataAccess.Getdate(dtpFrom.Value.ToShortDateString());
                                GrpEntity.OTdate = LookupDataAccess.Getdate(dtpTo.Value.ToShortDateString());
                                //GrpEntity.Hrd1 = txtnum1.Text.Trim();
                                //GrpEntity.Incld1 = chkb1.Checked ? "1" : "0";
                                //GrpEntity.Hrd2 = txtnum2.Text.Trim();
                                //GrpEntity.Incld2 = chkb2.Checked ? "1" : "0";
                                //GrpEntity.Hrd3 = txtnum3.Text.Trim();
                                //GrpEntity.Incld3 = chkb3.Checked ? "1" : "0";
                                //GrpEntity.Hrd4 = txtnum4.Text.Trim();
                                //GrpEntity.Incld4 = chkb4.Checked ? "1" : "0";
                                //GrpEntity.Hrd5 = txtnum5.Text.Trim();
                                //GrpEntity.Incld5 = chkb5.Checked ? "1" : "0";
                                //if (cmbAchvResCol.Text == "*")
                                //    GrpEntity.ExAchev = "0";
                                //else
                                //    GrpEntity.ExAchev = cmbAchvResCol.Text;
                                string status = chkbActive.Checked ? "Y" : "N";
                                GrpEntity.Active = status;

                                string PPRSw = chkbPPR.Checked ? "Y" : "N";
                                GrpEntity.PPR_SW = PPRSw;

                                GrpEntity.LSTCOperator = BaseForm.UserID;
                                GrpEntity.AddOperator = BaseForm.UserID;
                                GrpEntity.Mode = Mode;

                                if (chkbCopy.Checked == true)
                                {
                                    GrpEntity.ExAchev = ((RepListItem)cmbRefPer.SelectedItem).Value.ToString();
                                    GrpEntity.CopyAgency = ((RepListItem)cmbRefPer.SelectedItem).ID.ToString();
                                    GrpEntity.Mode = "MULTI";
                                }

                                IsInserted = _model.SPAdminData.InsertUpdateRNGGrp(GrpEntity);

                                if (IsInserted && Mode == "Add")
                                {
                                    // AlertBox.Show("Details Inserted Successfully");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    if (GrpEntity.Mode == "MULTI")
                                    {
                                        AlertBox.Show("Moved all records Successfully");
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                    else
                                    {
                                        //AlertBox.Show("Detials Updated Successfully");
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                }

                            }
                        }
                        else if (_TabType == "RServices")
                        {
                            if (chkbCopy.Checked.Equals(true) && cmbRefPer.Enabled == true)
                                MessageBox.Show("Are you sure want to copy all Group Definitions for the selected date range?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: SaveForm);
                            else
                            {
                                CaptainModel model = new CaptainModel();
                                SRCsb14GroupEntity SGrpEntity = new SRCsb14GroupEntity();

                                SGrpEntity.Agency = ((RepListItem)cmbHie.SelectedItem).Value.ToString();
                                SGrpEntity.Code = txtCode.Text;
                                //GrpEntity.GrpCode = txtCode.Text;
                                SGrpEntity.GrpDesc = txtDesc.Text;
                                SGrpEntity.OFdate = LookupDataAccess.Getdate(dtpFrom.Value.ToShortDateString());
                                SGrpEntity.OTdate = LookupDataAccess.Getdate(dtpTo.Value.ToShortDateString());


                                string status = chkbActive.Checked ? "Y" : "N";
                                SGrpEntity.Active = status;

                                string PPRSw = chkbPPR.Checked ? "Y" : "N";
                                SGrpEntity.PPR_SW = PPRSw;

                                SGrpEntity.LSTCOperator = BaseForm.UserID;
                                SGrpEntity.AddOperator = BaseForm.UserID;
                                SGrpEntity.Mode = Mode;
                                if (chkbCopy.Checked == true)
                                {
                                    SGrpEntity.ExAchev = ((RepListItem)cmbRefPer.SelectedItem).Value.ToString();
                                    SGrpEntity.Mode = "MULTI";
                                }
                                IsInserted = _model.SPAdminData.InsertUpdateRNGSRGrp(SGrpEntity);

                                if (IsInserted && Mode == "Add")
                                {
                                    //AlertBox.Show("Details Inserted Successfully");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    if (SGrpEntity.Mode == "MULTI")
                                    {
                                        AlertBox.Show("Moved all records Successfully");
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                    else
                                    {
                                        //AlertBox.Show("Updated Successfully");
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                }

                            }
                        }
                        //this.Close();

                        Mode = "View";
                        //CriticalActivity CsbControl = BaseForm.GetBaseUserControl() as CriticalActivity;
                        //if (CsbControl != null)
                        //{
                        //    CsbControl.RefreshGroupGrid();
                        //}
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else if (GridType == "Group")
            {
                try
                {
                    if (_TabType == "RPerfMeasures")
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            RCsb14GroupEntity GrpEntity = new RCsb14GroupEntity();

                            GrpEntity.Code = refFdate;
                            GrpEntity.Agency = refTdate;
                            GrpEntity.GrpCode = txtCode.Text;
                            GrpEntity.GrpDesc = txtDesc.Text;
                            GrpEntity.Hrd1 = txtnum1.Text.Trim();
                            GrpEntity.Incld1 = chkb1.Checked ? "1" : "0";
                            GrpEntity.Hrd2 = txtnum2.Text.Trim();
                            GrpEntity.Incld2 = chkb2.Checked ? "1" : "0";
                            GrpEntity.Hrd3 = txtnum3.Text.Trim();
                            GrpEntity.Incld3 = chkb3.Checked ? "1" : "0";
                            GrpEntity.Hrd4 = txtnum4.Text.Trim();
                            GrpEntity.Incld4 = chkb4.Checked ? "1" : "0";
                            GrpEntity.Hrd5 = txtnum5.Text.Trim();
                            GrpEntity.Incld5 = chkb5.Checked ? "1" : "0";
                            if (cmbAchvResCol.Text == "*")
                                GrpEntity.ExAchev = "0";
                            else
                                GrpEntity.ExAchev = cmbAchvResCol.Text;

                            if (!string.IsNullOrEmpty(((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim()))
                                GrpEntity.DomainHie = ((RepListItem)cmbHie.SelectedItem).Value.ToString();

                            GrpEntity.LSTCOperator = BaseForm.UserID;
                            GrpEntity.AddOperator = BaseForm.UserID;
                            GrpEntity.Mode = Mode;

                            IsInserted = _model.SPAdminData.InsertUpdateRNGGrp(GrpEntity);



                            //this.Close();

                            //CriticalActivity CsbControl = BaseForm.GetBaseUserControl() as CriticalActivity;
                            //if (CsbControl != null)
                            //{
                            //    CsbControl.RefreshGroupGrid();
                            //}

                            if (IsInserted && Mode == "Add")
                            {
                                //AlertBox.Show("Group Details Inserted Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                //AlertBox.Show("Group Details Updated Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            Mode = "View";
                        }
                    }
                    else if (_TabType == "RServices")
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            SRCsb14GroupEntity SGrpEntity = new SRCsb14GroupEntity();

                            SGrpEntity.Code = refFdate;
                            SGrpEntity.Agency = refTdate;
                            SGrpEntity.GrpCode = txtCode.Text;
                            SGrpEntity.GrpDesc = txtDesc.Text;
                            SGrpEntity.Hrd1 = txtnum1.Text.Trim();
                            SGrpEntity.Incld1 = chkb1.Checked ? "1" : "0";
                            SGrpEntity.Hrd2 = txtnum2.Text.Trim();
                            SGrpEntity.Incld2 = chkb2.Checked ? "1" : "0";
                            SGrpEntity.Hrd3 = txtnum3.Text.Trim();
                            SGrpEntity.Incld3 = chkb3.Checked ? "1" : "0";
                            SGrpEntity.Hrd4 = txtnum4.Text.Trim();
                            SGrpEntity.Incld4 = chkb4.Checked ? "1" : "0";
                            SGrpEntity.Hrd5 = txtnum5.Text.Trim();
                            SGrpEntity.Incld5 = chkb5.Checked ? "1" : "0";
                            if (cmbAchvResCol.Text == "*")
                                SGrpEntity.ExAchev = "0";
                            else
                                SGrpEntity.ExAchev = cmbAchvResCol.Text;
                            SGrpEntity.LSTCOperator = BaseForm.UserID;
                            SGrpEntity.AddOperator = BaseForm.UserID;

                            if (!string.IsNullOrEmpty(((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim()))
                                SGrpEntity.DomainHie = ((RepListItem)cmbHie.SelectedItem).Value.ToString();

                            SGrpEntity.CSBG_Domain = chkbCSBG.Checked ? "Y" : "N"; //Added by Vikash on 01/21/2025 new parameter

                            SGrpEntity.BenRsnDomainField = txtBenFResnDomain.Text.Trim();

                            SGrpEntity.Mode = Mode;

                            IsInserted = _model.SPAdminData.InsertUpdateRNGSRGrp(SGrpEntity);

                            //this.Close();

                            //CriticalActivity CsbControl = BaseForm.GetBaseUserControl() as CriticalActivity;
                            //if (CsbControl != null)
                            //{
                            //    CsbControl.RefreshGroupGrid();
                            //}

                            if (IsInserted && Mode == "Add")
                            {
                                // AlertBox.Show("Group Details Inserted Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                // AlertBox.Show("Group Updated Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            Mode = "View";
                        }
                    }


                }
                catch (Exception ex)
                {

                }
            }

            else if (GridType == "Table")
            {

                try
                {
                    if (_TabType == "RPerfMeasures")
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            RCsb14GroupEntity TblEntity = new RCsb14GroupEntity();

                            TblEntity.Code = refFdate;
                            TblEntity.Agency = refTdate;
                            TblEntity.GrpCode = groupCd;
                            TblEntity.GrpDesc = txtDesc.Text;
                            TblEntity.TblCode = txtCode.Text;
                            int Budget = 0;
                            if (gvGoals.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow dr in gvGoals.Rows)
                                {
                                    if (dr.Cells["Del"].Value.ToString() == "True")
                                    {
                                        if (!string.IsNullOrEmpty(dr.Cells["Budget"].Value.ToString().Trim()))
                                            Budget += int.Parse(dr.Cells["Budget"].Value.ToString());
                                    }
                                }
                            }
                            txtExpAch.Text = Budget.ToString();
                            TblEntity.ExAchev = txtExpAch.Text;
                            TblEntity.CntIndic = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                            if (chkbCalcCosts.Checked)
                                TblEntity.CalCost = "1";
                            else
                                TblEntity.CalCost = "0";
                            TblEntity.UseSer = chkbUseSer.Checked ? "1" : "0";
                            TblEntity.Duplicate = chkbDupl.Checked ? "1" : "0";
                            TblEntity.Disable = chkbDisabled.Checked ? "1" : "0";
                            TblEntity.AFrom = txtAgeFrm.Text;
                            TblEntity.Ato = txtAgeTo.Text;
                            TblEntity.IndSwitch = chkIndSwitch.Checked == true ? "Y" : "N";
                            TblEntity.Prog_Switch = chkbPrograms.Checked == true ? "Y" : "N";
                            TblEntity.LSTCOperator = BaseForm.UserID;
                            TblEntity.AddOperator = BaseForm.UserID;
                            TblEntity.Mode = Mode;
                            if (_model.SPAdminData.InsertUpdateRNGGrp(TblEntity))
                            {
                                if (gvGoals.Rows.Count > 0)
                                {
                                    RNGGAEntity GoalsEntity = new RNGGAEntity();
                                    GoalsEntity.Code = refFdate.ToString();
                                    GoalsEntity.Agency = refTdate;
                                    GoalsEntity.TblCode = txtCode.Text;
                                    GoalsEntity.GrpCode = groupCd.ToString();
                                    GoalsEntity.Mode = "Edit";
                                    GoalsEntity.LSTCOperator = BaseForm.UserID;
                                    string Tmp = "false";
                                    foreach (DataGridViewRow dr in gvGoals.Rows)
                                    {
                                        Tmp = dr.Cells["Del"].Value.ToString();
                                        if (dr.Cells["Del"].Value.ToString() == "True")
                                        {
                                            GoalsEntity.GoalCode = dr.Cells["Agy_Code"].Value.ToString();
                                            GoalsEntity.SerSP = dr.Cells["SRSP_Code"].Value.ToString();
                                            GoalsEntity.Desc = dr.Cells["Goals"].Value.ToString();
                                            GoalsEntity.Budget = dr.Cells["Budget"].Value.ToString();
                                            GoalsEntity.Sequence = dr.Cells["gvGSeq"].Value.ToString();
                                            _model.SPAdminData.InsertUpdateRNGGA(GoalsEntity);

                                            if (GoalHieEntity.Count > 0)
                                            {
                                                List<RNGGoalHEntity> SelGoalsHies = new List<RNGGoalHEntity>();
                                                SelGoalsHies = GoalHieEntity.FindAll(u => u.RNGGAH_CODE.Trim() == refFdate.ToString().Trim() && u.RNGGAH_AGENCY == refTdate.ToString() && u.RNGGAH_GRP_CODE.Trim() == groupCd.Trim() && u.RNGGAH_TBL_CODE.Trim() == txtCode.Text.Trim() && u.RNGGAH_GOAL_CODE.Trim() == GoalsEntity.GoalCode.Trim());
                                                if (SelGoalsHies.Count > 0)
                                                {
                                                    RNGGoalHEntity DEntity = new RNGGoalHEntity();
                                                    DEntity.RNGGAH_CODE = refFdate.ToString();
                                                    DEntity.RNGGAH_AGENCY = refTdate;
                                                    DEntity.RNGGAH_TBL_CODE = txtCode.Text;
                                                    DEntity.RNGGAH_GRP_CODE = groupCd.ToString();
                                                    DEntity.RNGGAH_GOAL_CODE = SelGoalsHies[0].RNGGAH_GOAL_CODE;
                                                    DEntity.Mode = "Delete";
                                                    DEntity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;
                                                    _model.SPAdminData.InsertUpdateRNGGAHIE(DEntity);

                                                    foreach (RNGGoalHEntity GHEntity in SelGoalsHies)
                                                    {
                                                        GHEntity.Mode = "Add";
                                                        GHEntity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;

                                                        _model.SPAdminData.InsertUpdateRNGGAHIE(GHEntity);
                                                    }
                                                }
                                            }

                                        }
                                        else if (dr.Cells["Del"].Value.ToString() == "False")
                                        {
                                            Captain.DatabaseLayer.SPAdminDB.DeleteRNGGA(refFdate, refTdate, groupCd, txtCode.Text, dr.Cells["Agy_Code"].Value.ToString(), string.Empty);

                                            if (PPR_SW != "Y")
                                            {
                                                RNGGoalHEntity DEntity = new RNGGoalHEntity();
                                                DEntity.RNGGAH_CODE = refFdate.ToString();
                                                DEntity.RNGGAH_AGENCY = refTdate;
                                                DEntity.RNGGAH_TBL_CODE = txtCode.Text;
                                                DEntity.RNGGAH_GRP_CODE = groupCd.ToString();
                                                DEntity.RNGGAH_GOAL_CODE = dr.Cells["Agy_Code"].Value.ToString();
                                                DEntity.Mode = "Delete";
                                                DEntity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;
                                                _model.SPAdminData.InsertUpdateRNGGAHIE(DEntity);
                                            }
                                        }

                                    }
                                }
                            }
                            //this.Close();
                            if (Mode == "Add")
                            {
                                AlertBox.Show("Group Details Inserted Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                AlertBox.Show("Group Details Updated Successfully");
                                //this.DialogResult = DialogResult.OK;
                                //this.Close();
                                fillGvGoals();
                            }
                            //Mode = "View";
                        }
                    }
                    else if (_TabType == "RServices")
                    {
                        if (ValidateForm())
                        {
                            CaptainModel model = new CaptainModel();
                            SRCsb14GroupEntity TblEntity = new SRCsb14GroupEntity();

                            TblEntity.Code = refFdate;
                            TblEntity.Agency = refTdate;
                            TblEntity.GrpCode = groupCd;
                            TblEntity.GrpDesc = txtDesc.Text;
                            TblEntity.TblCode = txtCode.Text;

                            int Budget = 0;
                            if (gvGoals.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow dr in gvGoals.Rows)
                                {
                                    if (dr.Cells["Del"].Value.ToString() == "True")
                                    {
                                        if (!string.IsNullOrEmpty(dr.Cells["Budget"].Value.ToString().Trim()))
                                            Budget += int.Parse(dr.Cells["Budget"].Value.ToString());
                                    }
                                }
                            }
                            txtExpAch.Text = Budget.ToString();
                            TblEntity.ExAchev = txtExpAch.Text;
                            TblEntity.CntIndic = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                            if (chkbCalcCosts.Checked)
                                TblEntity.CalCost = "1";
                            else
                                TblEntity.CalCost = "0";
                            TblEntity.UseSer = chkbUseSer.Checked ? "1" : "0";
                            TblEntity.Duplicate = chkbDupl.Checked ? "1" : "0";
                            TblEntity.Disable = chkbDisabled.Checked ? "1" : "0";
                            TblEntity.AFrom = txtAgeFrm.Text;
                            TblEntity.Ato = txtAgeTo.Text;
                            TblEntity.LSTCOperator = BaseForm.UserID;
                            TblEntity.AddOperator = BaseForm.UserID;
                            TblEntity.Prog_Switch = chkbPrograms.Checked == true ? "Y" : "N";
                            TblEntity.Mode = Mode;
                            if (_model.SPAdminData.InsertUpdateRNGSRGrp(TblEntity))
                            {
                                if (gvGoals.Rows.Count > 0)
                                {
                                    RNGSRGAEntity GoalsEntity = new RNGSRGAEntity();
                                    GoalsEntity.Code = refFdate.ToString();
                                    GoalsEntity.Agency = refTdate;
                                    GoalsEntity.TblCode = txtCode.Text;
                                    GoalsEntity.GrpCode = groupCd.ToString();
                                    GoalsEntity.Mode = "Edit";
                                    GoalsEntity.LSTCOperator = BaseForm.UserID;
                                    string Tmp = "false";
                                    foreach (DataGridViewRow dr in gvGoals.Rows)
                                    {
                                        Tmp = dr.Cells["Del"].Value.ToString();
                                        if (dr.Cells["Del"].Value.ToString() == "True")
                                        {
                                            GoalsEntity.GoalCode = dr.Cells["Agy_Code"].Value.ToString();
                                            GoalsEntity.SerSP = dr.Cells["SRSP_Code"].Value.ToString();
                                            GoalsEntity.Desc = dr.Cells["Goals"].Value.ToString();
                                            GoalsEntity.Sequence = dr.Cells["gvGSeq"].Value.ToString();
                                            GoalsEntity.Budget = dr.Cells["Budget"].Value.ToString();
                                            _model.SPAdminData.InsertUpdateRNGSRGA(GoalsEntity);

                                            if (SRGoalHieEntity.Count > 0)
                                            {
                                                List<RNGSRGoalHEntity> SelSRGoalsHies = new List<RNGSRGoalHEntity>();
                                                SelSRGoalsHies = SRGoalHieEntity.FindAll(u => u.RNGSRGAH_CODE.Trim() == refFdate.ToString().Trim() && u.RNGSRGAH_AGENCY == refTdate.ToString() && u.RNGSRGAH_GRP_CODE.Trim() == groupCd.Trim() && u.RNGSRGAH_TBL_CODE.Trim() == txtCode.Text.Trim() && u.RNGSRGAH_GOAL_CODE.Trim() == GoalsEntity.GoalCode.Trim());
                                                if (SelSRGoalsHies.Count > 0)
                                                {
                                                    RNGSRGoalHEntity DEntity = new RNGSRGoalHEntity();
                                                    DEntity.RNGSRGAH_CODE = refFdate.ToString();
                                                    DEntity.RNGSRGAH_AGENCY = refTdate;
                                                    DEntity.RNGSRGAH_TBL_CODE = txtCode.Text;
                                                    DEntity.RNGSRGAH_GRP_CODE = groupCd.ToString();
                                                    DEntity.RNGSRGAH_GOAL_CODE = SelSRGoalsHies[0].RNGSRGAH_GOAL_CODE;
                                                    DEntity.Mode = "Delete";
                                                    DEntity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;
                                                    _model.SPAdminData.InsertUpdateRNGSRGAHIE(DEntity);

                                                    foreach (RNGSRGoalHEntity GHEntity in SelSRGoalsHies)
                                                    {
                                                        GHEntity.Mode = "Add";
                                                        GHEntity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;

                                                        _model.SPAdminData.InsertUpdateRNGSRGAHIE(GHEntity);
                                                    }
                                                }
                                            }

                                        }
                                        else if (dr.Cells["Del"].Value.ToString() == "False")
                                        {
                                            Captain.DatabaseLayer.SPAdminDB.DeleteRNGSRGA(refFdate, refTdate, groupCd, txtCode.Text, dr.Cells["Agy_Code"].Value.ToString(), dr.Cells["SRSP_Code"].Value.ToString());

                                            if (PPR_SW != "Y")
                                            {
                                                RNGSRGoalHEntity DEntity = new RNGSRGoalHEntity();
                                                DEntity.RNGSRGAH_CODE = refFdate.ToString();
                                                DEntity.RNGSRGAH_AGENCY = refTdate;
                                                DEntity.RNGSRGAH_TBL_CODE = txtCode.Text;
                                                DEntity.RNGSRGAH_GRP_CODE = groupCd.ToString();
                                                DEntity.RNGSRGAH_GOAL_CODE = dr.Cells["Agy_Code"].Value.ToString();
                                                DEntity.Mode = "Delete";
                                                DEntity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;
                                                _model.SPAdminData.InsertUpdateRNGSRGAHIE(DEntity);
                                            }
                                        }

                                    }
                                }
                            }

                            //this.Close();
                            if (Mode == "Add")
                            {
                                AlertBox.Show("Group Details Inserted Successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                AlertBox.Show("Group Details Updated Successfully");
                                //this.DialogResult = DialogResult.OK;
                                //this.Close();
                                fillGvGoals();
                            }
                            Mode = "View";
                        }
                    }



                    //CriticalActivity CsbControl = BaseForm.GetBaseUserControl() as CriticalActivity;
                    //if (CsbControl != null)
                    //{
                    //    CsbControl.RefreshTableGrid();
                    //}
                }


                catch (Exception ex)
                {
                }

            }
        }

        public bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtCode.Text) || (string.IsNullOrWhiteSpace(txtCode.Text)))
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
                //else if (GridType == "Group")
                //{
                //    //if (txtCode.Text.Length > 9)
                //    //{
                //    //    //if (int.Parse(txtCode.Text.Trim()) < 2147483647)
                //    //        //MessageBox.Show("Group Code was exceeds integer limit", "Capsystems");
                //    //    if(Enumerable.Range(1,2147483647).Contains(int.Parse(txtCode.Text.Trim())))
                //    //        MessageBox.Show("Group Code was exceeds integer limit", "CAPTAIN");
                //    //}
                //    //else if (long.Parse(txtCode.Text.Trim()) < 1)
                //    //{
                //    //    _errorProvider.SetError(txtCode, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                //    //    isValid = false;
                //    //}
                //}
                else
                {
                    _errorProvider.SetError(txtCode, null);
                }
            }

            if (string.IsNullOrEmpty(txtDesc.Text) || string.IsNullOrWhiteSpace(txtDesc.Text))
            {
                _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtDesc, null);
            }

            if (GridType == "Code")
            {
                if (_TabType != "RServices")
                {
                    //if (dtpFrom.Checked.Equals(true) || dtpTo.Checked.Equals(true))
                    //{
                    //    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGGRP(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null);
                    //    DataTable dt = ds.Tables[0];

                    //    if (Mode == "Edit")
                    //    {
                    //        DataView dv = new DataView(dt);
                    //        dv.RowFilter = "RNGGRP_CODE<>'" + refFdate + "'";
                    //        dt = dv.ToTable();
                    //    }

                    //    if (dtpFrom.Checked.Equals(true))
                    //    {
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            foreach (DataRow dr in dt.Rows)
                    //            {
                    //                if ((Convert.ToDateTime(dr["RNGGRP_FDATE"].ToString().Trim()) <= Convert.ToDateTime(dtpFrom.Value)) && (Convert.ToDateTime(dtpFrom.Value) <= Convert.ToDateTime(dr["RNGGRP_TDATE"].ToString().Trim())))
                    //                {
                    //                    _errorProvider.SetError(dtpFrom, "Date Already Exists ".Replace(Consts.Common.Colon, string.Empty));
                    //                    isValid = false;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    if (dtpTo.Checked.Equals(true))
                    //    {
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            foreach (DataRow dr in dt.Rows)
                    //            {
                    //                if ((Convert.ToDateTime(dr["RNGGRP_FDATE"].ToString().Trim()) <= Convert.ToDateTime(dtpTo.Value)) && (Convert.ToDateTime(dtpTo.Value) <= Convert.ToDateTime(dr["RNGGRP_TDATE"].ToString().Trim())))
                    //                {
                    //                    _errorProvider.SetError(dtpFrom, "Date Already Exists ".Replace(Consts.Common.Colon, string.Empty));
                    //                    isValid = false;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    //if (dtpFrom.Checked.Equals(true) || dtpTo.Checked.Equals(true))
                    //{
                    //    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGSRGRP(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null);
                    //    DataTable dt = ds.Tables[0];

                    //    if (Mode == "Edit")
                    //    {
                    //        DataView dv = new DataView(dt);
                    //        dv.RowFilter = "RNGSRGRP_CODE<>'" + refFdate + "'";
                    //        dt = dv.ToTable();
                    //    }

                    //    if (dtpFrom.Checked.Equals(true))
                    //    {
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            foreach (DataRow dr in dt.Rows)
                    //            {
                    //                if ((Convert.ToDateTime(dr["RNGSRGRP_FDATE"].ToString().Trim()) <= Convert.ToDateTime(dtpFrom.Value)) && (Convert.ToDateTime(dtpFrom.Value) <= Convert.ToDateTime(dr["RNGSRGRP_TDATE"].ToString().Trim())))
                    //                {
                    //                    _errorProvider.SetError(dtpFrom, "Date Already Exists ".Replace(Consts.Common.Colon, string.Empty));
                    //                    isValid = false;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    if (dtpTo.Checked.Equals(true))
                    //    {
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            foreach (DataRow dr in dt.Rows)
                    //            {
                    //                if ((Convert.ToDateTime(dr["RNGSRGRP_FDATE"].ToString().Trim()) <= Convert.ToDateTime(dtpTo.Value)) && (Convert.ToDateTime(dtpTo.Value) <= Convert.ToDateTime(dr["RNGSRGRP_TDATE"].ToString().Trim())))
                    //                {
                    //                    _errorProvider.SetError(dtpFrom, "Date Already Exists ".Replace(Consts.Common.Colon, string.Empty));
                    //                    isValid = false;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }

                if (dtpFrom.Checked.Equals(true) && dtpTo.Checked.Equals(true))
                {
                    if (string.IsNullOrWhiteSpace(dtpFrom.Text))
                    {
                        _errorProvider.SetError(dtpFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpFrom, null);
                    }
                    if (string.IsNullOrWhiteSpace(dtpTo.Text))
                    {
                        _errorProvider.SetError(dtpTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpTo, null);
                    }
                }

                if (dtpFrom.Checked.Equals(true) && dtpTo.Checked.Equals(false))
                {
                    if (!string.IsNullOrEmpty(dtpTo.Text))
                    {
                        _errorProvider.SetError(dtpTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpTo, null);
                    }
                }

                if (dtpFrom.Checked.Equals(false) && dtpTo.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(dtpFrom.Text))
                    {
                        _errorProvider.SetError(dtpFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpFrom, null);
                    }
                }

                if (dtpFrom.Checked.Equals(true) && dtpTo.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(dtpFrom.Text) && (!string.IsNullOrEmpty(dtpTo.Text)))
                    {
                        if (Convert.ToDateTime(dtpFrom.Text) > Convert.ToDateTime(dtpTo.Text))
                        {
                            _errorProvider.SetError(dtpTo, "End Date should be greater than or equal to Start Date ".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(dtpTo, null);
                        }
                    }
                }
            }

            if (GridType == "Group" && _TabType != "RServices")
            {
                if ((string.IsNullOrEmpty(txtnum1.Text) || string.IsNullOrWhiteSpace(txtnum1.Text)) && (string.IsNullOrEmpty(txtnum2.Text) || string.IsNullOrWhiteSpace(txtnum2.Text)) && (string.IsNullOrEmpty(txtnum3.Text) || string.IsNullOrWhiteSpace(txtnum3.Text)) && (string.IsNullOrEmpty(txtnum4.Text) || string.IsNullOrWhiteSpace(txtnum4.Text)) && (string.IsNullOrEmpty(txtnum5.Text) || string.IsNullOrWhiteSpace(txtnum5.Text)))
                {
                    _errorProvider.SetError(txtnum1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Please Fill All Textboxes " + lblResHead.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtnum1, null);
                }

                switch (((ListItem)cmbAchvResCol.SelectedItem).Value.ToString())
                {
                    case "1":
                        if ((string.IsNullOrEmpty(txtnum1.Text) || string.IsNullOrWhiteSpace(txtnum1.Text)) || !chkb1.Checked)
                        {
                            _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 1".Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblnum1.Text.Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum2, null);
                            _errorProvider.SetError(txtnum3, null);
                            _errorProvider.SetError(txtnum4, null);
                            _errorProvider.SetError(txtnum5, null);
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(cmbAchvResCol, null);
                            _errorProvider.SetError(txtnum1, null);
                        }
                        break;
                    case "2":
                        if ((string.IsNullOrEmpty(txtnum2.Text) || string.IsNullOrWhiteSpace(txtnum2.Text)) || !chkb2.Checked)
                        {
                            _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 2".Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblnum2.Text.Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum1, null);
                            _errorProvider.SetError(txtnum3, null);
                            _errorProvider.SetError(txtnum4, null);
                            _errorProvider.SetError(txtnum5, null);
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(cmbAchvResCol, null);
                            _errorProvider.SetError(txtnum2, null);
                        }
                        break;
                    case "3":
                        if ((string.IsNullOrEmpty(txtnum3.Text) || string.IsNullOrWhiteSpace(txtnum3.Text)) || !chkb3.Checked)
                        {
                            _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 3".Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblnum3.Text.Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum2, null);
                            _errorProvider.SetError(txtnum1, null);
                            _errorProvider.SetError(txtnum4, null);
                            _errorProvider.SetError(txtnum5, null);
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(cmbAchvResCol, null);
                            _errorProvider.SetError(txtnum3, null);
                        }
                        break;
                    case "4":
                        if ((string.IsNullOrEmpty(txtnum4.Text) || string.IsNullOrWhiteSpace(txtnum4.Text)) || !chkb4.Checked)
                        {
                            _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 4".Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblnum4.Text.Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum2, null);
                            _errorProvider.SetError(txtnum3, null);
                            _errorProvider.SetError(txtnum1, null);
                            _errorProvider.SetError(txtnum5, null);
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(cmbAchvResCol, null);
                            _errorProvider.SetError(txtnum4, null);
                        }
                        break;
                    case "5":
                        if ((string.IsNullOrEmpty(txtnum5.Text) || string.IsNullOrWhiteSpace(txtnum5.Text)) || !chkb5.Checked)
                        {
                            _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 5".Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum5, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblnum5.Text.Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum2, null);
                            _errorProvider.SetError(txtnum3, null);
                            _errorProvider.SetError(txtnum4, null);
                            _errorProvider.SetError(txtnum1, null);
                            isValid = false;
                        }
                        else
                        {
                            _errorProvider.SetError(cmbAchvResCol, null);
                            _errorProvider.SetError(txtnum5, null);
                        }
                        break;
                    case "All":
                        if ((string.IsNullOrWhiteSpace(txtnum1.Text) || !chkb1.Checked) || (string.IsNullOrWhiteSpace(txtnum2.Text) || !chkb2.Checked) || (string.IsNullOrWhiteSpace(txtnum3.Text) || !chkb3.Checked) || (string.IsNullOrWhiteSpace(txtnum4.Text) || !chkb4.Checked) || (string.IsNullOrWhiteSpace(txtnum5.Text) || !chkb5.Checked))
                        {
                            _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " All ".Replace(Consts.Common.Colon, string.Empty)));
                            _errorProvider.SetError(txtnum2, null);
                            _errorProvider.SetError(txtnum3, null);
                            _errorProvider.SetError(txtnum4, null);
                            _errorProvider.SetError(txtnum5, null);
                            isValid = false;
                        }
                        else
                            _errorProvider.SetError(cmbAchvResCol, null);
                        break;
                }
                if (chkb1.Checked)
                {
                    if (string.IsNullOrEmpty(txtnum1.Text))
                    {
                        _errorProvider.SetError(txtnum1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Text Box " + txtnum1.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtnum1, null);
                    }
                }
                //else
                //{
                //    _errorProvider.SetError(txtnum1, null);
                //}
                if (chkb2.Checked)
                {
                    if ((string.IsNullOrEmpty(txtnum2.Text)))
                    {
                        _errorProvider.SetError(txtnum2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), " Text box " + txtnum2.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtnum2, null);
                    }
                }
                //else
                //{
                //    _errorProvider.SetError(txtnum2, null);
                //}
                if (chkb3.Checked)
                {
                    if ((string.IsNullOrEmpty(txtnum3.Text)))
                    {
                        _errorProvider.SetError(txtnum3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Text box " + txtnum3.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtnum3, null);
                    }
                }
                //else
                //{
                //    _errorProvider.SetError(txtnum3, null);
                //}
                if (chkb4.Checked)
                {
                    if ((string.IsNullOrEmpty(txtnum4.Text)))
                    {
                        _errorProvider.SetError(txtnum4, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Text box " + txtnum4.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtnum4, null);
                    }
                }
                //else
                //{
                //    _errorProvider.SetError(txtnum4, null);
                //}
                if (chkb5.Checked)
                {
                    if ((string.IsNullOrEmpty(txtnum5.Text)))
                    {
                        _errorProvider.SetError(txtnum5, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Text box " + txtnum5.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtnum5, null);
                    }
                }
                //else
                //{
                //    _errorProvider.SetError(txtnum5, null);
                //}
            }

            if (GridType == "Table")
            {
                if ((!string.IsNullOrEmpty(txtAgeFrm.Text) && string.IsNullOrEmpty(txtAgeTo.Text)) || (string.IsNullOrEmpty(txtAgeFrm.Text) && !string.IsNullOrEmpty(txtAgeTo.Text)))
                {
                    _errorProvider.SetError(txtAgeTo, string.Format(" From value or To value is required".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                if ((!string.IsNullOrEmpty(txtAgeFrm.Text)) && (!string.IsNullOrEmpty(txtAgeTo.Text)))
                {
                    if (int.Parse(txtAgeTo.Text) <= int.Parse(txtAgeFrm.Text))
                    {
                        _errorProvider.SetError(txtAgeTo, string.Format(lblAgeFrm.Text + " Should not be grteater than or equal to Age" + lblAgeTo.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtAgeTo, null);
                    }
                }
                //if (!string.IsNullOrEmpty(txtAgeFrm.Text))
                //{
                //    if (int.Parse(txtAgeFrm.Text.Trim()) < 0)
                //    {
                //        _errorProvider.SetError(txtAgeFrm, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                //        isValid = false;
                //    }
                //    else
                //        _errorProvider.SetError(txtAgeFrm, null);
                //}
                //if (!string.IsNullOrEmpty(txtAgeTo.Text))
                //{
                //    if (int.Parse(txtAgeTo.Text.Trim()) < 0)
                //    {
                //        _errorProvider.SetError(txtAgeTo, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                //        isValid = false;
                //    }
                //    else
                //        _errorProvider.SetError(txtAgeTo, null);
                //}
            }

            if (GridType == "Group")
            {
                if (chkbCSBG.Checked)
                {
                    if (txtBenFResnDomain.Text == string.Empty)
                    {
                        _errorProvider.SetError(txtBenFResnDomain, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBenfResnDomain.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtBenFResnDomain, null);
                }
                else
                    _errorProvider.SetError(txtBenFResnDomain, null);
            }
            else
                _errorProvider.SetError(txtBenFResnDomain, null);

            IsSaveValid = isValid;

            return (isValid);
        }

        public void Getmaincode()
        {
            if (Mode.Equals("Add"))
            {
                if (GridType == "Code" && _TabType == "RPerfMeasures")
                {
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGGRP(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null, BaseForm.UserID, ((RepListItem)cmbHie.SelectedItem).Value.ToString());
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        int maxLevel = Convert.ToInt32(dt.Compute("max([RNGGRP_CODE])", string.Empty));
                        txtCode.Text = (maxLevel + 1).ToString();
                    }
                    else txtCode.Text = "1";
                }
                else if (GridType == "Code" && _TabType == "RServices")
                {
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGSRGRP(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null, BaseForm.UserID, ((RepListItem)cmbHie.SelectedItem).Value.ToString());
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        int maxLevel = Convert.ToInt32(dt.Compute("max([RNGSRGRP_CODE])", string.Empty));
                        txtCode.Text = (maxLevel + 1).ToString();
                    }
                    else txtCode.Text = "1";
                }
            }
        }

        private bool isCodeExists(string Code)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                if (GridType == "Code" && _TabType == "RPerfMeasures")
                {
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGGRP(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null, string.Empty, string.Empty);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //int maxLevel = Convert.ToInt32(dt.Compute("max([RNGGRP_CODE])", string.Empty));
                        //txtCode.Text = (maxLevel + 1).ToString();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (string.IsNullOrWhiteSpace(dr["RNGgrp_group_code"].ToString().Trim()) && string.IsNullOrWhiteSpace(dr["RNGgrp_Table_code"].ToString().Trim()))
                            {
                                if (dr["RNGGRP_CODE"].ToString().Trim().TrimStart('0') == Code.ToString().Trim().TrimStart('0'))
                                {
                                    isExists = true;
                                }
                            }
                        }

                    }
                    //else txtCode.Text = "1";
                }
                else if (GridType == "Code" && _TabType == "RServices")
                {
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGSRGRP(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (string.IsNullOrWhiteSpace(dr["RNGSRgrp_group_code"].ToString().Trim()) && string.IsNullOrWhiteSpace(dr["RNGSRgrp_Table_code"].ToString().Trim()))
                            {
                                if (dr["RNGSRGRP_CODE"].ToString().Trim().TrimStart('0') == Code.ToString().Trim().TrimStart('0'))
                                {
                                    isExists = true;
                                }
                            }
                        }

                    }
                }
                else if (GridType == "Group" && _TabType == "RPerfMeasures")
                {
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGGRP(refFdate.Trim(), refTdate.Trim(), null, null, null, BaseForm.UserID, refTdate);//BaseForm.BaseAdminAgency
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["RNGGRP_group_code"].ToString().Trim().TrimStart('0') == Code.ToString().Trim().TrimStart('0'))
                            {
                                isExists = true;
                            }
                        }

                    }
                }
                else if (GridType == "Group" && _TabType == "RServices")
                {
                    DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_RNGSRGRP(refFdate.Trim(), refTdate.Trim(), null, null, null, BaseForm.UserID, refTdate);//BaseForm.BaseAdminAgency
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["RNGSRGRP_group_code"].ToString().Trim().TrimStart('0') == Code.ToString().Trim().TrimStart('0'))
                            {
                                isExists = true;
                            }
                        }

                    }
                }
                else if (GridType == "Table" && _TabType == "RPerfMeasures")
                {
                    DataSet dsTb = Captain.DatabaseLayer.SPAdminDB.Get_RNGGRP(refFdate, refTdate, groupCd, null, null, BaseForm.UserID, refTdate);//BaseForm.BaseAdminAgency
                    DataTable dtTb = dsTb.Tables[0];
                    if (dtTb.Rows.Count > 0)
                    {
                        foreach (DataRow drTb in dtTb.Rows)
                        {
                            if (drTb["RNGGRP_table_code"].ToString().Trim() == Code.ToString().Trim())
                                isExists = true;
                        }
                    }
                }
                else if (GridType == "Table" && _TabType == "RServices")
                {
                    DataSet dsTb = Captain.DatabaseLayer.SPAdminDB.Get_RNGSRGRP(refFdate, refTdate, groupCd, null, null, BaseForm.UserID, refTdate);
                    DataTable dtTb = dsTb.Tables[0];
                    if (dtTb.Rows.Count > 0)
                    {
                        foreach (DataRow drTb in dtTb.Rows)
                        {
                            if (drTb["RNGSRGRP_table_code"].ToString().Trim() == Code.ToString().Trim())
                                isExists = true;
                        }
                    }
                }
            }
            return isExists;
        }

        public string[] GetSelected_Group_Code()
        {
            string[] Added_Edited_GroupCode = new string[3];
            if (GridType == "Code")
            {
                Added_Edited_GroupCode[0] = txtCode.Text;
                Added_Edited_GroupCode[1] = Mode;
            }
            else if (GridType == "Group")
            {
                Added_Edited_GroupCode[0] = txtCode.Text;
                Added_Edited_GroupCode[1] = Mode;
            }
            else
            {
                Added_Edited_GroupCode[0] = groupCd;
                Added_Edited_GroupCode[1] = txtCode.Text;
                Added_Edited_GroupCode[2] = Mode;
            }

            return Added_Edited_GroupCode;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (GridType == "Table")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                this.Close();
        }

        private void txtnum4_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkbDupl_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnResult_Click(object sender, EventArgs e)
        {
            RNG_ResultAssociations ResultForm = new RNG_ResultAssociations(BaseForm, Mode, refFdate, refTdate, GridType, groupCd, tablecd, ReferDesc, Privileage);
            ResultForm.StartPosition = FormStartPosition.CenterScreen;
            ResultForm.ShowDialog();
        }
        string Services = null;

        private void BtnGoal_Click(object sender, EventArgs e)
        {
            if (_TabType == "RPerfMeasures")
            {
                if (chkbUseSer.Checked)
                {
                    //if (drTable["csb14grp_use_servs"].ToString() == "1")
                    Services = "CA";
                }
                else
                {
                    //if (drTable["csb14grp_use_servs"].ToString() == "0")
                    Services = "MS";
                }
                string ProgSw = "N";
                if (chkbPrograms.Checked)
                    ProgSw = "Y";

                RNG_Goalservices Goal = new RNG_Goalservices(BaseForm, Mode, refFdate, refTdate, GridType, groupCd, tablecd, Services, ReferDesc, ProgSw, GoalHieEntity, Privileage, PPR_SW);
                Goal.FormClosed += new FormClosedEventHandler(Goals_Form_Closed);
                Goal.StartPosition = FormStartPosition.CenterScreen;
                Goal.ShowDialog();
            }
            else if (_TabType == "RServices")
            {
                //if (chkbUseSer.Checked)
                //{
                //if (drTable["csb14grp_use_servs"].ToString() == "1")
                Services = "CA";
                //}
                //else
                //{
                //    //if (drTable["csb14grp_use_servs"].ToString() == "0")
                //    Services = "MS";
                //}
                string ProgSw = "N";
                if (chkbPrograms.Checked)
                    ProgSw = "Y";

                RNG_Goalservices Goal = new RNG_Goalservices(BaseForm, Mode, refFdate, refTdate, GridType, groupCd, tablecd, Services, ReferDesc, ProgSw, Privileage, PPR_SW);
                Goal.FormClosed += new FormClosedEventHandler(Goals_Form_Closed);
                Goal.StartPosition = FormStartPosition.CenterScreen;
                Goal.ShowDialog();
            }
            //DataSet dsTable = DatabaseLayer.SPAdminDB.Get_RNGGRP(refFdate, groupCd, tablecd, null);
            //DataRow drTable = dsTable.Tables[0].Rows[0];

        }

        private void cmbAchvResCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isValid = true;
            //switch (((ListItem)cmbAchvResCol.SelectedItem).Value.ToString())
            //{
            //    case "1": if (string.IsNullOrEmpty(txtnum1.Text) || !chkb1.Checked)
            //            {
            //                _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 1".Replace(Consts.Common.Colon, string.Empty)));
            //                isValid = false;
            //            }
            //            else
            //                _errorProvider.SetError(cmbAchvResCol, null);
            //            break;
            //    case "2": if (string.IsNullOrEmpty(txtnum2.Text) || !chkb2.Checked)
            //            {
            //                _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 2".Replace(Consts.Common.Colon, string.Empty)));
            //                isValid = false;
            //            }
            //            else
            //                _errorProvider.SetError(cmbAchvResCol, null);
            //            break;
            //    case "3": if (string.IsNullOrEmpty(txtnum3.Text) || !chkb3.Checked)
            //            {
            //                _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 3".Replace(Consts.Common.Colon, string.Empty)));
            //                isValid = false;
            //            }
            //            else
            //                _errorProvider.SetError(cmbAchvResCol, null);
            //            break;
            //    case "4": if (string.IsNullOrEmpty(txtnum4.Text) || !chkb4.Checked)
            //            {
            //                _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 4".Replace(Consts.Common.Colon, string.Empty)));
            //                isValid = false;
            //            }
            //            else
            //                _errorProvider.SetError(cmbAchvResCol, null);
            //            break;
            //    case "5": if (string.IsNullOrEmpty(txtnum5.Text) || !chkb5.Checked)
            //            {
            //                _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 5".Replace(Consts.Common.Colon, string.Empty)));
            //                isValid = false;
            //            }
            //            else
            //                _errorProvider.SetError(cmbAchvResCol, null);
            //            break;
            //    case "All": if ((string.IsNullOrEmpty(txtnum1.Text) && !chkb1.Checked) && (string.IsNullOrEmpty(txtnum2.Text) && !chkb2.Checked) && (string.IsNullOrEmpty(txtnum3.Text) && !chkb3.Checked) && (string.IsNullOrEmpty(txtnum4.Text) && !chkb4.Checked) && (string.IsNullOrEmpty(txtnum5.Text) && !chkb5.Checked))
            //            {
            //                _errorProvider.SetError(cmbAchvResCol, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResHead.Text + " 5".Replace(Consts.Common.Colon, string.Empty)));
            //                isValid = false;
            //            }
            //            else
            //                _errorProvider.SetError(cmbAchvResCol, null);
            //            break;
            //}
        }

        private void txtCode_LostFocus(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtCode.Text))
            //{
            //    if (GridType == "Group")
            //    {
            //        if (txtCode.Text.Length > 9)
            //        {
            //            //if (int.Parse(txtCode.Text.Trim()) < 2147483647)
            //            //    MessageBox.Show("Group Code was exceeds integer limit", "Capsystems");textBox1.Text = ValidateInt(textBox1.Text, 10, 5, 20).ToString();
            //            txtCode.Text = ValidateInt(txtCode.Text, 0, 0, 2147483647).ToString();
            //            if (txtCode.Text == "0")
            //                txtCode.Clear();
            //                //MessageBox.Show("Group Code was exceeds integer limit", "Capsystems");
            //        }
            //        else if (int.Parse(txtCode.Text.Trim()) < 1)
            //            _errorProvider.SetError(txtCode, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
            //        else
            //            _errorProvider.SetError(txtCode, null);
            //    }
            //    else
            //        _errorProvider.SetError(txtCode, null);
            //}
        }

        public int ValidateInt(object _Data, int _DefaultVal, int _MinVal, int _MaxVal)
        {
            int _val = _DefaultVal;

            try
            {
                if (_Data != null)
                {
                    _val = int.Parse(_Data.ToString());

                    if (_val < _MinVal)
                        _val = _MinVal;
                    else if (_val > _MaxVal)
                        _val = _MaxVal;
                }
            }
            catch (Exception _Exception)
            {
                // Error occured while trying to validate

                // set default value if we ran into a error
                //_val = _DefaultVal;
                txtCode.Clear();

                // You can debug for the error here
                //Console.WriteLine("Error : " + _Exception.Message);
                AlertBox.Show("Group Code exceeds integer limit", MessageBoxIcon.Warning);
            }

            return _val;
        }

        private void txtDesc_LostFocus(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtDesc.Text))
            //    _errorProvider.SetError(txtDesc, null);
        }

        private void cmbAchvResCol_LostFocus(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(cmbAchvResCol.Text))
            //    _errorProvider.SetError(cmbAchvResCol, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void txtAgeFrm_LostFocus(object sender, EventArgs e)
        {
            //bool isvalid = true;
            if (!string.IsNullOrEmpty(txtAgeFrm.Text))
            {
                if (int.Parse(txtAgeFrm.Text.Trim()) < 0)
                {
                    _errorProvider.SetError(txtAgeFrm, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                    //isvalid = false;
                }
                else
                    _errorProvider.SetError(txtAgeFrm, null);
            }
        }

        private void txtAgeTo_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAgeTo.Text))
            {
                if (int.Parse(txtAgeTo.Text.Trim()) < 0)
                {
                    _errorProvider.SetError(txtAgeTo, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                    //isvalid = false;
                }
                else
                    _errorProvider.SetError(txtAgeTo, null);
            }
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                if (GridType == "Group")
                {
                    //if (txtCode.Text.Length > 9)
                    //{
                    //    //if (int.Parse(txtCode.Text.Trim()) < 2147483647)
                    //    //    MessageBox.Show("Group Code was exceeds integer limit", "Capsystems");textBox1.Text = ValidateInt(textBox1.Text, 10, 5, 20).ToString();
                    //    txtCode.Text = ValidateInt(txtCode.Text, 0, 0, 2147483647).ToString();
                    //    if (txtCode.Text == "0")
                    //        txtCode.Clear();
                    //    //MessageBox.Show("Group Code was exceeds integer limit", "Capsystems");
                    //}
                    //else if (long.Parse(txtCode.Text.Trim()) < 1)
                    //    MessageBox.Show("Please provide Positive Value", "CAPTAIN");
                    //    _errorProvider.SetError(txtCode, string.Format("Please provide Positive Value".Replace(Consts.Common.Colon, string.Empty)));
                    //else
                    //    _errorProvider.SetError(txtCode, null);
                }
                //else
                //    _errorProvider.SetError(txtCode, null);
            }
        }

        List<MSMASTEntity> MSList;
        List<CAMASTEntity> CAList;
        List<RCsb14GroupEntity> GrpEntity;
        List<RNGGAEntity> GoalEntity;
        List<RNGGoalHEntity> GoalHieEntity;


        List<SRCsb14GroupEntity> SGrpEntity;
        List<RNGSRGAEntity> SGoalEntity;
        List<RNGSRGoalHEntity> SRGoalHieEntity;
        string AgyGoal_code = null; int BudgetValue = 0;
        private void fillGvGoals()
        {
            if (PPR_SW == "Y")
            {
                this.RNG_SP.Visible = true; RNG_SP.ShowInVisibilityMenu = true;
                this.Goals.Width = 240;
                this.RNG_SP.Width = 220;
                this.Img_Hie.Visible = false; Img_Hie.ShowInVisibilityMenu = false;
                this.pnlgvwHie.Visible = false;
            }
            else
            {
                this.RNG_SP.Visible = false; RNG_SP.ShowInVisibilityMenu = false;
                this.Goals.Width = 460;
                //pnlgvwHie.Visible = true;
                pnlgvwHie.Visible = false;
            }

            if (_TabType == "RPerfMeasures")
            {
                chkIndSwitch.Visible = true; int rowIndex = 0;
                //Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
                //Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
                GrpEntity = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null, BaseForm.UserID, refTdate);
                GoalEntity = _model.SPAdminData.Browse_RNGGA(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null);
                GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(refFdate, refTdate, groupCd, txtCode.Text.Trim(), string.Empty);
                //GoalsList = _model.SPAdminData.Get_AgyRecs("Goals");
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
                MSList = MSList.OrderBy(u => u.Desc.Trim()).ToList();
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                if (Services == "CA")
                    this.Goals.HeaderText = "Service";//"Critical Activity";
                else
                    this.Goals.HeaderText = "Outcome";

                //if (PPR_SW == "Y")
                //    GoalEntity = GoalEntity.FindAll(u => u.SerSP != "0");
                //else
                //    GoalEntity = GoalEntity.FindAll(u => u.SerSP == "0");

                gvGoals.Rows.Clear();

                this.Budget.HeaderText = "Target#";

                if (PPR_SW == "Y")
                    GoalEntity = GoalEntity.FindAll(u => u.SerSP != "0");
                else
                    GoalEntity = GoalEntity.FindAll(u => u.SerSP == "0");

                string Goal = null; string goaldesc = null;
                int i = 0;
                string temp_code = null;

                foreach (RCsb14GroupEntity dr in GrpEntity)
                {
                    if (dr.UseSer.ToString().Trim() == "True")
                    {
                        Services = "CA";
                        this.Goals.HeaderText = "Services";//"Critical Activity";
                    }
                    else
                    {
                        Services = "MS";
                        this.Goals.HeaderText = "Outcomes";
                    }

                }
                BudgetValue = 0;
                GoalEntity = GoalEntity.OrderBy(u => u.Sequence.Trim()).ToList();
                foreach (RNGGAEntity GAEntity in GoalEntity)
                {
                    //if (Services == "MS")
                    //{

                    rowIndex = gvGoals.Rows.Add(GAEntity.SP_desc, GAEntity.Desc, GAEntity.Budget, GAEntity.GoalCode, GAEntity.Sequence, GAEntity.SerSP, true);
                    if (!string.IsNullOrEmpty(GAEntity.Budget.Trim()))
                        BudgetValue += int.Parse(GAEntity.Budget);

                    if (!string.IsNullOrEmpty(GAEntity.CAMS_Cnt.Trim()))
                    {
                        int CA_Cnt = int.Parse(GAEntity.CAMS_Cnt.Trim());
                        //if (CA_Cnt > 0)
                        //    gvGoals.Rows[rowIndex].Cells["Budget"].ReadOnly = true; 
                    }

                    //foreach (MSMASTEntity Entity in MSList)
                    //{
                    //    Goal = Entity.Code;
                    //    goaldesc = Entity.Desc;
                    //    if (!string.IsNullOrWhiteSpace(temp_code))
                    //    {
                    //        if (Entity.Code.ToString().Trim() == GAEntity.GoalCode.ToString().Trim())
                    //        {
                    //            Entity.Sel_SW = true;
                    //            Entity.Mode = GAEntity.Budget;
                    //            //Ststus_Exists = Entity.Sel_WS;
                    //        }

                    //    }
                    //}
                    //}
                    //else
                    //{

                    //    gvGoals.Rows.Add(GAEntity.Desc, GAEntity.Budget,GAEntity.GoalCode);
                    //    if (!string.IsNullOrEmpty(GAEntity.Budget.Trim()))
                    //        BudgetValue += int.Parse(GAEntity.Budget);
                    //    //foreach (CAMASTEntity Entity in CAList)
                    //    //{
                    //    //    Goal = Entity.Code;
                    //    //    goaldesc = Entity.Desc;
                    //    //    if (!string.IsNullOrWhiteSpace(temp_code))
                    //    //    {
                    //    //        if (Entity.Code.ToString().Trim() == GAEntity.GoalCode.ToString().Trim())
                    //    //        {
                    //    //            Entity.Sel_SW = true;
                    //    //            Entity.Mode = GAEntity.Budget;
                    //    //            //Ststus_Exists = Entity.Sel_WS;
                    //    //        }

                    //    //    }
                    //    //}
                    //}
                }

                txtExpAch.Text = BudgetValue.ToString();
                if (gvGoals.Rows.Count > 0)
                {
                    gvGoals.CurrentCell = gvGoals.Rows[0].Cells[1];
                    gvGoals.Rows[0].Selected = true;
                }
                else
                {
                    gvwPrograms.Rows.Clear();
                }

                //if (Services == "MS")
                //{
                //    foreach (MSMASTEntity Entity in MSList)
                //    {
                //        if (Entity.Sel_SW == true)
                //            gvGoals.Rows.Add(Entity.Desc, Entity.Code, "Y", Entity.Mode);
                //        //else
                //        //    GvGoals.Rows.Add(Img_Blank, Entity.Desc, Entity.Code, "N", "");
                //    }
                //}
                //else
                //{
                //    foreach (CAMASTEntity Entity in CAList)
                //    {
                //        if (Entity.Sel_SW == true)
                //            gvGoals.Rows.Add(Entity.Desc, Entity.Code, "Y", Entity.Mode);
                //        //else
                //        //    GvGoals.Rows.Add(Img_Blank, Entity.Desc, Entity.Code, "N", "");
                //    }
                //}
            }
            else if (_TabType == "RServices")
            {


                int rowIndex = 0;
                SGrpEntity = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                SGoalEntity = _model.SPAdminData.Browse_RNGSRGA(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null);
                SRGoalHieEntity = _model.SPAdminData.Browse_RNGSRGAH(refFdate, refTdate, groupCd, txtCode.Text.Trim(), string.Empty);
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                CAList = CAList.OrderBy(u => u.Desc.Trim()).ToList();
                // CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                if (Services == "CA")
                    this.Goals.HeaderText = "Services";//"Critical Activity";
                else
                    this.Goals.HeaderText = "Outcomes";
                this.Budget.HeaderText = "Target#";
                gvGoals.Rows.Clear();
                if (PPR_SW == "Y")
                    SGoalEntity = SGoalEntity.FindAll(u => u.SerSP != "0");
                else
                    SGoalEntity = SGoalEntity.FindAll(u => u.SerSP == "0");

                string Goal = null; string goaldesc = null;
                int i = 0;
                string temp_code = null;

                //foreach (SRCsb14GroupEntity dr in SGrpEntity)
                //{
                //    if (dr.UseSer.ToString().Trim() == "True")
                //    {
                //        Services = "CA";
                //        this.Goals.HeaderText = "Service"; //Critical Activity";
                //    }
                //    else
                //    {
                //        Services = "MS";
                //        this.Goals.HeaderText = "Goals";
                //    }

                //}
                //// this.gvGoals.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvGoals_CellValueChanged);
                SGoalEntity = SGoalEntity.OrderBy(u => u.Sequence.Trim()).ToList();
                foreach (RNGSRGAEntity GAEntity in SGoalEntity)
                {
                    rowIndex = gvGoals.Rows.Add(GAEntity.SP_desc, GAEntity.Desc, GAEntity.Budget, GAEntity.GoalCode, GAEntity.Sequence, GAEntity.SerSP, true);
                    if (!string.IsNullOrEmpty(GAEntity.Budget.Trim()))
                        BudgetValue += int.Parse(GAEntity.Budget);


                    if (!string.IsNullOrEmpty(GAEntity.CAMS_Count.Trim()))
                    {
                        int CA_Cnt = int.Parse(GAEntity.CAMS_Count.Trim());
                        //if (CA_Cnt > 0)
                        //    gvGoals.Rows[rowIndex].Cells["Budget"].ReadOnly = true;
                    }

                }

                txtExpAch.Text = BudgetValue.ToString();

            }

        }

        //private void fillGvGoals()
        //{
        //    //Group = ((RepListItem)CmbGrp.SelectedItem).Value.ToString();
        //    //Table = ((RepListItem)CmbTbl.SelectedItem).Value.ToString();
        //    GrpEntity = _model.SPAdminData.Browse_CSB14Grp(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null);
        //    //GoalsList = _model.SPAdminData.Get_AgyRecs("Goals");
        //    MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
        //    MSList = MSList.OrderBy(u => u.Desc.Trim()).ToList();
        //    CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
        //    if (Services == "CA")
        //        this.Goals.HeaderText = "Critical Activity";
        //    else
        //        this.Goals.HeaderText = "Goals";
        //    gvGoals.Rows.Clear();
        //    bool Can_Continue = true;
        //    string Goal = null; string goaldesc = null;
        //    int i = 0;
        //    string temp_code = null;

        //    foreach (Csb14GroupEntity dr in GrpEntity)
        //    {
        //        if (dr.UseSer.ToString().Trim() == "True")
        //        {
        //            Services = "CA";
        //            this.Goals.HeaderText = "Critical Activity";
        //        }
        //        else
        //        {
        //            Services = "MS";
        //            this.Goals.HeaderText = "Goals";
        //        }
        //        temp_code = dr.GoalCds.ToString();
        //        if (!string.IsNullOrWhiteSpace(temp_code) && temp_code.Length >= 1)
        //        {
        //            //Ststus_Exists = false;
        //            for (i = 0; Can_Continue; )
        //            {
        //                if ((temp_code.Substring(i, temp_code.Length - i).Length) < 10)
        //                    AgyGoal_code = temp_code.Substring(i, temp_code.Substring(i, temp_code.Length - i).Length);
        //                else
        //                    AgyGoal_code = temp_code.Substring(i, 10);
        //                if (Services == "MS")
        //                {
        //                    foreach (MSMASTEntity Entity in MSList)
        //                    {
        //                        Goal = Entity.Code;
        //                        goaldesc = Entity.Desc;
        //                        if (!string.IsNullOrWhiteSpace(temp_code))
        //                        {
        //                            if (Entity.Code.ToString().Trim() == AgyGoal_code.ToString().Trim())
        //                            {
        //                                Entity.Sel_SW = true;
        //                                //Ststus_Exists = Entity.Sel_WS;
        //                            }

        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (CAMASTEntity Entity in CAList)
        //                    {
        //                        Goal = Entity.Code;
        //                        goaldesc = Entity.Desc;
        //                        if (!string.IsNullOrWhiteSpace(temp_code))
        //                        {
        //                            if (Entity.Code.ToString().Trim() == AgyGoal_code.ToString().Trim() && dr.UseSer.ToString().Trim() == "True")
        //                            {
        //                                Entity.Sel_SW = true;
        //                                //Ststus_Exists = Entity.Sel_WS;
        //                            }

        //                        }
        //                    }
        //                }

        //                i += 10;
        //                if (i >= temp_code.Length)
        //                    Can_Continue = false;
        //            }
        //        }
        //    }
        //    if (Services == "MS")
        //    {
        //        foreach (MSMASTEntity Entity in MSList)
        //        {
        //            if (Entity.Sel_SW)
        //                gvGoals.Rows.Add(Entity.Desc);
        //            //else
        //            //    gvGoals.Rows.Add(Entity.Desc);
        //        }
        //    }
        //    else
        //    {
        //        foreach (CAMASTEntity Entity in CAList)
        //        {
        //            if (Entity.Sel_SW)
        //                gvGoals.Rows.Add( Entity.Desc);
        //            //else
        //            //    gvGoals.Rows.Add(Entity.Desc);
        //        }
        //    }
        //}

        string Added_Edited_GroupCode = string.Empty;
        private void Goals_Form_Closed(object sender, FormClosedEventArgs e)
        {
            //RNG_Goalservices form = sender as RNG_Goalservices;
            //if (form.DialogResult == DialogResult.OK)
            //{
            fillGvGoals();
            //}
        }

        private void gvGoals_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != -1 && Mode != null)
            {
                if ((Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    int intcolindex = gvGoals.CurrentCell.ColumnIndex;
                    int introwindex = gvGoals.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvGoals.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvGoals.Rows[introwindex].Cells["Budget"].Value);

                    if (!System.Text.RegularExpressions.Regex.IsMatch(Points, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                    {
                        AlertBox.Show(Consts.Messages.NumericOnly, MessageBoxIcon.Warning);
                        //boolcellstatus = false; IsValid = false;
                        this.gvGoals.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvGoals_CellValueChanged);
                        gvGoals.Rows[introwindex].Cells["Budget"].Value = string.Empty;
                        this.gvGoals.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvGoals_CellValueChanged);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Points.Trim()))
                        {
                            BudgetValue = 0;
                            foreach (DataGridViewRow dr in gvGoals.Rows)
                            {
                                if (!string.IsNullOrEmpty(dr.Cells["Budget"].Value.ToString().Trim()))
                                    BudgetValue += int.Parse(dr.Cells["Budget"].Value.ToString().Trim());
                            }

                            txtExpAch.Text = BudgetValue.ToString();
                        }
                        else
                            gvGoals.Rows[introwindex].Cells["Budget"].Value = string.Empty;
                    }
                    //}
                }
            }
            else if (e.ColumnIndex == 4 && e.RowIndex != -1 && Mode != null)
            {
                if ((Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    int intcolindex = gvGoals.CurrentCell.ColumnIndex;
                    int introwindex = gvGoals.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvGoals.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvGoals.Rows[introwindex].Cells["gvGSeq"].Value);

                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                    {
                        AlertBox.Show(Consts.Messages.NumericOnly, MessageBoxIcon.Warning);
                        //boolcellstatus = false; IsValid = false;
                        this.gvGoals.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvGoals_CellValueChanged);
                        gvGoals.Rows[introwindex].Cells["gvGSeq"].Value = string.Empty;
                        this.gvGoals.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvGoals_CellValueChanged);

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Points.Trim()))
                            gvGoals.Rows[introwindex].Cells["gvGSeq"].Value = string.Empty;
                    }

                    //else
                    //{
                    //    BudgetValue = 0;
                    //    foreach (DataGridViewRow dr in gvGoals.Rows)
                    //    {
                    //        if (!string.IsNullOrEmpty(dr.Cells["Budget"].Value.ToString().Trim()))
                    //            BudgetValue += int.Parse(dr.Cells["Budget"].Value.ToString().Trim());
                    //    }

                    //    txtExpAch.Text = BudgetValue.ToString();
                    //}
                    ////}
                }
            }

        }

        List<HierarchyEntity> selectedHierarchies = new List<HierarchyEntity>();
        private void gvGoals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (PPR_SW != "Y")
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 7 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                    {
                        SPTargetHierarchyTarget HieaddForm = new SPTargetHierarchyTarget(BaseForm, selectedHierarchies, refFdate, groupCd, txtCode.Text.Trim(), refTdate, gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString(), _TabType);
                        HieaddForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                        HieaddForm.StartPosition = FormStartPosition.CenterScreen;
                        HieaddForm.ShowDialog();
                    }
                }
            }
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            SPTargetHierarchyTarget form = sender as SPTargetHierarchyTarget;
            string Hie = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> SelHies = form.SelectedHies();
                if (SelHies.Count > 0)
                {
                    List<HierarchyEntity> hierarchyEntities = selectedHierarchies.FindAll(u => u.PIPActive == SelHies[0].PIPActive);
                    if (hierarchyEntities.Count > 0)
                    {
                        selectedHierarchies = selectedHierarchies.FindAll(u => u.PIPActive != SelHies[0].PIPActive);

                    }

                    selectedHierarchies.AddRange(SelHies);
                }
                else
                {
                    List<HierarchyEntity> hierarchyEntity = selectedHierarchies.FindAll(u => u.PIPActive == gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString());
                    if (hierarchyEntity.Count > 0)
                    {
                        selectedHierarchies = selectedHierarchies.FindAll(u => u.PIPActive != hierarchyEntity[0].PIPActive);
                    }
                }


                if (selectedHierarchies.Count > 0)
                {
                    FillGrid();

                }
                else
                    FillGrid();


                //if (gvGoals.Rows.Count > 0 && (Mode == "Add" || Mode == "Edit"))
                //{
                //    foreach (DataGridViewRow dr in gvGoals.Rows)
                //    {
                //        if (dr.Cells["gvTColType"].Value.ToString() == "Service" || dr.Cells["gvTColType"].Value.ToString() == "Outcome")
                //        {
                //            var dataGridViewRows = dgvTarget.Rows.Cast<DataGridViewRow>().Where(g => g.Cells["Goal_Code"].Value.ToString() == dr.Cells["Goal_Code"].Value.ToString() && g.Cells["gvTColType"].Value.ToString().Trim() == "Program").ToArray();
                //            if (dataGridViewRows.Length > 0)
                //            {
                //                dr.Cells["gvTColProgTrgt"].ReadOnly = true;
                //            }
                //            else
                //            {
                //                if (dr.Cells["IsRead"].Value.ToString().Trim() == "Y")
                //                    dr.Cells["gvTColProgTrgt"].ReadOnly = true;
                //                else if (!string.IsNullOrEmpty(dr.Cells["Group_Code"].Value.ToString().Trim()))
                //                    dr.Cells["gvTColProgTrgt"].ReadOnly = false;
                //            }
                //        }

                //        if (dr.Cells["gvTColType"].Value.ToString() == "Program")
                //            dr.Cells["gvTColProgTrgt"].ReadOnly = false;
                //    }
                //}

            }
        }

        private void FillGrid()
        {
            gvwPrograms.Rows.Clear();

            pnlgvwHie.Visible = true;
            this.pnlGvTable.Size = new System.Drawing.Size(689, 181);


            if (_TabType == "RPerfMeasures")
            {
                SelGoalsHies = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));

                string Agy = string.Empty; string Dept = string.Empty;
                gvwPrograms.Rows.Clear();

                Agy = BaseForm.BaseAdminAgency == "**" ? "" : BaseForm.BaseAdminAgency;

                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                if (caseHierarchy.Count > 0)
                    caseHierarchy = caseHierarchy.FindAll(u => !u.Prog.Trim().Equals(""));

                //DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_ServicePlanFromSerOutcome(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim(), "MS", BaseForm.BaseAdminAgency);
                //if (ds.Tables.Count > 0)
                //{
                //    this.gvwProg.HeaderText = "Service Plan";
                //    DataTable dt = ds.Tables[0];
                //    if (dt.Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            string Tmp_SP_Code = "000000".Substring(0, (6 - dr["SP2_SERVICEPLAN"].ToString().Length)) + dr["SP2_SERVICEPLAN"].ToString();

                //            gvwPrograms.Rows.Add(Tmp_SP_Code + " - " + dr["SP_DESC"].ToString().Trim(), "", txtCode.Text, "");
                //        }
                //    }
                //}

                List<RCsb14GroupEntity> grpCntrls = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, groupCd, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (grpCntrls.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grpCntrls[0].DomainHie.Trim()))
                    {

                        Agy = grpCntrls[0].DomainHie.Substring(0, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(0, 2);
                        Dept = grpCntrls[0].DomainHie.Substring(3, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(3, 2);

                        if (!string.IsNullOrEmpty(Dept.Trim()))
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy) && u.Dept == Dept);
                        else
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(grpCntrls[0].Agency.Trim()))
                        {
                            Agy = grpCntrls[0].Agency.ToString() == "**" ? "" : grpCntrls[0].Agency.ToString();

                            if (!string.IsNullOrEmpty(Agy.Trim()))
                                caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                }

                if (caseHierarchy.Count > 0)
                {
                    int rowIndex = 0; bool isAdd = false;
                    foreach (HierarchyEntity entity in caseHierarchy)
                    {
                        string Target = string.Empty; isAdd = false;
                        if (SelGoalsHies.Count > 0)
                        {
                            List<RNGGoalHEntity> SelProgs = new List<RNGGoalHEntity>();
                            SelProgs = SelGoalsHies.FindAll(u => u.RNGGAH_HIE.Equals(entity.Agency + entity.Dept + entity.Prog));
                            if (SelProgs.Count > 0) Target = SelProgs[0].RNGGAH_TARGET.Trim();
                        }
                        if (!string.IsNullOrEmpty(Target.Trim()))
                            isAdd = true;

                        if (selectedHierarchies.Count > 0)
                        {
                            HierarchyEntity Hierar = selectedHierarchies.Find(u => u.PIPActive.Trim() == gvGoals.CurrentRow.Cells["AGY_CODE"].Value.ToString().Trim() && u.Agency == entity.Agency && u.Dept == entity.Dept && u.Prog == entity.Prog);
                            if (Hierar != null)
                            {
                                isAdd = true;
                            }
                        }

                        if (isAdd)
                            rowIndex = gvwPrograms.Rows.Add(entity.Code + "     " + entity.HirarchyName.Trim(), Target, txtCode.Text, entity.Agency + entity.Dept + entity.Prog);
                    }

                    //if (gvwPrograms.Rows.Count > 0)

                }

            }
            else if (_TabType == "RServices")
            {
                SelSRGoalsHies = SRGoalHieEntity.FindAll(u => u.RNGSRGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));

                string Agy = string.Empty; string Dept = string.Empty;
                gvwPrograms.Rows.Clear();

                Agy = BaseForm.BaseAdminAgency == "**" ? "" : BaseForm.BaseAdminAgency;

                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                if (caseHierarchy.Count > 0)
                    caseHierarchy = caseHierarchy.FindAll(u => !u.Prog.Trim().Equals(""));

                //DataSet ds = Captain.DatabaseLayer.SPAdminDB.Get_ServicePlanFromSerOutcome(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim(), "CA", BaseForm.BaseAdminAgency);
                //this.gvwProg.HeaderText = "Service Plan";
                //if (ds.Tables.Count>0)
                //{
                //    DataTable dt = ds.Tables[0];
                //    if(dt.Rows.Count>0)
                //    {
                //        foreach(DataRow dr in dt.Rows)
                //        {
                //            string Tmp_SP_Code = "000000".Substring(0, (6 - dr["SP2_SERVICEPLAN"].ToString().Length)) + dr["SP2_SERVICEPLAN"].ToString();

                //            gvwPrograms.Rows.Add(Tmp_SP_Code + " - " + dr["SP_DESC"].ToString().Trim() , "", txtCode.Text, "");
                //        }
                //    }
                //}


                List<SRCsb14GroupEntity> grpCntrls = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, groupCd, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (grpCntrls.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grpCntrls[0].DomainHie.Trim()))
                    {

                        Agy = grpCntrls[0].DomainHie.Substring(0, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(0, 2);
                        Dept = grpCntrls[0].DomainHie.Substring(3, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(3, 2);

                        if (!string.IsNullOrEmpty(Dept.Trim()))
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy) && u.Dept == Dept);
                        else
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(grpCntrls[0].Agency.Trim()))
                        {
                            Agy = grpCntrls[0].Agency.ToString() == "**" ? "" : grpCntrls[0].Agency.ToString();

                            if (!string.IsNullOrEmpty(Agy.Trim()))
                                caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                }

                if (caseHierarchy.Count > 0)
                {
                    int rowIndex = 0; bool isAdd = false;
                    foreach (HierarchyEntity entity in caseHierarchy)
                    {
                        string Target = string.Empty; isAdd = false;
                        if (SelSRGoalsHies.Count > 0)
                        {
                            List<RNGSRGoalHEntity> SelProgs = new List<RNGSRGoalHEntity>();
                            SelProgs = SelSRGoalsHies.FindAll(u => u.RNGSRGAH_HIE.Equals(entity.Agency + entity.Dept + entity.Prog));
                            if (SelProgs.Count > 0) Target = SelProgs[0].RNGSRGAH_TARGET.Trim();
                        }

                        if (!string.IsNullOrEmpty(Target.Trim()))
                            isAdd = true;

                        if (selectedHierarchies.Count > 0)
                        {
                            HierarchyEntity Hierar = selectedHierarchies.Find(u => u.PIPActive.Trim() == gvGoals.CurrentRow.Cells["AGY_CODE"].Value.ToString().Trim() && u.Agency == entity.Agency && u.Dept == entity.Dept && u.Prog == entity.Prog);
                            if (Hierar != null)
                            {
                                isAdd = true;
                            }
                        }

                        if (isAdd)
                            rowIndex = gvwPrograms.Rows.Add(entity.Code + "     " + entity.HirarchyName.Trim(), Target, txtCode.Text, entity.Agency + entity.Dept + entity.Prog);
                    }

                    //if(gvwPrograms.Rows.Count>0)

                }
            }

            if (gvwPrograms.Rows.Count > 0)
            {
                gvGoals.CurrentRow.Cells["Budget"].ReadOnly = true;
            }
            //this.Budget.ReadOnly = true;
        }

        string TmpDOB = null, TmpRefTdate = null;
        private void fillComboRefDate()
        {
            cmbRefPer.Items.Clear();
            if (!SendForm)
            {
                cmbRefPer.Items.Add(new ListItem("", "00", BaseForm.BaseAgency, "N"));
            }

            string UserAgency = string.Empty;
            if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            {
                HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******" && u.UsedFlag == "N");
                if (SelHie != null)
                    UserAgency = "**";
            }

            if (_TabType == "RServices")
            {
                List<SRCsb14GroupEntity> CODEList = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);

                if (BaseForm.BaseAdminAgency != "**")
                {
                    if (!string.IsNullOrEmpty(UserAgency))
                        CODEList = CODEList.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                }

                if (CODEList.Count > 0)
                {
                    foreach (SRCsb14GroupEntity GrpEnt in CODEList)
                    {
                        if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                        {
                            string Code = GrpEnt.Code.ToString();
                            string desc = GrpEnt.GrpDesc.ToString();
                            //if (desc.Length <= 50) desc = GetFixedLengthString(desc, 40);

                            cmbRefPer.Items.Add(new ListItem(LookupDataAccess.Getdate(GrpEnt.OFdate.ToString()) + "     " + LookupDataAccess.Getdate(GrpEnt.OTdate.ToString()) + " -" + desc.Trim(), Code, GrpEnt.Agency.Trim(), GrpEnt.PPR_SW.Trim()));
                        }
                    }
                    cmbRefPer.SelectedIndex = 0;
                }
            }
            if (_TabType == "RPerfMeasures")
            {
                List<RCsb14GroupEntity> CODEList = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);

                if (BaseForm.BaseAdminAgency != "**")
                {
                    if (!string.IsNullOrEmpty(UserAgency))
                        CODEList = CODEList.FindAll(u => u.Agency == BaseForm.BaseAdminAgency || u.Agency == UserAgency);
                }

                if (CODEList.Count > 0)
                {
                    foreach (RCsb14GroupEntity GrpEnt in CODEList)
                    {
                        if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                        {
                            string Code = GrpEnt.Code.ToString();
                            string desc = GrpEnt.GrpDesc.ToString();
                            //if (desc.Length <= 50) desc = GetFixedLengthString(desc, 40);
                            cmbRefPer.Items.Add(new ListItem(LookupDataAccess.Getdate(GrpEnt.OFdate.ToString()) + "     " + LookupDataAccess.Getdate(GrpEnt.OTdate.ToString()) + " -" + desc.Trim(), Code, GrpEnt.Agency.Trim(), GrpEnt.PPR_SW.Trim()));
                        }
                    }
                    cmbRefPer.SelectedIndex = 0;
                }
            }
        }
        bool SendForm = true;
        private void chkbCopy_CheckedChanged(object sender, EventArgs e)
        {
            cmbRefPer.Items.Clear();
            if (chkbCopy.Checked)
            {
                //List<Csb14GroupEntity> Exist_Count = _model.SPAdminData.Browse_CSB14Grp(dtpFrom.Value.ToString().Trim(), dtpTo.Value.ToString().Trim(), null, null, null);
                //if (Exist_Count.Count > 0)
                //{
                //     MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n" + "Group Definitions already been defined for Selected" + "\n" + "Refernce Period and will be deleted if you copy.", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, CopyFrom);
                //}
                //else
                //{
                SendForm = true;
                cmbRefPer.Enabled = true;
                fillComboRefDate();
                //}

            }
            else
            {
                chkbCopy.Checked = false;
                SendForm = false;
                cmbRefPer.Enabled = false;
                // this.cmbRefPer.SelectedIndexChanged -= new System.EventHandler(this.cmbRefPer_SelectedIndexChanged);
                fillComboRefDate();
                //  this.cmbRefPer.SelectedIndexChanged += new System.EventHandler(this.cmbRefPer_SelectedIndexChanged);
            }
        }

        private void SaveForm(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;
            //if (senderForm != null)
            //{
            if (dialogresult == DialogResult.Yes || (!chkbCopy.Checked))
            {
                if (GridType == "Code")
                {
                    try
                    {
                        if (ValidateForm())
                        {
                            if (_TabType == "RPerfMeasures")
                            {

                                CaptainModel model = new CaptainModel();
                                RCsb14GroupEntity GrpEntity = new RCsb14GroupEntity();

                                GrpEntity.Agency = ((RepListItem)cmbHie.SelectedItem).Value.ToString();
                                GrpEntity.Code = txtCode.Text;
                                //GrpEntity.GrpCode = txtCode.Text;
                                GrpEntity.GrpDesc = txtDesc.Text;
                                GrpEntity.OFdate = LookupDataAccess.Getdate(dtpFrom.Value.ToShortDateString());
                                GrpEntity.OTdate = LookupDataAccess.Getdate(dtpTo.Value.ToShortDateString());
                                //GrpEntity.Hrd1 = txtnum1.Text.Trim();
                                //GrpEntity.Incld1 = chkb1.Checked ? "1" : "0";
                                //GrpEntity.Hrd2 = txtnum2.Text.Trim();
                                //GrpEntity.Incld2 = chkb2.Checked ? "1" : "0";
                                //GrpEntity.Hrd3 = txtnum3.Text.Trim();
                                //GrpEntity.Incld3 = chkb3.Checked ? "1" : "0";
                                //GrpEntity.Hrd4 = txtnum4.Text.Trim();
                                //GrpEntity.Incld4 = chkb4.Checked ? "1" : "0";
                                //GrpEntity.Hrd5 = txtnum5.Text.Trim();
                                //GrpEntity.Incld5 = chkb5.Checked ? "1" : "0";
                                //if (cmbAchvResCol.Text == "*")
                                //    GrpEntity.ExAchev = "0";
                                //else
                                //    GrpEntity.ExAchev = cmbAchvResCol.Text;
                                GrpEntity.LSTCOperator = BaseForm.UserID;
                                GrpEntity.AddOperator = BaseForm.UserID;
                                string status = chkbActive.Checked ? "Y" : "N";
                                GrpEntity.Active = status;

                                string pprsw = chkbPPR.Checked ? "Y" : "N";
                                GrpEntity.PPR_SW = pprsw;

                                GrpEntity.CopyAgency = ((ListItem)cmbRefPer.SelectedItem).ID.ToString();

                                GrpEntity.ExAchev = ((ListItem)cmbRefPer.SelectedItem).Value.ToString();
                                GrpEntity.Mode = "MULTI";


                                _model.SPAdminData.InsertUpdateRNGGrp(GrpEntity);
                            }

                            else if (_TabType == "RServices")
                            {

                                CaptainModel model = new CaptainModel();
                                SRCsb14GroupEntity SGrpEntity = new SRCsb14GroupEntity();

                                SGrpEntity.Agency = ((RepListItem)cmbHie.SelectedItem).Value.ToString();
                                SGrpEntity.Code = txtCode.Text;
                                //GrpEntity.GrpCode = txtCode.Text;
                                SGrpEntity.GrpDesc = txtDesc.Text;
                                SGrpEntity.OFdate = dtpFrom.Value.ToShortDateString();
                                SGrpEntity.OTdate = dtpTo.Value.ToShortDateString();
                                //GrpEntity.Hrd1 = txtnum1.Text.Trim();
                                //GrpEntity.Incld1 = chkb1.Checked ? "1" : "0";
                                //GrpEntity.Hrd2 = txtnum2.Text.Trim();
                                //GrpEntity.Incld2 = chkb2.Checked ? "1" : "0";
                                //GrpEntity.Hrd3 = txtnum3.Text.Trim();
                                //GrpEntity.Incld3 = chkb3.Checked ? "1" : "0";
                                //GrpEntity.Hrd4 = txtnum4.Text.Trim();
                                //GrpEntity.Incld4 = chkb4.Checked ? "1" : "0";
                                //GrpEntity.Hrd5 = txtnum5.Text.Trim();
                                //GrpEntity.Incld5 = chkb5.Checked ? "1" : "0";
                                //if (cmbAchvResCol.Text == "*")
                                //    GrpEntity.ExAchev = "0";
                                //else
                                //    GrpEntity.ExAchev = cmbAchvResCol.Text;

                                string status = chkbActive.Checked ? "Y" : "N";
                                SGrpEntity.Active = status;

                                string pprsw = chkbPPR.Checked ? "Y" : "N";
                                SGrpEntity.PPR_SW = pprsw;

                                SGrpEntity.LSTCOperator = BaseForm.UserID;
                                SGrpEntity.AddOperator = BaseForm.UserID;

                                SGrpEntity.CopyAgency = ((ListItem)cmbRefPer.SelectedItem).ID.ToString();
                                SGrpEntity.ExAchev = ((ListItem)cmbRefPer.SelectedItem).Value.ToString();
                                SGrpEntity.Mode = "MULTI";
                                _model.SPAdminData.InsertUpdateRNGSRGrp(SGrpEntity);
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }

                    }

                    catch (Exception ex)
                    {

                    }
                }



            }
            //}
        }
        public void CopyFrom(DialogResult dialogresult)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
            if (dialogresult == DialogResult.Yes)
            {
                SendForm = true;
                cmbRefPer.Enabled = true;
                fillComboRefDate();
            }
            else
            {
                //SendForm = false;
                //cmbRefPer.Enabled = false;
                //fillComboRefDate();

            }
            //}
        }

        public static string GetFixedLengthString(string input, int length)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }

            return result;
        }

        List<RNGGoalHEntity> SelGoalsHies = new List<RNGGoalHEntity>();

        List<RNGSRGoalHEntity> SelSRGoalsHies = new List<RNGSRGoalHEntity>();
        private void gvGoals_SelectionChanged(object sender, EventArgs e)
        {
            if (gvGoals.Rows.Count > 0)
            {

                ////_model.SPAdminData.Browse_RNGGAH(refFdate, refTdate, groupCd, txtCode.Text.Trim(), gvGoals.CurrentRow.Cells["Agy_Code"].ToString());
                ////FillProgramsGrid();
                //if (PPR_SW != "Y")
                //{
                //    FillGrid();
                //}
                chkbTarCopyFrom.Checked = false;
            }
            else { gvwPrograms.Rows.Clear(); chkbTarCopyFrom.Checked = false; }
        }

        private void gvwPrograms_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex != -1 && Mode != null)
            {
                if ((Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    int intcolindex = gvwPrograms.CurrentCell.ColumnIndex;
                    int introwindex = gvwPrograms.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvwPrograms.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvwPrograms.Rows[introwindex].Cells["gvwTarget"].Value);

                    if (!System.Text.RegularExpressions.Regex.IsMatch(Points, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                    {
                        AlertBox.Show(Consts.Messages.NumericOnly, MessageBoxIcon.Warning);
                        //boolcellstatus = false; IsValid = false;
                        this.gvwPrograms.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvwPrograms_CellValueChanged);
                        gvwPrograms.Rows[introwindex].Cells["gvwTarget"].Value = string.Empty;
                        this.gvwPrograms.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvwPrograms_CellValueChanged);

                    }
                    else
                    {
                        if (_TabType == "RPerfMeasures")
                        {
                            RNGGoalHEntity Entity = new RNGGoalHEntity();
                            if (!CopyProgs)
                            {
                                Entity = GoalHieEntity.Find(u => u.RNGGAH_GOAL_CODE.Trim() == gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim() && u.RNGGAH_HIE == gvwPrograms.Rows[introwindex].Cells["gvwProgCode"].Value.ToString()); //&& u.RNGGAH_HIE.Equals(gvwPrograms.Rows[introwindex].Cells["gvwTarget"].Value.ToString()));
                                if (Entity != null)
                                {
                                    Entity.RNGGAH_TARGET = Points;

                                    GoalHieEntity.Remove(Entity);

                                    if (!string.IsNullOrEmpty(Points.Trim()))
                                        GoalHieEntity.Add(Entity);

                                }
                                else
                                {
                                    Entity = new RNGGoalHEntity();
                                    Entity.RNGGAH_CODE = refFdate;
                                    Entity.RNGGAH_AGENCY = refTdate;
                                    Entity.RNGGAH_GRP_CODE = groupCd;
                                    Entity.RNGGAH_TBL_CODE = txtCode.Text;
                                    Entity.RNGGAH_GOAL_CODE = gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString();
                                    Entity.RNGGAH_HIE = gvwPrograms.Rows[introwindex].Cells["gvwProgCode"].Value.ToString();
                                    Entity.RNGGAH_TARGET = Points;
                                    Entity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;
                                    Entity.RNGGAH_DATE_LSTC = DateTime.Now.ToString();

                                    GoalHieEntity.Add(Entity);
                                }
                            }
                            else
                            {
                                Entity = new RNGGoalHEntity();
                                Entity.RNGGAH_CODE = refFdate;
                                Entity.RNGGAH_AGENCY = refTdate;
                                Entity.RNGGAH_GRP_CODE = groupCd;
                                Entity.RNGGAH_TBL_CODE = txtCode.Text;
                                Entity.RNGGAH_GOAL_CODE = gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString();
                                if (!CopyProgs)
                                {
                                    Entity.RNGGAH_HIE = gvwPrograms.Rows[introwindex].Cells["gvwProgCode"].Value.ToString();
                                    Entity.RNGGAH_TARGET = Points;
                                }
                                else
                                {
                                    Entity.RNGGAH_HIE = ProgCode;
                                    Entity.RNGGAH_TARGET = CopyPoints;
                                }
                                Entity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;
                                Entity.RNGGAH_DATE_LSTC = DateTime.Now.ToString();

                                GoalHieEntity.Add(Entity);
                            }

                            int Targets = 0;

                            if (GoalHieEntity.Count > 0)
                            {
                                List<RNGGoalHEntity> SelTargets = new List<RNGGoalHEntity>();
                                SelTargets = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim() == gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim());
                                SelTargets = SelTargets.FindAll(u => u.RNGGAH_TARGET.Trim() != "");
                                if (SelTargets.Count > 0) Targets = SelTargets.Sum(u => int.Parse(u.RNGGAH_TARGET.Trim()));

                                gvGoals.CurrentRow.Cells["Budget"].Value = Targets.ToString();
                            }

                            //BudgetValue = 0;
                            //foreach (DataGridViewRow dr in gvGoals.Rows)
                            //{
                            //    if (!string.IsNullOrEmpty(dr.Cells["Budget"].Value.ToString().Trim()))
                            //        BudgetValue += int.Parse(dr.Cells["Budget"].Value.ToString().Trim());
                            //}

                            //txtExpAch.Text = BudgetValue.ToString();

                        }
                        else if (_TabType == "RServices")
                        {
                            RNGSRGoalHEntity Entity = new RNGSRGoalHEntity();
                            if (!CopyProgs)
                            {
                                Entity = SRGoalHieEntity.Find(u => u.RNGSRGAH_GOAL_CODE.Trim() == gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim() && u.RNGSRGAH_HIE == gvwPrograms.Rows[introwindex].Cells["gvwProgCode"].Value.ToString()); //&& u.RNGGAH_HIE.Equals(gvwPrograms.Rows[introwindex].Cells["gvwTarget"].Value.ToString()));
                                if (Entity != null)
                                {
                                    Entity.RNGSRGAH_TARGET = Points;

                                    SRGoalHieEntity.Remove(Entity);

                                    if (!string.IsNullOrEmpty(Points.Trim()))
                                        SRGoalHieEntity.Add(Entity);

                                }
                                else
                                {
                                    Entity = new RNGSRGoalHEntity();
                                    Entity.RNGSRGAH_CODE = refFdate;
                                    Entity.RNGSRGAH_AGENCY = refTdate;
                                    Entity.RNGSRGAH_GRP_CODE = groupCd;
                                    Entity.RNGSRGAH_TBL_CODE = txtCode.Text;
                                    Entity.RNGSRGAH_GOAL_CODE = gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString();
                                    Entity.RNGSRGAH_HIE = gvwPrograms.Rows[introwindex].Cells["gvwProgCode"].Value.ToString();
                                    Entity.RNGSRGAH_TARGET = Points;
                                    Entity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;
                                    Entity.RNGSRGAH_DATE_LSTC = DateTime.Now.ToString();

                                    SRGoalHieEntity.Add(Entity);
                                }
                            }
                            else
                            {
                                Entity = new RNGSRGoalHEntity();
                                Entity.RNGSRGAH_CODE = refFdate;
                                Entity.RNGSRGAH_AGENCY = refTdate;
                                Entity.RNGSRGAH_GRP_CODE = groupCd;
                                Entity.RNGSRGAH_TBL_CODE = txtCode.Text;
                                Entity.RNGSRGAH_GOAL_CODE = gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString();
                                if (!CopyProgs)
                                {
                                    Entity.RNGSRGAH_HIE = gvwPrograms.Rows[introwindex].Cells["gvwProgCode"].Value.ToString();
                                    Entity.RNGSRGAH_TARGET = Points;
                                }
                                else
                                {
                                    Entity.RNGSRGAH_HIE = ProgCode;
                                    Entity.RNGSRGAH_TARGET = CopyPoints;
                                }
                                Entity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;
                                Entity.RNGSRGAH_DATE_LSTC = DateTime.Now.ToString();

                                SRGoalHieEntity.Add(Entity);
                            }

                            int Targets = 0;

                            if (SRGoalHieEntity.Count > 0)
                            {
                                List<RNGSRGoalHEntity> SelTargets = new List<RNGSRGoalHEntity>();
                                SelTargets = SRGoalHieEntity.FindAll(u => u.RNGSRGAH_GOAL_CODE.Trim() == gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim());
                                SelTargets = SelTargets.FindAll(u => u.RNGSRGAH_TARGET.Trim() != "");
                                if (SelTargets.Count > 0) Targets = SelTargets.Sum(u => int.Parse(u.RNGSRGAH_TARGET.Trim()));

                                gvGoals.CurrentRow.Cells["Budget"].Value = Targets.ToString();
                            }
                        }
                    }
                    //}

                }
            }
        }

        private void FillProgramsGrid()
        {
            if (_TabType == "RPerfMeasures")
            {
                SelGoalsHies = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));

                string Agy = string.Empty; string Dept = string.Empty;
                gvwPrograms.Rows.Clear();

                Agy = BaseForm.BaseAdminAgency == "**" ? "" : BaseForm.BaseAdminAgency;

                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                if (caseHierarchy.Count > 0)
                    caseHierarchy = caseHierarchy.FindAll(u => !u.Prog.Trim().Equals(""));

                List<RCsb14GroupEntity> grpCntrls = _model.SPAdminData.Browse_RNGGrp(refFdate, refTdate, groupCd, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (grpCntrls.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grpCntrls[0].DomainHie.Trim()))
                    {

                        Agy = grpCntrls[0].DomainHie.Substring(0, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(0, 2);
                        Dept = grpCntrls[0].DomainHie.Substring(3, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(3, 2);

                        if (!string.IsNullOrEmpty(Dept.Trim()))
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy) && u.Dept == Dept);
                        else
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(grpCntrls[0].Agency.Trim()))
                        {
                            Agy = grpCntrls[0].Agency.ToString() == "**" ? "" : grpCntrls[0].Agency.ToString();

                            if (!string.IsNullOrEmpty(Agy.Trim()))
                                caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                }

                if (caseHierarchy.Count > 0)
                {

                    foreach (HierarchyEntity entity in caseHierarchy)
                    {
                        string Target = string.Empty;
                        if (SelGoalsHies.Count > 0)
                        {
                            List<RNGGoalHEntity> SelProgs = new List<RNGGoalHEntity>();
                            SelProgs = SelGoalsHies.FindAll(u => u.RNGGAH_HIE.Equals(entity.Agency + entity.Dept + entity.Prog));
                            if (SelProgs.Count > 0) Target = SelProgs[0].RNGGAH_TARGET.Trim();
                        }
                        int rowIndex = gvwPrograms.Rows.Add(entity.Code + "     " + entity.HirarchyName.Trim(), Target, txtCode.Text, entity.Agency + entity.Dept + entity.Prog);
                    }

                    //if(gvwPrograms.Rows.Count>0)

                }

            }
            else if (_TabType == "RServices")
            {
                SelSRGoalsHies = SRGoalHieEntity.FindAll(u => u.RNGSRGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));

                string Agy = string.Empty; string Dept = string.Empty;
                gvwPrograms.Rows.Clear();

                Agy = BaseForm.BaseAdminAgency == "**" ? "" : BaseForm.BaseAdminAgency;

                List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserID, "I", "I");
                if (caseHierarchy.Count > 0)
                    caseHierarchy = caseHierarchy.FindAll(u => !u.Prog.Trim().Equals(""));

                List<SRCsb14GroupEntity> grpCntrls = _model.SPAdminData.Browse_RNGSRGrp(refFdate, refTdate, groupCd, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                if (grpCntrls.Count > 0)
                {
                    if (!string.IsNullOrEmpty(grpCntrls[0].DomainHie.Trim()))
                    {

                        Agy = grpCntrls[0].DomainHie.Substring(0, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(0, 2);
                        Dept = grpCntrls[0].DomainHie.Substring(3, 2) == "**" ? "" : grpCntrls[0].DomainHie.Substring(3, 2);

                        if (!string.IsNullOrEmpty(Dept.Trim()))
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy) && u.Dept == Dept);
                        else
                            caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(grpCntrls[0].Agency.Trim()))
                        {
                            Agy = grpCntrls[0].Agency.ToString() == "**" ? "" : grpCntrls[0].Agency.ToString();

                            if (!string.IsNullOrEmpty(Agy.Trim()))
                                caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                        caseHierarchy = caseHierarchy.FindAll(u => u.Agency.Trim().Equals(Agy));
                }

                if (caseHierarchy.Count > 0)
                {

                    foreach (HierarchyEntity entity in caseHierarchy)
                    {
                        string Target = string.Empty;
                        if (SelSRGoalsHies.Count > 0)
                        {
                            List<RNGSRGoalHEntity> SelProgs = new List<RNGSRGoalHEntity>();
                            SelProgs = SelSRGoalsHies.FindAll(u => u.RNGSRGAH_HIE.Equals(entity.Agency + entity.Dept + entity.Prog));
                            if (SelProgs.Count > 0) Target = SelProgs[0].RNGSRGAH_TARGET.Trim();
                        }
                        int rowIndex = gvwPrograms.Rows.Add(entity.Code + "     " + entity.HirarchyName.Trim(), Target, txtCode.Text, entity.Agency + entity.Dept + entity.Prog);
                    }

                    //if(gvwPrograms.Rows.Count>0)

                }
            }
        }

        bool CopyProgs = false; string ProgCode = string.Empty; string CopyPoints = string.Empty;
        private void cmbGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGoals.Items.Count > 0)
            {
                CopyProgs = true;
                string GCd = ((ListItem)cmbGoals.SelectedItem).Value.ToString();
                List<RNGGoalHEntity> ProgTargets = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(GCd));
                if (gvwPrograms.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in gvwPrograms.Rows)
                    {
                        RNGGoalHEntity SelTar = ProgTargets.Find(u => u.RNGGAH_HIE.Equals(dr.Cells["gvwProgCode"].Value.ToString()));
                        if (SelTar != null)
                        {
                            ProgCode = dr.Cells["gvwProgCode"].Value.ToString();
                            CopyPoints = SelTar.RNGGAH_TARGET;

                            dr.Cells["gvwTarget"].Value = SelTar.RNGGAH_TARGET;


                        }
                        else
                        {
                            dr.Cells["gvwTarget"].Value = string.Empty;
                        }
                    }

                    CopyProgs = false;
                }
            }
        }

        private void chkbTarCopyFrom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbTarCopyFrom.Checked)
            {
                cmbGoals.Visible = true; CopyProgs = true;
                FillGoalsCombo();
            }
            else
            {
                chkbTarCopyFrom.Checked = false; CopyProgs = false;
                cmbGoals.Visible = false;
            }
        }

        private void chkbPrograms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbPrograms.Checked)
            {
                pnlgvwHie.Visible = true;
                this.pnlGvTable.Size = new System.Drawing.Size(689, 181);
                //this.Budget.ReadOnly = true;
                if (_TabType == "RPerfMeasures")
                {
                    GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(refFdate, refTdate, groupCd, txtCode.Text.Trim(), string.Empty);
                    //if (GoalHieEntity.Count > 0)
                    //    SelGoalsHies = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));
                    //else SelGoalsHies = new List<RNGGoalHEntity>();
                    FillProgramsGrid();
                }
                else if (_TabType == "RServices")
                {
                    SRGoalHieEntity = _model.SPAdminData.Browse_RNGSRGAH(refFdate, refTdate, groupCd, txtCode.Text.Trim(), string.Empty);
                    //if (GoalHieEntity.Count > 0)
                    //    SelGoalsHies = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim()));
                    //else SelGoalsHies = new List<RNGGoalHEntity>();
                    FillProgramsGrid();
                }


            }
            else
            {
                pnlgvwHie.Visible = false;
                this.pnlGvTable.Size = new System.Drawing.Size(689, 362);
                this.Budget.ReadOnly = false;

                if (_TabType == "RPerfMeasures")
                {
                    RNGGoalHEntity DEntity = new RNGGoalHEntity();
                    DEntity.RNGGAH_CODE = refFdate.ToString();
                    DEntity.RNGGAH_AGENCY = refTdate;
                    DEntity.RNGGAH_TBL_CODE = txtCode.Text;
                    DEntity.RNGGAH_GRP_CODE = groupCd.ToString();
                    //DEntity.RNGGAH_GOAL_CODE = dr.Cells["Agy_Code"].Value.ToString();
                    DEntity.Mode = "Delete";
                    DEntity.RNGGAH_LSTC_OPERATOR = BaseForm.UserID;
                    _model.SPAdminData.InsertUpdateRNGGAHIE(DEntity);
                }
                else if (_TabType == "RServices")
                {
                    RNGSRGoalHEntity DEntity = new RNGSRGoalHEntity();
                    DEntity.RNGSRGAH_CODE = refFdate.ToString();
                    DEntity.RNGSRGAH_AGENCY = refTdate;
                    DEntity.RNGSRGAH_TBL_CODE = txtCode.Text;
                    DEntity.RNGSRGAH_GRP_CODE = groupCd.ToString();
                    //DEntity.RNGGAH_GOAL_CODE = dr.Cells["Agy_Code"].Value.ToString();
                    DEntity.Mode = "Delete";
                    DEntity.RNGSRGAH_LSTC_OPERATOR = BaseForm.UserID;
                    _model.SPAdminData.InsertUpdateRNGSRGAHIE(DEntity);
                }



            }
        }

        private void RPerformanceMeasureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (GridType == "Table")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                this.Close();
        }

        private void cmbRefPer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbRefPer.Items.Count > 0) 
            {
                if (((ListItem)cmbRefPer.SelectedItem).ValueDisplayCode.ToString() == "Y")
                    chkbPPR.Checked = true;
                else 
                    chkbPPR.Checked = false;
            }
        }

        bool isBRD = true;
        private void txtBenFResnDomain_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBenFResnDomain.Text.Trim()))
            {
                string domain = string.Empty;
                if (txtBenFResnDomain.Text.Length < 2)
                {
                    domain = txtBenFResnDomain.Text.Trim().PadLeft(2, '0');
                    txtBenFResnDomain.Text = domain;
                }
                else
                    domain = txtBenFResnDomain.Text.Trim();

                if (domain != "07" && domain != "09" && domain != "10" && domain != "11" && domain != "12" && domain != "13" && domain != "14" && domain != "15" && domain != "16" && domain != "17")
                {
                    AlertBox.Show("Must enter a valid value (7 or 9 through 17)", MessageBoxIcon.Warning);
                    txtBenFResnDomain.Text = string.Empty;
                    isBRD = false;
                }
            }
        }

        private void chkbCSBG_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbCSBG.Checked)
            {
                lblBenfResnDomain_Req.Visible = true;
                txtBenFResnDomain.Enabled = true;
            }
            else
            {
                lblBenfResnDomain_Req.Visible = false;
                txtBenFResnDomain.Enabled = false;
                txtBenFResnDomain.Text = string.Empty;
            }
        }

        private void FillGoalsCombo()
        {
            if (_TabType == "RPerfMeasures")
            {
                this.cmbGoals.SelectedIndexChanged -= new System.EventHandler(this.cmbGoals_SelectedIndexChanged);
                cmbGoals.Items.Clear();
                this.cmbGoals.SelectedIndexChanged += new System.EventHandler(this.cmbGoals_SelectedIndexChanged);

                List<RNGGAEntity> AGoalEntity = _model.SPAdminData.Browse_RNGGA(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null);
                int cnt = 0;
                if (AGoalEntity.Count > 0)
                {
                    foreach (RNGGAEntity entity in AGoalEntity)
                    {
                        if (entity.GoalCode.Trim() != gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim())
                        {
                            List<RNGGoalHEntity> ProgTargets = GoalHieEntity.FindAll(u => u.RNGGAH_GOAL_CODE.Trim().Equals(entity.GoalCode.Trim()));

                            if (ProgTargets.Count > 0)
                            {
                                cmbGoals.Items.Add(new ListItem(entity.Desc.Trim(), entity.GoalCode, entity.TblCode, entity.Agency));

                                cnt++;
                            }
                        }
                    }

                    if (cnt > 0) cmbGoals.SelectedIndex = 0;
                }

            }
            else if (_TabType == "RServices")
            {
                this.cmbGoals.SelectedIndexChanged -= new System.EventHandler(this.cmbGoals_SelectedIndexChanged);
                cmbGoals.Items.Clear();
                this.cmbGoals.SelectedIndexChanged += new System.EventHandler(this.cmbGoals_SelectedIndexChanged);

                List<RNGSRGAEntity> AGoalEntity = _model.SPAdminData.Browse_RNGSRGA(refFdate, refTdate, groupCd, txtCode.Text.Trim(), null);
                int cnt = 0;
                if (AGoalEntity.Count > 0)
                {
                    foreach (RNGSRGAEntity entity in AGoalEntity)
                    {
                        if (entity.GoalCode.Trim() != gvGoals.CurrentRow.Cells["Agy_Code"].Value.ToString().Trim())
                        {
                            List<RNGSRGoalHEntity> ProgTargets = SRGoalHieEntity.FindAll(u => u.RNGSRGAH_GOAL_CODE.Trim().Equals(entity.GoalCode.Trim()));

                            if (ProgTargets.Count > 0)
                            {
                                cmbGoals.Items.Add(new ListItem(entity.Desc.Trim(), entity.GoalCode, entity.TblCode, entity.Agency));

                                cnt++;
                            }
                        }
                    }

                    if (cnt > 0) cmbGoals.SelectedIndex = 0;
                }
            }

        }

    }
}