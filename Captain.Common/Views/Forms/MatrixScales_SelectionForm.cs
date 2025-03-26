#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
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

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
//using System.Windows.Forms;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MatrixScales_SelectionForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;

        public MatrixScales_SelectionForm(BaseForm baseForm, string Mat_code, string Screen_Name, string Assesment_Dt, string Scl_Code)
        {
            InitializeComponent();

            _model = new CaptainModel();
            Baseform = baseForm;
            ScreenName = Screen_Name;
            Matcode = Mat_code;
            SclCode = Scl_Code;
            Assesment_Date = Assesment_Dt;
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            //ScaleName = Scl_name;
            //strFolderPath = "C:\\CapReports\\";
            propReportPath = _model.lookupDataAccess.GetReportPath();
            fillcombo();
            if (propAgencyControlDetails != null)
            {
                if (propAgencyControlDetails.MatAssesment.ToUpper() == "Y" && ScreenName == "MAT00003")
                {
                    cmbAssessmentType.Visible = true;
                    lblAssType.Visible = true;
                    Size = new Size(500,480);
                }
            }
            //strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";

            if (cmbAssessmentType.Visible == false)
                this.pnlMatrix.Size = new Size(this.pnlMatrix.Width, 39);
        }

        #region properties

        public BaseForm Baseform { get; set; }

        //public PrivilegeEntity priviliges { get; set; }

        public string propReportPath { get; set; }

        public string ScreenName { get; set; }

        public string Matcode { get; set; }

        public string SclCode { get; set; }

        public string Assesment_Date { get; set; }

        public string ScaleName { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }

        #endregion

        string Img_Blank = "blank"; // string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = "icon-gridtick";// string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        public void fillcombo()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            if (matdefEntity.Count > 0)
            {
                int Rowcnt = 0, SelInd = 0; bool Isfalse = true;
                foreach (MATDEFEntity matdef in matdefEntity)
                {
                    cmbMatrix.Items.Add(new Captain.Common.Utilities.ListItem(matdef.Desc, matdef.Mat_Code, matdef.Score, string.Empty));
                    if (matdef.Active == "A" && Isfalse && matdef.Mat_Code==Matcode) { SelInd = Rowcnt; Isfalse = false; }
                    Rowcnt++;
                }
                FillAssessment_Types();
                cmbMatrix.SelectedIndex = SelInd;
            }
            else
            {
                cmbMatrix.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", string.Empty, string.Empty));
                FillAssessment_Types();
                cmbMatrix.SelectedIndex = 0;
            }

        }

        private void FillAssessment_Types()
        {
            cmbAssessmentType.Items.Clear();
            List<Captain.Common.Utilities.ListItem> ListAssesmentType = new List<Captain.Common.Utilities.ListItem>();
            this.cmbAssessmentType.SelectedIndexChanged -=new EventHandler(cmbAssessmentType_SelectedIndexChanged);
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("All", "A"));
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("Both", "B"));
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("Households", "H"));
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("Individual", "I"));
            cmbAssessmentType.Items.AddRange(ListAssesmentType.ToArray());
            cmbAssessmentType.SelectedIndex = 0;
            this.cmbAssessmentType.SelectedIndexChanged += new EventHandler(cmbAssessmentType_SelectedIndexChanged);
        }

        List<MATDEFEntity> MATDEF_List = new List<MATDEFEntity>();
        private void cmbMatrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            Matcode = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString();
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Mat_Code = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString();
            MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            Scales_Grid.Rows.Clear();

            if (MATDEF_List.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbAssessmentType.SelectedItem).Value.ToString().Trim() == "B")
                    MATDEF_List = MATDEF_List.FindAll(u => u.Assessment_Type.Equals("B"));
                if (((Captain.Common.Utilities.ListItem)cmbAssessmentType.SelectedItem).Value.ToString().Trim() == "H")
                    MATDEF_List = MATDEF_List.FindAll(u => u.Assessment_Type.Equals("H"));
                if (((Captain.Common.Utilities.ListItem)cmbAssessmentType.SelectedItem).Value.ToString().Trim() == "I")
                    MATDEF_List = MATDEF_List.FindAll(u => u.Assessment_Type.Equals("I"));

                foreach (MATDEFEntity Entity in MATDEF_List)
                {
                    if (Entity.Scale_Code != "0")
                    {
                        if(Entity.Scale_Code==SclCode)
                            Scales_Grid.Rows.Add(Img_Tick, Entity.Desc, Entity.Scale_Code, "Y",Entity.Assessment_Type);
                        else
                            Scales_Grid.Rows.Add(Img_Blank, Entity.Desc, Entity.Scale_Code, "N", Entity.Assessment_Type);
                    }
                }
            }
            if (Scales_Grid.Rows.Count > 0)
                Scales_Grid.Rows[0].Selected = true;
        }

        private void Scales_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Scales_Grid.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (Scales_Grid.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            Scales_Grid.CurrentRow.Cells["Sel_Img"].Value = Img_Blank;
                            Scales_Grid.CurrentRow.Cells["Selected"].Value = "N";
                        }
                        else
                        {
                            Scales_Grid.CurrentRow.Cells["Sel_Img"].Value = Img_Tick;
                            Scales_Grid.CurrentRow.Cells["Selected"].Value = "Y";
                        }
                    }
                }
                
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in Scales_Grid.Rows)
            {
                item.Cells["Selected"].Value = "Y";
                item.Cells["Sel_Img"].Value = Img_Tick;
            }
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        string PdfName = "Pdf File";
        private void On_SaveForm_Closed(object sender, EventArgs e)
        {
            Random_Filename = null;

            PdfName = "ScoreRAM";
            //PdfName = strFolderPath + PdfName;

            PdfName = propReportPath + Baseform.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + Baseform.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + Baseform.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bftimes = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bftimes, 16, 2, new BaseColor(0, 0, 102));
            //BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            //iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
            //BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bftimes, 10);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;

            Matcode = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString();

            MATQUESTEntity Search_Entity = new MATQUESTEntity(true);
            Search_Entity.MatCode = Matcode;
            //Search_Entity.SclCode = SclCode;

            List<MATQUESTEntity> Questions_List = new List<MATQUESTEntity>();
            Questions_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity, "Browse");

            MATQUESREntity Resp_entity = new MATQUESREntity(true);
            Resp_entity.MatCode = Matcode;
            //Resp_entity.SclCode = SclCode;

            List<MATQUESREntity> Response_list = new List<MATQUESREntity>();
            Response_list = _model.MatrixScalesData.Browse_MATQUESR(Resp_entity, "Browse");

            MATDEFBMEntity Bench_Mark_entity = new MATDEFBMEntity(true);
            Bench_Mark_entity.MatCode = Matcode;


            List<MATDEFBMEntity> BenchM_List = new List<MATDEFBMEntity>();
            BenchM_List = _model.MatrixScalesData.Browse_MATDEFBM(Bench_Mark_entity, "Browse");

            MATOUTCEntity OutCome_Entity = new MATOUTCEntity(true);
            OutCome_Entity.MatCode = Matcode;
            //OutCome_Entity.SclCode = SclCode;

            List<MATOUTCEntity> OutC_List = new List<MATOUTCEntity>();
            OutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");

            MATOUTREntity OutR_entity = new MATOUTREntity(true);
            OutR_entity.MatCode = Matcode;
            //OutR_entity.SclCode = SclCode;

            List<MATOUTREntity> OutCR_list = _model.MatrixScalesData.Browse_MATOUTR(OutR_entity, "Browse");

            MATSGRPEntity Group_Entity = new MATSGRPEntity(true);
            Group_Entity.MatCode = Matcode;
            //Group_Entity.SclCode = SclCode;

            List<MATSGRPEntity> Group_List = _model.MatrixScalesData.Browse_MATSGRP(Group_Entity, "Browse");


            bool Isfirst = true;
            foreach (DataGridViewRow dr1 in Scales_Grid.Rows)
            {
                if (dr1.Cells["Selected"].Value.ToString() == "Y")
                {
                    if (!Isfirst)
                        document.NewPage();

                    MATASMTEntity ASMT_entity = new MATASMTEntity(true);
                    List<MATASMTEntity> MATAsmt_list = new List<MATASMTEntity>();
                    if (ScreenName == "MAT00003")
                    {
                        ASMT_entity.Agency = Baseform.BaseAgency.Trim(); ASMT_entity.Dept = Baseform.BaseDept.Trim(); ASMT_entity.Program = Baseform.BaseProg.Trim(); ASMT_entity.App = Baseform.BaseApplicationNo.Trim();
                        ASMT_entity.Year = Baseform.BaseYear.Trim(); ASMT_entity.MatCode = Matcode.Trim(); ASMT_entity.SclCode = dr1.Cells["Scale_code"].Value.ToString(); ASMT_entity.SSDate = Assesment_Date.Trim();
                        MATAsmt_list = _model.MatrixScalesData.Browse_MATASMT(ASMT_entity, "Browse");

                    }

                    if (ScreenName != "MAT00003" || MATAsmt_list.Count==0)
                    {

                        PdfPTable Scale = new PdfPTable(1);
                        Scale.TotalWidth = 750f;
                        Scale.WidthPercentage = 100;
                        Scale.LockedWidth = true;
                        //float[] widths = new float[] { 55f, 45f, 25f, 25f, 20f, 25f, 25f, 30f, 30f, 25f, 18f, 22f, 22f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        //Snp_Table.SetWidths(widths);
                        Scale.HorizontalAlignment = Element.ALIGN_CENTER;
                        //Snp_Table.SpacingBefore = 270f;


                        PdfPCell Matrix_Name = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Text.ToString(), helvetica));
                        Matrix_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Matrix_Name.HorizontalAlignment = Element.ALIGN_CENTER;
                        Scale.AddCell(Matrix_Name);

                        PdfPCell Scale_Name = new PdfPCell(new Phrase(dr1.Cells["Scale_desc"].Value.ToString().Trim(), helvetica));
                        Scale_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Scale_Name.HorizontalAlignment = Element.ALIGN_CENTER;
                        Scale.AddCell(Scale_Name);

                        PdfPTable APP_details = new PdfPTable(3);
                        APP_details.TotalWidth = 750f;
                        APP_details.WidthPercentage = 100;
                        APP_details.LockedWidth = true;
                        float[] APP_details_Widths = new float[] { 30f, 40f, 30f };
                        APP_details.SetWidths(APP_details_Widths);
                        APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
                        APP_details.SpacingBefore = 9f;
                        if (ScreenName == "MAT00003")
                        {

                            PdfPCell Appl_No = new PdfPCell(new Phrase("App# :" + Baseform.BaseApplicationNo.Trim(), TblFontBold));
                            Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            APP_details.AddCell(Appl_No);

                            PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + Baseform.BaseApplicationName.Trim(), TblFontBold));
                            App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            APP_details.AddCell(App_Name);

                            PdfPCell Date = new PdfPCell(new Phrase("Date :" + Assesment_Date.ToString().Trim(), TblFontBold));
                            Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            APP_details.AddCell(Date);
                        }

                        PdfPTable TableData = new PdfPTable(3);
                        TableData.TotalWidth = 750f;
                        TableData.WidthPercentage = 100;
                        TableData.LockedWidth = true;
                        float[] Widths = new float[] { 8f, 90f, 12f };
                        TableData.SetWidths(Widths);
                        TableData.HorizontalAlignment = Element.ALIGN_CENTER;
                        if (ScreenName == "MAT00003")
                            TableData.SpacingBefore = 12f;
                        else
                            TableData.SpacingBefore = 30f;


                        PdfPCell DataHeading = new PdfPCell(new Phrase("Complete the following observatiations or ask the customer the question exactly as listed and indicate the reason that most closely matches your observation or the customers response:", TblFontBold));
                        DataHeading.Colspan = 2;
                        DataHeading.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        DataHeading.Border = iTextSharp.text.Rectangle.BOX;
                        TableData.AddCell(DataHeading);

                        PdfPCell Score_Head = new PdfPCell(new Phrase("Item Score", TblFontBold));
                        //DataHeading.Colspan = 2;
                        Score_Head.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Score_Head.Border = iTextSharp.text.Rectangle.BOX;
                        TableData.AddCell(Score_Head);
                        int i = 0; int Totalscore = 0; int QuesCnt = 0;

                        List<MATQUESTEntity> SelQuestionsList = Questions_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()));

                        List<MATQUESREntity> SelResponse_list = Response_list.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()));

                        foreach (MATQUESTEntity Quesentity in SelQuestionsList)
                        {
                            i = i + 1;
                            PdfPCell Questions = new PdfPCell(new Phrase(i + ". " + Quesentity.Desc, Times));
                            Questions.Colspan = 3;
                            Questions.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Questions.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Questions);

                            foreach (MATQUESREntity dr in SelResponse_list)
                            {
                                if (Quesentity.Code.Trim() == dr.Code.Trim())
                                {
                                    PdfPCell Resp_Points = new PdfPCell(new Phrase(dr.Points + "=", Times));
                                    Resp_Points.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                    Resp_Points.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Resp_Points);

                                    PdfPCell Resp_Desc = new PdfPCell(new Phrase(dr.RespDesc, Times));
                                    Resp_Desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    Resp_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Resp_Desc);

                                    string Points = string.Empty;
                                    if (ScreenName == "MAT00003")
                                    {
                                        //MATASMTEntity ASMT_entity = new MATASMTEntity(true);
                                        ASMT_entity.Agency = Baseform.BaseAgency.Trim(); ASMT_entity.Dept = Baseform.BaseDept.Trim(); ASMT_entity.Program = Baseform.BaseProg.Trim(); ASMT_entity.App = Baseform.BaseApplicationNo.Trim();
                                        ASMT_entity.Year = Baseform.BaseYear.Trim(); ASMT_entity.MatCode = Matcode.Trim(); ASMT_entity.SclCode = dr1.Cells["Scale_code"].Value.ToString(); ASMT_entity.SSDate = Assesment_Date.Trim();
                                        ASMT_entity.QuesCode = dr.Code.Trim(); ASMT_entity.RespCode = dr.RespCode.Trim();
                                        List<MATASMTEntity> Asmt_list = _model.MatrixScalesData.Browse_MATASMT(ASMT_entity, "Browse");
                                        if (Asmt_list.Count > 0)
                                        {
                                            foreach (MATASMTEntity drASMT in Asmt_list)
                                            {
                                                if (drASMT.QuesCode.Trim() == dr.Code.Trim() && drASMT.RespCode.Trim() == dr.RespCode.Trim() && dr.Exclude == "N")
                                                {
                                                    Points = drASMT.Points.Trim();
                                                    QuesCnt = QuesCnt + 1;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Points.Trim()))
                                    {
                                        Totalscore = Totalscore + int.Parse(Points.Trim());

                                    }
                                    PdfPCell Score = new PdfPCell(new Phrase(Points, Times));
                                    Score.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    Score.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Score);
                                }
                            }
                        }

                        PdfPCell Total_Score = new PdfPCell(new Phrase("Total Score for Question 1 -" + SelQuestionsList.Count, TblFontBold));
                        Total_Score.Colspan = 2;
                        Total_Score.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        Total_Score.Border = iTextSharp.text.Rectangle.BOX;
                        TableData.AddCell(Total_Score);

                        if (ScreenName == "MAT00003")
                        {
                            string Tot = string.Empty;
                            if (Totalscore > 0) Tot = Totalscore.ToString();
                            PdfPCell Total_Score_Value = new PdfPCell(new Phrase(Tot, TblFontBold));
                            Total_Score_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            Total_Score_Value.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Total_Score_Value);
                        }
                        else
                        {
                            PdfPCell Total_Score_Value = new PdfPCell(new Phrase("", TblFontBold));
                            Total_Score_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            Total_Score_Value.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Total_Score_Value);
                        }

                        if (ScreenName == "MAT00003")
                        {
                            PdfPCell Score_Total = new PdfPCell(new Phrase("Score Total / " + QuesCnt, TblFontBold));
                            Score_Total.Colspan = 2;
                            Score_Total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Score_Total.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Score_Total);
                        }
                        else
                        {
                            PdfPCell Score_Total = new PdfPCell(new Phrase("Score Total / " + SelQuestionsList.Count, TblFontBold));
                            Score_Total.Colspan = 2;
                            Score_Total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            Score_Total.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Score_Total);
                        }

                        if (ScreenName == "MAT00003")
                        {
                            int Scr = 0;
                            if (Totalscore > 0)
                                Scr = Totalscore / QuesCnt;
                            string ScrTot = string.Empty; if (Scr > 0) ScrTot = Scr.ToString();
                            //float Scr=0;
                            //if(Totalscore>0)
                            //    Scr = float.Parse(Totalscore.ToString()) / float.Parse(QuesCnt.ToString());
                            //string ScrTot = string.Empty; if (Scr > 0) ScrTot = Math.Round(Scr,MidpointRounding.AwayFromZero).ToString();
                            PdfPCell Score_Total_Value = new PdfPCell(new Phrase(ScrTot, TblFontBold));
                            Score_Total_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            Score_Total_Value.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Score_Total_Value);
                        }
                        else
                        {
                            PdfPCell Score_Total_Value = new PdfPCell(new Phrase("", TblFontBold));
                            Score_Total_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            Score_Total_Value.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(Score_Total_Value);
                        }

                        PdfPTable SecondTable = new PdfPTable(3);
                        SecondTable.TotalWidth = 750f;
                        SecondTable.WidthPercentage = 100;
                        SecondTable.LockedWidth = true;
                        float[] SecondTable_Widths = new float[] { 18f, 80f, 12f };
                        SecondTable.SetWidths(SecondTable_Widths);
                        SecondTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        SecondTable.SpacingBefore = 30f;

                        PdfPCell Area = new PdfPCell(new Phrase("Area", TblFontBold));
                        //Area.Colspan = 2;
                        Area.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Area.Border = iTextSharp.text.Rectangle.BOX;
                        SecondTable.AddCell(Area);

                        PdfPCell Transfer = new PdfPCell(new Phrase("Transfer Score to corresponding Area of Matrix/Continuum", TblFontBold));
                        //DataHeading.Colspan = 2;
                        Transfer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Transfer.Border = iTextSharp.text.Rectangle.BOX;
                        SecondTable.AddCell(Transfer);

                        PdfPCell Score_Second = new PdfPCell(new Phrase("Score", TblFontBold));
                        //DataHeading.Colspan = 2;
                        Score_Second.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Score_Second.Border = iTextSharp.text.Rectangle.BOX;
                        SecondTable.AddCell(Score_Second);


                        foreach (MATDEFBMEntity drBM in BenchM_List)
                        {
                            PdfPCell BenchMarks = new PdfPCell(new Phrase(drBM.Desc.Trim() + " (" + drBM.Low.Trim() + "-" + drBM.High.Trim() + ")", Times));
                            //BenchMarks.Colspan = 3;
                            BenchMarks.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            BenchMarks.Border = iTextSharp.text.Rectangle.BOX;
                            SecondTable.AddCell(BenchMarks);
                            int j = 0;
                            OutCome_Entity.BmCode = drBM.Code.Trim();
                            List<MATOUTCEntity> SelOutC_List = OutC_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()) && u.BmCode.Equals(drBM.Code.Trim()));
                            //SelOutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");
                            if (SelOutC_List.Count > 0)
                            {
                                foreach (MATOUTCEntity drOutC in SelOutC_List)
                                {

                                    if (drBM.Code.ToString().Trim() == drOutC.BmCode.ToString().Trim())
                                    {
                                        j = j + 1;
                                        if (j > 1)
                                        {
                                            PdfPCell Space = new PdfPCell(new Phrase("", Times));
                                            Space.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                            Space.Border = iTextSharp.text.Rectangle.BOX;
                                            SecondTable.AddCell(Space);
                                        }
                                        PdfPCell OutC_Desc = new PdfPCell(new Phrase("(" + drOutC.Points.Trim() + ") " + drOutC.Desc.Trim(), Times));
                                        OutC_Desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                        OutC_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                        SecondTable.AddCell(OutC_Desc);

                                        PdfPCell OutScore = new PdfPCell(new Phrase("", Times));
                                        OutScore.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                        OutScore.Border = iTextSharp.text.Rectangle.BOX;
                                        SecondTable.AddCell(OutScore);
                                    }
                                }
                            }
                            else
                            {
                                PdfPCell Space_1 = new PdfPCell(new Phrase("", Times));
                                //Space_1.Colspan = 2;
                                Space_1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                Space_1.Border = iTextSharp.text.Rectangle.BOX;
                                SecondTable.AddCell(Space_1);

                                PdfPCell Space_2 = new PdfPCell(new Phrase("", Times));
                                Space_2.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                Space_2.Border = iTextSharp.text.Rectangle.BOX;
                                SecondTable.AddCell(Space_2);
                            }


                        }
                        if (Scale.Rows.Count > 0)
                        {
                            document.Add(Scale);
                            Scale.DeleteBodyRows();
                        }
                        if (ScreenName == "MAT00003")
                        {
                            if (APP_details.Rows.Count > 0)
                            {
                                document.Add(APP_details);
                                APP_details.DeleteBodyRows();
                            }
                        }

                        if (TableData.Rows.Count > 0) { document.Add(TableData); TableData.DeleteBodyRows(); }
                        if (SecondTable.Rows.Count > 0) { document.Add(SecondTable); SecondTable.DeleteBodyRows(); }

                    }
                    else if (ScreenName == "MAT00003")
                    {
                        var distSeq = MATAsmt_list.Select(u => u.FamSeq).Distinct().ToList();
                        if (dr1.Cells["ScaleAssType"].Value.ToString() == "I")
                                distSeq.Remove("0");
                        else if (dr1.Cells["ScaleAssType"].Value.ToString() == "H")
                        {
                            if (distSeq.Contains("0")) { distSeq.Clear(); distSeq.Add("0"); } else distSeq.Clear();
                        }
                        else if (dr1.Cells["ScaleAssType"].Value.ToString() == "B")
                        {
                            //if (distSeq.Contains("0")) { distSeq.Clear(); distSeq.Add("0"); } else distSeq.Clear();
                        }
                        else
                        {
                            if (distSeq.Contains("0")) { distSeq.Clear(); distSeq.Add("0"); } else distSeq.Clear();
                        }
                            
                        if (distSeq.Count > 0)
                        {
                           

                            bool IsFalse = true;
                            foreach (var Famseq in distSeq)
                            {
                                if (!IsFalse)
                                    document.NewPage();

                                PdfPTable Scale = new PdfPTable(1);
                                Scale.TotalWidth = 750f;
                                Scale.WidthPercentage = 100;
                                Scale.LockedWidth = true;
                                //float[] widths = new float[] { 55f, 45f, 25f, 25f, 20f, 25f, 25f, 30f, 30f, 25f, 18f, 22f, 22f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                //Snp_Table.SetWidths(widths);
                                Scale.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Snp_Table.SpacingBefore = 270f;


                                PdfPCell Matrix_Name = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Text.ToString(), helvetica));
                                Matrix_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Matrix_Name.HorizontalAlignment = Element.ALIGN_CENTER;
                                Scale.AddCell(Matrix_Name);

                                PdfPCell Scale_Name = new PdfPCell(new Phrase(dr1.Cells["Scale_desc"].Value.ToString().Trim(), helvetica));
                                Scale_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Scale_Name.HorizontalAlignment = Element.ALIGN_CENTER;
                                Scale.AddCell(Scale_Name);

                                PdfPTable APP_details = new PdfPTable(3);
                                APP_details.TotalWidth = 750f;
                                APP_details.WidthPercentage = 100;
                                APP_details.LockedWidth = true;
                                float[] APP_details_Widths = new float[] { 30f, 40f, 30f };
                                APP_details.SetWidths(APP_details_Widths);
                                APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
                                APP_details.SpacingBefore = 9f;
                                if (ScreenName == "MAT00003")
                                {

                                    PdfPCell Appl_No = new PdfPCell(new Phrase("App# :" + Baseform.BaseApplicationNo.Trim(), TblFontBold));
                                    Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    APP_details.AddCell(Appl_No);

                                    if (Famseq == Baseform.BaseCaseMstListEntity[0].FamilySeq || Famseq=="0")
                                    {
                                        if (Famseq == "0")
                                        {
                                            if (propAgencyControlDetails.MatAssesment.ToUpper() == "Y")
                                            {
                                                PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + Baseform.BaseApplicationName.Trim() + " (Household)", TblFontBold));
                                                App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                                App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                APP_details.AddCell(App_Name);
                                            }
                                            else
                                            {
                                                PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + Baseform.BaseApplicationName.Trim(), TblFontBold));
                                                App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                                App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                APP_details.AddCell(App_Name);
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + Baseform.BaseApplicationName.Trim(), TblFontBold));
                                            App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            APP_details.AddCell(App_Name);
                                        }
                                    }
                                    else
                                    {
                                        string strName = string.Empty;
                                        CaseSnpEntity casesnpName = Baseform.BaseCaseSnpEntity.Find(u => u.FamilySeq.ToString().Trim() == Famseq);
                                        if (casesnpName != null)
                                            strName = LookupDataAccess.GetMemberName(casesnpName.NameixFi, casesnpName.NameixMi, casesnpName.NameixLast, Baseform.BaseHierarchyCnFormat);

                                        PdfPCell App_Name = new PdfPCell(new Phrase("Mem Name :" + strName, TblFontBold));
                                        App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        APP_details.AddCell(App_Name);
                                    }

                                    PdfPCell Date = new PdfPCell(new Phrase("Date :" + Assesment_Date.ToString().Trim(), TblFontBold));
                                    Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                    Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    APP_details.AddCell(Date);
                                }

                                PdfPTable TableData = new PdfPTable(3);
                                TableData.TotalWidth = 750f;
                                TableData.WidthPercentage = 100;
                                TableData.LockedWidth = true;
                                float[] Widths = new float[] { 8f, 90f, 12f };
                                TableData.SetWidths(Widths);
                                TableData.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (ScreenName == "MAT00003")
                                    TableData.SpacingBefore = 12f;
                                else
                                    TableData.SpacingBefore = 30f;


                                PdfPCell DataHeading = new PdfPCell(new Phrase("Complete the following observatiations or ask the customer the question exactly as listed and indicate the reason that most closely matches your observation or the customers response:", TblFontBold));
                                DataHeading.Colspan = 2;
                                DataHeading.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                DataHeading.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(DataHeading);

                                PdfPCell Score_Head = new PdfPCell(new Phrase("Item Score", TblFontBold));
                                //DataHeading.Colspan = 2;
                                Score_Head.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                Score_Head.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(Score_Head);
                                int i = 0; int Totalscore = 0; int QuesCnt = 0;

                                List<MATQUESTEntity> SelQuestionsList = Questions_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()));

                                List<MATQUESREntity> SelResponse_list = Response_list.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()));

                                foreach (MATQUESTEntity Quesentity in SelQuestionsList)
                                {
                                    i = i + 1;
                                    PdfPCell Questions = new PdfPCell(new Phrase(i + ". " + Quesentity.Desc, Times));
                                    Questions.Colspan = 3;
                                    Questions.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    Questions.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Questions);

                                    foreach (MATQUESREntity dr in SelResponse_list)
                                    {
                                        if (Quesentity.Code.Trim() == dr.Code.Trim())
                                        {
                                            PdfPCell Resp_Points = new PdfPCell(new Phrase(dr.Points + "=", Times));
                                            Resp_Points.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                            Resp_Points.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(Resp_Points);

                                            PdfPCell Resp_Desc = new PdfPCell(new Phrase(dr.RespDesc, Times));
                                            Resp_Desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            Resp_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(Resp_Desc);

                                            string Points = string.Empty;
                                            if (ScreenName == "MAT00003")
                                            {
                                                //MATASMTEntity ASMT_entity = new MATASMTEntity(true);
                                                //ASMT_entity.Agency = Baseform.BaseAgency.Trim(); ASMT_entity.Dept = Baseform.BaseDept.Trim(); ASMT_entity.Program = Baseform.BaseProg.Trim(); ASMT_entity.App = Baseform.BaseApplicationNo.Trim();
                                                //ASMT_entity.Year = Baseform.BaseYear.Trim(); ASMT_entity.MatCode = Matcode.Trim(); ASMT_entity.SclCode = SclCode.Trim(); ASMT_entity.SSDate = Assesment_Date.Trim();
                                                //ASMT_entity.QuesCode = dr.Code.Trim(); ASMT_entity.RespCode = dr.RespCode.Trim();
                                                //List<MATASMTEntity> Asmt_list = _model.MatrixScalesData.Browse_MATASMT(ASMT_entity, "Browse");
                                                List<MATASMTEntity> Asmt_list = MATAsmt_list.FindAll(u => u.App.Equals(Baseform.BaseApplicationNo) && u.FamSeq.Equals(Famseq));
                                                if (Asmt_list.Count > 0)
                                                {
                                                    foreach (MATASMTEntity drASMT in Asmt_list)
                                                    {
                                                        if (drASMT.QuesCode.Trim() == dr.Code.Trim() && drASMT.RespCode.Trim() == dr.RespCode.Trim() && dr.Exclude == "N")
                                                        {
                                                            Points = drASMT.Points.Trim();
                                                            QuesCnt = QuesCnt + 1;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(Points.Trim()))
                                            {
                                                Totalscore = Totalscore + int.Parse(Points.Trim());

                                            }
                                            PdfPCell Score = new PdfPCell(new Phrase(Points, Times));
                                            Score.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            Score.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(Score);
                                        }
                                    }
                                }

                                PdfPCell Total_Score = new PdfPCell(new Phrase("Total Score for Question 1 -" + SelQuestionsList.Count, TblFontBold));
                                Total_Score.Colspan = 2;
                                Total_Score.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                Total_Score.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(Total_Score);

                                if (ScreenName == "MAT00003")
                                {
                                    string Tot = string.Empty;
                                    if (Totalscore > 0) Tot = Totalscore.ToString();
                                    PdfPCell Total_Score_Value = new PdfPCell(new Phrase(Tot, TblFontBold));
                                    Total_Score_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    Total_Score_Value.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Total_Score_Value);
                                }
                                else
                                {
                                    PdfPCell Total_Score_Value = new PdfPCell(new Phrase("", TblFontBold));
                                    Total_Score_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    Total_Score_Value.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Total_Score_Value);
                                }

                                if (ScreenName == "MAT00003")
                                {
                                    PdfPCell Score_Total = new PdfPCell(new Phrase("Score Total / " + QuesCnt, TblFontBold));
                                    Score_Total.Colspan = 2;
                                    Score_Total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                    Score_Total.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Score_Total);
                                }
                                else
                                {
                                    PdfPCell Score_Total = new PdfPCell(new Phrase("Score Total / " + SelQuestionsList.Count, TblFontBold));
                                    Score_Total.Colspan = 2;
                                    Score_Total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                    Score_Total.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Score_Total);
                                }

                                if (ScreenName == "MAT00003")
                                {
                                    int Scr = 0;
                                    if (Totalscore > 0)
                                        Scr = Totalscore / QuesCnt;
                                    string ScrTot = string.Empty; if (Scr > 0) ScrTot = Scr.ToString();
                                    //float Scr=0;
                                    //if(Totalscore>0)
                                    //    Scr = float.Parse(Totalscore.ToString()) / float.Parse(QuesCnt.ToString());
                                    //string ScrTot = string.Empty; if (Scr > 0) ScrTot = Math.Round(Scr,MidpointRounding.AwayFromZero).ToString();
                                    PdfPCell Score_Total_Value = new PdfPCell(new Phrase(ScrTot, TblFontBold));
                                    Score_Total_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    Score_Total_Value.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Score_Total_Value);
                                }
                                else
                                {
                                    PdfPCell Score_Total_Value = new PdfPCell(new Phrase("", TblFontBold));
                                    Score_Total_Value.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    Score_Total_Value.Border = iTextSharp.text.Rectangle.BOX;
                                    TableData.AddCell(Score_Total_Value);
                                }

                                PdfPTable SecondTable = new PdfPTable(3);
                                SecondTable.TotalWidth = 750f;
                                SecondTable.WidthPercentage = 100;
                                SecondTable.LockedWidth = true;
                                float[] SecondTable_Widths = new float[] { 18f, 80f, 12f };
                                SecondTable.SetWidths(SecondTable_Widths);
                                SecondTable.HorizontalAlignment = Element.ALIGN_CENTER;
                                SecondTable.SpacingBefore = 30f;

                                PdfPCell Area = new PdfPCell(new Phrase("Area", TblFontBold));
                                //Area.Colspan = 2;
                                Area.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                Area.Border = iTextSharp.text.Rectangle.BOX;
                                SecondTable.AddCell(Area);

                                PdfPCell Transfer = new PdfPCell(new Phrase("Transfer Score to corresponding Area of Matrix/Continuum", TblFontBold));
                                //DataHeading.Colspan = 2;
                                Transfer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                Transfer.Border = iTextSharp.text.Rectangle.BOX;
                                SecondTable.AddCell(Transfer);

                                PdfPCell Score_Second = new PdfPCell(new Phrase("Score", TblFontBold));
                                //DataHeading.Colspan = 2;
                                Score_Second.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                Score_Second.Border = iTextSharp.text.Rectangle.BOX;
                                SecondTable.AddCell(Score_Second);


                                foreach (MATDEFBMEntity drBM in BenchM_List)
                                {
                                    PdfPCell BenchMarks = new PdfPCell(new Phrase(drBM.Desc.Trim() + " (" + drBM.Low.Trim() + "-" + drBM.High.Trim() + ")", Times));
                                    //BenchMarks.Colspan = 3;
                                    BenchMarks.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    BenchMarks.Border = iTextSharp.text.Rectangle.BOX;
                                    SecondTable.AddCell(BenchMarks);
                                    int j = 0;
                                    OutCome_Entity.BmCode = drBM.Code.Trim();
                                    List<MATOUTCEntity> SelOutC_List = OutC_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()) && u.BmCode.Equals(drBM.Code.Trim()));
                                    //SelOutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");
                                    if (SelOutC_List.Count > 0)
                                    {
                                        foreach (MATOUTCEntity drOutC in SelOutC_List)
                                        {

                                            if (drBM.Code.ToString().Trim() == drOutC.BmCode.ToString().Trim())
                                            {
                                                j = j + 1;
                                                if (j > 1)
                                                {
                                                    PdfPCell Space = new PdfPCell(new Phrase("", Times));
                                                    Space.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                                    Space.Border = iTextSharp.text.Rectangle.BOX;
                                                    SecondTable.AddCell(Space);
                                                }
                                                PdfPCell OutC_Desc = new PdfPCell(new Phrase("(" + drOutC.Points.Trim() + ") " + drOutC.Desc.Trim(), Times));
                                                OutC_Desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                OutC_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                                SecondTable.AddCell(OutC_Desc);

                                                PdfPCell OutScore = new PdfPCell(new Phrase("", Times));
                                                OutScore.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                OutScore.Border = iTextSharp.text.Rectangle.BOX;
                                                SecondTable.AddCell(OutScore);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell Space_1 = new PdfPCell(new Phrase("", Times));
                                        //Space_1.Colspan = 2;
                                        Space_1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                        Space_1.Border = iTextSharp.text.Rectangle.BOX;
                                        SecondTable.AddCell(Space_1);

                                        PdfPCell Space_2 = new PdfPCell(new Phrase("", Times));
                                        Space_2.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                        Space_2.Border = iTextSharp.text.Rectangle.BOX;
                                        SecondTable.AddCell(Space_2);
                                    }


                                }
                                if (Scale.Rows.Count > 0)
                                {
                                    document.Add(Scale);
                                    Scale.DeleteBodyRows();
                                }
                                if (ScreenName == "MAT00003")
                                {
                                    if (APP_details.Rows.Count > 0)
                                    {
                                        document.Add(APP_details);
                                        APP_details.DeleteBodyRows();
                                    }
                                }

                                if (TableData.Rows.Count > 0) { document.Add(TableData); TableData.DeleteBodyRows(); }
                                if (SecondTable.Rows.Count > 0) { document.Add(SecondTable); SecondTable.DeleteBodyRows(); }

                                IsFalse = false;
                            }
                        }
                        
                        
                    }

                    
                    Isfirst = false;
                }
            }

            

            //PdfPTable APP_details = new PdfPTable(3);
            //APP_details.TotalWidth = 750f;
            //APP_details.WidthPercentage = 100;
            //APP_details.LockedWidth = true;
            //float[] APP_details_Widths = new float[] { 30f, 40f, 30f };
            //APP_details.SetWidths(APP_details_Widths);
            //APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
            //APP_details.SpacingBefore = 9f;
            //if (ScreenName == "MAT00003")
            //{

            //    PdfPCell Appl_No = new PdfPCell(new Phrase("App# :" + Baseform.BaseApplicationNo.Trim(), TblFontBold));
            //    Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //    Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    APP_details.AddCell(Appl_No);

            //    PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + Baseform.BaseApplicationName.Trim(), TblFontBold));
            //    App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //    App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    APP_details.AddCell(App_Name);

            //    PdfPCell Date = new PdfPCell(new Phrase("Date :" + Assesment_Date.ToString().Trim(), TblFontBold));
            //    Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //    Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    APP_details.AddCell(Date);
            //}

           

            document.Close();
            fs.Close();
            fs.Dispose();

        }

        private void btnGenPdf_Click(object sender, EventArgs e)
        {
            if (Scales_Grid.Rows.Count > 0)
            {
                string ISScale = "N";
                foreach (DataGridViewRow dr1 in Scales_Grid.Rows)
                {
                    if (dr1.Cells["Selected"].Value.ToString() == "Y")
                    {
                        ISScale = "Y"; break;
                    }
                }

                if (ISScale == "Y")
                {

                    if (((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).ID.ToString() == "R")
                    {
                        On_SaveForm_Closed(PdfName, EventArgs.Empty);
                        if (Baseform.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
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
                    }
                    else
                    {
                        On_OCMForm_Closed(PdfName, EventArgs.Empty);

                        if (Baseform.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
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
                    }
                }
                else
                    AlertBox.Show("Select atleast one Scale", MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
           System.IO.File.Delete(PdfName);
           
        }

        private void On_OCMForm_Closed(object sender, EventArgs e)
        {
            Random_Filename = null;

            PdfName = "ScoreOCM";
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + Baseform.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + Baseform.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + Baseform.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bftimes = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bftimes, 16, 2, new iTextSharp.text.BaseColor(0, 0, 102));
            //BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            //iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
            //BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bftimes, 10);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;



            MATQUESTEntity Search_Entity = new MATQUESTEntity(true);
            Search_Entity.MatCode = Matcode;
            //Search_Entity.SclCode = SclCode;

            List<MATQUESTEntity> Questions_List = new List<MATQUESTEntity>();
            Questions_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity, "Browse");

            MATQUESREntity Resp_entity = new MATQUESREntity(true);
            Resp_entity.MatCode = Matcode;
            //Resp_entity.SclCode = SclCode;

            List<MATQUESREntity> Response_list = new List<MATQUESREntity>();
            Response_list = _model.MatrixScalesData.Browse_MATQUESR(Resp_entity, "Browse");

            MATDEFBMEntity Bench_Mark_entity = new MATDEFBMEntity(true);
            Bench_Mark_entity.MatCode = Matcode;


            List<MATDEFBMEntity> BenchM_List = new List<MATDEFBMEntity>();
            BenchM_List = _model.MatrixScalesData.Browse_MATDEFBM(Bench_Mark_entity, "Browse");

            MATOUTCEntity OutCome_Entity = new MATOUTCEntity(true);
            OutCome_Entity.MatCode = Matcode;
            //OutCome_Entity.SclCode = SclCode;

            List<MATOUTCEntity> OutC_List = new List<MATOUTCEntity>();
            OutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");

            //MATOUTREntity OutR_entity = new MATOUTREntity(true);
            //OutR_entity.MatCode = Matcode;
            //OutR_entity.SclCode = SclCode;

            //List<MATOUTREntity> OutCR_list = _model.MatrixScalesData.Browse_MATOUTR(OutR_entity, "Browse");

            MATSGRPEntity Group_Entity = new MATSGRPEntity(true);
            Group_Entity.MatCode = Matcode;
            //Group_Entity.SclCode = SclCode;

            List<MATSGRPEntity> Group_List = _model.MatrixScalesData.Browse_MATSGRP(Group_Entity, "Browse");

            MATDEFEntity Main_Entity = new MATDEFEntity(true);
            Main_Entity.Mat_Code = Matcode;
            //Main_Entity.Scale_Code = SclCode;
            List<MATDEFEntity> scl_List = _model.MatrixScalesData.Browse_MATDEF(Main_Entity, "Browse");
            bool Isfirst = true;
            foreach (DataGridViewRow dr1 in Scales_Grid.Rows)
            {
                if (dr1.Cells["Selected"].Value.ToString() == "Y")
                {

                    List<MATDEFEntity> selscl_List = scl_List.FindAll(u => u.Scale_Code.Equals(dr1.Cells["Scale_Code"].Value.ToString()));

                    if (!Isfirst)
                        document.NewPage();

                    PdfPTable Scale = new PdfPTable(1);
                    Scale.TotalWidth = 750f;
                    Scale.WidthPercentage = 100;
                    Scale.LockedWidth = true;
                    //float[] widths = new float[] { 55f, 45f, 25f, 25f, 20f, 25f, 25f, 30f, 30f, 25f, 18f, 22f, 22f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    //Snp_Table.SetWidths(widths);
                    Scale.HorizontalAlignment = Element.ALIGN_CENTER;
                    //Snp_Table.SpacingBefore = 270f;

                    PdfPCell Scale_Name = new PdfPCell(new Phrase(dr1.Cells["Scale_Desc"].Value.ToString(), helvetica));
                    Scale_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Scale_Name.HorizontalAlignment = Element.ALIGN_CENTER;
                    Scale.AddCell(Scale_Name);

                    PdfPTable APP_details = new PdfPTable(3);
                    APP_details.TotalWidth = 750f;
                    APP_details.WidthPercentage = 100;
                    APP_details.LockedWidth = true;
                    float[] APP_details_Widths = new float[] { 30f, 40f, 30f };
                    APP_details.SetWidths(APP_details_Widths);
                    APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
                    APP_details.SpacingBefore = 9f;
                    if (ScreenName == "MAT00003")
                    {

                        PdfPCell Appl_No = new PdfPCell(new Phrase("App# :" + Baseform.BaseApplicationNo.Trim(), TblFontBold));
                        Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        APP_details.AddCell(Appl_No);

                        PdfPCell App_Name = new PdfPCell(new Phrase("App Name :" + Baseform.BaseApplicationName.Trim(), TblFontBold));
                        App_Name.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        APP_details.AddCell(App_Name);

                        PdfPCell Date = new PdfPCell(new Phrase("Date :" + Assesment_Date.ToString().Trim(), TblFontBold));
                        Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        APP_details.AddCell(Date);
                    }

                    PdfPTable TableData = new PdfPTable(3);
                    TableData.TotalWidth = 750f;
                    TableData.WidthPercentage = 100;
                    TableData.LockedWidth = true;
                    float[] Widths = new float[] { 25f, 30f, 60f };
                    TableData.SetWidths(Widths);
                    TableData.HorizontalAlignment = Element.ALIGN_CENTER;
                    if (ScreenName == "MAT00003")
                        TableData.SpacingBefore = 12f;
                    else
                        TableData.SpacingBefore = 18f;

                    PdfPCell Benchmark = new PdfPCell(new Phrase("Benchmark", TblFontBold));
                    Benchmark.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Benchmark.Border = iTextSharp.text.Rectangle.BOX;
                    TableData.AddCell(Benchmark);

                    PdfPCell Score = new PdfPCell(new Phrase("Score Sheet", TblFontBold));
                    Score.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Score.Border = iTextSharp.text.Rectangle.BOX;
                    TableData.AddCell(Score);

                    PdfPCell Outcome = new PdfPCell(new Phrase("Outcome", TblFontBold));
                    Outcome.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Outcome.Border = iTextSharp.text.Rectangle.BOX;
                    TableData.AddCell(Outcome);

                    List<MATOUTCEntity> SelOutC_List = OutC_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_code"].Value.ToString()));

                    if (SelOutC_List.Count > 0)
                    {
                        foreach (MATDEFBMEntity drBM in BenchM_List)
                        {
                            PdfPCell BenchMarks = new PdfPCell(new Phrase(drBM.Desc.Trim() + " (" + drBM.Low.Trim() + "-" + drBM.High.Trim() + ")", Times));
                            //BenchMarks.Colspan = 3;
                            BenchMarks.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            BenchMarks.Border = iTextSharp.text.Rectangle.BOX;
                            TableData.AddCell(BenchMarks);
                            int j = 0;
                            OutCome_Entity.BmCode = drBM.Code.Trim();
                            //OutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");
                            List<MATOUTCEntity> SSelOutC_List = new List<MATOUTCEntity>();
                            SSelOutC_List = SelOutC_List.FindAll(u => u.BmCode.Equals(drBM.Code.Trim()));

                            if (SSelOutC_List.Count > 0)
                            {
                                foreach (MATOUTCEntity drOutC in SSelOutC_List)
                                {
                                    string OUTC_Desc = null, Out_Cond = null;
                                    if (drBM.Code.Trim() == drOutC.BmCode.Trim())
                                    {
                                        OUTC_Desc = drOutC.Desc.Trim();
                                        if (!string.IsNullOrEmpty(drOutC.Condition.Trim()) && drOutC.Condition.Trim() == "A")
                                            Out_Cond = "AND";
                                        else if (!string.IsNullOrEmpty(drOutC.Condition.Trim()) && drOutC.Condition.Trim() == "O")
                                            Out_Cond = "OR";
                                        else
                                            Out_Cond = "";
                                        MATOUTREntity OutR_entity = new MATOUTREntity(true);
                                        OutR_entity.MatCode = Matcode;
                                        OutR_entity.SclCode = dr1.Cells["Scale_Code"].Value.ToString();
                                        OutR_entity.BmCode = drBM.Code.Trim();

                                        List<MATOUTREntity> OutCR_list = _model.MatrixScalesData.Browse_MATOUTR(OutR_entity, "Browse");
                                        if (OutCR_list.Count > 0)
                                        {
                                            foreach (MATOUTREntity drOUTR in OutCR_list)
                                            {
                                                string OUT_Ques_Code = null;
                                                if (drOUTR.Code.Trim() == drOutC.Code.Trim()) //drBM.Code.Trim() == drOUTR.BmCode.Trim() &&
                                                {
                                                    OUT_Ques_Code = drOUTR.Question.Trim();
                                                    int y = 0;
                                                    List<MATQUESREntity> SelResponse_list = Response_list.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_Code"].Value.ToString()));

                                                    foreach (MATQUESREntity QuesEntity in SelResponse_list)
                                                    {
                                                        if (QuesEntity.Code.Trim() == drOUTR.Question.Trim() && drOUTR.RespCode.Trim() == QuesEntity.RespCode.Trim())
                                                        {
                                                            y = y + 1; j = j + 1;

                                                            if (j > 1)
                                                            {
                                                                PdfPCell Space_Bench = new PdfPCell(new Phrase("", Times));
                                                                Space_Bench.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                                                Space_Bench.Border = iTextSharp.text.Rectangle.BOX;
                                                                TableData.AddCell(Space_Bench);
                                                            }
                                                            PdfPCell Out_Resp = new PdfPCell(new Phrase(drOUTR.Question.Trim() + "=" + QuesEntity.RespDesc.Trim() + " " + Out_Cond.Trim(), Times));
                                                            Out_Resp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                                            Out_Resp.Border = iTextSharp.text.Rectangle.BOX;
                                                            TableData.AddCell(Out_Resp);

                                                            if (y > 1)
                                                            {
                                                                PdfPCell Space = new PdfPCell(new Phrase("", Times));
                                                                Space.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                                                Space.Border = iTextSharp.text.Rectangle.BOX;
                                                                TableData.AddCell(Space);
                                                            }
                                                            else
                                                            {
                                                                PdfPCell Out_Cm = new PdfPCell(new Phrase(OUTC_Desc.Trim() + "(" + drOutC.Points.Trim() + ")", Times));
                                                                Out_Cm.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                                Out_Cm.Border = iTextSharp.text.Rectangle.BOX;
                                                                TableData.AddCell(Out_Cm);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //int y = 0;
                                            j = j + 1;
                                            if (j > 1)
                                            {
                                                PdfPCell Space_Bench = new PdfPCell(new Phrase("", Times));
                                                Space_Bench.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                                Space_Bench.Border = iTextSharp.text.Rectangle.BOX;
                                                TableData.AddCell(Space_Bench);
                                            }

                                            PdfPCell Space_1 = new PdfPCell(new Phrase("", Times));
                                            Space_1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                            Space_1.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(Space_1);

                                            PdfPCell Out_Cm = new PdfPCell(new Phrase(OUTC_Desc.Trim() + "(" + drOutC.Points.Trim() + ")", Times));
                                            Out_Cm.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            Out_Cm.Border = iTextSharp.text.Rectangle.BOX;
                                            TableData.AddCell(Out_Cm);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                PdfPCell Space_1 = new PdfPCell(new Phrase("", Times));
                                Space_1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                Space_1.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(Space_1);

                                PdfPCell Space_2 = new PdfPCell(new Phrase("", Times));
                                Space_2.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                                Space_2.Border = iTextSharp.text.Rectangle.BOX;
                                TableData.AddCell(Space_2);
                            }
                        }
                    }

                    //iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
                   // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

                    iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
                    iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));
                    _image_UnChecked.ScalePercent(60f);
                    _image_Checked.ScalePercent(60f);
                    PdfPTable GroupData = new PdfPTable(2);
                    GroupData.TotalWidth = 750f;
                    GroupData.WidthPercentage = 100;
                    GroupData.LockedWidth = true;
                    float[] GroupData_Widths = new float[] { 1f, 25f };
                    GroupData.SetWidths(GroupData_Widths);
                    GroupData.HorizontalAlignment = Element.ALIGN_CENTER;
                    GroupData.SpacingBefore = 13f;

                    List<MATSGRPEntity> SelGroup_List = Group_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_Code"].Value.ToString()));
                    List<MATQUESTEntity> SelQuestions_List = Questions_List.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_Code"].Value.ToString()));
                    if (ScreenName == "MAT00002" || ScreenName == "MAT00001")
                    {
                        

                        foreach (MATSGRPEntity drGRP in SelGroup_List)
                        {
                            PdfPCell Group = new PdfPCell(new Phrase(drGRP.Desc, TblFontBold));
                            Group.Colspan = 2;
                            Group.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Group.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            GroupData.AddCell(Group);

                            foreach (MATQUESTEntity drQues in SelQuestions_List)
                            {
                                if (drGRP.Group.Trim() == drQues.Group.Trim())
                                {
                                    PdfPCell Question = new PdfPCell(new Phrase("   " + drQues.Seq.Trim() + ". " + drQues.Desc.Trim(), Times));
                                    Question.Colspan = 2;
                                    Question.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    Question.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    GroupData.AddCell(Question);

                                    List<MATQUESREntity> SelResponse_list = Response_list.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_Code"].Value.ToString()));

                                    foreach (MATQUESREntity drQuesResp in SelResponse_list)
                                    {
                                        if (drGRP.Group.Trim() == drQuesResp.Group.Trim() && drQues.Code.Trim() == drQuesResp.Code.Trim())
                                        {
                                            PdfPCell Uncheked = new PdfPCell(_image_UnChecked);
                                            Uncheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            Uncheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //W2Cheked.FixedHeight = 15f;
                                            Uncheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            GroupData.AddCell(Uncheked);

                                            PdfPCell Question_Resp = new PdfPCell(new Phrase(drQuesResp.RespDesc, Times));
                                            Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            GroupData.AddCell(Question_Resp);
                                        }
                                    }

                                }
                            }
                        }
                        //PdfPCell Last_tab = new PdfPCell(new Phrase("This scale is written to identify a family s income compared to the federal poverty guidelines. A standard income eligibility test is used to determine placement on the Income scale.", Times));
                        //Last_tab.Colspan = 2;
                        //Last_tab.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        //Last_tab.Border = iTextSharp.text.Rectangle.BOX;
                        //GroupData.AddCell(Last_tab); scl_List[0].Rationale.Trim();

                        PdfPCell Last_tab = new PdfPCell(new Phrase(selscl_List[0].Rationale.Trim(), Times));
                        Last_tab.Colspan = 2;
                        Last_tab.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Last_tab.Border = iTextSharp.text.Rectangle.BOX;
                        GroupData.AddCell(Last_tab);

                    }
                    else
                    {
                        foreach (MATSGRPEntity drGRP in SelGroup_List)
                        {
                            PdfPCell Group = new PdfPCell(new Phrase(drGRP.Desc, TblFontBold));
                            Group.Colspan = 2;
                            Group.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            Group.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            GroupData.AddCell(Group);

                            foreach (MATQUESTEntity drQues in SelQuestions_List)
                            {
                                if (drGRP.Group.Trim() == drQues.Group.Trim())
                                {
                                    PdfPCell Question = new PdfPCell(new Phrase("   " + drQues.Seq.Trim() + ". " + drQues.Desc.Trim(), Times));
                                    Question.Colspan = 2;
                                    Question.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    Question.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    GroupData.AddCell(Question);
                                    string Ques_Code = null; string Priv_Ques = null;
                                    MATASMTEntity ASMT_entity = new MATASMTEntity(true);
                                    ASMT_entity.Agency = Baseform.BaseAgency.Trim(); ASMT_entity.Dept = Baseform.BaseDept.Trim(); ASMT_entity.Program = Baseform.BaseProg.Trim(); ASMT_entity.App = Baseform.BaseApplicationNo.Trim();
                                    ASMT_entity.Year = Baseform.BaseYear.Trim(); ASMT_entity.MatCode = Matcode.Trim(); ASMT_entity.SclCode = dr1.Cells["Scale_Code"].Value.ToString().Trim(); ASMT_entity.SSDate = Assesment_Date.Trim();
                                    ASMT_entity.QuesCode = drQues.Code.Trim();
                                    List<MATASMTEntity> Asmt_list = _model.MatrixScalesData.Browse_MATASMT(ASMT_entity, "Browse");
                                    if (Asmt_list.Count > 0)
                                    {
                                        foreach (MATASMTEntity drASMT in Asmt_list)
                                        {
                                            if (drASMT.QuesCode.Trim() == drQues.Code.Trim())
                                            {
                                                Ques_Code = drASMT.QuesCode.Trim();
                                                if (Ques_Code != Priv_Ques)
                                                {

                                                    List<MATQUESREntity> SelResponse_list = Response_list.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_Code"].Value.ToString()) && u.Code.Equals(drQues.Code.ToString().Trim()));

                                                    if (SelResponse_list.Count > 0)
                                                    {
                                                        foreach (MATQUESREntity drQuesResp in SelResponse_list)
                                                        {
                                                            //Ques_Code = drQues.Code.Trim();

                                                            if (drQues.Type.ToString() == "1" || drQues.Type.ToString() == "2" || drQues.Type.ToString() == "3")
                                                            {
                                                                if (drQues.Code.Trim() == drQuesResp.Code.Trim() && drASMT.RespCode.Trim() == drQuesResp.RespCode.Trim()) //drGRP.Group.Trim() == drQuesResp.Group.Trim() &&
                                                                {
                                                                    PdfPCell cheked = new PdfPCell(_image_Checked);
                                                                    cheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                                    cheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                                    //W2Cheked.FixedHeight = 15f;
                                                                    cheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    GroupData.AddCell(cheked);

                                                                    PdfPCell Question_Resp = new PdfPCell(new Phrase(drQuesResp.RespDesc, Times));
                                                                    Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                                    Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    GroupData.AddCell(Question_Resp);
                                                                }
                                                                else if (drGRP.Group.Trim() == drQuesResp.Group.Trim() && drQues.Code.Trim() == drQuesResp.Code.Trim())
                                                                {
                                                                    PdfPCell Uncheked = new PdfPCell(_image_UnChecked);
                                                                    Uncheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                                    Uncheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                                    //W2Cheked.FixedHeight = 15f;
                                                                    Uncheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    GroupData.AddCell(Uncheked);

                                                                    PdfPCell Question_Resp = new PdfPCell(new Phrase(drQuesResp.RespDesc, Times));
                                                                    Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                                    Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    GroupData.AddCell(Question_Resp);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PdfPCell Question_Resp0 = new PdfPCell(new Phrase("", Times));
                                                                Question_Resp0.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                                Question_Resp0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                GroupData.AddCell(Question_Resp0);

                                                                PdfPCell Question_Resp = new PdfPCell(new Phrase(drASMT.RespDesc, Times));
                                                                Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                                Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                GroupData.AddCell(Question_Resp);
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Question_Resp0 = new PdfPCell(new Phrase("", Times));
                                                        Question_Resp0.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                        Question_Resp0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        GroupData.AddCell(Question_Resp0);

                                                        PdfPCell Question_Resp = new PdfPCell(new Phrase(drASMT.RespDesc, Times));
                                                        Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                        Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        GroupData.AddCell(Question_Resp);
                                                    }

                                                }
                                            }
                                            Priv_Ques = Ques_Code;
                                        }
                                    }
                                    else
                                    {

                                        if (drQues.Type.ToString() == "1" || drQues.Type.ToString() == "2" || drQues.Type.ToString() == "3")
                                        {
                                            List<MATQUESREntity> SelResponse_list = Response_list.FindAll(u => u.SclCode.Equals(dr1.Cells["Scale_Code"].Value.ToString()));
                                            foreach (MATQUESREntity drQuesResp in SelResponse_list)
                                            {
                                                if (drGRP.Group.Trim() == drQuesResp.Group.Trim() && drQues.Code.Trim() == drQuesResp.Code.Trim())
                                                {
                                                    PdfPCell Uncheked = new PdfPCell(_image_UnChecked);
                                                    Uncheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    Uncheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    //W2Cheked.FixedHeight = 15f;
                                                    Uncheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    GroupData.AddCell(Uncheked);

                                                    PdfPCell Question_Resp = new PdfPCell(new Phrase(drQuesResp.RespDesc, Times));
                                                    Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                                    Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    GroupData.AddCell(Question_Resp);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell Question_Resp0 = new PdfPCell(new Phrase("", Times));
                                            Question_Resp0.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            Question_Resp0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            GroupData.AddCell(Question_Resp0);

                                            PdfPCell Question_Resp = new PdfPCell(new Phrase("", Times));
                                            Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                            Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            GroupData.AddCell(Question_Resp);
                                        }
                                    }

                                }

                                //    else if (drGRP.Group.Trim() == drQuesResp.Group.Trim() && drQues.Code.Trim() == drQuesResp.Code.Trim())
                                //    {
                                //        PdfPCell Uncheked = new PdfPCell(_image_UnChecked);
                                //        Uncheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                                //        Uncheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //        //W2Cheked.FixedHeight = 15f;
                                //        Uncheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        GroupData.AddCell(Uncheked);

                                //        PdfPCell Question_Resp = new PdfPCell(new Phrase(drQuesResp.RespDesc, Times));
                                //        Question_Resp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                //        Question_Resp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //        GroupData.AddCell(Question_Resp);
                                //    }
                                //}
                            }
                        }

                        //PdfPCell Last_tab = new PdfPCell(new Phrase("This scale is written to identify a family s income compared to the federal poverty guidelines. A standard income eligibility test is used to determine placement on the Income scale.", Times));
                        //Last_tab.Colspan = 2;
                        //Last_tab.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        //Last_tab.Border = iTextSharp.text.Rectangle.BOX;
                        //GroupData.AddCell(Last_tab);

                        PdfPCell Last_tab = new PdfPCell(new Phrase(selscl_List[0].Rationale.Trim(), Times));
                        Last_tab.Colspan = 2;
                        Last_tab.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Last_tab.Border = iTextSharp.text.Rectangle.BOX;
                        GroupData.AddCell(Last_tab);

                    }

                    if (Scale.Rows.Count > 0) { document.Add(Scale); Scale.DeleteBodyRows(); }
                    if (ScreenName == "MAT00003")
                    {
                        if (APP_details.Rows.Count > 0) { document.Add(APP_details); APP_details.DeleteBodyRows(); }
                    }
                    if (TableData.Rows.Count > 0) { document.Add(TableData); TableData.DeleteBodyRows(); }
                    if (GroupData.Rows.Count > 0) { document.Add(GroupData); GroupData.DeleteBodyRows(); }
                    Isfirst = false;
                }
            }

            document.Close();
            fs.Close();
            fs.Dispose();

        }

        private void btnUnSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in Scales_Grid.Rows)
            {
                item.Cells["Selected"].Value = "N";
                item.Cells["Sel_Img"].Value = Img_Blank;
            }
        }

        private void cmbAssessmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMatrix_SelectedIndexChanged(sender, e);
        }


    }
}