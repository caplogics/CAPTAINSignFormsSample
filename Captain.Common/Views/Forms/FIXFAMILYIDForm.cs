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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using System.IO;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FIXFAMILYIDForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public FIXFAMILYIDForm(BaseForm baseForm, PrivilegeEntity privileges, string stragency, string strdept, string strprog, string stryear, string strApp, string strMode)
        {

            InitializeComponent();
            this.Text = "HouseholdID Audit and Review";/*privileges.PrivilegeName;*/
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            pnlFamily.Visible = true;
            pnlMain.Visible = false;

            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Size = new Size(700, 200); // pnlFamily.Size;
           //** pnlFamily.Location = new Point(1, 1);
            BaseForm = baseForm;
            Agency = stragency;
            Dept = strdept;
            Prog = strprog;
            Program_Year = stryear;
            propAppNo = strApp;
            Mode = strMode;
            propRelation = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            pnlgvwMain.Size = new Size(930, 160);

            gvtBenLevel.Visible = false;
            if (Mode == "NEW")
            {
                txtApp.ReadOnly = false;
                Agency = BaseForm.BaseAgency;
                Dept = BaseForm.BaseDept;
                Prog = BaseForm.BaseProg;
                Program_Year = BaseForm.BaseYear;
                Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
                Pb_Search_Hie.Visible = true;

            }
            else
            {
                txtApp.Leave -= new EventHandler(txtApp_Leave);
                if (stryear.Trim() != string.Empty)
                    txtHieDesc.Text = stragency + " - " + strdept + " - " + strprog + " - " + stryear;
                else
                    txtHieDesc.Text = stragency + " - " + strdept + " - " + strprog;
                txtApp.Text = strApp;
                txtNewFamilyId.Focus();
                FillApplicantData();
            }
            // Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
        }
        string Mode { get; set; }

        public FIXFAMILYIDForm(BaseForm baseForm, PrivilegeEntity privileges, string strId)
        {

            InitializeComponent();
            this.Text = "HouseholdID Audit and Review"/*privileges.PrivilegeName*/;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            ToolTip exceltool = new ToolTip();
            exceltool.SetToolTip(PbExcel, "Generate Excel");
            pnlFamily.Visible = false;
            pnlMain.Visible = true;
            this.Size = pnlMain.Size;
            //this.Height = pnlMain.Height + 5;
            //this.Width = pnlMain.Width + 5;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            BaseForm = baseForm;
            if (BaseForm.BaseAgencyControlDetails.FamilyIdHie == "Y")
            {
                rdoSearchHierachy.Visible = true;
            }
            else
            {
                rdoSearchHierachy.Visible = false;
                rdoNameDobSearch.Checked = true;

            }
            rdoNameDobSearch_Click(rdoNameDobSearch, new EventArgs());
            Privileges = privileges;
            txtMaxim.Validator = TextBoxValidation.IntegerValidator;
            txtYear.Text = string.Empty;// DateTime.Now.Year.ToString();
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Prog = BaseForm.BaseProg;
            Program_Year = BaseForm.BaseYear;
            Program_Year2 = BaseForm.BaseYear;
            propRelation = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            Set_Report_Hierarchy1(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Program_Year { get; set; }
        public string Program_Year2 { get; set; }
        public string propAppNo { get; set; }
        List<CaseSnpEntity> propsnpdetails { get; set; }
        List<CommonEntity> propRelation { get; set; }
        public string propReportPath { get; set; }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "R");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

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
                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);

                    // propchldAttmsEntityList = _model.SPAdminData.GetChldAttMsDetails(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "ALL");
                }

            }
        }


        string Current_Hierarchy, Current_Hierarchy_DB;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            txtHieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            //if (Agy != "**")
            //{
            //    DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
            //    if (ds_AGY.Tables.Count > 0)
            //    {
            //        if (ds_AGY.Tables[0].Rows.Count > 0)
            //            txtHieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
            //    }
            //}
            //else
            //    txtHieDesc.Text += "AGY : ** - All Agencies      ";

            //if (Dept != "**")
            //{
            //    DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
            //    if (ds_DEPT.Tables.Count > 0)
            //    {
            //        if (ds_DEPT.Tables[0].Rows.Count > 0)
            //            txtHieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
            //    }
            //}
            //else
            //    txtHieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        txtHieDesc.Text += Current_Hierarchy_DB + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.txtHieDesc.Size = new System.Drawing.Size(185, 23);
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

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.txtHieDesc.Size = new System.Drawing.Size(185, 23);
            else
                this.txtHieDesc.Size = new System.Drawing.Size(264, 23);
        }



        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            }
        }
        CaseMstEntity propcasemstdata { get; set; }
        private void FillApplicantData()
        {
            propcasemstdata = null;
            if (!string.IsNullOrEmpty(txtApp.Text))
            {

                _errorProvider.SetError(txtNewFamilyId, null);
                _errorProvider.SetError(txtFamilyId, null);
                txtApp.Text = SetLeadingZeros(txtApp.Text);
                List<CaseMstEntity> casemst = _model.CaseMstData.GetCaseMstadpyn(Agency, Dept, Prog, Program_Year, txtApp.Text);
                List<CaseSnpEntity> casesnp = _model.CaseMstData.GetCaseSnpadpyn(Agency, Dept, Prog, Program_Year, txtApp.Text);
                if (casemst.Count > 0)
                {
                    CaseSnpEntity casesnpdata = casesnp.Find(u => u.FamilySeq.ToString() == casemst[0].FamilySeq.ToString());
                    if (casesnpdata != null)
                    {
                        propcasemstdata = casemst[0];
                        txtName.Text = LookupDataAccess.GetMemberName(casesnpdata.NameixFi, casesnpdata.NameixMi, casesnpdata.NameixLast, BaseForm.BaseHierarchyCnFormat);
                        txtSsnNum.Text = casesnpdata.Ssno;
                        txtFamilyId.Text = casemst[0].FamilyId;

                    }
                }
                else
                {
                    if (Mode != string.Empty)
                    {
                        txtNewFamilyId.Text = string.Empty;
                        txtName.Text = string.Empty;
                        txtSsnNum.Text = string.Empty;
                        txtFamilyId.Text = string.Empty;
                        CommonFunctions.MessageBoxDisplay("Applicant does not exist");
                    }
                }

            }

        }
        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtNewFamilyId, null);
            _errorProvider.SetError(txtFamilyId, null);
            if (txtNewFamilyId.Text != string.Empty)
            {
                if (propcasemstdata != null)
                {

                    MessageBox.Show("Are You Sure Want Update New Family Id", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxFamilyidHandler);
                }
                else
                {
                    _errorProvider.SetError(txtFamilyId, "Please Fill Applicant data");
                }
            }
            else
            {
                _errorProvider.SetError(txtNewFamilyId, "Please enter new familyid");
            }
        }
        private string strApplNo;
        private string strClientIdOut;
        private string strFamilyIdOut;
        private string strSSNNOOut;
        private string strErrorMsg;
        private void MessageBoxFamilyidHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    if (propcasemstdata != null)
                    {
                        string strFamilyId = txtNewFamilyId.Text;
                        strFamilyId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
                        // string strOldoperator = propcasemstdata.LstcOperator1;
                        // propcasemstdata.Mode = "FixFamilyId";
                        // propcasemstdata.FamilyId = strFamilyId;
                        // propcasemstdata.LstcOperator1 = BaseForm.UserID;
                        //if (_model.CaseMstData.InsertUpdateCaseMst(propcasemstdata, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg))
                        //{

                        Mode = "UPDLSTC";
                        if (_model.CaseMstData.UpdateSNPClientId(string.Empty, strFamilyId, propcasemstdata.ApplAgency + propcasemstdata.ApplDept + propcasemstdata.ApplProgram + (propcasemstdata.ApplYr == string.Empty ? "    " : propcasemstdata.ApplYr) + propcasemstdata.ApplNo + propcasemstdata.FamilySeq, string.Empty, string.Empty, "UPDFAMILYID", Mode, propcasemstdata.ApplAgency, propcasemstdata.ApplDept, propcasemstdata.ApplProgram, propcasemstdata.ApplYr, propcasemstdata.ApplNo, propcasemstdata.FamilySeq, BaseForm.UserID, propcasemstdata.FamilyId))
                        {


                            // _model.CaseMstData.InsertCasemstLog(propcasemstdata, "MST_FAMILY_ID", txtFamilyId.Text, strFamilyId, BaseForm.UserID);
                            ////CheckHistoryTableData(propcasemstdata, txtFamilyId.Text, strFamilyId, strOldoperator);
                            //if (_model.CaseMstData.INSERTUPDATEFIXSNPAUDIT(propcasemstdata.ApplAgency, propcasemstdata.ApplDept, propcasemstdata.ApplProgram, propcasemstdata.ApplYr, propcasemstdata.ApplNo, propcasemstdata.FamilySeq, "F", string.Empty, string.Empty, txtFamilyId.Text, strFamilyId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, "FAMILYSWITCH"))
                            //{

                            //}

                            //if (Mode != string.Empty)
                            //{
                            //    txtNewFamilyId.Text = string.Empty;
                            //    txtName.Text = string.Empty;
                            //    txtSsnNum.Text = string.Empty;
                            //    txtFamilyId.Text = string.Empty;
                            //}
                            //else
                            //{
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            // }


                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Error Process try again..");
                        }

                    }
                }
            //}
        }

        private void CheckHistoryTableData(CaseMstEntity Mstentitydata, string strFamilyId, string strNewFamilyId, string strOldOperator)
        {
            try
            {
                string strHistoryDetails = "<XmlHistory>";
                bool boolHistory = false;
                if (strFamilyId != strNewFamilyId)
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Family Id </FieldName><OldValue>" + strFamilyId + "</OldValue><NewValue>" + strNewFamilyId + "</NewValue></HistoryFields>";
                }
                if (strOldOperator.Trim().ToUpper() != Mstentitydata.LstcOperator1.Trim().ToUpper())
                {
                    boolHistory = true;
                    strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>LSTC Operator </FieldName><OldValue>" + strOldOperator + "</OldValue><NewValue>" + Mstentitydata.LstcOperator1 + "</NewValue></HistoryFields>";
                }

                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>LSTC DATE </FieldName><OldValue>" + Mstentitydata.DateLstc1 + "</OldValue><NewValue>" + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "</NewValue></HistoryFields>";

                strHistoryDetails = strHistoryDetails + "</XmlHistory>";
                if (boolHistory)
                {
                    CaseHistEntity caseHistEntity = new CaseHistEntity();
                    caseHistEntity.HistTblName = "CASEMST";
                    caseHistEntity.HistScreen = "CASE2001";
                    caseHistEntity.HistSubScr = "FIXFAMILYID";
                    caseHistEntity.HistTblKey = Mstentitydata.ApplAgency + Mstentitydata.ApplDept + Mstentitydata.ApplProgram + Mstentitydata.ApplYr + Mstentitydata.ApplNo + Mstentitydata.FamilySeq;
                    caseHistEntity.LstcOperator = BaseForm.UserID;
                    caseHistEntity.HistChanges = strHistoryDetails;
                    _model.CaseMstData.InsertCaseHist(caseHistEntity);
                }
            }
            catch (Exception ex)
            {


            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string strFillLoad = string.Empty;
        private void btnSearchDiffIds_Click(object sender, EventArgs e)
        {

            //if (txtMaxim.Text != string.Empty)
            //{
            lblSerachName.Text = btnSearchDiffIds.Text;
            strFillLoad = string.Empty;
            _errorProvider.SetError(txtMaxim, null);
            gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
            gvwMain.Rows.Clear();
            gvwSub.Rows.Clear();
            lblCount.Text = "0";
            lblCount2.Text = "0";

            string strYear = string.Empty, strFname = string.Empty, strLName = string.Empty, strdob = string.Empty, strFromDt = string.Empty, strToDt = string.Empty, strlAgency = string.Empty, strlDept = string.Empty, strlProgram = string.Empty, strlPrYear = string.Empty;
            string strRelationDesc = string.Empty;


            if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
            if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
            if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();

            //if (rdoMstDateRange.Checked)
            //{
            //    if (dtFrom.Checked)
            //    {
            //        strFromDt = dtFrom.Value.ToShortDateString();
            //        strToDt = dtTo.Value.ToShortDateString();
            //    }

            //}

            //if (rdoSearchYear.Checked)
            //{
            //    if (txtYear.Text != string.Empty)
            //    {
            //        strYear = txtYear.Text;
            //    }
            //}
            if (rdoSearchHierachy.Checked)
            {
                strlAgency = Agency;
                strlDept = Dept;
                strlProgram = Prog;
                strlPrYear = Program_Year2;
            }

            //if (rdoSearchYear.Checked)
            //    propsnpdetails = _model.CaseMstData.GetSnpFixFamilyId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "FAMILYID", "FIXFAMILYID", strFromDt, strToDt);
            //else
            propsnpdetails = _model.CaseMstData.GetSnpFixclinetIdHie(strYear, string.Empty, string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "FAMILYIDHIE", "FIXFAMILYID", strlAgency, strlDept, strlProgram, strlPrYear, strFromDt, strToDt);
            foreach (CaseSnpEntity item in propsnpdetails)
            {
                strRelationDesc = string.Empty;
                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                if (commonrelationdesc != null)
                    strRelationDesc = commonrelationdesc.Desc;
                int index = gvwMain.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                if (item.Status.Trim() != "A")
                    gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                gvwMain.Rows[index].Tag = item;
                CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
            }
            lblCount.Text = gvwMain.Rows.Count.ToString();
            lblCount2.Text = gvwMain.Rows.Count.ToString();
            if (gvwMain.Rows.Count > 0)
            {
                gvwMain.Rows[0].Selected = true;
                //btnFamilyId.Visible = true;
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("No Records found");
                // btnFamilyId.Visible = false;
            }
            gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
            gvwMain_SelectionChanged(sender, e);
        }

        private void gvwMain_SelectionChanged(object sender, EventArgs e)
        {
            if (rdoSearchHierachy.Checked)
            {
                if (gvwMain.SelectedRows.Count > 0)
                {

                    if (gvwMain.SelectedRows[0].Selected)
                    {
                        string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid, strRelationDesc;
                        if ((gvwMain.SelectedRows[0].Cells["gvtKey"].Value == null ? string.Empty : gvwMain.SelectedRows[0].Cells["gvtKey"].Value.ToString()) != string.Empty)

                        {
                            CaseSnpEntity casesnpdata = (gvwMain.SelectedRows[0].Tag as CaseSnpEntity);
                            if (casesnpdata != null)
                            {
                                gvwSub.Rows.Clear();
                                strkey = gvwMain.SelectedRows[0].Cells["gvtMainkey"].Value == null ? string.Empty : gvwMain.SelectedRows[0].Cells["gvtMainkey"].Value.ToString();
                                //strkey = casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                                List<CaseSnpEntity> snpsubdetails = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, casesnpdata.ClientId, string.Empty, string.Empty, string.Empty, strkey, "MSTDATA", "FIXFAMILYIDDATE", casesnpdata.Agency, casesnpdata.Dept, casesnpdata.Program, casesnpdata.Year, string.Empty, string.Empty);
                                int index; string strApplicantkey = string.Empty;
                                foreach (CaseSnpEntity subitem in snpsubdetails)
                                {
                                    if (strkey != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App + subitem.FamilySeq)
                                    {
                                        List<CaseSnpEntity> snpsub2details = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, string.Empty, subitem.NameixFi, subitem.NameixLast, subitem.AltBdate, string.Empty, "MSTFAMILYIDATA", "FIXFAMILYIDBENLEVEL", Agency, Dept, Prog, string.Empty, string.Empty, string.Empty);

                                        List<CaseSnpEntity> snpsubdetailstoporder = snpsub2details.FindAll(u => u.Agency + u.Dept + u.Program + u.Year + u.App == subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App);

                                        foreach (CaseSnpEntity item in snpsubdetailstoporder)
                                        {

                                            strRelationDesc = string.Empty;
                                            CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                                            if (commonrelationdesc != null)
                                                strRelationDesc = commonrelationdesc.Desc;


                                            if (strApplicantkey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                                            {

                                                index = gvwSub.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, item.IncomeBasis);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                                                strApplicantkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                                            }
                                            else
                                            {
                                                index = gvwSub.Rows.Add(string.Empty, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, string.Empty);
                                            }
                                            if (item.Status.Trim() != "A")
                                                gvwSub.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                            gvwSub.Rows[index].Tag = item;
                                            CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwSub);

                                        }
                                        List<CaseSnpEntity> snpsubdetailsbotorder = snpsub2details.FindAll(u => u.Agency + u.Dept + u.Program + u.Year + u.App != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App);

                                        foreach (CaseSnpEntity item in snpsubdetailsbotorder)
                                        {

                                            strRelationDesc = string.Empty;
                                            CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                                            if (commonrelationdesc != null)
                                                strRelationDesc = commonrelationdesc.Desc;


                                            if (strApplicantkey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                                            {

                                                index = gvwSub.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, item.IncomeBasis);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                                                strApplicantkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                                            }
                                            else
                                            {
                                                index = gvwSub.Rows.Add(string.Empty, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, string.Empty);
                                            }
                                            if (item.Status.Trim() != "A")
                                                gvwSub.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                            gvwSub.Rows[index].Tag = item;
                                            CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwSub);

                                        }

                                    }
                                }
                            }
                            //ssnno = string.Empty;
                            //strclientid = string.Empty;
                            //ssnno = casesnpdata.Ssno.Substring(3, 2);
                            //strkey = casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                            //Firstname = string.Empty; lastname = string.Empty; dateofbirth = string.Empty; ;
                            ////if (ssnno == "00")
                            ////{
                            ////    ssnno = string.Empty;
                            ////    Firstname = casesnpdata.NameixFi;
                            ////    lastname = casesnpdata.NameixLast;
                            ////    dateofbirth = casesnpdata.AltBdate;
                            ////}
                            ////else
                            ////{
                            //ssnno = casesnpdata.Ssno;
                            ////}
                            //if (strFillLoad != string.Empty)
                            //{
                            //    strclientid = casesnpdata.ClientId;
                            //    ssnno = string.Empty;
                            //    Firstname = string.Empty;
                            //    lastname = string.Empty;
                            //    dateofbirth = string.Empty;
                            //    // strkey = "*********";
                            //}
                            //List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixFamilyId(string.Empty, string.Empty, ssnno, strclientid, Firstname, lastname, dateofbirth, strkey, "MSTDATA", "FIXFAMILYIDDATE", string.Empty, string.Empty);

                            //var snpvardata = snpdetails.GroupBy(u => new { u.Agency, u.Dept, u.Program, u.App, u.ClientId, u.Ssno }).ToList();

                            //gvwSub.Rows.Clear();
                            //int mainclienid, subitemclientid;
                            //CaseSnpEntity casesnpitemdata = new CaseSnpEntity();
                            ////foreach (var item in snpvardata)
                            //{
                            //    mainclienid = Convert.ToInt32(casesnpdata.ClientId);
                            //    subitemclientid = Convert.ToInt32(item.Key.ClientId);

                            //    if (!(casesnpdata.Agency == item.Key.Agency && casesnpdata.Dept == item.Key.Dept && casesnpdata.Program == item.Key.Program && casesnpdata.App == item.Key.App && mainclienid == subitemclientid && casesnpdata.Ssno == item.Key.Ssno))
                            //    {
                            //        var itemList = (from t in snpdetails
                            //                        where t.Agency == item.Key.Agency && t.Dept == item.Key.Dept && t.Program == item.Key.Program && t.App == item.Key.App && t.ClientId == item.Key.ClientId && t.Ssno == item.Key.Ssno
                            //                        select t).OrderByDescending(c => c.Year).FirstOrDefault();



                            //        //casesnpitemdata = snpdetails.Find(u => u.Agency == item.Key.Agency && u.Dept == item.Key.Dept && u.Program == item.Key.Program && u.App == item.Key.App && u.ClientId == item.Key.ClientId && u.Ssno == item.Key.Ssno);
                            //        if (chkHierarchy.Checked && rdoSearchHierachy.Checked)
                            //        {
                            //            if (Agency == item.Key.Agency && Dept == item.Key.Dept && Prog == item.Key.Program && Program_Year2 == itemList.Year)
                            //            {
                            //                strRelationDesc = string.Empty;
                            //                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == itemList.MemberCode.ToString().Trim());
                            //                if (commonrelationdesc != null)
                            //                    strRelationDesc = commonrelationdesc.Desc;
                            //                int index = gvwSub.Rows.Add(item.Key.Agency + item.Key.Dept + item.Key.Program + " " + itemList.Year + " " + item.Key.App, item.Key.Ssno, item.Key.ClientId, LookupDataAccess.GetMemberName(itemList.NameixFi, itemList.NameixMi, itemList.NameixLast, "3"), LookupDataAccess.Getdate(itemList.AltBdate), strRelationDesc);// LookupDataAccess.Getdate(item.AltBdate),
                            //                gvwSub.Rows[index].Tag = item;
                            //                CommonFunctions.setTooltip(index, itemList.AddOperator, itemList.DateAdd, itemList.LstcOperator, itemList.DateLstc, gvwSub);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            strRelationDesc = string.Empty;
                            //            CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == itemList.MemberCode.ToString().Trim());
                            //            if (commonrelationdesc != null)
                            //                strRelationDesc = commonrelationdesc.Desc;
                            //            int index = gvwSub.Rows.Add(item.Key.Agency + item.Key.Dept + item.Key.Program + " " + itemList.Year + " " + item.Key.App, item.Key.Ssno, item.Key.ClientId, LookupDataAccess.GetMemberName(itemList.NameixFi, itemList.NameixMi, itemList.NameixLast, "3"), LookupDataAccess.Getdate(itemList.AltBdate), strRelationDesc);// LookupDataAccess.Getdate(item.AltBdate),
                            //            gvwSub.Rows[index].Tag = item;
                            //            CommonFunctions.setTooltip(index, itemList.AddOperator, itemList.DateAdd, itemList.LstcOperator, itemList.DateLstc, gvwSub);
                            //        }
                            //    }
                            //}
                        }
                        if (gvwSub.Rows.Count > 0)
                            gvwSub.Rows[0].Selected = true;

                    }

                }
            }
        }


        private void btnFamilyId_Click(object sender, EventArgs e)
        {
            if (rdoSearchHierachy.Checked)
            {
                if (gvwSub.Rows.Count > 0)
                {
                    if (gvwMain.SelectedRows[0].Selected)
                    {
                        MessageBox.Show("Are You Sure Want Update Family ID bottom grid Records", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandlerSelIDs);
                    }
                }

            }
            else
            {
                if (gvwMain.Rows.Count > 0)
                {
                    CaseSnpEntity casesnpdata = (gvwMain.SelectedRows[0].Tag as CaseSnpEntity);
                    if (casesnpdata != null)
                    {
                        // casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                        //if(gvwMain.SelectedRows[0].Cells["gvtFamilyId"].Value.ToString()!=string.Empty)
                        //{
                        FIXFAMILYIDForm fixclienform = new FIXFAMILYIDForm(BaseForm, Privileges, casesnpdata.Agency, casesnpdata.Dept, casesnpdata.Program, casesnpdata.Year, casesnpdata.App, string.Empty);
                        fixclienform.FormClosed += new FormClosedEventHandler(Fixclienform_FormClosed);
                        fixclienform.StartPosition = FormStartPosition.CenterScreen;
                        fixclienform.ShowDialog();
                        // }
                    }
                }
                //else
                //{
                //    FIXFAMILYIDForm fixclienform = new FIXFAMILYIDForm(BaseForm, Privileges, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "NEW");
                //    fixclienform.ShowDialog();

                //}
            }
        }

        private void Fixclienform_FormClosed(object sender, FormClosedEventArgs e)
        {
            FIXFAMILYIDForm form = sender as FIXFAMILYIDForm;
            if (form.DialogResult == DialogResult.OK)
            {
                btnNameSearch_Click(btnNameSearch, new EventArgs());
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
                    string strFamilyId = string.Empty;
                    _model.CaseMstData.GETCodegen("F", out strFamilyId);

                    strFamilyId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
                    bool boolupdate = false;
                    foreach (DataGridViewRow gvrowitem in gvwSub.Rows)
                    {
                        string strSubKey = gvrowitem.Cells["gvtSubKey"].Value.ToString();
                        if (strSubKey != string.Empty)
                        {
                            string Mode = string.Empty;
                            CaseSnpEntity casesnpdetails = gvrowitem.Tag as CaseSnpEntity;
                            if (casesnpdetails.Year.Trim() == Program_Year2.Trim())
                                Mode = "UPDLSTC";
                            if (_model.CaseMstData.UpdateSNPClientId(string.Empty, strFamilyId, casesnpdetails.Agency + casesnpdetails.Dept + casesnpdetails.Program + (casesnpdetails.Year.Trim() == string.Empty ? "    " : casesnpdetails.Year) + casesnpdetails.App + casesnpdetails.FamilySeq, string.Empty, string.Empty, "UPDFAMILYID", Mode, casesnpdetails.Agency, casesnpdetails.Dept, casesnpdetails.Program, casesnpdetails.Year, casesnpdetails.App, casesnpdetails.FamilySeq, BaseForm.UserID, casesnpdetails.ClientId))
                            {
                                boolupdate = true;
                            }
                        }

                    }
                    if (boolupdate)
                    {
                        btnNameSearch_Click(btnNameSearch, EventArgs.Empty);
                        //CaseSnpEntity casesnpdetailsMain = gvwMain.SelectedRows[0].Tag as CaseSnpEntity;
                        //if (casesnpdetailsMain != null)
                        //    propsnpdetails.Remove(casesnpdetailsMain);
                        //gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                        //gvwMain.Rows.Clear();
                        //gvwSub.Rows.Clear();
                        //int intcount = 0;
                        //string strRelationDesc = string.Empty;
                        //string strKey = string.Empty, strMainkey = string.Empty;
                        //int index;
                        //foreach (CaseSnpEntity item in propsnpdetails)
                        //{
                        //    strRelationDesc = string.Empty;
                        //    CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                        //    if (commonrelationdesc != null)
                        //        strRelationDesc = commonrelationdesc.Desc;


                        //    if (strKey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                        //    {
                        //        intcount = intcount + 1;
                        //        strMainkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App + item.FamilySeq;
                        //        index = gvwMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, strMainkey, item.IncomeBasis);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                        //        strKey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                        //        gvwMain.Rows[index].Cells["gvtchkSelect"].ReadOnly = false;

                        //    }
                        //    else
                        //    {
                        //        index = gvwMain.Rows.Add(false, string.Empty, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, strMainkey, string.Empty);
                        //        gvwMain.Rows[index].Cells["gvtchkSelect"].ReadOnly = true;
                        //    }
                        //    if (item.Status.Trim() != "A")
                        //        gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                        //    gvwMain.Rows[index].Tag = item;
                        //    CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
                        //}
                        //lblCount.Text = intcount.ToString();
                        //if (gvwMain.Rows.Count > 0)
                        //{
                        //    gvwMain.Rows[0].Selected = true;
                        //    btnFamilyId.Visible = true;
                        //    pnlExcel.Visible = true;
                        //    btn_Masssmash.Visible = true;
                        //    // btnFamilyId.Visible = true;
                        //}
                        //else
                        //{
                        //    CommonFunctions.MessageBoxDisplay("No Records found");
                        //    btnFamilyId.Visible = false;
                        //    pnlExcel.Visible = false;
                        //    btn_Masssmash.Visible = false;
                        //    // btnFamilyId.Visible = false;
                        //}
                        //gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                        //gvwMain_SelectionChanged(sender, e);
                    }

                }
            //}
        }


        private void btnExcel_Click(object sender, EventArgs e)
        {
            //FullClassExcelReport01();
            if (gvwMain.Rows.Count > 0)
            {
                if (strDOBNAMEbuttonMode != string.Empty)
                    On_SaveExcelDuplicateForm_Closed(string.Empty);
                else
                    On_SaveExcelForm_Closed();
            }
        }

        private void FullClassExcelReport01()
        {


            try
            {
                string Random_Filename = string.Empty;
                string propReportPath = _model.lookupDataAccess.GetReportPath();
                string strExcelName = propReportPath + BaseForm.UserID + "\\FIXCLIENTID";
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
                    string Tmpstr = strExcelName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = strExcelName + newFileName.Substring(0, length) + ".xls";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    strExcelName = Random_Filename;
                else
                    strExcelName += ".xls";


                ExcelDocument xlWorkSheet = new ExcelDocument();

                xlWorkSheet.ColumnWidth(0, 0);
                xlWorkSheet.ColumnWidth(1, 200);
                xlWorkSheet.ColumnWidth(2, 100);
                xlWorkSheet.ColumnWidth(3, 100);
                xlWorkSheet.ColumnWidth(4, 100);
                xlWorkSheet.ColumnWidth(5, 100);
                xlWorkSheet.ColumnWidth(6, 80);
                xlWorkSheet.ColumnWidth(7, 100);
                xlWorkSheet.ColumnWidth(8, 100);

                int excelcolumn = 0;

                if (gvwMain.Rows.Count > 0)
                {
                    excelcolumn = excelcolumn + 1;
                    ExcelHeaderFamilyIdData(excelcolumn, xlWorkSheet);

                    foreach (DataGridViewRow gvwmainrow in gvwMain.Rows)
                    {
                        string ssnno, Firstname, lastname, dateofbirth, strkey, strclientid;
                        CaseSnpEntity casesnpdata = (gvwMain.SelectedRows[0].Tag as CaseSnpEntity);

                        if (casesnpdata != null)
                        {
                            excelcolumn = excelcolumn + 1;
                            ExcelData(excelcolumn, xlWorkSheet, casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + " " + casesnpdata.Year + " " + casesnpdata.App + " " + casesnpdata.FamilySeq, casesnpdata.Ssno, casesnpdata.ClientId, casesnpdata.NameixFi, casesnpdata.NameixLast, LookupDataAccess.Getdate(casesnpdata.AltBdate), string.Empty, string.Empty);

                            ssnno = string.Empty;
                            strclientid = string.Empty;
                            ssnno = casesnpdata.Ssno;
                            strkey = casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                            Firstname = string.Empty; lastname = string.Empty; dateofbirth = string.Empty; ;

                            List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixFamilyId(string.Empty, string.Empty, LookupDataAccess.GetPhoneSsnNoFormat(ssnno), strclientid, Firstname, lastname, dateofbirth, strkey, "MSTDATA", "FIXFAMILYIDDATE", string.Empty, string.Empty,string.Empty);

                            gvwSub.Rows.Clear();
                            foreach (CaseSnpEntity item in snpdetails)
                            {
                                excelcolumn = excelcolumn + 1;
                                ExcelData(excelcolumn, xlWorkSheet, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App + " " + item.FamilySeq, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, item.NameixFi, item.NameixLast, string.Empty, item.AddOperator, LookupDataAccess.Getdate(item.DateAdd));

                            }

                        }
                    }
                }

                FileStream stream = new FileStream(strExcelName, FileMode.Create);

                xlWorkSheet.Save(stream);
                stream.Close();

            }
            catch (Exception ex) { }

        }


        private void ExcelHeaderFamilyIdData(int intcolumn, ExcelDocument xlWorkSheet)
        {


            xlWorkSheet[intcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 1].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 1, "Key");

            xlWorkSheet[intcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 2].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 2, "SSN #");

            xlWorkSheet[intcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 3].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 3, "Family Id");

            xlWorkSheet[intcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 4].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 4, "First Name");

            xlWorkSheet[intcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 5].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 5, "Last Name");

            xlWorkSheet[intcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 6].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 6, "Dob");

            xlWorkSheet[intcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 7].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 7, "Add Operator");

            xlWorkSheet[intcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 8].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 8, "Add Date");



        }

        private void ExcelData(int intcolumn, ExcelDocument xlWorkSheet, string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8)
        {

            xlWorkSheet.WriteCell(intcolumn, 1, str1);


            xlWorkSheet.WriteCell(intcolumn, 2, str2);


            xlWorkSheet.WriteCell(intcolumn, 3, str3);


            xlWorkSheet.WriteCell(intcolumn, 4, str4);


            xlWorkSheet.WriteCell(intcolumn, 5, str5);


            xlWorkSheet.WriteCell(intcolumn, 6, str6);


            xlWorkSheet.WriteCell(intcolumn, 7, str7);


            xlWorkSheet.WriteCell(intcolumn, 8, str8);
        }

        private void btnFamilyDiffSSN_Click(object sender, EventArgs e)
        {
            strFillLoad = "FamilyId";
            lblSerachName.Text = btnFamilyDiffSSN.Text;
            _errorProvider.SetError(txtMaxim, null);
            gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
            gvwMain.Rows.Clear();
            gvwSub.Rows.Clear();
            lblCount.Text = "0";
            lblCount2.Text = "0";
            string strYear = string.Empty, strFname = string.Empty, strLName = string.Empty, strdob = string.Empty, strFromDt = string.Empty, strToDt = string.Empty, strlAgency = string.Empty, strlDept = string.Empty, strlProgram = string.Empty, strlPrYear = string.Empty, strRelationDesc = string.Empty;


            if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
            if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
            if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();

            //if (rdoMstDateRange.Checked)
            //{
            //    if (dtFrom.Checked)
            //    {
            //        strFromDt = dtFrom.Value.ToShortDateString();
            //        strToDt = dtTo.Value.ToShortDateString();
            //    }

            //}
            //if (rdoSearchYear.Checked)
            //{
            //    if (txtYear.Text != string.Empty)
            //    {
            //        strYear = txtYear.Text;
            //    }
            //}
            if (rdoSearchHierachy.Checked)
            {
                strlAgency = Agency;
                strlDept = Dept;
                strlProgram = Prog;
                strlPrYear = Program_Year2;
            }
            //if (rdoSearchYear.Checked)
            //    propsnpdetails = _model.CaseMstData.GetSnpFixFamilyId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "FAMIDSSN", "FIXFAMILYID", strFromDt, strToDt);
            //else
            propsnpdetails = _model.CaseMstData.GetSnpFixclinetIdHie(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "FAMIDSSNHIE", "FIXFAMILYID", strlAgency, strlDept, strlProgram, strlPrYear, strFromDt, strToDt);

            foreach (CaseSnpEntity item in propsnpdetails)
            {
                strRelationDesc = string.Empty;
                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                if (commonrelationdesc != null)
                    strRelationDesc = commonrelationdesc.Desc;
                int index = gvwMain.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                if (item.Status.Trim() != "A")
                    gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                gvwMain.Rows[index].Tag = item;
                CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
            }
            lblCount.Text = gvwMain.Rows.Count.ToString();
            lblCount2.Text = gvwMain.Rows.Count.ToString();
            if (gvwMain.Rows.Count > 0)
            {
                gvwMain.Rows[0].Selected = true;
                // btnFamilyId.Visible = true;
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("No Records found");
                //btnFamilyId.Visible = false;
            }
            gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
            gvwMain_SelectionChanged(sender, e);
        }

        private void pb_Search_Hie2_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB1, "Master", "", "A", "R");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed1);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        private void OnHierarchieFormClosed1(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

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

                    Set_Report_Hierarchy1(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);

                    gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                    gvwMain.Rows.Clear();
                    gvwSub.Rows.Clear();
                    lblSerachName.Text = string.Empty;
                    lblCount.Text = "0";
                    lblCount2.Text = "0";
                    gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);


                    // propchldAttmsEntityList = _model.SPAdminData.GetChldAttMsDetails(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "ALL");
                }

            }
        }


        private void btnHierchywiseSSN_Click(object sender, EventArgs e)
        {
            strFillLoad = string.Empty;
            _errorProvider.SetError(txtMaxim, null);
            gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
            gvwMain.Rows.Clear();
            gvwSub.Rows.Clear();
            lblCount.Text = "0";
            lblCount2.Text = "0";
            string strYear = string.Empty;
            if (txtYear.Text != string.Empty)
            {
                strYear = txtYear.Text;
            }
            List<CaseSnpEntity> snpdetails = _model.CaseMstData.GetSnpFixFamilyId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "FAMILYID", "FIXFAMILYID", string.Empty, string.Empty, string.Empty);
            foreach (CaseSnpEntity item in snpdetails)
            {
                string strRelationDesc = string.Empty;
                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                if (commonrelationdesc != null)
                    strRelationDesc = commonrelationdesc.Desc;
                int index = gvwMain.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                if (item.Status.Trim() != "A")
                    gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                gvwMain.Rows[index].Tag = item;
                CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
            }
            lblCount.Text = gvwMain.Rows.Count.ToString();
            lblCount2.Text = gvwMain.Rows.Count.ToString();
            if (gvwMain.Rows.Count > 0)
            {
                gvwMain.Rows[0].Selected = true;
                // btnFamilyId.Visible = true;
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("No Records found");
                // btnFamilyId.Visible = false;
            }
            gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
            gvwMain_SelectionChanged(sender, e);
        }

        private void txtApp_Leave(object sender, EventArgs e)
        {
            FillApplicantData();
        }


        string Current_Hierarchy1, Current_Hierarchy_DB1;
        private void Set_Report_Hierarchy1(string Agy, string Dept, string Prog, string Year)
        {
            txtHie_Desc2.Clear();
            cmbYear2.Visible = false;
            Program_Year2 = "    ";
            Current_Hierarchy1 = Agy + Dept + Prog;
            Current_Hierarchy_DB1 = Agy + "-" + Dept + "-" + Prog;

            //if (Agy != "**")
            //{
            //    DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
            //    if (ds_AGY.Tables.Count > 0)
            //    {
            //        if (ds_AGY.Tables[0].Rows.Count > 0)
            //            txtHieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
            //    }
            //}
            //else
            //    txtHieDesc.Text += "AGY : ** - All Agencies      ";

            //if (Dept != "**")
            //{
            //    DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
            //    if (ds_DEPT.Tables.Count > 0)
            //    {
            //        if (ds_DEPT.Tables[0].Rows.Count > 0)
            //            txtHieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
            //    }
            //}
            //else
            //    txtHieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        txtHie_Desc2.Text += Current_Hierarchy_DB1 + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo1(Agy, Dept, Prog, Year);
            //else
            //    this.txtHie_Desc2.Size = new System.Drawing.Size(185, 23);
        }

        string DepYear1;
        bool DefHieExist1 = false;

        private void rdoSearchHierachy_Click(object sender, EventArgs e)
        {
            //if (rdoSearchHierachy.Checked)
            //{
            //    pnlYear.Visible = false;
            //    pnlHierarchy.Visible = true;

            //}
            //else
            //{
            //    pnlHierarchy.Visible = false;
            //    pnlYear.Visible = true;
            //}
        }

        private void chkHierarchy_CheckedChanged(object sender, EventArgs e)
        {
            // gvwMain_SelectionChanged(sender, e);
        }

        private void gvwMain_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (gvwMain.SelectedRows.Count > 0)
            {
                CaseSnpEntity casesnpdeatils = gvwMain.SelectedRows[0].Tag as CaseSnpEntity;
                if (casesnpdeatils != null)
                {
                    FIXIDSSWITCHFORM formswitch = new FIXIDSSWITCHFORM("F", casesnpdeatils, BaseForm, Privileges, "FAMILYSWITCH");
                    formswitch.FormClosed += new FormClosedEventHandler(Formswitch_FormClosed);
                    formswitch.StartPosition = FormStartPosition.CenterScreen;
                    //formswitch.FormClosing += Formswitch_FormClosed;
                    formswitch.ShowDialog();
                }

            }
        }

        private void Formswitch_FormClosed(object sender, FormClosedEventArgs e)
        {
            FIXIDSSWITCHFORM form = sender as FIXIDSSWITCHFORM;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);

                CaseSnpEntity casesnpdeatils = gvwMain.SelectedRows[0].Tag as CaseSnpEntity;
                propsnpdetails.Remove(casesnpdeatils);

                gvwMain.Rows.Clear();
                lblCount.Text = "0";
                lblCount2.Text = "0";
                foreach (CaseSnpEntity item in propsnpdetails)
                {
                    string strRelationDesc = string.Empty;
                    CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                    if (commonrelationdesc != null)
                        strRelationDesc = commonrelationdesc.Desc;
                    int index = gvwMain.Rows.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                    if (item.Status.Trim() != "A")
                        gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    gvwMain.Rows[index].Tag = item;
                    CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
                }
                lblCount.Text = gvwMain.Rows.Count.ToString();
                lblCount2.Text = gvwMain.Rows.Count.ToString();
                if (gvwMain.Rows.Count > 0)
                {
                    gvwMain.Rows[0].Selected = true;
                    // btnFamilyId.Visible = true;
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("No Records found");
                    // btnFamilyId.Visible = false;
                }
                gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                gvwMain_SelectionChanged(sender, e);

            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {

            if (gvwMain.Rows.Count > 0)
            {
                if (gvwMain.Rows[0].Tag is CaseSnpEntity)
                {
                    contextMenu1.MenuItems.Clear();
                    MenuItem menuLst = new MenuItem();
                    menuLst.Text = "Fill Reason";
                    contextMenu1.MenuItems.Add(menuLst);

                }
            }

        }

        private void btnGenerateFamilyId_Click(object sender, EventArgs e)
        {
            string strFamilyId = string.Empty;
            _model.CaseMstData.GETCodegen("F", out strFamilyId);

            txtNewFamilyId.Text = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
        }

        private void rdoNameDobSearch_Click(object sender, EventArgs e)
        {
            // chkHierarchy.Visible = false;
            chkb50rec.Visible = false;
            txtYear.Visible = false;
            btnDelete.Visible = false;
            //   btnExcel.Enabled = false;
            btnFamilyId.Visible = false;
            btn_Masssmash.Visible = false;
            btnDuplicateFamilyId.Visible = false;
            pnlExcel.Visible = false;
            pnlYear.Visible = false;
            txtMaxim.Text = string.Empty;
            if (rdoSearchHierachy.Checked)
            {
                pnlMassSmash.Visible = true;
                //lblTotal.Location = new Point(667, 268);
                //lblCount.Location = new Point(755, 268);
                this.Size = new Size(930, 557);// Vikash
                pnlMain.Size = new Size(930, 500);
                pnlSearch.Size = new Size(930, 109);
                pnlgvwMain.Size = new Size(930, 160);
               // pnlSmash.Visible = true;
                pnlHierarchy.Visible = true;
                pnlNameSearch.Visible = false;
                gvtBenLevel.Visible = true;
                gvtchkSelect.Visible = true;
                gvtchkSelect.ShowInVisibilityMenu = true;
                gvtBenLevel.ShowInVisibilityMenu = true;
                pnlYear.Visible = true;
                txtMaxim.Text = "100";
                btnNameSearch.Visible = true;
                btnSearchDiffIds.Visible = false;
                btnFamilyDiffSSN.Visible = false;
                txtFName.Text = string.Empty;
                txtLName.Text = string.Empty;
                gvwMain.Rows.Clear();
                gvwSub.Rows.Clear();
                pnlgvwSub.Visible = true;
                gvwSub.Visible = true;
                //**pnlgvwSub.Size = new Size(930,160);
                lblCount.Text = "0";
                lblCount2.Text = "0";
                if (BaseForm.UserID == "JAKE")
                {
                    btnDuplicateFamilyId.Visible = true;
                    this.Size = new Size(930, 600);
                    //pnlSmash.Visible = true;//Vikash
                    pnlMain.Size = new Size(930, 543);
                }
                //rdoMstDateRange.Visible = true;
                //rdoSearchYear.Visible = true;
                //rdoMstDateRange_Click(sender, e);
            }
            else if (rdoNameDobSearch.Checked)
            {
                pnlMassSmash.Visible = false;
                // btnExcel.Visible = false;
                this.Size = new Size(930, 590);// Vikash
                pnlMain.Size = new Size(930, 533/*570*/);
                gvtBenLevel.Visible = false;
                pnlHierarchy.Visible = false;
                pnlNameSearch.Visible = true;
                gvtchkSelect.Visible = false;
                pnlYear.Visible = false;
                txtMaxim.Text = string.Empty;
                pnlSearch.Size = new Size(930, 109);
                //lblTotal.Location = new Point(555, 473);
                //lblCount.Location = new Point(643, 473);

                //lblTotal.Location = new Point(655, 5);
                //lblCount.Location = new Point(725, 5);

                // pnlMassSmash.Visible = true;
                btnNameSearch.Visible = true;
                pnlSmash.Visible = true;
                btnSearchDiffIds.Visible = false;
                btnFamilyDiffSSN.Visible = false;
                _errorProvider.SetError(txtFName, null);
                _errorProvider.SetError(txtLName, null);
                txtFName.Text = string.Empty;
                txtLName.Text = string.Empty;
                txtYear.Visible = false;
                lblFrom.Visible = false;
                lblTo.Visible = false;
                dtFrom.Visible = false;
                dtTo.Visible = false;
                txtYear.Text = string.Empty;
                dtFrom.Checked = false;
                dtTo.Checked = false;
                rdoMstDateRange.Visible = false;
                rdoSearchYear.Visible = false;
                gvwMain.Rows.Clear();
                gvwSub.Rows.Clear();
                lblCount.Text = "0";
                lblCount2.Text = "0";
                gvtchkSelect.ShowInVisibilityMenu = false;
                gvtBenLevel.ShowInVisibilityMenu = false;
                pnlgvwMain.Size = new Size(930, 533/*570*/);
                pnlgvwSub.Visible = gvwSub.Visible = false;
            }
        }

        private void rdoMstDateRange_Click(object sender, EventArgs e)
        {
            if (rdoMstDateRange.Checked)
            {
                txtYear.Visible = false;
                lblFrom.Visible = true;
                lblTo.Visible = true;
                dtFrom.Visible = true;
                dtTo.Visible = true;
            }
            else if (rdoSearchYear.Checked)
            {
                txtYear.Visible = true;
                lblFrom.Visible = false;
                lblTo.Visible = false;
                dtFrom.Visible = false;
                dtTo.Visible = false;
            }
        }

        private void btnNameSearch_Click(object sender, EventArgs e)
        {
            int intcount = 0;
            strDOBNAMEbuttonMode = string.Empty;
            btnFamilyId.Visible = false;
            this.gvtRelation.HeaderText = "Relation";
            // btnDuplicateFamilyId.Visible = false;
            btn_Masssmash.Visible = false;
            pnlExcel.Visible = false;
            chkExcelAllrows.Visible = true;
            chkb50rec.Visible = false;
            btnDelete.Visible = false;
            if (rdoNameDobSearch.Checked)
            {

                if (ValidateForm())
                {

                    strFillLoad = string.Empty;
                    _errorProvider.SetError(txtMaxim, null);
                    gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);

                    lblCount.Visible = true;
                    lblTotal.Visible = true;
                    lblCount.Text = "0";

                    lblCount2.Visible = true;
                    lblTotal2.Visible = true;
                    lblCount2.Text = "0";


                    gvwMain.Rows.Clear();
                    gvwSub.Rows.Clear();
                    string strYear = string.Empty, strFname = string.Empty, strLName = string.Empty, strdob = string.Empty, strFromDt = string.Empty, strToDt = string.Empty, strlAgency = string.Empty, strlDept = string.Empty, strlProgram = string.Empty, strlPrYear = string.Empty;
                    string strRelationDesc = string.Empty;


                    if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                    if (dtpFrmDate.Checked) strdob = dtpFrmDate.Value.ToShortDateString();

                    if (rdoMstDateRange.Checked)
                    {
                        if (dtFrom.Checked)
                        {
                            strFromDt = dtFrom.Value.ToShortDateString();
                            strToDt = dtTo.Value.ToShortDateString();
                        }

                    }

                    //if (rdoSearchYear.Checked)
                    //    propsnpdetails = _model.CaseMstData.GetSnpFixFamilyId(strYear, txtMaxim.Text.ToString(), string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "FAMILYID", "FIXFAMILYID", strFromDt, strToDt);
                    //else
                    string strKey = string.Empty;
                    int index = 0;
                    propsnpdetails = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, string.Empty, strFname, strLName, strdob, string.Empty, "NAMESEARCH", "FIXFAMILYID", string.Empty, string.Empty, string.Empty, string.Empty, strFromDt, strToDt);
                    foreach (CaseSnpEntity item in propsnpdetails)
                    {
                        strRelationDesc = string.Empty;
                        CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                        if (commonrelationdesc != null)
                            strRelationDesc = commonrelationdesc.Desc;


                        if (strKey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                        {
                            intcount = intcount + 1;
                            index = gvwMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                            strKey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                        }
                        else
                        {
                            index = gvwMain.Rows.Add(false, string.Empty, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc);
                        }
                        if (item.Status.Trim() != "A")
                            gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                        if (strFname.ToUpper().Trim() == item.NameixFi.ToUpper().Trim() && strLName.ToUpper().Trim() == item.NameixLast.ToUpper().Trim())
                        {
                            gvwMain.Rows[index].Cells["gvtLastName"].Style.BackColor = Color.Yellow;
                        }
                        gvwMain.Rows[index].Tag = item;
                        CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
                    }
                    lblCount.Text = intcount.ToString();
                    lblCount2.Text = intcount.ToString();
                    if (gvwMain.Rows.Count > 0)
                    {
                        gvwMain.Rows[0].Selected = true;

                        btnFamilyId.Visible = true;
                        pnlExcel.Visible = true;
                        //btnFamilyId.Visible = true;
                    }
                    else
                    {
                        CommonFunctions.MessageBoxDisplay("No Records found");
                        // btnFamilyId.Visible = false;
                    }
                    //    gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                    //  gvwMain_SelectionChanged(sender, e);
                }
            }
            else
            {

                strFillLoad = string.Empty;
                _errorProvider.SetError(txtMaxim, null);
                gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
                gvwMain.Rows.Clear();
                gvwSub.Rows.Clear();

               //** gvwMain.Size = new Size(930, 100);
                gvwSub.Visible = true;
                lblCount.Visible = true;
                lblCount2.Visible = true;
                lblTotal.Visible = true;
                lblTotal2.Visible = true;

                lblCount.Text = "0";
                lblCount2.Text = "0";
                string strYear = string.Empty, strFname = string.Empty, strLName = string.Empty, strdob = string.Empty, strFromDt = string.Empty, strToDt = string.Empty, strlAgency = string.Empty, strlDept = string.Empty, strlProgram = string.Empty, strlPrYear = string.Empty;
                string strRelationDesc = string.Empty;
                string strDupLvl = string.Empty;

                if (chkHierarchy.Checked)
                {
                    strlAgency = Agency;
                    strlDept = Dept;
                    strlProgram = Prog;
                    strlPrYear = Program_Year2;
                }

                string strKey = string.Empty;
                string strMainkey = string.Empty;
                int index = 0;

                if (BaseForm.BaseAgencyControlDetails.FamilyIdDuplvl == "Y")
                    strDupLvl = "Y";
                propsnpdetails = _model.CaseMstData.GetSnpFixclinetIdHie(strDupLvl, txtMaxim.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "SEARCHFAMILYID", "FIXFAMILYIDBENLEVEL", strlAgency, strlDept, strlProgram, strlPrYear, string.Empty, string.Empty);
                foreach (CaseSnpEntity item in propsnpdetails)
                {
                    strRelationDesc = string.Empty;
                    CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                    if (commonrelationdesc != null)
                        strRelationDesc = commonrelationdesc.Desc;


                    if (strKey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                    {
                        intcount = intcount + 1;
                        strMainkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App + item.FamilySeq;
                        index = gvwMain.Rows.Add(false, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, strMainkey, item.IncomeBasis);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno
                        strKey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                        gvwMain.Rows[index].Cells["gvtchkSelect"].ReadOnly = false;

                    }
                    else
                    {
                        index = gvwMain.Rows.Add(false, string.Empty, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), string.Empty, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), strRelationDesc, strMainkey, string.Empty);
                        gvwMain.Rows[index].Cells["gvtchkSelect"].ReadOnly = true;
                    }
                    if (item.Status.Trim() != "A")
                        gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    gvwMain.Rows[index].Tag = item;
                    CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
                }
                lblCount.Text = intcount.ToString();
                lblCount2.Text = intcount.ToString();
                if (gvwMain.Rows.Count > 0)
                {
                    pnlSmash.Visible = true;
                    pnlMassSmash.Visible = true;

                    gvwMain.Rows[0].Selected = true;
                    chkb50rec.Visible = true;
                    btnFamilyId.Visible = true;
                    pnlExcel.Visible = true;
                    btn_Masssmash.Visible = true;
                    // btnDuplicateFamilyId.Visible = true;
                    // btnFamilyId.Visible = true;
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("No Records found");
                    // btnFamilyId.Visible = false;
                }
                gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);
                gvwMain_SelectionChanged(sender, e);
            }
        }
        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtFName.Text.Trim()))
            {
                _errorProvider.SetError(txtFName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFName, null);
            }
            if (String.IsNullOrEmpty(txtLName.Text.Trim()))
            {
                _errorProvider.SetError(txtLName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLName, null);
            }


            return (isValid);
        }


        private void FillYearCombo1(string Agy, string Dept, string Prog, string Year)
        {
            cmbYear2.Visible = DefHieExist1 = false;
            Program_Year2 = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist1 = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year2 = DepYear1 = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear1.Trim())) && DepYear1 != null && DepYear1 != "    ")
                    {
                        int TmpYear = int.Parse(DepYear1);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist1)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        cmbYear2.Items.AddRange(listItem.ToArray());

                        cmbYear2.Visible = true;

                        if (DefHieExist)
                            cmbYear2.SelectedIndex = YearIndex;
                        else
                            cmbYear2.SelectedIndex = 0;
                    }
                }
            }

            //if (!string.IsNullOrEmpty(Program_Year2.Trim()))
            //    this.txtHie_Desc2.Size = new System.Drawing.Size(185, 23);
            //else
            //    this.txtHie_Desc2.Size = new System.Drawing.Size(264, 23);
        }



        private void CmbYear2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbYear2.Items.Count > 0)
            {
                Program_Year2 = "    ";
                if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbYear2.SelectedItem).Text.ToString())))
                {
                    Program_Year2 = ((Captain.Common.Utilities.ListItem)cmbYear2.SelectedItem).Text.ToString();
                    gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);

                    //if (btnFamilyDiffSSN.Text == lblSerachName.Text)
                    //    btnFamilyDiffSSN_Click(sender, e);
                    //else if (btnSearchDiffIds.Text == lblSerachName.Text)
                    //    btnSearchDiffIds_Click(sender, e);
                    //else
                    //{
                    btnDelete.Visible = false;
                    gvwMain.Rows.Clear();
                    gvwSub.Rows.Clear();
                    pnlExcel.Visible = false;
                    btnFamilyId.Visible = false;
                    btn_Masssmash.Visible = false;
                    lblSerachName.Text = string.Empty;
                    lblCount.Text = "0";
                    lblCount2.Text = "0";
                    // }
                    gvwMain.SelectionChanged += new EventHandler(gvwMain_SelectionChanged);

                }
            }
        }

        #region Excelreport


        string Random_Filename = null; string PdfName = null;

        private void btn_Masssmash_Click(object sender, EventArgs e)
        {
            if (gvwMain.SelectedRows.Count > 0)
            {

                List<DataGridViewRow> SelectedgvRows = (from c in gvwMain.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvtchkSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();

                if (SelectedgvRows.Count > 1)
                {

                    if (gvwMain.SelectedRows[0].Selected)
                    {
                        MessageBox.Show("Are You Sure to Mass Update all selected rows in top grid", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandlerMultiple);
                    }
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("You must select 2 or more rows in top grid");
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
                    List<DataGridViewRow> SelectedgvRows = (from c in gvwMain.Rows.Cast<DataGridViewRow>().ToList()
                                                            where (((DataGridViewCheckBoxCell)c.Cells["gvtchkSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                            select c).ToList();

                    string strkey = string.Empty;
                    bool boolupdate = false;
                    foreach (DataGridViewRow grrowitem in SelectedgvRows)
                    {
                        if (grrowitem.Cells["gvtchkSelect"].Value.ToString().ToUpper() == "TRUE")
                        {


                            CaseSnpEntity casesnpdata = (grrowitem.Tag as CaseSnpEntity);
                            if (casesnpdata != null)
                            {

                                strkey = grrowitem.Cells["gvtMainkey"].Value == null ? string.Empty : grrowitem.Cells["gvtMainkey"].Value.ToString();
                                //strkey = casesnpdata.Agency + casesnpdata.Dept + casesnpdata.Program + casesnpdata.Year + casesnpdata.App + casesnpdata.FamilySeq;
                                List<CaseSnpEntity> snpsubdetails = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, casesnpdata.ClientId, string.Empty, string.Empty, string.Empty, strkey, "MSTDATA", "FIXFAMILYIDDATE", casesnpdata.Agency, casesnpdata.Dept, casesnpdata.Program, casesnpdata.Year, string.Empty, string.Empty);
                                string strApplicantkey = string.Empty;
                                string strFamilyId = string.Empty;
                                _model.CaseMstData.GETCodegen("F", out strFamilyId);

                                strFamilyId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;

                                foreach (CaseSnpEntity subitem in snpsubdetails)
                                {

                                    if (strkey != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App + subitem.FamilySeq)
                                    {


                                        List<CaseSnpEntity> snpsub2details = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, string.Empty, subitem.NameixFi, subitem.NameixLast, subitem.AltBdate, string.Empty, "MSTFAMILYIDATA", "FIXFAMILYIDBENLEVEL", Agency, Dept, Prog, string.Empty, string.Empty, string.Empty);

                                        foreach (CaseSnpEntity item in snpsub2details)
                                        {

                                            if (strApplicantkey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                                            {

                                                string Mode = string.Empty;

                                                if (item.Year.Trim() == Program_Year2.Trim())
                                                    Mode = "UPDLSTC";
                                                if (_model.CaseMstData.UpdateSNPClientId(string.Empty, strFamilyId, item.Agency + item.Dept + item.Program + (item.Year == string.Empty ? "    " : item.Year) + item.App + item.FamilySeq, string.Empty, string.Empty, "UPDFAMILYID", Mode, item.Agency, item.Dept, item.Program, item.Year, item.App, item.FamilySeq, BaseForm.UserID, item.ClientId))
                                                {
                                                    boolupdate = true;
                                                }
                                                strApplicantkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                                            }

                                        }

                                    }
                                }

                            }
                        }
                    }
                    if (boolupdate)
                        btnNameSearch_Click(btnNameSearch, EventArgs.Empty);


                }
            //}
        }

        private void chkb50rec_CheckedChanged(object sender, EventArgs e)
        {
            if (gvwMain.Rows.Count > 0)
            {
                if (chkb50rec.Checked)
                {
                    int Count = 0;
                    foreach (DataGridViewRow dr in gvwMain.Rows)
                    {
                        if (dr.Cells["gvtKey"].Value != string.Empty)
                        {
                            dr.Cells["gvtchkSelect"].Value = true;
                            Count++;
                        }
                        if (Count == 50)
                            break;
                    }
                }
                else
                {
                    foreach (DataGridViewRow dr in gvwMain.Rows)
                    {
                        dr.Cells["gvtchkSelect"].Value = false;

                    }
                }
            }
        }



        private void pnlMain_Click(object sender, EventArgs e)
        {

        }



        private void On_SaveExcelForm_Closed()
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            if (chkExcelAllrows.Checked)
                PdfName = "FIXFAMID_Audit_AllRows_" + BaseForm.UserID.Trim();
            else
                PdfName = "FIXFAMID_Audit_SelRow_" + BaseForm.UserID.Trim();

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

            string strrowadded = string.Empty;
            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);


            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;
            string ReportName = "Sheet1";
            if (gvwMain.Rows.Count > 0)
            {
                if (rdoNameDobSearch.Checked)
                {
                    sheet = book.Worksheets.Add(ReportName);
                    sheet.Table.DefaultRowHeight = 14.25F;

                    sheet.Table.Columns.Add(150);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(250);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(120);

                    Row0 = sheet.Table.Rows.Add();

                    cell = Row0.Cells.Add("SNP KEY", DataType.String, "s94");
                    cell = Row0.Cells.Add("SSN", DataType.String, "s94");
                    cell = Row0.Cells.Add("Family ID", DataType.String, "s94");
                    cell = Row0.Cells.Add("Name", DataType.String, "s94");
                    cell = Row0.Cells.Add("Date of Birth", DataType.String, "s94");
                    cell = Row0.Cells.Add("Last Changed", DataType.String, "s94");
                    cell = Row0.Cells.Add("Relation", DataType.String, "s94");
                    Row0 = sheet.Table.Rows.Add();


                    string strRelationDesc = string.Empty;
                    string strKey = string.Empty, strFname = string.Empty, strLName = string.Empty;

                    if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                    foreach (CaseSnpEntity item in propsnpdetails)
                    {
                        strRelationDesc = string.Empty;
                        CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                        if (commonrelationdesc != null)
                            strRelationDesc = commonrelationdesc.Desc;

                        Row0 = sheet.Table.Rows.Add();
                        if (strKey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                        {
                            cell = Row0.Cells.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), DataType.String, "s96");
                            cell = Row0.Cells.Add(item.ClientId, DataType.String, "s96");
                            if (rdoNameDobSearch.Checked)
                            {
                                if (strFname.ToUpper().Trim() == item.NameixFi.ToUpper().Trim() && strLName.ToUpper().Trim() == item.NameixLast.ToUpper().Trim())
                                {
                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96Yellow");
                                }
                                else
                                {
                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96");
                                }
                            }
                            else
                            {
                                cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96");
                            }
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.AltBdate), DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.DateLstc), DataType.String, "s96");
                            cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                            strKey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                        }
                        else
                        {
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), DataType.String, "s96");
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                            if (rdoNameDobSearch.Checked)
                            {
                                if (strFname.ToUpper().Trim() == item.NameixFi.ToUpper().Trim() && strLName.ToUpper().Trim() == item.NameixLast.ToUpper().Trim())
                                {
                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96Yellow");
                                }
                                else
                                {
                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96");
                                }
                            }
                            else
                            {
                                cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96");
                            }
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.AltBdate), DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.DateLstc), DataType.String, "s96");
                            cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");

                        }

                    }
                }
                else
                {
                    List<CaseSnpEntity> casesnpexceldetails = propsnpdetails;
                    List<DataGridViewRow> SelectedgvRows = (from c in gvwMain.Rows.Cast<DataGridViewRow>().ToList()
                                                            where (((DataGridViewCheckBoxCell)c.Cells["gvtchkSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                            select c).ToList();
                    if (chkExcelAllrows.Checked == false)
                    {


                        if (SelectedgvRows.Count <= 1)
                        {
                            CaseSnpEntity casesnpselected = gvwMain.SelectedRows[0].Tag as CaseSnpEntity;
                            casesnpexceldetails = propsnpdetails.FindAll(u => u.Agency + u.Dept + u.Program + u.Year.Trim() + u.App == casesnpselected.Agency + casesnpselected.Dept + casesnpselected.Program + casesnpselected.Year.Trim() + casesnpselected.App);


                        }
                        else
                        {
                            casesnpexceldetails = new List<CaseSnpEntity>();
                            foreach (DataGridViewRow gvtoprowitem in SelectedgvRows)
                            {
                                CaseSnpEntity casesnpselected = gvtoprowitem.Tag as CaseSnpEntity;

                                List<CaseSnpEntity> snpentitydata = propsnpdetails.FindAll(u => u.Agency + u.Dept + u.Program + u.Year.Trim() + u.App == casesnpselected.Agency + casesnpselected.Dept + casesnpselected.Program + casesnpselected.Year.Trim() + casesnpselected.App);
                                foreach (CaseSnpEntity snpitem in snpentitydata)
                                {
                                    casesnpexceldetails.Add(snpitem);
                                }
                            }
                        }
                    }
                    sheet = book.Worksheets.Add(ReportName);
                    sheet.Table.DefaultRowHeight = 14.25F;

                    sheet.Table.Columns.Add(150);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(250);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(120);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);

                    Row0 = sheet.Table.Rows.Add();

                    cell = Row0.Cells.Add("SNP KEY", DataType.String, "s94");
                    cell = Row0.Cells.Add("SSN", DataType.String, "s94");
                    cell = Row0.Cells.Add("Family ID", DataType.String, "s94");
                    cell = Row0.Cells.Add("Name", DataType.String, "s94");
                    cell = Row0.Cells.Add("Date of Birth", DataType.String, "s94");
                    cell = Row0.Cells.Add("Last Changed", DataType.String, "s94");
                    cell = Row0.Cells.Add("Relation", DataType.String, "s94");
                    cell = Row0.Cells.Add("Ben.Level", DataType.String, "s94");
                    cell = Row0.Cells.Add("CA Count", DataType.String, "s94");
                    cell = Row0.Cells.Add("MS Count", DataType.String, "s94");
                    cell = Row0.Cells.Add("Cont Count", DataType.String, "s94");
                    // Row0 = sheet.Table.Rows.Add();


                    string strRelationDesc = string.Empty;
                    string strKey = string.Empty, strFname = string.Empty, strLName = string.Empty;
                    string strMainkey = string.Empty, strFamilyId = string.Empty;

                    if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();
                    foreach (CaseSnpEntity item in casesnpexceldetails)
                    {
                        //Row0 = sheet.Table.Rows.Add();
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                        //cell = Row0.Cells.Add("", DataType.String, "s96");

                        strRelationDesc = string.Empty;
                        CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                        if (commonrelationdesc != null)
                            strRelationDesc = commonrelationdesc.Desc;

                        //Row0 = sheet.Table.Rows.Add();
                        if (strKey != item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App)
                        {
                            if (chkExcelAllrows.Checked == true || SelectedgvRows.Count > 1)
                            {
                                if (strKey != string.Empty)
                                {
                                    // Row0 = sheet.Table.Rows.Add();
                                    //Row0 = sheet.Table.Rows.Add();

                                    List<CaseSnpEntity> snpsubdetails1 = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, strFamilyId, string.Empty, string.Empty, string.Empty, strMainkey, "MSTDATA", "FIXFAMILYIDDATE", item.Agency, item.Dept, item.Program, item.Year, string.Empty, string.Empty);
                                    string strApplicantkey1 = string.Empty;
                                    foreach (CaseSnpEntity subitem in snpsubdetails1)
                                    {
                                        if (strMainkey != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App + subitem.FamilySeq)
                                        {

                                            List<CaseSnpEntity> snpsub2details = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, string.Empty, subitem.NameixFi, subitem.NameixLast, subitem.AltBdate, string.Empty, "MSTFAMILYIDATA", "FIXFAMILYIDBENLEVEL", Agency, Dept, Prog, string.Empty, string.Empty, string.Empty);

                                            List<CaseSnpEntity> snpsubdetailstoporder = snpsub2details.FindAll(u => u.Agency + u.Dept + u.Program + u.Year + u.App == subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App);

                                            foreach (CaseSnpEntity sub2item in snpsubdetailstoporder)
                                            {
                                                Row0 = sheet.Table.Rows.Add();
                                                strRelationDesc = string.Empty;
                                                commonrelationdesc = propRelation.Find(u => u.Code == sub2item.MemberCode.ToString().Trim());
                                                if (commonrelationdesc != null)
                                                    strRelationDesc = commonrelationdesc.Desc;


                                                if (strApplicantkey1 != sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App)
                                                {
                                                    cell = Row0.Cells.Add(sub2item.Agency + sub2item.Dept + sub2item.Program + " " + sub2item.Year + " " + sub2item.App, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.ClientId, DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.IncomeBasis, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.Cacount, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.Mscount, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.Contcount, DataType.String, "s96");

                                                    strApplicantkey1 = sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App;
                                                }
                                                else
                                                {
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                                }

                                            }
                                            List<CaseSnpEntity> snpsubdetailsbotorder = snpsub2details.FindAll(u => u.Agency + u.Dept + u.Program + u.Year + u.App != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App);

                                            foreach (CaseSnpEntity sub2item in snpsubdetailsbotorder)
                                            {
                                                Row0 = sheet.Table.Rows.Add();
                                                strRelationDesc = string.Empty;
                                                commonrelationdesc = propRelation.Find(u => u.Code == sub2item.MemberCode.ToString().Trim());
                                                if (commonrelationdesc != null)
                                                    strRelationDesc = commonrelationdesc.Desc;


                                                if (strApplicantkey1 != sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App)
                                                {
                                                    cell = Row0.Cells.Add(sub2item.Agency + sub2item.Dept + sub2item.Program + " " + sub2item.Year + " " + sub2item.App, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.ClientId, DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.IncomeBasis, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.Cacount, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.Mscount, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(sub2item.Contcount, DataType.String, "s96");

                                                    strApplicantkey1 = sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App;
                                                }
                                                else
                                                {
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                                }

                                            }

                                        }
                                    }



                                }
                            }

                            if (strrowadded != string.Empty)
                            {
                                Row0 = sheet.Table.Rows.Add();
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");
                                cell = Row0.Cells.Add("", DataType.String, "s96");

                            }
                            strrowadded = "row";
                            strRelationDesc = string.Empty;
                            commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                            if (commonrelationdesc != null)
                                strRelationDesc = commonrelationdesc.Desc;

                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add(item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), DataType.String, "s96");
                            cell = Row0.Cells.Add(item.ClientId, DataType.String, "s96");

                            cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96");

                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.AltBdate), DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.DateLstc), DataType.String, "s96");
                            cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                            cell = Row0.Cells.Add(item.IncomeBasis, DataType.String, "s96");
                            cell = Row0.Cells.Add(item.Cacount, DataType.String, "s96");
                            cell = Row0.Cells.Add(item.Mscount, DataType.String, "s96");
                            cell = Row0.Cells.Add(item.Contcount, DataType.String, "s96");
                            strKey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;
                            strMainkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App + item.FamilySeq;
                            strFamilyId = item.ClientId;
                        }
                        else
                        {

                            strRelationDesc = string.Empty;
                            commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                            if (commonrelationdesc != null)
                                strRelationDesc = commonrelationdesc.Desc;

                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), DataType.String, "s96");
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                            cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), DataType.String, "s96");

                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.AltBdate), DataType.String, "s96");
                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(item.DateLstc), DataType.String, "s96");
                            cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                            cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                        }

                    }
                    //if (chkExcelAllrows.Checked == false)
                    //{
                    //    foreach (DataGridViewRow gvitem in gvwSub.Rows)
                    //    {
                    //        Row0 = sheet.Table.Rows.Add();
                    //        cell = Row0.Cells.Add(gvitem.Cells[0].Value.ToString(), DataType.String, "s96");
                    //        cell = Row0.Cells.Add(gvitem.Cells[1].Value.ToString(), DataType.String, "s96");
                    //        cell = Row0.Cells.Add(gvitem.Cells[2].Value.ToString(), DataType.String, "s96");

                    //        cell = Row0.Cells.Add(gvitem.Cells[3].Value.ToString(), DataType.String, "s96");

                    //        cell = Row0.Cells.Add(gvitem.Cells[4].Value.ToString(), DataType.String, "s96");
                    //        cell = Row0.Cells.Add(gvitem.Cells[5].Value.ToString(), DataType.String, "s96");
                    //        cell = Row0.Cells.Add(gvitem.Cells[6].Value.ToString(), DataType.String, "s96");
                    //        cell = Row0.Cells.Add(gvitem.Cells[7].Value.ToString(), DataType.String, "s96");

                    //    }
                    //}
                    //else
                    //{
                    List<CaseSnpEntity> snpsubdetails = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, strFamilyId, string.Empty, string.Empty, string.Empty, strMainkey, "MSTDATA", "FIXFAMILYIDDATE", Agency, Dept, Prog, Program_Year2, string.Empty, string.Empty);
                    string strApplicantkey = string.Empty;
                    foreach (CaseSnpEntity subitem in snpsubdetails)
                    {
                        if (strMainkey != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App + subitem.FamilySeq)
                        {
                            List<CaseSnpEntity> snpsub2details = _model.CaseMstData.GetSnpFixclinetIdHie(string.Empty, string.Empty, string.Empty, string.Empty, subitem.NameixFi, subitem.NameixLast, subitem.AltBdate, string.Empty, "MSTFAMILYIDATA", "FIXFAMILYIDBENLEVEL", Agency, Dept, Prog, string.Empty, string.Empty, string.Empty);

                            List<CaseSnpEntity> snpsubdetailstoporder = snpsub2details.FindAll(u => u.Agency + u.Dept + u.Program + u.Year + u.App == subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App);

                            foreach (CaseSnpEntity sub2item in snpsubdetailstoporder)
                            {
                                Row0 = sheet.Table.Rows.Add();
                                strRelationDesc = string.Empty;
                                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == sub2item.MemberCode.ToString().Trim());
                                if (commonrelationdesc != null)
                                    strRelationDesc = commonrelationdesc.Desc;


                                if (strApplicantkey != sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App)
                                {
                                    cell = Row0.Cells.Add(sub2item.Agency + sub2item.Dept + sub2item.Program + " " + sub2item.Year + " " + sub2item.App, DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.ClientId, DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.IncomeBasis, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.Cacount, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.Mscount, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.Contcount, DataType.String, "s96");
                                    strApplicantkey = sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App;
                                }
                                else
                                {
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                }

                            }
                            List<CaseSnpEntity> snpsubdetailsbotorder = snpsub2details.FindAll(u => u.Agency + u.Dept + u.Program + u.Year + u.App != subitem.Agency + subitem.Dept + subitem.Program + subitem.Year + subitem.App);

                            foreach (CaseSnpEntity sub2item in snpsubdetailsbotorder)
                            {
                                Row0 = sheet.Table.Rows.Add();
                                strRelationDesc = string.Empty;
                                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == sub2item.MemberCode.ToString().Trim());
                                if (commonrelationdesc != null)
                                    strRelationDesc = commonrelationdesc.Desc;


                                if (strApplicantkey != sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App)
                                {
                                    cell = Row0.Cells.Add(sub2item.Agency + sub2item.Dept + sub2item.Program + " " + sub2item.Year + " " + sub2item.App, DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.ClientId, DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.IncomeBasis, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.Cacount, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.Mscount, DataType.String, "s96");
                                    cell = Row0.Cells.Add(sub2item.Contcount, DataType.String, "s96");

                                    strApplicantkey = sub2item.Agency + sub2item.Dept + sub2item.Program + sub2item.Year.Trim() + sub2item.App;
                                }
                                else
                                {
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                                    cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                                    cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");
                                    cell = Row0.Cells.Add(string.Empty, DataType.String, "s96");

                                }

                            }

                        }
                    }

                    //}
                }
            }

            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();

            //if (chkExcelAllrows.Checked)
            //    downloadGateway.Filename = "FIXFAMID_Audit_AllRows_" + BaseForm.UserID.Trim() + ".xls";
            //else
            //    downloadGateway.Filename = "FIXFAMID_Audit_SelRow_" + BaseForm.UserID.Trim() + ".xls";


            //// downloadGateway.Filename = PdfName;

            //// downloadGateway.Version = file.Version;

            //downloadGateway.SetContentType(DownloadContentType.OctetStream);

            //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

            FileInfo fiDownload = new FileInfo(PdfName);
            string name = fiDownload.Name;
            using (FileStream fileStream = fiDownload.OpenRead())
            {
                Application.Download(fileStream, name);
            }

        }

        string strDOBNAMEbuttonMode = string.Empty;
        private void btnDuplicateFamilyId_Click(object sender, EventArgs e)
        {
            strDOBNAMEbuttonMode = "EXCEL";
            string strRelationDesc = string.Empty;
            string strKey = string.Empty, strFname = string.Empty, strLName = string.Empty;
            string strMainkey = string.Empty, strFamilyId = string.Empty;
            this.Size = new Size(930, 590);
            pnlgvwMain.Size = new Size(930, 533);
            pnlSearch.Size = new Size(930, 109);
            pnlMain.Size = new Size(930, 533);
            this.gvtRelation.HeaderText = "Intake Date";
            //**chkb50rec.Visible = false;
            //lblCount.Visible = false;
            //lblTotal.Visible = false;
            //gvwSub.Visible = false;
            pnlgvwSub.Visible = false;
            //**btn_Masssmash.Visible = false;
            btnFamilyId.Visible = false;
            pnlMassSmash.Visible = false;
            btnDelete.Visible = false;
            chkExcelAllrows.Visible = false;
            pnlExcel.Visible = false;
            gvwMain.SelectionChanged -= new EventHandler(gvwMain_SelectionChanged);
            gvwMain.Rows.Clear();
            gvwSub.Rows.Clear();
            if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
            if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();

            string strYear = string.Empty, strdob = string.Empty, strFromDt = string.Empty, strToDt = string.Empty, strlAgency = string.Empty, strlDept = string.Empty, strlProgram = string.Empty, strlPrYear = string.Empty;

            string strDupLvl = string.Empty;

            if (chkHierarchy.Checked)
            {
                strlAgency = Agency;
                strlDept = Dept;
                strlProgram = Prog;
                strlPrYear = Program_Year2;
            }

            if (BaseForm.BaseAgencyControlDetails.FamilyIdDuplvl == "Y")
                strDupLvl = "Y";
            List<CaseSnpEntity> snpdupdetails = _model.CaseMstData.GetSnpFixclinetIdHie(strDupLvl, txtMaxim.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "SEARCHDUPFAMILYID", "FIXFAMILYIDBENLEVELINTAKE", strlAgency, strlDept, strlProgram, strlPrYear, string.Empty, string.Empty);
            bool boolchkstatus = false;
            bool boolreadonly = true;
            int index;
            gvtchkSelect.ReadOnly = false;
            foreach (CaseSnpEntity item in snpdupdetails)
            {

                if (snpdupdetails.FindAll(u => u.NameixFi.Trim() == item.NameixFi.Trim() && u.NameixLast.Trim() == item.NameixLast.Trim() && u.AltBdate == item.AltBdate).Count > 1)
                {
                    boolchkstatus = false;
                    boolreadonly = true;
                    strRelationDesc = string.Empty;
                    CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == item.MemberCode.ToString().Trim());
                    if (commonrelationdesc != null)
                        strRelationDesc = commonrelationdesc.Desc;

                    int intlpbcount = snpdupdetails.FindAll(u => u.ClientId == item.ClientId && u.IncomeBasis != string.Empty).Count;
                    if (intlpbcount > 0)
                    {
                        if (item.IncomeBasis == string.Empty && Convert.ToInt32(item.Cacount) == 0 && Convert.ToInt32(item.Mscount) == 0 && Convert.ToInt32(item.Contcount) == 0)
                        {
                            boolchkstatus = true;
                            boolreadonly = false;
                        }
                    }
                    else
                    {
                        if (item.IncomeBasis == string.Empty && Convert.ToInt32(item.Cacount) == 0 && Convert.ToInt32(item.Mscount) == 0 && Convert.ToInt32(item.Contcount) == 0)
                        {
                            boolreadonly = false;
                        }

                    }


                    strMainkey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App + item.FamilySeq;
                    index = gvwMain.Rows.Add(boolchkstatus, item.Agency + item.Dept + item.Program + " " + item.Year + " " + item.App, LookupDataAccess.GetPhoneSsnNoFormat(item.Ssno), item.ClientId, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, "3"), LookupDataAccess.Getdate(item.AltBdate), LookupDataAccess.Getdate(item.DateLstc), LookupDataAccess.Getdate(item.Mst_IntakeDate).Replace("01/01/1889",""), strMainkey, item.IncomeBasis);//, LookupDataAccess.GetMemberName(item.NameixFi, item.NameixMi, item.NameixLast, propBaseForm.BaseHierarchyCnFormat), item.ClaimSsno

                    gvwMain.Rows[index].Cells["gvtchkSelect"].ReadOnly = boolreadonly;

                    strKey = item.Agency + item.Dept + item.Program + item.Year.Trim() + item.App;



                    if (item.Status.Trim() != "A")
                        gvwMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    gvwMain.Rows[index].Tag = item;
                    CommonFunctions.setTooltip(index, item.AddOperator, item.DateAdd, item.LstcOperator, item.DateLstc, gvwMain);
                }
            }
            if (gvwMain.Rows.Count > 0)
            {
                btnDelete.Visible = true;
                pnlExcel.Visible = true;
                // On_SaveExcelDuplicateForm_Closed();
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvwMain.Rows.Count > 0)
            {
                List<DataGridViewRow> SelectedgvRows = (from c in gvwMain.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvtchkSelect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();
                if (SelectedgvRows.Count > 0)
                {
                    MessageBox.Show("Are you sure you want to delete the Selected Records? ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandler);
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("Please select atleast one record");
                }
            }

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
                    if (gvwMain.Rows.Count > 0)
                    {
                        On_SaveExcelDuplicateForm_Closed("Delete");
                    }
                    int intrecordcount = 0;
                    foreach (DataGridViewRow item in gvwMain.Rows)
                    {
                        if (item.Cells["gvtchkSelect"].Value.ToString().ToUpper() == "TRUE")
                        {
                            CaseSnpEntity snpentity = item.Tag as CaseSnpEntity;
                            _model.CaseMstData.DeleteAllApplicantData(snpentity);
                            intrecordcount = intrecordcount + 1;
                        }
                    }
                    if (intrecordcount > 0)
                    {
                        CommonFunctions.MessageBoxDisplay("Total " + intrecordcount + " Records deleted.");
                        btnDelete.Visible = false;
                        gvwMain.Rows.Clear();
                    }
                }
            //}
        }

        private void lblTotal2_Click(object sender, EventArgs e)
        {

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
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = false;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            //s96.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");




            WorksheetStyle s96Yellow = styles.Add("s96Yellow");
            s96Yellow.Font.FontName = "Arial";
            s96Yellow.Font.Color = "#000000";
            s96Yellow.Interior.Color = "#ffff00";
            s96Yellow.Font.Bold = false;
            s96Yellow.Interior.Pattern = StyleInteriorPattern.Solid;
            s96Yellow.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96Yellow.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96Yellow.Alignment.WrapText = true;
            s96Yellow.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

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

        private void On_SaveExcelDuplicateForm_Closed(string strDelete)
        {
            Random_Filename = null;

            PdfName = "FIXFAMID_Audit_AllRows_NameandDOB_" + BaseForm.UserID.Trim();


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

            string strrowadded = string.Empty;
            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);


            Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;
            string ReportName = "Sheet1";

            sheet = book.Worksheets.Add(ReportName);
            sheet.Table.DefaultRowHeight = 14.25F;

            sheet.Table.Columns.Add(150);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(250);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(100);
            sheet.Table.Columns.Add(100);
            if (strDelete != string.Empty)
                sheet.Table.Columns.Add(80);

            Row0 = sheet.Table.Rows.Add();

            cell = Row0.Cells.Add("SNP KEY", DataType.String, "s94");
            cell = Row0.Cells.Add("SSN", DataType.String, "s94");
            cell = Row0.Cells.Add("Family ID", DataType.String, "s94");
            cell = Row0.Cells.Add("Name", DataType.String, "s94");
            cell = Row0.Cells.Add("Date of Birth", DataType.String, "s94");
            cell = Row0.Cells.Add("Last Changed", DataType.String, "s94");
            cell = Row0.Cells.Add("Intake Date", DataType.String, "s94");
            cell = Row0.Cells.Add("Relation", DataType.String, "s94");
            cell = Row0.Cells.Add("Ben.Level", DataType.String, "s94");
            cell = Row0.Cells.Add("CA Count", DataType.String, "s94");
            cell = Row0.Cells.Add("MS Count", DataType.String, "s94");
            cell = Row0.Cells.Add("Cont Count", DataType.String, "s94");
            if (strDelete != string.Empty)
                cell = Row0.Cells.Add("Delete", DataType.String, "s94");
            // Row0 = sheet.Table.Rows.Add();


            string strRelationDesc = string.Empty;
            string strKey = string.Empty, strFname = string.Empty, strLName = string.Empty;
            string strMainkey = string.Empty, strFamilyId = string.Empty;

            if (!string.IsNullOrEmpty(txtFName.Text.Trim())) strFname = txtFName.Text.Trim();
            if (!string.IsNullOrEmpty(txtLName.Text.Trim())) strLName = txtLName.Text.Trim();

            string strYear = string.Empty, strdob = string.Empty, strFromDt = string.Empty, strToDt = string.Empty, strlAgency = string.Empty, strlDept = string.Empty, strlProgram = string.Empty, strlPrYear = string.Empty;

            string strDupLvl = string.Empty;

            if (chkHierarchy.Checked)
            {
                strlAgency = Agency;
                strlDept = Dept;
                strlProgram = Prog;
                strlPrYear = Program_Year2;
            }

            if (BaseForm.BaseAgencyControlDetails.FamilyIdDuplvl == "Y")
                strDupLvl = "Y";
            List<CaseSnpEntity> snpdupdetails = _model.CaseMstData.GetSnpFixclinetIdHie(strDupLvl, txtMaxim.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "SEARCHDUPFAMILYID", "FIXFAMILYIDBENLEVEL", strlAgency, strlDept, strlProgram, strlPrYear, string.Empty, string.Empty);
            foreach (DataGridViewRow gvsubrows in gvwMain.Rows)
            {

                CaseSnpEntity sub2item = gvsubrows.Tag as CaseSnpEntity;

                strRelationDesc = string.Empty;
                CommonEntity commonrelationdesc = propRelation.Find(u => u.Code == sub2item.MemberCode.ToString().Trim());
                if (commonrelationdesc != null)
                    strRelationDesc = commonrelationdesc.Desc;

                if (strFamilyId != string.Empty)
                {
                    if (strFamilyId != sub2item.ClientId)
                    {
                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                        strFamilyId = sub2item.ClientId;
                    }
                }

                Row0 = sheet.Table.Rows.Add();
                cell = Row0.Cells.Add(sub2item.Agency + sub2item.Dept + sub2item.Program + " " + sub2item.Year + " " + sub2item.App, DataType.String, "s96");

                cell = Row0.Cells.Add(LookupDataAccess.GetPhoneSsnNoFormat(sub2item.Ssno), DataType.String, "s96");
                cell = Row0.Cells.Add(sub2item.ClientId, DataType.String, "s96");

                cell = Row0.Cells.Add(LookupDataAccess.GetMemberName(sub2item.NameixFi, sub2item.NameixMi, sub2item.NameixLast, "3"), DataType.String, "s96");

                cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.AltBdate), DataType.String, "s96");
                cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.DateLstc), DataType.String, "s96");
                cell = Row0.Cells.Add(LookupDataAccess.Getdate(sub2item.Mst_IntakeDate).Replace("01/01/1889", ""), DataType.String, "s96");
                cell = Row0.Cells.Add(strRelationDesc, DataType.String, "s96");
                cell = Row0.Cells.Add(sub2item.IncomeBasis, DataType.String, "s96");
                cell = Row0.Cells.Add(sub2item.Cacount, DataType.String, "s96");
                cell = Row0.Cells.Add(sub2item.Mscount, DataType.String, "s96");
                cell = Row0.Cells.Add(sub2item.Contcount, DataType.String, "s96");
                if (strDelete != string.Empty)
                {
                    if (gvsubrows.Cells["gvtchkSelect"].Value.ToString().ToUpper() == "TRUE")
                        cell = Row0.Cells.Add("Y", DataType.String, "s96");
                    else
                        cell = Row0.Cells.Add("", DataType.String, "s96");
                }
                strFamilyId = sub2item.ClientId;
            }

            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            FileDownloadGateway downloadGateway = new FileDownloadGateway();


            downloadGateway.Filename = "FIXFAMID_Audit_AllRows_NameandDOB_" + BaseForm.UserID.Trim() + ".xls";



            // downloadGateway.Filename = PdfName;

            // downloadGateway.Version = file.Version;

            downloadGateway.SetContentType(DownloadContentType.OctetStream);

            downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

        }


        #endregion

    }
}