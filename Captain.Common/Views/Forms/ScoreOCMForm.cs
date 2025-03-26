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
    public partial class ScoreOCMForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        //private string strAgency = string.Empty;
        //private string strDept = string.Empty;
        //private string strProgram = string.Empty;
        private string strYear = string.Empty;
        private string strApplNo = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        public static string[] strkeys;
        //string strAgency, string strDept, string strProgram, string strYear, string strApplNo,
        public ScoreOCMForm(BaseForm baseForm, string Screen_Name,  string Mat_code, string Scl_Code, string Assesment_Dt,string Scl_name)
        {
            InitializeComponent();

            _model = new CaptainModel();
            Baseform = baseForm;
            ScreenName = Screen_Name;
            //Agency=strAgency;
            //Dept=strDept;
            //Program=strProgram;
            //Year=strYear;
            //ApplNo=strApplNo;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            //Name=strName;
            Matcode=Mat_code;
            SclCode=Scl_Code;
            Assesment_Date=Assesment_Dt;
            ScaleName=Scl_name;
            //strFolderPath = "C:\\CapReports\\";
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";
            On_SaveForm_Closed(PdfName, EventArgs.Empty);
            
            CloseForm();

        }

        #region properties

        public BaseForm Baseform { get; set; }

        //public PrivilegeEntity priviliges { get; set; }
        public string propReportPath { get; set; }
        public string ScreenName { get; set; }

        //public string Agency { get; set; }

        //public string Dept { get; set; }

        //public string Program { get; set; }

        //public string Year { get; set; }

        //public string ApplNo { get; set; }

        //public string Name { get; set; }

        public string Matcode { get; set; }

        public string SclCode { get; set; }

        public string Assesment_Date { get; set; }

        public string ScaleName { get; set; }

        //public bool IsSaveValid { get; set; }

        #endregion



         PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        string PdfName = "Pdf File";
        private void On_SaveForm_Closed(object sender, EventArgs e)
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

            //MATOUTREntity OutR_entity = new MATOUTREntity(true);
            //OutR_entity.MatCode = Matcode;
            //OutR_entity.SclCode = SclCode;

            //List<MATOUTREntity> OutCR_list = _model.MatrixScalesData.Browse_MATOUTR(OutR_entity, "Browse");

            MATSGRPEntity Group_Entity = new MATSGRPEntity(true);
            Group_Entity.MatCode = Matcode;
            Group_Entity.SclCode = SclCode;

            List<MATSGRPEntity> Group_List = _model.MatrixScalesData.Browse_MATSGRP(Group_Entity, "Browse");

            MATDEFEntity Main_Entity = new MATDEFEntity(true);
            Main_Entity.Mat_Code = Matcode;
            Main_Entity.Scale_Code = SclCode;
            List<MATDEFEntity> scl_List = _model.MatrixScalesData.Browse_MATDEF(Main_Entity, "Browse");
            

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
                float[] Widths = new float[] { 25f, 30f, 60f };
                TableData.SetWidths(Widths);
                TableData.HorizontalAlignment = Element.ALIGN_CENTER;
            if(ScreenName=="MAT00003")
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
                if (OutC_List.Count > 0)
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
                        OutC_List = _model.MatrixScalesData.Browse_MATOUTC(OutCome_Entity, "Browse");
                        if (OutC_List.Count > 0)
                        {
                            foreach (MATOUTCEntity drOutC in OutC_List)
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
                                    OutR_entity.SclCode = SclCode;
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
                                                foreach (MATQUESREntity QuesEntity in Response_list)
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

                iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
                iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));
                _image_UnChecked.ScalePercent(60f);
                _image_Checked.ScalePercent(60f);
                PdfPTable GroupData = new PdfPTable(2);
                GroupData.TotalWidth = 750f;
                GroupData.WidthPercentage = 100;
                GroupData.LockedWidth = true;
                float[] GroupData_Widths = new float[] { 1f, 25f};
                GroupData.SetWidths(GroupData_Widths);
                GroupData.HorizontalAlignment = Element.ALIGN_CENTER;
                GroupData.SpacingBefore = 13f;

                if (ScreenName == "MAT00002")
                {
                    foreach (MATSGRPEntity drGRP in Group_List)
                    {
                        PdfPCell Group = new PdfPCell(new Phrase(drGRP.Desc, TblFontBold));
                        Group.Colspan = 2;
                        Group.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Group.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        GroupData.AddCell(Group);

                        foreach (MATQUESTEntity drQues in Questions_List)
                        {
                            if (drGRP.Group.Trim() == drQues.Group.Trim())
                            {
                                PdfPCell Question = new PdfPCell(new Phrase("   " + drQues.Code.Trim() + ". " + drQues.Desc.Trim(), Times));
                                Question.Colspan = 2;
                                Question.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                Question.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                GroupData.AddCell(Question);

                                foreach (MATQUESREntity drQuesResp in Response_list)
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

                    PdfPCell Last_tab = new PdfPCell(new Phrase(scl_List[0].Rationale.Trim(), Times));
                    Last_tab.Colspan = 2;
                    Last_tab.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Last_tab.Border = iTextSharp.text.Rectangle.BOX;
                    GroupData.AddCell(Last_tab); 

                }
                else
                {
                    foreach (MATSGRPEntity drGRP in Group_List)
                    {
                        PdfPCell Group = new PdfPCell(new Phrase(drGRP.Desc, TblFontBold));
                        Group.Colspan = 2;
                        Group.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Group.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        GroupData.AddCell(Group);

                        foreach (MATQUESTEntity drQues in Questions_List)
                        {
                            if (drGRP.Group.Trim() == drQues.Group.Trim())
                            {
                                PdfPCell Question = new PdfPCell(new Phrase("   " + drQues.Code.Trim() + ". " + drQues.Desc.Trim(), Times));
                                Question.Colspan = 2;
                                Question.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                Question.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                GroupData.AddCell(Question);
                                string Ques_Code = null; string Priv_Ques = null;
                                MATASMTEntity ASMT_entity = new MATASMTEntity(true);
                                ASMT_entity.Agency = Baseform.BaseAgency.Trim(); ASMT_entity.Dept = Baseform.BaseDept.Trim(); ASMT_entity.Program = Baseform.BaseProg.Trim(); ASMT_entity.App = Baseform.BaseApplicationNo.Trim();
                                ASMT_entity.Year = Baseform.BaseYear.Trim(); ASMT_entity.MatCode = Matcode.Trim(); ASMT_entity.SclCode = SclCode.Trim(); ASMT_entity.SSDate = Assesment_Date.Trim();
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
                                                foreach (MATQUESREntity drQuesResp in Response_list)
                                                {
                                                    //Ques_Code = drQues.Code.Trim();


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
                                            }
                                        }
                                        Priv_Ques = Ques_Code;
                                    }
                                }
                                else
                                {
                                    foreach (MATQUESREntity drQuesResp in Response_list)
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

                    PdfPCell Last_tab = new PdfPCell(new Phrase(scl_List[0].Rationale.Trim(), Times));
                    Last_tab.Colspan = 2;
                    Last_tab.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Last_tab.Border = iTextSharp.text.Rectangle.BOX;
                    GroupData.AddCell(Last_tab); 
                    
                }
            
                    document.Add(Scale);
                    if (ScreenName == "MAT00003")
                        document.Add(APP_details);
                    document.Add(TableData);
                    document.Add(GroupData);
    
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