#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Views.UserControls;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ScreenControlAssignmentForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public ScreenControlAssignmentForm(BaseForm baseForm, string mode, string Scrcode, string Hie, string strPrivilegesName, int seltab_index)
        {
            BaseForm = baseForm;
            _model = new CaptainModel();
            InitializeComponent();
            Mode = mode;
            this.Text = "Incomplete Intake Controls  - " + mode;
            Seltab_Tndex = seltab_index;
            if (mode.ToUpper().Equals("EDIT"))
            {
                if (Hie != "Custom_Edit")
                    ScrHierarchy = Hie;
            }
            else
            {
                PBHierarchy.Visible = true;
            }

            Get_Cntl_From_HIE = "";
            if (Mode.ToUpper() == "ADD" && Hie != null)
            {
                if (Hie != "Custom_Edit")
                    Get_Cntl_From_HIE = Hie;

            }

            ScrCode = Scrcode;
            UserName = BaseForm.UserID;
          //  lblHeader.Text = strPrivilegesName;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            FillAllCombos();
            CommonFunctions.SetComboBoxValue(CmbScreen, ScrCode);
            fillGrid();
            
        }
        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string ScrCode { get; set; }

        public string SelectColName { get; set; }

        public string SelectChildCode { get; set; }

        public bool IsSaveValid { get; set; }

        public string ScrHierarchy { get; set; }

        public string Get_Cntl_From_HIE { get; set; }

        public string UserName { get; set; }

        public int Seltab_Tndex { get; set; }





        List<FLDSCRSEntity> FLDSCRS_List = new List<FLDSCRSEntity>();
        private void FillAllCombos()
        {
            CmbScreen.Items.Clear();
            List<ListItem> listItem2 = new List<ListItem>();
            //listItem.Add(new ListItem("Form Title                                    -  SCR-CODE", SCR-CODE, Empty, Custom_Filedls_Flag));

            FLDSCRSEntity Search_Entity = new FLDSCRSEntity(true);
            Search_Entity.Called_By = "ADMNCNTL";
            FLDSCRS_List = _model.FieldControls.Browse_FLDSCRS(Search_Entity);
            string Tmp_Desc = string.Empty;

            foreach (FLDSCRSEntity Entity in FLDSCRS_List)
            {
                //if ((Entity.Scr_Code == "CASE2001" && Entity.Scr_Sub_Code == "00") || Entity.Scr_Code != "CASE2001")
                //{
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-30}", Entity.Scr_Desc) + String.Format("{0,8}", " - " + Entity.Scr_Code);
                    CmbScreen.Items.Add(new ListItem(Tmp_Desc, Entity.Scr_Code, Entity.Scr_Desc, Entity.Cust_Ques_SW));
                //}
            }


        }
        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (Mode.ToUpper() == "ADD")
            {
                if (string.IsNullOrEmpty(TxtHierarchy.Text))
                {
                    _errorProvider.SetError(TxtHieDeSC, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHierarchy.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(TxtHieDeSC, null);
                }
            }
            return isValid;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                ScaFldsHieEntity oScaFldsHieEntity = new ScaFldsHieEntity();
                if (gvwCateDetails.Rows.Count > 0)
                {
                    bool InupSCData = false;
                    foreach (DataGridViewRow datarow in gvwCateDetails.Rows)
                    {
                        oScaFldsHieEntity.RecType = Mode;
                        oScaFldsHieEntity.ScrCode = ScrCode;
                        oScaFldsHieEntity.ScrHie = TxtHierarchy.Text.Substring(0, 2) + TxtHierarchy.Text.Substring(3, 2) + TxtHierarchy.Text.Substring(6, 2);
                        oScaFldsHieEntity.ScahCode = datarow.Cells["gvtCateCode"].Value.ToString();
                        oScaFldsHieEntity.Active = datarow.Cells["gvt_sel"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                        oScaFldsHieEntity.Sel = datarow.Cells["gvtCatesel"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                        oScaFldsHieEntity.Msg = datarow.Cells["gvt_MSG"].Value == null ? string.Empty : datarow.Cells["gvt_MSG"].Value.ToString();
                        InupSCData = _model.FieldControls.InsertUpdateSCAFLDSHIE(oScaFldsHieEntity);
                    }
                    if (InupSCData)
                        this.Close();
                }
                if (Mode == "Add")
                    AlertBox.Show("Saved Successfully");
                else
                    AlertBox.Show("Updated Successfully");
            }
        }
        private void PBHierarchy_Click(object sender, EventArgs e)
        {

            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master", "I", "*", "R");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;
            List<ScaFldsHieEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSHIEDATA(ScrCode, string.Empty, string.Empty);


            bool HieExist = false;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    string Tmpstr = null;
                    string Tmpstr1 = null;
                    foreach (ScaFldsHieEntity dr in ScaFldsHiedata)
                    {
                        Tmpstr = dr.ScrHie;
                        Tmpstr1 = row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2);
                        if (Tmpstr == Tmpstr1)
                            HieExist = true;
                    }
                    if (!HieExist)
                    {
                        TxtHierarchy.Text = row.Code;  //.Substring(0, 2) + '-' + row.Code.Substring(2, 2) + '-' + row.Code.Substring(4, 2)
                        TxtHieDeSC.Text = "   " + row.HirarchyName.ToString();
                        //_errorProvider.SetError(PBHierarchy, null);
                        lblReqHie.Visible = false;
                        _errorProvider.SetError(lblReqHie, null);

                    }
                    else
                    {
                        TxtHierarchy.Clear(); TxtHieDeSC.Clear();
                        AlertBox.Show("Incomplete Intake Controls are already Defined for Hierarchy '" + row.Code + "' !!!", MessageBoxIcon.Warning);
                    }
                    if (HieExist)
                        break;
                }
            }
        }
        string Copy_From_Hie = "";
        private void ScreenControlAssignmentForm_Load(object sender, EventArgs e)
        {
            if (Mode == "Add" && !string.IsNullOrEmpty(Get_Cntl_From_HIE.Trim()))
            {
                string Tmp_Hie_str = Get_Cntl_From_HIE, Prog_Name = "";
                if (Tmp_Hie_str.Length == 6)
                {
                    Tmp_Hie_str = Tmp_Hie_str.Substring(0, 2) + "-" + Tmp_Hie_str.Substring(2, 2) + "-" + Tmp_Hie_str.Substring(4, 2);
                    if (Get_Cntl_From_HIE != "******")
                    {                       
                        Prog_Name = Copy_From_Hie;
                    }
                }
                 MessageBox.Show("Do you want to Copy Incomplete Intake Controls from " + Tmp_Hie_str + "  " + Prog_Name + " ...?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: GetIncomepleteHierchydata);
            }

        }

        private void GetIncomepleteHierchydata(DialogResult dialogResult)
        {
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //if (senderForm != null)
            //{
                if (dialogResult.Equals(DialogResult.Yes))
                {
                    gvwCateDetails.Rows.Clear();
                    List<ScaFldsHieEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSHIEDATA(ScrCode, string.Empty, Get_Cntl_From_HIE);
                    foreach (ScaFldsHieEntity iTem in ScaFldsHiedata)
                    {
                        gvwCateDetails.Rows.Add(iTem.scaDesc, iTem.Sel == "Y" ? true : false, iTem.ScahCode, iTem.Active == "Y" ? true : false, iTem.Msg);
                    }

                    //foreach (DataGridViewRow item in gvwCateDetails.Rows)
                    //{
                    //    string strCode = item.Cells["gvtCateCode"].Value == null ? string.Empty : item.Cells["gvtCateCode"].Value.ToString();
                    //    ScaFldsHieEntity ScaFldsHieEntitydata = ScaFldsHiedata.Find(u => u.ScahCode == strCode);
                    //    if(ScaFldsHieEntitydata!=null)
                    //    {
                    //        item.Cells["gvtCatesel"].Value = ScaFldsHieEntitydata.Sel == "Y" ? true : false;
                    //        item.Cells["gvt_MSG"].Value = ScaFldsHieEntitydata.Msg;
                    //    }
                    //}
                }
           // }
        }


        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void fillGrid()
        {
            gvwCateDetails.Rows.Clear();
            if (Mode.ToUpper() == "ADD")
            {
                List<ScafldsEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSDATA(ScrCode, string.Empty, string.Empty);
                foreach (ScafldsEntity iTem in ScaFldsHiedata)
                {
                    gvwCateDetails.Rows.Add(iTem.ScaDesc, false, iTem.ScafldCode,false,string.Empty);
                }
            }
            else if (Mode.ToUpper() == "EDIT")
            {
                TxtHieDeSC.Text = "All Hierarchies";
                TxtHierarchy.Text = ScrHierarchy.Substring(0, 2) + '-' + ScrHierarchy.Substring(2, 2) + '-' + ScrHierarchy.Substring(4, 2);
                if (ScrHierarchy != "******")
                {
                    DataSet Prog = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(ScrHierarchy.Substring(0, 2), ScrHierarchy.Substring(2, 2), ScrHierarchy.Substring(4, 2));
                    if (Prog.Tables[0].Rows.Count > 0)
                        TxtHieDeSC.Text = "   " + (Prog.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                    else
                        TxtHieDeSC.Text = "Description Not Defined";
                }
                List<ScaFldsHieEntity> ScaFldsHiedata = _model.FieldControls.GETSCAFLDSHIEDATA(ScrCode, string.Empty, ScrHierarchy);
                foreach (ScaFldsHieEntity iTem in ScaFldsHiedata)
                {
                    gvwCateDetails.Rows.Add(iTem.scaDesc, iTem.Sel == "Y" ? true : false, iTem.ScahCode,iTem.Active == "Y" ? true : false,iTem.Msg);
                }
            }
        }
    }
}