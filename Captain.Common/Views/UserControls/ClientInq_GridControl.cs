#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.EventArg;
using Captain.Common.Handlers;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ClientInq_GridControl : UserControl
    {
        private List<CLINQHIEEntity> _selectedHierarchies = new List<CLINQHIEEntity>();

        public ClientInq_GridControl(BaseForm baseForm, string hieType, UserEntity userProfile,string mode)
        {
            InitializeComponent();
            BaseForm = baseForm;
            UserProfile = userProfile;
            HieType = hieType;
            GridViewControl = gvwControl;
        }

        #region Public Properties

        public UserEntity UserProfile { get; set; }

        public BaseForm BaseForm { get; set; }      

        public string HieType { get; set; }

        public string Mode { get; set; }

        public bool SetEditable
        {
            set
            {
                picAdd.Visible = !value;
                picEdit.Visible = value;
            }
        }

        public bool SetVisible
        {
            set
            {
                picAdd.Visible = value;
                picEdit.Visible = value;
            }
        }

        public List<CLINQHIEEntity> UserHierarchy { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView GridViewControl
        {
            get;
            set;
        }

        public List<CLINQHIEEntity> SelectedHierarchies
        {
            get
            {
                _selectedHierarchies = new List<CLINQHIEEntity>();
                foreach (DataGridViewRow row in gvwControl.Rows)
                {
                    CLINQHIEEntity hierarchy = row.Tag as CLINQHIEEntity;
                    if (!hierarchy.UsedFlag.Equals("Y"))
                    {
                        _selectedHierarchies.Add(hierarchy);
                    }
                }
                return _selectedHierarchies;
            }
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
          // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            ClientInquiry_Hierarchies form = sender as ClientInquiry_Hierarchies;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            if (form.DialogResult == DialogResult.OK)
            {
                List<CLINQHIEEntity> selectedHierarchies = form.SelectedHierar();

                List<CLINQHIEEntity> usedHierarchies = (from c in gvwControl.Rows.Cast<DataGridViewRow>().ToList()
                                                        select ((DataGridViewRow)c).Tag as CLINQHIEEntity).ToList();
                //usedHierarchies = usedHierarchies.FindAll(u => u.UsedFlag.Equals("Y"));
                gvwControl.Rows.Clear();
                
                //foreach (CLINQHIEEntity row in usedHierarchies)
                //{
                //    string code = row.Agency + "-" + (row.Dept == string.Empty ? "**" : row.Dept) + "-" + (row.Prog==string.Empty?"**":row.Prog);
                //    CLINQHIEEntity hieEntity = selectedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                //    if (hieEntity != null) { row.CLINQPdf = hieEntity.CLINQPdf; row.CLINQCNotes = hieEntity.CLINQCNotes; }

                //    int rowIndex = gvwControl.Rows.Add(code, row.HirarchyName.ToString(),row.CLINQPdf == "Y" ? true : false, row.CLINQCNotes == "Y" ? true : false);
                    
                //   // HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Agency+u.Dept+u.Prog == row.Agency+row.Dept+row.Prog);
                //    if (hieEntity != null)
                //    {
                //        row.UsedFlag = "N";
                //        gvwControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                //    }
                //    else
                //    {
                //        row.UsedFlag = "Y";
                //        gvwControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    }
                //    gvwControl.Rows[rowIndex].Tag = row;                    
                //}
                ////selectedHierarchies = selectedHierarchies.FindAll(u => !u.UsedFlag.Equals("Y"));
                foreach (CLINQHIEEntity row in selectedHierarchies)
                {
                    //CLINQHIEEntity hieEntity = usedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    //if (hieEntity == null)
                    //{
                        int rowIndex = gvwControl.Rows.Add(row.Code, row.HirarchyName.ToString(),row.CLINQPdf == "Y" ? true : false, row.CLINQCNotes == "Y" ? true : false);
                        gvwControl.Rows[rowIndex].Tag = row;
                    //}
                }
                ////RefreshGrid();
            }
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            try
            {
                ClientInquiry_Hierarchies addForm = new ClientInquiry_Hierarchies(BaseForm, SelectedHierarchies, "Add", "", "I", "I");
                //// HierarchieSelectionForm addForm = new HierarchieSelectionForm(BaseForm, SelectedHierarchies, HieType, null, "Add");
                // HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Add", "I", "*", "I", UserProfile);
                addForm.StartPosition = FormStartPosition.CenterScreen;
                addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                addForm.ShowDialog();
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            try
            {
                ClientInquiry_Hierarchies hsForm = new ClientInquiry_Hierarchies(BaseForm, SelectedHierarchies, "Edit", "I", "I", "I");
                ////HierarchieSelectionForm hsForm = new HierarchieSelectionForm(BaseForm, SelectedHierarchies, HieType, UserProfile, "Edit");  
                //HierarchieSelectionFormNew hsForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Edit", "I", "*", "I", UserProfile);
                hsForm.StartPosition = FormStartPosition.CenterScreen;
                hsForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                hsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                //
            }
        }

        #endregion
 
    }
}