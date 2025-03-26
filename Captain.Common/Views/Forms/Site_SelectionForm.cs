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
using DevExpress.XtraRichEdit.Model;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Site_SelectionForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;
        int rowsSel = 0;
        #endregion
        public Site_SelectionForm(BaseForm baseform, string mode, string agency, string dept, string prog, string year, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform; Mode = mode;
            Privileges = privileges;
            Agency = agency; Dept = dept; Prog = prog; Year = year;
            gvsite.Columns[0].Visible = false;
            this.Text = "Site Selection";
            GetAgencyControl();
            
        }
        /// <summary>
        /// Site selection data Room "****" ,Ampm "*"  extra record displayed
        /// </summary>
        /// <param name="baseform"></param>
        /// <param name="mode"></param>
        /// <param name="agency"></param>
        /// <param name="dept"></param>
        /// <param name="prog"></param>
        /// <param name="year"></param>
        /// <param name="privileges"></param>
        /// <param name="Form_Type"></param>
        public Site_SelectionForm(BaseForm baseform, string mode, string agency, string dept, string prog, string year, PrivilegeEntity privileges,string Form_Type)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            FormType = Form_Type;
            BaseForm = baseform; Mode = mode;
            Privileges = privileges;
            Agency = agency; Dept = dept; Prog = prog; Year = year;
            this.Text = "Site Selection";
            gvsite.Columns[0].Visible = false;
            GetAgencyControl();

        }


        public Site_SelectionForm(BaseForm baseform, string mode,List<CaseSiteEntity> sel_Sites_List, string Form_Type,string agency, string dept, string prog, string year, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform; Mode = mode;
            Agency = agency; Dept = dept; Prog = prog; Year = year;
            Privileges = privileges;
            FormType = Form_Type;
            Sel_Rooms_List=sel_Sites_List;
            //gvsite.Columns[5].Visible = false;
            //this.Site_Name.Width = 305;
            this.Text = "Select One or Multiple Sites";
            GetAgencyControl();
            Room.ShowInVisibilityMenu = AMPM.ShowInVisibilityMenu = true;
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Code { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Prog { get; set; }

        public string Year { get; set; }

        public string FormType { get; set; }

        public string AGYSiteSec { get; set; }

        
        public PrivilegeEntity Privileges { get; set; }

        public HierarchyEntity hierarchyEntity { get; set; }

        public List<CaseSiteEntity> Sel_Rooms_List { get; set; }
        List<HierarchyEntity> userHierarchy { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;
        //Gizmox.Wstring Img_Blank = webGUI.Common.Resources.ResourceHandle Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        private void GetAgencyControl()
        {
            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                AGYSiteSec = dsAgency.Tables[0].Rows[0]["ACR_SITE_SEC"].ToString().Trim();
            }
            //if (dsAgency.Tables[0].Rows[0]["SITE_ACTIVE"].ToString().Trim() == "N")
            //    gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            if (AGYSiteSec == "1")
            {
                userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                foreach (HierarchyEntity Entity in userHierarchy)
                {
                    if (Entity.InActiveFlag == "N")
                    {
                        if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        {
                            //Entity.Agency = Agency;
                            hierarchyEntity = Entity;
                        }
                    }
                }

            }

        }


        int row_Cnt = 0;
        private void fillGridSite()
        {
            gvsite.Rows.Clear();
            List<CaseSiteEntity> sitelist = new List<CaseSiteEntity>();
            CaseSiteEntity Search_Site = new CaseSiteEntity(true);
            Search_Site.SiteAGENCY = Agency.Trim();
            Search_Site.SiteDEPT = Dept.Trim();
            Search_Site.SitePROG = Prog.Trim();
            if(Year!="All")
                Search_Site.SiteYEAR = Year.Trim();
            sitelist = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");

            if (AGYSiteSec == "1")
            {
                List<CaseSiteEntity> ListcaseSiteEntity = new List<CaseSiteEntity>();

                if (hierarchyEntity.Sites.Length > 0)
                {
                    string[] Sites = hierarchyEntity.Sites.Split(',');

                    for (int i = 0; i < Sites.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                        {
                            foreach (CaseSiteEntity casesite in sitelist) //Site_List)//ListcaseSiteEntity)
                            {
                                if (Sites[i].ToString() == casesite.SiteNUMBER)
                                {
                                    ListcaseSiteEntity.Add(casesite);
                                    //break;
                                }
                                // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                            }
                        }
                    }
                }
                //else if(hierarchyEntity.Agency+hierarchyEntity.Dept+hierarchyEntity.Prog!="******") sitelist = ListcaseSiteEntity;

                if (ListcaseSiteEntity.Count > 0) sitelist = ListcaseSiteEntity;
            }

            //int row_Cnt = 0;
            if (FormType == "Report")
            {
                if (sitelist.Count > 0)
                {
                    sitelist = sitelist.OrderByDescending(u => u.SiteACTIVE).ToList();
                    
                    if (Mode == "Room")
                    {
                        this.Site_Name.Width = 210;
                        this.City.Width = 100;
                        string Active = "N"; string PrivSite = string.Empty;
                        foreach (CaseSiteEntity Entity in sitelist)
                        {
                            bool Sel_Ref = false;
                            int rowIndex = 0;
                            if (Entity.SiteNUMBER.Trim() != PrivSite)
                                Active = Entity.SiteACTIVE.Trim();
                            if (Entity.SiteROOM != "0000")
                            {
                                foreach (CaseSiteEntity SEntity in Sel_Rooms_List)
                                {
                                    if (SEntity.SiteNUMBER.Trim() == Entity.SiteNUMBER.Trim() && SEntity.SiteROOM.Trim()==Entity.SiteROOM.Trim() && SEntity.SiteAM_PM.Trim()==Entity.SiteAM_PM.Trim())
                                    {
                                        Sel_Ref = true;
                                        rowIndex = gvsite.Rows.Add(Img_Tick, Entity.SiteNUMBER.Trim(), Entity.SiteROOM.Trim(), Entity.SiteAM_PM.Trim(), Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "Y");
                                        rowsSel = rowIndex;
                                        if (Active=="N")
                                            gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                        gvsite.Rows[rowIndex].Tag = SEntity;
                                    }
                                }

                                if (!Sel_Ref)
                                {
                                    rowIndex = gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), Entity.SiteROOM.Trim(), Entity.SiteAM_PM.Trim(), Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "N");
                                    if (Active == "N")
                                        gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    gvsite.Rows[rowIndex].Tag = Entity;
                                }
                                row_Cnt++;
                            }

                            PrivSite = Entity.SiteNUMBER.Trim();
                        }
                    }
                    else
                    {
                        this.AMPM.Visible = false;
                        this.Room.Visible = false;
                        this.Site_Name.Width = 270;
                        this.City.Width = 170;
                        foreach (CaseSiteEntity Entity in sitelist)
                        {
                            bool Sel_Ref = false;
                            int rowIndex = 0;
                            if (Entity.SiteROOM == "0000")
                            {
                                foreach (CaseSiteEntity SEntity in Sel_Rooms_List)
                                {
                                        if (SEntity.SiteNUMBER.Trim() == Entity.SiteNUMBER.Trim())
                                    {
                                        Sel_Ref = true;
                                        rowIndex = gvsite.Rows.Add(Img_Tick, Entity.SiteNUMBER.Trim(), " ", " ", Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "Y");
                                        rowsSel = rowIndex;
                                        if (Entity.SiteACTIVE == "N")
                                            gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                        gvsite.Rows[rowIndex].Tag = SEntity;
                                    }
                                }

                                if (!Sel_Ref)
                                {
                                    rowIndex = gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), " ", " ", Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "N");
                                    if (Entity.SiteACTIVE == "N")
                                        gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    gvsite.Rows[rowIndex].Tag = Entity;
                                }
                                row_Cnt++;
                            }
                        }
                    }
                }
            }
            else if (FormType == "****")
            {

                if (sitelist.Count > 0)
                {
                    if (Mode == "Room")
                    {
                        this.Site_Name.Width = 220;
                        this.City.Width = 115;
                        string strSiteNumber = string.Empty;
                        string Active = "N"; string PrivSite = string.Empty;
                        sitelist = sitelist.OrderByDescending(u => u.SiteACTIVE).ToList();
                        foreach (CaseSiteEntity Entity in sitelist)
                        {

                            if (Entity.SiteNUMBER.Trim() != PrivSite)
                                Active = Entity.SiteACTIVE.Trim();

                            if ((Active == "Y" && (Privileges.Program == "CASE0010" || Privileges.Program == "ENRLHIST")) ||
                                 (Privileges.Program != "CASE0010" && Privileges.Program != "ENRLHIST"))
                            {
                                if (Entity.SiteROOM != "0000")
                                {
                                    if (strSiteNumber != Entity.SiteNUMBER.Trim())
                                    {
                                        gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), "****", "*", Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "", Entity.SiteACTIVE, Entity.SiteACTIVE.Equals("Y") ? Color.Black : Color.Red);
                                        strSiteNumber = Entity.SiteNUMBER.Trim();
                                        row_Cnt++;
                                    }
                                    gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), Entity.SiteROOM.Trim(), Entity.SiteAM_PM.Trim(), Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "", Entity.SiteACTIVE, Entity.SiteACTIVE.Equals("Y") ? Color.Black : Color.Red);
                                    row_Cnt++;
                                }
                            }
                            PrivSite = Entity.SiteNUMBER.Trim();
                        }
                    }
                    else
                    {
                        this.AMPM.Visible = false;
                        this.Room.Visible = false;
                        this.Site_Name.Width = 270;
                        this.City.Width = 170;
                        sitelist = sitelist.OrderByDescending(u => u.SiteACTIVE).ToList();
                        foreach (CaseSiteEntity Entity in sitelist)
                        {
                            if (Entity.SiteROOM == "0000")
                            {
                                gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), " ", " ", Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "",Entity.SiteACTIVE, Entity.SiteACTIVE.Equals("Y") ? Color.Black : Color.Red);
                                row_Cnt++;
                            }
                        }
                    }
                }
            }
            else
            {
                if (sitelist.Count > 0)
                {
                    if (Mode == "Room")
                    {
                        string Active = "N"; string PrivSite = string.Empty; int rowIndex = 0;
                        this.Site_Name.Width = 220;
                        this.City.Width = 115;

                        if(Year=="All")
                        {
                            sitelist = sitelist.OrderBy(u => u.SiteNUMBER).ThenBy(u => u.SiteROOM).ThenBy(u => AMPM).ToList();

                            //List<CaseSiteEntity> SiteRooms = new List<CaseSiteEntity>();

                            //SiteRooms = sitelist.GroupBy(d => new { d.SiteNUMBER, d.SiteROOM, d.SiteAM_PM }).Select(m => new { m.Key.SiteNUMBER, m.Key.SiteROOM, m.Key.SiteAM_PM }).ToList();

                            List<CaseSiteEntity> distinctProductList = sitelist.GroupBy(d => new { d.SiteNUMBER, d.SiteROOM, d.SiteAM_PM }).Select(group => group.First()).ToList();
                            foreach (CaseSiteEntity Entity in distinctProductList)
                            {
                                if (Entity.SiteNUMBER.Trim() != PrivSite)
                                    Active = Entity.SiteACTIVE.Trim();
                                if ((Active == "Y" && (Privileges.Program == "CASE0010" || Privileges.Program == "ENRLHIST")) ||
                                     (Privileges.Program != "CASE0010" && Privileges.Program != "ENRLHIST"))
                                {
                                    if (Entity.SiteROOM != "0000")
                                    {
                                        rowIndex = gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), Entity.SiteROOM.Trim(), Entity.SiteAM_PM.Trim(), Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "");
                                        if (Active == "N")
                                            gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                        row_Cnt++;
                                    }
                                }
                                PrivSite = Entity.SiteNUMBER.Trim();
                            }

                        }
                        else
                        {
                            foreach (CaseSiteEntity Entity in sitelist)
                            {
                                if (Entity.SiteNUMBER.Trim() != PrivSite)
                                    Active = Entity.SiteACTIVE.Trim();
                                if ((Active == "Y" && (Privileges.Program == "CASE0010" || Privileges.Program == "ENRLHIST")) ||
                                     (Privileges.Program != "CASE0010" && Privileges.Program != "ENRLHIST"))
                                {
                                    if (Entity.SiteROOM != "0000")
                                    {
                                        rowIndex = gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), Entity.SiteROOM.Trim(), Entity.SiteAM_PM.Trim(), Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "");
                                        if (Active == "N")
                                            gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                        row_Cnt++;
                                    }
                                }
                                PrivSite = Entity.SiteNUMBER.Trim();
                            }
                        }
                        
                    }
                    else
                    {
                        this.AMPM.Visible = false;
                        this.Room.Visible = false;
                        this.Site_Name.Width = 270;
                        this.City.Width = 170; int rowIndex = 0;

                        if (Year == "All")
                        {
                            sitelist = sitelist.OrderBy(u => u.SiteNUMBER).ThenBy(u => u.SiteROOM).ThenBy(u => AMPM).ToList();

                            //List<CaseSiteEntity> SiteRooms = new List<CaseSiteEntity>();

                            //SiteRooms = sitelist.GroupBy(d => new { d.SiteNUMBER, d.SiteROOM, d.SiteAM_PM }).Select(m => new { m.Key.SiteNUMBER, m.Key.SiteROOM, m.Key.SiteAM_PM }).ToList();

                            List<CaseSiteEntity> distinctSiteList = sitelist.GroupBy(d => new { d.SiteNUMBER }).Select(group => group.First()).ToList();

                            foreach (CaseSiteEntity Entity in distinctSiteList)
                            {
                                if (Entity.SiteROOM == "0000")
                                {
                                    rowIndex = gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), " ", " ", Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "");
                                    if (Entity.SiteACTIVE == "N")
                                        gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    row_Cnt++;
                                }
                            }

                        }
                        else
                        {
                            foreach (CaseSiteEntity Entity in sitelist)
                            {
                                if (Entity.SiteROOM == "0000")
                                {
                                    rowIndex = gvsite.Rows.Add(Img_Blank, Entity.SiteNUMBER.Trim(), " ", " ", Entity.SiteNAME.Trim(), Entity.SiteCITY.Trim(), "");
                                    if (Entity.SiteACTIVE == "N")
                                        gvsite.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    row_Cnt++;
                                }
                            }
                        }
                    }
                } 
            }
            //if(row_Cnt==0)
            //{
            //    MessageBox.Show("No Site is Available","CAP Systems");
            //    btnSelect.Visible = false;
            //}
            if (gvsite.Rows.Count > 0)
            { gvsite.Rows[rowsSel].Selected = true;
                btnSelect.Visible = true;
            }
            else
            {
                btnSelect.Visible = false;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string[] GetSelected_Site_Row()
        {
            string[] Added_Edited_SiteCode = new string[3];

            Added_Edited_SiteCode[0] = gvsite.CurrentRow.Cells["Site_No"].Value.ToString().Trim();
            Added_Edited_SiteCode[1] = gvsite.CurrentRow.Cells["Room"].Value.ToString().Trim();
            Added_Edited_SiteCode[2] = gvsite.CurrentRow.Cells["AMPM"].Value.ToString().Trim();
            //Added_Edited_SiteCode[2] = ;

            return Added_Edited_SiteCode;
        }

        public List<CaseSiteEntity> GetSelected_Sites()
        {
            List<CaseSiteEntity> sele_Rooms_List = new List<CaseSiteEntity>();
            //CaseSiteEntity Add_Entity = new CaseSiteEntity();
            foreach (DataGridViewRow dr in gvsite.Rows)
            {
                if (dr.Cells["Selected"].Value.ToString() == "Y")
                {
                    //Add_Entity.SiteNUMBER = dr.Cells["Site_No"].Value.ToString();
                    //Add_Entity.SiteROOM = dr.Cells["Room"].Value.ToString();
                    //Add_Entity.SiteAM_PM = dr.Cells["AMPM"].Value.ToString();
                    //Add_Entity.SiteNAME = dr.Cells["Site_Name"].Value.ToString();
                    CaseSiteEntity Add_Entity = dr.Tag as CaseSiteEntity;
                    sele_Rooms_List.Add(Add_Entity);
                }
            }
            return sele_Rooms_List;
        }

        private void Site_SelectionForm_Load(object sender, EventArgs e)
        {
            if (FormType == "Report")
            {
                fillGridSite();
                if (row_Cnt == 0)
                {
                    AlertBox.Show("No Site is available.", MessageBoxIcon.Warning);
                    btnSelect.Visible = false;
                }
            }
            else
            {
                fillGridSite();
                if (row_Cnt == 0)
                {
                    AlertBox.Show("No Site is available.", MessageBoxIcon.Warning);
                    btnSelect.Visible = false;
                }
            }
        }

        private void gvsite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvsite.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gvsite.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            gvsite.CurrentRow.Cells["Sel_Img"].Value = Img_Blank;
                            gvsite.CurrentRow.Cells["Selected"].Value = "N";
                        }
                        else
                        {
                            gvsite.CurrentRow.Cells["Sel_Img"].Value = Img_Tick;
                            gvsite.CurrentRow.Cells["Selected"].Value = "Y";
                        }
                    }
                }
            }
        }

        private void gvsite_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}