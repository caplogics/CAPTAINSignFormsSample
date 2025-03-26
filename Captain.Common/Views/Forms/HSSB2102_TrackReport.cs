#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
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
using System.Xml;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2102_TrackReport : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public HSSB2102_TrackReport(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = /*Privileges.Program + " - " + */Privileges.PrivilegeName.Trim();
            FillSequenceCombo(); FillFromComponentsCombo();
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string Calling_UserID { get; set; }

        public string propReportPath { get; set; }

        #endregion


        private void FillSequenceCombo()
        {
            cmbSeq.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("1 - Component and Task", "01"));
            listItem.Add(new Captain.Common.Utilities.ListItem("2 - Component and Sequence", "02"));

            cmbSeq.Items.AddRange(listItem.ToArray());
            cmbSeq.SelectedIndex = 0;

        }

        List<SPCommonEntity> AgyCommon_List = new List<SPCommonEntity>();
        DataSet dsComp = DatabaseLayer.Lookups.GetLookUpFromAGYTAB("00356");
        
        
        private void FillFromComponentsCombo()
        {
            string components = BaseForm.UserProfile.Components.ToString();
            string Tmp_Desc = string.Empty;
            List<string> incomeList = new List<string>();
            if (components != null)
            {
                string[] incomeTypes = components.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }

            cmbCompFrom.Items.Clear();
            DataTable dtcomp = dsComp.Tables[0];
            DataView dv = dtcomp.DefaultView;
            dv.Sort = "Code";
            dtcomp = dv.ToTable();
            if (dtcomp != null && dtcomp.Rows.Count > 0)
            {
                foreach (DataRow row in dtcomp.Rows)
                {
                    AgyCommon_List.Add(new SPCommonEntity(row));
                }
            }
            //cmbCompFrom.ColorMember = "FavoriteColor";
            //AgyCommon_List = _model.SPAdminData.Get_AgyRecs("COMPONENTS");
            cmbCompFrom.ColorMember = "FavoriteColor";
            AgyCommon_List = AgyCommon_List.OrderByDescending(u => u.Active).ToList();
            foreach (SPCommonEntity Entity in AgyCommon_List)
            {
                Tmp_Desc = string.Empty;
                Tmp_Desc = String.Format("{0,-8}", Entity.Code.ToString().Trim()) + String.Format("{0,8}", " - " + Entity.Desc.ToString().Trim());
                if (components.Contains("****"))
                {
                    cmbCompFrom.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red));
                }
                else
                {
                    if (components != null && incomeList.Contains(Entity.Code.ToString()))
                    {
                        cmbCompFrom.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red));
                    }
                }
            }


            //cmbCompFrom.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            cmbCompFrom.SelectedIndex = 0;
            
        }

        private void ToComponentsCombo()
        {
            string components = BaseForm.UserProfile.Components.ToString();
            string Tmp_Desc = string.Empty;
            List<string> incomeList = new List<string>();
            if (components != null)
            {
                string[] incomeTypes = components.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }

            cmbCompTo.Items.Clear();
            //cmbCompFrom.ColorMember = "FavoriteColor";
            //List<SPCommonEntity> AgyCommon_List_To = new List<SPCommonEntity>();
            //AgyCommon_List_To = _model.SPAdminData.Get_AgyRecs("COMPONENTS");
            cmbCompTo.ColorMember = "FavoriteColor";
            bool Isfalse = false;
            AgyCommon_List = AgyCommon_List.OrderByDescending(u => u.Active).ToList();
            foreach (SPCommonEntity Entity in AgyCommon_List)
            {
                if (Entity.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString().Trim())
                    Isfalse = true;

                //if (int.Parse(Entity.Code.Trim()) >= int.Parse(((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString().Trim()))
                if(Isfalse)
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-8}", Entity.Code.ToString().Trim()) + String.Format("{0,8}", " - " + Entity.Desc.ToString().Trim());
                    if (components.Contains("****"))
                    {
                        cmbCompTo.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red));
                    }
                    else
                    {
                        if (components != null && incomeList.Contains(Entity.Code.ToString()))
                        {
                            cmbCompTo.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red));
                        }
                    }
                }
            }


            //cmbCompFrom.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            cmbCompTo.SelectedIndex = 0;
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        string Program_Year;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(720, 25);
        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear)) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }

            Agency = Agy; Depart = Dept; Program = Prog; strYear = Year;
            //fillBusCombo(Agency, Depart, Program, strYear);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(640, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(720, 25);
        }


        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
           /* HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    
                }
            }
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }

        }

        private void cmbCompFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString().Trim()))
                ToComponentsCombo();
        }

        string strFolderPath = string.Empty; PdfContentByte cb; int X_Pos, Y_Pos;
        string Random_Filename = null; string PdfName = null;
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        private void On_SaveForm_Closed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                //PdfName = strFolderPath + PdfName;
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

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);
                cb = writer.DirectContent;
                
                //table.HeaderRows = 2;

                string SP_Agy = string.Empty, SP_Dept = string.Empty, SP_Prog = string.Empty;
                if (Agency == "**") SP_Agy = null; else SP_Agy = Agency;
                if (Depart == "**") SP_Dept = null; else SP_Dept = Depart;
                if (Program == "**") SP_Prog = null; else SP_Prog = Program;

                CustfldsEntity Search_Cust=new CustfldsEntity(true);
                Search_Cust.ScrCode = "HSS00133";
                List<CustfldsEntity> CustQues = _model.SPAdminData.Browse_CUSTFLDS(Search_Cust, "Browse");

                CustRespEntity Search_CustResp = new CustRespEntity(true);
                Search_CustResp.ScrCode = "HSS00133";
                List<CustRespEntity> CustResp = _model.FieldControls.Browse_CUSTRESP(Search_CustResp, "Browse");

                List<ChldTrckEntity> TrackList = new List<ChldTrckEntity>();
                List<ChldTrckEntity> Track_Det=new List<ChldTrckEntity>();
                List<ChldTrckREntity> chldtrackRList = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                //ChldTrckEntity Search_Entity = new ChldTrckEntity(true);
                TrackList=_model.ChldTrckData.Browse_CasetrckDetails(((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Value.ToString().Trim());

                try
                {
                    PrintHeaderPage(document, writer);

                    document.NewPage();
                    Y_Pos = 795;

                    PdfPTable table = new PdfPTable(4);
                    table.TotalWidth = 550f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 20f, 15f, 50f, 80f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    
                    if (TrackList.Count > 0)
                    {
                        int Start_Comp = int.Parse(((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString().Trim());
                        int End_Comp = int.Parse(((Captain.Common.Utilities.ListItem)cmbCompTo.SelectedItem).Value.ToString().Trim());
                        int Length = End_Comp - Start_Comp;
                        bool First_comp = true;
                        for (int i = 0; i <= Length; i++)
                        {
                            string Component_No = string.Empty;
                            Component_No = "0000".Substring(0, 4 - Start_Comp.ToString().Length) + Start_Comp.ToString().Trim();
                            if (Component_No == "0000")
                                Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(Component_No));
                            else
                                Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(Component_No) && u.Agency.Trim().Equals(SP_Agy) && u.Dept.Equals(SP_Dept) && u.Prog.Equals(SP_Prog));

                            if (Track_Det != null && Track_Det.Count > 0)
                            {
                                if (!First_comp)
                                {
                                    if (table.Rows.Count > 0)
                                        document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }
                                string Component_Name = string.Empty;
                                foreach (SPCommonEntity CompEntity in AgyCommon_List)
                                {
                                    if (Component_No.Trim() == CompEntity.Code.Trim())
                                    {
                                        Component_Name = CompEntity.Desc.Trim(); break;
                                    }
                                }

                                PdfPCell CompHeader = new PdfPCell(new Phrase("COMPONENT    " + Component_No.Trim() + "    " + Component_Name, TblFontBold));
                                CompHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                                CompHeader.Colspan = 4;
                                CompHeader.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(CompHeader);

                                PdfPCell Header = new PdfPCell(new Phrase("SEQ", TblFontBold));
                                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Header.Colspan = 4;
                                Header.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(Header);

                                PdfPCell Header1 = new PdfPCell(new Phrase("TASK", TblFontBold));
                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Header.Colspan = 4;
                                Header1.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(Header1);

                                PdfPCell Header2 = new PdfPCell(new Phrase("DESCRIPTION", TblFontBold));
                                Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Header.Colspan = 4;
                                Header2.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(Header2);

                                PdfPCell Header3 = new PdfPCell(new Phrase("QUESTION", TblFontBold));
                                Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Header.Colspan = 4;
                                Header3.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(Header3);

                                foreach (ChldTrckEntity Entity in Track_Det)
                                {
                                    string Question_Desc = string.Empty; bool First = true;
                                    PdfPCell Seq = new PdfPCell(new Phrase(Entity.SEQ.Trim(), TableFont));
                                    Seq.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Seq.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Seq);

                                    PdfPCell Task = new PdfPCell(new Phrase(Entity.TASK.Trim(), TableFont));
                                    Task.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Task.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Task);

                                    PdfPCell DESC = new PdfPCell(new Phrase(Entity.TASKDESCRIPTION.Trim(), TableFont));
                                    DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                    DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(DESC);
                                    ChldTrckEntity Trck_Ques = TrackList.Find(u => u.COMPONENT.Equals("0000") && u.TASK.Trim().Equals(Entity.TASK.Trim()));
                                    if (CustQues.Count > 0)
                                    {
                                        string Temp_Ques = string.Empty;
                                        if (Trck_Ques!=null)
                                        {
                                            if (!string.IsNullOrEmpty(Trck_Ques.CustQCodes.Trim()))
                                            {
                                                if (Trck_Ques.CustQCodes.Trim().Length >= 6)
                                                {
                                                    for (int Q = 0; Q < Trck_Ques.CustQCodes.Trim().Length; )
                                                    {
                                                        Temp_Ques = Trck_Ques.CustQCodes.Substring(Q, 6);

                                                        if (CustQues.Count > 0)
                                                        {
                                                            CustfldsEntity QuesList = CustQues.Find(u => u.CustCode.Trim().Equals(Temp_Ques.Trim()));
                                                            if (QuesList != null)
                                                            {
                                                                Question_Desc = QuesList.CustDesc.Trim();

                                                                List<CustRespEntity> RespList = CustResp.FindAll(u => u.ResoCode.Trim().Equals(QuesList.CustCode.Trim()));
                                                                string Resp = string.Empty;
                                                                if (RespList != null)
                                                                {

                                                                    foreach (CustRespEntity RspEntity in RespList)
                                                                    {
                                                                        Resp += RspEntity.DescCode.Trim();
                                                                    }
                                                                }
                                                                if (First)
                                                                {
                                                                    PdfPCell QUESTION = new PdfPCell(new Phrase(Question_Desc, TableFont));
                                                                    QUESTION.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    QUESTION.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                    table.AddCell(QUESTION);
                                                                }
                                                                else
                                                                {
                                                                    PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                                                    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Space1.Colspan = 3;
                                                                    Space1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                                    table.AddCell(Space1);

                                                                    PdfPCell QUESTION = new PdfPCell(new Phrase(Question_Desc, TableFont));
                                                                    QUESTION.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    QUESTION.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                    table.AddCell(QUESTION);
                                                                }

                                                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Space.Colspan = 3;
                                                                Space.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                                table.AddCell(Space);

                                                                PdfPCell Resp_Cell = new PdfPCell(new Phrase("VALID RESPONSES: " + Resp, TableFont));
                                                                Resp_Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Resp_Cell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                table.AddCell(Resp_Cell);
                                                            }
                                                            else
                                                            {
                                                                PdfPCell SpaceQues = new PdfPCell(new Phrase("", TableFont));
                                                                SpaceQues.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                SpaceQues.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                table.AddCell(SpaceQues);
                                                            }
                                                        }
                                                        Q += 7; First = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                PdfPCell SpaceQues = new PdfPCell(new Phrase("", TableFont));
                                                SpaceQues.HorizontalAlignment = Element.ALIGN_CENTER;
                                                SpaceQues.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                table.AddCell(SpaceQues);
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell SpaceQues = new PdfPCell(new Phrase("", TableFont));
                                            SpaceQues.HorizontalAlignment = Element.ALIGN_CENTER;
                                            SpaceQues.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            table.AddCell(SpaceQues);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell SpaceQues = new PdfPCell(new Phrase("", TableFont));
                                        SpaceQues.HorizontalAlignment = Element.ALIGN_CENTER;
                                        SpaceQues.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(SpaceQues);
                                    }

                                    List<CommonEntity> AgyFundList = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                                    AgyFundList = filterByHIE(AgyFundList);

                                    List<ChldTrckREntity> TrackR = chldtrackRList.FindAll(u => u.Agency.Trim().Equals(Entity.Agency.Trim()) && u.Dept.Trim().Equals(Entity.Dept.Trim())
                                        && u.Prog.Trim().Equals(Entity.Prog.Trim()) && u.COMPONENT.Trim().Equals(Entity.COMPONENT.Trim()) && u.TASK.Trim().Equals(Entity.TASK.Trim()));
                                    if (TrackR.Count>0)
                                    {
                                        PdfPTable nestedTable = new PdfPTable(7);
                                        nestedTable.WidthPercentage = 100;
                                        //table.LockedWidth = true;
                                        float[] Nestedwidths = new float[] { 15f, 25f, 15f, 15f,15f,20f,15f };
                                        nestedTable.SetWidths(Nestedwidths);
                                        nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;
                                        bool ISFundHeader = true;

                                        bool IsFund = true;
                                        foreach (CommonEntity FEntity in AgyFundList)
                                        {
                                            IsFund = true;
                                            foreach (ChldTrckREntity TREntity in TrackR)
                                            {
                                                //PdfPCell Fund = new PdfPCell(new Phrase(TREntity.FUND.Trim(), TableFont));
                                                //Fund.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //Fund.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                //nestedTable.AddCell(Fund);
                                                if (FEntity.Code.Trim() == TREntity.FUND.Trim())
                                                {
                                                    if (ISFundHeader)
                                                    {
                                                        FundHeader(nestedTable); ISFundHeader = false;
                                                    }

                                                    PdfPCell Fund = new PdfPCell(new Phrase(FEntity.Desc.Trim(), TableFont));
                                                    Fund.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Fund.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                    nestedTable.AddCell(Fund);

                                                    string Days = string.Empty; string EvryYear = string.Empty; string Nxt_Act = string.Empty;
                                                    switch (TREntity.INTAKEENTRY.Trim())
                                                    {
                                                        case "E": Days = "Days from Entry"; break;
                                                        case "A": Days = "Days from Intake"; break;
                                                        case "D": Days = "Days from Birth"; break;
                                                        case "N": Days = "No Next Action"; break;
                                                    }
                                                    if (TREntity.REQUIREYEAR.Trim().Equals("Y"))
                                                        EvryYear = "Y";
                                                    else EvryYear = "N";

                                                    PdfPCell Intake = new PdfPCell(new Phrase(Days.Trim(), TableFont));
                                                    Intake.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Intake.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    nestedTable.AddCell(Intake);

                                                    PdfPCell Entry_days = new PdfPCell(new Phrase(TREntity.ENTERDAYS.Trim(), TableFont));
                                                    Entry_days.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Entry_days.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    nestedTable.AddCell(Entry_days);

                                                    PdfPCell Year_evr = new PdfPCell(new Phrase(TREntity.REQUIREYEAR.Trim(), TableFont));
                                                    Year_evr.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Year_evr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    nestedTable.AddCell(Year_evr);

                                                    PdfPCell Next = new PdfPCell(new Phrase(TREntity.NXTACTION.Trim(), TableFont));
                                                    Next.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Next.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    nestedTable.AddCell(Next);

                                                    PdfPCell NextTask = new PdfPCell(new Phrase(TREntity.NEXTTASK.Trim(), TableFont));
                                                    NextTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    NextTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    nestedTable.AddCell(NextTask);

                                                    PdfPCell NextDays = new PdfPCell(new Phrase(TREntity.NEXTDAYS.Trim(), TableFont));
                                                    NextDays.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    NextDays.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    nestedTable.AddCell(NextDays);
                                                    IsFund = false;
                                                    break;
                                                }
                                            }
                                            //if (IsFund)
                                            //{
                                            //    PdfPCell Intake = new PdfPCell(new Phrase("", TableFont));
                                            //    Intake.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //    Intake.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //    nestedTable.AddCell(Intake);

                                            //    PdfPCell Entry_days = new PdfPCell(new Phrase("", TableFont));
                                            //    Entry_days.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //    Entry_days.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //    nestedTable.AddCell(Entry_days);

                                            //    PdfPCell Year_evr = new PdfPCell(new Phrase("", TableFont));
                                            //    Year_evr.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //    Year_evr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //    nestedTable.AddCell(Year_evr);

                                            //    PdfPCell Next = new PdfPCell(new Phrase("", TableFont));
                                            //    Next.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //    Next.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //    nestedTable.AddCell(Next);

                                            //    PdfPCell NextTask = new PdfPCell(new Phrase("", TableFont));
                                            //    NextTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //    NextTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //    nestedTable.AddCell(NextTask);

                                            //    PdfPCell NextDays = new PdfPCell(new Phrase("", TableFont));
                                            //    NextDays.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //    NextDays.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            //    nestedTable.AddCell(NextDays);
                                            //}

                                        }

                                        PdfPCell NestedLoop = new PdfPCell(nestedTable);
                                        NestedLoop.Colspan = 4;
                                        NestedLoop.Padding = 0f;
                                        NestedLoop.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(NestedLoop);

                                        //string NxtDays = string.Empty;
                                        //if (TrackR.NXTACTION.Trim().Equals("Y"))
                                        //{
                                        //    PdfPCell Next = new PdfPCell(new Phrase("NEXT TASK", TableFont));
                                        //    Next.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //    Next.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        //    table.AddCell(Next);

                                        //    PdfPCell NextTask = new PdfPCell(new Phrase(TrackR.NEXTTASK, TableFont));
                                        //    NextTask.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //    NextTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //    table.AddCell(NextTask);

                                        //    if (TrackR.NEXTTASK.Trim() == Entity.TASK.Trim())
                                        //    {
                                        //        PdfPCell NextTaskDesc = new PdfPCell(new Phrase(Entity.TASKDESCRIPTION.Trim(), TableFont));
                                        //        NextTaskDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //        NextTaskDesc.Colspan = 2;
                                        //        NextTaskDesc.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        //        table.AddCell(NextTaskDesc);
                                        //    }
                                        //    else
                                        //    {
                                        //        PdfPCell NextTaskDesc = new PdfPCell(new Phrase("", TableFont));
                                        //        NextTaskDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //        NextTaskDesc.Colspan = 2;
                                        //        NextTaskDesc.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        //        table.AddCell(NextTaskDesc);
                                        //    }
                                        //    NxtDays = "TRIGGER NEXT SBCB    " + TrackR.NEXTDAYS.Trim() + " DAYS FROM COMPLETION";
                                        //}
                                        //string Days = string.Empty; string EvryYear = string.Empty;
                                        //switch (TrackR.INTAKEENTRY.Trim())
                                        //{
                                        //    case "E": Days = "TRIGGER SBCB DATE    " + TrackR.ENTERDAYS.Trim() + " DAYS FROM ENTRY"; break;
                                        //    case "A": Days = "TRIGGER SBCB DATE    " + TrackR.ENTERDAYS.Trim() + " DAYS FROM INTAKE"; break;
                                        //    case "D": Days = "TRIGGER SBCB DATE    " + TrackR.ENTERDAYS.Trim() + " DAYS FROM BIRTH"; break;
                                        //}
                                        //if (TrackR.REQUIREYEAR.Trim().Equals("Y"))
                                        //    EvryYear = "EVERY YEAR";
                                        ////PdfPCell LastSpace = new PdfPCell(new Phrase("", TableFont));
                                        ////LastSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        ////LastSpace.Colspan = 2;
                                        ////LastSpace.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        ////table.AddCell(LastSpace);

                                        //PdfPCell Last = new PdfPCell(new Phrase(Days + "  " + EvryYear + "  " + NxtDays, TableFont));
                                        //Last.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Last.Colspan = 4;
                                        //Last.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                        //table.AddCell(Last);
                                        
                                    }
                                    PdfPCell LastBefore = new PdfPCell(new Phrase("", TableFont));
                                    LastBefore.HorizontalAlignment = Element.ALIGN_LEFT;
                                    LastBefore.Colspan = 4;
                                    LastBefore.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(LastBefore);

                                    PdfPCell Last = new PdfPCell(new Phrase("", TableFont));
                                    Last.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Last.Colspan = 4;
                                    Last.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Last);
                                }

                            }

                            Start_Comp++;
                            First_comp = false;
                        }

                        if (table.Rows.Count > 0)
                            document.Add(table);
                        else
                        {
                           
                            cb.BeginText();
                            cb.SetFontAndSize(bfTimes, 14);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No Tracks were Defined", 300, 600, 0);
                            cb.EndText();
                        }


                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();

                AlertBox.Show("Report Generated Successfully");
            }
        }

        private void BtnGenPdf_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
            pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }


        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
            /*BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);*/

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_TimesRomanI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bf_Calibri, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblParamsHeaderFont = new iTextSharp.text.Font(bf_Calibri, 11, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#2e5f71")));
            iTextSharp.text.Font TblHeaderTitleFont = new iTextSharp.text.Font(bf_Calibri, 14, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font fnttimesRoman_Italic = new iTextSharp.text.Font(bf_TimesRomanI, 9, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000")));

            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy("AGENCY", BaseForm.BaseAdminAgency, string.Empty, string.Empty, string.Empty, string.Empty);
            string _strImageFolderPath = "";
            if (hierarchyDetails != null)
            {
                string LogoName = hierarchyDetails.Logo.ToString();
                _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";
                FileInfo info = new FileInfo(_strImageFolderPath + LogoName);
                if (info.Exists)
                    _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\" + LogoName;
                else
                    _strImageFolderPath = "";

            }

            PdfPTable Headertable = new PdfPTable(2);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 25f, 80f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //border trails
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
                cellLogo.Padding = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }

            PdfPCell row1 = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblParamsHeaderFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 2;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row1);

            /*PdfPCell row2 = new PdfPCell(new Phrase("Run By : " +LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(),"3"), TableFont));
            row2.HorizontalAlignment = Element.ALIGN_LEFT;
            //row2.Colspan = 2;
            row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row2);

            PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            //row2.Colspan = 2;
            row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row21);*/

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblHeaderTitleFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.PaddingBottom = 5;
            row3.MinimumHeight = 6;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(row3);

            /* string Agy = "Agency : All"; string Dept = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
             if (Agency != "**") Agy = "Agency : " + Agency;
             if (Depart != "**") Dept = "Dept : " + Depart;
             if (Program != "**") Prg = "Program : " + Program;
             if (CmbYear.Visible == true)
                 Header_year = "Year : "+ ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


             PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
             Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
             Hierarchy.Colspan = 2;
             Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
             Headertable.AddCell(Hierarchy);*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Depart != "**") Dept = /*"Dept : " + */Depart;
            if (Program != "**") Prg = /*"Program : " +*/ Program;
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "   " + Header_year, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }
            else
            {
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            PdfPCell R1 = new PdfPCell(new Phrase("  " + "Sequence" /*+((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Text.Substring(4,((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Text.Length-4).ToString().Trim()*/, TableFont));
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Text.Substring(4, ((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Text.Length - 4).ToString().Trim(), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Component" /*+ ((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString().Trim() + "    To : " + ((Captain.Common.Utilities.ListItem)cmbCompTo.SelectedItem).Value.ToString().Trim()*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase("From: " + ((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString().Trim() + "   To: " + ((Captain.Common.Utilities.ListItem)cmbCompTo.SelectedItem).Value.ToString().Trim(), TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            //PdfPCell R3 = new PdfPCell(new Phrase(" To : " + ((Captain.Common.Utilities.ListItem)cmbCompTo.SelectedItem).Text.ToString().Trim(), TableFont));
            //R3.HorizontalAlignment = Element.ALIGN_LEFT;
            //R3.Colspan = 2;
            //R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R3);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
            //cb.BeginText();


            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
            ////cb.SetRGBColorFill(00, 00, 00);      //ss
            //cb.ShowTextAligned(100, "Program Name:", 30, 725, 0);
            //cb.ShowTextAligned(100, Privileges.Program + " - " + Privileges.PrivilegeName, 110, 725, 0);
            //cb.ShowTextAligned(100, "Run By :" + Privileges.UserID, 30, 705, 0);
            //cb.ShowTextAligned(100, "Module Desc :" + GetModuleDesc(), 30, 685, 0);
            //cb.ShowTextAligned(100, "Started : " + DateTime.Now.ToString(), 30, 665, 0);
            //cb.ShowTextAligned(100, "Report  Selection Criterion", 40, 635, 0);
            //string Header_year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Header_year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            //cb.ShowTextAligned(100, "Hierarchy : " + Agency + "-" + Depart + "-" + Program + "   " + Header_year, 120, 610, 0);
            //cb.ShowTextAligned(100, "Sequence : " + ((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Text.ToString().Trim(), 120, 590, 0);
            //cb.ShowTextAligned(100, "Component From : " + ((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Text.ToString().Trim(), 120, 570, 0);
            //cb.ShowTextAligned(100, "Component To : " + ((Captain.Common.Utilities.ListItem)cmbCompTo.SelectedItem).Text.ToString(), 120, 550, 0);

            //cb.SetFontAndSize(bfTimes, 12);
            //Y_Pos = 480;    // Y =  350

            //cb.EndText();

            //cb.SetLineWidth(0.7f);
            //cb.MoveTo(30f, 638f);
            //cb.LineTo(40f, 638f);

            //cb.LineTo(30f, 638f);
            //cb.LineTo(30f, 530f);

            //cb.LineTo(30f, 530f);
            //cb.LineTo(530f, 530f);

            //cb.LineTo(530f, 638f);
            //cb.LineTo(530f, 530f);

            //cb.MoveTo(170f, 638f);
            //cb.LineTo(530f, 638f);

            //cb.Stroke();
        }

        private string GetModuleDesc()
        {
            string ModuleDesc = null;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["APPL_CODE"].ToString() == Privileges.ModuleCode)
                {
                    ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                }
            }
            return ModuleDesc;
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2102");
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
            Save_Entity.Card_2 = string.Empty;
            Save_Entity.Card_3 = string.Empty;
            Save_Entity.Module = BaseForm.BusinessModuleID;

            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        private void HSSB2102_TrackReport_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);

            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string Seq = ((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Value.ToString();
            string From = ((Captain.Common.Utilities.ListItem)cmbCompFrom.SelectedItem).Value.ToString();
            string To = ((Captain.Common.Utilities.ListItem)cmbCompTo.SelectedItem).Value.ToString();
            
            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" Sequence = \"" + Seq + "\" CompFrom = \"" + From + "\" CompTo = \"" + To + 
                             "\"  />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());


                CommonFunctions.SetComboBoxValue(cmbSeq, dr["Sequence"].ToString());
                CommonFunctions.SetComboBoxValue(cmbCompFrom, dr["CompFrom"].ToString());
                CommonFunctions.SetComboBoxValue(cmbCompTo, dr["CompTo"].ToString());

            }
        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = Agency + Depart + Program;
            //if (LookupValues.Exists(u => u.Hierarchy.Equals(HIE)))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(HIE)).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")).ToList();
            //else
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();

            return LookupValues;
        }

        private void FundHeader(PdfPTable nestedTable)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

            PdfPCell nestedHeader = new PdfPCell(new Phrase("Fund", TblFontBold));
            nestedHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
            nestedTable.AddCell(nestedHeader);

            PdfPCell nestedHeader1 = new PdfPCell(new Phrase("Intake Entry", TblFontBold));
            nestedHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            nestedTable.AddCell(nestedHeader1);

            PdfPCell nestedHeader2 = new PdfPCell(new Phrase("Entry Days", TblFontBold));
            nestedHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            nestedTable.AddCell(nestedHeader2);

            PdfPCell nestedHeader3 = new PdfPCell(new Phrase("Every Year", TblFontBold));
            nestedHeader3.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            nestedTable.AddCell(nestedHeader3);

            PdfPCell nestedHeader4 = new PdfPCell(new Phrase("Next Action", TblFontBold));
            nestedHeader4.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            nestedTable.AddCell(nestedHeader4);

            PdfPCell nestedHeader5 = new PdfPCell(new Phrase("Next Task", TblFontBold));
            nestedHeader5.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader5.Border = iTextSharp.text.Rectangle.NO_BORDER;
            nestedTable.AddCell(nestedHeader5);

            PdfPCell nestedHeader6 = new PdfPCell(new Phrase("Next Days", TblFontBold));
            nestedHeader6.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedHeader6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
            nestedTable.AddCell(nestedHeader6);
        }


    }
}