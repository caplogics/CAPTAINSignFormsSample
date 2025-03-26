#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using DevExpress.DashboardCommon;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PIPAdmin : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public PIPAdmin(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strQuestionID, string strType)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            propCustomCode = strQuestionID;
            this.Text = Privileges.PrivilegeName;
            txtForcePassword.Validator = TextBoxValidation.IntegerValidator;
            propimagetypesCategory = _model.lookupDataAccess.GetImageNameConvention();

            propReportPath = _model.lookupDataAccess.GetReportPath();

            userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
            if (userHierarchy.Count > 0)
                userHierarchy = userHierarchy.FindAll(u => u.Agency == "**" && u.Dept == "**" && u.Prog == "**" && u.UsedFlag == "N");

            txtMessage.ReadOnly = true;
        }

        #region Properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string propType { get; set; }

        public string propCustomCode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> userHierarchy { get; set; }

        List<CommonEntity> propimagetypesCategory { get; set; }

        #endregion


        List<CaseELIGQUESEntity> propCust = new List<CaseELIGQUESEntity>();
        List<CustfldsEntity> propCustSingle = new List<CustfldsEntity>();
        private void FillCustomQuestions()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                //DataSet dsHierches = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("1", string.Empty, " ");
                //foreach (DataRow drhiechy in dsHierches.Tables[0].Rows)
                //{
                //    Cmbquestions.Items.Add(new ListItem(drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                //}
                if (dsPIPAgencies.Tables.Count > 0)
                {
                    foreach (DataRow drhiechy in dsPIPAgencies.Tables[0].Rows)
                    {
                        Cmbquestions.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                    }
                }
                //List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);

                if (userHierarchy.Count > 0)
                    Cmbquestions.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                // List<CustfldsEntity> Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
                propCust = _model.CaseSumData.GetFLDCUSTOMQUESPIP("00", "PIPCUS");
                List<ListItem> pipcusthie = GetPipCustHie("00");
                gvwResponses.Rows.Clear();
                if (propCust.Count > 0)
                {
                    //Cust = Cust.FindAll(u => u.EligFliedType == "A" || u.EligFliedType == "H");
                    propCust = propCust.OrderBy(u => u.EligQuesDesc).ToList();
                    gvtSeq.Visible = true;
                    bool boolselques = false;
                    bool boolActive = true;
                    int intIndex = 0;
                    string intSeq = "0";
                    if (propCust.Count > 0)
                    {
                        foreach (CaseELIGQUESEntity Entity in propCust)
                        {
                            boolselques = false;
                            boolActive = true;
                            if (chkShowAll.Checked)
                            {
                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.EligQuesCode);
                                if (listpiphie != null)
                                {
                                    boolselques = true;
                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;
                                    intSeq = listpiphie.ScreenCode;
                                }
                                else
                                {
                                    intSeq = (intIndex + 1).ToString();
                                }
                                intIndex = gvwResponses.Rows.Add(boolselques, Entity.EligQuesCode, Entity.EligQuesDesc, Entity.EligQuesCode, intSeq, boolActive);
                                gvwResponses.Rows[intIndex].Tag = Entity;
                            }
                            else
                            {
                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.EligQuesCode);
                                if (listpiphie != null)
                                {
                                    boolselques = true;
                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;

                                    intIndex = gvwResponses.Rows.Add(boolselques, Entity.EligQuesCode, Entity.EligQuesDesc, Entity.EligQuesCode, listpiphie.ScreenCode, boolActive);
                                    gvwResponses.Rows[intIndex].Tag = Entity;
                                }

                            }
                        }
                    }
                    lblShowcount.Text = pipcusthie.Count.ToString() + "/" + propCust.Count.ToString();

                }
            }
            else
            {
                gvwResponses.Rows.Clear();
                propCustSingle = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
                List<ListItem> pipcusthie = GetPipCustHie("00");
                if (propCustSingle.Count > 0)
                {
                    //Cust = Cust.FindAll(u => u.MemAccess == "A" || u.MemAccess == "H");
                    propCustSingle = propCustSingle.OrderBy(u => u.CustDesc).ToList();

                    if (propCustSingle.Count > 0)
                    {
                        int intIndex = 0;
                        bool boolselques = false;
                        bool boolActive = true;
                        string intSeq = "0";
                        foreach (CustfldsEntity Entity in propCustSingle)
                        {

                            boolselques = false;
                            boolActive = true;
                            if (chkShowAll.Checked)
                            {
                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.CustCode);
                                if (listpiphie != null)
                                {
                                    boolselques = true;
                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;
                                    intSeq = listpiphie.ScreenCode;
                                }
                                else
                                {
                                    intSeq = (intIndex + 1).ToString();
                                }
                                intIndex = gvwResponses.Rows.Add(boolselques, Entity.CustCode, Entity.CustDesc, Entity.CustCode, intSeq, boolActive);
                                gvwResponses.Rows[intIndex].Tag = Entity;
                            }
                            else
                            {
                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.CustCode);
                                if (listpiphie != null)
                                {
                                    boolselques = true;
                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;

                                    intIndex = gvwResponses.Rows.Add(boolselques, Entity.CustCode, Entity.CustDesc, Entity.CustCode, listpiphie.ScreenCode, boolActive);
                                    gvwResponses.Rows[intIndex].Tag = Entity;
                                }

                            }
                        }
                        lblShowcount.Text = pipcusthie.Count.ToString() + "/" + propCustSingle.Count.ToString();

                    }

                }
            }

            if (gvwResponses.Rows.Count > 0)
                gvwResponses.Rows[0].Selected = true;

            if (Cmbquestions.Items.Count > 0)
                Cmbquestions.SelectedIndex = 0;
            Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());
            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }


        private void FillCASEHIE()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            gvwResponses.Rows.Clear();


            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {

                if (dsPIPAgencies.Tables.Count > 0)
                {
                    foreach (DataRow drhiechy in dsPIPAgencies.Tables[0].Rows)
                    {
                        // Cmbquestions.Items.Add(new ListItem(drhiechy["PIPAGY_AGY"].ToString().Trim() + " - " + drhiechy["PIPAGY_DESC"].ToString(), drhiechy["PIPAGY_AGY"].ToString()));
                        Cmbquestions.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));

                    }
                }

                if (userHierarchy.Count > 0)
                    Cmbquestions.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                Cmbquestions.SelectedIndex = 0;

            }

            prophierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyDepartment(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "PROGRAM");

            if (prophierachyEntity.Count > 0)
            {
                prophierachyEntity = prophierachyEntity.OrderBy(u => u.HirarchyName).ToList();
                int intIndex;
                List<ListItem> pipservices = GetPipServiceHie("00");
                bool boolseldata = false;
                bool boolActive = true;
                foreach (HierarchyEntity HieEntity in prophierachyEntity)
                {
                    boolseldata = false;
                    boolActive = true;

                    if (chkShowAll.Checked)
                    {
                        ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == HieEntity.Code.Trim());
                        if (listpipservices != null)
                        {
                            boolseldata = true;
                            boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                        }
                        intIndex = gvwResponses.Rows.Add(boolseldata, HieEntity.Code, HieEntity.HirarchyName, HieEntity.Code, HieEntity.Code, boolActive);
                        gvwResponses.Rows[intIndex].Tag = HieEntity;
                    }
                    else
                    {
                        if (pipservices.Count > 0)
                        {
                            ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == HieEntity.Code.Trim());
                            if (listpipservices != null)
                            {
                                boolseldata = true;
                                boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                intIndex = gvwResponses.Rows.Add(boolseldata, HieEntity.Code, HieEntity.HirarchyName, HieEntity.Code, HieEntity.Code, boolActive);
                                gvwResponses.Rows[intIndex].Tag = HieEntity;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                lblShowcount.Text = pipservices.Count.ToString() + "/" + prophierachyEntity.Count.ToString();

            }

            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }

        private void FillCAMAST()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            gvwResponses.Rows.Clear();

            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {

                if (dsPIPAgencies.Tables.Count > 0)
                {
                    foreach (DataRow drhiechy in dsPIPAgencies.Tables[0].Rows)
                    {
                        //  Cmbquestions.Items.Add(new ListItem(drhiechy["PIPAGY_AGY"].ToString().Trim() + " - " + drhiechy["PIPAGY_DESC"].ToString(), drhiechy["PIPAGY_AGY"].ToString()));
                        Cmbquestions.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));

                    }
                }

                if (userHierarchy.Count > 0)
                    Cmbquestions.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                Cmbquestions.SelectedIndex = 0;
            }
            DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("CAMAST", "00", string.Empty, string.Empty, string.Empty, string.Empty);
            propserviceDT = serviceDS.Tables[0];

            if (propserviceDT.Rows.Count > 0)
            {
                int intIndex;
                List<ListItem> pipservices = GetPipServiceHie("00");
                bool boolseldata = false;
                bool boolActive = true;

                foreach (DataRow dr in propserviceDT.Rows)
                {
                    boolseldata = false;
                    boolActive = true;

                    if (chkShowAll.Checked)
                    {
                        ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                        if (listpipservices != null)
                        {

                            boolseldata = true;
                            boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                        }
                        intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                        gvwResponses.Rows[intIndex].Tag = dr;
                    }
                    else
                    {
                        if (pipservices.Count > 0)
                        {
                            ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                            if (listpipservices != null)
                            {

                                boolseldata = true;
                                boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                gvwResponses.Rows[intIndex].Tag = dr;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                lblShowcount.Text = pipservices.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();

            }
            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }

        private void FillCASESER()
        {
            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            gvwResponses.Rows.Clear();


            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {

                if (dsPIPAgencies.Tables.Count > 0)
                {
                    foreach (DataRow drhiechy in dsPIPAgencies.Tables[0].Rows)
                    {
                        //  Cmbquestions.Items.Add(new ListItem(drhiechy["PIPAGY_AGY"].ToString().Trim() + " - " + drhiechy["PIPAGY_DESC"].ToString(), drhiechy["PIPAGY_AGY"].ToString()));
                        Cmbquestions.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                    }
                }

                if (userHierarchy.Count > 0)
                    Cmbquestions.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                Cmbquestions.SelectedIndex = 0;
            }

            DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("CASESER", "00", string.Empty, string.Empty, string.Empty, string.Empty);
            propserviceDT = serviceDS.Tables[0];


            if (propserviceDT.Rows.Count > 0)
            {
                int intIndex;
                List<ListItem> pipservices = GetPipServiceHie("00");
                bool boolseldata = false;
                bool boolActive = true;
                foreach (DataRow dr in propserviceDT.Rows)
                {
                    boolseldata = false;
                    boolActive = true;

                    if (chkShowAll.Checked)
                    {
                        ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                        if (listpipservices != null)
                        {
                            boolseldata = true;
                            boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                        }
                        intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                        gvwResponses.Rows[intIndex].Tag = dr;
                    }
                    else
                    {
                        if (pipservices.Count > 0)
                        {
                            ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                            if (listpipservices != null)
                            {
                                boolseldata = true;
                                boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                gvwResponses.Rows[intIndex].Tag = dr;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                lblShowcount.Text = pipservices.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();

            }
            Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
        }

        public List<HierarchyEntity> prophierachyEntity = new List<HierarchyEntity>();
        DataTable propserviceDT;
        private void FillServicesData(string strAgency)
        {
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                gvwResponses.Rows.Clear();
                lblShowcount.Text = string.Empty;
                if (propType == "CASEHIE")
                {
                    prophierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyDepartment(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "PROGRAM");

                    if (prophierachyEntity.Count > 0)
                    {
                        if (strAgency != "00")
                            prophierachyEntity = prophierachyEntity.FindAll(u => u.Agency == strAgency);
                        prophierachyEntity = prophierachyEntity.OrderBy(u => u.HirarchyName).ToList();
                        int intIndex;
                        List<ListItem> pipservices = GetPipServiceHie(strAgency);
                        bool boolseldata = false;
                        bool boolActive = true;
                        foreach (HierarchyEntity HieEntity in prophierachyEntity)
                        {
                            boolseldata = false;
                            boolActive = true;

                            if (chkShowAll.Checked)
                            {
                                ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == HieEntity.Code.Trim());
                                if (listpipservices != null)
                                {
                                    boolseldata = true;
                                    boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                                }
                                intIndex = gvwResponses.Rows.Add(boolseldata, HieEntity.Code, HieEntity.HirarchyName, HieEntity.Code, HieEntity.Code, boolActive);
                                gvwResponses.Rows[intIndex].Tag = HieEntity;
                            }
                            else
                            {
                                ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == HieEntity.Code.Trim());
                                if (listpipservices != null)
                                {
                                    boolseldata = true;
                                    boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                    intIndex = gvwResponses.Rows.Add(boolseldata, HieEntity.Code, HieEntity.HirarchyName, HieEntity.Code, HieEntity.Code, boolActive);
                                    gvwResponses.Rows[intIndex].Tag = HieEntity;
                                }
                            }
                        }
                        lblShowcount.Text = pipservices.Count.ToString() + "/" + prophierachyEntity.Count.ToString();


                    }
                }
                if (propType == "CAMAST")
                {
                    DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("CAMAST", strAgency, string.Empty, string.Empty, string.Empty, string.Empty);
                    propserviceDT = serviceDS.Tables[0];


                    if (propserviceDT.Rows.Count > 0)
                    {
                        int intIndex;
                        List<ListItem> pipservices = GetPipServiceHie(strAgency);
                        bool boolseldata = false;
                        bool boolActive = true;
                        foreach (DataRow dr in propserviceDT.Rows)
                        {
                            boolseldata = false;
                            boolActive = true;
                            if (chkShowAll.Checked)
                            {
                                ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                if (listpipservices != null)
                                {
                                    boolseldata = true;
                                    boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                                }
                                intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                gvwResponses.Rows[intIndex].Tag = dr;
                            }
                            else
                            {
                                ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                if (listpipservices != null)
                                {
                                    boolseldata = true;
                                    boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                    intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                    gvwResponses.Rows[intIndex].Tag = dr;
                                }

                            }

                        }
                        lblShowcount.Text = pipservices.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();

                    }
                }
                if (propType == "CASESER")
                {
                    DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("CASESER", strAgency, string.Empty, string.Empty, string.Empty, string.Empty);
                    propserviceDT = serviceDS.Tables[0];


                    if (propserviceDT.Rows.Count > 0)
                    {
                        int intIndex;
                        List<ListItem> pipservices = GetPipServiceHie(strAgency);
                        bool boolseldata = false;
                        bool boolActive = true;
                        foreach (DataRow dr in propserviceDT.Rows)
                        {
                            boolseldata = false;
                            boolActive = true;
                            if (chkShowAll.Checked)
                            {
                                ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                if (listpipservices != null)
                                {
                                    boolseldata = true;
                                    boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                                }
                                intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                gvwResponses.Rows[intIndex].Tag = dr;
                            }
                            else
                            {
                                ListItem listpipservices = pipservices.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                if (listpipservices != null)
                                {
                                    boolseldata = true;
                                    boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                    intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                    gvwResponses.Rows[intIndex].Tag = dr;
                                }

                            }

                        }
                        lblShowcount.Text = pipservices.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();

                    }
                }
            }
        }

        private void Fill_AgyType_Combo()
        {


            Cmbquestions.SelectedIndexChanged -= Cmbquestions_SelectedIndexChanged;
            Cmbquestions.Items.Clear();
            gvwResponses.Rows.Clear();
            //Cmbquestions.Items.Insert(0, new ListItem("Select One", "0"));
            string Tmp_Desc = string.Empty;

            List<CommonEntity> lookupagyhdtabs = _model.lookupDataAccess.GetAgyTabsHeaders("PIPHD", string.Empty, string.Empty, "AGYHEADERS");

            if (lookupagyhdtabs.Count > 0)
            {
                foreach (CommonEntity dr in lookupagyhdtabs)
                {

                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-50}", dr.Desc.Trim()) + String.Format("{0,8}", " - " + dr.Code.ToString());

                    Cmbquestions.Items.Add(new ListItem(Tmp_Desc, dr.Code));

                }

                if (propCustomCode != string.Empty)
                {
                    List<CommonEntity> lookupagytabs = _model.lookupDataAccess.GetAgyTabs("LEAGY", propCustomCode, string.Empty);
                    if (lookupagytabs.Count > 0)
                    {
                        int intIndex;
                        foreach (CommonEntity agytabsEntity in lookupagytabs)
                        {
                            intIndex = gvwResponses.Rows.Add(false, agytabsEntity.Code, agytabsEntity.Desc, agytabsEntity.Code, agytabsEntity.Code);
                            gvwResponses.Rows[intIndex].Tag = agytabsEntity;
                        }
                    }
                }

                if (Cmbquestions.Items.Count > 0)
                    Cmbquestions.SelectedIndex = 0;
                Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());

                Cmbquestions.SelectedIndexChanged += Cmbquestions_SelectedIndexChanged;
            }
        }


        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(Cmbquestions, null);


            if (propType == "CUSTOM")
            {
                //if (Btn_Cancel.Text == "Cancel")
                //{
                //    //chkPipselectall.Enabled = false;  
                //    Btn_Cancel.Text = "Close"; btnMoveData.Enabled = true; cmbPipType.Enabled = Cmbquestions.Enabled = true;
                //}
                //else
                //{
                this.Close();
                // }
            }
            else
            {
                this.Close();

            }
        }

        //private void Btn_Save_Click(object sender, EventArgs e)
        //{
        //    if (ValidateCustControls())
        //    {
        //        if (propType == "CUSTOM")
        //        {
        //            //CustfldsEntity CustDetails = new CustfldsEntity();

        //            //CustDetails.UpdateType = "S";
        //            //CustDetails.Alpha = null;
        //            //CustDetails.Other = null;
        //            //CustDetails.CustCode = ((ListItem)Cmbquestions.SelectedItem).Value.ToString();

        //            //CustDetails.Equalto = "0";
        //            //CustDetails.Greater = "0";
        //            //CustDetails.Less = "0";
        //            //propCustomCode = ((ListItem)Cmbquestions.SelectedItem).Value.ToString();
        //            ////CustDetails.Active = chkSentPip.Checked == true ? "Y" : "N";
        //            ////CustDetails.FutureDate = chkActive.Checked == true ? "A" : "I";

        //            //string New_CUST_Code_Code = propCustomCode;

        //            //if (_model.FieldControls.UpdateCUSTFLDS(CustDetails, out New_CUST_Code_Code))
        //            //{

        //            //    propCustomCode = "";

        //            //    FillCustomQuestions();

        //            //    Btn_Save.Enabled = gvwResponses.Enabled = chkPipselectall.Enabled = false; Pb_Edit_Cust.Visible = true; cmbPipType.Enabled = Cmbquestions.Enabled = true; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";
        //            //}
        //        }
        //        else if (propType == "CASEHIE")
        //        {
        //            foreach (DataGridViewRow item in gvwResponses.Rows)
        //            {

        //                HierarchyEntity hierchyEntity = item.Tag as HierarchyEntity;

        //                // string strDesc = item.Cells["gvtSDesc"].Value == null ? string.Empty : item.Cells["gvtSDesc"].Value.ToString();
        //                string strSendPIP = item.Cells["gvtCheckPIP"].Value == null ? string.Empty : item.Cells["gvtCheckPIP"].Value.ToString();
        //                string strPIPActive = item.Cells["gvtCheckStatus"].Value == null ? string.Empty : item.Cells["gvtCheckStatus"].Value.ToString();
        //                // if (!string.IsNullOrEmpty(strDesc))
        //                // {
        //                hierchyEntity.Mode = "PIP";
        //                hierchyEntity.HirarchyName = string.Empty;
        //                hierchyEntity.Intake = strSendPIP.ToUpper() == "TRUE" ? "Y" : "N";
        //                hierchyEntity.HIERepresentation = strPIPActive.ToUpper() == "TRUE" ? "Y" : "N";
        //                _model.HierarchyAndPrograms.InsertUpdateHierarchy(hierchyEntity);
        //                // }

        //            }
        //            propCustomCode = string.Empty;
        //            propCustomCode = "";
        //            // txtQDescrption.Text = string.Empty;                   
        //            FillCASEHIE();
        //            cmbPipType.Enabled = true;
        //            Btn_Save.Enabled = false; gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = true; Pb_Edit_Cust.Visible = true; Cmbquestions.Enabled = false; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";

        //        }
        //        else if (propType == "CAMAST")
        //        {
        //            foreach (DataGridViewRow item in gvwResponses.Rows)
        //            {

        //                CAMASTEntity camastEntity = item.Tag as CAMASTEntity;


        //                string strSendPIP = item.Cells["gvtCheckPIP"].Value == null ? string.Empty : item.Cells["gvtCheckPIP"].Value.ToString();
        //                string strPIPActive = item.Cells["gvtCheckStatus"].Value == null ? string.Empty : item.Cells["gvtCheckStatus"].Value.ToString();
        //                //  if (!string.IsNullOrEmpty(strDesc))
        //                // {
        //                camastEntity.Mode = "PIP";
        //                camastEntity.Desc = string.Empty;
        //                camastEntity.AutoPost = strSendPIP.ToUpper() == "TRUE" ? "Y" : "N";
        //                camastEntity.Active = strPIPActive.ToUpper() == "TRUE" ? "Y" : "N";
        //                _model.SPAdminData.InsertCaMAST(camastEntity);

        //                // }
        //            }
        //            propCustomCode = string.Empty;
        //            propCustomCode = "";
        //            //  txtQDescrption.Text = string.Empty;                    
        //            FillCAMAST();
        //            cmbPipType.Enabled = true;
        //            Btn_Save.Enabled = false; gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = true; Pb_Edit_Cust.Visible = true; Cmbquestions.Enabled = false; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";
        //        }
        //        else
        //        {
        //            foreach (DataGridViewRow item in gvwResponses.Rows)
        //            {

        //                CommonEntity agytabsEntity = item.Tag as CommonEntity;
        //                //string strDesc = item.Cells["gvtSDesc"].Value == null ? string.Empty : item.Cells["gvtSDesc"].Value.ToString();
        //                //_model.Agytab.InsertUpdateAGYTABS("SPANISH", agytabsEntity.AgyCode, agytabsEntity.Code, string.Empty, string.Empty, string.Empty, string.Empty, strDesc);

        //            }
        //            propCustomCode = string.Empty;
        //            propCustomCode = "";
        //            Fill_AgyType_Combo();
        //            //if (Cmbquestions.Items.Count > 0)
        //            //    Cmbquestions.SelectedIndex = 0;
        //            Btn_Save.Enabled = false; gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = true; Pb_Edit_Cust.Visible = true; cmbPipType.Enabled = Cmbquestions.Enabled = true; btnMoveData.Enabled = true; Btn_Cancel.Text = "Close";

        //        }
        //    }


        //}

        //private void Pb_Edit_Cust_Click(object sender, EventArgs e)
        //{
        //    cmbPipType.Enabled = btnMoveData.Enabled = false; Cmbquestions.Enabled = false; Btn_Save.Enabled = chkPipselectall.Enabled = true; Pb_Edit_Cust.Visible = false; gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = false; Btn_Cancel.Text = "Cancel";
        //}

        private void Cmbquestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmbquestions.Items.Count > 0)
            {
                _errorProvider.SetError(Cmbquestions, null);
                btnCancel.Visible = false;
                if (propType == "CUSTOM")
                {
                    btnCopyAllQues.Visible = true;
                    btnCancel.Visible = true;
                    if (((ListItem)Cmbquestions.SelectedItem).Value.ToString() != string.Empty)
                    {
                        if (((ListItem)Cmbquestions.SelectedItem).Value.ToString() != "0")
                        {


                            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                            {
                                propCust = _model.CaseSumData.GetFLDCUSTOMQUESPIP(((ListItem)Cmbquestions.SelectedItem).Value.ToString(), "PIPCUS");
                                List<ListItem> pipcusthie = GetPipCustHie(((ListItem)Cmbquestions.SelectedItem).Value.ToString());
                                gvwResponses.Rows.Clear();
                                if (propCust.Count > 0)
                                {
                                    //Cust = Cust.FindAll(u => u.EligFliedType == "A" || u.EligFliedType == "H");
                                    propCust = propCust.OrderBy(u => u.EligQuesDesc).ToList();

                                    if (propCust.Count > 0)
                                    {
                                        bool boolselques = false;
                                        bool boolActive = true;
                                        int intIndex = 0;
                                        string intSeq = "0";
                                        foreach (CaseELIGQUESEntity Entity in propCust)
                                        {
                                            boolselques = false;
                                            boolActive = true;

                                            if (chkShowAll.Checked)
                                            {
                                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.EligQuesCode);
                                                if (listpiphie != null)
                                                {
                                                    boolselques = true;
                                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;
                                                    intSeq = listpiphie.ScreenCode;
                                                }
                                                else
                                                {
                                                    intSeq = (intIndex + 1).ToString();
                                                }
                                                intIndex = gvwResponses.Rows.Add(boolselques, Entity.EligQuesCode, Entity.EligQuesDesc, Entity.EligQuesCode, intSeq, boolActive);
                                                gvwResponses.Rows[intIndex].Tag = Entity;
                                            }
                                            else
                                            {
                                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.EligQuesCode);
                                                if (listpiphie != null)
                                                {
                                                    boolselques = true;
                                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;

                                                    intIndex = gvwResponses.Rows.Add(boolselques, Entity.EligQuesCode, Entity.EligQuesDesc, Entity.EligQuesCode, listpiphie.ScreenCode, boolActive);
                                                    gvwResponses.Rows[intIndex].Tag = Entity;
                                                }

                                            }
                                        }
                                        lblShowcount.Text = pipcusthie.Count.ToString() + "/" + propCust.Count.ToString();
                                    }

                                }
                            }
                            else
                            {


                                gvwResponses.Rows.Clear();
                                propCustSingle = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
                                if (propCustSingle.Count > 0)
                                {
                                    //Cust = Cust.FindAll(u => u.MemAccess == "A" || u.MemAccess == "H");
                                    propCustSingle = propCustSingle.OrderBy(u => u.CustDesc).ToList();

                                    if (propCustSingle.Count > 0)
                                    {
                                        List<ListItem> pipcusthie = GetPipCustHie("00");

                                        bool boolselques = false;
                                        bool boolActive = true;

                                        int intIndex = 0;
                                        string intSeq = "0";
                                        foreach (CustfldsEntity Entity in propCustSingle)
                                        {
                                            boolselques = false;
                                            boolActive = true;
                                            if (chkShowAll.Checked)
                                            {
                                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.CustCode);
                                                if (listpiphie != null)
                                                {
                                                    boolselques = true;
                                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;
                                                    intSeq = listpiphie.ScreenCode;
                                                }
                                                else
                                                {
                                                    intSeq = (intIndex + 1).ToString();
                                                }
                                                intIndex = gvwResponses.Rows.Add(boolselques, Entity.CustCode, Entity.CustDesc, Entity.CustCode, intSeq, boolActive);
                                                gvwResponses.Rows[intIndex].Tag = Entity;
                                            }
                                            else
                                            {
                                                ListItem listpiphie = pipcusthie.Find(u => u.Text == Entity.CustCode);
                                                if (listpiphie != null)
                                                {
                                                    boolselques = true;
                                                    boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;

                                                    intIndex = gvwResponses.Rows.Add(boolselques, Entity.CustCode, Entity.CustDesc, Entity.CustCode, listpiphie.ScreenCode, boolActive);
                                                    gvwResponses.Rows[intIndex].Tag = Entity;
                                                }

                                            }

                                        }
                                        lblShowcount.Text = pipcusthie.Count.ToString() + "/" + propCustSingle.Count.ToString();

                                    }

                                }
                            }
                        }
                        else
                        {
                            // txtQDescrption.Text = string.Empty;                            
                            gvwResponses.Rows.Clear();
                            // gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = true;

                        }
                    }
                }
                else if (propType == "CASEHIE" || propType == "CAMAST" || propType == "CASESER")
                {
                    btnCopyAllQues.Visible = true;
                    btnCancel.Visible = true;
                    FillServicesData(((ListItem)Cmbquestions.SelectedItem).Value.ToString());
                }
                else
                {
                    if (((ListItem)Cmbquestions.SelectedItem).Value.ToString() != "0")
                    {
                        gvwResponses.Visible = true;
                        gvwResponses.Rows.Clear();

                        List<CommonEntity> lookupagytabs = _model.lookupDataAccess.GetAgyTabspanish("LEAGY", ((ListItem)Cmbquestions.SelectedItem).Value.ToString(), string.Empty);
                        if (lookupagytabs.Count > 0)
                        {

                            int intIndex;
                            foreach (CommonEntity agytabsEntity in lookupagytabs)
                            {
                                intIndex = gvwResponses.Rows.Add(false, agytabsEntity.Code, agytabsEntity.Desc, agytabsEntity.Code, agytabsEntity.Code);
                                gvwResponses.Rows[intIndex].Tag = agytabsEntity;
                            }
                        }

                    }
                    else
                    {
                        gvwResponses.Rows.Clear();

                    }

                }
            }
            if (gvwResponses.Rows.Count > 0)
            {
                btnMoveData.Visible = true;
            }
            else
            {
                btnMoveData.Visible = false;
                btnCopyAllQues.Visible = false;
                btnCancel.Visible = false;
            }
        }


        private bool ValidateCustControls()
        {
            bool isValid = true;

            if (propType == "CUSTOM")
            {
                if ((Cmbquestions.SelectedItem == null || ((ListItem)Cmbquestions.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(Cmbquestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label4.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Cmbquestions, null);
                }


                //if ((String.IsNullOrEmpty(TxtQuesDesc.Text)))
                //{
                //    _errorProvider.SetError(TxtQuesDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label9.Text.Replace(Consts.Common.Colon, string.Empty)));
                //    isValid = false;
                //}
                //else
                //    _errorProvider.SetError(TxtQuesDesc, null);
            }
            else if (propType == "CAMAST")
            { }
            else if (propType == "CASEHIE")
            { }
            else
            {
                if ((Cmbquestions.SelectedItem == null || ((ListItem)Cmbquestions.SelectedItem).Text == Consts.Common.SelectOne))
                {
                    _errorProvider.SetError(Cmbquestions, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label4.Text));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Cmbquestions, null);
                }
            }

            return isValid;
        }

        private void btnMoveData_Click(object sender, EventArgs e)
        {
            if (propType != "AGYTABS")
            {
                List<DataGridViewRow> SelectedgvRows = (from c in gvwResponses.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvtCheckPIP"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();
                //if (SelectedgvRows.Count > 0)
                //{
                MessageBox.Show("Are you sure to copy from CAPTAIN to PIP database?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
                // }
                //else
                //{
                //    CommonFunctions.MessageBoxDisplay("Please select atleast one question");
                //}
            }
            else
            {
                MessageBox.Show("Are you sure to copy from CAPTAIN to PIP database?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);

            }
        }



        private void MessageBoxHandler(DialogResult dialogResult)
        {

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {

                if (propType == "CUSTOM")
                {
                    InsertPIPCUSTHIE();
                    chkShowAll_CheckedChanged(this, new EventArgs { });
                }
                else if ((propType == "CAMAST") || (propType == "CASEHIE") || (propType == "CASESER"))
                {
                    InsertPIPSERHIE();
                    chkShowAll_CheckedChanged(this, new EventArgs { });
                }
                else
                {
                    List<CommonEntity> lookupagytabs = _model.lookupDataAccess.GetAgyTabspanish("PIPAG", string.Empty, string.Empty);
                    if (lookupagytabs.Count > 0)
                    {
                        DeletePIPAgyTabs();
                        foreach (CommonEntity agytabsEntity in lookupagytabs)
                        {
                            InsertPIPAgyTabs(agytabsEntity.AgyCode, agytabsEntity.Code, agytabsEntity.Active, agytabsEntity.Desc, agytabsEntity.Default, agytabsEntity.Hierarchy);
                        }

                        AlertBox.Show("AGYTABS table copied to PIP successfully");
                    }
                }
            }

        }

        //int InsertPIPCustomeQuestions(string strLcode, string strSeq, string strActive, string strDesc, string strRespType, string strMemAccess, string strPIPACTIVE)
        //{
        //    int inti = 0;
        //    try
        //    {
        //        if (strDesc.Contains("'"))
        //            strDesc = strDesc.Replace("'", "''");

        //        SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.PIPCUSTQST (PCUST_AGENCY, PCUST_CODE,  PCUST_ACTIVE, PCUST_DESC, PCUST_RESP_TYPE, PCUST_MEM_ACCESS,  PCUST_DATE_LSTC, PCUST_LSTC_OPERATOR)VALUES('" + BaseForm.BaseAgencyControlDetails.AgyShortName + "', '" + strLcode + "','" + strActive + "','" + strDesc + "','" + strRespType + "','" + strMemAccess + "', GETDATE(),'" + BaseForm.UserID + "')", con))
        //        {
        //            inti = cmd.ExecuteNonQuery();
        //        }
        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return inti;

        //}


        void DeletePIPCustomeQuestions(string strLcode, string strapp)
        {

            try
            {
                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM PIPCUSTQST WHERE PCUST_AGENCY= '" + strLcode + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }

        }
        void DeletePIPCustResponses(string strLcode, string strapp)
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM PIPCUSTRESP WHERE PRSP_AGENCY= '" + strLcode + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {


            }

        }
        int InsertPIPCustResponses(SqlConnection con, string strLcode, string strSeq, string strDesc, string strRespCode, string Status)
        {

            int inti = 0;

            try
            {

                string stmt = "INSERT INTO [dbo].[PIPCUSTRESP]([PRSP_AGENCY],[PRSP_CUST_CODE],[PRSP_SEQ],[PRSP_RESP_CODE],[PRSP_DESC],[PRSP_DATE_LSTC],[PRSP_LSTC_OPERATOR],[PRSP_RESP_STATUS])VALUES(@PRSP_AGENCY,@PRSP_CUST_CODE,@PRSP_SEQ,@PRSP_RESP_CODE,@PRSP_DESC,GETDATE(),@PRSP_LSTC_OPERATOR,@PRSP_RESP_STATUS)";

                //if (strDesc.Contains("'"))
                //    strDesc = strDesc.Replace("'", "''");

                SqlCommand cmd = new SqlCommand(stmt, con);
                cmd.Parameters.Add("@PRSP_AGENCY", SqlDbType.NVarChar, 10);
                cmd.Parameters.Add("@PRSP_CUST_CODE", SqlDbType.NVarChar, 6);
                cmd.Parameters.Add("@PRSP_SEQ", SqlDbType.Decimal, 3);
                cmd.Parameters.Add("@PRSP_RESP_CODE", SqlDbType.VarChar, 6);
                cmd.Parameters.Add("@PRSP_DESC", SqlDbType.VarChar, 100);
                cmd.Parameters.Add("@PRSP_LSTC_OPERATOR", SqlDbType.VarChar, 20);
                cmd.Parameters.Add("@PRSP_RESP_STATUS", SqlDbType.Char, 1);

                cmd.Parameters["@PRSP_AGENCY"].Value = BaseForm.BaseAgencyControlDetails.AgyShortName;
                cmd.Parameters["@PRSP_CUST_CODE"].Value = strLcode;
                cmd.Parameters["@PRSP_SEQ"].Value = strSeq;
                cmd.Parameters["@PRSP_RESP_CODE"].Value = strRespCode;
                cmd.Parameters["@PRSP_DESC"].Value = strDesc;
                cmd.Parameters["@PRSP_RESP_STATUS"].Value = Status;

                cmd.Parameters["@PRSP_LSTC_OPERATOR"].Value = BaseForm.UserID;

                inti = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }
            return inti;

        }

        int InsertPIPAgyTabs(string strType, string strcode, string strActive, string strDesc, string strDefault, string strHierchy)
        {
            int inti = 0;
            try
            {
                //if (strDesc.Contains("'"))
                //    strDesc = strDesc.Replace("'", "''");

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                con.Open();

                string stmt = "INSERT INTO [dbo].[PIPAGYTABS]([AGYS_AGENCY],[AGYS_TYPE],[AGYS_CODE],[AGYS_DESC],[AGYS_ACTIVE],[AGYS_DEFAULT],[AGYS_HIERARCHY],[AGYS_UPDATED_ON],[AGYS_UPDATED_BY])VALUES(@AGYS_AGENCY,@AGYS_TYPE,@AGYS_CODE,@AGYS_DESC,@AGYS_ACTIVE,@AGYS_DEFAULT,@AGYS_HIERARCHY,GETDATE(),@AGYS_UPDATED_BY)";

                SqlCommand cmd = new SqlCommand(stmt, con);
                cmd.Parameters.Add("@AGYS_AGENCY", SqlDbType.NVarChar, 10);
                cmd.Parameters.Add("@AGYS_TYPE", SqlDbType.NVarChar, 5);
                cmd.Parameters.Add("@AGYS_CODE", SqlDbType.VarChar, 10);

                cmd.Parameters.Add("@AGYS_DESC", SqlDbType.VarChar, 100);
                cmd.Parameters.Add("@AGYS_ACTIVE", SqlDbType.VarChar, 1);
                cmd.Parameters.Add("@AGYS_DEFAULT", SqlDbType.VarChar, 1);
                cmd.Parameters.Add("@AGYS_HIERARCHY", SqlDbType.VarChar, 6);
                cmd.Parameters.Add("@AGYS_UPDATED_BY", SqlDbType.VarChar, 20);


                cmd.Parameters["@AGYS_AGENCY"].Value = BaseForm.BaseAgencyControlDetails.AgyShortName;
                cmd.Parameters["@AGYS_TYPE"].Value = strType;
                cmd.Parameters["@AGYS_CODE"].Value = strcode;
                cmd.Parameters["@AGYS_DESC"].Value = strDesc;
                cmd.Parameters["@AGYS_ACTIVE"].Value = strActive;
                cmd.Parameters["@AGYS_DEFAULT"].Value = strDefault;
                cmd.Parameters["@AGYS_HIERARCHY"].Value = strHierchy;
                cmd.Parameters["@AGYS_UPDATED_BY"].Value = BaseForm.UserID;

                inti = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return inti;

        }
        void DeletePIPAgyTabs()
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM PIPAGYTABS  WHERE AGYS_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            catch (Exception ex)
            {


            }

        }

        int InsertPIPServices(SqlConnection con, string strcode, string strDesc, string strType, string strPIPACTIVE)
        {
            int inti = 0;

            try
            {
                //if (strDesc.Contains("'"))
                //    strDesc = strDesc.Replace("'", "");



                //using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[PIPSERVICES]([PIPSER_AGENCY],[PIPSER_TYPE],[PIPSER_CODE],[PIPSER_DESC],[PIPSER_ACTIVE],[PIPSER_DATE_ADD],[PIPSER_ADDED_BY])VALUES('" + BaseForm.BaseAgencyControlDetails.AgyShortName + "','" + strType + "','" + strcode + "','" + strDesc + "','" + strPIPACTIVE + "',GETDATE(),'" + BaseForm.UserID + "')", con))
                //{
                //    inti = cmd.ExecuteNonQuery();
                //}


                string stmt = "INSERT INTO [dbo].[PIPSERVICES]([PIPSER_AGENCY],[PIPSER_TYPE],[PIPSER_CODE],[PIPSER_DESC],[PIPSER_ACTIVE],[PIPSER_DATE_ADD],[PIPSER_ADDED_BY])VALUES(@PIPSER_AGENCY,@PIPSER_TYPE,@PIPSER_CODE,@PIPSER_DESC,@PIPSER_ACTIVE,GETDATE(),@PIPSER_LSTC_OPERATOR)";



                SqlCommand cmd = new SqlCommand(stmt, con);
                cmd.Parameters.Add("@PIPSER_AGENCY", SqlDbType.NVarChar, 10);
                cmd.Parameters.Add("@PIPSER_TYPE", SqlDbType.NVarChar, 10);
                cmd.Parameters.Add("@PIPSER_CODE", SqlDbType.NVarChar, 10);
                cmd.Parameters.Add("@PIPSER_DESC", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@PIPSER_ACTIVE", SqlDbType.VarChar, 1);
                cmd.Parameters.Add("@PIPSER_LSTC_OPERATOR", SqlDbType.VarChar, 20);


                cmd.Parameters["@PIPSER_AGENCY"].Value = BaseForm.BaseAgencyControlDetails.AgyShortName;
                cmd.Parameters["@PIPSER_TYPE"].Value = strType;
                cmd.Parameters["@PIPSER_CODE"].Value = strcode;
                cmd.Parameters["@PIPSER_DESC"].Value = strDesc;
                cmd.Parameters["@PIPSER_ACTIVE"].Value = strPIPACTIVE;

                cmd.Parameters["@PIPSER_LSTC_OPERATOR"].Value = BaseForm.UserID;

                inti = cmd.ExecuteNonQuery();




            }
            catch (Exception ex)
            {


            }
            return inti;

        }
        void DeletePIPServices(string strType)
        {

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM PIPSERVICES  WHERE PIPSER_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPSER_TYPE = '" + strType + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand("UPDATE PIPAGENCY SET PIPAGY_SERVICE_TYPE ='" + strType + "'  WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY='00'", con))
                {
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            catch (Exception ex)
            {


            }

        }

        int CheckCustomQuestions(string strcode, string strType)
        {
            int inti = 0;

            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                if (strType == "CUSTOM")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Count(*) as count1  FROM PIPADDCUST WHERE ADDCUST_ACT_CODE ='" + strcode + "' AND ADDCUST_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "'", con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string strvalue1 = ds.Tables[0].Rows[0]["count1"].ToString();
                            if (strvalue1 != string.Empty)
                            {
                                inti = Convert.ToInt32(strvalue1);
                            }
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                inti = 0;

            }
            return inti;

        }


        List<ListItem> GetPipCustHie(string strHierchy)
        {

            List<ListItem> listpipcusthie = new List<ListItem>();
            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIPCUSTHIE WHERE PCH_AGY ='" + strHierchy + "' AND PCH_AGENCY ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drrow in ds.Tables[0].Rows)
                        {
                            listpipcusthie.Add(new ListItem(drrow["PCH_CUST_CODE"].ToString(), drrow["PCH_AGENCY"].ToString(), drrow["PCH_AGY"].ToString(), drrow["PCH_ACTIVE"].ToString(), drrow["PCH_SEQ"].ToString(), drrow["PCH_SEQ"].ToString()));
                        }
                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {


            }
            return listpipcusthie;

        }


        List<ListItem> GetPipServiceHie(string strHierchy)
        {

            List<ListItem> listpipserhie = new List<ListItem>();
            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIPSERHIE WHERE PSH_AGY ='" + strHierchy + "' AND PSH_AGENCY ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PSH_TYPE = '" + propType + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drrow in ds.Tables[0].Rows)
                        {
                            listpipserhie.Add(new ListItem(drrow["PSH_CODE"].ToString(), drrow["PSH_AGENCY"].ToString(), drrow["PSH_AGY"].ToString(), drrow["PSH_ACTIVE"].ToString()));
                        }
                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {


            }
            return listpipserhie;

        }


        //List<ListItem> GetPipServices(string strType)
        //{

        //    List<ListItem> listpipcusthie = new List<ListItem>();
        //    try
        //    {

        //        SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

        //        con.Open();

        //        using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIPSERVICES WHERE PIPSER_TYPE ='" + strType + "' AND PIPSER_AGENCY ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "'", con))
        //        {
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            da.Fill(ds);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow drrow in ds.Tables[0].Rows)
        //                {
        //                    listpipcusthie.Add(new ListItem(drrow["PIPSER_CODE"].ToString(), drrow["PIPSER_AGENCY"].ToString(), drrow["PIPSER_TYPE"].ToString(), drrow["PIPSER_ACTIVE"].ToString()));
        //                }
        //            }
        //        }

        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return listpipcusthie;

        //}



        List<ListItem> GetPipImageTypes()
        {

            List<ListItem> listpipimageTypes = new List<ListItem>();
            try
            {
                string strselectImgAgency = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (cmbImageTypeAgency.Items.Count > 0)
                    {
                        strselectImgAgency = ((ListItem)cmbImageTypeAgency.SelectedItem).Value.ToString();
                    }
                }

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIPIMGTYPES WHERE PIPIMGT_AGENCY ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPIMGT_AGY ='" + strselectImgAgency + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drrow in ds.Tables[0].Rows)
                        {
                            listpipimageTypes.Add(new ListItem(drrow["PIPIMGT_ID"].ToString(), drrow["PIPIMGT_IMG_TYPE"].ToString(), drrow["PIPIMGT_DESCRIPTION"].ToString(), drrow["PIPIMGT_AMH"].ToString(), drrow["PIPIMGT_IncTypes"].ToString(), drrow["PIPIMGT_AGENCY"].ToString(), drrow["PIPIMGT_REQUIRED"].ToString(), string.Empty));
                        }
                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {


            }
            return listpipimageTypes;

        }

        List<ListItem> GetPipFieldControlData()
        {

            List<ListItem> listpipFieldcontrols = new List<ListItem>();
            try
            {
                string strselectFieldAgency = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (cmbFieldAgency.Items.Count > 0)
                    {
                        strselectFieldAgency = ((ListItem)cmbFieldAgency.SelectedItem).Value.ToString();
                    }
                }

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIPFLDCNTL WHERE PIPFLD_ACTIVE ='Y' AND PIPFLD_AGENCY ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPFLD_AGY ='" + strselectFieldAgency + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drrow in ds.Tables[0].Rows)
                        {
                            listpipFieldcontrols.Add(new ListItem(drrow["PIPFLD_DESC"].ToString(), drrow["PIPFLD_CODE"].ToString(), drrow["PIPFLD_ACTIVE"].ToString(), drrow["PIPFLD_ENABLE"].ToString(), drrow["PIPFLD_REQUIRE"].ToString(), drrow["PIPFLD_SHARE"].ToString(), drrow["PIPFLD_AGY"].ToString(), drrow["PIPFLD_AGENCY"].ToString()));
                        }
                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {


            }
            return listpipFieldcontrols;

        }



        private void chkPipselectall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPipselectall.Checked)
            {
                foreach (DataGridViewRow gvitemrows in gvwResponses.Rows)
                {
                    gvitemrows.Cells["gvtCheckPIP"].Value = "True";
                }
            }
            else
            {
                foreach (DataGridViewRow gvitemrows in gvwResponses.Rows)
                {
                    gvitemrows.Cells["gvtCheckPIP"].Value = "False";
                }
            }
        }

        private void cmbPipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPipType.Items.Count > 0)
            {
                propType = ((ListItem)cmbPipType.SelectedItem).Value.ToString();

                chkPipselectall.Visible = false;

                //gvwResponses.Enabled = false;
                // gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = true;

                btnCopyAllQues.Visible = false;
                btnCancel.Visible = false;
                string strQuestionID = string.Empty;
                chkShowAll.Visible = false;
                lblShowcount.Text = string.Empty;
                gvtSeq.Visible = false;
                if (propType == "CUSTOM")
                {
                    chkShowAll.Visible = true;
                    btnCancel.Visible = true;
                    gvtCheckPIP.ReadOnly = gvtCheckStatus.ReadOnly = false;
                    btnMoveData.Text = "&SAVE";
                    btnMoveData.Size = new Size(77, 23);

                    btnCopyAllQues.Visible = true;

                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        label4.Visible = true;
                        label4.Text = "Agency";
                        Cmbquestions.Visible = true;

                    }
                    else
                    {
                        label4.Visible = false;
                        Cmbquestions.Visible = false;

                    }
                    gvtCheckPIP.Visible = true;
                    // gvtCheckStatus.Visible = true;
                    gvtDesc.Width = 550;


                    gvtSeq.Visible = true;
                    lblResponse.Text = "Questions";
                    FillCustomQuestions();
                }
                else if (propType == "CASEHIE")
                {
                    chkPipselectall.Visible = true;
                    chkShowAll.Visible = true;
                    lblResponse.Text = "Programs";
                    btnMoveData.Text = "&SAVE";
                    btnMoveData.Size = new Size(77, 23);
                    btnCopyAllQues.Visible = true;
                    btnCancel.Visible = true;
                    gvtCheckPIP.Visible = true;
                    gvtDesc.Width = 610;
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        label4.Visible = true;
                        label4.Text = "Agency";
                        Cmbquestions.Visible = true;

                    }
                    else
                    {
                        label4.Visible = false;
                        Cmbquestions.Visible = false;

                    }


                    FillCASEHIE();
                }
                else if (propType == "CASESER")
                {
                    chkPipselectall.Visible = true;
                    chkShowAll.Visible = true;
                    lblResponse.Text = "Programs";
                    btnMoveData.Text = "&SAVE";
                    btnMoveData.Size = new Size(77, 23);
                    btnCopyAllQues.Visible = true;
                    btnCancel.Visible = true;
                    gvtCheckPIP.Visible = true;
                    gvtDesc.Width = 610;
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        label4.Visible = true;
                        label4.Text = "Agency";
                        Cmbquestions.Visible = true;

                    }
                    else
                    {
                        label4.Visible = false;
                        Cmbquestions.Visible = false;

                    }

                    FillCASESER();
                }
                else if (propType == "CAMAST")
                {
                    chkPipselectall.Visible = true;
                    chkShowAll.Visible = true;
                    lblResponse.Text = "Services";
                    gvtCheckPIP.Visible = true;
                    // gvtCheckStatus.Visible = true;
                    gvtDesc.Width = 610;
                    btnMoveData.Text = "&SAVE";
                    btnMoveData.Size = new Size(77, 23);
                    btnCopyAllQues.Visible = true;
                    btnCancel.Visible = true;

                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        label4.Visible = true;
                        label4.Text = "Agency";
                        Cmbquestions.Visible = true;

                    }
                    else
                    {
                        label4.Visible = false;
                        Cmbquestions.Visible = false;

                    }
                    FillCAMAST();
                }
                else
                {
                    chkShowAll.Visible = false;
                    lblShowcount.Text = string.Empty;
                    Cmbquestions.Enabled = true;
                    Cmbquestions.Visible = true;
                    btnMoveData.Text = "COPY MASTER TABLE";
                    btnMoveData.Size = new Size(158, 23);
                    lblResponse.Text = "Options";

                    label4.Visible = true;
                    label4.Text = "Agency Table";
                    gvtCheckPIP.Visible = false;
                    // gvtCheckStatus.Visible = false;
                    gvtDesc.Width = 610;
                    Fill_AgyType_Combo();
                }
                if (gvwResponses.Rows.Count > 0)
                {
                    btnMoveData.Visible = true;
                }
                else
                {
                    btnMoveData.Visible = false;
                }
                CommonFunctions.SetComboBoxValue(Cmbquestions, strQuestionID);

            }
        }

        void InsertPIPCUSTHIE()
        {
            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                con.Open();

                string strAgency = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {

                    strAgency = ((ListItem)Cmbquestions.SelectedItem).Value.ToString();

                }


                List<DataGridViewRow> SelectedgvRows = (from c in gvwResponses.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvtCheckPIP"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[PIPCUSTHIE] WHERE [PCH_AGENCY] ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND [PCH_AGY] ='" + strAgency + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }

                if (SelectedgvRows.Count > 0)
                {


                    //List<CaseELIGQUESEntity> EligcusData = _model.CaseSumData.GetELIGCUSTOMQUESPIP(string.Empty, "PIPCUS");
                    foreach (DataGridViewRow gvrow in SelectedgvRows)
                    {

                        string strActive = gvrow.Cells["gvtCheckStatus"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[PIPCUSTHIE]([PCH_AGENCY],[PCH_AGY],[PCH_CUST_CODE],[PCH_ACTIVE],[PCH_UPDATED_ON],[PCH_UPDATED_BY],[PCH_SEQ])VALUES('" + BaseForm.BaseAgencyControlDetails.AgyShortName + "','" + strAgency + "','" + gvrow.Cells["gvtCustCode"].Value.ToString() + "','" + strActive + "',GETDATE(),'" + BaseForm.UserID + "'," + gvrow.Cells["gvtSeq"].Value.ToString() + ")", con))
                        {
                            cmd.ExecuteNonQuery();
                        }

                    }
                    AlertBox.Show("Selected " + SelectedgvRows.Count + " Questions Successfully Saved in PIP");
                }



                con.Close();
            }
            catch (Exception ex)
            {


            }

        }

        void InsertPIPSERHIE()
        {
            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                con.Open();

                string strAgency = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {

                    strAgency = ((ListItem)Cmbquestions.SelectedItem).Value.ToString();

                }


                List<DataGridViewRow> SelectedgvRows = (from c in gvwResponses.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvtCheckPIP"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[PIPSERHIE] WHERE [PSH_AGENCY] ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND [PSH_AGY] ='" + strAgency + "' AND [PSH_TYPE] = '" + propType + "'", con))
                {
                    cmd.ExecuteNonQuery();
                }

                if (SelectedgvRows.Count > 0)
                {


                    //List<CaseELIGQUESEntity> EligcusData = _model.CaseSumData.GetELIGCUSTOMQUESPIP(string.Empty, "PIPCUS");
                    foreach (DataGridViewRow gvrow in SelectedgvRows)
                    {

                        string strActive = gvrow.Cells["gvtCheckStatus"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[PIPSERHIE]([PSH_AGENCY],[PSH_AGY],[PSH_TYPE],[PSH_CODE],[PSH_ACTIVE],[PSH_UPDATED_ON],[PSH_UPDATED_BY])VALUES('" + BaseForm.BaseAgencyControlDetails.AgyShortName + "','" + strAgency + "','" + propType + "','" + gvrow.Cells["gvtCustCode"].Value.ToString() + "','" + strActive + "',GETDATE(),'" + BaseForm.UserID + "')", con))
                        {
                            cmd.ExecuteNonQuery();
                        }

                    }
                    if (propType == "CASEHIE")
                    {
                        AlertBox.Show("Selected " + SelectedgvRows.Count + " Programs Successfully Saved in PIP");
                    }
                    else
                    {
                        AlertBox.Show("Selected " + SelectedgvRows.Count + " Services Successfully Saved in PIP");

                    }
                }



                con.Close();
            }
            catch (Exception ex)
            {


            }

        }


        #region EmailContent

        public DataSet dsPIPAgencies { get; set; }
        void fillcombo()
        {

            cmbName.Items.Insert(0, new ListItem("Select One", "0"));
            cmbName.Items.Insert(1, new ListItem("Last Name", "1"));
            cmbName.Items.Insert(2, new ListItem("First Name", "2"));
            cmbName.Items.Insert(3, new ListItem("Last Name First Name", "3"));
            cmbName.Items.Insert(4, new ListItem("First Name Last Name", "4"));
            cmbName.SelectedIndex = 0;

            cmbMailType.Items.Insert(0, new ListItem("Mail Content After Registration", "REG"));
            cmbMailType.Items.Insert(1, new ListItem("Mail Content After Confirmation Number Generated", "INTK"));
            cmbMailType.Items.Insert(2, new ListItem("Mail Content of Forgot Password", "PWD"));
            cmbMailType.Items.Insert(3, new ListItem("Mail Content After Document Verification", "DOV"));
            cmbMailType.Items.Insert(4, new ListItem("Mail Content Completion / Submission Reminder", "SRM"));
            cmbMailType.SelectedIndex = 0;



            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                lblregAgency.Visible = lblAgency.Visible = lblImageTypeAgy.Visible = lblFieldAgency.Visible = lblSettingAgency.Visible = true;
                cmbEmailAgency.Visible = true;
                cmbSettingAgency.Visible = true;
                cmbImageTypeAgency.Visible = true;
                cmbFieldAgency.Visible = true;
                cmbRegAgency.Visible = true;

                lblRegisAgy.Visible = cmbRegisAgy.Visible = true;

                DataSet dsEmail = new DataSet();
                //SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                //con.Open();
                //using (SqlCommand cmdEmail = new SqlCommand("SELECT *  FROM PIPAGENCY WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY<>'00'", con))
                //{
                //    SqlDataAdapter daEmail = new SqlDataAdapter(cmdEmail);

                //    daEmail.Fill(dsEmail);
                //    dsPIPAgencies = dsEmail;


                //}
                //con.Close();
                DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once
                dsPIPAgencies = ds;
                //DataSet dsHierches = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("1", string.Empty, " ");
                if (dsPIPAgencies.Tables.Count > 0)
                {
                    foreach (DataRow drhiechy in dsPIPAgencies.Tables[0].Rows)
                    {
                        cmbEmailAgency.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                        cmbImageTypeAgency.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                        cmbRegAgency.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                        cmbFieldAgency.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                        cmbSettingAgency.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));

                        cmbRegisAgy.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                    }
                }
                if (userHierarchy.Count > 0)
                {
                    cmbEmailAgency.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                    cmbImageTypeAgency.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                    //  cmbRegAgency.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));
                    cmbFieldAgency.Items.Insert(0, new ListItem("** - All Remaining Agencies", "00"));

                }
                cmbEmailAgency.SelectedIndex = 0;
                cmbImageTypeAgency.SelectedIndex = 0;
                if (cmbRegAgency.Items.Count > 0)
                    cmbRegAgency.SelectedIndex = 0;
                if (cmbSettingAgency.Items.Count > 0)
                    cmbSettingAgency.SelectedIndex = 0;
                cmbFieldAgency.SelectedIndex = 0;

                if (cmbRegisAgy.Items.Count > 0)
                    cmbRegisAgy.SelectedIndex = 0;
            }

        }

        private void btnEmailEdit_Click(object sender, EventArgs e)
        {
            if (btnEMailEdit.Text == "&Edit")
            {
                btnEMailEdit.Text = "&Save";
                btnEmailClose.Text = "&Cancel";

                if (((ListItem)cmbName.SelectedItem).Value.ToString() == "0")
                    CommonFunctions.SetComboBoxValue(cmbName, "2");
                cmbName.Enabled = txtAddress.Enabled = txtSender.Enabled = txtNameofEmail.Enabled = true;//txtMessage.Enabled = 
                cmbEmailAgency.Enabled = cmbMailType.Enabled = false;

                txtMessage.ReadOnly = false;
            }
            else
            {
                if (ValidateEmailControls())
                {
                    if (UpdatePIPMAILS() > 0)
                    {
                        btnEMailEdit.Text = "&Edit";
                        btnEmailClose.Text = "&Close";

                        cmbName.Enabled = txtAddress.Enabled = txtSender.Enabled = txtNameofEmail.Enabled = false;//txtMessage.Enabled =
                        cmbEmailAgency.Enabled = cmbMailType.Enabled = true;

                        txtMessage.ReadOnly = true;
                        // Btn_Save.Enabled = false; cmbPrefix.Enabled = cmbName.Enabled = txtAddress.Enabled = txtMessage.Enabled = txtSender.Enabled = false; Pb_Edit_Cust.Visible = true; Btn_Cancel.Text = "Close";
                    }

                }
            }

        }


        int UpdatePIPMAILS()
        {
            int inti = 0;

            try
            {
                string strType = ((ListItem)cmbMailType.SelectedItem).Value.ToString();

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPMAILS_INSUPDEL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PIPMAIL_AGENCY", BaseForm.BaseAgencyControlDetails.AgyShortName);
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        cmd.Parameters.AddWithValue("@PIPMAIL_AGY", ((ListItem)cmbEmailAgency.SelectedItem).Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PIPMAIL_AGY", "00");
                    }
                    cmd.Parameters.AddWithValue("@PIPMAIL_PURPOSE", strType);
                    cmd.Parameters.AddWithValue("@PIPMAIL_NAME_FORMAT", ((ListItem)cmbName.SelectedItem).Value.ToString());
                    cmd.Parameters.AddWithValue("@PIPMAIL_CONTENT", txtMessage.Text);
                    cmd.Parameters.AddWithValue("@PIPMAIL_SENDER_NAME", txtSender.Text);
                    cmd.Parameters.AddWithValue("@PIPMAIL_SENDER_ADDR", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@PIPMAIL_OPERATOR", BaseForm.UserID);
                    cmd.Parameters.AddWithValue("@PIPAGY_DESC", txtNameofEmail.Text);

                    inti = cmd.ExecuteNonQuery();



                }
                con.Close();
            }
            catch (Exception ex)
            {
                inti = 0;

            }
            return inti;
        }


        void GetMessageDetails()
        {

            try
            {
                string strselectAgency = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (cmbEmailAgency.Items.Count > 0)
                    {
                        strselectAgency = ((ListItem)cmbEmailAgency.SelectedItem).Value.ToString();
                    }
                }
                string strType = "REG";
                if (cmbMailType.Items.Count > 0)
                {
                    strType = ((ListItem)cmbMailType.SelectedItem).Value.ToString();
                }

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();


                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM PIPMAILS WHERE PIPMAIL_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPMAIL_AGY='" + strselectAgency + "' AND PIPMAIL_PURPOSE = '" + strType + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        txtMessage.Text = ds.Tables[0].Rows[0]["PIPMAIL_CONTENT"].ToString();
                        txtAddress.Text = ds.Tables[0].Rows[0]["PIPMAIL_SENDER_ADDR"].ToString();
                        txtSender.Text = ds.Tables[0].Rows[0]["PIPMAIL_SENDER_NAME"].ToString();
                        CommonFunctions.SetComboBoxValue(cmbName, ds.Tables[0].Rows[0]["PIPMAIL_NAME_FORMAT"].ToString());

                    }
                    else
                    {
                        txtMessage.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtSender.Text = string.Empty;
                        CommonFunctions.SetComboBoxValue(cmbName, "0");

                    }
                    using (SqlCommand cmdEmail = new SqlCommand("SELECT *  FROM PIPAGENCY WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY = '" + strselectAgency + "' ", con))
                    {
                        SqlDataAdapter daEmail = new SqlDataAdapter(cmdEmail);
                        DataSet dsEmail = new DataSet();
                        daEmail.Fill(dsEmail);
                        if (dsEmail.Tables[0].Rows.Count > 0)
                        {
                            txtNameofEmail.Text = dsEmail.Tables[0].Rows[0]["PIPAGY_DESC"].ToString();
                        }
                        else
                        {
                            txtNameofEmail.Text = string.Empty;
                        }
                    }

                }

                con.Close();
            }
            catch (Exception ex)
            {


            }

        }


        private void btnEmailClose_Click(object sender, EventArgs e)
        {
            if (btnEmailClose.Text == "&Cancel")
            {
                _errorProvider.SetError(txtNameofEmail, null);
                _errorProvider.SetError(cmbName, null);
                _errorProvider.SetError(txtMessage, null);
                _errorProvider.SetError(txtAddress, null);
                _errorProvider.SetError(txtSender, null);
                btnEMailEdit.Text = "&Edit";
                cmbName.Enabled = txtAddress.Enabled = txtSender.Enabled = txtNameofEmail.Enabled = false;//txtMessage.Enabled =
                cmbEmailAgency.Enabled = cmbMailType.Enabled = true;

                txtMessage.ReadOnly = true;

                btnEmailClose.Text = "&Close";
                GetMessageDetails();
                //Btn_Save.Enabled = true; cmbPrefix.Enabled = cmbName.Enabled = txtAddress.Enabled = txtMessage.Enabled = txtSender.Enabled = false; Pb_Edit_Cust.Visible = true; 

            }
            else
            {
                this.Close();
            }
        }
        private bool ValidateEmailControls()
        {
            bool isValid = true;


            if (txtNameofEmail.Text == string.Empty)
            {
                _errorProvider.SetError(txtNameofEmail, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNameofemal.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtNameofEmail, null);
            }

            if ((cmbName.SelectedItem == null || ((ListItem)cmbName.SelectedItem).Text == Consts.Common.SelectOne))
            {
                _errorProvider.SetError(cmbName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbName, null);
            }


            if (txtMessage.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtMessage, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMessage.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtMessage, null);
            }
            if (txtAddress.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtAddress, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAddress.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtAddress, null);
            }
            if (txtSender.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtSender, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSender.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtSender, null);
            }


            return isValid;
        }

        private void cmbMailType_Click(object sender, EventArgs e)
        {
            GetMessageDetails();
        }

        #endregion

        private void tabControl1_Click(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 1)
            //{
            //    GetMessageDetails();
            //}
            //else if (tabControl1.SelectedIndex == 2)
            //{
            //    fillPIPREGUsers();
            //}
            //else if (tabControl1.SelectedIndex == 3)
            //{
            //    GetImageTypes();
            //}
            //else if (tabControl1.SelectedIndex == 4)
            //{
            //    GetFieldControlData();
            //}
            //else if (tabControl1.SelectedIndex == 5)
            //{
            //    GetForcePassword();
            //}
        }

        private void cmbEmailAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                if (cmbEmailAgency.Items.Count > 0)
                {
                    GetMessageDetails();
                }
            }
        }

        private void btnCopyAllQues_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure to copy from CAPTAIN to PIP database?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerAllques);
        }



        private void MessageBoxHandlerAllques(DialogResult dialogResult)
        {

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                if (propType == "CUSTOM")
                {
                    List<CustfldsEntity> Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
                    if (Cust.Count > 0)
                    {
                        // Cust = Cust.FindAll(u => (u.MemAccess == "A" || u.MemAccess == "H"));//&& u.custSendtoPip == "Y"

                        DeletePIPCustomeQuestions(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper(), string.Empty);
                        DeletePIPCustResponses(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper(), string.Empty);

                        try
                        {

                            SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                            con.Open();

                            string stmt = "INSERT INTO dbo.PIPCUSTQST (PCUST_AGENCY, PCUST_CODE,  PCUST_ACTIVE, PCUST_DESC, PCUST_RESP_TYPE, PCUST_MEM_ACCESS,  PCUST_DATE_LSTC, PCUST_LSTC_OPERATOR)VALUES(@PCUST_AGENCY, @PCUST_CODE,  @PCUST_ACTIVE, @PCUST_DESC, @PCUST_RESP_TYPE, @PCUST_MEM_ACCESS,  GETDATE(), @PCUST_LSTC_OPERATOR)";

                            SqlCommand cmd = new SqlCommand(stmt, con);
                            cmd.Parameters.Add("@PCUST_AGENCY", SqlDbType.NVarChar, 10);
                            cmd.Parameters.Add("@PCUST_CODE", SqlDbType.NVarChar, 6);
                            cmd.Parameters.Add("@PCUST_ACTIVE", SqlDbType.Char, 1);
                            cmd.Parameters.Add("@PCUST_DESC", SqlDbType.VarChar, 125);
                            cmd.Parameters.Add("@PCUST_RESP_TYPE", SqlDbType.Char, 1);
                            cmd.Parameters.Add("@PCUST_MEM_ACCESS", SqlDbType.Char, 1);
                            cmd.Parameters.Add("@PCUST_LSTC_OPERATOR", SqlDbType.VarChar, 20);

                            foreach (CustfldsEntity Entity in Cust)
                            {
                                cmd.Parameters["@PCUST_AGENCY"].Value = BaseForm.BaseAgencyControlDetails.AgyShortName;
                                cmd.Parameters["@PCUST_CODE"].Value = Entity.CustCode;
                                cmd.Parameters["@PCUST_ACTIVE"].Value = Entity.Active;
                                cmd.Parameters["@PCUST_DESC"].Value = Entity.CustDesc;
                                cmd.Parameters["@PCUST_RESP_TYPE"].Value = Entity.RespType;
                                cmd.Parameters["@PCUST_MEM_ACCESS"].Value = Entity.MemAccess.ToString();
                                cmd.Parameters["@PCUST_LSTC_OPERATOR"].Value = BaseForm.UserID;
                                int i = cmd.ExecuteNonQuery();

                                if (i > 0)
                                {
                                    //ListItem li = new ListItem(Entity.CustDesc, Entity.CustCode, Entity.custSpanishDesc, Entity.RespType, Entity.ScrCode, string.Empty);
                                    if ((Entity.RespType.ToString() == "D") || (Entity.RespType.ToString() == "C"))
                                    {
                                        List<CustRespEntity> CustResp = _model.FieldControls.GetCustRespByCustCode(Entity.CustCode, string.Empty);
                                        if (CustResp.Count > 0)
                                        {

                                            foreach (CustRespEntity RespEntity in CustResp)
                                            {
                                                InsertPIPCustResponses(con, Entity.CustCode, RespEntity.RespSeq, RespEntity.RespDesc, RespEntity.DescCode, RespEntity.RspStatus);

                                            }
                                        }
                                    }
                                }

                            }
                            con.Close();
                        }
                        catch (Exception ex)
                        {

                        }
                        AlertBox.Show("CUSTFLDS and CUSTRESP tables copied to PIP successfully");
                    }

                }
                else if (propType == "CAMAST")
                {
                    List<CAMASTEntity> CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                    if (CAList.Count > 0)
                    {
                        DeletePIPServices(propType);

                        SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                        con.Open();
                        foreach (CAMASTEntity CAMASTEntity in CAList)
                        {
                            InsertPIPServices(con, CAMASTEntity.Code, CAMASTEntity.Desc, propType, CAMASTEntity.PIPActive);
                        }
                        con.Close();
                        AlertBox.Show("CAMAST table copied to PIP successfully");
                    }
                }
                else if (propType == "CASEHIE")
                {

                    List<HierarchyEntity> hierachyEntity = _model.HierarchyAndPrograms.GetCaseHierarchyDepartment(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, "PROGRAM");

                    if (hierachyEntity.Count > 0)
                    {

                        DeletePIPServices(propType);
                        SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                        con.Open();
                        foreach (HierarchyEntity HieEntity in hierachyEntity)
                        {

                            InsertPIPServices(con, HieEntity.Code, HieEntity.HirarchyName, propType, HieEntity.PIPActive);
                        }
                        con.Close();
                        AlertBox.Show("CASEHIE table copied to PIP successfully");
                    }
                }
                else if (propType == "CASESER")
                {
                    DataSet serviceDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("CASESER", "00", string.Empty, string.Empty, string.Empty, string.Empty);
                    DataTable serviceDT = serviceDS.Tables[0];

                    if (serviceDT.Rows.Count > 0)
                    {
                        DeletePIPServices(propType);
                        SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                        con.Open();

                        foreach (DataRow SerEntity in serviceDT.Rows)
                        {
                            InsertPIPServices(con, SerEntity["INQ_CODE"].ToString(), SerEntity["INQ_DESC"].ToString(), propType, SerEntity["CAC_ACTIVE"].ToString());
                        }
                        con.Close();
                        AlertBox.Show("CASESER table copied to PIP successfully");
                    }

                }
            }

        }

        private void PIPAdmin_Load(object sender, EventArgs e)
        {
            try
            {


                SqlConnection connect = new SqlConnection();

                connect.ConnectionString = BaseForm.BaseLeanDataBaseConnectionString;
                connect.Open();
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();

                    fillcombo();

                    GetMessageDetails();
                    GetImageTypes();

                    cmbPipType.Items.Add(new ListItem("Custom Questions", "CUSTOM"));
                    if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "0")
                    {
                        cmbPipType.Items.Add(new ListItem("Services from CAMAST", "CAMAST"));
                    }
                    else if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "1")
                    {
                        cmbPipType.Items.Add(new ListItem("Programs from CASEHIE", "CASEHIE"));
                    }
                    else if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "2")
                    {
                        cmbPipType.Items.Add(new ListItem("Services from CASESER", "CASESER"));
                    }
                    cmbPipType.Items.Add(new ListItem("Agency Tables", "AGYTABS"));
                    cmbPipType.SelectedIndex = 0;

                    propType = "CUSTOM";

                    // Cmbquestions_SelectedIndexChanged(Cmbquestions, new EventArgs());
                }

                else
                {
                    MessageBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection issue with Server \n Please contact CAPSYSTEMS INC", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtRegEmail.Text != string.Empty)
            {
                _errorProvider.SetError(txtRegEmail, null);

                MessageBox.Show("Do you want to delete?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxDeleteIntakeHandler);

            }
            else
            {
                _errorProvider.SetError(txtRegEmail, "Please Enter Email Id");
            }
        }

        private void MessageBoxDeleteIntakeHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {


                string strType = "1";
                if (rbDelReg.Checked == true)
                {
                    strType = "2";
                }

                string strFilterAgy = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (cmbRegAgency.Items.Count > 0)
                    {
                        strFilterAgy = ((ListItem)cmbRegAgency.SelectedItem).Value.ToString();
                    }
                }

                string _propRegid = string.Empty;
                DataTable dt = PIPDATA.GetPIPIntakeSearchData(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, txtRegEmail.Text, "EMAIL", BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);

                if (dt.Rows.Count > 0)
                {
                    _propRegid = dt.Rows[0]["PIPREG_ID"].ToString();
                    // strFilterAgy = dt.Rows[0]["PIP_AGY"].ToString();

                    //using (NetworkShareAccesser.Access("CAPSYSAZ8", "PIP_Docs", "ApplicationUser", "7!Enough32Distract$"))
                    //{

                    //    string strAz8Filename = @"\\CAPSYSAZ8\PIP_DOCS\" + BaseForm.BaseAgencyControlDetails.AgyShortName + strFilterAgy + @"\DOCUPLDS\" + _propRegid;
                    //    DeleteRegistereddocument(strAz8Filename);
                    //}

                }

                string strstatus = PIPDATA.CheckorDeletePIPIntakeData(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, strType, txtRegEmail.Text, "DELETE", BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);


                if (strstatus == "Success")
                {
                    AlertBox.Show("Successfully Deleted");
                }
                else
                {
                    //if (chkDelReg.Checked == true)
                    //{
                    CommonFunctions.MessageBoxDisplay("Email ID does not exist");
                    //}
                    //else
                    //{

                    //}
                }
            }
        }
        private void DeleteRegistereddocument(string strAz8FileLocation)
        {
            try
            {
                if (Directory.Exists(strAz8FileLocation))
                {
                    DirectoryInfo dir1 = new DirectoryInfo(strAz8FileLocation);
                    FileInfo[] Folder1Files = dir1.GetFiles();
                    if (Folder1Files.Length > 0)
                    {
                        foreach (FileInfo aFile in Folder1Files)
                        {
                            var remote = Path.Combine(strAz8FileLocation, aFile.Name);
                            File.Delete(remote);
                        }
                        dir1.Delete();
                    }
                }
            }
            catch (Exception ex)
            {

                // CommonFunctions.MessageBoxDisplay(ex.Message);
            }
        }


        private void pnlMessages_Click(object sender, EventArgs e)
        {

        }

        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            List<DataGridViewRow> SelectedgvRows = (from c in gvwResponses.Rows.Cast<DataGridViewRow>().ToList()
                                                    where (((DataGridViewCheckBoxCell)c.Cells["gvtCheckPIP"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                    select c).ToList();

            int intIndex = 0;
            gvwResponses.Rows.Clear();
            if (chkShowAll.Checked == false)
            {
                foreach (DataGridViewRow item in SelectedgvRows)
                {
                    string str = item.Cells[0].Value == null ? "false" : "true";
                    intIndex = gvwResponses.Rows.Add(item.Cells[0].Value, item.Cells[1].Value, item.Cells[2].Value, item.Cells[3].Value, item.Cells[4].Value, item.Cells[5].Value);

                }
                if (propType == "CUSTOM")
                {
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        lblShowcount.Text = SelectedgvRows.Count.ToString() + "/" + propCust.Count.ToString();
                    else
                        lblShowcount.Text = SelectedgvRows.Count.ToString() + "/" + propCustSingle.Count.ToString();
                }
                else if (propType == "CASEHIE")
                {
                    lblShowcount.Text = SelectedgvRows.Count.ToString() + "/" + prophierachyEntity.Count.ToString();
                }
                else
                {
                    lblShowcount.Text = SelectedgvRows.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();
                }

            }
            else
            {
                List<ListItem> listcode = new List<Utilities.ListItem>();
                foreach (DataGridViewRow item in SelectedgvRows)
                {
                    listcode.Add(new Utilities.ListItem(item.Cells[1].Value.ToString(), item.Cells[2].Value, item.Cells[1].Value.ToString(), item.Cells[1].Value.ToString(), item.Cells[4].Value.ToString(), item.Cells[4].Value.ToString()));
                }


                if (propType == "CUSTOM")
                {
                    btnCopyAllQues.Visible = true;

                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        if (propCust.Count > 0)
                        {
                            //Cust = Cust.FindAll(u => u.EligFliedType == "A" || u.EligFliedType == "H");
                            propCust = propCust.OrderBy(u => u.EligQuesDesc).ToList();

                            if (propCust.Count > 0)
                            {
                                bool boolselques = false;
                                bool boolActive = true;
                                string intSeq = "0";

                                foreach (CaseELIGQUESEntity Entity in propCust)
                                {
                                    boolselques = false;
                                    boolActive = true;

                                    if (chkShowAll.Checked)
                                    {
                                        ListItem listpiphie = listcode.Find(u => u.Text == Entity.EligQuesCode);
                                        if (listpiphie != null)
                                        {
                                            boolselques = true;
                                            boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;
                                            intSeq = listpiphie.ScreenCode;
                                        }
                                        else
                                        {
                                            intSeq = (intIndex + 1).ToString();
                                        }
                                        intIndex = gvwResponses.Rows.Add(boolselques, Entity.EligQuesCode, Entity.EligQuesDesc, Entity.EligQuesCode, intSeq, boolActive);
                                        gvwResponses.Rows[intIndex].Tag = Entity;
                                    }
                                    else
                                    {
                                        ListItem listpiphie = listcode.Find(u => u.Text == Entity.EligQuesCode);
                                        if (listpiphie != null)
                                        {
                                            boolselques = true;
                                            boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;

                                            intIndex = gvwResponses.Rows.Add(boolselques, Entity.EligQuesCode, Entity.EligQuesDesc, Entity.EligQuesCode, listpiphie.ScreenCode, boolActive);
                                            gvwResponses.Rows[intIndex].Tag = Entity;
                                        }

                                    }
                                }
                                lblShowcount.Text = listcode.Count.ToString() + "/" + propCust.Count.ToString();
                            }

                        }
                    }
                    else
                    {
                        if (propCustSingle.Count > 0)
                        {
                            propCustSingle = propCustSingle.OrderBy(u => u.CustDesc).ToList();

                            if (propCustSingle.Count > 0)
                            {

                                bool boolselques = false;
                                bool boolActive = true;
                                string intSeq = "0";

                                foreach (CustfldsEntity Entity in propCustSingle)
                                {
                                    boolselques = false;
                                    boolActive = true;
                                    if (chkShowAll.Checked)
                                    {
                                        ListItem listpiphie = listcode.Find(u => u.Text == Entity.CustCode);
                                        if (listpiphie != null)
                                        {
                                            boolselques = true;
                                            boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;
                                            intSeq = listpiphie.ScreenCode;
                                        }
                                        else
                                        {
                                            intSeq = (intIndex + 1).ToString();
                                        }
                                        intIndex = gvwResponses.Rows.Add(boolselques, Entity.CustCode, Entity.CustDesc, Entity.CustCode, intSeq, boolActive);
                                        gvwResponses.Rows[intIndex].Tag = Entity;
                                    }
                                    else
                                    {
                                        ListItem listpiphie = listcode.Find(u => u.Text == Entity.CustCode);
                                        if (listpiphie != null)
                                        {
                                            boolselques = true;
                                            boolActive = listpiphie.ValueDisplayCode == "Y" ? true : false;

                                            intIndex = gvwResponses.Rows.Add(boolselques, Entity.CustCode, Entity.CustDesc, Entity.CustCode, listpiphie.ScreenCode, boolActive);
                                            gvwResponses.Rows[intIndex].Tag = Entity;
                                        }

                                    }

                                }
                                lblShowcount.Text = listcode.Count.ToString() + "/" + propCustSingle.Count.ToString();

                            }

                        }
                    }


                }
                else
                {
                    lblShowcount.Text = string.Empty;
                    if (propType == "CASEHIE")
                    {

                        if (prophierachyEntity.Count > 0)
                        {
                            prophierachyEntity = prophierachyEntity.OrderBy(u => u.HirarchyName).ToList();
                            bool boolseldata = false;
                            bool boolActive = true;
                            foreach (HierarchyEntity HieEntity in prophierachyEntity)
                            {
                                boolseldata = false;
                                boolActive = true;

                                if (chkShowAll.Checked)
                                {
                                    ListItem listpipservices = listcode.Find(u => u.Text.Trim() == HieEntity.Code.Trim());
                                    if (listpipservices != null)
                                    {
                                        boolseldata = true;
                                        boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                                    }
                                    intIndex = gvwResponses.Rows.Add(boolseldata, HieEntity.Code, HieEntity.HirarchyName, HieEntity.Code, HieEntity.Code, boolActive);
                                    gvwResponses.Rows[intIndex].Tag = HieEntity;
                                }
                                else
                                {
                                    ListItem listpipservices = listcode.Find(u => u.Text.Trim() == HieEntity.Code.Trim());
                                    if (listpipservices != null)
                                    {
                                        boolseldata = true;
                                        boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                        intIndex = gvwResponses.Rows.Add(boolseldata, HieEntity.Code, HieEntity.HirarchyName, HieEntity.Code, HieEntity.Code, boolActive);
                                        gvwResponses.Rows[intIndex].Tag = HieEntity;
                                    }
                                }
                            }
                            lblShowcount.Text = listcode.Count.ToString() + "/" + prophierachyEntity.Count.ToString();


                        }
                    }
                    if (propType == "CAMAST")
                    {


                        if (propserviceDT.Rows.Count > 0)
                        {
                            bool boolseldata = false;
                            bool boolActive = true;
                            foreach (DataRow dr in propserviceDT.Rows)
                            {
                                boolseldata = false;
                                boolActive = true;
                                if (chkShowAll.Checked)
                                {
                                    ListItem listpipservices = listcode.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                    if (listpipservices != null)
                                    {
                                        boolseldata = true;
                                        boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                                    }
                                    intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                    gvwResponses.Rows[intIndex].Tag = dr;
                                }
                                else
                                {
                                    ListItem listpipservices = listcode.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                    if (listpipservices != null)
                                    {
                                        boolseldata = true;
                                        boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                        intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                        gvwResponses.Rows[intIndex].Tag = dr;
                                    }

                                }

                            }
                            lblShowcount.Text = listcode.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();

                        }
                    }
                    if (propType == "CASESER")
                    {

                        if (propserviceDT.Rows.Count > 0)
                        {
                            bool boolseldata = false;
                            bool boolActive = true;
                            foreach (DataRow dr in propserviceDT.Rows)
                            {
                                boolseldata = false;
                                boolActive = true;
                                if (chkShowAll.Checked)
                                {
                                    ListItem listpipservices = listcode.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                    if (listpipservices != null)
                                    {
                                        boolseldata = true;
                                        boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;
                                    }
                                    intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                    gvwResponses.Rows[intIndex].Tag = dr;
                                }
                                else
                                {
                                    ListItem listpipservices = listcode.Find(u => u.Text.Trim() == dr["INQ_CODE"].ToString().Trim());
                                    if (listpipservices != null)
                                    {
                                        boolseldata = true;
                                        boolActive = listpipservices.ValueDisplayCode == "Y" ? true : false;

                                        intIndex = gvwResponses.Rows.Add(boolseldata, dr["INQ_CODE"], dr["INQ_DESC"], dr["INQ_CODE"], dr["INQ_CODE"], boolActive);
                                        gvwResponses.Rows[intIndex].Tag = dr;
                                    }

                                }

                            }
                            lblShowcount.Text = listcode.Count.ToString() + "/" + propserviceDT.Rows.Count.ToString();

                        }
                    }

                }
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Cmbquestions.Visible == true)
                Cmbquestions_SelectedIndexChanged(sender, e);
            else
                cmbPipType_SelectedIndexChanged(sender, e);
        }

        void GetImageTypes()
        {

            gvwImageTypes.SelectionChanged -= gvwImageTypes_SelectionChanged;
            gvwImageTypes.Rows.Clear();


            if (propimagetypesCategory != null)
            {


                if (propimagetypesCategory.Count > 0)
                {
                    List<ListItem> pipImageTypes = GetPipImageTypes();





                    DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.gvwImageTypes.Columns["gvComboApp"];

                    List<CommonEntity> commonApplicantTypes = new List<CommonEntity>();
                    commonApplicantTypes.Add(new CommonEntity("A", "Applicant"));
                    commonApplicantTypes.Add(new CommonEntity("M", "Member"));
                    commonApplicantTypes.Add(new CommonEntity("H", "All Household Members"));

                    cb.DataSource = commonApplicantTypes;
                    cb.DisplayMember = "DESC";
                    cb.ValueMember = "CODE";
                    cb = (DataGridViewComboBoxColumn)this.gvwImageTypes.Columns["gvComboApp"];





                    bool boolselques = false;
                    bool boolrequired = false;
                    bool boolActive = true;
                    string strIncomeTypes = string.Empty;
                    string strTypes = string.Empty;
                    string strDesc = string.Empty;
                    int intIndex;
                    foreach (CommonEntity ImgTypes in propimagetypesCategory)
                    {
                        boolselques = false;
                        boolrequired = false;
                        boolActive = true;
                        strIncomeTypes = string.Empty;
                        strTypes = "A";
                        strDesc = string.Empty;
                        if (chkImgshowall.Checked)
                        {
                            ListItem listpipimg = pipImageTypes.Find(u => u.Value.ToString().Trim() == ImgTypes.Code.Trim().Trim());
                            if (listpipimg != null)
                            {
                                strIncomeTypes = listpipimg.ScreenCode;
                                strTypes = listpipimg.ValueDisplayCode;
                                strDesc = listpipimg.ID.ToString();
                                boolrequired = listpipimg.Amount == "Y" ? true : false;
                                boolselques = true;
                                //boolActive = listpipimg.ValueDisplayCode == "Y" ? true : false;
                            }
                            intIndex = gvwImageTypes.Rows.Add(boolselques, ImgTypes.Extension.ToString(), ImgTypes.Code.Trim(), ImgTypes.Desc, strTypes, boolrequired, strIncomeTypes, "  ...  ", strDesc);
                            gvwImageTypes.Rows[intIndex].Tag = ImgTypes;
                        }
                        else
                        {
                            ListItem listpipimg = pipImageTypes.Find(u => u.Value.ToString().Trim() == ImgTypes.Code.Trim().Trim());
                            if (listpipimg != null)
                            {
                                strIncomeTypes = listpipimg.ScreenCode;
                                strTypes = listpipimg.ValueDisplayCode;
                                strDesc = listpipimg.ID.ToString();
                                boolrequired = listpipimg.Amount == "Y" ? true : false;
                                boolselques = true;

                                intIndex = gvwImageTypes.Rows.Add(boolselques, ImgTypes.Extension.ToString(), ImgTypes.Code.Trim(), ImgTypes.Desc, strTypes, boolrequired, strIncomeTypes, "  ...  ", strDesc);
                                gvwImageTypes.Rows[intIndex].Tag = ImgTypes;
                            }

                        }

                    }
                    lblImgCount.Text = pipImageTypes.Count.ToString() + "/" + propimagetypesCategory.Count.ToString();

                    gvwImageTypes.SelectionChanged += gvwImageTypes_SelectionChanged;
                    if (gvwImageTypes.Rows.Count > 0)
                    {
                        gvwImageTypes.Rows[0].Selected = true;
                        if (gvwImageTypes.SelectedRows.Count > 0)
                        {
                            txtimgDesc.Text = gvwImageTypes.SelectedRows[0].Cells["gvtIImagedesc2"].Value == null ? string.Empty : gvwImageTypes.SelectedRows[0].Cells["gvtIImagedesc2"].Value.ToString();
                        }
                    }

                }

            }
        }

        void GetFieldControlData()
        {


            gvwPipFields.Rows.Clear();

            int intIndex;
            List<ListItem> listFieldControldata = GetPipFieldControlData();
            foreach (ListItem fieldcontroldlst in listFieldControldata)
            {

                intIndex = gvwPipFields.Rows.Add(fieldcontroldlst.Text.ToString(), fieldcontroldlst.ID == "Y" ? true : false, fieldcontroldlst.ValueDisplayCode == "Y" ? true : false, fieldcontroldlst.ScreenCode == "Y" ? true : false, fieldcontroldlst.ScreenType == "Y" ? true : false, fieldcontroldlst.Value);
                gvwPipFields.Rows[intIndex].Tag = fieldcontroldlst;

            }
            if (gvwPipFields.Rows.Count > 0)
                gvwPipFields.Rows[0].Selected = true;
        }



        private void gvwImageTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (gvbIincometypes.Index == e.ColumnIndex)
                {
                    string strCode = gvwImageTypes.SelectedRows[0].Cells["gvtIIncomeTypes"].Value == null ? string.Empty : gvwImageTypes.SelectedRows[0].Cells["gvtIIncomeTypes"].Value.ToString();
                    AlertCodeForm objform = new AlertCodeForm(BaseForm, strCode, Privileges);
                    objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    objform.StartPosition = FormStartPosition.CenterScreen;
                    objform.ShowDialog();
                }
                if (gvcRequired.Index == e.ColumnIndex)
                {
                    gvwImageTypes.CellClick -= gvwImageTypes_CellClick;
                    string strSelect = gvwImageTypes.SelectedRows[0].Cells["gvChkIsentpip"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                    if (strSelect == "N")
                    {
                        string strselectreq = gvwImageTypes.SelectedRows[0].Cells["gvcRequired"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                        if (strselectreq == "Y")
                        {
                            gvwImageTypes.SelectedRows[0].Cells["gvChkIsentpip"].Value = true;
                        }
                    }
                    gvwImageTypes.CellClick += gvwImageTypes_CellClick;
                }
            }
        }



        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
                gvwImageTypes.SelectedRows[0].Cells["gvtIIncomeTypes"].Value = form.propAlertCode;

        }

        private void txtimgDesc_Leave(object sender, EventArgs e)
        {
            if (gvwImageTypes.Rows.Count > 0)
            {
                if (gvwImageTypes.SelectedRows.Count > 0)
                {
                    gvwImageTypes.SelectedRows[0].Cells["gvtIImagedesc2"].Value = txtimgDesc.Text;
                }
            }
        }

        private void gvwImageTypes_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwImageTypes.Rows.Count > 0)
            {
                if (gvwImageTypes.SelectedRows.Count > 0)
                {
                    txtimgDesc.Text = gvwImageTypes.SelectedRows[0].Cells["gvtIImagedesc2"].Value == null ? string.Empty : gvwImageTypes.SelectedRows[0].Cells["gvtIImagedesc2"].Value.ToString();
                }
            }
        }

        private void chkImgshowall_CheckedChanged(object sender, EventArgs e)
        {
            //List<DataGridViewRow> SelectedgvRows = (from c in gvwImageTypes.Rows.Cast<DataGridViewRow>().ToList()
            //                                        where (((DataGridViewCheckBoxCell)c.Cells["gvChkIsentpip"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
            //                                        select c).ToList();

            //int intIndex;
            //gvwImageTypes.Rows.Clear();
            //if (chkShowAll.Checked == false)
            //{
            //    foreach (DataGridViewRow item in SelectedgvRows)
            //    {
            //        string str = item.Cells[0].Value == null ? "false" : "true";
            //        intIndex = gvwImageTypes.Rows.Add(item.Cells[0].Value, item.Cells[1].Value, item.Cells[2].Value, item.Cells[3].Value, item.Cells[4].Value, "  ...  ", item.Cells[6].Value);

            //    }
            //    lblImgCount.Text = pipImageTypes.Count.ToString() + "/" + propimagetypesCategory.Count.ToString();

            //}

            GetImageTypes();
        }

        private void btnImgSave_Click(object sender, EventArgs e)
        {
            if (btnImgSave.Text == "&Edit")
            {
                PbExcel.Visible = false;
                btnImgSave.Text = "&Save";
                btnImgCancel.Text = "&Cancel";
                cmbImageTypeAgency.Enabled = false; gvwImageTypes.Enabled = txtimgDesc.Enabled = true;
            }
            else
            {
                if (gvwImageTypes.Rows.Count > 0)
                {
                    MessageBox.Show("Are you sure to copy from CAPTAIN to PIP database?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerImageType);
                }
            }
        }


        private void MessageBoxHandlerImageType(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                try
                {

                    SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                    con.Open();

                    List<DataGridViewRow> SelectedgvRows = (from c in gvwImageTypes.Rows.Cast<DataGridViewRow>().ToList()
                                                            where (((DataGridViewCheckBoxCell)c.Cells["gvChkIsentpip"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                            select c).ToList();

                    string strselectImgAgency = "00";
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        if (cmbImageTypeAgency.Items.Count > 0)
                        {
                            strselectImgAgency = ((ListItem)cmbImageTypeAgency.SelectedItem).Value.ToString();
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[PIPIMGTYPES] WHERE [PIPIMGT_AGENCY] ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPIMGT_AGY ='" + strselectImgAgency + "'", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    if (SelectedgvRows.Count > 0)
                    {
                        string strDesc = string.Empty;


                        //if (strDesc.Contains("'"))
                        //    strDesc = strDesc.Replace("'", "''");

                        //using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[PIPIMGTYPES]([PIPIMGT_AGENCY],[PIPIMGT_CODE],[PIPIMGT_DESC],[PIPIMGT_AMH],[PIPIMGT_INCTYPES],[PIPIMGT_UPDATED_ON],[PIPIMGT_UPDATED_BY])VALUES('" + BaseForm.BaseAgencyControlDetails.AgyShortName + "', '" + gvrow.Cells["gvtICode"].Value.ToString() + "','" + strDesc + "','" + gvrow.Cells["gvComboApp"].Value.ToString() + "','" + gvrow.Cells["gvtIIncomeTypes"].Value.ToString() + "', GETDATE(),'" + BaseForm.UserID + "')", con))
                        //{
                        //    cmd.ExecuteNonQuery();
                        //}



                        foreach (DataGridViewRow gvrow in SelectedgvRows)
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                strDesc = gvrow.Cells["gvtIImagedesc2"].Value.ToString();
                                cmd.Connection = con;
                                cmd.CommandText = "PIPIMGTYPES_INSUPDEL";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@PIPIMGT_AGENCY", BaseForm.BaseAgencyControlDetails.AgyShortName);

                                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                                {
                                    cmd.Parameters.AddWithValue("@PIPIMGT_AGY", ((ListItem)cmbImageTypeAgency.SelectedItem).Value.ToString());
                                }
                                else
                                {

                                    cmd.Parameters.AddWithValue("@PIPIMGT_AGY", "00");
                                }
                                cmd.Parameters.AddWithValue("@PIPIMGT_SECURITY", gvrow.Cells["gvtsecurity"].Value.ToString());
                                cmd.Parameters.AddWithValue("@PIPIMGT_IMG_TYPE", gvrow.Cells["gvtICode"].Value.ToString());
                                cmd.Parameters.AddWithValue("@PIPIMGT_IMG_DESC", gvrow.Cells["gvtImgDesc"].Value.ToString());
                                if (strDesc != string.Empty)
                                    cmd.Parameters.AddWithValue("@PIPIMGT_DESC", strDesc);

                                cmd.Parameters.AddWithValue("@PIPIMGT_AMH", gvrow.Cells["gvComboApp"].Value.ToString());
                                cmd.Parameters.AddWithValue("@PIPIMGT_INCTYPES", gvrow.Cells["gvtIIncomeTypes"].Value.ToString());
                                cmd.Parameters.AddWithValue("@PIPIMGT_UPDATED_BY", BaseForm.UserID);
                                cmd.Parameters.AddWithValue("@PIPIMGT_REQUIRED", (gvrow.Cells["gvcRequired"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N"));


                                int inti = cmd.ExecuteNonQuery();

                            }

                        }

                        AlertBox.Show("Selected " + SelectedgvRows.Count + " Image Types Successfully Saved in PIP");
                        GetImageTypes();
                        btnImgSave.Text = "&Edit";
                        btnImgCancel.Text = "&Close";
                        PbExcel.Visible = true;
                        cmbImageTypeAgency.Enabled = true; gvwImageTypes.Enabled = txtimgDesc.Enabled = false;
                    }



                    con.Close();
                }
                catch (Exception ex)
                {


                }

            }
        }

        private void btnImgCancel_Click(object sender, EventArgs e)
        {
            if (btnImgCancel.Text == "&Cancel")
            {
                btnImgSave.Text = "&Edit";
                btnImgCancel.Text = "&Close";
                PbExcel.Visible = true;
                cmbImageTypeAgency.Enabled = true; gvwImageTypes.Enabled = txtimgDesc.Enabled = false;
                GetImageTypes();

            }
            else
            {
                this.Close();
            }
        }

        private void cmbImageTypeAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbImageTypeAgency.Items.Count > 0)
            {
                GetImageTypes();
            }
        }

        private void txtimgDesc_TextChanged(object sender, EventArgs e)
        {

        }

        private void PbExcel_Click(object sender, EventArgs e)
        {
            On_ExcelFormClosed();
        }

        #region ExcelFilereport

        string fillIncomesTypesDesc(List<AgyTabEntity> lookUpIncomeTypes, string strIncomeTypes)
        {
            string strDesc = string.Empty;

            if (strIncomeTypes.Trim() != string.Empty)
            {
                string response = strIncomeTypes;
                string[] arrResponse = null;
                if (response.IndexOf(',') > 0)
                {
                    arrResponse = response.Split(',');
                }
                else if (!response.Equals(string.Empty))
                {
                    arrResponse = new string[] { response };
                }


                foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
                {

                    string resDesc = agyEntity.agycode.ToString().Trim();


                    if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                    {
                        strDesc = strDesc + agyEntity.agydesc + ",";
                    }

                }
            }
            return strDesc;

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
            //  s58
            // -----------------------------------------------
            WorksheetStyle s58 = styles.Add("s58");
            s58.Font.Bold = true;
            s58.Font.FontName = "Arial";
            s58.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s58.Alignment.Vertical = StyleVerticalAlignment.Center;
            s58.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s59
            // -----------------------------------------------
            WorksheetStyle s59 = styles.Add("s59");
            s59.Font.Bold = true;
            s59.Font.FontName = "Arial";
            s59.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s59.Alignment.Vertical = StyleVerticalAlignment.Center;
            s59.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s60
            // -----------------------------------------------
            WorksheetStyle s60 = styles.Add("s60");
            s60.Alignment.Vertical = StyleVerticalAlignment.Center;
            s60.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s61
            // -----------------------------------------------
            WorksheetStyle s61 = styles.Add("s61");
            s61.Font.Bold = true;
            s61.Font.FontName = "Arial";
            s61.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s61.Alignment.Vertical = StyleVerticalAlignment.Center;
            s61.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s62.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s63.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Font.FontName = "Arial";
            s64.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s64.Alignment.Vertical = StyleVerticalAlignment.Center;
            s64.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s65.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Font.FontName = "Arial";
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s66.Alignment.Vertical = StyleVerticalAlignment.Center;
            s66.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Font.FontName = "Arial";
            s68.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s68.Alignment.Vertical = StyleVerticalAlignment.Center;
            s68.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Font.Strikethrough = true;
            s69.Font.FontName = "Arial";
            s69.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s69.Alignment.Vertical = StyleVerticalAlignment.Center;
            s69.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Font.Bold = true;
            s70.Font.FontName = "Arial";
            s70.Font.Size = 12;
            s70.Font.Color = "#000000";
            s70.Interior.Color = "#ffcccc";
            s70.Interior.Pattern = StyleInteriorPattern.Solid;
            s70.Alignment.Vertical = StyleVerticalAlignment.Center;
            s70.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Font.FontName = "Arial";
            s71.Font.Size = 12;
            s71.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Font.Bold = true;
            s72.Font.FontName = "Arial";
            s72.Font.Size = 12;
            s72.Font.Color = "#000000";
            s72.Interior.Color = "#ffcccc";//"#B0C4DE";
            s72.Interior.Pattern = StyleInteriorPattern.Solid;
            s72.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s72.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s73.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s74.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Font.Bold = true;
            s75.Font.FontName = "Arial";
            s75.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s75.Alignment.Vertical = StyleVerticalAlignment.Center;
            s75.Alignment.WrapText = true;
        }

        string Random_Filename = null; string PdfName = null;

        public string propReportPath { get; set; }
        private void On_ExcelFormClosed()
        {
            try
            {
                Random_Filename = null;
                PdfName = "Pdf File";
                string strFileName = string.Empty;


                PdfName = "PIP Image Types";
                strFileName = PdfName;

                strFileName = strFileName + ".xls";

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

                Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;
                List<ListItem> pipImageTypes = GetPipImageTypes();

                if (pipImageTypes.Count > 0)
                {

                    List<AgyTabEntity> _listIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();

                    sheet = book.Worksheets.Add("Image Types");


                    sheet.Table.DefaultRowHeight = 14.25F;
                    sheet.Table.ExpandedColumnCount = 12;
                    sheet.Table.FullColumns = 1;
                    sheet.Table.FullRows = 1;
                    WorksheetColumn column0 = sheet.Table.Columns.Add();
                    column0.Width = 47;
                    column0.StyleID = "s74";
                    //sheet.Table.Columns.Add(58);
                    sheet.Table.Columns.Add(60);
                    sheet.Table.Columns.Add(150);
                    sheet.Table.Columns.Add(130);
                    sheet.Table.Columns.Add(70);
                    sheet.Table.Columns.Add(200);
                    // -----------------------------------------------
                    Row0 = sheet.Table.Rows.Add();
                    Row0.Height = 25;
                    Row0.AutoFitHeight = false;

                    Row0.Cells.Add("Image Types Available in Public Intake Portal", DataType.String, "s72");

                    cell = Row0.Cells.Add();
                    cell.StyleID = "s70";
                    cell = Row0.Cells.Add();
                    cell.StyleID = "s70";
                    cell = Row0.Cells.Add();
                    cell.StyleID = "s70";
                    cell = Row0.Cells.Add();
                    cell.StyleID = "s70";
                    cell = Row0.Cells.Add();
                    cell.StyleID = "s70";
                    // -----------------------------------------------
                    WorksheetRow Row1 = sheet.Table.Rows.Add();
                    Row1.Height = 27;
                    Row1.AutoFitHeight = false;
                    Row1.Cells.Add("SNo", DataType.String, "s75");
                    Row1.Cells.Add("Security", DataType.String, "s58");
                    Row1.Cells.Add("Image Type", DataType.String, "s58");
                    Row1.Cells.Add("Applicable for", DataType.String, "s58");
                    Row1.Cells.Add("Required", DataType.String, "s58");
                    Row1.Cells.Add("Income Types", DataType.String, "s58");


                    int introwcount = 0;

                    string strIncomeTypes = string.Empty;
                    string strTypes = string.Empty;
                    string strDesc = string.Empty;
                    string strRequired = string.Empty;


                    foreach (CommonEntity ImgTypes in propimagetypesCategory)
                    {

                        strIncomeTypes = string.Empty;
                        strTypes = "A";
                        strDesc = string.Empty;

                        ListItem listpipimg = pipImageTypes.Find(u => u.Value.ToString().Trim() == ImgTypes.Code.Trim().Trim());
                        if (listpipimg != null)
                        {
                            strIncomeTypes = listpipimg.ScreenCode;
                            if (listpipimg.ValueDisplayCode == "A")
                                strTypes = "Applicant";
                            if (listpipimg.ValueDisplayCode == "M")
                                strTypes = "Member";
                            if (listpipimg.ValueDisplayCode == "H")
                                strTypes = "All Household Members";


                            strDesc = listpipimg.ID.ToString();
                            strRequired = listpipimg.Amount == "Y" ? "Yes" : "No";



                            introwcount = introwcount + 1;
                            Row0 = sheet.Table.Rows.Add();

                            cell = Row0.Cells.Add(introwcount.ToString(), DataType.String, "s73");
                            cell = Row0.Cells.Add(ImgTypes.Extension.ToString().Trim(), DataType.String, "s66");

                            cell = Row0.Cells.Add(ImgTypes.Desc, DataType.String, "s66");
                            cell = Row0.Cells.Add(strTypes, DataType.String, "s66");
                            cell = Row0.Cells.Add(strRequired, DataType.String, "s66");
                            cell = Row0.Cells.Add(fillIncomesTypesDesc(_listIncomeTypes, strIncomeTypes), DataType.String, "s66");
                        }

                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);
                    book.Save(stream);
                    stream.Close();

                    FileInfo fiDownload = new FileInfo(PdfName);
                    /// Need to check for file exists, is local file, is allow to read, etc...
                    string name = fiDownload.Name;
                    using (FileStream fileStream = fiDownload.OpenRead())
                    {
                        Application.Download(fileStream, name);
                    }
                }
                else
                {
                    AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
                }



                //FileStream stream = new FileStream(PdfName, FileMode.Create);

                //book.Save(stream);
                //stream.Close();

                ////btnProcess_Click(btnProcess, new EventArgs());

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = strFileName;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);




            }
            catch (Exception ex)
            {
            }

        }

        #endregion

        private void btnFieldEdit_Click(object sender, EventArgs e)
        {
            if (btnFieldEdit.Text == "&Edit")
            {
                btnFieldEdit.Text = "&Save";
                btnFieldClose.Text = "&Cancel";
                cmbFieldAgency.Enabled = false; gvwPipFields.ReadOnly = false;
            }
            else
            {
                if (gvwImageTypes.Rows.Count > 0)
                {
                    MessageBox.Show("Are you sure to update PIP Field Controls data?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerFieldType);
                }
            }
        }

        private void btnFieldClose_Click(object sender, EventArgs e)
        {
            if (btnFieldClose.Text == "&Cancel")
            {
                btnFieldEdit.Text = "&Edit";
                btnFieldClose.Text = "&Close";
                cmbFieldAgency.Enabled = true; gvwPipFields.ReadOnly = true;
                GetFieldControlData();

            }
            else
            {
                this.Close();
            }
        }

        private void MessageBoxHandlerFieldType(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                try
                {

                    SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
                    con.Open();

                    string strselectFieldAgency = "00";
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        if (cmbFieldAgency.Items.Count > 0)
                        {
                            strselectFieldAgency = ((ListItem)cmbFieldAgency.SelectedItem).Value.ToString();
                        }
                    }

                    if (gvwPipFields.Rows.Count > 0)
                    {

                        foreach (DataGridViewRow gvrow in gvwPipFields.Rows)
                        {
                            string strCommandtext = "UPDATE PIPFLDCNTL SET PIPFLD_ENABLE = '" + (gvrow.Cells["gvcFieldEnable"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N") + "',PIPFLD_REQUIRE = '" + (gvrow.Cells["gvcFieldRequired"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N") + "',PIPFLD_SHARE = '" + (gvrow.Cells["gvcFieldShare"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N") + "', PIPFLD_DATE_LSTC = GETDATE(), PIPFLD_LSTC_OPERATOR = '" + BaseForm.UserID + "' WHERE PIPFLD_CODE = '" + (gvrow.Cells["gvtFieldCode"].Value.ToString().ToUpper()) + "' AND PIPFLD_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPFLD_AGY = '" + strselectFieldAgency + "'";
                            using (SqlCommand cmd = new SqlCommand(strCommandtext, con))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }

                        AlertBox.Show("Successfully Saved in Field Controls Maintenance Data");
                        GetFieldControlData();
                        btnFieldEdit.Text = "&Edit";
                        btnFieldClose.Text = "&Close";
                        cmbFieldAgency.Enabled = true; gvwPipFields.ReadOnly = true;
                    }



                    con.Close();
                }
                catch (Exception ex)
                {


                }

            }
        }

        private void cmbFieldAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFieldAgency.Items.Count > 0)
            {
                GetFieldControlData();
            }
        }

        private void gvwPipFields_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwPipFields.Rows.Count > 0)
            {
                int ColIdx = 0;
                int RowIdx = 0;
                ColIdx = gvwPipFields.CurrentCell.ColumnIndex;
                RowIdx = gvwPipFields.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == gvcFieldEnable.Index)
                    {
                        if (gvwPipFields.SelectedRows[0].Cells["gvcFieldEnable"].Value.ToString().ToUpper() == "FALSE")
                        {
                            gvwPipFields.SelectedRows[0].Cells["gvcFieldRequired"].Value = false;
                            gvwPipFields.SelectedRows[0].Cells["gvcFieldShare"].Value = false;
                        }
                    }
                    if (e.ColumnIndex == gvcFieldRequired.Index)
                    {
                        if (gvwPipFields.SelectedRows[0].Cells["gvcFieldRequired"].Value.ToString().ToUpper() == "TRUE")
                        {
                            gvwPipFields.SelectedRows[0].Cells["gvcFieldEnable"].Value = true;
                        }
                    }
                    if (e.ColumnIndex == gvcFieldShare.Index)
                    {
                        if (gvwPipFields.SelectedRows[0].Cells["gvcFieldShare"].Value.ToString().ToUpper() == "TRUE")
                        {
                            gvwPipFields.SelectedRows[0].Cells["gvcFieldEnable"].Value = true;
                        }
                    }
                }

            }
        }

        private void btnSettingClose_Click(object sender, EventArgs e)
        {
            if (btnSettingClose.Text == "&Cancel")
            {
                _errorProvider.SetError(txtForcePassword, null);

                btnSettingEdit.Text = "&Edit";
                txtForcePassword.Enabled = false;
                cmbSettingAgency.Enabled = cmbMailType.Enabled = true;

                btnSettingClose.Text = "&Close";

            }
            else
            {
                this.Close();
            }
        }

        private void btnSettingEdit_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtForcePassword, null);
            if (btnSettingEdit.Text == "&Edit")
            {
                btnSettingEdit.Text = "&Save";
                btnSettingClose.Text = "&Cancel";

                txtForcePassword.Enabled = true;
                cmbSettingAgency.Enabled = false;


            }
            else
            {
                if (txtForcePassword.Text == string.Empty)
                {
                    _errorProvider.SetError(txtForcePassword, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblForcePassword.Text));

                }
                else
                {

                    if (UPdateForcePassword() > 0)
                    {
                        btnSettingEdit.Text = "&Edit";
                        btnSettingClose.Text = "&Close";
                        txtForcePassword.Enabled = false;
                        cmbSettingAgency.Enabled = true;
                    }

                }
            }

        }

        int UPdateForcePassword()
        {

            int intstatus;
            try
            {
                string strsettingAgency = "00";

                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    strsettingAgency = ((ListItem)cmbSettingAgency.SelectedItem).Value.ToString();
                }


                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("UPDATE PIPAGENCY SET PIPAGY_FORCE_PWDAYS =" + txtForcePassword.Text + "  WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY='" + strsettingAgency + "'", con))
                {
                    intstatus = cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            catch (Exception ex)
            {

                intstatus = 0;
            }
            return intstatus;

        }

        void GetForcePassword()
        {
            string strselectAgency = "00";
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                if (cmbSettingAgency.Items.Count > 0)
                {
                    strselectAgency = ((ListItem)cmbSettingAgency.SelectedItem).Value.ToString();
                }
            }
            SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

            con.Open();

            using (SqlCommand cmdEmail = new SqlCommand("SELECT *  FROM PIPAGENCY WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY = '" + strselectAgency + "' ", con))
            {
                SqlDataAdapter daEmail = new SqlDataAdapter(cmdEmail);
                DataSet dsEmail = new DataSet();
                daEmail.Fill(dsEmail);
                if (dsEmail.Tables[0].Rows.Count > 0)
                {
                    txtForcePassword.Text = dsEmail.Tables[0].Rows[0]["PIPAGY_FORCE_PWDAYS"].ToString();
                }
                else
                {
                    txtForcePassword.Text = string.Empty;
                }
            }
            con.Close();
        }

        private void cmbSettingAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSettingAgency.Items.Count > 0)
            {
                GetForcePassword();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbControl.SelectedIndex == 1)
            {
                GetMessageDetails();
            }
            else if (tbControl.SelectedIndex == 2)
            {
                fillPIPREGUsers();
            }
            else if (tbControl.SelectedIndex == 3)
            {
                GetImageTypes();
            }
            else if (tbControl.SelectedIndex == 4)
            {
                GetFieldControlData();
            }
            else if (tbControl.SelectedIndex == 5)
            {
                GetForcePassword();
            }
            else if (tbControl.SelectedIndex == 6)
            {
                chkbRegistration.Focus();

                if (chkbRegistration.Checked)
                {
                    pnlRegisNotes.Enabled = false;
                }
                else
                {
                    pnlRegisNotes.Enabled = true;
                }

                GetRegistrationNotes();
            }
        }

        void fillPIPREGUsers()
        {
            string strFilterAgy = "00";
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                if (cmbRegAgency.Items.Count > 0)
                {
                    strFilterAgy = ((ListItem)cmbRegAgency.SelectedItem).Value.ToString();
                }
            }
            DataTable dt = PIPDATA.GetPIPIntakeSearchData(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, "NOTVEREMAILREG",
                BaseForm.BaseAgencyControlDetails.AgyShortName, strFilterAgy);

            dgvPIPRegUsers.Rows.Clear();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {

                    int rowIndex = dgvPIPRegUsers.Rows.Add(false, dr["PIPREG_FNAME"].ToString(), dr["PIPREG_LNAME"].ToString(), dr["PIPREG_EMAIL"].ToString(), Convert.ToDateTime(dr["PIPREG_DATE"].ToString()).ToString("MM/dd/yyyy"), dr["INTAKECNT"].ToString(), dr["PIPREG_ID"].ToString());

                    if (dr["INTAKECNT"].ToString() != "0")
                        dgvPIPRegUsers.Rows[rowIndex].Cells["colintakes"].ToolTipText = dr["PIPREG_FNAME"].ToString() + " has " + dr["INTAKECNT"].ToString() + " Intakes, but not submitted the records";
                    else
                        dgvPIPRegUsers.Rows[rowIndex].Cells["colintakes"].ToolTipText = dr["PIPREG_FNAME"].ToString() + " was not Activated his/her Account.";

                }
            }
        }

        private void cmbRegAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillPIPREGUsers();
        }

        private void btnDelRegusers_Click(object sender, EventArgs e)
        {
            if (dgvPIPRegUsers.Rows.Count > 0)
            {

                List<DataGridViewRow> lstSelRows = dgvPIPRegUsers.Rows.Cast<DataGridViewRow>().
                            Where(row => Convert.ToBoolean(row.Cells["colSel"].Value) == true).ToList();

                if (lstSelRows.Count > 0)
                {
                    MessageBox.Show("Do you want to delete the selected users?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxDeleteSelUsers);
                }
                else
                {
                    AlertBox.Show("Please select atleast one user to delete the record", MessageBoxIcon.Warning);
                }
            }
        }
        private void MessageBoxDeleteSelUsers(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                List<DataGridViewRow> lstSelRows = dgvPIPRegUsers.Rows.Cast<DataGridViewRow>().
                          Where(row => Convert.ToBoolean(row.Cells["colSel"].Value) == true).ToList();

                if (lstSelRows.Count > 0)
                {
                    string strstatus = "";
                    foreach (DataGridViewRow dRow in lstSelRows) {
                        strstatus = PIPDATA.DELNotverfRegUSERS(BaseForm.BaseLeanDataBaseConnectionString,dRow.Cells["colRegID"].Value.ToString() ,BaseForm.UserID, "DELNOTVERFUSERS");

                        dgvPIPRegUsers.Rows.Remove(dRow);
                    }

                    AlertBox.Show("Users deleted successfully.");
                }
            }
        }
        // Added by Vikash on 02/14/2025 for new Registration Control tab as per 2025 Enhancement Document - PIP Shut ON/OFF

        string RegisAllowed = string.Empty, RegisMessage = string.Empty;
        private void btnRegisSave_Click(object sender, EventArgs e)
        {
            if (ValidateRegistrationNotes())
            {
                if (chkbRegistration.Checked)
                {
                    RegisAllowed = "Y";
                    RegisMessage = txtRegisNotes.Text.Trim();//string.Empty;
                }
                else
                {
                    RegisAllowed = "N";
                    RegisMessage = txtRegisNotes.Text.Trim();
                }

                UpdateRegistraionStatus(RegisAllowed, RegisMessage);
            }
        }

        private bool ValidateRegistrationNotes()
        {
            bool isRegisNotesValid = true;

            if (!chkbRegistration.Checked)
            {
                if (string.IsNullOrEmpty(txtRegisNotes.Text.Trim()))
                {
                    AlertBox.Show("Please enter the Reason for not allowing Registraions", MessageBoxIcon.Warning);
                    isRegisNotesValid = false;
                }
                else
                {
                    isRegisNotesValid = true;
                }
            }
            else
                isRegisNotesValid = true;

            return isRegisNotesValid;
        }

        private void btnRegisCancel_Click(object sender, EventArgs e)
        {
            if (chkbRegistration.Checked)
            {
                txtRegisNotes.Text = string.Empty;
                pnlRegisNotes.Enabled = false;
            }
            else
            {
                pnlRegisNotes.Enabled = true;
            }
        }

        private void chkbRegistration_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbRegistration.Checked)
            {
                //txtRegisNotes.Text = string.Empty;

                pnlRegisNotes.Enabled = false;
            }
            else
            {
                pnlRegisNotes.Enabled = true;
            }
        }

        void UpdateRegistraionStatus(string isRegisAllow, string RegisMsg)
        {
            int intstatus;
            try
            {
                string strsettingAgency = "00";

                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    strsettingAgency = ((ListItem)cmbRegisAgy.SelectedItem).Value.ToString();
                }

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("UPDATE PIPAGENCY SET PIPAGY_ALLOW_REG ='" + isRegisAllow + "', PIPAGY_SHUT_NOTES = '" + RegisMsg + "'  WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY='" + strsettingAgency + "'", con))
                {
                    intstatus = cmd.ExecuteNonQuery();
                    AlertBox.Show("Saved Successfully");
                }

                //using (SqlCommand cmd = new SqlCommand("UPDATE PIPAGENCY SET PIPAGY_SHUT_NOTES ='" + RegisMsg + "'  WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY='" + strsettingAgency + "'", con))
                //{
                //    intstatus = cmd.ExecuteNonQuery();

                    //AlertBox.Show("Saved Successfully");
                //}

                con.Close();
            }
            catch (Exception ex)
            {
            }

        }

        private void cmbRegisAgy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRegisAgy.Items.Count > 0)
            {
                GetRegistrationNotes();
            }
        }

        private void btnRegisClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void GetRegistrationNotes()
        {
            string strselectAgency = "00";
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                if (cmbSettingAgency.Items.Count > 0)
                {
                    strselectAgency = ((ListItem)cmbRegisAgy.SelectedItem).Value.ToString();
                }
            }
            SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

            con.Open();

            using (SqlCommand cmdEmail = new SqlCommand("SELECT *  FROM PIPAGENCY WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY = '" + strselectAgency + "' ", con))
            {
                SqlDataAdapter daEmail = new SqlDataAdapter(cmdEmail);
                DataSet dsEmail = new DataSet();
                daEmail.Fill(dsEmail);
                if (dsEmail.Tables[0].Rows.Count > 0)
                {
                    txtRegisNotes.Text = dsEmail.Tables[0].Rows[0]["PIPAGY_SHUT_NOTES"].ToString();

                    if (dsEmail.Tables[0].Rows[0]["PIPAGY_ALLOW_REG"].ToString() == "N")
                        chkbRegistration.Checked = false;
                    else if(dsEmail.Tables[0].Rows[0]["PIPAGY_ALLOW_REG"].ToString() == "Y")
                        chkbRegistration.Checked = true;
                }
                else
                {
                    txtRegisNotes.Text = string.Empty;

                    if (dsEmail.Tables[0].Rows[0]["PIPAGY_ALLOW_REG"].ToString() == "N")
                        chkbRegistration.Checked = false;
                    else if (dsEmail.Tables[0].Rows[0]["PIPAGY_ALLOW_REG"].ToString() == "Y")
                        chkbRegistration.Checked = true;
                }
            }
            con.Close();
        }
    }
}


