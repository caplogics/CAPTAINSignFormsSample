#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;

using Wisej.Web;
using Wisej.Design;
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
using System.Text.RegularExpressions;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RankDefinitions : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        //string Img_Add = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("AddItem.gif");
        //string Img_Edit = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("EditIcon.gif");

        string Img_Add = "captain-add";//"Resources/Icons/16X16/AddItem.gif";
        string Img_Edit = "captain-edit";//"Resources/Icons/16X16/EditIcon.gif";

        public RankDefinitions(BaseForm Baseform,string Rank_code,string Rank_Desc,string Agy,string AgyDesc, PrivilegeEntity Priviliges)
        {
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            
            _model = new CaptainModel();

            baseform = Baseform;
            priviliges = Priviliges;
            RankCode = Rank_code;
            rankDesc = Rank_Desc;
            Agency = Agy; AgencyDesc = AgyDesc;
            this.Text = "Rank Definitions";//priviliges.Program + " - Rank Definitions";
            txtHie.Text = AgencyDesc;
            //this.gvHie.Location = new System.Drawing.Point(0, 120);
            //this.gvHie.Size = new System.Drawing.Size(195, 355);
            //this.gvFields.Location = new System.Drawing.Point(200, 120);
            //this.gvFields.Size = new System.Drawing.Size(510, 355);
            //this.lblScreen.Location = new System.Drawing.Point(21, 97);
            //this.cmbScreen.Location = new System.Drawing.Point(134, 95);
            txtRankCat.Text = rankDesc.ToString();
            FillComboScreen();
            //fillHieGrid();
            //if (Hier == null)
            //    btnAdd_Click(btnAdd, EventArgs.Empty);
            //else
            //    FillComboScreen();
            fillFielsGrid();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            
        }
        string Mode = "Edit";

        #region properties

        public BaseForm baseform { get; set; }

        public PrivilegeEntity priviliges { get; set; }

        public string rankDesc { get; set; }

        public string propReportPath { get; set; }

        public string RankCode { get; set; }

        public string SubCode { get; set; }

        public string Agency { get; set; }

        public string AgencyDesc { get; set; }

        public bool IsSaveValid { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                TmpGrid.Rows.Clear();
                return _selectedHierarchies = (from c in TmpGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }

        #endregion

        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Ranking Categories Form");
        }

        private void Set_EditControls()
        {
            //this.gvHie.Location = new System.Drawing.Point(0, 109);
            //this.gvHie.Size = new System.Drawing.Size(270, 358);
            //this.gvFields.Location = new System.Drawing.Point(276, 109);
            //this.gvFields.Size = new System.Drawing.Size(425, 358);
            //this.gvHie.Location = new System.Drawing.Point(0, 120);
            //this.gvHie.Size = new System.Drawing.Size(195, 355);
            //this.gvFields.Location = new System.Drawing.Point(200, 120);
            //this.gvFields.Size = new System.Drawing.Size(510, 355);
            //this.FldName.Width = 380;
            this.FldName.Width = 520;
            //btnAdd.Visible = true;
            //lblScreen.Visible = false;
            //label1.Visible = false;
            //lblHie.Visible = false;
            //txtHie.Visible = false;
            //this.lblScreen.Location = new System.Drawing.Point(21, 97);
            //this.cmbScreen.Location = new System.Drawing.Point(134, 95);
            //cmbScreen.Visible = true;
            //PbHierarchies.Visible = false;
            //gvHie.Visible = true;
        }

        private void fillHieGrid()
        {
            //gvHie.Rows.Clear();
            //int rowIndex = 0; int rowCnt = 0;

            //List<RNKCRIT1Entity> RankHieList;
            //RankHieList = _model.SPAdminData.GetRNKCRIT(RankCode);

            //if (RankHieList.Count > 0)
            //{
            //    foreach (RNKCRIT1Entity drRankHie in RankHieList)
            //    {
            //        string Hierarchy = drRankHie.Agency.ToString() + "-"+drRankHie.Dept.ToString() + "-"+drRankHie.Prog.ToString();
            //        rowIndex =gvHie.Rows.Add(Hierarchy);

            //        //string toolTipText = "Added By     : " + drRankHie.Addoperator.ToString().Trim() + " on " + drRankHie.Dateadd.ToString() + "\n" +
            //        //             "Modified By  : " + drRankHie.LstcOperator.ToString().Trim() + " on " + drRankHie.dateLstc.ToString();

            //        //foreach (DataGridViewCell cell in gvHie.Rows[rowIndex].Cells)
            //        //{
            //        //    cell.ToolTipText = toolTipText;
            //        //}
            //        rowCnt++;
            //        gvHie.Rows[rowIndex].Tag = RankHieList;
            //    }
            //}

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Mode = "Add";
            //btnAdd.Visible = false;
            //lblScreen.Visible = true;
            //lblHie.Visible = true;
            //txtHie.Visible = true;
            //cmbScreen.Visible = true;
            //PbHierarchies.Visible = true;
            ////label1.Visible = false;
            //gvHie.Visible = false; txtHie.Clear();
            //this.gvFields.Location = new System.Drawing.Point(0, 150);
            //this.gvFields.Size = new System.Drawing.Size(701, 316);
            //this.lblScreen.Location = new System.Drawing.Point(21, 121);
            //this.cmbScreen.Location = new System.Drawing.Point(134, 118);
            this.FldName.Width = 520;
            //HierarchyList = null;
            FillComboScreen();
            fillFielsGrid();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if (Mode == "Add")
            //{
            //    btnAdd.Visible = true;
            //    lblScreen.Visible = false;
            //    lblHie.Visible = false;
            //    txtHie.Visible = false;
            //    cmbScreen.Visible = false;
            //    PbHierarchies.Visible = false;
            //    gvHie.Visible = true;
            //    this.gvHie.Location = new System.Drawing.Point(0, 109);
            //    this.gvHie.Size = new System.Drawing.Size(270, 358);
            //    this.gvFields.Location = new System.Drawing.Point(276, 109);
            //    this.gvFields.Size = new System.Drawing.Size(425, 358);
            //    this.FldName.Width = 300;
            //    Mode = "Edit";
            //}
            //else
            //    this.Close();
        }

        private void FillComboScreen()
        {
            DataSet dsscr = DatabaseLayer.SPAdminDB.BrowseFldScrs("RANKCATG");
            DataTable dtscr = dsscr.Tables[0];
            this.cmbScreen.SelectedIndexChanged -= new System.EventHandler(this.cmbScreen_SelectedIndexChanged);
            cmbScreen.Items.Clear();

            List<RepListItem> list = new List<RepListItem>();

            foreach (DataRow drscr in dtscr.Rows)
            {
                cmbScreen.Items.Add(new RepListItem(drscr["FLDSCRS_DESC"].ToString(), drscr["FLDSCRS_CODE"].ToString(),drscr["FLDSCRS_CUST"].ToString()));
            }
            this.cmbScreen.SelectedIndexChanged += new System.EventHandler(this.cmbScreen_SelectedIndexChanged);
            if (dtscr.Rows.Count > 0)
                cmbScreen.SelectedIndex = 0;
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {

            ////HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(baseform, "Master", Current_Hierarchy_DB, string.Empty, "Rank_Def");
            ////hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            ////hierarchieSelectionForm.ShowDialog();

            //HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(baseform, string.Empty, "Master", string.Empty, "A", string.Empty);
            //addForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            //addForm.ShowDialog();

            ////HierarchieSelectionForm addForm = new HierarchieSelectionForm(baseform, "Master", string.Empty, "Hie_multiple_sel");
            ////addForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            ////addForm.ShowDialog();

        }
        string HierarchyList = null; string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            //HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            //string HieCd = null;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
            //    string hierarchy = string.Empty;

            //    foreach (HierarchyEntity row in selectedHierarchies)
            //    {
            //        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
            //        HieCd = row.Code;
            //    }
            //    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);
            //    //if (!string.IsNullOrEmpty(HierarchyList))
            //    //{
            //    //    HierarchyList = hierarchy.Substring(0, 2) + hierarchy.Substring(2, 2) + hierarchy.Substring(4, 2);
            //    //}
            //    txtHie.Text = HieCd;
            //}

            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    hierarchy += row.Agency + row.Dept + row.Prog;
                    if (gvHie.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow dr in gvHie.Rows)
                        {
                            if (dr.Cells["Hie"].Value.ToString() == row.Code)
                            {
                                MessageBox.Show("This Hierarchy (" + row.Code + ") is already defined", "CAPSYSTEMS");
                                txtHie.Text = string.Empty;
                            }
                            else
                            {
                                txtHie.Text = row.Code;
                                HierarchyList = txtHie.Text.Substring(0, 2) + txtHie.Text.Substring(3, 2) + txtHie.Text.Substring(6, 2);
                            }
                        }
                    }
                    else
                    {
                        txtHie.Text = row.Code;
                        HierarchyList = txtHie.Text.Substring(0, 2) + txtHie.Text.Substring(3, 2) + txtHie.Text.Substring(6, 2);
                    }
                    
                    
                    //txtHierachydesc.Text = row.HirarchyName;
                    //string strCode = CaseSum.GETCASEELIGGCode(row.Agency + row.Dept + row.Prog);
                    //txtGroupCode.Text = "0000".Substring(0, 4 - strCode.Length) + strCode;
                    //HiearchyCode = row.Agency + row.Dept + row.Prog;
                }
            }


        }


        string ScreenName = null, Hier = null, hierac = null; string ScreenSw = string.Empty;
        private void gvHie_SelectionChanged(object sender, EventArgs e)
        {
            Hier = gvHie.CurrentRow.Cells["Hie"].Value.ToString();
            //fillFielsGrid();
            HierarchyList = Hier.Substring(0, 2) + Hier.Substring(3, 2) + Hier.Substring(6, 2);
            FillComboScreen();
            ScreenName = ((RepListItem)cmbScreen.SelectedItem).Value.ToString();
            ScreenSw = ((RepListItem)cmbScreen.SelectedItem).ID.ToString();
            fillFielsGrid();
            if(Hier==null)
                btnAdd_Click(btnAdd, EventArgs.Empty);
        }

        private void gvFields_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex!=-1)
            {
                //if (string.IsNullOrWhiteSpace(HierarchyList) || string.IsNullOrEmpty(HierarchyList))
                //{
                //    _errorProvider.SetError(txtHie, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHie.Text.Replace(Consts.Common.Colon, string.Empty)));
                    
                //}
                //else
                //{
                //    _errorProvider.SetError(txtHie, null);
                RankCriteriaSelction criteriaform;
                    
                    if (Mode == "Add")
                        criteriaform = new RankCriteriaSelction(baseform, Agency.ToString(), RankCode.ToString(), AddScreen.ToString(), gvFields.CurrentRow.Cells["FldName"].Value.ToString(), gvFields.CurrentRow.Cells["Fld_Code"].Value.ToString(), "1", gvFields.CurrentRow.Cells["Agy_cd"].Value.ToString(), Mode, gvFields.CurrentRow.Cells["Resp_Type"].Value.ToString(), gvFields.CurrentRow.Cells["File_Name"].Value.ToString(), priviliges, this,ScreenSw);
                    else
                        criteriaform = new RankCriteriaSelction(baseform, Agency.ToString(), RankCode.ToString(), ScreenName.ToString(), gvFields.CurrentRow.Cells["FldName"].Value.ToString(), gvFields.CurrentRow.Cells["Fld_Code"].Value.ToString(), "1", gvFields.CurrentRow.Cells["Agy_cd"].Value.ToString(), Mode, gvFields.CurrentRow.Cells["Resp_Type"].Value.ToString(), gvFields.CurrentRow.Cells["File_Name"].Value.ToString(), priviliges, this,ScreenSw);
                        criteriaform.FormClosed += new FormClosedEventHandler(OncriteriaformClosed);
                        criteriaform.StartPosition = FormStartPosition.CenterScreen;
                        criteriaform.ShowDialog();
                //}
            }
        }

        private void OncriteriaformClosed(object sender, FormClosedEventArgs e)
        {
            string DeleteFunc = null;
            RankCriteriaSelction form = sender as RankCriteriaSelction;
            //TagClass selectedTabTagClass = baseform.ContentTabs.SelectedItem.Tag as TagClass;
            if (form.DialogResult == DialogResult.OK)
            {
                DeleteFunc = form.get_DelFlag();
                if (DeleteFunc == "Y")
                    btnAdd_Click(btnAdd, EventArgs.Empty);
                else
                {
                    Mode = "Edit";
                    Set_EditControls();
                    //fillHieGrid();
                    fillFielsGrid();
                }
            }
        }

        //Sudheer

        string Match_id = null;
        private void fillFielsGrid()
        {
            List<RNKCRIT1Entity1> RankCritlist;
            int rowIndex = 0; int rowCnt = 0;
            gvFields.Rows.Clear();
            DataSet dsQues = DatabaseLayer.SPAdminDB.BrowseCustFlds();
            if (ScreenName == "CUSTQUES")
            {
                DataTable dtQues = dsQues.Tables[0];
                DataView dv = new DataView(dtQues);
                dv.RowFilter = "CUST_SCR_CODE='CASE2001' AND CUST_ACTIVE='A'";
                dv.Sort = "CUST_DESC";
                dtQues = dv.ToTable();
                string Desc = null, Code = null, Agy_code = null, Resp_Type = null; 
                //string File_Name = null;

                foreach (DataRow drQues in dtQues.Rows)
                {
                        Desc = drQues["CUST_DESC"].ToString();
                        Code = drQues["CUST_CODE"].ToString();
                        Resp_Type = drQues["CUST_RESP_TYPE"].ToString();
                        if (Mode == "Add")
                        {
                            rowIndex = gvFields.Rows.Add(Desc, Code, Img_Add, " ", Resp_Type, " ");
                        }
                        else
                        {
                            RankCritlist = _model.SPAdminData.Browse_RankCritiria1(Agency, RankCode, "CUSTQUES", null);
                            bool Code_Exists = false;
                                foreach (RNKCRIT1Entity1 list in RankCritlist)
                                {
                                    Code_Exists = false; ;
                                    if (drQues["CUST_CODE"].ToString() == list.Fld_cd.ToString())
                                    {
                                        Match_id = list.Id.ToString();
                                        Code_Exists = true; break;
                                    }
                                }
                                if (Code_Exists)
                                    rowIndex = gvFields.Rows.Add(Desc, Code, Img_Edit, " ", Resp_Type, "  ");
                                else
                                    rowIndex = gvFields.Rows.Add(Desc, Code, Img_Add, " ", Resp_Type, " ");
                                
                            
                        }
                    }
            }
            else if (ScreenName == "PREASSES" && ScreenSw=="Y")
            {
                DataTable dtQues = dsQues.Tables[0];
                DataView dv = new DataView(dtQues);
                dv.RowFilter = "CUST_SCR_CODE='PREASSES' AND CUST_ACTIVE='A'";
                dv.Sort = "CUST_CODE";
                dtQues = dv.ToTable();
                string Desc = null, Code = null, Agy_code = null, Resp_Type = null;

                foreach (DataRow drQues in dtQues.Rows)
                {
                    Desc = drQues["CUST_DESC"].ToString();
                    Code = drQues["CUST_CODE"].ToString();
                    Resp_Type = drQues["CUST_RESP_TYPE"].ToString();
                    if (Mode == "Add")
                    {
                        rowIndex = gvFields.Rows.Add(Desc, Code, Img_Add, " ", Resp_Type, " ");
                    }
                    else
                    {
                        RankCritlist = _model.SPAdminData.Browse_RankCritiria1(Agency, RankCode, "PREASSES", null);
                        bool Code_Exists = false;
                        foreach (RNKCRIT1Entity1 list in RankCritlist)
                        {
                            Code_Exists = false; ;
                            if (drQues["CUST_CODE"].ToString() == list.Fld_cd.ToString())
                            {
                                Match_id = list.Id.ToString();
                                Code_Exists = true; break;
                            }
                        }
                        if (Code_Exists)
                            rowIndex = gvFields.Rows.Add(Desc, Code, Img_Edit, " ", Resp_Type, "  ");
                        else
                            rowIndex = gvFields.Rows.Add(Desc, Code, Img_Add, " ", Resp_Type, " ");

                    }
                }

            }
            else
            {
                DataSet dsFields = DatabaseLayer.SPAdminDB.BrowseRankQues(ScreenName);
                DataTable dtFields = dsFields.Tables[0];
                string Desc = null, Code = null, Agy_code = null, Resp_Type = null; ;
                string File_Name = null;
                foreach (DataRow drFields in dtFields.Rows)
                {
                    if (drFields["RANKQUES_SCR"].ToString() == ScreenName.ToString())
                    {
                        Desc = drFields["RANKQUES_DESC"].ToString();
                        Code = drFields["RANKQUES_CODE"].ToString();
                        Agy_code = drFields["RANKQUES_AGY_CODE"].ToString();
                        Resp_Type = drFields["RANKQUES_RESP_TYPE"].ToString();
                        File_Name = drFields["RANKQUES_FILENAME"].ToString();
                        if (Mode == "Add")
                        {
                            rowIndex = gvFields.Rows.Add(Desc, Code, Img_Add, Agy_code, Resp_Type, File_Name);
                        }
                        else
                        {
                            RankCritlist = _model.SPAdminData.Browse_RankCritiria1(Agency, RankCode, drFields["RANKQUES_SCR"].ToString(), null);
                            bool Code_Exists = false;
                            foreach (RNKCRIT1Entity1 list in RankCritlist)
                            {
                                Code_Exists = false; ;
                                if (drFields["RANKQUES_CODE"].ToString() == list.Fld_cd.ToString())
                                {
                                    Match_id = list.Id.ToString();
                                    Code_Exists = true; break;
                                }
                            }
                            if (Code_Exists)
                                rowIndex = gvFields.Rows.Add(Desc, Code, Img_Edit, Agy_code, Resp_Type, File_Name);
                            else
                                rowIndex = gvFields.Rows.Add(Desc, Code, Img_Add, Agy_code, Resp_Type, File_Name);



                        }

                    }
                }
            }
        }

        public void RefreshFieldsGrid()
        {
            fillFielsGrid();
            if (gvFields.Rows.Count != 0)
            {
                gvFields.CurrentCell = gvFields.Rows[strIndex].Cells[0];
                //gvFields.CurrentPage = strPageIndex;
            }
        }

        public void RefreshHieGrid()
        {
            fillHieGrid();
            if (gvHie.Rows.Count != 0)
            {
                gvHie.CurrentCell = gvHie.Rows[strIndex].Cells[0];
                //gvHie.CurrentPage = strPageIndex;
            }
        }
        string AddScreen = null;
        private void cmbScreen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (((RepListItem)cmbScreen.SelectedItem).Value.ToString() == "CUSTQUES")
            //{
            ScreenName = ((RepListItem)cmbScreen.SelectedItem).Value.ToString(); //"CUSTQUES";
            ScreenSw = ((RepListItem)cmbScreen.SelectedItem).ID.ToString();
                fillFielsGrid();
            //}
            //else if (((RepListItem)cmbScreen.SelectedItem).Value.ToString() == "CASE2001")
            //{
            //    ScreenName = "CASE2001";
            //    fillFielsGrid();
            //}
            //else if (((RepListItem)cmbScreen.SelectedItem).Value.ToString() == "CASE2330")
            //{
            //    ScreenName = "CASE2330";
            //    fillFielsGrid();
            //}
            //else if (((RepListItem)cmbScreen.SelectedItem).Value.ToString() == "PREASSES")
            //{
            //    ScreenName = "PREASSES";
            //    fillFielsGrid();
            //}
            AddScreen = ((RepListItem)cmbScreen.SelectedItem).Value.ToString();
            ScreenSw = ((RepListItem)cmbScreen.SelectedItem).ID.ToString();
        }

        // Begin Report Section.............

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        int pageNumber; string PdfName = "Pdf File";
        string PdfScreen = null;
        string ProgName = null;
        BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        private void On_SaveForm_Closed()//object sender, FormClosedEventArgs e)
        {
            //PdfListForm form = sender as PdfListForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
                Random_Filename = null;
                
                PdfName = "RankDefWithAgency";//form.GetFileName();
                PdfName = propReportPath + PdfName;
            pageNumber = 1;
                try
                {
                    string Tmpstr = PdfName + ".pdf";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".pdf";

                FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document();
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;
                try
                {
                    cb.BeginText();
                    printHText();
                    X_Pos = 60; Y_Pos = 455;
                    SetLine();
                    Y_Pos -= 14;
                    cb.BeginText();
                    CheckBottomBorderReached(document, writer);
                    HeadingPage();
                    cb.EndText();
                    cb.BeginText();
                    Y_Pos -= 5;
                    CheckBottomBorderReached(document, writer);
                    SetLine();
                    
                    List<RNKCRIT1Entity1> RankCritlist; List<RNKCRIT2Entity> RnkCrit2list;
                    RankCritlist = _model.SPAdminData.Browse_RankCritiria1(Agency, RankCode, ((RepListItem)cmbScreen.SelectedItem).Value.ToString(), null);
                    DataSet dsRnkQuee = DatabaseLayer.SPAdminDB.BrowseRankQues(null);
                    DataTable dtRnkQuee = dsRnkQuee.Tables[0];
                    DataSet dsCustFlds=DatabaseLayer.SPAdminDB.BrowseCustFlds();
                    DataTable dtCustflds=dsCustFlds.Tables[0];

                    X_Pos = 60; Y_Pos = 420; string Priv_Scr = null; bool First = true;
                    foreach (RNKCRIT1Entity1 list in RankCritlist)
                    {
                        if (list.Scr_Cd.ToString() == "CUSTQUES")
                        {
                            DataView dv = new DataView(dtCustflds);
                            dv.RowFilter = "CUST_SCR_CODE <>'PREASSES' AND CUST_ACTIVE='A'";
                            dv.Sort = "CUST_DESC";
                            DataTable dtCustfldsQ = dv.ToTable();
                            foreach (DataRow drCustflds in dtCustfldsQ.Rows)
                            {
                                cb.BeginText();
                                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                                cb.SetFontAndSize(bfTimes, 12);
                                string Code = drCustflds["CUST_CODE"].ToString();
                                if (RankCritlist.Count > 0)
                                {
                                    if (list.Fld_cd.ToString() == drCustflds["CUST_CODE"].ToString())
                                    {
                                        if (list.Scr_Cd != Priv_Scr)
                                        {
                                            PdfScreen = list.Scr_Cd;

                                            if (First == false)
                                            {
                                                Y_Pos = 20;
                                                CheckBottomBorderReached(document, writer);
                                            }
                                            else
                                                pdfScreenHead();
                                            Priv_Scr = list.Scr_Cd;
                                            First = false;
                                        }
                                        X_Pos = 60;
                                        cb.SetFontAndSize(bfTimes, 12);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, drCustflds["CUST_DESC"].ToString(), X_Pos, Y_Pos, 0);
                                        
                                            
                                        cb.EndText();
                                        RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                        string Cntind = null,privCntind=null;
                                            foreach (RNKCRIT2Entity drRnkCrit2 in RnkCrit2list)
                                            {
                                                Cntind = drRnkCrit2.CountInd.ToString();
                                                cb.BeginText();
                                                if (Cntind != privCntind)
                                                {
                                                    if (drRnkCrit2.CountInd.ToString() == "M")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "All Members", X_Pos + 355, Y_Pos, 0);
                                                    else if (drRnkCrit2.CountInd.ToString() == "A")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Applicant Only", X_Pos + 355, Y_Pos, 0);
                                                    else if (drRnkCrit2.CountInd.ToString() == "H")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Highest Points Only", X_Pos + 355, Y_Pos, 0);
                                                    else if (drRnkCrit2.CountInd.ToString() == "L")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Lowest Points Only", X_Pos + 355, Y_Pos, 0);
                                                    else
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.CountInd.ToString(), X_Pos + 355, Y_Pos, 0);
                                                    privCntind = Cntind;
                                                }
                                                if (drRnkCrit2.RespCd.ToString().Trim() == "G" || drRnkCrit2.RespCd.ToString().Trim() == "G")
                                                {
                                                    decimal FromYear = decimal.Divide((decimal.Parse(drRnkCrit2.GtNum.ToString())), 12);
                                                    decimal ToYear = decimal.Divide((decimal.Parse(drRnkCrit2.LtNum.ToString())), 12);
                                                    decimal frmNum = decimal.Truncate(FromYear);
                                                    decimal toNum = decimal.Truncate(ToYear);
                                                    decimal monthfrm = decimal.Remainder((decimal.Parse(drRnkCrit2.GtNum.ToString())), 12);
                                                    decimal monthto = decimal.Remainder((decimal.Parse(drRnkCrit2.LtNum.ToString())), 12);
                                                    monthfrm = decimal.Truncate(monthfrm); monthto = decimal.Truncate(monthto);
                                                    string FY = string.Empty, TY = string.Empty, FM = string.Empty, TM = string.Empty;
                                                    if (frmNum > 0) FY = frmNum + " Yr "; if (toNum > 0) TY = toNum + " Yr ";
                                                    if (monthfrm > 0) FM = monthfrm + " Mo "; if (monthto > 0) TM = monthto + " Mo ";
                                                    string RespText = drRnkCrit2.RespText.ToString() + " & " + FY + FM +" To "+ TY + TM;
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, RespText, X_Pos + 465, Y_Pos, 0);
                                                }
                                                else
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.RespText.ToString(), X_Pos + 465, Y_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.Points.ToString(), X_Pos + 710, Y_Pos, 0);
                                                Y_Pos -= 20;
                                                CheckBottomBorderReached(document, writer);
                                                cb.EndText();
                                            }
                                       
                                        cb.BeginText();
                                    }
                                }
                                cb.EndText();
                            }
                        }
                        else if (list.Scr_Cd.ToString() == "CASE2001" || list.Scr_Cd.ToString() == "CASE2330")
                        {
                            foreach (DataRow drRnkQuee in dtRnkQuee.Rows)
                            {
                                cb.BeginText();
                                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                                cb.SetFontAndSize(bfTimes, 12);
                                string Code = drRnkQuee["RANKQUES_CODE"].ToString();
                                if (RankCritlist.Count > 0)
                                {
                                    if (drRnkQuee["RANKQUES_SCR"].ToString() == "CASE2001" || drRnkQuee["RANKQUES_SCR"].ToString() == "CASE2330")
                                    {
                                        if (list.Fld_cd.ToString() == drRnkQuee["RANKQUES_CODE"].ToString())
                                        {
                                            if (list.Scr_Cd != Priv_Scr)
                                            {
                                                PdfScreen = list.Scr_Cd;
                                                if (First == false)
                                                {
                                                    Y_Pos = 20;
                                                    CheckBottomBorderReached(document, writer);
                                                }
                                                pdfScreenHead();
                                                Priv_Scr = list.Scr_Cd;
                                                First = false;
                                            }
                                            X_Pos = 60;
                                            cb.SetFontAndSize(bfTimes, 12);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkQuee["RANKQUES_DESC"].ToString(), X_Pos, Y_Pos, 0);
                                            
                                            cb.EndText();
                                            RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                            string Cntind = null, privCntind = null;
                                                foreach (RNKCRIT2Entity drRnkCrit2 in RnkCrit2list)
                                                {
                                                    Cntind = drRnkCrit2.CountInd.ToString();
                                                    cb.BeginText();
                                                    if (Cntind != privCntind)
                                                    {
                                                        if (drRnkCrit2.CountInd.ToString() == "M")
                                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "All Members", X_Pos + 355, Y_Pos, 0);
                                                        else if (drRnkCrit2.CountInd.ToString() == "A")
                                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Applicant Only", X_Pos + 355, Y_Pos, 0);
                                                        else if (drRnkCrit2.CountInd.ToString() == "H")
                                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Highest Points Only", X_Pos + 355, Y_Pos, 0);
                                                        else if (drRnkCrit2.CountInd.ToString() == "L")
                                                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Lowest Points Only", X_Pos + 355, Y_Pos, 0);
                                                        else
                                                            cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.CountInd.ToString(), X_Pos + 355, Y_Pos, 0);
                                                        privCntind = Cntind;
                                                    }

                                                    if (drRnkCrit2.RespCd.ToString().Trim() == "G" || drRnkCrit2.RespCd.ToString().Trim() == "G")
                                                    {
                                                        decimal FromYear = decimal.Divide((decimal.Parse(drRnkCrit2.GtNum.ToString())), 12);
                                                        decimal ToYear = decimal.Divide((decimal.Parse(drRnkCrit2.LtNum.ToString())), 12);
                                                        decimal frmNum = decimal.Truncate(FromYear);
                                                        decimal toNum = decimal.Truncate(ToYear);
                                                        decimal monthfrm = decimal.Remainder((decimal.Parse(drRnkCrit2.GtNum.ToString())), 12);
                                                        decimal monthto = decimal.Remainder((decimal.Parse(drRnkCrit2.LtNum.ToString())), 12);
                                                        monthfrm = decimal.Truncate(monthfrm); monthto = decimal.Truncate(monthto);
                                                        string FY=string.Empty,TY=string.Empty,FM=string.Empty,TM=string.Empty;
                                                        if (frmNum > 0) FY = frmNum + " Yr "; if (toNum > 0) TY = toNum + " Yr ";
                                                        if (monthfrm > 0) FM = monthfrm+ " Mo "; if (monthto> 0) TM = monthto+ " Mo ";
                                                        string RespText = drRnkCrit2.RespText.ToString() + " & " + FY + FM + " To " + TY + TM;
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, RespText, X_Pos + 465, Y_Pos, 0);
                                                    }
                                                    else
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.RespText.ToString(), X_Pos + 465, Y_Pos, 0);
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.Points.ToString(), X_Pos + 710, Y_Pos, 0);
                                                    Y_Pos -= 20;
                                                    CheckBottomBorderReached(document, writer);
                                                    cb.EndText();
                                                }
                                            
                                            cb.BeginText();
                                        }
                                    }
                                }
                                cb.EndText();
                            }
                        }
                        //else if (list.Scr_Cd.ToString() == "CASE2330")
                        //{
                        //    foreach (DataRow drRnkQuee in dtRnkQuee.Rows)
                        //    {
                        //        cb.BeginText();
                        //        //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                        //        cb.SetFontAndSize(bfTimes, 12);
                        //        string Code = drRnkQuee["RANKQUES_CODE"].ToString();
                        //        if (RankCritlist.Count > 0)
                        //        {
                        //            if (drRnkQuee["RANKQUES_SCR"].ToString() == "CASE2330")
                        //            {
                        //                if (list.Fld_cd.ToString() == drRnkQuee["RANKQUES_CODE"].ToString())
                        //                {
                        //                    if (list.Scr_Cd != Priv_Scr)
                        //                    {
                        //                        PdfScreen = list.Scr_Cd;
                        //                        if (First == false)
                        //                        {
                        //                            Y_Pos = 20;
                        //                            CheckBottomBorderReached(document, writer);
                        //                        }
                        //                        pdfScreenHead();
                        //                        Priv_Scr = list.Scr_Cd;
                        //                        First = false;
                        //                    }
                        //                    X_Pos = 60;
                        //                    cb.SetFontAndSize(bfTimes, 12);
                        //                    cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkQuee["RANKQUES_DESC"].ToString(), X_Pos, Y_Pos, 0);
                                            
                        //                    cb.EndText();
                        //                    RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                        //                    string Cntind = null, privCntind = null;
                        //                    foreach (RNKCRIT2Entity drRnkCrit2 in RnkCrit2list)
                        //                    {
                        //                        Cntind = drRnkCrit2.CountInd.ToString();
                        //                        cb.BeginText();
                        //                        if (Cntind != privCntind)
                        //                        {
                        //                            if (drRnkCrit2.CountInd.ToString() == "M")
                        //                                cb.ShowTextAligned(Element.ALIGN_LEFT, "All Members", X_Pos + 355, Y_Pos, 0);
                        //                            else if (drRnkCrit2.CountInd.ToString() == "A")
                        //                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Applicant Only", X_Pos + 355, Y_Pos, 0);
                        //                            else if (drRnkCrit2.CountInd.ToString() == "H")
                        //                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Highest Points Only", X_Pos + 355, Y_Pos, 0);
                        //                            else if (drRnkCrit2.CountInd.ToString() == "L")
                        //                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Lowest Points Only", X_Pos + 355, Y_Pos, 0);
                        //                            else
                        //                                cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.CountInd.ToString(), X_Pos + 355, Y_Pos, 0);
                        //                            privCntind = Cntind;
                        //                        }
                        //                        cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.RespText.ToString(), X_Pos + 465, Y_Pos, 0);
                        //                        cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.Points.ToString(), X_Pos + 710, Y_Pos, 0);
                        //                        Y_Pos -= 20;
                        //                        CheckBottomBorderReached(document, writer);
                        //                        cb.EndText();
                        //                    }

                        //                    cb.BeginText();
                        //                }
                        //            }
                        //        }
                        //        cb.EndText();
                        //    }
                        //}
                        else if (ScreenName == "PREASSES" && ScreenSw == "Y")
                        {
                            DataView dv = new DataView(dtCustflds);
                            dv.RowFilter = "CUST_SCR_CODE ='PREASSES' AND CUST_ACTIVE='A'";
                            dv.Sort = "CUST_DESC";
                            DataTable dtCustfldsQ = dv.ToTable();
                            foreach (DataRow drCustflds in dtCustfldsQ.Rows)
                            {
                                cb.BeginText();
                                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                                cb.SetFontAndSize(bfTimes, 12);
                                string Code = drCustflds["CUST_CODE"].ToString();
                                if (RankCritlist.Count > 0)
                                {
                                    if (list.Fld_cd.ToString() == drCustflds["CUST_CODE"].ToString())
                                    {
                                        if (list.Scr_Cd != Priv_Scr)
                                        {
                                            PdfScreen = list.Scr_Cd;

                                            if (First == false)
                                            {
                                                Y_Pos = 20;
                                                CheckBottomBorderReached(document, writer);
                                            }
                                            else
                                                pdfScreenHead();
                                            Priv_Scr = list.Scr_Cd;
                                            First = false;
                                        }
                                        X_Pos = 60;
                                        cb.SetFontAndSize(bfTimes, 12);
                                        cb.ShowTextAligned(Element.ALIGN_LEFT, drCustflds["CUST_DESC"].ToString(), X_Pos, Y_Pos, 0);


                                        cb.EndText();
                                        RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                        string Cntind = null, privCntind = null;
                                        foreach (RNKCRIT2Entity drRnkCrit2 in RnkCrit2list)
                                        {
                                            Cntind = drRnkCrit2.CountInd.ToString();
                                            cb.BeginText();
                                            if (Cntind != privCntind)
                                            {
                                                if (drRnkCrit2.CountInd.ToString() == "M")
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "All Members", X_Pos + 355, Y_Pos, 0);
                                                else if (drRnkCrit2.CountInd.ToString() == "A")
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Applicant Only", X_Pos + 355, Y_Pos, 0);
                                                else if (drRnkCrit2.CountInd.ToString() == "H")
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Highest Points Only", X_Pos + 355, Y_Pos, 0);
                                                else if (drRnkCrit2.CountInd.ToString() == "L")
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Lowest Points Only", X_Pos + 355, Y_Pos, 0);
                                                else
                                                    cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.CountInd.ToString(), X_Pos + 355, Y_Pos, 0);
                                                privCntind = Cntind;
                                            }
                                            if (drRnkCrit2.RespCd.ToString().Trim() == "G" || drRnkCrit2.RespCd.ToString().Trim() == "G")
                                            {
                                                decimal FromYear = decimal.Divide((decimal.Parse(drRnkCrit2.GtNum.ToString())), 12);
                                                decimal ToYear = decimal.Divide((decimal.Parse(drRnkCrit2.LtNum.ToString())), 12);
                                                decimal frmNum = decimal.Truncate(FromYear);
                                                decimal toNum = decimal.Truncate(ToYear);
                                                decimal monthfrm = decimal.Remainder((decimal.Parse(drRnkCrit2.GtNum.ToString())), 12);
                                                decimal monthto = decimal.Remainder((decimal.Parse(drRnkCrit2.LtNum.ToString())), 12);
                                                monthfrm = decimal.Truncate(monthfrm); monthto = decimal.Truncate(monthto);
                                                string FY = string.Empty, TY = string.Empty, FM = string.Empty, TM = string.Empty;
                                                if (frmNum > 0) FY = frmNum + " Yr "; if (toNum > 0) TY = toNum + " Yr ";
                                                if (monthfrm > 0) FM = monthfrm + " Mo "; if (monthto > 0) TM = monthto + " Mo ";
                                                string RespText = drRnkCrit2.RespText.ToString() + " & " + FY + FM + " To " + TY + TM;
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, RespText, X_Pos + 465, Y_Pos, 0);
                                            }
                                            else
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.RespText.ToString(), X_Pos + 465, Y_Pos, 0);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.Points.ToString(), X_Pos + 710, Y_Pos, 0);
                                            Y_Pos -= 20;
                                            CheckBottomBorderReached(document, writer);
                                            cb.EndText();
                                        }

                                        cb.BeginText();
                                    }
                                }
                                cb.EndText();
                            }
                        }
                        else if (list.Scr_Cd.ToString() == "PREASSES")
                        {
                            foreach (DataRow drRnkQuee in dtRnkQuee.Rows)
                            {
                                cb.BeginText();
                                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                                cb.SetFontAndSize(bfTimes, 12);
                                string Code = drRnkQuee["RANKQUES_CODE"].ToString();
                                if (RankCritlist.Count > 0)
                                {
                                    if (drRnkQuee["RANKQUES_SCR"].ToString() == "PREASSES")
                                    {
                                        if (list.Fld_cd.ToString() == drRnkQuee["RANKQUES_CODE"].ToString())
                                        {
                                            if (list.Scr_Cd != Priv_Scr)
                                            {
                                                PdfScreen = list.Scr_Cd;
                                                if (First == false)
                                                {
                                                    Y_Pos = 20;
                                                    CheckBottomBorderReached(document, writer);
                                                }
                                                pdfScreenHead();
                                                Priv_Scr = list.Scr_Cd;
                                                First = false;
                                            }
                                            X_Pos = 60;
                                            cb.SetFontAndSize(bfTimes, 12);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkQuee["RANKQUES_DESC"].ToString(), X_Pos, Y_Pos, 0);

                                            cb.EndText();
                                            RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                            string Cntind = null, privCntind = null;
                                            foreach (RNKCRIT2Entity drRnkCrit2 in RnkCrit2list)
                                            {
                                                Cntind = drRnkCrit2.CountInd.ToString();
                                                cb.BeginText();
                                                if (Cntind != privCntind)
                                                {
                                                    if (drRnkCrit2.CountInd.ToString() == "M")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "All Members", X_Pos + 355, Y_Pos, 0);
                                                    else if (drRnkCrit2.CountInd.ToString() == "A")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Applicant Only", X_Pos + 355, Y_Pos, 0);
                                                    else if (drRnkCrit2.CountInd.ToString() == "H")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Highest Points Only", X_Pos + 355, Y_Pos, 0);
                                                    else if (drRnkCrit2.CountInd.ToString() == "L")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Lowest Points Only", X_Pos + 355, Y_Pos, 0);
                                                    else
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.CountInd.ToString(), X_Pos + 355, Y_Pos, 0);
                                                    privCntind = Cntind;
                                                }
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.RespText.ToString(), X_Pos + 465, Y_Pos, 0);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkCrit2.Points.ToString(), X_Pos + 710, Y_Pos, 0);
                                                Y_Pos -= 20;
                                                CheckBottomBorderReached(document, writer);
                                                cb.EndText();
                                            }

                                            cb.BeginText();
                                        }
                                    }
                                }
                                cb.EndText();
                            }
                        }
                    }
                    }
                    catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

                cb.BeginText();
                Y_Pos = 07;
                X_Pos = 20;
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                cb.SetFontAndSize(bfTimes, 12);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.ShowTextAligned(800, DateTime.Now.ToLocalTime().ToString(), 07, 20, 0);
                Y_Pos = 07;
                X_Pos = 800;
                cb.ShowTextAligned(800, "Page:" + pageNumber.ToString(), 800, 07, 0);
                cb.EndText();

                document.Close();
                fs.Close();
                fs.Dispose();

                if (baseform.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
                else
                {
                    FrmViewer objfrm = new FrmViewer(PdfName);
                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
                //}
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void HeadingPage()
        {
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 13);
            cb.ShowTextAligned(Element.ALIGN_LEFT, "Field", X_Pos, Y_Pos, 0);
            cb.ShowTextAligned(Element.ALIGN_LEFT, "Counting Indicator", X_Pos + 340, Y_Pos, 0);
            cb.ShowTextAligned(Element.ALIGN_LEFT, "Criteria", X_Pos + 480, Y_Pos, 0);
            cb.ShowTextAligned(Element.ALIGN_LEFT, "Points", X_Pos + 700, Y_Pos, 0);
           
        }

        private void pdfScreenHead()
        {
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 14);
            if (PdfScreen == "CASE2001")
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Screen Name: Client Intake", 400, 460, 0);
            else if (PdfScreen == "CASE2330")
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Screen Name: Medical Emergency", 400, 460, 0);
            else if (PdfScreen == "PREASSES")
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Screen Name: Pre Assessment", 400, 460, 0);
            else
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Screen Name: Custom Questions", 400, 460, 0);
        }

        private void SetLine()
        {
            cb.EndText();
            cb.SetLineWidth(0.7f);
            cb.MoveTo(X_Pos, Y_Pos);
            cb.LineTo(790, Y_Pos);
            cb.Stroke();
        }

        private void gvHie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex!=-1)
            {
                //PdfListForm pdfListForm = new PdfListForm(baseform, priviliges, false);
                //pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveForm_Closed);
                //pdfListForm.ShowDialog();
                On_SaveForm_Closed();
            }
        }

        private void RankDefinitions_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CheckBottomBorderReached(Document document, PdfWriter writer)
        {
            if (Y_Pos <= 20)
            {

                cb.EndText();

                cb.BeginText();
                Y_Pos = 07;
                X_Pos = 20;
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                cb.SetFontAndSize(bfTimes, 12);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.ShowTextAligned(800, DateTime.Now.ToLocalTime().ToString(), 07, 20, 0);
                Y_Pos = 07;
                X_Pos = 800;
                cb.ShowTextAligned(800, "Page:" + pageNumber.ToString(), 800, 07, 0);
                cb.EndText();
                
                document.NewPage();
                pageNumber = writer.PageNumber;
                
                cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                cb.SetFontAndSize(bfTimes, 12);
                
                printHText();
                pdfScreenHead();
                X_Pos = 60; Y_Pos = 455;
                SetLine();
                Y_Pos -= 14;
                cb.BeginText();
                HeadingPage();
                Y_Pos -= 5;
                SetLine();

                Y_Pos = 420;
                X_Pos = 60;
                cb.BeginText();
                cb.SetFontAndSize(bfTimes, 12);
            }
        }

        private void PrintRec(string PrintText, int StrWidth)
        {

            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);//cb.SetFontAndSize(bfTimes, 10);
            cb.SetFontAndSize(bfTimes, 12);
            cb.ShowTextAligned(800, PrintText, X_Pos, Y_Pos, 0);
            X_Pos += StrWidth;
            PrintText = null;
        }

        private void printHText()
        {
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Rank Definitions", 400, 520, 0);
            //if (HierarchyList == "******")
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "All Programs" + "( " + HierarchyList.Substring(0, 2) + "-" + HierarchyList.Substring(2, 2) + "-" + HierarchyList.Substring(4, 2) + ")", 400, 500, 0);
            //else
            //{
                //DataSet dsProg = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(HierarchyList.Substring(0, 2), HierarchyList.Substring(2, 2), HierarchyList.Substring(4, 2));
                //ProgName = (dsProg.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, AgencyDesc.Trim(), 400, 500, 0);
            //}
            cb.ShowTextAligned(Element.ALIGN_CENTER, "Category: " + rankDesc.Trim(), 400, 480, 0);
        }

        private void picPDF_Click(object sender, EventArgs e)
        {
            On_SaveForm_Closed();
        }

        // End Report Section.................

    }
}