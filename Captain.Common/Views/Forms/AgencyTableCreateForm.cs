#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using Captain.Common.Views.Controls.Compatibility;
//using Gizmox.WebGUI.Common.Interfaces;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AgencyTableCreateForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;

        #endregion

        public AgencyTableCreateForm(BaseForm baseForm, string mode, string SelAgencyType, string strCode, string strPrivilegesName)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Mode = mode;
            SelectAgyType = SelAgencyType;
            Fillcmbsearchby();

            TxtCode.Validator = TextBoxValidation.IntegerMaskValidator;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            if (Mode.Equals("Edit"))
            {
                FillAgencyTableForm(SelectAgyType);
                this.Text = strPrivilegesName + " - Edit";
                //this.Text = strCode + " - Edit";
                TxtCode.Enabled = false;
                //PbInfo.Visible = true;
                TxtDesc.Focus();
            }
            else
            {
                FillApplications(string.Empty);
                //this.Text = strCode + " - Add";
                this.Text = strPrivilegesName + " - Add";
                TxtCode.Focus();
            }

            ToolTip tooltip = new ToolTip();
            //tooltip.SetToolTip(Hepl, "Help");
        }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string SelectAgyType { get; set; }

        public string SelectColName { get; set; }

        public string SelectChildCode { get; set; }

        public bool IsSaveValid { get; set; }

        private void Fillcmbsearchby()
        {
            CmbSortBy.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("CELL - 1", "1"));
            listItem.Add(new ListItem("CELL - 2", "2"));
            listItem.Add(new ListItem("CELL - 3", "3"));
            listItem.Add(new ListItem("CELL - 4", "4"));
            listItem.Add(new ListItem("CELL - 5", "5"));
            listItem.Add(new ListItem("CELL - 6", "6"));
            listItem.Add(new ListItem("CELL - 7", "7"));
            listItem.Add(new ListItem("CELL - 8", "8"));
            listItem.Add(new ListItem("CELL - 9", "9"));
            //listItem.Add(new ListItem("CODE", "10"));
            CmbSortBy.Items.AddRange(listItem.ToArray());

            listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("CELL - 1", "1"));
            listItem.Add(new ListItem("CELL - 2", "2"));
            listItem.Add(new ListItem("CELL - 3", "3"));
            listItem.Add(new ListItem("CELL - 4", "4"));
            listItem.Add(new ListItem("CELL - 5", "5"));
            listItem.Add(new ListItem("CELL - 6", "6"));
            CmbCode.Items.AddRange(listItem.ToArray());

            listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("CELL - 1", "1"));
            listItem.Add(new ListItem("CELL - 2", "2"));
            listItem.Add(new ListItem("CELL - 3", "3"));
            listItem.Add(new ListItem("CELL - 4", "4"));
            listItem.Add(new ListItem("CELL - 5", "5"));
            listItem.Add(new ListItem("CELL - 6", "6"));
            listItem.Add(new ListItem("CELL - 7", "7"));
            listItem.Add(new ListItem("CELL - 8", "8"));
            listItem.Add(new ListItem("CELL - 9", "9"));
            CmbDesc.Items.AddRange(listItem.ToArray());

            this.CmbCode.SelectedIndexChanged -= new System.EventHandler(this.CmbCode_SelectedIndexChanged);
            this.CmbSortBy.SelectedIndexChanged -= new System.EventHandler(this.CmbSortBy_SelectedIndexChanged);
            this.CmbDesc.SelectedIndexChanged -= new System.EventHandler(this.CmbDesc_SelectedIndexChanged);


            CmbSortBy.SelectedIndex = 0;
            CmbCode.SelectedIndex = 0;
            CmbDesc.SelectedIndex = 0;

            this.CmbCode.SelectedIndexChanged += new System.EventHandler(this.CmbCode_SelectedIndexChanged);
            this.CmbSortBy.SelectedIndexChanged += new System.EventHandler(this.CmbSortBy_SelectedIndexChanged);
            this.CmbDesc.SelectedIndexChanged += new System.EventHandler(this.CmbDesc_SelectedIndexChanged);
        }

        private void FillAgencyTableForm(string AgyType)
        {
            CaptainModel model = new CaptainModel();
            if (SelectAgyType != null)
            {
                List<AgyTabEntity> AgyList = model.Agytab.GetAgyTab(AgyType);
                if (AgyList.Count > 0)
                {
                    AgyTabEntity AgyProfile = AgyList[0];
                    TxtCode.Text = AgyProfile.agytype;
                    TxtDesc.Text = AgyProfile.agydesc.Trim();

                    SetComboBoxValue(CmbSortBy, AgyProfile.agy1.Trim());
                    SetComboBoxValue(CmbCode, AgyProfile.agyactive.Trim());
                    SetComboBoxValue(CmbDesc, AgyProfile.agydefault.Trim());

                    SetReqStartoRelatedCells(null, AgyProfile.agy1.Trim());
                    SetReqStartoRelatedCells(null, AgyProfile.agyactive.Trim());
                    SetReqStartoRelatedCells(null, AgyProfile.agydefault.Trim());
                    //FillHierarchyGrid(AgyProfile.agyhierarchy.Trim());
                    FillApplications(AgyProfile.agy8);
                    string Temp = null;
                    Temp = AgyProfile.agy9;

                    if (Temp.Substring(0, 1) == '1'.ToString())
                        CbCell1.Checked = true;
                    else
                        CbCell1.Checked = false;
                    if (Temp.Substring(1, 1) == '1'.ToString())
                        CbCell2.Checked = true;
                    else
                        CbCell2.Checked = false;

                    if (Temp.Substring(2, 1) == '1'.ToString())
                        CbCell3.Checked = true;
                    else
                        CbCell3.Checked = false;
                    if (Temp.Substring(3, 1) == '1'.ToString())
                        CbCell4.Checked = true;
                    else
                        CbCell4.Checked = false;
                    if (Temp.Substring(4, 1) == '1'.ToString())
                        CbCell5.Checked = true;
                    else
                        CbCell5.Checked = false;
                    if (Temp.Substring(5, 1) == '1'.ToString())
                        CbCell6.Checked = true;
                    else
                        CbCell6.Checked = false;
                    if (Temp.Substring(6, 1) == '1'.ToString())
                        CbCell7.Checked = true;
                    else
                        CbCell7.Checked = false;
                    if (Temp.Substring(7, 1) == '1'.ToString())
                        CbCell8.Checked = true;
                    else
                        CbCell8.Checked = false;
                    if (Temp.Substring(8, 1) == '1'.ToString())
                        CbCell9.Checked = true;
                    else
                        CbCell9.Checked = false;

                    ToolTip tooltip = new ToolTip();
                    string toolTipText = "Added By     : " + AgyProfile.agyaddoperator.Trim() + " on " + AgyProfile.agyadddate.ToString() + "\n" +
                                         "Modified By  : " + AgyProfile.agylstcoperator.Trim() + " on " + AgyProfile.agylstcdate.ToString();
                    // tooltip.SetToolTip(PbInfo, toolTipText);

                    //if (Temp.Substring(9, 1) == '1'.ToString())
                    //    CbCellA1.Checked = true;
                    //else
                    //    CbCellA1.Checked = false;
                    //if (Temp.Substring(10, 1) == '1'.ToString())
                    //    CbCellA2.Checked = true;
                    //else
                    //    CbCellA2.Checked = false;
                    //if (Temp.Substring(11, 1) == '1'.ToString())
                    //    CbCellA3.Checked = true;
                    //else
                    //    CbCellA3.Checked = false;
                    //if (Temp.Substring(12, 1) == '1'.ToString())
                    //    CbCellA4.Checked = true;
                    //else
                    //    CbCellA4.Checked = false;

                    //TxtName.Text = AgyProfile.agy8;
                    //TxtName.Text = AgyProfile.agy9;
                    //TxtName.Text = AgyProfile.agya1;
                    //TxtName.Text = AgyProfile.agya2;
                    //TxtName.Text = AgyProfile.agya3;
                    //TxtName.Text = AgyProfile.agya4;
                    //TxtName.Text = AgyProfile.agydesc;
                    //TxtName.Text = AgyProfile.agyactive;
                    //TxtName.Text = AgyProfile.agydefault;
                    //TxtName.Text = AgyProfile.agytype;




                    //if (AgycntlProfile.EditZip == "1")
                    //    CbEditZip.Checked = true;
                    //if (AgycntlProfile.ClearCaprep == "1")
                    //    CbClear.Checked = true;
                    //if (AgycntlProfile.SearchDataBase == "1")
                    //    RbDatabase.Checked = true;
                    //else
                    //    RbDropdown.Checked = true;
                }
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void FillHierarchyGrid(string AgyHierarchy)
        {
            if (AgyHierarchy.Length > 0)
            {
                int NextIndex = 0;
                {
                    NextIndex = AgyHierarchy.Length / 6;
                    string[] HierarchyArray = new string[NextIndex];
                    NextIndex = 0;
                    int sub = 0;
                    gvwApplication.Rows.Clear();

                    if (AgyHierarchy.Length == 6 && AgyHierarchy.Substring(0, 6) == "******")
                    {
                        int rowIndex = 0;
                        rowIndex = gvwApplication.Rows.Add("**-**-**", "All Hierarchies");
                        gvwApplication.Rows[rowIndex].Tag = 1;
                    }
                    else
                    {
                        string Tmp = null;
                        for (int count = 0; AgyHierarchy.Length > count;)
                        {
                            HierarchyArray[sub] = AgyHierarchy.Substring(NextIndex, 6);
                            Tmp = HierarchyArray[sub];
                            count = count + 6;
                            NextIndex = count;
                            FillHierarchyDescription(Tmp, sub);
                            sub++;
                        }
                    }
                }
            }
        }

        private void FillHierarchyDescription(string str, int Row)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(str.Substring(0, 2), str.Substring(2, 2), str.Substring(4, 2));
            string Hiename = (ds.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
            int rowIndex = 0;
            if (Hiename != null)
            {
                //rowIndex = gvwHierarchy.Rows.Add(str, Hiename);
                rowIndex = gvwApplication.Rows.Add(str.Substring(0, 2) + '-' + str.Substring(2, 2) + '-' + str.Substring(4, 2), Hiename);
                gvwApplication.Rows[rowIndex].Tag = (Row + 1);
            }
            switch (Row)
            {
                case 1: this.gvwApplication.Size = new System.Drawing.Size(560, 42); break;
                case 2: this.gvwApplication.Size = new System.Drawing.Size(560, 63); break;
                case 3: this.gvwApplication.Size = new System.Drawing.Size(560, 84); break;
                case 4: this.gvwApplication.Size = new System.Drawing.Size(560, 105); break;
                case 5: this.gvwApplication.Size = new System.Drawing.Size(560, 126); break;
                case 6: this.gvwApplication.Size = new System.Drawing.Size(560, 147); break;
                default: this.gvwApplication.Size = new System.Drawing.Size(560, 168); break;
            }
        }

        private void FillApplications(string appCode)
        {
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string appSelect = "false";
                string code = dr["APPL_CODE"].ToString();
                //if (appCode.Contains(code))
                //    appSelect = "true";
                for (int i = 0; i < appCode.Length;)
                {
                    if (appCode.Substring(i, 2) == code)
                    {
                        appSelect = "true";
                        break;
                    }
                    i += 2;
                }


                int rowIndex = gvwApplication.Rows.Add(appSelect, code, dr["APPL_DESCRIPTION"].ToString());
                gvwApplication.Rows[rowIndex].Tag = dr;
            }
        }

        int chkboxCheckedCnt()
        {

            int x = 0;

            if (CbCell1.Checked)
                x++;
            if (CbCell2.Checked)
                x++;
            if (CbCell3.Checked)
                x++;
            if (CbCell4.Checked)
                x++;
            if (CbCell5.Checked)
                x++;
            if (CbCell6.Checked)
                x++;
            if (CbCell7.Checked)
                x++;
            if (CbCell8.Checked)
                x++;
            if (CbCell9.Checked)
                x++;
            return x;
        }

        private bool ValidateForm()
        {
            int chkbCheckedcnt = chkboxCheckedCnt();
            bool isValid = true;

            if (Mode.Equals("Add") && (String.IsNullOrEmpty(TxtCode.Text)))
            {
                _errorProvider.SetError(TxtCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTC.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(TxtCode, null);
            }

            if ((!((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0")) &&
                 (!((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0")))
            {
                if (((ListItem)CmbCode.SelectedItem).Value.ToString() ==
                   ((ListItem)CmbDesc.SelectedItem).Value.ToString())
                {
                    _errorProvider.SetError(CmbCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(CmbCode, null);
                }
            }

            //kranthi 01/09/2023:: If there are any 2 or more Cell checkboxes are checked we are forcely giving validation to select Code and Description combobox
            if (chkbCheckedcnt >= 2)
            {
                if (((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0"))
                {
                    _errorProvider.SetError(CmbCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbCode, null);
            }

            if (!((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0"))
            {
                int Tmp = int.Parse(((ListItem)CmbCode.SelectedItem).Value.ToString());
                string TmpControl = ValidateReduiredCell(Tmp);

                if (!(String.IsNullOrEmpty(TmpControl)))
                {
                    _errorProvider.SetError(CmbCode, string.Format("Please Check " + TmpControl, label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(CmbCode, null);
                }
            }

            //kranthi 01/09/2023:: If there are any 2 or more Cell checkboxes are checked we are forcely giving validation to select Code and Description combobox
            if (chkbCheckedcnt >= 2)
            {
                if (((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0"))
                {
                    _errorProvider.SetError(CmbDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbDesc, null);
            }

            if (!((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0"))
            {
                int Tmp = int.Parse(((ListItem)CmbDesc.SelectedItem).Value.ToString());
                string TmpControl = ValidateReduiredCell(Tmp);

                if (!(String.IsNullOrEmpty(TmpControl)))
                {
                    _errorProvider.SetError(CmbDesc, string.Format("Please Check " + TmpControl, label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(CmbDesc, null);
                }
            }


            if (!((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0") &&
                !((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0"))
            {
                if (((ListItem)CmbCode.SelectedItem).Value.ToString() == ((ListItem)CmbDesc.SelectedItem).Value.ToString())
                {

                    string Tmpstr = ((ListItem)CmbDesc.SelectedItem).Value.ToString();
                    Tmpstr = "You can not select 'Cell - " + Tmpstr + "'" + "\n" + "in both 'Code' and 'Description' Combos";
                    _errorProvider.SetError(CmbDesc, string.Format(Tmpstr, label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(CmbSortBy, null);
                }

            }

            //if (((ListItem)CmbSortBy.SelectedItem).Value.ToString().Equals("0"))
            //{
            //    _errorProvider.SetError(CmbSortBy, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //    _errorProvider.SetError(CmbSortBy, null);


            if (!((ListItem)CmbSortBy.SelectedItem).Value.ToString().Equals("0"))
            {
                int Tmp = int.Parse(((ListItem)CmbSortBy.SelectedItem).Value.ToString());
                string TmpControl = ValidateReduiredCell(Tmp);

                if (!(String.IsNullOrEmpty(TmpControl)))
                {
                    _errorProvider.SetError(CmbSortBy, string.Format("Please Check " + TmpControl, label2.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(CmbSortBy, null);
                }
            }


            //if (!((ListItem)CmbSortBy.SelectedItem).Value.ToString().Equals("0"))
            //{
            //    if (((ListItem)CmbCode.SelectedItem).Value.ToString() != ((ListItem)CmbSortBy.SelectedItem).Value.ToString() &&
            //       ((ListItem)CmbDesc.SelectedItem).Value.ToString() != ((ListItem)CmbSortBy.SelectedItem).Value.ToString())
            //    {
            //        string Tmpstr = "You can select either ";
            //        if (!((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0"))
            //            Tmpstr = Tmpstr + "'Cell - " + ((ListItem)CmbCode.SelectedItem).Value.ToString() + "'";
            //        if (!((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0"))
            //            Tmpstr = Tmpstr + "'Cell - " + ((ListItem)CmbDesc.SelectedItem).Value.ToString() + "'";
            //        _errorProvider.SetError(CmbSortBy, string.Format(Tmpstr, label2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(CmbSortBy, null);
            //    }
            //}

            if (String.IsNullOrEmpty(TxtDesc.Text.Trim()))
            {
                _errorProvider.SetError(TxtDesc, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDesc.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(TxtDesc, null);
            }

            if (chkbCheckedcnt == 0)
            {
                isValid = false;
                AlertBox.Show("Please check atleast two cells", MessageBoxIcon.Warning);
            }


            IsSaveValid = isValid;
            return (isValid);
        }

        private string ValidateReduiredCell(int CellSubscript)
        {
            string TmpControl = null;
            switch (CellSubscript)
            {
                case 1:
                    if (!(CbCell1.Checked))
                        TmpControl = "Cell 1"; break;
                case 2:
                    if (!(CbCell2.Checked))
                        TmpControl = "Cell 2"; break;
                case 3:
                    if (!(CbCell3.Checked))
                        TmpControl = "Cell 3"; break;
                case 4:
                    if (!(CbCell4.Checked))
                        TmpControl = "Cell 4"; break;
                case 5:
                    if (!(CbCell5.Checked))
                        TmpControl = "Cell 5"; break;
                case 6:
                    if (!(CbCell6.Checked))
                        TmpControl = "Cell 6"; break;
                case 7:
                    if (!(CbCell7.Checked))
                        TmpControl = "Cell 7"; break;
                case 8:
                    if (!(CbCell8.Checked))
                        TmpControl = "Cell 8"; break;
                case 9:
                    if (!(CbCell9.Checked))
                        TmpControl = "Cell 9"; break;
            }
            return (TmpControl);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {

            if (ValidateForm())
            {
                CaptainModel model = new CaptainModel();
                AgyTabEntity AgyDetails = new AgyTabEntity();

                string Tmp = null;
                char[] TmpChar = new char[13];
                TmpChar[0] = TmpChar[1] = TmpChar[2] = TmpChar[3] = TmpChar[4] = '0';
                TmpChar[5] = TmpChar[6] = TmpChar[7] = TmpChar[8] = TmpChar[9] = '0';
                TmpChar[10] = TmpChar[11] = TmpChar[12] = '0';
                AgyDetails.agy2 = AgyDetails.agy3 = AgyDetails.agy4 = null;
                AgyDetails.agy5 = AgyDetails.agy6 = AgyDetails.agy8 = null;
                AgyDetails.agy1 = AgyDetails.agy9 = AgyDetails.agy7 = null;

                AgyDetails.agyactive = AgyDetails.agydefault = AgyDetails.agy1 = null;

                AgyDetails.agya1 = AgyDetails.agya2 = "0.00000";
                AgyDetails.agya3 = AgyDetails.agya4 = "0.00000";

                if (Mode.Equals("Add"))
                {
                    AgyDetails.agytype = TxtCode.Text;
                }

                if (CbCell1.Checked)
                {
                    TmpChar[0] = '1';
                }
                if (CbCell2.Checked)
                {
                    TmpChar[1] = '1';
                }
                if (CbCell3.Checked)
                {
                    TmpChar[2] = '1';
                }
                if (CbCell4.Checked)
                {
                    TmpChar[3] = '1';
                }
                if (CbCell5.Checked)
                {
                    TmpChar[4] = '1';
                }
                if (CbCell6.Checked)
                {
                    TmpChar[5] = '1';
                }
                if (CbCell7.Checked)
                {
                    TmpChar[6] = '1';
                }
                if (CbCell8.Checked)
                {
                    TmpChar[7] = '1';
                }
                if (CbCell9.Checked)
                {
                    TmpChar[8] = '1';
                }
                foreach (char c in TmpChar)
                {
                    Tmp = Tmp + c.ToString();
                }

                AgyDetails.agy9 = Tmp;

                AgyDetails.agydesc = TxtDesc.Text;

                if (!((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0"))
                {
                    AgyDetails.agyactive = ((ListItem)CmbCode.SelectedItem).Value.ToString();
                }
                if (!((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0"))
                {
                    AgyDetails.agydefault = ((ListItem)CmbDesc.SelectedItem).Value.ToString();
                }
                if (!((ListItem)CmbSortBy.SelectedItem).Value.ToString().Equals("0"))
                {
                    AgyDetails.agy1 = ((ListItem)CmbSortBy.SelectedItem).Value.ToString();
                }

                AgyDetails.agy8 = checkgvwApplicationData();
                AgyDetails.agyhierarchy = "******";
                AgyDetails.agycode = "00000";
                AgyDetails.agyaddoperator = AgyDetails.agylstcoperator = BaseForm.UserID;

                if (Mode.Equals("Edit"))
                {
                    AgyDetails.agytype = SelectAgyType;
                }
                else
                {
                    AgyDetails.agytype = TxtCode.Text;
                }

                if (model.Agytab.UpdateAGYTAB(AgyDetails))
                {
                    AgencyTableControl agencyTableControl = BaseForm.GetBaseUserControl() as AgencyTableControl;
                    if (agencyTableControl != null)
                    {
                        BaseForm.BaseAgyTabsEntity = model.lookupDataAccess.GetAgyTabs(string.Empty, string.Empty, string.Empty);
                        agencyTableControl.Refresh(AgyDetails.agytype, Mode);
                    }
                    if (Mode == "Add") AlertBox.Show("Record Inserted Successfully"); else AlertBox.Show("Record Updated Successfully");
                    this.Close();
                }
            }
        }

        public string checkgvwApplicationData()
        {
            string strdata = string.Empty;

            foreach (DataGridViewRow row in gvwApplication.Rows)
            {
                if (row.Cells["cbSelect"].Value != null && Convert.ToBoolean(row.Cells["cbSelect"].Value) == true)
                {
                    strdata = strdata + row.Cells["Code"].Value.ToString();
                }
            }

            return strdata;
        }

        public string GetSelectedHierarchy()
        {
            string SelHierarchy = null;
            if (gvwApplication != null)
            {
                int R_count = gvwApplication.RowCount;

                foreach (DataGridViewRow dr in gvwApplication.SelectedRows)

                    foreach (DataGridViewRow row in gvwApplication.Rows)
                    {
                        SelHierarchy = row.Cells[1].ToString();
                        //rowIndex = row.Index;
                        break;
                    }
            }

            //foreach (DataGridViewRow row in gvwHierarchy.Rows)
            //{
            //    foreach (DataGridViewCell cell in row.Cells)
            //    {
            //        if (cell.Value != null)
            //        {
            //            output += cell.Value.ToString() + " ";
            //        }
            //    }
            //}


            //DataGridViewRow row = gvwHierarchy.Rows.Cast<DataGridViewRow>();


            //    DataGridViewRow dr = gvwHierarchy.Rows;
            //for (int i = 0; i < R_count; i++)
            //{
            //    string s = null;
            //    //if (dr.Selected)
            //    //{
            //    DataRow srow = dr.Tag as DataRow;
            //    SelHierarchy = srow["Hierarchy"].ToString();
            //    s = SelHierarchy;
            //    //break;
            //    //}

            //}
            //    foreach (DataGridViewRow dr in gvwHierarchy.Rows)
            //    {
            //        string s = null;
            //        //if (dr.Selected)
            //        //{
            //        DataRow srow = dr.Tag as DataRow;
            //        SelHierarchy = srow["Hierarchy"].ToString();
            //        s = SelHierarchy;
            //        //break;
            //        //}
            //    }
            return SelHierarchy;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnHierarchy_Click(object sender, EventArgs e)
        {
            try
            {
                HierarchieSelectionFormNew hsForm = new HierarchieSelectionFormNew(BaseForm, "", "Service");
                hsForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                hsForm.StartPosition = FormStartPosition.CenterScreen;
                hsForm.ShowDialog();

                //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Service", "I", "*", "R");
                //hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
                //hierarchieSelectionForm.ShowDialog();

            }
            catch (Exception ex)
            {
                //
            }
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            //TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedItem.Tag as TagClass;


            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    int rowIndex = 0;
                    gvwApplication.Rows.Add(row.Code, row.HirarchyName);
                    gvwApplication.Rows[rowIndex].Tag = row;

                }
                //RefreshGrid();
            }
        }

        bool AgyCode_Already_Exists = false;
        private void TxtCode_LostFocus(object sender, EventArgs e)
        {
            if (Mode == "Add")
            {
                int Next_code_len = TxtCode.Text.Trim().Length;
                string TmpCode = null;
                TmpCode = TxtCode.Text.ToString().Trim();
                switch (Next_code_len)
                {
                    case 4: TmpCode = "0" + TmpCode; break;
                    case 3: TmpCode = "00" + TmpCode; break;
                    case 2: TmpCode = "000" + TmpCode; break;
                    case 1: TmpCode = "0000" + TmpCode; break;
                        //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                        //    break;
                }

                if (Next_code_len > 0)
                {
                    TxtCode.Text = TmpCode;
                    CaptainModel model = new CaptainModel();
                    if (TmpCode != null)
                    {
                        List<AgyTabEntity> AgyList = model.Agytab.GetAgyTab(TmpCode);
                        if (AgyList.Count > 0)
                        {
                            AlertBox.Show("Please try with another Code" + "\n" + "Table already Exists with given 'Code : " + TmpCode + "'", MessageBoxIcon.Warning);
                            TxtCode.Text = ""; AgyCode_Already_Exists = true;
                            TxtCode.Focus();

                        }
                        _errorProvider.SetError(TxtCode, null);
                    }
                }
            }
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "NewTable");
        }

        string PrivCodeCell = null;
        private void CmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TmpPrivCell = PrivCodeCell;
            if (TmpPrivCell == ((ListItem)CmbDesc.SelectedItem).Value.ToString() ||
                TmpPrivCell == ((ListItem)CmbSortBy.SelectedItem).Value.ToString())
                TmpPrivCell = null;

            //if (!((ListItem)CmbCode.SelectedItem).Value.ToString().Equals("0"))
            SetReqStartoRelatedCells(TmpPrivCell, ((ListItem)CmbCode.SelectedItem).Value.ToString());

            PrivCodeCell = ((ListItem)CmbCode.SelectedItem).Value.ToString();
        }

        private void SetReqStartoRelatedCells(string PrivCell, string Cell)
        {
            switch (PrivCell)
            {
                case "1": Star1.Visible = false; break;
                case "2": Star2.Visible = false; break;
                case "3": Star3.Visible = false; break;
                case "4": Star4.Visible = false; break;
                case "5": Star5.Visible = false; break;
                case "6": Star6.Visible = false; break;
                case "7": Star7.Visible = false; break;
                case "8": Star8.Visible = false; break;
                case "9": Star9.Visible = false; break;
            }

            switch (Cell)
            {
                case "1": Star1.Visible = true; break;
                case "2": Star2.Visible = true; break;
                case "3": Star3.Visible = true; break;
                case "4": Star4.Visible = true; break;
                case "5": Star5.Visible = true; break;
                case "6": Star6.Visible = true; break;
                case "7": Star7.Visible = true; break;
                case "8": Star8.Visible = true; break;
                case "9": Star9.Visible = true; break;
            }
        }

        string PrivDescCell = null;
        private void CmbDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TmpPrivCell = PrivDescCell;
            if (TmpPrivCell == ((ListItem)CmbCode.SelectedItem).Value.ToString() ||
                TmpPrivCell == ((ListItem)CmbSortBy.SelectedItem).Value.ToString())
                TmpPrivCell = null;


            // if (!((ListItem)CmbDesc.SelectedItem).Value.ToString().Equals("0"))
            SetReqStartoRelatedCells(TmpPrivCell, ((ListItem)CmbDesc.SelectedItem).Value.ToString());

            PrivDescCell = ((ListItem)CmbDesc.SelectedItem).Value.ToString();

        }

        string PrivOrderCell = null;
        private void CmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TmpPrivCell = PrivOrderCell;
            if (TmpPrivCell == ((ListItem)CmbDesc.SelectedItem).Value.ToString() ||
                TmpPrivCell == ((ListItem)CmbCode.SelectedItem).Value.ToString())
                TmpPrivCell = null;

            //if (!((ListItem)CmbSortBy.SelectedItem).Value.ToString().Equals("0"))
            SetReqStartoRelatedCells(TmpPrivCell, ((ListItem)CmbSortBy.SelectedItem).Value.ToString());

            PrivOrderCell = ((ListItem)CmbSortBy.SelectedItem).Value.ToString();

        }

        private void TxtDesc_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtDesc.Text))
                _errorProvider.SetError(TxtDesc, null);
        }

        private void TxtDesc_GotFocus(object sender, EventArgs e)
        {
            //if (AgyCode_Already_Exists)
            //{
            //    TxtCode.Focus();
            //    //AgyCode_Already_Exists = false;
            //}
        }

        private void AgencyTableCreateForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_HELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps("ADMN0006", 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }
    }
}