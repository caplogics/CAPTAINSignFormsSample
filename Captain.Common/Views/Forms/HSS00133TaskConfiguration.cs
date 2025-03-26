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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00133TaskConfiguration : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;

        #endregion
        public HSS00133TaskConfiguration(BaseForm baseForm, string mode, PrivilegeEntity privilege, ChldTrckEntity chldtrckEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            PrivilegEntity = privilege;
            Mode = mode;
            //this.Text = PrivilegEntity.PrivilegeName + " - Task Configuration";
            this.Text = "Task Configuration";

            _model = new CaptainModel();
            List<CommonEntity> listTask = _model.lookupDataAccess.GetHssb00133TaskConfiguration();
            string strEnable = string.Empty;
            string strRequire = string.Empty;
            int rowIndex;
            if (!Mode.Equals("View"))
            {
                btnOk.Visible = true;
                btnCancel.Text = "Cancel";
            }
            else
            {
                btnOk.Visible = false;
                btnCancel.Text = "Exit";
            }
            //if (!Mode.Equals("Add"))
            //{
            foreach (CommonEntity item in listTask)
            {
                switch (item.Code)
                {
                    case "QU":
                        strEnable = chldtrckEntity.QUESTIONE == string.Empty ? "N" : chldtrckEntity.QUESTIONE;
                        strRequire = chldtrckEntity.QUESTIONR == string.Empty ? "N" : chldtrckEntity.QUESTIONR;
                        break;
                    case "CA":
                        strEnable = chldtrckEntity.CASENOTESE == string.Empty ? "N" : chldtrckEntity.CASENOTESE;
                        strRequire = chldtrckEntity.CASENOTESR == string.Empty ? "N" : chldtrckEntity.CASENOTESR;
                        break;
                    case "SB":
                        strEnable = chldtrckEntity.SBCBE == string.Empty ? "N" : chldtrckEntity.SBCBE;
                        strRequire = chldtrckEntity.SBCBR == string.Empty ? "N" : chldtrckEntity.SBCBR;
                        break;
                    case "AD":
                        strEnable = "Y";
                        strRequire = "Y";
                        rowIndex = gvTaskConfig.Rows.Add(item.Desc, Img_Tick, Img_Tick, "Y", "Y", item.Code);
                        gvTaskConfig.Rows[rowIndex].Cells["gviEnable"].ReadOnly = true;
                        gvTaskConfig.Rows[rowIndex].Cells["gviReq"].ReadOnly = true;

                        break;
                    case "CO":
                        strEnable = chldtrckEntity.COMPLETEE == string.Empty ? "N" : chldtrckEntity.COMPLETEE;
                        strRequire = chldtrckEntity.COMPLETER == string.Empty ? "N" : chldtrckEntity.COMPLETER;
                        break;
                    case "FO":
                        strEnable = chldtrckEntity.FOLLOWUPE == string.Empty ? "N" : chldtrckEntity.FOLLOWUPE;
                        strRequire = chldtrckEntity.FOLLOWUPR == string.Empty ? "N" : chldtrckEntity.FOLLOWUPR;
                        break;
                    case "FC":
                        strEnable = chldtrckEntity.FOLLOWUPCE == string.Empty ? "N" : chldtrckEntity.FOLLOWUPCE;
                        strRequire = chldtrckEntity.FOLLOWUPCR == string.Empty ? "N" : chldtrckEntity.FOLLOWUPCR;
                        break;
                    case "DI":
                        strEnable = chldtrckEntity.DIAGNOSEE == string.Empty ? "N" : chldtrckEntity.DIAGNOSEE;
                        strRequire = chldtrckEntity.DIAGNOSER == string.Empty ? "N" : chldtrckEntity.DIAGNOSER;
                        break;
                    case "SP":
                        strEnable = chldtrckEntity.SSRE == string.Empty ? "N" : chldtrckEntity.SSRE;
                        strRequire = chldtrckEntity.SSRR == string.Empty ? "N" : chldtrckEntity.SSRR;
                        break;
                    case "AG":
                        strEnable = chldtrckEntity.WHEREE == string.Empty ? "N" : chldtrckEntity.WHEREE;
                        strRequire = chldtrckEntity.WHERER == string.Empty ? "N" : chldtrckEntity.WHERER;
                        break;
                    case "FD":
                        strEnable = chldtrckEntity.FUNDE == string.Empty ? "N" : chldtrckEntity.FUNDE;
                        strRequire = chldtrckEntity.FUNDR == string.Empty ? "N" : chldtrckEntity.FUNDR;
                        break;

                }
                if (item.Code != "AD")
                    rowIndex = gvTaskConfig.Rows.Add(item.Desc, (strEnable == "Y" ? Img_Tick : Img_Cross), (strRequire == "Y" ? Img_Tick : Img_Cross), strEnable, strRequire, item.Code);


            }
            // }
            //else
            //{
            //    foreach (CommonEntity item in listTask)
            //    {
            //        switch (item.Code)
            //        {
            //            case "QU":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "CA":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "SB":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "AD":
            //                strEnable = "Y";
            //                strRequire = "Y";
            //                rowIndex = gvTaskConfig.Rows.Add(item.Desc, Img_Tick, Img_Tick, "Y", "Y", item.Code);
            //                gvTaskConfig.Rows[rowIndex].Cells["gviEnable"].ReadOnly = true;
            //                gvTaskConfig.Rows[rowIndex].Cells["gviReq"].ReadOnly = true;

            //                break;
            //            case "CO":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "FO":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "FC":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "DI":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "SP":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;
            //            case "AG":
            //                strEnable = "Y";
            //                strRequire = "N";
            //                break;

            //        }
            //        if (item.Code != "AD")
            //            rowIndex = gvTaskConfig.Rows.Add(item.Desc, (strEnable == "Y" ? Img_Tick : Img_Cross), (strRequire == "Y" ? Img_Tick : Img_Cross), strEnable, strRequire, item.Code);

            //    }

            //}
        }
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Cross = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("cross.ico");
        //Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = "icon-gridtick"; // "Resources/Images/tick.ico";
        string Img_Cross = "icon-close?color=Red"; // "Resources/Images/cross.ico";

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegEntity { get; set; }

        public string Mode { get; set; }

        public string propQuestionE { get; set; }
        public string propQuestionR { get; set; }
        public string propCaseNotesE { get; set; }
        public string propCaseNotesR { get; set; }
        public string PropSBCBE { get; set; }
        public string PropSBCBR { get; set; }
        public string PropAddressE { get; set; }
        public string PropAddressR { get; set; }
        public string PropCompletE { get; set; }
        public string PropCompletR { get; set; }
        public string PropFollowupE { get; set; }
        public string PropFollowupR { get; set; }
        public string PropFollowupCE { get; set; }
        public string PropFollowupCR { get; set; }
        public string PropDiagnosedE { get; set; }
        public string PropDiagnosedR { get; set; }
        public string PropSSRE { get; set; }
        public string PropSSRR { get; set; }
        public string PropAgencyRefE { get; set; }
        public string PropAgencyRefR { get; set; }
        public string PropFundE { get; set; }
        public string PropFundR { get; set; }

        private void gvcTaskConfig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvTaskConfig.Rows.Count > 0)
            {

                if (e.ColumnIndex == 1 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvTaskConfig.CurrentRow.Cells["gvtCode"].Value.ToString() != "AD")
                    {
                        if (gvTaskConfig.CurrentRow.Cells["gvcEnable"].Value.ToString() == "Y")
                        {
                            gvTaskConfig.CurrentRow.Cells["gviEnable"].Value = Img_Cross;
                            gvTaskConfig.CurrentRow.Cells["gvcEnable"].Value = "N";
                        }
                        else
                        {
                            gvTaskConfig.CurrentRow.Cells["gviEnable"].Value = Img_Tick;
                            gvTaskConfig.CurrentRow.Cells["gvcEnable"].Value = "Y";
                        }
                    }
                }
                if (e.ColumnIndex == 2 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvTaskConfig.CurrentRow.Cells["gvtCode"].Value.ToString() != "AD")
                    {
                        if (gvTaskConfig.CurrentRow.Cells["gvcRequire"].Value.ToString() == "Y")
                        {
                            gvTaskConfig.CurrentRow.Cells["gviReq"].Value = Img_Cross;
                            gvTaskConfig.CurrentRow.Cells["gvcRequire"].Value = "N";
                        }
                        else
                        {
                            gvTaskConfig.CurrentRow.Cells["gviReq"].Value = Img_Tick;
                            gvTaskConfig.CurrentRow.Cells["gvcRequire"].Value = "Y";
                        }
                    }
                }

            }
        }


        public void FillTaskConfiguration()
        {

            foreach (DataGridViewRow item in gvTaskConfig.Rows)
            {
                switch (item.Cells["gvtCode"].Value.ToString())
                {
                    case "QU":
                        propQuestionE = item.Cells["gvcEnable"].Value.ToString();
                        propQuestionR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "CA":
                        propCaseNotesE = item.Cells["gvcEnable"].Value.ToString();
                        propCaseNotesR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "SB":
                        PropSBCBE = item.Cells["gvcEnable"].Value.ToString();
                        PropSBCBR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "AD":
                        PropAddressE = item.Cells["gvcEnable"].Value.ToString();
                        PropAddressR = item.Cells["gvcRequire"].Value.ToString();

                        break;
                    case "CO":
                        PropCompletE = item.Cells["gvcEnable"].Value.ToString();
                        PropCompletR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "FO":
                        PropFollowupE = item.Cells["gvcEnable"].Value.ToString();
                        PropFollowupR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "FC":
                        PropFollowupCE = item.Cells["gvcEnable"].Value.ToString();
                        PropFollowupCR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "DI":
                        PropDiagnosedE = item.Cells["gvcEnable"].Value.ToString();
                        PropDiagnosedR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "SP":
                        PropSSRE = item.Cells["gvcEnable"].Value.ToString();
                        PropSSRR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "AG":
                        PropAgencyRefE = item.Cells["gvcEnable"].Value.ToString();
                        PropAgencyRefR = item.Cells["gvcRequire"].Value.ToString();
                        break;
                    case "FD":
                        PropFundE = item.Cells["gvcEnable"].Value.ToString();
                        PropFundR = item.Cells["gvcRequire"].Value.ToString();
                        break;


                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}

