#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using System.IO;
using DevExpress.Xpo.Logger;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class LiheapPerfQuestions : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public LiheapPerfQuestions(BaseForm baseForm, CaseMstEntity caseMST, string mode, PrivilegeEntity privilegeEntity, string strFilter)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = privilegeEntity.PrivilegeName + " - " + mode;
            BaseForm = baseForm;
            propCaseMst = caseMST;
            strFilterques = strFilter;
            if (strFilter == "02" || strFilter == "04")
            {
                propQuesType = "N";
            }
            else
            {
                propQuesType = "Y";
            }
           
            //propCaseMst = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, caseMST.ApplNo);

            FillLihpQues(propCaseMst, strFilterques, BaseForm.BaseYear);

            if (mode.Equals(Consts.Common.View))
            {
                btnCancel.Text = "Close";
                btnSave.Visible = false;
                gvwHouseingQues.Enabled = false;

            }
        }
       
        public BaseForm BaseForm { get; set; }
        public CaseMstEntity propCaseMst { get; set; }
        public string strFilterques { get; set; }
        public string propQuesType { get; set; }

        private void gvwHouseingQues_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is string)
            {
                string responseValue = gvwHouseingQues.SelectedRows[0].Cells[2].Value.ToString();
                string responseCode = gvwHouseingQues.SelectedRows[0].Cells[2].Tag == null ? string.Empty : gvwHouseingQues.SelectedRows[0].Cells[2].Tag.ToString();


                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = objArgs.MenuItem.Tag.ToString();
                if (objArgs.MenuItem.Checked)
                {
                    responseValue = selectedValue;
                    responseCode = selectedCode;
                }
                else
                {

                    responseValue = selectedValue;
                    responseCode = selectedCode;

                }

                if (responseCode.Trim() == string.Empty)
                {
                    gvwHouseingQues.SelectedRows[0].Cells[2].Tag = string.Empty;
                    gvwHouseingQues.SelectedRows[0].Cells[2].Value = string.Empty;
                }
                else
                {
                    gvwHouseingQues.SelectedRows[0].Cells[2].Tag = responseCode;
                    gvwHouseingQues.SelectedRows[0].Cells[2].Value = responseValue;
                }
                //Commented by sudheer on 07/31/2020
                //if (BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020")
                if (BaseForm.BaseYear != "2015") //Added by Sudheer on 07/31/2020 
                {
                    if (propQuesType == "Y")
                    {
                        if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0002")
                        {
                            string strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0002")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0003")
                                {
                                    if (strdesccode == "N")
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = "U";
                                        item.Cells["gvtResponseDesc"].Value = "Not Applicable";
                                    }
                                    else
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                        item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    }
                                    if (strdesccode == "Y")
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }

                            }
                        }

                        if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0004" || gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0001")
                        {
                            string strdesccode = string.Empty;
                            string strFirstAnswer = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0001")
                                {
                                    strFirstAnswer = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0004")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0005")
                                {

                                    if (strdesccode == "N" && strFirstAnswer == "Y")
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                        item.Cells["gvtResponseDesc"].Value = string.Empty;
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = "U";
                                        item.Cells["gvtResponseDesc"].Value = "Not Applicable";
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        if (((strFirstAnswer == "Y" || strFirstAnswer == "N") && strdesccode == string.Empty) || ((strdesccode == "Y" || strdesccode == "N") && strFirstAnswer == string.Empty))
                                        {
                                            item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                            item.Cells["gvtResponseDesc"].Value = "";
                                        }
                                    }
                                }
                                else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0006" && gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0004")
                                {
                                    if (strdesccode == "Y" && (strFilterques == "01" || strFilterques == "03" || strFilterques == "07"))
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }
                            }
                        }
                        else if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0007" || gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0008" || gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0009")
                        {

                            string strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0007")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0008")
                                {
                                    if (strdesccode == "Y")
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0009")
                                {
                                    if (strdesccode == "N")
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }
                            }
                            if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0009")
                            {

                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0009")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0010")
                                    {
                                        if (strdesccode == "Y")
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                        }
                                        else
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        }
                                    }
                                }
                            }

                        }


                    }
                    else if (propQuesType == "N")
                    {
                        if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0002")
                        {
                            string strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0002")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0003")
                                {
                                    if (strdesccode == "N")
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = "U";
                                        item.Cells["gvtResponseDesc"].Value = "Not Applicable";
                                    }
                                    else
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                        item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    }
                                    if (strdesccode == "Y")
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }

                            }
                        }

                        if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0004" || gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0001")
                        {
                            string strdesccode = string.Empty;
                            string strFirstAnswer = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0001")
                                {
                                    strFirstAnswer = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0004")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0005")
                                {

                                    if (strdesccode == "N" && strFirstAnswer == "Y")
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                        item.Cells["gvtResponseDesc"].Value = string.Empty;
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.Cells["gvtResponseDesc"].Tag = "U";
                                        item.Cells["gvtResponseDesc"].Value = "Not Applicable";
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        if (((strFirstAnswer == "Y" || strFirstAnswer == "N") && strdesccode == string.Empty) || ((strdesccode == "Y" || strdesccode == "N") && strFirstAnswer == string.Empty))
                                        {
                                            item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                            item.Cells["gvtResponseDesc"].Value = "";
                                        }
                                    }
                                }

                            }
                        }
                        else if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0006" || gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0007" || gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0008")
                        {

                            string strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0006")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                }
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0007")
                                {
                                    if (strdesccode == "Y")
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0008")
                                {
                                    if (strdesccode == "N")
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }
                            }
                            if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0008")
                            {

                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0008")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0009")
                                    {
                                        if (strdesccode == "Y")
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                        }
                                        else
                                        {
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        }
                                    }
                                }
                            }

                        }

                    }


                    if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0012")
                    {

                        string strdesccode = string.Empty;
                        foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                        {
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0012")
                            {
                                strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                            }
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0013")
                            {
                                if (strdesccode == "Y")
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                }
                            }
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0014")
                            {
                                if (strdesccode == "Y")
                                {
                                    //item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                        }
                    }
                    if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0013")
                    {

                        string strdesccode = string.Empty;
                        foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                        {
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0013")
                            {
                                strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                            }
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0014")
                            {
                                if (strdesccode == "Y")
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                        }
                    }
                    if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0015")
                    {

                        string strdesccode = string.Empty;
                        foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                        {
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0015")
                            {
                                strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                            }
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0016")
                            {
                                if (strdesccode == "Y")
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                }
                            }
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0017")
                            {
                                if (strdesccode == "Y")
                                {
                                   // item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                        }
                    }
                    if (gvwHouseingQues.SelectedRows[0].Cells["gvtQuesCode"].Value.ToString().Trim() == "0016")
                    {

                        string strdesccode = string.Empty;
                        foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                        {
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0016")
                            {
                                strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                            }
                            if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0017")
                            {
                                if (strdesccode == "Y")
                                {
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    item.Cells["gvtResponseDesc"].Value = string.Empty;
                                    item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                    item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                        }
                    }
                }


            }
            else
            {
                //gvwHouseingQues.Rows[0].Cells[3].ReadOnly = false;
                //gvwHouseingQues.Rows[0].Cells[3].Selected = true;
            }
        }


        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (gvwHouseingQues.Rows.Count > 0)
            {
                bool boolshowpopup = true;
                if (BaseForm.BaseYear == "2015")
                {
                    if (gvwHouseingQues.Rows[0].Tag is LIHPMQuesEntity)
                    {

                        LIHPMQuesEntity drow = gvwHouseingQues.SelectedRows[0].Tag as LIHPMQuesEntity;

                        List<CommonEntity> lihpResp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTabs.LIHPMQUES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "View");
                        foreach (CommonEntity dr in lihpResp)
                        {
                            string resDesc = dr.Desc.ToString().Trim();
                            MenuItem menuItem = new MenuItem();
                            menuItem.Text = resDesc;
                            menuItem.Tag = dr.Code;
                            //if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                            //{
                            //    menuItem.Checked = true;
                            //}
                            contextMenu1.MenuItems.Add(menuItem);
                        }
                        MenuItem menuItem1 = new MenuItem();
                        menuItem1.Text = "Clear";
                        menuItem1.Tag = string.Empty;
                        contextMenu1.MenuItems.Add(menuItem1);
                    }

                }
                //if statement commented by sudheer on 07/31/2020
                else //if (BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020")
                {
                    if (gvwHouseingQues.Rows[0].Tag is LIHPMQuesEntity)
                    {

                        string strdesccode = string.Empty;
                        LIHPMQuesEntity drow = gvwHouseingQues.SelectedRows[0].Tag as LIHPMQuesEntity;

                        if (drow.LPMQ_CODE.ToString() == "0013")
                        {
                            boolshowpopup = false;
                            strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0012")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    break;
                                }
                            }
                            if (strdesccode == "Y")
                            {
                                boolshowpopup = true;
                            }
                        }
                        else if (drow.LPMQ_CODE.ToString() == "0014")
                        {
                            boolshowpopup = false;
                            strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0013")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    break;
                                }
                            }
                            if (strdesccode == "Y")
                            {
                                boolshowpopup = true;
                            }
                        }
                        else if (drow.LPMQ_CODE.ToString() == "0016")
                        {
                            boolshowpopup = false;
                            strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0015")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    break;
                                }
                            }
                            if (strdesccode == "Y")
                            {
                                boolshowpopup = true;
                            }
                        }
                        else if (drow.LPMQ_CODE.ToString() == "0017")
                        {
                            boolshowpopup = false;
                            strdesccode = string.Empty;
                            foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                            {
                                if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0016")
                                {
                                    strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    break;
                                }
                            }
                            if (strdesccode == "Y")
                            {
                                boolshowpopup = true;
                            }
                        }

                        if (propQuesType == "N")
                        {
                            if (drow.LPMQ_CODE.ToString() == "0003")
                            {
                                strdesccode = string.Empty;
                                boolshowpopup = false;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0002")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0003")
                                    {
                                        if (strdesccode == "N")
                                        {
                                            item.Cells["gvtResponseDesc"].Tag = "U";
                                            item.Cells["gvtResponseDesc"].Value = "Not Applicable";
                                        }
                                    }
                                }
                                if (strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }

                            }
                            else if (drow.LPMQ_CODE.ToString() == "0005")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                string str4ques = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0001")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0004")
                                    {
                                        str4ques = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                }
                                if (str4ques == "N" && strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }

                            }
                            else if (drow.LPMQ_CODE.ToString() == "0007" || drow.LPMQ_CODE.ToString() == "0008")
                            {
                                strdesccode = string.Empty;
                                boolshowpopup = false;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0006")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                        break;
                                    }

                                }
                                if (drow.LPMQ_CODE.ToString() == "0007" && strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }
                                if (drow.LPMQ_CODE.ToString() == "0008" && strdesccode == "N")
                                {
                                    boolshowpopup = true;
                                }
                            }
                            else if (drow.LPMQ_CODE.ToString() == "0009")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0008")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                        break;
                                    }
                                }
                                if (strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }
                            }

                        }
                        else
                        {
                            if (drow.LPMQ_CODE.ToString() == "0003")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0002")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0003")
                                    {
                                        if (strdesccode == "N")
                                        {
                                            item.Cells["gvtResponseDesc"].Tag = "U";
                                            item.Cells["gvtResponseDesc"].Value = "Not Applicable";
                                            item.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                        }
                                    }
                                }
                                if (strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }

                            }
                            else if (drow.LPMQ_CODE.ToString() == "0005")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                string strFirstAnswer = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0001")
                                    {
                                        strFirstAnswer = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0004")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                    else if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0005")
                                    {
                                        if (((strFirstAnswer == "Y" || strFirstAnswer == "N") && strdesccode == string.Empty) && ((strdesccode == "Y" || strdesccode == "N") && strFirstAnswer == string.Empty))
                                        {
                                            item.Cells["gvtResponseDesc"].Tag = string.Empty;
                                            item.Cells["gvtResponseDesc"].Value = "";
                                        }
                                    }
                                }
                                if (strdesccode == "N" && strFirstAnswer == "Y")
                                {
                                    boolshowpopup = true;
                                }

                            }
                            else if (drow.LPMQ_CODE.ToString() == "0006")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0004")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                    }
                                }
                                if (strdesccode == "Y" && (strFilterques == "01" || strFilterques == "03" || strFilterques == "07"))
                                {
                                    boolshowpopup = true;
                                }

                            }
                            else if (drow.LPMQ_CODE.ToString() == "0008" || drow.LPMQ_CODE.ToString() == "0009")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0007")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                        break;
                                    }
                                }
                                if (drow.LPMQ_CODE.ToString() == "0008" && strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }
                                if (drow.LPMQ_CODE.ToString() == "0009" && strdesccode == "N")
                                {
                                    boolshowpopup = true;
                                }
                            }
                            else if (drow.LPMQ_CODE.ToString() == "0010")
                            {
                                boolshowpopup = false;
                                strdesccode = string.Empty;
                                foreach (DataGridViewRow item in gvwHouseingQues.Rows)
                                {
                                    if (item.Cells["gvtQuesCode"].Value.ToString().Trim() == "0009")
                                    {
                                        strdesccode = Convert.ToString(item.Cells["gvtResponseDesc"].Tag);
                                        break;
                                    }
                                }
                                if (strdesccode == "Y")
                                {
                                    boolshowpopup = true;
                                }
                            }


                        }

                        if (boolshowpopup)
                        {
                            List<CommonEntity> lihpResp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, drow.LPMQ_AGYSTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "View");
                            foreach (CommonEntity dr in lihpResp)
                            {
                                string resDesc = dr.Desc.ToString().Trim();
                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = resDesc;
                                menuItem.Tag = dr.Code;
                                //if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                                //{
                                //    menuItem.Checked = true;
                                //}
                                contextMenu1.MenuItems.Add(menuItem);
                            }
                            MenuItem menuItem1 = new MenuItem();
                            menuItem1.Text = "Clear";
                            menuItem1.Tag = string.Empty;
                            contextMenu1.MenuItems.Add(menuItem1);
                        }
                    }
                }
                contextMenu1.Update();
            }
        }

        private void FillLihpQues(CaseMstEntity casemstdata, string strFilter, string stryear)
        {
            List<LIHPMQuesEntity> lihpmQues = _model.ZipCodeAndAgency.GetLIHPMQuesData(string.Empty, stryear);//"S0056"
            int rowindex;

            if (stryear == "2015")
            {
                foreach (LIHPMQuesEntity item in lihpmQues)
                {
                    switch (item.LPMQ_CODE)
                    {
                        case "0001":
                            //if (strFilter != "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0001), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0001;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            // }
                            break;
                        case "0002":
                            //if (strFilter != "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0002), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0002;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            // }
                            break;
                        case "0003":
                            //if (strFilter != "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0003), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0003;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            // }
                            break;
                        case "0004":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0004), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0004;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            break;
                        case "0005":
                            if (strFilter != "02" && strFilter != "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0005), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0005;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        case "0006":
                            if (strFilter != "02" && strFilter != "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0006), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0006;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        case "0007":
                            if (strFilter == "02" || strFilter == "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0007), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0007;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        case "0008":
                            if (strFilter == "02" || strFilter == "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0008), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0008;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        case "0009":
                            if (strFilter == "02" || strFilter == "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0009), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0009;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        case "0010":
                            if (strFilter == "02" || strFilter == "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0010), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0010;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        case "0011":
                            if (strFilter == "02" || strFilter == "04")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0011), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0011;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            //if statement commented by sudheer on 07/31/2020
            else //if (BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020")
            {
                lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE == propQuesType);
                foreach (LIHPMQuesEntity item in lihpmQues)
                {
                    switch (item.LPMQ_CODE)
                    {
                        case "0001":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0001), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0001;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            break;
                        case "0002":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0002), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0002;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            break;
                        case "0003":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0003), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0003;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (casemstdata.LPM0002 == "Y")
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                            }
                            break;
                        case "0004":
                            if (propQuesType == "Y")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0004), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0004;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                            }
                            else
                            {
                                if (strFilter == "02")
                                {
                                    rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0004), item.LPMQ_CODE);
                                    gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0004;
                                    gvwHouseingQues.Rows[rowindex].Tag = item;
                                }
                            }
                            break;
                        case "0005":
                            if (propQuesType == "Y")
                            {
                                rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0005), item.LPMQ_CODE);
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0005;
                                gvwHouseingQues.Rows[rowindex].Tag = item;
                                if (casemstdata.LPM0001 == "Y" && casemstdata.LPM0004 == "N")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            else
                            {
                                if (strFilter == "02")
                                {
                                    rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0005), item.LPMQ_CODE);
                                    gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0005;
                                    gvwHouseingQues.Rows[rowindex].Tag = item;
                                    if (casemstdata.LPM0001 == "Y" && casemstdata.LPM0004 == "N")
                                    {
                                        gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                    }
                                }
                            }
                            break;
                        case "0006":
                            //if (strFilter != "02" && strFilter != "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0006), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0006;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (propQuesType == "Y")
                            {
                                if (casemstdata.LPM0004 == "Y" && (strFilterques == "01" || strFilterques == "03" || strFilterques == "07"))
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            //}
                            break;
                        case "0007":
                            //if (strFilter == "02" || strFilter == "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0007), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0007;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (propQuesType == "N")
                            {
                                if (casemstdata.LPM0006 == "Y")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            //}
                            break;
                        case "0008":
                            //if (strFilter == "02" || strFilter == "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0008), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0008;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (propQuesType == "Y")
                            {
                                if (casemstdata.LPM0007 == "Y")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            if (propQuesType == "N")
                            {
                                if (casemstdata.LPM0006 == "N")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            // }
                            break;
                        case "0009":
                            //if (strFilter == "02" || strFilter == "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0009), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0009;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (propQuesType == "Y")
                            {
                                if (casemstdata.LPM0007 == "N")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            if (propQuesType == "N")
                            {
                                if (casemstdata.LPM0008 == "Y")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            // }
                            break;
                        case "0010":
                            //if (strFilter == "02" || strFilter == "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0010), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0010;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (propQuesType == "Y")
                            {
                                if (casemstdata.LPM0009 == "Y")
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                }
                            }
                            // }
                            break;
                        case "0011":
                            //if (strFilter == "02" || strFilter == "04")
                            //{
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0011), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0011;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            //}
                            break;
                        case "0012":

                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0012), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0012;
                            gvwHouseingQues.Rows[rowindex].Tag = item;

                            //if (casemstdata.LPM0012 == "Y")
                            //{
                            //    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            //}
                            //else
                            //{
                            //    gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                            //}


                            break;
                        case "0013":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0013), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0013;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (casemstdata.LPM0012 == "Y")
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Value = string.Empty;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = string.Empty;
                            }
                            break;
                        case "0014":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0014), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0014;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (casemstdata.LPM0013 == "Y")
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Value = string.Empty;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = string.Empty;
                            }

                            break;
                        case "0015":
                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0015), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0015;
                            gvwHouseingQues.Rows[rowindex].Tag = item;

                            break;
                        case "0016":

                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0016), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0016;
                            gvwHouseingQues.Rows[rowindex].Tag = item;

                            if (casemstdata.LPM0015 == "Y")
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Value = string.Empty;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = string.Empty;
                            }

                            break;
                        case "0017":

                            rowindex = gvwHouseingQues.Rows.Add(string.Empty, item.LPMQ_SNO + ". " + item.LPMQ_DESC, CommonFunctions.GetLIMPQuesResp(casemstdata.LPM0017), item.LPMQ_CODE);
                            gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = casemstdata.LPM0017;
                            gvwHouseingQues.Rows[rowindex].Tag = item;
                            if (casemstdata.LPM0016 == "Y")
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                gvwHouseingQues.Rows[rowindex].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Value = string.Empty;
                                gvwHouseingQues.Rows[rowindex].Cells["gvtResponseDesc"].Tag = string.Empty;
                            }
                            break;
                        default:
                            break;
                    }

                }
            }

            if(gvwHouseingQues.Rows.Count>0)
            {
                gvwHouseingQues.Rows[0].Selected = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            propCaseMst.LPM0001 = string.Empty;
            propCaseMst.LPM0002 = string.Empty;
            propCaseMst.LPM0003 = string.Empty;
            propCaseMst.LPM0004 = string.Empty;
            propCaseMst.LPM0005 = string.Empty;
            propCaseMst.LPM0006 = string.Empty;
            propCaseMst.LPM0007 = string.Empty;
            propCaseMst.LPM0008 = string.Empty;
            propCaseMst.LPM0009 = string.Empty;
            propCaseMst.LPM0010 = string.Empty;
            propCaseMst.LPM0011 = string.Empty;
            propCaseMst.LPM0011 = string.Empty;
            propCaseMst.LPM0012 = string.Empty;
            propCaseMst.LPM0013 = string.Empty;
            propCaseMst.LPM0014 = string.Empty;
            propCaseMst.LPM0015 = string.Empty;
            propCaseMst.LPM0016 = string.Empty;
            propCaseMst.LPM0017 = string.Empty;
            bool boolQuesvaldation = true;

            if (BaseForm.BaseYear == "2015")
            {

                foreach (DataGridViewRow gvwhousrows in gvwHouseingQues.Rows)
                {
                    if (!boolQuesvaldation)
                        break;
                    switch (Convert.ToString(gvwhousrows.Cells["gvtQuesCode"].Value))
                    {

                        case "0001":
                            propCaseMst.LPM0001 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0001.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0002":
                            propCaseMst.LPM0002 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0002.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0003":
                            propCaseMst.LPM0003 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0003.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0004":
                            propCaseMst.LPM0004 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0004.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0005":
                            propCaseMst.LPM0005 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0005.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0006":
                            propCaseMst.LPM0006 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0006.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0007":
                            propCaseMst.LPM0007 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0007.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0008":
                            propCaseMst.LPM0008 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0008.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0009":
                            propCaseMst.LPM0009 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0009.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0010":
                            propCaseMst.LPM0010 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0010.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0011":
                            propCaseMst.LPM0011 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0011.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            //Commented by Sudheer on 07/31/2020
            //else if ((BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020") && propQuesType == "Y")
            else if (propQuesType == "Y")
            {
                foreach (DataGridViewRow gvwhousrows in gvwHouseingQues.Rows)
                {
                    if (!boolQuesvaldation)
                        break;
                    switch (Convert.ToString(gvwhousrows.Cells["gvtQuesCode"].Value))
                    {

                        case "0001":
                            propCaseMst.LPM0001 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0001.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0002":
                            propCaseMst.LPM0002 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0002.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0003":
                            propCaseMst.LPM0003 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0002 == "Y")
                            {
                                if (propCaseMst.LPM0003.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0004":
                            propCaseMst.LPM0004 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0004.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0005":
                            propCaseMst.LPM0005 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0001 == "Y" && propCaseMst.LPM0004 == "N")
                            {
                                if (propCaseMst.LPM0005.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0006":
                            propCaseMst.LPM0006 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0004 == "Y" && (strFilterques == "01" || strFilterques == "03" || strFilterques == "07"))
                            {
                                if (propCaseMst.LPM0006.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0007":
                            propCaseMst.LPM0007 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0007.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0008":
                            propCaseMst.LPM0008 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0007 == "Y")
                            {
                                if (propCaseMst.LPM0008.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0009":
                            propCaseMst.LPM0009 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0007 == "N")
                            {
                                if (propCaseMst.LPM0009.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0010":
                            propCaseMst.LPM0010 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0009 == "Y")
                            {
                                if (propCaseMst.LPM0010.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0011":
                            propCaseMst.LPM0011 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0011.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0012":
                            propCaseMst.LPM0012 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0012.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0013":
                            propCaseMst.LPM0013 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0012.Trim() == "Y")
                                if (propCaseMst.LPM0013.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        case "0014":
                            propCaseMst.LPM0014 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0013.Trim() == "Y")
                                if (propCaseMst.LPM0014.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        case "0015":
                            propCaseMst.LPM0015 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0015.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0016":
                            propCaseMst.LPM0016 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0015.Trim() == "Y")
                                if (propCaseMst.LPM0016.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        case "0017":
                            propCaseMst.LPM0017 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0016.Trim() == "Y")
                                if (propCaseMst.LPM0017.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            //Commented by Sudheer on 07/31/2020
            //else if ((BaseForm.BaseYear == "2016" || BaseForm.BaseYear == "2017" || BaseForm.BaseYear == "2018" || BaseForm.BaseYear == "2019" || BaseForm.BaseYear == "2020") && propQuesType != "Y")
            else if (propQuesType != "Y")
            {
                foreach (DataGridViewRow gvwhousrows in gvwHouseingQues.Rows)
                {
                    if (!boolQuesvaldation)
                        break;
                    switch (Convert.ToString(gvwhousrows.Cells["gvtQuesCode"].Value))
                    {

                        case "0001":
                            propCaseMst.LPM0001 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0001.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0002":
                            propCaseMst.LPM0002 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0002.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0003":
                            propCaseMst.LPM0003 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0002 == "Y")
                            {
                                if (propCaseMst.LPM0003.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0004":
                            if (strFilterques == "02")
                            {
                                propCaseMst.LPM0004 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                                if (propCaseMst.LPM0004.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0005":
                            if (strFilterques == "02")
                            {
                                propCaseMst.LPM0005 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                                if (propCaseMst.LPM0001 == "Y" && propCaseMst.LPM0004 == "N")
                                {
                                    if (propCaseMst.LPM0005.Trim() == string.Empty)
                                        boolQuesvaldation = false;
                                }
                            }
                            break;
                        case "0006":
                            propCaseMst.LPM0006 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0006.Trim() == string.Empty)
                                boolQuesvaldation = false;

                            break;
                        case "0007":
                            propCaseMst.LPM0007 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0006 == "Y")
                            {
                                if (propCaseMst.LPM0007.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0008":
                            propCaseMst.LPM0008 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0006 == "N")
                            {
                                if (propCaseMst.LPM0008.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0009":
                            propCaseMst.LPM0009 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0008 == "Y")
                            {
                                if (propCaseMst.LPM0009.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            }
                            break;
                        case "0010":
                            propCaseMst.LPM0010 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0010.Trim() == string.Empty)
                                boolQuesvaldation = false;

                            break;
                        case "0011":
                            propCaseMst.LPM0011 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0011.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0012":
                            propCaseMst.LPM0012 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0012.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0013":
                            propCaseMst.LPM0013 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0012.Trim() == "Y")
                                if (propCaseMst.LPM0013.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        case "0014":
                            propCaseMst.LPM0014 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0013.Trim() == "Y")
                                if (propCaseMst.LPM0014.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        case "0015":
                            propCaseMst.LPM0015 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0015.Trim() == string.Empty)
                                boolQuesvaldation = false;
                            break;
                        case "0016":
                            propCaseMst.LPM0016 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0015.Trim() == "Y")
                                if (propCaseMst.LPM0016.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        case "0017":
                            propCaseMst.LPM0017 = Convert.ToString(gvwhousrows.Cells["gvtResponseDesc"].Tag);
                            if (propCaseMst.LPM0016.Trim() == "Y")
                                if (propCaseMst.LPM0017.Trim() == string.Empty)
                                    boolQuesvaldation = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (boolQuesvaldation)
            {
                this.DialogResult = Wisej.Web.DialogResult.OK;
                this.Close();
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Please fill All Responses");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}