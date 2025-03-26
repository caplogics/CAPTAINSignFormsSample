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
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00138_BM_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        public HSS00138_BM_Form(BaseForm baseform, string mode, string Mode_Type, string ChldBM_Number,string Route,string Route_Year, string hier_Desc, string hierar, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            strHiearchy = hierar; Number = ChldBM_Number; 
            strHierarcy_Desc = hier_Desc;
            Type_Mode = Mode_Type; Route_Id = Route; Year = Route_Year;
            Privileges = privileges;
            Form_Load();
            //hierarchyEntity = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
            txtBus_year.Validator = TextBoxValidation.IntegerValidator;
            txtOil.Validator = TextBoxValidation.IntegerValidator;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Number { get; set; }

        public string Year { get; set; }

        public string strHierarcy_Desc { get; set; }

        public string HiearchyCode { get; set; }

        public string Type_Mode { get; set; }

        public string Route_Id { get; set; }

        public string strHiearchy { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion


        private void Form_Load()
        {
            this.Text = Privileges.Program + " - " + Mode;
            if (Mode == "Add")
            {
                if (Type_Mode == "Bus")
                {
                    this.Text = "Bus Master Maintenance";
                    this.Size = new Size(771, 350);
                    pnlDrRoData.Visible = panel8.Visible = false;
                    pnlBusDesc.Visible = pnlBusRegis.Visible = pnlBusSave.Visible = true;
                }
                else
                {
                    this.Text = "Bus Route Maintenance";
                    this.Size = new Size(707, 345);
                    pnlDrRoData.Visible = panel8.Visible = true;
                    //pnlBusDesc.Visible = pnlBusRegis.Visible = pnlBusSave.Visible = false;
                    pnlBus.Visible = false;
                }
            }
            else if (Mode == "Edit")
            {
                if (Type_Mode == "Bus")
                {
                    txtBus_Id.Enabled = false;
                    this.Text = "Bus Master Maintenance";
                    this.Size = new Size(771, 350);
                    pnlDrRoData.Visible = panel8.Visible = false;
                    pnlBusDesc.Visible = pnlBusRegis.Visible = pnlBusSave.Visible = true;
                    fillBusControls();
                }
                else 
                {
                    txtRoute.Enabled = false;
                    this.Text = "Bus Route Maintenance";
                    this.Size = new Size(707, 345);
                    pnlDrRoData.Visible = panel8.Visible = true;
                    //pnlBusDesc.Visible = pnlBusRegis.Visible = pnlBusSave.Visible = false;
                    pnlBus.Visible = false;
                    FillBusRoute_Controls();
                }
            }
            else
            {
                if (Type_Mode == "Bus")
                {
                    pnlBusDesc.Enabled = pnlBusRegis.Enabled = pnlBusSave.Enabled = false;
                    this.Text = "Bus Master Maintenance";
                    this.Size = new Size(771, 350);
                    pnlDrRoData.Visible = panel8.Visible = false;
                    pnlBusDesc.Visible = pnlBusRegis.Visible = pnlBusSave.Visible = true;
                    fillBusControls();
                }
                else
                {
                    pnlDrRoData.Enabled = panel8.Enabled = false;
                    this.Text = "Bus Route Maintenance";
                    this.Size = new Size(707, 345);
                    pnlDrRoData.Visible = panel8.Visible = true;
                    //pnlBusDesc.Visible = pnlBusRegis.Visible = pnlBusSave.Visible = false;
                    pnlBus.Visible = false;
                    FillBusRoute_Controls();
                }
            }
        }
        string BM_Count = "0";
        private void fillBusControls()
        {
            List<ChldBMEntity> Buslist;
            ChldBMEntity SearchEntity = new ChldBMEntity(true);
            SearchEntity.ChldBMAgency = BaseForm.BaseAgency.Trim(); SearchEntity.chldBMDept = BaseForm.BaseDept.Trim();
            SearchEntity.ChldBMProg = BaseForm.BaseProg.Trim(); SearchEntity.ChldBMNumber = Number;
            Buslist = _model.SPAdminData.Browse_ChldBM(SearchEntity, "Browse");

            if (Buslist.Count > 0)
            {
                ChldBMEntity Entity = Buslist[0];
                txtBus_Id.Text = Entity.ChldBMNumber;
                txtType.Text = Entity.ChldBM_Type;
                txtBus_year.Text = Entity.ChldBMYear;
                txtMake.Text = Entity.Make;
                txtDesc.Text = Entity.Desc;
                txtLoc1.Text = Entity.Location1;
                txtLoc2.Text = Entity.Location2;
                if (Entity.OL.Trim() == "O")
                {
                    rbOwned.Checked = true;

                }
                else
                    rbLeased.Checked = true;
                txtLeaseID.Text = Buslist[0].OL_ID;
                if (Entity.OL.Trim() == "L")
                {
                    if (!string.IsNullOrEmpty(Entity.OL_Date.Trim()))
                    {
                        dtpLeasedate.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.OL_Date.Trim()));
                        dtpLeasedate.Checked = true;
                    }
                }
                txtRegNo.Text = Entity.Registration;
                if (!string.IsNullOrEmpty(Entity.Registration_Date.Trim()))
                {
                    dtpRegDate.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.Registration_Date.Trim()));
                    dtpRegDate.Checked = true;
                }
                maskPhone.Text = Entity.TelPhone.Trim();
                txtOil.Text = Entity.Last_Oil_Mile;
                if (!string.IsNullOrEmpty(Entity.Last_Oil_Date.Trim()))
                {
                    dtpChangeDate.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.Last_Oil_Date.Trim()));
                    dtpChangeDate.Checked = true;
                }
                BM_Count = Entity.ChldBM_Count.Trim();
            }
        }

        private void FillBusRoute_Controls()
        {
            List<BusRTEntity> Route_list;
            BusRTEntity SearchEntity = new BusRTEntity(true);
            SearchEntity.BUSRT_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity.BUSRT_DEPT = BaseForm.BaseDept.Trim();
            SearchEntity.BUSRT_PROGRAM = BaseForm.BaseProg.Trim(); SearchEntity.BUSRT_NUMBER = Number.Trim();
            SearchEntity.BUSRT_ROUTE = Route_Id.Trim(); if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim())) Baseyear = "    ";
            else Baseyear = BaseForm.BaseYear.Trim();
            SearchEntity.BUSRT_YEAR = Baseyear;
            Route_list = _model.SPAdminData.Browse_ChldBUSR(SearchEntity, "Browse");

            if (Route_list.Count > 0)
            {
                BusRTEntity Entity = Route_list[0];
                txtRoute.Text = Entity.BUSRT_ROUTE.Trim();
                if (!string.IsNullOrEmpty(Entity.BUSRT_PICKUP_STARTS.Trim()))
                {
                    dtpPickUp.Text = Entity.BUSRT_PICKUP_STARTS.ToString();
                    dtpPickUp.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSRT_ARRIVE_SCHOOL.Trim()))
                {
                    dtpArrive.Text = Entity.BUSRT_ARRIVE_SCHOOL.ToString();
                    dtpArrive.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSRT_LEAVES_SCHOOL.Trim()))
                {
                    dtpLeave.Text = Entity.BUSRT_LEAVES_SCHOOL.ToString();
                    dtpLeave.Checked = true;
                }
                txtArea.Text = Entity.BUSRT_AREA_SERVED.Trim();

                txtName.Text = Entity.BUSRT_DRIVER_NAME.Trim();
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_DOB.Trim()))
                {
                    dtpDob.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.BUSRT_DRIVER_DOB.Trim()));
                    dtpDob.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_CLD_DATE.Trim()))
                {
                    dtpCDL.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.BUSRT_DRIVER_LIC_CLD_DATE.Trim()));
                    dtpCDL.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_7D_DATE.Trim()))
                {
                    dtp7D.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.BUSRT_DRIVER_LIC_7D_DATE.Trim()));
                    dtp7D.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_DATE.Trim()))
                {
                    dtpLICExp.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.BUSRT_DRIVER_LIC_DATE.Trim()));
                    dtpLICExp.Checked = true;
                }
                if (!string.IsNullOrEmpty(Entity.BUSRT_DRIVER_LIC_DPU_DATE.Trim()))
                {
                    dtpDPU.Value = Convert.ToDateTime(LookupDataAccess.Getdate(Entity.BUSRT_DRIVER_LIC_DPU_DATE.Trim()));
                    dtpDPU.Checked = true;
                }
                txtLicNo.Text = Entity.BUSRT_DRIVER_LIC.Trim();
                mskTele_Phn.Text = Entity.BUSRT_DRIVER_TEL.Trim();
            }
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS00138");
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaptainModel Model = new CaptainModel();
                    string SqlMsg = string.Empty;
                    string msg = string.Empty;

                    ChldBMEntity Entity = new ChldBMEntity();
                    if (Mode == "Edit")
                    {
                        Entity.row_Type = "U";
                        Entity.ChldBM_Count = BM_Count.Trim();
                    }
                    else
                    {
                        Entity.row_Type = "I";
                        Entity.ChldBM_Count = "0";
                    }
                    Entity.ChldBMAgency = BaseForm.BaseAgency.Trim();
                    Entity.chldBMDept = BaseForm.BaseDept.Trim();
                    Entity.ChldBMProg = BaseForm.BaseProg.Trim();
                    Entity.ChldBMNumber = txtBus_Id.Text.Trim();
                    Entity.ChldBM_Type = txtType.Text.Trim();
                    Entity.ChldBMYear = txtBus_year.Text.Trim();
                    Entity.Make = txtMake.Text.Trim();
                    Entity.Desc = txtDesc.Text.Trim();
                    Entity.Location1 = txtLoc1.Text.Trim();
                    Entity.Location2 = txtLoc2.Text.Trim();
                    if (rbOwned.Checked.Equals(true))
                        Entity.OL = "O";
                    else
                        Entity.OL = "L";
                    Entity.OL_ID = txtLeaseID.Text.Trim();
                    if (dtpLeasedate.Checked.Equals(true))
                        Entity.OL_Date = dtpLeasedate.Text.Trim();
                    Entity.Registration = txtRegNo.Text.Trim();
                    if(dtpRegDate.Checked.Equals(true))
                        Entity.Registration_Date = dtpRegDate.Text.Trim();
                    Entity.TelPhone = maskPhone.Text.Trim();
                    Entity.Last_Oil_Mile = txtOil.Text.Trim();
                    if (dtpChangeDate.Checked.Equals(true))
                        Entity.Last_Oil_Date = dtpChangeDate.Text.Trim();
                    
                    Entity.AddOperator = BaseForm.UserID;
                    Entity.LstcOperator = BaseForm.UserID;

                    _model.SPAdminData.UpdateChldBM(Entity, "Update", out msg, out SqlMsg);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (Type_Mode == "Bus")
            {
                if (string.IsNullOrEmpty(txtBus_Id.Text.Trim()))
                {
                    _errorProvider.SetError(txtBus_Id, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBus_Id.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (isCodeExists(txtBus_Id.Text))
                    {
                        _errorProvider.SetError(txtBus_Id, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblBus_Id.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtBus_Id, null);
                    }
                }

                if (string.IsNullOrEmpty(txtBus_year.Text.Trim()))
                {
                    _errorProvider.SetError(txtBus_year, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBus_Year.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtBus_year.Text.Trim()))
                    {
                        if (1900 > int.Parse(txtBus_year.Text.Trim()) || int.Parse(txtBus_year.Text.Trim()) > DateTime.Now.AddYears(2).Year)
                        {
                            _errorProvider.SetError(txtBus_year, "Year range should be in between 1900 - " + DateTime.Now.AddYears(2).Year.ToString().Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                    }
                    else
                        _errorProvider.SetError(txtBus_year, null);
                }

                if (string.IsNullOrEmpty(txtMake.Text.Trim()))
                {
                    _errorProvider.SetError(txtMake, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMake.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtMake, null);

                if (string.IsNullOrEmpty(txtType.Text.Trim()))
                {
                    _errorProvider.SetError(txtType, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblType.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtType, null);

                if (string.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    _errorProvider.SetError(txtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtDesc, null);

                if (string.IsNullOrEmpty(txtLoc1.Text.Trim()))
                {
                    _errorProvider.SetError(txtLoc1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLoc1.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtLoc1, null);

                if (rbLeased.Checked.Equals(true))
                {
                    if (string.IsNullOrEmpty(txtLeaseID.Text.Trim()))
                    {
                        _errorProvider.SetError(txtLeaseID, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLeaseID.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtLeaseID, null);
                    if (dtpLeasedate.Checked.Equals(false))
                    {
                        _errorProvider.SetError(dtpLeasedate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLeaseExpDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(dtpLeasedate, null);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtRoute.Text.Trim()))
                {
                    _errorProvider.SetError(txtRoute, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRoute.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (isCodeExists(txtRoute.Text))
                    {
                        _errorProvider.SetError(txtRoute, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblRoute.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtRoute, null);
                    }
                }

                if(!string.IsNullOrEmpty(txtLicNo.Text.Trim()))
                {
                    if(dtpLICExp.Checked.Equals(false))
                    {
                        _errorProvider.SetError(dtpLICExp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLicExp.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(dtpLICExp, null);
                }
            }
            IsSaveValid = isValid;
            return (isValid);
        }

        private bool isCodeExists(string Code)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                if (Type_Mode == "Bus")
                {
                    List<ChldBMEntity> ChldBMList;
                    ChldBMEntity Search_entity = new ChldBMEntity(true);
                    Search_entity.ChldBMAgency = BaseForm.BaseAgency.Trim(); Search_entity.chldBMDept = BaseForm.BaseDept.Trim();
                    Search_entity.ChldBMProg = BaseForm.BaseProg.Trim(); Search_entity.ChldBMNumber = Code.Trim();
                    ChldBMList = _model.SPAdminData.Browse_ChldBM(Search_entity, "Browse");
                    foreach (ChldBMEntity Entity in ChldBMList)
                    {
                        if (Entity.ChldBMNumber.Trim() == Code.Trim())
                        {
                            isExists = true;
                        }
                    }
                    
                }
                else
                {
                    List<BusRTEntity> RouteList;
                    BusRTEntity Search_entity = new BusRTEntity(true);
                    Search_entity.BUSRT_AGENCY = BaseForm.BaseAgency.Trim(); Search_entity.BUSRT_DEPT = BaseForm.BaseDept.Trim();
                    Search_entity.BUSRT_PROGRAM = BaseForm.BaseProg.Trim(); Search_entity.BUSRT_NUMBER = Number.Trim();
                    if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                        Baseyear = "    ";
                    else Baseyear = BaseForm.BaseYear.Trim();
                    Search_entity.BUSRT_YEAR = Baseyear; //Search_entity.BUSRT_ROUTE = Route_Id.Trim();
                    RouteList = _model.SPAdminData.Browse_ChldBUSR(Search_entity, "Browse");
                    foreach (BusRTEntity Entity in RouteList)
                    {
                        if (Entity.BUSRT_ROUTE.Trim() == Code.Trim())
                        {
                            isExists = true;
                        }
                    }
                }
            }

            return isExists;
        }



        public string[] GetSelected_Number_Code()
        {
            string[] Added_Edited_Number = new string[3];

            Added_Edited_Number[0] = txtBus_Id.Text;
            Added_Edited_Number[1] = Mode;
            //Added_Edited_SiteCode[2] = txtHierarchy.Text;
            //Added_Edited_SiteCode[2] = ;

            return Added_Edited_Number;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbLeased_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLeased.Checked.Equals(true))
            {
                txtLeaseID.Visible = true; lblLeaseID.Visible = true;
                txtLeaseID.Enabled = true; dtpLeasedate.Enabled = true;
                dtpLeasedate.Visible = true; lblLeaseExpDate.Visible = true;
                label8.Visible = true; label9.Visible = true; dtpLeasedate.Checked = true;
                txtLeaseID.Focus();

            }
            else
            {
                txtLeaseID.Visible = false; lblLeaseID.Visible = false;
                dtpLeasedate.Visible = false; lblLeaseExpDate.Visible = false;
                label8.Visible = false; label9.Visible = false; dtpLeasedate.Checked = false;
                dtpLeasedate.Checked.Equals(false);
                rbLeased.Focus();
            }
        }

        private void rbOwned_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOwned.Checked.Equals(true))
            {
                txtLeaseID.Clear(); dtpLeasedate.Value = DateTime.Now.Date; dtpLeasedate.Checked = false;
                txtLeaseID.Visible = false; lblLeaseID.Visible = false;
                dtpLeasedate.Visible = false; lblLeaseExpDate.Visible = false;
                label8.Visible = false; label9.Visible = false;
                txtLeaseID.Enabled = false; dtpLeasedate.Enabled = false;
                rbLeased.Focus();
            }
        }
        string Baseyear = "    ";
        private void btnR_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaptainModel Model = new CaptainModel();
                    string SqlMsg = string.Empty;
                    string msg = string.Empty;

                    BusRTEntity Entity = new BusRTEntity();
                    if (Mode == "Edit")
                    {
                        Entity.row_Type = "U";
                        //Entity.ChldBM_Count = BM_Count.Trim();
                    }
                    else
                    {
                        Entity.row_Type = "I";
                        //Entity.ChldBM_Count = "0";
                    }
                    Entity.BUSRT_AGENCY = BaseForm.BaseAgency.Trim();
                    Entity.BUSRT_DEPT = BaseForm.BaseDept.Trim();
                    Entity.BUSRT_PROGRAM = BaseForm.BaseProg.Trim();
                    Entity.BUSRT_NUMBER = Number.Trim();
                    if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                        Baseyear = "    ";
                    else Baseyear = BaseForm.BaseYear.Trim();
                    Entity.BUSRT_YEAR = Baseyear;
                    Entity.BUSRT_ROUTE = txtRoute.Text.Trim();
                    if (dtpPickUp.Checked.Equals(true))
                        Entity.BUSRT_PICKUP_STARTS = dtpPickUp.Value.ToString("HH:mm:ss");
                    if (dtpArrive.Checked.Equals(true))
                        Entity.BUSRT_ARRIVE_SCHOOL = dtpArrive.Value.ToString("HH:mm:ss");
                    if (dtpLeave.Checked.Equals(true))
                        Entity.BUSRT_LEAVES_SCHOOL = dtpLeave.Value.ToString("HH:mm:ss");
                    Entity.BUSRT_AREA_SERVED = txtArea.Text.Trim();
                    Entity.BUSRT_DRIVER_NAME = txtName.Text.Trim();
                    if (dtpDob.Checked.Equals(true))
                        Entity.BUSRT_DRIVER_DOB = dtpDob.Text.Trim();
                    Entity.BUSRT_DRIVER_TEL = mskTele_Phn.Text.Trim();
                    Entity.BUSRT_DRIVER_LIC = txtLicNo.Text.Trim();
                    if (dtpCDL.Checked.Equals(true))
                        Entity.BUSRT_DRIVER_LIC_CLD_DATE = dtpCDL.Text.Trim();
                    if (dtpDPU.Checked.Equals(true))
                        Entity.BUSRT_DRIVER_LIC_DPU_DATE = dtpDPU.Text.Trim();
                    if (dtpLICExp.Checked.Equals(true))
                        Entity.BUSRT_DRIVER_LIC_DATE = dtpLICExp.Text.Trim();
                    if (dtp7D.Checked.Equals(true))
                        Entity.BUSRT_DRIVER_LIC_7D_DATE = dtp7D.Text.Trim();
                    Entity.AddOperator = BaseForm.UserID;
                    Entity.LstcOperator = BaseForm.UserID;

                    _model.SPAdminData.UpdateChldBusR(Entity, "Update", out msg, out SqlMsg);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            { }
        }


        public string[] GetSelected_Route_Code()
        {
            string[] Added_Edited_Route = new string[3];

            //**Added_Edited_Route[0] = Number;
            Added_Edited_Route[0] = txtRoute.Text;
            Added_Edited_Route[1] = Mode;
            //Added_Edited_SiteCode[2] = ;

            return Added_Edited_Route;
        }

        private void btnR_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtLicNo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLicNo.Text.Trim()))
            {
                dtpLICExp.Enabled = true;
                label12.Visible = true; dtpLICExp.Checked = true;
                dtpLICExp.Focus();
            }
            else
            {
                dtpDPU.Focus();
                dtpLICExp.Value = DateTime.Now.Date;
                dtpLICExp.Checked = false;
                dtpLICExp.Enabled = false;
                label12.Visible = false; dtpLICExp.Checked = false;

            }
        }

        private void HSS00138_BM_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        //private void txtLicNo_Leave(object sender, EventArgs e)
        //{
        //    //if (!string.IsNullOrEmpty(txtLicNo.Text.Trim()))
        //    //    dtpLICExp.Enabled = true;
        //    //else
        //    //    dtpLICExp.Enabled = false;
        //}

        //private void txtLicNo_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(txtLicNo.Text.Trim()))
        //    {
        //        dtpLICExp.Enabled = true;
        //    }
        //    else
        //        dtpLICExp.Enabled = false;
        //}

    }
}