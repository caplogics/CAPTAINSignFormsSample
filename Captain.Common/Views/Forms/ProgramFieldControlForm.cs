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
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ProgramFieldControlForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
       

        #endregion
        public ProgramFieldControlForm(BaseForm baseForm, string mode, string catcode, string Hie,List<PMTFLDCNTLHEntity> pmtFldcntlHentity)
        {
            InitializeComponent();
            _model = new CaptainModel();
            
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            propcatcode = catcode;
            propHie = Hie;
            CntlCAEntity = new List<FldcntlHieEntity>();
            propPMTFLDCNTLHEntity = pmtFldcntlHentity;
            if (propPMTFLDCNTLHEntity.Count == 0)
            {
                CntlCAEntity = _model.FieldControls.GetFLDCNTLHIE("PAYCAT" + propcatcode.Trim(), propHie, "FLDCNTL");
            }
            this.Text = "Field Control Settings";
            fillGridload();
        }

        //Added by Sudheer on 05/20/2021
        public ProgramFieldControlForm(BaseForm baseForm, string mode, string catcode, string Hie,string spcode,string branch,string grp,string cacode, List<PMTFLDCNTLHEntity> pmtFldcntlHentity)
        {
            InitializeComponent();
            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            propcatcode = catcode;
            propHie = Hie;
            SPCode = spcode;Branch = branch;GroupCd = grp;CACode = cacode;
            propPMTFLDCNTLHEntity = pmtFldcntlHentity;

            CntlCAEntity = new List<FldcntlHieEntity>();
            if (propPMTFLDCNTLHEntity.Count==0)
            {
                CntlCAEntity = _model.FieldControls.GetFLDCNTLHIE("PAYCAT" + propcatcode.Trim(), propHie, "FLDCNTL");
            }

            btnOk.Text = "Save";

            this.Text = "ADMN0020 – Field Control Settings";

            fillGridload();
        }

        public BaseForm BaseForm { get; set; }
        public string Mode { get; set; }
        public string propcatcode { get; set; }
        public string propHie { get; set; }
        public string SPCode { get; set; }
        public string Branch { get; set; }
        public string GroupCd { get; set; }
        public string CACode { get; set; }
        public List<PMTFLDCNTLHEntity> propPMTFLDCNTLHEntity = new List<PMTFLDCNTLHEntity>();
        public List<FldcntlHieEntity> CntlCAEntity { get; set; }
        private void fillGridload()
        {
            bool boolrequired = false;
            bool boolEnable = false;
            int rowIndex = 0;
            List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", propcatcode, propHie, "PMTSTDFLDS");
            if (propPMTFLDCNTLHEntity.Count > 0)
            {
                foreach (PMTSTDFLDSEntity gvitem in pmtstdfldsentity)
                {
                    boolrequired = false;
                    boolEnable = false;
                    if (propPMTFLDCNTLHEntity.Count > 0)
                    {
                        if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == gvitem.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                            boolEnable = true;
                        if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == gvitem.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                            boolrequired = true;
                    }
                    rowIndex=gvwProgramCode.Rows.Add(gvitem.PSTF_DESC, boolEnable, boolrequired, gvitem.PSTF_FLD_CODE);
                    if (gvitem.PSTF_FLD_CODE.Substring(1, 1) == "A" || gvitem.PSTF_FLD_CODE.Substring(1, 1) == "a")
                    {
                        gvwProgramCode.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  // Color.Red;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcEnabled"].Value = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcRequied"].Value = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcEnabled"].ReadOnly = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcRequied"].ReadOnly = true;
                    }
                    if (gvitem.PSTF_FLD_CODE == "S00022") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                    if (gvitem.PSTF_FLD_CODE == "S00023") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                    if (gvitem.PSTF_FLD_CODE == "S00212") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                    if (gvitem.PSTF_FLD_CODE == "S00216") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                    if (gvitem.PSTF_FLD_CODE == "S00206" || gvitem.PSTF_FLD_CODE == "S00207" || gvitem.PSTF_FLD_CODE == "S00208" || gvitem.PSTF_FLD_CODE == "S00209" || gvitem.PSTF_FLD_CODE == "S00210" || gvitem.PSTF_FLD_CODE == "S00211")
                    { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }

                }
            }
            else if(CntlCAEntity.Count>0)
            {
                foreach (PMTSTDFLDSEntity gvitem in pmtstdfldsentity)
                {
                    boolrequired = false;
                    boolEnable = false;
                    if (CntlCAEntity.Count > 0)
                    {
                        if (CntlCAEntity.Find(u => u.FldCode == gvitem.PSTF_FLD_CODE && u.Enab == "Y") != null)
                            boolEnable = true;
                        if (CntlCAEntity.Find(u => u.FldCode == gvitem.PSTF_FLD_CODE && u.Req == "Y") != null)
                            boolrequired = true;
                    }
                    rowIndex=gvwProgramCode.Rows.Add(gvitem.PSTF_DESC, boolEnable, boolrequired, gvitem.PSTF_FLD_CODE);
                    if (gvitem.PSTF_FLD_CODE.Substring(1, 1) == "A" || gvitem.PSTF_FLD_CODE.Substring(1, 1) == "a")
                    {
                        gvwProgramCode.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  // Color.Red;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcEnabled"].Value = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcRequied"].Value = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcEnabled"].ReadOnly = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcRequied"].ReadOnly = true;

                        if (gvitem.PSTF_FLD_CODE == "S00022") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00023") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00212") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false; */}
                        if (gvitem.PSTF_FLD_CODE == "S00216") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00206" || gvitem.PSTF_FLD_CODE == "S00207" || gvitem.PSTF_FLD_CODE == "S00208" || gvitem.PSTF_FLD_CODE == "S00209" || gvitem.PSTF_FLD_CODE == "S00210" || gvitem.PSTF_FLD_CODE == "S00211")
                        { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                    }
                }
            }
            else
            {
                foreach (PMTSTDFLDSEntity gvitem in pmtstdfldsentity)
                {
                    boolrequired = false;
                    boolEnable = false;
                    if (propPMTFLDCNTLHEntity.Count > 0)
                    {
                        if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == gvitem.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                            boolEnable = true;
                        if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == gvitem.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                            boolrequired = true;
                    }
                    rowIndex=gvwProgramCode.Rows.Add(gvitem.PSTF_DESC, boolEnable, boolrequired, gvitem.PSTF_FLD_CODE);
                    if (gvitem.PSTF_FLD_CODE.Substring(1, 1) == "A" || gvitem.PSTF_FLD_CODE.Substring(1, 1) == "a")
                    {
                        gvwProgramCode.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  // Color.Red;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcEnabled"].Value = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcRequied"].Value = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcEnabled"].ReadOnly = true;
                        gvwProgramCode.Rows[rowIndex].Cells["gvcRequied"].ReadOnly = true;

                        if (gvitem.PSTF_FLD_CODE == "S00022") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00023") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00212") { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00216") { gvwProgramCode.Rows[rowIndex].ReadOnly = true;/* gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                        if (gvitem.PSTF_FLD_CODE == "S00206" || gvitem.PSTF_FLD_CODE == "S00207" || gvitem.PSTF_FLD_CODE == "S00208" || gvitem.PSTF_FLD_CODE == "S00209" || gvitem.PSTF_FLD_CODE == "S00210" || gvitem.PSTF_FLD_CODE == "S00211")
                        { gvwProgramCode.Rows[rowIndex].ReadOnly = true; /*gvwProgramCode.Rows[rowIndex].Enabled = false;*/ }
                    }
                }
            }
             
        }

        private void fillGridloadwithSP()
        {
            bool boolrequired = false;
            bool boolEnable = false;
            List<PMTSTDFLDSEntity> pmtstdfldsentity = _model.FieldControls.GETPMTSTDFLDS("CASE0063", propcatcode, propHie, "PMTSTDFLDS");
            foreach (PMTSTDFLDSEntity gvitem in pmtstdfldsentity)
            {
                boolrequired = false;
                boolEnable = false;
                if (propPMTFLDCNTLHEntity.Count > 0)
                {
                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == gvitem.PSTF_FLD_CODE && u.PMFLDH_ENABLED == "Y") != null)
                        boolEnable = true;
                    if (propPMTFLDCNTLHEntity.Find(u => u.PMFLDH_CODE == gvitem.PSTF_FLD_CODE && u.PMFLDH_REQUIRED == "Y") != null)
                        boolrequired = true;
                }
                gvwProgramCode.Rows.Add(gvitem.PSTF_DESC, boolEnable, boolrequired, gvitem.PSTF_FLD_CODE);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (btnOk.Text == "Save")
            {
                savePMTFLDFLSH();
            }
            else
            {
                List<PMTFLDCNTLHEntity> pmtfldHentity = new List<PMTFLDCNTLHEntity>();
                foreach (DataGridViewRow gvrows in gvwProgramCode.Rows)
                {
                    pmtfldHentity.Add(new PMTFLDCNTLHEntity("CASE0063", propHie, gvrows.Cells["gvtCode"].Value.ToString(), (gvrows.Cells["gvcEnabled"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N"), (gvrows.Cells["gvcEnabled"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N"), (gvrows.Cells["gvcRequied"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N"), propcatcode));
                }
                propPMTFLDCNTLHEntity = pmtfldHentity;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void savePMTFLDFLSH()
        {
            PMTFLDCNTLHEntity pmtflddata = new Model.Objects.PMTFLDCNTLHEntity();
            pmtflddata.PMFLDH_SCR_CODE = "CASE0063";
            pmtflddata.PMFLDH_SCR_HIE = propHie;
            pmtflddata.PMFLDH_SP = SPCode;
            pmtflddata.PMFLDH_BRANCH = Branch;
            pmtflddata.PMFLDH_CURR_GRP = GroupCd;
            pmtflddata.PMFLDH_CA_CODE = CACode;
            pmtflddata.PMFLDH_CATG = propcatcode;
            pmtflddata.Mode = "DELETE";
            _model.FieldControls.InsertUpdateDelPMTFLDCNTLH(pmtflddata);
            bool IsFalse = false;
            foreach (DataGridViewRow gvrows in gvwProgramCode.Rows)
            {
                pmtflddata.PMFLDH_CODE = gvrows.Cells["gvtCode"].Value.ToString();
                pmtflddata.PMFLDH_ENABLED = gvrows.Cells["gvcEnabled"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                pmtflddata.PMFLDH_REQUIRED = gvrows.Cells["gvcRequied"].Value.ToString().ToUpper() == "TRUE" ? "Y" : "N";
                pmtflddata.PMFLDH_CATG = propcatcode;
                pmtflddata.Mode = "ADD";
                _model.FieldControls.InsertUpdateDelPMTFLDCNTLH(pmtflddata);

            }
            this.DialogResult = DialogResult.OK;
            this.Close();


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvwProgramCode_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwProgramCode.Rows.Count > 0 )
            {                
                string Tmp = "false";
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == gvcEnabled.Index)
                    {
                        Tmp = gvwProgramCode.CurrentRow.Cells["gvcEnabled"].Value.ToString();
                        if (Tmp == "True")
                            Tmp = "True";
                        else
                        {
                            gvwProgramCode.CurrentRow.Cells["gvcRequied"].Value = false;

                        }
                    }
                    if (e.ColumnIndex == gvcRequied.Index)
                    {
                        Tmp = gvwProgramCode.CurrentRow.Cells["gvcRequied"].Value.ToString();
                        if (Tmp == "True")
                        {
                            gvwProgramCode.CurrentRow.Cells["gvcEnabled"].Value = true;

                        }
                    }
                }
            }
        }
    }
}