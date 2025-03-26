#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class CustQuesMixedControl : UserControl
    {

        private CaptainModel _model = null;
        private List<CustomQuestionsandAnswers> _customQuestionsandAnswers = null;
        private List<FldcntlHieEntity> _custFLDCNTLEntity = new List<FldcntlHieEntity>();
        private List<CommonEntity> _custCASEELIGEntity = new List<CommonEntity>();

        public CustQuesMixedControl(BaseForm baseForm, CaseSnpEntity caseSNP, string screen, bool isApp, string mode, ProgramDefinitionEntity programEntity)
        {
            InitializeComponent();
            GridViewControl = gvwCustomQuestions;
            MaxButtonControl = picMax;
            Screen = screen;
            Mode = mode;
            BaseForm = baseForm;
            CaseSnpEntity = caseSNP;
            IsApplicant = isApp;
            HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            _model = new CaptainModel();

            AccessLevel = IsApplicant ? "A" : "*";

            fillCustomDropdowns(AccessLevel);
            if (programEntity != null)
            {
                SetComboBoxValue(cmbQuestionAccess, programEntity.LoadProgramQuestions+"_"+ AccessLevel);
            }

            fillCustomQuestions(screen, AccessLevel, "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[0].ToString(), HIE);
            cmbSEQ.SelectedIndexChanged -= OnSequenceSelectedIndexChanged;
            cmbSEQ.SelectedIndexChanged += OnSequenceSelectedIndexChanged;
            cmbQuestionType.SelectedIndexChanged -= OnTypeSelectedIndexChanged;
            cmbQuestionType.SelectedIndexChanged += OnTypeSelectedIndexChanged;
            if (!Mode.Equals(Consts.Common.View))
            {
                // contextMenu2.Popup -= new EventHandler(contextMenu1_Popup);
                //contextMenu2.Popup += new EventHandler(contextMenu1_Popup);
            }
            else
            {
                gvwCustomQuestions.ReadOnly = true;
            }
            //gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            //gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        }

        string IsHousehold=string.Empty;
        public CustQuesMixedControl(BaseForm baseForm, CaseSnpEntity caseSNP, string screen, bool isApp, string mode, ProgramDefinitionEntity programEntity,string household)
        {
            InitializeComponent();
            GridViewControl = gvwCustomQuestions;
            MaxButtonControl = picMax;
            Screen = screen;
            Mode = mode;
            BaseForm = baseForm;
            CaseSnpEntity = caseSNP;
            IsApplicant = isApp;
            HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            _model = new CaptainModel();

            AccessLevel = IsApplicant ? "A" : "*";

            if (household == "H")
            {
                IsHousehold = "H";
                AccessLevel="H";
            }

            fillCustomDropdowns(AccessLevel);
            if (programEntity != null)
            {
                SetComboBoxValue(cmbQuestionAccess, programEntity.LoadProgramQuestions + "_" + AccessLevel);
            }

            fillCustomQuestions(screen, AccessLevel, "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[0].ToString(), HIE);
            cmbSEQ.SelectedIndexChanged -= OnSequenceSelectedIndexChanged;
            cmbSEQ.SelectedIndexChanged += OnSequenceSelectedIndexChanged;
            cmbQuestionType.SelectedIndexChanged -= OnTypeSelectedIndexChanged;
            cmbQuestionType.SelectedIndexChanged += OnTypeSelectedIndexChanged;
            if (!Mode.Equals(Consts.Common.View))
            {
                // contextMenu2.Popup -= new EventHandler(contextMenu1_Popup);
                //contextMenu2.Popup += new EventHandler(contextMenu1_Popup);
            }
            else
            {
                gvwCustomQuestions.ReadOnly = true;
            }
            //gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            //gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        }

        public CustQuesMixedControl(BaseForm baseForm, string hieType, UserEntity userProfile)
        {
            InitializeComponent();
            BaseForm = baseForm;
            UserProfile = userProfile;
            HieType = hieType;
            GridViewControl = gvwCustomQuestions;
            MaxButtonControl = picMax;
            //contextMenu1 = new ContextMenu();
            //gvwCustomQuestions.ContextMenu = contextMenu1;
            fillCustomDropdowns("A");
            //fillCustomQuestions(Screen, "A", ((ListItem)cmbSEQ.SelectedItem).Text, ((ListItem)cmbQuestionType.SelectedItem).Text, ((ListItem)cmbQuestionAccess.SelectedItem).Text, HIE);
            fillCustomQuestions(Screen, "A", "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Text, HIE);

        }

        public void filterCustomQuestions(string filterValue)
        {
            fillCustomQuestions(Screen, filterValue, "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[0].ToString(), HIE);
        }

        #region Public Properties

        public UserEntity UserProfile { get; set; }

        public BaseForm BaseForm { get; set; }

        public string HieType { get; set; }

        public string HIE { get; set; }

        public bool IsMax { get; set; }

        public string Screen { get; set; }

        public string AccessLevel { get; set; }

        public string Mode { get; set; }

        public bool IsApplicant { get; set; }

        public CaseSnpEntity CaseSnpEntity { get; set; }

        public List<FldcntlHieEntity> FLDCNTLHIEEntity
        {
            get
            {
                return _custFLDCNTLEntity;
            }
            set
            {
                _custFLDCNTLEntity = value;
            }
        }

        public List<CommonEntity> CASEELIGEntity
        {
            get
            {
                return _custCASEELIGEntity;
            }
            set
            {
                _custCASEELIGEntity = value;
            }
        }

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

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void fillCustomDropdowns(string typ)
        {
            cmbSEQ.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("Description"));
            //listItem.Add(new ListItem("Sequence"));
            //listItem.Add(new ListItem("Required Questions"));
            //cmbSEQ.Items.AddRange(listItem.ToArray());
            //cmbSEQ.SelectedIndex = 1;

            cmbQuestionType.Items.Clear();
            //listItem = new List<ListItem>();
            //listItem.Add(new ListItem("All"));
            //listItem.Add(new ListItem("Active"));
            //listItem.Add(new ListItem("Inactive"));
            //cmbQuestionType.Items.AddRange(listItem.ToArray());
            //cmbQuestionType.SelectedIndex = 1;

            cmbQuestionAccess.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("Show All Questions", "A_"+ typ + ""));
            listItem.Add(new ListItem("Show Questions For This Program", "P_"+ typ + ""));
            //listItem.Add(new ListItem("Eligibility Criteria Specific", "E"));
            //listItem.Add(new ListItem("Not Eligibility Criteria Specific", "N"));
            cmbQuestionAccess.Items.AddRange(listItem.ToArray());
            cmbQuestionAccess.SelectedIndex = 0;
        }

        private void fillCustomQuestions(string screen, string memAccess, string seq, string questionType, string questionAccess, string HIE)
        {
            List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions(screen, memAccess, HIE, seq, questionType, questionAccess);

            List<CustomQuestionsEntity> custQuestionsProgramType = _model.FieldControls.GetCustomQuestions(screen, memAccess, HIE, seq, questionType, "P");

            List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetCustomQuestionAnswers(CaseSnpEntity);

            bool isResponse = false;

            string saveImage = "icon-save";

            string DeleteImage = "delete-item";
            gvwCustomQuestions.Rows.Clear();

            if(IsHousehold=="H")
            {
                custQuestions = custQuestions.FindAll(u => u.CUSTMEMACCESS == "H");
            }
            else
            {
                custQuestions = custQuestions.FindAll(u => u.CUSTMEMACCESS != "H");
            }

         
            if (custQuestions.Count > 0)
            {
                /**********************************************/
                if (gvwCustomQuestions.Columns.Count == 0)
                {
                    gvwCustomQuestions.Columns.Add("gvtRequire");

                    DataGridViewNumberColumn col = new DataGridViewNumberColumn();
                    col.HeaderText = "QNo"; col.Name = "gvtSeq";
                    col.HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    gvwCustomQuestions.Columns.Add(col);

                    gvwCustomQuestions.Columns.Add("imgsave");
                    gvwCustomQuestions.Columns.Add("Question");
                    gvwCustomQuestions.Columns.Add("Response");
                    gvwCustomQuestions.Columns.Add("ResponseCode");
                    gvwCustomQuestions.Columns.Add("FamilySeq");
                    gvwCustomQuestions.Columns.Add("ResponceSeq");
                    gvwCustomQuestions.Columns.Add("Code");
                    gvwCustomQuestions.Columns.Add("ResponceDelete");
                    gvwCustomQuestions.Columns.Add("gvtResponseQtype");
                    gvwCustomQuestions.Columns.Add("gvtActive");

                    DataGridViewNumberColumn Seqno = new DataGridViewNumberColumn();
                    Seqno.HeaderText = "SeqNo"; Seqno.Name = "gvtSeqNo";
                    Seqno.HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Seqno.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Seqno.Visible = false;
                    gvwCustomQuestions.Columns.Add(Seqno);
                }
                /**********************************************/

                gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                _customQuestionsandAnswers = new List<CustomQuestionsandAnswers>();
                int i =0;
                foreach (CustomQuestionsEntity dr in custQuestions)
                {
                    i++;
                    string custCode = dr.CUSTCODE.ToString();
                    string fieldType = dr.CUSTRESPTYPE.ToString();

                    List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                    //string saveImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("save.gif");
                    //string DeleteImage = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("DeleteItem.gif");

                    int rowIndex = gvwCustomQuestions.Rows.Add();

                    DataGridViewCell gvtRequire = new DataGridViewCell();
                    gvtRequire.Style.ForeColor = System.Drawing.Color.Red;
                    this.gvwCustomQuestions[0, rowIndex] = gvtRequire;
                    this.gvwCustomQuestions[0, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[0].Visible = true;
                    this.gvwCustomQuestions.Columns[0].HeaderText = "";
                    this.gvwCustomQuestions.Columns[0].Width = 15;

                    this.gvwCustomQuestions[1, rowIndex].ReadOnly = true;
                    this.gvwCustomQuestions[1, rowIndex].Value = i.ToString();
                    this.gvwCustomQuestions.Columns[1].Visible = true;
                    this.gvwCustomQuestions.Columns[1].Width = 60;
                    this.gvwCustomQuestions.Columns[1].HeaderText = "QNo";

                    DataGridViewImageCell ImgSave = new DataGridViewImageCell();
                    this.gvwCustomQuestions[2, rowIndex] = ImgSave;
                    this.gvwCustomQuestions[2, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[2].HeaderText = "";
                    this.gvwCustomQuestions.Columns[2].Width = 20;

                    DataGridViewCell Question = new DataGridViewCell();
                    Question.Style.WrapMode = DataGridViewTriState.True;
                    this.gvwCustomQuestions[3, rowIndex] = Question;
                    this.gvwCustomQuestions[3, rowIndex].Value = dr.CUSTDESC;
                    this.gvwCustomQuestions.Columns[3].Width = 500;//550

                    /****************************************************************************************/

                    if (fieldType.Equals("D"))
                    {
                        DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
                        contextMenu1_Popup(ComboBoxCell, fieldType, custCode, "");

                        //ComboBoxCell.Items.AddRange(new string[] { "aaa", "bbb", "ccc" });
                        ComboBoxCell.Style.BackgroundImageSource = "combo-arrow";
                        ComboBoxCell.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        ComboBoxCell.DropDownStyle = ComboBoxStyle.DropDownList;
                        ComboBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px; ";
                        // ComboBoxCell.Width = 100;

                        this.gvwCustomQuestions[4, rowIndex] = ComboBoxCell;
                        // this.gvwCustomQuestions[4, rowIndex].Value = "bbb";
                        this.gvwCustomQuestions[4, rowIndex].ToolTipText = "Question Type: Drop down";
                    }
                    else if (fieldType.Equals("X"))
                    {
                        DataGridViewTextBoxCell TextBoxCell = new DataGridViewTextBoxCell();
                        this.gvwCustomQuestions[4, rowIndex] = TextBoxCell;
                        this.gvwCustomQuestions[4, rowIndex].Value = "";
                        this.gvwCustomQuestions[4, rowIndex].ToolTipText = "Question Type: Text";
                        TextBoxCell.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";
                    }
                    else if (fieldType.Equals("N"))
                    {
                        DataGridViewTextBoxCell Response = new DataGridViewTextBoxCell();
                        //Response.HideUpDownButtons = true;
                        this.gvwCustomQuestions[4, rowIndex] = Response;
                        this.gvwCustomQuestions[4, rowIndex].Value = string.Empty;
                        this.gvwCustomQuestions[4, rowIndex].ToolTipText = "Question Type: Numeric";
                        Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                    }
                    else if (fieldType.Equals("T"))
                    {
                        DataGridViewDateTimePickerCell Response = new DataGridViewDateTimePickerCell();
                        //Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn Response = new Captain.Common.Views.Controls.Compatibility.DataGridViewDateTimeColumn();
                        Response.Format = DateTimePickerFormat.Short;
                        Response.Style.BackgroundImageSource = "icon-calendar";
                        Response.Style.BackgroundImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                        this.gvwCustomQuestions[4, rowIndex] = Response;
                        this.gvwCustomQuestions[4, rowIndex].Value = string.Empty;
                        this.gvwCustomQuestions[4, rowIndex].ToolTipText = "Question Type: Date";
                        Response.Style.CssStyle = "border:1px solid #ccc; border-radius:2px;";

                    }
                    else if (fieldType.Equals("C"))
                    {
                        DataGridViewButtonCell Response = new DataGridViewButtonCell();

                        // contextMenu1_Popup(null, Response, fieldType, custCode);
                        this.gvwCustomQuestions[4, rowIndex] = Response;
                        this.gvwCustomQuestions[4, rowIndex].Value = string.Empty;
                        this.gvwCustomQuestions[4, rowIndex].ToolTipText = "Question Type: Check Box";

                    }
                    else
                    {
                        DataGridViewCell Response = new DataGridViewCell();
                        this.gvwCustomQuestions[4, rowIndex] = Response;
                        this.gvwCustomQuestions[4, rowIndex].Value = string.Empty;
                    }
                    this.gvwCustomQuestions.Columns[4].Width = 300; //350

                    /****************************************************************************************/

                    DataGridViewCell ResponseCode = new DataGridViewCell();
                    this.gvwCustomQuestions[5, rowIndex] = ResponseCode;
                    this.gvwCustomQuestions[5, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[5].Visible = false;

                    DataGridViewCell FamilySeq = new DataGridViewCell();
                    this.gvwCustomQuestions[6, rowIndex] = FamilySeq;
                    this.gvwCustomQuestions[6, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[6].Visible = false;

                    DataGridViewCell ResponceSeq = new DataGridViewCell();
                    this.gvwCustomQuestions[7, rowIndex] = ResponceSeq;
                    this.gvwCustomQuestions[7, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[7].Visible = false;

                    DataGridViewCell Code = new DataGridViewCell();
                    this.gvwCustomQuestions[8, rowIndex] = Code;
                    this.gvwCustomQuestions[8, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[8].Visible = false;

                    DataGridViewImageCell ResponceDelete = new DataGridViewImageCell();
                    this.gvwCustomQuestions[9, rowIndex] = ResponceDelete;
                    this.gvwCustomQuestions[9, rowIndex].Value = string.Empty;
                    this.gvwCustomQuestions.Columns[9].Width = 40;
                    this.gvwCustomQuestions.Columns[9].HeaderText = "Delete";

                    DataGridViewCell gvtResponseQtype = new DataGridViewCell();
                    this.gvwCustomQuestions[10, rowIndex] = gvtResponseQtype;
                    this.gvwCustomQuestions[10, rowIndex].Value = "A";
                    this.gvwCustomQuestions.Columns[10].Visible = false;

                    DataGridViewCell gvtActive = new DataGridViewCell();
                    this.gvwCustomQuestions[11, rowIndex] = gvtActive;
                    this.gvwCustomQuestions[11, rowIndex].Value = dr.CUSTACTIVECUST;
                    this.gvwCustomQuestions.Columns[11].Visible = true;
                    this.gvwCustomQuestions.Columns[11].Width = 40;
                    this.gvwCustomQuestions.Columns[11].HeaderText = "A/I";


                    this.gvwCustomQuestions[12, rowIndex].ReadOnly = true;
                    this.gvwCustomQuestions[12, rowIndex].Value = dr.CUSTSEQ;
                    this.gvwCustomQuestions.Columns[12].Visible = false;
                    this.gvwCustomQuestions.Columns[12].Width = 60;
                    this.gvwCustomQuestions.Columns[12].HeaderText = "SeqNo";

                    // int rowIndex = gvwCustomQuestions.Rows.Add(string.Empty, string.Empty, dr.CUSTDESC, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "A");

                    if (questionAccess.ToUpper() != "P")
                    {
                        if (custQuestionsProgramType.Count > 0)
                        {
                            CustomQuestionsEntity custQuestionProgramType = custQuestionsProgramType.Find(u => u.CUSTCODE == custCode);
                            if (custQuestionProgramType != null)
                            {
                                if (custQuestionProgramType.CUSTREQUIRED == "Y")
                                {
                                    gvwCustomQuestions.Rows[rowIndex].Cells["gvtRequire"].Value = "*";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dr.CUSTREQUIRED == "Y")
                        {
                            gvwCustomQuestions.Rows[rowIndex].Cells["gvtRequire"].Value = "*";
                        }
                    }

                    if (response.Count > 0)
                    {
                        gvwCustomQuestions.Rows[rowIndex].Cells["imgsave"].Value = saveImage;
                        gvwCustomQuestions.Rows[rowIndex].Cells["ResponceDelete"].Value = DeleteImage;
                    }
                    gvwCustomQuestions.Rows[rowIndex].Tag = dr;

                    //Kranthi
                    //string fieldType = dr.CUSTRESPTYPE.ToString();
                    //if (fieldType.Equals("D"))
                    //{
                    //    gvwCustomQuestions.Rows[rowIndex].ReadOnly = true;
                    //    gvwCustomQuestions.Rows[rowIndex].Cells["Question"].ToolTipText = "Question Type: Drop down";
                    //}
                    //else if (fieldType.Equals("C"))
                    //{
                    //    gvwCustomQuestions.Rows[rowIndex].ReadOnly = true;
                    //    gvwCustomQuestions.Rows[rowIndex].Cells["Question"].ToolTipText = "Question Type: Check Box";
                    //}
                    //else
                    //{
                    //    gvwCustomQuestions.Rows[rowIndex].Cells[1].ReadOnly = true;
                    //    gvwCustomQuestions.Rows[rowIndex].Cells[2].ReadOnly = true;
                    //}

                    //if (fieldType.Equals("N"))
                    //{
                    //    gvwCustomQuestions.Rows[rowIndex].Cells["Question"].ToolTipText = "Question Type: Numeric";
                    //}
                    //else if (fieldType.Equals("T"))
                    //{
                    //    gvwCustomQuestions.Rows[rowIndex].Cells["Question"].ToolTipText = "Question Type: Date";
                    //}
                    //else if (fieldType.Equals("X"))
                    //{
                    //    gvwCustomQuestions.Rows[rowIndex].Cells["Question"].ToolTipText = "Question Type: Text";
                    //}

                    string custQuestionResp = string.Empty;
                    string custQuestionCode = string.Empty;
                    if (true)   //!Mode.Equals(Consts.Common.Add))
                    {
                        //List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                        if (response != null && response.Count > 0)
                        {
                            isResponse = true;
                            if (fieldType.Equals("D"))
                            {
                                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", response[0].ACTCODE);

                                foreach (CustomQuestionsEntity custResp in response)
                                {
                                    string code = custResp.ACTMULTRESP.Trim();
                                    CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
                                    if (custRespEntity != null)
                                    {
                                        custQuestionResp = custRespEntity.RespDesc;
                                        custQuestionCode = custResp.ACTMULTRESP.ToString();
                                        if (custRespEntity.RspStatus.ToUpper() == "I")
                                            gvwCustomQuestions.Rows[rowIndex].Cells["gvtResponseQType"].Value = "I";
                                    }
                                }
                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Tag = custQuestionCode.Trim();
                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Value = custQuestionCode.Trim();// custQuestionResp;
                                gvwCustomQuestions.Rows[rowIndex].Cells["Question"].Tag = "U";
                            }
                            else if (fieldType.Equals("C"))
                            {
                                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", response[0].ACTCODE);
                                if (custReponseEntity.Count > 0)
                                {
                                    string response1 = response[0].ACTALPHARESP;
                                    if (!string.IsNullOrEmpty(response1))
                                    {
                                        string[] arrResponse = null;
                                        if (response1.IndexOf(',') > 0)
                                        {
                                            arrResponse = response1.Split(',');
                                        }
                                        else if (!response1.Equals(string.Empty))
                                        {
                                            arrResponse = new string[] { response1 };
                                        }
                                        foreach (string stringitem in arrResponse)
                                        {
                                            CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                                            if (custRespEntity != null)
                                            {
                                                custQuestionResp += custRespEntity.RespDesc + ", ";
                                                //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                            }
                                        }
                                    }
                                }
                                // custQuestionResp = response[0].ACTALPHARESP;

                                if (custQuestionResp.Length > 1)
                                {
                                    custQuestionResp = custQuestionResp.Trim();
                                    if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                                    {
                                        custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                                    }
                                }
                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Tag = response[0].ACTALPHARESP;
                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Value = custQuestionResp;
                                gvwCustomQuestions.Rows[rowIndex].Cells["Question"].Tag = "U";
                            }
                            else if (fieldType.Equals("N"))
                            {
                                custQuestionResp = response[0].ACTNUMRESP.ToString();
                            }
                            else if (fieldType.Equals("T"))
                            {
                                custQuestionResp = LookupDataAccess.Getdate(response[0].ACTDATERESP.ToString());
                            }
                            else
                            {
                                custQuestionResp = response[0].ACTALPHARESP.ToString();
                            }

                            if (fieldType.Equals("D"))
                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Value = custQuestionCode.Trim();
                            else
                                gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Value = custQuestionResp;

                            gvwCustomQuestions.Rows[rowIndex].Cells["Question"].Tag = "U";
                            gvwCustomQuestions.Rows[rowIndex].Cells["FamilySeq"].Value = response[0].ACTSNPFAMILYSEQ;
                            gvwCustomQuestions.Rows[rowIndex].Cells["ResponceSeq"].Value = response[0].ACTRESPSEQ;
                            gvwCustomQuestions.Rows[rowIndex].Cells["Code"].Value = response[0].ACTCODE;
                        }
                        else
                        {
                            gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Tag = custQuestionCode;
                            gvwCustomQuestions.Rows[rowIndex].Cells["Response"].Value = custQuestionResp;
                        }
                    }
                    
                    bool isApplicant = dr.CUSTMEMACCESS.Equals("A") ? true : false;
                    _customQuestionsandAnswers.Add(new CustomQuestionsandAnswers(isResponse ? "U" : "I", dr.CUSTCODE, dr.CUSTDESC, string.Empty, string.Empty, string.Empty, string.Empty, isApplicant, custQuestionResp, custQuestionCode));

                    if (!dr.CUSTACTIVECUST.Equals("A"))
                        gvwCustomQuestions.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;

                }
                gvwCustomQuestions.Update();

                if (gvwCustomQuestions.Rows.Count > 0)
                {
                    gvwCustomQuestions.CurrentCell = gvwCustomQuestions.Rows[0].Cells[0];
                    gvwCustomQuestions.Rows[0].Selected = true;
                }

                gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);

            }
        }

        private void getFLDCNTLHIE(string screen, string HIE, string Type)
        {
            FLDCNTLHIEEntity = _model.FieldControls.GetFLDCNTLHIE(screen, HIE, Type);
        }

        private void getCASEELIGHIE(string screen, string HIE, string Type)
        {
            CASEELIGEntity = _model.FieldControls.GetCASEELIGHIE(screen, HIE, Type);
        }

        private void contextMenu1_Popup(DataGridViewComboBoxCell ComboBoxCell, string fieldType, string fieldCode, string _response)
        {
            if (gvwCustomQuestions.Rows.Count > 0)
            {
                if (fieldType.Equals("D"))
                {
                    List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", fieldCode);
                    if (custReponseEntity.Count > 0)
                    {
                        //string response = gvwCustomQuestions.SelectedRows[0].Cells[3].Value != null ? gvwCustomQuestions.SelectedRows[0].Cells[3].Value.ToString() : string.Empty;
                        //if (gvwCustomQuestions.SelectedRows[0].Cells["gvtResponseQType"].Value.ToString().ToUpper() == "A")
                        //{
                        //    custReponseEntity = custReponseEntity.FindAll(u => u.RspStatus.Trim().ToUpper() != "I");
                        //}
                        //else
                        //{
                        //    custReponseEntity = custReponseEntity.Where(u => (u.RspStatus.Trim().ToUpper() == "A") || (u.RespDesc.ToString().ToUpper() == response.ToString().ToUpper())).ToList();
                        //}

                        //string[] arrResponse = null;
                        //if (response.IndexOf(',') > 0)
                        //{
                        //    arrResponse = response.Split(',');
                        //}
                        //else if (!response.Equals(string.Empty))
                        //{
                        //    arrResponse = new string[] { response };
                        //}


                        /**************************************************************************************/
                        //foreach (CustRespEntity dr in custReponseEntity)
                        //{
                        //    string resDesc = dr.RespDesc.ToString().Trim();
                        //    string resCode = dr.DescCode.ToString().Trim();

                        //    //MenuItem menuItem = new MenuItem();
                        //    //menuItem.Text = resDesc;
                        //    //menuItem.Tag = dr;
                        //    //if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                        //    //{
                        //    //    menuItem.Checked = true;
                        //    //}
                        //    //contextMenu2.MenuItems.Add(menuItem);
                        //   // ComboBoxCell.Items.AddRange(new string[] { resDesc, resCode });
                        //    ComboBoxCell.Items.Add(new ListItem(resDesc, resCode));
                        /**************************************************************************************/

                        //}

                        ComboBoxCell.DataSource = custReponseEntity;
                        ComboBoxCell.DisplayMember = "RespDesc";
                        ComboBoxCell.ValueMember = "DescCode";
                    }
                }
                else if (fieldType.Equals("C"))
                {
                    string response = _response; // gvwCustomQuestions.SelectedRows[0].Cells[3].Tag != null ? gvwCustomQuestions.SelectedRows[0].Cells[3].Tag.ToString() : string.Empty;
                    PrivilegeEntity privileges = new PrivilegeEntity();
                    privileges.AddPriv = "true";
                    AlertCodeForm objform = new AlertCodeForm(BaseForm, privileges, response, fieldCode);
                    objform.StartPosition = FormStartPosition.CenterScreen;
                    objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                    objform.ShowDialog();


                }
                else if (fieldType.Equals("X"))
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Text = "Please enter text here";
                    menuItem.Tag = "X"; // menuItem.Tag = "A";
                    contextMenu2.MenuItems.Add(menuItem);
                    gvwCustomQuestions.Rows[0].Cells["Response"].ReadOnly = false;
                }
                else if (fieldType.Equals("T"))
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Text = "Please enter Date here";
                    menuItem.Tag = "T";//menuItem.Tag = "X";
                    contextMenu2.MenuItems.Add(menuItem);
                    gvwCustomQuestions.Rows[0].Cells["Response"].ReadOnly = false;
                }
                else if (fieldType.Equals("N"))
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Text = "Please enter number here";
                    menuItem.Tag = "N";
                    contextMenu2.MenuItems.Add(menuItem);
                    gvwCustomQuestions.Rows[0].Cells["Response"].ReadOnly = false;
                }
            }
        }

        private void gvwCustomQuestions_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is CustRespEntity)
            {
                string responseValue = gvwCustomQuestions.SelectedRows[0].Cells["Response"].Value.ToString();
                string responseCode = gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag == null ? string.Empty : gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag.ToString();
                CustRespEntity dr = (CustRespEntity)objArgs.MenuItem.Tag as CustRespEntity;

                string selectedValue = objArgs.MenuItem.Text;
                string selectedCode = dr.DescCode.ToString();
                if (objArgs.MenuItem.Checked)
                {
                    //responseValue = responseValue.Replace(selectedValue + Consts.Common.Comma, string.Empty);
                    //responseValue = responseValue.Replace(selectedValue, string.Empty);
                    //responseCode = responseCode.Replace(selectedCode + Consts.Common.Comma, string.Empty);
                    //responseCode = responseCode.Replace(selectedCode, string.Empty);
                    //gvwCustomQuestions.SelectedRows[0].Cells[3].Value = responseValue;
                    //gvwCustomQuestions.SelectedRows[0].Cells[3].Tag = responseCode;
                    responseValue = selectedValue;
                    responseCode = selectedCode;
                }
                else
                {
                    //if (!responseValue.Equals(string.Empty)) responseValue += ",";
                    //if (!responseCode.Equals(string.Empty)) responseCode += ",";
                    responseValue = selectedValue;
                    responseCode = selectedCode;
                    //gvwCustomQuestions.SelectedRows[0].Cells[3].Value = responseValue;
                    //gvwCustomQuestions.SelectedRows[0].Cells[3].Tag = responseCode;
                }
                string custCode = ((CustomQuestionsEntity)gvwCustomQuestions.SelectedRows[0].Tag).CUSTCODE;
                _customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseValue = responseValue);
                _customQuestionsandAnswers.FindAll(u => u.CustCode.Equals(custCode)).ForEach(c => c.ResponseCode = responseCode);
                gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag = responseCode;
                gvwCustomQuestions.SelectedRows[0].Cells["Response"].Value = responseValue;
            }
            else
            {
                gvwCustomQuestions.Rows[0].Cells["Response"].ReadOnly = false;
                gvwCustomQuestions.Rows[0].Cells["Response"].Selected = true;
            }
        }

        private void OnSequenceSelectedIndexChanged(object sender, EventArgs e)
        {
            gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            string _typ = ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[1].ToString();
            if (_typ == "H")
                AccessLevel = "H";
            fillCustomQuestions(Screen, AccessLevel, "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[0].ToString(), HIE);
            gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        }

        private void OnTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            string _typ = ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[1].ToString();
            if (_typ == "H")
                AccessLevel = "H";
            fillCustomQuestions(Screen, AccessLevel, "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[0].ToString(), HIE);
            gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        }

        private void cmbQuestionAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
            string _typ = ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[1].ToString();
            if (_typ == "H")
                AccessLevel = "H";

            fillCustomQuestions(Screen, AccessLevel, "Sequence", "All", ((ListItem)cmbQuestionAccess.SelectedItem).Value.ToString().Split('_')[0].ToString(), HIE);
            gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
        }

        //private void gvwCustomQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
        //    CustomQuestionsEntity questionEntity = gvwCustomQuestions.Rows[e.RowIndex].Tag as CustomQuestionsEntity;
        //    custEntity.ACTAGENCY = CaseSnpEntity.Agency;
        //    custEntity.ACTDEPT = CaseSnpEntity.Dept;
        //    custEntity.ACTPROGRAM = CaseSnpEntity.Program;
        //    custEntity.ACTYEAR = CaseSnpEntity.Year;
        //    custEntity.ACTAPPNO = CaseSnpEntity.App;
        //    custEntity.ACTSNPFAMILYSEQ = CaseSnpEntity.FamilySeq;
        //    custEntity.ACTCODE = questionEntity.CUSTCODE;
        //    if (questionEntity.CUSTRESPTYPE.Equals("D"))
        //    {
        //        custEntity.ACTMULTRESP = gvwCustomQuestions.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString();
        //    }
        //    else if (questionEntity.CUSTRESPTYPE.Equals("N"))
        //    {
        //        custEntity.ACTNUMRESP = gvwCustomQuestions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        //    }
        //    else if (questionEntity.CUSTRESPTYPE.Equals("X"))
        //    {
        //        custEntity.ACTDATERESP = gvwCustomQuestions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        //    }
        //    else
        //    {
        //        custEntity.ACTALPHARESP = gvwCustomQuestions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        //    }
        //    if (gvwCustomQuestions.Rows[e.RowIndex].Cells[2].Tag is string)
        //    {
        //        custEntity.Mode = gvwCustomQuestions.Rows[e.RowIndex].Cells[2].Tag as string;
        //    }
        //    custEntity.addoperator = BaseForm.UserID;
        //    custEntity.lstcoperator = BaseForm.UserID;
        //    _model.CaseMstData.InsertUpdateADDCUST(custEntity);
        //}

        private void gvwCustomQuestions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwCustomQuestions.SelectedRows.Count > 0)
            {
                int intcolindex = gvwCustomQuestions.CurrentCell.ColumnIndex;
                int introwindex = gvwCustomQuestions.CurrentCell.RowIndex;
                string strCurrectValue = Convert.ToString(gvwCustomQuestions.Rows[e.RowIndex].Cells["Response"].Value);
                CustomQuestionsEntity questionEntity = gvwCustomQuestions.Rows[e.RowIndex].Tag as CustomQuestionsEntity;

                if (gvwCustomQuestions.Columns[e.ColumnIndex].Name.Equals("Response") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("D"))
                {
                    DataGridViewComboBoxCell dgcombocell = ((DataGridViewComboBoxCell)gvwCustomQuestions.Rows[e.RowIndex].Cells[e.ColumnIndex]);

                    string strCode = Convert.ToString(dgcombocell.Value);
                    string strDesc = Convert.ToString(dgcombocell.FormattedValue);

                    dgcombocell.Tag = strCode;

                }

                if (gvwCustomQuestions.Columns[e.ColumnIndex].Name.Equals("Response") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("N"))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.TwoDecimalString) && strCurrectValue != string.Empty) //Kranthi 02/24/2023:: Small things to clean::Decimal number  is showing this error
                                                                                                                                                               //if (strCurrectValue != string.Empty)
                    {
                        gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                        gvwCustomQuestions.Rows[introwindex].Cells["Response"].Value = string.Empty;
                        gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                        AlertBox.Show(Consts.Messages.PleaseEnterNumbers, MessageBoxIcon.Warning);
                    }
                }
                else if (gvwCustomQuestions.Columns[e.ColumnIndex].Name.Equals("Response") && questionEntity != null && questionEntity.CUSTRESPTYPE.Equals("T"))
                {
                    if (strCurrectValue != "")
                    {
                        strCurrectValue = Convert.ToDateTime(strCurrectValue).ToString("MM/dd/yyyy");
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                    {
                        gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                        gvwCustomQuestions.Rows[introwindex].Cells["Response"].Value = string.Empty;
                        gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                        AlertBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (questionEntity.CUSTCALLOWFDATE == "N")
                        {
                            if (Convert.ToDateTime(strCurrectValue).Date > DateTime.Now.Date)
                            {
                                gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                                gvwCustomQuestions.Rows[introwindex].Cells["Response"].Value = string.Empty;
                                gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                                AlertBox.Show("Future Date is not Allowed...", MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
        }

        private void gvwCustomQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonCell &&
            //    e.RowIndex >= 0)
            //{
            //    //TODO - Button Clicked - Execute Code Here
            //}
            //if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
            // {
            if (e.ColumnIndex > 0 && e.RowIndex != -1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].CellRenderer == "ButtonCell" && e.RowIndex != -1)
                {
                    // string custCode = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString();

                    CustomQuestionsEntity drow = senderGrid.Rows[e.RowIndex].Tag as CustomQuestionsEntity;
                    string custCode = drow.CUSTCODE.ToString();
                    string fieldType = drow.CUSTRESPTYPE.ToString();
                    string _response = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null ? senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() : string.Empty;

                    //TODO - Button Clicked - Execute Code Here
                    contextMenu1_Popup(null, "C", custCode, _response);
                }

                if (e.ColumnIndex == 9 && e.RowIndex != -1 && senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].CellRenderer == "ImageCell")
                {
                    if (SelectedRow())
                    {
                        if (gvwCustomQuestions.Rows[e.RowIndex].Cells["Code"].Value != string.Empty)
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandlerQuestions);
                    }
                }
            }
           
            //}
        }

        private void MessageBoxHandlerQuestions(DialogResult dialogResult)
        {

            // Set DialogResult value of the Form as a text for label
            if (dialogResult == DialogResult.Yes)
            {
                CustomQuestionsEntity custEntity = new CustomQuestionsEntity();
                //CustomQuestionsEntity questionEntity = gvwCustomQuestions.Tag as CustomQuestionsEntity;
                custEntity.ACTAGENCY = CaseSnpEntity.Agency;
                custEntity.ACTDEPT = CaseSnpEntity.Dept;
                custEntity.ACTPROGRAM = CaseSnpEntity.Program;
                custEntity.ACTYEAR = CaseSnpEntity.Year;
                custEntity.ACTAPPNO = CaseSnpEntity.App;
                custEntity.ACTSNPFAMILYSEQ = gvwCustomQuestions.SelectedRows[0].Cells["FamilySeq"].Value.ToString();

                // = response[0].ACTRESPSEQ;
                //= response[0].ACTCODE;                        
                //if (IsApplicant && questionEntity.CUSTMEMACCESS.Equals("A"))
                //    custEntity.ACTSNPFAMILYSEQ = "9999999";
                //if (IsApplicant && questionEntity.CUSTMEMACCESS.Equals("H"))
                //    custEntity.ACTSNPFAMILYSEQ = "8888888";

                custEntity.ACTCODE = gvwCustomQuestions.SelectedRows[0].Cells["Code"].Value.ToString();
                custEntity.ACTRESPSEQ = gvwCustomQuestions.SelectedRows[0].Cells["ResponceSeq"].Value.ToString();
                custEntity.Mode = "Delete";
                if (_model.CaseMstData.InsertUpdateADDCUST(custEntity))
                {
                    gvwCustomQuestions.CellValueChanged -= new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);
                    gvwCustomQuestions.SelectedRows[0].Cells["imgsave"].Value = string.Empty;
                    gvwCustomQuestions.SelectedRows[0].Cells["ResponceDelete"].Value = string.Empty;
                    gvwCustomQuestions.SelectedRows[0].Cells["Response"].Value = string.Empty;
                    gvwCustomQuestions.SelectedRows[0].Cells["Code"].Value = string.Empty;
                    gvwCustomQuestions.SelectedRows[0].Cells["Question"].Tag = string.Empty;
                    gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag = null;

                    gvwCustomQuestions.CellValueChanged += new DataGridViewCellEventHandler(gvwCustomQuestions_CellValueChanged);

                }

            }

        }


        private bool SelectedRow()
        {

            bool boolrowselet = false;
            if (gvwCustomQuestions.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwCustomQuestions.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        boolrowselet = true;
                        break;
                    }
                }
            }
            return boolrowselet;
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            //AlertCodeForm form = sender as AlertCodeForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    gvwCustomQuestions.SelectedRows[0].Cells[3].Tag = form.propAlertCode;
            //    gvwCustomQuestions.SelectedRows[0].Cells[3].Value = form.propAlertCode;
            //}
            // //  txtAlertCodes.Text = form.propAlertCode;

            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                gvwCustomQuestions.SelectedRows[0].Cells["Response"].Tag = form.propAlertCode;

                string custQuestionResp = string.Empty;
                List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses(Screen, form.propFieldCode);
                if (custReponseEntity.Count > 0)
                {
                    string response1 = form.propAlertCode;
                    if (!string.IsNullOrEmpty(response1))
                    {
                        string[] arrResponse = null;
                        if (response1.IndexOf(',') > 0)
                        {
                            arrResponse = response1.Split(',');
                        }
                        else if (!response1.Equals(string.Empty))
                        {
                            arrResponse = new string[] { response1 };
                        }
                        foreach (string stringitem in arrResponse)
                        {

                            CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                            if (custRespEntity != null)
                            {
                                custQuestionResp += custRespEntity.RespDesc + ", ";
                            }
                        }
                    }
                }

                if (custQuestionResp.Length > 1)
                {
                    custQuestionResp = custQuestionResp.Trim();
                    if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                    {
                        custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                    }
                }
                gvwCustomQuestions.SelectedRows[0].Cells["Response"].Value = custQuestionResp;

            }

        }

        private void gvwCustomQuestions_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            //DO NOTHING HERE
        }
    }
}
