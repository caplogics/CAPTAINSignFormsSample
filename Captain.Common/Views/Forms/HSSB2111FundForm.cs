#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using NPOI.SS.Formula.Functions;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Web.Internal.XmlProcessor;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2111FundForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;
        int selRow = 0;
        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        public string strTaskCount = string.Empty;
        #endregion

        /// <summary>
        /// This Constractor Using Funding Code
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="selFundincode"></param>
        /// <param name="privileges"></param>

        public HSSB2111FundForm(BaseForm baseform, List<CommonEntity> selFundincode, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"Funding Selection";
            BaseForm = baseform;
            fundingList = selFundincode;
            fillGridFundings(string.Empty);


        }
        public HSSB2111FundForm(BaseForm baseform, List<CommonEntity> selFundincode, PrivilegeEntity privileges, int isInKindform)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            BaseForm = baseform;
            Privileges = privileges;
            _model = new CaptainModel();

            if (Privileges.Program == "CASE0012")
                this.Text = "Program Selection";
            else if (Privileges.Program == "HSSB0126")
                this.Text = "Select Funding Source";

            fundingList = selFundincode;
            fillGridFundings(string.Empty);
        }

        public HSSB2111FundForm(BaseForm baseform, List<CommonEntity> selFundincode, PrivilegeEntity privileges, string Agy, string Dept, string Prog)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"Funding Selection";
            BaseForm = baseform; Program = Prog;
            Agency = Agy; Depart = Dept;
            fundingList = selFundincode;
            FillGridEMSFunds();


        }

        //Added by Sudheer on 07/18/2022
        public HSSB2111FundForm(BaseForm baseform, List<CommonEntity> selFundincode, PrivilegeEntity privileges, string Agy, string Dept, string Prog, string HieFilter)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"Funding Selection";
            BaseForm = baseform; Program = Prog;
            Agency = Agy; Depart = Dept; StrFilter = HieFilter;
            fundingList = selFundincode;
            fillGridFundings(string.Empty);

        }

        public HSSB2111FundForm(BaseForm baseform, List<CommonEntity> selFundincode, PrivilegeEntity privileges, string Agy, string Dept, string Prog, string HieFilter,List<string> varFundlist)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"Funding Selection";
            BaseForm = baseform; Program = Prog;
            Agency = Agy; Depart = Dept; StrFilter = HieFilter;
            fundingList = selFundincode;
            VarfundingList = varFundlist;
            fillGridFundings(privileges.Program);

        }


        /// <summary>
        /// This Constractor Using Funding Code Filter Only DisplayType(Like Y Or N) values
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="selFundincode"></param>
        /// <param name="privileges"></param>

        public HSSB2111FundForm(BaseForm baseform, PrivilegeEntity privileges, List<CommonEntity> selFundincode, string strFilterType, string strDiplayType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"Funding Selection";
            BaseForm = baseform;
            fundingList = selFundincode;
            fillGridFilterFunds(strFilterType, strDiplayType);


        }


        /// <summary>
        /// This Constractor Using Funding Code 
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="selFundincode"></param>
        /// <param name="privileges"></param>

        public HSSB2111FundForm(BaseForm baseform, List<CommonEntity> selFundincode, PrivilegeEntity privileges, string Daycare)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            Privileges = privileges;
            GchartCode = string.Empty;
            this.Text = /*privileges.Program + " - */"Funding Selection";
            BaseForm = baseform;
            fundingList = selFundincode;
            fillGridFundings(Daycare);


        }

        /// <summary>
        /// This Constractor Using Task and Components Filling.
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="selTaskcode"></param>
        /// <param name="privileges"></param>
        /// <param name="Component"></param>
        /// <param name="TaskCount"></param>
        public HSSB2111FundForm(BaseForm baseform, List<ChldTrckEntity> selTaskcode, PrivilegeEntity privileges, string Component, string TaskCount, string TaskGchart)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            Privileges = privileges;
            BaseForm = baseform;
            this.Text = /*privileges.Program + " - */privileges.PrivilegeName;
            ComponentCode = Component;
            TasksList = selTaskcode;
            GchartCode = TaskGchart;
            //this.Size = new System.Drawing.Size(368, 341);
            //this.pnlCompleteForm.Size = new System.Drawing.Size(364, 338);
            //this.gvwFundSource.Size = new System.Drawing.Size(357, 308);
            //this.btnSelect.Location = new System.Drawing.Point(225, 312);
            //this.btnCancel.Location = new System.Drawing.Point(290, 312);
            //this.chkSelectAll.Location = new Point(10, 312);
            //this.chkUnselectAll.Location = new Point(89, 312);
            //this.gvtDesc.Width = 230;
            strTaskCount = TaskCount;
            FillGridTasks();
            if (privileges.Program == "HSSB0103")
            {
                chkSelectAll.Visible = false;
                chkUnselectAll.Visible = false;
            }


        }

        public HSSB2111FundForm(BaseForm baseform, List<ChldTrckEntity> selTaskcode, PrivilegeEntity privileges, string Component, string TaskCount, string TaskGchart, string FormName)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            Privileges = privileges;
            BaseForm = baseform;
            this.Text = /*privileges.Program + " - */privileges.PrivilegeName;
            ComponentCode = Component;
            TasksList = selTaskcode;
            GchartCode = TaskGchart;
            FormCode = FormName;
           // this.Size = new System.Drawing.Size(368, 341);
           // this.pnlCompleteForm.Size = new System.Drawing.Size(364, 338);
           // this.gvwFundSource.Size = new System.Drawing.Size(357, 308);
            //this.btnSelect.Location = new System.Drawing.Point(225, 312);
            //this.btnCancel.Location = new System.Drawing.Point(290, 312);
            //this.chkSelectAll.Location = new Point(10, 312);
           // this.chkUnselectAll.Location = new Point(89, 312);
            chkSelectAll.Visible = false; chkUnselectAll.Visible = false;
            //this.gvtDesc.Width = 230;
            strTaskCount = TaskCount;
            FillGridTasks();
            if (privileges.Program == "HSSB0103")
            {
                chkSelectAll.Visible = false;
                chkUnselectAll.Visible = false;
            }


        }

        /// <summary>
        /// This Constractor Using Task and Components Filling.
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="selTaskcode"></param>
        /// <param name="privileges"></param>
        /// <param name="Component"></param>
        /// <param name="TaskCount"></param>
        public HSSB2111FundForm(BaseForm baseform, List<HlsTrckEntity> selTaskcode, PrivilegeEntity privileges, string Component, string TaskCount, string TaskGchart)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = privileges;
            this.Text = /*privileges.Program + " - */privileges.PrivilegeName;
            ComponentCode = Component;
            HlsTasksList = selTaskcode;
            GchartCode = TaskGchart;
            this.Size = new System.Drawing.Size(368, 341);
            this.pnlCompleteForm.Size = new System.Drawing.Size(364, 338);
            this.gvwFundSource.Size = new System.Drawing.Size(357, 308);
            this.btnSelect.Location = new System.Drawing.Point(225, 312);
            this.btnCancel.Location = new System.Drawing.Point(290, 312);
            this.chkSelectAll.Location = new Point(10, 312);
            this.chkUnselectAll.Location = new Point(89, 312);
            this.gvtDesc.Width = 230;
            strTaskCount = TaskCount;
            FillGridTasks();
            if (privileges.Program == "HSSB0103")
            {
                chkSelectAll.Visible = false;
                chkUnselectAll.Visible = false;
            }


        }

        public HSSB2111FundForm(BaseForm baseform, List<HlsTrckEntity> selTaskcode, PrivilegeEntity privileges, string Component, string TaskCount, string TaskGchart, string FormName)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseform;
            this.Text = /*privileges.Program + " - */privileges.PrivilegeName;
            Privileges = privileges;
            ComponentCode = Component;
            HlsTasksList = selTaskcode;
            GchartCode = TaskGchart;
            FormCode = FormName;
            this.Size = new System.Drawing.Size(368, 341);
            this.pnlCompleteForm.Size = new System.Drawing.Size(364, 338);
            this.gvwFundSource.Size = new System.Drawing.Size(357, 308);
            this.btnSelect.Location = new System.Drawing.Point(225, 312);
            this.btnCancel.Location = new System.Drawing.Point(290, 312);
            this.chkSelectAll.Location = new Point(10, 312);
            this.chkUnselectAll.Location = new Point(89, 312);
            chkSelectAll.Visible = false; chkUnselectAll.Visible = false;
            this.gvtDesc.Width = 230;
            strTaskCount = TaskCount;
            FillGridTasks();
            if (privileges.Program == "HSSB0103")
            {
                chkSelectAll.Visible = false;
                chkUnselectAll.Visible = false;
            }

        }


        public HSSB2111FundForm(BaseForm baseform, List<SaldefEntity> selsaldefCodes, PrivilegeEntity privileges, string Agy, string Dept, string Prog, string formType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"SAL/CAL Selection";
            BaseForm = baseform;
            Program = Prog;
            Agency = Agy; Depart = Dept;
            SalDefList = selsaldefCodes;
            FormCode = formType;
            FillGridSALdefs();
        }

        public HSSB2111FundForm(BaseForm baseform, List<CMBDCEntity> selbudgetList, PrivilegeEntity privileges, string Agy, string Dept, string Prog, string stryear, string strfund)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + " - */"Budget Selection";
            BaseForm = baseform;
            Program = Prog;
            Agency = Agy; Depart = Dept; StrYear = stryear; StrFund = strfund;
            SelBudgetList = selbudgetList;
            //FormCode = formType;
            FillBudgets();
        }

        public HSSB2111FundForm(BaseForm baseform, List<CMBDCEntity> selbudgetList, PrivilegeEntity privileges, string Agy, string Dept, string Prog, string stryear, List<CommonEntity> selFundincode, string fdate, string ldate,string fundtype)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            GchartCode = string.Empty;
            Privileges = privileges;
            _model = new CaptainModel();
            this.Text = /*privileges.Program + */"Budget Selection";
            BaseForm = baseform;
            Program = Prog;
            Agency = Agy; Depart = Dept; StrYear = stryear; //StrFund = strfund;
            SelBudgetList = selbudgetList;
            fundingList = selFundincode;
            FDate = fdate; LDate = ldate;FundType= fundtype;
            //FormCode = formType;
            FillBudgets(); 
        }



        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Code { get; set; }

        public string ComponentCode { get; set; }

        public string GchartCode { get; set; }

        public string FormCode { get; set; }

        public string StrYear { get; set; }

        public string StrFund { get; set; }

        public string FDate { get; set; }

        public string LDate { get; set; }
        public string FundType { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public List<CommonEntity> fundingList { get; set; }

        public List<ChldTrckEntity> TasksList { get; set; }

        public List<HlsTrckEntity> HlsTasksList { get; set; }

        public List<SaldefEntity> SalDefList { get; set; }

        public List<SaldefEntity> SALDEFEntity { get; set; }

        public List<CMBDCEntity> SelBudgetList { get; set; }

        public List<string> VarfundingList { get; set; }

        public bool IsSaveValid { get; set; }

        public string Agency { get; set; }
        public string Depart { get; set; }
        public string Program { get; set; }
        public string StrFilter { get; set; }

        #endregion

        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        private void btnSelect_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(GchartCode.Trim()))
            {
                foreach (DataGridViewRow dr in gvwFundSource.Rows)
                {
                    if (dr.Cells["Selected"].Value.ToString().ToUpper() == "Y")
                    {
                        DatabaseLayer.ChldTrckDB.Update_chldtrack(dr.Cells["gvtCode"].Value.ToString().Trim(), GchartCode.Trim(), "Y");
                    }
                    else
                    {
                        DatabaseLayer.ChldTrckDB.Update_chldtrack(dr.Cells["gvtCode"].Value.ToString().Trim(), GchartCode.Trim(), "N");
                    }

                }
            }

            if (strTaskCount == string.Empty)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                bool boolPrimary = true;
                List<DataGridViewRow> SelectedgvRows = (from c in gvwFundSource.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (c.Cells["Selected"].Value.ToString().ToUpper().Equals("Y"))
                                                        select c).ToList();
                if (SelectedgvRows.Count > Convert.ToInt32(strTaskCount))
                {
                    AlertBox.Show("Only " + strTaskCount + " Task allowed.", MessageBoxIcon.Warning);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }
        }

        int Sel_Count = 0;
        private void gvsite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwFundSource.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gvwFundSource.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            gvwFundSource.CurrentRow.Cells["Sel_Img"].Value = Img_Blank;
                            gvwFundSource.CurrentRow.Cells["Selected"].Value = "N";
                            Sel_Count--;
                        }
                        else
                        {
                            gvwFundSource.CurrentRow.Cells["Sel_Img"].Value = Img_Tick;
                            gvwFundSource.CurrentRow.Cells["Selected"].Value = "Y";
                            Sel_Count++;
                        }
                        if (Sel_Count > 30 && FormCode == "HSSB2106")
                        {
                            gvwFundSource.CurrentRow.Cells["Sel_Img"].Value = Img_Blank;
                            gvwFundSource.CurrentRow.Cells["Selected"].Value = "N";
                            Sel_Count--;
                            AlertBox.Show("You may not select more than 30 services.", MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }


        private void fillGridFilterFunds(string strFilterType, string strDiplayFund)
        {
            gvwFundSource.Rows.Clear();
            List<CommonEntity> lookUpfundingResource = _model.lookupDataAccess.GetAgyFundsFilter(strFilterType, strDiplayFund);
            foreach (CommonEntity agyEntity in lookUpfundingResource)
            {
                bool Sel_Ref = false;
                int rowIndex = 0;
                foreach (CommonEntity SEntity in fundingList)
                {
                    if (SEntity.Code.Trim() == agyEntity.Code.Trim())
                    {
                        Sel_Ref = true;
                        rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                        selRow = rowIndex;
                        if (agyEntity.Active.ToString() == "N")
                            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        gvwFundSource.Rows[rowIndex].Tag = SEntity;
                    }
                }

                if (!Sel_Ref)
                {
                    rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                    if (agyEntity.Active.ToString() == "N")
                        gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    gvwFundSource.Rows[rowIndex].Tag = agyEntity;
                }
                row_Cnt++;
            }
            if (gvwFundSource.Rows.Count > 0)
            {
                gvwFundSource.Rows[selRow].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;
        }

        int row_Cnt = 0;
        private void fillGridFundings(string strDaycare)
        {
            gvwFundSource.Rows.Clear();
            List<CommonEntity> lookUpfundingResource = new List<CommonEntity>();


            if (StrFilter == "Y")
            {
                lookUpfundingResource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");
                lookUpfundingResource = filterByHIE(lookUpfundingResource, "VIEW");
            }
            else
                lookUpfundingResource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilterFunds(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");

            if (strDaycare == "DayCare")
            {
                lookUpfundingResource = lookUpfundingResource.FindAll(u => u.Extension != "Y");
                foreach (CommonEntity agyEntity in lookUpfundingResource)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;

                    foreach (CommonEntity SEntity in fundingList)
                    {
                        if (SEntity.Code.Trim() == agyEntity.Code.Trim())
                        {
                            //if (strDaycare == "DayCare")
                            //{
                            //    if (!agyEntity.Code.ToString().StartsWith("HS"))
                            //    {
                            //        Sel_Ref = true;
                            //        rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                            //        if (agyEntity.Active.ToString() == "N")
                            //            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            //        gvwFundSource.Rows[rowIndex].Tag = SEntity;
                            //    }
                            //}
                            //else
                            //{
                            Sel_Ref = true;
                            rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                            selRow = rowIndex;
                            if (agyEntity.Active.ToString() == "N")
                                gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            gvwFundSource.Rows[rowIndex].Tag = SEntity;
                            //}
                        }
                    }

                    if (!Sel_Ref)
                    {
                        //if (strDaycare == "DayCare")
                        //{
                        //    if (!agyEntity.Code.ToString().StartsWith("HS"))
                        //    {
                        //        rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                        //        if (agyEntity.Active.ToString() == "N")
                        //            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        //        gvwFundSource.Rows[rowIndex].Tag = agyEntity;
                        //    }
                        //}
                        //else
                        //{
                        rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                        if (agyEntity.Active.ToString() == "N")
                            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        gvwFundSource.Rows[rowIndex].Tag = agyEntity;
                        //}
                    }
                    row_Cnt++;

                }
            }
            else if (strDaycare == "Headstart")
            {
                lookUpfundingResource = lookUpfundingResource.FindAll(u => u.Extension.Equals("Y"));
                foreach (CommonEntity agyEntity in lookUpfundingResource)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;

                    foreach (CommonEntity SEntity in fundingList)
                    {
                        if (SEntity.Code.Trim() == agyEntity.Code.Trim())
                        {

                            Sel_Ref = true;
                            rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                            selRow = rowIndex;
                            if (agyEntity.Active.ToString() == "N")
                                gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            gvwFundSource.Rows[rowIndex].Tag = SEntity;
                        }
                    }

                    if (!Sel_Ref)
                    {
                        rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                        if (agyEntity.Active.ToString() == "N")
                            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        gvwFundSource.Rows[rowIndex].Tag = agyEntity;
                    }
                    row_Cnt++;

                }
            }
            else if (strDaycare == "TMSB0034" || strDaycare == "CASB0019" || strDaycare == "CASB0018" || strDaycare == "CEAPB002" || strDaycare == "CEAPB003")
            {

                foreach (string varitem in VarfundingList)
                {
                    CommonEntity agyEntity = lookUpfundingResource.Find(u => u.Code.Trim() == varitem.ToString().Trim());
                    if (agyEntity != null)
                    {
                        bool Sel_Ref = false;
                        int rowIndex = 0;

                        foreach (CommonEntity SEntity in fundingList)
                        {
                            if (SEntity.Code.Trim() == agyEntity.Code.Trim())
                            {

                                Sel_Ref = true;
                                rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                                selRow = rowIndex;
                                if (agyEntity.Active.ToString() == "N")
                                    gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                gvwFundSource.Rows[rowIndex].Tag = SEntity;

                            }
                        }

                        if (!Sel_Ref)
                        {

                            rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                            if (agyEntity.Active.ToString() == "N")
                                gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            gvwFundSource.Rows[rowIndex].Tag = agyEntity;

                        }
                        row_Cnt++;

                    }
                }
            }
            else
            {
                //lookUpfundingResource = lookUpfundingResource.FindAll(u => u.Extension != "Y");
                foreach (CommonEntity agyEntity in lookUpfundingResource)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;

                    foreach (CommonEntity SEntity in fundingList)
                    {
                        if (SEntity != null)
                        {
                            if (SEntity.Code.Trim() == agyEntity.Code.Trim())
                            {
                                //if (strDaycare == "DayCare")
                                //{
                                //    if (!agyEntity.Code.ToString().StartsWith("HS"))
                                //    {
                                //        Sel_Ref = true;
                                //        rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                                //        if (agyEntity.Active.ToString() == "N")
                                //            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                //        gvwFundSource.Rows[rowIndex].Tag = SEntity;
                                //    }
                                //}
                                //else
                                //{
                                Sel_Ref = true;
                                rowIndex = gvwFundSource.Rows.Add(Img_Tick, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "Y");
                                selRow = rowIndex;
                                if (agyEntity.Active.ToString() == "N")
                                    gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                gvwFundSource.Rows[rowIndex].Tag = SEntity;
                                //}
                            }
                        }
                    }

                    if (!Sel_Ref)
                    {
                        //if (strDaycare == "DayCare")
                        //{
                        //    if (!agyEntity.Code.ToString().StartsWith("HS"))
                        //    {
                        //        rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                        //        if (agyEntity.Active.ToString() == "N")
                        //            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        //        gvwFundSource.Rows[rowIndex].Tag = agyEntity;
                        //    }
                        //}
                        //else
                        //{
                        rowIndex = gvwFundSource.Rows.Add(Img_Blank, agyEntity.Code.Trim(), agyEntity.Desc.Trim(), "N");
                        if (agyEntity.Active.ToString() == "N")
                            gvwFundSource.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        gvwFundSource.Rows[rowIndex].Tag = agyEntity;
                        //}
                    }
                    row_Cnt++;

                }
            }
            if (gvwFundSource.Rows.Count > 0)
            {
                gvwFundSource.Rows[selRow].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;
        }

        private void FillGridEMSFunds()
        {
            gvwFundSource.Rows.Clear();
            List<CommonEntity> commonfundingsource = _model.lookupDataAccess.GetAgyFunds();
            //DataSet lookUpData = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FUNDCODS);
            //List<CommonEntity> commonfundingsource = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00501", Agency, Depart, Program);
            commonfundingsource = filterByHIE(commonfundingsource, string.Empty);
            //int rowIndex = 0;
            foreach (CommonEntity item in commonfundingsource)
            {
                bool Sel_Ref = false;
                int rowIndex = 0;
                foreach (CommonEntity SEntity in fundingList)
                {
                    if (SEntity.Code.Trim() == item.Code.Trim())
                    {
                        Sel_Ref = true;
                        rowIndex = gvwFundSource.Rows.Add(Img_Tick, item.Code.Trim(), item.Desc.Trim(), "Y");
                        selRow = rowIndex;
                        gvwFundSource.Rows[rowIndex].Tag = SEntity;
                    }
                }

                if (!Sel_Ref)
                {
                    rowIndex = gvwFundSource.Rows.Add(Img_Blank, item.Code.Trim(), item.Desc.Trim(), "N");
                    gvwFundSource.Rows[rowIndex].Tag = item;
                }
                row_Cnt++;

                //rowIndex = gvwFundSource.Rows.Add(Img_Blank, item.Code.Trim(), item.Desc.Trim(), "N");
                //gvwFundSource.Rows[rowIndex].Tag = item;
            }
            if (gvwFundSource.Rows.Count > 0)
            {
                gvwFundSource.Rows[selRow].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;

        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues, string Mode)
        {
            string HIE = Agency + Depart + Program;
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            //_AgytabsFilter = LookupValues;
            if (LookupValues.Count > 0)
            {
                int i = 0; bool Can_Continue = true;
                foreach (CommonEntity Entity in LookupValues)
                {
                    string Temp = Entity.Hierarchy.ToString().Trim();
                    Can_Continue = true; i = 0;
                    if (!string.IsNullOrEmpty(Temp.Trim()))
                    {
                        for (i = 0; Can_Continue;)
                        {
                            string TempCode = Temp.Substring(i, 6);

                            if (HIE == "******")
                                _AgytabsFilter.Add(Entity);
                            else if (Depart + Program == "****")
                            {
                                if (TempCode.Substring(0, 2).ToString().Trim() == Agency)
                                    _AgytabsFilter.Add(Entity);
                            }
                            else if (Program == "**")
                            {
                                if (TempCode.Substring(0, 4).ToString().Trim() == Agency + Depart)
                                    _AgytabsFilter.Add(Entity);
                            }
                            else
                            {
                                if (TempCode == HIE)
                                    _AgytabsFilter.Add(Entity);
                                else if (TempCode == "******")
                                    _AgytabsFilter.Add(Entity);
                                else if (TempCode.Substring(2, 4).ToString().Trim() == "****")
                                {
                                    if (TempCode.Substring(0, 2).ToString().Trim() == Agency)
                                        _AgytabsFilter.Add(Entity);
                                }
                                else if (TempCode.Substring(4, 2).ToString().Trim() == "**")
                                {
                                    if (TempCode.Substring(0, 4).ToString().Trim() == Agency + Depart)
                                        _AgytabsFilter.Add(Entity);
                                }

                            }


                            i += 6;
                            if (i >= Temp.Length)
                                Can_Continue = false;
                        }

                    }
                }
                //if (HIE == "******")
                //    _AgytabsFilter = _AgytabsFilter.ToList();
                //else if (Depart + Program == "****")
                //    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(Agency)).ToList();
                //else if (Program == "**")
                //    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(Agency + Depart)).ToList();
                //else
                //    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(HIE)).ToList();
                //                _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(Agency + Depart) || u.ListHierarchy.Contains(Agency) || u.ListHierarchy.Contains("******")).ToList();

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            return _AgytabsFilter;
        }

        public string ttlTaskCount = "";
        private void FillGridTasks()
        {
            gvwFundSource.Rows.Clear();
            if (Privileges.Program.ToUpper() == "HLS00134")
            {
                List<HlsTrckEntity> HlsTrack_Det = new List<HlsTrckEntity>();
                List<HlsTrckEntity> HlsTrackList = _model.HlsTrckData.Browse_HlstrckDetails("01");
                if (strTaskCount != string.Empty)
                {
                    HlsTrack_Det = _model.HlsTrckData.GetHlstrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
                }
                else
                {
                    if (ComponentCode == "0000")
                    {
                        if (!string.IsNullOrEmpty(GchartCode.Trim()))
                            HlsTrack_Det = HlsTrackList.FindAll(u => u.COMPONENT.Trim().Equals(ComponentCode) && u.GCHARTCODE.Trim().Equals(GchartCode.Trim()));
                        else
                            HlsTrack_Det = HlsTrackList.FindAll(u => u.COMPONENT.Trim().Equals(ComponentCode));
                    }
                    else
                        HlsTrack_Det = HlsTrackList.FindAll(u => u.COMPONENT.Trim().Equals(ComponentCode) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));
                }
                ttlTaskCount = HlsTrack_Det.Count.ToString();
                foreach (HlsTrckEntity Entity in HlsTrack_Det)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;

                    if (HlsTasksList.Count > 0)
                    {
                        foreach (HlsTrckEntity SEntity in HlsTasksList)
                        {
                            if (SEntity.TASK.Trim() == Entity.TASK.Trim())
                            {
                                if (!string.IsNullOrEmpty(GchartCode.Trim()))
                                {
                                    if (Entity.GCHARTSEL.Trim() == "Y")
                                    {
                                        Sel_Ref = true;
                                        rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "Y");
                                        selRow = rowIndex;
                                        gvwFundSource.Rows[rowIndex].Tag = SEntity;
                                    }
                                }
                                else
                                {
                                    Sel_Ref = true;
                                    rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "Y");
                                    selRow = rowIndex;
                                    gvwFundSource.Rows[rowIndex].Tag = SEntity;
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(GchartCode.Trim()))
                    {
                        if (Entity.GCHARTSEL.Trim() == "Y")
                        {
                            Sel_Ref = true;
                            rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "Y");
                            selRow = rowIndex;
                            gvwFundSource.Rows[rowIndex].Tag = Entity;
                        }
                    }

                    if (!Sel_Ref)
                    {
                        rowIndex = gvwFundSource.Rows.Add(Img_Blank, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "N");
                        gvwFundSource.Rows[rowIndex].Tag = Entity;
                    }
                    row_Cnt++;

                }

            }
            else
            {
                List<ChldTrckEntity> Track_Det = new List<ChldTrckEntity>();
                List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");
                if (strTaskCount != string.Empty)
                {
                    Track_Det = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
                }
                else
                {
                    if (ComponentCode == "0000")
                    {
                        if (!string.IsNullOrEmpty(GchartCode.Trim()))
                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(ComponentCode) && u.GCHARTCODE.Trim().Equals(GchartCode.Trim()));
                        else
                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(ComponentCode));
                    }
                    else
                        Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(ComponentCode) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));
                }
                ttlTaskCount = Track_Det.Count.ToString();
                foreach (ChldTrckEntity Entity in Track_Det)
                {
                    bool Sel_Ref = false;
                    int rowIndex = 0;

                    if (TasksList.Count > 0)
                    {
                        foreach (ChldTrckEntity SEntity in TasksList)
                        {
                            if (SEntity.TASK.Trim() == Entity.TASK.Trim())
                            {
                                if (!string.IsNullOrEmpty(GchartCode.Trim()))
                                {
                                    if (Entity.GCHARTSEL.Trim() == "Y")
                                    {
                                        Sel_Ref = true;
                                        rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "Y");
                                        selRow = rowIndex;
                                        gvwFundSource.Rows[rowIndex].Tag = SEntity;
                                    }
                                }
                                else
                                {
                                    Sel_Ref = true;
                                    rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "Y");
                                    selRow = rowIndex;
                                    gvwFundSource.Rows[rowIndex].Tag = SEntity;
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(GchartCode.Trim()))
                    {
                        if (Entity.GCHARTSEL.Trim() == "Y")
                        {
                            Sel_Ref = true;
                            rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "Y");
                            selRow = rowIndex;
                            gvwFundSource.Rows[rowIndex].Tag = Entity;
                        }
                    }

                    if (!Sel_Ref)
                    {
                        rowIndex = gvwFundSource.Rows.Add(Img_Blank, Entity.TASK.Trim(), Entity.TASKDESCRIPTION.Trim(), "N");
                        gvwFundSource.Rows[rowIndex].Tag = Entity;
                    }
                    row_Cnt++;

                }
            }
            if (gvwFundSource.Rows.Count > 0)
            {
                gvwFundSource.Rows[selRow].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;
        }

        private void FillGridSALdefs()
        {
            gvwFundSource.Rows.Clear();

            SaldefEntity Search_saldef_Entity = new SaldefEntity(true);

            List<SaldefEntity> SALDEF = _model.SALDEFData.Browse_SALDEF(Search_saldef_Entity, "Browse", BaseForm.UserID, BaseForm.BaseAdminAgency); //**Vikash added
            if (SALDEF.Count > 0)
            {
                if (FormCode == "SAL")
                {
                    if (Agency + Depart + Program == "******")
                        SALDEFEntity = SALDEF.FindAll(u => u.SALD_TYPE.Equals("S"));
                    else
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Agency + Depart + Program) || u.SALD_HIE.Contains(Agency + Depart + "**") || u.SALD_HIE.Contains(Agency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_TYPE.Equals("S"));
                }
                else if (FormCode == "CAL")
                {
                    if (Agency + Depart + Program == "******")
                        SALDEFEntity = SALDEF.FindAll(u => u.SALD_TYPE.Equals("C"));
                    else
                        SALDEFEntity = SALDEF.FindAll(u => (u.SALD_HIE.Contains(Agency + Depart + Program) || u.SALD_HIE.Contains(Agency + Depart + "**") || u.SALD_HIE.Contains(Agency + "****") || u.SALD_HIE.Contains("******")) && u.SALD_TYPE.Equals("C"));
                }

                if (SALDEFEntity.Count > 0)
                {
                    foreach (SaldefEntity Entity in SALDEFEntity)
                    {
                        bool Sel_Ref = false;
                        int rowIndex = 0;
                        foreach (SaldefEntity SEntity in SalDefList)
                        {
                            if (SEntity.SALD_ID.Trim() == Entity.SALD_ID.Trim())
                            {
                                Sel_Ref = true;
                                rowIndex = gvwFundSource.Rows.Add(Img_Tick, Entity.SALD_ID, Entity.SALD_NAME.Trim(), "Y");
                                selRow = rowIndex;
                                gvwFundSource.Rows[rowIndex].Tag = SEntity;
                            }
                        }
                        if (!Sel_Ref)
                        {
                            rowIndex = gvwFundSource.Rows.Add(Img_Blank, Entity.SALD_ID, Entity.SALD_NAME.Trim(), "N");
                            gvwFundSource.Rows[rowIndex].Tag = Entity;
                        }
                        row_Cnt++;
                    }
                }


            }
            if (gvwFundSource.Rows.Count > 0)
            {
                gvwFundSource.Rows[selRow].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;
        }

        private void FillBudgets()
        {
            gvwFundSource.Rows.Clear();

            List<CMBDCEntity> Bdc_List = new List<CMBDCEntity>();
            List<CMBDCEntity> MainBdc_List = _model.SPAdminData.GetCMBdcAllData(Agency, Depart, Program, StrYear, string.Empty, string.Empty);
            if (MainBdc_List.Count > 0)
            {
                if (fundingList.Count > 0)
                {
                    foreach (CommonEntity FEntiy in fundingList)
                    {
                        List<CMBDCEntity> SelFund_BDC = MainBdc_List.FindAll(u => u.BDC_FUND.Trim().Equals(FEntiy.Code.Trim()));
                        if (SelFund_BDC.Count > 0)
                        {
                            SelFund_BDC = SelFund_BDC.FindAll(u => u.BDC_FUND.Trim().Equals(FEntiy.Code.Trim()) && ((Convert.ToDateTime(FDate.Trim()) >= Convert.ToDateTime(u.BDC_START) && Convert.ToDateTime(FDate.Trim()) <= Convert.ToDateTime(u.BDC_END))
                                                                                                      || (Convert.ToDateTime(u.BDC_START) >= Convert.ToDateTime(FDate.Trim()) && Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(LDate.Trim()))));

                            if (SelFund_BDC.Count > 0)
                            {
                                Bdc_List.AddRange(SelFund_BDC);
                            }
                        }
                    }

                   
                }
                else
                {
                    if(!string.IsNullOrEmpty(FundType.Trim()))
                        MainBdc_List= MainBdc_List.FindAll(u => u.BDC_FUND.Trim().Contains(FundType.Trim()));
                    if (MainBdc_List.Count > 0)
                    {
                        MainBdc_List = MainBdc_List.FindAll(u => ((Convert.ToDateTime(FDate.Trim()) >= Convert.ToDateTime(u.BDC_START) && Convert.ToDateTime(FDate.Trim()) <= Convert.ToDateTime(u.BDC_END))
                                                                                                  || (Convert.ToDateTime(u.BDC_START) >= Convert.ToDateTime(FDate.Trim()) && Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(LDate.Trim()))));

                        if (MainBdc_List.Count > 0)
                        {
                            Bdc_List.AddRange(MainBdc_List);
                        }
                    }
                }

                if (Bdc_List.Count > 0)
                {

                    foreach (CMBDCEntity entity in Bdc_List)
                    {
                        int rowIndex = 0; bool Sel_Ref = false;
                        foreach (CMBDCEntity SEntity in SelBudgetList)
                        {
                            if (SEntity.BDC_ID.Trim() == entity.BDC_ID.Trim())
                            {
                                Sel_Ref = true;
                                rowIndex = gvwFundSource.Rows.Add(Img_Tick, entity.BDC_ID.Trim(), entity.BDC_DESCRIPTION.Trim(), "Y");
                                selRow = rowIndex;
                                gvwFundSource.Rows[rowIndex].Tag = SEntity;
                            }
                        }

                        if (!Sel_Ref)
                        {
                            rowIndex = gvwFundSource.Rows.Add(Img_Blank, entity.BDC_ID.Trim(), entity.BDC_DESCRIPTION.Trim(), "N");
                            gvwFundSource.Rows[rowIndex].Tag = entity;
                        }
                        row_Cnt++;
                    }
                }

                //if (!string.IsNullOrEmpty(StrFund.Trim()))
                //{
                //    Bdc_List = Bdc_List.FindAll(u => u.BDC_FUND.Trim().Equals(StrFund.Trim()));

                //    if (Bdc_List.Count > 0)
                //    {
                //        if (!string.IsNullOrEmpty(FDate.Trim()) && !string.IsNullOrEmpty(LDate.Trim()))
                //        {
                //            Bdc_List = Bdc_List.FindAll(u => u.BDC_FUND.Trim().Equals(StrFund.Trim()) && ((Convert.ToDateTime(FDate.Trim()) >= Convert.ToDateTime(u.BDC_START) && Convert.ToDateTime(FDate.Trim()) <= Convert.ToDateTime(u.BDC_END))
                //                                                                                      || (Convert.ToDateTime(u.BDC_START) >= Convert.ToDateTime(FDate.Trim()) && Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(LDate.Trim()))));

                //            //Bdc_List = Bdc_List.FindAll(u => u.BDC_FUND.Trim().Equals(StrFund.Trim()) && ((Convert.ToDateTime(FDate.Trim()) >= Convert.ToDateTime(u.BDC_START) && Convert.ToDateTime(FDate.Trim()) <= Convert.ToDateTime(FDate.Trim()))
                //            //                                                                          && (Convert.ToDateTime(FDate.Trim()) >= Convert.ToDateTime(FDate.Trim()) && Convert.ToDateTime(FDate.Trim()) <= Convert.ToDateTime(u.BDC_END)))
                //            //                                                                          || ((Convert.ToDateTime(LDate.Trim()) >= Convert.ToDateTime(u.BDC_START) && Convert.ToDateTime(LDate.Trim()) <= Convert.ToDateTime(LDate.Trim()))
                //            //                                                                          && (Convert.ToDateTime(LDate.Trim()) >= Convert.ToDateTime(LDate.Trim()) && Convert.ToDateTime(LDate.Trim()) <= Convert.ToDateTime(u.BDC_END))));


                //            //Bdc_List = Bdc_List.FindAll(u => u.BDC_FUND.Trim().Equals(StrFund.Trim()) && ((Convert.ToDateTime(u.BDC_START) >= Convert.ToDateTime(FDate.Trim()) && Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(u.BDC_START))
                //            //                                                                          && (Convert.ToDateTime(u.BDC_START) >= Convert.ToDateTime(u.BDC_START) && Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(LDate.Trim())))
                //            //                                                                          || ((Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(FDate.Trim()) && Convert.ToDateTime(u.BDC_END) <= Convert.ToDateTime(u.BDC_END))
                //            //                                                                          && (Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(u.BDC_END) && Convert.ToDateTime(u.BDC_END) <= Convert.ToDateTime(LDate.Trim()))));
                //        }
                //    }

                //}

                //if (Bdc_List.Count > 0)
                //{

                //    foreach (CMBDCEntity entity in Bdc_List)
                //    {
                //        int rowIndex = 0; bool Sel_Ref = false;
                //        foreach (CMBDCEntity SEntity in SelBudgetList)
                //        {
                //            if (SEntity.BDC_ID.Trim() == entity.BDC_ID.Trim())
                //            {
                //                Sel_Ref = true;
                //                rowIndex = gvwFundSource.Rows.Add(Img_Tick, entity.BDC_ID.Trim(), entity.BDC_DESCRIPTION.Trim(), "Y");
                //                selRow = rowIndex;
                //                gvwFundSource.Rows[rowIndex].Tag = SEntity;
                //            }
                //        }

                //        if (!Sel_Ref)
                //        {
                //            rowIndex = gvwFundSource.Rows.Add(Img_Blank, entity.BDC_ID.Trim(), entity.BDC_DESCRIPTION.Trim(), "N");
                //            gvwFundSource.Rows[rowIndex].Tag = entity;
                //        }
                //        row_Cnt++;
                //    }
                //}

            }
            if (gvwFundSource.Rows.Count > 0)
            {
                gvwFundSource.Rows[selRow].Selected = true;
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = true;
            }
            else
                btnSelect.Visible = btnCancel.Visible = chkSelectAll.Visible = chkUnselectAll.Visible = false;

        }

        public List<CommonEntity> GetSelectedFundings()
        {
            List<CommonEntity> sele_Rooms_List = new List<CommonEntity>();
            foreach (DataGridViewRow dr in gvwFundSource.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    CommonEntity Add_Entity = dr.Tag as CommonEntity;
                    sele_Rooms_List.Add(Add_Entity);
                }
            }
            return sele_Rooms_List;
        }

        public List<ChldTrckEntity> GetSelectedTracks()
        {
            List<ChldTrckEntity> sele_Rooms_List = new List<ChldTrckEntity>();
            foreach (DataGridViewRow dr in gvwFundSource.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    ChldTrckEntity Add_Entity = dr.Tag as ChldTrckEntity;
                    sele_Rooms_List.Add(Add_Entity);
                }
            }
            return sele_Rooms_List;
        }

        public List<HlsTrckEntity> GetSelectedHlsTracks()
        {
            List<HlsTrckEntity> sele_Rooms_List = new List<HlsTrckEntity>();
            foreach (DataGridViewRow dr in gvwFundSource.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    HlsTrckEntity Add_Entity = dr.Tag as HlsTrckEntity;
                    sele_Rooms_List.Add(Add_Entity);
                }
            }
            return sele_Rooms_List;
        }

        public List<SaldefEntity> GetSelectedSALs()
        {
            List<SaldefEntity> sele_SALs_List = new List<SaldefEntity>();
            foreach (DataGridViewRow dr in gvwFundSource.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    SaldefEntity Add_Entity = dr.Tag as SaldefEntity;
                    sele_SALs_List.Add(Add_Entity);
                }
            }
            return sele_SALs_List;
        }

        public List<CMBDCEntity> GetSelectedBudget()
        {
            List<CMBDCEntity> sele_Rooms_List = new List<CMBDCEntity>();
            foreach (DataGridViewRow dr in gvwFundSource.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    CMBDCEntity Add_Entity = dr.Tag as CMBDCEntity;
                    sele_Rooms_List.Add(Add_Entity);
                }
            }
            return sele_Rooms_List;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkUnselectAll.Checked = false;
                foreach (DataGridViewRow item in gvwFundSource.Rows)
                {
                    item.Cells["Sel_Img"].Value = Img_Tick;
                        item.Cells["Selected"].Value = "Y";
                }
            
            }
            else if (chkSelectAll.Checked == false)
            {
                foreach (DataGridViewRow item in gvwFundSource.Rows)
                {
                    item.Cells["Sel_Img"].Value = Img_Blank;
                    item.Cells["Selected"].Value = "N";
                }
            }
        }

        private void chkUnselectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnselectAll.Checked)
            {
                chkSelectAll.Checked = false;
                foreach (DataGridViewRow item in gvwFundSource.Rows)
                {
                    item.Cells["Sel_Img"].Value = Img_Blank;
                    item.Cells["Selected"].Value = "N";
                }
            }
        }

        private void gvwFundSource_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}