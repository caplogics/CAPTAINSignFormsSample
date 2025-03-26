/************************************************************************
 * Conversion On        :   11/28/2022
 * Converted By         :   Kranthi
 * Last Modification On :   11/28/2022
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0006CLOSESCREEN : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public CASE0006CLOSESCREEN(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propCASEMSProfile = new List<Model.Objects.CASEMSEntity>();
            propResult = new List<Model.Objects.CommonEntity>();
            propMSMASTERList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
            this.Text = privileges.Program;
            BaseForm = baseForm;
            Privileges = privileges;
            strFormLoad = string.Empty;
            fillCombo();
        }
        #region properties
        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<CASEMSEntity> propCASEMSProfile { get; set; }
        public List<CommonEntity> propResult { get; set; }
        public List<MSMASTEntity> propMSMASTERList { get; set; }
        public string strFormLoad { get; set; }

        #endregion


        private void fillCombo()
        {
            propResult = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RESULTS, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg); ////_model.lookupDataAccess.GetCaseType();
            // CaseType = filterByHIE(CaseType);
            cmbMsResult.Items.Insert(0, new ListItem("Select One", "0"));
            cmbMsResult.ColorMember = "FavoriteColor";
            cmbMsResult.SelectedIndex = 0;

            cmbResult.Items.Insert(0, new ListItem("Select One", "0"));
            cmbResult.ColorMember = "FavoriteColor";
            cmbResult.SelectedIndex = 0;
            foreach (CommonEntity casetype in propResult)
            {
                ListItem li = new ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Green : Color.Red);
                cmbMsResult.Items.Add(li);
                cmbResult.Items.Add(li);
            }

        }

        private void Hepl_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE0006_Add");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidateForm(string.Empty))
                {
                    strFormLoad = "Search";
                    gvwService.Rows.Clear();
                    chkSelectAll.Checked = false;
                    string strType = string.Empty;
                    if (rdoIntake.Checked)
                        strType = "INTAKE";

                    DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.GETSPMWITHMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, dtpFrmDate.Value.ToShortDateString(), dtpToDt.Value.ToShortDateString(), strType, ((ListItem)cmbMsResult.SelectedItem).Value.ToString());
                    List<CASESPMEntity> CASESPMProfile = new List<CASESPMEntity>();
                    if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in CASESPMData.Tables[0].Rows)
                            CASESPMProfile.Add(new CASESPMEntity(row, "SEARCHSPM"));
                    }

                    List<CASEMSEntity> CASEMSProfile = new List<CASEMSEntity>();

                    if (CASESPMData != null && CASESPMData.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row in CASESPMData.Tables[1].Rows)
                            CASEMSProfile.Add(new CASEMSEntity(row, string.Empty));
                    }
                    propCASEMSProfile = CASEMSProfile;

                    List<CASEMSEntity> SearchCasemsEntity = new List<CASEMSEntity>();
                    CASEMSEntity SelCasems = new CASEMSEntity();
                    gvwService.SelectionChanged -= new EventHandler(gvwService_SelectionChanged);
                    List<CASESPMEntity> casespmentity = CASESPMProfile;//_model.SPAdminData.GETSPMWITHMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, dtpFrmDate.Value.ToShortDateString(), dtpToDt.Value.ToShortDateString(), strType);
                    if (casespmentity.Count > 0)
                    {
                        int index = 0; int MatchCnt = 0, UnmatchCnt = 0;
                        foreach (CASESPMEntity caseitem in casespmentity)
                        {
                            MatchCnt = 0; UnmatchCnt = 0;

                            //if (caseitem.Bulk_Post.ToString().Trim() == caseitem.MS_Postings_Cnt.ToString().Trim()) 
                            //{
                            //    index = gvwService.Rows.Add(false, caseitem.app_no, caseitem.FirstName, caseitem.Sp0_Desc, LookupDataAccess.Getdate(caseitem.SPM_TrigDate), LookupDataAccess.Getdate(caseitem.startdate), caseitem.CA_Postings_Cnt, caseitem.MS_Postings_Cnt, caseitem.service_plan, caseitem.Seq, "N");
                            //    gvwService.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                            //}
                            //else
                            //{
                            SearchCasemsEntity = propCASEMSProfile.FindAll(u => u.Agency == BaseForm.BaseAgency && u.Dept == BaseForm.BaseDept && u.Program == BaseForm.BaseProg && u.Year == BaseForm.BaseYear && u.App_no.ToString() == caseitem.app_no && u.Service_plan.ToString() == caseitem.service_plan && u.SPM_Seq.ToString() == caseitem.Seq);
                            SearchCasemsEntity = SearchCasemsEntity.OrderByDescending(u => u.Date).ThenBy(u => u.MS_Code.ToString().Trim()).ToList();
                            if (SearchCasemsEntity.Count > 0)
                            {
                                SelCasems = null;
                                List<CASEMSEntity> CasemsSel = new List<CASEMSEntity>();
                                var MSCodes = SearchCasemsEntity.GroupBy(u => new { u.MS_Code, u.Branch, u.Group }).ToList();
                                //var MSCodes = SearchCasemsEntity.GroupBy(u => u.MS_Code.Trim()).Select(u=> new { MSCode=u.Key,MSCount=u.Count()});
                                foreach (var item in MSCodes)
                                {
                                    CasemsSel = SearchCasemsEntity.FindAll(u => u.MS_Code.Equals(item.Key.MS_Code) && u.Branch.Equals(item.Key.Branch) && u.Group.Equals(item.Key.Group)).OrderByDescending(u => Convert.ToDateTime(u.Date.ToString())).ToList();
                                    if (CasemsSel.Count > 0)
                                        SelCasems = CasemsSel[0];
                                    //SearchCasemsEntity.Find(u => u.MS_Code.Equals(item.Key.MS_Code) && u.Branch.Equals(item.Key.Branch) && u.Group.Equals(item.Key.Group));
                                    if (SelCasems != null)
                                    {
                                        if (SelCasems.Result.Trim() == ((ListItem)cmbMsResult.SelectedItem).Value.ToString())
                                        {
                                            MatchCnt++;
                                            //break;
                                        }
                                        else UnmatchCnt++;
                                    }

                                }
                            }
                            if (MatchCnt > 0)
                            {
                                index = gvwService.Rows.Add(false, caseitem.app_no, caseitem.FirstName, caseitem.Sp0_Desc, LookupDataAccess.Getdate(caseitem.SPM_TrigDate), LookupDataAccess.Getdate(caseitem.startdate), caseitem.CA_Postings_Cnt, (MatchCnt + UnmatchCnt).ToString(), caseitem.service_plan, caseitem.Seq, "N");
                                gvwService.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                index = gvwService.Rows.Add(false, caseitem.app_no, caseitem.FirstName, caseitem.Sp0_Desc, LookupDataAccess.Getdate(caseitem.SPM_TrigDate), LookupDataAccess.Getdate(caseitem.startdate), caseitem.CA_Postings_Cnt, (MatchCnt + UnmatchCnt).ToString(), caseitem.service_plan, caseitem.Seq, "Y");
                                gvwService.Rows[index].DefaultCellStyle.ForeColor = Color.Green;
                                gvwService.Rows[index].ReadOnly = false;
                            }
                        }
                        //else
                        //{
                        //    index = gvwService.Rows.Add(false, caseitem.app_no, caseitem.FirstName, caseitem.Sp0_Desc, LookupDataAccess.Getdate(caseitem.SPM_TrigDate), LookupDataAccess.Getdate(caseitem.startdate), caseitem.CA_Postings_Cnt, caseitem.MS_Postings_Cnt, caseitem.service_plan, caseitem.Seq, "Y");
                        //    gvwService.Rows[index].DefaultCellStyle.ForeColor = Color.Green;
                        //    gvwService.Rows[index].ReadOnly = false;
                        //}
                        //}
                        gvwService.SelectionChanged += new EventHandler(gvwService_SelectionChanged);
                        if (gvwService.Rows.Count > 0)
                        {
                            gvwService.Rows[0].Selected = true;
                            gvwService_SelectionChanged(gvwService, new EventArgs());
                        }

                        btnMassfill.Enabled = true;
                    }
                    else
                    {
                        btnMassfill.Enabled = false;
                        CommonFunctions.MessageBoxDisplay("No records exist");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool ValidateForm(string strType)
        {
            bool isValid = true;
            _errorProvider.SetError(dtcompletion, null);
            _errorProvider.SetError(dtpFrmDate, null);
            _errorProvider.SetError(dtpToDt, null);
            _errorProvider.SetError(cmbMsResult, null);
            if (strType != string.Empty)
            {
                if (cmbMsResult.SelectedItem == null || ((ListItem)cmbMsResult.SelectedItem).Text == Consts.Common.SelectOne)
                {
                    _errorProvider.SetError(cmbMsResult, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMSResult.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbMsResult, null);
                }

                if (dtcompletion.Checked == false)
                {
                    _errorProvider.SetError(dtcompletion, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCompletiondt.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtcompletion, null);
                }
            }
            else
            {

                if (dtpFrmDate.Checked == false)
                {
                    _errorProvider.SetError(dtpFrmDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFrmDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFrmDate, null);
                }
                if (dtpToDt.Checked == false)
                {
                    _errorProvider.SetError(dtpToDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDt, null);

                    if (dtpFrmDate.Value.Date > dtpToDt.Value.Date)
                    {
                        _errorProvider.SetError(dtpToDt, "End Date should be greater than Start Date");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpToDt, null);
                    }
                }
                if (cmbMsResult.SelectedItem == null || ((ListItem)cmbMsResult.SelectedItem).Text == Consts.Common.SelectOne)
                {
                    _errorProvider.SetError(cmbMsResult, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMSResult.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(cmbMsResult, null);
                }
            }


            return (isValid);
        }

        object _sender; EventArgs _e;
        private void btnMassfill_Click(object sender, EventArgs e)
        {
            if (ValidateForm("MASSFILL"))
            {
                _sender = sender; _e = e;

                int intupdate = 0;
                foreach (DataGridViewRow gvrowitem in gvwService.Rows)
                {
                    if (gvrowitem.Cells["gvtCheckbox"].Value.ToString().ToUpper() == "TRUE")
                    {
                        intupdate = intupdate + 1;
                    }
                }

                if (intupdate > 0) 
                    MessageBox.Show("Are you sure you want Close Actual Completion Date ?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                else
                    CommonFunctions.MessageBoxDisplay("Please select atleast single applicant");

            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                StringBuilder strMstApplUpdate = new StringBuilder();

                strMstApplUpdate.Append("<Applicants>");
                foreach (DataGridViewRow gvrowitem in gvwService.Rows)
                {
                    if (gvrowitem.Cells["gvtCheckbox"].Value.ToString().ToUpper() == "TRUE")
                    {

                        strMstApplUpdate.Append("<Details AppNo = \"" + gvrowitem.Cells["gvtAppNo"].Value.ToString() + "\"  SPM_CODE = \"" + gvrowitem.Cells["gvtSpmId"].Value.ToString() + "\" SPM_SEQ = \"" + gvrowitem.Cells["gvtSpmSeq"].Value.ToString() + "\" />");
                    }
                }
                strMstApplUpdate.Append("</Applicants>");

                if (_model.SPAdminData.UpdateSpmActualCompletedt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, dtcompletion.Value.ToShortDateString(), strMstApplUpdate.ToString(), "Multiple", BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ((ListItem)cmbMsResult.SelectedItem).Value.ToString().Trim()))
                {
                    gvwService.Rows.Clear();
                    chkSelectAll.Checked = false;
                    string strType = string.Empty;
                    if (rdoIntake.Checked)
                        strType = "INTAKE";
                    List<CASESPMEntity> casespmentity = _model.SPAdminData.GETSPMWITHMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, dtpFrmDate.Value.ToShortDateString(), dtpToDt.Value.ToShortDateString(), strType, ((ListItem)cmbMsResult.SelectedItem).Value.ToString());
                    if (casespmentity.Count > 0)
                    {
                        btnSearch_Click(_sender, _e);



                        //foreach (CASESPMEntity caseitem in casespmentity)
                        //{
                        //    int index = gvwService.Rows.Add(false, caseitem.app_no, caseitem.FirstName, caseitem.Sp0_Desc, LookupDataAccess.Getdate(caseitem.SPM_TrigDate), LookupDataAccess.Getdate(caseitem.startdate), caseitem.CA_Postings_Cnt, caseitem.MS_Postings_Cnt, caseitem.service_plan, caseitem.Seq);
                        //}
                        btnMassfill.Enabled = true;
                    }
                    else
                    {
                        btnMassfill.Enabled = false;
                    }
                }
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                foreach (DataGridViewRow gvrowitem in gvwService.Rows)
                {
                    if (gvrowitem.Cells["gvtMSSwitch"].Value.ToString() == "Y")
                    {
                        gvrowitem.Cells["gvtCheckbox"].Value = true;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow gvrowitem in gvwService.Rows)
                {
                    gvrowitem.Cells["gvtCheckbox"].Value = false;
                }
            }
        }

        private void gvwService_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvwService.Rows.Count > 0)
                {
                    gvwMSDetails.Rows.Clear();
                    btnMSUpdate.Visible = true; btnMSUpdate.Text = "Post";
                    lblResult.Visible = cmbResult.Visible = lblMSDate.Visible = dtpMSdate.Visible = false;
                    int introwintdex = 0;
                    string strMsDesc = string.Empty;
                    List<CASEMSEntity> CASEMSProfile = propCASEMSProfile.FindAll(u => u.Agency == BaseForm.BaseAgency && u.Dept == BaseForm.BaseDept && u.Program == BaseForm.BaseProg && u.Year == BaseForm.BaseYear && u.App_no.ToString() == gvwService.SelectedRows[0].Cells["gvtAppNo"].Value.ToString() && u.Service_plan.ToString() == gvwService.SelectedRows[0].Cells["gvtSpmId"].Value.ToString() && u.SPM_Seq.ToString() == gvwService.SelectedRows[0].Cells["gvtSpmSeq"].Value.ToString());
                    CASEMSProfile = CASEMSProfile.OrderByDescending(u => u.Date).ThenBy(u => u.MS_Code.ToString().Trim()).ToList();
                    CASEMSEntity SelCasems = null;
                    var MSCodes = CASEMSProfile.GroupBy(u => new { u.MS_Code, u.Branch, u.Curr_Grp }).ToList();
                    //var MSCodes = CASEMSProfile.GroupBy(u => u.MS_Code.Trim()).Select(u => new { MSCode = u.Key, MSCount = u.Count() });
                    foreach (var item in MSCodes)
                    {
                        //SelCasems = CASEMSProfile.Find(u => u.MS_Code.Equals(item.MSCode.Trim()));
                        SelCasems = CASEMSProfile.Find(u => u.MS_Code.Equals(item.Key.MS_Code.Trim()) && u.Branch.Equals(item.Key.Branch.Trim()) && u.Curr_Grp.Equals(item.Key.Curr_Grp.Trim()));
                        strMsDesc = string.Empty;
                        MSMASTEntity msmastdesc = propMSMASTERList.Find(u => u.Code.Trim() == item.Key.MS_Code.ToString().Trim());
                        if (msmastdesc != null)
                            strMsDesc = msmastdesc.Desc;


                        if (((ListItem)cmbMsResult.SelectedItem).Value.ToString().Trim() == SelCasems.Result.Trim())
                        {
                            //lblResult.Visible = cmbResult.Visible = lblMSDate.Visible = dtpMSdate.Visible = true;
                            //propResult.Find(u => u.Code.Trim() == msitem.Result.Trim()).Desc.Trim()
                            string ResultDesc = string.Empty;
                            CommonEntity Ent = propResult.Find(u => u.Code.Trim() == SelCasems.Result.Trim());
                            if (Ent != null) ResultDesc = Ent.Desc.Trim();

                            introwintdex = gvwMSDetails.Rows.Add(false, strMsDesc, ResultDesc, LookupDataAccess.Getdate(SelCasems.Date), SelCasems.MS_Code, SelCasems.Result, SelCasems.Branch, SelCasems.Group, SelCasems.SPM_Seq, SelCasems.Service_plan, SelCasems.App_no);
                            //kranthi//gvwMSDetails.Rows[introwintdex].Enabled = false;
                            CommonFunctions.SetComboBoxValue(cmbResult, "0");
                        }
                        else
                        {
                            string ResultDesc = string.Empty;
                            CommonEntity Ent = propResult.Find(u => u.Code.Trim() == SelCasems.Result.Trim());
                            if (Ent != null) ResultDesc = Ent.Desc.Trim();

                            // lblResult.Visible = false; cmbResult.Visible = false; lblMSDate.Visible = false; dtpMSdate.Visible = false;
                            btnMSUpdate.Visible = false;
                            introwintdex = gvwMSDetails.Rows.Add(false, strMsDesc, ResultDesc, LookupDataAccess.Getdate(SelCasems.Date), SelCasems.MS_Code, SelCasems.Result, SelCasems.Branch, SelCasems.Group, SelCasems.SPM_Seq, SelCasems.Service_plan, SelCasems.App_no);
                            //kranthi//gvwMSDetails.Rows[introwintdex].Enabled = false;
                        }

                    }
                    //foreach (CASEMSEntity msitem in CASEMSProfile)
                    //{
                    //    strMsDesc = string.Empty;
                    //    MSMASTEntity msmastdesc = propMSMASTERList.Find(u => u.Code.Trim() == msitem.MS_Code.Trim());
                    //    if (msmastdesc != null)
                    //        strMsDesc = msmastdesc.Desc;
                    //    if (((ListItem)cmbMsResult.SelectedItem).Value.ToString().Trim() == msitem.Result.Trim())
                    //    {
                    //        lblResult.Visible = cmbResult.Visible = lblMSDate.Visible = dtpMSdate.Visible = true;
                    //        //propResult.Find(u => u.Code.Trim() == msitem.Result.Trim()).Desc.Trim()
                    //        string ResultDesc = string.Empty;
                    //        CommonEntity Ent = propResult.Find(u => u.Code.Trim() == msitem.Result.Trim());
                    //        if (Ent != null) ResultDesc = Ent.Desc.Trim();

                    //        introwintdex = gvwMSDetails.Rows.Add(false, strMsDesc, ResultDesc, LookupDataAccess.Getdate(msitem.Date), msitem.MS_Code, msitem.Result, msitem.Branch, msitem.Group, msitem.SPM_Seq, msitem.Service_plan, msitem.App_no);
                    //        gvwMSDetails.Rows[introwintdex].Enabled = false;
                    //        CommonFunctions.SetComboBoxValue(cmbResult, "0");
                    //    }
                    //    else
                    //    {
                    //        string ResultDesc = string.Empty;
                    //        CommonEntity Ent = propResult.Find(u => u.Code.Trim() == msitem.Result.Trim());
                    //        if (Ent != null) ResultDesc = Ent.Desc.Trim();

                    //        lblResult.Visible = false; cmbResult.Visible = false; lblMSDate.Visible = false; dtpMSdate.Visible = false;
                    //        btnMSUpdate.Visible = false;
                    //        introwintdex = gvwMSDetails.Rows.Add(false, strMsDesc, ResultDesc, LookupDataAccess.Getdate(msitem.Date), msitem.MS_Code, msitem.Result, msitem.Branch, msitem.Group, msitem.SPM_Seq, msitem.Service_plan, msitem.App_no);
                    //        gvwMSDetails.Rows[introwintdex].Enabled = false;
                    //    }
                    //}

                    if (gvwMSDetails.Rows.Count == 0 || gvwService.CurrentRow.DefaultCellStyle.ForeColor == Color.Green)
                        lblResult.Visible = cmbResult.Visible = lblMSDate.Visible = dtpMSdate.Visible = btnMSUpdate.Visible = false;
                    else if (gvwService.CurrentRow.DefaultCellStyle.ForeColor == Color.Red)
                        lblResult.Visible = cmbResult.Visible = lblMSDate.Visible = dtpMSdate.Visible = btnMSUpdate.Visible = true;

                    if (gvwMSDetails.Rows.Count > 0)
                    {
                        gvwMSDetails.Rows[0].Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void lblResult_Click(object sender, EventArgs e)
        {

        }

        private void gvwService_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvwService.Rows.Count > 0)
                {
                    if (gvwService.CurrentCell.ColumnIndex == gvtCheckbox.Index)
                    {

                        if (gvwService.CurrentRow.Cells["gvtMSSwitch"].Value.ToString() == "N")
                        {
                            gvwService.CurrentRow.Cells["gvtCheckbox"].Value = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void btnMSUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateMSForm(string.Empty) && Validate_MS_Posting_Dtae())
            {
                _sender = sender; _e = e;
                int intupdate = 0;
                //foreach (DataGridViewRow gvrowitem in gvwMSDetails.Rows)
                //{
                //    if (gvrowitem.Cells["gvcMsSelect"].Value.ToString().ToUpper() == "TRUE")
                //    {
                //        intupdate = intupdate + 1;
                //    }
                //}
                //if (intupdate > 0)
                if (gvwMSDetails.Rows.Count > 0)
                    MessageBox.Show("Are you sure, do you want Post New MS Record(s)?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerResult);
                //else
                //    CommonFunctions.MessageBoxDisplay("Please select atleast single MS");

            }
        }

        private bool ValidateMSForm(string strType)
        {
            bool isValid = true;
            _errorProvider.SetError(dtcompletion, null);
            _errorProvider.SetError(cmbResult, null);
            //if (strType != string.Empty)
            //{
            if (cmbResult.SelectedItem == null || ((ListItem)cmbResult.SelectedItem).Text == Consts.Common.SelectOne)
            {
                _errorProvider.SetError(cmbResult, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResult.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbResult, null);
            }

            if (dtpMSdate.Checked == false)
            {
                _errorProvider.SetError(dtpMSdate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMSDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpMSdate, null);
            }

            //string[] dates = new string[gvwMSDetails.Rows.Count - 1];
            //int i = 0;
            //foreach (DataGridViewRow gvrowitem in gvwMSDetails.Rows)
            //{
            //    dates[i] = gvrowitem.Cells["gvtMSDate"].Value.ToString();
            //}
            //}

            return (isValid);
        }

        List<CASEMSEntity> MS_Post_Dates_List = new List<CASEMSEntity>();
        private bool Validate_MS_Posting_Dtae()
        {
            bool Can_Save = true;

            foreach (DataGridViewRow gvrowitem in gvwMSDetails.Rows)
            {
                CASEMSEntity Search_Entity = new CASEMSEntity(true);

                Search_Entity.Agency = BaseForm.BaseAgency;
                Search_Entity.Dept = BaseForm.BaseDept;
                Search_Entity.Program = BaseForm.BaseProg;
                Search_Entity.Year = BaseForm.BaseYear;
                Search_Entity.App_no = gvrowitem.Cells["gvtMsApplicant"].Value.ToString();
                Search_Entity.Service_plan = gvrowitem.Cells["gvtMsSpmplan"].Value.ToString();
                Search_Entity.SPM_Seq = gvrowitem.Cells["gvtMSSpmSeq"].Value.ToString();
                Search_Entity.Branch = gvrowitem.Cells["gvtMsBranch"].Value.ToString();
                Search_Entity.Group = gvrowitem.Cells["gvtMsGroup"].Value.ToString();
                Search_Entity.MS_Code = gvrowitem.Cells["gvtMsCode"].Value.ToString();
                if (MS_Post_Dates_List.Count == 0)
                    MS_Post_Dates_List = _model.SPAdminData.Browse_CASEMS(Search_Entity, "Browse");

                int Matched_Count = 0;
                foreach (CASEMSEntity Ent in MS_Post_Dates_List)
                {
                    if (Ent.Result.ToString().ToUpper() == ((ListItem)cmbMsResult.SelectedItem).Value.ToString().ToUpper())
                    {
                        if (Convert.ToDateTime(Ent.Date.Trim()).ToShortDateString() == dtpMSdate.Value.ToShortDateString())
                        {
                            Can_Save = false;
                            Matched_Count++;

                            //if (Pass_MS_Entity.Rec_Type == "I")
                            break;
                        }
                    }
                }

                if (!Can_Save)
                    break;
                //if ((Pass_MS_Entity.Rec_Type == "U" && Matched_Count > 1) || (Pass_MS_Entity.Rec_Type == "I" && Matched_Count > 0))

            }
            if (!Can_Save)
                MessageBox.Show("You Cannot have Multiple Postings for the Same Day", "CAP Systems");


            return Can_Save;
        }


        private void MessageBoxHandlerResult(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                int intupdate = 0;
                foreach (DataGridViewRow gvrowitem in gvwMSDetails.Rows)
                {
                    if (gvrowitem.Cells["gvtMSResultCode"].Value.ToString().ToUpper() == ((ListItem)cmbMsResult.SelectedItem).Value.ToString().ToUpper())
                    {
                        if (_model.SPAdminData.UpdateSpmActualCompletedt(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, dtpMSdate.Value.ToShortDateString(), string.Empty, "CASEMS", BaseForm.UserID, gvrowitem.Cells["gvtMsApplicant"].Value.ToString(), gvrowitem.Cells["gvtMsSpmplan"].Value.ToString(), gvrowitem.Cells["gvtMSSpmSeq"].Value.ToString(), gvrowitem.Cells["gvtMsBranch"].Value.ToString(), gvrowitem.Cells["gvtMsGroup"].Value.ToString(), gvrowitem.Cells["gvtMsCode"].Value.ToString(), gvrowitem.Cells["gvtMSDate"].Value.ToString(), ((ListItem)cmbResult.SelectedItem).Value.ToString().Trim()))
                        {
                            intupdate = intupdate + 1;
                        }

                    }
                }
                if (intupdate > 0)
                {
                    btnSearch_Click(_sender, _e);
                    if (gvwService.Rows.Count > 0)
                        gvwService.Rows[0].Selected = true;
                }

            }
        }

        private void cmbMsResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMsResult.Items.Count > 0)
                {
                    if (gvwService.Rows.Count > 0)
                        btnSearch_Click(sender, e);
                }
            }
            catch (Exception ex)
            {


            }

        }
    }
}