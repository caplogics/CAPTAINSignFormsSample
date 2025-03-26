#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SelStaffPosCodesForm : Form
    {
        private CaptainModel _model = null;
        public SelStaffPosCodesForm(BaseForm baseForm, PrivilegeEntity privilegesEntity, string mode, string strPositionCode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            PropPositionCode = string.Empty;
            this.Text = /*privilegesEntity.PrivilegeName +*/ "Select Position Codes";
            FillGrid();
            applicationCheckEdit(SplitByAppication(strPositionCode, 3));
            if (mode.Equals("View"))
            {
                panel1.Enabled = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
        }

        public string PropPositionCode { get; set; }

        private void FillGrid()
        {
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetPositionCode();
            //commonEntity = filterByHIE(commonEntity);     
            string strPostion;
            foreach (CommonEntity PositionCode in commonEntity)
            {

                if (PositionCode.Code.Trim().Length == 1)
                    strPostion = PositionCode.Code.Trim() + " ";
                else
                    strPostion = PositionCode.Code;

                gvPosCode.Rows.Add(false, strPostion, PositionCode.Desc, strPostion + " ");
            }
        }
        public void applicationCheckEdit(string[] strApplication)
        {
            foreach (DataGridViewRow row in gvPosCode.Rows)
            {
                if (row.Cells["PosCode"].Value != null)
                {
                    for (int i = 0; i < strApplication.Length; i++)
                    {
                        string strSaveCode = strApplication[i].ToString();
                        if (Convert.ToString(row.Cells["PosCode"].Value) == strSaveCode.Substring(0, 2))
                        {
                            row.Cells["chkAppcode"].Value = true;
                            row.Cells["SaveCode"].Value = strSaveCode;
                            if (strSaveCode.EndsWith("P"))
                                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                        }
                    }

                }
            }
        }

        public string checkgvwApplicationData()
        {
            string strdata = string.Empty;

            foreach (DataGridViewRow row in gvPosCode.Rows)
            {
                if (row.Cells["chkAppcode"].Value != null && Convert.ToBoolean(row.Cells["chkAppcode"].Value) == true)
                {
                    strdata = strdata + row.Cells["SaveCode"].Value.ToString();
                }
            }

            return strdata;
        }

        private string[] SplitByAppication(string s, int split)
        {
            //Like using List because I can just add to it 
            List<string> list = new List<string>();
            // Integer Division
            int TimesThroughTheLoop = s.Length / split;
            for (int i = 0; i < TimesThroughTheLoop; i++)
            {
                list.Add(s.Substring(i * split, split));
            }
            // Pickup the end of the string
            if (TimesThroughTheLoop * split != s.Length)
            {
                list.Add(s.Substring(TimesThroughTheLoop * split));
            }
            return list.ToArray();
        }

        private void gvPosCode_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            foreach (DataGridViewRow item in gvPosCode.Rows)
            {
                if (item.Cells["PosCode"].Value.ToString().Trim().Length == 1)
                    item.Cells["SaveCode"].Value = item.Cells["PosCode"].Value.ToString().Trim() + "  ";
                else
                    item.Cells["SaveCode"].Value = item.Cells["PosCode"].Value.ToString().Trim() + " ";
                item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }

            gvPosCode.SelectedRows[0].Cells["SaveCode"].Value = gvPosCode.SelectedRows[0].Cells["PosCode"].Value + "P";
            gvPosCode.SelectedRows[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
            gvPosCode.Update();
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if (gvPosCode.Rows.Count > 0)
            {
                contextMenu1.MenuItems.Clear();
                MenuItem menuLst = new MenuItem();
                menuLst.Text = "Primary";
                menuLst.Tag = "P";
                contextMenu1.MenuItems.Add(menuLst);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool boolPrimary = true;
            List<DataGridViewRow> SelectedgvRows = (from c in gvPosCode.Rows.Cast<DataGridViewRow>().ToList()
                                                    where (c.Cells["chkAppcode"].Value.ToString().ToUpper().Equals("TRUE"))
                                                    select c).ToList();
            if (SelectedgvRows.Count <= 5)
            {
                if (SelectedgvRows.Count == 1)
                {
                    foreach (DataGridViewRow item in SelectedgvRows)
                    {

                        if (item.Cells["PosCode"].Value.ToString().Trim().Length == 1)
                            item.Cells["SaveCode"].Value = item.Cells["PosCode"].Value.ToString().Trim() + " P";
                        else
                            item.Cells["SaveCode"].Value = item.Cells["PosCode"].Value.ToString().Trim() + "P";
                    }
                }
                else
                {
                    if (SelectedgvRows.Count > 1)
                    {
                        boolPrimary = false;
                        foreach (DataGridViewRow item in SelectedgvRows)
                        {
                            if (item.Cells["chkAppcode"].Value != null && Convert.ToBoolean(item.Cells["chkAppcode"].Value) == true)
                            {
                                if (item.Cells["SaveCode"].Value.ToString().EndsWith("P"))
                                {
                                    boolPrimary = true;
                                    break;
                                }
                            }
                        }
                        foreach (DataGridViewRow item in gvPosCode.Rows)
                        {
                            if (Convert.ToBoolean(item.Cells["chkAppcode"].Value) == false)
                            {
                                item.Cells["SaveCode"].Value = item.Cells["PosCode"].Value.ToString().Trim() + " ";
                                item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                    }
                }
                if (boolPrimary)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    gvPosCode.Update();
                    CommonFunctions.MessageBoxDisplay("Please Select The Primary Position");
                }
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("You Can't Add More Than 5 Position Codes");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}