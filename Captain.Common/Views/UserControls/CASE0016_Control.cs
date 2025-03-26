/************************************************************************
 * Conversion On    :   01/02/2022      * Converted By     :   Kranthi
 * Modified On      :   01/02/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Exceptions;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.Forms;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using Wisej.Web;
using Twilio.TwiML.Voice;
using Application = Wisej.Web.Application;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraRichEdit.Model;
using Microsoft.IdentityModel.Tokens;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CASE0016_Control : BaseUserControl
    {
        private ErrorProvider _errorProvider = null;
        private string strTempFolderName = string.Empty;
        private string strImageFolderName = string.Empty;
        private string strFolderName = string.Empty;
        private string strSubFolderName = string.Empty;
        private string strFullFolderName = string.Empty;
        private string strMainFolderName = string.Empty;
        private string strExtensionName = string.Empty;
        private string strDeleteEnable = string.Empty;
        private string strCheckUploadMode = string.Empty;
        private OpenFileDialog objUpload;
        public CASE0016_Control(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            GetCaseactdata();
            propReportPath = _model.lookupDataAccess.GetReportPath();

            DisableRows();
            Mode = "";
            //commented by sudheer on 11/25/2021
            //panel1.Enabled = false;
            panel2.Visible = false;

            ACR_SERV_Hies = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol;
            propfundingsource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "AGYTABS");

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Pb_Add_Benefit, "Add Benefit");
            tooltip.SetToolTip(Pb_Delete_Benefit, "Delete Benefit");
            tooltip.SetToolTip(Pb_Edit_Benefit, "Edit Benefit");
            tooltip.SetToolTip(pb_Print, "Print Letters");
            Get_Vendor_List();
            fillcombobox();
            Fill_CaseWorker();
            Fill_Sel_Hierarchy_SPs();

            if (BaseForm.BaseAgencyControlDetails.CEAPPostUsage.ToString() == "Y")
                btnPost.Text = "&Post from Usage";

            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            fillMonthlyQuestions();
            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            panel3.Visible = true;

            Fill_Applicant_SPs();
            fillBenefits(string.Empty, string.Empty);

            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                CEAPCNTL_List = _model.SPAdminData.GetCEAPCNTLData(string.Empty, BaseForm.BaseYear, string.Empty, string.Empty);

            if (custResponses.Count > 0)
            {
                
                if (Privileges.AddPriv.Equals("false"))
                {
                    Pb_Add_Benefit.Visible = false;
                }
                else
                {
                    Pb_Add_Benefit.Visible = true;
                }
            }
            else
                Pb_Add_Benefit.Visible = true;

            strFolderName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
            strTempFolderName = _model.lookupDataAccess.GetReportPath() + "\\Temp\\Invoices\\" + strFolderName; ; //"\\\\cap-dev\\C-Drive\\CapSystemsimages\\Temp"; //"C:\\CapsystemsImages\\Temp";//
            strImageFolderName = _model.lookupDataAccess.GetReportPath() + "\\Invoices" + strFolderName;//


            PopulateToolbar(oToolbarMnustrip);
        }

        List<CASEACTEntity> _propCaseact = new List<CASEACTEntity>();
        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();

        List<CEAPCNTLEntity> CEAPCNTL_List = new List<CEAPCNTLEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);
        }
        void gvwMonthQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {

                gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
                if (e.ColumnIndex == gvtElecpayment.Index || e.ColumnIndex == gvtKwhUsage.Index || e.ColumnIndex == gvtGaspay.Index || e.ColumnIndex == gvtCCFusage.Index)
                {
                    if (e.RowIndex > -1)
                    {
                        int introwindex = e.RowIndex; //gvwMonthQuestions.CurrentCell.RowIndex;
                        int intcolumnindex = e.ColumnIndex; //gvwMonthQuestions.CurrentCell.ColumnIndex;
                        string strAmtValue = Convert.ToString(gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value);

                        if (!string.IsNullOrEmpty(strAmtValue))
                        {
                            if (CommonFunctions.IsDecimalValid(strAmtValue.Trim()))
                            {
                                //if (!CommonFunctions.IsDecimalValid(strAmtValue.Trim()))
                                //{
                                if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                                {
                                }
                                else
                                {
                                    if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                    {
                                        gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    }
                                    else
                                    {
                                        if (strAmtValue.Length > 8)
                                        {
                                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange8String))
                                            {
                                                gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = "99999999.99";
                                            }
                                        }
                                        else
                                        {
                                            if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                            {
                                                gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
                                                gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                            }
                                        }
                                    }
                                }
                                CalculationAmount();
                                //}
                            }
                            else
                            {
                                if (intcolumnindex == 3 || intcolumnindex == 4 || intcolumnindex == 6 || intcolumnindex == 7)
                                {
                                    gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    gvwMonthQuestions.Update();
                                }
                            }

                            if ((introwindex + 1) < gvwMonthQuestions.Rows.Count)
                            {
                                if (intcolumnindex < 6)
                                {
                                    if (intcolumnindex > 3)
                                    {
                                        gvwMonthQuestions.Rows[introwindex + 1].Cells[intcolumnindex - 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex - 1, introwindex + 1];
                                    }
                                    else
                                    {
                                        gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                    }
                                }
                                else
                                {
                                    if (intcolumnindex > 6)
                                    {
                                        gvwMonthQuestions.Rows[introwindex + 1].Cells[intcolumnindex - 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex - 1, introwindex + 1];
                                    }
                                    else
                                    {
                                        gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                    }
                                }
                            }
                            else
                            {
                                if (introwindex + 1 == 12 && intcolumnindex == 4)
                                {
                                    gvwMonthQuestions.Rows[0].Cells[6].Selected = true;
                                    gvwMonthQuestions.CurrentCell = gvwMonthQuestions[6, 0];
                                }
                                else
                                {
                                    if (intcolumnindex < 6)
                                    {
                                        if (intcolumnindex > 3)
                                        {
                                            gvwMonthQuestions.Rows[introwindex + 1].Cells[intcolumnindex - 1].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex - 1, introwindex + 1];
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                        }
                                    }
                                    else
                                    {
                                        if (intcolumnindex > 6)
                                        {
                                            gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex, introwindex];
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            CalculationAmount();

                            if ((introwindex + 1) < gvwMonthQuestions.Rows.Count)
                            {
                                if (intcolumnindex < 6)
                                {
                                    if (intcolumnindex > 3)
                                    {
                                        gvwMonthQuestions.Rows[introwindex + 1].Cells[intcolumnindex - 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex - 1, introwindex + 1];
                                    }
                                    else
                                    {
                                        gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                    }
                                }
                                else
                                {
                                    if (intcolumnindex > 6)
                                    {
                                        gvwMonthQuestions.Rows[introwindex + 1].Cells[intcolumnindex - 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex - 1, introwindex + 1];
                                    }
                                    else
                                    {
                                        gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                        gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                    }
                                }
                            }
                            else
                            {
                                if (introwindex + 1 == 12 && intcolumnindex == 4)
                                {
                                    gvwMonthQuestions.Rows[0].Cells[6].Selected = true;
                                    gvwMonthQuestions.CurrentCell = gvwMonthQuestions[6, 0];
                                }
                                else
                                {
                                    if (intcolumnindex < 6)
                                    {
                                        if (intcolumnindex > 3)
                                        {
                                            gvwMonthQuestions.Rows[introwindex + 1].Cells[intcolumnindex - 1].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex - 1, introwindex + 1];
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                        }
                                    }
                                    else
                                    {
                                        if (intcolumnindex > 6)
                                        {
                                            gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex, introwindex];
                                        }
                                        else
                                        {
                                            gvwMonthQuestions.Rows[introwindex /*+ 1*/].Cells[intcolumnindex + 1].Selected = true;
                                            gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex + 1, introwindex /*+ 1*/];
                                        }
                                    }
                                }
                            }

                            //if ((introwindex + 1) < gvwMonthQuestions.Rows.Count)
                            //{
                            //    gvwMonthQuestions.Rows[introwindex].Cells[intcolumnindex].Selected = true;
                            //    gvwMonthQuestions.CurrentCell = gvwMonthQuestions[intcolumnindex, introwindex];
                            //}
                        }
                    }
                }

                gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            }
        }
        void gvwTotalGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            gvwTotalGrid.CellValueChanged -= new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            if (e.ColumnIndex == gvttElecpayment.Index || e.ColumnIndex == gvttKwhUsage.Index || e.ColumnIndex == gvttGaspay.Index || e.ColumnIndex == gvttCCFusage.Index)
            {

                int introwindex = gvwTotalGrid.CurrentCell.RowIndex;
                int intcolumnindex = gvwTotalGrid.CurrentCell.ColumnIndex;
                string strAmtValue = Convert.ToString(gvwTotalGrid.Rows[introwindex].Cells[intcolumnindex].Value);

                if (!string.IsNullOrEmpty(strAmtValue))
                {
                    if (CommonFunctions.IsDecimalValid(strAmtValue.Trim()))
                    {
                        //if (!CommonFunctions.IsDecimalValid(strAmtValue.Trim()))
                        //{
                        if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                        {
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                            {
                                gvwTotalGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                            }
                            else
                            {
                                if (strAmtValue.Length > 8)
                                {
                                    if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange8String))
                                    {
                                        gvwTotalGrid.Rows[introwindex].Cells[intcolumnindex].Value = "99999999.99";
                                    }
                                }
                                else
                                {
                                    if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                    {
                                        gvwTotalGrid.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                    }
                                }
                            }
                        }
                        CalculationAmount();
                        //}
                    }
                    else
                    {
                        if (intcolumnindex == 3 || intcolumnindex == 4 || intcolumnindex == 6 || intcolumnindex == 7)
                        {
                            gvwTotalGrid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                        }
                    }
                }
                else
                {
                    CalculationAmount();
                }


            }
            gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
        }

        private CaptainModel _model = null;
        private List<CustomQuestionsandAnswers> _customQuestionsandAnswers = null;
        List<CMBDCEntity> Emsbdc_List { get; set; }
        List<CommonEntity> propfundingsource { get; set; }

        List<Captain.Common.Utilities.ListItem> CaseWorker_List = new List<Captain.Common.Utilities.ListItem>();

        private void fillcombobox()
        {
            cmbPsource.SelectedIndexChanged -= cmbPsource_SelectedIndexChanged;
            cmbSource.SelectedIndexChanged -= cmbSource_SelectedIndexChanged;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("08004");
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            List<Utilities.ListItem> listItem = new List<Utilities.ListItem>();
            listItem.Add(new Utilities.ListItem("   ", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                listItem.Add(new Utilities.ListItem(dr["LookUpDesc"].ToString().Trim(), dr["Code"].ToString().Trim()));
            }
            cmbPsource.Items.AddRange(listItem.ToArray());
            cmbSource.Items.AddRange(listItem.ToArray());

            cmbPsource.SelectedIndex = 0;
            cmbSource.SelectedIndex = 0;
            cmbPsource.SelectedIndexChanged += cmbPsource_SelectedIndexChanged;
            cmbSource.SelectedIndexChanged += cmbSource_SelectedIndexChanged;
        }

        void GetCaseactdata()
        {
            CASEACTEntity Search_Enty = new CASEACTEntity(true);
            Search_Enty.Agency = BaseForm.BaseAgency;
            Search_Enty.Dept = BaseForm.BaseDept;
            Search_Enty.Program = BaseForm.BaseProg;
            Search_Enty.Year = BaseForm.BaseYear;                             // Year will be always Four-Spaces in CASEACT
            Search_Enty.App_no = BaseForm.BaseApplicationNo;


            _propCaseact = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");

            //if (BaseForm.BaseAgencyControlDetails.CEAPPostUsage == "Y")
            //{
            //    List<CASEACTEntity> caseactlist = _propCaseact.FindAll(u => u.Elec_Other.ToString().Trim() == "O");
            //    if (caseactlist.Count > 0) { upload2.Visible = true; }
            //    caseactlist = _propCaseact.FindAll(u => u.Elec_Other.ToString().Trim() == "E");
            //    if (caseactlist.Count > 0) { upload1.Visible = true; }
            //}

        }

        private void Fill_CaseWorker()
        {
            try
            {


                //DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(MainMenuAgency, MainMenuDept, MainMenuProgram);
                DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(BaseForm.BaseAgency, "**", "**");
                string strNameFormat = null, strCwFormat = null;
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }

                DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                if (ds1.Tables.Count > 0)
                {
                    DataTable dt1 = ds1.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt1.Rows)
                            CaseWorker_List.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim()));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        private void fillMonthlyQuestions()
        {
            cmbPsource.SelectedIndexChanged -= cmbPsource_SelectedIndexChanged;
            cmbSource.SelectedIndexChanged -= cmbSource_SelectedIndexChanged;
            gvwMonthQuestions.Rows.Clear();
            gvwTotalGrid.Rows.Clear();
            txtVendorName2.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            txtvendor2.Text = string.Empty;
            txtVendorId.Text = string.Empty;
            cmbSource.SelectedIndex = 0;
            cmbPsource.SelectedIndex = 0;
            List<CustomQuestionsEntity> custmonthQuestions = new List<CustomQuestionsEntity>();

            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            string custCode = string.Empty;
            //List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();

            int rowIndex;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                chkUtility.CheckedChanged -= new EventHandler(chkUtility_CheckedChanged);
                chkUtility.Checked = responsetot.USAGE_PRIM_12HIST.ToString() == "Y" ? true : false;
                chkGas.Checked = responsetot.USAGE_SEC_12HIST.ToString() == "Y" ? true : false;
                chkUtility.CheckedChanged += new EventHandler(chkUtility_CheckedChanged);
                CommonFunctions.SetComboBoxValue(cmbPsource, responsetot.USAGE_PSOURCE);

                cmbPsource_SelectedIndexChanged(cmbPsource, new EventArgs());

                CommonFunctions.SetComboBoxValue(cmbSource, responsetot.USAGE_SSOURCE);
                cmbSource_SelectedIndexChanged(cmbSource, new EventArgs());
                txtvendor2.Text = responsetot.USAGE_SEC_VEND;
                if (CaseVddlist.Count > 0)
                {
                    CASEVDDEntity Vdddata = CaseVddlist.Find(u => u.Code.Trim() == responsetot.USAGE_SEC_VEND.Trim());
                    if (Vdddata != null)
                    {
                        txtVendorName2.Text = Vdddata.Name;
                    }
                    Vdddata = CaseVddlist.Find(u => u.Code.Trim() == responsetot.USAGE_PRIM_VEND.Trim());
                    if (Vdddata != null)
                    {
                        txtVendorName.Text = Vdddata.Name;
                    }
                }
                txtVendorId.Text = responsetot.USAGE_PRIM_VEND;
            }
            CustomQuestionsEntity response = custResponses.Find(u => u.USAGE_MONTH.Equals("JAN"));
            bool boolcheckdata = false;
            bool boolGascheckdata = false;
            if (response != null)
            {

                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }


                rowIndex = gvwMonthQuestions.Rows.Add("JAN", "January", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);

                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;


            }
            else
            {
                rowIndex = gvwMonthQuestions.Rows.Add("JAN", "January", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("FEB"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }

                rowIndex = gvwMonthQuestions.Rows.Add("FEB", "February", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                rowIndex = gvwMonthQuestions.Rows.Add("FEB", "February", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("MAR"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("MAR", "March", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                rowIndex = gvwMonthQuestions.Rows.Add("MAR", "March", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("APR"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("APR", "April", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("APR", "April", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("MAY"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("MAY", "May", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("MAY", "May", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("JUN"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("JUN", "June", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("JUN", "June", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("JUL"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("JUL", "July", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("JUL", "July", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("AUG"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("AUG", "August", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("AUG", "August", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("SEP"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("SEP", "September", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("SEP", "September", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("OCT"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("OCT", "October", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("OCT", "October", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("NOV"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("NOV", "November", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("NOV", "November", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("DEC"));
            if (response != null)
            {
                boolcheckdata = false;
                boolGascheckdata = false;
                if (response.USAGE_PMON_SW == "Y")
                {
                    boolcheckdata = true;
                }

                if (response.USAGE_SMON_SW == "Y")
                {
                    boolGascheckdata = true;
                }
                rowIndex = gvwMonthQuestions.Rows.Add("DEC", "December", boolcheckdata, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, boolGascheckdata, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, response.USAGE_LUMP_PRIM, response.USAGE_LUMP_SEC);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtElecpayment"].ReadOnly = true;

                if (!string.IsNullOrEmpty(response.USAGE_PRIM_ACTID.Trim()))
                    gvwMonthQuestions.Rows[rowIndex].Cells["gvtGaspay"].ReadOnly = true;
            }
            else
            {
                gvwMonthQuestions.Rows.Add("DEC", "December", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            response = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (response != null)
            {
                // gvwMonthQuestions.Rows.Add("TOT", "Total", true, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, true, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL);
                gvwTotalGrid.Rows.Add("TOT", "Total", true, response.USAGE_PRIM_PAYMENT, response.USAGE_PRIM_USAGE, true, response.USAGE_SEC_PAYMENT, response.USAGE_SEC_USAGE, response.USAGE_TOTAL, string.Empty, string.Empty);
                set_Usage_Tooltip(rowIndex, response.adddate, response.addoperator, response.lstcdate, response.lstcoperator);
            }
            else
            {
                //gvwMonthQuestions.Rows.Add("TOT", "Total", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty);
                gvwTotalGrid.Rows.Add("TOT", "Total", true, string.Empty, string.Empty, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            }

            //CalculationAmount();
            gvwTotalGrid.Rows[0].Cells["gvctGasCheck"].ReadOnly = true;
            gvwTotalGrid.Rows[0].Cells["gvttCheckData"].ReadOnly = true;
            gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = true;
            gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = true;
            gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = true;
            gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = true;
            // chkUtility_CheckedChanged(chkUtility, new EventArgs());
            ////gvwMonthQuestions.RefreshEdit();
            ////gvwMonthQuestions.Update();
            chkUtilityCheckedwithoutamount();
            gvwMonthQuestions.Update();
            cmbPsource.SelectedIndexChanged += cmbPsource_SelectedIndexChanged;
            cmbSource.SelectedIndexChanged += cmbSource_SelectedIndexChanged;

            //Added by Sudheer on 05/25/2022
            if (custResponses.Count > 0)
            {
                if (Privileges.AddPriv.Equals("false"))
                {
                    Pb_Add_Benefit.Visible = false;
                }
                else
                {
                    Pb_Add_Benefit.Visible = true;
                }
            }
            else
                Pb_Add_Benefit.Visible = true;

        }

        private void set_Usage_Tooltip(int rowIndex, string Add_Date, string Add_Opr, string Lstc_Date, string Lstc_Opr)
        {
            string toolTipText = "Added By     : " + Add_Opr + " on " + Add_Date + "\n" +
                                 "Modified By  : " + Lstc_Opr + " on " + Lstc_Date;

            foreach (DataGridViewCell cell in gvwMonthQuestions.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }


        private void ShowButtonVisiable()
        {

            if (custResponses.Count > 0)
            {
                if (ToolBarNew != null)
                {
                    ToolBarNew.Visible = false;
                    ToolBarEdit.Visible = true;
                    ToolBarDel.Visible = true;
                    //ToolBarPrint.Visible = true;
                }
                ShowCaseNotesImages();
            }
            else
            {
                if (ToolBarNew != null)
                {
                    ToolBarNew.Visible = true;
                    ToolBarEdit.Visible = false;
                    ToolBarDel.Visible = false;
                    //ToolBarPrint.Visible = false;
                    ToolBarCaseNotes.Visible = false;
                }
            }
            //panel1.Enabled = false;
            DisableRows();
            Mode = "";
            panel2.Visible = false;
            if (Privileges.AddPriv.Equals("true"))
            {
                if (ToolBarNew != null)
                    ToolBarNew.Enabled = true;
            }
            if (Privileges.ChangePriv.Equals("true"))
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = true;
            }
            if (Privileges.DelPriv.Equals("true"))
            {
                if (ToolBarDel != null)
                    ToolBarDel.Enabled = true;
            }
        }

        private void EnableRows()
        {
            chkUtility.Enabled = true; chkGas.Enabled = true;
            gvtCheckData.ReadOnly = false;
            gvcGasCheck.ReadOnly = false;
            cmbPsource.Enabled = true;
            cmbSource.Enabled = true;
            btnvendor1.Enabled = true;
            btnVendor2.Enabled = true;
            gvttCheckData.ReadOnly = false;
            gvctGasCheck.ReadOnly = false;

            gvwMonthQuestions.Enabled = true;

            if (gvwMonthQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwMonthQuestions.Rows)
                {
                    //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA" 
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                    if(BaseForm.BaseAgencyControlDetails.State=="TX")
                    {
                        dr.Cells["gvtElecpayment"].ReadOnly = false;
                        dr.Cells["gvtKwhUsage"].ReadOnly = false;
                        dr.Cells["gvtGaspay"].ReadOnly = false;
                        dr.Cells["gvtCCFusage"].ReadOnly = false;

                        if (!string.IsNullOrEmpty(dr.Cells["gvPrimLump"].Value.ToString().Trim()))
                            dr.Cells["gvtElecpayment"].ReadOnly = true;

                        if (!string.IsNullOrEmpty(dr.Cells["gvSecLump"].Value.ToString().Trim()))
                            dr.Cells["gvtGaspay"].ReadOnly = true;
                    }
                    else
                    {
                        if (dr.Cells["gvtCheckData"].Value.ToString().ToUpper() == "TRUE")
                        {
                            dr.Cells["gvtElecpayment"].ReadOnly = false;
                            dr.Cells["gvtKwhUsage"].ReadOnly = false;
                        }
                        else
                        {
                            dr.Cells["gvtElecpayment"].ReadOnly = true;
                            dr.Cells["gvtKwhUsage"].ReadOnly = true;
                        }

                        if (dr.Cells["gvcGasCheck"].Value.ToString().ToUpper() == "TRUE")
                        {
                            dr.Cells["gvtGaspay"].ReadOnly = false;
                            dr.Cells["gvtCCFusage"].ReadOnly = false;
                        }
                        else
                        {
                            dr.Cells["gvtGaspay"].ReadOnly = true;
                            dr.Cells["gvtCCFusage"].ReadOnly = true;
                        }
                    }
                }
            }

            if (gvwTotalGrid.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwTotalGrid.Rows)
                {
                    //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                    if (BaseForm.BaseAgencyControlDetails.State == "TX")
                    {
                        dr.Cells["gvttElecpayment"].ReadOnly = true;
                        dr.Cells["gvttKwhUsage"].ReadOnly = true;
                    }
                    else
                    {
                        if (chkUtility.Checked)
                        {
                            dr.Cells["gvttElecpayment"].ReadOnly = true;
                            dr.Cells["gvttKwhUsage"].ReadOnly = true;
                        }
                        else
                        {
                            dr.Cells["gvttElecpayment"].ReadOnly = false;
                            dr.Cells["gvttKwhUsage"].ReadOnly = false;
                        }
                        if (chkGas.Checked)
                        {
                            dr.Cells["gvttGaspay"].ReadOnly = true;
                            dr.Cells["gvttCCFusage"].ReadOnly = true;
                        }
                        else
                        {
                            gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = false;
                            gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = false;
                        }

                    }
                }
            }
        }

        private void DisableRows()
        {
            _errorProvider.SetError(cmbPsource, null);
            _errorProvider.SetError(btnvendor1, null);
            _errorProvider.SetError(cmbSource, null);
            _errorProvider.SetError(btnVendor2, null);
            cmbPsource.Enabled = false;
            cmbSource.Enabled = false;
            btnvendor1.Enabled = false;
            btnVendor2.Enabled = false;
            chkUtility.Enabled = false; chkGas.Enabled = false;
            gvtCheckData.ReadOnly = true;
            gvcGasCheck.ReadOnly = true;

            gvwMonthQuestions.Enabled = false;

            gvtElecpayment.ReadOnly = true;
            gvtKwhUsage.ReadOnly = true;
            gvtGaspay.ReadOnly = true;
            gvtCCFusage.ReadOnly = true;

            gvttCheckData.ReadOnly = true;
            gvctGasCheck.ReadOnly = true;

            gvttElecpayment.ReadOnly = true;
            gvttKwhUsage.ReadOnly = true;
            gvttGaspay.ReadOnly = true;
            gvttCCFusage.ReadOnly = true;
        }

        #region Public Properties

        public UserEntity UserProfile { get; set; }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarDel { get; set; }
        public ToolBarButton ToolBarHelp { get; set; }
        public ToolBarButton ToolBarPrint { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }
        public ToolBarButton ToolBarCaseNotes { get; set; }

        public string HieType { get; set; }

        public string HIE { get; set; }

        public bool IsMax { get; set; }

        public string propSpmcode { get; set; }
        public string propSeq { get; set; }

        public string AccessLevel { get; set; }

        public string Mode { get; set; }

        public CaseSnpEntity CaseSnpEntity { get; set; }

        public List<CaseNotesEntity> caseNotesEntity { get; set; }


        #endregion

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (ToolBarNew == null)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add Service Usage/Consumption Posting";
                ToolBarNew.Enabled = true;
                ToolBarNew.Visible = false;
                ToolBarNew.ImageSource = "captain-add";
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit Service Usage/Consumption Posting";
                ToolBarEdit.Enabled = true;
                ToolBarEdit.Visible = false;
                ToolBarEdit.ImageSource = "captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarDel = new ToolBarButton();
                ToolBarDel.Tag = "Delete";
                ToolBarDel.ToolTipText = "Delete Service Usage/Consumption Posting";
                ToolBarDel.Enabled = true;
                ToolBarDel.Visible = false;
                ToolBarDel.ImageSource = "captain-delete";
                ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarCaseNotes = new ToolBarButton();
                ToolBarCaseNotes.Tag = "CaseNotes";
                ToolBarCaseNotes.ToolTipText = "Case Notes";
                ToolBarCaseNotes.Enabled = true;
                ToolBarCaseNotes.ImageSource = "captain-casenotes";
                ToolBarCaseNotes.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarCaseNotes.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarPrint = new ToolBarButton();
                //ToolBarPrint.Tag = "Print";
                //ToolBarPrint.ToolTipText = "Print Eligibility & Enrollment";
                //ToolBarPrint.Enabled = true;
                //ToolBarPrint.Visible = false;
                //ToolBarPrint.Image = new IconResourceHandle(Consts.Icons16x16.Print);
                //ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

                //ToolBarExcel = new ToolBarButton();
                //ToolBarExcel.Tag = "Excel";
                //ToolBarExcel.ToolTipText = "Service Usage/Consumption Posting Report in Excel";
                //ToolBarExcel.Enabled = true;
                //ToolBarExcel.Image = new IconResourceHandle(Consts.Icons16x16.MSExcel);
                //ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                //ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "Service Plan Help";
                ToolBarHelp.Enabled = true;
                ToolBarHelp.ImageSource = "icon-help";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }


            if (Privileges.AddPriv.Equals("false"))
            {
                Pb_Add_Benefit.Visible = false; ToolBarNew.Enabled = false;
            }
            if (Privileges.ChangePriv.Equals("false"))
            {
                ToolBarEdit.Enabled = Pb_Edit_Benefit.Visible = false;
            }
            if (Privileges.DelPriv.Equals("false"))
            {
                ToolBarDel.Enabled = Pb_Delete_Benefit.Visible = false;
            }

            ShowButtonVisiable();

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                ToolBarDel,
                //ToolBarPrint,
                //ToolBarExcel,
                ToolBarCaseNotes,
                ToolBarHelp
            });
        }

        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();

            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        panel2.Visible = true;
                        btnSave.Visible = true;
                        panel1.Enabled = true;
                        ToolBarNew.Enabled = false;
                        Mode = "Add";
                        EnableRows();
                        chkUtility.Checked = false;
                        chkGas.Checked = false;
                        SOURCECONTROLEnableDisable();
                        pnlBenefitsection.Enabled = false;
                        chkUtility.Focus();
                        break;
                    case Consts.ToolbarActions.Edit:
                        panel2.Visible = true;
                        btnSave.Visible = true;
                        panel1.Enabled = true;
                        ToolBarEdit.Enabled = false;
                        ToolBarDel.Enabled = false;
                        Mode = "Edit";
                        EnableRows();
                        SOURCECONTROLEnableDisable();
                        pnlBenefitsection.Enabled = false;

                        // gvwMonthQuestions.Rows[0].Selected = true;

                        //gvwMonthQuestions.Rows[0].Cells[3].Selected = true;
                        //gvwMonthQuestions.SetCurrentCell(0, 2);
                        //gvwMonthQuestions.BeginEdit();
                        //gvwMonthQuestions.CancelEdit();

                        break;
                    case Consts.ToolbarActions.Delete:
                        if (gvwBenefit.Rows.Count > 0)
                        {
                            AlertBox.Show("You can not delete this record, Benefit data is there", MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Are you sure you want to Delete Usage/Consumption All record?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                        }
                        break;
                    case Consts.ToolbarActions.CaseNotes:
                        string strYear = "    ";
                        if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                        {
                            strYear = BaseForm.BaseYear;
                        }
                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                        CaseNotes caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
                        caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
                        caseNotes.StartPosition = FormStartPosition.CenterScreen;
                        caseNotes.ShowDialog();

                        break;
                    //case Consts.ToolbarActions.Print:
                    //    On_SaveForm_Closed();
                    //    break;
                    case Consts.ToolbarActions.Excel:

                        break;
                    case Consts.ToolbarActions.Help:
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
                        break;
                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new System.Diagnostics.StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }


        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                custEntity.ACTAGENCY = BaseForm.BaseAgency;
                custEntity.ACTDEPT = BaseForm.BaseDept;
                custEntity.ACTPROGRAM = BaseForm.BaseProg;
                if (BaseForm.BaseYear == string.Empty)
                    custEntity.ACTYEAR = "    ";
                else
                    custEntity.ACTYEAR = BaseForm.BaseYear;
                custEntity.ACTAPPNO = BaseForm.BaseApplicationNo;


                custEntity.USAGE_MONTH = string.Empty;

                custEntity.addoperator = BaseForm.UserID;
                custEntity.lstcoperator = BaseForm.UserID;
                custEntity.Mode = "Delete";

                _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);
                Refresh();
            }
        }

        private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
        {
            CaseNotes form = sender as CaseNotes;

            //if (form.DialogResult == DialogResult.OK)
            //{
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
            if (caseNotesEntity.Count > 0)
            {
                //ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_View;
                ToolBarCaseNotes.ImageSource = "captain-casenotes";
            }
            else
            {
                // ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_New;
                ToolBarCaseNotes.ImageSource = "captain-casenotesadd";
            }
            caseNotesEntity = caseNotesEntity;

            //}
        }

        private void ShowCaseNotesImages()
        {
            string strYear = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            {
                strYear = BaseForm.BaseYear;
            }
            caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
            if (caseNotesEntity.Count > 0)
            {
                //ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_View;
                ToolBarCaseNotes.ImageSource = "captain-casenotes";
            }
            else
            {
                //ToolBarCaseNotes.ImageSource = Consts.Icons.ico_CaseNotes_New;
                ToolBarCaseNotes.ImageSource = "captain-casenotesadd";
            }
            if (!(custResponses.Count > 0))
            {
                ToolBarCaseNotes.Enabled = false;
                //    ToolBarImageTypes.Enabled = false;
                //    ToolBarHistory.Enabled = false;
                //    ToolBarEdit.Enabled = false;
                //    ToolBarNewMember.Enabled = false;
                //    ToolBarPrint.Enabled = false;
            }
            else
            {
                ToolBarCaseNotes.Enabled = true;
                //    ToolBarImageTypes.Enabled = true;
                //    ToolBarHistory.Enabled = true;
                if (Privileges.AddPriv.Equals("false"))
                {
                    if (ToolBarNew != null) ToolBarNew.Enabled = false;
                }
                else
                {
                    if (ToolBarNew != null) ToolBarNew.Enabled = true;
                }

                if (Privileges.ChangePriv.Equals("false"))
                {
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
                }
                else
                {
                    if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
                }
                if (Privileges.DelPriv.Equals("false"))
                {
                    if (ToolBarDel != null) ToolBarDel.Enabled = false;
                }
                else
                {
                    if (ToolBarDel != null) ToolBarDel.Enabled = true;
                }

                //    ToolBarPrint.Enabled = true;
                //    if (gvwCustomer.Rows.Count > 0)
                //    {
                //        gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());

                //    }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validations())
            {
                saveMonthlyQuestions(BaseForm.BaseCaseSnpEntity[0]);
                pnlBenefitsection.Enabled = true;
            }
            // this.Close();

        }
        bool Validations()
        {
            bool isValid = true;
            _errorProvider.SetError(cmbPsource, null);
            _errorProvider.SetError(btnvendor1, null);
            _errorProvider.SetError(cmbSource, null);
            _errorProvider.SetError(btnVendor2, null);
            //if (chkUtility.Checked)
            //{

            //    if ((((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString().Equals("0")))
            //    {
            //        _errorProvider.SetError(cmbPsource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblPSource.Text));
            //        isValid = false;
            //    }

            //    if(string.IsNullOrEmpty(txtVendorId.Text))
            //    {
            //        _errorProvider.SetError(btnvendor1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVendor.Text));
            //        isValid = false;
            //    }

            //}

            //if (chkGas.Checked)
            //{

            //    if ((((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString().Equals("0")))
            //    {
            //        _errorProvider.SetError(cmbSource, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSSource.Text));
            //        isValid = false;
            //    }

            //    if (string.IsNullOrEmpty(txtvendor2.Text))
            //    {
            //        _errorProvider.SetError(btnVendor2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVendor.Text));
            //        isValid = false;
            //    }

            //}
            if (gvwTotalGrid.Rows.Count > 0)
            {
                if (txtVendorId.Text == string.Empty && txtvendor2.Text == string.Empty)
                {
                    string strElec = string.Empty; string strKWH = string.Empty; string strGas = string.Empty; string strCCF = string.Empty; string strAnnual = string.Empty;

                    strElec = gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value.ToString() : "0";
                    strKWH = gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].Value.ToString() : "0";





                    strGas = gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value.ToString() : "0";
                    strCCF = gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].Value.ToString() : "0";


                    strAnnual = gvwTotalGrid.Rows[0].Cells["gvttAnnual"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttAnnual"].Value.ToString() : "0";

                    if (Convert.ToDecimal(strElec == string.Empty ? "0" : strElec) == 0 && Convert.ToDecimal(strKWH == string.Empty ? "0" : strKWH) == 0 && Convert.ToDecimal(strGas == string.Empty ? "0" : strGas) == 0 && Convert.ToDecimal(strCCF == string.Empty ? "0" : strCCF) == 0 && Convert.ToDecimal(strAnnual == string.Empty ? "0" : strAnnual) == 0)
                    {
                        AlertBox.Show("Please fill Usage/Consumption Data", MessageBoxIcon.Warning);
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //panel1.Enabled = false;

            DisableRows();
            Mode = "";
            panel2.Visible = false;

            gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            gvwTotalGrid.CellValueChanged -= new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            fillMonthlyQuestions();
            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            ShowButtonVisiable();
            pnlBenefitsection.Enabled = true;
        }

        private void chkUtility_CheckedChanged(object sender, EventArgs e)
        {
            gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            gvwTotalGrid.CellValueChanged -= new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);

            //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA" 
            //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
            //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
            if (BaseForm.BaseAgencyControlDetails.State == "TX")
            {
                foreach (DataGridViewRow gvrow in gvwMonthQuestions.Rows)
                {

                    string strdata = gvrow.Cells["gvtCheckData"].Value.ToString();
                    string strGasdata = gvrow.Cells["gvcGasCheck"].Value.ToString();
                    gvrow.Cells["gvtKwhUsage"].ReadOnly = false;
                    gvrow.Cells["gvtElecpayment"].ReadOnly = false;
                    gvrow.Cells["gvtGaspay"].ReadOnly = false;
                    gvrow.Cells["gvtCCFusage"].ReadOnly = false;
                    if (strdata.ToUpper() == "TRUE")
                    {
                        if (chkUtility.Checked)
                        {
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;

                        }
                        else
                        {
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;

                        }

                    }
                    else
                    {

                        gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;


                    }
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        if (chkGas.Checked)
                        {
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        else
                        {
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                    else
                    {
                        gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                    }



                }

                if (gvwTotalGrid.Rows.Count > 0)
                {
                    gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = true;
                    gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = true;
                    gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = true;
                    gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = true;
                    gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                    gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                }

            }
            else
            {

                if (gvwTotalGrid.Rows.Count > 0)
                {
                    if (chkUtility.Checked)
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = true;
                        gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = true;
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = false;
                        gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = false;
                    }
                    if (chkGas.Checked)
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = true;
                        gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = true;
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = false;
                        gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = false;
                    }

                    string strdata = gvwTotalGrid.Rows[0].Cells["gvttCheckData"].Value.ToString();
                    string strGasdata = gvwTotalGrid.Rows[0].Cells["gvctGasCheck"].Value.ToString();
                    if (strdata.ToUpper() == "TRUE")
                    {
                        if (chkUtility.Checked)
                        {

                            gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;

                        }
                        else
                        {
                            gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                        }

                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                    }
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        if (chkGas.Checked)
                        {
                            gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        else
                        {
                            gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Style.BackColor = System.Drawing.Color.Red;

                        }
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Style.BackColor = System.Drawing.Color.Red;

                    }


                }
                foreach (DataGridViewRow gvrow in gvwMonthQuestions.Rows)
                {

                    string strdata = gvrow.Cells["gvtCheckData"].Value.ToString();
                    string strGasdata = gvrow.Cells["gvcGasCheck"].Value.ToString();
                    if (strdata.ToUpper() == "TRUE")
                    {
                        if (chkUtility.Checked)
                        {
                            gvrow.Cells["gvtElecpayment"].ReadOnly = false;
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                            gvrow.Cells["gvtKwhUsage"].ReadOnly = false;
                        }
                        else
                        {
                            gvrow.Cells["gvtElecpayment"].ReadOnly = true;
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                            gvrow.Cells["gvtKwhUsage"].ReadOnly = true;
                        }

                    }
                    else
                    {
                        gvrow.Cells["gvtElecpayment"].ReadOnly = true;
                        gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                        gvrow.Cells["gvtKwhUsage"].ReadOnly = true;

                    }
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        if (chkGas.Checked)
                        {
                            gvrow.Cells["gvtGaspay"].ReadOnly = false;
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                            gvrow.Cells["gvtCCFusage"].ReadOnly = false;
                        }
                        else
                        {
                            gvrow.Cells["gvtGaspay"].ReadOnly = true;
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;
                            gvrow.Cells["gvtCCFusage"].ReadOnly = true;
                        }
                    }
                    else
                    {
                        gvrow.Cells["gvtGaspay"].ReadOnly = true;
                        gvrow.Cells["gvtCCFusage"].ReadOnly = true;
                        gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;

                    }



                }
            }


            // SOURCECONTROLEnableDisable();
            // CalculationAmount();
            gvwMonthQuestions.Update();
            //kranthi// gvwMonthQuestions.RefreshEdit();
            gvwTotalGrid.Update();
            //kranthi// gvwTotalGrid.RefreshEdit();
            CalculationAmount();
            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            //if (chkUtility.Checked)
            //{
            //    gvtElecpayment.ReadOnly = false;
            //    gvtKwhUsage.ReadOnly = false;
            //}
            //else
            //{
            //    gvtElecpayment.ReadOnly = true;
            //    gvtKwhUsage.ReadOnly = true;
            //}
        }

        public void SOURCECONTROLEnableDisable()
        {
            cmbSource.Enabled = true;
            btnVendor2.Enabled = true;

            cmbPsource.Enabled = true;
            btnvendor1.Enabled = true;
            //if (chkGas.Checked)
            //{

            if (gvwBenefit.Rows.Count > 0)
            {
                if (_propCaseact.Count > 0)
                {
                    List<CASEACTEntity> caseactlist = _propCaseact.FindAll(u => u.Elec_Other.ToString().Trim() == "O");
                    if (caseactlist.Count > 0)
                    {
                        if (Mode.ToUpper() == "ADD")
                        {
                            if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                            {
                                cmbSource.Enabled = true;
                                btnVendor2.Enabled = true;
                            }
                        }
                        else
                        {
                            if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                            {
                                cmbSource.Enabled = true;
                                btnVendor2.Enabled = true;
                            }
                            else
                            {
                                cmbSource.Enabled = false;
                                btnVendor2.Enabled = false;
                            }
                        }
                    }
                }
            }

            // }
            //else
            //{
            //    cmbSource.Enabled = false;
            //    cmbSource.SelectedIndex = 0;
            //    txtvendor2.Text = txtVendorName2.Text = string.Empty;
            //    btnVendor2.Enabled = false;

            //}
            //if (chkUtility.Checked)
            //{

            if (gvwBenefit.Rows.Count > 0)
            {
                if (_propCaseact.Count > 0)
                {
                    List<CASEACTEntity> caseactlist = _propCaseact.FindAll(u => u.Elec_Other.ToString().Trim() == "E");
                    if (caseactlist.Count > 0)
                    {
                        if (Mode.ToUpper() == "ADD")
                        {
                            if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                            {
                                cmbPsource.Enabled = true;
                                btnvendor1.Enabled = true;
                            }
                        }
                        else
                        {
                            if (BaseForm.UserProfile.Security.Trim() == "B" || BaseForm.UserProfile.Security.Trim() == "P")
                            {
                                cmbPsource.Enabled = false;
                                btnvendor1.Enabled = true;
                            }
                            else
                            {
                                cmbPsource.Enabled = false;
                                btnvendor1.Enabled = false;
                            }
                        }
                    }
                }
            }

            //}
            //else
            //{
            //    cmbPsource.Enabled = false;
            //    cmbPsource.SelectedIndex = 0;
            //    txtVendorName.Text = txtVendorId.Text = string.Empty;
            //    btnvendor1.Enabled = false;

            //}
        }
        private void chkUtilityCheckedwithoutamount()
        {
            gvwMonthQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            gvwTotalGrid.CellValueChanged -= new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA" 
            //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
            //    || BaseForm.BaseAgencyControlDetails.AgyShortName== "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
            if (BaseForm.BaseAgencyControlDetails.State == "TX")
            {
                foreach (DataGridViewRow gvrow in gvwMonthQuestions.Rows)
                {

                    string strdata = gvrow.Cells["gvtCheckData"].Value.ToString();
                    string strGasdata = gvrow.Cells["gvcGasCheck"].Value.ToString();
                    gvrow.Cells["gvtKwhUsage"].ReadOnly = false;
                    gvrow.Cells["gvtElecpayment"].ReadOnly = false;
                    gvrow.Cells["gvtGaspay"].ReadOnly = false;
                    gvrow.Cells["gvtCCFusage"].ReadOnly = false;
                    if (strdata.ToUpper() == "TRUE")
                    {
                        if (chkUtility.Checked)
                        {
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;

                        }
                        else
                        {
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;

                        }

                    }
                    else
                    {

                        gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;


                    }
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        if (chkGas.Checked)
                        {
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        else
                        {
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                    else
                    {
                        gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                    }



                }

                if (gvwTotalGrid.Rows.Count > 0)
                {
                    gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = true;

                    gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = true;
                    gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = true;

                    gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = true;
                    gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                    gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                }

            }
            else
            {

                if (gvwTotalGrid.Rows.Count > 0)
                {
                    if (chkUtility.Checked)
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = true;
                        gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = true;
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].ReadOnly = false;
                        gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].ReadOnly = false;
                    }
                    if (chkGas.Checked)
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = true;
                        gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = true;
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttGaspay"].ReadOnly = false;
                        gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].ReadOnly = false;
                    }
                }
                foreach (DataGridViewRow gvrow in gvwMonthQuestions.Rows)
                {

                    string strdata = gvrow.Cells["gvtCheckData"].Value.ToString();
                    string strGasdata = gvrow.Cells["gvcGasCheck"].Value.ToString();
                    if (strdata.ToUpper() == "TRUE")
                    {
                        if (chkUtility.Checked)
                        {
                            gvrow.Cells["gvtElecpayment"].ReadOnly = false;
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                            gvrow.Cells["gvtKwhUsage"].ReadOnly = false;
                        }
                        else
                        {
                            gvrow.Cells["gvtElecpayment"].ReadOnly = true;
                            gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                            gvrow.Cells["gvtKwhUsage"].ReadOnly = true;
                        }

                    }
                    else
                    {
                        gvrow.Cells["gvtElecpayment"].ReadOnly = true;
                        gvrow.Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                        gvrow.Cells["gvtKwhUsage"].ReadOnly = true;

                    }
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        if (chkGas.Checked)
                        {
                            gvrow.Cells["gvtGaspay"].ReadOnly = false;
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                            gvrow.Cells["gvtCCFusage"].ReadOnly = false;
                        }
                        else
                        {
                            gvrow.Cells["gvtGaspay"].ReadOnly = true;
                            gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;
                            gvrow.Cells["gvtCCFusage"].ReadOnly = true;
                        }
                    }
                    else
                    {
                        gvrow.Cells["gvtGaspay"].ReadOnly = true;
                        gvrow.Cells["gvtCCFusage"].ReadOnly = true;
                        gvrow.Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;

                    }



                }
            }
            gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
        }

        private void gvwMonthQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Mode == "Add" || Mode == "Edit")
            {
                if (e.ColumnIndex == gvtCheckData.Index && e.RowIndex != -1)
                {
                    string strdata = gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCheckData"].Value.ToString();
                    if (strdata.ToUpper() == "TRUE")
                    {
                        if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtMonthQuesCode"].Value.ToString().ToUpper() != "TOT")
                        {
                            if (chkUtility.Checked)
                            {
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].ReadOnly = false;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtKwhUsage"].ReadOnly = false;
                            }
                            else
                            {
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].ReadOnly = true;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtKwhUsage"].ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].ReadOnly = true;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtKwhUsage"].ReadOnly = true;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                    }
                    //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                    if (BaseForm.BaseAgencyControlDetails.State == "TX")
                    {
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].ReadOnly = false;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtKwhUsage"].ReadOnly = false;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                    }
                    //gvwMonthQuestions.RefreshEdit();
                    gvwMonthQuestions.Update();
                }
                if (e.ColumnIndex == gvcGasCheck.Index && e.RowIndex != -1)
                {
                    string strGasdata = gvwMonthQuestions.Rows[e.RowIndex].Cells["gvcGasCheck"].Value.ToString();
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtMonthQuesCode"].Value.ToString().ToUpper() != "TOT")
                        {
                            if (chkGas.Checked)
                            {
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].ReadOnly = false;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCCFusage"].ReadOnly = false;
                            }
                            else
                            {
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].ReadOnly = true;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;
                                gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCCFusage"].ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].ReadOnly = true;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCCFusage"].ReadOnly = true;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;
                    }

                    //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                    //    || BaseForm.BaseAgencyControlDetails.AgyShortName== "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                    if (BaseForm.BaseAgencyControlDetails.State == "TX")
                    {
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].ReadOnly = false;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCCFusage"].ReadOnly = false;
                        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
            }

        }

        private void CalculationAmount()
        {


            decimal decElecPayment = 0;
            decimal decKwhUsage = 0;
            decimal decGasPayment = 0;
            decimal decCCFUsage = 0;
            decimal decAnnualEnerge = 0;

            foreach (DataGridViewRow gvrows in gvwMonthQuestions.Rows)
            {
                if (gvrows.Cells["gvtMonthQuesCode"].Value.ToString().ToUpper() != "TOT")
                {
                    if (gvrows.Cells["gvtElecpayment"].Value != "")
                    {
                        decElecPayment = decElecPayment + Convert.ToDecimal(gvrows.Cells["gvtElecpayment"].Value);
                    }
                    if (gvrows.Cells["gvtKwhUsage"].Value != "")
                    {
                        decKwhUsage = decKwhUsage + Convert.ToDecimal(gvrows.Cells["gvtKwhUsage"].Value);
                    }
                    if (gvrows.Cells["gvtGaspay"].Value != "")
                    {
                        decGasPayment = decGasPayment + Convert.ToDecimal(gvrows.Cells["gvtGaspay"].Value);
                    }
                    if (gvrows.Cells["gvtCCFusage"].Value != "")
                    {
                        decCCFUsage = decCCFUsage + Convert.ToDecimal(gvrows.Cells["gvtCCFusage"].Value);
                    }
                }
                //if (gvrows.Cells["gvtMonthQuesCode"].Value.ToString().ToUpper() == "TOT")
                //{

                //    if (chkUtility.Checked == false)
                //    {
                //        if (gvrows.Cells["gvtElecpayment"].Value != string.Empty)
                //        {
                //            decElecPayment = Convert.ToDecimal(gvrows.Cells["gvtElecpayment"].Value);
                //        }
                //        else
                //        {
                //            decElecPayment = 0;
                //            gvrows.Cells["gvtElecpayment"].Value = "0";
                //        }
                //    }
                //    else
                //    {
                //        gvrows.Cells["gvtElecpayment"].Value = decElecPayment.ToString();
                //        gvrows.Cells["gvtKwhUsage"].Value = decKwhUsage.ToString();
                //    }
                //    if (chkGas.Checked == false)
                //    {
                //        if (gvrows.Cells["gvtGaspay"].Value != string.Empty)
                //        {
                //            decGasPayment = Convert.ToDecimal(gvrows.Cells["gvtGaspay"].Value);
                //        }
                //        else
                //        {
                //            decGasPayment = 0;
                //            gvrows.Cells["gvtGaspay"].Value = "0";
                //        }
                //    }
                //    else
                //    {
                //        gvrows.Cells["gvtGaspay"].Value = decGasPayment.ToString();
                //        gvrows.Cells["gvtCCFusage"].Value = decCCFUsage.ToString();
                //    }
                //    gvrows.Cells["gvtAnnual"].Value = (decElecPayment + decGasPayment).ToString();

                //}
            }
            if (gvwTotalGrid.Rows.Count > 0)
            {
                //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA" 
                //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                if (BaseForm.BaseAgencyControlDetails.State == "TX")
                {
                    gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value = decElecPayment.ToString();
                    gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].Value = decKwhUsage.ToString();

                    gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value = decGasPayment.ToString();
                    gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].Value = decCCFUsage.ToString();
                }
                else
                {
                    if (chkUtility.Checked == false)
                    {
                        if (gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value != string.Empty)
                        {
                            decElecPayment = Convert.ToDecimal(gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value);
                        }
                        else
                        {
                            decElecPayment = 0;
                            gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value = "0";
                        }
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value = decElecPayment.ToString();
                        gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].Value = decKwhUsage.ToString();
                    }
                    if (chkGas.Checked == false)
                    {
                        if (gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value != string.Empty)
                        {
                            decGasPayment = Convert.ToDecimal(gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value);
                        }
                        else
                        {
                            decGasPayment = 0;
                            gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value = "0";
                        }
                    }
                    else
                    {
                        gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value = decGasPayment.ToString();
                        gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].Value = decCCFUsage.ToString();
                    }
                }
                gvwTotalGrid.Rows[0].Cells["gvttAnnual"].Value = (decElecPayment + decGasPayment).ToString();

            }
        }

        private void saveMonthlyQuestions(CaseSnpEntity CaseSnpEntity)
        {
            int truechk = 0;
            foreach (DataGridViewRow dataGridViewRow in gvwMonthQuestions.Rows)
            {
                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                CustomQuestionsEntity questionEntity = dataGridViewRow.Tag as CustomQuestionsEntity;
                custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                custEntity.ACTDEPT = CaseSnpEntity.Dept;
                custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                if (CaseSnpEntity.Year == string.Empty)
                    custEntity.ACTYEAR = "    ";
                else
                    custEntity.ACTYEAR = CaseSnpEntity.Year;
                custEntity.ACTAPPNO = CaseSnpEntity.App;


                custEntity.USAGE_MONTH = dataGridViewRow.Cells["gvtMonthQuesCode"].Value.ToString();

                custEntity.USAGE_PMON_SW = "N";
                custEntity.USAGE_SMON_SW = "N";
                if (dataGridViewRow.Cells["gvtCheckData"].Value.ToString().ToUpper() == "TRUE")
                {
                    custEntity.USAGE_PMON_SW = "Y";
                }
                if (dataGridViewRow.Cells["gvcGasCheck"].Value.ToString().ToUpper() == "TRUE")
                {
                    custEntity.USAGE_SMON_SW = "Y";
                }


                custEntity.USAGE_PRIM_12HIST = chkUtility.Checked == true ? "Y" : "N";
                custEntity.USAGE_SEC_12HIST = chkGas.Checked == true ? "Y" : "N";
                //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA" 
                //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                if (BaseForm.BaseAgencyControlDetails.State == "TX")
                {
                    custEntity.USAGE_PRIM_PAYMENT = dataGridViewRow.Cells["gvtElecpayment"].Value != null ? dataGridViewRow.Cells["gvtElecpayment"].Value.ToString() : string.Empty;
                    custEntity.USAGE_PRIM_USAGE = dataGridViewRow.Cells["gvtKwhUsage"].Value != null ? dataGridViewRow.Cells["gvtKwhUsage"].Value.ToString() : string.Empty;

                }
                else
                {
                    if (chkUtility.Checked)
                    {
                        custEntity.USAGE_PRIM_PAYMENT = dataGridViewRow.Cells["gvtElecpayment"].Value != null ? dataGridViewRow.Cells["gvtElecpayment"].Value.ToString() : string.Empty;
                        custEntity.USAGE_PRIM_USAGE = dataGridViewRow.Cells["gvtKwhUsage"].Value != null ? dataGridViewRow.Cells["gvtKwhUsage"].Value.ToString() : string.Empty;
                    }
                    else
                    {

                        custEntity.USAGE_PRIM_PAYMENT = string.Empty;
                        custEntity.USAGE_PRIM_USAGE = string.Empty;

                    }
                }
                //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "STDC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS" || BaseForm.BaseAgencyControlDetails.AgyShortName == "NCCAA" 
                //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "FTW" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CSNT" || BaseForm.BaseAgencyControlDetails.AgyShortName == "BVCOG"
                //    || BaseForm.BaseAgencyControlDetails.AgyShortName == "HCCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName == "RMPC" || BaseForm.BaseAgencyControlDetails.AgyShortName == "CVCAA")
                if (BaseForm.BaseAgencyControlDetails.State == "TX")
                {
                    custEntity.USAGE_SEC_PAYMENT = dataGridViewRow.Cells["gvtGaspay"].Value != null ? dataGridViewRow.Cells["gvtGaspay"].Value.ToString() : string.Empty;
                    custEntity.USAGE_SEC_USAGE = dataGridViewRow.Cells["gvtCCFusage"].Value != null ? dataGridViewRow.Cells["gvtCCFusage"].Value.ToString() : string.Empty;

                }
                else
                {
                    if (chkGas.Checked)
                    {
                        custEntity.USAGE_SEC_PAYMENT = dataGridViewRow.Cells["gvtGaspay"].Value != null ? dataGridViewRow.Cells["gvtGaspay"].Value.ToString() : string.Empty;
                        custEntity.USAGE_SEC_USAGE = dataGridViewRow.Cells["gvtCCFusage"].Value != null ? dataGridViewRow.Cells["gvtCCFusage"].Value.ToString() : string.Empty;
                    }
                    else
                    {
                        custEntity.USAGE_SEC_PAYMENT = string.Empty;
                        custEntity.USAGE_SEC_USAGE = string.Empty;

                    }
                }
                custEntity.USAGE_TOTAL = dataGridViewRow.Cells["gvtAnnual"].Value != null ? dataGridViewRow.Cells["gvtAnnual"].Value.ToString() : string.Empty;
                custEntity.USAGE_SSOURCE = string.Empty;
                custEntity.USAGE_PRIM_VEND = string.Empty;
                custEntity.USAGE_SEC_VEND = string.Empty;
                custEntity.addoperator = BaseForm.UserID;
                custEntity.lstcoperator = BaseForm.UserID;
                custEntity.Mode = "";

                bool chk = _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);
                if (chk)
                    truechk++;

            }
            if (gvwTotalGrid.Rows.Count > 0)
            {
                CalculationAmount();
                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                CustomQuestionsEntity questionEntity = gvwTotalGrid.Rows[0].Tag as CustomQuestionsEntity;
                custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                custEntity.ACTDEPT = CaseSnpEntity.Dept;
                custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                if (CaseSnpEntity.Year == string.Empty)
                    custEntity.ACTYEAR = "    ";
                else
                    custEntity.ACTYEAR = CaseSnpEntity.Year;
                custEntity.ACTAPPNO = CaseSnpEntity.App;


                custEntity.USAGE_MONTH = gvwTotalGrid.Rows[0].Cells["gvttMonthQuesCode"].Value.ToString();

                custEntity.USAGE_PMON_SW = "N";
                custEntity.USAGE_SMON_SW = "N";
                if (gvwTotalGrid.Rows[0].Cells["gvttCheckData"].Value.ToString().ToUpper() == "TRUE")
                {
                    custEntity.USAGE_PMON_SW = "Y";
                }
                if (gvwTotalGrid.Rows[0].Cells["gvctGasCheck"].Value.ToString().ToUpper() == "TRUE")
                {
                    custEntity.USAGE_SMON_SW = "Y";
                }


                custEntity.USAGE_PRIM_12HIST = chkUtility.Checked == true ? "Y" : "N";
                custEntity.USAGE_SEC_12HIST = chkGas.Checked == true ? "Y" : "N";

                if (custEntity.USAGE_MONTH.ToUpper().ToString() == "TOT")
                {
                    custEntity.USAGE_PRIM_PAYMENT = gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttElecpayment"].Value.ToString() : string.Empty;
                    custEntity.USAGE_PRIM_USAGE = gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttKwhUsage"].Value.ToString() : string.Empty;
                }



                if (custEntity.USAGE_MONTH.ToUpper().ToString() == "TOT")
                {
                    custEntity.USAGE_SEC_PAYMENT = gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttGaspay"].Value.ToString() : string.Empty;
                    custEntity.USAGE_SEC_USAGE = gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttCCFusage"].Value.ToString() : string.Empty;
                }

                custEntity.USAGE_TOTAL = gvwTotalGrid.Rows[0].Cells["gvttAnnual"].Value != null ? gvwTotalGrid.Rows[0].Cells["gvttAnnual"].Value.ToString() : string.Empty;
                if (((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString() != "0")
                    custEntity.USAGE_PSOURCE = ((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString();
                if (((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString() != "0")
                    custEntity.USAGE_SSOURCE = ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString();
                custEntity.USAGE_PRIM_VEND = txtVendorId.Text;
                custEntity.USAGE_SEC_VEND = txtvendor2.Text;
                custEntity.addoperator = BaseForm.UserID;
                custEntity.lstcoperator = BaseForm.UserID;
                custEntity.Mode = "";

                _model.CaseMstData.CAPS_CASEUSAGE_INSUPDEL(custEntity);
            }
            custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            //Added by Sudheer on 05/25/2022
            if (custResponses.Count > 0)
            {
                if (Privileges.AddPriv.Equals("false"))
                {
                    Pb_Add_Benefit.Visible = false;
                }
                else
                {
                    Pb_Add_Benefit.Visible = true;
                }
            }
            else
                Pb_Add_Benefit.Visible = true;

            ShowButtonVisiable();
            buttonsEnableDisable();

            if (truechk > 0)
            {
                //gvwMonthQuestions.SelectedCells[0].Selected = false;
                AlertBox.Show("Saved Successfully");
            }
        }

        List<CASESPMEntity> CASESPM_SP_List;
        CASESPMEntity Search_Entity = new CASESPMEntity();
        private void Fill_Applicant_SPs()
        {
            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;
            //Search_Entity.year = BaseYear;
            Search_Entity.year = null;                          // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = BaseForm.BaseApplicationNo;

            Search_Entity.service_plan = Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program = //Search_Entity.SPM_MassClose =
            Search_Entity.SPM_MassClose = Search_Entity.Seq = Search_Entity.Bulk_Post = null;

            CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
        }

        public void Refresh()
        {
            //panel1.Enabled = false;

            DisableRows();
            panel2.Visible = false;
            chkGas.Checked = false; chkUtility.Checked = false;
            Mode = "";
            //gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            //gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            //fillMonthlyQuestions();
            //gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            //gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);


            GetCaseactdata();
            Fill_CaseWorker();
            Fill_Sel_Hierarchy_SPs();

            gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            fillMonthlyQuestions();
            gvwMonthQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwMonthQuestions_CellValueChanged);
            gvwTotalGrid.CellValueChanged += new DataGridViewCellEventHandler(gvwTotalGrid_CellValueChanged);
            ShowButtonVisiable();
            panel3.Visible = true;
            pnlBenefitsection.Enabled = true;
            Fill_Applicant_SPs();
            fillBenefits(string.Empty, string.Empty);

            if (custResponses.Count > 0)
            {
                if (Privileges.AddPriv.Equals("false"))
                {
                    Pb_Add_Benefit.Visible = false;
                }
                else
                {
                    Pb_Add_Benefit.Visible = true;
                }
            }
            else
                Pb_Add_Benefit.Visible = true;


        }

        private void fillBenefits(string Sel_SP_Code, string SPM_Seq)
        {
            if (CASESPM_SP_List.Count > 0)
                CASESPM_SP_List = CASESPM_SP_List.FindAll(u => u.SPM_EligStatus != string.Empty);

            gvwBenefit.Rows.Clear();

            Pb_Delete_Benefit.Visible = false;
            Pb_Edit_Benefit.Visible = false;

            int TmpCount = 0;
            if (CASESPM_SP_List.Count > 0)
            {
                string Tmp_SPCoce = null, Start_Date = null;
                int rowIndex = 0; int Sel_SP_Index = 0;
                foreach (CASESPMEntity Entity in CASESPM_SP_List)
                {
                    string IsLoad = "Y";
                    if (ACR_SERV_Hies == "Y")
                    {
                        if (Service_Hierarchies.Count > 0)
                        {
                            CASESP1Entity SelHieEntity = Service_Hierarchies.Find(u => u.Code.Trim() == Entity.service_plan.Trim());
                            if (SelHieEntity != null) IsLoad = "Y"; else IsLoad = "N";
                        }
                        else IsLoad = "N";
                    }

                    string strFunddesc = string.Empty;
                    if (propfundingsource.Count > 0)
                    {
                        CommonEntity commonfunddesc = propfundingsource.Find(u => u.Code == Entity.SPM_Fund.Trim());
                        if (commonfunddesc != null)
                            strFunddesc = commonfunddesc.Desc.ToString();
                    }

                    string Status = string.Empty;
                    if (Entity.SPM_EligStatus.Trim() == "P") Status = "Pending";
                    else if (Entity.SPM_EligStatus.Trim() == "E") Status = "Eligible";
                    else if (Entity.SPM_EligStatus.Trim() == "D") Status = "Denied";
                    else if (Entity.SPM_EligStatus.Trim() == "S") Status = "Eligible";
                    else if (Entity.SPM_EligStatus.Trim() == "M") Status = "Eligible";
                    else if (Entity.SPM_EligStatus.Trim() == "N") Status = "Eligible";


                    if (IsLoad == "Y") //if (List.Value.ToString() == Entity.year + Entity.Seq)
                    {
                        Tmp_SPCoce = Entity.service_plan.ToString();
                        Tmp_SPCoce = "000000".Substring(0, (6 - Tmp_SPCoce.Length)) + Tmp_SPCoce;

                        if (!string.IsNullOrEmpty(Entity.startdate))
                            Start_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.startdate.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        //if (Entity.have_addlbr == "Y")
                        //    rowIndex = gvwBenefit.Rows.Add(strFunddesc, Entity.Sp0_Desc, Entity.Site_Desc, Get_CaseWorker_DESC(Entity.caseworker), Start_Date, Img_Tick, Entity.Sp0_Validatetd, Entity.CA_Postings_Cnt, Entity.MS_Postings_Cnt, Entity.Seq, Entity.year);
                        //else
                        rowIndex = gvwBenefit.Rows.Add(strFunddesc, Entity.Sp0_Desc, Entity.caseworker, Start_Date, Entity.SPM_Amount, Entity.SPM_Balance, Status, Entity.service_plan, Entity.Seq, Entity.year, Entity.CA_Postings_Cnt, Entity.MS_Postings_Cnt, Entity.SPM_Fund.Trim());

                        if (Sel_SP_Code.Trim() == Entity.service_plan.Trim() && SPM_Seq == Entity.Seq)
                            Sel_SP_Index = TmpCount;

                        set_SP_Tooltip(TmpCount, Entity);

                        gvwBenefit.Rows[rowIndex].Tag = Entity;

                        TmpCount++;

                        if (Entity.Sp0_Validatetd == "N" || Entity.Sp0_Validatetd != "Y")
                            gvwBenefit.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red; //Color.Peru; //Color.DarkTurquoise;

                        //break;
                    }
                }

                if (TmpCount > 0)
                {

                    if (string.IsNullOrEmpty(Sel_SP_Code))
                    {
                        Sel_SP_Code = Sel_SP_Code;//gvwBenefit.Rows[0].Tag = 0;
                        gvwBenefit.CurrentCell = gvwBenefit.Rows[0].Cells[1];
                    }
                    else
                    {
                        gvwBenefit.CurrentCell = gvwBenefit.Rows[Sel_SP_Index].Cells[1];

                        int scrollPosition = 0;
                        scrollPosition = gvwBenefit.CurrentCell.RowIndex;
                        //int CurrentPage = (scrollPosition / gvwBenefit.ItemsPerPage);
                        // CurrentPage++;
                        // gvwBenefit.CurrentPage = CurrentPage;
                        //  gvwBenefit.FirstDisplayedScrollingRowIndex = scrollPosition;
                    }

                    if (Privileges.DelPriv.Equals("true"))
                        Pb_Delete_Benefit.Visible = true;
                    if (Privileges.ChangePriv.Equals("true"))
                    {
                        Pb_Edit_Benefit.Visible = true;
                    }
                    //if (BaseForm.BaseAgencyControlDetails.AgyShortName == "PCS")
                    //{
                    btnPost.Visible = true; 
                    if(BaseForm.BaseAgencyControlDetails.CEAPPostUsage!="Y")
                        btnOtherUsage.Visible = true;
                    buttonsEnableDisable();
                    //if (BaseForm.BusinessModuleID == "08")  //Commented by Sudheer on 10/12/2016
                    //    Btn_Triggers.Visible = true;

                    if (gvwBenefit.CurrentRow.Cells["gvStatus"].Value.ToString() == "Eligible" && gvwBenefit.CurrentRow.Cells["gvFundCode"].Value.ToString().Trim() != "")
                    {
                        if (btnPost.Enabled == true) btnPost.Enabled = true;
                        if (btnOtherUsage.Enabled == true) btnPost.Enabled = true;
                    }
                    else
                    {
                        btnPost.Enabled = false; btnOtherUsage.Enabled = false;
                    }
                    //}

                }
            }
            else
            {
                Pb_Delete_Benefit.Visible = false;
                Pb_Edit_Benefit.Visible = false;
                btnPost.Visible = false;
                btnOtherUsage.Visible = false;
            }

            if (gvwBenefit.Rows.Count > 0)
                gvwBenefit.Rows[0].Selected = true;
        }


        string ACR_SERV_Hies = string.Empty;
        public void RefreshGrid(string Sel_SP_Code, string SPM_Seq)
        {
            //strDeletMsg = "Delete";

            gvwBenefit.Rows.Clear();

            string strFund = string.Empty;
            string strDesc = string.Empty;

            fillMonthlyQuestions();

            CASESPM_SP_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
            GetCaseactdata();
            if (custResponses.Count > 0)
            {
                if (Privileges.AddPriv.Equals("false"))
                {
                    Pb_Add_Benefit.Visible = false;
                }
                else
                {
                    Pb_Add_Benefit.Visible = true;
                }
            }
            else
                Pb_Add_Benefit.Visible = true;


            fillBenefits(Sel_SP_Code, SPM_Seq);



        }

        private void set_SP_Tooltip(int rowIndex, CASESPMEntity Entity)
        {
            string toolTipText = "Added By     : " + Entity.add_operator.Trim() + " on " + Entity.date_add.ToString() + "\n" +
                                 "Modified By  : " + Entity.lstc_operator.Trim() + " on " + Entity.date_lstc.ToString();

            foreach (DataGridViewCell cell in gvwBenefit.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }


        private string Get_CaseWorker_DESC(string Worker_Code)
        {
            string DESC = null;
            foreach (Captain.Common.Utilities.ListItem List in CaseWorker_List)
            {
                if (List.Value.ToString().Trim() == Worker_Code.Trim())
                {
                    DESC = List.Text; break;
                }
            }

            return DESC;
        }


        List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
        List<CASESP1Entity> Service_Hierarchies = new List<CASESP1Entity>();
        private void Fill_Sel_Hierarchy_SPs()
        {
            //if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            //    Service_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, null, null, null, BaseForm.UserID);

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    Service_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, BaseForm.BaseDept, null, BaseForm.UserID);
                else
                    Service_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, BaseForm.BaseAgency, null, null, BaseForm.UserID);
            }

            SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
        }


        private void Pb_Add_Benefit_Click(object sender, EventArgs e)
        {
            if (CEAPCNTL_List.Count > 0)
            {
                if (AdminControlValidation("CASE0016"))
                {
                    CASE0016_Form addUserForm = new CASE0016_Form(BaseForm, "Add", "New SP", "1", BaseForm.BaseYear, ((Utilities.ListItem)cmbSource.SelectedItem).Text.Trim(), Privileges);
                    addUserForm.StartPosition = FormStartPosition.CenterScreen;
                    addUserForm.ShowDialog();
                }
            }
            else
            {
                AlertBox.Show("Please define CEAP Control Details", MessageBoxIcon.Warning);
            }

        }

        private void Pb_Edit_Benefit_Click(object sender, EventArgs e)
        {
            if (CEAPCNTL_List.Count > 0)
            {
                if (AdminControlValidation("CASE0016"))
                {
                    CASE0016_Form addUserForm = new CASE0016_Form(BaseForm, "Edit", gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString(), ((Utilities.ListItem)cmbSource.SelectedItem).Text.Trim(), Privileges);
                    addUserForm.StartPosition = FormStartPosition.CenterScreen;
                    addUserForm.ShowDialog();
                }
            }
            else
            {
                AlertBox.Show("Please define CEAP Control Details", MessageBoxIcon.Warning);
            }

        }

        private void Pb_Delete_Benefit_Click(object sender, EventArgs e)
        {
            if (gvwBenefit.Rows.Count > 0)
            {
                bool IsDelete = true;
                //if (!(BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                //{
                //    if (SERVStopEntity != null)
                //    {
                //        if (Convert.ToDateTime(SERVStopEntity.TDate.Trim()) >= Convert.ToDateTime(SPGrid.CurrentRow.Cells["Start_Date"].Value.ToString()) && Convert.ToDateTime(SERVStopEntity.FDate.Trim()) <= Convert.ToDateTime(SPGrid.CurrentRow.Cells["Start_Date"].Value.ToString()))
                //        {
                //            IsDelete = false;

                //            //_errorProvider.SetError(Start_Date, string.Format(" " + Lbl_StartDate.Text + " Should not be between " + LookupDataAccess.Getdate(SERVStopEntity.FDate.Trim()) + " and " + LookupDataAccess.Getdate(SERVStopEntity.TDate.Trim()).Replace(Consts.Common.Colon, string.Empty)));

                //        }

                //    }
                //}

                if (IsDelete)
                {
                    if ((int.Parse(gvwBenefit.CurrentRow.Cells["gvCA_Cnt"].Value.ToString()) == 0) && (int.Parse(gvwBenefit.CurrentRow.Cells["gvMS_Cnt"].Value.ToString()) == 0))
                        //MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Posting for Service Plan : " + SPGrid.CurrentRow.Cells["SP_Code"].Value.ToString(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, Delete_Selected_SPM, true);
                        MessageBox.Show("Are you sure you want to delete the Benefit?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Selected_SPM);
                    else
                        AlertBox.Show("You can’t delete this Benefit, \n as Service and/or Outcome Details already posted.", MessageBoxIcon.Warning);
                }
                //else
                //    MessageBox.Show("You Can’t delete this Service Plan between " + LookupDataAccess.Getdate(SERVStopEntity.FDate.Trim()) + " and " + LookupDataAccess.Getdate(SERVStopEntity.TDate.Trim()));
            }
        }

        string Sql_SP_Result_Message = string.Empty, Tmp_SPM_Sequence = string.Empty;
        private void Delete_Selected_SPM(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                CASESPMEntity Search_Entity = new CASESPMEntity(true);
                Search_Entity.Rec_Type = "D";
                Search_Entity.agency = BaseForm.BaseAgency;
                Search_Entity.dept = BaseForm.BaseDept;
                Search_Entity.program = BaseForm.BaseProg;

                //if (!string.IsNullOrEmpty(MainMenuYear))
                //    Search_Entity.year = MainMenuYear;
                Search_Entity.year = gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString();
                Search_Entity.app_no = BaseForm.BaseApplicationNo;
                Search_Entity.service_plan = gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString();
                Search_Entity.Seq = gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString();

                Search_Entity.lstc_operator = BaseForm.UserID;

                //Search_Entity.caseworker = Search_Entity.site = null;
                //Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
                //Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
                //Search_Entity.date_add = Search_Entity.add_operator = null;
                //Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = null;

                if (_model.SPAdminData.UpdateCASESPM(Search_Entity, "Delete", out Sql_SP_Result_Message, out Tmp_SPM_Sequence))
                {
                    AlertBox.Show("Benefit Record Deleteted Successfully");

                    if (gvwBenefit.CurrentRow.Cells["gvFundCode"].Value.ToString().Trim() == "")
                    {
                        RefreshGrid(string.Empty, string.Empty);
                    }
                    else
                    {
                        Emsbdc_List = _model.SPAdminData.GetCMBdcAllData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, gvwBenefit.CurrentRow.Cells["gvFundCode"].Value.ToString());


                        CMBDCEntity emsbdcentity = Emsbdc_List.Find(u => u.BDC_FUND == gvwBenefit.CurrentRow.Cells["gvFundCode"].Value.ToString() && (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(gvwBenefit.CurrentRow.Cells["gvBenDate"].Value.ToString()) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(gvwBenefit.CurrentRow.Cells["gvBenDate"].Value.ToString())));
                        if (emsbdcentity != null)
                        {
                            CMBDCEntity emsbdcdata = new CMBDCEntity();
                            emsbdcdata.BDC_AGENCY = emsbdcentity.BDC_AGENCY;
                            emsbdcdata.BDC_DEPT = emsbdcentity.BDC_DEPT;
                            emsbdcdata.BDC_PROGRAM = emsbdcentity.BDC_PROGRAM;
                            emsbdcdata.BDC_YEAR = emsbdcentity.BDC_YEAR;


                            emsbdcdata.BDC_DESCRIPTION = emsbdcentity.BDC_DESCRIPTION;
                            emsbdcdata.BDC_FUND = emsbdcentity.BDC_FUND;
                            emsbdcdata.BDC_START = emsbdcentity.BDC_START;
                            emsbdcdata.BDC_END = emsbdcentity.BDC_END;
                            emsbdcdata.BDC_ID = emsbdcentity.BDC_ID;
                            emsbdcdata.BDC_BUDGET = emsbdcentity.BDC_BUDGET;
                            emsbdcdata.BDC_LSTC_OPERATOR = BaseForm.UserID;
                            emsbdcdata.Mode = "BdcAmount";
                            string strstatus = string.Empty;
                            if (_model.SPAdminData.InsertUpdateDelCMBDC(emsbdcdata, out strstatus))
                            {
                            }


                            RefreshGrid(string.Empty, string.Empty);
                        }
                        else
                            AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }

                    //MessageBox.Show("Unsuccessful Service Plan Delete \n Reason : " + Sql_SP_Result_Message, "CAP Systems");
                }
            }
        }

        public string propReportPath { get; set; }

        private void buttonsEnableDisable()
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {
                CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
                if (responsetot != null)
                {
                    if (!string.IsNullOrEmpty(responsetot.USAGE_SEC_PAYMENT.Trim()))
                    {
                        if (Convert.ToDecimal(responsetot.USAGE_SEC_PAYMENT.Trim()) > 0)
                        {
                            btnOtherUsage.Enabled = true;
                            TypeOther = "O";
                        }
                        else
                        {
                            btnOtherUsage.Enabled = false;
                            TypeOther = string.Empty;
                        }
                    }

                    if (!string.IsNullOrEmpty(responsetot.USAGE_PRIM_PAYMENT.Trim()))
                    {
                        if (Convert.ToDecimal(responsetot.USAGE_PRIM_PAYMENT.Trim()) > 0)
                        {
                            if (gvwBenefit.Rows.Count > 0)
                            {
                                if (gvwBenefit.CurrentRow.Cells["gvStatus"].Value.ToString() == "Eligible")
                                {
                                    btnPost.Enabled = true;
                                    TypeElec = "E";
                                }
                            }
                        }
                        else
                        {
                            btnPost.Enabled = false;
                            TypeElec = string.Empty;
                        }
                    }
                }
                else
                {
                    btnPost.Enabled = false; btnOtherUsage.Enabled = false;
                }


            }
        }



        private void pb_Print_Click(object sender, EventArgs e)
        {
            string strSpcCode = string.Empty;
            string strSPMSEQ = string.Empty;
            string strEligStatus = string.Empty;
            if (gvwBenefit.Rows.Count > 0)
            {
                strSpcCode = gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString();
                strSPMSEQ = gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString();
                strEligStatus = gvwBenefit.CurrentRow.Cells["gvStatus"].Value.ToString();
            }
            string SecSource = string.Empty;
            if (((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString() != "0")
                SecSource = ((Utilities.ListItem)cmbSource.SelectedItem).Text.ToString();

            string PrimVend = string.Empty; string SecVend = string.Empty;
            if(!string.IsNullOrEmpty(txtVendorId.Text.Trim()))
                PrimVend=txtVendorId.Text;
            if (!string.IsNullOrEmpty(txtvendor2.Text.Trim()))
                SecVend= txtvendor2.Text;

            PrintLetters PrintAppl = new PrintLetters(BaseForm, Privileges, "CASE0016", strEligStatus, CASESPM_SP_List, strSpcCode, strSPMSEQ, SecSource,PrimVend,SecVend);
            PrintAppl.StartPosition = FormStartPosition.CenterScreen;
            PrintAppl.ShowDialog();
            //On_SaveForm_Closed();
        }

        private void gvwTotalGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (Mode == "Add" || Mode == "Edit")
            {
                if (e.ColumnIndex == gvttCheckData.Index && e.RowIndex != -1)
                {
                    string strdata = gvwTotalGrid.Rows[e.RowIndex].Cells["gvttCheckData"].Value.ToString();
                    if (strdata.ToUpper() == "TRUE")
                    {
                        //if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtMonthQuesCode"].Value.ToString().ToUpper() != "TOT")
                        //{
                        //    if (chkUtility.Checked)
                        //    {
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].ReadOnly = false;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtKwhUsage"].ReadOnly = false;
                        //    }
                        //    else
                        //    {
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].ReadOnly = true;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtElecpayment"].Style.BackColor = System.Drawing.Color.Red;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtKwhUsage"].ReadOnly = true;
                        //    }
                        //}
                    }
                    else
                    {
                        gvwTotalGrid.Rows[e.RowIndex].Cells["gvttElecpayment"].ReadOnly = true;
                        gvwTotalGrid.Rows[e.RowIndex].Cells["gvttKwhUsage"].ReadOnly = true;
                        gvwTotalGrid.Rows[e.RowIndex].Cells["gvttElecpayment"].Style.BackColor = System.Drawing.Color.LightGreen;
                    }
                    //gvwMonthQuestions.RefreshEdit();
                    gvwTotalGrid.Update();
                }
                if (e.ColumnIndex == gvctGasCheck.Index && e.RowIndex != -1)
                {
                    string strGasdata = gvwTotalGrid.Rows[e.RowIndex].Cells["gvctGasCheck"].Value.ToString();
                    if (strGasdata.ToUpper() == "TRUE")
                    {
                        //if (gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtMonthQuesCode"].Value.ToString().ToUpper() != "TOT")
                        //{
                        //    if (chkGas.Checked)
                        //    {
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].ReadOnly = false;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCCFusage"].ReadOnly = false;
                        //    }
                        //    else
                        //    {
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].ReadOnly = true;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtGaspay"].Style.BackColor = System.Drawing.Color.Red;
                        //        gvwMonthQuestions.Rows[e.RowIndex].Cells["gvtCCFusage"].ReadOnly = true;
                        //    }
                        //}
                    }
                    else
                    {
                        gvwTotalGrid.Rows[e.RowIndex].Cells["gvttGaspay"].ReadOnly = true;
                        gvwTotalGrid.Rows[e.RowIndex].Cells["gvttCCFusage"].ReadOnly = true;
                        gvwTotalGrid.Rows[e.RowIndex].Cells["gvttGaspay"].Style.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
            }



        }

        string PrintText = null;

        private void btnvendor1_Click(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString() != "0")
            {
                VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, ((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString(), string.Empty, null);
                Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Closed);
                Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                Vendor_Browse.ShowDialog();
            }
            else
            {
                AlertBox.Show("Please select Primary Source", MessageBoxIcon.Warning);
            }
        }

        private void btnVendor2_Click(object sender, EventArgs e)
        {
            if (((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString() != "0")
            {
                VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString(), string.Empty, null);
                Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse2_Closed);
                Vendor_Browse.StartPosition = FormStartPosition.CenterScreen;
                Vendor_Browse.ShowDialog();
            }
            else
            {
                AlertBox.Show("Please select Secondary Source", MessageBoxIcon.Warning);
            }
        }

        private void On_Vendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                // _errorProvider.SetError(txtWatVendorId, null);
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();
                txtVendorId.Text = Vendor_Details[0].Trim();
                txtVendorName.Text = Vendor_Details[1].Trim();

                //txtVendorId_TextChanged(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gvwMonthQuestions.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(gvwBenefit.CurrentRow.Tag.ToString().Trim()))
                {
                    CASESPMEntity Entity = gvwBenefit.CurrentRow.Tag as CASESPMEntity;
                    if (Entity != null)
                    {
                        if (string.IsNullOrEmpty(Entity.SPM_Gas_Vendor.Trim()) || string.IsNullOrEmpty(Entity.SPM_Gas_Account.Trim()) || string.IsNullOrEmpty(Entity.SPM_Gas_BillName_Type.Trim()))
                        {
                            AlertBox.Show("Please review the Service Plan and confirm Vendor, Acct# and Billing Name Details", MessageBoxIcon.Warning);
                        }
                        else if (CEAPCNTL_List.Count == 0)
                        {
                            AlertBox.Show("Please define CEAP Control Details", MessageBoxIcon.Warning);
                        }
                        else
                        {
                            CASE0016_Usage UsageForm = new CASE0016_Usage(BaseForm, "", gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSP"].Value.ToString(), ((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString().Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString().Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString(), "O", Entity, Privileges);
                            UsageForm.FormClosed += new FormClosedEventHandler(OnCase0016_UsageFormClosed);
                            UsageForm.StartPosition = FormStartPosition.CenterScreen;
                            UsageForm.ShowDialog();
                        }
                    }
                }


                //CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
                //if(responsetot!=null)
                //{
                //    if(!string.IsNullOrEmpty(responsetot.USAGE_SEC_PAYMENT.Trim()))
                //    {
                //        if(Convert.ToDecimal(responsetot.USAGE_SEC_PAYMENT.Trim())>0)
                //        {
                //            CASE0016_Usage UsageForm = new CASE0016_Usage(BaseForm, "", gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSP"].Value.ToString(), ((Utilities.ListItem)cmbSource.SelectedItem).Text.Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString(), "O", Privileges);
                //            UsageForm.ShowDialog();
                //        }
                //        else
                //        {
                //            MessageBox.Show("There is no Amounts defined in the Gas Payments on the Usage/Consumption");
                //        }
                //    }
                //}
            }

        }

        private void On_Vendor_Browse2_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                // _errorProvider.SetError(txtWatVendorId, null);
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();
                txtvendor2.Text = Vendor_Details[0].Trim();
                txtVendorName2.Text = Vendor_Details[1].Trim();

                //txtVendorId_TextChanged(sender, e);
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {

        }
        string TypeElec = string.Empty; string TypeOther = string.Empty;
        private void btnPost_Click(object sender, EventArgs e)
        {
            if (gvwBenefit.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(gvwBenefit.CurrentRow.Tag.ToString().Trim()))
                {
                    CASESPMEntity Entity = gvwBenefit.CurrentRow.Tag as CASESPMEntity;
                    if (Entity != null)
                    {
                        if (string.IsNullOrEmpty(Entity.SPM_Vendor.Trim()) || string.IsNullOrEmpty(Entity.SPM_Account.Trim()) || string.IsNullOrEmpty(Entity.SPM_BillName_Type.Trim()))
                        {
                            AlertBox.Show("Please review the Service Plan and confirm Vendor, Acct# and Billing Name Details", MessageBoxIcon.Warning);
                        }
                        else if (CEAPCNTL_List.Count == 0)
                        {
                            AlertBox.Show("Please define CEAP Control Details", MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (BaseForm.BaseAgencyControlDetails.CEAPPostUsage == "Y")
                            {
                                //if (string.IsNullOrEmpty(Entity.SPM_Gas_Vendor.Trim()) || string.IsNullOrEmpty(Entity.SPM_Gas_Account.Trim()) || string.IsNullOrEmpty(Entity.SPM_Gas_BillName_Type.Trim()))
                                //{
                                //    MessageBox.Show("Please review the Service Plan and confirm Vendor, Acct# and Billing Name Details", "");
                                //}


                                CASE1016_Usage postUsageForm = new CASE1016_Usage(BaseForm, "", gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSP"].Value.ToString(), ((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString().Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString().Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString(), TypeElec, TypeOther, Entity, Privileges);
                                postUsageForm.FormClosed += new FormClosedEventHandler(OnCase1016_UsageFormClosed);
                                postUsageForm.StartPosition = FormStartPosition.CenterScreen;
                                postUsageForm.ShowDialog();
                            }
                            else
                            {
                                CASE0016_Usage UsageForm = new CASE0016_Usage(BaseForm, "", gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSP"].Value.ToString(), ((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString().Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString().Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString(), "E", Entity, Privileges);
                                UsageForm.FormClosed += new FormClosedEventHandler(OnCase0016_UsageFormClosed);
                                UsageForm.StartPosition = FormStartPosition.CenterScreen;
                                UsageForm.ShowDialog();
                            }

                        }
                    }
                }


                //CASE0016_Usage UsageForm = new CASE0016_Usage(BaseForm, "", gvwBenefit.CurrentRow.Cells["gvSPCode"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSPMSeq"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSpm_Year"].Value.ToString(), gvwBenefit.CurrentRow.Cells["gvSP"].Value.ToString(),((Utilities.ListItem)cmbSource.SelectedItem).Text.Trim(), ((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString(),"E", Privileges);
                //UsageForm.ShowDialog();
            }
        }

        private void OnCase0016_UsageFormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid(string.Empty, string.Empty);
        }


        private void OnCase1016_UsageFormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid(string.Empty, string.Empty);
        }


        #region CEAP_Priority_Rating

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        int pageNumber = 1; string PdfName = "Pdf File";
        string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSource.Items.Count > 0)
            {
                if (((Utilities.ListItem)cmbSource.SelectedItem).Value.ToString() != "0")
                {
                    gvtCCFusage.HeaderText = UsageHeader(((Utilities.ListItem)cmbSource.SelectedItem).Text.ToString());
                    txtVendorName2.Text = string.Empty;
                    txtvendor2.Text = string.Empty;
                }
                else
                {
                    gvtCCFusage.HeaderText = "Usage";
                    txtVendorName2.Text = string.Empty;
                    txtvendor2.Text = string.Empty;
                }
            }
        }

        private void cmbPsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPsource.Items.Count > 0)
            {
                if (((Utilities.ListItem)cmbPsource.SelectedItem).Value.ToString() != "0")
                {
                    gvtKwhUsage.HeaderText = UsageHeader(((Utilities.ListItem)cmbPsource.SelectedItem).Text.ToString());
                    txtVendorName.Text = string.Empty;
                    txtVendorId.Text = string.Empty;
                }
                else
                {
                    gvtKwhUsage.HeaderText = "Usage";
                    txtVendorName.Text = string.Empty;
                    txtVendorId.Text = string.Empty;
                }
            }
        }

        private void txtVendorName2_TextChanged(object sender, EventArgs e)
        {

        }

        private void upload1_Uploaded(object sender, UploadedEventArgs e)
        {
            DirectoryInfo MyDir = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            FileInfo[] MyFiles = MyDir.GetFiles("*.*");
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    MyFile.Delete();
                }
            }

            var mobjResult = e.Files[0];
            string strName = mobjResult.FileName;
            string strType = mobjResult.ContentType;
            long strr = mobjResult.ContentLength;
            lblPInvFileName.Text = mobjResult.FileName;
            string strModifyfilename = mobjResult.FileName;
            strModifyfilename = strModifyfilename.Replace(',', '_').Replace('&', '_').Replace('$', '_').Replace('#', '_').Replace('/', '_').Replace("'", "_").Replace('{', '_').Replace('}', '_').Replace('@', '_').Replace('%', '_').Replace('/', '_').Replace('?', '_');
            lblPInvFileName.Text = strModifyfilename;
            //File.Move(mobjResult.FileName, strTempFolderName + "\\" + mobjResult.FileName); //Alaa: This one here always throws an exception, delete this line if it's the right way
            mobjResult.SaveAs(strTempFolderName + "\\" + strName);

            UploadLogPdf("ADD", string.Empty);
        }

        string UsageHeader(string strSourceDesc)
        {
            string strDesc = "Usage";
            switch (strSourceDesc.ToUpper())
            {
                case "ELECTRIC":
                    strDesc = "KWH Usage";
                    break;
                case "NATURAL GAS":
                    strDesc = "CCF Usage";
                    break;
                case "FUEL OIL":
                case "KEROSENE":
                case "PROPANE":
                    strDesc = "Gallons Usage";
                    break;
                case "WOOD":
                    strDesc = "Cord Usage";
                    break;
            }
            return strDesc;
        }

        private void gvwBenefit_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwBenefit.Rows.Count > 0)
            {
                if (gvwBenefit.CurrentRow.Cells["gvStatus"].Value.ToString() == "Eligible")
                {
                    if (btnPost.Enabled == true) btnPost.Enabled = true;
                    if (btnOtherUsage.Enabled == true) btnPost.Enabled = true;
                }
                else
                {
                    btnPost.Enabled = false; btnOtherUsage.Enabled = false;
                }
            }
        }

        private void On_SaveForm_Closed()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "CEAP_Priority_Rating.pdf";



            PdfName = "CEAP_Priority_Rating";//form.GetFileName();
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
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);


            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            //iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
            iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\tickmark_green.png"));
            // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            _image_Tick.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            int intfromage = 0; int inttoage = 0;
            if (programEntity != null)
            {
                intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
                inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
            }
            double doublesertotal = 0;
            CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
            if (responsetot != null)
            {
                doublesertotal = Convert.ToDouble(responsetot.USAGE_TOTAL == string.Empty ? "0" : responsetot.USAGE_TOTAL);
            }

            double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].FamIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].FamIncome);
            double totaldive = (doublesertotal / doubleTotalAmount) * 100;
            totaldive = Math.Round(totaldive, 2);
            try
            {
                X_Pos = 30; Y_Pos = 760;

                X_Pos = 150; Y_Pos -= 90;

                int inttotalcount = 0;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 500;


                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseCaseMstListEntity[0].ApplNo, TblFontBold), X_Pos, Y_Pos, 0);

                List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
                List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
                int inteldercount = casesnpElder.Count * 4;
                inttotalcount = inttotalcount + inteldercount;

                List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
                int intyoungercount = casesnpyounger.Count * 4;
                inttotalcount = inttotalcount + intyoungercount;

                List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
                int intdisablecount = casesnpdisable.Count * 4;
                inttotalcount = inttotalcount + intdisablecount;

                int intNoneabove = 0;
                if (inttotalcount == 0)
                {
                    inttotalcount = inttotalcount + intNoneabove;
                    intNoneabove = 1;
                }
                int intfity = 0; int intsenvtyfive = 0; int inttwentyfive = 0; int inttwentytofifty = 0; int intfiftyone = 0;
                decimal intmstpoverty = Convert.ToDecimal(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

                if (intmstpoverty <= 50)
                {
                    inttotalcount = inttotalcount + 10;
                    intfity = 10;
                }
                else if (intmstpoverty >= 51 && intmstpoverty <= 75)
                {
                    inttotalcount = inttotalcount + 7;
                    intsenvtyfive = 7;
                }
                else if (intmstpoverty >= 76 && intmstpoverty <= 125)
                {
                    inttotalcount = inttotalcount + 3;
                    inttwentyfive = 3;
                }
                else if (intmstpoverty >= 126 && intmstpoverty <= 150)
                {
                    inttotalcount = inttotalcount + 1;
                    inttwentytofifty = 1;
                }
                else if (intmstpoverty <= 151)
                {

                    intfiftyone = 0;
                }

                int intExceedYes = 0; int intExceedNo = 0;
                if (doublesertotal > 1000)
                {
                    inttotalcount = inttotalcount + 4;
                    intExceedYes = 4;
                }
                else
                {
                    inttotalcount = inttotalcount + 1;
                    intExceedNo = 1;
                }


                int intthirty = 0; int inttwenty = 0; int inteleven = 0; int intsix = 0; int intfive = 0;
                if (doubleTotalAmount == 0 || doublesertotal == 0)
                {
                    intfive = 0;
                }
                else
                {

                    if (totaldive >= 30)
                    {
                        inttotalcount = inttotalcount + 12;
                        intthirty = 12;
                    }
                    else if (totaldive >= 20 && totaldive <= 29.99)
                    {
                        inttotalcount = inttotalcount + 9;
                        inttwenty = 9;
                    }
                    else if (totaldive >= 11 && totaldive <= 19.99)
                    {
                        inttotalcount = inttotalcount + 6;
                        inteleven = 6;
                    }
                    else if (totaldive >= 6 && totaldive <= 10.99)
                    {
                        inttotalcount = inttotalcount + 3;
                        intsix = 3;
                    }
                    else if (totaldive <= 5.99)
                    {
                        if (doubleTotalAmount == 0 || doublesertotal == 0)
                        {
                            intfive = 0;
                        }
                        else
                        {
                            inttotalcount = inttotalcount + 1;
                            intfive = 1;
                        }
                    }
                }

                X_Pos = 535;
                Y_Pos -= 45;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 42;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 65; Y_Pos -= 45;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 200;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                X_Pos = 400;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(totaldive.ToString().ToUpper() == "NAN" ? string.Empty : totaldive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                X_Pos = 535;
                Y_Pos -= 37;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwenty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteleven.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsix.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


                Y_Pos -= 40;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intExceedYes.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 17;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intExceedNo.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
                Y_Pos -= 30;
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

                if (inttotalcount >= 17)
                {
                    X_Pos = 40;
                    Y_Pos -= 30;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount >= 11 && inttotalcount <= 16)
                {
                    X_Pos = 40;
                    Y_Pos -= 66;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }
                else if (inttotalcount <= 10)
                {
                    X_Pos = 40;
                    Y_Pos -= 93;
                    _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
                    cb.AddImage(_image_Tick);
                }






            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }
        #endregion


        #region  ValidationData

        public bool AdminControlValidation(string strScreenCode)
        {
            bool boolvalidation = true;
            bool boolAllowClientIntake = true;
            bool boolAllowCustom = true;
            bool boolAllowIncomeVer = true;
            bool boolDisplayClientIntake = true;
            bool boolDisplayCustom = true;
            bool boolDisplayIncomeVer = true;
            string strclientIntakeRequired = string.Empty;
            string strVerRequired = string.Empty;
            string strclientDisMsg = string.Empty;
            string strCustomDisMsg = string.Empty;
            string strVerDisMsg = string.Empty;
            string strMsg = string.Empty;
            if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
            {
                List<ScaFldsHieEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSHIEDATA(strScreenCode, string.Empty, string.Empty);
                if (ScaFldsHiedata.Count > 0)
                {
                    List<ScaFldsHieEntity> ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
                    if (ScaFldsHie.Count == 0)
                    {
                        ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == BaseForm.BaseAgency + BaseForm.BaseDept + "**");
                        if (ScaFldsHie.Count == 0)
                        {
                            ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == BaseForm.BaseAgency + "****");
                        }
                        if (ScaFldsHie.Count == 0)
                        {
                            ScaFldsHie = ScaFldsHiedata.FindAll(u => u.ScrHie == "******");
                        }
                    }
                    if (ScaFldsHie.FindAll(u => u.Active != "Y").Count > 0)
                    {
                        if (ScaFldsHie.Count > 0)
                        {
                            int intvalidcount = 0;
                            ScaFldsHieEntity ScaFldscase2001data = ScaFldsHie.Find(u => u.ScahCode == "S0001");
                            ScaFldsHieEntity ScaFldscustomdata = ScaFldsHie.Find(u => u.ScahCode == "S0002");
                            ScaFldsHieEntity ScaFldscaseverdata = ScaFldsHie.Find(u => u.ScahCode == "S0003");
                            CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                            if (ScaFldscase2001data.Sel.ToUpper() == "Y")
                            {

                                boolAllowClientIntake = _model.CaseMstData.CheckRequiredCase2001ControlsData(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, BaseForm.BaseCaseSnpEntity, BaseForm.BaseCaseMstListEntity[0], out strclientIntakeRequired);
                                if (!boolAllowClientIntake)
                                {
                                    boolDisplayClientIntake = false;
                                    intvalidcount = +1;
                                    strMsg = ScaFldscase2001data.Msg;
                                    strclientDisMsg = ScaFldscase2001data.Msg;
                                    if (ScaFldscase2001data.Active.ToUpper() == "Y")
                                        boolAllowClientIntake = true;

                                }
                            }
                            if (ScaFldscustomdata.Sel.ToUpper() == "Y")
                            {
                                boolAllowCustom = _model.CaseMstData.CheckRequiredCustomQuestionsData(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, snpdata);
                                if (!boolAllowCustom)
                                {
                                    boolDisplayCustom = false;
                                    intvalidcount = intvalidcount + 1;
                                    strMsg = strMsg + "\n" + ScaFldscustomdata.Msg;
                                    strCustomDisMsg = ScaFldscustomdata.Msg;
                                    if (ScaFldscustomdata.Active.ToUpper() == "Y")
                                        boolAllowCustom = true;
                                }
                            }

                            if (ScaFldscaseverdata.Sel.ToUpper() == "Y")
                            {
                                List<CaseVerEntity> caseVerList = _model.CaseMstData.GetCASEVeradpyalst(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                                if (caseVerList.Count > 0)
                                {
                                    boolAllowIncomeVer = _model.CaseMstData.CheckRequiredCaseverdata(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, caseVerList[0], out strVerRequired);
                                    if (!boolAllowIncomeVer)
                                    {
                                        boolDisplayIncomeVer = false;
                                        intvalidcount = intvalidcount + 1;
                                        strMsg = strMsg + "\n" + ScaFldscaseverdata.Msg;
                                        strVerDisMsg = ScaFldscaseverdata.Msg;
                                        if (ScaFldscaseverdata.Active.ToUpper() == "Y")
                                            boolAllowIncomeVer = true;
                                    }
                                }
                                else
                                {

                                    boolAllowIncomeVer = false;
                                    boolDisplayIncomeVer = false;
                                    intvalidcount = intvalidcount + 1;
                                    strMsg = strMsg + "\nIncome Verification Data Does Not Exist";
                                    strVerRequired = "Income Verification Data Does Not Exist";
                                    strVerDisMsg = ScaFldscaseverdata.Msg;
                                    if (ScaFldscaseverdata.Active.ToUpper() == "Y")
                                        boolAllowIncomeVer = true;

                                }
                            }
                            string strIncompleteMsg = _model.CaseMstData.DisplayIncomeMsgs(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, snpdata, BaseForm.BaseCaseMstListEntity[0]);
                            strMsg = strMsg + "\n" + strIncompleteMsg;
                            if (boolAllowClientIntake == true && boolAllowCustom == true && boolAllowIncomeVer == true)
                                boolvalidation = true;
                            else
                                boolvalidation = false;
                            if (intvalidcount > 0)
                            {
                                ScaFldsHieEntity ScaFldscaseGerdata = ScaFldsHie.Find(u => u.ScahCode == "S0004");
                                if (boolvalidation == false)
                                {
                                    if (ScaFldscaseGerdata != null)
                                    {
                                        if (ScaFldscaseGerdata.Sel.ToUpper() == "Y")
                                            strMsg = ScaFldscaseGerdata.Msg;
                                    }
                                }
                                AdminControlMessageForm objForm = new AdminControlMessageForm(BaseForm, strMsg, strclientIntakeRequired, strVerRequired, boolDisplayClientIntake, boolDisplayCustom, boolDisplayIncomeVer, strclientDisMsg, strCustomDisMsg, strVerDisMsg, strIncompleteMsg, true, string.Empty);
                                objForm.StartPosition = FormStartPosition.CenterScreen;
                                objForm.ShowDialog();
                                // CommonFunctions.MessageBoxDisplay(strMsg);
                            }

                        }

                    }

                }

            }

            return boolvalidation;
        }

        #endregion

        private string UploadLogPdf(string strMode, string strLogId)
        {

            string stroutLogid = strLogId;
            bool boolImageUpload = false;
            //string strFamilySeq = string.Empty;
            var Extension = lblPInvFileName.Text.Substring(lblPInvFileName.Text.LastIndexOf('.') + 1).ToLower();
            strExtensionName = Extension;

            string strUploadFolderName = string.Empty;//Consts.Common.ServerLocation + BaseForm.BaseAgencyControlDetails.Path + @"\\Invoices\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;

            string strOnlyFileName = string.Empty;

            strUploadFolderName = strImageFolderName + "\\" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;
            strOnlyFileName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseApplicationNo;

            if (Extension == "pdf")
            {
                boolImageUpload = true; // Valid file type
            }
            if (boolImageUpload)
            {

                try
                {
                    // Determine whether the directory exists.

                    if (Directory.Exists(strUploadFolderName))
                    {
                        stroutLogid = createLogos(strUploadFolderName, strMode, strLogId);

                    }
                    else
                    {
                        // Try to create the directory.
                        DirectoryInfo di = Directory.CreateDirectory(strUploadFolderName);
                        stroutLogid = createLogos(strUploadFolderName, strMode, strLogId);

                    }

                }
                catch (Exception ex)
                {
                    // Console.WriteLine("The process failed: {0}", ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("You should upload the invoice only in PDF format");
            }


            return stroutLogid;
        }

        //private string strExtensionName = string.Empty;
        private string createLogos(string strFPath, string strMode, string strLogId)
        {
            string strInvoiceId = string.Empty;
            DirectoryInfo dir1 = new DirectoryInfo(strTempFolderName);
            try
            {
                if (!Directory.Exists(strTempFolderName))
                { DirectoryInfo di = Directory.CreateDirectory(strTempFolderName); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }
            DirectoryInfo dir2 = new DirectoryInfo(strFPath);

            FileInfo[] Folder1Files = dir1.GetFiles();
            FileInfo[] Folder2Files = dir2.GetFiles();
            string[] strorgsplitFile = lblPInvFileName.Text.Split('.');
            string orginialFileName = string.Empty;
            if (strorgsplitFile.Length > 0)
                orginialFileName = strorgsplitFile[0];
            if (lblPInvFileName.Text != string.Empty)
            {
                strorgsplitFile = lblPInvFileName.Text.Split('.');
                if (strorgsplitFile.Length > 0)
                    orginialFileName = strorgsplitFile[0];
            }
            if (Folder1Files.Length > 0)
            {
                foreach (FileInfo aFile in Folder1Files)
                {
                    string newFileName = orginialFileName + "." + strExtensionName;
                    string strpathToCheck = strFPath + "\\" + newFileName;

                    string tempfileName = "";
                    // Check to see if a file already exists with the
                    // same name as the file to upload.        
                    if (System.IO.File.Exists(strpathToCheck))
                    {
                        int counter = 1;
                        while (System.IO.File.Exists(strpathToCheck))
                        {
                            // if a file with this name already exists,
                            // prefix the filename with a number.
                            tempfileName = orginialFileName + counter.ToString() + "." + strExtensionName;
                            strpathToCheck = strFPath + "\\" + tempfileName;
                            counter++;
                        }

                        newFileName = tempfileName;
                    }

                    if (aFile.Name == lblPInvFileName.Text)
                    {
                        File.Move(strTempFolderName + "\\" + aFile.Name, strFPath + "\\" + newFileName);
                        strInvoiceId = InsertDeleteImgUploagLog(strLogId, strMode, lblPInvFileName.Text, newFileName);


                    }

                }
            }

            else
            {
                MessageBox.Show("Upload new image");

            }
            return strInvoiceId;

        }

        private string InsertDeleteImgUploagLog(string strIMGId, string strOpertype, string strimgFileName, string strimgloadas)
        {
            string strInvoiceId = strIMGId;
            IMGUPLOGNEntity imglogentity = new IMGUPLOGNEntity();
            imglogentity.IMGLOG_ID = strIMGId;
            imglogentity.IMGLOG_AGY = BaseForm.BaseAgency;
            imglogentity.IMGLOG_DEP = BaseForm.BaseDept;
            imglogentity.IMGLOG_PROG = BaseForm.BaseProg;
            imglogentity.IMGLOG_YEAR = BaseForm.BaseYear;
            imglogentity.IMGLOG_APP = BaseForm.BaseApplicationNo;
            //CaseSnpEntity casesnpimag = dataGridHierchys.SelectedRows[0].Tag as CaseSnpEntity;
            //if (chkHouseHold.Checked == true)
            //{
            //    imglogentity.IMGLOG_FAMILY_SEQ = casesnpimag.FamilySeq;
            //}
            //else
                imglogentity.IMGLOG_FAMILY_SEQ = "1";
            imglogentity.IMGLOG_SCREEN = "CASE0016";

            if (strOpertype == string.Empty)
            {
                imglogentity.IMGLOG_SECURITY = string.Empty;
                imglogentity.IMGLOG_TYPE = string.Empty;
            }
            else
            {
                imglogentity.IMGLOG_SECURITY = string.Empty;
                imglogentity.IMGLOG_TYPE = string.Empty;
            }
            imglogentity.IMGLOG_UPLoadAs = strimgloadas;
            imglogentity.IMGLOG_UPLOAD_BY = BaseForm.UserID;
            imglogentity.IMGLOG_ORIG_FILENAME = strimgFileName;
            imglogentity.MODE = strOpertype;
            _model.ChldMstData.InsertIMGUPLOG(imglogentity, out strInvoiceId);

            return strInvoiceId;
        }


        private string InsertDeleteINVUpdlog(string strINVId, string strOpertype, string strimgFileName, string strimgloadas)
        {
            string strInvoiceId = strINVId;
            InvoiceLogEntity inclogentity = new InvoiceLogEntity();
            inclogentity.INVLOG_ID = strINVId;
            inclogentity.INVLOG_AGENCY = BaseForm.BaseAgency;
            inclogentity.INVLOG_DEPT = BaseForm.BaseDept;
            inclogentity.INVLOG_PROGRAM = BaseForm.BaseProg;
            //  inclogentity.INVLOG_YEAR = BaseForm.BaseYear;
            inclogentity.INVLOG_APP = BaseForm.BaseApplicationNo;


            inclogentity.INVLOG_UPLOAD_AS = strimgloadas;
            inclogentity.INVLOG_UPLOAD_BY = BaseForm.UserID;
            inclogentity.INVLOG_DELETED_BY = BaseForm.UserID;
            inclogentity.INVLOG_LSTC_OPERATOR = BaseForm.UserID;
            inclogentity.INVLOG_ORIG_NAME = strimgFileName;
            inclogentity.Mode = strOpertype;
            _model.EMSBDCData.InsertUpdateDelINVLOG(inclogentity, out strInvoiceId);
            return strInvoiceId;
        }




    }
}