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
using System.Xml;
using System.IO;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using DevExpress.Utils.Win.Hook;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MATB0003_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public MATB0003_Form(BaseForm baseform, PrivilegeEntity previliges)
        {
            InitializeComponent();
            Selected_Scales_List = new List<MATDEFEntity>();

            Privileges = previliges;
            BaseForm = baseform;
            this.Text = /*Privileges.Program + " - " +*/ Privileges.PrivilegeName;
            _model = new CaptainModel();

            //if (BaseForm.UserProfile.Security.Trim() == "P" || BaseForm.UserProfile.Security.Trim() == "B")
            //{
            //    btnChangeDates.Enabled = true;
            //    btnChangeDates.Visible = true;
            //}

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            //this.Site_Panel.Location = new System.Drawing.Point(58, 210);

            Agency = BaseForm.BaseAgency;
            Depart = BaseForm.BaseDept;
            Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            ReportPath = _model.lookupDataAccess.GetReportPath();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propGroup_List = _model.MatrixScalesData.GetSCLGRPS(string.Empty, string.Empty);
            Fill_Matrix_Combo();
            Fill_CaseWorker_Combo();
            //FillAssessment_Types();
            //if (BaseForm.BaseAgencyControlDetails != null)
            //{
            //    if (BaseForm.BaseAgencyControlDetails.MatAssesment.ToUpper() == "Y")
            //    {
            //        cmbAssessmentType.Visible = true;
            //        lblAssType.Visible = true;
            //        CommonFunctions.SetComboBoxValue(cmbAssessmentType, "H");
            //    }
            //}
        }

        
        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<MATDEFEntity> Selected_Scales_List { get; set; }
        List<MATGroupEntity> propGroup_List { get; set; }

        public string ReportPath { get; set; }

        //public List<HierarchyEntity> SelectedHierarchies
        //{
        //    get
        //    {
        //        return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
        //                                       select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
        //    }
        //}


        #endregion

        private void Fill_CaseWorker_Combo()
        {
            Cmb_Worker.Items.Clear();
            Cmb_Worker.ColorMember = "FavoriteColor";
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            //CmbWorker.Items.Insert(0, new ListItem("All", "**"));
            //listItem.Add(new Captain.Common.Utilities.ListItem("All", "**", " ", Color.White));
            listItem.Add(new Captain.Common.Utilities.ListItem("All", "**"));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(BaseForm.BaseHierarchyCwFormat, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        //if ((Mode == "Add" && dr["PWH_INACTIVE"].ToString().Trim() == "N") || (Mode == "Edit"))
                        //listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Green));

                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString().Trim(), dr["PWH_INACTIVE"].ToString().Trim().Equals("Y") ? Color.Red : Color.Black));
                    }
                }
            }
            Cmb_Worker.Items.AddRange(listItem.ToArray());
            Cmb_Worker.SelectedIndex = 0;

        }

        private void Fill_Matrix_Combo()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            Cmb_Matrix.Items.Clear();
            Cmb_Matrix.ColorMember = "FavoriteColor";
            matdefEntity = matdefEntity.OrderBy(u => u.Active).ToList();
            if (matdefEntity.Count > 0)
            {
                foreach (MATDEFEntity matdef in matdefEntity)
                    Cmb_Matrix.Items.Add(new Captain.Common.Utilities.ListItem(matdef.Desc, matdef.Mat_Code, matdef.Interval, matdef.Show_BA, matdef.Date_option, /*string.Empty,*/ matdef.Active.ToString(), matdef.Active.Equals("A") ? Color.Black : Color.Red));
            }
            else
                Cmb_Matrix.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0"));

            Cmb_Matrix.SelectedIndex = 0;
        }

        private bool Validate_Report()
        {
            bool Can_Generate = true;
            _errorProvider.SetError(Cmb_Matrix, null);
            _errorProvider.SetError(Rb_Sel_Scales, null);

            //if (Cb_Enroll.Checked)
            //{
            //    if (!Asof_From_Date.Checked)
            //    {
            //        _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Asof From Date".Replace(Consts.Common.Colon, string.Empty)));
            //        Can_Generate = false;
            //    }
            //    else
            //        _errorProvider.SetError(Asof_From_Date, null);


            //    if (!Asof_To_Date.Checked)
            //    {
            //        _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Asof To Date".Replace(Consts.Common.Colon, string.Empty)));
            //        Can_Generate = false;
            //    }
            //    else
            //        _errorProvider.SetError(Asof_To_Date, null);

            //    if (Asof_From_Date.Checked && Asof_To_Date.Checked)
            //    {

            //        if (Asof_From_Date.Value > Asof_To_Date.Value)
            //        {
            //            _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Asof From Date should not be greater than To Date".Replace(Consts.Common.Colon, string.Empty)));
            //            Can_Generate = false;
            //        }
            //        else
            //            _errorProvider.SetError(Asof_To_Date, null);
            //    }

            //}

            if ((string.IsNullOrEmpty(((ListItem)Cmb_Matrix.SelectedItem).Text.Trim())))
            {
                _errorProvider.SetError(Cmb_Matrix, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Cmb_Matrix, null);

            if (Rb_Sel_Scales.Checked)
            {
                if (Selected_Scales_List.Count == 0)
                {
                    _errorProvider.SetError(Rb_Sel_Scales, string.Format("Please Select at least One Scale".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_Sel_Scales, null);
            }
            else
            {
                if (Rb_All_Scales.Checked)
                    _errorProvider.SetError(Rb_Sel_Scales, null);
            }

            //if (!Base_FDate.Checked)
            //{
            //    _errorProvider.SetError(Base_FDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine From Date ".Replace(Consts.Common.Colon, string.Empty)));
            //    Can_Generate = false;
            //}
            //else
            //    _errorProvider.SetError(Base_FDate, null);


            //if (!Base_TDate.Checked)
            //{
            //    _errorProvider.SetError(Base_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine To Date ".Replace(Consts.Common.Colon, string.Empty)));
            //    Can_Generate = false;
            //}
            //else
            //    _errorProvider.SetError(Base_TDate, null);

            if (!Asmt_F_Date.Checked)
            {
                _errorProvider.SetError(Asmt_F_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment From Date ".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Asmt_F_Date, null);

            if (!Asmt_T_Date.Checked)
            {
                _errorProvider.SetError(Asmt_T_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment From Date ".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Asmt_T_Date, null);


            //if (Base_FDate.Checked && Base_TDate.Checked)
            //{
            //    if (Base_FDate.Value > Base_TDate.Value)
            //    {
            //        _errorProvider.SetError(Base_TDate, string.Format("BaseLine 'From Date' should not be prior to 'TO Date'".Replace(Consts.Common.Colon, string.Empty)));
            //        Can_Generate = false;
            //    }
            //}
            //else
            //{
            //    if (Base_FDate.Checked && Base_TDate.Checked)
            //        _errorProvider.SetError(Base_TDate, null);
            //}
            #region CheckPoint 1 Date validation

            if (Asmt_F_Date.Checked == false && Asmt_T_Date.Checked == false)
            {
                _errorProvider.SetError(Asmt_T_Date, string.Format("Please select Checkpoint #1 From and To Dates"));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Asmt_T_Date, null);

            if (Asmt_F_Date.Checked == true || Asmt_T_Date.Checked == true)
            {
                if (Asmt_F_Date.Checked == false && Asmt_T_Date.Checked == true)
                {
                    _errorProvider.SetError(Asmt_F_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #1 From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Asmt_F_Date, null);
                }
            }

            if (Asmt_F_Date.Checked == true || Asmt_T_Date.Checked == true)
            {
                if (Asmt_T_Date.Checked == false && Asmt_F_Date.Checked == true)
                {
                    _errorProvider.SetError(Asmt_T_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #1 To Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Asmt_T_Date, null);
                }
            }
            if (Asmt_F_Date.Checked.Equals(true) && Asmt_T_Date.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(Asmt_F_Date.Text))
                {
                    _errorProvider.SetError(Asmt_F_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #1 From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Asmt_F_Date, null);
                }
                if (string.IsNullOrWhiteSpace(Asmt_T_Date.Text))
                {
                    _errorProvider.SetError(Asmt_T_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #1 To Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Asmt_T_Date, null);
                }
            }

            if (Asmt_F_Date.Checked && Asmt_T_Date.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(Asmt_F_Date.Text) && !string.IsNullOrWhiteSpace(Asmt_T_Date.Text))
                {
                    if (Asmt_F_Date.Value.Date > Asmt_T_Date.Value.Date)
                    {
                        _errorProvider.SetError(Asmt_T_Date, "Checkpoint #1 'From Date' should be prior or equal to 'To Date'");
                        Can_Generate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(Asmt_T_Date, null);
                    }
                }
            }
            #endregion

            #region CheckPoint 2 Date Validation

            if (dtpCP2From.Checked == false && dtpCP2To.Checked == false)
            {
                _errorProvider.SetError(dtpCP2To, string.Format("Please select Checkpoint #2 From and To Dates"));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(dtpCP2To, null);

            if (dtpCP2From.Checked == true || dtpCP2To.Checked == true)
            {
                if (dtpCP2From.Checked == false && dtpCP2To.Checked == true)
                {
                    _errorProvider.SetError(dtpCP2From, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #2 From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP2From, null);
                }
            }

            if (dtpCP2From.Checked == true || dtpCP2To.Checked == true)
            {
                if (dtpCP2To.Checked == false && dtpCP2From.Checked == true)
                {
                    _errorProvider.SetError(dtpCP2To, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #2 To Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP2To, null);
                }
            }
            if (dtpCP2From.Checked.Equals(true) && dtpCP2To.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpCP2From.Text))
                {
                    _errorProvider.SetError(dtpCP2From, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #2 From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP2From, null);
                }
                if (string.IsNullOrWhiteSpace(dtpCP2To.Text))
                {
                    _errorProvider.SetError(dtpCP2To, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #2 To Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP2To, null);
                }
            }

            if (dtpCP2From.Checked && dtpCP2To.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(dtpCP2From.Text) && !string.IsNullOrWhiteSpace(dtpCP2To.Text))
                {
                    if (dtpCP2From.Value.Date > dtpCP2To.Value.Date)
                    {
                        _errorProvider.SetError(dtpCP2To, "Checkpoint #2 'From Date' should be prior or equal to 'To Date'");
                        Can_Generate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpCP2To, null);
                    }
                }
            }
            #endregion

            #region CheckPoint 3 Date Validation

            if (dtpCP3From.Checked == false && dtpCP3To.Checked == false)
            {
                _errorProvider.SetError(dtpCP3To, string.Format("Please select Checkpoint #3 From and To Dates"));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(dtpCP3To, null);

            if (dtpCP3From.Checked == true || dtpCP3To.Checked == true)
            {
                if (dtpCP3From.Checked == false && dtpCP3To.Checked == true)
                {
                    _errorProvider.SetError(dtpCP3From, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #3 From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP3From, null);
                }
            }

            if (dtpCP3From.Checked == true || dtpCP3To.Checked == true)
            {
                if (dtpCP3To.Checked == false && dtpCP3From.Checked == true)
                {
                    _errorProvider.SetError(dtpCP3To, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #3 To Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP3To, null);
                }
            }
            if (dtpCP3From.Checked.Equals(true) && dtpCP3To.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpCP3From.Text))
                {
                    _errorProvider.SetError(dtpCP3From, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #3 From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP3From, null);
                }
                if (string.IsNullOrWhiteSpace(dtpCP3To.Text))
                {
                    _errorProvider.SetError(dtpCP3To, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Checkpoint #3 To Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpCP3To, null);
                }
            }

            if (dtpCP3From.Checked && dtpCP3To.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(dtpCP3From.Text) && !string.IsNullOrWhiteSpace(dtpCP3To.Text))
                {
                    if (dtpCP3From.Value.Date > dtpCP3To.Value.Date)
                    {
                        _errorProvider.SetError(dtpCP3To, "Checkpoint #3 'From Date' should be prior or equal to 'To Date'");
                        Can_Generate = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpCP3To, null);
                    }
                }
            }
            #endregion

            /*if (Asmt_F_Date.Checked && Asmt_T_Date.Checked)
            {
                if (Asmt_F_Date.Value > Asmt_T_Date.Value)
                {
                    _errorProvider.SetError(Asmt_T_Date, string.Format("Checkpoint #1 'From Date' should be prior to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (Asmt_F_Date.Checked && Asmt_T_Date.Checked)
                    _errorProvider.SetError(Asmt_T_Date, null);
            }

            if (dtpCP2From.Checked && dtpCP2To.Checked)
            {
                if (dtpCP2From.Value > dtpCP2To.Value)
                {
                    _errorProvider.SetError(dtpCP2To, string.Format("Checkpoint #2 'From Date' should be prior to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (dtpCP2From.Checked && dtpCP2To.Checked)
                    _errorProvider.SetError(dtpCP2To, null);
            }

            if (dtpCP3From.Checked && dtpCP3To.Checked)
            {
                if (dtpCP3From.Value > dtpCP3To.Value)
                {
                    _errorProvider.SetError(dtpCP3To, string.Format("Checkpoint #3 'From Date' should be prior to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (dtpCP3From.Checked && dtpCP3To.Checked)
                    _errorProvider.SetError(dtpCP3To, null);
            }
            */
            //_errorProvider.SetError(Asof_From_Date, null);
            //_errorProvider.SetError(Pb_Prog, null);
            //_errorProvider.SetError(Pb_Withdraw_Enroll, null);
            //if (Cb_Enroll.Checked)
            //{
            //    if (!Asof_From_Date.Checked)
            //    {
            //        _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), Lbl_Asof_Date.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        Can_Generate = false;
            //    }
            //    else
            //        _errorProvider.SetError(Asof_From_Date, null);

            //    if (Privileges.ModuleCode == "03")
            //    {
            //        if (string.IsNullOrEmpty(Txt_Program.Text.Trim()))
            //        {
            //            _errorProvider.SetError(Pb_Prog, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text.Replace(Consts.Common.Colon, string.Empty)));
            //            Can_Generate = false;
            //        }
            //        else
            //            _errorProvider.SetError(Pb_Prog, null);
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text.Trim()))
            //        {
            //            _errorProvider.SetError(Pb_Withdraw_Enroll, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Site".Replace(Consts.Common.Colon, string.Empty)));
            //            Can_Generate = false;
            //        }
            //        else
            //            _errorProvider.SetError(Pb_Withdraw_Enroll, null);
            //    }
            //}


            return Can_Generate;
        }


        string propReportPath = ""; List<MATB0003Entity> MatRepList = new List<MATB0003Entity>(); DataSet dsMatb = new DataSet();
        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            if (Validate_Report())
            {
                //MatRepList = _model.AdhocData.MAT0003_MatAssessments_report(Get_Sql_Parametets_List());
                MatRepList = new List<MATB0003Entity>();
                string ScalesList=string.Empty;
                if (Rb_Sel_Scales.Checked)
                    ScalesList= Get_Sel_Sites();

                string Worker=string.Empty;
                if(((ListItem)Cmb_Worker.SelectedItem).Value.ToString()!="**")
                    Worker=((ListItem)Cmb_Worker.SelectedItem).Value.ToString();
                string AssDateFrom1 = string.Empty, AssDateFrom2 = string.Empty,AssDateFrom3 = string.Empty;
                string AssDateTo1 = string.Empty, AssDateTo2 = string.Empty, AssDateTo3 = string.Empty;
                if (Asmt_F_Date.Checked) AssDateFrom1 = Asmt_F_Date.Text.ToString(); if (Asmt_T_Date.Checked) AssDateTo1 = Asmt_T_Date.Text.ToString();
                if (dtpCP2From.Checked) AssDateFrom2 = dtpCP2From.Text.ToString(); if (dtpCP2To.Checked) AssDateTo2 = dtpCP2To.Text.ToString();
                if (dtpCP3From.Checked) AssDateFrom3 = dtpCP3From.Text.ToString(); if (dtpCP3To.Checked) AssDateTo3 = dtpCP3To.Text.ToString();


                dsMatb = DatabaseLayer.SPAdminDB.MATB0003_Report(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    "), ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString(), ScalesList, AssDateFrom1.ToString(), AssDateTo1.ToString(), AssDateFrom2.ToString(), AssDateTo2.ToString(), AssDateFrom3.ToString(), AssDateTo3.ToString(), Worker, Privileges.ModuleCode, BaseForm.UserID);
                //dsMatb = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(Get_Sql_Parametets_List(), "dbo.MATB0003_REPORT");

                if (dsMatb.Tables.Count > 0)
                {
                    if (dsMatb.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsMatb.Tables[0].Rows)
                            MatRepList.Add(new MATB0003Entity(row, string.Empty));
                    }
                }

                if (MatRepList.Count > 0)
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"XLS");
                    pdfListForm.FormClosed += new FormClosedEventHandler(OnExcel_Report);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
                else
                {
                    AlertBox.Show("No Records Found", MessageBoxIcon.Warning);
                }
            }
        }

        private string Get_Sel_Sites()
        {
            string Sel_Scales_Codes = string.Empty;

            foreach (MATDEFEntity Entity in Selected_Scales_List)
            {
                Sel_Scales_Codes += "'" + Entity.Scale_Code + "' ,";
            }

            if (Sel_Scales_Codes.Length > 0)
                Sel_Scales_Codes = Sel_Scales_Codes.Substring(0, (Sel_Scales_Codes.Length - 1));

            return Sel_Scales_Codes;
        }

        private List<SqlParameter> Get_Sql_Parametets_List()
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@Agency", Current_Hierarchy.Substring(0, 2)));
            sqlParamList.Add(new SqlParameter("@Dept", Current_Hierarchy.Substring(2, 2)));
            sqlParamList.Add(new SqlParameter("@Prog", Current_Hierarchy.Substring(4, 2)));
            sqlParamList.Add(new SqlParameter("@Year", (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    ")));
            //sqlParamList.Add(new SqlParameter("@App", Entity.Tabs_Type));
            //sqlParamList.Add(new SqlParameter("@Asmt_Type", (Rb_Asmt_FDate.Checked ? "S" : "L")));
            //sqlParamList.Add(new SqlParameter("@Base_From_Date", Base_FDate.Value.ToShortDateString()));
            //sqlParamList.Add(new SqlParameter("@Base_To_Date", Base_TDate.Value.ToShortDateString()));
            sqlParamList.Add(new SqlParameter("@Asmt_From_Date", Convert.ToDateTime(Asmt_F_Date.Value.ToShortDateString())));
            sqlParamList.Add(new SqlParameter("@Asmt_To_Date", Convert.ToDateTime(Asmt_T_Date.Value.ToShortDateString())));
            sqlParamList.Add(new SqlParameter("@MATCODE", ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString()));
            //sqlParamList.Add(new SqlParameter("@BaseLine_Date_Sel_SW", (Rb_Asmt_TDate.Checked ? "L" : "F")));

            if (((ListItem)Cmb_Worker.SelectedItem).Value.ToString() != "**")
                sqlParamList.Add(new SqlParameter("@CaseWorker", ((ListItem)Cmb_Worker.SelectedItem).Value.ToString()));

            //sqlParamList.Add(new SqlParameter("@Include_Enrl", (Cb_Enroll.Checked ? "Y" : "N")));
            //if (Cb_Enroll.Checked)
            //{
            //    sqlParamList.Add(new SqlParameter("@Enrl_Asof_Date", Asof_From_Date.Value.ToShortDateString()));
            //    sqlParamList.Add(new SqlParameter("@Enrl_Asof_Status", ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString()));
            //    //sqlParamList.Add(new SqlParameter("@Enrl_Prog", Txt_Program.Text.Substring(0, 6)));

            //    sqlParamList.Add(new SqlParameter("@Module_Code", Privileges.ModuleCode.Trim()));
            //    if (Privileges.ModuleCode == "03")
            //        sqlParamList.Add(new SqlParameter("@Enrl_Prog", Txt_Program.Text.Trim()));
            //    else if (Privileges.ModuleCode == "02")
            //    {
            //        sqlParamList.Add(new SqlParameter("@Asof_Site", Txt_DrawEnroll_Site.Text.Trim()));
            //        if (!string.IsNullOrEmpty(Txt_DrawEnroll_Room.Text.Trim()))
            //            sqlParamList.Add(new SqlParameter("@Asof_Romm", Txt_DrawEnroll_Room.Text.Trim()));
            //        if (!string.IsNullOrEmpty(Txt_DrawEnroll_AMPM.Text.Trim()))
            //            sqlParamList.Add(new SqlParameter("@Asof_AMPM", Txt_DrawEnroll_AMPM.Text.Trim()));
            //    }


            //    if (Asof_From_Date.Checked)
            //        sqlParamList.Add(new SqlParameter("@Asof_From_Date", Asof_From_Date.Value.ToShortDateString()));

            //    if (Asof_To_Date.Checked)
            //        sqlParamList.Add(new SqlParameter("@Asof_TO_Date", Asof_To_Date.Value.ToShortDateString()));
            //}

            if (Rb_Sel_Scales.Checked)
                sqlParamList.Add(new SqlParameter("@Scales_List", Get_Sel_Sites()));

            //if (Cb_Only_Asmt_Scale.Checked)
            //    sqlParamList.Add(new SqlParameter("@Only_Assed_Scales", "Y"));
            //if (cmbAssessmentType.Visible == true)
            //{
            //    sqlParamList.Add(new SqlParameter("@Assessment_Type", (((ListItem)cmbAssessmentType.SelectedItem).Value.ToString())));
            //}

            sqlParamList.Add(new SqlParameter("@Module_Code", Privileges.ModuleCode));
            sqlParamList.Add(new SqlParameter("@UserName", BaseForm.UserID));


            return sqlParamList;
        }


        private void btnRepMaintPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void Btn_Save_Params_Click(object sender, EventArgs e)
        {
            if (Validate_Report())
            {
                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);

                //Get_Selection_Criteria();
                //Save_Entity.Scr_Code = PrivilegeEntity.Program;
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = null;
                Save_Entity.Card_3 = null;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
        }
        List<MATDEFEntity> Get_Sel_Scales_List = new List<MATDEFEntity>();
        private string Get_XML_Format_for_Report_Controls()   // 12012012
        {
            string strAssessmenttype = string.Empty;
            //if (cmbAssessmentType.Visible == true)
            //{
            //    strAssessmenttype = (((ListItem)cmbAssessmentType.SelectedItem).Value.ToString());
            //}
            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            string strWorker = ((Captain.Common.Utilities.ListItem)Cmb_Worker.SelectedItem).Value.ToString();
            string strScaleCodes = string.Empty;
            string Year = string.Empty;
            if (CmbYear.Visible == true)
                Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (Rb_Sel_Scales.Checked == true)
            {
                foreach (MATDEFEntity ScaleCode in Selected_Scales_List)
                {
                    if (!strScaleCodes.Equals(string.Empty)) strScaleCodes += ",";
                    strScaleCodes += ScaleCode.Scale_Code;
                }
            }

            str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) + "\" YEAR = \"" + Year +
                "\" MATRIX = \"" + (((ListItem)Cmb_Matrix.SelectedItem).Text.Trim()) + "\" SCALE = \"" + (Rb_All_Scales.Checked ? "*" : "Sel") + "\"  Worker = \"" + strWorker + "\" ScaleCodes = \"" + strScaleCodes +
                "\" ASMT_FDATE = \"" + Asmt_F_Date.Value.ToShortDateString() + "\" ASMT_TDATE = \"" + Asmt_T_Date.Value.ToShortDateString() +
                "\" CP2_FDATE = \"" + dtpCP2From.Value.ToShortDateString() + "\" CP2_TDATE = \"" + dtpCP2To.Value.ToShortDateString() +
                "\" CP3_FDATE = \"" + dtpCP3From.Value.ToShortDateString() + "\" CP3_TDATE = \"" + dtpCP3To.Value.ToShortDateString() + "\" />"); //"\" ASSESSMENT_TYPE = \"" + strAssessmenttype +
            str.Append("</Rows>");

            return str.ToString();
        }


        private void Btn_Get_Params_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            //Save_Entity.Scr_Code = PrivilegeEntity.Program;
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                Clear_Error_Providers();

                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);
            }
        }

        private void Clear_Error_Providers()
        {
            _errorProvider.SetError(Cmb_Matrix, null);
            _errorProvider.SetError(Rb_Sel_Scales, null);
            //_errorProvider.SetError(Base_FDate, null);
            //_errorProvider.SetError(Base_TDate, null);
            _errorProvider.SetError(Asmt_F_Date, null);
            _errorProvider.SetError(Asmt_T_Date, null);
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];
                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(Cmb_Matrix, dr["MATRIX"].ToString());
                CommonFunctions.SetComboBoxValue(Cmb_Worker, dr["Worker"].ToString());

                switch (dr["SCALE"].ToString())
                {
                    case "*": Rb_All_Scales.Checked = true; break;
                    default: Rb_Sel_Scales.Checked = true; break;
                }

                if (Rb_Sel_Scales.Checked)
                {
                    Selected_Scales_List.Clear();
                    string strFunds = dr["ScaleCodes"].ToString();
                    List<string> siteList = new List<string>();
                    if (strFunds != null)
                    {
                        MATDEFEntity Search_Entity = new MATDEFEntity(true);
                        Search_Entity.Mat_Code = ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString();
                        List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");

                        string[] scaleCodes = strFunds.Split(',');
                        for (int i = 0; i < scaleCodes.Length; i++)
                        {
                            MATDEFEntity scaleDetails = matdefEntity.Find(u => u.Scale_Code == scaleCodes.GetValue(i).ToString());
                            Selected_Scales_List.Add(scaleDetails);
                        }
                    }
                    Selected_Scales_List = Selected_Scales_List;
                }
                //Cb_Only_Asmt_Scale.Checked = (dr["PRINTONLY"].ToString() == "Y" ? true : false);

                //if (dr["DATESEL"].ToString() == "F")
                //    Rb_Asmt_FDate.Checked = true;
                //else
                //    Rb_Asmt_TDate.Checked = true;

                //Base_FDate.Value = Convert.ToDateTime(dr["BASE_FDATE"].ToString());
                //Base_TDate.Value = Convert.ToDateTime(dr["BASE_TDATE"].ToString());

                Asmt_F_Date.Value = Convert.ToDateTime(dr["ASMT_FDATE"].ToString());
                Asmt_T_Date.Value = Convert.ToDateTime(dr["ASMT_TDATE"].ToString());
                dtpCP2From.Value = Convert.ToDateTime(dr["CP2_FDATE"].ToString());
                dtpCP2To.Value = Convert.ToDateTime(dr["CP2_TDATE"].ToString());
                dtpCP3From.Value = Convert.ToDateTime(dr["CP3_FDATE"].ToString());
                dtpCP3To.Value = Convert.ToDateTime(dr["CP3_TDATE"].ToString());

                //Base_FDate.Checked = Base_TDate.Checked =
                Asmt_F_Date.Checked = Asmt_T_Date.Checked = true;
                dtpCP2From.Checked = dtpCP2To.Checked = dtpCP3From.Checked = dtpCP3To.Checked = true;
                //if (cmbAssessmentType.Visible == true)
                //{
                //    if (Tmp_Table.Columns.Contains("ASSESSMENT_TYPE"))
                //    {
                //        SetComboBoxValue(cmbAssessmentType, dr["ASSESSMENT_TYPE"].ToString());
                //    }
                //}
            }
        }


        private void Pb_Prog_Click(object sender, EventArgs e)
        {

        }

        
        private void Rb_All_Scales_Click(object sender, EventArgs e)
        {
            Selected_Scales_List.Clear();
            _errorProvider.SetError(Rb_Sel_Scales, null);
        }

        private void Rb_Sel_Scales_Click(object sender, EventArgs e)
        {
            if (Rb_Sel_Scales.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, Privileges.Program, ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString(), Selected_Scales_List);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }

        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
                Selected_Scales_List = form.Get_Sel_Scales_List;
        }


        private void Rb_Asmt_FDate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Cmb_Matrix_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                }
            }
        }

        string Program_Year, Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
            {
                //Get_NameFormat_For_Agencirs(Agy);
                Member_NameFormat = BaseForm.BaseHierarchyCnFormat.ToString(); CAseWorkerr_NameFormat = BaseForm.BaseHierarchyCwFormat.ToString();
            }
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(620, 25);
                Agy = string.Empty; Dept = string.Empty; Prog = string.Empty;
            }

        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<ListItem> listItem = new List<ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }
            Agency = Agy; Depart = Dept; Program = Prog; strYear = Year;

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(540, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(620, 25);
        }

        string Random_Filename = null; //string PdfName = "Pdf File";
        private void OnExcel_Report(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                string AuditName = PdfName;
                //PdfName = strFolderPath + PdfName;
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }

                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                Workbook book = new Workbook();

                this.GenerateStyles(book.Styles);

                this.GenerateWorksheetParameters(book);

                Worksheet sheet = book.Worksheets.Add("Data");
                //sheet.Names.Add(new WorksheetNamedRange("_FilterDatabase", "=Data!R1C1:R"+(MatRepList.Count+1).ToString()+"C11", true));
                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.DefaultColumnWidth = 220.5F;
                sheet.Table.Columns.Add(52);
                sheet.Table.Columns.Add(52);
                sheet.Table.Columns.Add(148);
                sheet.Table.Columns.Add(45);
                sheet.Table.Columns.Add(113);
                sheet.Table.Columns.Add(139);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(125);
                sheet.Table.Columns.Add(85);
                //WorksheetColumn column6 = 
                ////column6.Index = 8;
                //column6.Width = 65;
                sheet.Table.Columns.Add(65);
                sheet.Table.Columns.Add(53);
                sheet.Table.Columns.Add(50);

                try
                {
                    WorksheetRow Row0 = sheet.Table.Rows.Add();

                    WorksheetCell cell;
                    cell = Row0.Cells.Add("App #", DataType.String, "s94");
                    cell = Row0.Cells.Add("Fam.ID", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Name", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Gender", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Ethnicity", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Race", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Assessment Number", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Scale", DataType.String, "s94");
                    cell = Row0.Cells.Add("Assessment Date", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Response", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Points", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Birth Date", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    cell = Row0.Cells.Add("Site", DataType.String, "s94");
                    //cell.NamedCell.Add("_FilterDatabase");
                    int SclCount = 0;
                    if (MatRepList.Count > 0)
                    {
                        

                        //List<MATOUTCEntity> OutComes_List = new List<MATOUTCEntity>();
                        //MATOUTCEntity Search_Entity1 = new MATOUTCEntity(true);
                        //OutComes_List = _model.MatrixScalesData.Browse_MATOUTC(Search_Entity1, "Browse");

        
                        foreach (MATB0003Entity Entity in MatRepList)
                        {
                            WorksheetRow Row1 = sheet.Table.Rows.Add();
                            //cell = Row1.Cells.Add();
                            cell = Row1.Cells.Add(Entity.App, DataType.String, "s95");
                            cell = Row1.Cells.Add(Entity.MST_FAMILY_ID, DataType.String, "s95");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.Name, DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.SNP_SEX, DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.ETHNIC, DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.RACE, DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            string Assesmentdesc = string.Empty;
                            switch (Entity.Assessment_Type.Trim())
                            {
                                case "1": Assesmentdesc = "CP Beginning of the Year"; break;
                                case "2": Assesmentdesc = "CP Middle of the Year"; break;
                                case "3": Assesmentdesc = "CP End of the Year"; break;
                                case "4": Assesmentdesc = "4th Assessment"; break;
                                case "5": Assesmentdesc = "5th Assessment"; break;
                                case "6": Assesmentdesc = "6th Assessment"; break;
                                case "7": Assesmentdesc = "7th Assessment"; break;
                                case "8": Assesmentdesc = "8th Assessment"; break;
                                case "9": Assesmentdesc = "9th Assessment"; break;
                            }

                            cell = Row1.Cells.Add(Assesmentdesc, DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.SclcodeDesc.Trim(), DataType.String, "s96");
                            cell = Row1.Cells.Add(LookupDataAccess.Getdate(Entity.SSDate.Trim()), DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.MATASMT_RESP_DESC.Trim(), DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.Points, DataType.Number, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(LookupDataAccess.Getdate(Entity.SNP_ALT_BDATE.Trim()), DataType.String, "s95");
                            //cell.NamedCell.Add("_FilterDatabase");
                            cell = Row1.Cells.Add(Entity.Site.Trim(), DataType.String, "s96");
                            //cell.NamedCell.Add("_FilterDatabase");
                        }

                        var DistSer = MatRepList.Select(u => u.SCALE).Distinct().ToList();
                        
                        SclCount = DistSer.Count+1;
                    }

            #region ProgramWide
                    //if (dsMatb.Tables.Count > 1)
                    //{
                    //    if (dsMatb.Tables[1].Rows.Count > 0)
                    //    {
                    //        sheet = book.Worksheets.Add("Program Wide");
                    //        sheet.Table.DefaultRowHeight = 14.25F;

                    //        sheet.Table.DefaultColumnWidth = 220.5F;
                    //        sheet.Table.Columns.Add(195);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        //WorksheetColumn column6 = sheet.Table.Columns.Add();
                    //        //column6.Index = 8;
                    //        //column6.Width = 65;
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(110);
                    //        sheet.Table.Columns.Add(175);

                    //        Row0 = sheet.Table.Rows.Add();

                    //        cell = Row0.Cells.Add("Program Wide", DataType.String, "s98");
                    //        Row0 = sheet.Table.Rows.Add();
                    //        cell = Row0.Cells.Add("All Sheets", DataType.String, "s98");

                    //        string PrivScale = string.Empty, privDate = string.Empty; int i = 0;
                    //        bool IsboolAll = true, IsPercent = false;
                    //        foreach (DataRow drProg in dsMatb.Tables[1].Rows)
                    //        {
                    //            if (IsboolAll)
                    //            {
                    //                //if (PrivScale != drProg["SCALE"].ToString())
                    //                //{
                    //                //Row0 = sheet.Table.Rows.Add();
                    //                //cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s94");

                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add("Class", DataType.String, "s98");

                    //                foreach (DataColumn dc in dsMatb.Tables[1].Columns)
                    //                {
                    //                    if (dc.ColumnName != "MATASMT_SS_DATE")
                    //                    {
                    //                        cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s98");
                    //                    }
                    //                }
                    //                cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                cell = Row0.Cells.Add("% Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s100");
                    //                i = 1;
                    //                IsboolAll = false;
                    //                //}
                    //            }
                    //            if (i < 11)
                    //            {
                    //                if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    //                {
                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    int RowTotValue = 0; string Checks = string.Empty;
                    //                    foreach (DataColumn col in dsMatb.Tables[1].Columns)
                    //                    {
                    //                        if (!drProg.IsNull(col))
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                                RowTotValue += d;
                    //                        }
                    //                    }

                    //                    switch (i)
                    //                    {
                    //                        case 1: Checks = "Grand Total First Checkpoint"; IsPercent = true; break;
                    //                        case 2: Checks = "Grand Total Second Checkpoint"; break;
                    //                        case 3: Checks = "Grand Total Third Checkpoint"; break;
                    //                        case 4: Checks = "Grand Total Fourth Checkpoint"; break;
                    //                        case 5: Checks = "Grand Total Fifth Checkpoint"; break;
                    //                        case 6: Checks = "Grand Total Sixth Checkpoint"; break;
                    //                        case 7: Checks = "Grand Total Seventh Checkpoint"; break;
                    //                        case 8: Checks = "Grand Total Eigth Checkpoint"; break;
                    //                        case 9: Checks = "Grand Total Nineth Checkpoint"; break;
                    //                        case 10: Checks = "Grand Total Tenth Checkpoint"; break;
                    //                        //case 11: Checks = "Grand Total Eleventh Checkpoint"; break;
                    //                        //case 12: Checks = "Grand Total twelfth Checkpoint"; break;
                    //                        //case 13: Checks = "Grand Total thirteenth Checkpoint"; break;
                    //                        //case 14: Checks = "Grand Total fourteenth Checkpoint"; break;
                    //                        //case 15: Checks = "Grand Total fifteenth Checkpoint"; break;
                    //                        //case 16: Checks = "Grand Total sixteenth Checkpoint"; break;
                    //                        //case 17: Checks = "Grand Total seventeenth Checkpoint"; break;
                    //                        //case 18: Checks = "Grand Total eighteenth Checkpoint"; break;
                    //                        //case 19: Checks = "Grand Total nineteenth Checkpoint"; break;
                    //                        //case 20: Checks = "Grand Total Tenth Checkpoint"; break;
                    //                    }
                    //                    cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                    foreach (DataColumn col in dsMatb.Tables[1].Columns)
                    //                    {
                    //                        if (col.ColumnName != "MATASMT_SS_DATE")
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                            {
                    //                                decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue)); //Convert.ToDecimal(((d * 100) / RowTotValue));
                    //                                cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                            }
                    //                        }
                    //                    }
                    //                    if (IsPercent)
                    //                    {
                    //                        cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                        IsPercent = false;
                    //                    }
                    //                    else
                    //                    {
                    //                        cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                        cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                    }
                    //                    cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                    cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                    i++;
                    //                }
                    //            }
                    //        }



                    
                    //        if (dsMatb.Tables[2].Rows.Count > 0)
                    //        {
                    //            foreach (DataRow drProg in dsMatb.Tables[2].Rows)
                    //            {
                    //                if (PrivScale != drProg["SCALE"].ToString())
                    //                {
                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    cell = Row0.Cells.Add("", DataType.String, "s99");

                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s98");

                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    cell = Row0.Cells.Add("Class", DataType.String, "s98");

                    //                    foreach (DataColumn dc in dsMatb.Tables[2].Columns)
                    //                    {
                    //                        if (dc.ColumnName != "MATASMT_SS_DATE" && dc.ColumnName != "MST_SITE" && dc.ColumnName != "SCALE")
                    //                        {
                    //                            cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s98");
                    //                        }
                    //                    }
                    //                    cell = Row0.Cells.Add("", DataType.String, "s98");
                    //                    cell = Row0.Cells.Add("% Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s100");
                    //                    i = 1;
                    //                    PrivScale = drProg["SCALE"].ToString();
                    //                }
                    //                if (i < 11)
                    //                {
                    //                    if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    //                    {
                    //                        Row0 = sheet.Table.Rows.Add();
                    //                        int RowTotValue = 0; string Checks = string.Empty;
                    //                        foreach (DataColumn col in dsMatb.Tables[2].Columns)
                    //                        {
                    //                            if (!drProg.IsNull(col))
                    //                            {
                    //                                string stringValue = drProg[col].ToString();
                    //                                int d;
                    //                                if (int.TryParse(stringValue, out d))
                    //                                    RowTotValue += d;
                    //                            }
                    //                        }

                    //                        switch (i)
                    //                        {
                    //                            case 1: Checks = "Grand Total First Checkpoint"; IsPercent = true; break;
                    //                            case 2: Checks = "Grand Total Second Checkpoint"; break;
                    //                            case 3: Checks = "Grand Total Third Checkpoint"; break;
                    //                            case 4: Checks = "Grand Total Fourth Checkpoint"; break;
                    //                            case 5: Checks = "Grand Total Fifth Checkpoint"; break;
                    //                            case 6: Checks = "Grand Total Sixth Checkpoint"; break;
                    //                            case 7: Checks = "Grand Total Seventh Checkpoint"; break;
                    //                            case 8: Checks = "Grand Total Eigth Checkpoint"; break;
                    //                            case 9: Checks = "Grand Total Nineth Checkpoint"; break;
                    //                            case 10: Checks = "Grand Total Tenth Checkpoint"; break;
                    //                        }
                    //                        cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                        foreach (DataColumn col in dsMatb.Tables[2].Columns)
                    //                        {
                    //                            if (col.ColumnName != "MATASMT_SS_DATE" && col.ColumnName != "MST_SITE" && col.ColumnName != "SCALE")
                    //                            {
                    //                                string stringValue = drProg[col].ToString();
                    //                                int d;
                    //                                if (int.TryParse(stringValue, out d))
                    //                                {
                    //                                    decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue));
                    //                                    //float Percent = ((d / RowTotValue) * 100);
                    //                                    cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                                }
                    //                            }
                    //                        }

                    //                        if (IsPercent)
                    //                        {
                    //                            cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                            IsPercent = false;
                    //                        }
                    //                        else
                    //                        {
                    //                            cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                            cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                        }
                    //                        cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                        cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                        i++;
                    //                        privDate = drProg["MATASMT_SS_DATE"].ToString();
                    //                    }

                    //                }

                    //            }
                    //        }



                    //    }

                    //    for (int j = 3; j < 21; j++)
                    //    {
                    //        if (dsMatb.Tables[j].Rows.Count > 0)
                    //        {
                    //            string ReportName = string.Empty;
                    //            switch (j)
                    //            {
                    //                case 3: ReportName = "BT&Mil"; break;
                    //                case 5: ReportName = "EH"; break;
                    //                case 7: ReportName = "FV"; break;
                    //                case 9: ReportName = "GF"; break;
                    //                case 11: ReportName = "MT"; break;
                    //                case 13: ReportName = "NC1"; break;
                    //                case 15: ReportName = "NC2"; break;
                    //                case 17: ReportName = "NC3"; break;
                    //                case 19: ReportName = "NC4"; break;
                    //                //case 12: ReportName = " Tenth Checkpoint"; break;
                    //            }

                    //            sheet = book.Worksheets.Add(ReportName);
                    //            sheet.Table.DefaultRowHeight = 14.25F;

                    //            sheet.Table.DefaultColumnWidth = 220.5F;
                    //            sheet.Table.Columns.Add(195);
                    //            sheet.Table.Columns.Add(65);
                    //            sheet.Table.Columns.Add(65);
                    //            sheet.Table.Columns.Add(65);
                    //            sheet.Table.Columns.Add(65);
                    //            sheet.Table.Columns.Add(65);
                    //            //WorksheetColumn column6 = sheet.Table.Columns.Add();
                    //            //column6.Index = 8;
                    //            //column6.Width = 65;
                    //            sheet.Table.Columns.Add(65);
                    //            sheet.Table.Columns.Add(65);
                    //            sheet.Table.Columns.Add(110);
                    //            sheet.Table.Columns.Add(175);

                    //            Row0 = sheet.Table.Rows.Add();


                    //            string PrivScale = string.Empty, privDate = string.Empty; int i = 0;
                    //            bool IsboolAll = true, IsPercent = false, IsChange = false;
                    //            string PrivSite = string.Empty;
                    //            foreach (DataRow drProg in dsMatb.Tables[j].Rows)
                    //            {
                    //                if (IsboolAll)
                    //                {
                    //                    //if (PrivScale != drProg["SCALE"].ToString())
                    //                    //{
                    //                    //Row0 = sheet.Table.Rows.Add();
                    //                    //cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s94");

                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    cell = Row0.Cells.Add("All Sheets", DataType.String, "s98");

                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    cell = Row0.Cells.Add("Class", DataType.String, "s98");

                    //                    foreach (DataColumn dc in dsMatb.Tables[j].Columns)
                    //                    {
                    //                        if (dc.ColumnName != "MATASMT_SS_DATE" && dc.ColumnName != "MST_SITE")
                    //                        {
                    //                            cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s98");
                    //                        }
                    //                    }
                    //                    cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                    cell = Row0.Cells.Add("% Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s100");
                    //                    i = 1;
                    //                    IsboolAll = false;
                    //                    //}
                    //                }
                    //                if (i < 11)
                    //                {
                    //                    if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    //                    {
                    //                        Row0 = sheet.Table.Rows.Add();
                    //                        int RowTotValue = 0; string Checks = string.Empty;
                    //                        foreach (DataColumn col in dsMatb.Tables[j].Columns)
                    //                        {
                    //                            if (!drProg.IsNull(col))
                    //                            {
                    //                                string stringValue = drProg[col].ToString();
                    //                                int d;
                    //                                if (int.TryParse(stringValue, out d))
                    //                                    RowTotValue += d;
                    //                            }
                    //                        }
                    //                        if (PrivSite != drProg["MST_SITE"].ToString())
                    //                        {
                    //                            if (i > 1) IsChange = true;
                    //                            i = 1;
                    //                        }

                    //                        string Site = drProg["MST_SITE"].ToString().Replace("-", "");
                    //                        switch (i)
                    //                        {
                    //                            case 1: Checks = Site + " First Checkpoint"; IsPercent = true; break;
                    //                            case 2: Checks = Site + " Second Checkpoint"; break;
                    //                            case 3: Checks = Site + " Third Checkpoint"; break;
                    //                            case 4: Checks = Site + " Fourth Checkpoint"; break;
                    //                            case 5: Checks = Site + " Fifth Checkpoint"; break;
                    //                            case 6: Checks = Site + " Sixth Checkpoint"; break;
                    //                            case 7: Checks = Site + " Seventh Checkpoint"; break;
                    //                            case 8: Checks = Site + " Eigth Checkpoint"; break;
                    //                            case 9: Checks = Site + " Nineth Checkpoint"; break;
                    //                            case 10: Checks = Site + " Tenth Checkpoint"; break;
                    //                        }
                    //                        cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                        foreach (DataColumn col in dsMatb.Tables[j].Columns)
                    //                        {
                    //                            if (col.ColumnName != "MATASMT_SS_DATE")
                    //                            {
                    //                                string stringValue = drProg[col].ToString();
                    //                                int d;
                    //                                if (int.TryParse(stringValue, out d))
                    //                                {
                    //                                    decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue)); //Convert.ToDecimal(((d * 100) / RowTotValue));
                    //                                    cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                                }
                    //                            }
                    //                        }
                    //                        if (IsPercent)
                    //                        {
                    //                            if (IsChange) cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                            else cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                            IsPercent = false;
                    //                        }
                    //                        else
                    //                        {
                    //                            cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                            cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                        }
                    //                        cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                        cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                        PrivSite = drProg["MST_SITE"].ToString();

                    //                        i++;
                    //                    }
                    //                }
                    //            }

                    //            j = j + 1;

                    //            if (dsMatb.Tables[j].Rows.Count > 0)
                    //            {
                    //                foreach (DataRow drProg in dsMatb.Tables[j].Rows)
                    //                {
                    //                    if (PrivScale != drProg["SCALE"].ToString())
                    //                    {
                    //                        Row0 = sheet.Table.Rows.Add();
                    //                        cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                        Row0 = sheet.Table.Rows.Add();
                    //                        cell = Row0.Cells.Add("", DataType.String, "s99");

                    //                        Row0 = sheet.Table.Rows.Add();
                    //                        cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s98");

                    //                        Row0 = sheet.Table.Rows.Add();
                    //                        cell = Row0.Cells.Add("Class", DataType.String, "s98");

                    //                        foreach (DataColumn dc in dsMatb.Tables[j].Columns)
                    //                        {
                    //                            if (dc.ColumnName != "MATASMT_SS_DATE" && dc.ColumnName != "MST_SITE" && dc.ColumnName != "SCALE")
                    //                            {
                    //                                cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s98");
                    //                            }
                    //                        }
                    //                        cell = Row0.Cells.Add("", DataType.String, "s98");
                    //                        cell = Row0.Cells.Add("% Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s100");
                    //                        i = 1; IsChange = false;
                    //                        PrivScale = drProg["SCALE"].ToString();
                    //                    }

                    //                    if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    //                    {
                    //                        if (i < 11)
                    //                        {
                    //                            Row0 = sheet.Table.Rows.Add();
                    //                            int RowTotValue = 0; string Checks = string.Empty;
                    //                            foreach (DataColumn col in dsMatb.Tables[j].Columns)
                    //                            {
                    //                                if (!drProg.IsNull(col))
                    //                                {
                    //                                    string stringValue = drProg[col].ToString();
                    //                                    int d;
                    //                                    if (int.TryParse(stringValue, out d))
                    //                                        RowTotValue += d;
                    //                                }
                    //                            }
                    //                            if (PrivSite != drProg["MST_SITE"].ToString())
                    //                            {
                    //                                if (i > 1) IsChange = true;
                    //                                i = 1;
                    //                            }
                    //                            string Site = drProg["MST_SITE"].ToString().Replace("-", "");
                    //                            switch (i)
                    //                            {
                    //                                case 1: Checks = Site + " First Checkpoint"; IsPercent = true; break;
                    //                                case 2: Checks = Site + " Second Checkpoint"; break;
                    //                                case 3: Checks = Site + " Third Checkpoint"; break;
                    //                                case 4: Checks = Site + " Fourth Checkpoint"; break;
                    //                                case 5: Checks = Site + " Fifth Checkpoint"; break;
                    //                                case 6: Checks = Site + " Sixth Checkpoint"; break;
                    //                                case 7: Checks = Site + " Seventh Checkpoint"; break;
                    //                                case 8: Checks = Site + " Eigth Checkpoint"; break;
                    //                                case 9: Checks = Site + " Nineth Checkpoint"; break;
                    //                                case 10: Checks = Site + " Tenth Checkpoint"; break;
                    //                            }
                    //                            cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                            foreach (DataColumn col in dsMatb.Tables[j].Columns)
                    //                            {
                    //                                if (col.ColumnName != "MATASMT_SS_DATE" && col.ColumnName != "MST_SITE" && col.ColumnName != "SCALE")
                    //                                {
                    //                                    string stringValue = drProg[col].ToString();
                    //                                    int d;
                    //                                    if (int.TryParse(stringValue, out d))
                    //                                    {
                    //                                        decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue));
                    //                                        //float Percent = ((d / RowTotValue) * 100);
                    //                                        cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                                    }
                    //                                }
                    //                            }

                    //                            if (IsPercent)
                    //                            {
                    //                                if (IsChange) cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                                else cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                                IsPercent = false;
                    //                            }
                    //                            else
                    //                            {
                    //                                cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                                cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                            }
                    //                            cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                            cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                            i++;
                    //                            privDate = drProg["MATASMT_SS_DATE"].ToString();
                    //                            PrivSite = drProg["MST_SITE"].ToString();
                    //                        }
                    //                    }
                    //                    else if (PrivSite != drProg["MST_SITE"].ToString())
                    //                    {
                    //                        if (i < 11)
                    //                        {
                    //                            Row0 = sheet.Table.Rows.Add();
                    //                            int RowTotValue = 0; string Checks = string.Empty;
                    //                            foreach (DataColumn col in dsMatb.Tables[j].Columns)
                    //                            {
                    //                                if (!drProg.IsNull(col))
                    //                                {
                    //                                    string stringValue = drProg[col].ToString();
                    //                                    int d;
                    //                                    if (int.TryParse(stringValue, out d))
                    //                                        RowTotValue += d;
                    //                                }
                    //                            }
                    //                            if (PrivSite != drProg["MST_SITE"].ToString())
                    //                            {
                    //                                if (i > 1) IsChange = true;
                    //                                i = 1;
                    //                            }
                    //                            string Site = drProg["MST_SITE"].ToString().Replace("-", "");
                    //                            switch (i)
                    //                            {
                    //                                case 1: Checks = Site + " First Checkpoint"; IsPercent = true; break;
                    //                                case 2: Checks = Site + " Second Checkpoint"; break;
                    //                                case 3: Checks = Site + " Third Checkpoint"; break;
                    //                                case 4: Checks = Site + " Fourth Checkpoint"; break;
                    //                                case 5: Checks = Site + " Fifth Checkpoint"; break;
                    //                                case 6: Checks = Site + " Sixth Checkpoint"; break;
                    //                                case 7: Checks = Site + " Seventh Checkpoint"; break;
                    //                                case 8: Checks = Site + " Eigth Checkpoint"; break;
                    //                                case 9: Checks = Site + " Nineth Checkpoint"; break;
                    //                                case 10: Checks = Site + " Tenth Checkpoint"; break;
                    //                            }
                    //                            cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                            foreach (DataColumn col in dsMatb.Tables[j].Columns)
                    //                            {
                    //                                if (col.ColumnName != "MATASMT_SS_DATE" && col.ColumnName != "MST_SITE" && col.ColumnName != "SCALE")
                    //                                {
                    //                                    string stringValue = drProg[col].ToString();
                    //                                    int d;
                    //                                    if (int.TryParse(stringValue, out d))
                    //                                    {
                    //                                        decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue));
                    //                                        //float Percent = ((d / RowTotValue) * 100);
                    //                                        cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                                    }
                    //                                }
                    //                            }

                    //                            if (IsPercent)
                    //                            {
                    //                                if (IsChange) cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                                else cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                                IsPercent = false;
                    //                            }
                    //                            else
                    //                            {
                    //                                cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                                cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                            }
                    //                            cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                            cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                            i++;
                    //                            privDate = drProg["MATASMT_SS_DATE"].ToString();
                    //                            PrivSite = drProg["MST_SITE"].ToString();
                    //                        }

                    //                    }


                    //                }
                    //            }

                    //        }
                    //    }
                    //    if (dsMatb.Tables[21].Rows.Count > 0)
                    //    {
                    //        string ReportName = "Age";

                    //        sheet = book.Worksheets.Add(ReportName);
                    //        sheet.Table.DefaultRowHeight = 14.25F;

                    //        sheet.Table.DefaultColumnWidth = 220.5F;
                    //        sheet.Table.Columns.Add(195);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        //WorksheetColumn column6 = sheet.Table.Columns.Add();
                    //        //column6.Index = 8;
                    //        //column6.Width = 65;
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(65);
                    //        sheet.Table.Columns.Add(110);
                    //        sheet.Table.Columns.Add(175);

                    //        Row0 = sheet.Table.Rows.Add();


                    //        string PrivScale = string.Empty, privDate = string.Empty; int i = 0;
                    //        bool IsboolAll = true, IsPercent = false, IsChange = false;
                    //        string PrivAge = string.Empty;
                    //        foreach (DataRow drProg in dsMatb.Tables[21].Rows)
                    //        {
                    //            if (IsboolAll)
                    //            {
                    //                //if (PrivScale != drProg["SCALE"].ToString())
                    //                //{
                    //                //Row0 = sheet.Table.Rows.Add();
                    //                //cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s94");

                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add("All Sheets", DataType.String, "s98");

                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add("Class", DataType.String, "s98");

                    //                foreach (DataColumn dc in dsMatb.Tables[21].Columns)
                    //                {
                    //                    if (dc.ColumnName != "MATASMT_SS_DATE" && dc.ColumnName != "SNP_AGE")
                    //                    {
                    //                        cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s98");
                    //                    }
                    //                }
                    //                cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                cell = Row0.Cells.Add("% Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s100");
                    //                i = 1;
                    //                IsboolAll = false;
                    //                //}
                    //            }
                    //            if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    //            {
                    //                if (i < 21)
                    //                {
                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    int RowTotValue = 0; string Checks = string.Empty;
                    //                    foreach (DataColumn col in dsMatb.Tables[21].Columns)
                    //                    {
                    //                        if (!drProg.IsNull(col))
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                                RowTotValue += d;
                    //                        }
                    //                    }
                    //                    if (PrivAge != drProg["SNP_AGE"].ToString())
                    //                    {
                    //                        if (i > 1)
                    //                            IsChange = true;
                    //                        i = 1;
                    //                    }

                    //                    string Site = "Age " + drProg["SNP_AGE"].ToString();
                    //                    switch (i)
                    //                    {
                    //                        case 1: Checks = Site + " First Checkpoint"; IsPercent = true; break;
                    //                        case 2: Checks = Site + " Second Checkpoint"; break;
                    //                        case 3: Checks = Site + " Third Checkpoint"; break;
                    //                        case 4: Checks = Site + " Fourth Checkpoint"; break;
                    //                        case 5: Checks = Site + " Fifth Checkpoint"; break;
                    //                        case 6: Checks = Site + " Sixth Checkpoint"; break;
                    //                        case 7: Checks = Site + " Seventh Checkpoint"; break;
                    //                        case 8: Checks = Site + " Eigth Checkpoint"; break;
                    //                        case 9: Checks = Site + " Nineth Checkpoint"; break;
                    //                        case 10: Checks = Site + " Tenth Checkpoint"; break;
                    //                        case 11: Checks = Site + " Eleventh Checkpoint"; break;
                    //                        case 12: Checks = Site + " Twelfth Checkpoint"; break;
                    //                        case 13: Checks = Site + " Thirteenth Checkpoint"; break;
                    //                        case 14: Checks = Site + " Fourteenth Checkpoint"; break;
                    //                        case 15: Checks = Site + " Fifteenth Checkpoint"; break;
                    //                        case 16: Checks = Site + " Sixteenth Checkpoint"; break;
                    //                        case 17: Checks = Site + " Seventeenth Checkpoint"; break;
                    //                        case 18: Checks = Site + " Eighteenth Checkpoint"; break;
                    //                        case 19: Checks = Site + " Nineteenth Checkpoint"; break;
                    //                        case 20: Checks = Site + " Twentieth Checkpoint"; break;
                    //                    }
                    //                    cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                    foreach (DataColumn col in dsMatb.Tables[21].Columns)
                    //                    {
                    //                        if (col.ColumnName != "MATASMT_SS_DATE" && col.ColumnName != "SNP_AGE")
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                            {
                    //                                decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue)); //Convert.ToDecimal(((d * 100) / RowTotValue));
                    //                                cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                            }
                    //                        }
                    //                    }
                    //                    if (IsPercent)
                    //                    {
                    //                        if (IsChange)
                    //                            cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                        else
                    //                            cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                        IsPercent = false;
                    //                    }
                    //                    else
                    //                    {
                    //                        cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                        cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                    }
                    //                    cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                    cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                    PrivAge = drProg["SNP_AGE"].ToString();

                    //                    i++;
                    //                }
                    //            }
                    //        }
                    //        foreach (DataRow drProg in dsMatb.Tables[22].Rows)
                    //        {
                    //            if (PrivScale != drProg["SCALE"].ToString())
                    //            {
                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add("", DataType.String, "s99");

                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s98");

                    //                Row0 = sheet.Table.Rows.Add();
                    //                cell = Row0.Cells.Add("Class", DataType.String, "s98");

                    //                foreach (DataColumn dc in dsMatb.Tables[22].Columns)
                    //                {
                    //                    if (dc.ColumnName != "MATASMT_SS_DATE" && dc.ColumnName != "SNP_AGE" && dc.ColumnName != "SCALE")
                    //                    {
                    //                        cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s98");
                    //                    }
                    //                }
                    //                cell = Row0.Cells.Add("", DataType.String, "s98");
                    //                cell = Row0.Cells.Add("% Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s100");
                    //                i = 1; IsChange = false;
                    //                PrivScale = drProg["SCALE"].ToString();
                    //            }
                    //            if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    //            {
                    //                if (i < 11)
                    //                {
                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    int RowTotValue = 0; string Checks = string.Empty;
                    //                    foreach (DataColumn col in dsMatb.Tables[22].Columns)
                    //                    {
                    //                        if (!drProg.IsNull(col))
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                                RowTotValue += d;
                    //                        }
                    //                    }
                    //                    if (PrivAge != drProg["SNP_AGE"].ToString())
                    //                    {
                    //                        if (i > 1) IsChange = true;
                    //                        i = 1;
                    //                    }
                    //                    string Site = "Age " + drProg["SNP_AGE"].ToString();
                    //                    switch (i)
                    //                    {
                    //                        case 1: Checks = Site + " First Checkpoint"; IsPercent = true; break;
                    //                        case 2: Checks = Site + " Second Checkpoint"; break;
                    //                        case 3: Checks = Site + " Third Checkpoint"; break;
                    //                        case 4: Checks = Site + " Fourth Checkpoint"; break;
                    //                        case 5: Checks = Site + " Fifth Checkpoint"; break;
                    //                        case 6: Checks = Site + " Sixth Checkpoint"; break;
                    //                        case 7: Checks = Site + " Seventh Checkpoint"; break;
                    //                        case 8: Checks = Site + " Eigth Checkpoint"; break;
                    //                        case 9: Checks = Site + " Nineth Checkpoint"; break;
                    //                        case 10: Checks = Site + " Tenth Checkpoint"; break;
                    //                    }
                    //                    cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                    foreach (DataColumn col in dsMatb.Tables[22].Columns)
                    //                    {
                    //                        if (col.ColumnName != "MATASMT_SS_DATE" && col.ColumnName != "SNP_AGE" && col.ColumnName != "SCALE")
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                            {
                    //                                decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue));
                    //                                //float Percent = ((d / RowTotValue) * 100);
                    //                                cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                            }
                    //                        }
                    //                    }

                    //                    if (IsPercent)
                    //                    {
                    //                        if (IsChange)
                    //                            cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                        else
                    //                            cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                        IsPercent = false;
                    //                    }
                    //                    else
                    //                    {
                    //                        cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                        cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                    }
                    //                    cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                    cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                    i++;
                    //                    privDate = drProg["MATASMT_SS_DATE"].ToString();
                    //                    PrivAge = drProg["SNP_AGE"].ToString();
                    //                }

                    //            }
                    //            else if (PrivAge != drProg["SNP_AGE"].ToString())
                    //            {
                    //                if (i < 11)
                    //                {
                    //                    Row0 = sheet.Table.Rows.Add();
                    //                    int RowTotValue = 0; string Checks = string.Empty;
                    //                    foreach (DataColumn col in dsMatb.Tables[22].Columns)
                    //                    {
                    //                        if (!drProg.IsNull(col))
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                                RowTotValue += d;
                    //                        }
                    //                    }
                    //                    if (PrivAge != drProg["SNP_AGE"].ToString())
                    //                    {
                    //                        if (i > 1) IsChange = true;
                    //                        i = 1;
                    //                    }
                    //                    string Site = "Age " + drProg["SNP_AGE"].ToString();
                    //                    switch (i)
                    //                    {
                    //                        case 1: Checks = Site + " First Checkpoint"; IsPercent = true; break;
                    //                        case 2: Checks = Site + " Second Checkpoint"; break;
                    //                        case 3: Checks = Site + " Third Checkpoint"; break;
                    //                        case 4: Checks = Site + " Fourth Checkpoint"; break;
                    //                        case 5: Checks = Site + " Fifth Checkpoint"; break;
                    //                        case 6: Checks = Site + " Sixth Checkpoint"; break;
                    //                        case 7: Checks = Site + " Seventh Checkpoint"; break;
                    //                        case 8: Checks = Site + " Eigth Checkpoint"; break;
                    //                        case 9: Checks = Site + " Nineth Checkpoint"; break;
                    //                        case 10: Checks = Site + " Tenth Checkpoint"; break;
                    //                    }
                    //                    cell = Row0.Cells.Add(Checks, DataType.String, "s81");

                    //                    foreach (DataColumn col in dsMatb.Tables[22].Columns)
                    //                    {
                    //                        if (col.ColumnName != "MATASMT_SS_DATE" && col.ColumnName != "SNP_AGE" && col.ColumnName != "SCALE")
                    //                        {
                    //                            string stringValue = drProg[col].ToString();
                    //                            int d;
                    //                            if (int.TryParse(stringValue, out d))
                    //                            {
                    //                                decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue));
                    //                                //float Percent = ((d / RowTotValue) * 100);
                    //                                cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s101");
                    //                            }
                    //                        }
                    //                    }

                    //                    if (IsPercent)
                    //                    {
                    //                        if (IsChange)
                    //                            cell = Row0.Cells.Add("", DataType.String, "s99");
                    //                        else
                    //                            cell = Row0.Cells.Add("Total % Point Change", DataType.String, "s99");
                    //                        IsPercent = false;
                    //                    }
                    //                    else
                    //                    {
                    //                        cell = Row0.Cells.Add("0", DataType.Number, "s103");
                    //                        cell.Formula = "=((RC[-7]+RC[-6])-(R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4])-(R[-1]C[-5]+R[-1]C[-4]))";
                    //                    }
                    //                    cell = Row0.Cells.Add("0", DataType.Number, "s102");
                    //                    cell.Formula = "=100%-(RC[-4]+RC[-3]+RC[-2])";

                    //                    i++;
                    //                    privDate = drProg["MATASMT_SS_DATE"].ToString();
                    //                    PrivAge = drProg["SNP_AGE"].ToString();
                    //                }
                    //            }


                    //        }

                    //    }
                    //}

                    ////if (dsMatb.Tables[1].Rows.Count > 0)
                    ////{
                    ////    sheet = book.Worksheets.Add("Program Wide");
                    ////    sheet.Table.DefaultRowHeight = 14.25F;

                    ////    sheet.Table.DefaultColumnWidth = 220.5F;
                    ////    sheet.Table.Columns.Add(250);
                    ////    sheet.Table.Columns.Add(150);
                    ////    sheet.Table.Columns.Add(150);
                    ////    sheet.Table.Columns.Add(150);
                    ////    sheet.Table.Columns.Add(150);
                    ////    sheet.Table.Columns.Add(150);
                    ////    //WorksheetColumn column6 = sheet.Table.Columns.Add();
                    ////    //column6.Index = 8;
                    ////    //column6.Width = 65;
                    ////    sheet.Table.Columns.Add(150);
                    ////    sheet.Table.Columns.Add(200);
                    ////    sheet.Table.Columns.Add(200);

                    ////    Row0 = sheet.Table.Rows.Add();

                    ////    cell = Row0.Cells.Add("Program Wide", DataType.String, "s94");
                    ////    //Row0 = sheet.Table.Rows.Add();
                    ////    //cell = Row0.Cells.Add("All Sheets", DataType.String, "s94");

                    ////    string PrivScale = string.Empty, privDate = string.Empty; int i = 0;
                    ////    foreach (DataRow drProg in dsMatb.Tables[1].Rows)
                    ////    {
                    ////        if (PrivScale != drProg["SCALE"].ToString())
                    ////        {
                    ////            Row0 = sheet.Table.Rows.Add();
                    ////            cell = Row0.Cells.Add(drProg["SCALE"].ToString(), DataType.String, "s94");

                    ////            Row0 = sheet.Table.Rows.Add();
                    ////            cell = Row0.Cells.Add("Class", DataType.String, "s94");

                    ////            foreach (DataColumn dc in dsMatb.Tables[1].Columns)
                    ////            {
                    ////                if (dc.ColumnName != "MATASMT_SS_DATE" || dc.ColumnName != "MST_SITE" || dc.ColumnName != "SCALE")
                    ////                {
                    ////                    cell = Row0.Cells.Add(dc.ColumnName.Trim(), DataType.String, "s94");
                    ////                }
                    ////            }
                    ////            cell = Row0.Cells.Add("", DataType.String, "s94");
                    ////            cell = Row0.Cells.Add("% Meeting "+ ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() +" Goals", DataType.String, "s94");
                    ////            i = 1;
                    ////        }
                    ////        if (privDate != drProg["MATASMT_SS_DATE"].ToString())
                    ////        {
                    ////            Row0 = sheet.Table.Rows.Add();
                    ////            int RowTotValue = 0; string Checks = string.Empty;
                    ////            foreach (DataColumn col in dsMatb.Tables[1].Columns)
                    ////            {
                    ////                if (!drProg.IsNull(col))
                    ////                {
                    ////                    string stringValue = drProg[col].ToString();
                    ////                    int d;
                    ////                    if (int.TryParse(stringValue, out d))
                    ////                        RowTotValue += d;
                    ////                }
                    ////            }

                    ////            switch (i)
                    ////            {
                    ////                case 1: Checks = " First Checkpoint"; break;
                    ////                case 2: Checks = " Second Checkpoint"; break;
                    ////                case 3: Checks = " Third Checkpoint"; break;
                    ////                case 4: Checks = " Fourth Checkpoint"; break;
                    ////                case 5: Checks = " Fifth Checkpoint"; break;
                    ////                case 6: Checks = " Sixth Checkpoint"; break;
                    ////                case 7: Checks = " Seventh Checkpoint"; break;
                    ////                case 8: Checks = " Eigth Checkpoint"; break;
                    ////                case 9: Checks = " Nineth Checkpoint"; break;
                    ////                case 10: Checks = " Tenth Checkpoint"; break;
                    ////            }
                    ////            cell = Row0.Cells.Add(drProg["MST_SITE"].ToString()+Checks, DataType.String, "s95");

                    ////            foreach (DataColumn col in dsMatb.Tables[1].Columns)
                    ////            {
                    ////                if (col.ColumnName != "MATASMT_SS_DATE" || col.ColumnName != "MST_SITE" || col.ColumnName != "SCALE")
                    ////                {
                    ////                    string stringValue = drProg[col].ToString();
                    ////                    int d;
                    ////                    if (int.TryParse(stringValue, out d))
                    ////                    {
                    ////                        decimal Percent = (d / RowTotValue) * 100;
                    ////                        cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s68");
                    ////                    }
                    ////                }
                    ////            }

                    ////            //cell = Row0.Cells.Add(Entity.Points, DataType.Number, "s96");
                    ////            i++;
                    ////        }
                            

                    ////    }

                    ////}
#endregion

                    #region New Changes on 06/29/2017

                    List<MATDEFBMEntity> Benchmarks_List = new List<MATDEFBMEntity>();

                        MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
                        Search_Entity.MatCode = ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString();
                        Benchmarks_List = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");
                        List<MATGroupEntity> Group_List = _model.MatrixScalesData.GetSCLGRPS(((ListItem)Cmb_Matrix.SelectedItem).Value.ToString(), string.Empty);
                        int G = 0;
                    for (int j = 1; j < dsMatb.Tables.Count; j++)
                    {
                        if (dsMatb.Tables[j].Rows.Count > 0)
                        {
                            string ReportName = "Total Percentages";

                            if (j > SclCount) { G++; ReportName = dsMatb.Tables[j].Rows[0]["SCLGRP_GRP_DESC"].ToString().Trim(); }

                            else if (j > 1)
                            {
                                var CodeScale = MatRepList.Find(u => u.SclcodeDesc.Trim().Equals(dsMatb.Tables[j].Rows[0]["SCALE"].ToString().Trim())).SCALE.Trim();
                                if (CodeScale != null)
                                    ReportName = ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString().Trim() + " " + CodeScale.ToString().Trim();
                                else ReportName = "Sheet" + j.ToString();
                            }
                            //if (j > 1) ReportName = ReportName;//dsMatb.Tables[j].Rows[0]["SCALE"].ToString();
                            

                            int Col = dsMatb.Tables[j].Columns.Count;

                            if (j > 1) Col = Col - 1;

                            sheet = book.Worksheets.Add(ReportName);
                            sheet.Table.DefaultRowHeight = 14.25F;

                            sheet.Table.DefaultColumnWidth = 220.5F;

                            for (int c = 0; c < Col; c++)
                            {
                                if (c == 0) sheet.Table.Columns.Add(80);
                                else if (c == 1) sheet.Table.Columns.Add(130);
                                else sheet.Table.Columns.Add(65);

                            }
                            //sheet.Table.Columns.Add(70);
                            //sheet.Table.Columns.Add(195);
                            //sheet.Table.Columns.Add(65);
                            //sheet.Table.Columns.Add(65);
                            //sheet.Table.Columns.Add(65);
                            //sheet.Table.Columns.Add(65);
                            ////WorksheetColumn column6 = sheet.Table.Columns.Add();
                            ////column6.Index = 8;
                            ////column6.Width = 65;
                            //sheet.Table.Columns.Add(65);
                            //sheet.Table.Columns.Add(65);
                            //sheet.Table.Columns.Add(65);
                            //sheet.Table.Columns.Add(65);
                            sheet.Table.Columns.Add(90);
                            sheet.Table.Columns.Add(175);

                            Row0 = sheet.Table.Rows.Add();
                            if (j > SclCount)
                            {
                                //SCLGRP_GRP_DESC
                                string GroupDesc=dsMatb.Tables[j].Rows[0]["SCLGRP_GRP_DESC"].ToString();
                                if (Group_List.Count > 0)
                                {
                                    var GrpDesc = Group_List.Find(u => u.ShortName.Trim().Equals(dsMatb.Tables[j].Rows[0]["SCLGRP_GRP_DESC"].ToString().Trim())).Desc.Trim();
                                    if (GrpDesc != null)
                                        GroupDesc = GrpDesc.ToString().Trim();
                                }
                                cell = Row0.Cells.Add(GroupDesc, DataType.String, "s23");
                                //cell = Row0.Cells.Add(GroupDesc(((ListItem)Cmb_Matrix.SelectedItem).Value.ToString().Trim(), dsMatb.Tables[j].Rows[0]["SCALE"].ToString()), DataType.String, "s23");
                            }
                            else if (j > 1)
                                cell = Row0.Cells.Add(dsMatb.Tables[j].Rows[0]["SCALE"].ToString(), DataType.String, "s23");
                            else cell = Row0.Cells.Add("Total Percentages", DataType.String, "s23");

                            bool IsboolAll = true;
                            string PrivSite = string.Empty, privDate = string.Empty; int i = 0,k=0;
                            foreach (DataRow drProg in dsMatb.Tables[j].Rows)
                            {
                                if (IsboolAll)
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add("Site", DataType.String, "s98");
                                    cell = Row0.Cells.Add("Assessment", DataType.String, "s98");

                                    foreach (DataColumn dc in dsMatb.Tables[j].Columns)
                                    {
                                        if (dc.ColumnName != "Assesment" && dc.ColumnName != "SCALE" && dc.ColumnName != "MST_SITE" && dc.ColumnName != "SCLGRP_GRP_DESC")
                                        {
                                            string ColName=string.Empty;
                                            if (Benchmarks_List.Count > 0)
                                                ColName = Benchmarks_List.Find(u => u.Low.Equals(dc.ColumnName)).Desc.Trim();
                                            cell = Row0.Cells.Add(ColName.Trim(), DataType.String, "s98");
                                        }
                                    }
                                    cell = Row0.Cells.Add("Total % Change", DataType.String, "s23");
                                    cell = Row0.Cells.Add("Total % Meeting " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString().Trim() + " Goals", DataType.String, "s23");

                                    IsboolAll = false;
                                }
                                int RowTotValue = 0;

                                Row0 = sheet.Table.Rows.Add();
                                foreach (DataColumn col in dsMatb.Tables[j].Columns)
                                {
                                    if (!drProg.IsNull(col))
                                    {
                                        if (col.ColumnName != "Assesment" && col.ColumnName != "SCALE" && col.ColumnName != "MST_SITE" && col.ColumnName != "SCLGRP_GRP_DESC")
                                        {
                                            string stringValue = drProg[col].ToString();
                                            int d;
                                            if (int.TryParse(stringValue, out d))
                                                RowTotValue += d;
                                        }
                                    }
                                }
                                string Assesment = string.Empty;
                                if (PrivSite != drProg["MST_SITE"].ToString().Trim())
                                {
                                    //Row0 = sheet.Table.Rows.Add();
                                    k++;
                                    Assesment = "CP Beginning of the Year";
                                    if (k == 1)
                                    {
                                        cell = Row0.Cells.Add(drProg["MST_SITE"].ToString().Trim(), DataType.String, "s27");

                                        cell = Row0.Cells.Add(Assesment, DataType.String, "s25");
                                    }
                                    else if (k == 2)
                                    {
                                        cell = Row0.Cells.Add(drProg["MST_SITE"].ToString().Trim(), DataType.String, "s20");

                                        cell = Row0.Cells.Add(Assesment, DataType.String, "s19");
                                    }

                                    i = 2; PrivSite = drProg["MST_SITE"].ToString().Trim();
                                   
                                }
                                else
                                {
                                    switch (drProg["Assesment"].ToString().Trim())
                                    {
                                        case "2": Assesment = "CP Middle of the Year"; break;
                                        case "3": Assesment = "CP End of the Year"; break;
                                        case "4": Assesment = "4th Assessment"; break;
                                        case "5": Assesment = "5th Assessment"; break;
                                        case "6": Assesment = "6th Assessment"; break;
                                        case "7": Assesment = "7th Assessment"; break;
                                        case "8": Assesment = "8th Assessment"; break;
                                        case "9": Assesment = "9th Assessment"; break;
                                    }
                                    if (k == 1)
                                    {
                                        cell = Row0.Cells.Add(drProg["MST_SITE"].ToString().Trim(), DataType.String, "s27");
                                        cell = Row0.Cells.Add(Assesment, DataType.String, "s25");
                                    }
                                    else
                                    {
                                        cell = Row0.Cells.Add(drProg["MST_SITE"].ToString().Trim(), DataType.String, "s20");
                                        cell = Row0.Cells.Add(Assesment, DataType.String, "s19");
                                    }
                                    i++;
                                }

                                foreach (DataColumn col in dsMatb.Tables[j].Columns)
                                {
                                    if (col.ColumnName != "Assesment" && col.ColumnName != "SCALE" && col.ColumnName != "MST_SITE")
                                    {
                                        string stringValue = drProg[col].ToString();
                                        int d;
                                        if (int.TryParse(stringValue, out d))
                                        {
                                            decimal Percent = (Convert.ToDecimal(d) / Convert.ToDecimal(RowTotValue)); //Convert.ToDecimal(((d * 100) / RowTotValue));
                                            if(k==1)
                                                cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s26");
                                            else cell = Row0.Cells.Add(Percent.ToString(), DataType.Number, "s17");
                                        }
                                    }
                                }

                                if (k == 1)
                                {
                                    if (Assesment != "CP Beginning of the Year")
                                    {
                                        cell = Row0.Cells.Add("0", DataType.Number, "s28");
                                        cell.Formula = "=((RC[-2])-(R[-1]C[-2]))+((RC[-1])-(R[-1]C[-1]))";
                                        //cell.Formula = "=((RC[-8]+RC[-7]+RC[-6])-(R[-1]C[-8]+R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4]+RC[-3])-(R[-1]C[-5]+R[-1]C[-4]+R[-1]C[-3]))";
                                    }
                                    else cell = Row0.Cells.Add("", DataType.String, "s24");


                                    cell = Row0.Cells.Add("0", DataType.Number, "s26");
                                    cell.Formula = "=SUM(100%-(RC[-4]))";
                                }
                                else
                                {
                                    if (Assesment != "CP Beginning of the Year")
                                    {
                                        cell = Row0.Cells.Add("0", DataType.Number, "s21");
                                        cell.Formula = "=((RC[-2])-(R[-1]C[-2]))+((RC[-1])-(R[-1]C[-1]))";
                                        //cell.Formula = "=((RC[-8]+RC[-7]+RC[-6])-(R[-1]C[-8]+R[-1]C[-7]+R[-1]C[-6]))+((RC[-5]+RC[-4]+RC[-3])-(R[-1]C[-5]+R[-1]C[-4]+R[-1]C[-3]))";
                                    }
                                    else cell = Row0.Cells.Add("", DataType.String, "s19");


                                    cell = Row0.Cells.Add("0", DataType.Number, "s17");
                                    cell.Formula = "=SUM(100%-(RC[-4]))";//"=SUM(100%-(RC[-3]+RC[-2]))";
                                }
                                if (k == 2) k = 0;

                            }

                            //j = j + 1;
                        }
                    }
                    #endregion

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();

                    AlertBox.Show("Report Generated Successfully");
                }
                catch (Exception ex) { }

                //Generate(PdfName);
            }
        }

        private void GenerateWorksheetParameters(Workbook book)
        {
            WorksheetCell cell;
            Worksheet sheet = book.Worksheets.Add("Parameters");
            #region Header Page
            WorksheetColumn columnHead = sheet.Table.Columns.Add();
            columnHead.Index = 2;
            columnHead.Width = 5;
            sheet.Table.Columns.Add(163);
            WorksheetColumn column2Head = sheet.Table.Columns.Add();
            column2Head.Width = 370;//332;
            column2Head.StyleID = "s172";
            sheet.Table.Columns.Add(100);//(59);
            // -----------------------------------------------
            //  s137
            // -----------------------------------------------
            WorksheetStyle s137 = book.Styles.Add("s137");
            s137.Name = "Normal 3";
            s137.Font.FontName = "Calibri";
            s137.Font.Size = 11;
            s137.Font.Color = "#000000";
            s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m2611536909264
            // -----------------------------------------------
            WorksheetStyle m2611536909264 = book.Styles.Add("m2611536909264");
            m2611536909264.Parent = "s137";
            m2611536909264.Font.FontName = "Arial";
            m2611536909264.Font.Color = "#9400D3";
            m2611536909264.Interior.Color = "#FFFFFF";
            m2611536909264.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909264.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909264.Alignment.WrapText = true;
            m2611536909264.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909264.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909284
            // -----------------------------------------------
            WorksheetStyle m2611536909284 = book.Styles.Add("m2611536909284");
            m2611536909284.Parent = "s137";
            m2611536909284.Font.FontName = "Arial";
            m2611536909284.Font.Color = "#9400D3";
            m2611536909284.Interior.Color = "#FFFFFF";
            m2611536909284.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909284.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909284.Alignment.WrapText = true;
            m2611536909284.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909284.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909304
            // -----------------------------------------------
            WorksheetStyle m2611536909304 = book.Styles.Add("m2611536909304");
            m2611536909304.Parent = "s137";
            m2611536909304.Font.FontName = "Arial";
            m2611536909304.Font.Color = "#9400D3";
            m2611536909304.Interior.Color = "#FFFFFF";
            m2611536909304.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909304.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909304.Alignment.WrapText = true;
            m2611536909304.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909304.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909324
            // -----------------------------------------------
            WorksheetStyle m2611536909324 = book.Styles.Add("m2611536909324");
            m2611536909324.Parent = "s137";
            m2611536909324.Font.FontName = "Arial";
            m2611536909324.Font.Color = "#9400D3";
            m2611536909324.Interior.Color = "#FFFFFF";
            m2611536909324.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909324.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909324.Alignment.WrapText = true;
            m2611536909324.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909324.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909344
            // -----------------------------------------------
            WorksheetStyle m2611536909344 = book.Styles.Add("m2611536909344");
            m2611536909344.Parent = "s137";
            m2611536909344.Font.FontName = "Arial";
            m2611536909344.Font.Color = "#9400D3";
            m2611536909344.Interior.Color = "#FFFFFF";
            m2611536909344.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909344.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909344.Alignment.WrapText = true;
            m2611536909344.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909344.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549552
            // -----------------------------------------------
            WorksheetStyle m2611540549552 = book.Styles.Add("m2611540549552");
            m2611540549552.Parent = "s137";
            m2611540549552.Font.FontName = "Arial";
            m2611540549552.Font.Color = "#9400D3";
            m2611540549552.Interior.Color = "#FFFFFF";
            m2611540549552.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549552.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549552.Alignment.WrapText = true;
            m2611540549552.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549572
            // -----------------------------------------------
            WorksheetStyle m2611540549572 = book.Styles.Add("m2611540549572");
            m2611540549572.Parent = "s137";
            m2611540549572.Font.FontName = "Arial";
            m2611540549572.Font.Color = "#9400D3";
            m2611540549572.Interior.Color = "#FFFFFF";
            m2611540549572.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549572.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549572.Alignment.WrapText = true;
            m2611540549572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2611540549572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549592
            // -----------------------------------------------
            WorksheetStyle m2611540549592 = book.Styles.Add("m2611540549592");
            m2611540549592.Parent = "s137";
            m2611540549592.Font.FontName = "Arial";
            m2611540549592.Font.Color = "#9400D3";
            m2611540549592.Interior.Color = "#FFFFFF";
            m2611540549592.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549592.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549592.Alignment.WrapText = true;
            m2611540549592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549612
            // -----------------------------------------------
            WorksheetStyle m2611540549612 = book.Styles.Add("m2611540549612");
            m2611540549612.Parent = "s137";
            m2611540549612.Font.FontName = "Arial";
            m2611540549612.Font.Color = "#9400D3";
            m2611540549612.Interior.Color = "#FFFFFF";
            m2611540549612.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549612.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549612.Alignment.WrapText = true;
            m2611540549612.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549632
            // -----------------------------------------------
            WorksheetStyle m2611540549632 = book.Styles.Add("m2611540549632");
            m2611540549632.Parent = "s137";
            m2611540549632.Font.FontName = "Arial";
            m2611540549632.Font.Color = "#9400D3";
            m2611540549632.Interior.Color = "#FFFFFF";
            m2611540549632.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549632.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549632.Alignment.WrapText = true;
            m2611540549632.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549632.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549652
            // -----------------------------------------------
            WorksheetStyle m2611540549652 = book.Styles.Add("m2611540549652");
            m2611540549652.Parent = "s137";
            m2611540549652.Font.FontName = "Arial";
            m2611540549652.Font.Color = "#9400D3";
            m2611540549652.Interior.Color = "#FFFFFF";
            m2611540549652.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549652.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549652.Alignment.WrapText = true;
            m2611540549652.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549652.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549672
            // -----------------------------------------------
            WorksheetStyle m2611540549672 = book.Styles.Add("m2611540549672");
            m2611540549672.Parent = "s137";
            m2611540549672.Font.FontName = "Arial";
            m2611540549672.Font.Color = "#9400D3";
            m2611540549672.Interior.Color = "#FFFFFF";
            m2611540549672.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549672.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549672.Alignment.WrapText = true;
            m2611540549672.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s139
            // -----------------------------------------------
            WorksheetStyle s139 = book.Styles.Add("s139");
            s139.Parent = "s137";
            s139.Font.FontName = "Calibri";
            s139.Font.Size = 11;
            s139.Interior.Color = "#FFFFFF";
            s139.Interior.Pattern = StyleInteriorPattern.Solid;
            s139.Alignment.Vertical = StyleVerticalAlignment.Top;
            s139.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s140
            // -----------------------------------------------
            WorksheetStyle s140 = book.Styles.Add("s140");
            s140.Parent = "s137";
            s140.Font.FontName = "Calibri";
            s140.Font.Size = 11;
            s140.Interior.Color = "#FFFFFF";
            s140.Interior.Pattern = StyleInteriorPattern.Solid;
            s140.Alignment.Vertical = StyleVerticalAlignment.Top;
            s140.Alignment.WrapText = true;
            s140.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s140.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s141
            // -----------------------------------------------
            WorksheetStyle s141 = book.Styles.Add("s141");
            s141.Parent = "s137";
            s141.Font.FontName = "Calibri";
            s141.Font.Size = 11;
            s141.Interior.Color = "#FFFFFF";
            s141.Interior.Pattern = StyleInteriorPattern.Solid;
            s141.Alignment.Vertical = StyleVerticalAlignment.Top;
            s141.Alignment.WrapText = true;
            s141.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s142
            // -----------------------------------------------
            WorksheetStyle s142 = book.Styles.Add("s142");
            s142.Parent = "s137";
            s142.Font.FontName = "Calibri";
            s142.Font.Size = 11;
            s142.Interior.Color = "#FFFFFF";
            s142.Interior.Pattern = StyleInteriorPattern.Solid;
            s142.Alignment.Vertical = StyleVerticalAlignment.Top;
            s142.Alignment.WrapText = true;
            s142.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s142.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s143
            // -----------------------------------------------
            WorksheetStyle s143 = book.Styles.Add("s143");
            s143.Parent = "s137";
            s143.Font.FontName = "Calibri";
            s143.Font.Size = 11;
            s143.Interior.Color = "#FFFFFF";
            s143.Interior.Pattern = StyleInteriorPattern.Solid;
            s143.Alignment.Vertical = StyleVerticalAlignment.Top;
            s143.Alignment.WrapText = true;
            s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s144
            // -----------------------------------------------
            WorksheetStyle s144 = book.Styles.Add("s144");
            s144.Parent = "s137";
            s144.Font.FontName = "Arial";
            s144.Font.Color = "#9400D3";
            s144.Interior.Color = "#FFFFFF";
            s144.Interior.Pattern = StyleInteriorPattern.Solid;
            s144.Alignment.Vertical = StyleVerticalAlignment.Top;
            s144.Alignment.WrapText = true;
            s144.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s145
            // -----------------------------------------------
            WorksheetStyle s145 = book.Styles.Add("s145");
            s145.Parent = "s137";
            s145.Font.FontName = "Calibri";
            s145.Font.Size = 11;
            s145.Interior.Color = "#FFFFFF";
            s145.Interior.Pattern = StyleInteriorPattern.Solid;
            s145.Alignment.Vertical = StyleVerticalAlignment.Top;
            s145.Alignment.WrapText = true;
            s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s146
            // -----------------------------------------------
            WorksheetStyle s146 = book.Styles.Add("s146");
            s146.Parent = "s137";
            s146.Font.FontName = "Calibri";
            s146.Font.Size = 11;
            s146.Interior.Color = "#FFFFFF";
            s146.Interior.Pattern = StyleInteriorPattern.Solid;
            s146.Alignment.Vertical = StyleVerticalAlignment.Top;
            s146.Alignment.WrapText = true;
            s146.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s146.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s169
            // -----------------------------------------------
            WorksheetStyle s169 = book.Styles.Add("s169");
            s169.Parent = "s137";
            s169.Font.FontName = "Arial";
            s169.Font.Color = "#9400D3";
            s169.Interior.Color = "#FFFFFF";
            s169.Interior.Pattern = StyleInteriorPattern.Solid;
            s169.Alignment.Vertical = StyleVerticalAlignment.Top;
            s169.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s170
            // -----------------------------------------------
            WorksheetStyle s170 = book.Styles.Add("s170");
            s170.Parent = "s137";
            s170.Font.FontName = "Calibri";
            s170.Font.Size = 11;
            s170.Interior.Color = "#FFFFFF";
            s170.Interior.Pattern = StyleInteriorPattern.Solid;
            s170.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171 = book.Styles.Add("s171");
            s171.Parent = "s137";
            s171.Font.FontName = "Calibri";
            s171.Font.Size = 11;
            s171.Interior.Color = "#FFFFFF";
            s171.Interior.Pattern = StyleInteriorPattern.Solid;
            s171.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172 = book.Styles.Add("s172");
            s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

            // -----------------------------------------------
            WorksheetRow RowHead = sheet.Table.Rows.Add();
            //WorksheetCell cell;
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s170";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            // -----------------------------------------------
            WorksheetRow Row1Head = sheet.Table.Rows.Add();
            Row1Head.Height = 14;
            Row1Head.AutoFitHeight = false;
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s140";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s141";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s171";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s142";
            // -----------------------------------------------
            WorksheetRow Row2Head = sheet.Table.Rows.Add();
            Row2Head.Height = 14;
            Row2Head.AutoFitHeight = false;
            cell = Row2Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row2Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row2Head.Cells.Add();
            cell.StyleID = "m2611536909264";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 14;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row101 = sheet.Table.Rows.Add();
            Row101.Height = 14;
            Row101.AutoFitHeight = false;
            cell = Row101.Cells.Add();
            cell.StyleID = "s139";
            cell = Row101.Cells.Add();
            cell.StyleID = "s143";
            cell = Row101.Cells.Add();
            cell.StyleID = "m2611540549592";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row88Head = sheet.Table.Rows.Add();
            Row88Head.Height = 14;
            Row88Head.AutoFitHeight = false;
            cell = Row88Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row88Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row88Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row88Head.Cells.Add();
            cell.StyleID = "s170";
            cell = Row88Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row3Head = sheet.Table.Rows.Add();
            Row3Head.Height = 14;
            Row3Head.AutoFitHeight = false;
            cell = Row3Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row3Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row3Head.Cells.Add();
            cell.StyleID = "m2611536909284";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Selected Report Parameters";
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row4Head = sheet.Table.Rows.Add();
            Row4Head.Height = 14;
            Row4Head.AutoFitHeight = false;
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s170";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row5Head = sheet.Table.Rows.Add();
            Row5Head.Height = 14;
            Row5Head.AutoFitHeight = false;
            cell = Row5Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row5Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row5Head.Cells.Add();
            cell.StyleID = "m2611536909304";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row6Head = sheet.Table.Rows.Add();
            Row6Head.Height = 14;
            Row6Head.AutoFitHeight = false;
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s170";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            string Header_year = string.Empty;

            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            WorksheetRow Row78Head = sheet.Table.Rows.Add();
            Row78Head.Height = 14;
            Row78Head.AutoFitHeight = false;
            cell = Row78Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row78Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row78Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            Hierarchy: " + Txt_HieDesc.Text.Trim() + "   " + Header_year;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row10 = sheet.Table.Rows.Add();
            Row10.Height = 14;
            Row10.AutoFitHeight = false;
            cell = Row10.Cells.Add();
            cell.StyleID = "s139";
            cell = Row10.Cells.Add();
            cell.StyleID = "s143";
            cell = Row10.Cells.Add();
            cell.StyleID = "m2611540549592";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row11 = sheet.Table.Rows.Add();
            Row11.Height = 14;
            Row11.AutoFitHeight = false;
            cell = Row11.Cells.Add();
            cell.StyleID = "s139";
            cell = Row11.Cells.Add();
            cell.StyleID = "s143";
            cell = Row11.Cells.Add();
            cell.StyleID = "s139";
            cell = Row11.Cells.Add();
            cell.StyleID = "s170";
            cell = Row11.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow ReportType = sheet.Table.Rows.Add();
            ReportType.Height = 14;
            ReportType.AutoFitHeight = false;
            cell = ReportType.Cells.Add();
            cell.StyleID = "s139";
            cell = ReportType.Cells.Add();
            cell.StyleID = "s143";
            ReportType.Cells.Add("           Matrix", DataType.String, "s144");
            ReportType.Cells.Add(": " + ((Captain.Common.Utilities.ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "s169");
            cell = ReportType.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row13 = sheet.Table.Rows.Add();
            Row13.Height = 14;
            Row13.AutoFitHeight = false;
            cell = Row13.Cells.Add();
            cell.StyleID = "s139";
            cell = Row13.Cells.Add();
            cell.StyleID = "s143";
            cell = Row13.Cells.Add();
            cell.StyleID = "s139";
            cell = Row13.Cells.Add();
            cell.StyleID = "s170";
            cell = Row13.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            string Scale = Rb_All_Scales.Checked == true ? Rb_All_Scales.Text.Trim() : Rb_Sel_Scales.Text.Trim();

            WorksheetRow ScaleType = sheet.Table.Rows.Add();
            ScaleType.Height = 14;
            ScaleType.AutoFitHeight = false;
            cell = ScaleType.Cells.Add();
            cell.StyleID = "s139";
            cell = ScaleType.Cells.Add();
            cell.StyleID = "s143";
            ScaleType.Cells.Add("            Scale", DataType.String, "s144");
            ScaleType.Cells.Add(": " + Scale, DataType.String, "s169");
            cell = ScaleType.Cells.Add();
            cell.StyleID = "s145";

            // -----------------------------------------------
            WorksheetRow Row133 = sheet.Table.Rows.Add();
            Row133.Height = 14;
            Row133.AutoFitHeight = false;
            cell = Row133.Cells.Add();
            cell.StyleID = "s139";
            cell = Row133.Cells.Add();
            cell.StyleID = "s143";
            cell = Row133.Cells.Add();
            cell.StyleID = "s139";
            cell = Row133.Cells.Add();
            cell.StyleID = "s170";
            cell = Row133.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow ChkP1 = sheet.Table.Rows.Add();
            ChkP1.Height = 14;
            ChkP1.AutoFitHeight = false;
            cell = ChkP1.Cells.Add();
            cell.StyleID = "s139";
            cell = ChkP1.Cells.Add();
            cell.StyleID = "s143";
            ChkP1.Cells.Add("           Checkpoint #1 From", DataType.String, "s144");
            ChkP1.Cells.Add(": " + Asmt_F_Date.Value.ToShortDateString() + "   To: " + Asmt_T_Date.Value.ToShortDateString(), DataType.String, "s169");
            cell = ChkP1.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row013 = sheet.Table.Rows.Add();
            Row013.Height = 14;
            Row013.AutoFitHeight = false;
            cell = Row013.Cells.Add();
            cell.StyleID = "s139";
            cell = Row013.Cells.Add();
            cell.StyleID = "s143";
            cell = Row013.Cells.Add();
            cell.StyleID = "s139";
            cell = Row013.Cells.Add();
            cell.StyleID = "s170";
            cell = Row013.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow ChkP2 = sheet.Table.Rows.Add();
            ChkP2.Height = 14;
            ChkP2.AutoFitHeight = false;
            cell = ChkP2.Cells.Add();
            cell.StyleID = "s139";
            cell = ChkP2.Cells.Add();
            cell.StyleID = "s143";
            ChkP2.Cells.Add("           Checkpoint #2 From", DataType.String, "s144");
            ChkP2.Cells.Add(": " + dtpCP2From.Value.ToShortDateString() + "   To: " + dtpCP2To.Value.ToShortDateString(), DataType.String, "s169");
            cell = ChkP2.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row103 = sheet.Table.Rows.Add();
            Row103.Height = 14;
            Row103.AutoFitHeight = false;
            cell = Row103.Cells.Add();
            cell.StyleID = "s139";
            cell = Row103.Cells.Add();
            cell.StyleID = "s143";
            cell = Row103.Cells.Add();
            cell.StyleID = "s139";
            cell = Row103.Cells.Add();
            cell.StyleID = "s170";
            cell = Row103.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow ChkP3 = sheet.Table.Rows.Add();
            ChkP3.Height = 14;
            ChkP3.AutoFitHeight = false;
            cell = ChkP3.Cells.Add();
            cell.StyleID = "s139";
            cell = ChkP3.Cells.Add();
            cell.StyleID = "s143";
            ChkP3.Cells.Add("           Checkpoint #3 From", DataType.String, "s144");
            ChkP3.Cells.Add(": " + dtpCP3From.Value.ToShortDateString() + "   To: " + dtpCP3To.Value.ToShortDateString(), DataType.String, "s169");
            cell = ChkP3.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row111 = sheet.Table.Rows.Add();
            Row111.Height = 14;
            Row111.AutoFitHeight = false;
            cell = Row111.Cells.Add();
            cell.StyleID = "s139";
            cell = Row111.Cells.Add();
            cell.StyleID = "s143";
            cell = Row111.Cells.Add();
            cell.StyleID = "s139";
            cell = Row111.Cells.Add();
            cell.StyleID = "s170";
            cell = Row111.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow CseWorker = sheet.Table.Rows.Add();
            CseWorker.Height = 14;
            CseWorker.AutoFitHeight = false;
            cell = CseWorker.Cells.Add();
            cell.StyleID = "s139";
            cell = CseWorker.Cells.Add();
            cell.StyleID = "s143";
            CseWorker.Cells.Add("           Case Worker", DataType.String, "s144");
            CseWorker.Cells.Add(": " + ((Captain.Common.Utilities.ListItem)Cmb_Worker.SelectedItem).Text.Trim(), DataType.String, "s169");
            cell = CseWorker.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row24 = sheet.Table.Rows.Add();
            Row24.Height = 14;
            Row24.AutoFitHeight = false;
            cell = Row24.Cells.Add();
            cell.StyleID = "s139";
            cell = Row24.Cells.Add();
            cell.StyleID = "s143";
            cell = Row24.Cells.Add();
            cell.StyleID = "m2611540549632";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row25 = sheet.Table.Rows.Add();
            Row25.Height = 14;
            Row25.AutoFitHeight = false;
            cell = Row25.Cells.Add();
            cell.StyleID = "s139";
            cell = Row25.Cells.Add();
            cell.StyleID = "s143";
            cell = Row25.Cells.Add();
            cell.StyleID = "m2611540549652";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row26Head = sheet.Table.Rows.Add();
            Row26Head.Height = 14;
            Row26Head.AutoFitHeight = false;
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s170";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row27Head = sheet.Table.Rows.Add();
            Row27Head.Height = 14;
            Row27Head.AutoFitHeight = false;
            cell = Row27Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row27Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row27Head.Cells.Add();
            cell.StyleID = "m2611540549672";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row28 = sheet.Table.Rows.Add();
            Row28.Height = 14;
            Row28.AutoFitHeight = false;
            cell = Row28.Cells.Add();
            cell.StyleID = "s139";
            cell = Row28.Cells.Add();
            cell.StyleID = "s143";
            cell = Row28.Cells.Add();
            cell.StyleID = "s139";
            cell = Row28.Cells.Add();
            cell.StyleID = "s170";
            cell = Row28.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row29 = sheet.Table.Rows.Add();
            Row29.Height = 14;
            Row29.AutoFitHeight = false;
            cell = Row29.Cells.Add();
            cell.StyleID = "s139";
            cell = Row29.Cells.Add();
            cell.StyleID = "s143";
            cell = Row29.Cells.Add();
            cell.StyleID = "m2611540549552";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row30 = sheet.Table.Rows.Add();
            Row30.Height = 14;
            Row30.AutoFitHeight = false;
            cell = Row30.Cells.Add();
            cell.StyleID = "s139";
            cell = Row30.Cells.Add();
            cell.StyleID = "s143";
            cell = Row30.Cells.Add();
            cell.StyleID = "s139";
            cell = Row30.Cells.Add();
            cell.StyleID = "s170";
            cell = Row30.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row31 = sheet.Table.Rows.Add();
            Row31.Height = 14;
            Row31.AutoFitHeight = false;
            cell = Row31.Cells.Add();
            cell.StyleID = "s139";
            cell = Row31.Cells.Add();
            cell.StyleID = "s146";
            cell = Row31.Cells.Add();
            cell.StyleID = "m2611540549572";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.Selected = true;
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;

            #endregion
        }

        public void Generate(string filename)
        {
            Workbook book = new Workbook();
            // -----------------------------------------------
            //  Properties
            // -----------------------------------------------
            book.Properties.Created = new System.DateTime(2016, 10, 19, 0, 56, 58, 0);
            book.Properties.LastSaved = new System.DateTime(2016, 10, 26, 2, 29, 4, 0);
            book.Properties.Version = "12.00";
            book.ExcelWorkbook.WindowHeight = 7755;
            book.ExcelWorkbook.WindowWidth = 20490;
            book.ExcelWorkbook.WindowTopX = 0;
            book.ExcelWorkbook.WindowTopY = 0;
            book.ExcelWorkbook.ActiveSheetIndex = 2;
            book.ExcelWorkbook.ProtectWindows = false;
            book.ExcelWorkbook.ProtectStructure = false;
            // -----------------------------------------------
            //  Generate Styles
            // -----------------------------------------------
            this.GenerateStyles(book.Styles);
            // -----------------------------------------------
            //  Generate Total Percentages Worksheet
            // -----------------------------------------------
            ////this.GenerateWorksheetTotalPercentages(book.Worksheets);
            // -----------------------------------------------
            //  Generate Approaches to Learning Worksheet
            // -----------------------------------------------
            ////this.GenerateWorksheetApproachestoLearning(book.Worksheets);
            // -----------------------------------------------
            //  Generate Data Worksheet
            // -----------------------------------------------
            ////this.GenerateWorksheetData(book.Worksheets);
            //this.GenerateWorkData(book.Worksheets);
            book.Save(filename);
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";
        }

        //private void GenerateWorkData(WorksheetCollection sheets)
        //{
        //    Worksheet sheet = sheets.Add("Data");
        //    //sheet.Names.Add(new WorksheetNamedRange("_FilterDatabase", "=Data!R1C1:R360C11", true));
        //    sheet.Table.DefaultColumnWidth = 220.5F;
        //    sheet.Table.ExpandedColumnCount = 11;
        //    sheet.Table.ExpandedRowCount = 360;
        //    sheet.Table.FullColumns = 1;
        //    sheet.Table.FullRows = 1;
        //    sheet.Table.Columns.Add(52);
        //    sheet.Table.Columns.Add(148);
        //    sheet.Table.Columns.Add(45);
        //    sheet.Table.Columns.Add(113);
        //    sheet.Table.Columns.Add(139);
        //    sheet.Table.Columns.Add(84);
        //    WorksheetColumn column6 = sheet.Table.Columns.Add();
        //    column6.Index = 8;
        //    column6.Width = 65;
        //    sheet.Table.Columns.Add(34);
        //    sheet.Table.Columns.Add(53);
        //    sheet.Table.Columns.Add(86);

        //    WorksheetRow Row0 = sheet.Table.Rows.Add();
        //    WorksheetCell cell;
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Fam.ID";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Name";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Gender";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Ethnicity";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Race";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Matrix";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Scale";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Response";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Points";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Birth Date";
        //    //cell.NamedCell.Add("_FilterDatabase");
        //    cell = Row0.Cells.Add();
        //    cell.StyleID = "s94";
        //    cell.Data.Type = DataType.String;
        //    cell.Data.Text = "Site";
        //    //cell.NamedCell.Add("_FilterDatabase");

        //    if (MatRepList.Count > 0)
        //    {
        //        foreach (MATB0003Entity Entity in MatRepList)
        //        {
        //            WorksheetRow Row1 = sheet.Table.Rows.Add();
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s95";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.MST_FAMILY_ID;
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.Name;
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.SNP_SEX;
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.ETHNIC.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.RACE.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.MatDesc.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.SclcodeDesc.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.MATASMT_RESP_DESC.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.Number;
        //            cell.Data.Text = Entity.Points.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s95";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = LookupDataAccess.Getdate(Entity.SNP_ALT_BDATE.Trim());
        //            //cell.NamedCell.Add("_FilterDatabase");
        //            cell = Row1.Cells.Add();
        //            cell.StyleID = "s96";
        //            cell.Data.Type = DataType.String;
        //            cell.Data.Text = Entity.Site.Trim();
        //            //cell.NamedCell.Add("_FilterDatabase");
        //        }

        //        sheet.Options.Selected = true;
        //        sheet.Options.ProtectObjects = false;
        //        sheet.Options.ProtectScenarios = false;
        //        sheet.Options.PageSetup.Layout.Orientation = CarlosAg.ExcelXmlWriter.Orientation.Landscape;
        //        sheet.Options.PageSetup.Header.Margin = 0.2F;
        //        sheet.Options.PageSetup.Footer.Data = "&L&C&\"Arial\"&10&P &R";
        //        sheet.Options.PageSetup.Footer.Margin = 0.2F;
        //        sheet.Options.PageSetup.PageMargins.Bottom = 0.5508299F;
        //        sheet.Options.PageSetup.PageMargins.Left = 0.2F;
        //        sheet.Options.PageSetup.PageMargins.Right = 0.2F;
        //        sheet.Options.PageSetup.PageMargins.Top = 0.2F;
        //        sheet.Options.Print.VerticalResolution = 0;
        //        sheet.Options.Print.ValidPrinterInfo = true;
        //        //sheet.AutoFilter.Range = "R1C1:R360C11";
        //        sheet.Sorting.Add("Emerging (3)");
        //    }
            
        //}

        public string GroupDesc(string strMatcode, string strGroupId)
        {
            string strDesc = string.Empty;
            if (propGroup_List.Count > 0)
            {
                MATGroupEntity matgroupdata = propGroup_List.Find(u => u.MatCode.ToString().Trim() == strMatcode.ToString().Trim() && u.Code.ToString().Trim() == strGroupId.ToString().Trim());
                if (matgroupdata != null)
                    strDesc = matgroupdata.Desc;
            }
            return strDesc;
        }

    }
}