#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PreassesDimentionForm : Form
    {
        private CaptainModel _model = null;
        public PreassesDimentionForm(BaseForm baseForm)
        {
            InitializeComponent();
            _model = new CaptainModel();
            propFormType = string.Empty;
            lblPQuestionType.Visible = true;
            cmbQuestionType.Visible = true;
            pnlQuestionFilter.Visible = true;
            //this.gvwPreassesData.Location = new System.Drawing.Point(3, 36);
            //this.gvwPreassesData.Size = new System.Drawing.Size(721, 389);
            BaseForm = baseForm;
            this.Text = "Dimension Form";
            gvtPoints.Visible = false;
            gvtDScore.Visible = false;
            preassessMasterEntity = _model.lookupDataAccess.GetDimension();
            // preassessMasterEntity = _model.FieldControls.GetPreassessData("MASTER");
            preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
            proppreassesQuestions = _model.FieldControls.GetCustomQuestions("PREALL", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");
            cmbQuestionType.SelectedIndexChanged -= new EventHandler(cmbQuestionType_SelectedIndexChanged);
            List<ListItem> listItem = new List<ListItem>();
            cmbQuestionType.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("All", "ALL"));
            listItem.Add(new ListItem("Active", "A"));
            listItem.Add(new ListItem("Inactive", "I"));
            cmbQuestionType.Items.AddRange(listItem.ToArray());
            cmbQuestionType.SelectedIndex = 1;
            cmbQuestionType.SelectedIndexChanged += new EventHandler(cmbQuestionType_SelectedIndexChanged);
            cmbQuestionType_SelectedIndexChanged(cmbQuestionType, new EventArgs());
        }

        public PreassesDimentionForm(BaseForm baseForm, string strType, SalquesEntity _quesEntity)
        {
            InitializeComponent();
            propFormType = "SALLINK";
            _model = new CaptainModel();
            gvtPQDesc.HeaderText = "Description";
            propSalquesEntity = _quesEntity;
            lblPQuestionType.Visible = true;
            cmbQuestionType.Visible = true;
            lblPQuestionType.Visible = false;
            //this.gvwPreassesData.Location = new System.Drawing.Point(1, 1);
            //this.gvwPreassesData.Size = new System.Drawing.Size(721, 425);
            BaseForm = baseForm;
            if (strType == "S")
            {
                this.Text = "SAL Questions Linking";
            }
            else
            {
                this.Text = "CAL Questions Linking";
            }
            gvtPoints.Visible = false;
            gvtDScore.Visible = false;
            cmbQuestionType.Visible = false;
            SalqrespEntity _sqlqrespentity = new SalqrespEntity(true);
            propsalresplist = _model.SALDEFData.Browse_SALQRESP(_sqlqrespentity, "Browse");

            fillSALQuestions();
            // preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
            //proppreassesQuestions = _model.FieldControls.GetCustomQuestions("PREALL", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");

        }

        public List<CommonEntity> preassessMasterEntity { get; set; }
        public List<PreassessQuesEntity> preassessChildEntity { get; set; }
        public List<CustomQuestionsEntity> proppreassesQuestions { get; set; }
        public BaseForm BaseForm { get; set; }
        List<RankCatgEntity> propPreassGroup { get; set; }
        string proppresstotal { get; set; }
        string propStatus { get; set; }
        string propFormType { get; set; }
        SalquesEntity propSalquesEntity { get; set; }
        List<SalqrespEntity> propsalresplist { get; set; }
        List<SalquesEntity> propsalqueslist { get; set; }

        private void contextResult_Popup(object sender, EventArgs e)
        {
            contextResult.MenuItems.Clear();
            if (gvwPreassesData.Rows.Count > 0)
            {
                if (propFormType == string.Empty)
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is CustomQuestionsEntity)
                    {
                        string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString() : string.Empty;
                        if (strDimentionId != string.Empty)
                        {
                            string strQcode = string.Empty;
                            string response = string.Empty;
                            PrivilegeEntity privileges = new PrivilegeEntity();
                            response = gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value.ToString() : string.Empty;
                            strQcode = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString() : string.Empty;
                            privileges.AddPriv = "true";
                            AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strQcode);
                            objform.StartPosition = FormStartPosition.CenterScreen;
                            objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                            objform.StartPosition = FormStartPosition.CenterScreen;
                            objform.ShowDialog();
                        }

                    }
                }
                else
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is SalquesEntity)
                    {
                        SalquesEntity salquesdata = gvwPreassesData.SelectedRows[0].Tag as SalquesEntity;
                        string strQcode = string.Empty;
                        string response = string.Empty;
                        PrivilegeEntity privileges = new PrivilegeEntity();
                        response = gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value.ToString() : string.Empty;
                        strQcode = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString() : string.Empty;
                        privileges.AddPriv = "true";
                        AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strQcode, salquesdata);
                        objform.StartPosition = FormStartPosition.CenterScreen;

                        objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                        objform.StartPosition = FormStartPosition.CenterScreen;
                        objform.ShowDialog();

                    }

                }
            }
        }

        private void contextDimetionId_Popup(object sender, EventArgs e)
        {
            contextDimetionId.MenuItems.Clear();
            if (gvwPreassesData.Rows.Count > 0)
            {
                if (propFormType == string.Empty)
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is CustomQuestionsEntity)
                    {
                        CustomQuestionsEntity drow = gvwPreassesData.SelectedRows[0].Tag as CustomQuestionsEntity;
                        string fieldCode = drow.CUSTCODE.ToString();
                        string fieldType = drow.CUSTRESPTYPE.ToString();

                        string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtDimetionId"].Value == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtDimetionId"].Value.ToString();
                        string strQuestionId = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString();
                        string strDimenQuestionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag.ToString();

                        List<CustomQuestionsEntity> dimentionQuestions = proppreassesQuestions.FindAll(u => u.CUSTCODE != strQuestionId);
                        if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "A")
                        {
                            dimentionQuestions = dimentionQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
                        }
                        if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "I")
                        {
                            dimentionQuestions = dimentionQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");
                        }

                        if (fieldType.Equals("D"))
                        {
                            foreach (CustomQuestionsEntity dr in dimentionQuestions)
                            {


                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = dr.CUSTDESC;
                                menuItem.Tag = dr;
                                if (dr.CUSTCODE == strDimenQuestionId)
                                {
                                    menuItem.Checked = true;
                                }
                                contextDimetionId.MenuItems.Add(menuItem);
                            }
                            MenuItem menuItem1 = new MenuItem();
                            menuItem1.Text = "Clear";
                            menuItem1.Tag = "DQCLR";
                            contextDimetionId.MenuItems.Add(menuItem1);
                        }
                        //}
                        //else
                        //{
                        //    MenuItem menuItem1 = new MenuItem();
                        //    menuItem1.Text = "Dimension Question Not defined";
                        //    menuItem1.Tag = "DQ";
                        //    contextDimetionId.MenuItems.Add(menuItem1);
                        //}
                    }
                }
                else
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is SalquesEntity)
                    {
                        SalquesEntity drow = gvwPreassesData.SelectedRows[0].Tag as SalquesEntity;
                        string fieldCode = drow.SALQ_ID.ToString();
                        string fieldType = drow.SALQ_TYPE.ToString();

                        string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtDimetionId"].Value == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtDimetionId"].Value.ToString();
                        string strQuestionId = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString();
                        string strDimenQuestionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag.ToString();



                        if (fieldType.Equals("D") || fieldType.Equals("C"))
                        {
                            foreach (SalquesEntity dr in propsalqueslist)
                            {


                                MenuItem menuItem = new MenuItem();
                                menuItem.Text = dr.SALQ_DESC;
                                menuItem.Tag = dr;
                                if (dr.SALQ_ID == strDimenQuestionId)
                                {
                                    menuItem.Checked = true;
                                }
                                contextDimetionId.MenuItems.Add(menuItem);
                            }
                            MenuItem menuItem1 = new MenuItem();
                            menuItem1.Text = "Clear";
                            menuItem1.Tag = "DQCLR";
                            contextDimetionId.MenuItems.Add(menuItem1);
                        }
                        //}
                        //else
                        //{
                        //    MenuItem menuItem1 = new MenuItem();
                        //    menuItem1.Text = "Dimension Question Not defined";
                        //    menuItem1.Tag = "DQ";
                        //    contextDimetionId.MenuItems.Add(menuItem1);
                        //}
                    }

                }
            }
        }

        private void gvwPreassessData_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {

            if (propFormType == string.Empty)
            {
                if (objArgs.MenuItem.Tag is CustomQuestionsEntity)
                {

                    string response = gvwPreassesData.SelectedRows[0].Cells[3].Value != null ? gvwPreassesData.SelectedRows[0].Cells[3].Value.ToString() : string.Empty;

                    string responseValue = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString();
                    string responseCode = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag.ToString();
                    CustomQuestionsEntity dr = (CustomQuestionsEntity)objArgs.MenuItem.Tag as CustomQuestionsEntity;

                    string selectedValue = objArgs.MenuItem.Text;
                    string selectedCode = dr.CUSTCODE.ToString();
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

                    gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag = responseCode;
                    gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value = responseValue;
                }

                else if (objArgs.MenuItem.Tag is string)
                {
                    string strQcode = string.Empty;
                    string response = string.Empty;

                    PrivilegeEntity privileges = new PrivilegeEntity();
                    string selectedValue = objArgs.MenuItem.Text;
                    string selectedCode = objArgs.MenuItem.Tag.ToString();

                    if (selectedCode == "DQCLR")
                    {
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value = string.Empty;

                        gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value = string.Empty;
                    }

                }
            }
            else
            {

                if (objArgs.MenuItem.Tag is SalquesEntity)
                {

                    string response = gvwPreassesData.SelectedRows[0].Cells[3].Value != null ? gvwPreassesData.SelectedRows[0].Cells[3].Value.ToString() : string.Empty;

                    string responseValue = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString();
                    string responseCode = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag == null ? string.Empty : gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag.ToString();
                    SalquesEntity dr = (SalquesEntity)objArgs.MenuItem.Tag as SalquesEntity;

                    string selectedValue = objArgs.MenuItem.Text;
                    string selectedCode = dr.SALQ_ID.ToString();
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

                    gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag = responseCode;
                    gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value = responseValue;
                }

                else if (objArgs.MenuItem.Tag is string)
                {
                    string strQcode = string.Empty;
                    string response = string.Empty;

                    PrivilegeEntity privileges = new PrivilegeEntity();
                    string selectedValue = objArgs.MenuItem.Text;
                    string selectedCode = objArgs.MenuItem.Tag.ToString();

                    if (selectedCode == "DQCLR")
                    {
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Tag = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value = string.Empty;

                        gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value = string.Empty;
                        gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value = string.Empty;
                    }

                }
            }
        }
        void objform_FormClosed(object sender, FormClosedEventArgs e)

        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Tag = form.propAlertCode;
                gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Value = form.propAlertCode;
            }

            //AlertCodeForm form = sender as AlertCodeForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    gvwPreassesData.SelectedRows[0].Cells["gvtPDRequire"].Tag = form.propAlertCode;

            //    string custQuestionResp = string.Empty;
            //    List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", form.propFieldCode);
            //    if (custReponseEntity.Count > 0)
            //    {
            //        string response1 = form.propAlertCode;
            //        string[] arrResponse = null;
            //        if (response1.IndexOf(',') > 0)
            //        {
            //            arrResponse = response1.Split(',');
            //        }
            //        else if (!response1.Equals(string.Empty))
            //        {
            //            arrResponse = new string[] { response1 };
            //        }
            //        foreach (string stringitem in arrResponse)
            //        {

            //            CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
            //            if (custRespEntity != null)
            //            {
            //                custQuestionResp += custRespEntity.RespDesc + ", ";
            //            }
            //        }
            //    }

            //    gvwPreassesData.SelectedRows[0].Cells[3].Value = custQuestionResp;
            //}

        }

        void objEnableform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Tag = form.propAlertCode;
                gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value = form.propAlertCode;
            }

        }

        void objDisableform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Tag = form.propAlertCode;
                gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value = form.propAlertCode;
            }


        }


        #region PreassesDatafilling
        string strPreassGroup = string.Empty;
        private void fillPreassCustomQuestions()
        {

            bool isResponse = false;

            gvwPreassesData.Rows.Clear();

            List<CommonEntity> propPreassMasterData = preassessMasterEntity;
            List<CustomQuestionsEntity> custQuestions = proppreassesQuestions;
            if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "A")
            {
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
            }
            if (((ListItem)cmbQuestionType.SelectedItem).Value.ToString().ToUpper() == "I")
            {
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");
                //propPreassMasterData = preassessMasterEntity.FindAll(u => u.Active.ToUpper() == "N");
            }


            if (custQuestions.Count > 0)
            {
                foreach (CommonEntity preassesdata in propPreassMasterData)
                {
                    int rowIndex = gvwPreassesData.Rows.Add(preassesdata.Desc, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, preassesdata.Code);
                    gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    List<PreassessQuesEntity> preassessChildList = preassessChildEntity.FindAll(u => u.PRECHILD_DID == preassesdata.Code);
                    preassessChildList = preassessChildList.OrderBy(u => Convert.ToInt32(u.PRECHILD_SEQ)).ToList();
                    foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                    {
                        CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID && preasschilddata.PRECHILD_DID == preassesdata.Code);
                        if (dr != null)
                        {

                            string custCode = dr.CUSTCODE.ToString();

                            string strDQDesc = string.Empty;
                            CustomQuestionsEntity dqdesc = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_DQID);
                            if (dqdesc != null)
                            {
                                strDQDesc = dqdesc.CUSTDESC;
                            }
                            rowIndex = gvwPreassesData.Rows.Add(dr.CUSTDESC, strDQDesc, preasschilddata.PRECHILD_ENABLE, preasschilddata.PRECHILD_DISABLE, preasschilddata.PRECHILD_REQ, preasschilddata.PRECHILD_QID, preassesdata.Code);
                            gvwPreassesData.Rows[rowIndex].Cells["gvtPQDesc"].ToolTipText = dr.CUSTDESC;
                            if (preasschilddata.PRECHILD_DQID != string.Empty)
                            {
                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDQid"].Tag = preasschilddata.PRECHILD_DQID;
                            }

                            gvwPreassesData.Rows[rowIndex].Cells["gvtPDRequire"].Tag = "REQUIRE";
                            gvwPreassesData.Rows[rowIndex].Cells["gvtPEnable"].Tag = "ENABLE";
                            gvwPreassesData.Rows[rowIndex].Cells["gvtPDisable"].Tag = "DISABLE";


                            gvwPreassesData.Rows[rowIndex].Tag = dr;

                            string fieldType = dr.CUSTRESPTYPE.ToString();

                            string custQuestionResp = string.Empty;
                            string custQuestionCode = string.Empty;

                            //isResponse = true;
                            //if (fieldType.Equals("D"))
                            //{
                            //    if (preasschilddata.PRECHILD_REQ != string.Empty)
                            //    {
                            //        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("PREASSES", preasschilddata.PRECHILD_QID);

                            //        CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(preasschilddata.PRECHILD_REQ));
                            //        if (custRespEntity != null)
                            //        {
                            //            gvwPreassesData.Rows[rowIndex].Cells["gvtPDRequire"].Tag = preasschilddata.PRECHILD_REQ;
                            //            gvwPreassesData.Rows[rowIndex].Cells["gvtPDRequire"].Value = custRespEntity.RespDesc;
                            //        }
                            //    }

                            //}
                            //else if (fieldType.Equals("C"))
                            //{
                            //    if (preasschilddata.PRECHILD_REQ != string.Empty)
                            //    {
                            //        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("PREASSES", preasschilddata.PRECHILD_QID);
                            //        CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(preasschilddata.PRECHILD_REQ));
                            //        if (custRespEntity != null)
                            //        {
                            //            gvwPreassesData.Rows[rowIndex].Cells["gvtPDRequire"].Tag = preasschilddata.PRECHILD_REQ;
                            //            gvwPreassesData.Rows[rowIndex].Cells["gvtPDRequire"].Value = custRespEntity.RespDesc;
                            //        }
                            //    }
                            //}

                            if (!dr.CUSTACTIVECUST.Equals("A"))
                                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;

                        }
                    }
                }
                gvwPreassesData.Update();
                if (gvwPreassesData.Rows.Count > 0)
                {
                        gvwPreassesData.Rows[0].Selected = true;
                        gvwPreassesData.CurrentCell = gvwPreassesData.Rows[0].Cells[1];
                }
            }
        }

        private void fillSALQuestions()
        {

            bool isResponse = false;

            gvwPreassesData.Rows.Clear();




            if (propSalquesEntity != null)
            {
                SalquesEntity _sqlquesEntity = new SalquesEntity(true);
                _sqlquesEntity.SALQ_SALD_ID = propSalquesEntity.SALQ_SALD_ID;
                _sqlquesEntity.SALQ_GRP_CODE = propSalquesEntity.SALQ_GRP_CODE;
                List<SalquesEntity> salquesdata = _model.SALDEFData.Browse_SALQUES(_sqlquesEntity, "Browse");
                salquesdata = salquesdata.FindAll(u => u.SALQ_SEQ != "0");
                propsalqueslist = salquesdata;
                salquesdata = salquesdata.OrderBy(u => u.SALQ_DESC).OrderBy(u => Convert.ToInt32(u.SALQ_SEQ)).ToList();
                int rowIndex = gvwPreassesData.Rows.Add(propSalquesEntity.SALQ_DESC, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, propSalquesEntity.SALQ_ID);
                gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                List<SALQLNKEntity> salQlinkEntitylist = _model.SALDEFData.Browse_SALQLNK(string.Empty, propSalquesEntity.SALQ_ID, string.Empty);
                foreach (SalquesEntity quesdata in salquesdata)
                {
                    if (quesdata.SALQ_TYPE == "D" || quesdata.SALQ_TYPE == "C")
                    {
                        string custCode = string.Empty;
                        string strDQDesc = string.Empty;
                        SALQLNKEntity dr = salQlinkEntitylist.Find(u => u.SALQL_GROUP == propSalquesEntity.SALQ_ID && u.SALQL_Q_ID == quesdata.SALQ_ID);
                        if (dr != null)
                        {

                            custCode = dr.SALQL_LINKQ.ToString();


                            SalquesEntity dqdesc = salquesdata.Find(u => u.SALQ_ID == dr.SALQL_LINKQ.ToString());
                            if (dqdesc != null)
                            {
                                strDQDesc = dqdesc.SALQ_DESC;
                            }

                            rowIndex = gvwPreassesData.Rows.Add(quesdata.SALQ_DESC, strDQDesc, dr.SALQL_ENABLE, dr.SALQL_DISABLE, dr.SALQL_REQ, dr.SALQL_Q_ID, propSalquesEntity.SALQ_ID);
                            gvwPreassesData.Rows[rowIndex].Cells["gvtPQDesc"].ToolTipText = quesdata.SALQ_DESC;
                            if (dr.SALQL_LINKQ != string.Empty)
                            {
                                gvwPreassesData.Rows[rowIndex].Cells["gvtPDQid"].Tag = dr.SALQL_LINKQ;
                            }
                            CommonFunctions.setTooltip(rowIndex, dr.SALQL_LSTC_OPERATOR, dr.SALQL_DATE_LSTC, gvwPreassesData);
                        }
                        else
                        {
                            rowIndex = gvwPreassesData.Rows.Add(quesdata.SALQ_DESC, strDQDesc, string.Empty, string.Empty, string.Empty, quesdata.SALQ_ID, propSalquesEntity.SALQ_ID);
                        }
                        gvwPreassesData.Rows[rowIndex].Cells["gvtPDRequire"].Tag = "REQUIRE";
                        gvwPreassesData.Rows[rowIndex].Cells["gvtPEnable"].Tag = "ENABLE";
                        gvwPreassesData.Rows[rowIndex].Cells["gvtPDisable"].Tag = "DISABLE";


                        gvwPreassesData.Rows[rowIndex].Tag = quesdata;

                        // string fieldType = dr.CUSTRESPTYPE.ToString();

                        string custQuestionResp = string.Empty;
                        string custQuestionCode = string.Empty;


                        // }
                    }
                }
            }
            gvwPreassesData.Update();
            if(gvwPreassesData.Rows.Count>0)
                gvwPreassesData.Rows[0].Selected = true;
        }



        #endregion

        private void Btn_Save_Click(object sender, EventArgs e)
        {

            if (propFormType == string.Empty)
            {
                foreach (DataGridViewRow gvrow in gvwPreassesData.Rows)
                {

                    if (gvrow.Tag is CustomQuestionsEntity)
                    {
                        string strDimention = gvrow.Cells["gvtDimetionId"].Value == null ? string.Empty : gvrow.Cells["gvtDimetionId"].Value.ToString();
                        string strDQId = gvrow.Cells["gvtPDQid"].Tag == null ? string.Empty : gvrow.Cells["gvtPDQid"].Tag.ToString();
                        string strResult = gvrow.Cells["gvtPDRequire"].Value == null ? string.Empty : gvrow.Cells["gvtPDRequire"].Value.ToString();
                        string strQuestion = gvrow.Cells["gvtPQid"].Value == null ? string.Empty : gvrow.Cells["gvtPQid"].Value.ToString();
                        string strEnable = gvrow.Cells["gvtPEnable"].Value == null ? string.Empty : gvrow.Cells["gvtPEnable"].Value.ToString();
                        string strDisable = gvrow.Cells["gvtPDisable"].Value == null ? string.Empty : gvrow.Cells["gvtPDisable"].Value.ToString();

                        _model.FieldControls.InsertUpdatePrechild(strQuestion, strDimention, strDQId, strResult, string.Empty, strEnable, strDisable, string.Empty);
                        preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);

                    }
                }

            }
            else
            {
                _model.SALDEFData.CAP_SALQLNK_INSUPDEL("Delete", string.Empty, propSalquesEntity.SALQ_ID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                foreach (DataGridViewRow gvrow in gvwPreassesData.Rows)
                {
                    if (gvrow.Tag is SalquesEntity)
                    {
                        string strDimention = gvrow.Cells["gvtDimetionId"].Value == null ? string.Empty : gvrow.Cells["gvtDimetionId"].Value.ToString();
                        string strDQId = gvrow.Cells["gvtPDQid"].Tag == null ? string.Empty : gvrow.Cells["gvtPDQid"].Tag.ToString();
                        string strResult = gvrow.Cells["gvtPDRequire"].Value == null ? string.Empty : gvrow.Cells["gvtPDRequire"].Value.ToString();
                        string strQuestion = gvrow.Cells["gvtPQid"].Value == null ? string.Empty : gvrow.Cells["gvtPQid"].Value.ToString();
                        string strEnable = gvrow.Cells["gvtPEnable"].Value == null ? string.Empty : gvrow.Cells["gvtPEnable"].Value.ToString();
                        string strDisable = gvrow.Cells["gvtPDisable"].Value == null ? string.Empty : gvrow.Cells["gvtPDisable"].Value.ToString();


                        _model.SALDEFData.CAP_SALQLNK_INSUPDEL("Add", strQuestion, strDimention, strDQId, strResult,  strEnable, strDisable, BaseForm.UserID);
                    }
                }

                //ADMN0022Control ADMN0022Control = BaseForm.GetBaseUserControl() as ADMN0022Control;
                //if (ADMN0022Control != null)
                //{
                //    ADMN0022Control.Refresh("G");

                //}
            }
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextDisable_Popup(object sender, EventArgs e)
        {
            if (gvwPreassesData.Rows.Count > 0)
            {
                if (propFormType == string.Empty)
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is CustomQuestionsEntity)
                    {

                        string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString() : string.Empty;
                        if (strDimentionId != string.Empty)
                        {
                            string strQcode = string.Empty;
                            string response = string.Empty;
                            PrivilegeEntity privileges = new PrivilegeEntity();
                            response = gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value.ToString() : string.Empty;
                            strQcode = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString() : string.Empty;

                            privileges.AddPriv = "true";
                            AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strQcode);
                            objform.StartPosition = FormStartPosition.CenterScreen;
                            objform.FormClosed += new FormClosedEventHandler(objDisableform_FormClosed);
                            objform.ShowDialog();
                        }
                    }
                }
                else
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is SalquesEntity)
                    {

                        //string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString() : string.Empty;
                        //if (strDimentionId != string.Empty)
                        //{
                        SalquesEntity salquesdata = gvwPreassesData.SelectedRows[0].Tag as SalquesEntity;
                        string strQcode = string.Empty;
                        string response = string.Empty;
                        PrivilegeEntity privileges = new PrivilegeEntity();
                        response = gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDisable"].Value.ToString() : string.Empty;
                        strQcode = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString() : string.Empty;

                        privileges.AddPriv = "true";
                        AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strQcode, salquesdata);
                        objform.StartPosition = FormStartPosition.CenterScreen;

                        objform.FormClosed += new FormClosedEventHandler(objDisableform_FormClosed);
                        objform.StartPosition = FormStartPosition.CenterScreen;
                        objform.ShowDialog();
                        // }
                    }
                }
            }
        }

        private void contextEnable_Popup(object sender, EventArgs e)
        {
            if (gvwPreassesData.Rows.Count > 0)
            {
                if (propFormType == string.Empty)
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is CustomQuestionsEntity)
                    {

                        string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString() : string.Empty;
                        if (strDimentionId != string.Empty)
                        {
                            string strQcode = string.Empty;
                            string response = string.Empty;
                            PrivilegeEntity privileges = new PrivilegeEntity();

                            response = gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value.ToString() : string.Empty;
                            strQcode = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString() : string.Empty;

                            privileges.AddPriv = "true";
                            AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strQcode);
                            objform.StartPosition = FormStartPosition.CenterScreen;

                            objform.FormClosed += new FormClosedEventHandler(objEnableform_FormClosed);
                            objform.ShowDialog();
                        }
                    }
                }
                else
                {
                    if (gvwPreassesData.SelectedRows[0].Tag is SalquesEntity)
                    {

                        //string strDimentionId = gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPDQid"].Value.ToString() : string.Empty;
                        //if (strDimentionId != string.Empty)
                        //{
                        SalquesEntity salquesdata = gvwPreassesData.SelectedRows[0].Tag as SalquesEntity;
                        string strQcode = string.Empty;
                        string response = string.Empty;
                        PrivilegeEntity privileges = new PrivilegeEntity();

                        response = gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPEnable"].Value.ToString() : string.Empty;
                        strQcode = gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value != null ? gvwPreassesData.SelectedRows[0].Cells["gvtPQid"].Value.ToString() : string.Empty;

                        privileges.AddPriv = "true";
                        AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, strQcode, salquesdata);
                        objform.StartPosition = FormStartPosition.CenterScreen;

                        objform.FormClosed += new FormClosedEventHandler(objEnableform_FormClosed);
                        objform.StartPosition = FormStartPosition.CenterScreen;
                        objform.ShowDialog();
                        //  }
                    }

                }
            }
        }


        public PreassesDimentionForm(BaseForm baseForm, string strFormType, string strStatus, List<CustomQuestionsEntity> custresponceall)
        {
            InitializeComponent();
            _model = new CaptainModel();
            propFormType = string.Empty;
            lblPQuestionType.Visible = false;
            cmbQuestionType.Visible = false;
            pnlQuestionFilter.Visible = false;
            cmbQuestionType.Enabled = false;
            BaseForm = baseForm;
            //this.panel1.Size = new System.Drawing.Size(470, 431);
            //this.pnlGvwPreassesData.Location = new Point(0, 0);
            //this.gvwPreassesData.Location = new System.Drawing.Point(0, 0);
            Size = new Size(875, 462);

            //this.panel2.Size = new System.Drawing.Size(465, 32);
            //this.Btn_Cancel.Location = new System.Drawing.Point(4, 3);
            gvtPQDesc.Width = 320;
            gvtDimetionId.Visible = false;
            gvtPDisable.Visible = false;
            gvtPDQid.Visible = false;
            gvtPDRequire.Visible = false;
            gvtPEnable.Visible = false;
            gvtPQid.Visible = false;
            Btn_Save.Visible = false;
            lblPresTotal.Visible = true;
            lblCategory.Visible = true;
            proppresstotal = (BaseForm.BaseCaseMstListEntity[0].PressTotal.ToString() == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].PressTotal.ToString());
            lblPresTotal.Text = "Preasses Total : " + proppresstotal;
            gvwPreassesData.ReadOnly = true;
            this.Text = "Pre-Assessment Score";

            preassessMasterEntity = _model.lookupDataAccess.GetDimension();
            // preassessMasterEntity = _model.FieldControls.GetPreassessData("MASTER");
            preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
            proppreassesQuestions = _model.FieldControls.GetPreassesQuestions("PREASSES", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");//_model.FieldControls.GetCustomQuestions("PREALL", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");
            propPreassGroup = _model.SPAdminData.Browse_PreassGroups();
            fillPreassPoints(strFormType, strStatus, custresponceall);
        }

        private void fillPreassPoints(string strFormType, string strStatus, List<CustomQuestionsEntity> custresponceall)
        {

            bool isResponse = false;

            gvwPreassesData.Rows.Clear();

            int intDIndex;
            decimal intDScore, intTotDScore = 0;

            List<CustomQuestionsEntity> custQuestions = proppreassesQuestions;
            if (strStatus == "A")
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
            if (strStatus == "I")
                custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");


            if (custQuestions.Count > 0)
            {
                intDIndex = -1;
                string strDpoint = string.Empty;
                List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
                List<CustomQuestionsEntity> customdimension = _model.CaseMstData.GETDIMSCORE(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty, string.Empty);
                if (strFormType == string.Empty)
                {
                    custResponses = _model.CaseMstData.GetPreassesQuestionAnswers(BaseForm.BaseCaseSnpEntity[0], "PRESRESP");
                }
                else
                {
                    custResponses = custresponceall;
                }
                foreach (CommonEntity preassesdata in preassessMasterEntity)
                {
                    strDpoint = string.Empty;
                    intDScore = 0;
                    intDIndex = 99999;
                    CustomQuestionsEntity custdimdata = customdimension.Find(u => u.ACTCODE.Trim() == preassesdata.Code.Trim());
                    if (custdimdata != null)
                        strDpoint = custdimdata.PRESPOINTS;
                    List<PreassessQuesEntity> preassessChildList = preassessChildEntity.FindAll(u => u.PRECHILD_DID == preassesdata.Code);

                    bool boolQuestions = false;
                    foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                    {
                        CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
                        if (dr != null)
                        {
                            CustomQuestionsEntity custResppoint = custResponses.Find(u => u.ACTCODE.Trim() == preasschilddata.PRECHILD_QID.Trim());
                            if (custResppoint != null)
                            {
                                boolQuestions = true;
                                break;
                            }
                        }
                    }
                    if (boolQuestions)
                    {
                        int rowIndex = gvwPreassesData.Rows.Add(preassesdata.Desc, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, preassesdata.Code, string.Empty, strDpoint);
                        intDIndex = rowIndex;
                        gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        preassessChildList = preassessChildList.OrderBy(u => Convert.ToInt32(u.PRECHILD_SEQ)).ToList();
                        foreach (PreassessQuesEntity preasschilddata in preassessChildList)
                        {
                            strDpoint = string.Empty;
                            CustomQuestionsEntity dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID && preasschilddata.PRECHILD_DID == preassesdata.Code);
                            if (dr != null)
                            {

                                string custCode = dr.CUSTCODE.ToString();

                                string strDQDesc = string.Empty;
                                CustomQuestionsEntity dqdesc = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_DQID);
                                if (dqdesc != null)
                                {
                                    strDQDesc = dqdesc.CUSTDESC;
                                }

                                CustomQuestionsEntity custResppoint = custResponses.Find(u => u.ACTCODE.Trim() == preasschilddata.PRECHILD_QID.Trim());
                                if (custResppoint != null)
                                {

                                    strDpoint = custResppoint.PRESPOINTS == null ? string.Empty : custResppoint.PRESPOINTS.ToString();

                                    if (strPreassGroup == string.Empty)
                                    {
                                        strPreassGroup = dr.CUSTOTHER.ToString();
                                    }

                                    rowIndex = gvwPreassesData.Rows.Add(dr.CUSTDESC, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, preassesdata.Code, strDpoint, string.Empty);
                                    if (strDpoint != string.Empty)
                                    {
                                        intDScore = intDScore + Convert.ToDecimal(strDpoint);
                                    }
                                    gvwPreassesData.Rows[rowIndex].Cells["gvtPQDesc"].ToolTipText = dr.CUSTDESC;
                                    if (!dr.CUSTACTIVECUST.Equals("A"))
                                        gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                }

                            }
                        }
                    }
                    if (strFormType != string.Empty)
                    {
                        intTotDScore = intTotDScore + intDScore;
                        if (gvwPreassesData.Rows.Count > 0)
                        {
                            if (intDIndex != 99999)
                                gvwPreassesData.Rows[intDIndex].Cells["gvtDScore"].Value = intDScore.ToString();
                        }
                    }
                }
                gvwPreassesData.Update();
            }
            if (strFormType != string.Empty)
            {
                lblPresTotal.Text = "Preasses Total : " + (intTotDScore.ToString());
                List<RankCatgEntity> pressgroupcategorylist = propPreassGroup.FindAll(u => u.Code.Trim() == strPreassGroup.Trim() && u.SubCode.Trim() != string.Empty && u.PointsLow != string.Empty && u.PointsHigh != string.Empty);
                RankCatgEntity preasscategory = pressgroupcategorylist.Find(u => (Convert.ToDecimal(u.PointsLow) <= Convert.ToDecimal(intTotDScore)) && (Convert.ToDecimal(u.PointsHigh) >= Convert.ToDecimal(intTotDScore)));
                if (preasscategory != null)
                {
                    lblCategory.Text = "Category : " + preasscategory.Desc.ToString().Trim();
                }
            }
            else
            {
                List<RankCatgEntity> pressgroupcategorylist = propPreassGroup.FindAll(u => u.Code.Trim() == BaseForm.BaseCaseMstListEntity[0].PressGrp && u.SubCode.Trim() != string.Empty && u.PointsLow != string.Empty && u.PointsHigh != string.Empty);
                RankCatgEntity preasscategory = pressgroupcategorylist.Find(u => (Convert.ToDecimal(u.PointsLow) <= Convert.ToDecimal(proppresstotal)) && (Convert.ToDecimal(u.PointsHigh) >= Convert.ToDecimal(proppresstotal)));
                if (preasscategory != null)
                {
                    lblCategory.Text = "Category : " + preasscategory.Desc.ToString().Trim();
                }
            }


        }

        private void cmbQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbQuestionType.Items.Count > 0)
            {
                fillPreassCustomQuestions();
            }
        }
    }

}
