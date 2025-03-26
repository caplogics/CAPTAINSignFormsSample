#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using Wisej.Web;
using Captain.Common.Utilities;
using System.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using System.Text.RegularExpressions;
using DevExpress.XtraSpreadsheet.Commands;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PIPntakeSearchForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public PIPntakeSearchForm(string strHierchy, BaseForm baseForm, string strAgency, string strDept, string strProgram, string strYear)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            strFormLoad = string.Empty;
            BaseForm = baseForm;
            _propRegid = string.Empty;
            propHierchy = strHierchy;
            propAgency = strAgency; ;
            propDept = strDept;
            propProgram = strProgram;
            propYear = strYear;
            propClientRulesSwitch = "N";
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", baseForm.BaseAgency, baseForm.BaseDept, " ");
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];
                }
            }

            Fill_Gender_MemberCode_Desc_List();
        }
        string strFormLoad = string.Empty;
        public PIPntakeSearchForm(string strHierchy, BaseForm baseForm, string strAgency, string strDept, string strProgram, string strYear, string strConfNum, string strpEmailId)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            strFormLoad = "PublicPortal";
            BaseForm = baseForm;
            _propRegid = string.Empty;
            propHierchy = strHierchy;
            propAgency = strAgency; ;
            propDept = strDept;
            propProgram = strProgram;
            propYear = strYear;
            propClientRulesSwitch = "N";
            //panel2.Enabled = false;
            btnSearch.Visible = false;
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", propAgency, propDept, " ");
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];
                }
            }

            Fill_Gender_MemberCode_Desc_List();
            txtTokenNumber.Text = strConfNum;
            txtEmailId.Visible = false;
            txtTokenNumber.Visible = false;
            lblEmailId.Visible = false;
            lblTokenNo.Text = "Confirmation#: " + strConfNum + "                                                  Email ID: " + strpEmailId;
            lblTokenNo.Font = new Font(lblTokenNo.Font, FontStyle.Bold);
            lblTokenNo.Size = new System.Drawing.Size(700, 20);
            txtEmailId.Text = strpEmailId;

        }


        DataTable User_Hie_Acc_List = new DataTable();
        public string propHierchy;
        public string propAgency;
        public string propDept;
        public string propProgram;
        public string propYear;
        public string propSSN;
        public string propApplicantNumber;
        public string propFirstName;
        public string propLastName;
        public string propDob;
        public DataTable _propPIPData { get; set; }
        public DataTable _propCasesnpsubdt { get; set; }
        public string propLeanUserId = string.Empty;
        public string propLeanServices = string.Empty;
        public string _propRegid = string.Empty;
        public BaseForm BaseForm { get; set; }
        private void Hepl_Click(object sender, EventArgs e)
        {

        }


        private bool Get_Hi_Access_Status(string Curr_Hie)
        {
            bool Can_Add_Hie = false;

            if (string.IsNullOrEmpty(Curr_Hie))
            {
                foreach (DataRow dr in User_Hie_Acc_List.Rows)
                {
                    if (dr["PWH_AGENCY"].ToString() + dr["PWH_DEPT"].ToString() + dr["PWH_PROG"].ToString() == "******")
                    {
                        Can_Add_Hie = true; break;
                    }
                }
            }
            else
            {
                if (User_Hie_Acc_List.Rows.Count > 0)
                {
                    string All_Dept_Prog = "    ";
                    foreach (DataRow dr in User_Hie_Acc_List.Rows)
                    {
                        All_Dept_Prog = "CHECK";

                        if (dr["PWH_DEPT"].ToString() == "**")
                            All_Dept_Prog = "DEPT";

                        if (dr["PWH_DEPT"].ToString() != "**" && dr["PWH_PROG"].ToString() == "**")
                            All_Dept_Prog = "PROG";


                        if (All_Dept_Prog != "CHECK")
                        {
                            switch (All_Dept_Prog)
                            {
                                case "DEPT":
                                    if (Curr_Hie.Substring(0, 2) == dr["PWH_AGENCY"].ToString())
                                        Can_Add_Hie = true;
                                    break;
                                case "PROG":
                                    if (Curr_Hie.Substring(0, 4) == dr["PWH_AGENCY"].ToString() + dr["PWH_DEPT"].ToString())
                                        Can_Add_Hie = true;
                                    break;
                            }

                            if (Can_Add_Hie)
                                break;
                        }
                        else
                        {
                            if (Curr_Hie == dr["PWH_AGENCY"].ToString() + dr["PWH_DEPT"].ToString() + dr["PWH_PROG"].ToString())
                            {
                                Can_Add_Hie = true; break;
                            }
                        }
                    }
                }
            }

            return Can_Add_Hie;
        }

        string strPIPRegSubmitSwitch = string.Empty;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                string strEmailId = txtEmailId.Text;
                if (strFormLoad != string.Empty)
                    strEmailId = string.Empty;

                propSSN = string.Empty;
                string strFilterAgy = string.Empty;
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    strFilterAgy = propAgency;
                string strresult = CheckLeanIntakeData(string.Empty, txtTokenNumber.Text, strEmailId, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);
                gvwCustomer.Rows.Clear();
                TopGrid.SelectionChanged -= new EventHandler(TopGrid_SelectionChanged);
                TopGrid.Rows.Clear();
                BottomGrid.Rows.Clear();
                btnLeanData.Visible = false;
                btnServices.Visible = false;

                if (strresult == string.Empty)
                {
                    DataTable dt = GetPIPIntakeData(string.Empty, txtTokenNumber.Text, strEmailId, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);
                    List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();
                    if (dt.Rows.Count > 0)
                    {
                        _propPIPData = dt;
                        btnLeanData.Visible = true;
                        _propRegid = dt.Rows[0]["PIPREG_ID"].ToString().Trim();
                        foreach (DataRow dritem in dt.Rows)
                        {
                            string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);
                            string memberCode = string.Empty;
                            CommonEntity rel = Relation.Find(u => u.Code.Equals(dritem["PIP_MEMBER_CODE"].ToString()));
                            if (rel != null) memberCode = rel.Desc;
                            string DOB = string.Empty;
                            if (dritem["PIP_DOB"].ToString() != string.Empty)
                            {
                                DOB = CommonFunctions.ChangeDateFormat(dritem["PIP_DOB"].ToString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                DOB = Convert.ToDateTime(DOB).ToString("MM/dd/yyyy");
                            }
                            int rowIndex = gvwCustomer.Rows.Add(ApplicantName, dritem["PIP_SSN"].ToString(), DOB, memberCode, string.Empty);
                            gvwCustomer.Rows[rowIndex].Tag = dritem;
                            if (dritem["PIP_FAM_SEQ"].ToString() == "0")
                            {
                                propSSN = dritem["PIP_SSN"].ToString();
                                propFirstName = dritem["PIP_FNAME"].ToString();
                                propLastName = dritem["PIP_LNAME"].ToString();
                                propDob = dritem["PIP_DOB"].ToString();
                                propLeanUserId = dritem["PIP_REG_ID"].ToString();
                                propLeanServices = dritem["PIP_SERVICES"].ToString();
                                gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                                FillTopGridData(propFirstName, propLastName, propSSN, propDob, strFilterAgy);
                                if (strFormLoad != string.Empty)
                                {
                                    if (dritem["PIP_TOWNSHIP"].ToString().Trim() != string.Empty)
                                    {

                                       List<ZipCodeEntity> propzipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, dritem["PIP_TOWNSHIP"].ToString().Trim(), string.Empty);
                                        if (propzipcodeEntity.FindAll(u => u.Zcrcitycode.Trim() == dritem["PIP_TOWNSHIP"].ToString().Trim()).Count == 0)
                                        {
                                            CommonFunctions.MessageBoxDisplay("Customer not in a town/city the agency services");
                                        }
                                    }
                                }
                            }
                        }
                        if (TopGrid.Rows.Count > 0)
                        {
                            btnServices.Visible = true;
                        }
                        if (dt.Rows[0]["PIPREG_SUBMITTED"].ToString().Trim() == string.Empty)
                        {
                            strPIPRegSubmitSwitch = "Y";
                            btnLeanData.Visible = false;
                            btnServices.Visible = false;
                            if (strFormLoad != string.Empty)
                                btnEmailSend.Visible = true;
                            CommonFunctions.MessageBoxDisplay("Applicant is not yet submitted in PIP");
                        }

                    }
                    //  btn email_logic
                }
                else
                {
                    if (strresult == "Not exist")
                    {
                        if (txtEmailId.Text != string.Empty && txtTokenNumber.Text != string.Empty)
                        {
                            CommonFunctions.MessageBoxDisplay("No Intake Records found with this Email Id or Confirmation Number");
                        }
                        else
                        {
                            if (txtEmailId.Text != string.Empty && txtTokenNumber.Text == string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("No Intake Records found with this Email Id");
                            }
                            else if (txtEmailId.Text == string.Empty && txtTokenNumber.Text != string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("No Intake Records found with this Confirmation Number");
                            }
                        }
                    }
                    else if (strresult == "Email")
                    {
                        if (txtEmailId.Text != string.Empty && txtTokenNumber.Text != string.Empty)
                        {
                            CommonFunctions.MessageBoxDisplay("Confirmation Number or Email Id does not exist");
                        }
                        else
                        {
                            if (txtEmailId.Text != string.Empty && txtTokenNumber.Text == string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("Email Id does not exist");
                            }
                            else if (txtEmailId.Text == string.Empty && txtTokenNumber.Text != string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("Confirmation Number does not exist");
                            }
                        }

                    }
                    else if (strresult == "Email Agy" || strresult == "Exist Other Agy")
                    {
                        if (txtEmailId.Text != string.Empty && txtTokenNumber.Text != string.Empty)
                        {
                            CommonFunctions.MessageBoxDisplay("Confirmation Number or Email Id doesn't exist in " + propAgency);
                        }
                        else
                        {
                            if (txtEmailId.Text != string.Empty && txtTokenNumber.Text == string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("Email Id doesn't exist in " + propAgency);
                            }
                            else if (txtEmailId.Text == string.Empty && txtTokenNumber.Text != string.Empty)
                            {
                                CommonFunctions.MessageBoxDisplay("Confirmation Number doesn't exist in " + propAgency);
                            }
                        }

                    }
                    else
                        CommonFunctions.MessageBoxDisplay("PIP Data is already moved to  " + strresult);
                }

            }
        }

        private bool isValidate()
        {
            bool isValid = true;


            if (String.IsNullOrEmpty(txtTokenNumber.Text.Trim()) && String.IsNullOrEmpty(txtEmailId.Text.Trim()))
            {
                _errorProvider.SetError(txtTokenNumber, "Please enter Confirmation Number or Email id");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTokenNumber, null);
            }
            return isValid;
        }

        string CheckLeanIntakeData(string struserid, string strLcode, string strEmail, string strMode, string strDBName, string strAgency)
        {
            string strLocstatus = string.Empty;
            try
            {


                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (struserid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", struserid);
                    if (strLcode != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_CONFNO", strLcode);
                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_EMAIL", strEmail);
                    cmd.Parameters.AddWithValue("@AGENCY", strDBName);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgency);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    // Start getting the RetrunValue and Output from Stored Procedure
                    strLocstatus = cmd.Parameters["@Msg"].Value.ToString();
                    //if (IntReturn == 1)
                    //{
                    //    outputParam = cmd.Parameters["@OutputParam"].Value;
                    //    inoutParam = cmd.Parameters["@InputOutputParam"].Value;
                    //}

                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return strLocstatus;
        }

        DataTable GetPIPIntakeData(string struserid, string strLcode, string strEmail, string strMode, string strDBName, string strAgency)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (struserid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", struserid);
                    cmd.Parameters.AddWithValue("@PIP_CONFNO", strLcode);

                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_EMAIL", strEmail);
                    cmd.Parameters.AddWithValue("@AGENCY", strDBName);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgency);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 10);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);

            }
            return dt;

        }

        void UpdateLeanIntakeData(string strLcode, string strapp, string strUserid, string strEmail, string strDBName, string draggedFrm)
        {

            try
            {


                //using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[LEANAPPHISTORY](LAH_CODE,LAH_EMAIL,LAH_AGENCY,LAH_DEPT,LAH_PROGRAM,LAH_YEAR,LAH_APP_NO,LAH_DBNAME,LAH_ADD_DATE)VALUES('"+ strLcode + "','"+ strEmail+"','"+ propAgency + "','"+ propDept + "','"+ propProgram + "','"+ propYear + "','"+ strapp + "','"+ strDBName +"','"+ DateTime.Now.Date +"')", con))
                //{
                //    cmd.ExecuteNonQuery();
                //}
                if (strapp != string.Empty)
                {
                    SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE PIPINTAKE SET PIP_DRAGGED='A',PIP_CAP_AGY='" + propAgency + "',PIP_CAP_DEPT ='" + propDept + "',PIP_CAP_PROG ='" + propProgram + "',PIP_CAP_YEAR ='" + propYear + "',PIP_CAP_APP_NO='" + strapp + "', PIP_DRAG_FRM='" + draggedFrm +"' WHERE PIP_REG_ID= '" + strLcode + "'", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


            }

        }




        private void btnLeanData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you would like to move PIP Intake into " + propHierchy + " ? ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
        }
        private string strApplNo;
        private string strClientIdOut;
        private string strFamilyIdOut;
        private string strSSNNOOut;
        private string strErrorMsg;
        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchLeanIntake("APP", "ALL", null, null, null, string.Empty, propFirstName, string.Empty, null, null, null, null, null, null, null, null, propDob, BaseForm.UserID, string.Empty, string.Empty);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        PIPIntakeSaveMethod(string.Empty);
                    }
                    else
                    {
                        PIPIntakeSaveMethod("OLDFAMID");
                        // MessageBox.Show("You have selected a HHLD MBR to become the primary applicant in the new intake.  You must decide \n if this is a new household or the same household with a different primary applicant. \n Select YES, if this is a NEW household. (New FamilyID created) \n Select NO, if this is the same household being copied to a new hierarchy.  (same FamilyID follows)", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageHandlerFamilyIdConformation, true);

                    }

                //}
            }
        }

        private void MessageHandlerFamilyIdConformation(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    PIPIntakeSaveMethod(string.Empty);
                }
                else
                {
                    PIPIntakeSaveMethod("OLDFAMID");
                }
            //}
        }

        void PIPIntakeSaveMethod(string strFamilyIdNew)
        {
            if (GetDup_APP_MEM_Status(propSSN))
            {
                //if (BottomGrid.Rows.Count > 0)
                //{
                //    int intmembercount = 0;
                //    foreach (DataGridViewRow gvBottomRow in BottomGrid.Rows)
                //    {
                //        foreach (DataGridViewRow gvleanIntakeRow in gvwCustomer.Rows)
                //        {
                //            if (gvBottomRow.Cells["gvtbotApp"].Value.ToString() == gvleanIntakeRow.Cells["gvtSSNM"].Value.ToString())
                //            {
                //                intmembercount = intmembercount + 1;
                //                break;
                //            }
                //        }
                //    }
                //    if (BottomGrid.Rows.Count > intmembercount)
                //    {

                //    }
                //    else
                //    {

                //    }
                //}
                //else
                //{
                bool boolSnpInsert = false;
                string strLAppNo = string.Empty;
                string strUserId = string.Empty;
                List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(BaseForm.BaseHierarchyCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                if (hierarchyEntity.Count > 0)
                    hierarchyEntity = hierarchyEntity.FindAll(u => u.InActiveFlag == "N" && u.UserID == BaseForm.UserID).ToList();

                CaseMstEntity mstdata = new CaseMstEntity();
                foreach (DataGridViewRow dritem in gvwCustomer.Rows)
                {
                    DataRow dr = dritem.Tag as DataRow;
                    if (dr != null)
                    {
                        
                        CaseSnpEntity SnpEntity = new CaseSnpEntity();
                        if (dr["PIP_FAM_SEQ"].ToString() == "0")
                        {
                            mstdata.ApplAgency = propAgency;
                            mstdata.ApplDept = propDept;
                            mstdata.ApplProgram = propProgram;
                            if (propYear == string.Empty)
                                mstdata.ApplYr = "    ";
                            else
                                mstdata.ApplYr = propYear;
                            mstdata.FamilySeq = "1";

                            mstdata.Email = dr["PIP_EMAIL"].ToString();
                            mstdata.Ssn = dr["PIP_SSN"].ToString();
                            mstdata.Language = dr["PIP_PRI_LANGUAGE"].ToString();
                            //Added by SUdheer on 06/20/24
                            mstdata.LanguageOt = dr["PIP_SEC_LANGUAGE"].ToString();
                            mstdata.FamilyType = dr["PIP_FAMILY_TYPE"].ToString();
                            mstdata.Housing = dr["PIP_HOUSING"].ToString();
                            // mstdata.snpSchoolDistrict = dr["PIP_"].ToString();
                            mstdata.IncomeTypes = dr["PIP_INCOME_TYPES"].ToString();
                            //  mstdata.AddressDetails = dr["PIP_ADDRESS"].ToString();
                            mstdata.Area = dr["PIP_AREA"].ToString();
                            mstdata.Phone = dr["PIP_HOME_PHONE"].ToString();
                            mstdata.CellPhone = dr["PIP_CELL_NUMBER"].ToString();
                            mstdata.Hn = dr["PIP_HOUSENO"].ToString();
                            mstdata.Direction = dr["PIP_DIRECTION"].ToString();
                            mstdata.Street = dr["PIP_STREET"].ToString();
                            mstdata.Suffix = dr["PIP_SUFFIX"].ToString();
                            mstdata.Apt = dr["PIP_APT"].ToString();
                            mstdata.Flr = dr["PIP_FLR"].ToString();
                            mstdata.City = dr["PIP_CITY"].ToString();
                            mstdata.State = dr["PIP_STATE"].ToString();
                            mstdata.Zip = dr["PIP_ZIP"].ToString();
                            mstdata.TownShip = dr["PIP_TOWNSHIP"].ToString();
                            mstdata.County = dr["PIP_COUNTY"].ToString();
                            mstdata.MstNCashBen = dr["PIP_NCASHBEN"].ToString();

                            //commented this code by Sudheer on 02/06/25 as per the 'PIP INTAKE AUTO-ASSIGNING SITE 2/4/2025' in the document CSEOP WiseJ.docx. 
                            ////we added the Site Added by Sudheer on 06/20/24 as per the document 'CSEOP Intake Portal notes from EOP testers.docx'
                            //We changed the logic on 02/10/25 as per the document 'CSEOP Intake Portal notes from EOP testers.docx'
                            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "EOC")
                            {
                                mstdata.Site = "MAIN";

                            }

                            mstdata.IntakeDate = DateTime.Now.ToShortDateString();

                            /// Murali added Defaullt filling logic on 07/09/2021
                            /// Murali added OK server 08 agency hard code 'CU' casetype on 01/07/2022
                            if (dr["PIP_AGENCY"].ToString().Trim() == "NEOCAA" && dr["PIP_AGY"].ToString().Trim() == "08")
                            {
                                mstdata.CaseType = "CU";
                            }
                            else
                            {
                                List<CommonEntity> commontypede = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CASETYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontypede.Count > 0)
                                {
                                    CommonEntity commondata = commontypede.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        mstdata.CaseType = commondata.Code;
                                    }
                                }
                            }
                            //Commented by SUdheer on 06/20/24 because we added a new field added (Secondary language) in PIP
                            //List<CommonEntity> commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                            //if (commontype.Count > 0)
                            //{
                            //    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                            //    if (commondata != null)
                            //    {
                            //        mstdata.LanguageOt = commondata.Code;
                            //    }
                            //}
                            List<CommonEntity> commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.BESTWAYTOCONTACT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                            if (commontype.Count > 0)
                            {
                                CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                if (commondata != null)
                                {
                                    mstdata.BestContact = commondata.Code;
                                }
                            }
                            commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                            if (commontype.Count > 0)
                            {
                                CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                if (commondata != null)
                                {
                                    mstdata.AboutUs = commondata.Code;
                                }
                            }
                            commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEATSOURCE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                            if (commontype.Count > 0)
                            {
                                CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                if (commondata != null)
                                {
                                    mstdata.Source = commondata.Code;
                                }
                            }
                            commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.METHODOFPAYINGFORHEAT, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                            if (commontype.Count > 0)
                            {
                                CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                if (commondata != null)
                                {
                                    mstdata.HeatIncRent = commondata.Code;
                                }
                            }
                            commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DWELLINGTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                            if (commontype.Count > 0)
                            {
                                CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                if (commondata != null)
                                {
                                    mstdata.Dwelling = commondata.Code;
                                }
                            }
                            if(hierarchyEntity.Count>0)
                            {
                                mstdata.CaseManager = hierarchyEntity[0].CaseWorker;
                                mstdata.IntakeWorker  = hierarchyEntity[0].CaseWorker;
                                mstdata.ExpCaseWorker = hierarchyEntity[0].CaseWorker;                                
                            }
                            mstdata.AddOperator1 = BaseForm.UserID;
                            mstdata.LstcOperator1 = BaseForm.UserID;
                            mstdata.ActiveStatus = "Y";
                            mstdata.Mode = "Add";
                            strUserId = dr["PIP_REG_ID"].ToString();

                            // Murali modified new family id genertate or existing family id used logic implemented once conformation button 02/12/2021
                            mstdata.FamilyId = strFamilyIdNew;
                            mstdata.ClientId = string.Empty;
                            //if (BottomGrid.Rows.Count > 0)
                            //{

                            //mstdata.FamilyId = BottomGrid.Rows[0].Cells["gvtBotFamilyId"].Value.ToString();
                            //foreach (DataGridViewRow gvBottomRow in BottomGrid.Rows)
                            //{
                            //    if (mstdata.Ssn.ToString().Trim() != string.Empty)
                            //    {
                            //        if (gvBottomRow.Cells["gvtBotSSNMain"].Value.ToString() == mstdata.Ssn.ToString())
                            //        {
                            //            mstdata.ClientId = gvBottomRow.Cells["gvtBotClientId"].Value.ToString();
                            //            break;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        string DOB;
                            //        if (!string.IsNullOrEmpty(gvBottomRow.Cells["gvtBotDOB"].Value.ToString()))
                            //            DOB = Convert.ToDateTime(gvBottomRow.Cells["gvtBotDOB"].Value.ToString()).ToShortDateString();
                            //        else
                            //            DOB = "01/01/1900";

                            //        if (dr["PIP_FNAME"].ToString().Trim() == gvBottomRow.Cells["gvtFirstName"].Value.ToString() && Convert.ToDateTime(dr["PIP_DOB"].ToString()).ToShortDateString() == DOB)
                            //        {
                            //            mstdata.Ssn = gvBottomRow.Cells["gvtBotSSNMain"].Value.ToString();
                            //            mstdata.ClientId = gvBottomRow.Cells["gvtBotClientId"].Value.ToString();
                            //            break;
                            //        }
                            //    }
                            //}

                            //}
                            //else
                            //{
                            //    mstdata.FamilyId = string.Empty;
                            //    mstdata.ClientId = string.Empty;
                            //}
                            mstdata.Type = Consts.CASE2001.ManualType;
                            if (mstdata.Ssn.ToString().Trim() == string.Empty)
                                mstdata.Ssn = "000000000";
                            boolSnpInsert = _model.CaseMstData.InsertUpdateCaseMstLeanIntake(mstdata, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg, SnpEntity, dr["PIP_FNAME"].ToString(), dr["PIP_DOB"].ToString());
                            strLAppNo = strApplNo;


                            // services insert
                            if (boolSnpInsert)
                            {
                                CaseMSTSER MSTSEREntity = new CaseMSTSER();
                                if (dr["PIP_SERVICES"].ToString().Trim() != string.Empty)
                                {
                                    string[] strServices = dr["PIP_SERVICES"].ToString().Split(',');
                                    foreach (string strserv in strServices)
                                    {
                                        if (strserv.ToString() != string.Empty)
                                        {
                                            MSTSEREntity = new CaseMSTSER();
                                            MSTSEREntity.Agency = propAgency;
                                            MSTSEREntity.Dept = propDept;
                                            MSTSEREntity.Program = propProgram;
                                            if (propYear == string.Empty)
                                                MSTSEREntity.Year = "    ";
                                            else
                                                MSTSEREntity.Year = propYear;


                                            MSTSEREntity.AppNo = strLAppNo;
                                            MSTSEREntity.Service = strserv;
                                            MSTSEREntity.AddOperator = BaseForm.UserID;
                                            MSTSEREntity.LstcOperator = BaseForm.UserID;
                                            _model.CaseMstData.InsertUpdateDelMSTSER(MSTSEREntity);
                                        }
                                    }
                                }

                                // Custom Questions

                                DataTable dtcustomQues = PIPDATA.GETPIPCUSTORRESPORADDCUST(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, strUserId, string.Empty, "ADDCUSTRESP");
                                if (dtcustomQues.Rows.Count > 0)
                                {

                                    foreach (DataRow drcustom in dtcustomQues.Rows)
                                    {
                                        CustomQuestionsEntity custEntity = new CustomQuestionsEntity();

                                        custEntity.ACTAGENCY = propAgency;
                                        custEntity.ACTDEPT = propDept;
                                        custEntity.ACTPROGRAM = propProgram;
                                        if (propYear == string.Empty)
                                            custEntity.ACTYEAR = "    ";
                                        else
                                            custEntity.ACTYEAR = propYear;
                                        custEntity.ACTAPPNO = strLAppNo;
                                        if (drcustom["ADDCUST_SEQ"].ToString() == "0")
                                        {
                                            custEntity.ACTSNPFAMILYSEQ = "1";
                                        }
                                        else
                                        { custEntity.ACTSNPFAMILYSEQ = drcustom["ADDCUST_SEQ"].ToString(); }

                                        custEntity.ACTCODE = drcustom["ADDCUST_CODE"].ToString();

                                        custEntity.ACTMULTRESP = drcustom["ADDCUST_MULT_RESP"].ToString();

                                        custEntity.ACTNUMRESP = drcustom["ADDCUST_NUM_RESP"].ToString();


                                        custEntity.ACTDATERESP = drcustom["ADDCUST_DATE_RESP"].ToString();

                                        custEntity.ACTALPHARESP = drcustom["ADDCUST_ALPHA_RESP"].ToString();

                                        custEntity.Mode = string.Empty;

                                        custEntity.addoperator = BaseForm.UserID;
                                        custEntity.lstcoperator = BaseForm.UserID;
                                        _model.CaseMstData.InsertUpdateADDCUST(custEntity);
                                    }

                                }

                                SnpEntity.Agency = propAgency;
                                SnpEntity.Dept = propDept;
                                SnpEntity.Program = propProgram;
                                if (propYear == string.Empty)
                                    SnpEntity.Year = "    ";
                                else
                                    SnpEntity.Year = propYear;

                                SnpEntity.FamilySeq = "1";
                                SnpEntity.App = strApplNo;
                                // SnpEntity.ClientId = strClientIdOut;
                                SnpEntity.Type = "MST";
                                if (dr["PIP_SSN"].ToString() == string.Empty)
                                {
                                    if (mstdata.Ssn.ToString().Trim() == "000000000")
                                        SnpEntity.Ssno = strSSNNOOut;
                                    else
                                        SnpEntity.Ssno = mstdata.Ssn;
                                }
                                else
                                {
                                    SnpEntity.Ssno = dr["PIP_SSN"].ToString();
                                }

                                SnpEntity.NameixFi = dr["PIP_FNAME"].ToString();
                                SnpEntity.NameixMi = dr["PIP_MNAME"].ToString();
                                SnpEntity.NameixLast = dr["PIP_LNAME"].ToString();
                                SnpEntity.AltBdate = dr["PIP_DOB"].ToString();
                                SnpEntity.Sex = dr["PIP_GENDER"].ToString();
                                SnpEntity.MaritalStatus = dr["PIP_MARITAL_STATUS"].ToString();
                                SnpEntity.MemberCode = dr["PIP_MEMBER_CODE"].ToString();
                                SnpEntity.Ethnic = dr["PIP_ETHNIC"].ToString();
                                SnpEntity.Race = dr["PIP_RACE"].ToString();
                                SnpEntity.Education = dr["PIP_EDUCATION"].ToString();
                                SnpEntity.Disable = dr["PIP_DISABLE"].ToString();
                                SnpEntity.WorkStatus = dr["PIP_WORK_STAT"].ToString();
                                SnpEntity.SchoolDistrict = dr["PIP_SCHOOL"].ToString();
                                SnpEntity.HealthIns = dr["PIP_HEALTH_INS"].ToString();
                                SnpEntity.Vet = dr["PIP_VETERAN"].ToString();
                                SnpEntity.FootStamps = dr["PIP_FOOD_STAMP"].ToString();
                                SnpEntity.Farmer = dr["PIP_FARMER"].ToString();
                                SnpEntity.Wic = dr["PIP_WIC"].ToString();
                                SnpEntity.Relitran = dr["PIP_RELITRAN"].ToString();
                                SnpEntity.Drvlic = dr["PIP_DRVLIC"].ToString();
                                SnpEntity.MilitaryStatus = dr["PIP_MILITARY_STATUS"].ToString();

                                SnpEntity.Pregnant = dr["PIP_PREGNANT"].ToString();
                                SnpEntity.Health_Codes = dr["PIP_HEALTH_CODES"].ToString();
                                SnpEntity.NonCashBenefits = dr["PIP_NCASHBEN"].ToString();
                                // SnpEntity.Youth = dr["PIP_YOUTH"].ToString();
                                SnpEntity.AddOperator = BaseForm.UserID;
                                SnpEntity.LstcOperator = BaseForm.UserID;
                                SnpEntity.DobNa = "0";
                                SnpEntity.Status = "A";
                                SnpEntity.Exclude = "N";
                                

                                SnpEntity.SnpSuffix = dr["PIP_SUFFIX"].ToString();
                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RESIDENTCODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.Resident = commondata.Code;
                                    }
                                }
                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.Youth = commondata.Code;
                                    }
                                }

                                //Added by Sudheer on 06/20/24 based on the document 'CSEOP Intake Portal notes from EOP testers'
                                //if (BaseForm.BaseAgencyControlDetails.AgyShortName.Trim() == "CSEOP")
                                //{
                                    if (!string.IsNullOrEmpty(SnpEntity.AltBdate.Trim()))
                                    {
                                        string Age = string.Empty;
                                        Age=CommonFunctions.CalculationYear(Convert.ToDateTime(SnpEntity.AltBdate.Trim()), Convert.ToDateTime(mstdata.IntakeDate.Trim()));

                                        if (Convert.ToInt32(Age) > 24)
                                        {
                                            SnpEntity.Youth = "2";
                                        }

                                    }
                                //}

                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBTITLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.JobTitle = commondata.Code;
                                    }
                                }
                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBCATEGORY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.JobCategory = commondata.Code;
                                    }
                                }

                                SnpEntity.Mode = "Add";


                                if (SnpEntity.Ssno.Trim() == string.Empty)
                                {
                                    SnpEntity.Ssno = "000000000";
                                }

                                string strOutFamilyseq = SnpEntity.FamilySeq;
                                if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                {

                                    PIPDATA.InsertUpdatePIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, propAgency, propDept, propProgram, propYear, strApplNo, strOutFamilyseq, dr["PIP_AGENCY"].ToString(), dr["PIP_AGY"].ToString(), dr["PIP_REG_ID"].ToString(), dr["PIP_ID"].ToString(), BaseForm.UserID, "A",string.Empty);
                                    if (dr["PIP_INCOME_TYPES"].ToString().Trim() != string.Empty)
                                    {
                                        string[] strINCOMETypes = dr["PIP_INCOME_TYPES"].ToString().Split(',');
                                        foreach (string strIncomeType in strINCOMETypes)
                                        {
                                            if (strIncomeType.ToString() != string.Empty)
                                            {
                                                CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                incomeupdatedetails.Agency = propAgency;
                                                incomeupdatedetails.Dept = propDept;
                                                incomeupdatedetails.Program = propProgram;
                                                incomeupdatedetails.App = strApplNo;
                                                incomeupdatedetails.Year = propYear;
                                                incomeupdatedetails.FamilySeq = strOutFamilyseq;
                                                incomeupdatedetails.Type = strIncomeType;
                                                incomeupdatedetails.Interval = "E";
                                                incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                incomeupdatedetails.Mode = string.Empty;
                                                incomeupdatedetails.Exclude = "N";
                                                incomeupdatedetails.Factor = "1.00";


                                                if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                {
                                                }
                                            }
                                        }
                                    }
                                    UpdateLeanIntakeData(dr["PIP_REG_ID"].ToString(), strLAppNo, string.Empty, string.Empty, string.Empty,"C");
                                }
                            }
                        }
                        else
                        {
                            if (boolSnpInsert && strLAppNo != string.Empty)
                            {

                                SnpEntity.Agency = propAgency;
                                SnpEntity.Dept = propDept;
                                SnpEntity.Program = propProgram;
                                if (propYear == string.Empty)
                                    SnpEntity.Year = "    ";
                                else
                                    SnpEntity.Year = propYear;

                                SnpEntity.NameixFi = dr["PIP_FNAME"].ToString();
                                SnpEntity.NameixMi = dr["PIP_MNAME"].ToString();
                                SnpEntity.NameixLast = dr["PIP_LNAME"].ToString();
                                SnpEntity.AltBdate = dr["PIP_DOB"].ToString();
                                SnpEntity.Sex = dr["PIP_GENDER"].ToString();
                                SnpEntity.Ssno = dr["PIP_SSN"].ToString();
                                SnpEntity.MaritalStatus = dr["PIP_MARITAL_STATUS"].ToString();
                                SnpEntity.MemberCode = dr["PIP_MEMBER_CODE"].ToString();
                                SnpEntity.Ethnic = dr["PIP_ETHNIC"].ToString();
                                SnpEntity.Race = dr["PIP_RACE"].ToString();
                                SnpEntity.Education = dr["PIP_EDUCATION"].ToString();
                                SnpEntity.Disable = dr["PIP_DISABLE"].ToString();
                                SnpEntity.WorkStatus = dr["PIP_WORK_STAT"].ToString();
                                SnpEntity.SchoolDistrict = dr["PIP_SCHOOL"].ToString();
                                SnpEntity.HealthIns = dr["PIP_HEALTH_INS"].ToString();
                                SnpEntity.Vet = dr["PIP_VETERAN"].ToString();
                                SnpEntity.FootStamps = dr["PIP_FOOD_STAMP"].ToString();
                                SnpEntity.Farmer = dr["PIP_FARMER"].ToString();
                                SnpEntity.Wic = dr["PIP_WIC"].ToString();
                                SnpEntity.Relitran = dr["PIP_RELITRAN"].ToString();
                                SnpEntity.Drvlic = dr["PIP_DRVLIC"].ToString();
                                SnpEntity.MilitaryStatus = dr["PIP_MILITARY_STATUS"].ToString();

                                SnpEntity.Pregnant = dr["PIP_PREGNANT"].ToString();
                                SnpEntity.Health_Codes = dr["PIP_HEALTH_CODES"].ToString();
                                SnpEntity.NonCashBenefits = dr["PIP_NCASHBEN"].ToString();
                                //  SnpEntity.Youth = dr["PIP_YOUTH"].ToString();
                                SnpEntity.AddOperator = BaseForm.UserID;
                                SnpEntity.LstcOperator = BaseForm.UserID;
                                SnpEntity.DobNa = "0";
                                SnpEntity.Status = "A";
                                SnpEntity.Exclude = "N";
                                SnpEntity.App = strLAppNo;
                                SnpEntity.SnpSuffix = dr["PIP_SUFFIX"].ToString();
                              List<CommonEntity>  commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RESIDENTCODES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.Resident = commondata.Code;
                                    }
                                }
                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DisconnectedYouth, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.Youth = commondata.Code;
                                    }
                                }

                                //Added by Sudheer on 06/20/24 based on the document 'CSEOP Intake Portal notes from EOP testers'
                                //if (BaseForm.BaseAgencyControlDetails.AgyShortName.Trim() == "CSEOP")
                                //{
                                    if (!string.IsNullOrEmpty(SnpEntity.AltBdate.Trim()))
                                    {
                                        string Age = string.Empty;
                                        Age = CommonFunctions.CalculationYear(Convert.ToDateTime(SnpEntity.AltBdate.Trim()), Convert.ToDateTime(mstdata.IntakeDate.Trim()));

                                        if (Convert.ToInt32(Age) > 24)
                                        {
                                            SnpEntity.Youth = "2";
                                        }

                                    }
                                //}

                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBTITLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.JobTitle = commondata.Code;
                                    }
                                }
                                commontype = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.JOBCATEGORY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "ADD"); //_model.lookupDataAccess.GetReliableTrans();
                                if (commontype.Count > 0)
                                {
                                    CommonEntity commondata = commontype.Find(u => u.Default.Equals("Y"));
                                    if (commondata != null)
                                    {
                                        SnpEntity.JobCategory = commondata.Code;
                                    }
                                }
                                SnpEntity.Type = "SNP";
                                SnpEntity.ClientId = string.Empty;
                                //if (BottomGrid.Rows.Count > 0)
                                //{
                                //    SnpEntity.ClientId = string.Empty;
                                //    foreach (DataGridViewRow gvBottomRow in BottomGrid.Rows)
                                //    {

                                //        if (SnpEntity.Ssno.ToString().Trim() != string.Empty)
                                //        {
                                //            if (gvBottomRow.Cells["gvtBotSSNMain"].Value.ToString() == SnpEntity.Ssno.ToString())
                                //            {
                                //                SnpEntity.ClientId = gvBottomRow.Cells["gvtBotClientId"].Value.ToString();
                                //                break;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            string DOB;
                                //            if (!string.IsNullOrEmpty(gvBottomRow.Cells["gvtBotDOB"].Value.ToString()))
                                //                DOB = Convert.ToDateTime(gvBottomRow.Cells["gvtBotDOB"].Value.ToString()).ToShortDateString();
                                //            else
                                //                DOB = "01/01/1900";

                                //            if (dr["PIP_FNAME"].ToString().Trim() == gvBottomRow.Cells["gvtFirstName"].Value.ToString() && Convert.ToDateTime(dr["PIP_DOB"].ToString()).ToShortDateString() == DOB)
                                //            {
                                //                SnpEntity.Ssno = gvBottomRow.Cells["gvtBotSSNMain"].Value.ToString();
                                //                SnpEntity.ClientId = gvBottomRow.Cells["gvtBotClientId"].Value.ToString();
                                //                break;
                                //            }
                                //        }

                                //    }
                                //}
                                //else
                                //{

                                //    SnpEntity.ClientId = string.Empty;
                                //}
                                if (SnpEntity.Ssno.Trim() == string.Empty)
                                {
                                    SnpEntity.Ssno = "000000000";
                                }
                                string strOutFamilyseq = SnpEntity.FamilySeq;
                                if (_model.CaseMstData.InsertSNPDETAILSLeanIntake(SnpEntity, out strOutFamilyseq))
                                {
                                    PIPDATA.InsertUpdatePIPCAPLNK(BaseForm.BaseLeanDataBaseConnectionString, propAgency, propDept, propProgram, propYear, strApplNo, strOutFamilyseq, dr["PIP_AGENCY"].ToString(), dr["PIP_AGY"].ToString(), dr["PIP_REG_ID"].ToString(), dr["PIP_ID"].ToString(), BaseForm.UserID, "A",string.Empty);


                                    if (dr["PIP_INCOME_TYPES"].ToString().Trim() != string.Empty)
                                    {
                                        string[] strINCOMETypes = dr["PIP_INCOME_TYPES"].ToString().Split(',');
                                        foreach (string strIncomeType in strINCOMETypes)
                                        {
                                            if (strIncomeType.ToString() != string.Empty)
                                            {
                                                CaseIncomeEntity incomeupdatedetails = new CaseIncomeEntity();
                                                incomeupdatedetails.Agency = propAgency;
                                                incomeupdatedetails.Dept = propDept;
                                                incomeupdatedetails.Program = propProgram;
                                                incomeupdatedetails.App = strApplNo;
                                                incomeupdatedetails.Year = propYear;
                                                incomeupdatedetails.FamilySeq = strOutFamilyseq;
                                                incomeupdatedetails.Type = strIncomeType;
                                                incomeupdatedetails.Interval = "E";
                                                incomeupdatedetails.AddOperator = BaseForm.UserID;
                                                incomeupdatedetails.LstcOperator = BaseForm.UserID;
                                                incomeupdatedetails.Mode = string.Empty;
                                                incomeupdatedetails.Exclude = "N";
                                                incomeupdatedetails.Factor = "1.00";

                                                if (_model.CaseMstData.InsertCaseIncome(incomeupdatedetails))
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (boolSnpInsert)
                {
                    if (strLAppNo != string.Empty)
                    {
                        if (strFormLoad == string.Empty)
                        {
                            propApplicantNumber = strLAppNo;
                            BaseForm.BaseAgency = propAgency;
                            BaseForm.BaseDept = propDept;
                            BaseForm.BaseProg = propProgram;
                            BaseForm.BaseYear = propYear == string.Empty ? "    " : propYear;
                            BaseForm.BaseApplicationNo = strLAppNo;
                            BaseForm.BasePIPDragSwitch = "Y";
                        }
                        this.DialogResult = DialogResult.OK;
                        this.Close();


                        // CommonFunctions.MessageBoxDisplay("Saved as Applicant# "+ strLAppNo + " in CAPTAIN");
                    }
                }
                // }
            }

        }

        private bool GetDup_APP_MEM_Status(string ssnnumber)
        {
            bool Can_Drag = true;
            string SSn = string.Empty, Error = string.Empty, strReasonflag = "N";
            if (!string.IsNullOrEmpty(ssnnumber))
            {
                SSn = ssnnumber;
                ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(propAgency, propDept, propProgram);
                if (programEntity != null)
                    strReasonflag = programEntity.SSNReasonFlag.ToString();

                if (SSn == "000000000" && strReasonflag == "Y")
                {
                    Can_Drag = true;
                }
                else
                {
                    DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuOtherPrograms(SSn, propAgency + propDept + propProgram + (!(string.IsNullOrEmpty(propYear)) ? propYear : "    "), BaseForm.UserID, "AddApplicant", null, null, null,null);
                    DataTable dt2;
                    if (ds1.Tables.Count > 0)
                    {
                        dt2 = ds1.Tables[0];
                        if (dt2.Rows.Count > 0)
                        {

                            if (dt2.Rows[0]["APP_MEM"].ToString() == "A")
                                Error = "Applicant already exists in  " + propAgency + "-" + propDept + "-" + propProgram + " " + propYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   cannot Copy/Drag";
                            else
                                Error = "Member already exists in " + propAgency + "-" + propDept + "-" + propProgram + " " + propYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   cannot Copy/Drag";

                            MessageBox.Show(Error, "CAP Systems");
                            Can_Drag = false;
                        }
                    }
                }
            }

            return Can_Drag;
        }


        string propClientRulesSwitch { get; set; }
        private void FillTopGridData(string strFName, string strLName, string strSSN, string strDob, string strAGY)
        {
            string DateofBirth = null;

            if (strDob != string.Empty)
                DateofBirth = strDob;


            DataSet ds = new DataSet();

            string strClientSwitch = string.Empty;
            if (propClientRulesSwitch == "Y")
            {
                if (BaseForm.BaseAgencyControlDetails != null)
                    strClientSwitch = BaseForm.BaseAgencyControlDetails.ClientRules;
            }

            ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchLeanIntake("APP", "ALL", null, null, null, strLName, strFName, strSSN, null, null, null, null, null, null, null, null, string.Empty, BaseForm.UserID, strClientSwitch, strAGY);

            if (ds.Tables[0].Rows.Count == 0)
            {
                if (strSSN != string.Empty)
                {
                    ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchLeanIntake("APP", "ALL", null, null, null, string.Empty, string.Empty, strSSN, null, null, null, null, null, null, null, null, strDob, BaseForm.UserID, strClientSwitch, strAGY);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchLeanIntake("APP", "ALL", null, null, null, string.Empty, strFName, string.Empty, null, null, null, null, null, null, null, null, strDob, BaseForm.UserID, strClientSwitch, strAGY);
                    }
                }
                else
                {
                    ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchLeanIntake("APP", "ALL", null, null, null, string.Empty, strFName, string.Empty, null, null, null, null, null, null, null, null, strDob, BaseForm.UserID, strClientSwitch, strAGY);
                }
            }

            TopGrid.SelectionChanged -= new EventHandler(TopGrid_SelectionChanged);
            TopGrid.Rows.Clear();
            BottomGrid.Rows.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                int TmpRows = 0;
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        string TmpHierarchy = null, DOB = null, Snp_Lstc_Date = null;
                        string Address = null;
                        string TmpAddress = null;
                        string TmpSsn = null;
                        int TmpLength = 0;
                        char TmpSpace = ' ';
                        string TmpYear = "    ";
                        string TmpName = "    ";
                        string Mst_Key = null;

                        //if (BaseForm.BaseAgencyControlDetails.ClientRules == "Y" && propClientRulesSwitch == "Y")
                        //{
                        //    List<CaseSnpEntity> snpalldata = new List<CaseSnpEntity>();
                        //    int intsearchcount = 0;
                        //    int SSNPoint = 0, DOBpoint = 0, FNamePoint = 0, LastNamePoint = 0, DOBLNamePoint = 0, SSNLNamePoint = 0, DOBFNamePoint = 0;
                        //    if (BaseForm.BaseAgencyControlDetails.SSNPoint != string.Empty)
                        //        SSNPoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.SSNPoint);
                        //    if (BaseForm.BaseAgencyControlDetails.DOBPoint != string.Empty)
                        //        DOBpoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.DOBPoint);
                        //    if (BaseForm.BaseAgencyControlDetails.FirstNamePoint != string.Empty)
                        //        FNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.FirstNamePoint);
                        //    if (BaseForm.BaseAgencyControlDetails.LastNamePoint != string.Empty)
                        //        LastNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.LastNamePoint);
                        //    if (BaseForm.BaseAgencyControlDetails.DOBLastNamePoint != string.Empty)
                        //        DOBLNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.DOBLastNamePoint);
                        //    //added by Sudheer on 02/21/2018
                        //    if (BaseForm.BaseAgencyControlDetails.DOBFirstNamePoint != string.Empty)
                        //        DOBFNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.DOBFirstNamePoint);

                        //    if (BaseForm.BaseAgencyControlDetails.SSNLastNamePoint != string.Empty)
                        //        SSNLNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.SSNLastNamePoint);
                        //    int intSearchhit = 0;
                        //    int intsearchrating = 0;
                        //    if (BaseForm.BaseAgencyControlDetails.SearchHit == "3")
                        //    {
                        //        if (BaseForm.BaseAgencyControlDetails.SearchRating != string.Empty)
                        //            intsearchrating = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.SearchRating);
                        //    }
                        //    string SsnSwitch, NameSwitch, DobSwitch, LastNameSwitch;
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        intsearchcount = 0;
                        //        SsnSwitch = NameSwitch = LastNameSwitch = DobSwitch = "N";
                        //        //if (MtxtSsn.Text.Trim() == dr["SSN"].ToString())
                        //        //{
                        //        //    SsnSwitch = "Y";
                        //        //    intsearchcount = SSNPoint;
                        //        //}
                        //        //if (DtDOB.Checked && dr["DOB"].ToString() != string.Empty)
                        //        //{
                        //        //    if (Convert.ToDateTime(DateofBirth) == Convert.ToDateTime(dr["DOB"].ToString()))
                        //        //    {
                        //        //        intsearchcount = intsearchcount + DOBpoint;
                        //        //        DobSwitch = "Y";
                        //        //    }
                        //        //    if ((dr["LName"].ToString().ToUpper() == TxtLName.Text.ToUpper()) && (Convert.ToDateTime(DateofBirth) == Convert.ToDateTime(dr["DOB"].ToString())))
                        //        //    {
                        //        //        intsearchcount = intsearchcount + DOBLNamePoint;
                        //        //        if (BaseForm.BaseAgencyControlDetails.SearchHit == "2")
                        //        //            intSearchhit = intSearchhit + 1;
                        //        //    }
                        //        //    //added by sudheer on 02/21/2018
                        //        //    if ((dr["FName"].ToString().ToUpper() == TxtFName.Text.ToUpper()) && (Convert.ToDateTime(DateofBirth) == Convert.ToDateTime(dr["DOB"].ToString())))
                        //        //    {
                        //        //        intsearchcount = intsearchcount + DOBFNamePoint;
                        //        //        if (BaseForm.BaseAgencyControlDetails.SearchHit == "4")
                        //        //            intSearchhit = intSearchhit + 1;
                        //        //    }

                        //        //}
                        //        //if (dr["FName"].ToString().ToUpper() == TxtFName.Text.ToUpper())
                        //        //{
                        //        //    NameSwitch = "Y";
                        //        //    intsearchcount = intsearchcount + FNamePoint;
                        //        //}
                        //        //if (dr["LName"].ToString().ToUpper() == TxtLName.Text.ToUpper())
                        //        //{
                        //        //    LastNameSwitch = "Y";
                        //        //    intsearchcount = intsearchcount + LastNamePoint;
                        //        //}
                        //        //if ((dr["LName"].ToString().ToUpper() == TxtLName.Text.ToUpper()) && (MtxtSsn.Text.Trim() == dr["SSN"].ToString()))
                        //        //{
                        //        //    intsearchcount = intsearchcount + SSNLNamePoint;
                        //        //    if (BaseForm.BaseAgencyControlDetails.SearchHit == "1")
                        //        //        intSearchhit = intSearchhit + 1;
                        //        //}
                        //        //if (BaseForm.BaseAgencyControlDetails.SearchHit == "3")
                        //        //{
                        //        //    if (intsearchrating < intsearchcount)
                        //        //        intSearchhit = intSearchhit + 1;
                        //        //}

                        //        snpalldata.Add(new CaseSnpEntity(dr, "MAINMENUSEARCH", intsearchcount, SsnSwitch, NameSwitch, LastNameSwitch, DobSwitch));
                        //    }
                        //    snpalldata = snpalldata.OrderByDescending(u => u.searchAppCount).ThenByDescending(u => Convert.ToDateTime(u.DateLstc)).ToList();
                        //    bool Can_Add_Cur_Hie = false;
                        //    foreach (CaseSnpEntity snpdata in snpalldata)
                        //    {

                        //        int rowIndex = 0;
                        //        DOB = Snp_Lstc_Date = " ";

                        //        TmpSsn = snpdata.Ssno.ToString();
                        //        TmpLength = (9 - TmpSsn.Length);
                        //        for (int i = 0; i < TmpLength; i++)
                        //            TmpAddress += TmpSpace;
                        //        TmpSsn += TmpAddress;
                        //        TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", "N", string.Empty);
                        //        TmpHierarchy = snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString();    //RecKey
                        //        TmpYear = "    ";
                        //        TmpName = null;


                        //        Mst_Key = null;
                        //        Mst_Key = snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString() + (snpdata.Year.ToString() == string.Empty ? "    " : snpdata.Year.ToString()) + snpdata.Appdetails.ToString();

                        //        if (!string.IsNullOrEmpty(snpdata.AltBdate.ToString()))
                        //            DOB = Convert.ToDateTime(snpdata.AltBdate.ToString()).ToShortDateString();

                        //        if (!string.IsNullOrEmpty(snpdata.DateLstc.ToString()))
                        //            Snp_Lstc_Date = snpdata.DateLstc.ToString();
                        //        else
                        //            Snp_Lstc_Date = "01/01/1900";

                        //        // TmpName = LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat.ToString());

                        //        var lstcdatetime = Convert.ToDateTime(Snp_Lstc_Date).Date;

                        //         Can_Add_Cur_Hie = Get_Hi_Access_Status(TmpHierarchy);
                        //        rowIndex = TopGrid.Rows.Add(TmpHierarchy, snpdata.Year, snpdata.Appdetails, TmpSsn, snpdata.NameixFi.ToString(), DOB, lstcdatetime.Date, " ", Mst_Key, snpdata.FamilySeq, snpdata.Ssno.ToString(), snpdata.searchAppCount, snpdata.NameixLast.ToString(), (Can_Add_Cur_Hie== true?"Y":"N"));
                        //        TopGrid.Rows[rowIndex].Tag = snpdata;
                        //        if (snpdata.SnpAcitveStatus.ToString().Trim() != "A")// && (dr["AppNo"].ToString()).Substring(10, 1) == "A")  
                        //            TopGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        //        if (snpdata.SsnSwitch == "Y")
                        //            TopGrid.Rows[rowIndex].Cells[3].Style.ForeColor = Color.Green;
                        //        if (snpdata.DobSwitch == "Y")
                        //            TopGrid.Rows[rowIndex].Cells[5].Style.ForeColor = Color.Green;
                        //        if (snpdata.NameSwitch == "Y")
                        //            TopGrid.Rows[rowIndex].Cells[4].Style.ForeColor = Color.Green;
                        //        if (snpdata.LastNameSwitch == "Y")
                        //            TopGrid.Rows[rowIndex].Cells[12].Style.ForeColor = Color.Green;

                        //        //if (propFormType == "ADSEARCH")
                        //        //{
                        //        //    string strAppno = snpdata.Appdetails.ToString();
                        //        //    if (strAppno.Length > 8)
                        //        //    {
                        //        //        strAppno = strAppno.Substring(0, 8);
                        //        //    }
                        //        //    if (propHiearchy.Trim() == TmpHierarchy.Trim() && propYear.Trim() == snpdata.Year.ToString().Trim() && propApp.Trim() == strAppno.Trim())
                        //        //    {
                        //        //        TopGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                        //        //    }
                        //        //}

                        //        TmpRows++;
                        //    }
                        //    if (TmpRows > 0)
                        //    {
                        //        Loading_Complete = true;
                        //        if (TopGrid.Rows.Count > 0)
                        //        {

                        //            TopGrid.CurrentCell = TopGrid.Rows[0].Cells[0];
                        //            TopGrid.Rows[0].Selected = true;
                        //            GetHie_App_Details();

                        //        }


                        //    }

                        //}
                        //else
                        //{

                        bool Can_Add_Cur_Hie = false;
                        bool boolAllHIEACCESS = false;
                        if (string.IsNullOrEmpty(string.Empty))
                            boolAllHIEACCESS = Get_Hi_Access_Status(string.Empty);
                        foreach (DataRow dr in dt.Rows)
                        {
                            int rowIndex = 0;
                            DOB = Snp_Lstc_Date = " ";

                            TmpSsn = dr["Ssn"].ToString();
                            TmpLength = (9 - TmpSsn.Length);
                            for (int i = 0; i < TmpLength; i++)
                                TmpAddress += TmpSpace;
                            TmpSsn += TmpAddress;
                            TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", "N", string.Empty);
                            TmpHierarchy = dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();    //RecKey
                            TmpYear = "    ";
                            TmpName = null;

                            //if (dr["Fname"].ToString().Trim().Length > 0)
                            //    TmpName = dr["Fname"].ToString().Trim();
                            //if (dr["Lname"].ToString().Trim().Length > 0)
                            //{
                            //    if (!(string.IsNullOrEmpty(TmpName)))
                            //        TmpName += ", ";
                            //    TmpName += dr["Lname"].ToString().Trim();
                            //}

                            Mst_Key = null;
                            Mst_Key = dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString() + (dr["SnpYear"].ToString() == string.Empty ? "    " : dr["SnpYear"].ToString()) + dr["AppNo"].ToString();

                            if (!string.IsNullOrEmpty(dr["DOB"].ToString()))
                                DOB = Convert.ToDateTime(dr["DOB"].ToString()).ToString("MM/dd/yyyy");

                            if (!string.IsNullOrEmpty(dr["SNP_DATE_LSTC"].ToString()))
                                Snp_Lstc_Date = dr["SNP_DATE_LSTC"].ToString();
                            else
                                Snp_Lstc_Date = "01/01/1900";

                            TmpName = LookupDataAccess.GetMemberName(dr["Fname"].ToString(), dr["MName"].ToString(), dr["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat.ToString());

                            var lstcdatetime = Convert.ToDateTime(Snp_Lstc_Date).Date;
                            if (!boolAllHIEACCESS)
                                Can_Add_Cur_Hie = Get_Hi_Access_Status(TmpHierarchy);
                            else
                                Can_Add_Cur_Hie = true;
                            rowIndex = TopGrid.Rows.Add(TmpHierarchy, dr["SnpYear"], dr["AppNo"], TmpSsn, TmpName, DOB, Convert.ToDateTime(lstcdatetime.Date).ToString("MM/dd/yyyy"), " ", Mst_Key, dr["RecFamSeq"], dr["Ssn"].ToString(), (Can_Add_Cur_Hie == true ? "Y" : "N"));


                            TopGrid.Rows[rowIndex].Tag = dr;
                            if (dr["AppStatus"].ToString().Trim() != "A")// && (dr["AppNo"].ToString()).Substring(10, 1) == "A")  
                                TopGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            //if (propFormType == "ADSEARCH")
                            //{
                            //    string strAppno = dr["AppNo"].ToString();
                            //    if (strAppno.Length > 8)
                            //    {
                            //        strAppno = strAppno.Substring(0, 8);
                            //    }
                            //    if (propHiearchy.Trim() == TmpHierarchy.Trim() && propYear.Trim() == dr["SnpYear"].ToString().Trim() && propApp.Trim() == strAppno.Trim())
                            //    {
                            //        TopGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                            //    }
                            //}

                            TmpRows++;
                        }
                        if (TmpRows > 0)
                        {
                            TopGrid.Sort(TopGrid.Columns["gvtTopLstdate"], ListSortDirection.Descending);
                            Loading_Complete = true;
                            if (TopGrid.Rows.Count > 0)
                            {
                                TopGrid.CurrentCell = TopGrid.Rows[0].Cells[0];
                                TopGrid.Rows[0].Selected = true;

                                //string strMemberData = TopGrid.Rows[0].Cells["SnpKey"].Value.ToString() + TopGrid.Rows[0].Cells["SnpFamSeq"].Value.ToString();
                                GetHie_App_Details();

                            }
                            LblHeader.Text = "Client is in the programs/hierarchies listed below ";

                            //if(AddPriv)
                            //    BtnDragApp.Visible = true;
                            ////////AppDetailsPanel.Visible = true;
                            //////btnAppliciant.Visible = true;
                        }
                        //}
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                // MessageBox.Show("No Record(s) Exists with selected Criteria", "CAP Systems"); 

            }
            TopGrid.SelectionChanged += new EventHandler(TopGrid_SelectionChanged);

        }

        bool Loading_Complete = false;
        string[,] Gender_Desc, Mem_Desc;
        private void Fill_Gender_MemberCode_Desc_List()
        {
            List<CommonEntity> Gender = _model.lookupDataAccess.GetGender();
            List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();
            Gender_Desc = new string[Gender.Count, 2];
            Mem_Desc = new string[Relation.Count, 2];

            int i = 0;
            foreach (CommonEntity gender in Gender)
            {
                Gender_Desc[i, 0] = gender.Code;
                Gender_Desc[i, 1] = gender.Desc;
                i++;
            }
            i = 0;
            foreach (CommonEntity Rel in Relation)
            {
                Mem_Desc[i, 0] = Rel.Code;
                Mem_Desc[i, 1] = Rel.Desc;
                i++;
            }

        }

        private void GetHie_App_Details()
        {

            if (Loading_Complete)
            {
                if (TopGrid.SelectedRows[0].Selected == true)
                {
                    DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuGetHouseDetails(TopGrid.SelectedRows[0].Cells["gvtTopSnpKey"].Value.ToString() + TopGrid.SelectedRows[0].Cells["gvtTopSnpFamSeq"].Value.ToString());
                    DataTable dt1 = ds1.Tables[0];
                    _propCasesnpsubdt = dt1;
                    //GetDup_APP_MEM_Status();

                    BottomGrid.Rows.Clear();
                    try
                    {
                        int TmpRows = 0;
                        string TmpName = null;
                        string TmpAddress = null;
                        string TmpDOB = null;
                        string TmpUpdated = null;
                        string TmpSsn = null;
                        int TmpLength = 0;
                        char TmpSpace = ' ';
                        string Tme_Gender = null, Tmp_Relation = null;

                        foreach (DataRow dr in dt1.Rows)
                        {
                            int rowIndex = 0;

                            //TmpName = dr["Fname"] + ", " + dr["Lname"] + "  " + dr["MName"];
                            TmpName = LookupDataAccess.GetMemberName(dr["Fname"].ToString(), dr["MName"].ToString(), dr["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat.ToString());

                            TmpSsn = dr["Ssn"].ToString();
                            TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", "N", string.Empty);
                            //TmpLength = (9 - TmpSsn.Length);
                            //for (int i = 0; i < TmpLength; i++)
                            //    TmpAddress += TmpSpace;
                            //TmpSsn += TmpAddress;
                            //TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);
                            TmpDOB = " ";

                            if (!string.IsNullOrEmpty(dr["Dob"].ToString()))
                                TmpDOB = Convert.ToDateTime(dr["Dob"].ToString()).ToString("MM/dd/yyyy");

                            //TmpDOB = dr["Dob"].ToString();
                            //TmpDOB = TmpDOB.Substring(4,2) + '/' + TmpDOB.Substring(1,2) + '/' + TmpDOB.Substring(7,2);
                            string[] time = Regex.Split(TmpDOB.ToString(), " ");
                            TmpDOB = time[0];
                            TmpUpdated = null;
                            TmpUpdated = Convert.ToDateTime(dr["Updated"]).ToString("MM/dd/yyyy");
                            time = null;
                            time = Regex.Split(TmpUpdated.ToString(), " ");
                            TmpUpdated = time[0];
                            //TmpUpdated = TmpUpdated.Substring(3, 2) + '/' + TmpUpdated.Substring(0, 2) + '/' + TmpUpdated.Substring(6, 2);

                            Tme_Gender = Tmp_Relation = null;


                            for (int i = 0; i < (Gender_Desc.Length / 2); i++)
                            {
                                if (Gender_Desc[i, 0] == dr["SNP_SEX"].ToString())
                                {
                                    Tme_Gender = Gender_Desc[i, 1];
                                    break;
                                }
                            }
                            for (int i = 0; i < (Mem_Desc.Length / 2); i++)
                            {
                                if (Mem_Desc[i, 0] == dr["SNP_MEMBER_CODE"].ToString())
                                {
                                    Tmp_Relation = Mem_Desc[i, 1];
                                    break;
                                }
                            }


                            if (TopGrid.SelectedRows[0].Cells["gvtTopSnpFamSeq"].Value.ToString() == dr["FamSeq"].ToString())
                                rowIndex = BottomGrid.Rows.Add(true, TmpName, TmpSsn, TmpDOB, Tmp_Relation, Tme_Gender, dr["SNP_AGE"].ToString(), TmpUpdated, " ", dr["FamSeq"].ToString(), dr["Ssn"].ToString(), dr["App_Mem_SW"].ToString(), dr["Ssn"].ToString(), dr["MST_FAMILY_ID"].ToString(), dr["SNP_CLIENT_ID"].ToString(), dr["Fname"].ToString());
                            else
                                rowIndex = BottomGrid.Rows.Add(false, TmpName, TmpSsn, TmpDOB, Tmp_Relation, Tme_Gender, dr["SNP_AGE"].ToString(), TmpUpdated, " ", dr["FamSeq"].ToString(), dr["Ssn"].ToString(), dr["App_Mem_SW"].ToString(), dr["Ssn"].ToString(), dr["MST_FAMILY_ID"].ToString(), dr["SNP_CLIENT_ID"].ToString(), dr["Fname"].ToString());

                            BottomGrid.Rows[rowIndex].Tag = dr;

                            if (dr["App_Mem_SW"].ToString().Trim() == "A")
                                BottomGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                            if (dr["SNP_STATUS"].ToString().Trim() != "A")//|| dr["SNP_STATUS"].ToString() != "i")
                                BottomGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;


                            TmpRows++;
                        }

                        if (TmpRows > 0)
                            BottomGrid.Rows[0].Tag = 0;
                    }
                    catch (Exception ex) { }
                }

            }
        }




        private void TopGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (TopGrid.Rows.Count > 0)
            {
                GetHie_App_Details();
            }
        }

        public CaseMstEntity propCaseMstEntity { get; set; }
        public List<CaseSnpEntity> propCaseSnpEntity { get; set; }

        private void btnUpdateselected_Click(object sender, EventArgs e)
        {
            //if (_propPIPData.Rows.Count > 0 && _propCasesnpsubdt.Rows.Count > 0)
            //{
            //    PIPUpdateApplicantForm pipupdateForm = new Forms.PIPUpdateApplicantForm(_propPIPData, _propCasesnpsubdt, BaseForm, _propCasesnpsubdt.Rows[0]["SNP_AGENCY"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_DEPT"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_PROGRAM"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_YEAR"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_APP"].ToString());
            //    pipupdateForm.FormClosed += new FormClosedEventHandler(PipupdateForm_FormClosed);
            //    pipupdateForm.ShowDialog();
            //}
        }

        private void PipupdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //PIPUpdateApplicantForm form = sender as PIPUpdateApplicantForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    //  GetHie_App_Details();
            //    //TopGrid.SelectedRows[0].Cells["gvtTopApp"].Value.ToString() + TopGrid.SelectedRows[0].Cells["gvtTopSnpFamSeq"].Value.ToString();
            //    propApplicantNumber = TopGrid.SelectedRows[0].Cells["gvtTopApp"].Value.ToString();
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //    //this.Close();
            //}
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            //PipCustomquestionsForm pipcustom = new PipCustomquestionsForm(BaseForm, string.Empty, _propCasesnpsubdt.Rows[0]["SNP_AGENCY"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_DEPT"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_PROGRAM"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_YEAR"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_APP"].ToString(), propLeanUserId, propLeanServices, string.Empty);
            //pipcustom.StartPosition = FormStartPosition.CenterScreen;
            //pipcustom.ShowDialog();
        }

        private void TopGrid_DoubleClick(object sender, EventArgs e)
        {
            if (TopGrid.Rows.Count > 0)
            {
                if (_propPIPData.Rows.Count > 0 && _propCasesnpsubdt.Rows.Count > 0)
                {
                    if (strPIPRegSubmitSwitch == string.Empty)
                    {
                        if (TopGrid.SelectedRows[0].Cells["gvtPermisson"].Value == "Y")
                        {
                            string strCloseFormstatus = string.Empty;
                            if (strFormLoad != string.Empty)
                                strCloseFormstatus = "Y";
                            //PIPUpdateApplicantForm pipupdateForm = new Forms.PIPUpdateApplicantForm(_propPIPData, _propCasesnpsubdt, BaseForm, _propCasesnpsubdt.Rows[0]["SNP_AGENCY"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_DEPT"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_PROGRAM"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_YEAR"].ToString(), _propCasesnpsubdt.Rows[0]["SNP_APP"].ToString(), strCloseFormstatus);
                            //pipupdateForm.FormClosed += new FormClosedEventHandler(PipupdateForm_FormClosed);
                            //pipupdateForm.StartPosition = FormStartPosition.CenterScreen;
                            //pipupdateForm.ShowDialog();
                        }
                        else
                        {
                            CommonFunctions.MessageBoxDisplay("Client is not in a program you are authorized to access !!!");
                        }
                    }

                }
            }
        }

        private void PIPntakeSearchForm_Load(object sender, EventArgs e)
        {
            if (strFormLoad != string.Empty)
                btnSearch_Click(btnSearch, new EventArgs());
        }

        private void btnEmailSend_Click(object sender, EventArgs e)
        {
            // string strConformNum = dataGridCaseSnp.SelectedRows[0].Cells["gvtConformNum"].Value == null ? string.Empty : dataGridCaseSnp.SelectedRows[0].Cells["gvtConformNum"].Value.ToString();
            PrivilegeEntity privileges = new PrivilegeEntity();
            privileges.Program = "PIPINTAKESearch";
            List<PIPDocEntity> propDocentitylist = new List<PIPDocEntity>();
            DataTable dtPIPIntake = new DataTable();
            //PIP0001SendMailForm form = new PIP0001SendMailForm(BaseForm, privileges, txtTokenNumber.Text, txtEmailId.Text, propDocentitylist, dtPIPIntake, _propRegid);
            //// form.FormClosed += new Wisej.Web.Form.FormClosedEventHandler(objform_FormClosed);
            //form.ShowDialog();
        }

        private void AddLeanIntakeData()
        {
            foreach (DataGridViewRow dritem in gvwCustomer.Rows)
            {
                DataRow dr = dritem.Tag as DataRow;
                if (dr != null)
                {
                    CaseMstEntity mstdata = new CaseMstEntity();
                    CaseSnpEntity SnpEntity = new CaseSnpEntity();
                    if (dr["PIP_FAM_SEQ"].ToString() == "0")
                    {
                        mstdata.ApplAgency = propAgency;
                        mstdata.ApplDept = propDept;
                        mstdata.ApplProgram = propProgram;
                        if (propYear == string.Empty)
                            mstdata.ApplYr = "    ";
                        else
                            mstdata.ApplYr = propYear;
                        mstdata.FamilySeq = "1";

                        mstdata.Email = dr["PIP_EMAIL"].ToString();
                        mstdata.Ssn = dr["PIP_SSN"].ToString();
                        mstdata.Language = dr["PIP_PRI_LANGUAGE"].ToString();
                        mstdata.FamilyType = dr["PIP_FAMILY_TYPE"].ToString();
                        mstdata.Housing = dr["PIP_HOUSING"].ToString();
                        // mstdata.snpSchoolDistrict = dr["PIP_"].ToString();
                        mstdata.IncomeTypes = dr["PIP_INCOME_TYPES"].ToString();
                        //  mstdata.AddressDetails = dr["PIP_ADDRESS"].ToString();
                        mstdata.Area = dr["PIP_AREA"].ToString();
                        mstdata.Phone = dr["PIP_HOME_PHONE"].ToString();
                        mstdata.CellPhone = dr["PIP_CELL_NUMBER"].ToString();
                        mstdata.Hn = dr["PIP_HOUSENO"].ToString();
                        mstdata.Direction = dr["PIP_DIRECTION"].ToString();
                        mstdata.Street = dr["PIP_STREET"].ToString();
                        mstdata.Suffix = dr["PIP_SUFFIX"].ToString();
                        mstdata.Apt = dr["PIP_APT"].ToString();
                        mstdata.Flr = dr["PIP_FLR"].ToString();
                        mstdata.City = dr["PIP_CITY"].ToString();
                        mstdata.State = dr["PIP_STATE"].ToString();
                        mstdata.Zip = dr["PIP_ZIP"].ToString();
                        mstdata.TownShip = dr["PIP_TOWNSHIP"].ToString();
                        mstdata.County = dr["PIP_COUNTY"].ToString();
                        mstdata.MstNCashBen = dr["PIP_NCASHBEN"].ToString();
                        mstdata.AddOperator1 = BaseForm.UserID;
                        mstdata.LstcOperator1 = BaseForm.UserID;
                        mstdata.ActiveStatus = "Y";
                        mstdata.Mode = "Add";
                        mstdata.Type = Consts.CASE2001.ManualType;



                        SnpEntity.Agency = propAgency;
                        SnpEntity.Dept = propDept;
                        SnpEntity.Program = propProgram;
                        if (propYear == string.Empty)
                            SnpEntity.Year = "    ";
                        else
                            SnpEntity.Year = propYear;

                        SnpEntity.FamilySeq = "1";
                        SnpEntity.App = strApplNo;
                        // SnpEntity.ClientId = strClientIdOut;
                        SnpEntity.Type = "MST";
                        SnpEntity.Ssno = dr["PIP_SSN"].ToString();

                        SnpEntity.NameixFi = dr["PIP_FNAME"].ToString();
                        SnpEntity.NameixMi = dr["PIP_MNAME"].ToString();
                        SnpEntity.NameixLast = dr["PIP_LNAME"].ToString();
                        SnpEntity.AltBdate = dr["PIP_DOB"].ToString();
                        SnpEntity.Sex = dr["PIP_GENDER"].ToString();
                        SnpEntity.MaritalStatus = dr["PIP_MARITAL_STATUS"].ToString();
                        SnpEntity.MemberCode = dr["PIP_MEMBER_CODE"].ToString();
                        SnpEntity.Ethnic = dr["PIP_ETHNIC"].ToString();
                        SnpEntity.Race = dr["PIP_RACE"].ToString();
                        SnpEntity.Education = dr["PIP_EDUCATION"].ToString();
                        SnpEntity.Disable = dr["PIP_DISABLE"].ToString();
                        SnpEntity.WorkStatus = dr["PIP_WORK_STAT"].ToString();
                        SnpEntity.SchoolDistrict = dr["PIP_SCHOOL"].ToString();
                        SnpEntity.HealthIns = dr["PIP_HEALTH_INS"].ToString();
                        SnpEntity.Vet = dr["PIP_VETERAN"].ToString();
                        SnpEntity.FootStamps = dr["PIP_FOOD_STAMP"].ToString();
                        SnpEntity.Farmer = dr["PIP_FARMER"].ToString();
                        SnpEntity.Wic = dr["PIP_WIC"].ToString();
                        SnpEntity.Relitran = dr["PIP_RELITRAN"].ToString();
                        SnpEntity.Drvlic = dr["PIP_DRVLIC"].ToString();
                        SnpEntity.MilitaryStatus = dr["PIP_MILITARY_STATUS"].ToString();

                        SnpEntity.Pregnant = dr["PIP_PREGNANT"].ToString();
                        SnpEntity.Health_Codes = dr["PIP_HEALTH_CODES"].ToString();
                        SnpEntity.NonCashBenefits = dr["PIP_NCASHBEN"].ToString();
                        // SnpEntity.Youth = dr["PIP_YOUTH"].ToString();
                        SnpEntity.AddOperator = BaseForm.UserID;
                        SnpEntity.LstcOperator = BaseForm.UserID;
                        SnpEntity.DobNa = "0";
                        SnpEntity.Status = "A";
                        SnpEntity.Exclude = "N";
                        SnpEntity.Mode = "Add";


                    }
                    else
                    {


                        SnpEntity.Agency = propAgency;
                        SnpEntity.Dept = propDept;
                        SnpEntity.Program = propProgram;
                        if (propYear == string.Empty)
                            SnpEntity.Year = "    ";
                        else
                            SnpEntity.Year = propYear;

                        SnpEntity.NameixFi = dr["PIP_FNAME"].ToString();
                        SnpEntity.NameixMi = dr["PIP_MNAME"].ToString();
                        SnpEntity.NameixLast = dr["PIP_LNAME"].ToString();
                        SnpEntity.AltBdate = dr["PIP_DOB"].ToString();
                        SnpEntity.Sex = dr["PIP_GENDER"].ToString();
                        SnpEntity.Ssno = dr["PIP_SSN"].ToString();
                        SnpEntity.MaritalStatus = dr["PIP_MARITAL_STATUS"].ToString();
                        SnpEntity.MemberCode = dr["PIP_MEMBER_CODE"].ToString();
                        SnpEntity.Ethnic = dr["PIP_ETHNIC"].ToString();
                        SnpEntity.Race = dr["PIP_RACE"].ToString();
                        SnpEntity.Education = dr["PIP_EDUCATION"].ToString();
                        SnpEntity.Disable = dr["PIP_DISABLE"].ToString();
                        SnpEntity.WorkStatus = dr["PIP_WORK_STAT"].ToString();
                        SnpEntity.SchoolDistrict = dr["PIP_SCHOOL"].ToString();
                        SnpEntity.HealthIns = dr["PIP_HEALTH_INS"].ToString();
                        SnpEntity.Vet = dr["PIP_VETERAN"].ToString();
                        SnpEntity.FootStamps = dr["PIP_FOOD_STAMP"].ToString();
                        SnpEntity.Farmer = dr["PIP_FARMER"].ToString();
                        SnpEntity.Wic = dr["PIP_WIC"].ToString();
                        SnpEntity.Relitran = dr["PIP_RELITRAN"].ToString();
                        SnpEntity.Drvlic = dr["PIP_DRVLIC"].ToString();
                        SnpEntity.MilitaryStatus = dr["PIP_MILITARY_STATUS"].ToString();

                        SnpEntity.Pregnant = dr["PIP_PREGNANT"].ToString();
                        SnpEntity.Health_Codes = dr["PIP_HEALTH_CODES"].ToString();
                        SnpEntity.NonCashBenefits = dr["PIP_NCASHBEN"].ToString();
                        // SnpEntity.Youth = dr["PIP_YOUTH"].ToString();
                        SnpEntity.AddOperator = BaseForm.UserID;
                        SnpEntity.LstcOperator = BaseForm.UserID;
                        SnpEntity.DobNa = "0";
                        SnpEntity.Status = "A";
                        SnpEntity.Exclude = "N";
                        SnpEntity.Type = "SNP";


                    }
                }
            }

        }
    }
}