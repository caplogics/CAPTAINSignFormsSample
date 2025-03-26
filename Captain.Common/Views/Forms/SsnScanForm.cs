#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SsnScanForm : Form
    {
       
        private CaptainModel _model = null;
        private string strNameFormat = string.Empty;
        public SsnScanForm(BaseForm baseform,string SsnNo)
        {
           
            _model = new CaptainModel();
            InitializeComponent();
            baseForm = baseform;
            HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(baseForm.BaseAgency, "**", "**");
            if (HierarchyEntity != null)
            {
                strNameFormat = HierarchyEntity.CNFormat.ToString();
            }
            SsnScanSearch(SsnNo);
        }

        public BaseForm baseForm { get; set; }

        private void btnSSNSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SsnScanSearch(string SsnNo)
        {
            gvwssnscansearch.Rows.Clear();

            List<CaseMstSnpEntity> CaseSnpEntityList = _model.CaseMstData.GetSSNSearch("ALL", SsnNo, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Y", string.Empty, string.Empty, string.Empty, string.Empty, baseForm.UserID, baseForm.BaseAgency + baseForm.BaseDept + baseForm.BaseProg,string.Empty);
            List<string> listSSN = new List<string>();
            foreach (CaseMstSnpEntity CaseSnp in CaseSnpEntityList)
            {
                string ApplicantName = LookupDataAccess.GetMemberName(CaseSnp.NameixFi, CaseSnp.NameixMi, CaseSnp.NameixLast, strNameFormat);

                //if (IsDup.Equals("N"))
                //{
                //    if (listSSN.Exists(u => u.Equals(CaseSnp.Ssno.ToString() + ApplicantName))) continue;
                //}

                //listSSN.Add(CaseSnp.Ssno.ToString() + ApplicantName);
                //string Address = CaseSnp.Hn.Trim();
                //if (!CaseSnp.Street.Equals(string.Empty))
                //{
                //    if (!Address.Equals(string.Empty)) { Address += ", "; }
                //    Address += CaseSnp.Street.Trim();
                //}
                //if (!CaseSnp.City.Equals(string.Empty))
                //{
                //    if (!Address.Equals(string.Empty)) { Address += ", "; }
                //    Address += CaseSnp.City.Trim();
                //}
                //if (!CaseSnp.State.Equals(string.Empty))
                //{
                //    if (!Address.Equals(string.Empty)) { Address += ", "; }
                //    Address += CaseSnp.State.Trim();
                //}
                //string phone = "   ".Substring(0, 3 - CaseSnp.Area.Length) + CaseSnp.Area + CaseSnp.Phone + "       ".Substring(0, 7 - CaseSnp.Phone.Length);

                int rowIndex = gvwssnscansearch.Rows.Add(CaseSnp.Agency + "  " + CaseSnp.Dept + "  " + CaseSnp.Program + "  " + CaseSnp.Year + "  " + CaseSnp.ApplNo, ApplicantName, LookupDataAccess.Getdate(CaseSnp.AltBdate), LookupDataAccess.Getdate(CaseSnp.DateLstc),CaseSnp.Ssno,CaseSnp.FamilySeq);                  
               // int rowIndex = gvwssnscansearch.Rows.Add(CaseSnp.Ssno, ApplicantName, phone, Address);
                gvwssnscansearch.Rows[rowIndex].Tag = CaseSnp;
                if (CaseSnp.SnpActive.Equals("I"))
                    gvwssnscansearch.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
           
            //if (Rows == 0)
            //{
            //    MessageBox.Show("No Records Exists...", "CAPTAIN", MessageBoxButtons.OK);
            //}
        }

        public CaseMstSnpEntity GetSelectedRow()
        {
            CaseMstSnpEntity entity = null;
            if (gvwssnscansearch != null)
            {
                foreach (DataGridViewRow dr in gvwssnscansearch.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        entity = dr.Tag as CaseMstSnpEntity;
                        //string DOB = string.Empty;
                        //if (!entity.AltBdate.Equals(string.Empty))
                        //{
                        //    DOB = CommonFunctions.ChangeDateFormat(entity.AltBdate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        //}
                        //string yr = entity.Year.Equals(string.Empty) ? "    " : entity.Year;
                        ////lblFooter.Text = entity.Agency + "  " + entity.Dept + "  " + entity.Program + " " + yr + "   App #" + entity.ApplNo + "       DOB : " + DOB;
                        break;
                    }
                }
            }
            return entity;
        }


        public string[] GetScanSelected()
        {
            string[] SelDetails = new string[3];
            string SelDetails2 = null;
            SelDetails[0] = SelDetails[1] = SelDetails[2] = null;
            if (gvwssnscansearch != null)
            {
                foreach (DataGridViewRow dr in gvwssnscansearch.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        //DataRow srow = dr.Tag as DataRow;

                        SelDetails2 = dr.Cells["SsnNo"].Value.ToString();
                        SelDetails[0] = SelDetails2;
                        SelDetails2 = dr.Cells["ApplicantNumber"].Value.ToString();
                        SelDetails[1] = SelDetails2;
                        SelDetails2 = dr.Cells["MemSeq"].Value.ToString();
                        SelDetails[2] = SelDetails2;                                            

                        break;
                    }
                }
            }
            return SelDetails;
        }

        private String[] SsnScanSearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            string[] SelSsn = new string[3];
            SelSsn = GetScanSelected();

            return SelSsn;
        }
    }
}