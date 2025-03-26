#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AgencySelectionForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        private List<ListItem> _selectedListItem = null;
        private CaptainModel _model = null;
        private bool boolhierchy = true;
        public AgencySelectionForm(BaseForm baseform,string mode,string selType, string Agency,string Dept, string Program)
        {
            InitializeComponent();
            BaseForm = baseform;
            MODE = mode;
            SelType = selType; Agy = Agency; dept = Dept; prog = Program;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            if (SelType == "A")
            {
                lblChoose.Text = "Choose Agency Here";
                this.Text = "Agency Selection";
            }
            else if (SelType == "D")
            {
                lblChoose.Text = "Choose Department Here";
                this.Text = "Department Selection";
            }
            else if (SelType == "P")
            {
                lblChoose.Text = "Choose Program Here";
                this.Text = "Program Selection";
            }

            FillAgencies();
        }

        public BaseForm BaseForm { get; set; }

        public string MODE { get; set; }

        public string SelType { get; set; }

        public string Agy { get; set; }

        public string dept { get; set; }

        public string prog { get; set; }


        private void FillAgencies()
        {
            List<CaseHierarchyEntity> hieEntity = new List<CaseHierarchyEntity>();
            string Message = string.Empty;
            string Code_type = string.Empty;
            if (SelType == "A")
            {
                hieEntity = _model.AdhocData.Browse_CASEHIE("**", "  ", "  ",BaseForm.UserID,BaseForm.BaseAdminAgency);
                Message = "All Agencies"; Code_type = "**";
            }
            else if (SelType == "D")
            {
                hieEntity = _model.AdhocData.Browse_CASEHIE("**", "**", "  ", BaseForm.UserID, BaseForm.BaseAdminAgency);
                hieEntity = hieEntity.FindAll(u => !u.Dept.Trim().Equals(""));
                Message = "All Departments"; Code_type = "**-**";
            }
            else if (SelType == "P")
            {
                hieEntity = _model.AdhocData.Browse_CASEHIE("**", "**", "**", BaseForm.UserID, BaseForm.BaseAdminAgency);
                hieEntity = hieEntity.FindAll(u => (!u.Dept.Trim().Equals("") && !u.Prog.Trim().Equals("")) || !u.Prog.Trim().Equals(""));
                Message = "All Programs"; Code_type = "**-**-**";
            }

            //List<HierarchyEntity> PasswordHies = new List<HierarchyEntity>();
            //PasswordHies = LookupDataAccess.GetPasswordHieByUserID(BaseForm.UserID);

            if (hieEntity.Count > 0)
            {
                if (MODE == "*")
                    gvwHierarchie.Rows.Add(false, Code_type, Message);
                foreach (CaseHierarchyEntity Entity in hieEntity)
                {
                    if (SelType == "A")
                    {
                        if(Agy==Entity.Agency)
                            gvwHierarchie.Rows.Add(true, Entity.Agency, Entity.HierarchyName.Trim());
                        else
                            gvwHierarchie.Rows.Add(false, Entity.Agency, Entity.HierarchyName.Trim());
                    }
                    else if (SelType == "D")
                    {
                        if(Agy==Entity.Agency && dept==Entity.Dept)
                            gvwHierarchie.Rows.Add(true, Entity.Agency + "-" + Entity.Dept, Entity.HierarchyName.Trim());
                        else
                            gvwHierarchie.Rows.Add(false, Entity.Agency + "-" + Entity.Dept, Entity.HierarchyName.Trim());
                    }
                    else if (SelType == "P")
                    {
                        if (Agy == Entity.Agency && dept == Entity.Dept && prog==Entity.Prog)
                            gvwHierarchie.Rows.Add(true, Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Entity.HierarchyName.Trim());
                        else
                            gvwHierarchie.Rows.Add(false, Entity.Agency + "-" + Entity.Dept + "-" + Entity.Prog, Entity.HierarchyName.Trim());
                    }
                }
            }
            if (gvwHierarchie.Rows.Count > 0)
                gvwHierarchie.Rows[0].Selected = true;
        }

        private void gvwHierarchie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwHierarchie.Rows.Count > 0)
            {
                string SelCode = gvwHierarchie.CurrentRow.Cells["Code"].Value.ToString();

                foreach (DataGridViewRow dr in gvwHierarchie.Rows)
                {
                    if (e.ColumnIndex == 0 && e.RowIndex != -1)
                    {
                        //if (Convert.ToBoolean(gvwHierarchie.CurrentRow.Cells["Select"].Value) == true)
                        //{
                        //    dr.Cells["Select"].Value = true;
                        //}
                        //else
                        //{
                        //    dr.Cells["Select"].Value = false;
                        //}
                        string rowcode = dr.Cells["Code"].Value.ToString();
                        if (!rowcode.Equals(SelCode))
                        {
                            dr.Cells["Select"].Value = false;
                        }

                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public string[] GetAgency()
        {
            string[] Agency_Code = new string[2];
            foreach (DataGridViewRow dr in gvwHierarchie.Rows)
            {
                if (Convert.ToBoolean(dr.Cells["Select"].Value) == true)
                {
                    Agency_Code[0] = dr.Cells["Code"].Value.ToString();
                    Agency_Code[1] = dr.Cells["Desc"].Value.ToString(); break;
                }
            }

            return Agency_Code;
        }
    }
}