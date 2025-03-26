#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB2012_ConditionsForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public CASB2012_ConditionsForm(string Field_Name, string Format_type, string data_dype, string max_length, string equalTO, string greater, string lesser, string Add_Crit)
        {
            InitializeComponent();

            Data_Type = data_dype;

            Max_Length = 0;
            if (!string.IsNullOrEmpty(max_length.Trim()))
                Max_Length = int.Parse(max_length);

            EqualTO = equalTO.Trim();
            Greater = greater.Trim();
            Lesser = lesser.Trim();
            Format_Type = Format_type.Trim();
            //Pass_AgyTabs_List = pass_agyTabs_list;

            switch (data_dype)
            {
                case "Text":
                    //this.Size = new System.Drawing.Size(412, 146);
                    this.Text = "Alphanumeric - Criteria Selection Form";
                    label11.Text = Field_Name;
                    Text_Panel.Visible = true;
                    this.Size = new System.Drawing.Size(this.Width, this.Height - (Agytab_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));
                    Txt_Alpha_Resp.Focus();
                    Txt_Alpha_Resp.MaxLength = Max_Length;
                    if (!string.IsNullOrEmpty(equalTO))
                        Txt_Alpha_Resp.Text = equalTO.Trim();

                    break;

                case "Numeric":
                    this.Text = "Numeric - Criteria Selection Form";
                    //this.Numeric_Panel.Location = new System.Drawing.Point(2, 2);
                    //this.Size = new System.Drawing.Size(412, 148);
                    label12.Text = Field_Name;
                    Numeric_Panel.Visible = true;
                    this.Size = new System.Drawing.Size(this.Width, this.Height - (Agytab_Panel.Height + Text_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

                    if (!string.IsNullOrEmpty(equalTO))
                        Txt_Equal.Text = equalTO.Trim();

                    if (!string.IsNullOrEmpty(Greater))
                        Txt_Greater.Text = Greater.Trim();

                    if (!string.IsNullOrEmpty(Lesser))
                        Txt_Lesser.Text = Lesser.Trim();

                    Txt_Equal.MaxLength = Txt_Greater.MaxLength = Txt_Lesser.MaxLength = Max_Length;

                    if (Field_Name == "Current Age")
                    {
                        Age_Asof_Panel.Visible = true;
                        if (!string.IsNullOrEmpty(Add_Crit.Trim()))
                        {
                            Asof_Date.Value = Convert.ToDateTime(Add_Crit.Trim());
                            Asof_Date.Checked = true;
                        }
                        else
                            Asof_Date.Value = DateTime.Today;
                    }
                    break;

                case "Date":
                    this.Text = "Date - Criteria Selection Form";
                    //this.Date_Panel.Location = new System.Drawing.Point(2, 2);
                   // this.Size = new System.Drawing.Size(412, 167);
                    label13.Text = Field_Name;

                    if (Field_Name == "As Of") // For this field we require only Equality condition
                    {
                        //Dt_Greater.Visible = Dt_Lesser.Visible = label8.Visible = label7.Visible = false;
                        Dt_Lesser.Visible = label7.Visible = false;
                        label9.Text = "As Of/From Date";
                        label8.Text = "To Date";
                        //Dt_Equal.Location = new System.Drawing.Point(161, 64);
                        //Dt_Greater.Location = new System.Drawing.Point(161, 93);
                        //label9.Location = new System.Drawing.Point(63, 68);
                        //label8.Location = new System.Drawing.Point(63, 95);
                    }
                    Date_Panel.Visible = true;
                    this.Size = new System.Drawing.Size(this.Width, this.Height - (Agytab_Panel.Height + Text_Panel.Height + Numeric_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

                    if (!string.IsNullOrEmpty(equalTO.Trim()))
                    {
                        Dt_Equal.Text = equalTO; Dt_Equal.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(Greater.Trim()))
                    {
                        Dt_Greater.Text = Greater; Dt_Greater.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(Lesser.Trim()))
                    {
                        Dt_Lesser.Text = Lesser; Dt_Lesser.Checked = true;
                    }

                    if (!string.IsNullOrEmpty(Add_Crit.Trim()))
                    {
                        if (Add_Crit == "Y")
                            Cb_Nulls.Checked = true;
                    }
                    break;

                case "Time":
                    this.Text = "Time - Criteria Selection Form";
                    //this.Time_Panel.Location = new System.Drawing.Point(2, 2);
                    //this.Size = new System.Drawing.Size(412, 167);
                    label15.Text = Field_Name;
                    Time_Panel.Visible = true;
                    this.Size = new System.Drawing.Size(this.Width, this.Height - (Agytab_Panel.Height + Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));
                    if (!string.IsNullOrEmpty(equalTO.Trim()))
                    {
                        Time_Equal.Text = equalTO; Time_Equal.Checked = true;
                    }


                    break;
            }
            set_TextBox_Field_Validations();
        }

        public CASB2012_ConditionsForm(string Field_Name, string equalTO, string notequalTO, List<AGYTABSEntity> pass_agyTabs_list, int delimiter_length)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_AgyTabs_List = pass_agyTabs_list;
            Delimiter_Length = delimiter_length;
            button2.Focus();
            //this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;

            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));
            Fill_AgyTabs_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<ZipCodeEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_ZIPEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            //this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height 
                ));
            Fill_Zipcode_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<STAFFMSTEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_STAFFMSTEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height
                ));
            Fill_STAFFMST_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<STAFFTRAINEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_STAFFTRAINEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height
                ));
            Fill_STAFFTrain_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CASEREFEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_ReferralEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height
                ));
            Fill_Referral_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CMBDCEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_BDCEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height
                ));
            Fill_Budget_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<AGCYPARTEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_PartnerEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height
                ));
            Fill_Partner_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CAMASTEntity> pass_hieentity)
        {
            InitializeComponent();
            label24.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_ActivityEntity = pass_hieentity;
            AgyType = agytype;

            lblCAMS.Text = "Service";
            button7.Focus();
            //this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //this.Size = new System.Drawing.Size(487, 239);
            //Agytab_Panel.Visible = true;

            //this.pnlCAMS.Location = new System.Drawing.Point(2, 2);
            //this.Size = new System.Drawing.Size(487, 282);
            pnlCAMS.Visible = true;

            panel15.Visible = false;

            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height +  Agytab_Panel.Height));
            //Fill_Activity_Grid();
            Fill_Activity_Grid1();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<MSMASTEntity> pass_hieentity)
        {
            InitializeComponent();
            label24.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_MilestoneEntity = pass_hieentity;
            AgyType = agytype;
            button7.Focus();
            lblCAMS.Text = "Outcome";

            //this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //this.Size = new System.Drawing.Size(487, 239);
            //Agytab_Panel.Visible = true;

         //   this.pnlCAMS.Location = new System.Drawing.Point(2, 2);
          //  this.Size = new System.Drawing.Size(487, 282);
            pnlCAMS.Visible = true;
            panel15.Visible = false;

            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + Agytab_Panel.Height));

            //Fill_Milestone_Grid();
            Fill_Milestone_Grid1();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CASEVDDEntity> pass_CASEVDDEntity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_CASEVDDEntity = pass_CASEVDDEntity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //  this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height ));
            Fill_Vendor_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<SaldefEntity> pass_SALDEFEntity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_SALDEFEntity = pass_SALDEFEntity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_SalName_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<SalquesEntity> pass_SALQUESEntity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_SALQUESEntity = pass_SALQUESEntity;
            AgyType = agytype;
            button2.Focus();
            //  this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //  this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_SalQues_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<SalqrespEntity> pass_salresp_Entity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            pass_SALRESP_Entity = pass_salresp_Entity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_SALRESP_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CaseHierarchyEntity> pass_hieentity, string Dashboard_hie)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_HIEEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            //this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);

            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Grid_HIE_Descriptions(AgyType, Dashboard_hie);
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<HierarchyEntity> pass_hie_entity, string Dashboard_hie)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Grid_Screen_HIE_Descriptions(AgyType, pass_hie_entity);
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<MATDEFEntity> pass_Mat_entity, string Dashboard_hie)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Grid_Matrix_Scale(AgyType, pass_Mat_entity);
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<MATQUESTEntity> pass_Mat_entity, string Dashboard_hie)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //   this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Grid_Mat_Questions(AgyType, pass_Mat_entity);
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, DataTable pass_dataTable)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_DataTable = pass_dataTable;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Grid_With_Table(agytype);
        }

        string TableName = string.Empty;
        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, DataTable pass_dataTable,string tableName)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";
            TableName = tableName;

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_DataTable = pass_dataTable;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Grid_With_Table(agytype);
        }


        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CustfldsEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_CustFldsEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            //  this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            //  this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_CUSTFLDS_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<CustRespEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_CustRespEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_CUSTRESP_Grid();
        }


        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<ChldTrckEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_TasksEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Tasks_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<HlsTrckEntity> pass_hieentity)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            Pass_HLSTasksEntity = pass_hieentity;
            AgyType = agytype;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_HTasks_Grid();
        }

        string Curr_Hie = "", Curr_Hie_Year = "", User_Name = "";
        public CASB2012_ConditionsForm(string Date_SW, string From_Date, string To_Date, string curr_hie, string curr_year, string user_name)
        {
            InitializeComponent();


            //Initialize_database_Exception_Handling_Parameters();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 0;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.Icon = null;

            Data_Type = "Date";

            //EqualTO = equalTO.Trim();
            Greater = To_Date.Trim();
            Lesser = From_Date.Trim();
            Format_Type = "Date";
            User_Name = user_name;
            //Pass_AgyTabs_List = pass_agyTabs_list;
           // this.CAMS_Work_Panel.Location = new System.Drawing.Point(2, 2);
           // this.Size = new System.Drawing.Size(367, 166);
            CAMS_Work_Panel.Visible = true;

            this.Size = new Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height  + pnlCAMS.Height + Agytab_Panel.Height));

            Curr_Hie = curr_hie;
            Curr_Hie_Year = curr_year;
            this.Text = "Work File Form";

            if (Date_SW == "P")
                Rb_Posting_Date.Checked = true;

            CAMS_From_Date.Value = DateTime.Now.AddMonths(-1);
            CAMS_From_Date.Checked = true;
            CAMS_To_date.Value = DateTime.Today;
            CAMS_To_date.Checked = true;

            //DateTime.Now.AddMonths(-1);

            //if (!string.IsNullOrEmpty(Greater.Trim()))
            //{
            //    CAMS_From_Date.Value = Convert.ToDateTime(Lesser);
            //    CAMS_From_Date.Checked = true;
            //}

            //if (!string.IsNullOrEmpty(Lesser.Trim()))
            //{
            //    CAMS_To_date.Value = Convert.ToDateTime(Greater);
            //    CAMS_To_date.Checked = true;
            //}
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<AGCYREPEntity> pass_Rep_entity, string Dashboard_hie)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            AgyType = agytype;
            Pass_RepresentEntity = pass_Rep_entity;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_Representative_Grid();
        }

        public CASB2012_ConditionsForm(string Field_Name, string agytype, string equalTO, string notequalTO, List<AGCYSEREntity> pass_ser_entity, string Dashboard_hie)
        {
            InitializeComponent();
            label14.Text = Field_Name;
            this.Text = "Multiple - Criteria Selection Form";

            EqualTO = equalTO.Trim();
            NotEqualTO = notequalTO.Trim();
            AgyType = agytype;
            Pass_PartSerEntity = pass_ser_entity;
            button2.Focus();
            // this.Agytab_Panel.Location = new System.Drawing.Point(2, 2);
            // this.Size = new System.Drawing.Size(487, 239);
            Agytab_Panel.Visible = true;
            this.Size = new System.Drawing.Size(this.Width, this.Height - (Text_Panel.Height + Numeric_Panel.Height + Date_Panel.Height + Time_Panel.Height + CAMS_Work_Panel.Height + pnlCAMS.Height));

            Fill_PartnerServices_Grid();
        }



        #region properties

        public string Data_Type { get; set; }

        public string Format_Type { get; set; }

        public int Max_Length { get; set; }

        public string EqualTO { get; set; }

        public string NotEqualTO { get; set; }

        public string Greater { get; set; }

        public string Lesser { get; set; }

        public int Delimiter_Length { get; set; }

        public List<AGYTABSEntity> Pass_AgyTabs_List { get; set; }

        public DataTable Pass_DataTable { get; set; }

        public string AgyType { get; set; }

        public List<CaseHierarchyEntity> Pass_HIEEntity { get; set; }

        public List<ZipCodeEntity> Pass_ZIPEntity { get; set; }

        public List<STAFFMSTEntity> Pass_STAFFMSTEntity { get; set; }

        public List<STAFFTRAINEntity> Pass_STAFFTRAINEntity { get; set; }

        public List<CASEREFEntity> Pass_ReferralEntity { get; set; }

        public List<CMBDCEntity> Pass_BDCEntity { get; set; }

        public List<CAMASTEntity> Pass_ActivityEntity { get; set; }

        public List<MSMASTEntity> Pass_MilestoneEntity { get; set; }

        public List<CASEVDDEntity> Pass_CASEVDDEntity { get; set; }

        public List<CustfldsEntity> Pass_CustFldsEntity { get; set; }

        public List<CustRespEntity> Pass_CustRespEntity { get; set; }

        public List<ChldTrckEntity> Pass_TasksEntity { get; set; }

        public List<HlsTrckEntity> Pass_HLSTasksEntity { get; set; }

        public List<SaldefEntity> Pass_SALDEFEntity { get; set; }

        public List<SalquesEntity> Pass_SALQUESEntity { get; set; }

        public List<SalqrespEntity> pass_SALRESP_Entity { get; set; }

        public List<AGCYREPEntity> Pass_RepresentEntity { get; set; }

        public List<AGCYSEREntity> Pass_PartSerEntity { get; set; }

        public List<AGCYPARTEntity> Pass_PartnerEntity { get; set; }
        #endregion


        private void Fill_Grid_With_Table(string AgyType)
        {
            string Desc = " ", Code = " ", Site_Code = " ", Press_grp_4_cat = "",Active=string.Empty;

            if (AgyType == "PRCAT")
            {
                Get_PressGrp_Table();
            }
            int rowIndex = 0;
            foreach (DataRow dr in Pass_DataTable.Rows)
            {
                switch (AgyType)
                {
                    case "WORKR": Desc = dr["NAME"].ToString().Trim(); Code = dr["PWH_CASEWORKER"].ToString().Trim(); Active = dr["PWR_INACTIVE_FLAG"].ToString().Trim(); break;
                    case "OPRTR": Desc = dr["PWR_EMPLOYEE_NO"].ToString().Trim(); Code = dr["PWR_EMPLOYEE_NO"].ToString().Trim(); Active = dr["PWR_INACTIVE_FLAG"].ToString().Trim(); break;
                    //case "SITES": Desc = dr["SITE_KEY"].ToString().Trim() + " - " + dr["SITE_NAME"].ToString().Trim(); Code = dr["SITE_KEY"].ToString().Trim(); break;
                    case "SITES": Desc = dr["SITE_NAME"].ToString().Trim(); Code = dr["SITE_NUMBER"].ToString().Trim(); Site_Code = dr["SITE_KEY"].ToString().Trim(); Active = (dr["SITE_ACTIVE"].ToString()).Trim(); break;
                    case "HIEAG": Desc = dr["HIE_NAME"].ToString().Trim(); Code = dr["HIE_AGENCY"].ToString().Trim(); this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false; this.Sel_Agy_Desc.Width = 380; break;
                    case "HIEDE": Desc = dr["HIE_NAME"].ToString().Trim(); Code = (dr["HIE_AGENCY"].ToString() + dr["HIE_DEPT"].ToString()).Trim(); this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false; this.Sel_Agy_Desc.Width = 380; break;
                    case "SERVS":
                    case "HIEPR": Desc = dr["HIE_NAME"].ToString().Trim(); Code = (dr["HIE_AGENCY"].ToString() + dr["HIE_DEPT"].ToString() + dr["HIE_PROGRAM"].ToString()).Trim(); this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false; this.Sel_Agy_Desc.Width = 380; break;
                    case "SPLAN": Desc = dr["sp0_description"].ToString().Trim(); Code = (dr["sp0_servicecode"].ToString()).Trim();Active= (dr["SP0_ACTIVE"].ToString()).Trim(); break;
                    case "PRCAT": Desc = dr["PREASSGRP_DESC"].ToString().Trim(); Code = (dr["PREASSGRP_SUBCODE"].ToString()).Trim(); this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false; this.Sel_Agy_Desc.Width = 380; break;
                    case "PRGRP": Desc = dr["PREASSGRP_DESC"].ToString().Trim(); Code = (dr["PREASSGRP_CODE"].ToString()).Trim(); this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false; this.Sel_Agy_Desc.Width = 380; break;
                }

                string ActiveDesc = string.Empty;

                if (AgyType == "SITES" || AgyType == "SPLAN")
                {
                    if (Active == "Y") ActiveDesc = "Active"; else if (Active == "N") ActiveDesc = "Inactive";
                }
                else if(AgyType=="OPRTR" || AgyType=="WORKR")
                {
                    if (Active == "Y") ActiveDesc = "Inactive"; else if (Active == "N") ActiveDesc = "Active";
                }

                Press_grp_4_cat = "";
                if (AgyType == "PRCAT")
                {
                    foreach (DataRow dr1 in Press_Grp.Rows)
                    {
                        if (dr["PREASSGRP_CODE"].ToString().Trim() == dr1["PREASSGRP_CODE"].ToString().Trim())
                        {
                            Code = dr1["PREASSGRP_CODE"].ToString().Trim() + "-" + Code;
                            Desc = dr1["PREASSGRP_DESC"].ToString().Trim() + " - " + Desc;
                            break;
                        }
                    }
                }


                //if (AgyType == "SITES")
                //{
                //    if (EqualTO.Contains("'" + Site_Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Tick, "E");
                //    else
                //        if (NotEqualTO.Contains("'" + Site_Code + "'"))
                //            AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Cross, "N");
                //        else
                //            AgyTab_Grid.Rows.Add(false, Code, Desc, Img_Blank, " ");
                //}
                //else
                {
                    if (EqualTO.Contains("'" + Code + "'"))
                        rowIndex= AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Tick, "E", ActiveDesc);
                    else
                        if (NotEqualTO.Contains("'" + Code + "'"))
                        rowIndex= AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Cross, "N", ActiveDesc);
                    else
                        rowIndex= AgyTab_Grid.Rows.Add(false, Code, Desc, Img_Blank, " ", ActiveDesc);


                    if (AgyType == "OPRTR" || AgyType== "WORKR")
                    {
                        if(Active == "Y")
                            AgyTab_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else if (Active == "N")
                        AgyTab_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
                //if (EqualTO.Contains("'" + Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Desc, Img_Tick, Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Desc, Img_Cross, Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Desc, Img_Blank, Code, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        DataTable Press_Grp = new DataTable();
        DataTable Press_Cat = new DataTable();
        private void Get_PressGrp_Table()
        {
            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetCaseWorker("1", "**", "**", "**");
            DataSet ds = Captain.DatabaseLayer.AdhocDB.Get_PressGrp_Table();
            if (ds.Tables.Count > 0)
            {
                Press_Grp = ds.Tables[0];
                Press_Cat = ds.Tables[1];
            }
        }

        //private void Fill_Sites_Grid()
        //{
        //    foreach (DataRow dr in Pass_DataTable.Rows)
        //    {
        //        if (EqualTO.Contains("'" + dr["PWH_CASEWORKER"] + "'"))
        //            AgyTab_Grid.Rows.Add(true, dr["PWH_CASEWORKER"], dr["NAME"], Img_Tick, "E");
        //        else
        //            if (NotEqualTO.Contains("'" + dr["PWH_CASEWORKER"] + "'"))
        //            AgyTab_Grid.Rows.Add(true, dr["PWH_CASEWORKER"], dr["NAME"], Img_Cross, "N");
        //        else
        //            AgyTab_Grid.Rows.Add(false, dr["PWH_CASEWORKER"], dr["NAME"], Img_Blank, " ");

        //        //if (EqualTO.Contains("'" + dr["PWH_CASEWORKER"] + "'"))
        //        //    AgyTab_Grid.Rows.Add(true, dr["NAME"], Img_Tick, dr["PWH_CASEWORKER"], "E");
        //        //else
        //        //    if (NotEqualTO.Contains("'" + dr["PWH_CASEWORKER"] + "'"))
        //        //        AgyTab_Grid.Rows.Add(true, dr["NAME"], Img_Cross, dr["PWH_CASEWORKER"], "N");
        //        //    else
        //        //        AgyTab_Grid.Rows.Add(false, dr["NAME"], Img_Blank, dr["PWH_CASEWORKER"], " ");
        //    }
        //    if (AgyTab_Grid.Rows.Count > 0)
        //        AgyTab_Grid.Rows[0].Selected = true;
        //}

        private void Fill_Zipcode_Grid()
        {
            Pass_ZIPEntity = Pass_ZIPEntity.OrderBy(u => u.InActive).ToList();
            string Disp_Code = ""; int row_Index = 0;
            foreach (ZipCodeEntity Entity in Pass_ZIPEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.InActive == "N") ActiveDesc = "Active"; else if (Entity.InActive== "Y") ActiveDesc = "Inactive";


                Disp_Code = "00000".Substring(0, 5 - Entity.Zcrzip.Trim().Length) + Entity.Zcrzip.Trim();
                if (EqualTO.Contains("'" + Entity.Zcrzip + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Disp_Code, Entity.Zcrcity, Img_Tick, "E", ActiveDesc); //Entity.Zcrzip
                else if (NotEqualTO.Contains("'" + Entity.Zcrzip + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Disp_Code, Entity.Zcrcity, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = AgyTab_Grid.Rows.Add(false, Disp_Code, Entity.Zcrcity, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.Zcrzip + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Zcrzip + " - " + Entity.Zcrcity, Img_Tick, Entity.Zcrzip, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Zcrzip + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Zcrzip + " - " + Entity.Zcrcity, Img_Cross, Entity.Zcrzip, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Zcrzip + " - " + Entity.Zcrcity, Img_Blank, Entity.Zcrzip, " ");

                if (Entity.InActive.Trim() == "Y")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;

            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_CUSTFLDS_Grid()
        {
            Pass_CustFldsEntity = Pass_CustFldsEntity.OrderBy(u => u.Active).ToList();
            int row_Index = 0;
            foreach (CustfldsEntity Entity in Pass_CustFldsEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";

                if (EqualTO.Contains("'" + Entity.CustCode + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.CustCode, Entity.CustDesc, Img_Tick, "E", ActiveDesc);
                else if (NotEqualTO.Contains("'" + Entity.CustCode + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.CustCode, Entity.CustDesc, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = AgyTab_Grid.Rows.Add(false, Entity.CustCode, Entity.CustDesc, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.CustCode + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.CustCode + " - " + Entity.CustDesc, Img_Tick, Entity.CustCode, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.CustCode + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.CustCode + " - " + Entity.CustDesc, Img_Cross, Entity.CustCode, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.CustCode + " - " + Entity.CustDesc, Img_Blank, Entity.CustCode, " ");

                if (Entity.Active.Trim() == "I")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_CUSTRESP_Grid()
        {
            Pass_CustRespEntity = Pass_CustRespEntity.OrderBy(u => u.status).ToList();
            //Sel_Agy_Code1.Visible = true;
            //Sel_Agy_Desc.Width = 327;
            int row_Index = 0;
            foreach (CustRespEntity Entity in Pass_CustRespEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.RspStatus == "A") ActiveDesc = "Active"; else if (Entity.RspStatus == "I") ActiveDesc = "Inactive";

                if (EqualTO.Contains("'" + Entity.DescCode + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.DescCode, Entity.RespDesc, Img_Tick, "E", ActiveDesc);
                else
                    if (NotEqualTO.Contains("'" + Entity.DescCode + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.DescCode, Entity.RespDesc, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = AgyTab_Grid.Rows.Add(false, Entity.DescCode, Entity.RespDesc, Img_Blank, " ", ActiveDesc);


                //if (EqualTO.Contains("'" + Entity.DescCode + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.DescCode + " - " + Entity.RespDesc, Img_Tick, Entity.DescCode, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.DescCode + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.DescCode + " - " + Entity.RespDesc, Img_Cross, Entity.DescCode, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.DescCode + " - " + Entity.RespDesc, Img_Blank, Entity.DescCode, " ");

                if (Entity.RspStatus.Trim() == "I")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Tasks_Grid()
        {
            this.AgyActive.Visible = false;this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;

            foreach (ChldTrckEntity Entity in Pass_TasksEntity)
            {
                if (EqualTO.Contains("'" + Entity.TASK + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.TASK, Entity.TASKDESCRIPTION, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.TASK + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.TASK, Entity.TASKDESCRIPTION, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.TASK, Entity.TASKDESCRIPTION, Img_Blank, " ");

                //if (EqualTO.Contains("'" + Entity.TASK + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.TASK + " - " + Entity.TASKDESCRIPTION, Img_Tick, Entity.TASK, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.TASK + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.TASK + " - " + Entity.TASKDESCRIPTION, Img_Cross, Entity.TASK, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.TASK + " - " + Entity.TASKDESCRIPTION, Img_Blank, Entity.TASK, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }


        private void Fill_HTasks_Grid()
        {
            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;
            foreach (HlsTrckEntity Entity in Pass_HLSTasksEntity)
            {
                if (EqualTO.Contains("'" + Entity.TASK + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.TASK, Entity.TASKDESCRIPTION, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.TASK + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.TASK, Entity.TASKDESCRIPTION, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.TASK, Entity.TASKDESCRIPTION, Img_Blank, " ");

                //if (EqualTO.Contains("'" + Entity.TASK + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.TASK + " - " + Entity.TASKDESCRIPTION, Img_Tick, Entity.TASK, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.TASK + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.TASK + " - " + Entity.TASKDESCRIPTION, Img_Cross, Entity.TASK, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.TASK + " - " + Entity.TASKDESCRIPTION, Img_Blank, Entity.TASK, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_STAFFMST_Grid()
        {
            Pass_STAFFMSTEntity = Pass_STAFFMSTEntity.OrderBy(u => Active).ToList();
            string Name = string.Empty; int row_Index = 0;
            foreach (STAFFMSTEntity Entity in Pass_STAFFMSTEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";

                Name = Entity.Last_Name + ", " + Entity.First_Name + ". " + Entity.Middle_Name;

                if (EqualTO.Contains("'" + Entity.Staff_Code + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.Staff_Code, Name, Img_Tick, "E", ActiveDesc);
                else
                    if (NotEqualTO.Contains("'" + Entity.Staff_Code + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.Staff_Code, Name, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = AgyTab_Grid.Rows.Add(false, Entity.Staff_Code, Name, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.Staff_Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Staff_Code + " - " + Name, Img_Tick, Entity.Staff_Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Staff_Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Staff_Code + " - " + Name, Img_Cross, Entity.Staff_Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Staff_Code + " - " + Name, Img_Blank, Entity.Staff_Code, " ");

                if (Entity.Active.Trim() == "I")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_STAFFTrain_Grid()
        {
            Pass_STAFFTRAINEntity = Pass_STAFFTRAINEntity.OrderBy(u => Active).ToList();
            string Name = string.Empty; int row_Index = 0;
            foreach (STAFFTRAINEntity Entity in Pass_STAFFTRAINEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";

                Name = Entity.Name;// + ", " + Entity.First_Name + ". " + Entity.Middle_Name;

                if (EqualTO.Contains("'" + Entity.ID + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.ID, Name, Img_Tick, "E", ActiveDesc);
                else
                    if (NotEqualTO.Contains("'" + Entity.ID + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.ID, Name, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = AgyTab_Grid.Rows.Add(false, Entity.ID, Name, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.Staff_Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Staff_Code + " - " + Name, Img_Tick, Entity.Staff_Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Staff_Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Staff_Code + " - " + Name, Img_Cross, Entity.Staff_Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Staff_Code + " - " + Name, Img_Blank, Entity.Staff_Code, " ");

                if (Entity.Active.Trim() == "I")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Referral_Grid()
        {
            Pass_ReferralEntity = Pass_ReferralEntity.OrderByDescending(u => u.Active).ToList();
            string Name = string.Empty;
            int row_Index = 0;
            foreach (CASEREFEntity Entity in Pass_ReferralEntity)
            {
                Name = Entity.Name1;

                string ActiveDesc = string.Empty;
                if (Entity.Active == "Y") ActiveDesc = "Active"; else if (Entity.Active == "N") ActiveDesc = "Inactive";

                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Tick, "E", ActiveDesc);
                else
                    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index = AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = AgyTab_Grid.Rows.Add(false, Entity.Code, Name, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");

                if (Entity.Active.Trim() == "N")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Budget_Grid()
        {
            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;
            string Name = string.Empty;
            foreach (CMBDCEntity Entity in Pass_BDCEntity)
            {
                Name = Entity.BDC_DESCRIPTION;

                if (EqualTO.Contains("'" + Entity.BDC_ID + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.BDC_ID, Name, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.BDC_ID + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.BDC_ID, Name, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.BDC_ID, Name, Img_Blank, " ");

            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Partner_Grid()
        {
            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;
            string Name = string.Empty;
            foreach (AGCYPARTEntity Entity in Pass_PartnerEntity)
            {
                Name = Entity.Name;

                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.Code, Name, Img_Blank, " ");

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Representative_Grid()
        {
            string Name = string.Empty; int row_Index = 0;
            Pass_RepresentEntity = Pass_RepresentEntity.OrderByDescending(u => u.AGYR_REP_ACTIVE).ToList();
            if (Pass_RepresentEntity.Count > 0)
            {
                foreach (AGCYREPEntity Entity in Pass_RepresentEntity)
                {
                    Name = string.Empty;

                    string ActiveDesc = string.Empty;
                    if (Entity.AGYR_REP_ACTIVE == "Y") ActiveDesc = "Active"; else if (Entity.AGYR_REP_ACTIVE == "N") ActiveDesc = "Inactive";

                    if (!string.IsNullOrEmpty(Entity.FName.Trim()))
                        Name = Entity.FName.Trim();
                    if (!string.IsNullOrEmpty(Entity.LName.Trim()))
                        Name = Name + " " + Entity.LName.Trim();

                    if (EqualTO.Contains("'" + Entity.RepCode + "'"))
                        row_Index = AgyTab_Grid.Rows.Add(true, Entity.RepCode, Name, Img_Tick, "E");
                    else
                        if (NotEqualTO.Contains("'" + Entity.RepCode + "'"))
                        row_Index = AgyTab_Grid.Rows.Add(true, Entity.RepCode, Name, Img_Cross, "N");
                    else
                        row_Index = AgyTab_Grid.Rows.Add(false, Entity.RepCode, Name, Img_Blank, " ");

                    if (Entity.AGYR_REP_ACTIVE.Trim() == "N")
                        AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;

                }
                if (AgyTab_Grid.Rows.Count > 0)
                    AgyTab_Grid.Rows[0].Selected = true;
            }
        }

        private void Fill_PartnerServices_Grid()
        {
            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;
            string Name = string.Empty;
            if (Pass_PartSerEntity.Count > 0)
            {
                foreach (AGCYSEREntity Entity in Pass_PartSerEntity)
                {
                    Name = Entity.Service.Trim();

                    if (EqualTO.Contains("'" + Entity.SerCode + "'"))
                        AgyTab_Grid.Rows.Add(true, Entity.SerCode, Name, Img_Tick, "E");
                    else
                        if (NotEqualTO.Contains("'" + Entity.SerCode + "'"))
                        AgyTab_Grid.Rows.Add(true, Entity.SerCode, Name, Img_Cross, "N");
                    else
                        AgyTab_Grid.Rows.Add(false, Entity.SerCode, Name, Img_Blank, " ");

                }
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Activity_Grid()
        {
            string Name = string.Empty;
            foreach (CAMASTEntity Entity in Pass_ActivityEntity)
            {
                Name = Entity.Desc;

                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.Code, Name, Img_Blank, " ");

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Activity_Grid1()
        {
            gvCAMS.Rows.Clear();

            List<CAMASTEntity> CAList = new List<CAMASTEntity>();
            if (txtCAMS.Text.Trim() != string.Empty)
                CAList = Pass_ActivityEntity.FindAll(x => x.Desc.ToUpper().Contains(txtCAMS.Text.ToUpper()));
            else CAList = Pass_ActivityEntity;

            CAList = CAList.OrderBy(u => u.Active).ToList();

            int row_Index = 0;
            string Name = string.Empty;
            foreach (CAMASTEntity Entity in CAList)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";

                Name = Entity.Desc;

                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index = gvCAMS.Rows.Add(true, Entity.Code, Name, Img_Tick, "E",ActiveDesc);
                else if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index = gvCAMS.Rows.Add(true, Entity.Code, Name, Img_Cross, "N",ActiveDesc);
                else
                    row_Index = gvCAMS.Rows.Add(false, Entity.Code, Name, Img_Blank, " ",ActiveDesc);

                if (Entity.Active.Trim() == "I")
                    gvCAMS.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;

            }
            if (gvCAMS.Rows.Count > 0)
                gvCAMS.Rows[0].Selected = true;
        }


        private void Fill_Milestone_Grid()
        {
            string Name = string.Empty;
            foreach (MSMASTEntity Entity in Pass_MilestoneEntity)
            {
                Name = Entity.Desc;
                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.Code, Name, Img_Blank, " ");

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Milestone_Grid1()
        {
            gvCAMS.Rows.Clear();

            List<MSMASTEntity> MSList = new List<MSMASTEntity>();
            if (txtCAMS.Text.Trim() != string.Empty)
                MSList = Pass_MilestoneEntity.FindAll(x => x.Desc.ToUpper().Contains(txtCAMS.Text.ToUpper()));
            else MSList = Pass_MilestoneEntity;

            MSList = MSList.OrderBy(u => u.Active).ToList();

            int row_Index = 0;
            string Name = string.Empty;
            foreach (MSMASTEntity Entity in MSList)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";

                Name = Entity.Desc;
                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index = gvCAMS.Rows.Add(true, Entity.Code, Name, Img_Tick, "E", ActiveDesc);
                else if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                       row_Index = gvCAMS.Rows.Add(true, Entity.Code, Name, Img_Cross, "N", ActiveDesc);
                else
                    row_Index = gvCAMS.Rows.Add(false, Entity.Code, Name, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");

                if (Entity.Active.Trim() == "I")
                    gvCAMS.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (gvCAMS.Rows.Count > 0)
                gvCAMS.Rows[0].Selected = true;
        }

        private void Fill_Vendor_Grid()
        {
            int row_Index = 0;
            string Name = string.Empty;

            Pass_CASEVDDEntity = Pass_CASEVDDEntity.OrderBy(u => u.Active).ToList();
            foreach (CASEVDDEntity Entity in Pass_CASEVDDEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";

                Name = Entity.Name;
                if (EqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index=AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Tick, "E", ActiveDesc);
                else
                    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                    row_Index=AgyTab_Grid.Rows.Add(true, Entity.Code, Name, Img_Cross, "N", ActiveDesc);
                else
                    row_Index=AgyTab_Grid.Rows.Add(false, Entity.Code, Name, Img_Blank, " ", ActiveDesc);


                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");

                if (Entity.Active.Trim() == "I")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_SalName_Grid()
        {
            int row_Index = 0;
            string Name = string.Empty;

            Pass_SALDEFEntity = Pass_SALDEFEntity.OrderBy(u => u.SALD_ACTIVE).ToList();

            foreach (SaldefEntity Entity in Pass_SALDEFEntity)
            {
                string ActiveDesc = string.Empty;
                if (Entity.SALD_ACTIVE == "A") ActiveDesc = "Active"; else if (Entity.SALD_ACTIVE == "I") ActiveDesc = "Inactive";

                Name = Entity.SALD_NAME;
                if (EqualTO.Contains("'" + Entity.SALD_ID + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.SALD_ID, Name, Img_Tick, "E", ActiveDesc);
                else
                    if (NotEqualTO.Contains("'" + Entity.SALD_ID + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.SALD_ID, Name, Img_Cross, "N", ActiveDesc);
                else
                    AgyTab_Grid.Rows.Add(false, Entity.SALD_ID, Name, Img_Blank, " ", ActiveDesc);

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");

                if (Entity.SALD_ACTIVE.Trim() == "I")
                    AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;

            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_SalQues_Grid()
        {
            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;
            string Name = string.Empty;
            foreach (SalquesEntity Entity in Pass_SALQUESEntity)
            {
                Name = Entity.SALQ_DESC;
                if (EqualTO.Contains("'" + Entity.SALQ_ID + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.SALQ_ID, Name, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.SALQ_ID + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.SALQ_ID, Name, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.SALQ_ID, Name, Img_Blank, " ");

                //if (EqualTO.Contains("'" + Entity.Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Tick, Entity.Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code + " - " + Name, Img_Cross, Entity.Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code + " - " + Name, Img_Blank, Entity.Code, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_SALRESP_Grid()
        {
            //Sel_Agy_Code1.Visible = true;
            //Sel_Agy_Desc.Width = 327;

            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;

            foreach (SalqrespEntity Entity in pass_SALRESP_Entity)
            {
                if (EqualTO.Contains("'" + Entity.SALQR_CODE + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.SALQR_CODE, Entity.SALQR_DESC, Img_Tick, "E");
                else
                    if (NotEqualTO.Contains("'" + Entity.SALQR_CODE + "'"))
                    AgyTab_Grid.Rows.Add(true, Entity.SALQR_CODE, Entity.SALQR_DESC, Img_Cross, "N");
                else
                    AgyTab_Grid.Rows.Add(false, Entity.SALQR_CODE, Entity.SALQR_DESC, Img_Blank, " ");


                //if (EqualTO.Contains("'" + Entity.DescCode + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.DescCode + " - " + Entity.RespDesc, Img_Tick, Entity.DescCode, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.DescCode + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.DescCode + " - " + Entity.RespDesc, Img_Cross, Entity.DescCode, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.DescCode + " - " + Entity.RespDesc, Img_Blank, Entity.DescCode, " ");
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }


        private void Fill_Grid_HIE_Descriptions(string AgyType, string Dashboard_hie)
        {
            string Desc = " ", Code = " ", DashBoard_Agy = Dashboard_hie.Substring(0, 2), DashBoard_Dept = Dashboard_hie.Substring(3, 2), DashBoard_Prog = Dashboard_hie.Substring(6, 2), Dashboard_Code = string.Empty, Hie_Code_To_Compare = string.Empty;
            bool Fill_All = false;

            this.AgyActive.Visible = false;this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;

            switch (AgyType)
            {
                case "HIEDE": Hie_Code_To_Compare = (DashBoard_Agy == "**" ? "**" : DashBoard_Agy); break;
                case "SERVS":
                case "HIEPR": Hie_Code_To_Compare = (DashBoard_Dept == "**" ? (DashBoard_Agy == "**" ? "**" : DashBoard_Agy) + "**" : DashBoard_Agy + DashBoard_Dept); break;
            }


            foreach (CaseHierarchyEntity Entity in Pass_HIEEntity)
            {
                switch (AgyType)
                {
                    case "HIEAG": Code = Entity.Agency; Desc = Code + " - " + Entity.HierarchyName.Trim(); Dashboard_Code = DashBoard_Agy; Fill_All = Dashboard_Code == "**" ? true : false; break;
                    case "HIEDE": Code = (Entity.Agency + Entity.Dept); Desc = Code + " - " + Entity.HierarchyName.Trim(); Dashboard_Code = DashBoard_Agy + DashBoard_Dept; Fill_All = Dashboard_hie.Substring(3, 2) == "**" ? true : false; break;
                    case "SERVS":
                    case "HIEPR": Code = (Entity.Agency + Entity.Dept + Entity.Prog); Desc = Code + " - " + Entity.HierarchyName.Trim(); Dashboard_Code = DashBoard_Agy + DashBoard_Dept + DashBoard_Prog; Fill_All = Dashboard_hie.Substring(6, 2) == "**" ? true : false; break;
                }

                if ((AgyType == "HIEAG" && (Fill_All || Dashboard_Code == Code)) || AgyType == "SERVS" ||
                    (AgyType == "HIEDE" && ((Fill_All && (Dashboard_Code.Substring(0, 2) == Entity.Agency || Hie_Code_To_Compare == "**")) || Dashboard_Code == Code)) ||
                    ((AgyType == "HIEPR") && ((Fill_All && (Hie_Code_To_Compare == "****" || (Dashboard_Code.Substring(0, 2) == Entity.Agency && DashBoard_Dept == "**") || (Hie_Code_To_Compare == Entity.Agency + Entity.Dept)) || Dashboard_Code == Code))))
                {
                    if (EqualTO.Contains("'" + Code + "'"))
                        AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Tick, "E");
                    else
                        if (NotEqualTO.Contains("'" + Code + "'"))
                        AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Cross, "N");
                    else
                        AgyTab_Grid.Rows.Add(false, Code, Desc, Img_Blank, " ");

                    //if (EqualTO.Contains("'" + Code + "'"))
                    //    AgyTab_Grid.Rows.Add(true, Desc, Img_Tick, Code, "E");
                    //else
                    //    if (NotEqualTO.Contains("'" + Code + "'"))
                    //        AgyTab_Grid.Rows.Add(true, Desc, Img_Cross, Code, "N");
                    //    else
                    //        AgyTab_Grid.Rows.Add(false, Desc, Img_Blank, Code, " ");
                }
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Grid_Screen_HIE_Descriptions(string AgyType, List<HierarchyEntity> Ent)
        {
            string Desc = " ", Code = " ";

            this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
            this.Sel_Agy_Desc.Width = 380;
            foreach (HierarchyEntity Entity in Ent)
            {
                Code = Entity.Agency + Entity.Dept + Entity.Prog;
                Desc = Code + " - " + Entity.HirarchyName.Trim();

                if (!Entity.Code.Contains('*'))
                {
                    if (EqualTO.Contains("'" + Code + "'"))
                        AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Tick, "E");
                    else
                        if (NotEqualTO.Contains("'" + Code + "'"))
                        AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Cross, "N");
                    else
                        AgyTab_Grid.Rows.Add(false, Code, Desc, Img_Blank, " ");

                    //if (EqualTO.Contains("'" + Code + "'"))
                    //    AgyTab_Grid.Rows.Add(true, Desc, Img_Tick, Code, "E");
                    //else
                    //    if (NotEqualTO.Contains("'" + Code + "'"))
                    //        AgyTab_Grid.Rows.Add(true, Desc, Img_Cross, Code, "N");
                    //    else
                    //        AgyTab_Grid.Rows.Add(false, Desc, Img_Blank, Code, " ");
                }
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Grid_Matrix_Scale(string AgyType, List<MATDEFEntity> Ent)
        {
            List<MATDEFEntity> Scales = new List<MATDEFEntity>();
            Scales = Ent.FindAll(u => u.Scale_Code != "0");

            if (Scales.Count > 0)
            {
                this.AgyActive.Visible = false; this.AgyActive.ShowInVisibilityMenu = false;
                this.Sel_Agy_Desc.Width = 380;
            }

            string Desc = " ", Code = " ";
            int row_Index = 0;
            foreach (MATDEFEntity Entity in Ent)
            {
                string ActiveDesc = string.Empty;
                Code = Entity.Mat_Code;
                //Desc = Code + " - " + Entity.Desc.Trim();
                Desc = Entity.Desc.Trim();
                if (Entity.Scale_Code != "0")
                {
                    //Code = Code + " " + Entity.Scale_Code;
                    Code = Entity.Scale_Code;
                    //Desc = "Mat-" + Entity.Mat_Code + " Scl-" + Entity.Scale_Code + " - " + Entity.Desc.Trim();
                    Desc = Entity.Desc.Trim();
                }
                else
                {
                    
                    if (Entity.Active == "A") ActiveDesc = "Active"; else if (Entity.Active == "I") ActiveDesc = "Inactive";
                }

                {
                    if (EqualTO.Contains("'" + Code + "'"))
                        row_Index=AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Tick, "E", ActiveDesc);
                    else
                        if (NotEqualTO.Contains("'" + Code + "'"))
                        row_Index=AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Cross, "N", ActiveDesc);
                    else
                        row_Index=AgyTab_Grid.Rows.Add(false, Code, Desc, Img_Blank, " ", ActiveDesc);

                    //if (EqualTO.Contains("'" + Code + "'"))
                    //    AgyTab_Grid.Rows.Add(true, Desc, Img_Tick, Code, "E");
                    //else
                    //    if (NotEqualTO.Contains("'" + Code + "'"))
                    //        AgyTab_Grid.Rows.Add(true, Desc, Img_Cross, Code, "N");
                    //    else
                    //        AgyTab_Grid.Rows.Add(false, Desc, Img_Blank, Code, " ");

                    if (Entity.Active.Trim() == "I" && Entity.Scale_Code=="0")
                        AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void Fill_Grid_Mat_Questions(string AgyType, List<MATQUESTEntity> Ent)
        {
            string Desc = " ", Code = " ";

            Ent= Ent.OrderByDescending(u=>u.QuesActive).ToList();
            int row_Index = 0;
            foreach (MATQUESTEntity Entity in Ent)
            {
                string ActiveDesc = string.Empty;
                if (Entity.QuesActive == "Y") ActiveDesc = "Active"; else if (Entity.QuesActive == "N") ActiveDesc = "Inactive";

                Code = Entity.Code;
                //Desc = Code + " - " + Entity.Desc.Trim();
                Desc = Entity.Desc.Trim();
                //if (Entity.Code != "0")
                {
                    //Code = Code + " " + Entity.Scale_Code;
                    Code = Entity.Code;
                    //Desc = "Mat-" + Entity.Mat_Code + " Scl-" + Entity.Scale_Code + " - " + Entity.Desc.Trim();
                    Desc = Entity.Desc.Trim();
                }

                {
                    if (EqualTO.Contains("'" + Code + "'"))
                        row_Index = AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Tick, "E", ActiveDesc);
                    else if (NotEqualTO.Contains("'" + Code + "'"))
                         row_Index = AgyTab_Grid.Rows.Add(true, Code, Desc, Img_Cross, "N", ActiveDesc);
                    else
                        row_Index = AgyTab_Grid.Rows.Add(false, Code, Desc, Img_Blank, " ", ActiveDesc);

                    //if (EqualTO.Contains("'" + Code + "'"))
                    //    AgyTab_Grid.Rows.Add(true, Desc, Img_Tick, Code, "E");
                    //else
                    //    if (NotEqualTO.Contains("'" + Code + "'"))
                    //        AgyTab_Grid.Rows.Add(true, Desc, Img_Cross, Code, "N");
                    //    else
                    //        AgyTab_Grid.Rows.Add(false, Desc, Img_Blank, Code, " ");

                    if (Entity.QuesActive.Trim() != "Y")
                        AgyTab_Grid.Rows[row_Index].DefaultCellStyle.ForeColor = Color.Red;

                }
            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }


        private void Btn_Txt_Save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            AlertBox.Show("Saved Successfully");
        }

        private void Btn_Txt_Close_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void Btn_Txt_Clear_Click(object sender, EventArgs e)
        {
            Txt_Alpha_Resp.Clear();
        }

        private void Btn_Num_Clear_Click(object sender, EventArgs e)
        {
            Txt_Equal.Clear();
            Txt_Greater.Clear();
            Txt_Lesser.Clear();
        }

        private void Btn_DT_Clear_Click(object sender, EventArgs e)
        {
            Dt_Equal.Value = DateTime.Today; Dt_Equal.Checked = false;
            Dt_Greater.Value = DateTime.Today; Dt_Greater.Checked = false;
            Dt_Lesser.Value = DateTime.Today; Dt_Lesser.Checked = false;
        }


        public string Get_Text_Cindition()
        {
            string Txt_Condition = " ";

            if (!(string.IsNullOrEmpty(Txt_Alpha_Resp.Text.Trim())))
                Txt_Condition = Txt_Alpha_Resp.Text.Trim();

            return Txt_Condition;
        }

        public string[] Get_Numeric_Cindition()
        {
            string[] Num_Conditions = new string[4];
            Num_Conditions[0] = Num_Conditions[1] = Num_Conditions[2] = Num_Conditions[3] = " ";

            if (!(string.IsNullOrEmpty(Txt_Equal.Text.Trim())))
                Num_Conditions[0] = Txt_Equal.Text.Trim();

            if (!(string.IsNullOrEmpty(Txt_Greater.Text.Trim())))
                Num_Conditions[1] = Txt_Greater.Text.Trim();

            if (!(string.IsNullOrEmpty(Txt_Lesser.Text.Trim())))
                Num_Conditions[2] = Txt_Lesser.Text.Trim();

            if (Asof_Date.Checked)
                Num_Conditions[3] = Asof_Date.Value.ToShortDateString();

            return Num_Conditions;
        }

        public string[] Get_Date_Cindition()
        {
            string[] Date_Conditions = new string[4];
            Date_Conditions[0] = Date_Conditions[1] = Date_Conditions[2] = Date_Conditions[3] = " ";

            if (Dt_Equal.Checked)
                Date_Conditions[0] = Dt_Equal.Value.ToShortDateString();

            if (Dt_Greater.Checked)
                Date_Conditions[1] = Dt_Greater.Value.ToShortDateString();

            if (Dt_Lesser.Checked)
                Date_Conditions[2] = Dt_Lesser.Value.ToShortDateString();

            if (Cb_Nulls.Checked)
                Date_Conditions[3] = "Y";

            return Date_Conditions;
        }

        public string[] Get_CAMS_WorkFile_Date_Cindition()
        {
            string[] Date_Conditions = new string[3];
            Date_Conditions[0] = Date_Conditions[1] = Date_Conditions[2] = " ";

            Date_Conditions[0] = "A";
            if (Rb_Posting_Date.Checked)
                Date_Conditions[0] = "P";

            if (CAMS_From_Date.Checked)
                Date_Conditions[1] = CAMS_From_Date.Value.ToShortDateString();

            if (CAMS_To_date.Checked)
                Date_Conditions[2] = CAMS_To_date.Value.ToShortDateString();

            return Date_Conditions;
        }

        public string Get_Time_Cindition()
        {
            string Time = " ";

            if (Time_Equal.Checked)
                Time = Time_Equal.Value.ToShortTimeString();

            return Time;
        }

        private void Btn_Num_Save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            AlertBox.Show("Saved Successfully");
        }

        private void Txt_Alpha_Resp_LostFocus(object sender, EventArgs e)
        {
            if (Format_Type == "P" && (!string.IsNullOrEmpty(Txt_Alpha_Resp.Text.Trim())))
            {
                if (Max_Length > 0)
                {
                    string Tmp_Str = string.Empty;
                    for (int i = 1; i <= Max_Length; i++)
                        Tmp_Str += "0";

                    Txt_Alpha_Resp.Text = Tmp_Str.Substring(0, Max_Length - Txt_Alpha_Resp.Text.Length) + Txt_Alpha_Resp.Text;
                }

            }
        }


        private void set_TextBox_Field_Validations()
        {
            switch (Format_Type)
            {
                case "E":  // Telephone
                case "Y":  // Age
                case "S":  // SSN
                case "P":  // Positive
                case "N":  // Numeric
                    switch (Data_Type)
                    {
                        case "Text":
                            Txt_Alpha_Resp.Multiline = false;
                            Txt_Alpha_Resp.Validator = TextBoxValidation.IntegerValidator; break;
                        case "Numeric":
                            Txt_Equal.Validator = TextBoxValidation.IntegerValidator;
                            Txt_Greater.Validator = TextBoxValidation.IntegerValidator;
                            Txt_Lesser.Validator = TextBoxValidation.IntegerValidator;
                            break;
                    }
                    break;

                case "%":  // %
                case "F":  // Decimal 
                    switch (Data_Type)
                    {
                        case "Text": Txt_Alpha_Resp.Validator = TextBoxValidation.FloatValidator; break;
                        case "Numeric":
                            Txt_Equal.Validator = TextBoxValidation.FloatValidator;
                            Txt_Greater.Validator = TextBoxValidation.FloatValidator;
                            Txt_Lesser.Validator = TextBoxValidation.FloatValidator;
                            break;
                    }
                    break;
            }
        }


        string Img_Blank = Consts.Icons.ico_Blank;// new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        string Img_Tick = Consts.Icons.ico_Tick; //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");
        string Img_Cross = Consts.Icons.ico_Cross; //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("cross.ico");
        private void Fill_AgyTabs_Grid()
        {
            Pass_AgyTabs_List = Pass_AgyTabs_List.OrderByDescending(u => u.Active).ToList();
            int rowIndex = 0;
            foreach (AGYTABSEntity Entity in Pass_AgyTabs_List)
            {
                string ActiveDesc = string.Empty;
                if (Entity.Active == "Y") ActiveDesc = "Active"; else if (Entity.Active == "N") ActiveDesc = "Inactive";

                if (Delimiter_Length == 0)
                {
                    if (EqualTO.Contains("'" + Entity.Table_Code + "'"))
                        rowIndex=AgyTab_Grid.Rows.Add(true, Entity.Table_Code, Entity.Code_Desc, Img_Tick, "E", ActiveDesc);
                    else if (NotEqualTO.Contains("'" + Entity.Table_Code + "'"))
                        rowIndex = AgyTab_Grid.Rows.Add(true, Entity.Table_Code, Entity.Code_Desc, Img_Cross, "N",ActiveDesc);
                    else
                        rowIndex = AgyTab_Grid.Rows.Add(false, Entity.Table_Code, Entity.Code_Desc, Img_Blank, " ", ActiveDesc);
                }
                else
                {
                    if (EqualTO.Contains(Entity.Table_Code))
                        rowIndex = AgyTab_Grid.Rows.Add(true, Entity.Table_Code, Entity.Code_Desc, Img_Tick, "E", ActiveDesc);
                    else if (NotEqualTO.Contains(Entity.Table_Code))
                        rowIndex = AgyTab_Grid.Rows.Add(true, Entity.Table_Code, Entity.Code_Desc, Img_Cross, "N", ActiveDesc);
                    else
                        rowIndex = AgyTab_Grid.Rows.Add(false, Entity.Table_Code, Entity.Code_Desc, Img_Blank, " ", ActiveDesc);
                }

                //if (EqualTO.Contains("'" + Entity.Table_Code + "'"))
                //    AgyTab_Grid.Rows.Add(true, Entity.Code_Desc, Img_Tick, Entity.Table_Code, "E");
                //else
                //    if (NotEqualTO.Contains("'" + Entity.Table_Code + "'"))
                //        AgyTab_Grid.Rows.Add(true, Entity.Code_Desc, Img_Cross, Entity.Table_Code, "N");
                //    else
                //        AgyTab_Grid.Rows.Add(false, Entity.Code_Desc, Img_Blank, Entity.Table_Code, " ");

                if (Entity.Active == "N")
                    AgyTab_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

            }
            if (AgyTab_Grid.Rows.Count > 0)
                AgyTab_Grid.Rows[0].Selected = true;
        }

        private void AgyTab_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (AgyTab_Grid.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = AgyTab_Grid.CurrentCell.ColumnIndex;
                int RowIdx = AgyTab_Grid.CurrentCell.RowIndex;

                switch (e.ColumnIndex)
                {
                    case 0:
                        if (AgyTab_Grid.CurrentRow.Cells["Sel_Agy_Selected"].Value.ToString() == true.ToString())
                        {
                            AgyTab_Grid.Rows[e.RowIndex].Cells["Sel_Agy_Equal"].ReadOnly = true;

                            AgyTab_Grid.CurrentRow.Cells["Sel_Agy_Equal"].Value = Img_Tick;
                            AgyTab_Grid.CurrentRow.Cells["Sel_Equal_Code"].Value = "E";
                        }
                        else
                        {
                            AgyTab_Grid.Rows[e.RowIndex].Cells["Sel_Agy_Equal"].ReadOnly = false;

                            AgyTab_Grid.CurrentRow.Cells["Sel_Agy_Equal"].Value = Img_Blank;
                            AgyTab_Grid.CurrentRow.Cells["Sel_Equal_Code"].Value = " ";
                        }
                        break;

                    case 3:
                        if (AgyTab_Grid.CurrentRow.Cells["Sel_Agy_Selected"].Value.ToString() == true.ToString())
                        {
                            if (AgyTab_Grid.CurrentRow.Cells["Sel_Equal_Code"].Value.ToString() == "E")
                            {
                                AgyTab_Grid.CurrentRow.Cells["Sel_Agy_Equal"].Value = Img_Cross;
                                AgyTab_Grid.CurrentRow.Cells["Sel_Equal_Code"].Value = "N";
                            }
                            else
                            {
                                AgyTab_Grid.Rows[e.RowIndex].Cells["Sel_Agy_Equal"].ReadOnly = false;

                                AgyTab_Grid.CurrentRow.Cells["Sel_Agy_Equal"].Value = Img_Tick;
                                AgyTab_Grid.CurrentRow.Cells["Sel_Equal_Code"].Value = "E";
                            }
                        }
                        break;
                }
            }
        }


        public string[] Get_AgyTab_Cindition()
        {
            string[] AGYTAB_Conditions = new string[2];
            AGYTAB_Conditions[0] = AGYTAB_Conditions[1] = " ";

            string Tmp_Code = string.Empty;
            int Tmp_Code1 = 0;

            bool Meet_Equal_Case = false, Meet_NotEqual_Case = false;
            foreach (DataGridViewRow dr in AgyTab_Grid.Rows)
            {
                if (dr.Cells["Sel_Equal_Code"].Value.ToString() != " ")
                {
                    Tmp_Code = dr.Cells["Sel_Agy_Code"].Value.ToString();

                    if (AgyType == "ZIPLS")
                    {
                        Tmp_Code1 = int.Parse(Tmp_Code);
                        Tmp_Code = Tmp_Code1.ToString();
                    }

                    //if (!string.IsNullOrEmpty(AgyType))
                    //{
                    //    if (AgyType == "SITES")
                    //        Tmp_Code = Tmp_Code.Substring(2, Tmp_Code.Length - 2);
                    //}

                    if (Delimiter_Length == 0)
                    {
                        switch (dr.Cells["Sel_Equal_Code"].Value.ToString())
                        {
                            case "E": if (TableName == "AGCYPART" || TableName == "AGCYREP") AGYTAB_Conditions[0] += ( Tmp_Code + ","); 
                                      else AGYTAB_Conditions[0] += ("'" + Tmp_Code + "'" + ","); Meet_Equal_Case = true;
                                    break;
                            case "N": AGYTAB_Conditions[1] += ("'" + Tmp_Code + "'" + ","); Meet_NotEqual_Case = true; break;
                        }
                    }
                    else
                    {
                        Tmp_Code = Tmp_Code + "          ".Substring(0, Delimiter_Length - Tmp_Code.Length);

                        switch (dr.Cells["Sel_Equal_Code"].Value.ToString())
                        {
                            case "E": AGYTAB_Conditions[0] += Tmp_Code; Meet_Equal_Case = true; break;
                            case "N": AGYTAB_Conditions[1] += Tmp_Code; Meet_NotEqual_Case = true; break;
                        }

                    }
                }
            }


            if (Delimiter_Length == 0)
            {
                if (Meet_Equal_Case)
                    AGYTAB_Conditions[0] = AGYTAB_Conditions[0].Substring(0, AGYTAB_Conditions[0].Length - 1);

                if (Meet_NotEqual_Case)
                    AGYTAB_Conditions[1] = AGYTAB_Conditions[1].Substring(0, AGYTAB_Conditions[1].Length - 1);
            }
            else
            {
                //if (Meet_Equal_Case)
                //    AGYTAB_Conditions[0] = "'" + AGYTAB_Conditions[0] + "'";

                //if (Meet_NotEqual_Case)
                //    AGYTAB_Conditions[1] = "'" + AGYTAB_Conditions[1] + "'";

                if (Meet_Equal_Case)
                    AGYTAB_Conditions[0] = AGYTAB_Conditions[0].Substring(1, AGYTAB_Conditions[0].Length - 1);

                if (Meet_NotEqual_Case)
                    AGYTAB_Conditions[1] = AGYTAB_Conditions[1].Substring(1, AGYTAB_Conditions[1].Length - 1);
            }


            return AGYTAB_Conditions;
        }

        public string[] Get_CAMS_Condition()
        {
            string[] AGYTAB_Conditions = new string[2];
            AGYTAB_Conditions[0] = AGYTAB_Conditions[1] = " ";

            string Tmp_Code = string.Empty;
            int Tmp_Code1 = 0;

            bool Meet_Equal_Case = false, Meet_NotEqual_Case = false;
            if (!string.IsNullOrEmpty(EqualTO.Trim()))
            { AGYTAB_Conditions[0] = EqualTO; Meet_Equal_Case = true; }
            if (!string.IsNullOrEmpty(NotEqualTO.Trim()))
            { AGYTAB_Conditions[1] = NotEqualTO; Meet_NotEqual_Case = true; }
            //foreach (DataGridViewRow dr in gvCAMS.Rows)
            //{
            //    if (dr.Cells["Sel_CAMSEqual_Code"].Value.ToString() != " ")
            //    {
            //        Tmp_Code = dr.Cells["Sel_CAMS_Code"].Value.ToString();


            //        if (Delimiter_Length == 0)
            //        {
            //            switch (dr.Cells["Sel_CAMSEqual_Code"].Value.ToString())
            //            {
            //                case "E": AGYTAB_Conditions[0] += ("'" + Tmp_Code + "'" + ","); Meet_Equal_Case = true; break;
            //                case "N": AGYTAB_Conditions[1] += ("'" + Tmp_Code + "'" + ","); Meet_NotEqual_Case = true; break;
            //            }
            //        }

            //    }
            //}

            if (Delimiter_Length == 0)
            {
                if (Meet_Equal_Case)
                    AGYTAB_Conditions[0] = AGYTAB_Conditions[0].Substring(0, AGYTAB_Conditions[0].Length - 1);

                if (Meet_NotEqual_Case)
                    AGYTAB_Conditions[1] = AGYTAB_Conditions[1].Substring(0, AGYTAB_Conditions[1].Length - 1);
            }

            return AGYTAB_Conditions;
        }

        private void Btn_Multiple_Clear_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in AgyTab_Grid.Rows)
            {
                dr.Cells["Sel_Agy_Selected"].Value = false;
                dr.Cells["Sel_Agy_Equal"].ReadOnly = true;

                dr.Cells["Sel_Agy_Equal"].Value = Img_Blank;
                dr.Cells["Sel_Equal_Code"].Value = " ";
            }
        }

        private void Btn_Time_Clear_Click(object sender, EventArgs e)
        {
            Time_Equal_2.Text = "00:00:00";
            Time_Equal_2.Checked = false;
        }


        private bool validate_CAMS_Dates()
        {
            bool can_Save = true;

            if (!CAMS_From_Date.Checked)
            {
                _errorProvider.SetError(CAMS_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                can_Save = false;
            }
            else
            {
                if(CAMS_From_Date.Value.ToString().Contains("001"))
                {
                    _errorProvider.SetError(CAMS_From_Date, string.Format("Please enter (MM/DD/YY) or (MM/DD/YYYY) date format", "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    can_Save = false;
                }
                else
                    _errorProvider.SetError(CAMS_From_Date, null);
            }

            if (!CAMS_To_date.Checked)
            {
                _errorProvider.SetError(CAMS_To_date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                can_Save = false;
            }
            else
            {
                if (CAMS_To_date.Value.ToString().Contains("001"))
                {
                    _errorProvider.SetError(CAMS_To_date, string.Format("Please enter (MM/DD/YY) or (MM/DD/YYYY) date format", "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    can_Save = false;
                }
                else
                    _errorProvider.SetError(CAMS_To_date, null);
            }


            if (CAMS_From_Date.Checked && CAMS_To_date.Checked)
            {
                if (CAMS_From_Date.Value > CAMS_To_date.Value)
                {
                    _errorProvider.SetError(CAMS_To_date, string.Format("'From Date' sholud be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    can_Save = false;
                }
                else
                    _errorProvider.SetError(CAMS_To_date, null);
            }

            return can_Save;
        }


        private void Btn_CAMS_Generate_Work_File_Click(object sender, EventArgs e)
        {

            if (validate_CAMS_Dates())
            {
                CaptainModel _model = new CaptainModel();

                Application.Session["usersessionid"] = CommonFunctions.GetAlphanumericCode(); //Application.Session.SessionID;
                string ip = Application.ServerVariables["HTTP_X_FORWARDED_FOR"]; // System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = Application.ServerVariables["REMOTE_ADDR"]; //System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }


                DataSet ds = _model.AdhocData.Generate_CAMS_Work_File(Curr_Hie.Substring(0, 2), Curr_Hie.Substring(2, 2), Curr_Hie.Substring(4, 2), Curr_Hie_Year, Rb_Add_Date.Checked ? "A" : "P", CAMS_From_Date.Value.ToShortDateString(), CAMS_To_date.Value.ToShortDateString(), User_Name, Application.Session["usersessionid"].ToString(), ip);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (int.Parse(ds.Tables[0].Rows[0]["Added_Recs"].ToString()) > 0)
                            AlertBox.Show("ACTMS Table Regenerated for Selected Hierarchy");
                        else
                            AlertBox.Show("No records to Regenerated the ACTMS Table  for Selected Hierarchy", MessageBoxIcon.Warning);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
                //MessageBox.Show("Are You Sure You Want to Generate WorkFile?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, Delete_Authrization, true);
            }

        }

        private void gvCAMS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvCAMS.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = gvCAMS.CurrentCell.ColumnIndex;
                int RowIdx = gvCAMS.CurrentCell.RowIndex;

                switch (e.ColumnIndex)
                {
                    case 0:
                        if (gvCAMS.CurrentRow.Cells["Sel_CAMS_Selected"].Value.ToString() == true.ToString())
                        {
                            gvCAMS.Rows[e.RowIndex].Cells["Sel_CAMS_Equal"].ReadOnly = true;

                            gvCAMS.CurrentRow.Cells["Sel_CAMS_Equal"].Value = Img_Tick;
                            gvCAMS.CurrentRow.Cells["Sel_CAMSEqual_Code"].Value = "E";

                            string Tmp_Code = ("'" + gvCAMS.CurrentRow.Cells["Sel_CAMS_Code"].Value.ToString() + "'" + ",");

                            if (!string.IsNullOrEmpty(EqualTO.Trim()))
                            {
                                char mystr = EqualTO[EqualTO.Length - 1];
                                if (mystr != ',') Tmp_Code = "," + Tmp_Code;
                            }

                            EqualTO = EqualTO + Tmp_Code;

                        }
                        else
                        {
                            gvCAMS.Rows[e.RowIndex].Cells["Sel_CAMS_Equal"].ReadOnly = false;

                            gvCAMS.CurrentRow.Cells["Sel_CAMS_Equal"].Value = Img_Blank;
                            gvCAMS.CurrentRow.Cells["Sel_CAMSEqual_Code"].Value = " ";

                            string Tmp_Code = ("'" + gvCAMS.CurrentRow.Cells["Sel_CAMS_Code"].Value.ToString() + "'" + ",");
                            EqualTO = EqualTO.Replace(Tmp_Code, "");

                        }
                        break;

                    case 3:
                        if (gvCAMS.CurrentRow.Cells["Sel_CAMS_Selected"].Value.ToString() == true.ToString())
                        {
                            if (gvCAMS.CurrentRow.Cells["Sel_CAMSEqual_Code"].Value.ToString() == "E")
                            {
                                gvCAMS.CurrentRow.Cells["Sel_CAMS_Equal"].Value = Img_Cross;
                                gvCAMS.CurrentRow.Cells["Sel_CAMSEqual_Code"].Value = "N";

                                string Tmp_Code = ("'" + gvCAMS.CurrentRow.Cells["Sel_CAMS_Code"].Value.ToString() + "'" + ",");


                                EqualTO = EqualTO.Replace(Tmp_Code, "");
                                NotEqualTO = NotEqualTO + Tmp_Code;
                            }
                            else
                            {
                                gvCAMS.Rows[e.RowIndex].Cells["Sel_CAMS_Equal"].ReadOnly = false;

                                gvCAMS.CurrentRow.Cells["Sel_CAMS_Equal"].Value = Img_Tick;
                                gvCAMS.CurrentRow.Cells["Sel_CAMSEqual_Code"].Value = "E";

                                string Tmp_Code = ("'" + gvCAMS.CurrentRow.Cells["Sel_CAMS_Code"].Value.ToString() + "'" + ",");

                                NotEqualTO = NotEqualTO.Replace(Tmp_Code, "");
                                EqualTO = EqualTO + Tmp_Code;
                            }
                        }
                        break;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (AgyType == "CACTI")
                Fill_Activity_Grid1();
            else if (AgyType == "MSTON")
                Fill_Milestone_Grid1();
        }

        private void btn_CAMS_Clear_Click(object sender, EventArgs e)
        {
            if (gvCAMS.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvCAMS.Rows)
                {
                    dr.Cells["Sel_CAMS_Selected"].Value = false;
                    dr.Cells["Sel_CAMS_Equal"].ReadOnly = true;

                    dr.Cells["Sel_CAMS_Equal"].Value = Img_Blank;
                    dr.Cells["Sel_CAMSEqual_Code"].Value = " ";
                }

                EqualTO = string.Empty;
                NotEqualTO = string.Empty;
            }
        }

        private void AgyTab_Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void gvCAMS_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void Delete_Authrization(object sender, EventArgs e)
        {
            // Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;
            Form senderForm = (Form)sender;

            if (senderForm != null)
            {
                if (senderForm.DialogResult.ToString() == "Yes")
                {
                    CaptainModel _model = new CaptainModel();

                    Application.Session["usersessionid"] = CommonFunctions.GetAlphanumericCode();//Application.Session.SessionID;
                    string ip = Application.ServerVariables["HTTP_X_FORWARDED_FOR"];  //System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = Application.ServerVariables["REMOTE_ADDR"]; //System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }


                    DataSet ds = _model.AdhocData.Generate_CAMS_Work_File(Curr_Hie.Substring(0, 2), Curr_Hie.Substring(2, 2), Curr_Hie.Substring(4, 2), Curr_Hie_Year, Rb_Add_Date.Checked ? "A" : "P", CAMS_From_Date.Value.ToShortDateString(), CAMS_To_date.Value.ToShortDateString(), User_Name, Application.Session["usersessionid"].ToString(), ip);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (int.Parse(ds.Tables[0].Rows[0]["Added_Recs"].ToString()) > 0)
                                AlertBox.Show("ACTMS Table Regenerated for Selected Hierarchy");
                            else
                                AlertBox.Show("No records to Regenerated the ACTMS Table  for Selected Hierarchy", MessageBoxIcon.Warning);
                        }
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void Cb_Nulls_Click(object sender, EventArgs e)
        {
            if (Cb_Nulls.Checked)
            {
                Dt_Equal.Checked = Dt_Greater.Checked = Dt_Lesser.Checked = false;
            }
        }







    }
}
