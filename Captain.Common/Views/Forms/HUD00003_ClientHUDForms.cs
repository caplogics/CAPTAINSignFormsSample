using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using DevExpress.Pdf.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD00003_ClientHUDForms : Form
    {
        #region Private Variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        int currentRowIndex = 0;

        #endregion

        public HUD00003_ClientHUDForms(BaseForm baseform, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;
            _model = new CaptainModel();

           

            FillImpacts();

            FillSessionStatus();

            FillSessionType();

            FillSessPurVisit();

            Fill_Individual_Grid();
        }

        #region Properties

        public BaseForm _baseForm
        {
            get; set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get; set;
        }

        public List<HUDFORMENTITY> hudformEntity
        {
            get; set;
        }

        public List<HUDIMPACTENTITY> hudimpactEntity
        {
            get; set;
        }

        public DataTable dtCounselor
        {
            get; set;
        }

        #endregion

        private void btnIndvSessDet_Click(object sender, EventArgs e)
        {
            if (dgvIndvSess.Rows.Count > 0)
                currentRowIndex = dgvIndvSess.CurrentRow.Index;

            string strindvSeq = string.Empty;

            if (dgvIndvSess.Rows.Count > 0)
            {
                strindvSeq = dgvIndvSess.CurrentRow.Cells["gvIndv_Seq"].Value.ToString();
            }

            HUD00003_IndvSessDetails indvForm = new HUD00003_IndvSessDetails(_baseForm, _privilegeEntity, strindvSeq);
            indvForm.FormClosed += new FormClosedEventHandler(On_Form_Closed);
            indvForm.StartPosition = FormStartPosition.CenterScreen;
            indvForm.ShowDialog();
        }

        private void On_Form_Closed(object sender, FormClosedEventArgs e)
        {
            HUD00003_IndvSessDetails form = sender as HUD00003_IndvSessDetails;

            if (form.DialogResult == DialogResult.OK)
            {
                Fill_Individual_Grid();
            }
        }

        public void Fill_Individual_Grid()
        {
            dgvIndvSess.Rows.Clear();
            this.dgvIndvSess.SelectionChanged -= new System.EventHandler(this.dgvIndvSess_SelectionChanged);
            hudformEntity = _model.HUDCNTLData.GetHUDINDIV(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, "");

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseForm.BaseAdminAgency;
            List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

            if (hudformEntity.Count > 0)
            {
                foreach (HUDFORMENTITY indvFormEntity in hudformEntity)
                {
                    string StatusDesc = string.Empty;
                    if (StatusList.Count > 0)
                    {
                        CommonEntity CEntity = StatusList.Find(u => u.Code == indvFormEntity.Sess_Status && u.Code != "0");
                        if (CEntity != null)
                            StatusDesc = CEntity.Desc.Trim();
                    }

                    string CounsDesc = string.Empty;
                    if (STAFFMST_List.Count > 0)
                    {
                        CounsDesc = STAFFMST_List.Find(x => x.Staff_Code == indvFormEntity.Sess_Counselor).First_Name;
                        CounsDesc += " " + STAFFMST_List.Find(x => x.Staff_Code == indvFormEntity.Sess_Counselor).Last_Name;
                    }

                    string sess_Type_Desc = string.Empty;
                    if (SessTypeList.Count > 0)
                    {
                        CommonEntity TypeEntity = SessTypeList.Find(u => u.Code == indvFormEntity.Sess_Type && u.Code != "0");

                        if(TypeEntity != null)
                            sess_Type_Desc = TypeEntity.Desc.Trim();
                    }

                    string sess_PurVisit_Desc = string.Empty;
                    if (SessPurVisitList.Count > 0)
                    {
                        CommonEntity PurVisitEntity = SessPurVisitList.Find(u => u.Code == indvFormEntity.Sess_Pur_Visit && u.Code != "00");

                        if (PurVisitEntity != null)
                            sess_PurVisit_Desc = PurVisitEntity.Desc.Trim();
                    }

                    dgvIndvSess.Rows.Add(StatusDesc, indvFormEntity.Sess_Date == "" ? "" : Convert.ToDateTime(indvFormEntity.Sess_Date).ToString("MM/dd/yyyy"), CounsDesc, indvFormEntity.Sess_Start_Time, indvFormEntity.Sess_End_Time, sess_Type_Desc, sess_PurVisit_Desc, indvFormEntity.Seq);
                }
            }

            if (dgvIndvSess.Rows.Count > 0)
            {
                dgvIndvSess.Rows[currentRowIndex].Selected = true;
                dgvIndvSess.CurrentCell = dgvIndvSess.Rows[currentRowIndex].Cells[1];

                dgvIndvSess_SelectionChanged(this, new EventArgs());
            }
            this.dgvIndvSess.SelectionChanged += new System.EventHandler(this.dgvIndvSess_SelectionChanged);
        }

        private void dgvIndvSess_SelectionChanged(object sender, EventArgs e)
        {
            hudimpactEntity = _model.HUDCNTLData.GetHUDIMPACT(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear, _baseForm.BaseApplicationNo, "", "");

            dgvImpacts.Rows.Clear();
            if (hudimpactEntity.Count > 0)
            {
                foreach (HUDIMPACTENTITY impactEntity in hudimpactEntity)
                {
                    if (dgvIndvSess.CurrentRow.Cells["gvIndv_Seq"].Value.ToString() == impactEntity.Indv_Seq) 
                    {
                        string ImpactName = string.Empty;
                        if (ImpactsList.Count > 0)
                        {
                            CommonEntity CEntity=ImpactsList.Find(u=>u.Code==impactEntity.Impacts);
                            if (CEntity != null)
                                ImpactName = CEntity.Desc.Trim();
                        }

                        dgvImpacts.Rows.Add(impactEntity.Impact_Date == "" ? "" : Convert.ToDateTime(impactEntity.Impact_Date).ToString("MM/dd/yyyy"), ImpactName, impactEntity.Seq, impactEntity.Indv_Seq);
                    }
                }
            }

            if (dgvImpacts.Rows.Count > 0)
            {
                dgvImpacts.Rows[0].Selected = true;
                dgvImpacts.CurrentCell = dgvImpacts.Rows[0].Cells[1];
            }
        }

        List<CommonEntity> ImpactsList = new List<CommonEntity>();
        public void FillImpacts()
        {

            ImpactsList.Add(new CommonEntity("1", "Households that received one-on-one counseling that also received education services."));
            ImpactsList.Add(new CommonEntity("2", "Households that received information fair housing, fair lending and/or accessibility rights."));
            ImpactsList.Add(new CommonEntity("3", "Households for whom counselor developed a budget customized to a client's current situation."));
            ImpactsList.Add(new CommonEntity("4", "Households that improved their financial capacity (e.g. increased discretionary income, decreased debt load, increased savings, increased credit score, etc.) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("5", "Households that gained access to resources to help improve their housing situation (e.g. down payment assistance, rental assistance, utility assistance, etc.) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("6", "Households that gained access to non-housing resources (e.g. social service programs, legal services, public benefits such as Social Security or Medicaid, etc.) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("7", "Homeless or potentially homeless households that obtained temporary or permanent housing after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("8", "Households that gained access to disaster recovery non-housing resources after receiving Housing Counseling Services (e.g. Red Cross/FEMA relief items, legal services, assistance"));
            ImpactsList.Add(new CommonEntity("9", "Households obtained disaster recovery housing resources after receiving Housing Counseling Services (e.g. temporary shelter, homeowner rehab, relocation, etc."));
            ImpactsList.Add(new CommonEntity("10", "Households for whom counselor developed or updated an emergency preparedness plan."));
            ImpactsList.Add(new CommonEntity("11", "Household that received rental counseling and avoided eviction after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("12", "Household that received rental counseling and improved living conditions after receiving Housing Counseling Services"));
            ImpactsList.Add(new CommonEntity("13", "Household that received pre-purchase/homebuying counseling and purchased housing after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("14", "Household that received reverse mortgage counseling and obtained a Home Enquiry Conversion Mortgage (HECM) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("15", "Household that received non-delinquency post-purchase counseling that were able to improve home conditions or home affordability after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("16", "Household that prevented or resolved a forward mortgage default after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("17", "Household that prevented or resolved a reverse mortgage default after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("18", "Household that received a forward mortgage modification and remained current in their modified mortgage after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("19", "Household that received a forward mortgage modification and improved their financial capacity after receiving Housing Counseling Services."));
        }

        List<CommonEntity> StatusList = new List<CommonEntity>();
        public void FillSessionStatus()
        {
            StatusList.Add(new CommonEntity("0", "Select One"));
            StatusList.Add(new CommonEntity("O", "Opened"));
            StatusList.Add(new CommonEntity("C", "Closed"));
        }
        List<CommonEntity> SessTypeList = new List<CommonEntity>();
        public void FillSessionType()
        {
            SessTypeList.Add(new CommonEntity("0", "Select One"));
            SessTypeList.Add(new CommonEntity("F", "Face to Face"));
            SessTypeList.Add(new CommonEntity("P", "Phone"));
            SessTypeList.Add(new CommonEntity("G", "Group"));
            SessTypeList.Add(new CommonEntity("I", "Internet"));
            SessTypeList.Add(new CommonEntity("O", "Other"));
            SessTypeList.Add(new CommonEntity("N", "N/A"));
        }

        List<CommonEntity> SessPurVisitList = new List<CommonEntity>();
        public void FillSessPurVisit()
        {
            SessPurVisitList.Add(new CommonEntity("00", "Select One"));
            SessPurVisitList.Add(new CommonEntity("HA", "Homeless Assistance"));
            SessPurVisitList.Add(new CommonEntity("RT", "Rental Topics"));
            SessPurVisitList.Add(new CommonEntity("PH", "Prepurchase/Homebuying"));
            SessPurVisitList.Add(new CommonEntity("NP", "Non-Delinquency Post-Purchase"));
            SessPurVisitList.Add(new CommonEntity("RM", "Reverse Mortgage"));
            SessPurVisitList.Add(new CommonEntity("RF", "Resolving or Preventing Forward Mortgage Delinquency or Default"));
            SessPurVisitList.Add(new CommonEntity("RR", "Resolving or Preventing Reverse Mortgage Delinquency or Default"));
            SessPurVisitList.Add(new CommonEntity("DP", "Disaster Preparedness Assistance"));
            SessPurVisitList.Add(new CommonEntity("DR", "Disaster Recovery Assistance"));
        }

    }
}
