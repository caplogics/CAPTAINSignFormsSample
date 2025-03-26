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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ScoreRAMForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        //private string strAgency = string.Empty;
        //private string strDept = string.Empty;
        //private string strProgram = string.Empty;
        //private string strYear = string.Empty;
        //private string strApplNo = string.Empty;
        //private string strMode = string.Empty;
        //private string strNameFormat = string.Empty;
        //private string strVerfierFormat = string.Empty;
        public static string[] strkeys;

        public ScoreRAMForm(BaseForm baseForm, string Screen_Name, string Mat_code, string Scl_Code, string Assesment_Dt, string Scl_name)
        {
            InitializeComponent();

            _model = new CaptainModel();
            Baseform = baseForm;
            ScreenName = Screen_Name;
            Matcode = Mat_code;
            SclCode = Scl_Code;
            Assesment_Date = Assesment_Dt;
            ScaleName = Scl_name;
            //strFolderPath = "C:\\CapReports\\";
            propReportPath = _model.lookupDataAccess.GetReportPath();
            
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            On_SaveForm_Closed(PdfName, EventArgs.Empty);

            CloseForm();
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

        #endregion

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



            MATQUESTEntity Search_Entity = new MATQUESTEntity(true);
            Search_Entity.MatCode = Matcode;
            Search_Entity.SclCode = SclCode;

            List<MATQUESTEntity> Questions_List = new List<MATQUESTEntity>();
            Questions_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity, "Browse");

            MATQUESREntity Resp_entity = new MATQUESREntity(true);
            Resp_entity.MatCode = Matcode;
            Resp_entity.SclCode = SclCode;

            List<MATQUESREntity> Response_list = new List<MATQUESREntity>();
            Response_list = _model.MatrixScalesData.Browse_MATQUESR(Resp_entity, "Browse");

            MATDEFBMEntity Bench_Mark_entity = new MATDEFBMEntity(true);
            Bench_Mark_entity.MatCode = Matcode;


            List<MATDEFBMEntity> BenchM_List = new List<MATDEFBMEntity>();
            BenchM_List = _model.MatrixScalesData.Browse_MATDEFBM(Bench_Mark_entity, "Browse");

            MATOUTCEntity OutCome_Entity = new MATOUTCEntity(true);
            OutCome_Entity.MatCode = Matcode;
            OutCome_Entity.SclCode = SclCode;

            List<MATOUTCEntity> OutC_List = new List<MATOUTCEntity>();
            OutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");

            MATOUTREntity OutR_entity = new MATOUTREntity(true);
            OutR_entity.MatCode = Matcode;
            OutR_entity.SclCode = SclCode;

            List<MATOUTREntity> OutCR_list = _model.MatrixScalesData.Browse_MATOUTR(OutR_entity, "Browse");

            MATSGRPEntity Group_Entity = new MATSGRPEntity(true);
            Group_Entity.MatCode = Matcode;
            Group_Entity.SclCode = SclCode;

            List<MATSGRPEntity> Group_List = _model.MatrixScalesData.Browse_MATSGRP(Group_Entity, "Browse");

            PdfPTable Scale = new PdfPTable(1);
            Scale.TotalWidth = 750f;
            Scale.WidthPercentage = 100;
            Scale.LockedWidth = true;
            //float[] widths = new float[] { 55f, 45f, 25f, 25f, 20f, 25f, 25f, 30f, 30f, 25f, 18f, 22f, 22f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
            //Snp_Table.SetWidths(widths);
            Scale.HorizontalAlignment = Element.ALIGN_CENTER;
            //Snp_Table.SpacingBefore = 270f;

            PdfPCell Scale_Name = new PdfPCell(new Phrase(ScaleName.Trim(), helvetica));
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
            foreach (MATQUESTEntity Quesentity in Questions_List)
            {
                i = i + 1;
                PdfPCell Questions = new PdfPCell(new Phrase(i + ". " + Quesentity.Desc, Times));
                Questions.Colspan = 3;
                Questions.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Questions.Border = iTextSharp.text.Rectangle.BOX;
                TableData.AddCell(Questions);
                
                foreach (MATQUESREntity dr in Response_list)
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
                            MATASMTEntity ASMT_entity = new MATASMTEntity(true);
                            ASMT_entity.Agency = Baseform.BaseAgency.Trim(); ASMT_entity.Dept = Baseform.BaseDept.Trim(); ASMT_entity.Program = Baseform.BaseProg.Trim(); ASMT_entity.App = Baseform.BaseApplicationNo.Trim();
                            ASMT_entity.Year = Baseform.BaseYear.Trim(); ASMT_entity.MatCode = Matcode.Trim(); ASMT_entity.SclCode = SclCode.Trim(); ASMT_entity.SSDate = Assesment_Date.Trim();
                            ASMT_entity.QuesCode = dr.Code.Trim(); ASMT_entity.RespCode = dr.RespCode.Trim();
                            List<MATASMTEntity> Asmt_list = _model.MatrixScalesData.Browse_MATASMT(ASMT_entity, "Browse");
                            if (Asmt_list.Count > 0)
                            {
                                foreach (MATASMTEntity drASMT in Asmt_list)
                                {
                                    if (drASMT.QuesCode.Trim() == dr.Code.Trim() && drASMT.RespCode.Trim() == dr.RespCode.Trim() && dr.Exclude=="N")
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

            PdfPCell Total_Score = new PdfPCell(new Phrase("Total Score for Question 1 -" + Questions_List.Count, TblFontBold));
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
                PdfPCell Score_Total = new PdfPCell(new Phrase("Score Total / " + Questions_List.Count, TblFontBold));
                Score_Total.Colspan = 2;
                Score_Total.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                Score_Total.Border = iTextSharp.text.Rectangle.BOX;
                TableData.AddCell(Score_Total);
            }

            if (ScreenName == "MAT00003")
            {
                int Scr = 0;
                if (Totalscore > 0)
                    Scr = Totalscore/ QuesCnt;
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
                OutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");
                if (OutC_List.Count > 0)
                {
                    foreach (MATOUTCEntity drOutC in OutC_List)
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
            document.Add(Scale);
            if (ScreenName == "MAT00003")
                document.Add(APP_details);
            document.Add(TableData);
            document.Add(SecondTable);

            document.Close();
            fs.Close();
            fs.Dispose();

        }

        public string Get_Pdf_Path()
        {
            return PdfName;
        }

        private void CloseForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            //FrmViewer objfrm = new FrmViewer(PdfName);
            //objfrm.ShowDialog();
        }
    }
}