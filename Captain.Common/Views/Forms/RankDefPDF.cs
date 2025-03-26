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
using System.Text.RegularExpressions;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RankDefPDF : Form
    {

        private CaptainModel _model = null;
        public RankDefPDF(BaseForm baseform,PrivilegeEntity priviliges)
        {
            InitializeComponent();

            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = priviliges;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = "Rank Selection for PDF";//priviliges.Program + " - Rank Selection for PDF";
            FillCmbHie();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }
        #endregion

        private void PbPdf_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        DataSet dsRnkCrit1 = new DataSet();
        private void FillCmbRnk()
        {
            if (((RepListItem)cmbHie.SelectedItem).Value.ToString() == "*")
                dsRnkCrit1 = DatabaseLayer.SPAdminDB.GetRnkCatg(null);
            else
                dsRnkCrit1 = DatabaseLayer.SPAdminDB.GetRnkCatg(((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim());
            DataTable dtRnkCrit1 = dsRnkCrit1.Tables[0];
            List<RankCatgEntity> Ranksgrid;
            Ranksgrid = _model.SPAdminData.Browse_RankCtg();
            cmbRnk.Items.Clear();
            List<RepListItem> listItem = new List<RepListItem>();
            if (Ranksgrid.Count > 0)
            {
                cmbRnk.Items.Add(new RepListItem("All","*"));

                if (((RepListItem)cmbHie.SelectedItem).Value.ToString() != "*")
                {
                    foreach (RankCatgEntity drRank in Ranksgrid)
                    {
                        if (string.IsNullOrWhiteSpace(drRank.SubCode.ToString()) && drRank.Agency.ToString().Trim() == ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim())
                        {
                            foreach (DataRow dr in dtRnkCrit1.Rows)
                            {
                                if (dr["RNKCRIT1_RANK_CTG"].ToString().Trim() == drRank.Code.ToString().Trim())
                                {
                                    string Code = drRank.Code.ToString().Trim();
                                    string Desc = drRank.Desc.ToString().Trim();
                                    cmbRnk.Items.Add(new RepListItem(Desc, Code));
                                }
                            }
                        }
                    }
                }
            }
            cmbRnk.SelectedIndex = 0;
        }
        string Hierac = null;
        private void FillCmbHie()
        {
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once
            DataTable dt = ds.Tables[0]; 

            List<RNKCRIT1Entity> RankHieList;
            //if(((RepListItem)cmbRnk.SelectedItem).Value.ToString()=="*")
            RankHieList = _model.SPAdminData.GetRNKCRIT(null);
            //else
            //    RankHieList = _model.SPAdminData.GetRNKCRIT(((RepListItem)cmbRnk.SelectedItem).Value.ToString());
            cmbHie.Items.Clear();
            List<RepListItem> ListHie = new List<RepListItem>();
            if (RankHieList.Count > 0)
            {
                cmbHie.Items.Add(new RepListItem("All", "*"));
                foreach (RNKCRIT1Entity drRankHie in RankHieList)  
                {
                    string Hierarchy = string.Empty;
                    foreach (DataRow dr in dt.Rows)//dr["Agy"] + " - " + dr["Name"], dr["Agy"]
                    {
                        
                        if (dr["Agy"].ToString().Trim() == drRankHie.Agency.Trim())
                        {
                            Hierac = drRankHie.Agency.ToString();
                            Hierarchy = dr["Agy"] + " - " + dr["Name"].ToString();
                            break;
                        }
                        else if (drRankHie.Agency.Trim() == "**")
                        {
                            Hierac = drRankHie.Agency.ToString();
                            Hierarchy = "**" + " - " + "All Agencies";
                            break;
                        }
                    }
                    cmbHie.Items.Add(new RepListItem(Hierarchy,Hierac));
                }
            }
            cmbHie.SelectedIndex = 0;
        }


        string PdfMode = null;
        private void btnGenPdf_Click(object sender, EventArgs e)
        {
            PdfMode = "Main";
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
            pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        int pageNumber = 1;
        string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        string Hierach = null, privhireac = null, ProgName = null, ScreenName = null;
        private void On_SaveForm_Closed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Random_Filename = null;
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
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
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;
                string Priv_Scr = null;

                try
                {
                    List<RNKCRIT1Entity1> RankCritlist; List<RNKCRIT2Entity> RnkCrit2list;
                    if (((RepListItem)cmbRnk.SelectedItem).Value.ToString() == "*")
                    {
                        if (((RepListItem)cmbHie.SelectedItem).Value.ToString() == "*")
                            RankCritlist = _model.SPAdminData.Browse_RankCritiria1(null, null, null, null);
                        else
                            RankCritlist = _model.SPAdminData.Browse_RankCritiria1(((RepListItem)cmbHie.SelectedItem).Value.ToString(), null, null, null);
                    }
                    else
                    {
                        if (((RepListItem)cmbHie.SelectedItem).Value.ToString() == "*")
                            RankCritlist = _model.SPAdminData.Browse_RankCritiria1(null,  ((RepListItem)cmbRnk.SelectedItem).Value.ToString(), null, null);
                        else
                            RankCritlist = _model.SPAdminData.Browse_RankCritiria1(((RepListItem)cmbHie.SelectedItem).Value.ToString(), ((RepListItem)cmbRnk.SelectedItem).Value.ToString(), null, null);
                    }
                    bool First = true; bool Rnk_var = false;
                    bool Doc_firstPage = true;

                    DataSet dsRnkQuee = DatabaseLayer.SPAdminDB.BrowseRankQues(null);
                    DataTable dtRnkQuee = dsRnkQuee.Tables[0];
                    DataSet dsCustFlds = DatabaseLayer.SPAdminDB.BrowseCustFlds();
                    DataTable dtCustflds = dsCustFlds.Tables[0];
                    DataSet DsRnkRankCtg = Captain.DatabaseLayer.SPAdminDB.Browse_RnkCatg();
                    DataTable dtRnkRankCtg = DsRnkRankCtg.Tables[0];
                    if (RankCritlist.Count > 0)
                    {
                        foreach (RNKCRIT1Entity1 list in RankCritlist)
                        {
                            cb.BeginText();
                            Hierach = list.Agency.ToString();
                            rnkCd = list.RankCtg.ToString();
                            ScreenName = list.Scr_Cd.ToString();
                            Rnk_var = false;
                            if (Hierach != privhireac)
                                PrivrnkCd = null;

                            if (rnkCd != PrivrnkCd)
                            {
                                Rnk_var = true;
                                foreach (DataRow drrnkctg in dtRnkRankCtg.Rows)
                                {
                                    if (list.RankCtg.ToString().Trim() == drrnkctg["RANKCTG_GRPCTG_CODE"].ToString().Trim() && (string.IsNullOrWhiteSpace(drrnkctg["RANKCTG_SUBCTG_CODE"].ToString())) && list.Agency.Trim() == drrnkctg["RANKCTG_AGENCY"].ToString().Trim())
                                    {
                                        Rankdesc = drrnkctg["RANKCTG_DESC"].ToString();

                                        break;
                                    }
                                }
                                PrivrnkCd = rnkCd;
                            }

                            if (Hierach != privhireac || Rnk_var)
                            {
                                if (!Doc_firstPage)
                                {
                                    Y_Pos = 20;
                                    CheckBottomBorderReached(document, writer);
                                }
                                printHText();
                                privhireac = Hierach; First = true;
                                Doc_firstPage = false;
                            }

                            cb.EndText();
                            if (First)
                            {
                                cb.BeginText();
                                X_Pos = 60; Y_Pos = 475;
                                //cb.EndText();
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
                                First = false;
                                X_Pos = 60; Y_Pos = 440;
                            }

                            if (list.Scr_Cd.ToString() == "CUSTQUES")
                            {
                                foreach (DataRow drCustflds in dtCustflds.Rows)
                                {
                                    cb.BeginText();
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                                    string Code = drCustflds["CUST_CODE"].ToString();
                                    if (RankCritlist.Count > 0)
                                    {
                                        if (list.Fld_cd.ToString() == drCustflds["CUST_CODE"].ToString())
                                        {
                                            if (list.Scr_Cd != Priv_Scr)
                                            {
                                                //PdfScreen = list.Scr_Cd;
                                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE).BaseFont, 13);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Screen : Custom Questions", 60, Y_Pos, 0);

                                                Priv_Scr = list.Scr_Cd;
                                            }
                                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                                            X_Pos = 60; Y_Pos -= 20;
                                            CheckBottomBorderReached(document, writer);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, drCustflds["CUST_DESC"].ToString(), X_Pos, Y_Pos, 0);


                                            cb.EndText();
                                            RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                            if (RnkCrit2list.Count > 0)
                                            {
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
                                            }
                                            cb.BeginText();
                                        }
                                    }
                                    cb.EndText();
                                }
                                cb.BeginText();
                            }
                            else if (list.Scr_Cd.ToString() == "PREASSES" && list.Fld_cd.ToString().StartsWith("P"))
                            {
                                foreach (DataRow drCustflds in dtCustflds.Rows)
                                {
                                    cb.BeginText();
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                                    string Code = drCustflds["CUST_CODE"].ToString();
                                    if (RankCritlist.Count > 0)
                                    {
                                        if (list.Fld_cd.ToString() == drCustflds["CUST_CODE"].ToString())
                                        {
                                            if (list.Scr_Cd != Priv_Scr)
                                            {
                                                //PdfScreen = list.Scr_Cd;
                                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE).BaseFont, 13);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, "Screen : Pre Assessment", 60, Y_Pos, 0);

                                                Priv_Scr = list.Scr_Cd;
                                            }
                                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                                            X_Pos = 60; Y_Pos -= 20;
                                            CheckBottomBorderReached(document, writer);
                                            cb.ShowTextAligned(Element.ALIGN_LEFT, drCustflds["CUST_DESC"].ToString(), X_Pos, Y_Pos, 0);


                                            cb.EndText();
                                            RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                            if (RnkCrit2list.Count > 0)
                                            {
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
                                            }
                                            cb.BeginText();
                                        }
                                    }
                                    cb.EndText();
                                }
                                cb.BeginText();
                            }
                            else
                            {
                                foreach (DataRow drRnkQuee in dtRnkQuee.Rows)
                                {
                                    cb.BeginText();
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                                    string Code = drRnkQuee["RANKQUES_CODE"].ToString();
                                    if (RankCritlist.Count > 0)
                                    {
                                        if (drRnkQuee["RANKQUES_SCR"].ToString() == ScreenName.ToString())
                                        {
                                            if (list.Fld_cd.ToString() == drRnkQuee["RANKQUES_CODE"].ToString())
                                            {
                                                if (list.Scr_Cd != Priv_Scr)
                                                {
                                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE).BaseFont, 13);
                                                    PdfScreen = list.Scr_Cd;
                                                    if (PdfScreen == "CASE2001")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Screen : Client Intake", 60, Y_Pos, 0);
                                                    else if (PdfScreen == "CASE2330")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Screen : Medical Emergency", 60, Y_Pos, 0);
                                                    else if (PdfScreen == "PREASSES")
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Screen : Pre Assessment", 60, Y_Pos, 0);
                                                    else
                                                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Screen : Custom Questions", 60, Y_Pos, 0);
                                                    Priv_Scr = list.Scr_Cd;
                                                }
                                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                                                X_Pos = 60; Y_Pos -= 20;
                                                CheckBottomBorderReached(document, writer);
                                                cb.ShowTextAligned(Element.ALIGN_LEFT, drRnkQuee["RANKQUES_DESC"].ToString(), X_Pos, Y_Pos, 0);

                                                cb.EndText();
                                                RnkCrit2list = _model.SPAdminData.Browse_RankCritiria2(list.Id.ToString());
                                                if (RnkCrit2list.Count > 0)
                                                {
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
                                                }
                                                cb.BeginText();
                                            }
                                        }
                                    }
                                    cb.EndText();
                                }
                                cb.BeginText();
                            }
                            
                            cb.EndText();
                        }
                    }
                    else
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 14);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No records found", 200, 600, 0);
                        cb.EndText();
                    }
                    AlertBox.Show("Report Generated Successfully");
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                cb.BeginText();
                Y_Pos = 07;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.ShowTextAligned(800, DateTime.Now.ToLocalTime().ToString(), 07, 20, 0);
                Y_Pos = 07;
                X_Pos = 800;
                cb.ShowTextAligned(800, "Page:" + pageNumber.ToString(), 800, 07, 0);
                cb.EndText();
                document.Close();
                fs.Close();
                fs.Dispose();
                pageNumber = 1;
            }
        }

        private void HeadingPage()
        {
            //X_Pos = 60; Y_Pos = 460;
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
            //X_Pos = 60; Y_Pos = 475;
            cb.EndText();
            cb.SetLineWidth(0.7f);
            cb.MoveTo(X_Pos, Y_Pos);
            cb.LineTo(790, Y_Pos);
            cb.Stroke();
        }

        
        private void CheckBottomBorderReached(Document document, PdfWriter writer)
        {
            if (Y_Pos <= 20)
            {

                cb.EndText();

                cb.BeginText();
                Y_Pos = 07;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.ShowTextAligned(800, DateTime.Now.ToLocalTime().ToString(), 07, 20, 0);
                Y_Pos = 07;
                X_Pos = 800;
                cb.ShowTextAligned(800, "Page:" + pageNumber.ToString(), 800, 07, 0);
                cb.EndText();

                document.NewPage();
                pageNumber = writer.PageNumber;

                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);

                printHText();
                //pdfScreenHead();
                X_Pos = 60; Y_Pos = 475;
                SetLine();
                Y_Pos -= 14;
                cb.BeginText();
                HeadingPage();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                Y_Pos -= 5;
                SetLine();

                Y_Pos = 440;
                X_Pos = 60;
                cb.BeginText();

            }
        }

        private void PrintRec(string PrintText, int StrWidth)
        {

            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);//cb.SetFontAndSize(bfTimes, 10);
            cb.ShowTextAligned(800, PrintText, X_Pos, Y_Pos, 0);
            X_Pos += StrWidth;
            PrintText = null;
        }

        private void printHText()
        {
            //if (Hierach == "******")
            //    ProgName = "All Programs";
            //else
            //{
            //    DataSet dsProg = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Hierach.Substring(0, 2), Hierach.Substring(2, 2), Hierach.Substring(4, 2));
            //    ProgName = (dsProg.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
            //}

            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Rank Definitions", 400, 520, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,  "Agency: "+Hierach, 400, 500, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Category: " + Rankdesc.ToString().Trim(), 400, 480, 0);
        }

        private void btnPDFprev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void cmbRnk_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillCmbHie();
        }

        private void cmbHie_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCmbRnk();
        }
    }
}