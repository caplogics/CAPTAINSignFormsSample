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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CA_DuplicatesScreen : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        //public int strIndex = 0;
        //public int strCrIndex = 0;
        //public int strPageIndex = 1;

        #endregion

        public CA_DuplicatesScreen(BaseForm baseForm, string mode, string CAMS_code, string CAMS_Desc, PrivilegeEntity privileges,string scrtype)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            CaMs_Code = CAMS_code;
            Mode = mode;
            CaMs_Desc = CAMS_Desc;
            ScrType = scrtype;

            txtdupDesc.Text = CaMs_Desc.Trim();

            if (ScrType == "CA")
            {
                this.Text = "Service - Duplicate Description";
                this.Dup_Desc.HeaderText = "Service Description";
                if (CaMs_Desc.Trim().Length > 25)
                    FillDupGrid();
                else
                    FillDupGridExactMatch();
            }
            else if(scrtype=="MS")
            {
                this.Text = "Outcome - Duplicate Description";
                this.Dup_Desc.HeaderText = "Outcome Description";
                if (CaMs_Desc.Trim().Length > 25)
                    FillMSDupGrid();
                else
                    FillMSDupGridExactMatch();
            }
           
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string CaMs_Desc { get; set; }

        public string CaMs_Code { get; set; }

        //public string button_Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        //public bool IsSaveValid { get; set; }

        public string ScrType { get; set; }

        #endregion


        // Filling Grid with  Duplicate Service Description.....
        private void FillDupGrid()
        {
            gvDupCA.Rows.Clear();
            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            //DataSet dsCA = DatabaseLayer.SPAdminDB.Browse_CAMAST(null, null, null, null);
            //DataTable dtCA = dsCA.Tables[0];
            int rowIndex = 0;
            string Desc = CaMs_Desc.Trim();
            int length=Desc.Length-25;
            for (int i = 0; i < length; i++)
            {
                bool isValid = true;
                string dupDesc = Desc.Trim().Substring(i, 25);
                foreach (CAMASTEntity dr in CAList)
                {
                    if (dr.Desc.ToString().Trim().Contains(dupDesc) == true)
                    {
                        if (dr.Code.ToString().Trim() != CaMs_Code.Trim())
                        {
                            if (gvDupCA.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow drGv in gvDupCA.Rows)
                                {
                                    if (dr.Code.ToString().Trim() == drGv.Cells["CODE"].Value.ToString().Trim())
                                    isValid = false;
                                }
                            }
                            if(isValid)
                                rowIndex = gvDupCA.Rows.Add(dr.Code.ToString(), dr.Desc.ToString());
                        }
                    }
                }
            }
        }

        private void FillDupGridExactMatch()
        {
            gvDupCA.Rows.Clear();
            List<CAMASTEntity> CAList;
            CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
            int rowIndex = 0;
            string Desc = CaMs_Desc.Trim();
            int length = Desc.Length;
            for (int i = 0; i < length; i++)
            {
                bool isValid = true;
                string dupDesc = Desc.Trim();
                foreach (CAMASTEntity dr in CAList)
                {
                    if (dr.Desc.ToString().Trim() == dupDesc.Trim())
                    {
                        if (dr.Code.ToString().Trim() != CaMs_Code.Trim())
                        {
                            if (gvDupCA.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow drGv in gvDupCA.Rows)
                                {
                                    if (dr.Code.ToString().Trim() == drGv.Cells["CODE"].Value.ToString().Trim())
                                        isValid = false;
                                }
                            }
                            if (isValid)
                                rowIndex = gvDupCA.Rows.Add(dr.Code.ToString(), dr.Desc.ToString());
                        }
                    }
                }
            }
        }

        public string[] Get_Selected_Rows()
        {
            if (ScrType == "CA") FillDupGrid();
            else if (ScrType == "MS") FillMSDupGrid();
            string[] Added_Edited_SubCode = new string[2];
            Added_Edited_SubCode[0] = Mode;
            Added_Edited_SubCode[1] = gvDupCA.Rows.Count.ToString();
            return Added_Edited_SubCode;
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CAMS_Form");
        }

        private void FillMSDupGrid()
        {
            gvDupCA.Rows.Clear();
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null,null);
            //DataSet dsCA = DatabaseLayer.SPAdminDB.Browse_CAMAST(null, null, null, null);
            //DataTable dtCA = dsCA.Tables[0];
            int rowIndex = 0;
            string Desc = CaMs_Desc.Trim();
            int length = Desc.Length - 25;
            for (int i = 0; i < length; i++)
            {
                bool isValid = true;
                string dupDesc = Desc.Trim().Substring(i, 25);
                foreach (MSMASTEntity dr in MSList)
                {
                    if (dr.Desc.ToString().Trim().Contains(dupDesc) == true)
                    {
                        if (dr.Code.ToString().Trim() != CaMs_Code.Trim())
                        {
                            if (gvDupCA.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow drGv in gvDupCA.Rows)
                                {
                                    if (dr.Code.ToString().Trim() == drGv.Cells["CODE"].Value.ToString().Trim())
                                        isValid = false;
                                }
                            }
                            if (isValid)
                                rowIndex = gvDupCA.Rows.Add(dr.Code.ToString(), dr.Desc.ToString());
                        }
                    }
                }
            }
        }

        private void FillMSDupGridExactMatch()
        {
            gvDupCA.Rows.Clear();
            List<MSMASTEntity> MSList;
            MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null,null);
            int rowIndex = 0;
            string Desc = CaMs_Desc.Trim();
            int length = Desc.Length;
            for (int i = 0; i < length; i++)
            {
                bool isValid = true;
                string dupDesc = Desc.Trim();
                foreach (MSMASTEntity dr in MSList)
                {
                    if (dr.Desc.ToString().Trim() == dupDesc.Trim())
                    {
                        if (dr.Code.ToString().Trim() != CaMs_Code.Trim())
                        {
                            if (gvDupCA.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow drGv in gvDupCA.Rows)
                                {
                                    if (dr.Code.ToString().Trim() == drGv.Cells["CODE"].Value.ToString().Trim())
                                        isValid = false;
                                }
                            }
                            if (isValid)
                                rowIndex = gvDupCA.Rows.Add(dr.Code.ToString(), dr.Desc.ToString());
                        }
                    }
                }
            }
        }


    }
}