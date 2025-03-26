#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.Forms;
using Captain.Common.Utilities;
using Captain.Common.Model.Parameters;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class AgencyNameControl : UserControl
    {
        private CaptainModel _model = null;
        public AgencyNameControl(BaseForm baseForm)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            try
            {
                txtAgencyNo.Validator = TextBoxValidation.IntegerValidator;
                Application = lblAgencyName;

            }
            catch
            {
            }
        }

        #region Public Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public Label Application { get; set; }

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public BaseForm BaseForm { get; set; }      
        #endregion

        private void txtAgenyno_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAgencyNo.Text))
            {
                                

                AgencyPartnerControl agencyControl = BaseForm.GetBaseUserControl() as AgencyPartnerControl;
                if (agencyControl != null)
                {
                    DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(string.Empty,
                                                           txtAgencyNo.Text, string.Empty);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblAgencyName.Text = ds.Tables[0].Rows[0]["AGYP_NAME"].ToString();
                    }

                    agencyControl.TxtAgencyCode.Text = txtAgencyNo.Text;
                    agencyControl.TxtAgencyNo_LostFocus(sender, e);
                    if (BaseForm.BaseAgencyNo == string.Empty)
                    {
                        txtAgencyNo.Text = string.Empty;
                    }
                  
                }
                else
                {
                    DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(string.Empty,
                                                           txtAgencyNo.Text, string.Empty);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (txtAgencyNo.Text != BaseForm.BaseAgencyNo)
                        {
                            ShowHierachyandAgencyNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, txtAgencyNo.Text, "Display");
                           
                        }
                        else
                        {
                            if (BaseForm.BaseTopApplSelect == "N")
                            {
                                ShowHierachyandAgencyNo(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, txtAgencyNo.Text, "Display");
                               
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtAgencyNo.Text.Trim()))
                        {
                            MessageBox.Show("Agency Code# : " + txtAgencyNo.Text + " Does Not Exist", "CAPSystems", MessageBoxButtons.OK);
                            txtAgencyNo.Text = BaseForm.BaseApplicationNo;
                        }

                    }
                }
                // btnAdvance.Focus();
            }
        }

      
        public void ShowHierachyandAgencyNo(string strAgency, string strDept, string strProg, string strYear1, string strAgencyNumber, string strDisplay)
        {

            CaseMstEntity caseMstEntity = null;
            List<CaseSnpEntity> caseSnpEntity = null;
            string strYear = strYear1;
            string _strAgencyNumber = strAgencyNumber;

            if (string.IsNullOrEmpty(strYear))
                strYear = "    ";

            string strAgencyName = strAgency + " - " + _model.lookupDataAccess.GetHierachyDescription("1", strAgency, strDept, strProg);
            string strDeptName = strDept + " - " + _model.lookupDataAccess.GetHierachyDescription("2", strAgency, strDept, strProg);
            string strProgName = strProg + " - " + _model.lookupDataAccess.GetHierachyDescription("3", strAgency, strDept, strProg);

            DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(string.Empty,
                                                         txtAgencyNo.Text, string.Empty);
            if (ds.Tables[0].Rows.Count>0)
            {
                

                BaseForm.GetAgencyDetails(ds.Tables[0].Rows[0], strAgencyName, strDeptName, strProgName, strYear.ToString(), string.Empty, strDisplay);
                if (strDisplay == "Display")
                    BaseForm.BaseTopApplSelect = "Y";
            }
            else
            {
                if (!string.IsNullOrEmpty(txtAgencyNo.Text.Trim()))
                {
                    MessageBox.Show("Agency Code# : " + txtAgencyNo.Text + " Does Not Exist", "CAPSystems", MessageBoxButtons.OK);
                    txtAgencyNo.Clear();
                }
                if (BaseForm.BaseTopApplSelect == "Y")
                    txtAgencyNo.Text = BaseForm.BaseAgencyNo;
            }

        }

      

        private void Navigation_Click(object sender, EventArgs e)
        {
            string Navigation_Option = string.Empty;
            bool Rec_Exists = false;
            if (sender == Btn_First)
                Navigation_Option = "First";
            else if (sender == BtnP10)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "RR";
            }
            else if (sender == BtnNxt)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "Next";
            }
            else if (sender == BtnPrev)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "Previous";
            }
            else if (sender == BtnN10)
            {
                if (string.IsNullOrEmpty(BaseForm.BaseAgencyNo))
                    Navigation_Option = "First";
                else
                    Navigation_Option = "FF";
            }
            else if (sender == BtnLast)
                Navigation_Option = "Last";


            DataSet ds = Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(Navigation_Option,
                                                         BaseForm.BaseAgencyNo, "NAVIGATE");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Rec_Exists = true;
                    txtAgencyNo.Text = ds.Tables[0].Rows[0]["AGYP_CODE"].ToString();
                    BaseForm.BaseAgencyNo = ds.Tables[0].Rows[0]["AGYP_CODE"].ToString();
                    
                    txtAgenyno_LostFocus(txtAgencyNo, EventArgs.Empty);
                }
            }

            if (!Rec_Exists)
            {
                string Error_Msg = string.Empty;
                switch (Navigation_Option)
                {
                    case "First":
                    case "RR":
                    case "Previous": Error_Msg = "You are Already at the First Record"; break;

                    case "Next":
                    case "FF":
                    case "Last": Error_Msg = "You are Already at the Last Record"; break;
                }
                MessageBox.Show(Error_Msg, "CAPTAIN");
            }
        }

        private void NavigationLoading(string strMainmenu)
        {
            BaseUserControl baseUserControl = BaseForm.GetBaseUserControl();
            if (baseUserControl != null)
            {
                if (strMainmenu != string.Empty)
                {
                    if (baseUserControl is AgencyPartnerControl)
                    {
                        (baseUserControl as AgencyPartnerControl).RefreshClearControl();
                    }
                }
               
                //if (baseUserControl is ClientIntakeControl)
                //{
                //    (baseUserControl as ClientIntakeControl).Refresh();
                //}
                //if (baseUserControl is Case3001Control)
                //{
                //    (baseUserControl as Case3001Control).Refresh();
                //}
               

            }
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            // Adv_search = true;
            
            AdvancedAgencypartnerSearch advancedAgencypartnerSearch = new AdvancedAgencypartnerSearch(BaseForm, true, true);
            advancedAgencypartnerSearch.FormClosed += new FormClosedEventHandler(On_ADV_SerachFormClosed);
            advancedAgencypartnerSearch.ShowDialog();
        }

        private void On_ADV_SerachFormClosed(object sender, FormClosedEventArgs e)
        {
            AdvancedAgencypartnerSearch form = sender as AdvancedAgencypartnerSearch;
            if (form.DialogResult == DialogResult.OK)
            {
                
                    BaseForm.BaseTopApplSelect = "Y";
                    BaseUserControl baseUserControl = BaseForm.GetBaseUserControl();
                    if (baseUserControl != null)
                    {
                        if (baseUserControl is AgencyPartnerControl)
                        {
                            AgencyPartnerControl agencyPartnerControl = BaseForm.GetBaseUserControl() as AgencyPartnerControl;
                            if (agencyPartnerControl != null)
                            {
                                agencyPartnerControl.TxtAgencyCode.Text = BaseForm.BaseAgencyNo;
                                agencyPartnerControl.TxtAgencyNo_LostFocus(sender, e);
                            }
                            
                        }
                        else
                        {
                           
                        }
                    }
                    
                
               
            }


        }

        string strkeyApp = string.Empty;
        private void txtAppNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {

            txtAgenyno_LostFocus(txtAgencyNo, EventArgs.Empty);

        }
        private void setTreeviewhierachy(string BusinessModuleID)
        {
            switch (BusinessModuleID)
            {
                case Consts.Applications.Code.Administration:
                    InitializeModule("MiddleBanner.gif", TreeType.Administration);
                    break;
                case Consts.Applications.Code.HeadStart:
                    InitializeModule("MiddleBanner.gif", TreeType.HeadStart);
                    break;
                case Consts.Applications.Code.CaseManagement:
                    InitializeModule("MiddleBanner.gif", TreeType.CaseManagement);
                    break;
                case Consts.Applications.Code.EnergyRI:
                    InitializeModule("MiddleBanner.gif", TreeType.EnergyRI);
                    break;
                case Consts.Applications.Code.EnergyCT:
                    InitializeModule("MiddleBanner.gif", TreeType.EnergyCT);
                    break;
                case Consts.Applications.Code.EmergencyAssistance:
                    InitializeModule("MiddleBanner.gif", TreeType.EmergencyAssistance);
                    break;
                case Consts.Applications.Code.AccountsReceivable:
                    InitializeModule("MiddleBanner.gif", TreeType.AccountsReceivable);
                    break;
                case Consts.Applications.Code.HousingWeatherization:
                    InitializeModule("MiddleBanner.gif", TreeType.HousingWeatherization);
                    break;
                case Consts.Applications.Code.DashBoard:
                    InitializeModule("MiddleBanner.gif", TreeType.DashBoard);
                    break;
                case Consts.Applications.Code.HealthyStart:
                    InitializeModule("MiddleBanner.gif", TreeType.HealthyStart);
                    break;
                //case Consts.Applications.Code.CEAPAssistance:
                //    InitializeModule("MiddleBanner.gif", TreeType.CEAPAssistance);
                //    break;
            }
        }

        private void InitializeModule(string headerTitleImage, TreeType treeViewType)
        {
            //TreeViewControllerParameter treeViewControllerParameter = null;

            //// pnlApplicationHeaderImage.BackgroundImage = new ImageResourceHandle(headerTitleImage);
            ////  pnlApplicationHeaderImage.Width = 30;
            //BaseForm.NavigationTreeView.Nodes.Clear();
            //string HIE = HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg; ;


            //treeViewControllerParameter = new TreeViewControllerParameter()
            //{
            //    TreeType = treeViewType,
            //    TreeView = BaseForm.NavigationTreeView,
            //    Hierarchy = HIE
            //};

            //BaseForm.TreeViewController.Initialize(treeViewControllerParameter);
        }

    }
}