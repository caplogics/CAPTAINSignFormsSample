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
//using Gizmox.WebGUI.Common.Resources;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Drawing;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB2012_AdhocPageSetup : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public CASB2012_AdhocPageSetup(double tot_SelCol_Width, int tot_Sel_Columns, string callinf_form)
        {
            InitializeComponent();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Tot_SelCol_Width = tot_SelCol_Width;
            Tot_Sel_Columns = tot_Sel_Columns;
            Callinf_Form = callinf_form;

            Lbl_SelCol_Cnt.Text = tot_Sel_Columns.ToString();
            Lbl_SelCol_Width.Text = tot_SelCol_Width.ToString() + " in";

            this.Text = "Page Setup";

            if (tot_SelCol_Width < 8.27)
                Lbl_Rep_Type.Text = "A4 Portrait";
            else if(tot_SelCol_Width > 8.27 && tot_SelCol_Width < 11)
                    Lbl_Rep_Type.Text = "A4 Landscape";
            else
                Lbl_Rep_Type.Text = "Custom Setup";

            switch (Lbl_Rep_Type.Text)
            {
                case "A4 Portrait": Rb_A4_Port.Checked = true; break;
                case "A4 Landscape": Rb_A4_Land.Checked = true; break;
                case "Custom Setup": Rb_Custom.Checked = true; break;
            }


            //switch (Callinf_Form)
            //{
            //    case "CASB0012": Txt_Header_Title.Text = "Adhoc Report"; break;
            //    case "CASB0004": Txt_Header_Title.Text = "Demographics/Performance Measures Report"; break;
            //    default: Txt_Header_Title.Text = "Test Report"; break;
            //}
        }


       #region properties

        public BaseForm BaseForm { get; set; }

        public double Tot_SelCol_Width { get; set; }

        public int Tot_Sel_Columns { get; set; }

        public string Callinf_Form { get; set; }

        #endregion

        


        private void Cb_Header_CheckedChanged(object sender, EventArgs e)
        {
            if (Cb_Header.Checked)
                Cb_Inc_Title.Enabled = Cb_Inc_Image.Enabled = true;
            else
                Cb_Inc_Title.Checked = Cb_Inc_Image.Checked = Cb_Inc_Title.Enabled = Cb_Inc_Image.Enabled = false;
        }

        private void Cb_Footer_CheckedChanged(object sender, EventArgs e)
        {
            if (Cb_Footer.Checked)
                Cb_Inc_PageCount.Enabled = true;
            else
                Cb_Inc_PageCount.Checked = Cb_Inc_PageCount.Enabled = false;
        }

        private void Cb_Inc_Title_CheckedChanged(object sender, EventArgs e)
        {
            if (Cb_Inc_Title.Checked)
                Txt_Header_Title.Enabled = true;
            else
            {
                Txt_Header_Title.Clear();
                Txt_Header_Title.Enabled = false;
            }
        }

        public bool[] Get_Checkbox_Status()
        {
            bool[] All_checkbox_Status = new bool[6];
            All_checkbox_Status[0] = All_checkbox_Status[1] = All_checkbox_Status[2] = 
            All_checkbox_Status[3] = All_checkbox_Status[4] = All_checkbox_Status[5] = false;

            if (Cb_Header.Checked)
                All_checkbox_Status[0] = true;

            if (Cb_Inc_Title.Checked)
                All_checkbox_Status[1] = true;

            if (Cb_Inc_Image.Checked)
                All_checkbox_Status[2] = true;

            if (Cb_Footer.Checked)
                All_checkbox_Status[3] = true;

            if (Cb_Inc_PageCount.Checked)
                All_checkbox_Status[4] = true;

            if (Cb_Save_Criteria.Checked)
                All_checkbox_Status[5] = true;

            return All_checkbox_Status;
        }

        public string Get_Header_Title()
        {
            string Header_Title = string.Empty;

            if (!string.IsNullOrEmpty(Txt_Header_Title.Text.Trim()))
                Header_Title = Txt_Header_Title.Text.Trim();

            return Header_Title;
        }

        public string Get_Report_Name()
        {
            string Report_name = " ";
            Report_name = Txt_Report_Name.Text.Trim();
            if (Report_name.Contains(".rdlc"))
            {
                string[] Name = Regex.Split(Report_name, ".rdlc");
                Report_name = Name[0];
            }

            return Report_name;
        }


        public string Get_Page_Orientation()
        {
            string Page_Orientation = "A4 Portrait";
            if(Rb_A4_Land.Checked)
                Page_Orientation = "A4 Landscape";
            else
                if(Rb_Custom.Checked)
                    Page_Orientation = "Custom";

            return Page_Orientation;
        }

        
        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateForm()
        {
            bool IsValid = true;
            Regex RgxUrl = new Regex("[^a-z0-9],_,-, ");

            //if (Cb_Inc_Title.Checked)
            //{
            //    if (string.IsNullOrEmpty(Txt_Header_Title.Text.Trim())) //((ListItem)CmbSP.SelectedItem).Value.ToString())
            //    {
            //        _errorProvider.SetError(Txt_Header_Title, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label9.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        IsValid = false;
            //    }
            //    else
            //    {
            //        if (RgxUrl.IsMatch(Txt_Header_Title.Text.Trim()))
            //        {
            //            _errorProvider.SetError(Txt_Header_Title, string.Format("Remove Special Characters"));
            //            IsValid = false;
            //        }
            //        else
            //            _errorProvider.SetError(Txt_Header_Title, null);
            //    }
            //}no


            char[] SpecialChars = "!@#$%^&*()-+={}[]|:;'<>,.?/".ToCharArray();

            int indexOf = -1;
            if (string.IsNullOrEmpty(Txt_Report_Name.Text.Trim())) //((ListItem)CmbSP.SelectedItem).Value.ToString())
            {
                _errorProvider.SetError(Txt_Report_Name, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                IsValid = false;
            }
            else
            {
                //if (RgxUrl.IsMatch(Txt_Report_Name.Text.Trim()))
                //{
                //    _errorProvider.SetError(Txt_Report_Name, string.Format("Remove Special Characters"));
                //    IsValid = false;
                //}
                //else
                //    _errorProvider.SetError(Txt_Report_Name, null);

                indexOf = Txt_Report_Name.Text.IndexOfAny(SpecialChars);
                if (indexOf != -1)
                {
                    _errorProvider.SetError(Txt_Report_Name, string.Format("Remove Special Characters"));
                    IsValid = false;
                }
                else
                    _errorProvider.SetError(Txt_Report_Name, null);

            }
            return IsValid;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Txt_Rep_Name_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_Header_Title.Text.Trim()))
                _errorProvider.SetError(Txt_Header_Title, null);
        }







 
    }
}