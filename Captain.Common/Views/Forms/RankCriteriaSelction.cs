#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Wisej.Design;
//using Wisej.Web.Design;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using Captain.Common.Views.UserControls;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RankCriteriaSelction : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        //string Img_tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");
        //string Img_Blank = Consts.Icons.ico_Blank;

        string Img_Tick = "Resources/Images/tick.ico";
        string Img_Blank = "Resources/Icons/16X16/Blank.JPG";

        public RankCriteriaSelction(BaseForm baseform, string Hier, string RankCode, string ScreenCd, string Field_Name, string Field_Cd, string field_Type, string Agy_Code, string mode, string Resp_type, string File_Name, PrivilegeEntity Priviliges, RankDefinitions RdForm,string ScreenSw)
        {
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            InitializeComponent();
            Baseform = baseform;
            _model = new CaptainModel();
            
            priviliges = Priviliges;
            hierarichy = Hier;
            RnkCd = RankCode;
            ScrCd = ScreenCd;
            Screen_Sw = ScreenSw;
            FieldName = Field_Name;
            FieldCode = Field_Cd;
            FieldType = field_Type;
            AGYCODE = Agy_Code;
            RD_Form = RdForm;
            FileName = File_Name;
            Mode = mode;
            RespType = Resp_type;
            txtField.Text = FieldName.ToString();
            Resp_type_screen();
            this.Text = "Rank Criteria Selction";//priviliges.Program + " -Rank Criteria Selction";
            if (FileName.Trim() == "CASESNP")
            {
                cmbCntInd.Visible = true;
                lblCntInd.Visible = true;
                Size = new Size(687,571);
                if (RespType == "G" || RespType == "H")
                {
                    lblAgeCalInd.Visible = true;
                    cmbAgeCalInd.Visible = true;
                    FillAgeCalculationIndicator();
                    FillCountIndicator();
                }
                else
                {
                    lblAgeCalInd.Visible = false;
                    cmbAgeCalInd.Visible = false;
                    FillCountIndicator();
                }
                FillResponsGrid(RelationDesc, RelationCode);
            }
            else
            {
                cmbCntInd.Visible = false;
                lblCntInd.Visible = false;
                pnlSelectedField.Size = new Size(687,44);
                Size = new Size(687,539);
                gvResponse.Rows.Clear();
                if (RespType == "B" || RespType == "T" || RespType == "N" || RespType == "$" || RespType == "&" || RespType=="X")
                {
                    this.gvResponse.AllowUserToAddRows = true;
                    

                    //FillCountIndicator();
                }
                else
                    this.gvResponse.AllowUserToAddRows = false;
                FillResponsGrid(null, null);
            }

        }

        #region properties

        public BaseForm Baseform { get; set; }

        public PrivilegeEntity priviliges { get; set; }

        public string hierarichy { get; set; }

        public string RnkCd { get; set; }

        public string ScrCd { get; set; }

        public string Screen_Sw { get; set; }

        public string FieldName { get; set; }

        public string FieldCode { get; set; }

        public string FieldType { get; set; }

        public string AGYCODE { get; set; }

        public string Mode { get; set; }

        public string FileName { get; set; }

        public string RespType { get; set; }

        public RankDefinitions RD_Form { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion
        private bool boolChangeStatus = false;


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Ranking Categories Form");
        }

        private void FillCountIndicator()
        {
            cmbCntInd.Items.Clear();
            List<ListItem> listitem = new List<ListItem>();
            listitem.Add(new ListItem("All Members", "M"));
            listitem.Add(new ListItem("Applicant Only", "A"));
            listitem.Add(new ListItem("Highest Points Only", "H"));
            listitem.Add(new ListItem("Lowest Points Only", "L"));
            cmbCntInd.Items.AddRange(listitem.ToArray());
            cmbCntInd.SelectedIndex = 0;
        }

        private void FillAgeCalculationIndicator()
        {
            cmbCntInd.Items.Clear();
            List<ListItem> list = new List<ListItem>();
            if (RespType == "H")
            {
                list.Add(new ListItem("Today's Date", "T"));
                list.Add(new ListItem("Intake Date", "I"));
                list.Add(new ListItem("Kindergarten Start Date", "K"));
            }
            else
            {
                list.Add(new ListItem("Today's Date", "T"));
                list.Add(new ListItem("Intake Date", "I"));
            }
            cmbAgeCalInd.Items.AddRange(list.ToArray());
            cmbAgeCalInd.SelectedIndex = 0;
        }

        private void Resp_type_screen()
        {
            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
            if (RespType == "B" || RespType == "T" || RespType == "N" || RespType == "$" || RespType == "&")
            {
                this.FromDt.Visible = true;
                this.ToDt.Visible = true;
                this.Response.ReadOnly = true;
                if (RespType == "B" || RespType == "T")
                {
                    this.FromDt.HeaderText = "From Dt";
                    this.ToDt.HeaderText = "To Dt";
                    
                }
                if (FileName.Trim() != "CASESNP")
                    this.Response.Visible = false;
                else
                {
                    this.Response.Visible = true;
                    this.Response.Width = 345;
                }
                this.FromDt.Width = 65;
                this.ToDt.Width = 65;

            }
            else if (RespType == "G" || RespType == "H")
            {
                this.FromDt.Visible = true;
                this.ToDt.Visible = true;
                this.Frm_Months.Visible = true;
                this.To_Months.Visible = true;
                this.Response.ReadOnly = true;
                this.FromDt.HeaderText = "From Yrs";
                this.ToDt.HeaderText = "To Yrs";
                this.Frm_Months.HeaderText = "Frm Months";
                this.To_Months.HeaderText = "To Months";
                this.FromDt.MaxInputLength = 3;
                this.ToDt.MaxInputLength = 3;
                this.Frm_Months.MaxInputLength = 2;
                this.To_Months.MaxInputLength = 2;
                this.Response.Width = 300;
            }
            else if (RespType == "X")
            {
                this.FromDt.Visible = false;
                this.ToDt.Visible = false;
                this.Response.Width = 490;
                this.Response.ReadOnly = false;
            }
            else
            {
                this.FromDt.Visible = false;
                this.ToDt.Visible = false;
                this.Response.ReadOnly = true;
                this.Response.Width = 490;
            }
            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
        }


        string Crit_Id = null; string SequenceCd = null;

        private void FillResponsGrid(string Relation, string code)
        {
            //gvResponse.Rows.Clear();
            List<RNKCRIT1Entity1> Critlist1;
            Critlist1 = _model.SPAdminData.Browse_RankCritiria1(hierarichy, RnkCd.ToString(), ScrCd.ToString(), FieldCode.ToString());
            if (Critlist1.Count > 0)
            {
                foreach (RNKCRIT1Entity1 dr in Critlist1)
                {
                    Crit_Id = dr.Id; break;
                }
            }
            else
                Mode = "Add";

            switch (RespType)
            {
                case "D": DropDownList(Relation, code); break;
                case "C": CheckedorRadio(Relation, code); break;
                case "L": DropDownList(Relation, code); break;
                case "R": CheckedorRadio(Relation, code); break;
                case "W": Caseworker(Relation, code); break;
                case "Z": ZipCode(Relation, code); break;
                case "B": DateSection(Relation, code); break;
                case "G": AgeSection(Relation, code); break;
                case "T": DateSection(Relation, code); break;
                case "N": NumericSection(Relation, code); break;
                case "$": NumericSection(Relation, code); break;
                case "&": NumericSection(Relation, code); break;
                case "H": AgeSection(Relation, code); break;
                case "S": SiteSection(Relation, code); break;
                case "Q": ServicesSection(Relation, code); break;
                case "X": TextSection(Relation, code); break;
            }
        }

        private void DropDownList(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List;
            int rowindex = 0;
            if (ScrCd == "CUSTQUES" || (ScrCd=="PREASSES" && Screen_Sw=="Y"))
            {
                DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(FieldCode);
                DataTable dtCustResp = dsCustResp.Tables[0];

                foreach (DataRow drCustResp in dtCustResp.Rows)
                {
                    string Desc = drCustResp["RSP_DESC"].ToString();
                    string RspCd = drCustResp["RSP_RESP_CODE"].ToString();
                    if (Mode == "Add")
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                    else
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                    bool Code_Exists = false; string Point = null;

                    string seq = null;
                    foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                    {
                        Code_Exists = false;
                        if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == crit2list.RespCd.ToString().Trim())
                        {
                            if (Mode == "Edit")
                            {
                                Point = crit2list.Points.ToString();
                                seq = crit2list.Seq.ToString();
                                Code_Exists = true; break;
                            }
                        }
                    }

                    if (Code_Exists)
                        rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", RspCd, Point, Point, " ", " ", " ", " ", "N", seq, " ");
                    else
                        rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", RspCd, " ", " ", " ", " ", " ", " ", "N", " ", " ");

                }
            }
            else
            {
                DataSet dsResp = new DataSet();
                DataTable dtResp = new DataTable();
                if (!string.IsNullOrEmpty(AGYCODE.ToString()))
                {
                    if (AGYCODE.ToString().Substring(0, 1).ToString() == "S")
                    {
                        dsResp = DatabaseLayer.CaseSum.GetAGYTABS(AGYCODE);
                        dtResp = dsResp.Tables[0];
                    }
                    else
                    {
                        dsResp = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(AGYCODE);
                        dtResp = dsResp.Tables[0];
                    }
                    foreach (DataRow drResp in dtResp.Rows)
                    {
                        string Agy_cd = null, Desc=null;
                        if (RespType == "L")
                        {
                            Desc = drResp["AGYS_DESC"].ToString();
                            Agy_cd = drResp["AGYS_CODE"].ToString();
                        }
                        else
                        {
                            Desc = drResp["LookUpDesc"].ToString();
                            Agy_cd = drResp["Code"].ToString();
                        }
                        if (Mode == "Add")
                            RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                        else
                            RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                        bool Code_Exists = false; string Point = null;

                        if (RespType == "D" || RespType == "L")
                        {
                            string seq = null;
                            foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                            {
                                Code_Exists = false;
                                if (FileName.Trim() == "CASESNP")
                                {
                                    if (Agy_cd.Trim() == crit2list.RespCd.ToString().Trim() && crit2list.Relation.ToString() == code)
                                    {
                                        if (Mode == "Edit")
                                        {
                                            if (crit2list.CountInd.Trim() == ((ListItem)cmbCntInd.SelectedItem).Value.ToString().Trim())
                                            {
                                                Point = crit2list.Points.ToString();
                                                seq = crit2list.Seq.ToString();
                                                Code_Exists = true; break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((ListItem)cmbCntInd.SelectedItem).Value.ToString() == "A")
                                        {
                                            if (Agy_cd.Trim() == crit2list.RespCd.ToString().Trim() &&  string.IsNullOrEmpty(crit2list.Relation.ToString().Trim()))
                                            {
                                                if (Mode == "Edit")
                                                {
                                                    Point = crit2list.Points.ToString();
                                                    seq = crit2list.Seq.ToString();
                                                    Code_Exists = true; break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Agy_cd.Trim() == crit2list.RespCd.ToString().Trim())
                                    {
                                        if (Mode == "Edit")
                                        {
                                            Point = crit2list.Points.ToString();
                                            seq = crit2list.Seq.ToString();
                                            Code_Exists = true; break;
                                        }
                                    }
                                }
                            }

                            if (Code_Exists)
                                rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Agy_cd, Point, Point, " ", " ", " ", " ", "N", seq, code);
                            else
                                rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Agy_cd, " ", " ", " ", " ", " ", " ", "N", " ", code);

                        }
                    }
                }
            }
        }


        private void CheckedorRadio(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List;
            if (RespType == "C" || RespType == "R")
            {
                if (ScrCd == "CUSTQUES" || (ScrCd=="PREASSES" && Screen_Sw=="Y"))
                {
                    int rowindex = 0;
                    DataSet dsCustResp = DatabaseLayer.SPAdminDB.BrowseCustResp(FieldCode);
                    DataTable dtCustResp = dsCustResp.Tables[0];

                    foreach (DataRow drCustResp in dtCustResp.Rows)
                    {
                        string Desc = drCustResp["RSP_DESC"].ToString();
                        string RspCd = drCustResp["RSP_RESP_CODE"].ToString();
                        if (Mode == "Add")
                            RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                        else
                            RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                        bool Code_Exists = false; string Point = null;

                        string seq = null;
                        foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                        {
                            Code_Exists = false;
                            if (drCustResp["RSP_RESP_CODE"].ToString().Trim() == crit2list.RespCd.ToString().Trim())
                            {
                                if (Mode == "Edit")
                                {
                                    Point = crit2list.Points.ToString();
                                    seq = crit2list.Seq.ToString();
                                    Code_Exists = true; break;
                                }
                            }
                        }

                        if (Code_Exists)
                            rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", RspCd, Point, Point, " ", " ", " ", " ", "N", seq, " ");
                        else
                            rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", RspCd, " ", " ", " ", " ", " ", " ", "N", " ", " ");

                    }
                }
                else
                {

                    if (Mode == "Add")
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                    else
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                    bool Code_Exists = false; string Point = null;

                    if (RespType == "C" || RespType == "R")
                    {
                        Code_Exists = false;
                        Point = null; string seq = null;
                        foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                        {
                            if (crit2list.RespCd.Trim() == "Y" || crit2list.RespCd.Trim() == "C")
                            {
                                if (Mode == "Edit")
                                {
                                    Code_Exists = true;
                                    Point = crit2list.Points.ToString();
                                    seq = crit2list.Seq.ToString();
                                }
                            }
                        }
                        if (Code_Exists)
                        {
                            if (RespType == "C")
                                gvResponse.Rows.Add(Relation + "Checked", " ", " ", " ", " ", "Y", Point, Point, " ", " ", " ", " ", "N", seq, code);
                            else
                                gvResponse.Rows.Add(Relation + "Selected", " ", " ", " ", " ", "C", Point, Point, " ", " ", " ", " ", "N", seq, code);
                        }
                        else
                        {
                            if (RespType == "C")
                                gvResponse.Rows.Add(Relation + "Checked", " ", " ", " ", " ", "Y", " ", " ", " ", " ", " ", " ", "N", seq, code);
                            else
                                gvResponse.Rows.Add(Relation + "Selected", " ", " ", " ", " ", "C", " ", " ", " ", " ", " ", " ", "N", seq, code);
                        }

                        Code_Exists = false;
                        Point = null;
                        foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                        {
                            if (crit2list.RespCd.Trim() == "N" || crit2list.RespCd.Trim() == "U")
                            {
                                if (Mode == "Edit")
                                {
                                    Code_Exists = true;
                                    Point = crit2list.Points.ToString();
                                    seq = crit2list.Seq.ToString();
                                }
                            }
                        }
                        if (Code_Exists)
                        {
                            if (RespType == "C")
                                gvResponse.Rows.Add(Relation + "Unchecked", " ", " ", " ", " ", "N", Point, Point, " ", " ", " ", " ", "N", seq, code);
                            else
                                gvResponse.Rows.Add(Relation + "UnSelected", " ", " ", " ", " ", "U", Point, Point, " ", " ", " ", " ", "N", seq, code);
                        }
                        else
                        {
                            if (RespType == "C")
                                gvResponse.Rows.Add(Relation + "Unchecked", " ", " ", " ", " ", "N", " ", " ", " ", " ", " ", " ", "N", seq, code);
                            else
                                gvResponse.Rows.Add(Relation + "UnSelected", " ", " ", " ", " ", "U", " ", " ", " ", " ", " ", " ", "N", seq, code);
                        }
                    }

                }
            }
        }

        private void Caseworker(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List;
            int rowindex = 0;
            //DataSet dsWorker = DatabaseLayer.CaseMst.GetCaseWorker("1", "**", "**", "**");
            //DataTable dtWorker = dsWorker.Tables[0];
            DataSet dsWorker = new DataSet();
            if(hierarichy=="**")
                dsWorker = DatabaseLayer.CaseMst.GetAllCaseWorkers(string.Empty);
            else
                dsWorker = DatabaseLayer.CaseMst.GetAllCaseWorkers(hierarichy);
            DataTable dtWorker = dsWorker.Tables[0];

            foreach (DataRow drWorker in dtWorker.Rows)
            {
                string Desc = drWorker["NAME"].ToString();
                string Code = drWorker["PWH_CASEWORKER"].ToString();

                if (Mode == "Add")
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                else
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                bool Code_Exists = false; string Point = null;

                string seq = null;
                foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                {
                    Code_Exists = false;
                    if (Code.ToString().Trim() == crit2list.RespCd.ToString().Trim())
                    {
                        if (Mode == "Edit")
                        {
                            Point = crit2list.Points.ToString();
                            seq = crit2list.Seq.ToString();
                            Code_Exists = true; break;
                        }
                    }

                }

                if (Code_Exists)
                    rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, Point, Point, " ", " ", " ", " ", "N", seq, code);
                else
                    rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, " ", " ", " ", " ", " ", " ", "N", " ", code);

            }
        }

        private void SiteSection(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List;
            List<CaseSiteEntity> Site_List;
            int rowindex = 0;
            if(hierarichy=="**")
                Site_List = _model.CaseMstData.GetCaseSiteAll();
            else
                Site_List = _model.CaseMstData.GetCaseSite(hierarichy.Trim(), null, null, "SiteHie");
            string Priv_Code = string.Empty;
            if (Site_List.Count > 0)
            {
                foreach (CaseSiteEntity drSite in Site_List)
                {
                    string Desc = drSite.SiteNAME.ToString().Trim();
                    string Code = drSite.SiteNUMBER.ToString().Trim();
                    if (Code != Priv_Code)
                    {
                        if (Mode == "Add")
                            RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                        else
                            RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                        bool Code_Exists = false; string Point = null;

                        string seq = null;
                        foreach (RNKCRIT2Entity crit2List in RankCrit2List)
                        {
                            Code_Exists = false;
                            if (drSite.SiteNUMBER.ToString().Trim() == crit2List.RespCd.Trim())
                            {
                                if (Mode == "Edit")
                                {
                                    Point = crit2List.Points.ToString();
                                    seq = crit2List.Seq.ToString();
                                    Code_Exists = true; break;
                                }
                            }
                        }

                        if (Code_Exists)
                            rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, Point, Point, " ", " ", " ", " ", "N", seq, code);
                        else
                            rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, " ", " ", " ", " ", " ", " ", "N", " ", code);

                        Priv_Code = Code;
                    }
                }
            }

        }

        private void ServicesSection(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List;
            int rowindex = 0;
            DataSet dsServices = DatabaseLayer.HierarchyPlusProgram.GetPrograms();
            DataTable dtServices = dsServices.Tables[0];

            if (dtServices.Rows.Count > 0)
            {
                foreach (DataRow drProgrammes in dtServices.Rows)
                {
                    string Desc = drProgrammes["DEP_AGCY"].ToString().Trim();
                    string Code = drProgrammes["DEP_AGENCY"].ToString().Trim() + drProgrammes["DEP_DEPT"].ToString().Trim() + drProgrammes["DEP_PROGRAM"].ToString().Trim();

                    if (Mode == "Add")
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                    else
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                    bool Code_Exists = false; string Point = null;

                    string seq = null;
                    foreach (RNKCRIT2Entity crit2List in RankCrit2List)
                    {
                        Code_Exists = false;
                        if (Code.Trim() == crit2List.RespCd.Trim())
                        {
                            if (Mode == "Edit")
                            {
                                Point = crit2List.Points.ToString();
                                seq = crit2List.Seq.ToString();
                                Code_Exists = true; break;
                            }
                        }
                    }

                    if (Code_Exists)
                        rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, Point, Point, " ", " ", " ", " ", "N", seq, code);
                    else
                        rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, " ", " ", " ", " ", " ", " ", "N", " ", code);
                }
            }
        }

        private void ZipCode(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List;
            int rowindex = 0;
            DataSet dsZip = DatabaseLayer.ZipCodePlusAgency.GetZipCode();
            DataTable dtZip = dsZip.Tables[0];
            foreach (DataRow drZip in dtZip.Rows)
            {
                string Desc = SetLeadingZeros(drZip["ZCR_ZIP"].ToString()) + "-" + SetLeadingZeros(drZip["ZCRPLUS_4"].ToString()) + "-" + drZip["ZCR_CITY"].ToString();
                string Code = drZip["ZCR_ZIP"].ToString();

                if (Mode == "Add")
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                else
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                bool Code_Exists = false; string Point = null;

                string seq = null;
                foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                {
                    Code_Exists = false;
                    if (drZip["ZCR_ZIP"].ToString().Trim() == crit2list.RespCd.ToString().Trim())
                    {
                        if (Mode == "Edit")
                        {
                            Point = crit2list.Points.ToString();
                            seq = crit2list.Seq.ToString();
                            Code_Exists = true; break;
                        }
                    }

                }

                if (Code_Exists)
                    rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, Point, Point, " ", " ", " ", " ", "N", seq, code);
                else
                    rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", Code, " ", " ", " ", " ", " ", " ", "N", " ", code);
            }

        }

        private void DateSection(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List; int rowindex = 0;
            if (ScrCd == "CUSTQUES" || (ScrCd == "PREASSES" && Screen_Sw == "Y"))
            {
                if (Mode == "Add")
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                else
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                bool Code_Exists = false; string Point = null;

                Code_Exists = false;
                Point = null;
                string seq = null, frmDt = null, toDt = null;
                foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                {
                    if (crit2list.RespCd.Trim() == RespType)
                    {
                        if (Mode == "Edit")
                        {
                            Code_Exists = false;
                            Point = crit2list.Points.ToString();
                            frmDt = LookupDataAccess.Getdate(crit2list.GtDate.ToString());
                            toDt = LookupDataAccess.Getdate(crit2list.LtDate.ToString());
                            seq = crit2list.Seq.ToString();
                            Code_Exists = true;
                        }
                        if (Code_Exists)
                            rowindex = gvResponse.Rows.Add(Relation, frmDt, " ", toDt, " ", RespType, Point, Point, frmDt, " ", toDt, " ", "N", seq, code);
                    }

                }
                
                if(!Code_Exists)
                    rowindex = gvResponse.Rows.Add(Relation, null, " ", null, " ", RespType, " ", " ", " ", " ", " ", " ", "N", " ", code);

            }
            else
            {
                if (RespType == "B" || RespType == "T")
                {
                    if (Mode == "Add")
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                    else
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                    bool Code_Exists = false; string Point = null;
                    string seq = null, frmDt = null, toDt = null;
                    bool EditCritaria = false;
                    foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                    {
                        Code_Exists = false;
                        if (crit2list.RespCd.Trim() == RespType)
                        {
                            if (FileName.Trim() == "CASESNP")
                            {
                                if (crit2list.Relation == code)
                                {
                                    if (Mode == "Edit")
                                    {
                                        if (crit2list.CountInd.Trim() == ((ListItem)cmbCntInd.SelectedItem).Value.ToString().Trim())
                                        {
                                            Point = crit2list.Points.ToString();
                                            frmDt = LookupDataAccess.Getdate(crit2list.GtDate.ToString());
                                            toDt = LookupDataAccess.Getdate(crit2list.LtDate.ToString());
                                            seq = crit2list.Seq.ToString();
                                            Code_Exists = true; //break;
                                            EditCritaria = true;
                                            rowindex = gvResponse.Rows.Add(Relation, frmDt, " ", toDt, " ", RespType, Point, Point, frmDt, " ", toDt, " ", "N", seq, code);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Mode == "Edit")
                                {
                                    Point = crit2list.Points.ToString();
                                    frmDt = LookupDataAccess.Getdate(crit2list.GtDate.ToString());
                                    toDt = LookupDataAccess.Getdate(crit2list.LtDate.ToString());
                                    seq = crit2list.Seq.ToString();
                                    Code_Exists = true; //break;                                   
                                }
                                if (Code_Exists)
                                    rowindex = gvResponse.Rows.Add(Relation, frmDt, " ", toDt, " ", RespType, Point, Point, frmDt, " ", toDt, " ", "N", seq, code);
                               
                            }
                        }
                        else if (FileName.Trim() == "CASEMST" || FileName.Trim() == "CHLDMST")
                        {
                            if (Mode == "Edit")
                            {
                                Point = crit2list.Points.ToString();
                                frmDt = LookupDataAccess.Getdate(crit2list.GtDate.ToString());
                                toDt = LookupDataAccess.Getdate(crit2list.LtDate.ToString());
                                seq = crit2list.Seq.ToString();
                                Code_Exists = true; //break;                                   
                            }
                            if (Code_Exists)
                                rowindex = gvResponse.Rows.Add(Relation, frmDt, " ", toDt, " ", RespType, Point, Point, frmDt, " ", toDt, " ", "N", seq, code);
                        }


                    }
                    if (FileName.Trim() == "CASESNP")
                    {
                        if (Code_Exists==false && EditCritaria == false)                     
                            rowindex = gvResponse.Rows.Add(Relation, null, " ", null, " ", RespType, " ", " ", " ", " ", " ", " ", "N", " ", code);
                    }
                }

            }
        }

        private void NumericSection(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List; int rowindex = 0;
            if (ScrCd == "CUSTQUES" || (ScrCd == "PREASSES" && Screen_Sw == "Y"))
            {
                if (Mode == "Add")
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                else
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                bool Code_Exists = false; string Point = null;

                Code_Exists = false;
                Point = null;
                string seq = null, frmNum = null, toNum = null;
                foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                {
                    //if (crit2list.RespCd.Trim() == RespType)
                    //{
                    //    if (Mode == "Edit")
                        {
                            Code_Exists = false;
                            Point = crit2list.Points.ToString();
                            frmNum = crit2list.GtNum.ToString();
                            toNum = crit2list.LtNum.ToString();
                            seq = crit2list.Seq.ToString();
                            Code_Exists = true;
                        }
                    //}
                        if (Code_Exists)
                            rowindex = gvResponse.Rows.Add(Relation, frmNum, " ", toNum, " ", RespType, Point, Point, frmNum, " ", toNum, " ", "N", seq, code);
                }
                if (!Code_Exists)
                   rowindex = gvResponse.Rows.Add(Relation, null, " ", null, " ", RespType, " ", " ", " ", " ", " ", " ", "N", " ", code);

            }

            else
            {
                if (RespType == "N" || RespType == "$" || RespType == "&")
                {
                    if (Mode == "Add")
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                    else
                        RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                    bool Code_Exists = false; string Point = null;
                    bool EditCritaria = false;
                    if (RespType == "N" || RespType == "$" || RespType == "&")
                    {
                        Code_Exists = false;
                        Point = null;
                        string seq = null, frmNum = null, toNum = null;

                        foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                        {
                            Code_Exists = false;
                            if (crit2list.RespCd.Trim() == RespType)
                            {
                                if (FileName.Trim() == "CASESNP")
                                {
                                    if (crit2list.Relation == code)
                                    {

                                        if (Mode == "Edit" && crit2list.CountInd.Trim() != "A")
                                        {
                                            if (crit2list.CountInd.Trim() == ((ListItem)cmbCntInd.SelectedItem).Value.ToString().Trim())
                                            {
                                                Point = crit2list.Points.ToString();
                                                frmNum = crit2list.GtNum.ToString();
                                                toNum = crit2list.LtNum.ToString();
                                                seq = crit2list.Seq.ToString();
                                                Code_Exists = true; //break;
                                                EditCritaria = true;
                                                rowindex = gvResponse.Rows.Add(Relation, frmNum, " ", toNum, " ", RespType, Point, Point, frmNum, " ", toNum, " ", "N", seq, code);
                                            }
                                        }
                                        else if (crit2list.CountInd.Trim() == "A")
                                        {
                                            if (Mode == "Edit")
                                            {
                                                Point = crit2list.Points.ToString();
                                                frmNum = crit2list.GtNum.ToString();
                                                toNum = crit2list.LtNum.ToString();
                                                seq = crit2list.Seq.ToString();
                                                Code_Exists = true; //break;
                                            }
                                            if (Code_Exists)
                                                rowindex = gvResponse.Rows.Add(Relation, frmNum, " ", toNum, " ", RespType, Point, Point, frmNum, " ", toNum, " ", "N", seq, code);
                                        }
                                    
                                    }
                                }
                                else
                                {
                                    if (Mode == "Edit")
                                    {
                                        Point = crit2list.Points.ToString();
                                        frmNum = crit2list.GtNum.ToString();
                                        toNum = crit2list.LtNum.ToString();
                                        seq = crit2list.Seq.ToString();
                                        Code_Exists = true; //break;
                                    }
                                    if (Code_Exists)
                                        rowindex = gvResponse.Rows.Add(Relation, frmNum, " ", toNum, " ", RespType, Point, Point, frmNum, " ", toNum, " ", "N", seq, code);
                                }

                            }
                            else if (FileName.Trim() == "CASEMST")
                            {
                                if (Mode == "Edit")
                                {
                                    Point = crit2list.Points.ToString();
                                    frmNum = crit2list.GtNum.ToString();
                                    toNum = crit2list.LtNum.ToString();
                                    seq = crit2list.Seq.ToString();
                                    Code_Exists = true; //break;
                                }
                                if (Code_Exists)
                                    rowindex = gvResponse.Rows.Add(Relation, frmNum, " ", toNum, " ", RespType, Point, Point, frmNum, " ", toNum, " ", "N", seq, code);
                            }

                        }
                        if (FileName.Trim() == "CASESNP")
                        {
                            if (Code_Exists == false && EditCritaria == false)
                                rowindex = gvResponse.Rows.Add(Relation, null, " ", null, " ", RespType, " ", " ", " ", " ", " ", " ", "N", " ", code);
                        }
                            

                    }
                }
            }
        }

        private void AgeSection(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List; int rowindex = 0;
            if (RespType == "G" || RespType == " H")
            {
                if (Mode == "Add")
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                else
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                bool Code_Exists = false; string Point = null;
                if (RespType == "G" || RespType == "H")
                {
                    Code_Exists = false;
                    Point = null;
                    string seq = null;
                    decimal frmNum = 0, toNum = 0, monthfrm = 0, monthto = 0;
                    
                    foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                    {
                        if (crit2list.RespCd == RespType && crit2list.Relation == code)
                        {
                            if (Mode == "Edit")
                            {
                                Code_Exists = true;
                                decimal FromYear = decimal.Divide((decimal.Parse(crit2list.GtNum.ToString())), 12);
                                decimal ToYear = decimal.Divide((decimal.Parse(crit2list.LtNum.ToString())), 12);
                                frmNum = decimal.Truncate(FromYear);
                                toNum = decimal.Truncate(ToYear);
                                monthfrm = decimal.Remainder((decimal.Parse(crit2list.GtNum.ToString())), 12);
                                monthto = decimal.Remainder((decimal.Parse(crit2list.LtNum.ToString())), 12);
                                monthfrm = decimal.Truncate(monthfrm); monthto = decimal.Truncate(monthto);
                                Point = crit2list.Points.ToString();
                                seq = crit2list.Seq.ToString();
                                //CommonFunctions.SetComboBoxValue(cmbAgeCalInd, crit2list.AgeClcInd);
                                
                            }
                        }
                    }
                    if (Code_Exists)
                        rowindex = gvResponse.Rows.Add(Relation, frmNum, monthfrm, toNum, monthto, RespType, Point, Point, frmNum, monthfrm, toNum, monthto, "N", seq, code);
                    else
                        rowindex = gvResponse.Rows.Add(Relation, null, null, null, null, RespType, " ", " ", " ", " ", " ", " ", "N", " ", code);
                }

            }
        }

        private void TextSection(string Relation, string code)
        {
            List<RNKCRIT2Entity> RankCrit2List; int rowindex = 0;
            if (ScrCd == "CUSTQUES" || (ScrCd == "PREASSES" && Screen_Sw == "Y"))
            {
                this.Response.ReadOnly = false;
                if (Mode == "Add")
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(null);
                else
                    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                bool Code_Exists = false; string Point = null;

                Code_Exists = false;
                Point = null; string Desc = null;
                string seq = null; //frmNum = null, toNum = null;
                foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                {
                    if (crit2list.RespCd.Trim() == "")
                    {
                        if (Mode == "Edit")
                        {
                            Code_Exists = false;
                            Point = crit2list.Points.ToString();
                            Desc = crit2list.RespText.ToString();
                            seq = crit2list.Seq.ToString();
                            Code_Exists = true;
                        }
                        if (Code_Exists)
                            rowindex = gvResponse.Rows.Add(Relation + Desc, " ", " ", " ", " ", RespType, Point, Point, " ", " ", " ", " ", "N", seq, code);
                    }

                }
                if (!Code_Exists)
                    rowindex = gvResponse.Rows.Add(Relation, " ", " ", " ", " ", RespType, " ", " ", " ", " ", " ", " ", "N", " ", code);

            }
        }


        bool IsValid = true;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string DelteFunc = null;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (boolChangeStatus)
            {
                if (GridValidate())
                {
                    try
                    {
                        int NewCode = 0;
                        int NewSeq = 1; string Del_Flag = "N";
                        CaptainModel model = new CaptainModel();
                        RNKCRIT1Entity1 rnkCrit1 = new RNKCRIT1Entity1();
                        string CurrentHie = null;
                        //for (int i = 0; i < hierarichy.Length; )
                        //{

                        //CurrentHie = hierarichy.Substring(i, 6);

                        if (Mode == "Add")
                            AlertBox.Show("Ranking Criteria Selection Saved Successfully");
                        else
                            AlertBox.Show("Ranking Criteria Selection Updated Successfully");

                        if (Mode == "Add")
                        {
                            rnkCrit1.Agency = hierarichy;
                            //rnkCrit1.Dept = CurrentHie.Substring(2, 2).ToString();
                            //rnkCrit1.Prog = CurrentHie.Substring(4, 2).ToString();
                        }
                        else
                        {
                            rnkCrit1.Agency = hierarichy;
                            //rnkCrit1.Dept = hierarichy.Substring(2, 2).ToString();
                            //rnkCrit1.Prog = hierarichy.Substring(4, 2).ToString();
                            rnkCrit1.Id = Crit_Id.ToString();
                        }
                        rnkCrit1.RankCtg = RnkCd.ToString();
                        rnkCrit1.Scr_Cd = ScrCd.ToString();
                        rnkCrit1.Fld_cd = FieldCode.ToString();
                        rnkCrit1.Fld_type = FieldType.ToString();
                        //if (FileName.Trim() == "CASESNP")
                        //{
                        //    //if (RespType == "G" || RespType == "H")
                        //    //    rnkCrit1.AgeClcInd = ((ListItem)cmbAgeCalInd.SelectedItem).Value.ToString();
                        //    //rnkCrit1.CountInd = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                        //}
                        rnkCrit1.Addoperator = Baseform.UserID;
                        rnkCrit1.LstcOperator = Baseform.UserID;
                        rnkCrit1.Mode = Mode;

                        _model.SPAdminData.InsertRankCRIT1(rnkCrit1, out NewCode);

                        RNKCRIT2Entity RespEntity = new RNKCRIT2Entity();
                        if (Mode == "Add")
                            RespEntity.Id = NewCode.ToString();
                        else
                            RespEntity.Id = Crit_Id.ToString();

                        foreach (DataGridViewRow dr in gvResponse.Rows)
                        {

                            if (dr.Cells["Row_Status"].Value.ToString() != "N")
                            {
                                if (dr.Cells["Row_Status"].Value.ToString() == "D")
                                {
                                    RespEntity.Seq = dr.Cells["Seq"].Value.ToString();
                                    RespEntity.RespCd = dr.Cells["Res_code"].Value.ToString();
                                }
                                else
                                {
                                    if (Mode == "Add")
                                        RespEntity.Seq = NewSeq.ToString();
                                    else
                                        RespEntity.Seq = dr.Cells["Seq"].Value.ToString() == null ? string.Empty : dr.Cells["Seq"].Value.ToString();
                                    if ((ScrCd == "CUSTQUES" || (ScrCd == "PREASSES" && Screen_Sw == "Y")) && string.IsNullOrEmpty(dr.Cells["Res_code"].Value.ToString().Trim()))
                                        RespEntity.RespCd = RespType;
                                    else
                                        RespEntity.RespCd = dr.Cells["Res_code"].Value.ToString().Trim();
                                    if (RespType == "B" || RespType == "T")
                                    {
                                        if (FileName.Trim() == "CASEMST" || FileName.Trim() == "CHLDMST")
                                        {
                                            RespEntity.GtDate = dr.Cells["FromDt"].Value.ToString();
                                            RespEntity.LtDate = dr.Cells["ToDt"].Value.ToString();
                                            RespEntity.RespText = "From Dt: " + LookupDataAccess.Getdate(dr.Cells["FromDt"].Value.ToString().Trim()) + "  To Dt: " + LookupDataAccess.Getdate(dr.Cells["ToDt"].Value.ToString().Trim());
                                        }
                                        else
                                        {
                                            //if (!string.IsNullOrEmpty(dr.Cells["Response"].Value.ToString().Trim()) && !string.IsNullOrEmpty(dr.Cells["RelationCd"].Value.ToString().Trim()))
                                            //{
                                            //    RespEntity.RespText = dr.Cells["Response"].Value.ToString().Trim();
                                            //    RespEntity.Relation = dr.Cells["RelationCd"].Value.ToString();
                                            //}
                                            if (FileName.Trim() == "CASESNP")
                                            {
                                                RespEntity.RespText = dr.Cells["Response"].Value.ToString().Trim();
                                                RespEntity.Relation = dr.Cells["RelationCd"].Value.ToString();
                                            }
                                            RespEntity.GtDate = dr.Cells["FromDt"].Value.ToString();
                                            RespEntity.LtDate = dr.Cells["ToDt"].Value.ToString();
                                        }
                                    }
                                    else if (RespType == "N" || RespType == "$" || RespType == "&")
                                    {
                                        if (FileName.Trim() == "CASEMST" || ScrCd == "CUSTQUES" || FileName.Trim() == "CHLDMST" || (ScrCd == "PREASSES" && Screen_Sw == "Y"))
                                        {
                                            RespEntity.GtNum = dr.Cells["FromDt"].Value.ToString();
                                            RespEntity.LtNum = dr.Cells["ToDt"].Value.ToString();
                                            RespEntity.RespText = "From : " + dr.Cells["FromDt"].Value.ToString().Trim() + "  To : " + dr.Cells["ToDt"].Value.ToString().Trim();
                                        }
                                        else
                                        {
                                            if (((ListItem)cmbCntInd.SelectedItem).Value.ToString() != "A")
                                            {
                                                RespEntity.RespText = dr.Cells["Response"].Value.ToString().Trim();
                                                RespEntity.Relation = dr.Cells["RelationCd"].Value.ToString();
                                            }
                                            else
                                                RespEntity.RespText = "From : " + dr.Cells["FromDt"].Value.ToString().Trim() + "  To : " + dr.Cells["ToDt"].Value.ToString().Trim();

                                            RespEntity.GtNum = dr.Cells["FromDt"].Value.ToString();
                                            RespEntity.LtNum = dr.Cells["ToDt"].Value.ToString();
                                        }
                                    }
                                    else if (RespType == "X")
                                    {
                                        RespEntity.RespText = dr.Cells["Response"].Value.ToString().Trim();
                                        RespEntity.RespCd = "";
                                    }
                                    else if (RespType == "G" || RespType == "H")
                                    {
                                        int FromAge = int.Parse(dr.Cells["FromDt"].Value.ToString());
                                        int frmMnths = int.Parse(dr.Cells["Frm_Months"].Value.ToString());
                                        int ToAge = int.Parse(dr.Cells["ToDt"].Value.ToString());
                                        int ToMnths = int.Parse(dr.Cells["To_Months"].Value.ToString());
                                        RespEntity.RespText = dr.Cells["Response"].Value.ToString().Trim();
                                        RespEntity.Relation = dr.Cells["RelationCd"].Value.ToString();
                                        if (FileName.Trim() == "CASESNP")
                                            RespEntity.AgeClcInd = ((ListItem)cmbAgeCalInd.SelectedItem).Value.ToString();
                                        RespEntity.GtNum = ((FromAge * 12) + frmMnths).ToString();
                                        RespEntity.LtNum = ((ToAge * 12) + ToMnths).ToString();
                                    }
                                    else
                                    {
                                        if (FileName.Trim() == "CASESNP")
                                        {
                                            if (((ListItem)cmbCntInd.SelectedItem).Value.ToString() != "A")
                                                RespEntity.Relation = dr.Cells["RelationCd"].Value.ToString();
                                        }
                                        RespEntity.RespText = dr.Cells["Response"].Value.ToString().Trim();
                                    }
                                    RespEntity.Points = dr.Cells["Points"].Value.ToString();
                                    if (FileName.Trim() == "CASESNP")
                                        RespEntity.CountInd = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                                    RespEntity.Addoperator = Baseform.UserID;
                                    RespEntity.LstcOperator = Baseform.UserID;
                                }
                                RespEntity.Mode = dr.Cells["Row_Status"].Value.ToString();
                                RespEntity.RankCd = RnkCd.ToString();
                                _model.SPAdminData.InsertRankCRIT2(RespEntity, out NewSeq, out Del_Flag);
                                
                            }

                        }

                        //if (!IsUpdate)
                        //{
                        //    List<RNKCRIT2Entity> RankCrit2List;
                        //    RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());

                        //    if (RankCrit2List.Count > 0)
                        //    {
                        //        foreach (RNKCRIT2Entity crit2list in RankCrit2List)
                        //        {
                        //            RespEntity = crit2list;
                        //            if (RespType == "G" || RespType == "H")
                        //            {
                        //                if (FileName.Trim() == "CASESNP")
                        //                    RespEntity.AgeClcInd = ((ListItem)cmbAgeCalInd.SelectedItem).Value.ToString();
                        //            }

                        //            if (FileName.Trim() == "CASESNP")
                        //                RespEntity.CountInd = ((ListItem)cmbCntInd.SelectedItem).Value.ToString();
                        //            RespEntity.Addoperator = Baseform.UserID;
                        //            RespEntity.LstcOperator = Baseform.UserID;
                        //            _model.SPAdminData.InsertRankCRIT2(RespEntity, out NewSeq, out Del_Flag);
                        //        }
                        //    }
                        //}

                        //i += 6;
                        //}
                        if (Del_Flag == "Y")
                            DelteFunc = "Y";
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                    MessageBox.Show(" Provide Points for Atleast one Response", "CAPTAIN");
                
            }
            else
            {
                bool Cell_Points = false;
                int i = 0;
                //this.Close();
                Cell_Points = false;
                foreach (DataGridViewRow dr in gvResponse.Rows)
                {
                    if ((RespType == "B" || RespType == "T" || RespType == "N" || RespType == "$" || RespType == "&") && (FileName.Trim() == "CASEMST" || FileName.Trim() == "CHLDMST" || ScrCd == "CUSTQUES" || (ScrCd == "PREASSES" && Screen_Sw == "Y")))
                    {
                        if (i < gvResponse.Rows.Count-1)
                        {
                            if (!string.IsNullOrEmpty(dr.Cells["Points"].Value.ToString().Trim()))
                            {
                                Cell_Points = true;
                                string test = dr.Cells["Points"].Value.ToString(); //break;
                            }
                        }
                    }
                    else
                    {
                        if (i < gvResponse.Rows.Count)
                        {
                            if (!string.IsNullOrEmpty(dr.Cells["Points"].Value.ToString().Trim()))
                            {
                                Cell_Points = true;
                                string test = dr.Cells["Points"].Value.ToString(); //break;
                            }
                        }
                    }
                    i++;
                }
                if (!Cell_Points)
                    MessageBox.Show(" Provide Points for Atleast One Response", "CAPTAIN");
                else
                {
                    this.Close();
                }

            }
        }

        public bool ValidateForm()
        {
            bool isValid = true;
            foreach (DataGridViewRow dr in gvResponse.Rows)
            {
                if (string.IsNullOrEmpty(dr.Cells["FromDt"].ToString()))
                    MessageBox.Show("From Date Required", "CAPTAIN");
                if(string.IsNullOrEmpty(dr.Cells["ToDt"].ToString()))
                    MessageBox.Show("To Date Required", "CAPTAIN");
            }
            return isValid;
        }

        public bool GridValidate()
        {
            bool Cell_Points = false;
            int i = 0;
            //this.Close();
            Cell_Points = false;

            if (Mode == "Add")
            {
                foreach (DataGridViewRow dr in gvResponse.Rows)
                {
                    if (i <= gvResponse.Rows.Count-1)
                    {
                        if (!string.IsNullOrEmpty(dr.Cells["Points"].Value.ToString().Trim()))
                        {
                            Cell_Points = true;
                            string test = dr.Cells["Points"].Value.ToString(); //break;
                        }
                    }
                    i++;
                }
            }
            else
                Cell_Points = true;
            return Cell_Points;
        }

        public string get_DelFlag()
        {
            string Del_FlagCrit = null;
            if (DelteFunc == "Y")
            {
                DataSet dsRnkCrit1 = DatabaseLayer.SPAdminDB.BrowseRNKCRIT1(hierarichy, RnkCd.Trim(), null, null);
                DataTable dtRnkCrit1 = dsRnkCrit1.Tables[0];
                if(dtRnkCrit1.Rows.Count>0)
                    Del_FlagCrit = "N";
                else
                    Del_FlagCrit = DelteFunc.ToString();
            }

            return Del_FlagCrit;
        }

        private void gvResponse_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvResponse_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                if ((RespType == "B" || RespType == "T" || RespType == "N" || RespType == "$" || RespType == "&" && FileName.ToString() == "CASEMST") || RespType=="X")
                {
                    if (gvResponse.CurrentRow.Cells["Points"].Value != null || gvResponse.CurrentRow.Cells["Points"].Value != string.Empty)
                    {
                        if (gvResponse.CurrentRow.Cells["Dup_Points"].Value == null)
                        {
                            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                            gvResponse.CurrentRow.Cells["Dup_Points"].Value = " ";
                            gvResponse.CurrentRow.Cells["Res_code"].Value = RespType;
                            if (RespType == "X")
                            {
                            }
                            else
                                gvResponse.CurrentRow.Cells["Response"].Value = " ";
                            gvResponse.CurrentRow.Cells["RelationCd"].Value = " ";
                            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                        }

                    }
                }
                if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Points"].EditedFormattedValue.ToString()))
                {
                    //if (gvResponse.CurrentRow.Cells["Points"].Value.ToString() == "0")
                    //    MessageBox.Show("Points should not be Zero", "Capsystems");
                    //else 
                    if (gvResponse.CurrentRow.Cells["Dup_Points"].Value.ToString() != gvResponse.CurrentRow.Cells["Points"].Value.ToString())
                    {
                        if (gvResponse.CurrentRow.Cells["Dup_Points"].Value.ToString() == " ")
                            gvResponse.CurrentRow.Cells["Row_Status"].Value = "I";
                        else
                            gvResponse.CurrentRow.Cells["Row_Status"].Value = "U";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Dup_Points"].Value.ToString()))
                    {
                        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                        gvResponse.CurrentRow.Cells["Row_Status"].Value = "D";
                        boolChangeStatus = true;
                        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    }

                }
            }

                if (e.ColumnIndex == 1)
                {
                   if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["FromDt"].EditedFormattedValue.ToString()))
                    {
                        if ((gvResponse.CurrentRow.Cells["Dup_From"].Value.ToString() == null ? string.Empty : gvResponse.CurrentRow.Cells["Dup_From"].Value) != (gvResponse.CurrentRow.Cells["FromDt"].Value.ToString() == null ? string.Empty : gvResponse.CurrentRow.Cells["FromDt"].Value.ToString()))
                        {
                            if (gvResponse.CurrentRow.Cells["Dup_From"].Value.ToString() == " ")
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "I";
                            else
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "U";
                        }
                    }
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Dup_From"].Value.ToString()))
                    //    {
                    //        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //        gvResponse.CurrentRow.Cells["Row_Status"].Value = "D";
                    //        boolChangeStatus = true;
                    //        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //    }
                    //}
                }
                if (e.ColumnIndex == 2)
                {
                   if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Frm_Months"].EditedFormattedValue.ToString()))
                    {
                        if (gvResponse.CurrentRow.Cells["Dup_FrmMonths"].Value.ToString() != gvResponse.CurrentRow.Cells["Frm_Months"].Value.ToString())
                        {
                            if (gvResponse.CurrentRow.Cells["Dup_FrmMonths"].Value.ToString() == " ")
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "I";
                            else
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "U";
                        }
                    }
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Dup_FrmMonths"].Value.ToString()))
                    //    {
                    //        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //        gvResponse.CurrentRow.Cells["Row_Status"].Value = "D";
                    //        boolChangeStatus = true;
                    //        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //    }
                    //}
                }
                if (e.ColumnIndex == 3)
                {
                   if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["ToDt"].EditedFormattedValue.ToString()))
                    {
                        if ((gvResponse.CurrentRow.Cells["Dup_To"].Value.ToString() == null ? string.Empty : gvResponse.CurrentRow.Cells["Dup_To"].Value) != (gvResponse.CurrentRow.Cells["ToDt"].Value.ToString() == null ? string.Empty : gvResponse.CurrentRow.Cells["ToDt"].Value.ToString()))
                        {

                            if (gvResponse.CurrentRow.Cells["Dup_To"].Value.ToString() == " ")
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "I";
                            else
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "U";
                        }
                    }
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Dup_To"].Value.ToString()))
                    //    {
                    //        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //        gvResponse.CurrentRow.Cells["Row_Status"].Value = "D";
                    //        boolChangeStatus = true;
                    //        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //    }
                    //}
                }
                if (e.ColumnIndex == 4)
                {
                    if (RespType == "B" || RespType == "T" || RespType == "N" || RespType == "$" || RespType == "&" && FileName.ToString() == "CASEMST")
                    {
                        if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["To_Months"].Value.ToString()))
                        {
                            if (gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value == null)
                            {
                                gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value = " ";
                                gvResponse.CurrentRow.Cells["Res_code"].Value = RespType;
                                gvResponse.CurrentRow.Cells["Response"].Value = " ";
                                gvResponse.CurrentRow.Cells["RelationCd"].Value = " ";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["To_Months"].EditedFormattedValue.ToString()))
                    {
                        if (gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value.ToString() != gvResponse.CurrentRow.Cells["To_Months"].Value.ToString())
                        {
                            if (gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value.ToString() == " ")
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "I";
                            else
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "U";
                        }
                    }
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value.ToString()))
                    //    {
                    //        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //        gvResponse.CurrentRow.Cells["Row_Status"].Value = "D";
                    //        boolChangeStatus = true;
                    //        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                    //    }
                    //}
                }
                if (e.ColumnIndex == 0)
                {
                    if (RespType == "X")
                    {
                        if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Response"].Value.ToString()))
                        {
                            if (gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value == null)
                            {
                                gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value = " ";
                                gvResponse.CurrentRow.Cells["Res_code"].Value = RespType;
                                //gvResponse.CurrentRow.Cells["Response"].Value = " ";
                                gvResponse.CurrentRow.Cells["RelationCd"].Value = " ";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(gvResponse.CurrentRow.Cells["Response"].EditedFormattedValue.ToString()))
                    {
                        if (gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value.ToString() != gvResponse.CurrentRow.Cells["Response"].Value.ToString())
                        {
                            if (gvResponse.CurrentRow.Cells["Dup_ToMonths"].Value.ToString() == " ")
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "I";
                            else
                                gvResponse.CurrentRow.Cells["Row_Status"].Value = "U";
                        }
                    }
                }

            
        }

        private void RankCriteriaSelction_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void gvResponse_SelectionChanged(object sender, EventArgs e)
        {
            if (RespType == "D")
                SequenceCd = gvResponse.CurrentRow.Cells["Seq"].Value.ToString();
            else if (RespType == "G" || RespType == " H")
            {
                if (Mode == "Edit")
                {
                    List<RNKCRIT2Entity> RankCrit2List = _model.SPAdminData.Browse_RankCritiria2(Crit_Id.ToString());
                    RNKCRIT2Entity rnk2entity = RankCrit2List.Find(u => u.RespCd.Trim().Equals(gvResponse.CurrentRow.Cells["Res_code"].Value.ToString()) && u.Relation.Trim().Equals(gvResponse.CurrentRow.Cells["RelationCd"].Value.ToString()));
                    if (rnk2entity != null)
                        CommonFunctions.SetComboBoxValue(cmbAgeCalInd, rnk2entity.AgeClcInd.Trim());
                    else
                        CommonFunctions.SetComboBoxValue(cmbAgeCalInd, "T");
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
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
            }
            return (TmpCode);
        }

        List<CommonEntity> relationlist;

        string RelationDesc = null; string RelationCode = null;
        private void cmbCntInd_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvResponse.Rows.Clear();
            if (FileName.Trim() == "CASESNP" && !string.IsNullOrEmpty(cmbCntInd.Text))
            {
                if (((ListItem)cmbCntInd.SelectedItem).Value.ToString() == "A")
                {
                    if (RespType == "D" || RespType == "L" || RespType == "W" || RespType == "Z" || RespType == "C" || RespType == "R" || RespType=="N" || RespType=="$" || RespType=="&")
                        FillResponsGrid(null, string.Empty);
                    else
                    {
                        relationlist = _model.lookupDataAccess.GetRelationship();
                        if (relationlist.Count > 0)
                        {
                            int i = 0;
                            foreach (CommonEntity drRelation in relationlist)
                            {
                                if (i == 0)
                                    FillResponsGrid("All ", "*");

                                RelationDesc = drRelation.Desc.ToString() + " ";
                                RelationCode = drRelation.Code.ToString();
                                FillResponsGrid(RelationDesc, RelationCode);
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    relationlist = _model.lookupDataAccess.GetRelationship();
                    if (relationlist.Count > 0)
                    {
                        int i = 0;
                        foreach (CommonEntity drRelation in relationlist)
                        {
                            if (i == 0)
                                FillResponsGrid("All ", "*");

                            RelationDesc = drRelation.Desc.ToString() + " ";
                            RelationCode = drRelation.Code.ToString();
                            FillResponsGrid(RelationDesc, RelationCode);
                            i++;
                        }
                    }
                }
                //boolChangeStatus = true;
            }
        }

        private void gvResponse_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (RespType == "B" || RespType == "T")
            {
                if (gvResponse != null)
                {
                    if (gvResponse.IsCurrentCellInEditMode)
                    {
                        string FromDt = Convert.ToString(gvResponse.CurrentRow.Cells["FromDt"].Value);
                        string Todate = Convert.ToString(gvResponse.CurrentRow.Cells["ToDt"].Value);

                        if (FromDt != "")
                        {
                            if (Todate != "")
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(Todate, Consts.StaticVars.DateFormatMMDDYYYY))
                                {
                                    try
                                    {
                                        if (DateTime.Parse(Todate) < Convert.ToDateTime("01/01/1800"))
                                        {
                                            MessageBox.Show("01/01/1800 below date not except", "CAPTAIN");
                                            //gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                            //boolcellstatus = false; IsValid = false;
                                        }
                                        else
                                        {
                                            // gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                            MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, "CAPTAIN");
                                            //boolcellstatus = false; IsValid = false;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                        MessageBox.Show("Please Enter Correct Values", "CAPTAIN");
                                        // boolcellstatus = false; IsValid = false;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDateTime(FromDt) >= Convert.ToDateTime(Todate))
                                    {
                                        MessageBox.Show("From date greater than To date", "CAPTAIN");
                                        boolChangeStatus = false;
                                        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                        gvResponse.CurrentRow.Cells["FromDt"].Value = "";
                                        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);

                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (RespType == "N" || RespType == "&" || RespType == "$")
            {
                if (gvResponse != null)
                {
                    if (gvResponse.IsCurrentCellInEditMode)
                    {
                        string FromValue = Convert.ToString(gvResponse.CurrentRow.Cells["FromDt"].Value);
                        string ToValue = Convert.ToString(gvResponse.CurrentRow.Cells["ToDt"].Value);

                        if (FromValue != "")
                        {
                            if (ToValue != "")
                            {
                                if (Convert.ToDecimal(FromValue) >= Convert.ToDecimal(ToValue))
                                {
                                    MessageBox.Show("From value should not be > to value", "CAPTAIN");
                                    boolChangeStatus = false;
                                    this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                    gvResponse.CurrentRow.Cells["ToDt"].Value = "";
                                    this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                }
                            }
                        }
                    }
                }
            }
            if (RespType == "G" || RespType == "H")
            {
                if (gvResponse != null)
                {
                    if (gvResponse.IsCurrentCellInEditMode)
                    {
                        string FromValue = Convert.ToString(gvResponse.CurrentRow.Cells["FromDt"].Value);
                        string ToValue = Convert.ToString(gvResponse.CurrentRow.Cells["ToDt"].Value);
                        string FromMonths = Convert.ToString(gvResponse.CurrentRow.Cells["Frm_Months"].Value);
                        string ToMonths = Convert.ToString(gvResponse.CurrentRow.Cells["To_Months"].Value);
                        if (FromValue != "" && FromMonths != "")
                        {
                            if (ToValue != "" && ToMonths != "")
                            {
                                if (Convert.ToInt32(FromValue) >= Convert.ToInt32(ToValue))
                                {
                                    if (Convert.ToInt32(FromValue) == Convert.ToInt32(ToValue))
                                    {
                                        if (Convert.ToInt32(FromMonths) >= Convert.ToInt32(ToMonths))
                                        {
                                            MessageBox.Show("To Value less than the From Value","CAPTAIN");
                                            boolChangeStatus = false;
                                            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                            gvResponse.CurrentRow.Cells["To_Months"].Value = "";
                                            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("To Value less than the From Value", "CAPTAIN");
                                        boolChangeStatus = false;
                                        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                        gvResponse.CurrentRow.Cells["ToDt"].Value = "";
                                        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void gvResponse_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (RespType == "B" || RespType == "T")
            {
                if (gvResponse.IsCurrentCellInEditMode)
                {
                    string formatedValue = e.FormattedValue as string;

                    if (gvResponse != null)
                    {
                        if (gvResponse.IsCurrentCellInEditMode)
                        {
                            string FromDt = Convert.ToString(gvResponse.CurrentRow.Cells["FromDt"].Value);
                            string Todate = Convert.ToString(gvResponse.CurrentRow.Cells["ToDt"].Value);

                            if (FromDt != "")
                            {
                                if (Todate != "")
                                {
                                    if (!System.Text.RegularExpressions.Regex.IsMatch(Todate, Consts.StaticVars.DateFormatMMDDYYYY))
                                    {
                                        try
                                        {
                                            if (DateTime.Parse(Todate) < Convert.ToDateTime("01/01/1800"))
                                            {
                                                MessageBox.Show("01/01/1800 below date not except", "CAPTAIN");
                                                //gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                                //boolcellstatus = false; IsValid = false;
                                            }
                                            else
                                            {
                                               // gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                                MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, "CAPTAIN");
                                                //boolcellstatus = false; IsValid = false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            //gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                            MessageBox.Show("Please Enter Correct Values", "CAPTAIN");
                                           // boolcellstatus = false; IsValid = false;
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(FromDt) >= Convert.ToDateTime(Todate))
                                        {
                                            MessageBox.Show("To date greater than From date", "CAPTAIN");
                                            boolChangeStatus = false;
                                            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                            gvResponse.CurrentRow.Cells["ToDt"].Value = "";
                                            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                            e.Cancel = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (RespType == "N" || RespType == "&" || RespType == "$")
            {
                if (gvResponse.IsCurrentCellInEditMode)
                {
                    string formattedValue = e.FormattedValue as string;

                    if (gvResponse != null)
                    {
                        if (gvResponse.IsCurrentCellInEditMode)
                        {
                            string FromValue = Convert.ToString(gvResponse.CurrentRow.Cells["FromDt"].Value);
                            string ToValue = Convert.ToString(gvResponse.CurrentRow.Cells["ToDt"].Value);

                            if (FromValue != "")
                            {
                                if (ToValue != "")
                                {
                                    if (Convert.ToDecimal(FromValue) >= Convert.ToDecimal(ToValue))
                                    {
                                        MessageBox.Show("From value should not be > to value", "CAPTAIN");
                                        boolChangeStatus = false;
                                        this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                        gvResponse.CurrentRow.Cells["ToDt"].Value = "";
                                        this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                        e.Cancel = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (RespType == "G" || RespType == "H")
            {
                if (gvResponse.IsCurrentCellInEditMode)
                {
                    string formattedValue = e.FormattedValue as string;
                    if (gvResponse != null)
                    {
                        if (gvResponse.IsCurrentCellInEditMode)
                        {
                            string FromValue = Convert.ToString(gvResponse.CurrentRow.Cells["FromDt"].Value);
                            string ToValue = Convert.ToString(gvResponse.CurrentRow.Cells["ToDt"].Value);
                            string FromMonths = Convert.ToString(gvResponse.CurrentRow.Cells["Frm_Months"].Value);
                            string ToMonths = Convert.ToString(gvResponse.CurrentRow.Cells["To_Months"].Value);
                            if (FromValue != "" && FromMonths != "")
                            {
                                if (ToValue != "" && ToMonths != "")
                                {
                                    if (Convert.ToInt32(FromValue) >= Convert.ToInt32(ToValue))
                                    {
                                        if (Convert.ToInt32(FromValue) == Convert.ToInt32(ToValue))
                                        {
                                            if (Convert.ToInt32(FromMonths) >= Convert.ToInt32(ToMonths))
                                            {
                                                MessageBox.Show("To Value less than the From Value", "CAPTAIN");
                                                boolChangeStatus = false;
                                                this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                                gvResponse.CurrentRow.Cells["To_Months"].Value = "";
                                                this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                                e.Cancel = false;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("To Value less than the From Value", "CAPTAIN");
                                            boolChangeStatus = false;
                                            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                            gvResponse.CurrentRow.Cells["ToDt"].Value = "";
                                            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                                            e.Cancel = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private void gvResponse_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool boolcellstatus = true;
            //string strType = "";
            if (RespType == "B" || RespType == "T")
            {
                if (gvResponse.Rows[e.RowIndex].Cells["Row_Status"].Value.ToString() != "N")
                {

                    int intcolindex = gvResponse.CurrentCell.ColumnIndex;
                    int introwindex = gvResponse.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvResponse.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvResponse.Rows[introwindex].Cells["Points"].Value);
                    string FromDt = Convert.ToString(gvResponse.Rows[introwindex].Cells["FromDt"].Value);
                    string Todate = Convert.ToString(gvResponse.Rows[introwindex].Cells["ToDt"].Value);

                    if (gvResponse.Columns[intcolindex].Name.Equals("FromDt"))
                    {
                        if (strCurrectValue == string.Empty)
                        {
                            boolcellstatus = false;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                        {
                            try
                            {
                                if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                                {
                                    MessageBox.Show("01/01/1800 below date not except", "CAPTAIN");
                                    IsValid = false;
                                    gvResponse.Rows[introwindex].Cells["FromDt"].Value = string.Empty;
                                    boolcellstatus = false;
                                }
                                else
                                {
                                    gvResponse.Rows[introwindex].Cells["FromDt"].Value = string.Empty;
                                    MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, "CAPTAIN");
                                    IsValid = false;
                                    boolcellstatus = false;
                                }
                            }
                            catch (Exception)
                            {
                                gvResponse.Rows[introwindex].Cells["FromDt"].Value = string.Empty;
                                MessageBox.Show("Please Enter Correct Values", "CAPTAIN");
                                IsValid = false;
                                boolcellstatus = false;
                            }
                        }
                    }

                    if (gvResponse.Columns[intcolindex].Name.Equals("ToDt"))
                    {
                        if (FromDt == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequireddateFields, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                        {
                            try
                            {
                                if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                                {
                                    MessageBox.Show("01/01/1800 below date not except", "CAPTAIN");
                                    gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                    boolcellstatus = false; IsValid = false;
                                }
                                else
                                {
                                    gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                    MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, "CAPTAIN");
                                    boolcellstatus = false; IsValid = false;
                                }
                            }
                            catch (Exception)
                            {
                                gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                                MessageBox.Show("Please Enter Correct Values", "CAPTAIN");
                                boolcellstatus = false; IsValid = false;
                            }
                        }
                    }
                    if (gvResponse.Columns[intcolindex].Name.Equals("Points"))
                    {
                        if (string.IsNullOrEmpty(FromDt.Trim()) || string.IsNullOrEmpty(Todate.Trim()))
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show("From & To" + Consts.Messages.RequireddateFields, "CAPTAIN");
                        }
                        else if (Points == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.PNNumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                            gvResponse.Rows[introwindex].Cells["Points"].Value = string.Empty;
                            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);                                         
                            
                        }
                    }

                    if (FromDt != string.Empty && Todate != string.Empty && Points!=string.Empty && Points!="0")
                    {
                        if (boolcellstatus)
                        {
                            boolChangeStatus = true; IsValid = true;
                        }
                    }
                }
            }
            else if (RespType == "N" || RespType == "&" || RespType == "$")
            {
                if (gvResponse.Rows[e.RowIndex].Cells["Row_Status"].Value.ToString() != "N")
                {
                    int intcolindex = gvResponse.CurrentCell.ColumnIndex;
                    int introwindex = gvResponse.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvResponse.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvResponse.Rows[introwindex].Cells["Points"].Value);
                    string FromDt = Convert.ToString(gvResponse.Rows[introwindex].Cells["FromDt"].Value);
                    string Todate = Convert.ToString(gvResponse.Rows[introwindex].Cells["ToDt"].Value);

                    if (gvResponse.Columns[intcolindex].Name.Equals("FromDt"))
                    {
                        if (FromDt == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.PleaseEnterDecimals, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            gvResponse.Rows[introwindex].Cells["FromDt"].Value = string.Empty;
                        }
                    }

                    if (gvResponse.Columns[intcolindex].Name.Equals("ToDt"))
                    {
                        if (FromDt == string.Empty || Todate == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty)
                        {

                            MessageBox.Show(Consts.Messages.PleaseEnterDecimals, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                        }
                    }

                    if (gvResponse.Columns[intcolindex].Name.Equals("Points"))
                    {
                        if (string.IsNullOrEmpty(FromDt.Trim()) || string.IsNullOrEmpty(Todate.Trim()))
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequireddateFields, "CAPTAIN");
                        }
                        else if (Points == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show( "From & To Numeric"+Consts.Messages.RequiredData, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.PNNumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            this.gvResponse.CellValueChanged -= new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);
                            gvResponse.Rows[introwindex].Cells["Points"].Value = string.Empty;
                            this.gvResponse.CellValueChanged += new Wisej.Web.DataGridViewCellEventHandler(this.gvResponse_CellValueChanged);

                        }
                    }

                    if (FromDt != string.Empty && Todate != string.Empty && Points!=string.Empty && Points!="0")
                    {
                        if (boolcellstatus)
                        {
                            boolChangeStatus = true; IsValid = true;
                        }
                    }
                }
            }
            else if (RespType == "G" || RespType == "H")
            {
                if (gvResponse.Rows[e.RowIndex].Cells["Row_Status"].Value.ToString() != "N")
                {
                    int intcolindex = gvResponse.CurrentCell.ColumnIndex;
                    int introwindex = gvResponse.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvResponse.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvResponse.Rows[introwindex].Cells["Points"].Value);
                    string FromDt = Convert.ToString(gvResponse.Rows[introwindex].Cells["FromDt"].Value);
                    string Todate = Convert.ToString(gvResponse.Rows[introwindex].Cells["ToDt"].Value);
                    string FromMonths = Convert.ToString(gvResponse.Rows[introwindex].Cells["Frm_Months"].Value);
                    string ToMonths = Convert.ToString(gvResponse.Rows[introwindex].Cells["To_Months"].Value);

                    if (gvResponse.Columns[intcolindex].Name.Equals("FromDt"))
                    {
                        if (FromDt == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false;
                            gvResponse.Rows[introwindex].Cells["FromDt"].Value = string.Empty;
                        }
                    }

                    if (gvResponse.Columns[intcolindex].Name.Equals("ToDt"))
                    {
                        if (FromDt == string.Empty || Todate == string.Empty || FromMonths == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                        {

                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            gvResponse.Rows[introwindex].Cells["ToDt"].Value = string.Empty;
                        }
                    }

                    if (gvResponse.Columns[intcolindex].Name.Equals("Frm_Months"))
                    {
                        if (FromDt == string.Empty || FromMonths == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            gvResponse.Rows[introwindex].Cells["Frm_Months"].Value = string.Empty;
                        }
                        else if (int.Parse(strCurrectValue) > int.Parse("11"))
                        {
                            MessageBox.Show("enter Correct values in Months", "CAPTAIN");
                            gvResponse.Rows[introwindex].Cells["Frm_Months"].Value = string.Empty;
                            boolcellstatus = false; IsValid = false;
                        }
                    }

                    if (gvResponse.Columns[intcolindex].Name.Equals("To_Months"))
                    {
                        if (FromDt == string.Empty || Todate == string.Empty || FromMonths == string.Empty || ToMonths == string.Empty)
                        {
                            boolcellstatus = false; IsValid = false;
                            MessageBox.Show(Consts.Messages.RequiredbelowFields1, "CAPTAIN");
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.NumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            gvResponse.Rows[introwindex].Cells["To_Months"].Value = string.Empty;
                            //try
                            //{
                            //    if (int.Parse(strCurrectValue) > int.Parse("11"))
                            //    {
                            //        MessageBox.Show("enter Correct values in Months");
                            //        gvResponse.Rows[introwindex].Cells["To_Months"].Value = string.Empty;
                            //        boolcellstatus = false;
                            //    }
                            //    else
                            //    {
                            //        gvResponse.Rows[introwindex].Cells["To_Months"].Value = string.Empty;
                            //        MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
                            //        boolcellstatus = false;
                            //    }
                            //}
                            //catch (Exception)
                            //{
                            //    gvResponse.Rows[introwindex].Cells["To_Months"].Value = string.Empty;
                            //    MessageBox.Show(Consts.Messages.PleaseEnterNumbers);
                            //    boolcellstatus = false;
                            //}
                        }
                        else if (int.Parse(strCurrectValue) > int.Parse("11"))
                        {
                            MessageBox.Show("enter Correct values in Months", "CAPTAIN");
                            gvResponse.Rows[introwindex].Cells["To_Months"].Value = string.Empty;
                            boolcellstatus = false; IsValid = false;
                        }
                    }
                    if (gvResponse.Columns[intcolindex].Name.Equals("Points"))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.PNNumericString) && strCurrectValue != string.Empty)
                        {
                            MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                            boolcellstatus = false; IsValid = false;
                            gvResponse.Rows[introwindex].Cells["Points"].Value = string.Empty;
                        }
                    }

                    if (FromDt != string.Empty && Todate != string.Empty && FromMonths != string.Empty && ToMonths != string.Empty && Points!=string.Empty && Points!="0")
                    {
                        if (boolcellstatus)
                        {
                            boolChangeStatus = true; IsValid = true;
                        }
                        
                    }

                }
            }
            else if (RespType == "D" || RespType == "C" || RespType == "L" || RespType == "R" || RespType == "W" || RespType == "Z" || RespType=="S" || RespType=="Q" || RespType=="X")
            {
                if (gvResponse.Rows[e.RowIndex].Cells["Row_Status"].Value.ToString() != "N")
                {
                    int intcolindex = gvResponse.CurrentCell.ColumnIndex;
                    int introwindex = gvResponse.CurrentCell.RowIndex;

                    string strCurrectValue = Convert.ToString(gvResponse.Rows[introwindex].Cells[intcolindex].Value);
                    string Points = Convert.ToString(gvResponse.Rows[introwindex].Cells["Points"].Value);
                    if (Points != "")
                    {
                        if (gvResponse.Columns[intcolindex].Name.Equals("Points"))
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.PNNumericString) && strCurrectValue != string.Empty)
                            {
                                MessageBox.Show(Consts.Messages.NumericOnly, "CAPTAIN");
                                boolcellstatus = false; IsValid = false;
                                gvResponse.Rows[introwindex].Cells["Points"].Value = string.Empty;
                            }
                        }
                    }

                    if (Points != string.Empty /*&& Points!="0"*/)
                    {
                        if (boolcellstatus)
                        {
                            boolChangeStatus = true; IsValid = true;
                        }
                    }
                }
            }

        }

        private void gvResponse_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (gvResponse.Rows[e.RowIndex].Cells["Row_Status"].Value == null)  //!dataGridView.Rows[e.RowIndex].Cells["Mode"].Value.Equals("U"))
            {
                gvResponse.Rows[e.RowIndex].Cells["Row_Status"].Value = "N";
                gvResponse.Rows[e.RowIndex].Cells["Res_code"].Value = "";
                gvResponse.Rows[e.RowIndex].Cells["Dup_From"].Value = "";
                gvResponse.Rows[e.RowIndex].Cells["Dup_to"].Value = "";
                gvResponse.Rows[e.RowIndex].Cells["Seq"].Value = "";
                gvResponse.Rows[e.RowIndex].Cells["Points"].Value = "";
                gvResponse.Rows[e.RowIndex].Cells["Dup_Points"].Value = " ";

            }
        }


    }
}