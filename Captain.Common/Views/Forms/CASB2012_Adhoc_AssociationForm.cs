#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB2012_Adhoc_AssociationForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;

        #endregion

        public CASB2012_Adhoc_AssociationForm(ControlCard_Entity card_entity, BaseForm baseForm)
        {
            InitializeComponent();
            Card_Entity = card_entity;
            BaseForm = baseForm;
            Cmb_Modules.Focus();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Fill_Associations_List();
            Fill_Modules_Dropdown();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Cuttent_Operator { get; set; }

        public string Mode { get; set; }

        public string SP_Code { get; set; }


        //public string M_Hierarchy { get; set; }

        ////public string M_HieDesc { get; set; }

        //public string M_Year { get; set; }

        public string SchSite { get; set; }

        public string SchDate { get; set; }

        public string SchType { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ControlCard_Entity Card_Entity { get; set; }

        #endregion

        string Sql_Reslut_Msg = string.Empty;

        string Img_Saved = Consts.Icons.ico_Save;   // new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("save.gif");
        string Img_Blank = Consts.Icons.ico_Blank;  // new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        string Img_Tick = Consts.Icons.ico_Tick;    // new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");


        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private void Fill_Modules_Dropdown()
        {
            Cmb_Modules.Items.Clear();

            List<CommonEntity> commonappl = _model.lookupDataAccess.GetCapAppl();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("All Modules", "00"));

            foreach (CommonEntity Entity in commonappl)
            {
                if (Entity.Code.Trim() != "01" || Entity.Code.Trim() != "99")
                    listItem.Add(new ListItem(Entity.Desc, Entity.Code));
            }

            //listItem.Add(new ListItem("Head Start", "02"));
            //listItem.Add(new ListItem("Case Management", "03"));
            //listItem.Add(new ListItem("Emergency Assistance", "05"));
            Cmb_Modules.Items.AddRange(listItem.ToArray());
            Cmb_Modules.SelectedIndex = 0;
        }

        List<ControlCard_Entity> Card_List = new List<ControlCard_Entity>();
        private void Fill_Saved_Parameters_Dropdown()
        {
            IDs_Grid.Rows.Clear();
            Select_Users_Grid.Rows.Clear();
            Selected_Users_Grid.Rows.Clear();
            Txt_AssoC_Desc.Clear();
            Btn_Save.Visible = false;

            Card_List = _model.AdhocData.Browse_CONTROLCARD(Card_Entity, "Browse");

            if (Card_List.Count > 0)
            {
                int Tmp_Row_Count = 0, Tmp_Sel_Index = 0 ;
                string Tmp_Date = string.Empty, Tmp_Module = string.Empty;
                foreach (ControlCard_Entity Entity in Card_List)
                {
                    Tmp_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.LSTC_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    switch (Entity.Module)
                    {
                        case "02": Tmp_Module = "HSS"; break;
                        case "03": Tmp_Module = "CM"; break;
                    }

                    if (((ListItem)Cmb_Modules.SelectedItem).Value.ToString() == Entity.Module ||
                        ((ListItem)Cmb_Modules.SelectedItem).Value.ToString() == "00")
                    {
                        if (Edited_Adhoc_ID == Entity.Card_ID)
                            Tmp_Sel_Index = Tmp_Row_Count;

                        IDs_Grid.Rows.Add(Entity.Card_ID, Entity.Card_DESC, Tmp_Module, Tmp_Date, Entity.UserID);
                        Tmp_Row_Count++;
                    }
                }

                if (Tmp_Row_Count > 0)
                {
                    IDs_Grid.CurrentCell = IDs_Grid.Rows[Tmp_Sel_Index].Cells[1];

                    //int scrollPosition = 0;
                    //scrollPosition = IDs_Grid.CurrentCell.RowIndex;
                    //int CurrentPage = (scrollPosition / IDs_Grid.ItemsPerPage);
                    //CurrentPage++;
                    //IDs_Grid.CurrentPage = CurrentPage;
                    //IDs_Grid.FirstDisplayedScrollingRowIndex = scrollPosition;
                    Btn_Save.Visible = true;
                    Fill_Users_List();
                }
            }
        }

        private void Fill_Users_List()
        {
            Select_Users_Grid.Rows.Clear();
            Selected_Users_Grid.Rows.Clear();
            Txt_AssoC_Desc.Clear();

            DataSet ds = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<ListItem> listItem = new List<ListItem>();
                    listItem.Clear();   bool User_Assoc_Flg = false;

                    if (Associations_List.Count > 0)
                        Txt_AssoC_Desc.Text = Associations_List[0].Adh_Assc_Desc;

                    foreach (DataRow dr in dt.Rows)
                    {
                        User_Assoc_Flg = false;
                        foreach (AdhocAssoc_Entity Assoc in Associations_List)
                        {
                            if (Assoc.Adh_Assc_UserID == dr["PWR_EMPLOYEE_NO"].ToString().Trim() &&
                                Assoc.Adh_ID == IDs_Grid.CurrentRow.Cells["ID"].Value.ToString() &&
                                Assoc.Adh_ID_UserID == IDs_Grid.CurrentRow.Cells["ID_USERID"].Value.ToString())
                                { User_Assoc_Flg = true;    break; }
                        }

                        if (User_Assoc_Flg)
                        {
                            Selected_Users_Grid.Rows.Add(true, dr["PWR_EMPLOYEE_NO"].ToString().Trim(), "Y");//Img_Tick
                            Selected_Users_Grid.Rows[0].Selected = true;
                        }
                        else
                        {
                            if (BaseForm.UserID != dr["PWR_EMPLOYEE_NO"].ToString().Trim())
                            {
                                Select_Users_Grid.Rows.Add(false, dr["PWR_EMPLOYEE_NO"].ToString().Trim(), "N");//Img_Blank
                                Select_Users_Grid.Rows[0].Selected = true;
                            }
                        }
                    }

                    foreach (AdhocAssoc_Entity Assoc in Associations_List)
                    {
                        if (Assoc.Adh_ID_UserID == IDs_Grid.CurrentRow.Cells["ID_USERID"].Value.ToString() &&
                            Assoc.Adh_ID == IDs_Grid.CurrentRow.Cells["ID"].Value.ToString())
                        { Txt_AssoC_Desc.Text = Assoc.Adh_Assc_Desc; break; }
                    }
                }
            }

        }

        private void Cmb_Modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            Edited_Adhoc_ID = string.Empty;
            Fill_Saved_Parameters_Dropdown();
        }


        List<AdhocAssoc_Entity> Associations_List = new List<AdhocAssoc_Entity>();
        private void Fill_Associations_List()
        {
            AdhocAssoc_Entity Search_Entity = new AdhocAssoc_Entity(true);
            Search_Entity.Adh_ID_UserID = Card_Entity.UserID;

            Associations_List = _model.AdhocData.Browse_ADHOCASSOC(Search_Entity, "Browse");
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Select_Users_Click(object sender, EventArgs e)
        {

        }

        string Edited_Adhoc_ID = string.Empty;
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Edited_Adhoc_ID = string.Empty;
                int Assoc_User_Cnt = 0;
                AdhocAssoc_Entity Search_Entity = new AdhocAssoc_Entity(true);
                Search_Entity.Rowtype = "I";
                Search_Entity.Adh_ID = IDs_Grid.CurrentRow.Cells["ID"].Value.ToString();
                Search_Entity.Adh_ID_UserID = IDs_Grid.CurrentRow.Cells["ID_USERID"].Value.ToString();
                Search_Entity.Adh_Assc_Desc = Txt_AssoC_Desc.Text.Trim();
                Search_Entity.Adh_Assc_UserID = "Temp ID";
                Search_Entity.Adh_Assc_Module = BaseForm.BusinessModuleID;
                Search_Entity.Adh_Assc_AddOpr = BaseForm.UserID;

                StringBuilder str = new StringBuilder();
                str.Append("<Rows>");

                foreach (DataGridViewRow dr in Select_Users_Grid.Rows) //Right_Sel_Sw
                {
                    if (dr.Cells["Left_Sel_Sw"].Value.ToString() == "Y")
                    { str.Append("<Row USERID = \"" + dr.Cells["L_User"].Value.ToString() + "\" />"); Assoc_User_Cnt++; }
                }

                int unassoc_Cnt = 0; string Del_All_Assoc = "N";
                foreach (DataGridViewRow dr in Selected_Users_Grid.Rows) //Right_Sel_Sw
                {
                    if (dr.Cells["Right_Sel_Sw"].Value.ToString() == "Y")
                    { str.Append("<Row USERID = \"" + dr.Cells["R_User"].Value.ToString() + "\" />"); Assoc_User_Cnt++; }
                    else
                    { unassoc_Cnt++; }
                }

                str.Append("</Rows>");

                if (unassoc_Cnt > 0 && Assoc_User_Cnt <= 0)
                    Del_All_Assoc = "Y";

                if (Assoc_User_Cnt > 0 || Del_All_Assoc == "Y")
                {
                    if (_model.AdhocData.UpdateADHOCASSOC(Search_Entity, str.ToString(), Del_All_Assoc, out  Sql_Reslut_Msg))
                    {
                        Edited_Adhoc_ID = Search_Entity.Adh_ID;
                        AlertBox.Show("Association Saved Successfully");
                        Fill_Associations_List();
                        Fill_Saved_Parameters_Dropdown();
                    }
                    else
                        AlertBox.Show("Error in saving: " + Sql_Reslut_Msg, MessageBoxIcon.Warning);
                }
                else
                    AlertBox.Show("Please select at least one user to Associate.",MessageBoxIcon.Warning);
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(Txt_AssoC_Desc.Text.Trim()))
            {
                _errorProvider.SetError(Txt_AssoC_Desc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Association Description".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Txt_AssoC_Desc, null);

            return (isValid);
        }

        private void Select_Users_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Select_Users_Grid.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 0)
                {
                    if (Select_Users_Grid.CurrentRow.Cells["Left_Sel_Sw"].Value.ToString() == "Y")
                    {
                        Select_Users_Grid.CurrentRow.Cells["L_Image"].Value = false;//Img_Blank;
                        Select_Users_Grid.CurrentRow.Cells["Left_Sel_Sw"].Value = "N";
                    }
                    else
                    {
                        Select_Users_Grid.CurrentRow.Cells["L_Image"].Value = true;//Img_Tick;
                        Select_Users_Grid.CurrentRow.Cells["Left_Sel_Sw"].Value = "Y";
                    }
                }
            }
        }

        private void Selected_Users_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Selected_Users_Grid.Rows.Count > 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 0)
                {
                    if (Selected_Users_Grid.CurrentRow.Cells["Right_Sel_Sw"].Value.ToString() == "Y")
                    {
                        Selected_Users_Grid.CurrentRow.Cells["R_Image"].Value = false;//Img_Blank;
                        Selected_Users_Grid.CurrentRow.Cells["Right_Sel_Sw"].Value = "N";
                    }
                    else
                    {
                        Selected_Users_Grid.CurrentRow.Cells["R_Image"].Value = true;//Img_Tick;
                        Selected_Users_Grid.CurrentRow.Cells["Right_Sel_Sw"].Value = "Y";
                    }
                }
            }
        }

        private void IDs_Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (IDs_Grid.Rows.Count > 0)
                Fill_Users_List();
        }
    }
}