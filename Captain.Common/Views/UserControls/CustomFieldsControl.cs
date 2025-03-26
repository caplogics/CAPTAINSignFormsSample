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
using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CustomFieldsControl : UserControl
    {
        //ProgramHeaderControl _checkedListBoxControl = null;
        private CaptainModel _model = null;
        private List<CustomQuestionsandAnswers> _customQuestionsandAnswers = null;

        public CustomFieldsControl(CaseSnpEntity caseSNP, string screen, string mode)
        {
            InitializeComponent();
            GridViewControl = gvwCustomQuestions;
            MaxButtonControl = picMax;
            Screen = screen;
            Mode = mode;
            CaseSnpEntity = caseSNP;
            _model = new CaptainModel();
            getFLDCNTLHIE(screen, string.Empty);
            //contextMenu1 = new ContextMenu();
            //gvwCustomQuestions.ContextMenu = contextMenu1;
            fillCustomDropdowns();
            fillCustomQuestions(screen, "A", ((ListItem)cmbSEQ.SelectedItem).Text, ((ListItem)cmbQuestionType.SelectedItem).Text, ((ListItem)cmbQuestionAccess.SelectedItem).Text);
            if(!Mode.Equals(Consts.Common.View))
            {
                contextMenu2.Popup -= new EventHandler(contextMenu1_Popup);
                contextMenu2.Popup += new EventHandler(contextMenu1_Popup);
            }
        }
        
        public CustomFieldsControl(BaseForm BasePage, string hieType, UserEntity userProfile)
        {
            InitializeComponent();
            BasePage = BasePage;
            UserProfile = userProfile;
            HieType = hieType;
            GridViewControl = gvwCustomQuestions;
            MaxButtonControl = picMax;
            //contextMenu1 = new ContextMenu();
            //gvwCustomQuestions.ContextMenu = contextMenu1;
            fillCustomDropdowns();
            fillCustomQuestions(Screen, "A", ((ListItem)cmbSEQ.SelectedItem).Text, ((ListItem)cmbQuestionType.SelectedItem).Text, ((ListItem)cmbQuestionAccess.SelectedItem).Text);
        }

        #region Public Properties

        public UserEntity UserProfile { get; set; }

        public BaseForm BasePage { get; set; }

        public string HieType { get; set; }

        public bool IsMax { get; set; }

        public string Screen { get; set; }

        public string Mode { get; set; }

        public bool IsApplicant { get; set; }

        public CaseSnpEntity CaseSnpEntity { get; set; }

        public List<FldcntlHieEntity> FLDCNTLHIEEntity { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView GridViewControl
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PictureBox MaxButtonControl
        {
            get;
            set;
        }

        #endregion

        private void gvwCustomQuestions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Nothing here
        }

        private void fillCustomDropdowns()
        {
            cmbSEQ.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Description"));
            listItem.Add(new ListItem("Sequence"));
            listItem.Add(new ListItem("Required Questions"));
            cmbSEQ.Items.AddRange(listItem.ToArray());
            cmbSEQ.SelectedIndex = 1;

            cmbQuestionType.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("All"));
            listItem.Add(new ListItem("Active"));
            listItem.Add(new ListItem("InActive"));
            cmbQuestionType.Items.AddRange(listItem.ToArray());
            cmbQuestionType.SelectedIndex = 1;

            cmbQuestionAccess.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("All"));
            listItem.Add(new ListItem("Show Question for this Program"));
            listItem.Add(new ListItem("Eligibility Criteria Specific"));
            listItem.Add(new ListItem("Not Eligibility Criteria Specific"));
            cmbQuestionAccess.Items.AddRange(listItem.ToArray());
            cmbQuestionAccess.SelectedIndex = 0;
        }

        private void fillCustomQuestions(string screen, string memAccess, string seq, string questionType, string questionAccess)
        {
            List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions(screen, memAccess, HIE: null,seq,questionType,questionAccess) ;
            List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetCustomQuestionAnswers(CaseSnpEntity);
            bool isResponse = false;
            if (custQuestions.Count > 0)
            {
                _customQuestionsandAnswers = new List<CustomQuestionsandAnswers>();
                foreach (CustomQuestionsEntity dr in custQuestions)
                {
                    string custCode = dr.CUSTCODE.ToString();
                    
                    int rowIndex = gvwCustomQuestions.Rows.Add(string.Empty, dr.CUSTDESC, string.Empty);
                    gvwCustomQuestions.Rows[rowIndex].Tag = dr;
                    
                    string fieldType = dr.CUSTRESPTYPE.ToString();
                    if (fieldType.Equals("D"))
                    {
                        gvwCustomQuestions.Rows[rowIndex].ReadOnly = true;                        
                    }
                    else
                    {
                        gvwCustomQuestions.Rows[rowIndex].Cells[0].ReadOnly = true;
                        gvwCustomQuestions.Rows[rowIndex].Cells[1].ReadOnly = true;
                    }

                    string custQuestionResp = string.Empty;
                    string custQuestionCode = string.Empty;
                    if (!Mode.Equals(Consts.Common.Add))
                    {
                        List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                        if(response != null)
                        {
                            isResponse = true;
                            if (fieldType.Equals("D"))
                            {
                                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", response[0].CUSTCODE);
                  
                                foreach (CustomQuestionsEntity custResp in response)
                                {
                                    custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                    custQuestionResp += custReponseEntity.Find(u => u.ResoCode.Equals(custResp.ACTMULTRESP.ToString())).RespDesc;
                                }
                                gvwCustomQuestions.Rows[rowIndex].Cells[3].Value = custQuestionCode;
                                gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = custQuestionResp;
                            }
                            else if (fieldType.Equals("N"))
                            {
                                custQuestionResp = response[0].ACTNUMRESP.ToString();
                            }
                            else
                            {
                                custQuestionResp = response[0].ACTALPHARESP.ToString();
                            }
                            gvwCustomQuestions.Rows[rowIndex].Cells[2].Value = custQuestionResp;
                        }
                    }
                    bool isApplicant = dr.CUSTMEMACCESS.Equals("A") ? true : false;
                    _customQuestionsandAnswers.Add(new CustomQuestionsandAnswers(isResponse? "U":"I",dr.CUSTCODE, dr.CUSTDESC,string.Empty,string.Empty,string.Empty,string.Empty, isApplicant, custQuestionResp, custQuestionCode));
                }
            }
        }

        private void getFLDCNTLHIE(string screen,string HIE)
        {
            //FLDCNTLHIEEntity = _model.FieldControls.GetFLDCNTLHIE(screen, HIE);
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu2.MenuItems.Clear();
            if (gvwCustomQuestions.Rows.Count > 0)
            {
                if (gvwCustomQuestions.Rows[0].Tag is CustomQuestionsEntity)
                {
                    CustomQuestionsEntity drow = gvwCustomQuestions.SelectedRows[0].Tag as CustomQuestionsEntity;
                    string fieldCode = drow.CUSTCODE.ToString();
                    string fieldType = drow.CUSTRESPTYPE.ToString();

                    if (fieldType.Equals("D"))
                    {
                        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", fieldCode);
                        if (custReponseEntity.Count > 0)
                        {
                            string response = gvwCustomQuestions.SelectedRows[0].Cells[2].Value != null ? gvwCustomQuestions.SelectedRows[0].Cells[2].Value.ToString() : string.Empty;
                            string[] arrResponse = null;
                            if (response.IndexOf(',') > 0)
                            {
                                arrResponse = response.Split(',');
                            }
                            else if (!response.Equals(string.Empty))
                            {
                                arrResponse = new string[] { response };
                            }

                            foreach (CustRespEntity dr in custReponseEntity)
                            {
                                string resDesc = dr.RespDesc.ToString().Trim();
                           
                                    MenuItem menuItem = new MenuItem();
                                    menuItem.Text = resDesc;
                                    menuItem.Tag = dr;
                                    if (arrResponse != null && arrResponse.ToList().Exists(u=>u.Equals(resDesc)))
                                    {
                                        menuItem.Checked = true;
                                    }
                                    contextMenu2.MenuItems.Add(menuItem);
                            }
                         }
                    }
                    else if (fieldType.Equals("A"))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Please enter text here";
                        menuItem.Tag = "A";
                        contextMenu2.MenuItems.Add(menuItem);
                        gvwCustomQuestions.Rows[0].Cells[2].ReadOnly= false;
                    }
                    else if (fieldType.Equals("X"))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Please enter Date here";
                        menuItem.Tag = "X";
                        contextMenu2.MenuItems.Add(menuItem);
                        gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                    }
                    else if (fieldType.Equals("N"))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Text = "Please enter number here";
                        menuItem.Tag = "N";
                        contextMenu2.MenuItems.Add(menuItem);
                        gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                    }
                }
            }
        }

        private void gvwCustomQuestions_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is CustRespEntity)
            {
                string responseValue = gvwCustomQuestions.SelectedRows[0].Cells[2].Value.ToString();
                string responseCode = gvwCustomQuestions.SelectedRows[0].Cells[3].Value.ToString();
                CustRespEntity dr = (CustRespEntity)objArgs.MenuItem.Tag as CustRespEntity;
                
                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = dr.ResoCode.ToString();
                if (objArgs.MenuItem.Checked)
                {
                    responseValue = responseValue.Replace(selectedValue + Consts.Common.Comma, string.Empty);
                    responseValue = responseValue.Replace(selectedValue, string.Empty);
                    responseCode = responseCode.Replace(selectedCode + Consts.Common.Comma, string.Empty);
                    responseCode = responseCode.Replace(selectedCode, string.Empty);
                    gvwCustomQuestions.SelectedRows[0].Cells[2].Value = responseValue;
                    gvwCustomQuestions.SelectedRows[0].Cells[3].Value = responseCode;
                }
                else
                {
                    if (!responseValue.Equals(string.Empty)) responseValue += ",";
                    if (!responseCode.Equals(string.Empty)) responseCode += ",";
                    responseValue += selectedValue;
                    responseCode += selectedCode;
                    gvwCustomQuestions.SelectedRows[0].Cells[2].Value = responseValue;
                    gvwCustomQuestions.SelectedRows[0].Cells[3].Value = responseCode;
                }
               string custCode= ((CustomQuestionsEntity)gvwCustomQuestions.SelectedRows[0].Tag).CUSTCODE;
               _customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseValue = responseValue);
               _customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseCode = responseCode);
            }
            else
            {
                gvwCustomQuestions.Rows[0].Cells[2].ReadOnly = false;
                gvwCustomQuestions.Rows[0].Cells[2].Selected = true;
            }
        }

        private void OnSequenceSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OnTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in gvwCustomQuestions.Rows)
            {
                
            }
        }

        private void OnAccessSelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }

}