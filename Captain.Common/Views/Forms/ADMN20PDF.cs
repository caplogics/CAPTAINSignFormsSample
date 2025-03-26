#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
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
using CarlosAg.ExcelXmlWriter;
using ListItem = Captain.Common.Utilities.ListItem;
using DevExpress.XtraReports.UI;
using Captain.DatabaseLayer;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.CodeParser;
using System.Net;
using System.Reflection.Emit;
using DevExpress.Utils.Win.Hook;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using DevExpress.XtraCharts;
using DevExpress.Web.ASPxGantt;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.Model;
using DevExpress.Xpo.Helpers;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMN20PDF : Form
    {
        private CaptainModel _model = null; string _servicePlanDesc = "";
        public ADMN20PDF(BaseForm baseform, PrivilegeEntity priviliges, string SerPlan, string SerPlanDesc, string MemActivity)
        {
            InitializeComponent();

            _model = new CaptainModel();
            //this.Size = new Size(this.Width, this.Height - panel1.Height);
            BaseForm = baseform;
            Privileges = priviliges;
            SelServicePlan = SerPlan;
            _servicePlanDesc = SerPlanDesc;
            Member_Activity = MemActivity;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = "Service Plan Print"; // "Service Plan Selection for PDF";
            FillCmbHie();
            FillServicePlans();
            fillCombo();
            //chkRptFrmtCntrl3();
            //Added by Vikash on 02/13/2024 as per "service plan print" document
            AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            searchAgytabs.Tabs_Type = "00201";

            List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);
            //if (AgyTabs_List.Count > 0)
            //    pnlSPFieldCntl.Visible = true;
            //else
            //    pnlSPFieldCntl.Visible = false;

            IsCategory(SelServicePlan);


            //CASEDEP_List = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);
            //SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SelServicePlan, null, null, null);
            //CAMA_Details = _model.SPAdminData.Browse_CASESP2(SelServicePlan, null, null, null);
            //SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SelServicePlan, null, null, null, null, null, null, null, null);
            //PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", null, null, null);
            //PAYMENTService = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00201", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);


            //if (CASEDEP_List.Count > 0)
            //{
            //    ProgramDefinitionEntity DefEntity = CASEDEP_List.Find(u => u.Agency == SP_Hierarchies[0].Agency && u.Dept == SP_Hierarchies[0].Dept && u.Prog == SP_Hierarchies[0].Prog);
            //    if (DefEntity != null)
            //    {
            //        if (!string.IsNullOrEmpty(DefEntity.DepSerpostPAYCAT.Trim()))
            //        {
            //            CommonEntity AgyEnt = new CommonEntity();
            //            if (PAYMENTService.Count > 0)
            //                AgyEnt = PAYMENTService.Find(u => u.Code == DefEntity.DepSerpostPAYCAT);

            //            if (AgyEnt != null)
            //            {
            //                pnlSPFieldCntl.Visible = true;
            //                PayCatg = DefEntity.DepSerpostPAYCAT;
            //            }
            //        }
            //        else
            //        {
            //            pnlSPFieldCntl.Visible = false;
            //        }
            //    }
            //}
            //if (string.IsNullOrEmpty(PayCatg.Trim()))
            //{
            //    List<Agy_Ext_Entity> PPC_List = new List<Agy_Ext_Entity>();
            //    PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", null, null, null);

            //    if (PPC_List.Count > 0)
            //    {
            //        PPC_List = PPC_List.FindAll(u => u.Ext_1 != "");
            //        if (PPC_List.Count > 0)
            //        {
            //            foreach (Agy_Ext_Entity Entity in PPC_List)
            //            {
            //                if (!string.IsNullOrEmpty(Entity.Ext_1.Trim()))
            //                {
            //                    if (SP_Hierarchies[0].Agency == Entity.Ext_1.Substring(0, 2))
            //                    {
            //                        PayCatg = Entity.Code.Trim();
            //                        pnlSPFieldCntl.Visible = true;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            pnlSPFieldCntl.Visible = false;
            //        }

            //    }
            //}
        }

        void chkRptFrmtCntrl3()
        {
            chkbSPFielCntl.Checked = false;
            chkbSPFielCntl.Enabled = false;
            chkbSPFielCntl.ForeColor= System.Drawing.Color.Silver;
            List<CommonEntity> ServPostlst = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00201", BaseForm.BaseAgency, "**", "**");
            if (ServPostlst.Count > 0)
            {
                chkbSPFielCntl.Enabled = true;
                chkbSPFielCntl.ForeColor= System.Drawing.Color.Black;
            }
        }
        void FillServicePlans()
        {
            List<Utilities.ListItem> listItem = new List<Utilities.ListItem>();
            listItem.Add(new Utilities.ListItem(_servicePlanDesc, "SSP")); //"Selected Service Plan"
            listItem.Add(new Utilities.ListItem("All Active Service Plans", "ASP"));
            listItem.Add(new Utilities.ListItem("All Active and Inactive Service Plans", "AISP"));
            cmbSPOptions.Items.AddRange(listItem.ToArray());
            cmbSPOptions.SelectedIndex = 0;
        }
        //string PayCatg = string.Empty;
        List<AGYTABSEntity> AgyTabs_List;
        //List<ProgramDefinitionEntity> CASEDEP_List = new List<ProgramDefinitionEntity>();

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }

        public string SelServicePlan { get; set; }

        public string Member_Activity { get; set; }

        #endregion

        private void PbPdf_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void FillCmbHie()
        {
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once
            DataTable dt = ds.Tables[0];

            List<RNKCRIT1Entity> RankHieList;
            //if(((RepListItem)cmbRnk.SelectedItem).Value.ToString()=="*")
            //RankHieList = _model.SPAdminData.GetRNKCRIT(null);
            //else
            //    RankHieList = _model.SPAdminData.GetRNKCRIT(((RepListItem)cmbRnk.SelectedItem).Value.ToString());
            cmbHie.Items.Clear();
            int SelIndx = 0;
            List<RepListItem> ListHie = new List<RepListItem>();
            if (dt.Rows.Count > 0)
            {
                cmbHie.Items.Add(new RepListItem("**" + " - " + "All Agencies", "**"));
                //foreach (RNKCRIT1Entity drRankHie in RankHieList)
                //{
                string Hierarchy = string.Empty; int rowIndex = 1;
                foreach (DataRow dr in dt.Rows)//dr["Agy"] + " - " + dr["Name"], dr["Agy"]
                {
                    Hierarchy = dr["Agy"] + " - " + dr["Name"].ToString();
                    cmbHie.Items.Add(new RepListItem(Hierarchy, dr["Agy"].ToString()));

                    if (BaseForm.BaseAdminAgency == dr["Agy"].ToString())
                        SelIndx = rowIndex;
                    //if (dr["Agy"].ToString().Trim() == drRankHie.Agency.Trim())
                    //{
                    //    Hierac = drRankHie.Agency.ToString();
                    //    Hierarchy = dr["Agy"] + " - " + dr["Name"].ToString();
                    //    break;
                    //}
                    //else if (drRankHie.Agency.Trim() == "**")
                    //{
                    //    Hierac = drRankHie.Agency.ToString();
                    //    Hierarchy = "**" + " - " + "All Agencies";
                    //    break;
                    //}
                    rowIndex++;
                }
                //cmbHie.Items.Add(new RepListItem(Hierarchy, Hierac));
                //}
            }
            cmbHie.SelectedIndex = SelIndx;
        }

        private void fillCombo()
        {
            if (cmbSPOptions.Items.Count > 0)
            {
                //this.cmbDteRnge.SelectedIndexChanged -= new System.EventHandler(this.dgvServPlan_SelectionChanged);
                cmbDteRnge.Items.Clear();
               // chkPrgProcess.Visible = false;
                chkbTargets.Visible = false;
                chkPrgProcess.Enabled = false;
                chkPrgProcess.ForeColor= System.Drawing.Color.Silver;
                string SelAgency = String.Empty;

                //if (rbAllSP.Checked)
                //    SelAgency = ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim();
                //else SelAgency = BaseForm.BaseAdminAgency;

                //SelAgency = ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim();


                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP" ||
                    ((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "AISP"
                    )
                    SelAgency = ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim();
                else SelAgency = BaseForm.BaseAdminAgency;

                if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
                {
                    HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                    if (SelHie != null)
                        SelAgency = "**";
                }

                List<SRCsb14GroupEntity> servdte = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null, BaseForm.UserID, SelAgency);
                List<RCsb14GroupEntity> outcdte = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
                if (servdte.Count > 0)
                {

                    servdte = servdte.FindAll(u => (u.Agency.Equals(BaseForm.BaseAdminAgency) || u.Agency.Equals(SelAgency)) && u.PPR_SW == "Y");
                    outcdte = outcdte.FindAll(u => (u.Agency.Equals(BaseForm.BaseAdminAgency) || u.Agency.Equals(SelAgency)) && u.PPR_SW == "Y");

                }

                if (servdte.Count > 0)
                {
                    servdte = servdte.OrderByDescending(u => u.Active).ThenBy(u => u.Agency).ThenBy(u => u.Code).ToList();
                    servdte = servdte.OrderByDescending(u => u.OFdate).ToList();
                    foreach (SRCsb14GroupEntity GrpEnt in servdte)
                    {
                        if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                        {
                            string OutcCode = string.Empty;
                            if (outcdte.Count > 0)
                            {
                                RCsb14GroupEntity OutcEntity = outcdte.Find(u => u.Agency == GrpEnt.Agency && Convert.ToDateTime(u.OFdate.ToString()) == Convert.ToDateTime(GrpEnt.OFdate.ToString()) && Convert.ToDateTime(u.OTdate.ToString()) == Convert.ToDateTime(GrpEnt.OTdate.ToString()) && u.GrpDesc.Trim() == GrpEnt.GrpDesc.Trim() && u.GrpCode.Trim() == GrpEnt.GrpCode.Trim() && u.TblCode.Trim() == GrpEnt.TblCode.Trim());
                                if (OutcEntity != null)
                                    OutcCode = OutcEntity.Code.Trim();

                            }

                            cmbDteRnge.Items.Add(new Utilities.ListItem(Convert.ToDateTime(GrpEnt.OFdate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(GrpEnt.OTdate).ToString("MM/dd/yyyy") + "  " + GrpEnt.GrpDesc.Trim(), GrpEnt.Code, GrpEnt.Agency, OutcCode,GrpEnt.GrpDesc.Trim(),string.Empty, string.Empty, string.Empty));

                            //cmbDteRnge.Items.Add(new Utilities.ListItem(Convert.ToDateTime(GrpEnt.OFdate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(GrpEnt.OTdate).ToString("MM/dd/yyyy") + " " + GrpEnt.GrpDesc.Trim(), GrpEnt.Code, GrpEnt.Agency, GrpEnt.GrpDesc.Trim()));
                        }
                    }
                    //this.cmbDteRnge.SelectedIndexChanged += new System.EventHandler(this.dgvServPlan_SelectionChanged);

                    if (cmbDteRnge.Items.Count > 0)
                    {
                        cmbDteRnge.SelectedIndex = 0;

                        //chkPrgProcess.Visible = true;
                        chkPrgProcess.Enabled = true; chkPrgProcess.ForeColor= System.Drawing.Color.Black;
                        chkbTargets.Visible = true;
                    }
                }
            }
        }

        private void rbAllSP_CheckedChanged(object sender, EventArgs e)
        {
            //if (rbAllSP.Checked)
            //{

            //    //pnlHie.Visible = true; //kranthi 05092024
            //    pnlHie.Enabled = true;
            //    FillCmbHie();

            //    //if (AgyTabs_List.Count > 0)
            //    //    pnlSPFieldCntl.Visible = true;
            //    //else
            //    pnlSPFieldCntl.Visible = true;
            //}
            //else
            //{

            //    //pnlHie.Visible = false; //kranthi 05092024


            //    pnlHie.Enabled = false;

            //    pnlSPFieldCntl.Visible = false;
            //    IsCategory(SelServicePlan);



            //    //pnlSPFieldCntl.Visible = true;
            //}
        }

        private void IsCategory(string SelSerplan)
        {
            List<ProgramDefinitionEntity> CASEDEP_List = new List<ProgramDefinitionEntity>();
            CASEDEP_List = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);
            List<CASESP1Entity> SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SelSerplan, null, null, null);
            //List<CommonEntity> PAYMENTService = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00201", SP_Hierarchies[0].Agency, SP_Hierarchies[0].Dept, SP_Hierarchies[0].Prog, string.Empty);
            List<Agy_Ext_Entity> PAYMENTService= _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);
            if (CASEDEP_List.Count > 0)
            {
                ProgramDefinitionEntity DefEntity = CASEDEP_List.Find(u => u.Agency == SP_Hierarchies[0].Agency && u.Dept == SP_Hierarchies[0].Dept && u.Prog == SP_Hierarchies[0].Prog);
                if (DefEntity != null)
                {
                    if (!string.IsNullOrEmpty(DefEntity.DepSerpostPAYCAT.Trim()))
                    {
                        Agy_Ext_Entity AgyEnt = new Agy_Ext_Entity();
                        if (PAYMENTService.Count > 0)
                            AgyEnt = PAYMENTService.Find(u => u.Ext_2 == DefEntity.DepSerpostPAYCAT);

                        if (AgyEnt != null)
                        {
                            chkbSPFielCntl.Enabled = true;
                            chkbSPFielCntl.ForeColor = System.Drawing.Color.Black;
                        }

                    }
                }
            }


        }

        private string IsFieldPrint(string SelSerPlan)
        {
            string IsCatg = "N";

            List<ProgramDefinitionEntity> CASEDEP_List = new List<ProgramDefinitionEntity>();
            CASEDEP_List = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);
            List<CASESP1Entity> SP_Hierarchies1 = _model.SPAdminData.Browse_CASESP1(SelSerPlan, null, null, null);
            //List<CommonEntity> PAYMENTService = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00201", SP_Hierarchies1[0].Agency, SP_Hierarchies1[0].Dept, SP_Hierarchies1[0].Prog, string.Empty);
            List<Agy_Ext_Entity> PAYMENTService = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);
            if (CASEDEP_List.Count > 0)
            {
                ProgramDefinitionEntity DefEntity = CASEDEP_List.Find(u => u.Agency == SP_Hierarchies1[0].Agency && u.Dept == SP_Hierarchies1[0].Dept && u.Prog == SP_Hierarchies1[0].Prog);
                if (DefEntity != null)
                {
                    if (!string.IsNullOrEmpty(DefEntity.DepSerpostPAYCAT.Trim()))
                    {
                        Agy_Ext_Entity AgyEnt = new Agy_Ext_Entity();
                        if (PAYMENTService.Count > 0)
                            AgyEnt = PAYMENTService.Find(u => u.Ext_2 == DefEntity.DepSerpostPAYCAT);

                        if (AgyEnt != null)
                        { IsCatg = "Y"; }

                    }
                }
            }

            return IsCatg;
        }

        private void btnPDFprev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnGenPdf_Click(object sender, EventArgs e)
        {
            string strHiearchy = null;
            SP_Hierarchies = new List<CASESP1Entity>();
            //Kranthi
            //if (rbSelSP.Checked)
            //{
            //    SP_Hierarchies = _model.SPAdminData.Browse_CASESP1AndSp0(SelServicePlan, null, null, null);
            //    if (BaseForm.BaseAdminAgency != "**")
            //        SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Agency == BaseForm.BaseAdminAgency);

            //}
            //else
            //{
            //    strHiearchy = ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim();
            //    if (strHiearchy == "**")
            //        strHiearchy = null;
            //    SP_Hierarchies = _model.SPAdminData.Browse_CASESP1AndSp0(null, strHiearchy, null, null);
            //}

            /** DROPDOWN EVENT FOR SP checkboxes**/

            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
            {
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1AndSp0(SelServicePlan, null, null, null);
                if (BaseForm.BaseAdminAgency != "**")
                    SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Agency == BaseForm.BaseAdminAgency);

            }
            else
            {
                strHiearchy = ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim();
                if (strHiearchy == "**")
                    strHiearchy = null;
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1AndSp0(null, strHiearchy, null, null);
            }




            if (chkbPDF.Checked)
            {
                //PdfMode = "Main";
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                //Kranthi
                //if (rbSelSP.Checked)
                //    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
                //else if (rbAllSP.Checked)
                //    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_ClosedAll);


                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_ClosedAll1);
                else
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_ClosedAll1);


                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
            else if (chkbExcel.Checked)
            {
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "xls");
                //Kranthi
                //if (rbSelSP.Checked)
                //    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveExcelForm_Closed);
                //else if (rbAllSP.Checked)
                //    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveAllExcelForm_Closed);

                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveAllExcelForm_Closed_Devexpress); //On_SaveAllExcelForm_Closed_Devexpress
                else
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveAllExcelForm_Closed_Devexpress);

                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
            else
            {
                AlertBox.Show("Please select Output Format", MessageBoxIcon.Warning);
            }
        }

        List<CASESP2Entity> CAMA_Details = new List<CASESP2Entity>();
        CASESP0Entity SP_Header_Rec;
        List<Agy_Ext_Entity> PPC_List = new List<Agy_Ext_Entity>();
        List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
        //string isEnabled = "N";
        //string isRequired = "N";

        private void ServicePlanFieldControl_Report()
        {
            Random_Filename = null;
            string PdfName = "Service Plan Field Controls";
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".xls";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xls";

            CarlosAg.ExcelXmlWriter.Workbook book = new CarlosAg.ExcelXmlWriter.Workbook();
            CarlosAg.ExcelXmlWriter.Worksheet sheet;
            WorksheetCell cell;
            this.GenerateStylesFieldControls(book.Styles);


            try
            {
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SelServicePlan, null, null, null);
                CAMA_Details = _model.SPAdminData.Browse_CASESP2(SelServicePlan, null, null, null);
                SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SelServicePlan, null, null, null, null, null, null, null, null);
                PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", null, null, null);


                List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "PMTSTDFLDS");
                List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "FLDCNTL");
                List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                sheet = book.Worksheets.Add("Data");
                sheet.Table.DefaultRowHeight = 14.25F;

                WorksheetRow Row0;
                WorksheetRow Row1;
                WorksheetRow Row2;

                Row0 = sheet.Table.Rows.Add();

                cell = Row0.Cells.Add("SPM", DataType.String, "s94");
                cell.MergeAcross = 4;
                cell = Row0.Cells.Add("Description", DataType.String, "s94");
                cell.MergeAcross = 6;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        cell = Row0.Cells.Add(Entity.PSTF_DESC.ToString().Trim(), DataType.String, "s94");
                        cell.MergeAcross = 2;
                    }
                }

                Row1 = sheet.Table.Rows.Add();

                cell = Row1.Cells.Add("", DataType.String, "s94");
                cell.MergeAcross = 4;
                cell = Row1.Cells.Add("", DataType.String, "s94");
                cell.MergeAcross = 6;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        cell = Row1.Cells.Add("Enabled", DataType.String, "s94");
                        cell = Row1.Cells.Add("Required", DataType.String, "s94");
                        cell = Row1.Cells.Add("Disabled", DataType.String, "s94");
                    }
                }

                //Row2 = sheet.Table.Rows.Add();

                foreach (CASESP1Entity spEntity in SP_Hierarchies)
                {
                    Row2 = sheet.Table.Rows.Add();
                    cell = Row2.Cells.Add(spEntity.SP_Desc, DataType.String, "s96");
                    cell.MergeAcross = 4;
                    int x = 0;
                    foreach (CASESP2Entity CAEntity in CAMA_Details)
                    {
                        if (CAEntity.Type1 == "CA")
                        {
                            if (x > 0)
                            {
                                Row2 = sheet.Table.Rows.Add();
                                cell = Row2.Cells.Add("", DataType.String, "s95B");
                                cell.MergeAcross = 4;
                            }
                            x = 1;
                            cell = Row2.Cells.Add(CAEntity.CAMS_Desc, DataType.String, "s96");
                            cell.MergeAcross = 6;

                            propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, SP_Hierarchies[0].Code, SP_Hierarchies[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                            propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                            if (propPMTFLDCNTLHEntity.Count == 0)
                            {
                                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                            }

                            foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                            {
                                string isEnabled = string.Empty;
                                string isRequired = string.Empty;

                                if (propPMTFLDCNTLHEntity.Count > 0)
                                {
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                        isEnabled = "Y";
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                        isRequired = "Y";
                                }

                                if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                {
                                    cell = Row2.Cells.Add(isEnabled.ToString(), DataType.String, "s95");
                                    cell = Row2.Cells.Add(isRequired.ToString(), DataType.String, "s95");
                                    cell = Row2.Cells.Add("", DataType.String, "s95");
                                }
                            }
                        }
                    }

                }


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void GenerateStylesFieldControls(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";
        }




        List<CASESP1Entity> SP_Hierarchies;
        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        int pageNumber = 1;
        string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        string Hierach = null, privhireac = null, ProgName = null, ScreenName = null;

        private void chkbTargets_CheckedChanged(object sender, EventArgs e)
        {
            //Kranthi
            //if (chkbTargets.Checked)
            //{
            //    pnlDateRange.Visible = true;
            //    fillCombo();
            //}
            //else
            //{
            //    pnlDateRange.Visible = false;
            //}
        }

        private void cmbHie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHie.Items.Count > 0)
            {
                // if (chkbTargets.Checked)
                fillCombo();

            }
        }

        private void cmbSPOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (cmbSPOptions.SelectedIndex > 0)
            //{
            pnlHie.CssStyle = "opacity:0.5;";
            pnlHie.Enabled = false;
            chkbSPFielCntl.Enabled = false; chkbSPFielCntl.ForeColor = System.Drawing.Color.Silver; chkbSPFielCntl.Checked = false;
            IsCategory(SelServicePlan);
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
            {
                pnlHie.Enabled = false;
                pnlHie.CssStyle = "opacity:0.5;";
                FillCmbHie();
            }
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP")
            {
                chkRptFrmtCntrl3();
                pnlHie.Enabled = true;
                pnlHie.CssStyle = "opacity:1;";
                FillCmbHie();
                // chkbSPFielCntl.Visible = true;
            }
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "AISP")
            {
                chkRptFrmtCntrl3();
                pnlHie.Enabled = true;
                pnlHie.CssStyle = "opacity:1;";
                FillCmbHie();
                // chkbSPFielCntl.Visible = true;
            }
            //}
        }

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

                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;
                string Name = BaseForm.BaseApplicationName;
                string AppNo = BaseForm.BaseApplicationNo;

                BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont bf_Times = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font fcGray = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.DARK_GRAY);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

                BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

                BaseFont Fbf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);

                BaseFont FbfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont FbfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
                iTextSharp.text.Font Ffc = new iTextSharp.text.Font(bfTimes, 12, 2);
                iTextSharp.text.Font Ffc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
                iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));

                iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
                iTextSharp.text.Font FTableFont = new iTextSharp.text.Font(bf_calibri, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
                iTextSharp.text.Font FTblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
                iTextSharp.text.Font TblFontHeadBold = new iTextSharp.text.Font(bfTimes, 12, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

                //DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(SelServicePlan, null, null, null, null, null, null, null, null);
                //DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];



                PdfPTable Header = new PdfPTable(1);
                Header.TotalWidth = 500f;
                Header.WidthPercentage = 100;
                Header.LockedWidth = true;
                float[] Headerwidths = new float[] { 90f };
                Header.SetWidths(Headerwidths);
                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.SpacingAfter = 10f;


                PdfPTable table;
                if (chkbTargets.Checked)
                {
                    table = new PdfPTable(4);
                    table.TotalWidth = 530f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 80f, 25f, 25f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                }
                else
                {
                    table = new PdfPTable(3);
                    table.TotalWidth = 530f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 80f, 25f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                }

                int X_Pos, Y_Pos;

                //SP_Hierarchies = new List<CASESP1Entity>();
                //SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SelServicePlan, null, null, null);


                DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(SelServicePlan.Trim(), null, null, null);
                DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];
                //dr["sp2_cams_code"].ToString().Trim()
                List<CAMASTEntity> CAList;
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                List<MSMASTEntity> MSList;
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

                //DataSet dsCAMAST = DatabaseLayer.SPAdminDB.Browse_CAMAST(null, null, null, null);
                //DataTable dtCAMAST=dsCAMAST.Tables[0];

                //DataSet MSMast = DatabaseLayer.SPAdminDB.Browse_MSMAST(null,null, null, null, null);
                //DataTable DtMSMast=MSMast.Tables[0];

                List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
                List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();

                List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");

                bool First = true; string MS_Type = string.Empty;
                string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null; bool IsFirst = true;
                if (SP_Hierarchies.Count > 0)
                {
                    string Hiename = null; string AgencyName = string.Empty;
                    foreach (CASESP1Entity Entity in SP_Hierarchies)
                    {
                        //HierarchyEntity hierchyEntity = null;
                        if (Entity.Agency + Entity.Dept + Entity.Prog == "******")
                        {
                            Hiename = "All Hierarchies";
                            //hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                        }
                        else
                            Hiename = "Not Defined in CASEHIE";

                        DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.Agency, Entity.Dept, Entity.Prog);
                        if (ds_Hie_Name.Tables.Count > 0)
                        {
                            if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                            {
                                if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                    Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                //hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

                                if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                    ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                    ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

                                //hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
                            }
                        }

                        if (!IsFirst) { table.DeleteBodyRows(); Priv_Branch = null; }

                        IsFirst = false;

                        AgencyName = Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog + "  " + Hiename;

                        if (dtSP_CaseSP2.Rows.Count > 0)
                        {
                            DataView dv = dtSP_CaseSP2.DefaultView;
                            dv.Sort = "sp2_branch DESC";
                            dtSP_CaseSP2 = dv.ToTable();
                            Y_Pos = 770;
                            X_Pos = 55;
                            //cb.BeginText();
                            int Count = 0;
                            foreach (DataRow dr in dtSP_CaseSP2.Rows)
                            {
                                //SP_Plan_desc = drSP_Services["sp0_description"].ToString().Trim();
                                SP_Plan_desc = SP_Hierarchies[0].SP_Desc.Trim();
                                Branch = dr["sp2_branch"].ToString().Trim();
                                if (Branch != Priv_Branch)
                                {
                                    //cb.EndText();
                                    document.Add(table);
                                    //if (Count > 0)
                                    //{
                                    //    for (int i = 0; i <= Count; i++)
                                    //    {
                                    //        table.DeleteRow(i);
                                    //    }
                                    //}
                                    table.DeleteBodyRows();

                                    //table.DeleteRow(1);

                                    document.NewPage();
                                    if (Branch != "P")
                                        First = false;
                                    //cb.BeginText();
                                    string Service_desc = SP_Hierarchies[0].BpDesc.Trim();//drSP_Services["sp0_pbranch_desc"].ToString();
                                    if (!First)
                                    {
                                        if (Branch.Trim() == SP_Hierarchies[0].B1Code.ToString().Trim())
                                            Service_desc = SP_Hierarchies[0].B1Desc;
                                        else if (Branch.Trim() == SP_Hierarchies[0].B2Code.ToString().Trim())
                                            Service_desc = SP_Hierarchies[0].B2Desc;
                                        else if (Branch.Trim() == SP_Hierarchies[0].B3Code.ToString().Trim())
                                            Service_desc = SP_Hierarchies[0].B3Desc;
                                        else if (Branch.Trim() == SP_Hierarchies[0].B4Code.ToString().Trim())
                                            Service_desc = SP_Hierarchies[0].B4Desc.ToString();
                                        else if (Branch.Trim() == SP_Hierarchies[0].B5Code.ToString().Trim())
                                            Service_desc = SP_Hierarchies[0].B5Desc.ToString();
                                        else if (Branch.Trim() == "9")
                                            Service_desc = "Additional Branch";

                                        //if (Branch.Trim() == drSP_Services["sp0_branch1_code"].ToString().Trim())
                                        //    Service_desc = drSP_Services["sp0_branch1_desc"].ToString();
                                        //else if (Branch.Trim() == drSP_Services["sp0_branch2_code"].ToString().Trim())
                                        //    Service_desc = drSP_Services["sp0_branch2_desc"].ToString();
                                        //else if (Branch.Trim() == drSP_Services["sp0_branch3_code"].ToString().Trim())
                                        //    Service_desc = drSP_Services["sp0_branch3_desc"].ToString();
                                        //else if (Branch.Trim() == drSP_Services["sp0_branch4_code"].ToString().Trim())
                                        //    Service_desc = drSP_Services["sp0_branch4_desc"].ToString();
                                        //else if (Branch.Trim() == drSP_Services["sp0_branch5_code"].ToString().Trim())
                                        //    Service_desc = drSP_Services["sp0_branch5_desc"].ToString();
                                        //else if (Branch.Trim() == "9")
                                        //    Service_desc = "Additional Branch";

                                    }
                                    Y_Pos = 770;
                                    X_Pos = 55;
                                    table.SpacingBefore = 10f;

                                    PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim() + " (" + SelServicePlan.Trim() + ")", fc1));
                                    Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                                    if (chkbTargets.Checked)
                                        Sp_Plan.Colspan = 4;
                                    else
                                        Sp_Plan.Colspan = 3;
                                    Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Sp_Plan);

                                    PdfPCell Sp_Hie = new PdfPCell(new Phrase("Hierarchy :" + AgencyName.Trim(), fcGray));
                                    Sp_Hie.HorizontalAlignment = Element.ALIGN_LEFT;
                                    if (chkbTargets.Checked)
                                        Sp_Hie.Colspan = 4;
                                    else
                                        Sp_Hie.Colspan = 3;
                                    Sp_Hie.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Sp_Hie);

                                    PdfPCell ServiceDesc = new PdfPCell(new Phrase("Branch :" + Service_desc.Trim(), TblFontBold));
                                    ServiceDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                    if (chkbTargets.Checked)
                                        ServiceDesc.Colspan = 4;
                                    else
                                        ServiceDesc.Colspan = 3;
                                    ServiceDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(ServiceDesc);

                                    if (chkbTargets.Checked)
                                    {
                                        string[] col = { "Type", "Description", "Code", "Target" };
                                        for (int i = 0; i < col.Length; ++i)
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                            table.AddCell(cell);
                                        }
                                    }
                                    else
                                    {
                                        string[] col = { "Type", "Description", "Code" };
                                        for (int i = 0; i < col.Length; ++i)
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                            table.AddCell(cell);
                                        }
                                    }

                                    Priv_Branch = Branch;
                                    First = false;
                                }
                                string CAMSType = dr["sp2_type"].ToString();
                                string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty, strCAMS = string.Empty; string ProgTarget = string.Empty;
                                string ServiceCount = string.Empty;
                                if (CAMSType == "CA")
                                {
                                    foreach (CAMASTEntity drCAMAST in CAList)
                                    {
                                        if (drCAMAST.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                        {
                                            CAMSDesc = drCAMAST.Desc.ToString().Trim(); break;
                                        }
                                        else
                                            CAMSDesc = "";
                                    }

                                    /*//foreach(DataRow drCAMAST in dtCAMAST.Rows)
                                    //{
                                    //    if(drCAMAST["CA_CODE"].ToString().Trim()==dr["sp2_cams_code"].ToString().Trim())
                                    //    {
                                    //        CAMSDesc = drCAMAST["CA_DESC"].ToString();break;
                                    //    }
                                    //    else
                                    //        CAMSDesc="";
                                    //}

                                    //CAMSDesc = drCAMAST["CA_DESC"].ToString();*/

                                    if (chkbTargets.Checked)
                                    {
                                        DataSet ds = new DataSet();
                                        DataSet ds1 = new DataSet();
                                        DataTable dt = new DataTable();
                                        DataTable dt1 = new DataTable();
                                        DataTable dtserOut = new DataTable();

                                        if (cmbDteRnge.Items.Count > 0)
                                        {
                                            RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                            RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                        }

                                        SPCOde = SelServicePlan;
                                        GoalSRHieEntity = _model.SPAdminData.Browse_RNGSRGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());



                                        ServiceCount = string.Empty;
                                        //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, SPCOde, string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                        //if (dsSerOut.Tables.Count > 0)
                                        //    dtserOut = dsSerOut.Tables[0];
                                        //if (dtserOut.Rows.Count > 0)
                                        //{
                                        //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                        //}

                                        if (GoalSRHieEntity.Count > 0)
                                        {
                                            GoalSRHieEntity = GoalSRHieEntity.FindAll(u => u.RNGSRGAH_TARGET != "" && u.RNGSRGAH_HIE == Entity.Agency + Entity.Dept + Entity.Prog);
                                            if (GoalSRHieEntity.Count > 0)
                                                ServiceCount = GoalSRHieEntity.Sum(u => Convert.ToInt32(u.RNGSRGAH_TARGET.Trim())).ToString();
                                        }

                                        PdfPCell RowType = new PdfPCell(new Phrase("Service", TableFont));
                                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowType);

                                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowDesc);

                                        PdfPCell RowCode = new PdfPCell(new Phrase(dr["sp2_cams_code"].ToString().Trim(), TableFont));
                                        RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowCode);

                                        PdfPCell RowTarget = new PdfPCell(new Phrase(ServiceCount.ToString().Trim(), TableFont));
                                        RowTarget.HorizontalAlignment = Element.ALIGN_CENTER;
                                        RowTarget.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowTarget);
                                    }

                                    else
                                    {
                                        PdfPCell RowType = new PdfPCell(new Phrase("Service", TableFont));
                                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowType);

                                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowDesc);

                                        PdfPCell RowCode = new PdfPCell(new Phrase(dr["sp2_cams_code"].ToString().Trim(), TableFont));
                                        RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowCode);
                                    }
                                    //dr["sp2_cams_code"].ToString().Trim()
                                }
                                else
                                {
                                    string Type_Desc = string.Empty;
                                    foreach (MSMASTEntity drMSMast in MSList)
                                    {
                                        if (drMSMast.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                        {
                                            CAMSDesc = drMSMast.Desc.ToString().Trim();
                                            MS_Type = drMSMast.Type1.ToString();
                                            //if (MS_Type == "M")
                                            //    Type_Desc = "Milestone";
                                            //else
                                            Type_Desc = "Outcome";
                                            break;
                                        }
                                        else
                                            CAMSDesc = "";
                                    }


                                    /*//foreach(DataRow drMSMast in DtMSMast.Rows)
                                    //{
                                    //    if (drMSMast["MS_CODE"].ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                    //    {
                                    //        CAMSDesc = drMSMast["MS_DESC"].ToString(); break;
                                    //    }
                                    //    else
                                    //        CAMSDesc = "";
                                    //}*/
                                    if (chkbTargets.Checked)
                                    {
                                        DataSet ds = new DataSet();
                                        DataSet ds1 = new DataSet();
                                        DataTable dt = new DataTable();
                                        DataTable dt1 = new DataTable();
                                        DataTable dtserOut = new DataTable();

                                        if (cmbDteRnge.Items.Count > 0)
                                        {
                                            RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                            RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                        }
                                        SPCOde = SelServicePlan;

                                        GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());

                                        ServiceCount = string.Empty;
                                        //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, SPCOde, string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                        //if (dsSerOut.Tables.Count > 0)
                                        //    dtserOut = dsSerOut.Tables[0];
                                        //if (dtserOut.Rows.Count > 0)
                                        //{
                                        //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                        //}

                                        if (GoalHieEntity.Count > 0)
                                        {
                                            GoalHieEntity = GoalHieEntity.FindAll(u => u.RNGGAH_TARGET != "" && u.RNGGAH_HIE == Entity.Agency + Entity.Dept + Entity.Prog);
                                            if (GoalHieEntity.Count > 0)
                                                ServiceCount = GoalHieEntity.Sum(u => Convert.ToInt32(u.RNGGAH_TARGET.Trim())).ToString();
                                        }

                                        PdfPCell RowType = new PdfPCell(new Phrase(Type_Desc, TableFont));
                                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowType);

                                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowDesc);

                                        PdfPCell RowCode = new PdfPCell(new Phrase(dr["sp2_cams_code"].ToString().Trim(), TblFontBold));
                                        RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowCode);

                                        PdfPCell RowTarget = new PdfPCell(new Phrase(ServiceCount.ToString().Trim(), TblFontBold));
                                        RowTarget.HorizontalAlignment = Element.ALIGN_CENTER;
                                        RowTarget.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowTarget);
                                    }
                                    else
                                    {
                                        PdfPCell RowType = new PdfPCell(new Phrase(Type_Desc, TblFontBold));
                                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowType);

                                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TblFontBold));
                                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowDesc);

                                        PdfPCell RowCode = new PdfPCell(new Phrase(dr["sp2_cams_code"].ToString().Trim(), TblFontBold));
                                        RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowCode);
                                    }
                                }
                            }
                            Count++;


                        }

                        document.Add(Header);
                        if (table.Rows.Count > 0)
                        {
                            document.Add(table);

                            document.NewPage();
                            table.Rows.Clear();

                            PdfPTable table1 = new PdfPTable(3);
                            table1.TotalWidth = 530f;
                            table1.WidthPercentage = 100;
                            table1.LockedWidth = true;
                            float[] widths1 = new float[] { 70f, 20f, 70f };
                            table1.SetWidths(widths1);
                            table1.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPTable tablehie = new PdfPTable(2);
                            tablehie.TotalWidth = 220f;
                            tablehie.WidthPercentage = 100;
                            tablehie.LockedWidth = true;
                            float[] widthsHie = new float[] { 20f, 70f };
                            tablehie.SetWidths(widthsHie);
                            tablehie.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPTable tablefund = new PdfPTable(2);
                            tablefund.TotalWidth = 220f;
                            tablefund.WidthPercentage = 100;
                            tablefund.LockedWidth = true;
                            float[] widthsfund = new float[] { 20f, 70f };
                            tablefund.SetWidths(widthsfund);
                            tablefund.HorizontalAlignment = Element.ALIGN_CENTER;



                            PdfPCell cell = new PdfPCell(new Phrase("Programs Associated", TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = iTextSharp.text.Rectangle.BOX;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            cell.FixedHeight = 20f;
                            cell.Colspan = 2;
                            tablehie.AddCell(cell);

                            foreach (CASESP1Entity selctsp1item in SP_Hierarchies)
                            {
                                //HierarchyEntity hierchyEntity = null;
                                if (selctsp1item.Agency + selctsp1item.Dept + selctsp1item.Prog == "******")
                                {
                                    Hiename = "All Hierarchies";
                                    //hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                                }
                                else
                                    Hiename = "Not Defined in CASEHIE";

                                DataSet ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(selctsp1item.Agency, selctsp1item.Dept, selctsp1item.Prog);
                                if (ds_Hie_Name1.Tables.Count > 0)
                                {
                                    if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                            Hiename = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                        if (string.IsNullOrEmpty(ds_Hie_Name1.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                            ds_Hie_Name1.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                        if (string.IsNullOrEmpty(ds_Hie_Name1.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                            ds_Hie_Name1.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";
                                    }
                                }

                                PdfPCell RowCode = new PdfPCell(new Phrase(selctsp1item.Agency + "-" + selctsp1item.Dept + "-" + selctsp1item.Prog.Trim(), TableFont));
                                RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                RowCode.FixedHeight = 17f;
                                tablehie.AddCell(RowCode);

                                PdfPCell RowDesc = new PdfPCell(new Phrase(Hiename.Trim(), TableFont));
                                RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                RowDesc.FixedHeight = 17f;
                                tablehie.AddCell(RowDesc);

                                cell = new PdfPCell(new Phrase("", TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                cell.Colspan = 2;
                                tablehie.AddCell(cell);

                                if (FundingList.Count > 0)
                                {

                                    if (!string.IsNullOrEmpty(SP_Hierarchies[0].Funds.ToString().Trim()))
                                    {
                                        cell = new PdfPCell(new Phrase("Funding Sources", TblFontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.Border = iTextSharp.text.Rectangle.BOX;
                                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        cell.FixedHeight = 20f;
                                        cell.Colspan = 2;
                                        tablefund.AddCell(cell);



                                        foreach (SPCommonEntity funditem in FundingList)
                                        {
                                            if (SP_Hierarchies[0].Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                            {
                                                PdfPCell RowCode1 = new PdfPCell(new Phrase(funditem.Code.ToString().Trim(), TableFont));
                                                RowCode1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                RowCode1.Border = iTextSharp.text.Rectangle.BOX;
                                                RowCode1.FixedHeight = 17f;
                                                tablefund.AddCell(RowCode1);

                                                PdfPCell RowDesc1 = new PdfPCell(new Phrase(funditem.Desc.Trim(), TableFont));
                                                RowDesc1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                RowDesc1.Border = iTextSharp.text.Rectangle.BOX;
                                                RowDesc1.FixedHeight = 17f;
                                                tablefund.AddCell(RowDesc1);


                                            }
                                        }

                                        cell = new PdfPCell(new Phrase("", TblFontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        cell.Colspan = 2;
                                        tablehie.AddCell(cell);
                                    }

                                }

                                PdfPCell Celltb = new PdfPCell(tablehie);
                                Celltb.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(Celltb);

                                Celltb = new PdfPCell(new Phrase(""));
                                Celltb.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(Celltb);

                                Celltb = new PdfPCell(tablefund);
                                Celltb.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(Celltb);


                                PdfPCell Sp_Hie = new PdfPCell(table1);
                                Sp_Hie.Colspan = 3;
                                Sp_Hie.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Sp_Hie);

                                if (table.Rows.Count > 0)
                                    document.Add(table);

                            }


                        }
                        else
                        {
                            document.NewPage();
                            cb.BeginText();
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 15);
                            cb.ShowTextAligned(Element.ALIGN_CENTER, "Services and Outcomes are not Defined for the Service Plan" + SP_Plan_desc + "( " + Entity.Code + " )", 300, 600, 0);
                            cb.EndText();
                        }

                    }
                }
                else
                {
                    cb.BeginText();
                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 15);
                    cb.ShowTextAligned(Element.ALIGN_CENTER, "Hierarchy not Defined for this Service Plan", 300, 600, 0);
                    cb.EndText();
                }

                #region Service Plan Field Control
                try
                {
                    if (chkbSPFielCntl.Checked)
                    {
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        document.NewPage();

                        CAMA_Details = _model.SPAdminData.Browse_CASESP2(SP_Hierarchies[0].Code, null, null, null);
                        PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", null, null, null);

                        List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "PMTSTDFLDS");
                        List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "FLDCNTL");
                        List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                        ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                        int HeaderCount = 0;
                        foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                        {
                            if (Entity != null)
                            {
                                if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                                {
                                    HeaderCount++;
                                }
                            }
                        }
                        float[] widths;
                        if (HeaderCount > 0)
                        {
                            for (int i = 0; i <= HeaderCount; i++)
                            {
                                widths = new float[] { 100f };
                            }
                        }


                        PdfPTable fieldCntl_Headers = new PdfPTable(HeaderCount + 2);
                        fieldCntl_Headers.TotalWidth = 750f;
                        fieldCntl_Headers.WidthPercentage = 100;
                        fieldCntl_Headers.LockedWidth = true;
                        float[] Header_widths = new float[] { 300f, 400f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                        fieldCntl_Headers.SetWidths(Header_widths);
                        //fieldCntl_Headers.SpacingAfter = 10f;

                        PdfPCell pdfMainHeader = new PdfPCell(new Phrase("Field Control Summary", fcRed));
                        pdfMainHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfMainHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfMainHeader.Colspan = HeaderCount + 2;
                        pdfMainHeader.FixedHeight = 20;
                        fieldCntl_Headers.AddCell(pdfMainHeader);

                        PdfPCell Headers0 = new PdfPCell(new Phrase("SPM", FTblFontBold));
                        Headers0.HorizontalAlignment = Element.ALIGN_CENTER;
                        Headers0.VerticalAlignment = Element.ALIGN_MIDDLE;
                        Headers0.Border = iTextSharp.text.Rectangle.BOX;
                        Headers0.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        Headers0.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        fieldCntl_Headers.AddCell(Headers0);

                        PdfPCell Headers1 = new PdfPCell(new Phrase("Description", FTblFontBold));
                        Headers1.HorizontalAlignment = Element.ALIGN_CENTER;
                        Headers1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        Headers1.Border = iTextSharp.text.Rectangle.BOX;
                        Headers1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        Headers1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        fieldCntl_Headers.AddCell(Headers1);

                        foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                        {
                            if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                            {
                                PdfPCell Headers = new PdfPCell(new Phrase(Entity.PSTF_DESC.ToString().Trim(), FTblFontBold));
                                Headers.HorizontalAlignment = Element.ALIGN_LEFT;
                                Headers.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Headers.Border = iTextSharp.text.Rectangle.BOX;
                                Headers.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                Headers.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                fieldCntl_Headers.AddCell(Headers);
                            }
                        }

                        PdfPCell SPM = new PdfPCell(new Phrase("", FTblFontBold));
                        SPM.HorizontalAlignment = Element.ALIGN_LEFT;
                        SPM.VerticalAlignment = Element.ALIGN_MIDDLE;
                        SPM.Border = iTextSharp.text.Rectangle.BOX;
                        SPM.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        SPM.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        fieldCntl_Headers.AddCell(SPM);

                        PdfPCell Desc = new PdfPCell(new Phrase("", FTblFontBold));
                        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                        Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                        Desc.Border = iTextSharp.text.Rectangle.BOX;
                        Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        fieldCntl_Headers.AddCell(Desc);

                        foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                        {
                            if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                            {
                                PdfPTable fieldCntl_Headers2 = new PdfPTable(3);
                                fieldCntl_Headers2.TotalWidth = 36f;
                                fieldCntl_Headers2.WidthPercentage = 100;
                                fieldCntl_Headers2.LockedWidth = true;
                                float[] YesNo_widths = new float[] { 12f, 12f, 12f };
                                fieldCntl_Headers2.SetWidths(YesNo_widths);

                                PdfPCell Enabled = new PdfPCell(new Phrase("Enabled", FTblFontBold));
                                Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Enabled.Rotation = 270;
                                Enabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //Enabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                fieldCntl_Headers2.AddCell(Enabled);

                                PdfPCell Required = new PdfPCell(new Phrase("Required", FTblFontBold));
                                Required.HorizontalAlignment = Element.ALIGN_CENTER;
                                Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Required.Rotation = 270;
                                Required.Border = iTextSharp.text.Rectangle.BOX;
                                Required.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                fieldCntl_Headers2.AddCell(Required);

                                PdfPCell Disabled = new PdfPCell(new Phrase("Disabled", FTblFontBold));
                                Disabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                Disabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Disabled.Rotation = 270;
                                Disabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //Disabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                Disabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                fieldCntl_Headers2.AddCell(Disabled);

                                PdfPCell NestedHeaders = new PdfPCell(fieldCntl_Headers2);
                                NestedHeaders.Border = iTextSharp.text.Rectangle.BOX;
                                NestedHeaders.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                fieldCntl_Headers.AddCell(NestedHeaders);
                            }
                        }

                        foreach (CASESP1Entity spEntity in SP_Hierarchies)
                        {
                            PdfPCell SP_Desc = new PdfPCell(new Phrase(spEntity.SP_Desc, FTblFontBold));
                            SP_Desc.HorizontalAlignment = Element.ALIGN_CENTER;
                            SP_Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                            SP_Desc.Border = iTextSharp.text.Rectangle.BOX;
                            SP_Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                            SP_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            fieldCntl_Headers.AddCell(SP_Desc);
                            int x = 0;
                            foreach (CASESP2Entity CAEntity in CAMA_Details)
                            {
                                if (CAEntity.Type1 == "CA")
                                {
                                    if (x > 0)
                                    {
                                        PdfPCell CA_Desc_Space = new PdfPCell(new Phrase("", FTblFontBold));
                                        CA_Desc_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                                        CA_Desc_Space.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        CA_Desc_Space.Border = iTextSharp.text.Rectangle.BOX;
                                        CA_Desc_Space.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                        CA_Desc_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        fieldCntl_Headers.AddCell(CA_Desc_Space);
                                    }
                                    x = 1;

                                    PdfPCell CA_Desc = new PdfPCell(new Phrase(CAEntity.CAMS_Desc, FTblFontBold));
                                    CA_Desc.HorizontalAlignment = Element.ALIGN_CENTER;
                                    CA_Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    CA_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                    CA_Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                    CA_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    fieldCntl_Headers.AddCell(CA_Desc);

                                    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, SP_Hierarchies[0].Code, SP_Hierarchies[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                                    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                                    if (propPMTFLDCNTLHEntity.Count == 0)
                                    {
                                        propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                                        propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                                    }

                                    foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                                    {
                                        string isEnabled = string.Empty;
                                        string isRequired = string.Empty;

                                        if (propPMTFLDCNTLHEntity.Count > 0)
                                        {
                                            if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                                isEnabled = "Y";
                                            if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                                isRequired = "Y";
                                        }

                                        if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                        {
                                            PdfPTable fieldCntl_Data = new PdfPTable(3);
                                            fieldCntl_Data.TotalWidth = 36f;
                                            fieldCntl_Data.WidthPercentage = 100;
                                            fieldCntl_Data.LockedWidth = true;
                                            float[] YesNo_widths = new float[] { 12f, 12f, 12f };
                                            fieldCntl_Data.SetWidths(YesNo_widths);

                                            PdfPCell Enabled = new PdfPCell(new Phrase(isEnabled.ToString().Trim(), FTableFont));
                                            Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            Enabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //Enabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                            Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            fieldCntl_Data.AddCell(Enabled);

                                            PdfPCell Required = new PdfPCell(new Phrase(isRequired.ToString().Trim(), FTableFont));
                                            Required.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            Required.Border = iTextSharp.text.Rectangle.BOX;
                                            Required.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                            Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            fieldCntl_Data.AddCell(Required);

                                            PdfPCell Disabled = new PdfPCell(new Phrase("", FTableFont));
                                            Disabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Disabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            Disabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //Disabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                            Disabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            fieldCntl_Data.AddCell(Disabled);

                                            PdfPCell NestedHeaders_Data = new PdfPCell(fieldCntl_Data);
                                            NestedHeaders_Data.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedHeaders_Data.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                            fieldCntl_Headers.AddCell(NestedHeaders_Data);
                                        }
                                    }
                                }
                            }

                        }

                        if (fieldCntl_Headers.Rows.Count > 0)
                            document.Add(fieldCntl_Headers);
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }


                #endregion


                AlertBox.Show("Report Generated Successfully");
                document.Close();
                fs.Close();
                fs.Dispose();

                if (chkbExcel.Checked)
                    On_SaveExcelForm_Closed(sender, e);

                //if (chkbSPFielCntl.Checked)
                //{
                //    ServicePlanFieldControl_Report();
                //}
            }
        }

        void GeneratePDF_OnlyPDF(string _filename)
        {
            #region FileName
            Random_Filename = null;
            string PdfName = "Pdf File";
            PdfName = _filename;
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


            #endregion

            #region Font styles
            BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcGray = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.DARK_GRAY);

            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 2, BaseColor.WHITE);
            iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 2, new BaseColor(26, 71, 119));
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
            iTextSharp.text.Font TableFonterr = new iTextSharp.text.Font(bf_calibri, 8, 0, BaseColor.RED);
            iTextSharp.text.Font TableFontWhite = new iTextSharp.text.Font(bf_calibri, 8, 2, BaseColor.WHITE);
            iTextSharp.text.Font BoldFont = new iTextSharp.text.Font(bf_calibri, 8, 1);
            iTextSharp.text.Font BoldUnderlineFont = new iTextSharp.text.Font(bf_calibri, 8, 3);
            iTextSharp.text.Font TableFontPrgNotes = new iTextSharp.text.Font(bf_calibri, 8, 0, new BaseColor(111, 48, 160));
            iTextSharp.text.Font TableFontPrgNotesBold = new iTextSharp.text.Font(bf_calibri, 8, 1, new BaseColor(111, 48, 160));
            iTextSharp.text.Font TableFontPrgNotesBoldUndline = new iTextSharp.text.Font(bf_calibri, 8, 5, new BaseColor(111, 48, 160));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font FTblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font FTableFont = new iTextSharp.text.Font(bf_calibri, 8);
            #endregion

            #region Cell Color Define
            BaseColor DarkBlue = new BaseColor(26, 71, 119);
            BaseColor SecondBlue = new BaseColor(184, 217, 255);
            BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);
            #endregion

            #region GetData
            string _servPlanID = null;
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
                _servPlanID = SelServicePlan;

            DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(_servPlanID, null, null, null);
            DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];
            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
            List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
            List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();
            List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP")
            {
                SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
            }
            // Order byHierarchy
            if (SP_Hierarchies.Count > 0)
            {
                SP_Hierarchies = SP_Hierarchies.OrderBy(x => x.Agency).ThenBy(x => x.Dept).ThenBy(x => x.Prog).ToList();
            }

            List<CommonEntity> AgyOBF_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0038", "", "", "", string.Empty);
            List<CommonEntity> AgyCOUNTY_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0102", "", "", "", string.Empty);

            List<RNGGAEntity> GoalEntity = new List<RNGGAEntity>();
            List<RNGSRGAEntity> SRGoalEntity = new List<RNGSRGAEntity>();
            string dtrCode= string.Empty, RAgy = string.Empty, dtrOutCode=string.Empty;
            if (cmbDteRnge.Items.Count > 0)
            {
                dtrCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                dtrOutCode = ((ListItem)cmbDteRnge.SelectedItem).ValueDisplayCode.ToString();
                RAgy = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();

                GoalEntity = _model.SPAdminData.Browse_RNGGA(dtrOutCode, RAgy, null, null, null);
                SRGoalEntity = _model.SPAdminData.Browse_RNGSRGA(dtrCode, RAgy, null, null, null);
            }
            #endregion

            FileStream fs = new FileStream(PdfName, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;
            string Pri_SP = string.Empty; string Branch = null, Priv_Branch = null; bool First = true; bool SPNum = true;
            foreach (CASESP1Entity Entity in SP_Hierarchies)
            {
                DataView dv = dtSP_CaseSP2.DefaultView;
                dv.RowFilter = "sp2_serviceplan=" + Entity.Code.ToString(); // drSP_Services["sp0_servicecode"].ToString();
                dv.Sort = "sp2_branch DESC";
                DataTable CaseSP2 = dv.ToTable();

                if (Entity.Code.ToString().Trim() != Pri_SP.Trim())
                {
                    PdfPTable Table = new PdfPTable(9);
                    Table.TotalWidth = 550f;
                    Table.WidthPercentage = 100;
                    Table.LockedWidth = true;
                    float[] Lastwidths = new float[] { 2f, 20f, 30f, 20f, 20f, 15f, 15f, 15f, 2f };
                    Table.SetWidths(Lastwidths);
                    Table.HorizontalAlignment = Element.ALIGN_CENTER;
                    Table.SpacingBefore = 10f;

                    #region Main Header
                    PdfPCell Cell = new PdfPCell(new Phrase(" ", TableFont));
                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell.BackgroundColor = DarkBlue;
                    Table.AddCell(Cell);

                    PdfPCell Cell1 = new PdfPCell(new Phrase(" Service Plan", HeaderTextfnt));//(new Phrase(" Service Plan - Program Process ", HeaderTextfnt));
                    Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell1.BackgroundColor = DarkBlue;
                    Cell1.Colspan = 7;
                    Table.AddCell(Cell1);

                    PdfPCell Cell2 = new PdfPCell(new Phrase(" ", TableFont));
                    Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2.BackgroundColor = DarkBlue;
                    Table.AddCell(Cell2);
                    #endregion

                    #region Program Association
                    string strAgyDesc = ""; string strDeptDesc = ""; string strProgDesc = ""; string strProgShortName = "";
                    string strHIE = ""; string strHIEDesc = "";

                    List<CASESP1Entity> sp_Hierachcyselectsp = SP_Hierarchies.FindAll(u => u.Code.Trim() == Entity.Code.Trim());
                    if (sp_Hierachcyselectsp.Count > 0)
                    {
                        strHIE = sp_Hierachcyselectsp[0].Agency + "-" + sp_Hierachcyselectsp[0].Dept + "-" + sp_Hierachcyselectsp[0].Prog.Trim();

                        if (sp_Hierachcyselectsp[0].Agency + sp_Hierachcyselectsp[0].Dept + sp_Hierachcyselectsp[0].Prog == "******")
                            strProgDesc = "All Hierarchies";
                        else
                            strProgDesc = "Not Defined in CASEHIE";

                        DataSet ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, sp_Hierachcyselectsp[0].Prog.Trim());
                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                            {
                                strHIEDesc = strProgDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                strProgShortName = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_SHORT_NAME"].ToString()).Trim();
                            }
                        }

                        ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, "**");
                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                strDeptDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        }

                        ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, "**", "**");
                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                strAgyDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        }

                        if (sp_Hierachcyselectsp[0].Dept == "**")
                            strDeptDesc = "All Departments";
                        if (sp_Hierachcyselectsp[0].Prog.Trim() == "**")
                            strProgDesc = "All Programs";
                    }

                    #endregion

                    for (int x = 0; x < 8; x++)
                    {
                        if (x == 0 || x == 7)
                        {
                            PdfPCell MCellRL = new PdfPCell(new Phrase(" ", TableFont));
                            MCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                            MCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            MCellRL.BackgroundColor = DarkBlue;
                            Table.AddCell(MCellRL);
                        }
                        else
                        {
                            string _txt = "";
                            if (x == 1)
                                _txt = strHIE;
                            if (x == 2)
                            {
                                _txt = Entity.SP_Desc;

                                //_txt = strHIEDesc;
                            }
                            //if (x == 3)
                            //    _txt = strProgShortName;


                            PdfPCell CellData = new PdfPCell(new Phrase(_txt, SubHeadTextfnt));
                            CellData.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            if (x == 2)
                                CellData.Colspan = 2;
                            CellData.BackgroundColor = SecondBlue;
                            Table.AddCell(CellData);
                        }
                    }

                    PdfPCell EmptyCell = new PdfPCell(new Phrase("", TableFont));
                    EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    EmptyCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    EmptyCell.BackgroundColor = DarkBlue;
                    EmptyCell.Colspan = 9;
                    Table.AddCell(EmptyCell);

                    #region Page Paramters
                    for (int xx = 0; xx < 14; xx++)//(int xx = 0; xx < 11; xx++)
                    {
                        PdfPCell CellLB = new PdfPCell(new Phrase(" ", BoldFont));
                        CellLB.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellLB.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellLB.BackgroundColor = DarkBlue;
                        Table.AddCell(CellLB);

                        string _strHead = ""; string _strDesc = "";
                        iTextSharp.text.Font _fntParams = TableFont;
                        iTextSharp.text.Font _fntParamsHeader = BoldFont;
                        if (xx == 0)
                        {
                            _strHead = "Agency :";
                            _strDesc = strAgyDesc;
                        }
                        if (xx == 1)
                        {
                            _strHead = "Department :";
                            _strDesc = strDeptDesc;
                        }
                        if (xx == 2)
                        {
                            _strHead = "Prorgram :";
                            _strDesc = strProgDesc;
                        }
                        if (xx == 3)
                        {
                            _strHead = "Date Description :";
                            if (cmbDteRnge.Items.Count > 0)
                                _strDesc = ((ListItem)cmbDteRnge.SelectedItem).ScreenCode.ToString();
                            else
                                _strDesc = "";
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }
                        if (xx == 4)
                        {
                            _strHead = "Date Range :";
                            if (cmbDteRnge.Items.Count > 0)
                                _strDesc = ((ListItem)cmbDteRnge.SelectedItem).Text.ToString().Substring(0, 23);
                            else
                                _strDesc="";
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }
                        if (xx == 5)
                        {
                            _strHead = "Funding Sources :";
                            string _strFunds = "";
                            if (FundingList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(Entity.Funds.ToString().Trim()))
                                {
                                    foreach (SPCommonEntity funditem in FundingList)
                                    {
                                        if (Entity.Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                        {
                                            _strFunds += funditem.Code.ToString() + ",";
                                        }
                                    }
                                }
                            }
                            _strDesc = _strFunds.TrimEnd(',');
                        }
                        if (xx == 6)
                        {
                            _strHead = "Branches :";
                            _strDesc = getBranches4PDF(CaseSP2, Entity);
                        }
                        if (xx == 7)
                        {
                            _strHead = "Run Date :";
                            _strDesc = DateTime.Now.ToString("MM/dd/yyyy");
                        }
                        if (xx == 8)
                        {
                            if (chkbTargets.Checked)
                            {
                                _strHead = "Targets :";
                                _strDesc = "Included";
                            }
                            else
                            {
                                _strHead = "Targets :";
                                _strDesc = "Not Included";
                            }
                        }
                        if (xx == 9)
                        {
                            if (chkPrgProcess.Checked)
                            {
                                _strHead = "Program Process :";
                                _strDesc = "Included";
                            }
                            else
                            {
                                _strHead = "Program Process :";
                                _strDesc = "Not Included";
                            }
                        }
                        if (xx == 10)
                        {
                            if (chkbSPFielCntl.Checked)
                            {
                                _strHead = "Field Control Settings :";
                                _strDesc = "Included";
                            }
                            else
                            {
                                _strHead = "Field Control Settings :";
                                _strDesc = "Not Included";
                            }
                        }
                        if (xx == 11)
                        {
                            _strHead = "Current Process";
                            _strDesc = "";
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBoldUndline;
                        }
                        if (xx == 12)
                        {
                            _strHead = "Created :";
                            if (sp_Hierachcyselectsp.Count > 0)
                                _strDesc = Convert.ToDateTime(sp_Hierachcyselectsp[0].Dateadd).ToString("MM/dd/yyyy") + " by " + sp_Hierachcyselectsp[0].addoperator;
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }
                        if (xx == 13)
                        {
                            _strHead = "Modified :";
                            if (sp_Hierachcyselectsp.Count > 0)
                                _strDesc = Convert.ToDateTime(sp_Hierachcyselectsp[0].DateLstc).ToString("MM/dd/yyyy") + " by " + sp_Hierachcyselectsp[0].lstcOperator;
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }



                        PdfPCell CellLHead = new PdfPCell(new Phrase(_strHead, _fntParamsHeader));
                        CellLHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                        CellLHead.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table.AddCell(CellLHead);

                        PdfPCell CellLDesc = new PdfPCell(new Phrase(_strDesc, _fntParams));
                        CellLDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellLDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellLDesc.Colspan = 6;
                        Table.AddCell(CellLDesc);

                        PdfPCell CellRB = new PdfPCell(new Phrase(" ", TableFont));
                        CellRB.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellRB.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellRB.BackgroundColor = DarkBlue;
                        Table.AddCell(CellRB);
                    }
                    #endregion

                    #region Table Data Header Creation
                    for (int x = 0; x < 9; x++)
                    {
                        if (x == 0 || x == 8)
                        {
                            PdfPCell TCellRL = new PdfPCell(new Phrase(" ", TableFont));
                            TCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                            TCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            TCellRL.BackgroundColor = DarkBlue;
                            Table.AddCell(TCellRL);
                        }
                        else
                        {
                            string _txt = "";
                            if (x == 1)
                                _txt = "Type";
                            if (x == 2)
                                _txt = "Description";
                            if (x == 3)
                                _txt = "Recipient";
                            if (x == 4)
                                _txt = "Aggregate Fun.";
                            if (x == 5)
                                _txt = "Target";
                            if (x == 6)
                                _txt = "Add Date";
                            if (x == 7)
                                _txt = "Change Date";

                            if (chkbTargets.Checked)
                            {
                                PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldFont));
                                if (x == 6 || x == 7 || x == 4 || x == 5)
                                    CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                //else if (x == 5)
                                //    CellData.HorizontalAlignment = Element.ALIGN_RIGHT;

                                CellData.Border = iTextSharp.text.Rectangle.BOX;
                                CellData.BackgroundColor = SecondBlue;
                                Table.AddCell(CellData);
                            }
                            else
                            {
                                if (x != 5)
                                {
                                    PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldFont));
                                    if (x == 6 || x == 7)
                                        CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                    else if (x == 4)
                                    {
                                        CellData.Colspan = 2;
                                        CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                    }
                                    CellData.Border = iTextSharp.text.Rectangle.BOX;
                                    CellData.BackgroundColor = SecondBlue;
                                    Table.AddCell(CellData);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Table Data  Creation
                    bool CAMS = true; Branch = null; Priv_Branch = null; First = true; string strBranchDesc = "";
                    if (CaseSP2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CaseSP2.Rows)
                        {

                            Branch = dr["sp2_branch"].ToString().Trim();
                            if (Branch != Priv_Branch)
                            {
                                if (Branch != "P")
                                    First = false;
                                string Service_desc = Entity.BpDesc.ToString(); //drSP_Services["sp0_pbranch_desc"].ToString();
                                if (!First)
                                {
                                    if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                                        Service_desc = Entity.B1Desc;
                                    else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                                        Service_desc = Entity.B2Desc;
                                    else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                                        Service_desc = Entity.B3Desc;
                                    else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                                        Service_desc = Entity.B4Desc.ToString();
                                    else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                                        Service_desc = Entity.B5Desc.ToString();
                                    else if (Branch.Trim() == "9")
                                        Service_desc = "Additional Branch";

                                }
                                CAMS = true;

                                string _strType = ""; string _type = "";

                                if (SPNum)
                                {
                                    _strType = "Service :";
                                    _type = Entity.SP_Desc.Trim();
                                }
                                if (!CAMS)
                                    _strType = "";
                                else
                                {
                                    _strType = "Branch :";
                                    _type = Service_desc.Trim();
                                }

                                strBranchDesc += _type + ", ";
                                for (int x = 0; x < 9; x++)
                                {
                                    if (x == 0 || x == 8)
                                    {
                                        PdfPCell TCellRL = new PdfPCell(new Phrase(" ", TableFont));
                                        TCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                                        TCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        TCellRL.BackgroundColor = DarkBlue;
                                        Table.AddCell(TCellRL);
                                    }
                                    else
                                    {
                                        string _txt = "";
                                        if (x == 1)
                                            _txt = _strType;
                                        if (x == 2)
                                            _txt = _type;

                                        if (chkbTargets.Checked)
                                        {
                                            PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldUnderlineFont));
                                            if (x == 6 || x == 7 || x == 4 || x == 5)
                                            {
                                                CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                            }
                                            //else if (x == 5)
                                            //    CellData.HorizontalAlignment = Element.ALIGN_RIGHT;

                                            CellData.Border = iTextSharp.text.Rectangle.BOX;
                                            //CellData.BackgroundColor = SecondBlue;
                                            Table.AddCell(CellData);
                                        }
                                        else
                                        {
                                            if (x != 5)
                                            {
                                                PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldUnderlineFont));
                                                if (x == 6 || x == 7)
                                                    CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                                else if (x == 4)
                                                {
                                                    CellData.Colspan = 2;
                                                    CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                                }

                                                CellData.Border = iTextSharp.text.Rectangle.BOX;
                                                Table.AddCell(CellData);
                                            }
                                        }
                                    }
                                }
                                Priv_Branch = Branch;
                                First = false; SPNum = false;
                            }

                            string CAMSType = "Service"; string CAMSDesc = "";
                            if (dr["sp2_type"].ToString() != "CA")
                                CAMSType = "Outcome";
                            string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty;
                            string ServiceCount = string.Empty;

                            bool Active = true;
                            if (dr["sp2_type"].ToString() == "CA")
                            {
                                List<CAMASTEntity> _lstDesc = CAList.Where(x => x.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim()).ToList();
                                if (_lstDesc.Count > 0)
                                {
                                    CAMSDesc = _lstDesc[0].Desc.ToString().Trim();
                                    if (_lstDesc[0].Active == "False")
                                        Active = false;
                                }

                                if (chkbTargets.Checked)
                                {
                                    RNGSRGAEntity GAEntity = SRGoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                    if (GAEntity != null)
                                        ServiceCount = GAEntity.Budget.ToString();
                                }
                            }
                            else
                            {
                                List<MSMASTEntity> _lstMSDesc = MSList.Where(x => x.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim()).ToList();
                                if (_lstMSDesc.Count > 0)
                                {
                                    CAMSDesc = _lstMSDesc[0].Desc.ToString().Trim();
                                    if (_lstMSDesc[0].Active == "False")
                                        Active = false;
                                }

                                if (chkbTargets.Checked)
                                {
                                    RNGGAEntity GAEntity = GoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                    if (GAEntity != null)
                                        ServiceCount = GAEntity.Budget.ToString();
                                }
                            }

                            bool _isActive = true;
                            if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                _isActive = false;

                            for (int x = 0; x < 9; x++)
                            {
                                if (x == 0 || x == 8)
                                {
                                    PdfPCell TCellRL = new PdfPCell(new Phrase(" ", TableFont));
                                    TCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    TCellRL.BackgroundColor = DarkBlue;
                                    Table.AddCell(TCellRL);
                                }
                                else
                                {
                                    string _txt = "";
                                    if (x == 1)
                                        _txt = CAMSType;
                                    if (x == 2)
                                        _txt = CAMSDesc;
                                    if (x == 3)
                                    {
                                        string strReciepent = "";
                                        if (AgyOBF_List.Count > 0)
                                        {
                                            List<CommonEntity> AgyOBF_List1 = AgyOBF_List.Where(u => u.Code == dr["SP2_OBF"].ToString().Trim()).ToList();
                                            if (AgyOBF_List1.Count > 0)
                                                strReciepent = AgyOBF_List1[0].Desc.ToString();
                                        }

                                        _txt = strReciepent;
                                    }
                                    if (x == 4)
                                    {
                                        string strCounty = "";
                                        if (AgyCOUNTY_List.Count > 0)
                                        {
                                            List<CommonEntity> AgyCOUNTY_List1 = AgyCOUNTY_List.Where(u => u.Code == dr["SP2_COUNT_TYPE"].ToString().Trim()).ToList();
                                            if (AgyCOUNTY_List1.Count > 0)
                                                strCounty = AgyCOUNTY_List1[0].Desc.ToString();
                                        }
                                        _txt = strCounty;
                                    }
                                    if (x == 5)
                                    {
                                        _txt = ServiceCount;
                                    }
                                    if (x == 6)
                                    {
                                        _txt = LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim());
                                    }
                                    if (x == 7)
                                    {
                                        _txt = LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim());
                                    }

                                    if (chkbTargets.Checked)
                                    {
                                        PdfPCell CellData = new PdfPCell(new Phrase(_txt, (_isActive == true ? TableFont : TableFonterr)));
                                        if (x == 1 || x == 6 || x == 7 || x == 4 || x == 5)
                                            CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //else if (x == 5)
                                        //    CellData.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        CellData.Border = iTextSharp.text.Rectangle.BOX;
                                        //CellData.BackgroundColor = SecondBlue;
                                        Table.AddCell(CellData);
                                    }
                                    else
                                    {
                                        if (x != 5)
                                        {
                                            PdfPCell CellData = new PdfPCell(new Phrase(_txt, (_isActive == true ? TableFont : TableFonterr)));
                                            if (x == 1 || x == 6 || x == 7)
                                                CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                            else if (x == 4)
                                            {
                                                CellData.Colspan = 2;
                                                CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                            }
                                            CellData.Border = iTextSharp.text.Rectangle.BOX;
                                            Table.AddCell(CellData);
                                        }
                                    }
                                }
                            }
                        }
                        if (chkPrgProcess.Checked)
                        {
                            PdfPCell PP1 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                            PP1.HorizontalAlignment = Element.ALIGN_LEFT;
                            PP1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PP1.BackgroundColor = DarkBlue;
                            Table.AddCell(PP1);

                            PdfPCell PP2 = new PdfPCell(new Phrase("Program process :  ", TableFontPrgNotesBoldUndline));
                            PP2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            PP2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Table.AddCell(PP2);

                            // GET CASENOTES of SERVICE PLAN PROCESS
                            /*****************************************************************************/
                            string _strPrgPRocessNotes = " - ";
                            string ScreenCode = "ADMN0030"; string _strSPCode = "000000".Substring(0, (6 - Entity.Code.ToString().Length)) + Entity.Code.ToString();
                            string RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                            string KeyField = BaseForm.BaseAgency + "_" + RNGCode + "_" + _strSPCode;
                            List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode, KeyField);
                            if (caseNotesEntity.Count > 0)
                            {
                                _strPrgPRocessNotes = caseNotesEntity[0].Data.ToString();
                            }

                            PdfPCell PP4 = new PdfPCell(new Phrase(_strPrgPRocessNotes, TableFontPrgNotes));
                            PP4.HorizontalAlignment = Element.ALIGN_LEFT;
                            PP4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PP4.Colspan = 6;
                            Table.AddCell(PP4);

                            PdfPCell PP5 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                            PP5.HorizontalAlignment = Element.ALIGN_LEFT;
                            PP5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PP5.BackgroundColor = DarkBlue;
                            Table.AddCell(PP5);


                            /*** COMMUNICATION HISTORY OF SELECTED SP ***/
                            DataTable dtHist = TmsApcn.GETPROCHIST(ScreenCode, KeyField, "");
                            if (dtHist.Rows.Count > 0)
                            {

                                int _cnt = 0;
                                foreach (DataRow _dr in dtHist.Rows)
                                {
                                    string _commHist = "";
                                    if (_cnt == 0)
                                    {
                                        _commHist = "Communications History : ";
                                        _cnt = 1;
                                    }

                                    PdfPCell CH1 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                                    CH1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CH1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CH1.BackgroundColor = DarkBlue;
                                    Table.AddCell(CH1);

                                    PdfPCell CH2 = new PdfPCell(new Phrase(_commHist, TableFontPrgNotesBoldUndline));
                                    CH2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    CH2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Table.AddCell(CH2);

                                    PdfPCell CH3 = new PdfPCell(new Phrase(_dr["PRH_DATE"].ToString() + "    " + _dr["PRH_USER_ID"].ToString(), TableFontPrgNotes));
                                    CH3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    CH3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Table.AddCell(CH3);

                                    PdfPCell CH4 = new PdfPCell(new Phrase(_dr["PRH_short_Notes"].ToString(), TableFontPrgNotes));
                                    CH4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CH4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CH4.Colspan = 5;
                                    Table.AddCell(CH4);

                                    PdfPCell CH5 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                                    CH5.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CH5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CH5.BackgroundColor = DarkBlue;
                                    Table.AddCell(CH5);

                                }
                            }

                        }
                    }
                    #endregion

                    PdfPCell CelllL = new PdfPCell(new Phrase(" ", TableFont));
                    CelllL.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelllL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CelllL.BackgroundColor = DarkBlue;
                    Table.AddCell(CelllL);

                    PdfPCell CellML = new PdfPCell(new Phrase("©2024 CAPSystems Inc", TableFontWhite));
                    CellML.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellML.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellML.BackgroundColor = DarkBlue;
                    CellML.Colspan = 7;
                    Table.AddCell(CellML);

                    PdfPCell CellRL = new PdfPCell(new Phrase(" ", TableFont));
                    CellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellRL.BackgroundColor = DarkBlue;
                    Table.AddCell(CellRL);


                    document.Add(Table);
                    if (Table.Rows.Count > 0)
                    {
                        //document.Add(Table);
                        Table.DeleteBodyRows();
                        //document.Add(Table1);
                        //Table1.DeleteBodyRows();
                        document.NewPage();
                    }
                    Pri_SP = Entity.Code.Trim();
                }
            }

            AlertBox.Show("Report Generated Successfully");
            document.Close();
            fs.Close();
            fs.Dispose();
            //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, PdfName);
            //pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            //pdfListForm.ShowDialog();

            PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();
        }

        void GeneratePDF(string _filename)
        {
            #region FileName
            Random_Filename = null;
            string PdfName = "Pdf File";
            PdfName = _filename;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
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


            #endregion

            #region Font styles
            BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcGray = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.DARK_GRAY);

            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 2, BaseColor.WHITE);
            iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 2, new BaseColor(26, 71, 119));
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
            iTextSharp.text.Font TableFonterr = new iTextSharp.text.Font(bf_calibri, 8, 0, BaseColor.RED);
            iTextSharp.text.Font TableFontWhite = new iTextSharp.text.Font(bf_calibri, 8, 2, BaseColor.WHITE);
            iTextSharp.text.Font BoldFont = new iTextSharp.text.Font(bf_calibri, 8, 1);
            iTextSharp.text.Font BoldUnderlineFont = new iTextSharp.text.Font(bf_calibri, 8, 3);
            iTextSharp.text.Font TableFontPrgNotes = new iTextSharp.text.Font(bf_calibri, 8, 0, new BaseColor(111, 48, 160));
            iTextSharp.text.Font TableFontPrgNotesBold = new iTextSharp.text.Font(bf_calibri, 8, 1, new BaseColor(111, 48, 160));
            iTextSharp.text.Font TableFontPrgNotesBoldUndline = new iTextSharp.text.Font(bf_calibri, 8, 5, new BaseColor(111, 48, 160));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font FTblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font FTableFont = new iTextSharp.text.Font(bf_calibri, 8);
            #endregion

            #region Cell Color Define
            BaseColor DarkBlue = new BaseColor(26, 71, 119);
            BaseColor SecondBlue = new BaseColor(184, 217, 255);
            BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);
            #endregion

            #region GetData
            string _servPlanID = null;
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
                _servPlanID = SelServicePlan;

            DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(_servPlanID, null, null, null);
            DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];
            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
            List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
            List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();
            List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
            if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP")
            {
                SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
            }
            // Order byHierarchy
            if (SP_Hierarchies.Count > 0)
            {
                SP_Hierarchies = SP_Hierarchies.OrderBy(x => x.Agency).ThenBy(x => x.Dept).ThenBy(x => x.Prog).ToList();
            }

            List<CommonEntity> AgyOBF_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0038", "", "", "", string.Empty);
            List<CommonEntity> AgyCOUNTY_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0102", "", "", "", string.Empty);

            List<RNGGAEntity> GoalEntity = new List<RNGGAEntity>();
            List<RNGSRGAEntity> SRGoalEntity = new List<RNGSRGAEntity>();
            string dtrCode = string.Empty, RAgy = string.Empty, dtrOutCode = string.Empty;
            if (cmbDteRnge.Items.Count > 0)
            {
                dtrCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                dtrOutCode = ((ListItem)cmbDteRnge.SelectedItem).ValueDisplayCode.ToString();
                RAgy = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();

                GoalEntity = _model.SPAdminData.Browse_RNGGA(dtrOutCode, RAgy, null, null, null);
                SRGoalEntity = _model.SPAdminData.Browse_RNGSRGA(dtrCode, RAgy, null, null, null);
            }
            #endregion

            FileStream fs = new FileStream(PdfName, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;
            string Pri_SP = string.Empty;
            string Branch = null, Priv_Branch = null;
            bool First = true;
            bool SPNum = true;
            foreach (CASESP1Entity Entity in SP_Hierarchies)
            {
                DataView dv = dtSP_CaseSP2.DefaultView;
                dv.RowFilter = "sp2_serviceplan=" + Entity.Code.ToString(); // drSP_Services["sp0_servicecode"].ToString();
                dv.Sort = "sp2_branch DESC";
                DataTable CaseSP2 = dv.ToTable();

                if (Entity.Code.ToString().Trim() != Pri_SP.Trim())
                {
                    PdfPTable Table = new PdfPTable(9);
                    Table.TotalWidth = 550f;
                    Table.WidthPercentage = 100;
                    Table.LockedWidth = true;
                    float[] Lastwidths = new float[] { 2f, 20f, 30f, 20f, 20f, 15f, 15f, 15f, 2f };
                    Table.SetWidths(Lastwidths);
                    Table.HorizontalAlignment = Element.ALIGN_CENTER;
                    Table.SpacingBefore = 10f;

                    #region Main Header
                    PdfPCell Cell = new PdfPCell(new Phrase(" ", TableFont));
                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell.BackgroundColor = DarkBlue;
                    Table.AddCell(Cell);

                    PdfPCell Cell1 = new PdfPCell(new Phrase(" Service Plan - Program Process ", HeaderTextfnt));
                    Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell1.BackgroundColor = DarkBlue;
                    Cell1.Colspan = 7;
                    Table.AddCell(Cell1);

                    PdfPCell Cell2 = new PdfPCell(new Phrase(" ", TableFont));
                    Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2.BackgroundColor = DarkBlue;
                    Table.AddCell(Cell2);
                    #endregion

                    #region Program Association
                    string strAgyDesc = "";
                    string strDeptDesc = "";
                    string strProgDesc = "";
                    string strProgShortName = "";
                    string strHIE = "";
                    string strHIEDesc = "";

                    List<CASESP1Entity> sp_Hierachcyselectsp = SP_Hierarchies.FindAll(u => u.Code.Trim() == Entity.Code.Trim());
                    if (sp_Hierachcyselectsp.Count > 0)
                    {
                        strHIE = sp_Hierachcyselectsp[0].Agency + "-" + sp_Hierachcyselectsp[0].Dept + "-" + sp_Hierachcyselectsp[0].Prog.Trim();

                        if (sp_Hierachcyselectsp[0].Agency + sp_Hierachcyselectsp[0].Dept + sp_Hierachcyselectsp[0].Prog == "******")
                            strProgDesc = "All Hierarchies";
                        else
                            strProgDesc = "Not Defined in CASEHIE";

                        DataSet ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, sp_Hierachcyselectsp[0].Prog.Trim());
                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                            {
                                strHIEDesc = strProgDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                strProgShortName = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_SHORT_NAME"].ToString()).Trim();
                            }
                        }

                        ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, "**");
                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                strDeptDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        }

                        ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, "**", "**");
                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                        {
                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                strAgyDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        }

                        if (sp_Hierachcyselectsp[0].Dept == "**")
                            strDeptDesc = "All Departments";
                        if (sp_Hierachcyselectsp[0].Prog.Trim() == "**")
                            strProgDesc = "All Programs";
                    }

                    #endregion

                    for (int x = 0; x < 8; x++)
                    {
                        if (x == 0 || x == 7)
                        {
                            PdfPCell MCellRL = new PdfPCell(new Phrase(" ", TableFont));
                            MCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                            MCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            MCellRL.BackgroundColor = DarkBlue;
                            Table.AddCell(MCellRL);
                        }
                        else
                        {
                            string _txt = "";
                            if (x == 1)
                                _txt = strHIE;
                            if (x == 2)
                                _txt = strHIEDesc;
                            if (x == 3)
                                _txt = strProgShortName;


                            PdfPCell CellData = new PdfPCell(new Phrase(_txt, SubHeadTextfnt));
                            CellData.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            if (x == 2)
                                CellData.Colspan = 2;
                            CellData.BackgroundColor = SecondBlue;
                            Table.AddCell(CellData);
                        }
                    }

                    PdfPCell EmptyCell = new PdfPCell(new Phrase("", TableFont));
                    EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    EmptyCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    EmptyCell.BackgroundColor = DarkBlue;
                    EmptyCell.Colspan = 9;
                    Table.AddCell(EmptyCell);

                    #region Page Paramters
                    for (int xx = 0; xx < 11; xx++)
                    {
                        PdfPCell CellLB = new PdfPCell(new Phrase(" ", BoldFont));
                        CellLB.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellLB.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellLB.BackgroundColor = DarkBlue;
                        Table.AddCell(CellLB);

                        string _strHead = "";
                        string _strDesc = "";
                        iTextSharp.text.Font _fntParams = TableFont;
                        iTextSharp.text.Font _fntParamsHeader = BoldFont;
                        if (xx == 0)
                        {
                            _strHead = "Agency :";
                            _strDesc = strAgyDesc;
                        }
                        if (xx == 1)
                        {
                            _strHead = "Department :";
                            _strDesc = strDeptDesc;
                        }
                        if (xx == 2)
                        {
                            _strHead = "Prorgram :";
                            _strDesc = strProgDesc;
                        }
                        if (xx == 3)
                        {
                            _strHead = "Date Description :";
                            if (cmbDteRnge.Items.Count > 0)
                                _strDesc = ((ListItem)cmbDteRnge.SelectedItem).ScreenCode.ToString();
                            else
                                _strDesc = "";
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }
                        if (xx == 4)
                        {
                            _strHead = "Date Range :";
                            if (cmbDteRnge.Items.Count > 0)
                                _strDesc = ((ListItem)cmbDteRnge.SelectedItem).Text.ToString().Substring(0, 23);
                            else
                                _strDesc = "";
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }
                        if (xx == 5)
                        {
                            _strHead = "Funding Sources :";
                            string _strFunds = "";
                            if (FundingList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(Entity.Funds.ToString().Trim()))
                                {
                                    foreach (SPCommonEntity funditem in FundingList)
                                    {
                                        if (Entity.Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                        {
                                            _strFunds += funditem.Code.ToString() + ",";
                                        }
                                    }
                                }
                            }
                            _strDesc = _strFunds.TrimEnd(',');
                        }
                        if (xx == 6)
                        {
                            _strHead = "Branches :";
                            _strDesc = getBranches4PDF(CaseSP2, Entity);
                        }
                        if (xx == 7)
                        {
                            _strHead = "Run Date :";
                            _strDesc = DateTime.Now.ToString("MM/dd/yyyy");
                        }
                        if (xx == 8)
                        {
                            _strHead = "Current Process";
                            _strDesc = "";
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBoldUndline;
                        }
                        if (xx == 9)
                        {
                            _strHead = "Created :";
                            if (sp_Hierachcyselectsp.Count > 0)
                                _strDesc = Convert.ToDateTime(sp_Hierachcyselectsp[0].Dateadd).ToString("MM/dd/yyyy") + " by " + sp_Hierachcyselectsp[0].addoperator;
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }
                        if (xx == 10)
                        {
                            _strHead = "Modified :";
                            if (sp_Hierachcyselectsp.Count > 0)
                                _strDesc = Convert.ToDateTime(sp_Hierachcyselectsp[0].DateLstc).ToString("MM/dd/yyyy") + " by " + sp_Hierachcyselectsp[0].lstcOperator;
                            _fntParams = TableFontPrgNotes;
                            _fntParamsHeader = TableFontPrgNotesBold;
                        }



                        PdfPCell CellLHead = new PdfPCell(new Phrase(_strHead, _fntParamsHeader));
                        CellLHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                        CellLHead.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table.AddCell(CellLHead);

                        PdfPCell CellLDesc = new PdfPCell(new Phrase(_strDesc, _fntParams));
                        CellLDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellLDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellLDesc.Colspan = 6;
                        Table.AddCell(CellLDesc);

                        PdfPCell CellRB = new PdfPCell(new Phrase(" ", TableFont));
                        CellRB.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellRB.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        CellRB.BackgroundColor = DarkBlue;
                        Table.AddCell(CellRB);
                    }
                    #endregion

                    #region Table Data Header Creation
                    for (int x = 0; x < 9; x++)
                    {
                        if (x == 0 || x == 8)
                        {
                            PdfPCell TCellRL = new PdfPCell(new Phrase(" ", TableFont));
                            TCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                            TCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            TCellRL.BackgroundColor = DarkBlue;
                            Table.AddCell(TCellRL);
                        }
                        else
                        {
                            string _txt = "";
                            if (x == 1)
                                _txt = "Type";
                            if (x == 2)
                                _txt = "Description";
                            if (x == 3)
                                _txt = "Recipient";
                            if (x == 4)
                                _txt = "Aggregate Fun.";
                            if (x == 5)
                                _txt = "Target";
                            if (x == 6)
                                _txt = "Add Date";
                            if (x == 7)
                                _txt = "Change Date";

                            if (chkbTargets.Checked)
                            {
                                PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldFont));
                                if (x == 6 || x == 7)
                                    CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                else if (x == 5)
                                    CellData.HorizontalAlignment = Element.ALIGN_RIGHT;

                                CellData.Border = iTextSharp.text.Rectangle.BOX;
                                CellData.BackgroundColor = SecondBlue;
                                Table.AddCell(CellData);
                            }
                            else
                            {
                                if (x != 5)
                                {
                                    PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldFont));
                                    if (x == 6 || x == 7)
                                        CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                    else if (x == 4)
                                    {
                                        CellData.Colspan = 2;
                                        //CellData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                    CellData.Border = iTextSharp.text.Rectangle.BOX;
                                    CellData.BackgroundColor = SecondBlue;
                                    Table.AddCell(CellData);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Table Data  Creation
                    bool CAMS = true;
                    Branch = null;
                    Priv_Branch = null;
                    First = true;
                    string strBranchDesc = "";
                    if (CaseSP2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CaseSP2.Rows)
                        {

                            Branch = dr["sp2_branch"].ToString().Trim();
                            if (Branch != Priv_Branch)
                            {
                                if (Branch != "P")
                                    First = false;
                                string Service_desc = Entity.BpDesc.ToString(); //drSP_Services["sp0_pbranch_desc"].ToString();
                                if (!First)
                                {
                                    if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                                        Service_desc = Entity.B1Desc;
                                    else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                                        Service_desc = Entity.B2Desc;
                                    else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                                        Service_desc = Entity.B3Desc;
                                    else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                                        Service_desc = Entity.B4Desc.ToString();
                                    else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                                        Service_desc = Entity.B5Desc.ToString();
                                    else if (Branch.Trim() == "9")
                                        Service_desc = "Additional Branch";

                                }
                                CAMS = true;

                                string _strType = "";
                                string _type = "";

                                if (SPNum)
                                {
                                    _strType = "Service :";
                                    _type = Entity.SP_Desc.Trim();
                                }
                                if (!CAMS)
                                    _strType = "";
                                else
                                {
                                    _strType = "Branch :";
                                    _type = Service_desc.Trim();
                                }

                                strBranchDesc += _type + ", ";
                                for (int x = 0; x < 9; x++)
                                {
                                    if (x == 0 || x == 8)
                                    {
                                        PdfPCell TCellRL = new PdfPCell(new Phrase(" ", TableFont));
                                        TCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                                        TCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        TCellRL.BackgroundColor = DarkBlue;
                                        Table.AddCell(TCellRL);
                                    }
                                    else
                                    {
                                        string _txt = "";
                                        if (x == 1)
                                            _txt = _strType;
                                        if (x == 2)
                                            _txt = _type;

                                        if (chkbTargets.Checked)
                                        {
                                            PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldUnderlineFont));
                                            if (x == 6 || x == 7)
                                                CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                            else if (x == 5)
                                                CellData.HorizontalAlignment = Element.ALIGN_RIGHT;

                                            CellData.Border = iTextSharp.text.Rectangle.BOX;
                                            //CellData.BackgroundColor = SecondBlue;
                                            Table.AddCell(CellData);
                                        }
                                        else
                                        {
                                            if (x != 5)
                                            {
                                                PdfPCell CellData = new PdfPCell(new Phrase(_txt, BoldUnderlineFont));
                                                if (x == 6 || x == 7)
                                                    CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                                else if (x == 4)
                                                    CellData.Colspan = 2;

                                                CellData.Border = iTextSharp.text.Rectangle.BOX;
                                                Table.AddCell(CellData);
                                            }
                                        }
                                    }
                                }
                                Priv_Branch = Branch;
                                First = false;
                                SPNum = false;
                            }

                            string CAMSType = "Service";
                            string CAMSDesc = "";
                            if (dr["sp2_type"].ToString() != "CA")
                                CAMSType = "Outcome";
                            string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty;
                            string ServiceCount = string.Empty;

                            bool Active = true;
                            if (dr["sp2_type"].ToString() == "CA")
                            {
                                List<CAMASTEntity> _lstDesc = CAList.Where(x => x.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim()).ToList();
                                if (_lstDesc.Count > 0)
                                {
                                    CAMSDesc = _lstDesc[0].Desc.ToString().Trim();
                                    if (_lstDesc[0].Active == "False")
                                        Active = false;
                                }

                                if (chkbTargets.Checked)
                                {
                                    RNGSRGAEntity GAEntity = SRGoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                    if (GAEntity != null)
                                        ServiceCount = GAEntity.Budget.ToString();
                                }
                            }
                            else
                            {
                                List<MSMASTEntity> _lstMSDesc = MSList.Where(x => x.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim()).ToList();
                                if (_lstMSDesc.Count > 0)
                                {
                                    CAMSDesc = _lstMSDesc[0].Desc.ToString().Trim();
                                    if (_lstMSDesc[0].Active == "False")
                                        Active = false;
                                }

                                if (chkbTargets.Checked)
                                {
                                    RNGGAEntity GAEntity = GoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                    if (GAEntity != null)
                                        ServiceCount = GAEntity.Budget.ToString();
                                }
                            }

                            bool _isActive = true;
                            if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                _isActive = false;

                            for (int x = 0; x < 9; x++)
                            {
                                if (x == 0 || x == 8)
                                {
                                    PdfPCell TCellRL = new PdfPCell(new Phrase(" ", TableFont));
                                    TCellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TCellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    TCellRL.BackgroundColor = DarkBlue;
                                    Table.AddCell(TCellRL);
                                }
                                else
                                {
                                    string _txt = "";
                                    if (x == 1)
                                        _txt = CAMSType;
                                    if (x == 2)
                                        _txt = CAMSDesc;
                                    if (x == 3)
                                    {
                                        string strReciepent = "";
                                        if (AgyOBF_List.Count > 0)
                                        {
                                            List<CommonEntity> AgyOBF_List1 = AgyOBF_List.Where(u => u.Code == dr["SP2_OBF"].ToString().Trim()).ToList();
                                            if (AgyOBF_List1.Count > 0)
                                                strReciepent = AgyOBF_List1[0].Desc.ToString();
                                        }

                                        _txt = strReciepent;
                                    }
                                    if (x == 4)
                                    {
                                        string strCounty = "";
                                        if (AgyCOUNTY_List.Count > 0)
                                        {
                                            List<CommonEntity> AgyCOUNTY_List1 = AgyCOUNTY_List.Where(u => u.Code == dr["SP2_COUNT_TYPE"].ToString().Trim()).ToList();
                                            if (AgyCOUNTY_List1.Count > 0)
                                                strCounty = AgyCOUNTY_List1[0].Desc.ToString();
                                        }
                                        _txt = strCounty;
                                    }
                                    if (x == 5)
                                    {
                                        _txt = ServiceCount;
                                    }
                                    if (x == 6)
                                    {
                                        _txt = LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim());
                                    }
                                    if (x == 7)
                                    {
                                        _txt = LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim());
                                    }

                                    if (chkbTargets.Checked)
                                    {
                                        PdfPCell CellData = new PdfPCell(new Phrase(_txt, (_isActive == true ? TableFont : TableFonterr)));
                                        if (x == 1 || x == 6 || x == 7)
                                            CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                        else if (x == 5)
                                            CellData.HorizontalAlignment = Element.ALIGN_RIGHT;

                                        CellData.Border = iTextSharp.text.Rectangle.BOX;
                                        //CellData.BackgroundColor = SecondBlue;
                                        Table.AddCell(CellData);
                                    }
                                    else
                                    {
                                        if (x != 5)
                                        {
                                            PdfPCell CellData = new PdfPCell(new Phrase(_txt, (_isActive == true ? TableFont : TableFonterr)));
                                            if (x == 1 || x == 6 || x == 7)
                                                CellData.HorizontalAlignment = Element.ALIGN_CENTER;
                                            else if (x == 4)
                                            {
                                                CellData.Colspan = 2;
                                            }
                                            CellData.Border = iTextSharp.text.Rectangle.BOX;
                                            Table.AddCell(CellData);
                                        }
                                    }
                                }
                            }
                        }
                        if (chkPrgProcess.Checked)
                        {
                            PdfPCell PP1 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                            PP1.HorizontalAlignment = Element.ALIGN_LEFT;
                            PP1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PP1.BackgroundColor = DarkBlue;
                            Table.AddCell(PP1);

                            PdfPCell PP2 = new PdfPCell(new Phrase("Program process :  ", TableFontPrgNotesBoldUndline));
                            PP2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            PP2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Table.AddCell(PP2);

                            // GET CASENOTES of SERVICE PLAN PROCESS
                            /*****************************************************************************/
                            string _strPrgPRocessNotes = " - ";
                            string ScreenCode = "ADMN0030";
                            string _strSPCode = "000000".Substring(0, (6 - Entity.Code.ToString().Length)) + Entity.Code.ToString();
                            string RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                            string KeyField = BaseForm.BaseAgency + "_" + RNGCode + "_" + _strSPCode;
                            List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode, KeyField);
                            if (caseNotesEntity.Count > 0)
                            {
                                _strPrgPRocessNotes = caseNotesEntity[0].Data.ToString();
                            }

                            PdfPCell PP4 = new PdfPCell(new Phrase(_strPrgPRocessNotes, TableFontPrgNotes));
                            PP4.HorizontalAlignment = Element.ALIGN_LEFT;
                            PP4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PP4.Colspan = 6;
                            Table.AddCell(PP4);

                            PdfPCell PP5 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                            PP5.HorizontalAlignment = Element.ALIGN_LEFT;
                            PP5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PP5.BackgroundColor = DarkBlue;
                            Table.AddCell(PP5);


                            /*** COMMUNICATION HISTORY OF SELECTED SP ***/
                            DataTable dtHist = TmsApcn.GETPROCHIST(ScreenCode, KeyField, "");
                            if (dtHist.Rows.Count > 0)
                            {

                                int _cnt = 0;
                                foreach (DataRow _dr in dtHist.Rows)
                                {
                                    string _commHist = "";
                                    if (_cnt == 0)
                                    {
                                        _commHist = "Communications History : ";
                                        _cnt = 1;
                                    }

                                    PdfPCell CH1 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                                    CH1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CH1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CH1.BackgroundColor = DarkBlue;
                                    Table.AddCell(CH1);

                                    PdfPCell CH2 = new PdfPCell(new Phrase(_commHist, TableFontPrgNotesBoldUndline));
                                    CH2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    CH2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Table.AddCell(CH2);

                                    PdfPCell CH3 = new PdfPCell(new Phrase(_dr["PRH_DATE"].ToString() + "    " + _dr["PRH_USER_ID"].ToString(), TableFontPrgNotes));
                                    CH3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    CH3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Table.AddCell(CH3);

                                    PdfPCell CH4 = new PdfPCell(new Phrase(_dr["PRH_short_Notes"].ToString(), TableFontPrgNotes));
                                    CH4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CH4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CH4.Colspan = 5;
                                    Table.AddCell(CH4);

                                    PdfPCell CH5 = new PdfPCell(new Phrase(" ", TableFontPrgNotes));
                                    CH5.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CH5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CH5.BackgroundColor = DarkBlue;
                                    Table.AddCell(CH5);

                                }
                            }

                        }
                    }
                    #endregion



                    PdfPCell CelllL = new PdfPCell(new Phrase(" ", TableFont));
                    CelllL.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelllL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CelllL.BackgroundColor = DarkBlue;
                    Table.AddCell(CelllL);

                    PdfPCell CellML = new PdfPCell(new Phrase("©2024 CAPSystems Inc", TableFontWhite));
                    CellML.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellML.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellML.BackgroundColor = DarkBlue;
                    CellML.Colspan = 7;
                    Table.AddCell(CellML);

                    PdfPCell CellRL = new PdfPCell(new Phrase(" ", TableFont));
                    CellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellRL.BackgroundColor = DarkBlue;
                    Table.AddCell(CellRL);


                    document.Add(Table);
                    if (Table.Rows.Count > 0)
                    {
                        //document.Add(Table);
                        Table.DeleteBodyRows();
                        //document.Add(Table1);
                        //Table1.DeleteBodyRows();
                        document.NewPage();
                    }
                    #region Service Plan Field Control
                    //if (rbtnRptFormat3.Checked) //if (chkbSPFielCntl.Checked)
                    //{
                    //    string Catg = IsFieldPrint(Entity.Code.Trim());
                    //    if (Catg == "Y")
                    //    {
                    //        try
                    //        {
                    //            if (rbtnRptFormat3.Checked)
                    //            {
                    //                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    //                document.NewPage();


                    //                CAMA_Details = _model.SPAdminData.Browse_CASESP2(Entity.Code, null, null, null);
                    //                PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", null, null, null);

                    //                List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "PMTSTDFLDS");
                    //                List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "FLDCNTL");
                    //                List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                    //                ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                    //                int HeaderCount = 0;
                    //                foreach (PMTSTDFLDSEntity HEntity in pmtstdfldsentity)
                    //                {
                    //                    if (HEntity != null)
                    //                    {
                    //                        if (HEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && HEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && HEntity.PSTF_FLD_CODE != "S00022" && HEntity.PSTF_FLD_CODE != "S00023" && HEntity.PSTF_FLD_CODE != "S00212" && HEntity.PSTF_FLD_CODE != "S00216" && HEntity.PSTF_FLD_CODE != "S00206" && HEntity.PSTF_FLD_CODE != "S00207" && HEntity.PSTF_FLD_CODE != "S00208" && HEntity.PSTF_FLD_CODE != "S00209" && HEntity.PSTF_FLD_CODE != "S00210" && HEntity.PSTF_FLD_CODE != "S00211")
                    //                        {
                    //                            HeaderCount++;
                    //                        }
                    //                    }
                    //                }
                    //                float[] widths;
                    //                if (HeaderCount > 0)
                    //                {
                    //                    for (int i = 0; i <= HeaderCount; i++)
                    //                    {
                    //                        widths = new float[] { 100f };
                    //                    }
                    //                }


                    //                PdfPTable fieldCntl_Headers = new PdfPTable(HeaderCount + 2);
                    //                fieldCntl_Headers.TotalWidth = 750f;
                    //                fieldCntl_Headers.WidthPercentage = 100;
                    //                fieldCntl_Headers.LockedWidth = true;
                    //                float[] Header_widths = new float[] { 300f, 400f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                    //                fieldCntl_Headers.SetWidths(Header_widths);
                    //                //fieldCntl_Headers.SpacingAfter = 10f;

                    //                PdfPCell pdfMainHeader = new PdfPCell(new Phrase("Field Control Summary", fcRed));
                    //                pdfMainHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                pdfMainHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                pdfMainHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                pdfMainHeader.Colspan = HeaderCount + 2;
                    //                pdfMainHeader.FixedHeight = 20;
                    //                fieldCntl_Headers.AddCell(pdfMainHeader);

                    //                PdfPCell Headers0 = new PdfPCell(new Phrase("SPM", FTblFontBold));
                    //                Headers0.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                Headers0.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                Headers0.Border = iTextSharp.text.Rectangle.BOX;
                    //                Headers0.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                Headers0.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                fieldCntl_Headers.AddCell(Headers0);

                    //                PdfPCell Headers1 = new PdfPCell(new Phrase("Description", FTblFontBold));
                    //                Headers1.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                Headers1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                Headers1.Border = iTextSharp.text.Rectangle.BOX;
                    //                Headers1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                Headers1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                fieldCntl_Headers.AddCell(Headers1);

                    //                foreach (PMTSTDFLDSEntity H2Entity in pmtstdfldsentity)
                    //                {
                    //                    if (H2Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && H2Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && H2Entity.PSTF_FLD_CODE != "S00022" && H2Entity.PSTF_FLD_CODE != "S00023" && H2Entity.PSTF_FLD_CODE != "S00212" && H2Entity.PSTF_FLD_CODE != "S00216" && H2Entity.PSTF_FLD_CODE != "S00206" && H2Entity.PSTF_FLD_CODE != "S00207" && H2Entity.PSTF_FLD_CODE != "S00208" && H2Entity.PSTF_FLD_CODE != "S00209" && H2Entity.PSTF_FLD_CODE != "S00210" && H2Entity.PSTF_FLD_CODE != "S00211")
                    //                    {
                    //                        PdfPCell Headers = new PdfPCell(new Phrase(H2Entity.PSTF_DESC.ToString().Trim(), FTblFontBold));
                    //                        Headers.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                        Headers.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                        Headers.Border = iTextSharp.text.Rectangle.BOX;
                    //                        Headers.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                        Headers.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                        fieldCntl_Headers.AddCell(Headers);
                    //                    }
                    //                }

                    //                PdfPCell SPM = new PdfPCell(new Phrase("", FTblFontBold));
                    //                SPM.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                SPM.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                SPM.Border = iTextSharp.text.Rectangle.BOX;
                    //                SPM.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                SPM.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                fieldCntl_Headers.AddCell(SPM);

                    //                PdfPCell Desc = new PdfPCell(new Phrase("", FTblFontBold));
                    //                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                Desc.Border = iTextSharp.text.Rectangle.BOX;
                    //                Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                fieldCntl_Headers.AddCell(Desc);

                    //                foreach (PMTSTDFLDSEntity H3Entity in pmtstdfldsentity)
                    //                {
                    //                    if (H3Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && H3Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && H3Entity.PSTF_FLD_CODE != "S00022" && H3Entity.PSTF_FLD_CODE != "S00023" && H3Entity.PSTF_FLD_CODE != "S00212" && H3Entity.PSTF_FLD_CODE != "S00216" && H3Entity.PSTF_FLD_CODE != "S00206" && H3Entity.PSTF_FLD_CODE != "S00207" && H3Entity.PSTF_FLD_CODE != "S00208" && H3Entity.PSTF_FLD_CODE != "S00209" && H3Entity.PSTF_FLD_CODE != "S00210" && H3Entity.PSTF_FLD_CODE != "S00211")
                    //                    {
                    //                        PdfPTable fieldCntl_Headers2 = new PdfPTable(3);
                    //                        fieldCntl_Headers2.TotalWidth = 36f;
                    //                        fieldCntl_Headers2.WidthPercentage = 100;
                    //                        fieldCntl_Headers2.LockedWidth = true;
                    //                        float[] YesNo_widths = new float[] { 12f, 12f, 12f };
                    //                        fieldCntl_Headers2.SetWidths(YesNo_widths);

                    //                        PdfPCell Enabled = new PdfPCell(new Phrase("Enabled", FTblFontBold));
                    //                        Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                        Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                        Enabled.Rotation = 270;
                    //                        Enabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                        //Enabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                        Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                        fieldCntl_Headers2.AddCell(Enabled);

                    //                        PdfPCell Required = new PdfPCell(new Phrase("Required", FTblFontBold));
                    //                        Required.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                        Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                        Required.Rotation = 270;
                    //                        Required.Border = iTextSharp.text.Rectangle.BOX;
                    //                        Required.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                        Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                        fieldCntl_Headers2.AddCell(Required);

                    //                        PdfPCell Disabled = new PdfPCell(new Phrase("Disabled", FTblFontBold));
                    //                        Disabled.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                        Disabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                        Disabled.Rotation = 270;
                    //                        Disabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                        //Disabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                        Disabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                        fieldCntl_Headers2.AddCell(Disabled);

                    //                        PdfPCell NestedHeaders = new PdfPCell(fieldCntl_Headers2);
                    //                        NestedHeaders.Border = iTextSharp.text.Rectangle.BOX;
                    //                        NestedHeaders.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                        fieldCntl_Headers.AddCell(NestedHeaders);
                    //                    }
                    //                }

                    //                foreach (CASESP1Entity spEntity in SP_Hierarchies)
                    //                {
                    //                    PdfPCell SP_Desc = new PdfPCell(new Phrase(spEntity.SP_Desc, FTblFontBold));
                    //                    SP_Desc.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                    SP_Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                    SP_Desc.Border = iTextSharp.text.Rectangle.BOX;
                    //                    SP_Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                    SP_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                    fieldCntl_Headers.AddCell(SP_Desc);
                    //                    int x = 0;
                    //                    foreach (CASESP2Entity CAEntity in CAMA_Details)
                    //                    {
                    //                        if (CAEntity.Type1 == "CA")
                    //                        {
                    //                            if (x > 0)
                    //                            {
                    //                                PdfPCell CA_Desc_Space = new PdfPCell(new Phrase("", FTblFontBold));
                    //                                CA_Desc_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                                CA_Desc_Space.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                                CA_Desc_Space.Border = iTextSharp.text.Rectangle.BOX;
                    //                                CA_Desc_Space.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                                CA_Desc_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                                fieldCntl_Headers.AddCell(CA_Desc_Space);
                    //                            }
                    //                            x = 1;

                    //                            PdfPCell CA_Desc = new PdfPCell(new Phrase(CAEntity.CAMS_Desc, FTblFontBold));
                    //                            CA_Desc.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                            CA_Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                            CA_Desc.Border = iTextSharp.text.Rectangle.BOX;
                    //                            CA_Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                            CA_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //                            fieldCntl_Headers.AddCell(CA_Desc);

                    //                            propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, SP_Hierarchies[0].Code, SP_Hierarchies[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                    //                            propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                    //                            if (propPMTFLDCNTLHEntity.Count == 0)
                    //                            {
                    //                                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                    //                                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                    //                            }

                    //                            foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                    //                            {
                    //                                string isEnabled = string.Empty;
                    //                                string isRequired = string.Empty;

                    //                                if (propPMTFLDCNTLHEntity.Count > 0)
                    //                                {
                    //                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                    //                                        isEnabled = "Y";
                    //                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                    //                                        isRequired = "Y";
                    //                                }

                    //                                if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                    //                                {
                    //                                    PdfPTable fieldCntl_Data = new PdfPTable(3);
                    //                                    fieldCntl_Data.TotalWidth = 36f;
                    //                                    fieldCntl_Data.WidthPercentage = 100;
                    //                                    fieldCntl_Data.LockedWidth = true;
                    //                                    float[] YesNo_widths = new float[] { 12f, 12f, 12f };
                    //                                    fieldCntl_Data.SetWidths(YesNo_widths);

                    //                                    PdfPCell Enabled = new PdfPCell(new Phrase(isEnabled.ToString().Trim(), FTableFont));
                    //                                    Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                                    Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                                    Enabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                                    //Enabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                                    Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                    //                                    fieldCntl_Data.AddCell(Enabled);

                    //                                    PdfPCell Required = new PdfPCell(new Phrase(isRequired.ToString().Trim(), FTableFont));
                    //                                    Required.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                                    Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                                    Required.Border = iTextSharp.text.Rectangle.BOX;
                    //                                    Required.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                                    Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                    //                                    fieldCntl_Data.AddCell(Required);

                    //                                    PdfPCell Disabled = new PdfPCell(new Phrase("", FTableFont));
                    //                                    Disabled.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                                    Disabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //                                    Disabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                                    //Disabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                                    Disabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                    //                                    fieldCntl_Data.AddCell(Disabled);

                    //                                    PdfPCell NestedHeaders_Data = new PdfPCell(fieldCntl_Data);
                    //                                    NestedHeaders_Data.Border = iTextSharp.text.Rectangle.BOX;
                    //                                    NestedHeaders_Data.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    //                                    fieldCntl_Headers.AddCell(NestedHeaders_Data);
                    //                                }
                    //                            }
                    //                        }
                    //                    }

                    //                }

                    //                if (fieldCntl_Headers.Rows.Count > 0)
                    //                {
                    //                    document.Add(fieldCntl_Headers);
                    //                    document.NewPage();
                    //                }
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            string msg = ex.Message;
                    //        }



                    //    }
                    //}
                    #endregion
                    Pri_SP = Entity.Code.Trim();
                }
            }

            AlertBox.Show("Report Generated Successfully");
            document.Close();
            fs.Close();
            fs.Dispose();
            //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, PdfName);
            //pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            //pdfListForm.ShowDialog();

            PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();
        }

        string getBranches4PDF(DataTable _dtCaseSP2, CASESP1Entity Entity)
        {
            string _resBranches = ""; string Branch = "", Priv_Branch = null; bool SPNum = true;
            bool CAMS = true; Branch = null; Priv_Branch = null; bool First = true; string strBranchDesc = "";
            foreach (DataRow dr in _dtCaseSP2.Rows)
            {
                Branch = dr["sp2_branch"].ToString().Trim();
                if (Branch != Priv_Branch)
                {
                    string Service_desc = Entity.BpDesc.ToString();
                    if (Branch != "P")
                        First = false;

                    if (!First)
                    {
                        if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                            Service_desc = Entity.B1Desc;
                        else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                            Service_desc = Entity.B2Desc;
                        else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                            Service_desc = Entity.B3Desc;
                        else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                            Service_desc = Entity.B4Desc.ToString();
                        else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                            Service_desc = Entity.B5Desc.ToString();
                        else if (Branch.Trim() == "9")
                            Service_desc = "Additional Branch";

                    }
                    CAMS = true;

                    string _strType = ""; string _type = "";

                    if (SPNum)
                    {
                        _strType = "Service :";
                        _type = Entity.SP_Desc.Trim();
                    }
                    if (!CAMS)
                        _strType = "";
                    else
                    {
                        _strType = "Branch :";
                        _type = Service_desc.Trim();
                    }

                    strBranchDesc += _type + ", ";

                    Priv_Branch = Branch;
                }


            }
            if(!string.IsNullOrEmpty(strBranchDesc.Trim()))
                _resBranches = strBranchDesc.Substring(0, strBranchDesc.Length - 2);
            return _resBranches;
        }
        private void On_SaveForm_ClosedAll1(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (chkbExcel.Checked)
                    On_SaveAllExcelForm_Closed_Devexpress(sender, e);


                string strHiearchy = null;
                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
                {
                    SP_Hierarchies = _model.SPAdminData.Browse_CASESP1AndSp0(SelServicePlan, null, null, null);
                    if (BaseForm.BaseAdminAgency != "**")
                        SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Agency == BaseForm.BaseAdminAgency);

                }
                else
                {
                    strHiearchy = ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim();
                    if (strHiearchy == "**")
                        strHiearchy = null;
                    SP_Hierarchies = _model.SPAdminData.Browse_CASESP1AndSp0(null, strHiearchy, null, null);
                }

                if (Privileges.Program == "ADMN0030")
                {
                    if (chkbPDF.Checked && chkbExcel.Checked)
                        GeneratePDF(form.GetFileName());
                    else if ((chkbPDF.Checked && !chkbExcel.Checked))
                        GeneratePDF_OnlyPDF(form.GetFileName());
                }
                else
                {
                    GeneratePDF(form.GetFileName());
                }
            }
        }
        private void On_SaveForm_ClosedAll(object sender, FormClosedEventArgs e)
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

                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;
                string Name = BaseForm.BaseApplicationName;
                string AppNo = BaseForm.BaseApplicationNo;

                BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont bf_Times = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font fcGray = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.DARK_GRAY);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

                BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

                BaseFont Fbf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);

                BaseFont FbfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont FbfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
                iTextSharp.text.Font Ffc = new iTextSharp.text.Font(bfTimes, 12, 2);
                iTextSharp.text.Font Ffc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
                iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));

                iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
                iTextSharp.text.Font FTableFont = new iTextSharp.text.Font(bf_calibri, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
                iTextSharp.text.Font FTblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
                iTextSharp.text.Font TblFontHeadBold = new iTextSharp.text.Font(bfTimes, 12, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);



                PdfPTable Header = new PdfPTable(1);
                Header.TotalWidth = 500f;
                Header.WidthPercentage = 100;
                Header.LockedWidth = true;
                float[] Headerwidths = new float[] { 90f };
                Header.SetWidths(Headerwidths);
                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.SpacingAfter = 10f;


                PdfPTable table;
                if (chkbTargets.Checked)
                {
                    table = new PdfPTable(4);
                    table.TotalWidth = 530f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 80f, 25f, 25f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                }
                else
                {
                    table = new PdfPTable(3);
                    table.TotalWidth = 530f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 80f, 25f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                }

                int X_Pos, Y_Pos;

                //SP_Hierarchies = new List<CASESP1Entity>();
                //SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, ((RepListItem)cmbHie.SelectedItem).Value.ToString().Trim(), null, null);

                DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(null, null, null, null);
                DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];

                //dr["sp2_cams_code"].ToString().Trim()
                List<CAMASTEntity> CAList;
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                List<MSMASTEntity> MSList;
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

                //DataSet dsCAMAST = DatabaseLayer.SPAdminDB.Browse_CAMAST(null, null, null, null);
                //DataTable dtCAMAST=dsCAMAST.Tables[0];

                //DataSet MSMast = DatabaseLayer.SPAdminDB.Browse_MSMAST(null,null, null, null, null);
                //DataTable DtMSMast=MSMast.Tables[0];

                List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
                List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();

                List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");

                bool First = true; string MS_Type = string.Empty;
                string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null; bool IsFirst = true;
                if (SP_Hierarchies.Count > 0)
                {
                    //kranthi
                    //if (rbAllSP.Checked)
                    //{
                    //    SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
                    //}
                    if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP")
                    {
                        SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
                    }

                    var varSp = SP_Hierarchies.Select(u => u.Code).Distinct().ToList();
                    string Hiename = null; string AgencyName = string.Empty;
                    foreach (CASESP1Entity Entity in SP_Hierarchies)
                    {

                        //DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(Entity.Code, null, null, null, null, null, null, null, null);
                        //DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];



                        //HierarchyEntity hierchyEntity = null;
                        if (Entity.Agency + Entity.Dept + Entity.Prog == "******")
                        {
                            Hiename = "All Hierarchies";
                            //hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                        }
                        else
                            Hiename = "Not Defined in CASEHIE";

                        DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.Agency, Entity.Dept, Entity.Prog);
                        if (ds_Hie_Name.Tables.Count > 0)
                        {
                            if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                            {
                                if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                    Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                //hierchyEntity = new HierarchyEntity(Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Hiename);

                                if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                    ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                    ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";

                                //hierchyEntity = new HierarchyEntity(ds_Hie_Name.Tables[0].Rows[0], "CASEHIE");
                            }
                        }

                        if (!IsFirst)
                        {
                            table.DeleteBodyRows();
                            Priv_Branch = null;
                        }

                        IsFirst = false;

                        AgencyName = Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog + "  " + Hiename;

                        if (Entity.Sp0_Active == "Y" && Entity.SP_validated == "Y")
                        {
                            if (dtSP_CaseSP2.Rows.Count > 0 && Entity.Sp0_Active == "Y")
                            {
                                DataView dv = dtSP_CaseSP2.DefaultView;
                                dv.RowFilter = "sp2_serviceplan=" + Entity.Code.ToString();
                                dv.Sort = "sp2_branch DESC";
                                //dtSP_CaseSP2 = dv.ToTable();
                                DataTable CaseSP2 = dv.ToTable();
                                Y_Pos = 770;
                                X_Pos = 55;
                                //cb.BeginText();
                                int Count = 0;
                                foreach (DataRow dr in CaseSP2.Rows)
                                {
                                    SP_Plan_desc = Entity.SP_Desc.Trim();
                                    Branch = dr["sp2_branch"].ToString().Trim();
                                    if (Branch != Priv_Branch)
                                    {
                                        //cb.EndText();
                                        document.Add(table);
                                        //if (Count > 0)
                                        //{
                                        //    for (int i = 0; i <= Count; i++)
                                        //    {
                                        //        table.DeleteRow(i);
                                        //    }
                                        //}
                                        table.DeleteBodyRows();

                                        //table.DeleteRow(1);
                                        document.SetPageSize(iTextSharp.text.PageSize.A4);
                                        document.NewPage();
                                        if (Branch != "P")
                                            First = false;
                                        //cb.BeginText();
                                        string Service_desc = Entity.BpDesc.Trim();
                                        if (!First)
                                        {
                                            if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                                                Service_desc = Entity.B1Desc;
                                            else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                                                Service_desc = Entity.B2Desc;
                                            else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                                                Service_desc = Entity.B3Desc;
                                            else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                                                Service_desc = Entity.B4Desc.ToString();
                                            else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                                                Service_desc = Entity.B5Desc.ToString();
                                            else if (Branch.Trim() == "9")
                                                Service_desc = "Additional Branch";

                                        }
                                        Y_Pos = 770;
                                        X_Pos = 55;
                                        table.SpacingBefore = 10f;

                                        PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim() + " (" + Entity.Code + ")", fc1));
                                        Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                                        if (chkbTargets.Checked)
                                            Sp_Plan.Colspan = 4;
                                        else
                                            Sp_Plan.Colspan = 3;
                                        Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Sp_Plan);

                                        PdfPCell Sp_Hie = new PdfPCell(new Phrase("Hierarchy :" + AgencyName.Trim(), fcGray));
                                        Sp_Hie.HorizontalAlignment = Element.ALIGN_LEFT;
                                        if (chkbTargets.Checked)
                                            Sp_Hie.Colspan = 4;
                                        else
                                            Sp_Hie.Colspan = 3;
                                        Sp_Hie.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Sp_Hie);

                                        PdfPCell ServiceDesc = new PdfPCell(new Phrase("Branch :" + Service_desc.Trim(), TblFontBold));
                                        ServiceDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                        if (chkbTargets.Checked)
                                            ServiceDesc.Colspan = 4;
                                        else
                                            ServiceDesc.Colspan = 3;
                                        ServiceDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(ServiceDesc);

                                        if (chkbTargets.Checked)
                                        {
                                            string[] col = { "Type", "Description", "Code", "Target" };
                                            for (int i = 0; i < col.Length; ++i)
                                            {
                                                PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                                table.AddCell(cell);
                                            }
                                        }
                                        else
                                        {
                                            string[] col = { "Type", "Description", "Code" };
                                            for (int i = 0; i < col.Length; ++i)
                                            {
                                                PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                                table.AddCell(cell);
                                            }
                                        }

                                        Priv_Branch = Branch;
                                        First = false;
                                    }
                                    string CAMSType = dr["sp2_type"].ToString();
                                    string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty, strCAMS = string.Empty;
                                    string ProgTarget = string.Empty;
                                    string ServiceCount = string.Empty;
                                    if (CAMSType == "CA")
                                    {
                                        foreach (CAMASTEntity drCAMAST in CAList)
                                        {
                                            if (drCAMAST.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                            {
                                                CAMSDesc = drCAMAST.Desc.ToString().Trim();
                                                break;
                                            }
                                            else
                                                CAMSDesc = "";
                                        }

                                        //foreach(DataRow drCAMAST in dtCAMAST.Rows)
                                        //{
                                        //    if(drCAMAST["CA_CODE"].ToString().Trim()==dr["sp2_cams_code"].ToString().Trim())
                                        //    {
                                        //        CAMSDesc = drCAMAST["CA_DESC"].ToString();break;
                                        //    }
                                        //    else
                                        //        CAMSDesc="";
                                        //}

                                        //CAMSDesc = drCAMAST["CA_DESC"].ToString();

                                        if (chkbTargets.Checked)
                                        {
                                            DataSet ds = new DataSet();
                                            DataSet ds1 = new DataSet();
                                            DataTable dt = new DataTable();
                                            DataTable dt1 = new DataTable();
                                            DataTable dtserOut = new DataTable();

                                            if (cmbDteRnge.Items.Count > 0)
                                            {
                                                RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                            }

                                            SPCOde = Entity.Code;
                                            GoalSRHieEntity = _model.SPAdminData.Browse_RNGSRGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());

                                            ServiceCount = string.Empty;
                                            //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, SPCOde, string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                            //if (dsSerOut.Tables.Count > 0)
                                            //    dtserOut = dsSerOut.Tables[0];
                                            //if (dtserOut.Rows.Count > 0)
                                            //{
                                            //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                            //}

                                            if (GoalSRHieEntity.Count > 0)
                                            {
                                                GoalSRHieEntity = GoalSRHieEntity.FindAll(u => u.RNGSRGAH_TARGET != "" && u.RNGSRGAH_HIE == Entity.Agency + Entity.Dept + Entity.Prog);
                                                if (GoalSRHieEntity.Count > 0)
                                                    ServiceCount = GoalSRHieEntity.Sum(u => Convert.ToInt32(u.RNGSRGAH_TARGET.Trim())).ToString();
                                            }

                                        }

                                        PdfPCell RowType = new PdfPCell(new Phrase("Service", TableFont));
                                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowType);

                                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowDesc);

                                        PdfPCell RowCode = new PdfPCell(new Phrase(dr["sp2_cams_code"].ToString().Trim(), TableFont));
                                        RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowCode);

                                        if (chkbTargets.Checked)
                                        {
                                            PdfPCell RowTarget = new PdfPCell(new Phrase(ServiceCount.ToString().Trim(), TableFont));
                                            RowTarget.HorizontalAlignment = Element.ALIGN_CENTER;
                                            RowTarget.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(RowTarget);
                                        }
                                    }
                                    else
                                    {
                                        string Type_Desc = string.Empty;
                                        foreach (MSMASTEntity drMSMast in MSList)
                                        {
                                            if (drMSMast.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                            {
                                                CAMSDesc = drMSMast.Desc.ToString().Trim();
                                                MS_Type = drMSMast.Type1.ToString();
                                                //if (MS_Type == "M")
                                                //    Type_Desc = "Milestone";
                                                //else
                                                Type_Desc = "Outcome";
                                                break;
                                            }
                                            else
                                                CAMSDesc = "";
                                        }


                                        //foreach(DataRow drMSMast in DtMSMast.Rows)
                                        //{
                                        //    if (drMSMast["MS_CODE"].ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                        //    {
                                        //        CAMSDesc = drMSMast["MS_DESC"].ToString(); break;
                                        //    }
                                        //    else
                                        //        CAMSDesc = "";
                                        //}

                                        if (chkbTargets.Checked)
                                        {
                                            DataSet ds = new DataSet();
                                            DataSet ds1 = new DataSet();
                                            DataTable dt = new DataTable();
                                            DataTable dt1 = new DataTable();
                                            DataTable dtserOut = new DataTable();

                                            if (cmbDteRnge.Items.Count > 0)
                                            {
                                                RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                            }
                                            SPCOde = Entity.Code;
                                            GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());

                                            ServiceCount = string.Empty;
                                            //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, SPCOde, string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                            //if (dsSerOut.Tables.Count > 0)
                                            //    dtserOut = dsSerOut.Tables[0];
                                            //if (dtserOut.Rows.Count > 0)
                                            //{
                                            //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                            //}

                                            if (GoalHieEntity.Count > 0)
                                            {
                                                GoalHieEntity = GoalHieEntity.FindAll(u => u.RNGGAH_TARGET != "" && u.RNGGAH_HIE == Entity.Agency + Entity.Dept + Entity.Prog);
                                                if (GoalHieEntity.Count > 0)
                                                    ServiceCount = GoalHieEntity.Sum(u => Convert.ToInt32(u.RNGGAH_TARGET.Trim())).ToString();
                                            }

                                        }

                                        PdfPCell RowType = new PdfPCell(new Phrase(Type_Desc, TblFontBold));
                                        RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowType);

                                        PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TblFontBold));
                                        RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowDesc);

                                        PdfPCell RowCode = new PdfPCell(new Phrase(dr["sp2_cams_code"].ToString().Trim(), TblFontBold));
                                        RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(RowCode);

                                        if (chkbTargets.Checked)
                                        {
                                            PdfPCell RowTarget = new PdfPCell(new Phrase(ServiceCount.ToString().Trim(), TblFontBold));
                                            RowTarget.HorizontalAlignment = Element.ALIGN_CENTER;
                                            RowTarget.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(RowTarget);
                                        }
                                    }
                                }
                                Count++;

                            }

                            document.Add(Header);
                            if (table.Rows.Count > 0)
                            {
                                document.Add(table);

                                document.NewPage();
                                table.Rows.Clear();

                                PdfPTable table1 = new PdfPTable(3);
                                table1.TotalWidth = 530f;
                                table1.WidthPercentage = 100;
                                table1.LockedWidth = true;
                                float[] widths1 = new float[] { 70f, 20f, 70f };
                                table1.SetWidths(widths1);
                                table1.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPTable tablehie = new PdfPTable(2);
                                tablehie.TotalWidth = 220f;
                                tablehie.WidthPercentage = 100;
                                tablehie.LockedWidth = true;
                                float[] widthsHie = new float[] { 20f, 70f };
                                tablehie.SetWidths(widthsHie);
                                tablehie.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPTable tablefund = new PdfPTable(2);
                                tablefund.TotalWidth = 220f;
                                tablefund.WidthPercentage = 100;
                                tablefund.LockedWidth = true;
                                float[] widthsfund = new float[] { 20f, 70f };
                                tablefund.SetWidths(widthsfund);
                                tablefund.HorizontalAlignment = Element.ALIGN_CENTER;



                                PdfPCell cell = new PdfPCell(new Phrase("Programs Associated", TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.Border = iTextSharp.text.Rectangle.BOX;
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                cell.FixedHeight = 20f;
                                cell.Colspan = 2;
                                tablehie.AddCell(cell);

                                List<CASESP1Entity> SelSP1Hies = SP_Hierarchies.FindAll(u => u.Code.Trim() == Entity.Code.Trim());
                                foreach (CASESP1Entity selctsp1item in SelSP1Hies)
                                {
                                    //HierarchyEntity hierchyEntity = null;
                                    if (selctsp1item.Agency + selctsp1item.Dept + selctsp1item.Prog == "******")
                                    {
                                        Hiename = "All Hierarchies";
                                        //hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                                    }
                                    else
                                        Hiename = "Not Defined in CASEHIE";

                                    DataSet ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(selctsp1item.Agency, selctsp1item.Dept, selctsp1item.Prog);
                                    if (ds_Hie_Name1.Tables.Count > 0)
                                    {
                                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                        {
                                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                Hiename = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                            if (string.IsNullOrEmpty(ds_Hie_Name1.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                                ds_Hie_Name1.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                            if (string.IsNullOrEmpty(ds_Hie_Name1.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                                ds_Hie_Name1.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";
                                        }
                                    }

                                    PdfPCell RowCode = new PdfPCell(new Phrase(selctsp1item.Agency + "-" + selctsp1item.Dept + "-" + selctsp1item.Prog.Trim(), TableFont));
                                    RowCode.HorizontalAlignment = Element.ALIGN_LEFT;
                                    RowCode.Border = iTextSharp.text.Rectangle.BOX;
                                    RowCode.FixedHeight = 17f;
                                    tablehie.AddCell(RowCode);

                                    PdfPCell RowDesc = new PdfPCell(new Phrase(Hiename.Trim(), TableFont));
                                    RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                    RowDesc.FixedHeight = 17f;
                                    tablehie.AddCell(RowDesc);

                                    cell = new PdfPCell(new Phrase("", TblFontBold));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    cell.Colspan = 2;
                                    tablehie.AddCell(cell);

                                    if (FundingList.Count > 0)
                                    {

                                        if (!string.IsNullOrEmpty(SP_Hierarchies[0].Funds.ToString().Trim()))
                                        {
                                            cell = new PdfPCell(new Phrase("Funding Sources", TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            cell.Border = iTextSharp.text.Rectangle.BOX;
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                            cell.FixedHeight = 20f;
                                            cell.Colspan = 2;
                                            tablefund.AddCell(cell);



                                            foreach (SPCommonEntity funditem in FundingList)
                                            {
                                                if (SP_Hierarchies[0].Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                                {
                                                    PdfPCell RowCode1 = new PdfPCell(new Phrase(funditem.Code.ToString().Trim(), TableFont));
                                                    RowCode1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    RowCode1.Border = iTextSharp.text.Rectangle.BOX;
                                                    RowCode1.FixedHeight = 17f;
                                                    tablefund.AddCell(RowCode1);

                                                    PdfPCell RowDesc1 = new PdfPCell(new Phrase(funditem.Desc.Trim(), TableFont));
                                                    RowDesc1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    RowDesc1.Border = iTextSharp.text.Rectangle.BOX;
                                                    RowDesc1.FixedHeight = 17f;
                                                    tablefund.AddCell(RowDesc1);


                                                }
                                            }

                                            cell = new PdfPCell(new Phrase("", TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            cell.Colspan = 2;
                                            tablehie.AddCell(cell);
                                        }

                                    }

                                    PdfPCell Celltb = new PdfPCell(tablehie);
                                    Celltb.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Celltb);

                                    Celltb = new PdfPCell(new Phrase(""));
                                    Celltb.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Celltb);

                                    Celltb = new PdfPCell(tablefund);
                                    Celltb.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Celltb);


                                    PdfPCell Sp_Hie = new PdfPCell(table1);
                                    Sp_Hie.Colspan = 3;
                                    Sp_Hie.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Sp_Hie);

                                    if (table.Rows.Count > 0)
                                        document.Add(table);

                                }
                            }
                            else
                            {
                                document.NewPage();
                                cb.BeginText();
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 15);
                                cb.ShowTextAligned(Element.ALIGN_CENTER, "Critical Activities and MileStones are not Defined for the Service Plan" + SP_Plan_desc + "( " + Entity.Code + " )", 300, 600, 0);
                                cb.EndText();
                            }
                        }

                        #region Service Plan Field Control

                        if (chkbSPFielCntl.Checked)
                        {
                            string Catg = IsFieldPrint(Entity.Code.Trim());
                            if (Catg == "Y")
                            {
                                try
                                {
                                    if (chkbSPFielCntl.Checked)
                                    {
                                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                        document.NewPage();


                                        CAMA_Details = _model.SPAdminData.Browse_CASESP2(Entity.Code, null, null, null);
                                        PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);

                                        List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Ext_2, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "PMTSTDFLDS");
                                        List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "FLDCNTL");
                                        List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Ext_2, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                                        ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                                        int HeaderCount = 0;
                                        foreach (PMTSTDFLDSEntity HEntity in pmtstdfldsentity)
                                        {
                                            if (HEntity != null)
                                            {
                                                if (HEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && HEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && HEntity.PSTF_FLD_CODE != "S00022" && HEntity.PSTF_FLD_CODE != "S00023" && HEntity.PSTF_FLD_CODE != "S00212" && HEntity.PSTF_FLD_CODE != "S00216" && HEntity.PSTF_FLD_CODE != "S00206" && HEntity.PSTF_FLD_CODE != "S00207" && HEntity.PSTF_FLD_CODE != "S00208" && HEntity.PSTF_FLD_CODE != "S00209" && HEntity.PSTF_FLD_CODE != "S00210" && HEntity.PSTF_FLD_CODE != "S00211")
                                                {
                                                    HeaderCount++;
                                                }
                                            }
                                        }
                                        float[] widths;
                                        if (HeaderCount > 0)
                                        {
                                            for (int i = 0; i <= HeaderCount; i++)
                                            {
                                                widths = new float[] { 100f };
                                            }
                                        }


                                        PdfPTable fieldCntl_Headers = new PdfPTable(HeaderCount + 2);
                                        fieldCntl_Headers.TotalWidth = 750f;
                                        fieldCntl_Headers.WidthPercentage = 100;
                                        fieldCntl_Headers.LockedWidth = true;
                                        float[] Header_widths = new float[] { 300f, 400f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                                        fieldCntl_Headers.SetWidths(Header_widths);
                                        //fieldCntl_Headers.SpacingAfter = 10f;

                                        PdfPCell pdfMainHeader = new PdfPCell(new Phrase("Field Control Summary", fcRed));
                                        pdfMainHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfMainHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        pdfMainHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        pdfMainHeader.Colspan = HeaderCount + 2;
                                        pdfMainHeader.FixedHeight = 20;
                                        fieldCntl_Headers.AddCell(pdfMainHeader);

                                        PdfPCell Headers0 = new PdfPCell(new Phrase("SPM", FTblFontBold));
                                        Headers0.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headers0.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        Headers0.Border = iTextSharp.text.Rectangle.BOX;
                                        Headers0.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                        Headers0.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        fieldCntl_Headers.AddCell(Headers0);

                                        PdfPCell Headers1 = new PdfPCell(new Phrase("Description", FTblFontBold));
                                        Headers1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headers1.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        Headers1.Border = iTextSharp.text.Rectangle.BOX;
                                        Headers1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                        Headers1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        fieldCntl_Headers.AddCell(Headers1);

                                        foreach (PMTSTDFLDSEntity H2Entity in pmtstdfldsentity)
                                        {
                                            if (H2Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && H2Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && H2Entity.PSTF_FLD_CODE != "S00022" && H2Entity.PSTF_FLD_CODE != "S00023" && H2Entity.PSTF_FLD_CODE != "S00212" && H2Entity.PSTF_FLD_CODE != "S00216" && H2Entity.PSTF_FLD_CODE != "S00206" && H2Entity.PSTF_FLD_CODE != "S00207" && H2Entity.PSTF_FLD_CODE != "S00208" && H2Entity.PSTF_FLD_CODE != "S00209" && H2Entity.PSTF_FLD_CODE != "S00210" && H2Entity.PSTF_FLD_CODE != "S00211")
                                            {
                                                PdfPCell Headers = new PdfPCell(new Phrase(H2Entity.PSTF_DESC.ToString().Trim(), FTblFontBold));
                                                Headers.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Headers.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                Headers.Border = iTextSharp.text.Rectangle.BOX;
                                                Headers.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                Headers.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                fieldCntl_Headers.AddCell(Headers);
                                            }
                                        }

                                        PdfPCell SPM = new PdfPCell(new Phrase("", FTblFontBold));
                                        SPM.HorizontalAlignment = Element.ALIGN_LEFT;
                                        SPM.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        SPM.Border = iTextSharp.text.Rectangle.BOX;
                                        SPM.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                        SPM.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        fieldCntl_Headers.AddCell(SPM);

                                        PdfPCell Desc = new PdfPCell(new Phrase("", FTblFontBold));
                                        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        Desc.Border = iTextSharp.text.Rectangle.BOX;
                                        Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                        Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        fieldCntl_Headers.AddCell(Desc);

                                        foreach (PMTSTDFLDSEntity H3Entity in pmtstdfldsentity)
                                        {
                                            if (H3Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && H3Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && H3Entity.PSTF_FLD_CODE != "S00022" && H3Entity.PSTF_FLD_CODE != "S00023" && H3Entity.PSTF_FLD_CODE != "S00212" && H3Entity.PSTF_FLD_CODE != "S00216" && H3Entity.PSTF_FLD_CODE != "S00206" && H3Entity.PSTF_FLD_CODE != "S00207" && H3Entity.PSTF_FLD_CODE != "S00208" && H3Entity.PSTF_FLD_CODE != "S00209" && H3Entity.PSTF_FLD_CODE != "S00210" && H3Entity.PSTF_FLD_CODE != "S00211")
                                            {
                                                PdfPTable fieldCntl_Headers2 = new PdfPTable(3);
                                                fieldCntl_Headers2.TotalWidth = 36f;
                                                fieldCntl_Headers2.WidthPercentage = 100;
                                                fieldCntl_Headers2.LockedWidth = true;
                                                float[] YesNo_widths = new float[] { 12f, 12f, 12f };
                                                fieldCntl_Headers2.SetWidths(YesNo_widths);

                                                PdfPCell Enabled = new PdfPCell(new Phrase("Enabled", FTblFontBold));
                                                Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                Enabled.Rotation = 270;
                                                Enabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                //Enabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                fieldCntl_Headers2.AddCell(Enabled);

                                                PdfPCell Required = new PdfPCell(new Phrase("Required", FTblFontBold));
                                                Required.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                Required.Rotation = 270;
                                                Required.Border = iTextSharp.text.Rectangle.BOX;
                                                Required.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                fieldCntl_Headers2.AddCell(Required);

                                                PdfPCell Disabled = new PdfPCell(new Phrase("Disabled", FTblFontBold));
                                                Disabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Disabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                Disabled.Rotation = 270;
                                                Disabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                //Disabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                Disabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                fieldCntl_Headers2.AddCell(Disabled);

                                                PdfPCell NestedHeaders = new PdfPCell(fieldCntl_Headers2);
                                                NestedHeaders.Border = iTextSharp.text.Rectangle.BOX;
                                                NestedHeaders.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                fieldCntl_Headers.AddCell(NestedHeaders);
                                            }
                                        }

                                        foreach (CASESP1Entity spEntity in SP_Hierarchies)
                                        {
                                            PdfPCell SP_Desc = new PdfPCell(new Phrase(spEntity.SP_Desc, FTblFontBold));
                                            SP_Desc.HorizontalAlignment = Element.ALIGN_CENTER;
                                            SP_Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            SP_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                            SP_Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                            SP_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            fieldCntl_Headers.AddCell(SP_Desc);
                                            int x = 0;
                                            foreach (CASESP2Entity CAEntity in CAMA_Details)
                                            {
                                                if (CAEntity.Type1 == "CA")
                                                {
                                                    if (x > 0)
                                                    {
                                                        PdfPCell CA_Desc_Space = new PdfPCell(new Phrase("", FTblFontBold));
                                                        CA_Desc_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        CA_Desc_Space.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                        CA_Desc_Space.Border = iTextSharp.text.Rectangle.BOX;
                                                        CA_Desc_Space.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                        CA_Desc_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        fieldCntl_Headers.AddCell(CA_Desc_Space);
                                                    }
                                                    x = 1;

                                                    PdfPCell CA_Desc = new PdfPCell(new Phrase(CAEntity.CAMS_Desc, FTblFontBold));
                                                    CA_Desc.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    CA_Desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    CA_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                                    CA_Desc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                    CA_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    fieldCntl_Headers.AddCell(CA_Desc);

                                                    propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, SP_Hierarchies[0].Code, SP_Hierarchies[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                                                    propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                                                    if (propPMTFLDCNTLHEntity.Count == 0)
                                                    {
                                                        propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                                                        propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                                                    }

                                                    foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                                                    {
                                                        string isEnabled = string.Empty;
                                                        string isRequired = string.Empty;

                                                        if (propPMTFLDCNTLHEntity.Count > 0)
                                                        {
                                                            if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                                                isEnabled = "Y";
                                                            if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                                                isRequired = "Y";
                                                        }

                                                        if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                                        {
                                                            PdfPTable fieldCntl_Data = new PdfPTable(3);
                                                            fieldCntl_Data.TotalWidth = 36f;
                                                            fieldCntl_Data.WidthPercentage = 100;
                                                            fieldCntl_Data.LockedWidth = true;
                                                            float[] YesNo_widths = new float[] { 12f, 12f, 12f };
                                                            fieldCntl_Data.SetWidths(YesNo_widths);

                                                            PdfPCell Enabled = new PdfPCell(new Phrase(isEnabled.ToString().Trim(), FTableFont));
                                                            Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                                            Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                            Enabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            //Enabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                            Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            fieldCntl_Data.AddCell(Enabled);

                                                            PdfPCell Required = new PdfPCell(new Phrase(isRequired.ToString().Trim(), FTableFont));
                                                            Required.HorizontalAlignment = Element.ALIGN_CENTER;
                                                            Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                            Required.Border = iTextSharp.text.Rectangle.BOX;
                                                            Required.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                            Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            fieldCntl_Data.AddCell(Required);

                                                            PdfPCell Disabled = new PdfPCell(new Phrase("", FTableFont));
                                                            Disabled.HorizontalAlignment = Element.ALIGN_CENTER;
                                                            Disabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                            Disabled.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            //Disabled.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                            Disabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            fieldCntl_Data.AddCell(Disabled);

                                                            PdfPCell NestedHeaders_Data = new PdfPCell(fieldCntl_Data);
                                                            NestedHeaders_Data.Border = iTextSharp.text.Rectangle.BOX;
                                                            NestedHeaders_Data.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                                            fieldCntl_Headers.AddCell(NestedHeaders_Data);
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                        if (fieldCntl_Headers.Rows.Count > 0)
                                            document.Add(fieldCntl_Headers);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string msg = ex.Message;
                                }


                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    cb.BeginText();
                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 15);
                    cb.ShowTextAligned(Element.ALIGN_CENTER, "Hierarchy not Defined for this Service Plan", 300, 600, 0);
                    cb.EndText();
                }


                AlertBox.Show("Report Generated Successfully");
                document.Close();
                fs.Close();
                fs.Dispose();

                if (chkbExcel.Checked)
                    On_SaveAllExcelForm_Closed_Devexpress(sender, e);

                //if (chkbSPFielCntl.Checked)
                //{
                //    ServicePlanFieldControl_Report();
                //}

            }
        }

        private void rbSelSP_CheckedChanged(object sender, EventArgs e)
        {
            //Kranthi
            //if (rbSelSP.Checked)
            //{
            //    //pnlHie.Visible = false;

            //    pnlHie.Enabled = false;
            //}
        }

        private void rbtnRptFormat3_Click(object sender, EventArgs e)
        {
            if (chkbSPFielCntl.Checked)
            {
                chkbPDF.Enabled = false;
                chkbPDF.ReadOnly = true;
                chkbPDF.Checked = false;
                chkbPDF.ForeColor = System.Drawing.Color.Silver;
                chkbExcel.Checked = true;
            }
            else
            {
                chkbPDF.Enabled = true;
                chkbPDF.ReadOnly = false;
                chkbPDF.Checked = true;
                chkbPDF.ForeColor = System.Drawing.Color.Black;
                chkbExcel.Checked = false;
            }
        }

        private void rbtnRptFormat2_Click(object sender, EventArgs e)
        {
            if (chkPrgProcess.Checked)
            {
                chkbPDF.Enabled = true;
                chkbPDF.ReadOnly = false;
                chkbPDF.Checked = true;
                chkbPDF.ForeColor = System.Drawing.Color.Black;
                chkbExcel.Checked = false;
            }
            if (chkbSPFielCntl.Checked)
            {
                chkbPDF.Enabled = false;
                chkbPDF.ReadOnly = true;
                chkbPDF.Checked = false;
                chkbPDF.ForeColor = System.Drawing.Color.Silver;
                chkbExcel.Checked = true;
            }
        }

        private void rbtnRptFormat1_Click(object sender, EventArgs e)
        {
            //if (rbtnRptFormat1.Checked)
            //{
            //    chkbPDF.Enabled = true;
            //    chkbPDF.ReadOnly = false;
            //    chkbPDF.Checked = true;
            //    chkbExcel.Checked = false;
            //    chkbPDF.ForeColor = System.Drawing.Color.Black;
            //}
        }

        private void On_SaveExcelForm_Closed(object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;
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
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }

                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                CarlosAg.ExcelXmlWriter.Workbook book = new CarlosAg.ExcelXmlWriter.Workbook();

                this.GenerateStyles(book.Styles);



                //string SPCode = string.Empty;
                //SPCode = SPGrid.CurrentRow.Cells["Code"].Value.ToString().Trim();

                DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(SelServicePlan, null, null, null, null, null, null, null, null);
                //DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];



                DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(SelServicePlan, null, null, null);
                DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];

                List<CAMASTEntity> CAList;
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                List<MSMASTEntity> MSList;
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

                List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
                List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();
                List<RNGGAEntity> GoalEntity = new List<RNGGAEntity>();
                List<RNGSRGAEntity> SRGoalEntity = new List<RNGSRGAEntity>();
                string dtrCode = string.Empty, RAgy = string.Empty;
                if (cmbDteRnge.Items.Count > 0)
                {
                    dtrCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                    RAgy = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();

                    GoalEntity = _model.SPAdminData.Browse_RNGGA(dtrCode, RAgy, null, null, null);
                    SRGoalEntity = _model.SPAdminData.Browse_RNGSRGA(dtrCode, RAgy, null, null, null);
                }



                List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");

                bool First = true; string MS_Type = string.Empty;
                string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null;
                bool SPNum = true;
                CarlosAg.ExcelXmlWriter.Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;
                foreach (DataRow drSP_Services in dsSP_Services.Tables[0].Rows)
                {
                    if (dtSP_CaseSP2.Rows.Count > 0)
                    {
                        DataView dv = dtSP_CaseSP2.DefaultView;
                        dv.RowFilter = "sp2_serviceplan=" + drSP_Services["sp0_servicecode"].ToString();
                        dv.Sort = "sp2_branch DESC";
                        DataTable CaseSP2 = dv.ToTable();

                        string ReportName = drSP_Services["sp0_description"].ToString();//"Sheet1";

                        ReportName = ReportName.Replace("/", "");

                        ReportName = Regex.Replace(ReportName, @"[^a-zA-Z0-9\s]", "");

                        if (ReportName.Length >= 31)
                        {
                            ReportName = ReportName.Substring(0, 31);
                        }

                        //if (Count > 1) ReportName = "Sheet" + Count.ToString();

                        sheet = book.Worksheets.Add(ReportName);
                        sheet.Table.DefaultRowHeight = 14.25F;

                        sheet.Table.Columns.Add(200);
                        sheet.Table.Columns.Add(200);
                        sheet.Table.Columns.Add(250);
                        sheet.Table.Columns.Add(52);
                        if (Member_Activity == "Y")
                            sheet.Table.Columns.Add(75);
                        sheet.Table.Columns.Add(75);
                        sheet.Table.Columns.Add(75);
                        if (chkbTargets.Checked)
                            sheet.Table.Columns.Add(75);

                        Row0 = sheet.Table.Rows.Add();

                        SPNum = true;

                        //if (!SPNum)
                        //{
                        //sheet = book.Worksheets.Add(drSP_Services["sp0_description"].ToString().Trim());
                        //}

                        cell = Row0.Cells.Add("SPM", DataType.String, "s94");
                        cell = Row0.Cells.Add("Branch", DataType.String, "s94");
                        cell = Row0.Cells.Add("Description", DataType.String, "s94");
                        cell = Row0.Cells.Add("Type", DataType.String, "s94");
                        if (Member_Activity == "Y")
                            cell = Row0.Cells.Add("Category", DataType.String, "s94");
                        cell = Row0.Cells.Add("Add Date", DataType.String, "s94");
                        cell = Row0.Cells.Add("Change Date", DataType.String, "s94");
                        if (chkbTargets.Checked)
                            cell = Row0.Cells.Add("Target", DataType.String, "s94");

                        bool CAMS = true; Branch = null; Priv_Branch = null; First = true;
                        if (CaseSP2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in CaseSP2.Rows)
                            {
                                SP_Plan_desc = drSP_Services["sp0_description"].ToString().Trim();
                                Branch = dr["sp2_branch"].ToString().Trim();
                                if (Branch != Priv_Branch)
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    ////cb.EndText();
                                    //document.Add(table);
                                    //table.DeleteBodyRows();

                                    ////table.DeleteRow(1);


                                    //document.NewPage();
                                    if (Branch != "P")
                                        First = false;
                                    //cb.BeginText();
                                    string Service_desc = drSP_Services["sp0_pbranch_desc"].ToString();
                                    if (!First)
                                    {
                                        if (Branch.Trim() == drSP_Services["sp0_branch1_code"].ToString().Trim())
                                            Service_desc = drSP_Services["sp0_branch1_desc"].ToString();
                                        else if (Branch.Trim() == drSP_Services["sp0_branch2_code"].ToString().Trim())
                                            Service_desc = drSP_Services["sp0_branch2_desc"].ToString();
                                        else if (Branch.Trim() == drSP_Services["sp0_branch3_code"].ToString().Trim())
                                            Service_desc = drSP_Services["sp0_branch3_desc"].ToString();
                                        else if (Branch.Trim() == drSP_Services["sp0_branch4_code"].ToString().Trim())
                                            Service_desc = drSP_Services["sp0_branch4_desc"].ToString();
                                        else if (Branch.Trim() == drSP_Services["sp0_branch5_code"].ToString().Trim())
                                            Service_desc = drSP_Services["sp0_branch5_desc"].ToString();
                                        else if (Branch.Trim() == "9")
                                            Service_desc = "Additional Branch";

                                        Row0 = sheet.Table.Rows.Add();

                                    }
                                    CAMS = true;

                                    //cell.MergeAcross = 1;

                                    //cell = Row0.Cells.Add("Service :" + SP_Plan_desc.Trim(), DataType.String, "s95B");

                                    //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_add"].ToString().Trim()), DataType.String, "s95B");
                                    //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_lstc"].ToString().Trim()), DataType.String, "s95B");

                                    //PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim(), fc1));
                                    //Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //Sp_Plan.Colspan = 2;
                                    //Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Sp_Plan);

                                    if (SPNum)
                                        cell = Row0.Cells.Add("Service :" + drSP_Services["sp0_description"].ToString().Trim(), DataType.String, "s96");
                                    else cell = Row0.Cells.Add("", DataType.String, "s96");
                                    //Row0 = sheet.Table.Rows.Add();
                                    if (!CAMS) Row0.Cells.Add("", DataType.String, "s96");
                                    else cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    //cell.MergeAcross = 1;

                                    //cell = Row0.Cells.Add("", DataType.String, "s96");
                                    //cell = Row0.Cells.Add("", DataType.String, "s96");

                                    //PdfPCell ServiceDesc = new PdfPCell(new Phrase("Branch :" + Service_desc.Trim(), TblFontBold));
                                    //ServiceDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //ServiceDesc.Colspan = 2;
                                    //ServiceDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(ServiceDesc);

                                    //string[] col = { "Type", "Description" };
                                    //for (int i = 0; i < col.Length; ++i)
                                    //{
                                    //    PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                                    //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    //    table.AddCell(cell);
                                    //}
                                    Priv_Branch = Branch;
                                    First = false; SPNum = false;
                                }
                                string CAMSType = dr["sp2_type"].ToString();

                                string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty;
                                string ServiceCount = string.Empty;


                                if (CAMSType == "CA")
                                {
                                    bool Active = true;
                                    foreach (CAMASTEntity drCAMAST in CAList)
                                    {
                                        if (drCAMAST.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                        {
                                            CAMSDesc = drCAMAST.Desc.ToString().Trim();
                                            if (drCAMAST.Active == "False") Active = false;
                                            break;
                                        }
                                        else
                                            CAMSDesc = "";
                                    }

                                    if (chkbTargets.Checked)
                                    {
                                        if (cmbDteRnge.Items.Count > 0)
                                        {
                                            RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                            RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                        }

                                        //DataTable dtserOut = new DataTable();
                                        //ServiceCount = string.Empty;
                                        //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, SelServicePlan, string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                        //if (dsSerOut.Tables.Count > 0)
                                        //    dtserOut = dsSerOut.Tables[0];
                                        //if (dtserOut.Rows.Count > 0)
                                        //{
                                        //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                        //}

                                        RNGSRGAEntity GAEntity = SRGoalEntity.Find(u => u.SerSP == drSP_Services["sp0_servicecode"].ToString() && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                        if (GAEntity != null)
                                            ServiceCount = GAEntity.Budget.ToString();
                                        //GoalSRHieEntity = _model.SPAdminData.Browse_RNGSRGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());
                                        //if (GoalSRHieEntity.Count > 0)
                                        //{
                                        //    GoalSRHieEntity = GoalSRHieEntity.FindAll(u => u.RNGSRGAH_TARGET != "");
                                        //    if (GoalSRHieEntity.Count > 0)
                                        //        ServiceCount = GoalSRHieEntity.Sum(u => Convert.ToInt32(u.RNGSRGAH_TARGET.Trim())).ToString();
                                        //}
                                    }



                                    if (!CAMS)
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s95");
                                        cell = Row0.Cells.Add("", DataType.String, "s95");
                                    }

                                    if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                    {
                                        cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95R");
                                        cell = Row0.Cells.Add("Service", DataType.String, "s95R");
                                        if (Member_Activity == "Y")
                                            cell = Row0.Cells.Add(drSP_Services["SP0_Category"].ToString().Trim(), DataType.String, "s95R");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");
                                        if (chkbTargets.Checked)
                                            cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95R");
                                    }
                                    else
                                    {
                                        cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                                        cell = Row0.Cells.Add("Service", DataType.String, "s95");
                                        if (Member_Activity == "Y")
                                            cell = Row0.Cells.Add(drSP_Services["SP0_Category"].ToString().Trim(), DataType.String, "s95");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95");
                                        if (chkbTargets.Checked)
                                            cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95");
                                    }

                                    //PdfPCell RowType = new PdfPCell(new Phrase(CAMSType.Trim(), TableFont));
                                    //RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //RowType.Border = iTextSharp.text.Rectangle.BOX;
                                    //table.AddCell(RowType);

                                    //PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                                    //RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                    //table.AddCell(RowDesc);
                                    CAMS = false;
                                }
                                else
                                {
                                    string Type_Desc = string.Empty; bool Active = true;
                                    foreach (MSMASTEntity drMSMast in MSList)
                                    {
                                        if (drMSMast.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                        {
                                            CAMSDesc = drMSMast.Desc.ToString().Trim();
                                            MS_Type = drMSMast.Type1.ToString();
                                            if (drMSMast.Active == "False") Active = false;
                                            //if (MS_Type == "M")
                                            //    Type_Desc = "Milestone";
                                            //else
                                            Type_Desc = "Outcome";
                                            break;
                                        }
                                        else
                                            CAMSDesc = "";
                                    }

                                    if (chkbTargets.Checked)
                                    {
                                        if (cmbDteRnge.Items.Count > 0)
                                        {
                                            RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                            RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                        }

                                        RNGGAEntity GAEntity = GoalEntity.Find(u => u.SerSP == drSP_Services["sp0_servicecode"].ToString() && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                        if (GAEntity != null)
                                            ServiceCount = GAEntity.Budget.ToString();

                                        //GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());
                                        //if (GoalHieEntity.Count > 0)
                                        //{
                                        //    GoalHieEntity = GoalHieEntity.FindAll(u => u.RNGGAH_TARGET != "");
                                        //    if (GoalHieEntity.Count > 0)
                                        //        ServiceCount = GoalHieEntity.Sum(u => Convert.ToInt32(u.RNGGAH_TARGET.Trim())).ToString();
                                        //}

                                        ////DataTable dtserOut = new DataTable();
                                        ////ServiceCount = string.Empty;
                                        ////DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, SelServicePlan, string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                        ////if (dsSerOut.Tables.Count > 0)
                                        ////    dtserOut = dsSerOut.Tables[0];
                                        ////if (dtserOut.Rows.Count > 0)
                                        ////{
                                        ////    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                        ////}
                                    }

                                    if (!CAMS)
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s95");
                                        cell = Row0.Cells.Add("", DataType.String, "s95");
                                    }

                                    if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                    {
                                        cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95R");
                                        cell = Row0.Cells.Add(Type_Desc, DataType.String, "s95R");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");
                                        if (chkbTargets.Checked)
                                            cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95R");

                                    }
                                    else
                                    {
                                        cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                                        cell = Row0.Cells.Add(Type_Desc, DataType.String, "s95");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95");
                                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95");
                                        if (chkbTargets.Checked)
                                            cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95");
                                    }

                                    CAMS = false;
                                }
                            }

                        }
                        else
                        {
                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add("Service :" + drSP_Services["sp0_description"].ToString().Trim(), DataType.String, "s96");
                            string Service_desc = drSP_Services["sp0_pbranch_desc"].ToString();
                            cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");

                            if (!string.IsNullOrEmpty(drSP_Services["sp0_branch1_code"].ToString().Trim()))
                            {
                                Service_desc = drSP_Services["sp0_branch1_desc"].ToString();
                                if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add("", DataType.String, "s96");
                                    cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                }
                            }
                            if (!string.IsNullOrEmpty(drSP_Services["sp0_branch2_code"].ToString().Trim()))
                            {
                                Service_desc = drSP_Services["sp0_branch2_desc"].ToString();
                                if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add("", DataType.String, "s96");
                                    cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                }
                            }
                            if (!string.IsNullOrEmpty(drSP_Services["sp0_branch3_code"].ToString().Trim()))
                            {
                                Service_desc = drSP_Services["sp0_branch3_desc"].ToString();
                                if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add("", DataType.String, "s96");
                                    cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                }
                            }
                            if (!string.IsNullOrEmpty(drSP_Services["sp0_branch4_code"].ToString().Trim()))
                            {
                                Service_desc = drSP_Services["sp0_branch4_desc"].ToString();
                                if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add("", DataType.String, "s96");
                                    cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                }
                            }
                            if (!string.IsNullOrEmpty(drSP_Services["sp0_branch5_code"].ToString().Trim()))
                            {
                                Service_desc = drSP_Services["sp0_branch5_desc"].ToString();
                                if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                {
                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add("", DataType.String, "s96");
                                    cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                }
                            }

                        }
                        Count++;

                        Row0 = sheet.Table.Rows.Add();

                        cell = Row0.Cells.Add();
                        cell.StyleID = "s96";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = "";
                        cell.MergeAcross = 2;


                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add();
                        cell.StyleID = "s96";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = "Programs Associated";
                        cell.MergeAcross = 1;
                        cell = Row0.Cells.Add("", DataType.String, "s96");

                        string Hiename = null;
                        List<CASESP1Entity> sp_Hierachcyselectsp = SP_Hierarchies.FindAll(u => u.Code.Trim() == drSP_Services["sp0_servicecode"].ToString().Trim());
                        foreach (CASESP1Entity selctsp1item in sp_Hierachcyselectsp)
                        {
                            //HierarchyEntity hierchyEntity = null;
                            if (selctsp1item.Agency + selctsp1item.Dept + selctsp1item.Prog == "******")
                            {
                                Hiename = "All Hierarchies";
                                //hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                            }
                            else
                                Hiename = "Not Defined in CASEHIE";

                            DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(selctsp1item.Agency, selctsp1item.Dept, selctsp1item.Prog);
                            if (ds_Hie_Name.Tables.Count > 0)
                            {
                                if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                                {
                                    if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                        Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                    if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                        ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                    if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                        ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";
                                }
                            }


                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add(selctsp1item.Agency + "-" + selctsp1item.Dept + "-" + selctsp1item.Prog.Trim(), DataType.String, "s95");
                            cell = Row0.Cells.Add(Hiename.Trim(), DataType.String, "s95");
                            cell = Row0.Cells.Add("", DataType.String, "s95");


                            if (FundingList.Count > 0)
                            {

                                if (!string.IsNullOrEmpty(drSP_Services["SP0_FUNDS"].ToString().Trim()))
                                {


                                    Row0 = sheet.Table.Rows.Add();

                                    cell = Row0.Cells.Add();
                                    cell.StyleID = "s96";
                                    cell.Data.Type = DataType.String;
                                    cell.Data.Text = "";
                                    cell.MergeAcross = 2;


                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add();
                                    cell.StyleID = "s96";
                                    cell.Data.Type = DataType.String;
                                    cell.Data.Text = "Funding Sources";
                                    cell.MergeAcross = 1;
                                    cell = Row0.Cells.Add("", DataType.String, "s96");


                                    foreach (SPCommonEntity funditem in FundingList)
                                    {
                                        if (drSP_Services["SP0_FUNDS"].ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                        {
                                            Row0 = sheet.Table.Rows.Add();
                                            cell = Row0.Cells.Add(funditem.Code.ToString().Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add(funditem.Desc.Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add("", DataType.String, "s95");

                                        }
                                    }


                                }

                            }





                        }




                    }

                }

                if (chkbSPFielCntl.Checked)
                    this.GenerateWorksheetServiceCatg(book.Worksheets, SelServicePlan);
                //this.ServicePlanFieldControl_Sheet(book);


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "SPREPAPP_Report.xls";

                //// downloadGateway.Version = file.Version;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

                FileInfo fiDownload = new FileInfo(PdfName);
                /// Need to check for file exists, is local file, is allow to read, etc...
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }

                //if (chkbSPFielCntl.Checked && !chkbPDF.Checked)
                //{
                //    ServicePlanFieldControl_Report();
                //}
            }
        }


        private void On_SaveAllExcelForm_Closed_Devexpress(object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild
                /*************************************************************/
                Random_Filename = null;
                string xlFileName = form.GetFileName();
                xlFileName = propReportPath + BaseForm.UserID + "\\" + xlFileName;
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
                    string Tmpstr = xlFileName + ".xlsx";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = xlFileName + newFileName.Substring(0, length) + ".xlsx";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    xlFileName = Random_Filename;
                else
                    xlFileName += ".xlsx";

                /*************************************************************/
                string _excelPath = xlFileName;
                #endregion

                #region GetData
                string _servPlanID = null;
                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "SSP")
                    _servPlanID = SelServicePlan;

                DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(_servPlanID, null, null, null);
                DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];
                List<CAMASTEntity> CAList;
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                List<MSMASTEntity> MSList;
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
                List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
                List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();
                List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP")
                {
                    SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
                }

                // Order byHierarchy
                if (SP_Hierarchies.Count > 0)
                {
                    SP_Hierarchies = SP_Hierarchies.OrderBy(x => x.Agency).ThenBy(x => x.Dept).ThenBy(x => x.Prog).ToList();
                }


                //else if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "AISP")
                //{
                //SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
                //}
                List<CommonEntity> AgyOBF_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0038", "", "", "", string.Empty);
                List<CommonEntity> AgyCOUNTY_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0102", "", "", "", string.Empty);
                #endregion

                #region excelGeneration
                using (DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook())
                {
                    bool First = true; string MS_Type = string.Empty;
                    string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null;
                    bool SPNum = true;
                    CarlosAg.ExcelXmlWriter.Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;
                    string PrvRepName = ""; int i = 0; int k = 1; string Pri_SP = string.Empty;

                    #region Excel Cell Styles

                    /********************** SET EXCEL FONT PROPERTIES ******************************/
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = workbook;
                    oDevExpress_Excel_Properties.sxlTitleFont = "Trebuchet MS";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";
                    /*******************************************************************************/
                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    #region Custom Styles
                    DevExpress.Spreadsheet.Style xTitleCellstyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlTitleFont, 20, "#002060", true, "#ffffff", "center", 0, 0, 0, 0, 0, "Thin", "#F8F9D0", "");
                    DevExpress.Spreadsheet.Style xTitleCellstyle2 = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 14, "#0070C0", true, "#EEF5FC", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");
                    DevExpress.Spreadsheet.Style xsubTitleintakeCellstyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 10, "#0070C0", true, "#F8FBFE", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
                    DevExpress.Spreadsheet.Style xPageTitleCellstyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlTitleFont, 16, "#002060", true, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
                    DevExpress.Spreadsheet.Style reportNameStyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlTitleFont, 16, "#002060", true, "#CFE6F9", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");
                    DevExpress.Spreadsheet.Style paramsCellStyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 8, "#000000", false, "#EEF5FC", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");

                    DevExpress.Spreadsheet.Style xlFrameStyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#FFFFFF", false, "#1A4777", "left", 1, 1, 1, 1, 1, "Thin", "#1A4777", "");
                    DevExpress.Spreadsheet.Style xlFrameStyleR = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#FFFFFF", false, "#1A4777", "right", 0, 1, 1, 1, 1, "Thin", "#1A4777", "");
                    DevExpress.Spreadsheet.Style xlFrameBlockStyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", true, "#b8d9ff", "center", 1, 1, 1, 1, 1, "Thin", "#b8d9ff", "");
                    DevExpress.Spreadsheet.Style xlFrameFooterStyle = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 8, "#FFFFFF", false, "#1A4777", "right", 1, 1, 1, 1, 1, "Thin", "#1A4777", "");


                    DevExpress.Spreadsheet.Style gxlErrBLC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#ffffff", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
                    DevExpress.Spreadsheet.Style gxlErrBCC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
                    DevExpress.Spreadsheet.Style gxlErrNumb_NRC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");
                    DevExpress.Spreadsheet.Style gxlErrDeci_NRC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "D");
                    DevExpress.Spreadsheet.Style gxlGenerate_cr = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlTitleFont, 12, "#0070C0", false, "#FFFFFF", "right", 0, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    //Parameters  Styles ***/////
                    DevExpress.Spreadsheet.Style gxlNRC_bold_NB = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", false, "#FFFFFF", "right", 0, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
                    DevExpress.Spreadsheet.Style gxlNLC_NB = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", false, "#FFFFFF", "left", 0, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
                    DevExpress.Spreadsheet.Style gxlNLC_PRPCol_NB = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "left", 0, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
                    DevExpress.Spreadsheet.Style gxlNRC_PRPCol_NB_bold = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "right", 0, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
                    ////****************************///
                    DevExpress.Spreadsheet.Style gxlPrgNotes = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "left", 0, 0, 0, 0, 0, "None", "#d3e6f5", "");

                    DevExpress.Spreadsheet.Style gxlNCC_PRPCol_NB = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
                    DevExpress.Spreadsheet.Style gxlPRPCol_BCHC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#9bc2e6", "center", 1, 1, 1, 1, 1, "Thin", "#b0d4f5", "");

                    DevExpress.Spreadsheet.Style gxlBCHC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", true, "#B8D9FF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
                    DevExpress.Spreadsheet.Style gxlPRCHC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", true, "#B8D9FF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
                    DevExpress.Spreadsheet.Style gxlNLC_bo = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#BFBFBF", "");
                    DevExpress.Spreadsheet.Style gxlNLC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", false, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
                    DevExpress.Spreadsheet.Style gxlNCC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");
                    DevExpress.Spreadsheet.Style gxlNumb_NRC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", false, "#FFFFFF", "right", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");


                    #region Vikash added following styles 07/03/2024 as per per Service Plan clean - CONTINUED FROM PRIOR ITEM ABOVE point

                    DevExpress.Spreadsheet.Style gxlNLC_bo_ = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", true, "#FFFFFF", "left", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    DevExpress.Spreadsheet.Style gxlNumb_NCC_Purple = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");

                    DevExpress.Spreadsheet.Style gxlNCC_Purple = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    DevExpress.Spreadsheet.Style gxlNCC_No_left_Boarder = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#1A4777", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    DevExpress.Spreadsheet.Style gxlErrBCC_No_left_Boarder = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    DevExpress.Spreadsheet.Style gxlErrBCC_ = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    DevExpress.Spreadsheet.Style gxlErrNumb_NCC = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#fc0303", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "N");

                    DevExpress.Spreadsheet.Style gxlNCC_PRPCol_NB_ = oDevExpress_Excel_Properties.xfnCELL_STYLE(workbook, oDevExpress_Excel_Properties.sxlbodyFont, 12, "#7030A0", false, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#B8D9FF", "");

                    #endregion

                    #endregion

                    #endregion

                    int _Rowindex = 1; int _Columnindex = 1; bool _isfirst = true;
                    #region Parameters Sheet
                    DevExpress.Spreadsheet.Worksheet ParamsSheet = workbook.Worksheets[0];
                    ParamsSheet.Name = "Parameters";
                    ParamsSheet.ActiveView.ShowGridlines = false;
                    workbook.Unit = DevExpress.Office.DocumentUnit.Point;

                    ParamsSheet.Columns[1].Width = 120;
                    ParamsSheet.Columns[2].Width = 200;
                    ParamsSheet.Columns[3].Width = 50;
                    ParamsSheet.Columns[4].Width = 120;
                    ParamsSheet.Columns[5].Width = 200;
                    //ParamsSheet.Columns[6].Width = 200;
                    //ParamsSheet.Columns[7].Width = 200;
                    //ParamsSheet.Columns[8].Width = 200;
                    //ParamsSheet.Columns[9].Width = 600;

                    string strAgy = ((RepListItem)cmbHie.SelectedItem).Value.ToString(); // BaseForm.BaseAgency; //Current_Hierarchy_DB.Split('-')[0];
                    if (strAgy == "**") strAgy = "00";

                    AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);

                    string ImgName = "";
                    if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                    {
                        ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
                    }
                    else
                        ImgName = BaseForm.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

                    
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = BAgyControlDetails.AgyName;// "Community Action Resource & Development";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = xTitleCellstyle; //  oDevExpress_Excel_Properties.gxlTitle_CellStyle1; //
                    oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 5, xTitleCellstyle);
                    _Rowindex++;

                    string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;  // Application.MapPath("\\Resources\\Images\\NEOCAA_02_LOGO.png");
                                                                                              //DevExpress.Spreadsheet.Picture pic = ParamsSheet.Pictures.AddPicture(imagesPath, 700, 80, 630, 280);
                    DevExpress.Spreadsheet.SpreadsheetImageSource imgsrc = DevExpress.Spreadsheet.SpreadsheetImageSource.FromUri(imagesPath, workbook);
                    DevExpress.Spreadsheet.Picture pic = ParamsSheet.Pictures.AddPicture(imgsrc, 50, 2, 120, 70);


                    AgencyControlEntity _agycntrldets = new AgencyControlEntity();
                    _agycntrldets = BaseForm.BaseAgencyControlDetails;

                    if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                        _agycntrldets = BAgyControlDetails;
                    else
                        _agycntrldets = BaseForm.BaseAgencyControlDetails;

                    string strStreet = (_agycntrldets.Street != "" ? _agycntrldets.Street + ", " : "");
                    string strCity = (_agycntrldets.City != "" ? _agycntrldets.City + ", " : "");
                    string strState = (_agycntrldets.State != "" ? _agycntrldets.State + ", " : "");
                    string strZipcode = (_agycntrldets.Zip1 != "" ? _agycntrldets.Zip1 + "" : "");

                    string strAddress = strStreet + strCity + strState + strZipcode; //_agycntrldets.Street + ", " + _agycntrldets.City + ", " + _agycntrldets.State + ", " + _agycntrldets.Zip1;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = strAddress.Trim().TrimEnd(',');
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 5,oDevExpress_Excel_Properties.gxlEMPTC);

                    _Rowindex = 4;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = Privileges.PrivilegeName;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = reportNameStyle;
                    oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 5);

                    _Rowindex = 5;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Report Parameters";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = xTitleCellstyle2; // oDevExpress_Excel_Properties.gxlTitle_CellStyle1;
                    oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 5);
                    _Rowindex++;

                    string _HeaderText = "Service Plan";
                    if (chkbSPFielCntl.Checked)
                        _HeaderText = "Service Plan - Fields Control";
                    else if (chkPrgProcess.Checked)
                        _HeaderText = "Service Plan - Program Process";
                    
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.RepListItem)cmbHie.SelectedItem).Text.ToString().Trim();
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = xsubTitleintakeCellstyle; // oDevExpress_Excel_Properties.gxlTitle_CellStyle1;
                    oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 5);
                    _Rowindex++;


                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "";
                    oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 5);
                    _Rowindex++;



                    //** PARAMETERS LIST **//
                    string strAgency = string.Empty;
                    strAgency = ((Captain.Common.Utilities.RepListItem)cmbHie.SelectedItem).Text.ToString().Trim();
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = lblHie.Text.Trim();
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    // oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = strAgency;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;

                    

                    string strServiceType = ((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Text.ToString().Trim(); ;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = lblRnk.Text;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = strServiceType;
                     ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);
                    


                    _Rowindex++;
                    _Columnindex = 1;

                    string strDateRange = string.Empty;
                    if(cmbDteRnge.Items.Count>0)
                        strDateRange = ((Captain.Common.Utilities.ListItem)cmbDteRnge.SelectedItem).Text.ToString().Substring(0, 23);  

                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = lblDteRnge.Text;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    //_Columnindex = _Columnindex + 2;
                    _Columnindex++;


                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = strDateRange;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;


                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Date Description";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;


                    //_Columnindex = _Columnindex + 2;
                    if (cmbDteRnge.Items.Count > 0)
                        ParamsSheet.Rows[_Rowindex][_Columnindex].Value = ((ListItem)cmbDteRnge.SelectedItem).ScreenCode.ToString();
                    else
                        ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);



                    _Rowindex++;
                    _Columnindex = 1;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Include Targets";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;



                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = (chkbTargets.Checked == true ? "Yes" : "No");
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);
                    _Columnindex++;

                   // _Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;


                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Include Program Process"; 
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = (chkPrgProcess.Checked == true ? "Yes" : "No");
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);


                    _Rowindex++;
                    _Columnindex = 1;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Include Field Control Settings";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;



                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = (chkbSPFielCntl.Checked == true ? "Yes" : "No");
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;


                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = paramsCellStyle;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, paramsCellStyle);
                    _Columnindex++;

                    //_Columnindex = _Columnindex + 2;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "";
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlNLC);





                    _Rowindex = _Rowindex + 3;
                    _Columnindex = 5;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Generated Date : " + DateTime.Now.ToString("MM/dd/yyyy");
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = gxlGenerate_cr;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2);
                    
                    _Rowindex++;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Value = "Generated By : " + BaseForm.UserID;
                    ParamsSheet.Rows[_Rowindex][_Columnindex].Style = gxlGenerate_cr;
                    //oDevExpress_Excel_Properties.xlRowsMerge(ParamsSheet, _Rowindex, _Columnindex, 2);
                    #endregion


                    _Rowindex = 0; _Columnindex = 0; int _branchColRindx = 0; int _branchColColindx = 0;
                    List<RNGGAEntity> GoalEntity = new List<RNGGAEntity>();
                    List<RNGSRGAEntity> SRGoalEntity = new List<RNGSRGAEntity>();
                    string dtrCode = string.Empty, RAgy = string.Empty;
                    if (cmbDteRnge.Items.Count > 0)
                    {
                        dtrCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                        RAgy = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();

                        GoalEntity = _model.SPAdminData.Browse_RNGGA(dtrCode, RAgy, null, null, null);
                        SRGoalEntity = _model.SPAdminData.Browse_RNGSRGA(dtrCode, RAgy, null, null, null);
                    }
                    List<string> lstSheetsname = new List<string>();
                 
                    foreach (CASESP1Entity Entity in SP_Hierarchies)
                    {
                        if (dtSP_CaseSP2.Rows.Count > 0)
                        {
                            DataView dv = dtSP_CaseSP2.DefaultView;
                            dv.RowFilter = "sp2_serviceplan=" + Entity.Code.ToString(); // drSP_Services["sp0_servicecode"].ToString();
                            dv.Sort = "sp2_branch DESC";
                            DataTable CaseSP2 = dv.ToTable();

                            if (Entity.Code.ToString().Trim() != Pri_SP.Trim())
                            {
                                if (chkbSPFielCntl.Checked)
                                {
                                    string Catg = IsFieldPrint(Entity.Code.Trim());
                                    //if (Catg == "Y")
                                    //{
                                        //this.GenerateWorksheetServiceCatg_DE(workbook, Entity.Code.Trim(), oDevExpress_Excel_Properties);

                                        #region ReportName
                                        string ReportName = Entity.SP_Desc.ToString(); //drSP_Services["sp0_description"].ToString();//"Sheet1";
                                        ReportName = ReportName.Replace("/", "");

                                    ReportName = Regex.Replace(ReportName, @"[^a-zA-Z0-9\s]", "");

                                    ReportName = Entity.Code.Trim() + "_" + ReportName.Trim();


                                        k++;
                                        if (ReportName.Length >= 31)
                                            ReportName = ReportName.Substring(0, 31);
                                        if (PrvRepName == ReportName)
                                        {
                                            i++;
                                            ReportName = ReportName.Substring(0, ReportName.Length - 2) + i.ToString();
                                        }

                                        bool _isflag = lstSheetsname.Contains(ReportName);
                                        if (_isflag)
                                        {
                                            i++;
                                            ReportName = ReportName.Substring(0, ReportName.Length - 2) + i.ToString();
                                        }

                                        PrvRepName = ReportName;
                                        lstSheetsname.Add(ReportName);
                                        #endregion

                                        //DevExpress.Spreadsheet.Worksheet SheetDetails = workbook.Worksheets.Add(Entity.Code.Trim() + "_FieldControl");
                                        DevExpress.Spreadsheet.Worksheet SheetDetails = workbook.Worksheets.Add(ReportName);
                                        SheetDetails.ActiveView.TabColor = Color.SkyBlue;

                                        //Worksheet sheet = sheets.Add(SelSerPlan + "_FieldControl");
                                        //sheet.Table.DefaultRowHeight = 14.25F;

                                        try
                                        {
                                            List<CASESP1Entity> SP_Hierarchies1 = _model.SPAdminData.Browse_CASESP1(Entity.Code.Trim(), null, null, null);
                                            CAMA_Details = _model.SPAdminData.Browse_CASESP2(Entity.Code.Trim(), null, null, null);
                                            SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(Entity.Code.Trim(), null, null, null, null, null, null, null, null);
                                            PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);

                                            List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "PMTSTDFLDS");
                                            List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "FLDCNTL");
                                            List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "0", " ", "0", "          ", "hie");
                                            ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                                            _Rowindex = 0; _Columnindex = 0; _branchColRindx = 0; _branchColColindx = 0;

                                            #region Program Association
                                            string strAgyDesc = ""; string strDeptDesc = ""; string strProgDesc = ""; string strProgShortName = "";
                                            string strHIE = ""; string strHIEDesc = "";

                                            List<CASESP1Entity> sp_Hierachcyselectsp = SP_Hierarchies.FindAll(u => u.Code.Trim() == Entity.Code.Trim());
                                            if (sp_Hierachcyselectsp.Count > 0)
                                            {
                                                strHIE = sp_Hierachcyselectsp[0].Agency + "-" + sp_Hierachcyselectsp[0].Dept + "-" + sp_Hierachcyselectsp[0].Prog.Trim();

                                                if (sp_Hierachcyselectsp[0].Agency + sp_Hierachcyselectsp[0].Dept + sp_Hierachcyselectsp[0].Prog == "******")
                                                    strProgDesc = "All Hierarchies";
                                                else
                                                    strProgDesc = "Not Defined in CASEHIE";

                                                DataSet ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, sp_Hierachcyselectsp[0].Prog.Trim());
                                                if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                {
                                                    if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                    {
                                                        strHIEDesc = strProgDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                                        strProgShortName = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_SHORT_NAME"].ToString()).Trim();
                                                    }
                                                }

                                                ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, "**");
                                                if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                {
                                                    if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                        strDeptDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                                }

                                                ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, "**", "**");
                                                if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                {
                                                    if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                        strAgyDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                                }

                                                if (sp_Hierachcyselectsp[0].Dept == "**")
                                                    strDeptDesc = "All Departments";
                                                if (sp_Hierachcyselectsp[0].Prog.Trim() == "**")
                                                    strProgDesc = "All Programs";
                                            }

                                            #endregion

                                            #region Sheet Header
                                            workbook.Unit = DevExpress.Office.DocumentUnit.Point;
                                            SheetDetails.ActiveView.ShowGridlines = false;

                                            #region Column Widths
                                            SheetDetails.Columns[0].Width = 8;
                                            SheetDetails.Columns[1].Width = 110;//80
                                            SheetDetails.Columns[2].Width = 250;
                                            SheetDetails.Columns[3].Width = 70;
                                            SheetDetails.Columns[4].Width = 70;
                                            SheetDetails.Columns[5].Width = 70;
                                            int _cellIndx = 5;
                                            foreach (PMTSTDFLDSEntity PMTEntity in pmtstdfldsentity)
                                            {
                                                if (PMTEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && PMTEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && PMTEntity.PSTF_FLD_CODE != "S00022"
                                                    && PMTEntity.PSTF_FLD_CODE != "S00023" && PMTEntity.PSTF_FLD_CODE != "S00212" && PMTEntity.PSTF_FLD_CODE != "S00216" &&
                                                    PMTEntity.PSTF_FLD_CODE != "S00206" && PMTEntity.PSTF_FLD_CODE != "S00207" && PMTEntity.PSTF_FLD_CODE != "S00208" &&
                                                    PMTEntity.PSTF_FLD_CODE != "S00209" && PMTEntity.PSTF_FLD_CODE != "S00210" && PMTEntity.PSTF_FLD_CODE != "S00211")
                                                {
                                                    _cellIndx++;
                                                    SheetDetails.Columns[_cellIndx].Width = 30;
                                                }
                                            }
                                            _cellIndx = ((_cellIndx - 5) * 3) + 5;
                                            _cellIndx++;
                                            SheetDetails.Columns[_cellIndx].Width = 8;
                                            #endregion

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            _Columnindex++;


                                            _HeaderText = "Service Plan";
                                            string strProcesstext = "";

                                            if (chkbTargets.Checked || chkbSPFielCntl.Checked || chkPrgProcess.Checked)
                                            {
                                                strProcesstext = "Program";
                                                if (chkbTargets.Checked)
                                                {
                                                    strProcesstext = strProcesstext + " / " + chkbTargets.Text.Trim();
                                                }
                                                if (chkPrgProcess.Checked)
                                                {
                                                    strProcesstext = strProcesstext + " / " + chkPrgProcess.Text.Trim();
                                                }
                                                if (chkbSPFielCntl.Checked)
                                                {
                                                    strProcesstext = strProcesstext + " / " + chkbSPFielCntl.Text.Trim();
                                                }
                                            }

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = _HeaderText;// "Service Plan - Fields Control";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyleR;
                                            //oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 1); //cell.MergeAcross = 9;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strProcesstext;// "Service Plan - Fields Control";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2); //cell.MergeAcross = 9;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                                            _Columnindex = _Columnindex + (_cellIndx - 2);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                            _Columnindex++;

                                            for (int x = 0; x < (_cellIndx - 1); x++)
                                            {
                                                if (x == 0)
                                                {
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strHIE;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                                    _Columnindex++;
                                                }
                                                else if (x == 1)
                                                {
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strHIEDesc; //It is program descriprion
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                                    _Columnindex++;
                                                }
                                                else if (x == 2)
                                                {
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strProgShortName;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                                    _Columnindex++;
                                                }
                                                else
                                                {
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                                    _Columnindex++;
                                                }
                                            }


                                            SheetDetails.Rows[_Rowindex][_cellIndx].Value = "";
                                            SheetDetails.Rows[_Rowindex][_cellIndx].Style = xlFrameStyle;



                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx + 1);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                                            #region Page Parameters
                                            #region 1
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Agency :";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                            SheetDetails.Rows[_Rowindex].Height = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strAgyDesc;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2, gxlNLC_NB);

                                            _Columnindex = _Columnindex + (_cellIndx - 2);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            #endregion
                                            #region 2
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Department :";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                            SheetDetails.Rows[_Rowindex].Height = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strDeptDesc;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2, gxlNLC_NB);

                                            _Columnindex = _Columnindex + (_cellIndx - 2);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                            #endregion
                                            #region 3
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Prorgram :";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                            SheetDetails.Rows[_Rowindex].Height = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strProgDesc;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2, gxlNLC_NB);

                                            _Columnindex = _Columnindex + (_cellIndx - 2);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                            #endregion
                                            #region 6
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Funding Sources :";
                                            SheetDetails.Rows[_Rowindex].Height = 15;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                            _Columnindex++;

                                            string _strFunds = "";
                                            if (FundingList.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(Entity.Funds.ToString().Trim()))
                                                {
                                                    foreach (SPCommonEntity funditem in FundingList)
                                                    {
                                                        if (Entity.Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                                        {
                                                            _strFunds += funditem.Code.ToString() + ",";
                                                        }
                                                    }
                                                }
                                            }

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = _strFunds.TrimEnd(',');
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2, gxlNLC_NB);

                                            _Columnindex = _Columnindex + (_cellIndx - 2);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                            #endregion
                                            #region 7
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Branches :";
                                            SheetDetails.Rows[_Rowindex].Height = 15;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";// strBranchDesc.Substring(0, strBranchDesc.Length - 2); // Update the cell after below report print
                                            _branchColRindx = _Rowindex; _branchColColindx = _Columnindex;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, (_cellIndx - 2), gxlNLC_NB);

                                            _Columnindex = _Columnindex + (_cellIndx - 2);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                            #endregion
                                            #region 8
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Run Date :";
                                            SheetDetails.Rows[_Rowindex].Height = 15;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = DateTime.Now.ToString("MM/dd/yyyy");
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                            //SheetDetails.Rows[_Rowindex].AutoFitRows();
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 4, gxlNLC_NB);
                                            _Columnindex = _Columnindex + 4;
                                            foreach (PMTSTDFLDSEntity PMTEntity2 in pmtstdfldsentity)
                                            {
                                                if (PMTEntity2.PSTF_FLD_CODE.Substring(1, 1) != "A" && PMTEntity2.PSTF_FLD_CODE.Substring(1, 1) != "a" && PMTEntity2.PSTF_FLD_CODE != "S00022"
                                                    && PMTEntity2.PSTF_FLD_CODE != "S00023" && PMTEntity2.PSTF_FLD_CODE != "S00212" && PMTEntity2.PSTF_FLD_CODE != "S00216" && PMTEntity2.PSTF_FLD_CODE != "S00206"
                                                    && PMTEntity2.PSTF_FLD_CODE != "S00207" && PMTEntity2.PSTF_FLD_CODE != "S00208" && PMTEntity2.PSTF_FLD_CODE != "S00209" && PMTEntity2.PSTF_FLD_CODE != "S00210"
                                                    && PMTEntity2.PSTF_FLD_CODE != "S00211")
                                                {
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = PMTEntity2.PSTF_DESC.ToString().Trim();
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRPCol_BCHC;// oDevExpress_Excel_Properties.gxlBLHC;
                                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 3);
                                                    _Columnindex = _Columnindex + 3;
                                                }
                                            }


                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                            #endregion

                                            #endregion

                                            #region Table Data Creation
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            _Columnindex++;


                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Type";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC;// oDevExpress_Excel_Properties.gxlBLHC;
                                            _Columnindex++;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Description";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC;// oDevExpress_Excel_Properties.gxlBLHC;
                                            _Columnindex++;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Recipient";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC; // oDevExpress_Excel_Properties.gxlBLHC;
                                            _Columnindex++;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Aggregate Function";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC; // oDevExpress_Excel_Properties.gxlBLHC;


                                            if (chkbTargets.Checked)
                                            {
                                                _Columnindex++;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Target";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRCHC; // oDevExpress_Excel_Properties.gxlBLHC;
                                                _Columnindex++;
                                            }
                                            else
                                            {
                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, gxlBCHC);
                                                _Columnindex = _Columnindex + 2;
                                            }
                                            foreach (PMTSTDFLDSEntity PMTEntity3 in pmtstdfldsentity)
                                            {
                                                if (PMTEntity3.PSTF_FLD_CODE.Substring(1, 1) != "A" && PMTEntity3.PSTF_FLD_CODE.Substring(1, 1) != "a" && PMTEntity3.PSTF_FLD_CODE != "S00022"
                                                    && PMTEntity3.PSTF_FLD_CODE != "S00023" && PMTEntity3.PSTF_FLD_CODE != "S00212" && PMTEntity3.PSTF_FLD_CODE != "S00216" && PMTEntity3.PSTF_FLD_CODE != "S00206"
                                                    && PMTEntity3.PSTF_FLD_CODE != "S00207" && PMTEntity3.PSTF_FLD_CODE != "S00208" && PMTEntity3.PSTF_FLD_CODE != "S00209" && PMTEntity3.PSTF_FLD_CODE != "S00210"
                                                    && PMTEntity3.PSTF_FLD_CODE != "S00211")
                                                {

                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Enabled";
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRPCol_BCHC;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.RotationAngle = 90;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 30;
                                                    SheetDetails.Rows[_Rowindex].Height = 60;
                                                    _Columnindex++;

                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Required";
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRPCol_BCHC;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.RotationAngle = 90;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 30;
                                                    SheetDetails.Rows[_Rowindex].Height = 60;
                                                    _Columnindex++;

                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Disabled";
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRPCol_BCHC;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.RotationAngle = 90;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 30;
                                                    SheetDetails.Rows[_Rowindex].Height = 60;
                                                    _Columnindex++;
                                                }
                                            }

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                            #endregion

                                            #region Data to print
                                            Branch = null; Priv_Branch = null; First = true; string strBranchDesc = ""; bool CAMS = true; Branch = null;
                                            foreach (CASESP1Entity spEntity in SP_Hierarchies1)
                                            {
                                                _Rowindex++;
                                                _Columnindex = 0;
                                                foreach (CASESP2Entity CAEntity in CAMA_Details)
                                                {
                                                    if (CAEntity.Type1 == "CA")
                                                    {
                                                        SP_Plan_desc = Entity.Code.ToString();
                                                        Branch = CAEntity.Branch.ToString().Trim();
                                                        if (Branch != Priv_Branch)
                                                        {
                                                            if (Branch != "P")
                                                                First = false;
                                                            string Service_desc = Entity.BpDesc.ToString(); //drSP_Services["sp0_pbranch_desc"].ToString();
                                                            if (!First)
                                                            {
                                                                if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                                                                    Service_desc = Entity.B1Desc;
                                                                else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                                                                    Service_desc = Entity.B2Desc;
                                                                else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                                                                    Service_desc = Entity.B3Desc;
                                                                else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                                                                    Service_desc = Entity.B4Desc.ToString();
                                                                else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                                                                    Service_desc = Entity.B5Desc.ToString();
                                                                else if (Branch.Trim() == "9")
                                                                    Service_desc = "Additional Branch";

                                                            }
                                                            CAMS = true;

                                                            string _strType = "";
                                                            string _type = "";

                                                            if (SPNum)
                                                            {
                                                                _strType = "Service :";
                                                                _type = Entity.SP_Desc.Trim();
                                                            }
                                                            if (!CAMS)
                                                                _strType = "";
                                                            else
                                                            {
                                                                _strType = "Branch :";
                                                                _type = Service_desc.Trim();
                                                            }

                                                            strBranchDesc += _type + ", ";

                                                            //_Rowindex++; _Columnindex = 0;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                            _Columnindex++;

                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = _strType;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_bo_;//gxlNLC_bo;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                            _Columnindex++;

                                                            strBranchDesc += _type + ", ";
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = _type;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_bo_;//gxlNLC_bo;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Top;
                                                            _Columnindex++;

                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC;
                                                            _Columnindex++;

                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC;

                                                            if (chkbTargets.Checked)
                                                            {
                                                                _Columnindex++;
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNCC;//gxlNumb_NRC;
                                                                _Columnindex++;
                                                            }
                                                            else
                                                            {
                                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, gxlNCC/*gxlNLC*/);
                                                                _Columnindex = _Columnindex + 2;
                                                            }

                                                            foreach (PMTSTDFLDSEntity PMTEntity3 in pmtstdfldsentity)
                                                            {
                                                                if (PMTEntity3.PSTF_FLD_CODE.Substring(1, 1) != "A" && PMTEntity3.PSTF_FLD_CODE.Substring(1, 1) != "a" && PMTEntity3.PSTF_FLD_CODE != "S00022"
                                                                    && PMTEntity3.PSTF_FLD_CODE != "S00023" && PMTEntity3.PSTF_FLD_CODE != "S00212" && PMTEntity3.PSTF_FLD_CODE != "S00216" && PMTEntity3.PSTF_FLD_CODE != "S00206"
                                                                    && PMTEntity3.PSTF_FLD_CODE != "S00207" && PMTEntity3.PSTF_FLD_CODE != "S00208" && PMTEntity3.PSTF_FLD_CODE != "S00209" && PMTEntity3.PSTF_FLD_CODE != "S00210"
                                                                    && PMTEntity3.PSTF_FLD_CODE != "S00211")
                                                                {

                                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNCC;//gxlNLC;
                                                                    _Columnindex++;

                                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNCC;//gxlNLC;
                                                                    _Columnindex++;

                                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNCC;//gxlNLC;
                                                                    _Columnindex++;
                                                                }
                                                            }

                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                                            Priv_Branch = Branch;
                                                            First = false;
                                                            SPNum = false;
                                                        }

                                                        string CAMSType = "Outcome";
                                                        CAMSDesc = "";
                                                        string ServiceCount = string.Empty;
                                                        bool Active = true;
                                                        if (CAEntity.Type1 == "CA")
                                                        {
                                                            CAMSType = "Service";
                                                            List<CAMASTEntity> _lstDesc = CAList.Where(x => x.Code.ToString().Trim() == CAEntity.CamCd.ToString().Trim()).ToList();// dr["sp2_cams_code"]
                                                            if (_lstDesc.Count > 0)
                                                            {
                                                                CAMSDesc = _lstDesc[0].Desc.ToString().Trim();
                                                                if (_lstDesc[0].Active == "False")
                                                                    Active = false;
                                                            }

                                                            if (chkbTargets.Checked)
                                                            {
                                                                RNGSRGAEntity GAEntity = SRGoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == CAEntity.CamCd.ToString().Trim()); // dr["sp2_cams_code"]
                                                                if (GAEntity != null)
                                                                    ServiceCount = GAEntity.Budget.ToString();
                                                            }
                                                        }


                                                        _Rowindex++;
                                                        _Columnindex = 0;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                        _Columnindex++;

                                                        bool _isActive = true;
                                                        if (!Active || CAEntity.SP2_CAMS_Active.ToString().Trim() == "I")
                                                            _isActive = false;


                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAMSType;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_No_left_Boarder : gxlErrBCC_No_left_Boarder);
                                                        _Columnindex++;


                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAMSDesc;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNLC : gxlErrBLC);
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Top;
                                                        _Columnindex++;

                                                        string strReciepent = "";

                                                        if (AgyOBF_List.Count > 0)
                                                        {
                                                            // AgyOBF_List = AgyOBF_List.OrderBy(u => Convert.ToInt32(u.Code.Trim())).ToList();
                                                            List<CommonEntity> AgyOBF_List1 = AgyOBF_List.Where(x => x.Code == CAEntity.SP2_OBF.ToString().Trim()).ToList(); // dr["SP2_OBF"]
                                                            if (AgyOBF_List1.Count > 0)
                                                                strReciepent = AgyOBF_List1[0].Desc.ToString();
                                                        }

                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = strReciepent;// dr["SP2_OBF"].ToString().Trim(); //Recipent
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);
                                                        _Columnindex++;

                                                        string strCounty = "";
                                                        if (AgyCOUNTY_List.Count > 0)
                                                        {
                                                            List<CommonEntity> AgyCOUNTY_List1 = AgyCOUNTY_List.Where(x => x.Code == CAEntity.SP2_COUNT_TYPE.ToString().Trim()).ToList(); //dr["SP2_COUNT_TYPE"]
                                                            if (AgyCOUNTY_List1.Count > 0)
                                                                strCounty = AgyCOUNTY_List1[0].Desc.ToString();
                                                        }
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = strCounty;// dr["SP2_COUNT_TYPE"].ToString().Trim();
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);


                                                        if (chkbTargets.Checked)
                                                        {
                                                            _Columnindex++;
                                                            if (ServiceCount != "")
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = Convert.ToInt32(ServiceCount);
                                                            //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNumb_NRC : gxlErrNumb_NRC);
                                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNumb_NCC_Purple : gxlErrNumb_NCC);
                                                            _Columnindex++;
                                                        }
                                                        else
                                                        {
                                                            //oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, (_isActive == true ? gxlNLC : gxlErrBLC));
                                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, (_isActive == true ? gxlNCC_Purple : gxlErrBCC_));
                                                            _Columnindex = _Columnindex + 2;
                                                        }

                                                        propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, SP_Hierarchies1[0].Code, SP_Hierarchies1[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                                                        propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                                                        if (propPMTFLDCNTLHEntity.Count == 0)
                                                        {
                                                            propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "0", " ", "0", "          ", "hie");
                                                            propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                                                        }

                                                        foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                                                        {
                                                            string isEnabled = string.Empty;
                                                            string isRequired = string.Empty;

                                                            if (propPMTFLDCNTLHEntity.Count > 0)
                                                            {
                                                                if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                                                    isEnabled = "Y";
                                                                if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                                                    isRequired = "Y";
                                                            }

                                                            if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                                            {
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = isEnabled;
                                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_PRPCol_NB : gxlErrBCC);
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_PRPCol_NB_ : gxlErrBCC_);
                                                                _Columnindex++;

                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = isRequired;
                                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_PRPCol_NB : gxlErrBCC);
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_PRPCol_NB_ : gxlErrBCC_);
                                                                _Columnindex++;

                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_PRPCol_NB : gxlErrBCC);
                                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_PRPCol_NB_ : gxlErrBCC_);
                                                                _Columnindex++;
                                                            }
                                                        }


                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                    }
                                                }
                                            }

                                            SheetDetails.Rows[_branchColRindx][_branchColColindx].Value = strBranchDesc.Substring(0, strBranchDesc.Length - 2);


                                            // If program Process is checked
                                            /*****************************************************************************************************************************************/

                                            if (chkPrgProcess.Checked)
                                            {
                                                _Rowindex++; _Columnindex = 0;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Program Process : ";
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 1, gxlPrgNotes);
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                _Columnindex = _Columnindex + (_cellIndx - 1);

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                                // GET CASENOTES of SERVICE PLAN PROCESS
                                                /*****************************************************************************/
                                                string _strPrgPRocessNotes = " - ";
                                                string ScreenCode = "ADMN0030"; string _strSPCode = "000000".Substring(0, (6 - Entity.Code.ToString().Length)) + Entity.Code.ToString();
                                                string RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                string KeyField = BaseForm.BaseAgency + "_" + RNGCode + "_" + _strSPCode;
                                                List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode, KeyField);
                                                if (caseNotesEntity.Count > 0)
                                                {
                                                    _strPrgPRocessNotes = caseNotesEntity[0].Data.ToString();
                                                }

                                                /*****************************************************************************/

                                                _Rowindex++;
                                                _Columnindex = 0;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = _strPrgPRocessNotes;// "It has been said that astronomy is a humbling and character-building experience. There is perhaps no better demonstration of the folly of human conceits than this distant image of our tiny world. To me, it underscores our responsibility to deal more kindly with one another, and to preserve and cherish the pale blue dot, the only home we've ever known.\r\n";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                if (_strPrgPRocessNotes != " - ")
                                                    SheetDetails.Rows[_Rowindex].Height = 120;

                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2, gxlPrgNotes);
                                                //oDevExpress_Excel_Properties.AutoFitRows(SheetDetails, _Rowindex);
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                                                _Columnindex = _Columnindex + (_cellIndx - 2);

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                                /*** COMMUNICATION HISTORY OF SELECTED SP ***/
                                                DataTable dtHist = TmsApcn.GETPROCHIST(ScreenCode, KeyField, "");
                                                if (dtHist.Rows.Count > 0)
                                                {

                                                    int _cnt = 0;
                                                    foreach (DataRow _dr in dtHist.Rows)
                                                    {
                                                        //dgvProcHist.Rows.Add(_dr["PRH_DATE"].ToString(), _dr["PRH_USER_ID"].ToString(), _dr["PRH_short_Notes"].ToString());
                                                        //if (_dr["PRH_SEQ"].ToString() == "1")
                                                        //{
                                                        //    lblHeadmsg.Text = "Process Started by " + _dr["PRH_USER_ID"].ToString() + " on " + Convert.ToDateTime(_dr["PRH_DATE"].ToString()).ToString("MM/dd/yyyy");
                                                        //}
                                                        string _commHist = "";
                                                        if (_cnt == 0)
                                                        {
                                                            _commHist = "Communications History : ";
                                                            _cnt = 1;
                                                        }
                                                        _Rowindex++; _Columnindex = 0;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                        _Columnindex++;

                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = _commHist;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, (_cellIndx - 1), gxlPrgNotes);
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                        _Columnindex = _Columnindex + (_cellIndx - 1);

                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                                        _Rowindex++;
                                                        _Columnindex = 0;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                        _Columnindex++;

                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                        _Columnindex++;


                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = _dr["PRH_DATE"].ToString() + "    " + _dr["PRH_USER_ID"].ToString();
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                                        _Columnindex++;

                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Value =;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                        //_Columnindex++;

                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = _dr["PRH_short_Notes"].ToString().Trim();
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 70;

                                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, (_cellIndx - 3), gxlPrgNotes);
                                                        //oDevExpress_Excel_Properties.AutoFitRows(SheetDetails, _Rowindex);
                                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                                        _Columnindex = _Columnindex + (_cellIndx - 3);

                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                                    }
                                                }
                                            }

                                            /******************************************************************************************************************************************/





                                            /******************************************************************************************************************************************/
                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "©2024 CAPSystems Inc";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameFooterStyle;
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 1); //cell.MergeAcross = 9;

                                            _Columnindex = _Columnindex + (_cellIndx - 1);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            /******************************************************************************************************************************************/

                                            #endregion

                                            #endregion
                                        }
                                        catch (Exception ex) { }

                                    //}
                                }
                                else
                                {
                                    _branchColRindx = 0; _branchColColindx = 0;
                                    #region ReportName
                                    string ReportName = Entity.SP_Desc.ToString(); //drSP_Services["sp0_description"].ToString();//"Sheet1";
                                    ReportName = ReportName.Replace("/", "");

                                    ReportName = Regex.Replace(ReportName, @"[^a-zA-Z0-9\s]", "");

                                    ReportName = Entity.Code.Trim() + "_" + ReportName.Trim();

                                    
                                    k++;
                                    if (ReportName.Length >= 31)
                                        ReportName = ReportName.Substring(0, 31);
                                    if (PrvRepName == ReportName)
                                    {
                                        i++;
                                        ReportName = ReportName.Substring(0, ReportName.Length - 2) + i.ToString();
                                    }

                                    bool _isflag = lstSheetsname.Contains(ReportName);
                                    if (_isflag)
                                    {
                                        i++;
                                        ReportName = ReportName.Substring(0, ReportName.Length - 2) + i.ToString();
                                    }

                                    PrvRepName = ReportName;
                                    #endregion

                                    lstSheetsname.Add(ReportName);
                                    DevExpress.Spreadsheet.Worksheet SheetDetails = null;
                                    //if (_isfirst)
                                    //{
                                    //    SheetDetails = workbook.Worksheets[0];
                                    //    SheetDetails.Name = ReportName;
                                    //    _isfirst = false;
                                    //}
                                    //else
                                        SheetDetails = workbook.Worksheets.Add(ReportName);
                                    SheetDetails.ActiveView.TabColor = Color.SkyBlue;
                                    SheetDetails.ActiveView.ShowGridlines = false;
                                    workbook.Options.Cells.AutoFitMergedCellRowHeight = true;
                                    _Rowindex = 0; _Columnindex = 0;

                                    SheetDetails.Columns[0].Width = 8;
                                    SheetDetails.Columns[1].Width = 110;
                                    SheetDetails.Columns[2].Width = 200;
                                    SheetDetails.Columns[3].Width = 70;
                                    SheetDetails.Columns[4].Width = 80;
                                    SheetDetails.Columns[5].Width = 70;
                                    SheetDetails.Columns[6].Width = 70;
                                    SheetDetails.Columns[7].Width = 70;
                                    SheetDetails.Columns[8].Width = 8;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    _Columnindex++;


                                    _HeaderText = "Service Plan";
                                    string strProcesstext = "";

                                    if (chkbTargets.Checked || chkbSPFielCntl.Checked || chkPrgProcess.Checked)
                                    {
                                        strProcesstext = "Program";
                                        if (chkbTargets.Checked)
                                        {
                                            strProcesstext = strProcesstext + " / " + chkbTargets.Text.Trim();
                                        }
                                        if (chkPrgProcess.Checked)
                                        {
                                            strProcesstext = strProcesstext + " / " + chkPrgProcess.Text.Trim();
                                        }
                                        if (chkbSPFielCntl.Checked)
                                        {
                                            strProcesstext = strProcesstext + " / " + chkbSPFielCntl.Text.Trim();
                                        }
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = _HeaderText;// "Service Plan - Program Process";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyleR;
                                    //oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 7); //cell.MergeAcross = 9;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strProcesstext;// "Service Plan - Program Process";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6); //cell.MergeAcross = 9;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                   


                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                    _Columnindex++;

                                    #region Program Association
                                    string strAgyDesc = ""; string strDeptDesc = ""; string strProgDesc = ""; string strProgShortName = "";
                                    string strHIE = ""; string strHIEDesc = "";

                                    List<CASESP1Entity> sp_Hierachcyselectsp = SP_Hierarchies.FindAll(u => u.Code.Trim() == Entity.Code.Trim());
                                    if (sp_Hierachcyselectsp.Count > 0)
                                    {
                                        strHIE = sp_Hierachcyselectsp[0].Agency + "-" + sp_Hierachcyselectsp[0].Dept + "-" + sp_Hierachcyselectsp[0].Prog.Trim();

                                        if (sp_Hierachcyselectsp[0].Agency + sp_Hierachcyselectsp[0].Dept + sp_Hierachcyselectsp[0].Prog == "******")
                                            strProgDesc = "All Hierarchies";
                                        else
                                            strProgDesc = "Not Defined in CASEHIE";

                                        DataSet ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, sp_Hierachcyselectsp[0].Prog.Trim());
                                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                        {
                                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                            {
                                                strHIEDesc = strProgDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                                strProgShortName = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_SHORT_NAME"].ToString()).Trim();
                                            }
                                        }

                                        ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, sp_Hierachcyselectsp[0].Dept, "**");
                                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                        {
                                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                strDeptDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                        }

                                        ds_Hie_Name1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(sp_Hierachcyselectsp[0].Agency, "**", "**");
                                        if (ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                        {
                                            if (ds_Hie_Name1.Tables.Count > 0 && ds_Hie_Name1.Tables[0].Rows.Count > 0)
                                                strAgyDesc = (ds_Hie_Name1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                                        }

                                        if (sp_Hierachcyselectsp[0].Dept == "**")
                                            strDeptDesc = "All Departments";
                                        if (sp_Hierachcyselectsp[0].Prog.Trim() == "**")
                                            strProgDesc = "All Programs";
                                    }

                                    #endregion
                                    for (int x = 0; x < 7; x++)
                                    {
                                        if (x == 0)
                                        {
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strHIE;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                            _Columnindex++;
                                        }
                                        else if (x == 1)
                                        {
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strHIEDesc; //It is program descriprion
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                            _Columnindex++;
                                        }
                                        else if (x == 2)
                                        {
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strProgShortName;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                            _Columnindex++;
                                        }
                                        else
                                        {
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameBlockStyle;
                                            _Columnindex++;
                                        }
                                    }


                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    _Columnindex++;

                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 9);
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                                    #region Page Parameters

                                    #region 1
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Agency :";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strAgyDesc;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    #endregion
                                    #region 2
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Department :";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strDeptDesc;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 3
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Prorgram :";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = strProgDesc;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 4
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Date Description :";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_PRPCol_NB_bold;
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    _Columnindex++;

                                    string DtDesc = string.Empty;
                                    if (cmbDteRnge.Items.Count > 0)
                                    {
                                        DtDesc = ((ListItem)cmbDteRnge.SelectedItem).ScreenCode.ToString();
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = DtDesc;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_PRPCol_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_PRPCol_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 5
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Date Range :";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_PRPCol_NB_bold;
                                    _Columnindex++;

                                    string DtRange = string.Empty;
                                    if (cmbDteRnge.Items.Count > 0)
                                    {
                                        DtRange = ((ListItem)cmbDteRnge.SelectedItem).Text.ToString().Substring(0, 23);
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = DtRange;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_PRPCol_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_PRPCol_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 6
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Funding Sources :";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                    _Columnindex++;

                                    string _strFunds = "";
                                    if (FundingList.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(Entity.Funds.ToString().Trim()))
                                        {
                                            foreach (SPCommonEntity funditem in FundingList)
                                            {
                                                if (Entity.Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                                {
                                                    _strFunds += funditem.Code.ToString() + ",";
                                                }
                                            }
                                        }
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = _strFunds.TrimEnd(',');
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 7
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Branches :";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = " "; // Update the cell after below report print
                                    _branchColRindx = _Rowindex; _branchColColindx = _Columnindex;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 8
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Run Date :";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_bold_NB;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = DateTime.Now.ToString("MM/dd/yyyy");
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 9
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Current Process :";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_PRPCol_NB_bold;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = " ";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 10
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Created :";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_PRPCol_NB_bold;
                                    _Columnindex++;

                                    if (sp_Hierachcyselectsp.Count > 0)
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Convert.ToDateTime(sp_Hierachcyselectsp[0].Dateadd).ToString("MM/dd/yyyy") + " by " + sp_Hierachcyselectsp[0].addoperator;
                                    else
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_PRPCol_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_PRPCol_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion
                                    #region 11
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 15;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Modified ";
                                    SheetDetails.Rows[_Rowindex].Height = 15;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNRC_PRPCol_NB_bold;
                                    _Columnindex++;

                                    if (sp_Hierachcyselectsp.Count > 0)
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Convert.ToDateTime(sp_Hierachcyselectsp[0].DateLstc).ToString("MM/dd/yyyy") + " by " + sp_Hierachcyselectsp[0].lstcOperator;
                                    else
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_PRPCol_NB;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlNLC_PRPCol_NB);

                                    _Columnindex = _Columnindex + 6;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                    #endregion

                                    #endregion

                                    #region Table Data Creation

                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Type";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC; // oDevExpress_Excel_Properties.gxlBLHC;
                                    _Columnindex++;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Description";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC; // oDevExpress_Excel_Properties.gxlBLHC;
                                    _Columnindex++;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Recipient";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC; // oDevExpress_Excel_Properties.gxlBLHC;
                                    _Columnindex++;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Aggregate Function";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlBCHC; // oDevExpress_Excel_Properties.gxlBLHC;


                                    if (chkbTargets.Checked)
                                    {
                                        _Columnindex++;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Target";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRCHC; // oDevExpress_Excel_Properties.gxlBRHC;
                                        _Columnindex++;
                                    }
                                    else
                                    {
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, gxlBCHC);
                                        _Columnindex = _Columnindex + 2;
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Add Date";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRCHC;//gxlBCHC;// oDevExpress_Excel_Properties.gxlBCHC;
                                    _Columnindex++;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Change Date";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPRCHC;//gxlBCHC;// oDevExpress_Excel_Properties.gxlBCHC;
                                    _Columnindex++;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    #endregion

                                    bool CAMS = true; Branch = null; Priv_Branch = null; First = true; string strBranchDesc = "";
                                    if (CaseSP2.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in CaseSP2.Rows)
                                        {


                                            SP_Plan_desc = Entity.Code.ToString();
                                            Branch = dr["sp2_branch"].ToString().Trim();

                                            if (Branch != Priv_Branch)
                                            {
                                                if (Branch != "P")
                                                    First = false;
                                                string Service_desc = Entity.BpDesc.ToString(); //drSP_Services["sp0_pbranch_desc"].ToString();
                                                if (!First)
                                                {
                                                    if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                                                        Service_desc = Entity.B1Desc;
                                                    else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                                                        Service_desc = Entity.B2Desc;
                                                    else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                                                        Service_desc = Entity.B3Desc;
                                                    else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                                                        Service_desc = Entity.B4Desc.ToString();
                                                    else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                                                        Service_desc = Entity.B5Desc.ToString();
                                                    else if (Branch.Trim() == "9")
                                                        Service_desc = "Additional Branch";

                                                }
                                                CAMS = true;

                                                string _strType = ""; string _type = "";

                                                if (SPNum)
                                                {
                                                    _strType = "Service :";
                                                    _type = Entity.SP_Desc.Trim();
                                                }
                                                if (!CAMS)
                                                    _strType = "";
                                                else
                                                {
                                                    _strType = "Branch :";
                                                    _type = Service_desc.Trim();
                                                }


                                                _Rowindex++; _Columnindex = 0;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = _strType;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_bo_;//gxlNLC_bo;// oDevExpress_Excel_Properties.gxlNLC_bo;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                _Columnindex++;

                                                strBranchDesc += _type + ", ";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = _type;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC_bo_;//gxlNLC_bo;// oDevExpress_Excel_Properties.gxlNLC_bo;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC; // oDevExpress_Excel_Properties.gxlNLC;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC;// oDevExpress_Excel_Properties.gxlNLC;


                                                if (chkbTargets.Checked)
                                                {
                                                    _Columnindex++;
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNCC;//oDevExpress_Excel_Properties.gxlNRC;
                                                    _Columnindex++;
                                                }
                                                else
                                                {
                                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, gxlNCC /*gxlNLC*/);
                                                    _Columnindex = _Columnindex + 2;
                                                }

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC; // oDevExpress_Excel_Properties.gxlNLC;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlNLC;// oDevExpress_Excel_Properties.gxlNLC;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                                Priv_Branch = Branch;
                                                First = false; SPNum = false;
                                            }

                                            string CAMSType = "Service"; CAMSDesc = "";
                                            if (dr["sp2_type"].ToString() != "CA")
                                                CAMSType = "Outcome";
                                            string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty;
                                            string ServiceCount = string.Empty;



                                            _Rowindex++; _Columnindex = 0;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                            _Columnindex++;



                                            bool Active = true;
                                            if (dr["sp2_type"].ToString() == "CA")
                                            {
                                                List<CAMASTEntity> _lstDesc = CAList.Where(x => x.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim()).ToList();
                                                if (_lstDesc.Count > 0)
                                                {
                                                    CAMSDesc = _lstDesc[0].Desc.ToString().Trim();
                                                    if (_lstDesc[0].Active == "False")
                                                        Active = false;
                                                }

                                                if (chkbTargets.Checked)
                                                {
                                                    RNGSRGAEntity GAEntity = SRGoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                                    if (GAEntity != null)
                                                        ServiceCount = GAEntity.Budget.ToString();

                                                    //if (cmbDteRnge.Items.Count > 0)
                                                    //{
                                                    //    RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                    //    RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                                    //}

                                                    //GoalSRHieEntity = _model.SPAdminData.Browse_RNGSRGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());
                                                    //if (GoalSRHieEntity.Count > 0)
                                                    //{
                                                    //    GoalSRHieEntity = GoalSRHieEntity.FindAll(u => u.RNGSRGAH_TARGET != "");
                                                    //    if (GoalSRHieEntity.Count > 0)
                                                    //        ServiceCount = GoalSRHieEntity.Sum(u => Convert.ToInt32(u.RNGSRGAH_TARGET.Trim())).ToString();
                                                    //}
                                                }
                                            }
                                            else
                                            {
                                                List<MSMASTEntity> _lstMSDesc = MSList.Where(x => x.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim()).ToList();
                                                if (_lstMSDesc.Count > 0)
                                                {
                                                    CAMSDesc = _lstMSDesc[0].Desc.ToString().Trim();
                                                    if (_lstMSDesc[0].Active == "False")
                                                        Active = false;
                                                }

                                                if (chkbTargets.Checked)
                                                {
                                                    RNGGAEntity GAEntity = GoalEntity.Find(u => u.SerSP == Entity.Code && u.GoalCode.Trim() == dr["sp2_cams_code"].ToString().Trim());
                                                    if (GAEntity != null)
                                                        ServiceCount = GAEntity.Budget.ToString();

                                                    //if (cmbDteRnge.Items.Count > 0)
                                                    //{
                                                    //    RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                    //    RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                                    //}

                                                    //GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());
                                                    //if (GoalHieEntity.Count > 0)
                                                    //{
                                                    //    GoalHieEntity = GoalHieEntity.FindAll(u => u.RNGGAH_TARGET != "");
                                                    //    if (GoalHieEntity.Count > 0)
                                                    //        ServiceCount = GoalHieEntity.Sum(u => Convert.ToInt32(u.RNGGAH_TARGET.Trim())).ToString();
                                                    //}
                                                }
                                            }

                                            bool _isActive = true;
                                            if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                                _isActive = false;


                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAMSType;
                                            //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_No_left_Boarder : gxlErrBCC_No_left_Boarder);
                                            _Columnindex++;


                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAMSDesc;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNLC : gxlErrBLC);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Top;
                                            _Columnindex++;


                                            string strReciepent = "";

                                            if (AgyOBF_List.Count > 0)
                                            {
                                                // AgyOBF_List = AgyOBF_List.OrderBy(u => Convert.ToInt32(u.Code.Trim())).ToList();
                                                List<CommonEntity> AgyOBF_List1 = AgyOBF_List.Where(x => x.Code == dr["SP2_OBF"].ToString().Trim()).ToList();
                                                if (AgyOBF_List1.Count > 0)
                                                    strReciepent = AgyOBF_List1[0].Desc.ToString();
                                            }

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strReciepent;// dr["SP2_OBF"].ToString().Trim(); //Recipent
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);
                                            _Columnindex++;

                                            string strCounty = "";
                                            if (AgyCOUNTY_List.Count > 0)
                                            {
                                                List<CommonEntity> AgyCOUNTY_List1 = AgyCOUNTY_List.Where(x => x.Code == dr["SP2_COUNT_TYPE"].ToString().Trim()).ToList();
                                                if (AgyCOUNTY_List1.Count > 0)
                                                    strCounty = AgyCOUNTY_List1[0].Desc.ToString();
                                            }
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = strCounty;// dr["SP2_COUNT_TYPE"].ToString().Trim();
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);


                                            if (chkbTargets.Checked)
                                            {
                                                _Columnindex++;
                                                if (ServiceCount != "")
                                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = Convert.ToInt32(ServiceCount);
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNumb_NRC : gxlErrNumb_NRC);
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNumb_NCC_Purple : gxlErrNumb_NCC);
                                                _Columnindex++;
                                            }
                                            else
                                            {
                                               // oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, (_isActive == true ? gxlNLC : gxlErrBLC));
                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, (_isActive == true ? gxlNCC_Purple : gxlErrBCC_));
                                                _Columnindex = _Columnindex + 2;
                                            }

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim());
                                            //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_Purple : gxlErrBCC_);
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim());
                                            //SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC : gxlErrBCC);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = (_isActive == true ? gxlNCC_Purple : gxlErrBCC_);
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                        }

                                        //Branch Header cell update
                                        SheetDetails.Rows[_branchColRindx][_branchColColindx].Value = strBranchDesc.Substring(0, strBranchDesc.Length - 2);
                                    }


                                    // If program Process is checked
                                    /*****************************************************************************************************************************************/

                                    if (chkPrgProcess.Checked)
                                    {
                                        _Rowindex++; _Columnindex = 0;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Program Process : ";
                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 7, gxlPrgNotes);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                        _Columnindex = _Columnindex + 7;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;

                                        // GET CASENOTES of SERVICE PLAN PROCESS
                                        /*****************************************************************************/
                                        string _strPrgPRocessNotes = " - ";
                                        string ScreenCode = "ADMN0030"; string _strSPCode = "000000".Substring(0, (6 - Entity.Code.ToString().Length)) + Entity.Code.ToString();
                                        string RNGCode = string.Empty;
                                        if(cmbDteRnge.Items.Count>0)
                                            RNGCode=((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                        string KeyField = BaseForm.BaseAgency + "_" + RNGCode + "_" + _strSPCode;
                                        List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(ScreenCode, KeyField);
                                        if (caseNotesEntity.Count > 0)
                                        {
                                            _strPrgPRocessNotes = caseNotesEntity[0].Data.ToString();
                                        }

                                        /*****************************************************************************/

                                        _Rowindex++;
                                        _Columnindex = 0;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = _strPrgPRocessNotes;// "It has been said that astronomy is a humbling and character-building experience. There is perhaps no better demonstration of the folly of human conceits than this distant image of our tiny world. To me, it underscores our responsibility to deal more kindly with one another, and to preserve and cherish the pale blue dot, the only home we've ever known.\r\n";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                        if (_strPrgPRocessNotes != " - ")
                                            SheetDetails.Rows[_Rowindex].Height = 120;

                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 6, gxlPrgNotes);
                                        //oDevExpress_Excel_Properties.AutoFitRows(SheetDetails, _Rowindex);
                                        //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                                        _Columnindex = _Columnindex + 6;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                        /*** COMMUNICATION HISTORY OF SELECTED SP ***/
                                        DataTable dtHist = TmsApcn.GETPROCHIST(ScreenCode, KeyField, "");
                                        if (dtHist.Rows.Count > 0)
                                        {

                                            int _cnt = 0;
                                            foreach (DataRow _dr in dtHist.Rows)
                                            {
                                                //dgvProcHist.Rows.Add(_dr["PRH_DATE"].ToString(), _dr["PRH_USER_ID"].ToString(), _dr["PRH_short_Notes"].ToString());
                                                //if (_dr["PRH_SEQ"].ToString() == "1")
                                                //{
                                                //    lblHeadmsg.Text = "Process Started by " + _dr["PRH_USER_ID"].ToString() + " on " + Convert.ToDateTime(_dr["PRH_DATE"].ToString()).ToString("MM/dd/yyyy");
                                                //}
                                                string _commHist = "";
                                                if (_cnt == 0)
                                                {
                                                    _commHist = "Communications History : ";
                                                    _cnt = 1;
                                                }
                                                _Rowindex++; _Columnindex = 0;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = _commHist;
                                                //**SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 7, gxlPrgNotes);
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                _Columnindex = _Columnindex + 7;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                                _Rowindex++;
                                                _Columnindex = 0;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = _dr["PRH_DATE"].ToString() + "    " + _dr["PRH_USER_ID"].ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                                _Columnindex++;

                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Value =;
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Font.UnderlineType = DevExpress.Spreadsheet.UnderlineType.Single;
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                //_Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = _dr["PRH_short_Notes"].ToString().Trim();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = gxlPrgNotes;
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 70;

                                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, gxlPrgNotes);
                                                //oDevExpress_Excel_Properties.AutoFitRows(SheetDetails, _Rowindex);
                                                //SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                                _Columnindex = _Columnindex + 5;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                                            }
                                        }
                                    }

                                    /******************************************************************************************************************************************/
                                    _Rowindex++; _Columnindex = 0;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "©2024 CAPSystems Inc";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameFooterStyle;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 7); //cell.MergeAcross = 9;

                                    _Columnindex = _Columnindex + 7;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                                }

                                Pri_SP = Entity.Code.Trim();



                              
                            }
                        }
                    }

                    //ParamsSheet.ActiveView.IsSelected = true;
                    workbook.Sheets.ActiveSheet = workbook.Sheets[0];
                    // Save the document file under the specified name.
                    workbook.SaveDocument(_excelPath, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = _excelPath;
                        /// Need to check for null value of localFilePath, etc...
                        FileInfo fiDownload = new FileInfo(localFilePath);
                        /// Need to check for file exists, is local file, is allow to read, etc...
                        string name = fiDownload.Name;
                        using (FileStream fileStream = fiDownload.OpenRead())
                        {
                            Application.Download(fileStream, name);
                            // System.Diagnostics.Process.Start(_excelPath);
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                    //AlertBox.Show("Audit Report Generated Successfully");
                }
                #endregion
            }
        }

        private void On_SaveAllExcelForm_Closed(object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;
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
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }

                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                CarlosAg.ExcelXmlWriter.Workbook book = new CarlosAg.ExcelXmlWriter.Workbook();

                this.GenerateStyles(book.Styles);



                //string SPCode = string.Empty;
                //SPCode = SPGrid.CurrentRow.Cells["Code"].Value.ToString().Trim();

                // DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(null, null, null, null, null, null, null, null, null);
                //DataRow drSP_Services = dsSP_Services.Tables[0].Rows[0];



                DataSet dsSP_CaseSP2 = DatabaseLayer.SPAdminDB.Browse_CASESP2(null, null, null, null);
                DataTable dtSP_CaseSP2 = dsSP_CaseSP2.Tables[0];

                List<CAMASTEntity> CAList;
                CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
                List<MSMASTEntity> MSList;
                MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

                List<RNGGoalHEntity> GoalHieEntity = new List<RNGGoalHEntity>();
                List<RNGSRGoalHEntity> GoalSRHieEntity = new List<RNGSRGoalHEntity>();

                List<SPCommonEntity> FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");

                //if (rbAllSP.Checked)
                //{
                //    SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
                //}
                if (((Captain.Common.Utilities.ListItem)cmbSPOptions.SelectedItem).Value.ToString().Trim() == "ASP")
                {
                    SP_Hierarchies = SP_Hierarchies.FindAll(u => u.Sp0_Active == "Y");
                }

                bool First = true; string MS_Type = string.Empty;
                string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null;
                bool SPNum = true;
                CarlosAg.ExcelXmlWriter.Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;
                string PrvRepName = ""; int i = 0; int k = 1; string Pri_SP = string.Empty;
                foreach (CASESP1Entity Entity in SP_Hierarchies)
                {
                    if (dtSP_CaseSP2.Rows.Count > 0)
                    {
                        DataView dv = dtSP_CaseSP2.DefaultView;
                        dv.RowFilter = "sp2_serviceplan=" + Entity.Code.ToString(); // drSP_Services["sp0_servicecode"].ToString();
                        dv.Sort = "sp2_branch DESC";
                        DataTable CaseSP2 = dv.ToTable();

                        if (Entity.Code.ToString().Trim() != Pri_SP.Trim())
                        {
                            string ReportName = Entity.SP_Desc.ToString(); //drSP_Services["sp0_description"].ToString();//"Sheet1";

                            ReportName = ReportName.Replace("/", "");

                            ReportName = Regex.Replace(ReportName, @"[^a-zA-Z0-9\s]", "");

                            ReportName = Entity.Code.Trim() + "_" + ReportName.Trim();
                            k++;
                            if (ReportName.Length >= 31)
                            {
                                ReportName = ReportName.Substring(0, 31);
                            }

                            if (PrvRepName == ReportName)
                            {
                                i++;
                                ReportName = ReportName.Substring(0, ReportName.Length - 2) + i.ToString();
                            }
                            PrvRepName = ReportName;

                            //if (Count > 1) ReportName = "Sheet" + Count.ToString();

                            sheet = book.Worksheets.Add(ReportName);
                            sheet.Table.DefaultRowHeight = 14.25F;

                            sheet.Table.Columns.Add(200);
                            sheet.Table.Columns.Add(200);
                            sheet.Table.Columns.Add(250);
                            sheet.Table.Columns.Add(52);
                            if (Member_Activity == "Y")
                                sheet.Table.Columns.Add(75);
                            sheet.Table.Columns.Add(75);
                            sheet.Table.Columns.Add(75);
                            if (chkbTargets.Checked)
                                sheet.Table.Columns.Add(75);

                            Row0 = sheet.Table.Rows.Add();

                            SPNum = true;

                            //if (!SPNum)
                            //{
                            //sheet = book.Worksheets.Add(drSP_Services["sp0_description"].ToString().Trim());
                            //}

                            cell = Row0.Cells.Add("SPM", DataType.String, "s94");
                            cell = Row0.Cells.Add("Branch", DataType.String, "s94");
                            cell = Row0.Cells.Add("Description", DataType.String, "s94");
                            cell = Row0.Cells.Add("Type", DataType.String, "s94");
                            if (Member_Activity == "Y")
                                cell = Row0.Cells.Add("Category", DataType.String, "s94");
                            cell = Row0.Cells.Add("Add Date", DataType.String, "s94");
                            cell = Row0.Cells.Add("Change Date", DataType.String, "s94");
                            if (chkbTargets.Checked)
                                cell = Row0.Cells.Add("Target", DataType.String, "s94");

                            bool CAMS = true; Branch = null; Priv_Branch = null; First = true;
                            if (CaseSP2.Rows.Count > 0)
                            {
                                foreach (DataRow dr in CaseSP2.Rows)
                                {
                                    SP_Plan_desc = Entity.Code.ToString(); //drSP_Services["sp0_description"].ToString().Trim();
                                    Branch = dr["sp2_branch"].ToString().Trim();
                                    if (Branch != Priv_Branch)
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        ////cb.EndText();
                                        //document.Add(table);
                                        //table.DeleteBodyRows();

                                        ////table.DeleteRow(1);


                                        //document.NewPage();
                                        if (Branch != "P")
                                            First = false;
                                        //cb.BeginText();
                                        string Service_desc = Entity.BpDesc.ToString(); //drSP_Services["sp0_pbranch_desc"].ToString();
                                        if (!First)
                                        {
                                            if (Branch.Trim() == Entity.B1Code.ToString().Trim())
                                                Service_desc = Entity.B1Desc;
                                            else if (Branch.Trim() == Entity.B2Code.ToString().Trim())
                                                Service_desc = Entity.B2Desc;
                                            else if (Branch.Trim() == Entity.B3Code.ToString().Trim())
                                                Service_desc = Entity.B3Desc;
                                            else if (Branch.Trim() == Entity.B4Code.ToString().Trim())
                                                Service_desc = Entity.B4Desc.ToString();
                                            else if (Branch.Trim() == Entity.B5Code.ToString().Trim())
                                                Service_desc = Entity.B5Desc.ToString();
                                            else if (Branch.Trim() == "9")
                                                Service_desc = "Additional Branch";

                                            Row0 = sheet.Table.Rows.Add();

                                        }
                                        CAMS = true;

                                        //cell.MergeAcross = 1;

                                        //cell = Row0.Cells.Add("Service :" + SP_Plan_desc.Trim(), DataType.String, "s95B");

                                        //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_add"].ToString().Trim()), DataType.String, "s95B");
                                        //cell = Row0.Cells.Add(LookupDataAccess.Getdate(drSP_Services["sp0_date_lstc"].ToString().Trim()), DataType.String, "s95B");

                                        //PdfPCell Sp_Plan = new PdfPCell(new Phrase("Service :" + SP_Plan_desc.Trim(), fc1));
                                        //Sp_Plan.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //Sp_Plan.Colspan = 2;
                                        //Sp_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //table.AddCell(Sp_Plan);

                                        if (SPNum)
                                            cell = Row0.Cells.Add("Service :" + Entity.SP_Desc.Trim(), DataType.String, "s96");
                                        else cell = Row0.Cells.Add("", DataType.String, "s96");
                                        //Row0 = sheet.Table.Rows.Add();
                                        if (!CAMS) Row0.Cells.Add("", DataType.String, "s96");
                                        else cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                        //cell.MergeAcross = 1;

                                        //cell = Row0.Cells.Add("", DataType.String, "s96");
                                        //cell = Row0.Cells.Add("", DataType.String, "s96");

                                        //PdfPCell ServiceDesc = new PdfPCell(new Phrase("Branch :" + Service_desc.Trim(), TblFontBold));
                                        //ServiceDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //ServiceDesc.Colspan = 2;
                                        //ServiceDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //table.AddCell(ServiceDesc);

                                        //string[] col = { "Type", "Description" };
                                        //for (int i = 0; i < col.Length; ++i)
                                        //{
                                        //    PdfPCell cell = new PdfPCell(new Phrase(col[i], TblFontBold));
                                        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        //    table.AddCell(cell);
                                        //}
                                        Priv_Branch = Branch;
                                        First = false; SPNum = false;
                                    }
                                    string CAMSType = dr["sp2_type"].ToString();
                                    string RNGCode = string.Empty, SPCOde = string.Empty, RNGAGENCY = string.Empty;
                                    string ServiceCount = string.Empty;
                                    if (CAMSType == "CA")
                                    {
                                        bool Active = true;
                                        foreach (CAMASTEntity drCAMAST in CAList)
                                        {
                                            if (drCAMAST.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                            {
                                                CAMSDesc = drCAMAST.Desc.ToString().Trim();
                                                if (drCAMAST.Active == "False") Active = false;
                                                break;
                                            }
                                            else
                                                CAMSDesc = "";
                                        }

                                        if (chkbTargets.Checked)
                                        {
                                            if (cmbDteRnge.Items.Count > 0)
                                            {
                                                RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                            }



                                            //DataTable dtserOut = new DataTable();
                                            //ServiceCount = string.Empty;
                                            //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, drSP_Services["sp0_servicecode"].ToString(), string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                            //if (dsSerOut.Tables.Count > 0)
                                            //    dtserOut = dsSerOut.Tables[0];
                                            //if (dtserOut.Rows.Count > 0)
                                            //{
                                            //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                            //}

                                            GoalSRHieEntity = _model.SPAdminData.Browse_RNGSRGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());
                                            if (GoalSRHieEntity.Count > 0)
                                            {
                                                GoalSRHieEntity = GoalSRHieEntity.FindAll(u => u.RNGSRGAH_TARGET != "");
                                                if (GoalSRHieEntity.Count > 0)
                                                    ServiceCount = GoalSRHieEntity.Sum(u => Convert.ToInt32(u.RNGSRGAH_TARGET.Trim())).ToString();
                                            }
                                        }

                                        if (!CAMS)
                                        {
                                            Row0 = sheet.Table.Rows.Add();
                                            cell = Row0.Cells.Add("", DataType.String, "s95");
                                            cell = Row0.Cells.Add("", DataType.String, "s95");
                                        }

                                        if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                        {
                                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95R");
                                            cell = Row0.Cells.Add("Service", DataType.String, "s95R");
                                            if (Member_Activity == "Y")
                                                cell = Row0.Cells.Add(Entity.Category.ToString().Trim(), DataType.String, "s95R");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");
                                            if (chkbTargets.Checked)
                                                cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95R");
                                        }
                                        else
                                        {
                                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add("Service", DataType.String, "s95");
                                            if (Member_Activity == "Y")
                                                cell = Row0.Cells.Add(Entity.Category.ToString().Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95");
                                            if (chkbTargets.Checked)
                                                cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95");
                                        }

                                        //PdfPCell RowType = new PdfPCell(new Phrase(CAMSType.Trim(), TableFont));
                                        //RowType.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //RowType.Border = iTextSharp.text.Rectangle.BOX;
                                        //table.AddCell(RowType);

                                        //PdfPCell RowDesc = new PdfPCell(new Phrase(CAMSDesc.Trim(), TableFont));
                                        //RowDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //RowDesc.Border = iTextSharp.text.Rectangle.BOX;
                                        //table.AddCell(RowDesc);
                                        CAMS = false;
                                    }
                                    else
                                    {
                                        string Type_Desc = string.Empty; bool Active = true;
                                        foreach (MSMASTEntity drMSMast in MSList)
                                        {
                                            if (drMSMast.Code.ToString().Trim() == dr["sp2_cams_code"].ToString().Trim())
                                            {
                                                CAMSDesc = drMSMast.Desc.ToString().Trim();
                                                MS_Type = drMSMast.Type1.ToString();
                                                if (drMSMast.Active == "False") Active = false;
                                                //if (MS_Type == "M")
                                                //    Type_Desc = "Milestone";
                                                //else
                                                Type_Desc = "Outcome";
                                                break;
                                            }
                                            else
                                                CAMSDesc = "";
                                        }

                                        if (chkbTargets.Checked)
                                        {
                                            if (cmbDteRnge.Items.Count > 0)
                                            {
                                                RNGCode = ((ListItem)cmbDteRnge.SelectedItem).Value.ToString();
                                                RNGAGENCY = ((ListItem)cmbDteRnge.SelectedItem).ID.ToString();
                                            }



                                            //DataTable dtserOut = new DataTable();
                                            //ServiceCount = string.Empty;
                                            //DataSet dsSerOut = SPAdminDB.GET_SPTARGET(RNGCode, drSP_Services["sp0_servicecode"].ToString(), string.Empty, string.Empty, RNGAGENCY, string.Empty, dr["sp2_cams_code"].ToString().Trim(), string.Empty, BaseForm.UserID);

                                            //if (dsSerOut.Tables.Count > 0)
                                            //    dtserOut = dsSerOut.Tables[0];
                                            //if (dtserOut.Rows.Count > 0)
                                            //{
                                            //    ServiceCount = dtserOut.AsEnumerable().Sum(x => x.Field<int>("SPT_TARGET")).ToString();
                                            //}

                                            GoalHieEntity = _model.SPAdminData.Browse_RNGGAH(RNGCode, RNGAGENCY, string.Empty, string.Empty, dr["sp2_cams_code"].ToString().Trim());
                                            if (GoalHieEntity.Count > 0)
                                            {
                                                GoalHieEntity = GoalHieEntity.FindAll(u => u.RNGGAH_TARGET != "");
                                                if (GoalHieEntity.Count > 0)
                                                    ServiceCount = GoalHieEntity.Sum(u => Convert.ToInt32(u.RNGGAH_TARGET.Trim())).ToString();
                                            }
                                        }

                                        if (!CAMS)
                                        {
                                            Row0 = sheet.Table.Rows.Add();
                                            cell = Row0.Cells.Add("", DataType.String, "s95");
                                            cell = Row0.Cells.Add("", DataType.String, "s95");
                                        }

                                        if (!Active || dr["sp2_active"].ToString().Trim() == "I")
                                        {
                                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95R");
                                            cell = Row0.Cells.Add(Type_Desc, DataType.String, "s95R");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95R");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95R");
                                            if (chkbTargets.Checked)
                                                cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95R");
                                        }
                                        else
                                        {
                                            cell = Row0.Cells.Add(CAMSDesc.Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add(Type_Desc, DataType.String, "s95");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_add"].ToString().Trim()), DataType.String, "s95");
                                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["sp2_date_lstc"].ToString().Trim()), DataType.String, "s95");
                                            if (chkbTargets.Checked)
                                                cell = Row0.Cells.Add(ServiceCount, DataType.String, "s95");
                                        }

                                        CAMS = false;
                                    }
                                }

                            }
                            else
                            {
                                Row0 = sheet.Table.Rows.Add();
                                cell = Row0.Cells.Add("Service :" + Entity.SP_Desc.ToString().Trim(), DataType.String, "s96");
                                string Service_desc = Entity.BpDesc.ToString().Trim();
                                cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");

                                if (!string.IsNullOrEmpty(Entity.B1Code.ToString().Trim()))
                                {
                                    Service_desc = Entity.B1Desc.ToString().Trim();
                                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s96");
                                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.B2Code.ToString().Trim()))
                                {
                                    Service_desc = Entity.B2Desc.ToString().Trim();
                                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s96");
                                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.B3Code.ToString().Trim()))
                                {
                                    Service_desc = Entity.B3Desc.ToString().Trim();
                                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s96");
                                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.B4Code.ToString().Trim()))
                                {
                                    Service_desc = Entity.B4Desc.ToString().Trim();
                                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s96");
                                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    }
                                }
                                if (!string.IsNullOrEmpty(Entity.B5Code.ToString().Trim()))
                                {
                                    Service_desc = Entity.B5Desc.ToString().Trim();
                                    if (!string.IsNullOrEmpty(Service_desc.Trim()))
                                    {
                                        Row0 = sheet.Table.Rows.Add();
                                        cell = Row0.Cells.Add("", DataType.String, "s96");
                                        cell = Row0.Cells.Add("Branch :" + Service_desc.Trim(), DataType.String, "s96");
                                    }
                                }

                            }
                            Count++;



                            Row0 = sheet.Table.Rows.Add();

                            cell = Row0.Cells.Add();
                            cell.StyleID = "s96";
                            cell.Data.Type = DataType.String;
                            cell.Data.Text = "";
                            cell.MergeAcross = 2;


                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add();
                            cell.StyleID = "s96";
                            cell.Data.Type = DataType.String;
                            cell.Data.Text = "Programs Associated";
                            cell.MergeAcross = 1;
                            cell = Row0.Cells.Add("", DataType.String, "s96");

                            string Hiename = null;
                            List<CASESP1Entity> sp_Hierachcyselectsp = SP_Hierarchies.FindAll(u => u.Code.Trim() == Entity.Code.ToString().Trim());
                            foreach (CASESP1Entity selctsp1item in sp_Hierachcyselectsp)
                            {
                                //HierarchyEntity hierchyEntity = null;
                                if (selctsp1item.Agency + selctsp1item.Dept + selctsp1item.Prog == "******")
                                {
                                    Hiename = "All Hierarchies";
                                    //hierchyEntity = new HierarchyEntity("**-**-**", "All Hierarchies");
                                }
                                else
                                    Hiename = "Not Defined in CASEHIE";

                                DataSet ds_Hie_Name = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(selctsp1item.Agency, selctsp1item.Dept, selctsp1item.Prog);
                                if (ds_Hie_Name.Tables.Count > 0)
                                {
                                    if (ds_Hie_Name.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds_Hie_Name.Tables.Count > 0 && ds_Hie_Name.Tables[0].Rows.Count > 0)
                                            Hiename = (ds_Hie_Name.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();

                                        if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"].ToString().Trim()))
                                            ds_Hie_Name.Tables[0].Rows[0]["HIE_DEPT"] = "**";
                                        if (string.IsNullOrEmpty(ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"].ToString().Trim()))
                                            ds_Hie_Name.Tables[0].Rows[0]["HIE_PROGRAM"] = "**";
                                    }
                                }


                                Row0 = sheet.Table.Rows.Add();
                                cell = Row0.Cells.Add(selctsp1item.Agency + "-" + selctsp1item.Dept + "-" + selctsp1item.Prog.Trim(), DataType.String, "s95");
                                cell = Row0.Cells.Add(Hiename.Trim(), DataType.String, "s95");
                                cell = Row0.Cells.Add("", DataType.String, "s95");


                            }
                            if (FundingList.Count > 0)
                            {

                                if (!string.IsNullOrEmpty(Entity.Funds.ToString().Trim()))
                                {


                                    Row0 = sheet.Table.Rows.Add();

                                    cell = Row0.Cells.Add();
                                    cell.StyleID = "s96";
                                    cell.Data.Type = DataType.String;
                                    cell.Data.Text = "";
                                    cell.MergeAcross = 2;


                                    Row0 = sheet.Table.Rows.Add();
                                    cell = Row0.Cells.Add();
                                    cell.StyleID = "s96";
                                    cell.Data.Type = DataType.String;
                                    cell.Data.Text = "Funding Sources";
                                    cell.MergeAcross = 1;
                                    cell = Row0.Cells.Add("", DataType.String, "s96");


                                    foreach (SPCommonEntity funditem in FundingList)
                                    {
                                        if (Entity.Funds.ToString().ToUpper().Contains(funditem.Code.ToUpper()))
                                        {
                                            Row0 = sheet.Table.Rows.Add();
                                            cell = Row0.Cells.Add(funditem.Code.ToString().Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add(funditem.Desc.Trim(), DataType.String, "s95");
                                            cell = Row0.Cells.Add("", DataType.String, "s95");

                                        }
                                    }


                                }

                            }

                            Pri_SP = Entity.Code.Trim();

                            if (chkbSPFielCntl.Checked)
                            {
                                string Catg = IsFieldPrint(Entity.Code.Trim());
                                if (Catg == "Y")
                                {
                                    this.GenerateWorksheetServiceCatg(book.Worksheets, Entity.Code.Trim());
                                }
                            }
                        }

                    }

                }




                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "SPREPAPP_Report.xls";

                //// downloadGateway.Version = file.Version;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

                FileInfo fiDownload = new FileInfo(PdfName);
                /// Need to check for file exists, is local file, is allow to read, etc...
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }

                //if (chkbSPFielCntl.Checked && !chkbPDF.Checked)
                //{
                //    ServicePlanFieldControl_Report();
                //}


            }




        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = true;
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";

            // -----------------------------------------------
            //  m2126043536976
            // -----------------------------------------------
            WorksheetStyle m2126043536976 = styles.Add("m2126043536976");
            m2126043536976.Font.Bold = true;
            m2126043536976.Font.FontName = "Arial";
            m2126043536976.Font.Color = "#000000";
            m2126043536976.Interior.Color = "#B0C4DE";
            m2126043536976.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043536976.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m2126043536976.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043536976.Alignment.WrapText = true;
            m2126043536976.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043536976.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043536976.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043536976.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043536976.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2126043536996
            // -----------------------------------------------
            WorksheetStyle m2126043536996 = styles.Add("m2126043536996");
            m2126043536996.Font.Bold = true;
            m2126043536996.Font.FontName = "Arial";
            m2126043536996.Font.Color = "#000000";
            m2126043536996.Interior.Color = "#B0C4DE";
            m2126043536996.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043536996.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m2126043536996.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043536996.Alignment.WrapText = true;
            m2126043536996.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043536996.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043536996.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043536996.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043536996.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2126043537016
            // -----------------------------------------------
            WorksheetStyle m2126043537016 = styles.Add("m2126043537016");
            m2126043537016.Font.Bold = true;
            m2126043537016.Font.FontName = "Arial";
            m2126043537016.Font.Color = "#000000";
            m2126043537016.Interior.Color = "#B0C4DE";
            m2126043537016.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043537016.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m2126043537016.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043537016.Alignment.WrapText = true;
            m2126043537016.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043537016.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043537016.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043537016.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043537016.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2126043539572
            // -----------------------------------------------
            WorksheetStyle m2126043539572 = styles.Add("m2126043539572");
            m2126043539572.Font.Bold = true;
            m2126043539572.Font.FontName = "Arial";
            m2126043539572.Font.Color = "#000000";
            m2126043539572.Interior.Color = "#B0C4DE";
            m2126043539572.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043539572.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m2126043539572.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043539572.Alignment.WrapText = true;
            m2126043539572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043539572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043539572.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043539572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043539572.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2126043539592
            // -----------------------------------------------
            WorksheetStyle m2126043539592 = styles.Add("m2126043539592");
            m2126043539592.Font.Bold = true;
            m2126043539592.Font.FontName = "Arial";
            m2126043539592.Font.Color = "#000000";
            m2126043539592.Interior.Color = "#B0C4DE";
            m2126043539592.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043539592.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m2126043539592.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043539592.Alignment.WrapText = true;
            m2126043539592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043539592.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043539592.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043539592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043539592.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2126043537600
            // -----------------------------------------------
            WorksheetStyle m2126043537600 = styles.Add("m2126043537600");
            m2126043537600.Font.Bold = true;
            m2126043537600.Font.FontName = "Arial";
            m2126043537600.Font.Color = "#000000";
            m2126043537600.Interior.Color = "#FFFFFF";
            m2126043537600.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043537600.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m2126043537600.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043537600.Alignment.WrapText = true;
            m2126043537600.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043537600.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043537600.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043537600.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043537600.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2126043537620
            // -----------------------------------------------
            WorksheetStyle m2126043537620 = styles.Add("m2126043537620");
            m2126043537620.Font.Bold = true;
            m2126043537620.Font.FontName = "Arial";
            m2126043537620.Font.Color = "#000000";
            m2126043537620.Interior.Color = "#FFFFFF";
            m2126043537620.Interior.Pattern = StyleInteriorPattern.Solid;
            m2126043537620.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m2126043537620.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2126043537620.Alignment.WrapText = true;
            m2126043537620.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2126043537620.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2126043537620.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            m2126043537620.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            m2126043537620.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle sS62 = styles.Add("sS62");
            sS62.Font.Bold = true;
            sS62.Font.FontName = "Arial";
            sS62.Font.Color = "#000000";
            sS62.Interior.Color = "#B0C4DE";
            sS62.Interior.Pattern = StyleInteriorPattern.Solid;
            sS62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            sS62.Alignment.Vertical = StyleVerticalAlignment.Top;
            sS62.Alignment.WrapText = true;
            sS62.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            sS62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            sS62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            sS62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            sS62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  sS81
            // -----------------------------------------------
            WorksheetStyle sS81 = styles.Add("sS81");
            sS81.Font.FontName = "Arial";
            sS81.Font.Color = "#000000";
            sS81.Interior.Color = "#FFFFFF";
            sS81.Interior.Pattern = StyleInteriorPattern.Solid;
            sS81.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            sS81.Alignment.Vertical = StyleVerticalAlignment.Top;
            sS81.Alignment.WrapText = true;
            sS81.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            sS81.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            sS81.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            sS81.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            sS81.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        }

        private void GenerateWorksheetServiceCatg_DE(DevExpress.Spreadsheet.Workbook workbook, string SelSerPlan, DevExpress_Excel_Properties oDevExpress_Excel_Properties)
        {
            DevExpress.Spreadsheet.Worksheet SheetDetails = workbook.Worksheets.Add(SelSerPlan + "_FieldControl"); ;
            //Worksheet sheet = sheets.Add(SelSerPlan + "_FieldControl");
            //sheet.Table.DefaultRowHeight = 14.25F;

            try
            {
                List<CASESP1Entity> SP_Hierarchies1 = _model.SPAdminData.Browse_CASESP1(SelSerPlan, null, null, null);
                CAMA_Details = _model.SPAdminData.Browse_CASESP2(SelSerPlan, null, null, null);
                SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SelSerPlan, null, null, null, null, null, null, null, null);
                PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);

                List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "PMTSTDFLDS");
                List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "FLDCNTL");
                List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "0", " ", "0", "          ", "hie");
                ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                int _Rowindex = 0; int _Columnindex = 0;
                #region Sheet Header
                workbook.Unit = DevExpress.Office.DocumentUnit.Point;
                SheetDetails.ActiveView.ShowGridlines = false;
                SheetDetails.Columns[0].Width = 150;
                SheetDetails.Columns[1].Width = 250;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "SPM";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                _Columnindex++;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Description";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                _Columnindex++;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        //cell = Row0.Cells.Add(Entity.PSTF_DESC.ToString().Trim(), DataType.String, "m2126043537016");
                        //cell.MergeAcross = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Entity.PSTF_DESC.ToString().Trim();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 3);
                        _Columnindex = _Columnindex + 3;
                    }
                }

                _Rowindex++; _Columnindex = 0;
                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                _Columnindex++;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                _Columnindex++;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        //cell = Row0.Cells.Add("Enabled", DataType.String, "sS62");
                        //cell = Row0.Cells.Add("Required", DataType.String, "sS62");
                        //cell = Row0.Cells.Add("Disabled", DataType.String, "sS62");

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Enabled";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Required";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Disabled";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        _Columnindex++;
                    }
                }

                foreach (CASESP1Entity spEntity in SP_Hierarchies1)
                {
                    _Rowindex++; _Columnindex = 0;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = spEntity.SP_Desc;
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                    _Columnindex++;

                    //cell = Row0.Cells.Add(spEntity.SP_Desc, DataType.String, "m2126043537600");
                    //cell.MergeAcross = 4;
                    int x = 0;
                    foreach (CASESP2Entity CAEntity in CAMA_Details)
                    {

                        if (CAEntity.Type1 == "CA")
                        {
                            if (x > 0)
                            {
                                //Row0 = sheet.Table.Rows.Add();
                                //cell = Row0.Cells.Add("", DataType.String, "m2126043537600");
                                //cell.MergeAcross = 4;
                                _Rowindex++; _Columnindex = 0;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                                _Columnindex++;
                            }
                            x = 1;
                            //cell = Row0.Cells.Add(CAEntity.CAMS_Desc, DataType.String, "m2126043537620");
                            //cell.MergeAcross = 6;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAEntity.CAMS_Desc;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                            _Columnindex++;

                            propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, SP_Hierarchies1[0].Code, SP_Hierarchies1[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                            propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                            if (propPMTFLDCNTLHEntity.Count == 0)
                            {
                                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "0", " ", "0", "          ", "hie");
                                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                            }

                            foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                            {
                                string isEnabled = string.Empty;
                                string isRequired = string.Empty;

                                if (propPMTFLDCNTLHEntity.Count > 0)
                                {
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                        isEnabled = "Y";
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                        isRequired = "Y";
                                }

                                if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                {
                                    //cell = Row0.Cells.Add(isEnabled.ToString(), DataType.String, "sS81");
                                    //cell = Row0.Cells.Add(isRequired.ToString(), DataType.String, "sS81");
                                    //cell = Row0.Cells.Add("", DataType.String, "sS81");

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = isEnabled;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = isRequired;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                    _Columnindex++;
                                }
                            }
                        }
                    }

                }

                #endregion
            }
            catch (Exception ex) { }

        }

        private void GenerateWorksheetServiceCatg_NEWFORMATDE(DevExpress.Spreadsheet.Workbook workbook, string SelSerPlan,
            DevExpress_Excel_Properties oDevExpress_Excel_Properties, DevExpress.Spreadsheet.Style xlFrameStyle)
        {
            DevExpress.Spreadsheet.Worksheet SheetDetails = workbook.Worksheets.Add(SelSerPlan + "_FieldControl"); ;
            //Worksheet sheet = sheets.Add(SelSerPlan + "_FieldControl");
            //sheet.Table.DefaultRowHeight = 14.25F;

            try
            {
                List<CASESP1Entity> SP_Hierarchies1 = _model.SPAdminData.Browse_CASESP1(SelSerPlan, null, null, null);
                CAMA_Details = _model.SPAdminData.Browse_CASESP2(SelSerPlan, null, null, null);
                SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SelSerPlan, null, null, null, null, null, null, null, null);
                PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);

                List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "PMTSTDFLDS");
                List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "FLDCNTL");
                List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "0", " ", "0", "          ", "hie");
                ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                int _Rowindex = 0; int _Columnindex = 0;



                #region Sheet Header
                workbook.Unit = DevExpress.Office.DocumentUnit.Point;
                SheetDetails.ActiveView.ShowGridlines = false;

                #region Column Widths
                SheetDetails.Columns[0].Width = 15;
                SheetDetails.Columns[1].Width = 80;
                SheetDetails.Columns[2].Width = 200;
                SheetDetails.Columns[3].Width = 70;
                SheetDetails.Columns[4].Width = 70;
                SheetDetails.Columns[5].Width = 70;
                int _cellIndx = 5;
                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        _cellIndx++;
                        SheetDetails.Columns[_cellIndx].Width = 50;
                    }
                }
                _cellIndx++;
                SheetDetails.Columns[_cellIndx].Width = 15;
                #endregion

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                _Columnindex++;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Service Plan - Fields Control";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, _cellIndx - 2); //cell.MergeAcross = 9;

                _Columnindex = _Columnindex + (_cellIndx - 2);
                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;


                _Rowindex++; _Columnindex = 0;
                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                _Columnindex++;



                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = xlFrameStyle;
                _Columnindex++;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "SPM";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                _Columnindex++;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Description";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                _Columnindex++;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Entity.PSTF_DESC.ToString().Trim();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 3);
                        _Columnindex = _Columnindex + 3;
                    }
                }

                _Rowindex++; _Columnindex = 0;
                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                _Columnindex++;

                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                _Columnindex++;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        //cell = Row0.Cells.Add("Enabled", DataType.String, "sS62");
                        //cell = Row0.Cells.Add("Required", DataType.String, "sS62");
                        //cell = Row0.Cells.Add("Disabled", DataType.String, "sS62");

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Enabled";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Required";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Disabled";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        _Columnindex++;
                    }
                }

                foreach (CASESP1Entity spEntity in SP_Hierarchies1)
                {
                    _Rowindex++; _Columnindex = 0;

                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = spEntity.SP_Desc;
                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                    _Columnindex++;

                    //cell = Row0.Cells.Add(spEntity.SP_Desc, DataType.String, "m2126043537600");
                    //cell.MergeAcross = 4;
                    int x = 0;
                    foreach (CASESP2Entity CAEntity in CAMA_Details)
                    {

                        if (CAEntity.Type1 == "CA")
                        {
                            if (x > 0)
                            {
                                //Row0 = sheet.Table.Rows.Add();
                                //cell = Row0.Cells.Add("", DataType.String, "m2126043537600");
                                //cell.MergeAcross = 4;
                                _Rowindex++; _Columnindex = 0;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                                _Columnindex++;
                            }
                            x = 1;
                            //cell = Row0.Cells.Add(CAEntity.CAMS_Desc, DataType.String, "m2126043537620");
                            //cell.MergeAcross = 6;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAEntity.CAMS_Desc;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                            _Columnindex++;

                            propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, SP_Hierarchies1[0].Code, SP_Hierarchies1[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                            propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                            if (propPMTFLDCNTLHEntity.Count == 0)
                            {
                                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies1[0].Agency + SP_Hierarchies1[0].Dept + SP_Hierarchies1[0].Prog, "0", " ", "0", "          ", "hie");
                                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                            }

                            foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                            {
                                string isEnabled = string.Empty;
                                string isRequired = string.Empty;

                                if (propPMTFLDCNTLHEntity.Count > 0)
                                {
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                        isEnabled = "Y";
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                        isRequired = "Y";
                                }

                                if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                {
                                    //cell = Row0.Cells.Add(isEnabled.ToString(), DataType.String, "sS81");
                                    //cell = Row0.Cells.Add(isRequired.ToString(), DataType.String, "sS81");
                                    //cell = Row0.Cells.Add("", DataType.String, "sS81");

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = isEnabled;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = isRequired;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNCC;
                                    _Columnindex++;
                                }
                            }
                        }
                    }

                }

                #endregion
            }
            catch (Exception ex) { }

        }
        private void GenerateWorksheetServiceCatg(CarlosAg.ExcelXmlWriter.WorksheetCollection sheets, string SelSerPlan)
        {
            CarlosAg.ExcelXmlWriter.Worksheet sheet = sheets.Add(SelSerPlan + "_FieldControl");
            sheet.Table.DefaultRowHeight = 14.25F;

            try
            {
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(SelSerPlan, null, null, null);
                CAMA_Details = _model.SPAdminData.Browse_CASESP2(SelSerPlan, null, null, null);
                SP_Header_Rec = _model.SPAdminData.Browse_CASESP0(SelSerPlan, null, null, null, null, null, null, null, null);
                PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);

                List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", PPC_List[0].Ext_2, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "PMTSTDFLDS");
                List<FldcntlHieEntity> FldcntlEntity = _model.FieldControls.GetFLDCNTLHIE("CASE0063", SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "FLDCNTL");
                List<PMTFLDCNTLHEntity> ProgDefFields = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Ext_2, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                ProgDefFields = ProgDefFields.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code.Trim());

                WorksheetRow Row0 = sheet.Table.Rows.Add();
                WorksheetCell cell;
                cell = Row0.Cells.Add("SPM", DataType.String, "m2126043536976");
                cell.MergeAcross = 4;
                cell = Row0.Cells.Add("Description", DataType.String, "m2126043536996");
                cell.MergeAcross = 6;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        cell = Row0.Cells.Add(Entity.PSTF_DESC.ToString().Trim(), DataType.String, "m2126043537016");
                        cell.MergeAcross = 2;
                    }
                }

                Row0 = sheet.Table.Rows.Add();
                cell = Row0.Cells.Add("", DataType.String, "m2126043539572");
                cell.MergeAcross = 4;
                cell = Row0.Cells.Add("", DataType.String, "m2126043539592");
                cell.MergeAcross = 6;

                foreach (PMTSTDFLDSEntity Entity in pmtstdfldsentity)
                {
                    if (Entity.PSTF_FLD_CODE.Substring(1, 1) != "A" && Entity.PSTF_FLD_CODE.Substring(1, 1) != "a" && Entity.PSTF_FLD_CODE != "S00022" && Entity.PSTF_FLD_CODE != "S00023" && Entity.PSTF_FLD_CODE != "S00212" && Entity.PSTF_FLD_CODE != "S00216" && Entity.PSTF_FLD_CODE != "S00206" && Entity.PSTF_FLD_CODE != "S00207" && Entity.PSTF_FLD_CODE != "S00208" && Entity.PSTF_FLD_CODE != "S00209" && Entity.PSTF_FLD_CODE != "S00210" && Entity.PSTF_FLD_CODE != "S00211")
                    {
                        cell = Row0.Cells.Add("Enabled", DataType.String, "sS62");
                        cell = Row0.Cells.Add("Required", DataType.String, "sS62");
                        cell = Row0.Cells.Add("Disabled", DataType.String, "sS62");
                    }
                }

                //Row0 = sheet.Table.Rows.Add();

                foreach (CASESP1Entity spEntity in SP_Hierarchies)
                {
                    Row0 = sheet.Table.Rows.Add();
                    cell = Row0.Cells.Add(spEntity.SP_Desc, DataType.String, "m2126043537600");
                    cell.MergeAcross = 4;
                    int x = 0;
                    foreach (CASESP2Entity CAEntity in CAMA_Details)
                    {
                        if (CAEntity.Type1 == "CA")
                        {
                            if (x > 0)
                            {
                                Row0 = sheet.Table.Rows.Add();
                                cell = Row0.Cells.Add("", DataType.String, "m2126043537600");
                                cell.MergeAcross = 4;
                            }
                            x = 1;
                            cell = Row0.Cells.Add(CAEntity.CAMS_Desc, DataType.String, "m2126043537620");
                            cell.MergeAcross = 6;

                            propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, SP_Hierarchies[0].Code, SP_Hierarchies[0].BpCode.ToString(), CAEntity.Orig_Grp.ToString(), CAEntity.CamCd.ToString(), "SP");
                            propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);

                            if (propPMTFLDCNTLHEntity.Count == 0)
                            {
                                propPMTFLDCNTLHEntity = _model.FieldControls.GETPMTFLDCNTLHSP("CASE0063", PPC_List[0].Code, SP_Hierarchies[0].Agency + SP_Hierarchies[0].Dept + SP_Hierarchies[0].Prog, "0", " ", "0", "          ", "hie");
                                propPMTFLDCNTLHEntity = propPMTFLDCNTLHEntity.FindAll(u => u.PMFLDH_CATG == PPC_List[0].Code);
                            }

                            foreach (PMTSTDFLDSEntity YNEntity in pmtstdfldsentity)
                            {
                                string isEnabled = string.Empty;
                                string isRequired = string.Empty;

                                if (propPMTFLDCNTLHEntity.Count > 0)
                                {
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                                        isEnabled = "Y";
                                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == YNEntity.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                                        isRequired = "Y";
                                }

                                if (YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "A" && YNEntity.PSTF_FLD_CODE.Substring(1, 1) != "a" && YNEntity.PSTF_FLD_CODE != "S00022" && YNEntity.PSTF_FLD_CODE != "S00023" && YNEntity.PSTF_FLD_CODE != "S00212" && YNEntity.PSTF_FLD_CODE != "S00216" && YNEntity.PSTF_FLD_CODE != "S00206" && YNEntity.PSTF_FLD_CODE != "S00207" && YNEntity.PSTF_FLD_CODE != "S00208" && YNEntity.PSTF_FLD_CODE != "S00209" && YNEntity.PSTF_FLD_CODE != "S00210" && YNEntity.PSTF_FLD_CODE != "S00211")
                                {
                                    cell = Row0.Cells.Add(isEnabled.ToString(), DataType.String, "sS81");
                                    cell = Row0.Cells.Add(isRequired.ToString(), DataType.String, "sS81");
                                    cell = Row0.Cells.Add("", DataType.String, "sS81");
                                }
                            }
                        }
                    }

                }

                // -----------------------------------------------
                //  Options
                // -----------------------------------------------
                sheet.Options.ProtectObjects = false;
                sheet.Options.ProtectScenarios = false;
                sheet.Options.PageSetup.Header.Margin = 0.3F;
                sheet.Options.PageSetup.Footer.Margin = 0.3F;
                sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
                sheet.Options.PageSetup.PageMargins.Left = 0.7F;
                sheet.Options.PageSetup.PageMargins.Right = 0.7F;
                sheet.Options.PageSetup.PageMargins.Top = 0.75F;
            }
            catch (Exception ex) { }

        }


    }
}