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
using System.Xml;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using DevExpress.XtraPrinting.Design;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB0009_Report : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public CASB0009_Report(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm; Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();

            this.Text = /*Privileges.Program + " - " + */Privileges.PrivilegeName.Trim();

            Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));
            Rep_From_Date.Checked = Rep_To_Date.Checked = true;

            Getdata();

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }


        public string propReportPath { get; set; }


        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }

        public List<CommonEntity> propfundingSource { get; set; }

        public List<SalquesEntity> SALQUESEntity { get; set; }
        public List<SalqrespEntity> SALQUESRespEntity { get; set; }

        public List<SaldefEntity> SALDEF { get; set; }

        public List<SPCommonEntity> FundingList { get; set; }
        public List<CommonEntity> Status { get; set; }
        public List<CommonEntity> Location { get; set; }
        public List<CommonEntity> Recipient { get; set; }
        public List<CommonEntity> Attn { get; set; }

        public List<HierarchyEntity> propCaseworkerList = new List<HierarchyEntity>();


        #endregion

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;

        private void Getdata()
        {
            SaldefEntity Search_saldef_Entity = new SaldefEntity(true);
            SALDEF = _model.SALDEFData.Browse_SALDEF(Search_saldef_Entity, "Browse", BaseForm.UserID, BaseForm.BaseAdminAgency); //vikash added user id and adim agy

            SalquesEntity Search_Salques_Entity = new SalquesEntity(true);
            SALQUESEntity = _model.SALDEFData.Browse_SALQUES(Search_Salques_Entity, "Browse");

            SalqrespEntity Search_Salqresp_Entity = new SalqrespEntity(true);
            SALQUESRespEntity = _model.SALDEFData.Browse_SALQRESP(Search_Salqresp_Entity, "Browse");

            FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");

            propCaseworkerList = _model.CaseMstData.GetCaseWorker("I", "**", "**", "**");

            Status = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALStatus, string.Empty, string.Empty, string.Empty);
            Location = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALLocation, string.Empty, string.Empty, string.Empty);
            Recipient = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALRecipient, string.Empty, string.Empty, string.Empty);
            Attn = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.SALAttendence, string.Empty, string.Empty, string.Empty);
        }

        string Program_Year;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            txtHieDesc.Clear();
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
                        txtHieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                txtHieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        txtHieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                txtHieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        txtHieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                txtHieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
            {
                this.txtHieDesc.Size = new System.Drawing.Size(687, 25);
                Agency = Agy; Depart = Dept; Program = Prog; //strYear = Year;
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
                    if (!(String.IsNullOrEmpty(DepYear)) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
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
            //fillBusCombo(Agency, Depart, Program, strYear);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.txtHieDesc.Size = new System.Drawing.Size(606, 25);
            else
                this.txtHieDesc.Size = new System.Drawing.Size(687, 25);
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }

        }





        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", " ", "D", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", " ", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";

        private void rdoSelected_Click(object sender, EventArgs e)
        {
            if (rdoSelected.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelsalcalList, Privileges, Agency, Depart, Program, rbCAL.Checked == true ? "CAL" : "SAL");
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        List<SaldefEntity> SelsalcalList = new List<SaldefEntity>();
        private void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelsalcalList = form.GetSelectedSALs();
            }
        }

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
                    propReportPath = _model.lookupDataAccess.GetReportPath();
                }
            }
        }
        #region Vikash added for Save and Get Parameters 12/02/2022
        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
        }
        private void btnGetParameters_Click(object sender, EventArgs e)
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
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);


            }
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());
                if (dr["Type"].ToString() == "C")
                {
                    rbCAL.Checked = true;
                }
                else
                {
                    rbSAL.Checked = true;
                }
                chkHeader.Checked = (dr["PRINTHEADER"].ToString() == "Y" ? true : false);

                //switch (dr["SALCAL"].ToString())
                //{
                //    case "*": rdoAll.Checked = true; break;
                //    default: rdoSelected.Checked = true; break;
                //}
                if (dr["SALCAL"].ToString() == "Y")
                    rdoAll.Checked = true;
                else
                {
                    rdoSelected.Checked = true;
                    SelsalcalList.Clear();
                    string strSALCAL = dr["SALCALCodes"].ToString();
                    List<string> siteList1 = new List<string>();
                    if (strSALCAL != null)
                    {
                        string[] SALCALCodes = strSALCAL.Split(',');
                        for (int i = 0; i < SALCALCodes.Length; i++)
                        {
                            SaldefEntity SALCALDetails = SALDEF.Find(u => u.SALD_NAME == SALCALCodes.GetValue(i).ToString());
                            SelsalcalList.Add(SALCALDetails);
                        }
                    }
                    SelsalcalList = SelsalcalList;
                }
                Rep_From_Date.Checked = Rep_To_Date.Checked = true;
                Rep_From_Date.Value = Convert.ToDateTime(dr["FROMDATE"].ToString());
                Rep_To_Date.Value = Convert.ToDateTime(dr["TODATE"].ToString());

            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string Type = rbCAL.Checked ? "C" : "S";

            string strSALCAL = rdoAll.Checked ? "Y" : "N";
            string strSALCALCodes = string.Empty;

            if (rdoSelected.Checked == true)
            {
                foreach (SaldefEntity SALCALName in SelsalcalList)
                {
                    if (!strSALCALCodes.Equals(string.Empty)) strSALCALCodes += ",";
                    strSALCALCodes += SALCALName.SALD_NAME;
                }
            }

        StringBuilder str = new StringBuilder();
        str.Append("<Rows>");

            str.Append("<Row AGENCY =" +
                " \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                "\" YEAR = \"" + Program_Year +
                "\" Type = \"" + (rbCAL.Checked ? "C" : "S") + "\" PRINTHEADER = \"" + (chkHeader.Checked? "Y" : "N") +
                "\" SALCAL = \"" + strSALCAL + "\" SALCALCodes = \"" + strSALCALCodes +
                "\" FROMDATE = \"" + Rep_From_Date.Value.ToShortDateString() + "\" TODATE = \"" + Rep_To_Date.Value.ToShortDateString() + "\"/>");
            str.Append("</Rows>");

            return str.ToString();
        }

        #endregion


        string strSALCodes = string.Empty;
        DataSet ds = new DataSet();
        //DataTable dtSummary = new DataTable();
        //DataTable dt = new DataTable();
        private void BtnGenPdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                strSALCodes = string.Empty;
                if (rdoSelected.Checked == true)
                {
                    foreach (SaldefEntity Entity in SelsalcalList)
                    {
                        if (!strSALCodes.Equals(string.Empty)) strSALCodes += ",";
                        strSALCodes += Entity.SALD_ID;
                    }
                }
                else
                {
                    strSALCodes = "A";
                }


                ds = DatabaseLayer.SALDB.GetCase0009_REPORT(Agency, Depart, Program, Year, Rep_From_Date.Value.ToShortDateString(), Rep_To_Date.Value.ToShortDateString(),rbCAL.Checked==true?"C":"S",strSALCodes);
                
                if(ds.Tables.Count>0)
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"XLS");
                    pdfListForm.FormClosed += new FormClosedEventHandler(OnExcel_Report);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();

                    AlertBox.Show("Report Generated Successfully");
                }
                else
                {
                    AlertBox.Show("No Records Found", MessageBoxIcon.Warning);
                }
            }
        }

        private void rbSAL_Click(object sender, EventArgs e)
        {
            if (rbSAL.Checked)
                chkHeader.Visible = true;
            else chkHeader.Visible = false;
        }

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Name = "Normal 2";
            s62.Font.FontName = "Calibri";
            s62.Font.Size = 11;
            s62.Font.Color = "#000000";
            s62.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m1698893805328
            // -----------------------------------------------
            WorksheetStyle m1698893805328 = styles.Add("m1698893805328");
            m1698893805328.Parent = "s62";
            m1698893805328.Font.FontName = "Arial";
            m1698893805328.Font.Color = "#000000";
            m1698893805328.Interior.Color = "#FFFFFF";
            m1698893805328.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893805328.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893805328.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893805328.Alignment.WrapText = true;
            m1698893805328.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893805328.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893805328.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893805328.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893805328.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804912
            // -----------------------------------------------
            WorksheetStyle m1698893804912 = styles.Add("m1698893804912");
            m1698893804912.Parent = "s62";
            m1698893804912.Font.FontName = "Arial";
            m1698893804912.Font.Color = "#000000";
            m1698893804912.Interior.Color = "#FFFFFF";
            m1698893804912.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804912.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804912.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804912.Alignment.WrapText = true;
            m1698893804912.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804912.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804912.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804912.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804912.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804932
            // -----------------------------------------------
            WorksheetStyle m1698893804932 = styles.Add("m1698893804932");
            m1698893804932.Parent = "s62";
            m1698893804932.Font.FontName = "Arial";
            m1698893804932.Font.Color = "#000000";
            m1698893804932.Interior.Color = "#FFFFFF";
            m1698893804932.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804932.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804932.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804932.Alignment.WrapText = true;
            m1698893804932.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804932.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804932.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804932.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804932.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804952
            // -----------------------------------------------
            WorksheetStyle m1698893804952 = styles.Add("m1698893804952");
            m1698893804952.Parent = "s62";
            m1698893804952.Font.FontName = "Arial";
            m1698893804952.Font.Color = "#000000";
            m1698893804952.Interior.Color = "#FFFFFF";
            m1698893804952.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804952.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804952.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804952.Alignment.WrapText = true;
            m1698893804952.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804952.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804952.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804952.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804952.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804972
            // -----------------------------------------------
            WorksheetStyle m1698893804972 = styles.Add("m1698893804972");
            m1698893804972.Parent = "s62";
            m1698893804972.Font.FontName = "Arial";
            m1698893804972.Font.Color = "#000000";
            m1698893804972.Interior.Color = "#FFFFFF";
            m1698893804972.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804972.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804972.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804972.Alignment.WrapText = true;
            m1698893804972.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804972.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804972.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804972.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804972.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804992
            // -----------------------------------------------
            WorksheetStyle m1698893804992 = styles.Add("m1698893804992");
            m1698893804992.Parent = "s62";
            m1698893804992.Font.FontName = "Arial";
            m1698893804992.Font.Color = "#000000";
            m1698893804992.Interior.Color = "#FFFFFF";
            m1698893804992.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804992.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804992.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804992.Alignment.WrapText = true;
            m1698893804992.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804992.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804992.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804992.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804992.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893805012
            // -----------------------------------------------
            WorksheetStyle m1698893805012 = styles.Add("m1698893805012");
            m1698893805012.Parent = "s62";
            m1698893805012.Font.FontName = "Arial";
            m1698893805012.Font.Color = "#000000";
            m1698893805012.Interior.Color = "#FFFFFF";
            m1698893805012.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893805012.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893805012.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893805012.Alignment.WrapText = true;
            m1698893805012.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893805012.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893805012.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893805012.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893805012.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804744
            // -----------------------------------------------
            WorksheetStyle m1698893804744 = styles.Add("m1698893804744");
            m1698893804744.Parent = "s62";
            m1698893804744.Font.FontName = "Arial";
            m1698893804744.Font.Color = "#000000";
            m1698893804744.Interior.Color = "#FFFFFF";
            m1698893804744.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804744.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804744.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804744.Alignment.WrapText = true;
            m1698893804744.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804744.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804744.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804744.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804744.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804784
            // -----------------------------------------------
            WorksheetStyle m1698893804784 = styles.Add("m1698893804784");
            m1698893804784.Parent = "s62";
            m1698893804784.Font.Bold = true;
            m1698893804784.Font.FontName = "Arial";
            m1698893804784.Font.Color = "#000000";
            m1698893804784.Interior.Color = "#D3D3D3";
            m1698893804784.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804784.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m1698893804784.Alignment.Vertical = StyleVerticalAlignment.Center;
            m1698893804784.Alignment.WrapText = true;
            m1698893804784.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804784.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804784.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804784.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804784.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804804
            // -----------------------------------------------
            WorksheetStyle m1698893804804 = styles.Add("m1698893804804");
            m1698893804804.Parent = "s62";
            m1698893804804.Font.FontName = "Arial";
            m1698893804804.Font.Color = "#000000";
            m1698893804804.Interior.Color = "#FFFFFF";
            m1698893804804.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804804.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804804.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804804.Alignment.WrapText = true;
            m1698893804804.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804804.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804804.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804804.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804804.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s137
            // -----------------------------------------------
            WorksheetStyle s137 = styles.Add("s137");
            s137.Name = "Normal 3";
            s137.Font.FontName = "Calibri";
            s137.Font.Size = 11;
            s137.Font.Color = "#000000";
            s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m2611536909264
            // -----------------------------------------------
            WorksheetStyle m2611536909264 = styles.Add("m2611536909264");
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
            WorksheetStyle m2611536909284 = styles.Add("m2611536909284");
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
            WorksheetStyle m2611536909304 = styles.Add("m2611536909304");
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
            WorksheetStyle m2611536909324 = styles.Add("m2611536909324");
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
            WorksheetStyle m2611536909344 = styles.Add("m2611536909344");
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
            WorksheetStyle m2611540549552 = styles.Add("m2611540549552");
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
            WorksheetStyle m2611540549572 = styles.Add("m2611540549572");
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
            WorksheetStyle m2611540549592 = styles.Add("m2611540549592");
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
            WorksheetStyle m2611540549612 = styles.Add("m2611540549612");
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
            WorksheetStyle m2611540549632 = styles.Add("m2611540549632");
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
            WorksheetStyle m2611540549652 = styles.Add("m2611540549652");
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
            WorksheetStyle m2611540549672 = styles.Add("m2611540549672");
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
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Parent = "s62";
            s63.Font.FontName = "Calibri";
            s63.Font.Size = 11;
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Parent = "s62";
            s64.Font.Bold = true;
            s64.Font.FontName = "Calibri";
            s64.Font.Size = 11;
            s64.Font.Color = "#FF0000";
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Parent = "s62";
            s65.Font.Bold = true;
            s65.Font.FontName = "Arial";
            s65.Font.Color = "#7030A0";
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s65.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Parent = "s62";
            s66.Font.FontName = "Calibri";
            s66.Font.Size = 11;
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s66.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Parent = "s62";
            s67.Font.FontName = "Calibri";
            s67.Font.Size = 11;
            s67.Font.Color = "#7030A0";
            s67.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s67.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Parent = "s62";
            s68.Font.FontName = "Calibri";
            s68.Font.Size = 11;
            s68.Font.Color = "#7030A0";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Parent = "s62";
            s69.Font.FontName = "Calibri";
            s69.Font.Size = 11;
            s69.Font.Color = "#7030A0";
            s69.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s69.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Parent = "s62";
            s70.Font.FontName = "Calibri";
            s70.Font.Size = 11;
            s70.Interior.Color = "#FFFFFF";
            s70.Interior.Pattern = StyleInteriorPattern.Solid;
            s70.Alignment.Vertical = StyleVerticalAlignment.Top;
            s70.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Parent = "s62";
            s71.Font.Bold = true;
            s71.Font.FontName = "Arial";
            s71.Font.Color = "#000000";
            s71.Interior.Color = "#D3D3D3";
            s71.Interior.Pattern = StyleInteriorPattern.Solid;
            s71.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s71.Alignment.Vertical = StyleVerticalAlignment.Center;
            s71.Alignment.WrapText = true;
            s71.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s71.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s71.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s71.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.Parent = "s62";
            s73.Font.Bold = true;
            s73.Font.FontName = "Arial";
            s73.Font.Color = "#000000";
            s73.Interior.Color = "#B0C4DE";
            s73.Interior.Pattern = StyleInteriorPattern.Solid;
            s73.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s73.Alignment.Vertical = StyleVerticalAlignment.Center;
            s73.Alignment.WrapText = true;
            s73.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s73.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s73.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s73.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s73.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Parent = "s62";
            s74.Font.Bold = true;
            s74.Font.FontName = "Arial";
            s74.Font.Color = "#000000";
            s74.Interior.Color = "#B0C4DE";
            s74.Interior.Pattern = StyleInteriorPattern.Solid;
            s74.Alignment.Rotate = -90;
            s74.Alignment.Vertical = StyleVerticalAlignment.Top;
            s74.Alignment.WrapText = true;
            s74.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s74.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s74.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Parent = "s62";
            s75.Font.FontName = "Arial";
            s75.Font.Color = "#000000";
            s75.Interior.Color = "#FFFFFF";
            s75.Interior.Pattern = StyleInteriorPattern.Solid;
            s75.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s75.Alignment.Vertical = StyleVerticalAlignment.Top;
            s75.Alignment.WrapText = true;
            s75.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s75.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Parent = "s62";
            s76.Font.FontName = "Arial";
            s76.Font.Color = "#000000";
            s76.Interior.Color = "#FFFFFF";
            s76.Interior.Pattern = StyleInteriorPattern.Solid;
            s76.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s76.Alignment.Vertical = StyleVerticalAlignment.Top;
            s76.Alignment.WrapText = true;
            s76.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s76.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Parent = "s62";
            s77.Font.FontName = "Arial";
            s77.Font.Color = "#000000";
            s77.Interior.Color = "#FFFFFF";
            s77.Interior.Pattern = StyleInteriorPattern.Solid;
            s77.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s77.Alignment.Vertical = StyleVerticalAlignment.Top;
            s77.Alignment.WrapText = true;
            s77.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Parent = "s62";
            s79.Font.FontName = "Arial";
            s79.Font.Color = "#000000";
            s79.Interior.Color = "#FFFFFF";
            s79.Interior.Pattern = StyleInteriorPattern.Solid;
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s79.Alignment.Vertical = StyleVerticalAlignment.Top;
            s79.Alignment.WrapText = true;
            s79.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s79.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s80
            // -----------------------------------------------
            WorksheetStyle s80 = styles.Add("s80");
            s80.Parent = "s62";
            s80.Font.FontName = "Arial";
            s80.Font.Color = "#000000";
            s80.Interior.Color = "#FFFFFF";
            s80.Interior.Pattern = StyleInteriorPattern.Solid;
            s80.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s80.Alignment.Vertical = StyleVerticalAlignment.Top;
            s80.Alignment.WrapText = true;
            s80.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Parent = "s62";
            s81.Font.FontName = "Calibri";
            s81.Font.Size = 11;
            s81.Interior.Color = "#FFFFFF";
            s81.Interior.Pattern = StyleInteriorPattern.Solid;
            s81.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s81.Alignment.Vertical = StyleVerticalAlignment.Top;
            s81.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Parent = "s62";
            s82.Font.FontName = "Arial";
            s82.Font.Color = "#000000";
            s82.Interior.Color = "#FFFFFF";
            s82.Interior.Pattern = StyleInteriorPattern.Solid;
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s82.Alignment.Vertical = StyleVerticalAlignment.Top;
            s82.Alignment.WrapText = true;
            s82.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s83
            // -----------------------------------------------
            WorksheetStyle s83 = styles.Add("s83");
            s83.Parent = "s62";
            s83.Font.Bold = true;
            s83.Font.FontName = "Arial";
            s83.Font.Color = "#000000";
            s83.Interior.Color = "#D3D3D3";
            s83.Interior.Pattern = StyleInteriorPattern.Solid;
            s83.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s83.Alignment.Vertical = StyleVerticalAlignment.Center;
            s83.Alignment.WrapText = true;
            s83.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s83.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s83.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s83.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s85
            // -----------------------------------------------
            WorksheetStyle s85 = styles.Add("s85");
            s85.Parent = "s62";
            s85.Font.FontName = "Arial";
            s85.Font.Color = "#000000";
            s85.Interior.Color = "#FFFFFF";
            s85.Interior.Pattern = StyleInteriorPattern.Solid;
            s85.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s85.Alignment.Vertical = StyleVerticalAlignment.Top;
            s85.Alignment.WrapText = true;
            s85.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s85.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Parent = "s62";
            s93.Font.FontName = "Calibri";
            s93.Font.Size = 11;
            s93.Interior.Color = "#FFFFFF";
            s93.Interior.Pattern = StyleInteriorPattern.Solid;
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s93.Alignment.Vertical = StyleVerticalAlignment.Top;
            s93.Alignment.WrapText = true;
            s93.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Parent = "s62";
            s95.Font.Bold = true;
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#D3D3D3";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Parent = "s62";
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s133
            // -----------------------------------------------
            WorksheetStyle s133 = styles.Add("s133");
            s133.Font.Bold = true;
            s133.Font.FontName = "Tahoma";
            s133.Font.Size = 12;
            s133.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s133.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s134
            // -----------------------------------------------
            WorksheetStyle s134 = styles.Add("s134");
            s134.Font.Bold = true;
            s134.Font.FontName = "Tahoma";
            s134.Font.Size = 12;
            s134.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s134.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s135
            // -----------------------------------------------
            WorksheetStyle s135 = styles.Add("s135");
            s135.Font.FontName = "Tahoma";
            // -----------------------------------------------
            //  s136
            // -----------------------------------------------
            WorksheetStyle s136 = styles.Add("s136");
            s136.Font.FontName = "Tahoma";
            s136.Font.Color = "#0000FF";
            s136.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s136.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s139
            // -----------------------------------------------
            WorksheetStyle s139 = styles.Add("s139");
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
            WorksheetStyle s140 = styles.Add("s140");
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
            WorksheetStyle s141 = styles.Add("s141");
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
            WorksheetStyle s142 = styles.Add("s142");
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
            WorksheetStyle s143 = styles.Add("s143");
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
            WorksheetStyle s144 = styles.Add("s144");
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
            WorksheetStyle s145 = styles.Add("s145");
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
            WorksheetStyle s146 = styles.Add("s146");
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
            WorksheetStyle s169 = styles.Add("s169");
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
            WorksheetStyle s170 = styles.Add("s170");
            s170.Parent = "s137";
            s170.Font.FontName = "Calibri";
            s170.Font.Size = 11;
            s170.Interior.Color = "#FFFFFF";
            s170.Interior.Pattern = StyleInteriorPattern.Solid;
            s170.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171 = styles.Add("s171");
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
            WorksheetStyle s172 = styles.Add("s172");
            s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

        }


        private bool ValidateForm()
        {
            bool isValid = true;

            if (rdoSelected.Checked == true)
            {
                if (SelsalcalList.Count == 0)
                {
                    _errorProvider.SetError(rdoSelected, "Select at least One SAL/CAL");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoSelected, null);
                }
            }
            else
                _errorProvider.SetError(rdoSelected, null);


            if (!Rep_From_Date.Checked)
            {
                _errorProvider.SetError(Rep_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Rep_From_Date, null);

            if (!Rep_To_Date.Checked)
            {
                _errorProvider.SetError(Rep_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (Rep_From_Date.Checked)
                    _errorProvider.SetError(Rep_To_Date, null);
            }
            //**
            if (Rep_From_Date.Checked.Equals(true) && Rep_To_Date.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(Rep_From_Date.Text))
                {
                    _errorProvider.SetError(Rep_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Rep_From_Date, null);
                }
                if (string.IsNullOrWhiteSpace(Rep_To_Date.Text))
                {
                    _errorProvider.SetError(Rep_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date ".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Rep_To_Date, null);
                }
            }

            if (Rep_To_Date.Checked && Rep_From_Date.Checked)
            {
                if (Rep_From_Date.Value > Rep_To_Date.Value)
                {
                    _errorProvider.SetError(Rep_From_Date, string.Format("'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
            }
            else
            {
                if (Rep_To_Date.Checked && Rep_From_Date.Checked)
                    _errorProvider.SetError(Rep_From_Date, null);
            }

            return (isValid);
        }


        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        private void OnExcel_Report1(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();

                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
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
                Worksheet sheet;

                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Tahoma";
                styleb.Font.Size = 12;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle style3 = book.Styles.Add("WrapText");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 10;
                style3.Alignment.WrapText = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                style = book.Styles.Add("Default");
                style.Font.FontName = "Tahoma";
                style.Alignment.WrapText = true;
                style.Font.Size = 10;

                int S = 1;
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = new DataTable();
                    if (ds.Tables[i].Rows.Count > 0)
                        dt = ds.Tables[i];
                    int ColCount = 0;
                    
                    if (dt.Rows.Count > 0)
                    {
                        sheet = book.Worksheets.Add("Sheet"+(S));
                        S++;

                        int QuesCount = Convert.ToInt32(dt.Rows[0]["QUES_COUNT"].ToString().Trim());

                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(180));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(210));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(210));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        ColCount++;
                        if (chkHeader.Checked)
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(80));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(80));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(80));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(120));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(120));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                        }
                        for(int j=0;j<QuesCount;j++)
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                        }


                        try
                        {
                            WorksheetRow row = sheet.Table.Rows.Add();

                            string SAL_Name = string.Empty;
                            if (SALDEF.Count > 0) SAL_Name = SALDEF.Find(u => u.SALD_ID.Equals(dt.Rows[0]["SALACT_SALID"].ToString().Trim())).SALD_NAME.Trim();

                            WorksheetCell cell = row.Cells.Add(SAL_Name, DataType.String, "HeaderStyleBlue");
                            cell.MergeAcross = ColCount-1;

                            row = sheet.Table.Rows.Add();

                            
                            row.Cells.Add(new WorksheetCell("Ag", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("De", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Pr", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Year", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("App #", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Name (first and last)", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Service Plan", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Activity", "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Act_Date", "HeaderStyle"));
                            if(chkHeader.Checked)
                            {
                                row.Cells.Add(new WorksheetCell("Time In", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Time Out", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Time Spent", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Fund", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Staff", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Status", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Recipient", "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell("Attendance", "HeaderStyle"));
                            }

                            for (int z = QuesCount; z >0; z--)
                            {
                                string QuesName = string.Empty;
                                if (SALQUESEntity.Count > 0) QuesName = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count-z].ColumnName)).SALQ_DESC.Trim();
                                row.Cells.Add(new WorksheetCell(QuesName, "HeaderStyle"));
                            }


                            string PrivApp = string.Empty;string PrivSalID = string.Empty;
                            foreach(DataRow dr in dt.Rows)
                            {
                                if(dr["CASEACT_ID"].ToString().Trim()!=PrivApp && dr["SALACT_SALID"].ToString().Trim()!= PrivSalID)
                                {
                                    row = sheet.Table.Rows.Add();

                                    row.Cells.Add(new WorksheetCell(dr["MST_AGENCY"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["MST_DEPT"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["MST_PROGRAM"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["MST_YEAR"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["SNP_NAME_IX_FI"].ToString() + " " + dr["SNP_NAME_IX_LAST"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["SP_NAME"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["ACT_NAME"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CASEACT_ACTY_DATE"].ToString().Trim()), "Default"));

                                    if(chkHeader.Checked)
                                    {
                                        row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_IN"].ToString(), "Default"));
                                        row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_OUT"].ToString(), "Default"));
                                        row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_SPENT"].ToString(), "Default"));

                                        string FundName = string.Empty;
                                        if (FundingList.Count>0 && !string.IsNullOrEmpty(dr["CASEACT_FUND1"].ToString().Trim()))
                                        {
                                            FundName = FundingList.Find(u => u.Code.Equals(dr["CASEACT_FUND1"].ToString().Trim())).Desc.Trim();
                                        }


                                        string StaffName = string.Empty;
                                        HierarchyEntity hiedata = propCaseworkerList.Find(u => u.CaseWorker.ToString() == dr["CASEACT_CASEWRKR"].ToString().Trim());
                                        if (hiedata != null)
                                        {
                                            StaffName = hiedata.HirarchyName.ToString().Trim();
                                        }

                                        string StatusDesc = string.Empty;
                                        if (Status.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_STATUS"].ToString().Trim()))
                                        {
                                            StatusDesc = Status.Find(u => u.Code.Equals(dr["SALACT_STATUS"].ToString().Trim())).Desc.Trim();
                                        }

                                        string LocationDesc = string.Empty;
                                        if (Location.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_LOCATION"].ToString().Trim()))
                                        {
                                            LocationDesc = Location.Find(u => u.Code.Equals(dr["SALACT_LOCATION"].ToString().Trim())).Desc.Trim();
                                        }

                                        string RecipentDesc = string.Empty;
                                        if (Recipient.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_RECIPIENT"].ToString().Trim()))
                                        {
                                            RecipentDesc = Recipient.Find(u => u.Code.Equals(dr["SALACT_RECIPIENT"].ToString().Trim())).Desc.Trim();
                                        }

                                        string AttnDesc = string.Empty;
                                        if (Attn.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_ATTN"].ToString().Trim()))
                                        {
                                            AttnDesc = Attn.Find(u => u.Code.Equals(dr["SALACT_ATTN"].ToString().Trim())).Desc.Trim();
                                        }

                                        row.Cells.Add(new WorksheetCell(FundName, "Default"));
                                        row.Cells.Add(new WorksheetCell(StaffName, "Default"));
                                        row.Cells.Add(new WorksheetCell(StatusDesc, "Default"));
                                        row.Cells.Add(new WorksheetCell(LocationDesc, "Default"));
                                        row.Cells.Add(new WorksheetCell(RecipentDesc, "Default"));
                                        row.Cells.Add(new WorksheetCell(AttnDesc, "Default"));
                                    }

                                    for (int z = QuesCount; z > 0; z--)
                                    {
                                        string QuesType = string.Empty;string QuesCode = string.Empty;string RespDesc = string.Empty;
                                        
                                        if (SALQUESEntity.Count > 0)
                                        {
                                            QuesType = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_TYPE.Trim();
                                            QuesCode = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_ID.Trim();
                                        }
                                        
                                        if(QuesType=="D")
                                        {
                                            RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString().Trim();
                                            if (SALQUESRespEntity.Count>0)
                                            {
                                                RespDesc = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(QuesCode.Trim()) && u.SALQR_CODE.Trim().Equals(RespDesc.Trim())).SALQR_DESC.Trim();
                                            }
                                        }
                                        else if(QuesType=="C")
                                        {
                                            RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString().Trim();
                                            string CheckResponse = string.Empty;
                                            string response = RespDesc;
                                            string[] arrResponse = null;
                                            if (response.IndexOf(',') > 0)
                                            {
                                                arrResponse = response.Split(',');
                                            }
                                            else if (!response.Equals(string.Empty))
                                            {
                                                arrResponse = new string[] { response };
                                            }
                                            if (SALQUESRespEntity.Count > 0)
                                            {
                                                List<SalqrespEntity> SALResponseEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(QuesCode.Trim()));
                                                if(SALResponseEntity.Count>0)
                                                {
                                                    foreach (SalqrespEntity Entity in SALResponseEntity)
                                                    {
                                                        
                                                        string resDesc = Entity.SALQR_CODE.ToString().Trim();


                                                        if (arrResponse != null && arrResponse.ToList().Exists(u => u.Trim().Equals(resDesc)))
                                                        {
                                                            CheckResponse = CheckResponse + Entity.SALQR_DESC.Trim() + ",";
                                                        }
                                                    }
                                                }
                                            }
                                            if(!string.IsNullOrEmpty(CheckResponse.Trim()))
                                                RespDesc = CheckResponse.Substring(0, CheckResponse.Length - 1);
                                        }
                                        else
                                            RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString();

                                        row.Cells.Add(new WorksheetCell(RespDesc, "Default"));


                                    }


                                    PrivApp = dr["CASEACT_ID"].ToString().Trim(); PrivSalID = dr["SALACT_SALID"].ToString().Trim();
                                }
                            }



                            FileStream stream = new FileStream(PdfName, FileMode.Create);

                            book.Save(stream);
                            stream.Close();
                        }
                        catch (Exception ex) { }

                    }

                    

                }

            }
        }

        private void OnExcel_Report(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();

                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
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
                Worksheet sheet;

                // -----------------------------------------------
                //  Default
                // -----------------------------------------------
                WorksheetStyle Default = book.Styles.Add("Default");
                Default.Name = "Normal";
                Default.Font.FontName = "Calibri";
                Default.Font.Size = 11;
                Default.Font.Color = "#000000";
                Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                // -----------------------------------------------
                //  m386123900
                // -----------------------------------------------
                WorksheetStyle m386123900 = book.Styles.Add("m386123900");
                m386123900.Font.FontName = "Calibri";
                m386123900.Font.Size = 12;
                m386123900.Font.Color = "#000000";
                m386123900.Interior.Color = "#F2DCDB";
                m386123900.Interior.Pattern = StyleInteriorPattern.Solid;
                m386123900.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m386123900.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                m386123900.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                m386123900.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                m386123900.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                m386123900.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  m386123920
                // -----------------------------------------------
                WorksheetStyle m386123920 = book.Styles.Add("m386123920");
                m386123920.Font.Bold = true;
                m386123920.Font.FontName = "Calibri";
                m386123920.Font.Size = 11;
                m386123920.Font.Color = "#000000";
                m386123920.Interior.Color = "#DCE6F1";
                m386123920.Interior.Pattern = StyleInteriorPattern.Solid;
                m386123920.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m386123920.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                m386123920.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                m386123920.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                m386123920.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                m386123920.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  m386123940
                // -----------------------------------------------
                WorksheetStyle m386123940 = book.Styles.Add("m386123940");
                m386123940.Font.Bold = true;
                m386123940.Font.FontName = "Calibri";
                m386123940.Font.Size = 12;
                m386123940.Font.Color = "#000000";
                m386123940.Interior.Color = "#FCD5B4";
                m386123940.Interior.Pattern = StyleInteriorPattern.Solid;
                m386123940.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m386123940.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                m386123940.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                m386123940.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                m386123940.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s16
                // -----------------------------------------------
                WorksheetStyle s16 = book.Styles.Add("s16");
                s16.Font.Bold = true;
                s16.Font.FontName = "Calibri";
                s16.Font.Size = 11;
                s16.Font.Color = "#000000";
                s16.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s17
                // -----------------------------------------------
                WorksheetStyle s17 = book.Styles.Add("s17");
                s17.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s18
                // -----------------------------------------------
                WorksheetStyle s18 = book.Styles.Add("s18");
                s18.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s18.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s21
                // -----------------------------------------------
                WorksheetStyle s21 = book.Styles.Add("s21");
                s21.Alignment.Vertical = StyleVerticalAlignment.Center;
                s21.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s21.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s21.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s21.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s23
                // -----------------------------------------------
                WorksheetStyle s23 = book.Styles.Add("s23");
                s23.Alignment.Vertical = StyleVerticalAlignment.Center;
                s23.Alignment.WrapText = true;
                s23.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s23.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s23.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s23.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s24
                // -----------------------------------------------
                WorksheetStyle s24 = book.Styles.Add("s24");
                s24.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s24.Alignment.Vertical = StyleVerticalAlignment.Center;
                s24.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s24.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s24.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                s24.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s24.NumberFormat = "Short Date";
                // -----------------------------------------------
                //  s25
                // -----------------------------------------------
                WorksheetStyle s25 = book.Styles.Add("s25");
                s25.Alignment.Vertical = StyleVerticalAlignment.Center;
                s25.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s25.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s25.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s25.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s27
                // -----------------------------------------------
                WorksheetStyle s27 = book.Styles.Add("s27");
                s27.Alignment.Vertical = StyleVerticalAlignment.Center;
                s27.Alignment.WrapText = true;
                s27.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s27.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s27.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s27.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s28
                // -----------------------------------------------
                WorksheetStyle s28 = book.Styles.Add("s28");
                s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s28.Alignment.Vertical = StyleVerticalAlignment.Center;
                s28.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s28.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s28.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                s28.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s28.NumberFormat = "Short Date";
                // -----------------------------------------------
                //  s29
                // -----------------------------------------------
                WorksheetStyle s29 = book.Styles.Add("s29");
                s29.Font.Bold = true;
                s29.Font.FontName = "Calibri";
                s29.Font.Size = 11;
                s29.Font.Color = "#000000";
                s29.Interior.Color = "#D8E4BC";
                s29.Interior.Pattern = StyleInteriorPattern.Solid;
                s29.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s29.Alignment.Vertical = StyleVerticalAlignment.Center;
                s29.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s30
                // -----------------------------------------------
                WorksheetStyle s30 = book.Styles.Add("s30");
                s30.Font.Bold = true;
                s30.Font.FontName = "Calibri";
                s30.Font.Size = 11;
                s30.Font.Color = "#000000";
                s30.Interior.Color = "#D8E4BC";
                s30.Interior.Pattern = StyleInteriorPattern.Solid;
                s30.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s30.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s31
                // -----------------------------------------------
                WorksheetStyle s31 = book.Styles.Add("s31");
                s31.Font.Bold = true;
                s31.Font.FontName = "Calibri";
                s31.Font.Size = 11;
                s31.Font.Color = "#000000";
                s31.Interior.Color = "#D8E4BC";
                s31.Interior.Pattern = StyleInteriorPattern.Solid;
                s31.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s31.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s32
                // -----------------------------------------------
                WorksheetStyle s32 = book.Styles.Add("s32");
                s32.Interior.Color = "#EBF1DE";
                s32.Interior.Pattern = StyleInteriorPattern.Solid;
                s32.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s32.Alignment.Vertical = StyleVerticalAlignment.Center;
                s32.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s32.NumberFormat = "Medium Time";
                // -----------------------------------------------
                //  s33
                // -----------------------------------------------
                WorksheetStyle s33 = book.Styles.Add("s33");
                s33.Interior.Color = "#EBF1DE";
                s33.Interior.Pattern = StyleInteriorPattern.Solid;
                s33.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s33.Alignment.Vertical = StyleVerticalAlignment.Center;
                s33.NumberFormat = "Medium Time";
                // -----------------------------------------------
                //  s34
                // -----------------------------------------------
                WorksheetStyle s34 = book.Styles.Add("s34");
                s34.Interior.Color = "#EBF1DE";
                s34.Interior.Pattern = StyleInteriorPattern.Solid;
                s34.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s34.Alignment.Vertical = StyleVerticalAlignment.Center;
                s34.NumberFormat = "Short Time";
                // -----------------------------------------------
                //  s35
                // -----------------------------------------------
                WorksheetStyle s35 = book.Styles.Add("s35");
                s35.Interior.Color = "#EBF1DE";
                s35.Interior.Pattern = StyleInteriorPattern.Solid;
                s35.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s35.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s36
                // -----------------------------------------------
                WorksheetStyle s36 = book.Styles.Add("s36");
                s36.Interior.Color = "#EBF1DE";
                s36.Interior.Pattern = StyleInteriorPattern.Solid;
                s36.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s37
                // -----------------------------------------------
                WorksheetStyle s37 = book.Styles.Add("s37");
                s37.Interior.Color = "#EBF1DE";
                s37.Interior.Pattern = StyleInteriorPattern.Solid;
                s37.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s37.Alignment.Vertical = StyleVerticalAlignment.Center;
                s37.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s37.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s37.NumberFormat = "Medium Time";
                // -----------------------------------------------
                //  s38
                // -----------------------------------------------
                WorksheetStyle s38 = book.Styles.Add("s38");
                s38.Interior.Color = "#EBF1DE";
                s38.Interior.Pattern = StyleInteriorPattern.Solid;
                s38.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s38.Alignment.Vertical = StyleVerticalAlignment.Center;
                s38.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s38.NumberFormat = "Medium Time";
                // -----------------------------------------------
                //  s39
                // -----------------------------------------------
                WorksheetStyle s39 = book.Styles.Add("s39");
                s39.Interior.Color = "#EBF1DE";
                s39.Interior.Pattern = StyleInteriorPattern.Solid;
                s39.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s39.Alignment.Vertical = StyleVerticalAlignment.Center;
                s39.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s39.NumberFormat = "Short Time";
                // -----------------------------------------------
                //  s40
                // -----------------------------------------------
                WorksheetStyle s40 = book.Styles.Add("s40");
                s40.Interior.Color = "#EBF1DE";
                s40.Interior.Pattern = StyleInteriorPattern.Solid;
                s40.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s40.Alignment.Vertical = StyleVerticalAlignment.Center;
                s40.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s41
                // -----------------------------------------------
                WorksheetStyle s41 = book.Styles.Add("s41");
                s41.Interior.Color = "#EBF1DE";
                s41.Interior.Pattern = StyleInteriorPattern.Solid;
                s41.Alignment.Vertical = StyleVerticalAlignment.Center;
                s41.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s45
                // -----------------------------------------------
                WorksheetStyle s45 = book.Styles.Add("s45");
                s45.Font.Bold = true;
                s45.Font.FontName = "Calibri";
                s45.Font.Size = 11;
                s45.Font.Color = "#000000";
                s45.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s45.Alignment.Vertical = StyleVerticalAlignment.Top;
                s45.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s45.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s45.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s46
                // -----------------------------------------------
                WorksheetStyle s46 = book.Styles.Add("s46");
                s46.Font.Bold = true;
                s46.Font.FontName = "Calibri";
                s46.Font.Size = 11;
                s46.Font.Color = "#000000";
                s46.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s46.Alignment.Vertical = StyleVerticalAlignment.Center;
                s46.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s46.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s46.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s53
                // -----------------------------------------------
                WorksheetStyle s53 = book.Styles.Add("s53");
                s53.Font.Bold = true;
                s53.Font.FontName = "Calibri";
                s53.Font.Size = 11;
                s53.Font.Color = "#000000";
                s53.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s53.Alignment.Vertical = StyleVerticalAlignment.Top;
                s53.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s53.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s53.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s53.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s54
                // -----------------------------------------------
                WorksheetStyle s54 = book.Styles.Add("s54");
                s54.Font.Bold = true;
                s54.Font.FontName = "Calibri";
                s54.Font.Size = 11;
                s54.Font.Color = "#000000";
                s54.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s54.Alignment.Vertical = StyleVerticalAlignment.Top;
                s54.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s54.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s54.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s54.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s55
                // -----------------------------------------------
                WorksheetStyle s55 = book.Styles.Add("s55");
                s55.Font.Bold = true;
                s55.Font.FontName = "Calibri";
                s55.Font.Size = 11;
                s55.Font.Color = "#000000";
                s55.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s55.Alignment.Vertical = StyleVerticalAlignment.Top;
                s55.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s55.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s55.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                s55.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
                // -----------------------------------------------
                //  s56
                // -----------------------------------------------
                WorksheetStyle s56 = book.Styles.Add("s56");
                s56.Alignment.Vertical = StyleVerticalAlignment.Center;
                s56.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s56.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s56.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s56.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s57
                // -----------------------------------------------
                WorksheetStyle s57 = book.Styles.Add("s57");
                s57.Alignment.Vertical = StyleVerticalAlignment.Center;
                s57.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s57.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s57.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                s57.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s58
                // -----------------------------------------------
                WorksheetStyle s58 = book.Styles.Add("s58");
                s58.Alignment.Vertical = StyleVerticalAlignment.Center;
                s58.Alignment.WrapText = true;
                s58.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s58.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s58.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                s58.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s59
                // -----------------------------------------------
                WorksheetStyle s59 = book.Styles.Add("s59");
                s59.Alignment.Vertical = StyleVerticalAlignment.Center;
                s59.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s59.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s59.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s59.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s60
                // -----------------------------------------------
                WorksheetStyle s60 = book.Styles.Add("s60");
                s60.Alignment.Vertical = StyleVerticalAlignment.Center;
                s60.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s60.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s60.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                s60.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s61
                // -----------------------------------------------
                WorksheetStyle s61 = book.Styles.Add("s61");
                s61.Font.Bold = true;
                s61.Font.FontName = "Calibri";
                s61.Font.Size = 11;
                s61.Font.Color = "#000000";
                s61.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s61.Alignment.Vertical = StyleVerticalAlignment.Center;
                s61.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s61.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s61.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s62
                // -----------------------------------------------
                WorksheetStyle s62 = book.Styles.Add("s62");
                s62.Font.Bold = true;
                s62.Font.FontName = "Calibri";
                s62.Font.Size = 11;
                s62.Font.Color = "#000000";
                s62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s62.Alignment.Vertical = StyleVerticalAlignment.Center;
                s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s63
                // -----------------------------------------------
                WorksheetStyle s63 = book.Styles.Add("s63");
                s63.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s63.Alignment.Vertical = StyleVerticalAlignment.Center;
                s63.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s63.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s64
                // -----------------------------------------------
                WorksheetStyle s64 = book.Styles.Add("s64");
                s64.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s64.Alignment.Vertical = StyleVerticalAlignment.Center;
                s64.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s64.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s65
                // -----------------------------------------------
                WorksheetStyle s65 = book.Styles.Add("s65");
                s65.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s65.Alignment.Vertical = StyleVerticalAlignment.Center;
                s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s66
                // -----------------------------------------------
                WorksheetStyle s66 = book.Styles.Add("s66");
                s66.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s66.Alignment.Vertical = StyleVerticalAlignment.Center;
                s66.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                s66.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s66.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s67
                // -----------------------------------------------
                WorksheetStyle s67 = book.Styles.Add("s67");
                s67.Font.Bold = true;
                s67.Font.FontName = "Calibri";
                s67.Font.Size = 16;
                s67.Font.Color = "#000000";
                s67.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s67.Alignment.Vertical = StyleVerticalAlignment.Bottom;
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


                //this.GenerateStyles(book.Styles);

                sheet = book.Worksheets.Add("Header");

                #region Header Page
                WorksheetColumn columnHead = sheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                sheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = sheet.Table.Columns.Add();
                column2Head.Width = 332;
                column2Head.StyleID = "s172";
                sheet.Table.Columns.Add(59);
                // -----------------------------------------------
                WorksheetRow RowHead = sheet.Table.Rows.Add();
                WorksheetCell cell;
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
                Row1Head.Height = 12;
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
                Row2Head.Height = 12;
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
                WorksheetRow Row3Head = sheet.Table.Rows.Add();
                Row3Head.Height = 12;
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
                Row4Head.Height = 12;
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
                Row5Head.Height = 12;
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
                Row6Head.Height = 12;
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
                WorksheetRow Row7Head = sheet.Table.Rows.Add();
                Row7Head.Height = 12;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            Agency: " + Agency + " , Department : " + Depart + " , Program : " + Program;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row8 = sheet.Table.Rows.Add();
                Row8.Height = 12;
                Row8.AutoFitHeight = false;
                cell = Row8.Cells.Add();
                cell.StyleID = "s139";
                cell = Row8.Cells.Add();
                cell.StyleID = "s143";
                cell = Row8.Cells.Add();
                cell.StyleID = "m2611536909344";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row9 = sheet.Table.Rows.Add();
                Row9.Height = 12;
                Row9.AutoFitHeight = false;
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s143";
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s170";
                cell = Row9.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row10 = sheet.Table.Rows.Add();
                Row10.Height = 12;
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
                Row11.Height = 12;
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
                string SalCal = string.Empty;
                SalCal = rbCAL.Checked ? rbCAL.Text : rbSAL.Text;
                WorksheetRow Row15Head = sheet.Table.Rows.Add();
                Row15Head.Height = 12;
                Row15Head.AutoFitHeight = false;
                cell = Row15Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row15Head.Cells.Add();
                cell.StyleID = "s143";
                Row15Head.Cells.Add("            Type", DataType.String, "s144");
                Row15Head.Cells.Add(" : " + SalCal, DataType.String, "s169");
                cell = Row15Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row16Head = sheet.Table.Rows.Add();
                //Row16Head.Height = 12;
                //Row16Head.AutoFitHeight = false;
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row17 = sheet.Table.Rows.Add();
                Row17.Height = 12;
                Row17.AutoFitHeight = false;
                cell = Row17.Cells.Add();
                cell.StyleID = "s139";
                cell = Row17.Cells.Add();
                cell.StyleID = "s143";
                Row17.Cells.Add("            SAL/CAL Name", DataType.String, "s144");
                Row17.Cells.Add(": " + (rdoAll.Checked ? "All" : rdoSelected.Text), DataType.String, "s169");
                cell = Row17.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row12 = sheet.Table.Rows.Add();
                Row12.Height = 12;
                Row12.AutoFitHeight = false;
                cell = Row12.Cells.Add();
                cell.StyleID = "s139";
                cell = Row12.Cells.Add();
                cell.StyleID = "s143";
                Row12.Cells.Add("            Date Range", DataType.String, "s144");
                Row12.Cells.Add(": From " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), DataType.String, "s169");
                cell = Row12.Cells.Add();
                cell.StyleID = "s145";
                //WorksheetRow Row18 = sheet.Table.Rows.Add();
                //Row18.Height = 12;
                //Row18.AutoFitHeight = false;
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                // -----------------------------------------------
                WorksheetRow Row24 = sheet.Table.Rows.Add();
                Row24.Height = 12;
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
                Row25.Height = 12;
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
                Row26Head.Height = 12;
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
                Row27Head.Height = 12;
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
                Row28.Height = 12;
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
                Row29.Height = 12;
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
                Row30.Height = 12;
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
                Row31.Height = 12;
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

                int S = 1;
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = new DataTable();
                    if (ds.Tables[i].Rows.Count > 0)
                        dt = ds.Tables[i];
                    int ColCount = 0;

                    if (dt.Rows.Count > 0)
                    {
                        sheet = book.Worksheets.Add("Sheet" + (S));
                        S++;

                        int QuesCount = Convert.ToInt32(dt.Rows[0]["QUES_COUNT"].ToString().Trim());

                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(30));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(180));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(210));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(210));
                        ColCount++;
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        ColCount++;
                        if (chkHeader.Checked)
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(80));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(80));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(80));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(120));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(120));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                        }
                        for (int j = 0; j < QuesCount; j++)
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            ColCount++;
                        }


                        try
                        {
                            WorksheetRow row = sheet.Table.Rows.Add();

                            string SAL_Name = string.Empty;
                            if (rbCAL.Checked)
                            {
                                if (SALDEF.Count > 0) SAL_Name = SALDEF.Find(u => u.SALD_ID.Equals(dt.Rows[0]["CALCONT_SALID"].ToString().Trim())).SALD_NAME.Trim();
                            }
                            else
                            {
                                if (SALDEF.Count > 0) SAL_Name = SALDEF.Find(u => u.SALD_ID.Equals(dt.Rows[0]["SALACT_SALID"].ToString().Trim())).SALD_NAME.Trim();
                            }

                            cell = row.Cells.Add(SAL_Name, DataType.String, "s67");
                            cell.MergeAcross = ColCount - 1;

                            row = sheet.Table.Rows.Add();

                            if (rbCAL.Checked)
                                cell = row.Cells.Add("Contact Posting Information", DataType.String, "m386123900");
                            else
                                cell = row.Cells.Add("Service Posting Information", DataType.String, "m386123900");
                            cell.MergeAcross = 8;

                            if (chkHeader.Checked)
                            {
                                cell = row.Cells.Add("SAL Header Fields", DataType.String, "m386123920");
                                cell.MergeAcross = 8;
                            }

                            if (rbCAL.Checked)
                            {
                                cell = row.Cells.Add("CAL Questions and Answeres", DataType.String, "m386123940");
                            }
                            else
                                cell = row.Cells.Add("SAL Questions and Answeres", DataType.String, "m386123940");
                            cell.MergeAcross = QuesCount-1;

                            row = sheet.Table.Rows.Add();
                            cell = row.Cells.Add("", DataType.String, "m386123900");
                            cell.MergeAcross = 8;

                            string PrivGroup = string.Empty; bool First = false; int GrpCnt = 0; int b = 0; bool IsFalse = true;
                            for (int z = QuesCount; z > 0; z--)
                            {
                                string GroupName = string.Empty;
                                if (SALQUESEntity.Count > 0)
                                {
                                    SalquesEntity QuesEntity = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName));
                                    if (QuesEntity != null)
                                    {
                                        SalquesEntity GroupEntity = SALQUESEntity.Find(u => u.SALQ_SALD_ID == QuesEntity.SALQ_SALD_ID && u.SALQ_GRP_CODE == QuesEntity.SALQ_GRP_CODE && u.SALQ_SEQ == "0");
                                        if (GroupEntity != null)
                                        {
                                            GroupName = GroupEntity.SALQ_DESC;
                                            if (PrivGroup != GroupName) { GrpCnt++; if (IsFalse) { PrivGroup = GroupName; IsFalse = false; }  First = true; }
                                            //if (First) { PrivGroup = GroupName; First = false; }
                                        }
                                    }
                                }

                                if (GrpCnt>1)
                                {
                                    if (First)
                                    {
                                        cell = row.Cells.Add(PrivGroup, DataType.String, "m386123940");
                                        cell.MergeAcross = b-1;
                                        PrivGroup = GroupName;
                                        First = false;
                                        b = 0;
                                    }
                                }
                                b++;
                            }

                            if (GrpCnt > 1)
                            {
                                cell = row.Cells.Add(PrivGroup, DataType.String, "m386123940");
                                cell.MergeAcross = b-1;
                            }
                            else
                            {
                                if (First)
                                {
                                    cell = row.Cells.Add(PrivGroup, DataType.String, "m386123940");
                                    cell.MergeAcross = QuesCount - 1;
                                    First = false;
                                }
                            }


                            row = sheet.Table.Rows.Add();


                            row.Cells.Add(new WorksheetCell("Ag", "s61"));
                            row.Cells.Add(new WorksheetCell("De", "s62"));
                            row.Cells.Add(new WorksheetCell("Pr", "s62"));
                            row.Cells.Add(new WorksheetCell("Year", "s45"));
                            row.Cells.Add(new WorksheetCell("App #", "s45"));
                            row.Cells.Add(new WorksheetCell("Name (first and last)", "s45"));
                            if (rbCAL.Checked)
                            {
                                row.Cells.Add(new WorksheetCell("Contact Name", "s45"));
                                row.Cells.Add(new WorksheetCell("CaseWorker", "s45"));
                                row.Cells.Add(new WorksheetCell("Contact_Date", "s46"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell("Service Plan", "s45"));
                                row.Cells.Add(new WorksheetCell("Activity", "s45"));
                                row.Cells.Add(new WorksheetCell("Act_Date", "s46"));
                            }
                            //if (chkHeader.Checked)
                            //{
                            //    row.Cells.Add(new WorksheetCell("Time In", "s29"));
                            //    row.Cells.Add(new WorksheetCell("Time Out", "s30"));
                            //    row.Cells.Add(new WorksheetCell("Time Spent", "s30"));
                            //    row.Cells.Add(new WorksheetCell("Fund", "s30"));
                            //    row.Cells.Add(new WorksheetCell("Staff", "s30"));
                            //    row.Cells.Add(new WorksheetCell("Status", "s31"));
                            //    row.Cells.Add(new WorksheetCell("Location", "s31"));
                            //    row.Cells.Add(new WorksheetCell("Recipient", "s31"));
                            //    row.Cells.Add(new WorksheetCell("Attendance", "s31"));
                            //}

                            for (int z = QuesCount; z > 0; z--)
                            {
                                string QuesName = string.Empty;
                                if (SALQUESEntity.Count > 0)
                                {
                                    SalquesEntity QuesEntity = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName));
                                    if(QuesEntity!=null)
                                        QuesName = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_DESC.Trim();
                                }
                                row.Cells.Add(new WorksheetCell(QuesName, "s54"));
                            }


                            string PrivApp = string.Empty; string PrivSalID = string.Empty;
                            if (rbCAL.Checked)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["MST_APP_NO"].ToString().Trim() != PrivApp || dr["CASECONT_ID"].ToString().Trim() != PrivSalID)
                                    {
                                        row = sheet.Table.Rows.Add();

                                        row.Cells.Add(new WorksheetCell(dr["MST_AGENCY"].ToString(), "s63"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_DEPT"].ToString(), "s64"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_PROGRAM"].ToString(), "s64"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_YEAR"].ToString(), "s21"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "s21"));
                                        row.Cells.Add(new WorksheetCell(dr["SNP_NAME_IX_FI"].ToString() + " " + dr["SNP_NAME_IX_LAST"].ToString(), "s21"));
                                        row.Cells.Add(new WorksheetCell(dr["CASECONT_CONTACT_NAME"].ToString(), "s23"));

                                        string StaffName = string.Empty;
                                        HierarchyEntity hiedata = propCaseworkerList.Find(u => u.CaseWorker.ToString() == dr["CASECONT_CASEWORKER"].ToString().Trim());
                                        if (hiedata != null)
                                        {
                                            StaffName = hiedata.HirarchyName.ToString().Trim();
                                        }

                                        row.Cells.Add(new WorksheetCell(StaffName, "s23"));
                                        row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CASECONT_DATE"].ToString().Trim()), "s24"));

                                        //if (chkHeader.Checked)
                                        //{
                                        //    row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_IN"].ToString(), "s32"));
                                        //    row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_OUT"].ToString(), "s33"));
                                        //    row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_SPENT"].ToString(), "s34"));

                                        //    string FundName = string.Empty;
                                        //    if (FundingList.Count > 0 && !string.IsNullOrEmpty(dr["CASEACT_FUND1"].ToString().Trim()))
                                        //    {
                                        //        FundName = FundingList.Find(u => u.Code.Equals(dr["CASEACT_FUND1"].ToString().Trim())).Desc.Trim();
                                        //    }


                                        //    string StaffName = string.Empty;
                                        //    HierarchyEntity hiedata = propCaseworkerList.Find(u => u.CaseWorker.ToString() == dr["CASEACT_CASEWRKR"].ToString().Trim());
                                        //    if (hiedata != null)
                                        //    {
                                        //        StaffName = hiedata.HirarchyName.ToString().Trim();
                                        //    }

                                        //    string StatusDesc = string.Empty;
                                        //    if (Status.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_STATUS"].ToString().Trim()))
                                        //    {
                                        //        StatusDesc = Status.Find(u => u.Code.Equals(dr["SALACT_STATUS"].ToString().Trim())).Desc.Trim();
                                        //    }

                                        //    string LocationDesc = string.Empty;
                                        //    if (Location.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_LOCATION"].ToString().Trim()))
                                        //    {
                                        //        LocationDesc = Location.Find(u => u.Code.Equals(dr["SALACT_LOCATION"].ToString().Trim())).Desc.Trim();
                                        //    }

                                        //    string RecipentDesc = string.Empty;
                                        //    if (Recipient.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_RECIPIENT"].ToString().Trim()))
                                        //    {
                                        //        RecipentDesc = Recipient.Find(u => u.Code.Equals(dr["SALACT_RECIPIENT"].ToString().Trim())).Desc.Trim();
                                        //    }

                                        //    string AttnDesc = string.Empty;
                                        //    if (Attn.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_ATTN"].ToString().Trim()))
                                        //    {
                                        //        AttnDesc = Attn.Find(u => u.Code.Equals(dr["SALACT_ATTN"].ToString().Trim())).Desc.Trim();
                                        //    }

                                        //    row.Cells.Add(new WorksheetCell(FundName, "s35"));
                                        //    row.Cells.Add(new WorksheetCell(StaffName, "s35"));
                                        //    row.Cells.Add(new WorksheetCell(StatusDesc, "s36"));
                                        //    row.Cells.Add(new WorksheetCell(LocationDesc, "s36"));
                                        //    row.Cells.Add(new WorksheetCell(RecipentDesc, "s36"));
                                        //    row.Cells.Add(new WorksheetCell(AttnDesc, "s36"));
                                        //}

                                        for (int z = QuesCount; z > 0; z--)
                                        {
                                            string QuesType = string.Empty; string QuesCode = string.Empty; string RespDesc = string.Empty;

                                            if (SALQUESEntity.Count > 0)
                                            {
                                                QuesType = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_TYPE.Trim();
                                                QuesCode = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_ID.Trim();
                                            }

                                            if (QuesType == "D")
                                            {
                                                RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString().Trim();
                                                if (SALQUESRespEntity.Count > 0 && !string.IsNullOrEmpty(RespDesc.Trim()))
                                                {
                                                    RespDesc = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(QuesCode.Trim()) && u.SALQR_CODE.Trim().Equals(RespDesc.Trim())).SALQR_DESC.Trim();
                                                }
                                            }
                                            else if (QuesType == "C")
                                            {
                                                RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString().Trim();
                                                string CheckResponse = string.Empty;
                                                string response = RespDesc;
                                                string[] arrResponse = null;
                                                if (response.IndexOf(',') > 0)
                                                {
                                                    arrResponse = response.Split(',');
                                                }
                                                else if (!response.Equals(string.Empty))
                                                {
                                                    arrResponse = new string[] { response };
                                                }
                                                if (SALQUESRespEntity.Count > 0)
                                                {
                                                    List<SalqrespEntity> SALResponseEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(QuesCode.Trim()));
                                                    if (SALResponseEntity.Count > 0)
                                                    {
                                                        foreach (SalqrespEntity Entity in SALResponseEntity)
                                                        {

                                                            string resDesc = Entity.SALQR_CODE.ToString().Trim();


                                                            if (arrResponse != null && arrResponse.ToList().Exists(u => u.Trim().Equals(resDesc)))
                                                            {
                                                                CheckResponse = CheckResponse + Entity.SALQR_DESC.Trim() + ",";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(CheckResponse.Trim()))
                                                    RespDesc = CheckResponse.Substring(0, CheckResponse.Length - 1);
                                            }
                                            else
                                            {
                                                RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString();
                                                if (RespDesc.Contains("\n"))
                                                    RespDesc = RespDesc.Replace("\n", "\r\n");

                                            }

                                            row.Cells.Add(new WorksheetCell(RespDesc, "s23"));
                                        }


                                        PrivApp = dr["MST_APP_NO"].ToString().Trim(); PrivSalID = dr["CASECONT_ID"].ToString().Trim();
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["MST_APP_NO"].ToString().Trim() != PrivApp && dr["CASEACT_ID"].ToString().Trim() != PrivSalID) //SALACT_SALID
                                    {
                                        row = sheet.Table.Rows.Add();

                                        row.Cells.Add(new WorksheetCell(dr["MST_AGENCY"].ToString(), "s63"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_DEPT"].ToString(), "s64"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_PROGRAM"].ToString(), "s64"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_YEAR"].ToString(), "s21"));
                                        row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "s21"));
                                        row.Cells.Add(new WorksheetCell(dr["SNP_NAME_IX_FI"].ToString() + " " + dr["SNP_NAME_IX_LAST"].ToString(), "s21"));
                                        row.Cells.Add(new WorksheetCell(dr["SP_NAME"].ToString(), "s23"));
                                        row.Cells.Add(new WorksheetCell(dr["ACT_NAME"].ToString(), "s23"));
                                        row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CASEACT_ACTY_DATE"].ToString().Trim()), "s24"));

                                        if (chkHeader.Checked)
                                        {
                                            row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_IN"].ToString(), "s32"));
                                            row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_OUT"].ToString(), "s33"));
                                            row.Cells.Add(new WorksheetCell(dr["SALACT_TIME_SPENT"].ToString(), "s34"));

                                            string FundName = string.Empty;
                                            if (FundingList.Count > 0 && !string.IsNullOrEmpty(dr["CASEACT_FUND1"].ToString().Trim()))
                                            {
                                                FundName = FundingList.Find(u => u.Code.Equals(dr["CASEACT_FUND1"].ToString().Trim())).Desc.Trim();
                                            }


                                            string StaffName = string.Empty;
                                            HierarchyEntity hiedata = propCaseworkerList.Find(u => u.CaseWorker.ToString() == dr["CASEACT_CASEWRKR"].ToString().Trim());
                                            if (hiedata != null)
                                            {
                                                StaffName = hiedata.HirarchyName.ToString().Trim();
                                            }

                                            string StatusDesc = string.Empty;
                                            if (Status.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_STATUS"].ToString().Trim()))
                                            {
                                                StatusDesc = Status.Find(u => u.Code.Equals(dr["SALACT_STATUS"].ToString().Trim())).Desc.Trim();
                                            }

                                            string LocationDesc = string.Empty;
                                            if (Location.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_LOCATION"].ToString().Trim()))
                                            {
                                                LocationDesc = Location.Find(u => u.Code.Equals(dr["SALACT_LOCATION"].ToString().Trim())).Desc.Trim();
                                            }

                                            string RecipentDesc = string.Empty;
                                            if (Recipient.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_RECIPIENT"].ToString().Trim()))
                                            {
                                                RecipentDesc = Recipient.Find(u => u.Code.Equals(dr["SALACT_RECIPIENT"].ToString().Trim())).Desc.Trim();
                                            }

                                            string AttnDesc = string.Empty;
                                            if (Attn.Count > 0 && !string.IsNullOrEmpty(dr["SALACT_ATTN"].ToString().Trim()))
                                            {
                                                AttnDesc = Attn.Find(u => u.Code.Equals(dr["SALACT_ATTN"].ToString().Trim())).Desc.Trim();
                                            }

                                            row.Cells.Add(new WorksheetCell(FundName, "s35"));
                                            row.Cells.Add(new WorksheetCell(StaffName, "s35"));
                                            row.Cells.Add(new WorksheetCell(StatusDesc, "s36"));
                                            row.Cells.Add(new WorksheetCell(LocationDesc, "s36"));
                                            row.Cells.Add(new WorksheetCell(RecipentDesc, "s36"));
                                            row.Cells.Add(new WorksheetCell(AttnDesc, "s36"));
                                        }

                                        for (int z = QuesCount; z > 0; z--)
                                        {
                                            string QuesType = string.Empty; string QuesCode = string.Empty; string RespDesc = string.Empty;

                                            if (SALQUESEntity.Count > 0)
                                            {
                                                QuesType = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_TYPE.Trim();
                                                QuesCode = SALQUESEntity.Find(u => u.SALQ_ID.Equals(dt.Columns[dt.Columns.Count - z].ColumnName)).SALQ_ID.Trim();
                                            }

                                            if (QuesType == "D")
                                            {
                                                RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString().Trim();
                                                if (SALQUESRespEntity.Count > 0)
                                                {
                                                    RespDesc = SALQUESRespEntity.Find(u => u.SALQR_Q_ID.Equals(QuesCode.Trim()) && u.SALQR_CODE.Trim().Equals(RespDesc.Trim())).SALQR_DESC.Trim();
                                                }
                                            }
                                            else if (QuesType == "C")
                                            {
                                                RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString().Trim();
                                                string CheckResponse = string.Empty;
                                                string response = RespDesc;
                                                string[] arrResponse = null;
                                                if (response.IndexOf(',') > 0)
                                                {
                                                    arrResponse = response.Split(',');
                                                }
                                                else if (!response.Equals(string.Empty))
                                                {
                                                    arrResponse = new string[] { response };
                                                }
                                                if (SALQUESRespEntity.Count > 0)
                                                {
                                                    List<SalqrespEntity> SALResponseEntity = SALQUESRespEntity.FindAll(u => u.SALQR_Q_ID.Equals(QuesCode.Trim()));
                                                    if (SALResponseEntity.Count > 0)
                                                    {
                                                        foreach (SalqrespEntity Entity in SALResponseEntity)
                                                        {

                                                            string resDesc = Entity.SALQR_CODE.ToString().Trim();


                                                            if (arrResponse != null && arrResponse.ToList().Exists(u => u.Trim().Equals(resDesc)))
                                                            {
                                                                CheckResponse = CheckResponse + Entity.SALQR_DESC.Trim() + ",";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(CheckResponse.Trim()))
                                                    RespDesc = CheckResponse.Substring(0, CheckResponse.Length - 1);
                                            }
                                            else
                                            {
                                                RespDesc = dr[dt.Columns[dt.Columns.Count - z].ColumnName].ToString();
                                                if (RespDesc.Contains("\n"))
                                                    RespDesc = RespDesc.Replace("\n", "\r\n");

                                            }

                                            row.Cells.Add(new WorksheetCell(RespDesc, "s23"));
                                        }


                                        PrivApp = dr["MST_APP_NO"].ToString().Trim(); PrivSalID = dr["CASEACT_ID"].ToString().Trim();
                                    }
                                }
                            }




                        }
                        catch (Exception ex) { }

                        FileStream stream = new FileStream(PdfName, FileMode.Create);

                        book.Save(stream);
                        stream.Close();
                        
                    }
                    //else
                    //    AlertBox.Show("No Records Found", MessageBoxIcon.Warning);


                }

            }
        }



    }
}