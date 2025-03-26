#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using Wisej.Web;
using Wisej.Design;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.IO;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

#endregion

namespace Captain.Common.Views.UserControls
{
	public partial class Case3001Control_DEMO : BaseUserControl
	{
		#region private variables

		private CaptainModel _model = null;
		private AlertCodes alertCodesUserControl = null;
		private string strNameFormat = string.Empty;
		private string strCwFormat = string.Empty;
		private string strYear = "    ";
		private int strIndex = 0;

		#endregion

		public Case3001Control_DEMO(BaseForm baseForm, PrivilegeEntity privileges)
			: base(baseForm)
		{
			InitializeComponent();
			BaseForm = baseForm;
			Privileges = privileges;
			_model = new CaptainModel();
			if (BaseForm.BaseCaseMstListEntity != null)
			{
				CaseMST = BaseForm.BaseCaseMstListEntity[0];
				CaseSnpEntityProp = BaseForm.BaseCaseSnpEntity;
			}

			MainMenuAppNo = string.Empty;
			ApplicantLastName = string.Empty;
			setControlEnabled(false);
			propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
			var HIE = CaseMST.ApplAgency + CaseMST.ApplDept + CaseMST.ApplProgram;
			preassesCntlEntity = _model.FieldControls.GetFLDCNTLHIE("PREASSES", HIE, "PREASSES");
			proppreassesQuestions = new List<CustomQuestionsEntity>();
			// AbcdControlsVisable();
			if (preassesCntlEntity.Count > 0)
			{
				if (!preassesCntlEntity.Exists(u => u.Enab.Equals("Y")))
				{
					tabUserDetails.TabPages[4].Hidden = true;
				}
				else
				{
					preassessMasterEntity = _model.lookupDataAccess.GetDimension();
					// preassessMasterEntity = _model.FieldControls.GetPreassessData("MASTER");
					preassessChildEntity = _model.FieldControls.GetPreassessData(string.Empty);
					proppreassesQuestions = _model.FieldControls.GetPreassesQuestions(
						"PREASSES", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence",
						"ACTIVE", "P");

					tabUserDetails.TabPages[4].Hidden = false;
					;
				}
			}
			else
			{
				tabUserDetails.TabPages[4].Hidden = true;
			}

			Enableoutservicecmb();
			GetSelectedProgram();
			fillWaitList();
			fillOutofService();

			//HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(MainMenuAgency, "**", "**");
			//if (HierarchyEntity != null)
			//{
			strNameFormat = BaseForm.BaseHierarchyCnFormat.ToString();
			strCwFormat = BaseForm.BaseHierarchyCwFormat.ToString();
			//}

			fillDropDowns();
			//strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";
			var programEntity =
				_model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
			if (programEntity != null) ProgramDefinition = programEntity;

			// List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(Privileges.ModuleCode, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
			var userPrivilege = _model.UserProfileAccess.GetScreensByUserID(
				BaseForm.BusinessModuleID, BaseForm.UserID,
				BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
			CaseVerPrivileges = Privileges;
			CaseIncomePrivileges = Privileges;
			TmsSupplierPrivileges = Privileges;
			ImageUploadPrivileges = Privileges;
			if (userPrivilege.Count > 0)
			{
				bool boolver, boolincome, boolsupp;
				boolver = boolincome = boolsupp = false;
				CaseVerPrivileges = userPrivilege.Find(u => u.Program == "CASE2003");
				CaseIncomePrivileges = userPrivilege.Find(u => u.Program == "CASINCOM");
				TmsSupplierPrivileges = userPrivilege.Find(u => u.Program == "TMS00201");
				ImageUploadPrivileges = userPrivilege.Find(u => u.Program == "IMGUPLOD");
				if (CaseVerPrivileges != null)
				{
					if (CaseVerPrivileges.ViewPriv.Equals("true"))
					{
						btnIncVerfication.Visible = true;
						boolver = true;
					}
					else
					{
						btnIncVerfication.Visible = false;
					}
				}

				if (CaseIncomePrivileges != null)
				{
					if (CaseIncomePrivileges.ViewPriv.Equals("true"))
					{
						btnIncomeDetails.Visible = true;
						boolincome = true;
					}
					else
					{
						btnIncomeDetails.Visible = false;
					}
				}

				if (TmsSupplierPrivileges != null)
				{
					if (TmsSupplierPrivileges.ViewPriv.Equals("true"))
					{
						btnSupplier.Visible = true;
						boolsupp = true;
					}
					else
					{
						btnSupplier.Visible = false;
					}
				}

				if (ImageUploadPrivileges != null)
				{
					if (ImageUploadPrivileges.ViewPriv.Equals("true"))
					{
						if (ToolBarImageTypes != null)
							ToolBarImageTypes.Visible = true;
					}
					else
					{
						if (ToolBarImageTypes != null)
							ToolBarImageTypes.Visible = false;
					}
				}
				else
				{
					if (ToolBarImageTypes != null)
						ToolBarImageTypes.Visible = false;
				}


				if (boolincome == false && boolver == false && boolsupp == true) btnSupplier.Location = new Point(5, 1);
				if (boolincome == false && boolver == true && boolsupp == true)
				{
					btnIncVerfication.Location = new Point(5, 1);
					btnSupplier.Location = new Point(112, 1);
				}

				if (boolincome == true && boolver == false && boolsupp == true)
					btnSupplier.Location = new Point(112, 1);
				if (boolincome == false && boolver == true && boolsupp == false)
					btnIncVerfication.Location = new Point(5, 1);
			}

			fillClientIntake();
			alertCodesUserControl = new AlertCodes(BaseForm, privileges, ProgramDefinition);
			alertCodesUserControl.Dock = DockStyle.Fill;
			pnlAlertcode.Controls.Add(alertCodesUserControl);
			DisplayIncomeMsgs();
			propzipCodeEntity =
				_model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
			var endCellWidth = gvwCustomer.Width - 300;
			gvwCustomer.Columns["cellEnd"].Width = endCellWidth > 0 ? endCellWidth : 5;
			gvwCustomer.Update();
			//if (Privileges.ModuleCode.Trim() == "08" || Privileges.ModuleCode.Trim() == "09")
			//{
			//    btnSupplier.Visible = true;
			//}

			if (propAgencyControlDetails != null)
				//if (propAgencyControlDetails.AgyShortName.ToUpper() == "UETHDA")
				//{
				//    lblPDss1.Text = "DHS Programs :";
				//    lblPDss2.Text = "Are you currently receiving DHS Services?";
				//}
				if (propAgencyControlDetails.State.ToUpper() == "TX")
				{
					lblHousingTX.Visible = true;
					cmbHousingTx.Visible = true;
				}

            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
				gvwCustomer.Rows[0].Selected = true;
			}
        }


		public void Refresh()
		{
			gvwCustomer.Rows.Clear();
			gvwCustomer.Update();
			gvwCustomer.ResumeLayout();
			txtInHouse.Text = string.Empty;
			txtTotalIncome.Text = string.Empty;
			txtProgramIncome.Text = string.Empty;
			txtInProg.Text = string.Empty;
			gvwPreassesData.Rows.Clear();
			ClearControls();
			GetSelectedProgram();
			fillClientIntake();
			ShowCaseNotesImages();
			RefreshAlertCode();
			tabUserDetails.SelectedIndex = 0;
			if (gvwCustomer.Rows.Count != 0)
			{
				if (gvwCustomer.Rows.Count > strIndex)
				{
					gvwCustomer.Rows[strIndex].Selected = true;
					gvwCustomer.CurrentCell = gvwCustomer.Rows[strIndex].Cells[1];
				}
				else
				{
					gvwCustomer.Rows[0].Selected = true;
					gvwCustomer.CurrentCell = gvwCustomer.Rows[0].Cells[1];
				}
			}
			else
			{
				btnIncomeDetails.Enabled = false;
				btnIncVerfication.Enabled = false;
			}

			if (gvwCustomer.Rows.Count > 0) gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
		}

		public void RefreshAlertCode()
		{
			pnlAlertcode.Controls.Clear();
			alertCodesUserControl = new AlertCodes(BaseForm, Privileges, ProgramDefinition);
			alertCodesUserControl.Dock = DockStyle.Fill;
			pnlAlertcode.Controls.Add(alertCodesUserControl);
			DisplayIncomeMsgs();
		}

		private void ShowCaseNotesImages()
		{
			strYear = "    ";
			if (!string.IsNullOrEmpty(BaseForm.BaseYear)) strYear = BaseForm.BaseYear;
			caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(
				Privileges.Program,
				BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
			if (caseNotesEntity.Count > 0)
				ToolBarCaseNotes.ImageSource = "captain-notes";
			else
				ToolBarCaseNotes.ImageSource = "case-notes";
			if (!(gvwCustomer.Rows.Count > 0))
			{
				ToolBarCaseNotes.Enabled = false;
				ToolBarImageTypes.Enabled = false;
				ToolBarHistory.Enabled = false;
				ToolBarEdit.Enabled = false;
				ToolBarNewMember.Enabled = false;
				ToolBarPrint.Enabled = false;
			}
			else
			{
				ToolBarCaseNotes.Enabled = true;
				ToolBarImageTypes.Enabled = true;
				ToolBarHistory.Enabled = true;
				if (Privileges.AddPriv.Equals("false"))
				{
					if (ToolBarNewMember != null) ToolBarNewMember.Enabled = false;
				}
				else
				{
					if (ToolBarNewMember != null) ToolBarNewMember.Enabled = true;
				}

				if (Privileges.ChangePriv.Equals("false"))
				{
					if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
				}
				else
				{
					if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
				}

				if (Privileges.DelPriv.Equals("false"))
				{
					if (ToolBarDel != null) ToolBarDel.Enabled = false;
				}
				else
				{
					if (ToolBarDel != null) ToolBarDel.Enabled = true;
				}

				ToolBarPrint.Enabled = true;
				if (gvwCustomer.Rows.Count > 0) gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
			}

			if (ImageUploadPrivileges != null)
			{
				if (ImageUploadPrivileges.ViewPriv.Equals("true"))
				{
					if (ToolBarImageTypes != null)
						ToolBarImageTypes.Visible = true;
				}
				else
				{
					if (ToolBarImageTypes != null)
						ToolBarImageTypes.Visible = false;
				}
			}
			else
			{
				if (ToolBarImageTypes != null)
					ToolBarImageTypes.Visible = false;
			}
		}

		private void setTooltip(int rowIndex, CaseSnpEntity entity)
		{
			var toolTipText = "Added By    : " + entity.AddOperator.ToString().Trim() + " on " +
							entity.DateAdd.ToString() + "\n";
			var modifiedBy = string.Empty;
			if (!entity.LstcOperator.ToString().Trim().Equals(string.Empty))
				modifiedBy = entity.LstcOperator.ToString().Trim() + " on " + entity.DateLstc.ToString();
			toolTipText += "Modified By : " + modifiedBy;
			foreach (DataGridViewCell cell in gvwCustomer.Rows[rowIndex].Cells) cell.ToolTipText = toolTipText;
		}

		#region properties

		public BaseForm BaseForm { get; set; }

		public PrivilegeEntity Privileges { get; set; }

		public PrivilegeEntity CaseVerPrivileges { get; set; }

		public PrivilegeEntity CaseIncomePrivileges { get; set; }

		public PrivilegeEntity TmsSupplierPrivileges { get; set; }

		public PrivilegeEntity ImageUploadPrivileges { get; set; }

		public ToolBarButton ToolBarEdit { get; set; }

		public ToolBarButton ToolBarDel { get; set; } //ToolBarPrint

		//public ToolBarButton ToolBarNew { get; set; }

		public ToolBarButton ToolBarPrint { get; set; }

		public ToolBarButton ToolBarNewMember { get; set; }

		public ToolBarButton ToolBarImageTypes { get; set; }

		public ToolBarButton ToolBarHistory { get; set; }

		public ToolBarButton ToolBarHelp { get; set; }

		public ToolBarButton ToolBarCaseNotes { get; set; }

		public ToolBarButton ToolBarSignature { get; set; }

		public ToolBarButton ToolBarRecentSearch { get; set; }

		public string MainMenuAgency { get; set; }

		public string MainMenuDept { get; set; }

		public string MainMenuProgram { get; set; }

		public string MainMenuYear { get; set; }

		public string MainMenuAppNo { get; set; }

		public string MainMenuHIE { get; set; }

		public CaseMstEntity CaseMST { get; set; }

		public List<CaseSnpEntity> CaseSnpEntityProp { get; set; }

		public bool IsAddApplicant { get; set; }

		public List<CaseNotesEntity> caseNotesEntity { get; set; }

		public ProgramDefinitionEntity ProgramDefinition { get; set; }

		public string ApplicantLastName { get; set; }
		public bool IsDeleteApplicant { get; set; }
		public List<ZipCodeEntity> propzipCodeEntity { get; set; }
		public List<FldcntlHieEntity> preassesCntlEntity { get; set; }
		public AgencyControlEntity propAgencyControlDetails { get; set; }
		public List<CommonEntity> preassessMasterEntity { get; set; }
		public List<PreassessQuesEntity> preassessChildEntity { get; set; }
		private List<CustomQuestionsEntity> proppreassesQuestions { get; set; }
		public string propPreassStatus { get; set; }

		#endregion

		private void ClearControls()
		{
			// txtAlertCodes.Text = string.Empty;

			txtHN.Text = string.Empty;
			txtDirection.Text = string.Empty;
			txtSuffix.Text = string.Empty;
			txtStreet.Text = string.Empty;
			txtFloor.Text = string.Empty;
			txtPrecinct.Text = string.Empty;
			txtApartment.Text = string.Empty;
			cbActive.Checked = false;
			cbJuvenile.Checked = false;
			cbSecret.Checked = false;
			cbSenior.Checked = false;
			txtZipCode.Text = string.Empty;
			txtCity.Text = string.Empty;
			txtState.Text = string.Empty;
			txtSite.Text = string.Empty;
			CaseReviewDate.Text = string.Empty;

			SetComboBoxValue(cmbAboutUs, Consts.Common.SelectOne);
			SetComboBoxValue(cmbCaseType, Consts.Common.SelectOne);
			SetComboBoxValue(cmbContact, Consts.Common.SelectOne);
			SetComboBoxValue(cmbCounty, Consts.Common.SelectOne);
			SetComboBoxValue(cmbFamilyType, Consts.Common.SelectOne);
			SetComboBoxValue(cmbHousingSituation, Consts.Common.SelectOne);
			SetComboBoxValue(cmbHousingTx, Consts.Common.SelectOne);
			SetComboBoxValue(cmbPrimaryLang, Consts.Common.SelectOne);
			SetComboBoxValue(cmbSecondLang, Consts.Common.SelectOne);
			SetComboBoxValue(cmbStaff, Consts.Common.SelectOne);
			SetComboBoxValue(cmbTownship, Consts.Common.SelectOne);
			SetComboBoxValue(cmbWaitingList, Consts.Common.SelectOne);
			SetComboBoxValue(cmbOutService, "");
			txtHomePhone.Text = string.Empty;
			txtCell.Text = string.Empty;
			txtMessage.Text = string.Empty;
			txtTTY.Text = string.Empty;
			txtFax.Text = string.Empty;
			txtEmail.Text = string.Empty;
			dtpInitialDate.Text = string.Empty;
			dtpIntakeDate.Text = string.Empty;
			dtpIntakeDate.Text = string.Empty;

			txtRent.Text = string.Empty;
			txtHeating.Text = string.Empty;
			txtWater.Text = string.Empty;
			txtElectric.Text = string.Empty;
			txtExpand1.Text = string.Empty;
			txtExpand2.Text = string.Empty;
			txtExpand3.Text = string.Empty;
			txtExpand4.Text = string.Empty;
			txtTotalLiving.Text = string.Empty;
			txtTotalHouseHold.Text = string.Empty;

			txtFirst.Text = string.Empty;
			txtLast.Text = string.Empty;
			txtHouseNo.Text = string.Empty;
			txtCityName.Text = string.Empty;
			txtMailStreet.Text = string.Empty;
			txtMailZipCode.Text = string.Empty;
			txtMailZipPlus.Text = string.Empty;
			txtMailState.Text = string.Empty;
			txtMailSuffix.Text = string.Empty;
			txtMailApartment.Text = string.Empty;
			SetComboBoxValue(cmbMailCounty, Consts.Common.SelectOne);
			txtMailFloor.Text = string.Empty;
			txtMailPhone.Text = string.Empty;
		}

		private void GetSelectedProgram()
		{
			if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
			{
				var mainMenuControl = BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl;
				//MainMenuAgency = mainMenuControl.Agency;
				//MainMenuDept = mainMenuControl.Dept;
				//MainMenuProgram = mainMenuControl.Program;
				//MainMenuYear = mainMenuControl.ProgramYear;
				//MainMenuAppNo = mainMenuControl.ApplicationNo;

				MainMenuAgency = BaseForm.BaseAgency;
				MainMenuDept = BaseForm.BaseDept;
				MainMenuProgram = BaseForm.BaseProg;
				MainMenuYear = BaseForm.BaseYear;
				MainMenuAppNo = BaseForm.BaseApplicationNo;
			}
		}

		private void OnSearchClick(object sender, EventArgs e)
		{
			gvwCustomer.Rows.Clear();

			if (gvwCustomer.Rows.Count > 0) gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
		}

		private void gvwCustomer_SelectionChanged(object sender, EventArgs e)
		{
			if (gvwCustomer.SelectedRows.Count > 0)
			{
				var caseSnpEntity = GetSelectedRow();
				var caseMST = CaseMST;
				if (caseSnpEntity != null)
				{
					if (ToolBarDel != null) ToolBarDel.Visible = true;
					if (caseMST.FamilySeq.Equals(caseSnpEntity.FamilySeq))
					{
						if (gvwCustomer.Rows.Count == 1)
						{
							if (BaseForm.BaseAgencyControlDetails.DelAppSwitch == "Y")
							{
								if (CaseMST.AddOperator1.Trim() == BaseForm.UserID.Trim() ||
									BaseForm.UserProfile.Security.Trim() == "B" ||
									BaseForm.UserProfile.Security.Trim() == "P")
								{
									if (ToolBarDel != null) ToolBarDel.Visible = true;
								}
								else
								{
									if (ToolBarDel != null) ToolBarDel.Visible = false;
								}
							}
							else
							{
								if (BaseForm.UserProfile.Security.Trim() == "B" ||
									BaseForm.UserProfile.Security.Trim() == "P")
								{
									if (ToolBarDel != null) ToolBarDel.Visible = true;
								}
								else
								{
									if (ToolBarDel != null) ToolBarDel.Visible = false;
								}
							}
						}
						else
						{
							if (ToolBarDel != null) ToolBarDel.Visible = false;
						}
					}
				}

				setControlEnabled(false);
			}
		}

		public override void PopulateToolbar(ToolBar toolBar)
		{
			base.PopulateToolbar(toolBar);

			var toolbarButtonInitialized = ToolBarNewMember != null;
			var divider = new ToolBarButton();
			divider.Style = ToolBarButtonStyle.Separator;

			if (toolBar.Controls.Count == 0)
			{
				//ToolBarNew = new ToolBarButton();
				//ToolBarNew.Tag = "NewApp";
				//ToolBarNew.ToolTipText = "New Applicant";
				//ToolBarNew.Enabled = true;
				//ToolBarNew.ImageSource = "resource.wx/"+Consts.Icons16x16.AddItem);
				//ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
				//ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);


				ToolBarNewMember = new ToolBarButton();
				ToolBarNewMember.Tag = "NewMem";
				ToolBarNewMember.ToolTipText = "Add Household Member";
				ToolBarNewMember.Enabled = true;
				ToolBarNewMember.ImageSource = "captain-add";
				ToolBarNewMember.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarNewMember.Click += new EventHandler(OnToolbarButtonClicked);


				ToolBarEdit = new ToolBarButton();
				ToolBarEdit.Tag = "Edit";
				ToolBarEdit.ToolTipText = "Edit Member Details";
				ToolBarEdit.Enabled = true;
				ToolBarEdit.ImageSource = "captain-edit";
				ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);
				if (Privileges.ChangePriv.Equals("false")) ToolBarEdit.Enabled = false;

				ToolBarDel = new ToolBarButton();
				ToolBarDel.Tag = "Delete";
				ToolBarDel.ToolTipText = "Delete Member";
				ToolBarDel.Enabled = true;
				ToolBarDel.ImageSource = "captain-delete";
				ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

				ToolBarDel = new ToolBarButton();
				ToolBarDel.Tag = "Delete";
				ToolBarDel.ToolTipText = "Delete Member";
				ToolBarDel.Enabled = true;
				ToolBarDel.ImageSource = "captain-delete";
				ToolBarDel.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarDel.Click += new EventHandler(OnToolbarButtonClicked);

				ToolBarImageTypes = new ToolBarButton();
				ToolBarImageTypes.Tag = "ImageTypes";
				ToolBarImageTypes.ToolTipText = "Image Upload";
				ToolBarImageTypes.Enabled = true;
				ToolBarImageTypes.ImageSource = "captain-upload";
				ToolBarImageTypes.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarImageTypes.Click += new EventHandler(OnToolbarButtonClicked);


				ToolBarHistory = new ToolBarButton();
				ToolBarHistory.Tag = "History";
				ToolBarHistory.ToolTipText = "Case History";
				ToolBarHistory.Enabled = true;
				ToolBarHistory.ImageSource = "captain-caseHistory";
				ToolBarHistory.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarHistory.Click += new EventHandler(OnToolbarButtonClicked);


				ToolBarCaseNotes = new ToolBarButton();
				ToolBarCaseNotes.Tag = "CaseNotes";
				ToolBarCaseNotes.ToolTipText = "Case Notes";
				ToolBarCaseNotes.Enabled = true;
				ToolBarCaseNotes.ImageSource = "captain-notes";
				ToolBarCaseNotes.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarCaseNotes.Click += new EventHandler(OnToolbarButtonClicked);

				ToolBarPrint = new ToolBarButton();
				ToolBarPrint.Tag = "Print";
				ToolBarPrint.ToolTipText = "Print Form";
				ToolBarPrint.Enabled = true;
				ToolBarPrint.ImageSource = "captain-print";
				ToolBarPrint.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarPrint.Click += new EventHandler(OnToolbarButtonClicked);

				//ToolBarSignature = new ToolBarButton();
				//ToolBarSignature.Tag = "Signature";
				//ToolBarSignature.ToolTipText = "Signature Form";
				//ToolBarSignature.Enabled = true;
				//ToolBarSignature.ImageSource = "resource.wx/"+Consts.Icons16x16.Signature);
				//ToolBarSignature.Click -= new EventHandler(OnToolbarButtonClicked);
				//ToolBarSignature.Click += new EventHandler(OnToolbarButtonClicked);

				//ToolBarSignature = new ToolBarButton();
				//ToolBarSignature.Tag = "RecentSearch";
				//ToolBarSignature.ToolTipText = "Recent Applicant";
				//ToolBarSignature.Enabled = true;
				//ToolBarSignature.ImageSource = "resource.wx/"+Consts.Icons16x16.RecentSearch);
				//ToolBarSignature.Click -= new EventHandler(OnToolbarButtonClicked);
				//ToolBarSignature.Click += new EventHandler(OnToolbarButtonClicked);


				ToolBarHelp = new ToolBarButton();
				ToolBarHelp.Tag = "Help";
				ToolBarHelp.ToolTipText = "Help";
				ToolBarHelp.Enabled = true;
				ToolBarHelp.ImageSource = "icon-help";
				ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
				ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
			}

			if (Privileges.AddPriv.Equals("false"))
			{
				//if (ToolBarNew != null) ToolBarNew.Enabled = false;
				if (ToolBarNewMember != null) ToolBarNewMember.Enabled = false;
			}
			else
			{
				// if (ToolBarNew != null) ToolBarNew.Enabled = true;
				if (ToolBarNewMember != null) ToolBarNewMember.Enabled = true;
			}

			if (Privileges.ChangePriv.Equals("false"))
			{
				if (ToolBarEdit != null) ToolBarEdit.Enabled = false;
			}
			else
			{
				if (ToolBarEdit != null) ToolBarEdit.Enabled = true;
			}


			if (Privileges.DelPriv.Equals("false"))
			{
				if (ToolBarDel != null) ToolBarDel.Enabled = false;
			}
			else
			{
				if (ToolBarDel != null) ToolBarDel.Enabled = true;
			}

			ShowCaseNotesImages();
			toolBar.Show();
			toolBar.Buttons.AddRange(new ToolBarButton[]
			{
				// ToolBarNew,
				ToolBarNewMember,
				ToolBarEdit,
				ToolBarDel,
				ToolBarImageTypes,
				ToolBarHistory,
				ToolBarPrint,
				ToolBarCaseNotes,
				ToolBarSignature,
				ToolBarHelp
			});
		}

		/// <summary>
		/// Handles the toolbar button clicked event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnToolbarButtonClicked(object sender, EventArgs e)
		{
			var btn = (ToolBarButton) sender;
			var executeCode = new StringBuilder();

			executeCode.Append(Consts.Javascript.BeginJavascriptCode);
			if (btn.Tag == null) return;
			try
			{
				switch (btn.Tag.ToString())
				{
					case "NewApp":
						if (CheckCasedepprivileage())
						{
							var caseMSTEntity = new CaseMstEntity();
							caseMSTEntity.ApplAgency = MainMenuAgency;
							caseMSTEntity.ApplDept = MainMenuDept;
							caseMSTEntity.ApplProgram = MainMenuProgram;
							caseMSTEntity.ApplYr = MainMenuYear;
							BaseForm.RemoveTabPages("Client");
							if (Privileges.Program.ToString().ToUpper() == "CASE2002")
							{
								using (var clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, null,
																	Consts.Common.Add, Privileges, CaseSnpEntityProp,
																	ApplicantLastName))
								{
									clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
									clientSNPForm.ShowDialog();
								}
							}
							else
							{
								if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
								{
									using (var clientSNPForm = new Case4001FormDEMO(
										BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges,
										CaseSnpEntityProp, ApplicantLastName))
									{
										clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
										clientSNPForm.ShowDialog();
									}
								}
								else
								{
									using (var clientSNPForm = new Case3001Form(
										BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges,
										CaseSnpEntityProp, ApplicantLastName))
									{
										clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
										clientSNPForm.ShowDialog();
									}
								}
							}
						}
						else
						{
							CommonFunctions.MessageBoxDisplay("Cannot add to prior program year");
						}

						break;
					case "NewMem":
						var caseMST = CaseMST;
						if (CheckCasedepprivileage())
						{
							if (Privileges.Program.ToString().ToUpper() == "CASE2002")
							{
								using (var clientSNPMemForm = new Case3001Form(BaseForm, false, caseMST, null,
																		Consts.Common.Add, Privileges,
																		CaseSnpEntityProp, ApplicantLastName))
								{
									clientSNPMemForm.StartPosition = FormStartPosition.CenterScreen;
									clientSNPMemForm.ShowDialog();
								}
							}
							else
							{
								if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() ==
									"Y") //(BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OK" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SCCAP")
								{
									using (var clientSNPMemForm = new Case4001FormDEMO(
										BaseForm, false, caseMST, null, Consts.Common.Add, Privileges,
										CaseSnpEntityProp, ApplicantLastName))
									{
										clientSNPMemForm.StartPosition = FormStartPosition.CenterScreen;
										clientSNPMemForm.ShowDialog();
									}
								}
								else
								{
									using (var clientSNPMemForm = new Case3001Form(
										BaseForm, false, caseMST, null, Consts.Common.Add, Privileges,
										CaseSnpEntityProp, ApplicantLastName))
									{
										clientSNPMemForm.StartPosition = FormStartPosition.CenterScreen;
										clientSNPMemForm.ShowDialog();
									}
								}
							}
						}
						else
						{
							CommonFunctions.MessageBoxDisplay("Cannot add to prior program year");
						}

						break;
					case Consts.ToolbarActions.Edit:
						var caseSnp = GetSelectedRow();
						caseMST = CaseMST;
						var isApplicant = false;
						if (caseMST.FamilySeq.Equals(caseSnp.FamilySeq)) isApplicant = true;
						if (CheckCasedepprivileage())
						{
							if (Privileges.Program.ToString().ToUpper() == "CASE2002")
							{
								using (var clientSNPEditForm =
									new Case3001Form(BaseForm, isApplicant, caseMST, caseSnp, Consts.Common.Edit,
													Privileges, CaseSnpEntityProp, ApplicantLastName))
								{
									clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
									clientSNPEditForm.ShowDialog();
								}
							}
							else
							{
								if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
								{
									using (var clientSNPEditForm = new Case4001FormDEMO(
										BaseForm, isApplicant, caseMST, caseSnp, Consts.Common.Edit, Privileges,
										CaseSnpEntityProp, ApplicantLastName))
									{
										clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
										clientSNPEditForm.ShowDialog();
									}
								}
								else
								{
									using (var clientSNPEditForm = new Case3001Form(
										BaseForm, isApplicant, caseMST, caseSnp, Consts.Common.Edit, Privileges,
										CaseSnpEntityProp, ApplicantLastName))
									{
										clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
										clientSNPEditForm.ShowDialog();
									}
								}
							}
						}
						else
						{
							CommonFunctions.MessageBoxDisplay("Cannot edit to prior program year");
						}

						break;
					case Consts.ToolbarActions.Delete:
						caseSnp = GetSelectedRow();
						caseMST = CaseMST;
						if (CheckCasedepprivileage())
						{
							if (caseSnp != null)
							{
								IsDeleteApplicant = false;
								isApplicant = false;
								if (caseMST.FamilySeq.Equals(caseSnp.FamilySeq)) isApplicant = true;
								if (isApplicant)
								{
									IsDeleteApplicant = true;
									if (gvwCustomer.Rows.Count == 1)
									{
										if (BaseForm.UserProfile.Security.ToUpper() == "P" ||
											BaseForm.UserProfile.Security.ToUpper() == "B")
										{
											var snpName = caseSnp.NameixFi.Trim() + " " + caseSnp.NameixLast.Trim();
											MessageBox.Show("Are you sure you want to delete " + snpName + " record?",
															Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo,
															MessageBoxIcon.Question, onclose: MessageBoxHandler);
										}
										else
										{
											if (BaseForm.BaseAgencyControlDetails.DelAppSwitch == "Y")
											{
												if (CaseMST.AddOperator1.Trim() == BaseForm.UserID.Trim())
												{
													var snpName = caseSnp.NameixFi.Trim() + " " +
																caseSnp.NameixLast.Trim();
													MessageBox.Show(
														"Are you sure you want to delete " + snpName + " record?",
														Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo,
														MessageBoxIcon.Question, onclose: MessageBoxHandler);
												}
											}
											else
											{
												CommonFunctions.MessageBoxDisplay(
													"Program Administrator can only delete an applicant");
											}
										}
									}
								}
								else
								{
									var snpName = caseSnp.NameixFi.Trim() + " " + caseSnp.NameixLast.Trim();
									MessageBox.Show("Are you sure you want to delete " + snpName + " record?",
													Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo,
													MessageBoxIcon.Question, onclose: MessageBoxHandler);
								}
							}
						}
						else
						{
							CommonFunctions.MessageBoxDisplay("Cannot delete to prior program year");
						}

						break;
					case Consts.ToolbarActions.ImageTypes:
						// caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(Privileges.Program, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
						using (var imageupload =
							new ImageUpload(BaseForm, ImageUploadPrivileges, ProgramDefinition, "CASE2001"))
						{
							//caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
							imageupload.StartPosition = FormStartPosition.CenterScreen;
							imageupload.ShowDialog();
						}
						break;

					case Consts.ToolbarActions.History:
						var caseHistSnp = GetSelectedRow();
						if (caseHistSnp != null)
						{
							using (var historyForm = new HistoryForm(BaseForm, Privileges, caseHistSnp))
							{
								historyForm.StartPosition = FormStartPosition.CenterScreen;
								historyForm.ShowDialog();
							}
						}

						break;

					case Consts.ToolbarActions.CaseNotes:
						if (BaseForm.BusinessModuleID == "05")
						{
							caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(
								Privileges.Program,
								BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear +
								BaseForm.BaseApplicationNo);
							using (var Prog_Form = new ProgressNotes_Form(BaseForm, "View", Privileges,
																	BaseForm.BaseAgency + BaseForm.BaseDept +
																	BaseForm.BaseProg + strYear +
																	BaseForm
																		.BaseApplicationNo))
							{  //+ "0000".Substring(0, (4 - Pass_Entity.Seq.Length)) + Pass_Entity.Seq);
								Prog_Form.StartPosition = FormStartPosition.CenterScreen;
								Prog_Form.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
								Prog_Form.ShowDialog();
							}
						}
						else
						{
							caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(
								Privileges.Program,
								BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear +
								BaseForm.BaseApplicationNo);
							using (var caseNotes = new CaseNotes(BaseForm, Privileges, caseNotesEntity,
														BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg +
														strYear + BaseForm.BaseApplicationNo))
							{
								caseNotes.StartPosition = FormStartPosition.CenterScreen;
								caseNotes.FormClosed += new FormClosedEventHandler(OnCaseNotesFormClosed);
								caseNotes.ShowDialog();
							}
						}

						break;
					case Consts.ToolbarActions.Print:
						using (var PrintAppl = new PrintApplicants(BaseForm, Privileges, "Case2001"))
						{
							PrintAppl.StartPosition = FormStartPosition.CenterScreen;
							PrintAppl.ShowDialog();
						}
						//On_SaveFormClosed();
						break;
					case Consts.ToolbarActions.RecentSearch:

						var snpApplicant =
							BaseForm.BaseCaseSnpEntity.Find(
								u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);

						var ds = DatabaseLayer.MainMenu.MainMenuSearchEMS(
							"APP", "ALL", null, null, null, snpApplicant.NameixLast, snpApplicant.NameixFi, null, null,
							null, null, null, null, null, null, null, snpApplicant.AltBdate, BaseForm.UserID, "Single",
							string.Empty, string.Empty);

						if (ds.Tables[0].Rows.Count > 0)
						{
							if (ds.Tables[0].Rows[0]["AGENCY"].ToString() == BaseForm.BaseAgency &&
								ds.Tables[0].Rows[0]["DEPT"].ToString() == BaseForm.BaseDept &&
								ds.Tables[0].Rows[0]["PROG"].ToString() == BaseForm.BaseProg &&
								ds.Tables[0].Rows[0]["SnpYear"].ToString().Trim() == BaseForm.BaseYear.Trim() &&
								ds.Tables[0].Rows[0]["APPLICANTNO"].ToString() == BaseForm.BaseApplicationNo)
							{
								CommonFunctions.MessageBoxDisplay("This Applicant is recent record");
							}
							else
							{
								//PIPUpdateApplicantForm pipupdateForm = new Forms.PIPUpdateApplicantForm(BaseForm, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
								////pipupdateForm.FormClosed += new FormClosedEventHandler(PipupdateForm_FormClosed);
								//pipupdateForm.ShowDialog();
							}
						}

						break;
					//case Consts.ToolbarActions.Signature:
					//    string strSignYear = "YYYY";
					//    if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
					//    {
					//        strSignYear = BaseForm.BaseYear;
					//    }
					//    PdfViewerNewForm Signature = new PdfViewerNewForm(string.Empty, BaseForm.BaseAgency.ToString() + BaseForm.BaseDept.ToString() + BaseForm.BaseProg.ToString() + strSignYear + BaseForm.BaseApplicationNo.ToString());
					//    Signature.ShowDialog();

					//    break;
					case Consts.ToolbarActions.Help:
						//Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE2001");
						break;
				}

				executeCode.Append(Consts.Javascript.EndJavascriptCode);
			}
			catch (Exception ex)
			{
				ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None,
															ExceptionSeverityLevel.High);
			}
		}

		private bool CheckCasedepprivileage()
		{
			var boolprivilege = true;
			if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
				switch (BaseForm.UserProfile.Security)
				{
					case "R":
					case "C":
						if (ProgramDefinition.DepYear != BaseForm.BaseYear.Trim()) boolprivilege = false;
						break;
				}

			return boolprivilege;
		}

		private void MessageBoxHandler(DialogResult dialogResult)
		{
			// Set DialogResult value of the Form as a text for label
			if (dialogResult == DialogResult.Yes)
			{
				var boolDeleteObo = true;
				var boolDeleteCAObo = true;
				var caseSnp = GetSelectedRow();
				var search_Act_Details = new CASEACTEntity(true);
				var Search_MS_Details = new CASEMSEntity(true);
				Search_MS_Details.Agency = caseSnp.Agency;
				Search_MS_Details.Dept = caseSnp.Dept;
				Search_MS_Details.Program = caseSnp.Program;
				Search_MS_Details.Year = caseSnp.Year; // Year will be always Four-Spaces in CASEMS
				Search_MS_Details.App_no = caseSnp.App;

				search_Act_Details.Agency = caseSnp.Agency;
				search_Act_Details.Dept = caseSnp.Dept;
				search_Act_Details.Program = caseSnp.Program;
				search_Act_Details.Year = caseSnp.Year; // Year will be always Four-Spaces in CASEMS
				search_Act_Details.App_no = caseSnp.App;

				var Tmp_SP_ACT_Details = _model.SPAdminData.Browse_CASEACT(search_Act_Details, "Browse");
				if (Tmp_SP_ACT_Details.Count > 0)
				{
					var Search_CAOBO_Entity = new CAOBOEntity();
					Search_CAOBO_Entity.ID = Tmp_SP_ACT_Details[0].ACT_ID;
					Search_CAOBO_Entity.Fam_Seq = caseSnp.FamilySeq;
					Search_CAOBO_Entity.Seq = Search_CAOBO_Entity.CLID = null;

					var CAOBO_List = _model.SPAdminData.Browse_CAOBO(Search_CAOBO_Entity, "Browse");
					if (CAOBO_List.Count > 0)
						boolDeleteCAObo = false;
				}

				var Tmp_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_MS_Details, "Browse");
				if (Tmp_SP_MS_Details.Count > 0)
				{
					var Search_OBO_Entity = new CASEMSOBOEntity();
					Search_OBO_Entity.ID = Tmp_SP_MS_Details[0].ID;
					Search_OBO_Entity.Fam_Seq = caseSnp.FamilySeq;
					Search_OBO_Entity.Seq = Search_OBO_Entity.CLID = null;

					var CASEMSOBO_List = _model.SPAdminData.Browse_CASEMSOBO(Search_OBO_Entity, "Browse");
					if (CASEMSOBO_List.Count > 0)
						boolDeleteObo = false;
				}

				if (boolDeleteCAObo)
				{
					if (boolDeleteObo)
					{
						var strMsg = _model.CaseMstData.DeleteCaseSNP(caseSnp, BaseForm.UserID);
						if (strMsg == "Success")
						{
							if (strIndex != 0)
								strIndex = strIndex - 1;
							//Refresh();   

							CaseMstEntity caseMstEntity = null;
							List<CaseSnpEntity> caseSnpEntity = null;
							var strYear1 = BaseForm.BaseYear;

							if (string.IsNullOrEmpty(BaseForm.BaseYear))
								strYear1 = "    ";


							var strAgencyName = BaseForm.BaseAgency + " - " +
												_model.lookupDataAccess.GetHierachyDescription(
													"1", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
							var strDeptName = BaseForm.BaseDept + " - " +
											_model.lookupDataAccess.GetHierachyDescription(
												"2", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
							var strProgName = BaseForm.BaseProg + " - " +
											_model.lookupDataAccess.GetHierachyDescription(
												"3", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
							if (gvwCustomer.Rows.Count == 0)
							{
								caseMstEntity = _model.CaseMstData.GetCaseMST(
									BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1, string.Empty);
								var caseMstList = new List<CaseMstEntity>();
								caseMstList.Add(caseMstEntity);
								BaseForm.BaseCaseMstListEntity = caseMstList;
							}
							else
							{
								caseMstEntity = _model.CaseMstData.GetCaseMST(
									BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
									BaseForm.BaseApplicationNo);
							}

							if (caseMstEntity != null)
							{
								BaseForm.BaseApplicationNo = caseMstEntity.ApplNo;
								;
								caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(
									BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1,
									BaseForm.BaseApplicationNo);
							}
							else
							{
								caseMstEntity = _model.CaseMstData.GetCaseMST(
									BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1, string.Empty);
								var caseMstList = new List<CaseMstEntity>();
								caseMstList.Add(caseMstEntity);
								BaseForm.BaseCaseMstListEntity = caseMstList;
								if (caseMstEntity != null)
								{
									BaseForm.BaseApplicationNo = caseMstEntity.ApplNo;
									;
									caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(
										BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strYear1,
										BaseForm.BaseApplicationNo);
								}
								else
								{
									BaseForm.BaseApplicationNo = null;
								}
							}

							BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, strAgencyName, strDeptName,
														strProgName, strYear.ToString(), string.Empty, "Display");
							BaseForm.BaseTopApplSelect = "Y";
							Refresh();
							if (IsDeleteApplicant)
							{
								var mainMenuControl = BaseForm.GetBaseUserControlMainMenu() as MainMenuControl;
								if (mainMenuControl != null) mainMenuControl.RefreshMainMenuClientIntake();
							}
						}
						else if (strMsg == "Dependency")
						{
							MessageBox.Show("You can’t delete this member, as there are Dependents",
											Consts.Common.ApplicationCaption, MessageBoxButtons.OK,
											MessageBoxIcon.Information);
						}
						else if (strMsg == "CASEACT")
						{
							MessageBox.Show("'You can't delete this member, as there are CA Postings",
											Consts.Common.ApplicationCaption, MessageBoxButtons.OK,
											MessageBoxIcon.Information);
						}
						else if (strMsg == "CASEMS")
						{
							MessageBox.Show("'You can't delete this member, as there are MS Postings",
											Consts.Common.ApplicationCaption, MessageBoxButtons.OK,
											MessageBoxIcon.Information);
						}
						else if (strMsg == "CASESPM")
						{
							MessageBox.Show("'You can't delete this member, as there are Service Plan Master",
											Consts.Common.ApplicationCaption, MessageBoxButtons.OK,
											MessageBoxIcon.Information);
						}
						else if (strMsg == "CASECONT")
						{
							MessageBox.Show("'You can't delete this member, as there are Postings in CASECONT",
											Consts.Common.ApplicationCaption, MessageBoxButtons.OK,
											MessageBoxIcon.Information);
						}
						else if (strMsg == "LIHEAPV")
						{
							MessageBox.Show(
								"'You can't delete this member, as there are vendors in the Supplier Screen",
								Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else if (strMsg == "LIHEAPB")
						{
							MessageBox.Show(
								"You can’t delete this member, as there are records in Control Card - Benefit Maintenance Screen",
								Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else if (strMsg == "CHLDMST")
						{
							MessageBox.Show(
								"You can't delete this member, as there are records in Medical/Emergency Screen",
								Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else if (strMsg == "CASEENRL")
						{
							MessageBox.Show(
								"You can’t delete this member, as there are records in Enrollment/Withdrawals Screen",
								Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else if (strMsg == "MATASMT")
						{
							MessageBox.Show(
								"You can’t delete this member, as there are records in Matrix Assessment Screen",
								Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else if (strMsg == "EMSRES")
						{
							MessageBox.Show(
								"You can’t delete this member, as there are records in Budget Resource Screen",
								Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					else
					{
						MessageBox.Show("You can’t delete this member, as there are MS Postings and OBO",
										Consts.Common.ApplicationCaption, MessageBoxButtons.OK,
										MessageBoxIcon.Information);
					}
				}
				else
				{
					MessageBox.Show("You can’t delete this member, as there are CA Postings and OBO",
									Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void fillClientIntake()
		{
			try
			{
				if (BaseForm.BaseCaseMstListEntity != null)
				{
					var caseMSTEntity = BaseForm.BaseCaseMstListEntity[0];
					if (caseMSTEntity != null)
					{
						// btnAlertCodes.Enabled = true;
						btnIncomeDetails.Enabled = true;
						btnIncVerfication.Enabled = true;
						CaseMST = caseMSTEntity;
						decimal totIncome = 0;
						decimal programIncome = 0;
						MainMenuAppNo = caseMSTEntity.ApplNo;
						CaseSnpEntityProp = BaseForm.BaseCaseSnpEntity;
						if (CaseSnpEntityProp != null)
						{
							var Relation = _model.lookupDataAccess.GetRelationship();
							foreach (var snpEntity in CaseSnpEntityProp)
								if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq))
								{
									ApplicantLastName = snpEntity.NameixLast;
									var ApplicantName = LookupDataAccess.GetMemberName(
										snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast,
										strNameFormat); //snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();
									var DOB = string.Empty;
									var SSNum = LookupDataAccess.GetCardNo(
										snpEntity.Ssno, "1", ProgramDefinition.SSNReasonFlag.Trim(),
										snpEntity.SsnReason);
									if (!snpEntity.AltBdate.Equals(string.Empty))
										DOB = CommonFunctions.ChangeDateFormat(
											snpEntity.AltBdate, Consts.DateTimeFormats.DateSaveFormat,
											Consts.DateTimeFormats.DateDisplayFormat);
									// if (!snpEntity.Ssno.Equals(string.Empty))
									//     SSNum = "nn-nnnn-" + snpEntity.Ssno.Substring(5, 4);

									var memberCode = string.Empty;
									var rel = Relation.Find(u => u.Code.Equals(snpEntity.MemberCode));
									if (rel != null) memberCode = rel.Desc;
									var cellTotIncome = !snpEntity.TotIncome.Equals(string.Empty)
										? snpEntity.TotIncome
										: "0.0";
									var cellProgIncome = !snpEntity.ProgIncome.Equals(string.Empty)
										? snpEntity.ProgIncome
										: "0.0";

									var rowIndex = gvwCustomer.Rows.Add(ApplicantName, SSNum, DOB, cellTotIncome,
																		cellProgIncome, memberCode, string.Empty);
									gvwCustomer.Rows[rowIndex].Tag = snpEntity;
									if (snpEntity.Status.Trim() != "A")
										gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
									if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) &&
										snpEntity.Status.Trim() == "A")
										gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor =
											Color.FromName("@captainBlue");
									if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) &&
										snpEntity.Status.Trim() != "A")
										gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Blue;
									if (snpEntity.Exclude == "Y")
										gvwCustomer.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

									if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) &&
										caseMSTEntity.ActiveStatus.Trim() != "Y")
										gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Red;
									if (!snpEntity.TotIncome.Equals(string.Empty))
										totIncome += decimal.Parse(snpEntity.TotIncome);
									if (!snpEntity.ProgIncome.Equals(string.Empty))
										programIncome += decimal.Parse(snpEntity.ProgIncome);
									//if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq))
									//{
									//    gvwCustomer.Rows[rowIndex].Cells["cellEnd"].Value = "casemst";
									//}
									//else
									//{
									//    gvwCustomer.Rows[rowIndex].Cells["cellEnd"].Value = "casesnp";
									//}
									//if (snpEntity.FamilySeq.Equals("1"))
									setTooltip(rowIndex, snpEntity);
								}


							foreach (var snpEntity in CaseSnpEntityProp)
								if (!caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq))
								{
									var ApplicantName = LookupDataAccess.GetMemberName(
										snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast,
										strNameFormat); //snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();
									var DOB = string.Empty;
									var SSNum = LookupDataAccess.GetCardNo(
										snpEntity.Ssno, "1", ProgramDefinition.SSNReasonFlag.Trim(),
										snpEntity.SsnReason);

									if (!snpEntity.AltBdate.Equals(string.Empty))
										DOB = CommonFunctions.ChangeDateFormat(
											snpEntity.AltBdate, Consts.DateTimeFormats.DateSaveFormat,
											Consts.DateTimeFormats.DateDisplayFormat);
									//if (!snpEntity.Ssno.Equals(string.Empty))
									//    SSNum = "nn-nnnn-" + snpEntity.Ssno.Substring(5, 4);

									var memberCode = string.Empty;
									var rel = Relation.Find(u => u.Code.Equals(snpEntity.MemberCode));
									if (rel != null) memberCode = rel.Desc;
									var cellTotIncome = !snpEntity.TotIncome.Equals(string.Empty)
										? snpEntity.TotIncome
										: "0.0";
									var cellProgIncome = !snpEntity.ProgIncome.Equals(string.Empty)
										? snpEntity.ProgIncome
										: "0.0";

									var rowIndex = gvwCustomer.Rows.Add(ApplicantName, SSNum, DOB, cellTotIncome,
																		cellProgIncome, memberCode, string.Empty);
									gvwCustomer.Rows[rowIndex].Tag = snpEntity;
									if (snpEntity.Status.Trim() != "A")
										gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
									if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) &&
										snpEntity.Status.Trim() == "A")
										gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
									if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) &&
										snpEntity.Status.Trim() != "A")
										gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Blue;
									if (snpEntity.Exclude == "Y")
										gvwCustomer.Rows[rowIndex].Cells["Relation"].Style.ForeColor = Color.Red;

									if (caseMSTEntity.FamilySeq.Equals(snpEntity.FamilySeq) &&
										caseMSTEntity.ActiveStatus.Trim() != "Y")
										gvwCustomer.Rows[rowIndex].Cells["AppName"].Style.ForeColor = Color.Red;

									if (!snpEntity.TotIncome.Equals(string.Empty))
										totIncome += decimal.Parse(snpEntity.TotIncome);
									if (!snpEntity.ProgIncome.Equals(string.Empty))
										programIncome += decimal.Parse(snpEntity.ProgIncome);
									//if (snpEntity.FamilySeq.Equals("1"))
									setTooltip(rowIndex, snpEntity);
								}

							txtTotalIncome.Text =
								caseMSTEntity.FamIncome.Equals("0")
									? "0.0"
									: caseMSTEntity.FamIncome; //totIncome.ToString();
							txtProgramIncome.Text = caseMSTEntity.ProgIncome.ToString().Equals("0")
								? "0.0"
								: caseMSTEntity.ProgIncome.ToString();
							txtInProg.Text = caseMSTEntity.NoInProg.ToString();
							txtInHouse.Text = caseMSTEntity.NoInhh.ToString();
							var casesnp = CaseSnpEntityProp.Find(u => u.FamilySeq == CaseMST.FamilySeq);

							propPreassStatus = "A";
							fillPreassCustomQuestions(casesnp, string.Empty);
							var boolinactiveQuestions = false;
							foreach (var gvrows in gvwPreassesData.Rows)
								if ((gvrows.Cells["gvtPQuestions"].Tag == null
										? string.Empty
										: gvrows.Cells["gvtPQuestions"].Tag.ToString()) == "Y")
								{
									boolinactiveQuestions = true;
									break;
								}

							if (boolinactiveQuestions)
							{
								fillPreassCustomQuestions(casesnp, "I");
								propPreassStatus = "I";
							}
							else
							{
								fillPreassCustomQuestions(casesnp, "A");
							}
						}

						ClearControls();
						// txtAlertCodes.Text = caseMSTEntity.AlertCodes;
						txtHN.Text = caseMSTEntity.Hn.Trim();
						txtDirection.Text = caseMSTEntity.Direction.Trim();
						txtSuffix.Text = caseMSTEntity.Suffix.Trim();
						txtStreet.Text = caseMSTEntity.Street.Trim();
						txtFloor.Text = caseMSTEntity.Flr;
						if (caseMSTEntity.ProgIncome.ToString() != string.Empty)
						{
							var MonthlyIncome = double.Parse(CaseMST.ProgIncome);
							MonthlyIncome = MonthlyIncome / 12;
							MonthlyIncome = Math.Round(MonthlyIncome, 2);
							txtMonthlyIncome.Text = MonthlyIncome.ToString();
						}
						else
						{
							txtMonthlyIncome.Text = "0";
						}

						txtPrecinct.Text = caseMSTEntity.Precinct;
						txtApartment.Text = caseMSTEntity.Apt;
						if (caseMSTEntity.ActiveStatus.Equals("Y")) cbActive.Checked = true;
						else cbActive.Checked = false;
						if (caseMSTEntity.Juvenile.Equals("Y")) cbJuvenile.Checked = true;
						else cbJuvenile.Checked = false;
						if (caseMSTEntity.Secret.Equals("Y")) cbSecret.Checked = true;
						else cbSecret.Checked = false;
						if (caseMSTEntity.Senior.Equals("Y")) cbSenior.Checked = true;
						else cbSenior.Checked = false;
						var strZip = "00000".Substring(0, 5 - caseMSTEntity.Zip.Length) + caseMSTEntity.Zip;
						var strZipPlus = "0000".Substring(0, 4 - caseMSTEntity.Zipplus.Length) + caseMSTEntity.Zipplus;
						txtZipCode.Text = strZip + strZipPlus;
						txtCity.Text = caseMSTEntity.City.Trim();
						txtState.Text = caseMSTEntity.State;
						txtSite.Text = caseMSTEntity.Site.Trim();

						SetComboBoxValue(cmbAboutUs, caseMSTEntity.AboutUs);
						SetComboBoxValue(cmbCaseType, caseMSTEntity.CaseType);
						SetComboBoxValue(cmbContact, caseMSTEntity.BestContact);
						SetComboBoxValue(cmbCounty, caseMSTEntity.County);
						SetComboBoxValue(cmbFamilyType, caseMSTEntity.FamilyType);
						FamilyTypeWaringMsg(caseMSTEntity.FamilyType);
						SetComboBoxValue(cmbHousingSituation, caseMSTEntity.Housing);
						SetComboBoxValue(cmbHousingTx, caseMSTEntity.Housing);
						SetComboBoxValue(cmbPrimaryLang, caseMSTEntity.Language);
						SetComboBoxValue(cmbSecondLang, caseMSTEntity.LanguageOt);
						SetComboBoxValue(cmbStaff, caseMSTEntity.IntakeWorker);
						SetComboBoxValue(cmbTownship, caseMSTEntity.TownShip);
						SetComboBoxValue(cmbWaitingList, caseMSTEntity.WaitList);
						if (propAgencyControlDetails.State.ToUpper() == "TX")
							SetComboBoxValue(cmbOutService, caseMSTEntity.OutOfService);
						SetComboBoxValue(cmbVerifiedStaff, caseMSTEntity.ExpCaseWorker);
						// string phone = ("0000000000" + caseMSTEntity.Phone);
						txtHomePhone.Text = caseMSTEntity.Area + caseMSTEntity.Phone;
						txtCell.Text = caseMSTEntity.CellPhone;
						txtMessage.Text = caseMSTEntity.MessagePhone;
						txtTTY.Text = caseMSTEntity.TtyNumber;
						txtFax.Text = caseMSTEntity.FaxNumber;
						txtEmail.Text = caseMSTEntity.Email.Trim();

						if (!caseMSTEntity.InitialDate.Equals(string.Empty))
							dtpInitialDate.Text = CommonFunctions.ChangeDateFormat(
								caseMSTEntity.InitialDate, Consts.DateTimeFormats.DateSaveFormat,
								Consts.DateTimeFormats.DateDisplayFormat);
						if (!caseMSTEntity.IntakeDate.Equals(string.Empty))
							dtpIntakeDate.Text = CommonFunctions.ChangeDateFormat(
								caseMSTEntity.IntakeDate, Consts.DateTimeFormats.DateSaveFormat,
								Consts.DateTimeFormats.DateDisplayFormat);
						if (!caseMSTEntity.CaseReviewDate.Equals(string.Empty))
							CaseReviewDate.Text = CommonFunctions.ChangeDateFormat(
								caseMSTEntity.CaseReviewDate, Consts.DateTimeFormats.DateSaveFormat,
								Consts.DateTimeFormats.DateDisplayFormat);
						else
							CaseReviewDate.Text = string.Empty;
						txtRent.Text = caseMSTEntity.ExpRent;
						txtHeating.Text = caseMSTEntity.ExpHeat;
						txtWater.Text = caseMSTEntity.ExpWater;
						txtElectric.Text = caseMSTEntity.ExpElectric;
						txtExpand1.Text = caseMSTEntity.Debtcc;
						txtExpand2.Text = caseMSTEntity.DebtLoans;
						txtExpand3.Text = caseMSTEntity.DebtMed;
						txtExpand4.Text = caseMSTEntity.DebtOther;
						txtTotalLiving.Text = caseMSTEntity.ExpLivexpense;
						txtTotalHouseHold.Text = caseMSTEntity.ExpTotal; //ExpCaseWorker
						txtMiscExpenses.Text = caseMSTEntity.ExpMisc;
						txtMiscDebt.Text = caseMSTEntity.DebtMisc;
						txtMiscAssets.Text = caseMSTEntity.AsetMisc;
						txtPhysicalAssets.Text = caseMSTEntity.AsetPhy;
						txtOtherAssets.Text = caseMSTEntity.AsetOth;
						txtLiquidAssets.Text = caseMSTEntity.AsetLiq;
						txtTotalAssets.Text = caseMSTEntity.AsetTotal;
						txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio;
						txtTotalHHDebt.Text = caseMSTEntity.DebtTotal;
						txtDebtIncomeRatio.Text = caseMSTEntity.DebIncmRation;
						// txtTotalHouseholdIncome.Text = caseMSTEntity.; 

						CalclationEmployee();
						txtNoOfYears.Text = caseMSTEntity.AddressYears;

						SetComboBoxValue(cmbDwelling, caseMSTEntity.Dwelling);
						SetComboBoxValue(cmbPMOPfHeat, caseMSTEntity.HeatIncRent);
						SetComboBoxValue(cmbPrimarySourceoHeat, caseMSTEntity.Source);
						chkSubsidized.Checked = caseMSTEntity.SubShouse == "Y" ? true : false;
						SetComboBoxValue(cmbSubsidized, caseMSTEntity.SubStype);


						var caseDiffDetails = _model.CaseMstData.GetCaseDiffadpya(
							BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(),
							caseMSTEntity.ApplYr, caseMSTEntity.ApplNo, string.Empty);
						if (caseDiffDetails != null)
						{
							tabPageService.Hidden = true;
							txtCityName.Text = caseDiffDetails.City;
							txtHouseNo.Text = caseDiffDetails.Hn;
							txtLast.Text = caseDiffDetails.IncareLast;
							txtFirst.Text = caseDiffDetails.IncareFirst;
							txtMailApartment.Text = caseDiffDetails.Apt;
							txtMailFloor.Text = caseDiffDetails.Flr;
							txtMailPhone.Text = caseDiffDetails.Phone;
							txtMailState.Text = caseDiffDetails.State;
							txtMailStreet.Text = caseDiffDetails.Street;
							txtMailSuffix.Text = caseDiffDetails.Suffix;
							txtMailZipCode.Text = "00000".Substring(0, 5 - caseDiffDetails.Zip.Length) +
												caseDiffDetails.Zip;
							txtMailZipPlus.Text = "0000".Substring(0, 4 - caseDiffDetails.ZipPlus.Length) +
												caseDiffDetails.ZipPlus;
							CommonFunctions.SetComboBoxValue(cmbMailCounty, caseDiffDetails.County);
						}
						else
						{
							tabPageService.Hidden = true;
						}

						var caseLandlordDetails = _model.CaseMstData.GetLandlordadpya(
							BaseForm.BaseAgency.ToString(), BaseForm.BaseDept.ToString(), BaseForm.BaseProg.ToString(),
							caseMSTEntity.ApplYr, caseMSTEntity.ApplNo, "Landlord");
						if (caseLandlordDetails != null)
						{
							tabPageLandlord.Hidden = false;
							txtLCityName.Text = caseLandlordDetails.City;
							txtLHno.Text = caseLandlordDetails.Hn;
							txtLLast.Text = caseLandlordDetails.IncareLast;
							txtLFirst.Text = caseLandlordDetails.IncareFirst;
							txtLApt.Text = caseLandlordDetails.Apt;
							txtLFlr.Text = caseLandlordDetails.Flr;
							txtLPhone.Text = caseLandlordDetails.Phone;
							txtLState.Text = caseLandlordDetails.State;
							txtLStreet.Text = caseLandlordDetails.Street;
							txtLSuffix.Text = caseLandlordDetails.Suffix;
							txtLZip.Text = "00000".Substring(0, 5 - caseLandlordDetails.Zip.Length) +
											caseLandlordDetails.Zip;
							txtLzipplus.Text = "0000".Substring(0, 4 - caseLandlordDetails.ZipPlus.Length) +
												caseLandlordDetails.ZipPlus;
							CommonFunctions.SetComboBoxValue(cmbLCounty, caseLandlordDetails.County);
						}
						else
						{
							tabPageLandlord.Hidden = true;
						}

						var serviceDS = DatabaseLayer.CaseMst.GetSelectServicesByHIE(
							string.Empty, caseMSTEntity.ApplAgency, caseMSTEntity.ApplDept, caseMSTEntity.ApplProgram,
							caseMSTEntity.ApplYr, caseMSTEntity.ApplNo);
						var serviceDT = serviceDS.Tables[0];
						var serviceList = new List<string>();
						gvwServices.Rows.Clear();
						foreach (DataRow dr in serviceDT.Rows)
						{
							gvwServices.Rows.Add(false, dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString());
							// listBoxSelectionControl1.ListBoxSelected.Items.Add(new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString()));
							serviceList.Add(dr["INQ_CODE"].ToString());
						}

						var serviceSaveDS = DatabaseLayer.CaseMst.GetSelectServicesByHIE(
							"SAVE", caseMSTEntity.ApplAgency, caseMSTEntity.ApplDept, caseMSTEntity.ApplProgram,
							caseMSTEntity.ApplYr, caseMSTEntity.ApplNo);
						var serviceSaveDT = serviceSaveDS.Tables[0];
						var serviceSaveList = new List<string>();
						if (serviceSaveDT.Rows.Count > 0)
							foreach (var row in gvwServices.Rows)
								if (row.Cells["ServiceCode"].Value != null)
									for (var i = 0; i < serviceSaveDT.Rows.Count; i++)
										if (Convert.ToString(row.Cells["ServiceCode"].Value.ToString().Trim()) ==
											serviceSaveDT.Rows[i]["INQ_CODE"].ToString().Trim())
										{
											row.Cells["Servicechk"].Value = true;
											break;
										}

						gvwServices.Update();
						gvwServices.ResumeLayout();

						//Pre asses form tab loading
						if (preassesCntlEntity.Exists(u => u.Enab.Equals("Y")))
						{
						}

						if (gvwCustomer.Rows.Count > 0) gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
					}
				}
			}
			catch (Exception ex)
			{
			}
		}

		public void GetSelectService()
		{
			//DataSet ds = Captain.DatabaseLayer.CaseMst.GetSelectServices();
			//DataTable dt = ds.Tables[0];

			//foreach (DataRow dr in dt.Rows)
			//{
			//    lstAvailable.Items.Add(new ListItem(dr["INQ_DESC"].ToString(), dr["INQ_CODE"].ToString()));

			//}
			//lstAvailable.SelectedIndex = 0;
		}

		private void fillOutofService()
		{
			cmbOutService.Items.Clear();
			var listItem = new List<Utilities.ListItem>();
			listItem.Add(new Utilities.ListItem("", "0"));
			listItem.Add(new Utilities.ListItem("Out of Service Area", "O"));
			listItem.Add(new Utilities.ListItem("In Service Area (in File)", "I"));
			listItem.Add(new Utilities.ListItem("In Service Area (Not in File)", "X"));
			cmbOutService.Items.AddRange(listItem.ToArray());
			cmbOutService.SelectedIndex = 0;
		}

		private void fillWaitList()
		{
			//cmbWaitingList.Items.Clear();
			//List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
			//listItem.Add(new Captain.Common.Utilities.ListItem("Select One", ""));
			//listItem.Add(new Captain.Common.Utilities.ListItem("None", "O"));
			//listItem.Add(new Captain.Common.Utilities.ListItem("Yes", "Y"));
			//listItem.Add(new Captain.Common.Utilities.ListItem("No", "N"));
			//cmbWaitingList.Items.AddRange(listItem.ToArray());
			//cmbWaitingList.SelectedIndex = 0;
			var commonwailtinglist = new List<CommonEntity>();

			if (propAgencyControlDetails != null)
			{
				if (propAgencyControlDetails.State.ToUpper() == "TX")
					commonwailtinglist = CommonFunctions.AgyTabsFilterCode(
						BaseForm.BaseAgyTabsEntity, "S0067", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
						string.Empty); ////_model.lookupDataAccess.GetCaseType();
				else
					commonwailtinglist = CommonFunctions.AgyTabsFilterCode(
						BaseForm.BaseAgyTabsEntity, "S0002", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
						string.Empty); ////_model.lookupDataAccess.GetCaseType();
			}
			else
			{
				commonwailtinglist = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0002",
																		BaseForm.BaseAgency, BaseForm.BaseDept,
																		BaseForm.BaseProg, string.Empty);
			}

			// CaseType = filterByHIE(CaseType);
			cmbWaitingList.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbWaitingList.ColorMember = "FavoriteColor";
			cmbWaitingList.SelectedIndex = 0;
			foreach (var wailtlist in commonwailtinglist)
			{
				var li = new Utilities.ListItem(wailtlist.Desc, wailtlist.Code);
				cmbWaitingList.Items.Add(li);
				//if (Mode.Equals(Consts.Common.Add) && wailtlist.Default.Equals("Y")) cmbCaseType.SelectedItem = li;
			}
		}

		private void fillDropDowns()
		{
			var CaseType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CASETYPES,
															BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
															string.Empty); //_model.lookupDataAccess.GetCaseType();
			foreach (var casetype in CaseType)
				cmbCaseType.Items.Add(new Utilities.ListItem(casetype.Desc, casetype.Code));
			cmbCaseType.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbCaseType.SelectedIndex = 0;

			var Township = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CITYTOWNTABLE,
															BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
															string.Empty); // _model.lookupDataAccess.GetTownship();
			foreach (var township in Township)
				cmbTownship.Items.Add(new Utilities.ListItem(township.Desc, township.Code));
			cmbTownship.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbTownship.SelectedIndex = 0;

			var Country = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY,
															BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
															string.Empty); //_model.lookupDataAccess.GetCountry();
			foreach (var country in Country)
			{
				cmbCounty.Items.Add(new Utilities.ListItem(country.Desc, country.Code));
				cmbMailCounty.Items.Add(new Utilities.ListItem(country.Desc, country.Code));
				cmbLCounty.Items.Add(new Utilities.ListItem(country.Desc, country.Code));
			}

			cmbCounty.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbCounty.SelectedIndex = 0;
			cmbMailCounty.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbMailCounty.SelectedIndex = 0;
			cmbLCounty.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbLCounty.SelectedIndex = 0;

			var Housing = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES,
															BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
															string.Empty); //_model.lookupDataAccess.GetHousing();
			foreach (var housing in Housing)
			{
				cmbHousingSituation.Items.Add(new Utilities.ListItem(housing.Desc, housing.Code));
				cmbHousingTx.Items.Add(new Utilities.ListItem(housing.Desc, housing.Code));
			}

			cmbHousingSituation.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbHousingSituation.SelectedIndex = 0;
			cmbHousingTx.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbHousingTx.SelectedIndex = 0;

			var PrimaryLanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity,
																	Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency,
																	BaseForm.BaseDept, BaseForm.BaseProg,
																	string
																		.Empty); //_model.lookupDataAccess.GetPrimaryLanguage();
			foreach (var primarylanguage in PrimaryLanguage)
				cmbPrimaryLang.Items.Add(new Utilities.ListItem(primarylanguage.Desc, primarylanguage.Code));
			cmbPrimaryLang.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbPrimaryLang.SelectedIndex = 0;

			var Secondarylanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity,
																	Consts.AgyTab.LANGUAGECODES, BaseForm.BaseAgency,
																	BaseForm.BaseDept, BaseForm.BaseProg,
																	string
																		.Empty); //_model.lookupDataAccess.GetSecondaryLanguage();
			foreach (var secondlanguage in Secondarylanguage)
				cmbSecondLang.Items.Add(new Utilities.ListItem(secondlanguage.Desc, secondlanguage.Code));
			cmbSecondLang.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbSecondLang.SelectedIndex = 0;


			var FamilyType = _model.lookupDataAccess.GetAgyFamilyTypes();

			FamilyType = filterByHIE(FamilyType);
			cmbFamilyType.Items.Clear();
			cmbFamilyType.ColorMember = "FavoriteColor";

			foreach (var familyType in FamilyType)
			{
				var li = new Utilities.ListItem(familyType.Desc, familyType.Code, familyType.Active,
												familyType.Active.Equals("Y") ? Color.Green : Color.Red,
												familyType.Default.ToString(), familyType.Extension.ToString());
				cmbFamilyType.Items.Add(li);
			}

			cmbFamilyType.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbFamilyType.SelectedIndex = 0;

			//List<CommonEntity> FamilyType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.FAMILYTYPE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); // _model.lookupDataAccess.GetFamilyType();
			//foreach (CommonEntity familyType in FamilyType)
			//{
			//    cmbFamilyType.Items.Add(new Captain.Common.Utilities.ListItem(familyType.Desc, familyType.Code));
			//}
			//cmbFamilyType.Items.Insert(0, new Captain.Common.Utilities.ListItem("Select One", "0"));
			//cmbFamilyType.SelectedIndex = 0;


			var contactyou = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity,
																Consts.AgyTab.BESTWAYTOCONTACT, BaseForm.BaseAgency,
																BaseForm.BaseDept, BaseForm.BaseProg,
																string
																	.Empty); //_model.lookupDataAccess.Getcontactyou();
			foreach (var Contactyou in contactyou)
				cmbContact.Items.Add(new Utilities.ListItem(Contactyou.Desc, Contactyou.Code));
			cmbContact.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbContact.SelectedIndex = 0;

			var AboutUs = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity,
															Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM,
															BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
															string.Empty); //_model.lookupDataAccess.GetaboutUs();
			foreach (var aboutUs in AboutUs) cmbAboutUs.Items.Add(new Utilities.ListItem(aboutUs.Desc, aboutUs.Code));
			cmbAboutUs.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbAboutUs.SelectedIndex = 0;

			var listItem = new List<Utilities.ListItem>();
			var cwDataSet =
				DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept,
													BaseForm.BaseProg);
			var dt = cwDataSet.Tables[0];
			if (dt.Rows.Count > 0)
				foreach (DataRow dr in dt.Rows)
					listItem.Add(new Utilities.ListItem(dr["NAME"].ToString().Trim(),
														dr["PWH_CASEWORKER"].ToString().Trim()));
			cmbStaff.Items.AddRange(listItem.ToArray());
			cmbStaff.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbStaff.SelectedIndex = 0;
			cmbVerifiedStaff.Items.AddRange(listItem.ToArray());
			cmbVerifiedStaff.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbVerifiedStaff.SelectedIndex = 0;


			//AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
			//searchAgytabs.Tabs_Type = "S0030";
			//AdhocData AgyTabs = new AdhocData();
			//List<AGYTABSEntity> SubsizedHouse = AgyTabs.Browse_AGYTABS(searchAgytabs);

			var SubsizedHouse = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0030",
																BaseForm.BaseAgency, BaseForm.BaseDept,
																BaseForm.BaseProg,
																string
																	.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.Subsidized_Housing_Type);
			//SubsizedHouse = filterByHIE(SubsizedHouse);
			cmbSubsidized.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbSubsidized.ColorMember = "FavoriteColor";
			cmbSubsidized.SelectedIndex = 0;
			foreach (var SubsizedH in SubsizedHouse)
			{
				var li = new Utilities.ListItem(SubsizedH.Desc, SubsizedH.Code);
				cmbSubsidized.Items.Add(li);
			}

			var DwellingType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.DWELLINGTYPE,
																BaseForm.BaseAgency, BaseForm.BaseDept,
																BaseForm.BaseProg,
																string
																	.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.DWELLINGTYPE);
			//DwellingType = filterByHIE(DwellingType);
			cmbDwelling.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbDwelling.ColorMember = "FavoriteColor";
			cmbDwelling.SelectedIndex = 0;
			foreach (var Dwellingitems in DwellingType)
			{
				var li = new Utilities.ListItem(Dwellingitems.Desc, Dwellingitems.Code, Dwellingitems.Active,
												Dwellingitems.Active.Equals("Y") ? Color.Green : Color.Red);
				cmbDwelling.Items.Add(li);
			}

			var PrimarySourceHeat = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity,
																	Consts.AgyTab.HEATSOURCE, BaseForm.BaseAgency,
																	BaseForm.BaseDept, BaseForm.BaseProg,
																	string
																		.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);
			// PrimarySourceHeat = filterByHIE(PrimarySourceHeat);
			cmbPrimarySourceoHeat.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbPrimarySourceoHeat.ColorMember = "FavoriteColor";
			cmbPrimarySourceoHeat.SelectedIndex = 0;
			foreach (var PrimarySourceHeatItems in PrimarySourceHeat)
			{
				var li = new Utilities.ListItem(PrimarySourceHeatItems.Desc, PrimarySourceHeatItems.Code,
												PrimarySourceHeatItems.Active,
												PrimarySourceHeatItems.Active.Equals("Y") ? Color.Green : Color.Red);
				cmbPrimarySourceoHeat.Items.Add(li);
			}

			var PrimaryMethodofHeat = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity,
																		Consts.AgyTab.METHODOFPAYINGFORHEAT,
																		BaseForm.BaseAgency, BaseForm.BaseDept,
																		BaseForm.BaseProg,
																		string
																			.Empty); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.METHODOFPAYINGFORHEAT);
			//PrimaryMethodofHeat = filterByHIE(PrimaryMethodofHeat);
			cmbPMOPfHeat.Items.Insert(0, new Utilities.ListItem("Select One", "0"));
			cmbPMOPfHeat.ColorMember = "FavoriteColor";
			cmbPMOPfHeat.SelectedIndex = 0;
			foreach (var PrimaryMethodofHeatItems in PrimaryMethodofHeat)
			{
				var li = new Utilities.ListItem(PrimaryMethodofHeatItems.Desc, PrimaryMethodofHeatItems.Code,
												PrimaryMethodofHeatItems.Active,
												PrimaryMethodofHeatItems.Active.Equals("Y") ? Color.Green : Color.Red);
				cmbPMOPfHeat.Items.Add(li);
			}


			GetSelectService();
		}

		private void SetAlertTypes(string AlertTypes)
		{
			var alertTypeList = new List<string>();
			if (AlertTypes != null)
			{
				var imageTypes = AlertTypes.Split(' ');
				for (var i = 0; i < imageTypes.Length; i++) alertTypeList.Add(imageTypes.GetValue(i).ToString());
			}
		}

		private void fillEMS()
		{
			cmbTownship.Items.Clear();
			var listItem = new List<Utilities.ListItem>();
			listItem.Add(new Utilities.ListItem("Select One", ""));
			listItem.Add(new Utilities.ListItem("Authorization (Level 0)", "A"));
			listItem.Add(new Utilities.ListItem("Supervisory (Level 1)", "S"));
			listItem.Add(new Utilities.ListItem("Departmental (Level 2)", "D"));
			cmbTownship.Items.AddRange(listItem.ToArray());
			cmbTownship.SelectedIndex = 0;
		}

		private void fillSecurity()
		{
			cmbCaseType.Items.Clear();
			var listItem = new List<Utilities.ListItem>();
			listItem.Add(new Utilities.ListItem("Select One", ""));
			listItem.Add(new Utilities.ListItem("Reception/Clerical", "R"));
			listItem.Add(new Utilities.ListItem("Case Manager", "C"));
			listItem.Add(new Utilities.ListItem("Program Administrator", "P"));
			listItem.Add(new Utilities.ListItem("Both PA and CM", "B"));
			cmbCaseType.Items.AddRange(listItem.ToArray());
			cmbCaseType.SelectedIndex = 0;
		}

		/// <summary>
		/// Get Selected Rows Tag Clas.
		/// </summary>
		/// <returns></returns>
		public CaseSnpEntity GetSelectedRow()
		{
			CaseSnpEntity caseSnpEntity = null;
			if (gvwCustomer != null)
				foreach (DataGridViewRow dr in gvwCustomer.SelectedRows)
					if (dr.Selected)
					{
						strIndex = dr.Index;
						caseSnpEntity = dr.Tag as CaseSnpEntity;
						break;
					}

			return caseSnpEntity;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="comboBox"></param>
		/// <param name="value"></param>
		private void SetComboBoxValue(ComboBox comboBox, string value)
		{
			if (comboBox != null && comboBox.Items.Count > 0)
				foreach (Utilities.ListItem li in comboBox.Items)
					if (li.Value.Equals(value) || li.Text.Equals(value))
					{
						comboBox.SelectedItem = li;
						break;
					}
		}

		private void setControlEnabled(bool flag)
		{
			txtHN.Enabled = flag;
			txtStreet.Enabled = flag;
		}

		private void OnSnpDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (gvwCustomer.Rows[e.RowIndex].Tag is CaseSnpEntity)
			{
				var caseSnpEntity = gvwCustomer.Rows[e.RowIndex].Tag as CaseSnpEntity;
				var caseMST = CaseMST;
				var isApplicant = false;
				if (caseMST.FamilySeq.Equals(caseSnpEntity.FamilySeq)) isApplicant = true;
				if (Privileges.Program.ToString().ToUpper() == "CASE2002")
				{
					using (var clientSNPEditForm = new Case3001Form(BaseForm, isApplicant, caseMST, caseSnpEntity,
															Consts.Common.View, Privileges, CaseSnpEntityProp,
															ApplicantLastName))
					{
						clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
						clientSNPEditForm.ShowDialog();
					}
				}
				else
				{
					if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
					{
						using (var clientSNPEditForm = new Case4001FormDEMO(BaseForm, isApplicant, caseMST, caseSnpEntity,
																Consts.Common.View, Privileges, CaseSnpEntityProp,
																ApplicantLastName))
						{
							clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
							clientSNPEditForm.ShowDialog();
						}
					}
					else
					{
						using (var clientSNPEditForm = new Case3001Form(BaseForm, isApplicant, caseMST, caseSnpEntity,
																Consts.Common.View, Privileges, CaseSnpEntityProp,
																ApplicantLastName))
						{
							clientSNPEditForm.StartPosition = FormStartPosition.CenterScreen;
							clientSNPEditForm.ShowDialog();
						}
					}
				}
			}
		}

		private void btnIncome_Click(object sender, EventArgs e)
		{
			if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
			{
				var mainMenuControl = BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl;
				//CaseIncome caseIncomeForm = new CaseIncome("V", BaseForm, Privileges);
				//caseIncomeForm.FormClosed += new FormClosedEventHandler(OnCaseIncomeFormClosed);
				//caseIncomeForm.ShowDialog();

				using (var objIncome = new CaseIncomeForm2(string.Empty, BaseForm, CaseIncomePrivileges, ProgramDefinition,
													ProgramDefinition.IncomeTypeOnly.ToString(),string.Empty))
				{
					objIncome.FormClosed += new FormClosedEventHandler(OnCaseIncomeFormClosed);
					objIncome.StartPosition = FormStartPosition.CenterScreen;
					objIncome.ShowDialog();
				}
			}
		}


		private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
		{
			var HIE = MainMenuAgency + MainMenuDept + MainMenuProgram;
			LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) ||
													u.ListHierarchy.Contains(MainMenuAgency + MainMenuDept + "**") ||
													u.ListHierarchy.Contains(MainMenuAgency + "****") ||
													u.ListHierarchy.Contains("******"));
			return LookupValues;
		}


		private void pictureBox3_Click(object sender, EventArgs e)
		{
		}

		private void btnIncVerfication_Click(object sender, EventArgs e)
		{
			// GetSelectedProgram();
			using (var caseIncomeverfication = new CaseIncomeVerification(BaseForm, BaseForm.BaseAgency, BaseForm.BaseDept,
																	BaseForm.BaseProg, BaseForm.BaseYear,
																	BaseForm.BaseApplicationNo, CaseVerPrivileges, "V"))
			{
				caseIncomeverfication.StartPosition = FormStartPosition.CenterScreen;
				caseIncomeverfication.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCaseNotesFormClosed(object sender, FormClosedEventArgs e)
		{
			var form = sender as CaseNotes;

			//if (form.DialogResult == DialogResult.OK)
			//{
			var strYear = "    ";
			if (!string.IsNullOrEmpty(BaseForm.BaseYear)) strYear = BaseForm.BaseYear;
			caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName(
				Privileges.Program,
				BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + BaseForm.BaseApplicationNo);
			if (caseNotesEntity.Count > 0)
				ToolBarCaseNotes.ImageSource = "case-notes";
			else
				ToolBarCaseNotes.ImageSource = "case-notes";
			caseNotesEntity = caseNotesEntity;

			//}
		}


		private void OnCaseIncomeFormClosed(object sender, FormClosedEventArgs e)
		{
			Refresh();
		}

		private void contextMenu2_Popup(object sender, EventArgs e)
		{
			if (gvwCustomer.Rows.Count > 0)
				if (gvwCustomer.Rows[0].Tag is CaseSnpEntity)
				{
					var drow = gvwCustomer.SelectedRows[0].Tag as CaseSnpEntity;
					if (!CaseMST.FamilySeq.Equals(drow.FamilySeq))
					{
						contextMenu2.MenuItems.Clear();
						var menuLst = new MenuItem();
						menuLst.Text = "Link To Applicant";
						contextMenu2.MenuItems.Add(menuLst);
					}
					else
					{
						contextMenu2.MenuItems.Clear();
					}
				}
		}

		private void gvwCustomer_MenuClick(object objSource, MenuItemEventArgs objArgs)
		{
			if (gvwCustomer.Rows.Count > 0)
			{
				var caseLinkApplicant = GetSelectedRow();
				if (caseLinkApplicant != null)
				{
					if (ProgramDefinition.PRODUPSSN.Equals("Y"))
					{
						var boolupdate = true;
						var CaseMstAllList = _model.CaseMstData.GetCaseMstSSno(string.Empty, string.Empty, string.Empty, string.Empty);
						var caseMstAlllistEntity = CaseMstAllList.Find(
							u => u.ApplAgency.Equals(CaseMST.ApplAgency) && u.ApplDept.Equals(CaseMST.ApplDept) &&
								u.ApplProgram.Equals(CaseMST.ApplProgram) &&
								u.ApplYr.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssn.Equals(caseLinkApplicant.Ssno));
						if (caseMstAlllistEntity != null)
						{
							if (caseLinkApplicant.Ssno == "000000000")
							{
								boolupdate = true;
							}
							else
							{
								CommonFunctions.MessageBoxDisplay(
									"Applicant already exists with this SSN in   Hierarchy: " +
									caseMstAlllistEntity.ApplAgency + caseMstAlllistEntity.ApplDept +
									caseMstAlllistEntity.ApplProgram + caseMstAlllistEntity.ApplYr + " with App No: " +
									caseMstAlllistEntity.ApplNo);
								boolupdate = false;
							}
						}

						if (boolupdate)
						{
							var CaseMstEntity = new CaseMstEntity();

							CaseMstEntity.ApplAgency = CaseMST.ApplAgency;
							CaseMstEntity.ApplDept = CaseMST.ApplDept;
							CaseMstEntity.ApplProgram = CaseMST.ApplProgram;
							CaseMstEntity.ApplYr = CaseMST.ApplYr;
							CaseMstEntity.ApplNo = CaseMST.ApplNo;
							CaseMstEntity.ClientId = caseLinkApplicant.ClientId;
							CaseMstEntity.FamilySeq = caseLinkApplicant.FamilySeq;
							CaseMstEntity.Ssn = caseLinkApplicant.Ssno;
							CaseMstEntity.LstcOperator4 = BaseForm.UserID;
							CaseMstEntity.Mode = "LinkApplicant";
							var strApplNo = string.Empty;
							var strClientId = string.Empty;
							var strFamilyId = string.Empty;
							var strNewSSNNO = string.Empty;
							var strErrorMsg = string.Empty;
							if (_model.CaseMstData.InsertUpdateCaseMst(CaseMstEntity, out strApplNo, out strClientId,
																		out strFamilyId, out strNewSSNNO,
																		out strErrorMsg))
							{
								var caseMstEntity = _model.CaseMstData.GetCaseMST(
									BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
									BaseForm.BaseApplicationNo);

								if (caseMstEntity != null)
								{
									var caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(
										BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
										BaseForm.BaseApplicationNo);
									BaseForm.BaseTopApplSelect = "Y";
									BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, CaseMstEntity.ApplAgency,
																CaseMstEntity.ApplDept, CaseMstEntity.ApplProgram,
																CaseMstEntity.ApplYr, "LinkApplicant", "Display");
									Refresh();
								}
							}
							else
							{
								CommonFunctions.MessageBoxDisplay(strErrorMsg);
							}
						}
					}
					else
					{
						var CaseMstEntity = new CaseMstEntity();

						CaseMstEntity.ApplAgency = CaseMST.ApplAgency;
						CaseMstEntity.ApplDept = CaseMST.ApplDept;
						CaseMstEntity.ApplProgram = CaseMST.ApplProgram;
						CaseMstEntity.ApplYr = CaseMST.ApplYr;
						CaseMstEntity.ApplNo = CaseMST.ApplNo;
						CaseMstEntity.ClientId = caseLinkApplicant.ClientId;
						CaseMstEntity.FamilySeq = caseLinkApplicant.FamilySeq;
						CaseMstEntity.Ssn = caseLinkApplicant.Ssno;
						CaseMstEntity.LstcOperator4 = BaseForm.UserID;
						CaseMstEntity.Mode = "LinkApplicant";
						var strApplNo = string.Empty;
						var strClientId = string.Empty;
						var strFamilyId = string.Empty;
						var strSsnNo = string.Empty;
						var strErrorMsg = string.Empty;
						if (_model.CaseMstData.InsertUpdateCaseMst(CaseMstEntity, out strApplNo, out strClientId,
																	out strFamilyId, out strSsnNo, out strErrorMsg))
						{
							var caseMstEntity = _model.CaseMstData.GetCaseMST(
								BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
								BaseForm.BaseApplicationNo);

							if (caseMstEntity != null)
							{
								var caseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(
									BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
									BaseForm.BaseApplicationNo);
								BaseForm.BaseTopApplSelect = "Y";
								BaseForm.GetApplicantDetails(caseMstEntity, caseSnpEntity, CaseMstEntity.ApplAgency,
															CaseMstEntity.ApplDept, CaseMstEntity.ApplProgram,
															CaseMstEntity.ApplYr, "LinkApplicant", "Display");
								Refresh();
							}
						}
						else
						{
							CommonFunctions.MessageBoxDisplay(strErrorMsg);
						}
					}
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//EligibilityConditionForm obj = new EligibilityConditionForm(BaseForm);
			//obj.Show();
		}

		public void CalclationEmployee()
		{
			txtTotalHouseholdIncome.Text = "0";
			txtTotalHHDebt.Text = "0";
			txtTotalAssets.Text = "0";
			var rent = txtRent.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtRent.Text);
			var water = txtWater.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtWater.Text);
			var electric = txtElectric.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtElectric.Text);
			var heating = txtHeating.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtHeating.Text);
			var expand1 = txtExpand1.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand1.Text);
			var expand2 = txtExpand2.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand2.Text);
			var expand3 = txtExpand3.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand3.Text);
			var expand4 = txtExpand4.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtExpand4.Text);
			var miscExpenses = txtMiscExpenses.Text.Trim().Equals(string.Empty)
				? 0.0
				: double.Parse(txtMiscExpenses.Text);
			var miscDebt = txtMiscDebt.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscDebt.Text);
			var miscAsset = txtMiscAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtMiscAssets.Text);
			var physcialAsset = txtPhysicalAssets.Text.Trim().Equals(string.Empty)
				? 0.0
				: double.Parse(txtPhysicalAssets.Text);
			var OtherAsset = txtOtherAssets.Text.Trim().Equals(string.Empty) ? 0.0 : double.Parse(txtOtherAssets.Text);
			var LiquidAsset = txtLiquidAssets.Text.Trim().Equals(string.Empty)
				? 0.0
				: double.Parse(txtLiquidAssets.Text);


			//txtTotalAssets.Text = caseMSTEntity.AsetTotal;
			//txtDebtAssetRatio.Text = caseMSTEntity.AsetRatio;           
			//txtDebtIncomeRatio.Text = string.Empty;
			//txtTotalHouseholdIncome.Text = string.Empty;

			var TotalDebt = expand1 + expand2 + expand3 + expand4 + miscDebt;
			txtTotalHHDebt.Text = TotalDebt.ToString();

			var TotalAssets = physcialAsset + OtherAsset + LiquidAsset; //miscAsset
			txtTotalAssets.Text = TotalAssets.ToString();

			var total = rent + water + electric + heating + miscExpenses;
			txtTotalHouseHold.Text = total.ToString();
			double MonthlyIncome = 0;
			double TotalAllIncome = 0;
			txtMonthlyIncome.Text = "0";
			txtTotalLiving.Text = "0";

			if (TotalAssets > 0)
				txtDebtAssetRatio.Text = Math.Round(TotalDebt / TotalAssets, 2).ToString();
			else
				txtDebtAssetRatio.Text = "0.00";

			if (CaseMST != null)
			{
				if (CaseMST.ProgIncome.ToString() != string.Empty) MonthlyIncome = double.Parse(CaseMST.ProgIncome);


				if (MonthlyIncome > 0)
				{
					TotalAllIncome = MonthlyIncome;
					txtTotalHouseholdIncome.Text = MonthlyIncome.ToString();
					txtDebtIncomeRatio.Text = Math.Round(TotalDebt / TotalAllIncome, 2).ToString();
					MonthlyIncome = MonthlyIncome / 12;
					MonthlyIncome = Math.Round(MonthlyIncome, 2);
					txtMonthlyIncome.Text = MonthlyIncome.ToString();
					if (!((Utilities.ListItem) cmbVerifiedStaff.SelectedItem).Value.ToString().Equals("0"))
					{
						double totLiveExp = 0;
						if (total > 0)
						{
							totLiveExp = total / MonthlyIncome * 1000 + 0.5;
							totLiveExp = Math.Round(totLiveExp);
							totLiveExp = totLiveExp / 10;
						}

						txtTotalLiving.Text = totLiveExp.ToString();
					}
				}
				else
				{
					txtDebtIncomeRatio.Text = "0.00";
				}
			}
		}

		private void btnProcess_Click(object sender, EventArgs e)
		{
			//PrintRdlcForm objForm = new PrintRdlcForm(BaseForm, Privileges);
			//objForm.ShowDialog();
			//ChldMstEntity chldMst = _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
			//GetRankCategoryDetails(BaseForm.BaseCaseMstListEntity[0], BaseForm.BaseCaseSnpEntity, chldMst);
		}

		#region RankCateogryPointsCalculation

		public CaseMstEntity propMstRank { get; set; }

		private void GetRankCategoryDetails(CaseMstEntity caseMst, List<CaseSnpEntity> caseSnp, ChldMstEntity chldMst)
		{
			var RnkQuesFledsEntity = _model.SPAdminData.GetRanksCrit2Data("RANKQUES", string.Empty, string.Empty);
			var RnkQuesFledsAllDataEntity =
				_model.SPAdminData.GetRanksCrit2Data(string.Empty, BaseForm.BaseAgency, string.Empty);
			var RnkCustFldsAllDataEntity =
				_model.SPAdminData.GetRanksCrit2Data("CUSTFLDS", BaseForm.BaseAgency, string.Empty);
			var custResponses = _model.CaseMstData.GetCustomQuestionAnswersRank(
				BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
				BaseForm.BaseApplicationNo, string.Empty, string.Empty, string.Empty);
			var ListRankPoints = new List<CommonEntity>();
			for (var intRankCtg = 1; intRankCtg <= 6; intRankCtg++)
			{
				var RnkQuesFledsDataEntity =
					RnkQuesFledsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == intRankCtg.ToString());
				var RnkCustFldsDataEntity =
					RnkCustFldsAllDataEntity.FindAll(u => u.RankCategory.Trim().ToString() == intRankCtg.ToString());

				List<RNKCRIT2Entity> RnkQuesSearchList;
				propMstRank = caseMst;
				RNKCRIT2Entity RnkQuesSearch = null;
				// List<RNKCRIT2Entity> RnkQuesCaseSnp = null;
				var intRankPoint = 0;
				var strApplicationcode = string.Empty;
				foreach (var rnkQuesData in RnkQuesFledsEntity)
				{
					RnkQuesSearch = null;
					switch (rnkQuesData.RankFldName.Trim())
					{
						case Consts.RankQues.MZip:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Zip.Trim());
							break;
						case Consts.RankQues.MCounty:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.County.Trim());
							break;
						case Consts.RankQues.MLanguage:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Language.Trim());
							break;
						case Consts.RankQues.MAlertCode:
							intRankPoint = intRankPoint +
											fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity,
																rnkQuesData.RankFldName.Trim());
							break;
						case Consts.RankQues.MAboutUs:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.AboutUs.Trim());
							break;
						case Consts.RankQues.MAddressYear:
							if (caseMst.AddressYears != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.AddressYears) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.AddressYears));
							break;
						case Consts.RankQues.MBestContact:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.BestContact.Trim());
							break;
						case Consts.RankQues.MCaseReviewDate:
							if (caseMst.CaseReviewDate != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDateTime(u.LtDate).Date >=
										Convert.ToDateTime(caseMst.CaseReviewDate).Date &&
										Convert.ToDateTime(u.GtDate).Date <=
										Convert.ToDateTime(caseMst.CaseReviewDate).Date);
							break;
						case Consts.RankQues.MCaseType:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.CaseType.Trim());
							break;
						case Consts.RankQues.MCmi:
							if (caseMst.Cmi != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Cmi) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Cmi));
							break;
						case Consts.RankQues.MEElectric:
							if (caseMst.ExpElectric != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpElectric) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpElectric));
							break;
						case Consts.RankQues.MEDEBTCC:
							if (caseMst.Debtcc != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Debtcc) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Debtcc));
							break;
						case Consts.RankQues.MEDEBTLoans:
							if (caseMst.DebtLoans != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtLoans) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtLoans));
							break;
						case Consts.RankQues.MEDEBTMed:
							if (caseMst.DebtMed != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.DebtMed) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.DebtMed));
							break;
						case Consts.RankQues.MEHeat:
							if (caseMst.ExpHeat != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpHeat) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpHeat));
							break;
						case Consts.RankQues.MEligDate:
							if (caseMst.EligDate != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDateTime(u.LtDate).Date >=
										Convert.ToDateTime(caseMst.EligDate).Date &&
										Convert.ToDateTime(u.GtDate).Date <= Convert.ToDateTime(caseMst.EligDate).Date);
							break;
						case Consts.RankQues.MELiveExpenses:
							if (caseMst.ExpLivexpense != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpLivexpense) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpLivexpense));
							// RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.ExpLivexpense.Trim());
							break;
						case Consts.RankQues.MERent:
							if (caseMst.ExpRent != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpRent) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpRent));
							break;
						case Consts.RankQues.METotal:
							if (caseMst.ExpTotal != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpTotal) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpTotal));
							break;
						case Consts.RankQues.MEWater:
							if (caseMst.ExpWater != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ExpWater) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ExpWater));
							break;

						case Consts.RankQues.MExpCaseworker:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.ExpCaseWorker.Trim());
							break;
						case Consts.RankQues.MFamilyType:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.FamilyType.Trim());
							break;
						case Consts.RankQues.MFamIncome:
							if (caseMst.FamIncome != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.FamIncome) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.FamIncome));
							break;
						case Consts.RankQues.MHousing:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Housing.Trim());
							break;
						case Consts.RankQues.MHud:
							if (caseMst.Hud != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Hud) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Hud));
							break;

						case Consts.RankQues.MIncomeTypes:
							intRankPoint = intRankPoint +
											fillAlertIncomeCodes(caseMst.AlertCodes, RnkQuesFledsDataEntity,
																rnkQuesData.RankFldName.Trim());
							//RnkQuesSearch = RnkQuesFledsDataEntity.Find(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() && u.RespCd.Trim() == caseMst.IncomeTypes.Trim());
							break;
						case Consts.RankQues.MInitialDate:
							if (caseMst.InitialDate != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDateTime(u.LtDate).Date >=
										Convert.ToDateTime(caseMst.InitialDate).Date &&
										Convert.ToDateTime(u.GtDate).Date <=
										Convert.ToDateTime(caseMst.InitialDate).Date);
							break;
						case Consts.RankQues.MIntakeDate:
							if (caseMst.IntakeDate != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDateTime(u.LtDate).Date >=
										Convert.ToDateTime(caseMst.IntakeDate).Date &&
										Convert.ToDateTime(u.GtDate).Date <=
										Convert.ToDateTime(caseMst.IntakeDate).Date);
							break;
						case Consts.RankQues.MIntakeWorker:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.IntakeWorker.Trim());
							break;
						case Consts.RankQues.MJuvenile:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Juvenile.Trim());
							break;
						case Consts.RankQues.MLanguageOt:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.LanguageOt.Trim());
							break;
						case Consts.RankQues.MNoInprog:
							if (caseMst.NoInProg != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.NoInProg) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.NoInProg));
							break;
						case Consts.RankQues.Mpoverty:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Poverty.Trim());
							break;
						case Consts.RankQues.MProgIncome:
							if (caseMst.ProgIncome != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.ProgIncome) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.ProgIncome));
							break;
						case Consts.RankQues.MReverifyDate:
							if (caseMst.ReverifyDate != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDateTime(u.LtDate).Date >=
										Convert.ToDateTime(caseMst.ReverifyDate).Date &&
										Convert.ToDateTime(u.GtDate).Date <=
										Convert.ToDateTime(caseMst.ReverifyDate).Date);
							break;
						case Consts.RankQues.MSECRET:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Secret.Trim());
							break;
						case Consts.RankQues.MSenior:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Senior.Trim());
							break;
						case Consts.RankQues.MSite:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.Site.Trim());
							break;
						case Consts.RankQues.MSMi:
							if (caseMst.Smi != string.Empty)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										Convert.ToDecimal(u.LtNum) >= Convert.ToDecimal(caseMst.Smi) &&
										Convert.ToDecimal(u.GtNum) <= Convert.ToDecimal(caseMst.Smi));
							break;
						case Consts.RankQues.MVefiryCheckstub:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.VerifyCheckStub.Trim());
							break;
						case Consts.RankQues.MVerifyW2:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.VerifyW2.Trim());
							break;
						case Consts.RankQues.MVeriTaxReturn:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.VerifyTaxReturn.Trim());
							break;
						case Consts.RankQues.MVerLetter:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.VerifyLetter.Trim());
							break;
						case Consts.RankQues.MVerOther:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.VerifyOther.Trim());
							break;
						case Consts.RankQues.MWaitList:
							RnkQuesSearch = RnkQuesFledsDataEntity.Find(
								u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
									u.RespCd.Trim() == caseMst.WaitList.Trim());
							break;
						case Consts.RankQues.SEducation:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Education
														.ToString();
							var SnpFieldsCodesList = new List<string>();
							var SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Education);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.S1shift:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IstShift
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].IstShift);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.S2ndshift:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIndShift
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].IIndShift);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.S3rdShift:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).IIIrdShift
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].IIIrdShift);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SAge:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode =
								caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Age.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Age);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						//case Consts.RankQues.SAltBdate:
						//    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
						//    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).AltBdate.ToString();
						//    SnpFieldsCodesList = new List<string>();
						//    SnpFieldsRelationList = new List<string>();
						//    for (int i = 0; i < caseSnp.Count; i++)
						//    {
						//        SnpFieldsCodesList.Add(caseSnp[i].AltBdate);
						//        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
						//    }
						//    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
						//    break;
						case Consts.RankQues.SDisable:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Disable
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Disable);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SDrvlic:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Drvlic
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Drvlic);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SEmployed:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Employed);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SEthinic:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Ethnic
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Ethnic);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						//case Consts.RankQues.SExpireWorkDate:
						//    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
						//    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).ExpireWorkDate.ToString();
						//    SnpFieldsCodesList = new List<string>();
						//    SnpFieldsRelationList = new List<string>();
						//    for (int i = 0; i < caseSnp.Count; i++)
						//    {
						//        SnpFieldsCodesList.Add(caseSnp[i].ExpireWorkDate);
						//        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
						//    }
						//    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
						//    break;
						case Consts.RankQues.SFarmer:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Farmer
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Farmer);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SFoodStamps:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FootStamps
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].FootStamps);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SFThours:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).FullTimeHours
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].FullTimeHours);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SHealthIns:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HealthIns
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].HealthIns);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						//case Consts.RankQues.SHireDate:
						//    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
						//    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HireDate.ToString();
						//    SnpFieldsCodesList = new List<string>();
						//    SnpFieldsRelationList = new List<string>();
						//    for (int i = 0; i < caseSnp.Count; i++)
						//    {
						//        SnpFieldsCodesList.Add(caseSnp[i].HireDate);
						//        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
						//    }
						//    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
						//    break;
						case Consts.RankQues.SHourlyWage:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).HourlyWage
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Education);
								SnpFieldsRelationList.Add(caseSnp[i].HourlyWage);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SjobCategory:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobCategory
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].JobCategory);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SjobTitle:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).JobTitle
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].JobTitle);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						//case Consts.RankQues.SLastWorkDate:
						//    RnkQuesSearchList = RnkQuesFledsDataEntity.FindAll(u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
						//    strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LastWorkDate.ToString();
						//    SnpFieldsCodesList = new List<string>();
						//    SnpFieldsRelationList = new List<string>();
						//    for (int i = 0; i < caseSnp.Count; i++)
						//    {
						//        SnpFieldsCodesList.Add(caseSnp[i].LastWorkDate);
						//        SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
						//    }
						//    intRankPoint = intRankPoint + CaseSnpDetailsCalc(RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList, SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(), rnkQuesData.RankFldRespType.Trim());
						//    break;
						case Consts.RankQues.SLegalTowork:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).LegalTowork
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].LegalTowork);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SMartialStatus:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MaritalStatus
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].MaritalStatus);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SMemberCode:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).MemberCode
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].MemberCode);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SNofcjob:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberOfcjobs
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].NumberOfcjobs);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SNofljobs:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).NumberofLvjobs
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].NumberofLvjobs);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SPFrequency:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PayFrequency
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].PayFrequency);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SPregnant:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Pregnant
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Pregnant);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SPThours:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).PartTimeHours
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].PartTimeHours);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SRace:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode =
								caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Race.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Race);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SRelitran:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Relitran
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Relitran);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SResident:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Resident
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Resident);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SRshift:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).RShift
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].RShift);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SSchoolDistrict:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).SchoolDistrict
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].SchoolDistrict);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SSEmploy:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Employed
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Employed);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SSex:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode =
								caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Sex.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Sex);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SSnpVet:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode =
								caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Vet.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Vet);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SStatus:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Status
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Status);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.STranserv:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Transerv
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Transerv);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SWic:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode =
								caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).Wic.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].Wic);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.SworkLimit:
							RnkQuesSearchList =
								RnkQuesFledsDataEntity.FindAll(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim());
							strApplicationcode = caseSnp.Find(u => u.FamilySeq.Equals(caseMst.FamilySeq)).WorkLimit
														.ToString();
							SnpFieldsCodesList = new List<string>();
							SnpFieldsRelationList = new List<string>();
							for (var i = 0; i < caseSnp.Count; i++)
							{
								SnpFieldsCodesList.Add(caseSnp[i].WorkLimit);
								SnpFieldsRelationList.Add(caseSnp[i].MemberCode);
							}

							intRankPoint = intRankPoint + CaseSnpDetailsCalc(
								RnkQuesSearchList, caseSnp, strApplicationcode, SnpFieldsCodesList,
								SnpFieldsRelationList, rnkQuesData.RankFldName.Trim(),
								rnkQuesData.RankFldRespType.Trim());
							break;
						case Consts.RankQues.CDentalCoverage:
							if (chldMst != null)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										u.RespCd.Trim() == chldMst.DentalCoverage.Trim());
							break;
						case Consts.RankQues.CDiagNosisDate:
							if (chldMst != null)
								if (chldMst.DiagnosisDate != string.Empty)
									RnkQuesSearch = RnkQuesFledsDataEntity.Find(
										u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
											Convert.ToDateTime(u.LtDate).Date >=
											Convert.ToDateTime(chldMst.DiagnosisDate).Date &&
											Convert.ToDateTime(u.GtDate).Date <=
											Convert.ToDateTime(chldMst.DiagnosisDate).Date);
							break;
						case Consts.RankQues.CDisability:
							if (chldMst != null)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										u.RespCd.Trim() == chldMst.Disability.Trim());
							break;
						case Consts.RankQues.CInsCat:
							if (chldMst != null)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										u.RespCd.Trim() == chldMst.InsCat.Trim());
							break;
						case Consts.RankQues.CMedCoverage:
							if (chldMst != null)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										u.RespCd.Trim() == chldMst.MedCoverage.Trim());
							break;
						case Consts.RankQues.CMedicalCoverageType:
							if (chldMst != null)
								RnkQuesSearch = RnkQuesFledsDataEntity.Find(
									u => u.RankFldName.Trim() == rnkQuesData.RankFldName.Trim() &&
										u.RespCd.Trim() == chldMst.MedCoverType.Trim());
							break;
					}

					if (RnkQuesSearch != null)
						intRankPoint = intRankPoint + Convert.ToInt32(RnkQuesSearch.Points);
				}

				CustomQuestionsEntity custResponcesearch = null;
				foreach (var item in RnkCustFldsDataEntity)
				{
					custResponcesearch = null;
					if (item.RankFldRespType.Trim().Equals("D"))
					{
						custResponcesearch =
							custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) &&
													u.ACTMULTRESP.Trim() == item.RespCd.Trim());
					}
					else if (item.RankFldRespType.Trim().Equals("N"))
					{
						custResponcesearch = custResponses.Find(
							u => u.ACTCODE.Trim().Equals(item.RankFiledCode) &&
								Convert.ToDecimal(u.ACTNUMRESP) >= Convert.ToDecimal(item.GtNum) &&
								Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));
					}
					else if (item.RankFldRespType.Trim().Equals("T"))
					{
						// custResponcesearch = custResponses.Find(u => u.ACTCODE.Trim().Equals(item.RankFiledCode) && Convert.ToDateTime(u.ACTDATERESP) >= Convert.ToDecimal(item.GtNum) && Convert.ToDecimal(u.ACTNUMRESP) <= Convert.ToDecimal(item.LtNum));           
					}

					if (custResponcesearch != null)
						intRankPoint = intRankPoint + Convert.ToInt32(item.Points);
				}

				ListRankPoints.Add(new CommonEntity(intRankCtg.ToString(), intRankPoint.ToString()));
			}

			foreach (var item in ListRankPoints) txtProcess.Text = txtProcess.Text + item.Code + ":" + item.Desc + ",";
		}

		private int fillAlertIncomeCodes(string alertCodes, List<RNKCRIT2Entity> rnkSearchEntity, string FieldName)
		{
			var intAlertcode = 0;
			var AlertList = new List<string>();
			if (alertCodes != null)
			{
				var incomeTypes = alertCodes.Split(' ');
				for (var i = 0; i < incomeTypes.Length; i++) AlertList.Add(incomeTypes.GetValue(i).ToString());
			}

			var RnkAlertCode = rnkSearchEntity.FindAll(u => u.RankFldName.Trim() == FieldName);

			foreach (var rnkEntity in RnkAlertCode)
				if (alertCodes != null && AlertList.Contains(rnkEntity.RespCd))
					intAlertcode = intAlertcode + Convert.ToInt32(rnkEntity.Points);
			return intAlertcode;
		}


		private int CaseSnpDetailsCalc(List<RNKCRIT2Entity> rnkCaseSnp, List<CaseSnpEntity> caseSnpDetails,
										string strApplicantCode, List<string> listCodestring,
										List<string> listRelationstring, string FilterCode, string ResponceType)
		{
			var intSnpPoints = 0;
			foreach (var item in rnkCaseSnp)
				if (item.CountInd.Trim() == "A")
				{
					if (item.RespCd == strApplicantCode) intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
				}
				else if (item.CountInd.Trim() == "M")
				{
					if (item.Relation == "*")
					{
						var count = 0;
						switch (FilterCode)
						{
							case Consts.RankQues.S1shift:
								count = caseSnpDetails.FindAll(u => u.IstShift.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.S2ndshift:
								count = caseSnpDetails.FindAll(u => u.IIndShift.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.S3rdShift:
								count = caseSnpDetails.FindAll(u => u.IIIrdShift.Trim().Equals(item.RespCd)).Count;
								break;
							//case Consts.RankQues.SAge:
							//    foreach (CaseSnpEntity snpDate in caseSnpDetails)
							//    {
							//        if (snpDate.AltBdate != string.Empty)
							//        {
							//            DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
							//            int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpDate.AltBdate), EndDate);
							//            if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
							//            {
							//                count = count + 1;
							//            }
							//        }
							//    }
							//    break;
							//case Consts.RankQues.SAltBdate:
							//    foreach (CaseSnpEntity snpDate in caseSnpDetails)
							//    {
							//        if (snpDate.AltBdate != string.Empty)
							//            if (Convert.ToDateTime(snpDate.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//            {
							//                count = count + 1;
							//            }
							//    }

							//    break;
							case Consts.RankQues.SSchoolDistrict:
								count = caseSnpDetails.FindAll(u => u.SchoolDistrict.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SEducation:
								count = caseSnpDetails.FindAll(u => u.Education.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SWic:
								count = caseSnpDetails.FindAll(u => u.Wic.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SDisable:
								count = caseSnpDetails.FindAll(u => u.Disable.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SDrvlic:
								count = caseSnpDetails.FindAll(u => u.Drvlic.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SEmployed:
								count = caseSnpDetails.FindAll(u => u.Employed.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SEthinic:
								count = caseSnpDetails.FindAll(u => u.Ethnic.Trim().Equals(item.RespCd)).Count;
								break;
							//case Consts.RankQues.SExpireWorkDate:
							//    foreach (CaseSnpEntity snpDate in caseSnpDetails)
							//    {
							//        if (snpDate.ExpireWorkDate != string.Empty)
							//            if (Convert.ToDateTime(snpDate.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//            {
							//                count = count + 1;
							//            }
							//    }
							//    break;
							case Consts.RankQues.SFarmer:
								count = caseSnpDetails.FindAll(u => u.Farmer.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SFoodStamps:
								count = caseSnpDetails.FindAll(u => u.FoodStampsDesc.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SFThours:
								foreach (var snpNumeric in caseSnpDetails)
									if (snpNumeric.FullTimeHours != string.Empty)
										if (Convert.ToDecimal(snpNumeric.FullTimeHours) >=
											Convert.ToDecimal(item.GtNum) &&
											Convert.ToDecimal(snpNumeric.FullTimeHours) <=
											Convert.ToDecimal(item.LtNum))
											count = count + 1;
								break;
							case Consts.RankQues.SHealthIns:
								count = caseSnpDetails.FindAll(u => u.HealthIns.Trim().Equals(item.RespCd)).Count;
								break;
							//case Consts.RankQues.SHireDate:
							//    foreach (CaseSnpEntity snpDate in caseSnpDetails)
							//    {
							//        if (snpDate.HireDate != string.Empty)
							//            if (Convert.ToDateTime(snpDate.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//            {
							//                count = count + 1;
							//            }
							//    }
							//    break;
							case Consts.RankQues.SHourlyWage:
								foreach (var snpNumeric in caseSnpDetails)
									if (snpNumeric.HourlyWage != string.Empty)
										if (Convert.ToDecimal(snpNumeric.HourlyWage) >= Convert.ToDecimal(item.GtNum) &&
											Convert.ToDecimal(snpNumeric.HourlyWage) <= Convert.ToDecimal(item.LtNum))
											count = count + 1;
								break;
							case Consts.RankQues.SjobCategory:
								count = caseSnpDetails.FindAll(u => u.JobCategory.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SjobTitle:
								count = caseSnpDetails.FindAll(u => u.JobTitle.Trim().Equals(item.RespCd)).Count;
								break;
							//case Consts.RankQues.SLastWorkDate:
							//    foreach (CaseSnpEntity snpDate in caseSnpDetails)
							//    {
							//        if (snpDate.LastWorkDate != string.Empty)
							//            if (Convert.ToDateTime(snpDate.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpDate.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//            {
							//                count = count + 1;
							//            }
							//    }
							//    break;
							case Consts.RankQues.SLegalTowork:
								count = caseSnpDetails.FindAll(u => u.LegalTowork.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SMartialStatus:
								count = caseSnpDetails.FindAll(u => u.MaritalStatus.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SMemberCode:
								count = caseSnpDetails.FindAll(u => u.MemberCode.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SNofcjob:
								foreach (var snpNumeric in caseSnpDetails)
									if (snpNumeric.NumberOfcjobs != string.Empty)
										if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >=
											Convert.ToDecimal(item.GtNum) &&
											Convert.ToDecimal(snpNumeric.NumberOfcjobs) <=
											Convert.ToDecimal(item.LtNum))
											count = count + 1;
								break;
							case Consts.RankQues.SNofljobs:
								foreach (var snpNumeric in caseSnpDetails)
									if (snpNumeric.NumberofLvjobs != string.Empty)
										if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >=
											Convert.ToDecimal(item.GtNum) &&
											Convert.ToDecimal(snpNumeric.NumberofLvjobs) <=
											Convert.ToDecimal(item.LtNum))
											count = count + 1;
								break;
							case Consts.RankQues.SPFrequency:
								count = caseSnpDetails.FindAll(u => u.PayFrequency.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SPregnant:
								count = caseSnpDetails.FindAll(u => u.Pregnant.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SPThours:
								foreach (var snpNumeric in caseSnpDetails)
									if (snpNumeric.PartTimeHours != string.Empty)
										if (Convert.ToDecimal(snpNumeric.PartTimeHours) >=
											Convert.ToDecimal(item.GtNum) &&
											Convert.ToDecimal(snpNumeric.PartTimeHours) <=
											Convert.ToDecimal(item.LtNum))
											count = count + 1;
								break;
							case Consts.RankQues.SRace:
								count = caseSnpDetails.FindAll(u => u.Race.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SRelitran:
								count = caseSnpDetails.FindAll(u => u.Relitran.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SResident:
								count = caseSnpDetails.FindAll(u => u.Resident.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SRshift:
								count = caseSnpDetails.FindAll(u => u.RShift.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SSEmploy:
								count = caseSnpDetails.FindAll(u => u.SeasonalEmploy.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SSex:
								count = caseSnpDetails.FindAll(u => u.Sex.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SSnpVet:
								count = caseSnpDetails.FindAll(u => u.Vet.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SStatus:
								count = caseSnpDetails.FindAll(u => u.Status.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.STranserv:
								count = caseSnpDetails.FindAll(u => u.Transerv.Trim().Equals(item.RespCd)).Count;
								break;
							case Consts.RankQues.SworkLimit:
								count = caseSnpDetails.FindAll(u => u.WorkLimit.Trim().Equals(item.RespCd)).Count;
								break;
						}

						if (caseSnpDetails.Count == count)
							intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
					}
					else
					{
						switch (ResponceType)
						{
							case "D":
								if (listRelationstring.Contains(item.Relation))
									if (listCodestring.Contains(item.RespCd))
										intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
								break;
							case "N":
								foreach (var snpNumeric in caseSnpDetails)
									switch (FilterCode)
									{
										//case Consts.RankQues.SAge:
										//    if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
										//    {
										//        DateTime EndDate = GetEndDateAgeCalculation(item.AgeClcInd.Trim(), propMstRank);
										//        int AgeMonth = _model.lookupDataAccess.GetAgeCalculationMonths(Convert.ToDateTime(snpNumeric.AltBdate), EndDate);
										//        if (AgeMonth >= Convert.ToDecimal(item.GtNum) && AgeMonth <= Convert.ToDecimal(item.LtNum))
										//        {
										//            intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
										//        }
										//    }
										//    break;

										case Consts.RankQues.SNofcjob:
											if (snpNumeric.NumberOfcjobs != string.Empty &&
												item.Relation.Trim() == snpNumeric.MemberCode)
												if (Convert.ToDecimal(snpNumeric.NumberOfcjobs) >=
													Convert.ToDecimal(item.GtNum) &&
													Convert.ToDecimal(snpNumeric.NumberOfcjobs) <=
													Convert.ToDecimal(item.LtNum))
													intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
											break;
										case Consts.RankQues.SNofljobs:
											if (snpNumeric.NumberofLvjobs != string.Empty &&
												item.Relation.Trim() == snpNumeric.MemberCode)
												if (Convert.ToDecimal(snpNumeric.NumberofLvjobs) >=
													Convert.ToDecimal(item.GtNum) &&
													Convert.ToDecimal(snpNumeric.NumberofLvjobs) <=
													Convert.ToDecimal(item.LtNum))
													intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
											break;
										case Consts.RankQues.SFThours:
											if (snpNumeric.FullTimeHours != string.Empty &&
												item.Relation.Trim() == snpNumeric.MemberCode)
												if (Convert.ToDecimal(snpNumeric.FullTimeHours) >=
													Convert.ToDecimal(item.GtNum) &&
													Convert.ToDecimal(snpNumeric.FullTimeHours) <=
													Convert.ToDecimal(item.LtNum))
													intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
											break;
										case Consts.RankQues.SPThours:
											if (snpNumeric.PartTimeHours != string.Empty &&
												item.Relation.Trim() == snpNumeric.MemberCode)
												if (Convert.ToDecimal(snpNumeric.PartTimeHours) >=
													Convert.ToDecimal(item.GtNum) &&
													Convert.ToDecimal(snpNumeric.PartTimeHours) <=
													Convert.ToDecimal(item.LtNum))
													intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
											break;
										case Consts.RankQues.SHourlyWage:
											if (snpNumeric.HourlyWage != string.Empty &&
												item.Relation.Trim() == snpNumeric.MemberCode)
												if (Convert.ToDecimal(snpNumeric.HourlyWage) >=
													Convert.ToDecimal(item.GtNum) &&
													Convert.ToDecimal(snpNumeric.HourlyWage) <=
													Convert.ToDecimal(item.LtNum))
													intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
											break;
									}

								break;
							//case "B":
							//case "T":
							//    foreach (CaseSnpEntity snpNumeric in caseSnpDetails)
							//    {

							//        switch (FilterCode)
							//        {
							//            case Consts.RankQues.SAltBdate:
							//                if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
							//                    if (Convert.ToDateTime(snpNumeric.AltBdate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.AltBdate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//                    {
							//                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
							//                    }
							//                break;
							//            case Consts.RankQues.SExpireWorkDate:
							//                if (snpNumeric.ExpireWorkDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
							//                    if (Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.ExpireWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//                    {
							//                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
							//                    }
							//                break;
							//            case Consts.RankQues.SLastWorkDate:
							//                if (snpNumeric.AltBdate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
							//                    if (Convert.ToDateTime(snpNumeric.LastWorkDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.LastWorkDate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//                    {
							//                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
							//                    }
							//                break;
							//            case Consts.RankQues.SHireDate:
							//                if (snpNumeric.HireDate != string.Empty && item.Relation.Trim() == snpNumeric.MemberCode)
							//                    if (Convert.ToDateTime(snpNumeric.HireDate).Date >= Convert.ToDateTime(item.GtDate).Date && Convert.ToDateTime(snpNumeric.HireDate).Date <= Convert.ToDateTime(item.LtDate).Date)
							//                    {
							//                        intSnpPoints = intSnpPoints + Convert.ToInt32(item.Points);
							//                    }
							//                break;


							//        }
							//    }
							//    break;
						}
					}
				}

			return intSnpPoints;
		}

		public DateTime GetEndDateAgeCalculation(string Type, CaseMstEntity caseMst)
		{
			var EndDate = DateTime.Now.Date;
			if (Type == "T")
			{
				EndDate = DateTime.Now.Date;
			}
			else if (Type == "I")
			{
				EndDate = Convert.ToDateTime(caseMst.IntakeDate);
			}
			else if (Type == "K")
			{
				var strDate = DateTime.Now.Date.ToShortDateString();
				string strYear;
				var zipentity = propzipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
				if (zipentity != null)
				{
					if (zipentity.Zcrhssyear.Trim() == "2")
						strYear = DateTime.Now.AddYears(1).Year.ToString();
					else
						strYear = DateTime.Now.Year.ToString();
					strDate = zipentity.Zcrhssmo + "/" + zipentity.Zcrhssday + "/" + strYear;
				}

				EndDate = Convert.ToDateTime(strDate);
			}

			return EndDate;
		}

		#endregion


		private void button1_Click_1(object sender, EventArgs e)
		{
			using (var objClientFrom = new HSS20001ClientSupplierForm(BaseForm, TmsSupplierPrivileges))
			{
				objClientFrom.FormClosed += new FormClosedEventHandler(objClientFrom_FormClosed);
				objClientFrom.ShowDialog();
			}
		}

		private void objClientFrom_FormClosed(object sender, FormClosedEventArgs e)
		{
			PrimarySourceSetting();
		}

		private void DisplayIncomeMsgs()
		{
			lblFraudalert.Visible = false;
			if (BaseForm.BaseCaseMstListEntity != null)
			{
				CaseMST = BaseForm
					.BaseCaseMstListEntity
						[0]; //_model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
				if (CaseMST != null)
				{
					lblIncomeVerified.Text = "";
					if (ProgramDefinition.IncomeVerMsg.Equals("Y"))
					{
						if (string.IsNullOrEmpty(CaseMST.EligDate))
						{
							lblIncomeVerified.Text = "Income Not Verified";
						}
						else
						{
							var caseVerList = _model.CaseMstData.GetCASEVeradpyalst(
								BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear,
								BaseForm.BaseApplicationNo, string.Empty, string.Empty);
							if (caseVerList.Count > 0)
							{
								if (!(Convert.ToDecimal(CaseMST.ProgIncome == string.Empty
															? "0"
															: CaseMST.ProgIncome) ==
									Convert.ToDecimal(caseVerList[0].IncomeAmount == string.Empty
														? "0"
														: caseVerList[0].IncomeAmount)))
									lblIncomeVerified.Text =
										"Household income needs to be reverified as the income was changed";
							}
							else
							{
								lblIncomeVerified.Text = "Income Not Verified";
							}
						}
					}
					else
					{
						lblIncomeVerified.Text = "";
					}

					if (Privileges != null)
						if (Privileges.ModuleCode == "08")
							if (CaseMST.CbFraud == "1" && CaseMST.FraudDate != string.Empty)
								lblFraudalert.Visible = true;
				}
			}
		}

		private void PrimarySourceSetting()
		{
			if (BaseForm.BaseCaseMstListEntity != null)
				CommonFunctions.SetComboBoxValue(cmbPrimarySourceoHeat, BaseForm.BaseCaseMstListEntity[0].Source);
		}


		#region PreassesDatafilling

		private void fillPreassCustomQuestions(CaseSnpEntity casesnpdata, string strType)
		{
			var custQuestions = proppreassesQuestions;
			if (strType == "A")
				custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "A");
			if (strType == "I")
				custQuestions = custQuestions.FindAll(u => u.CUSTACTIVECUST.ToUpper() == "I");


			var custResponses = _model.CaseMstData.GetPreassesQuestionAnswers(casesnpdata, "PRESRESP");
			var isResponse = false;

			gvwPreassesData.Rows.Clear();
			if (custQuestions.Count > 0)
			{
				//foreach (PreassessQuesEntity preassesdata in preassessMasterEntity)
				//{

				foreach (var preassesdata in preassessMasterEntity)
				{
					var preassessChildList = preassessChildEntity.FindAll(u => u.PRECHILD_DID == preassesdata.Code);


					var boolQuestions = false;
					foreach (var preasschilddata in preassessChildList)
					{
						var dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
						if (dr != null) boolQuestions = true;
					}

					if (boolQuestions)
					{
						var rowIndex = gvwPreassesData.Rows.Add(preassesdata.Desc, string.Empty, string.Empty);
						gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
						preassessChildList = preassessChildList.OrderBy(u => Convert.ToInt32(u.PRECHILD_SEQ)).ToList();
						foreach (var preasschilddata in preassessChildList)
						{
							var dr = custQuestions.Find(u => u.CUSTCODE == preasschilddata.PRECHILD_QID);
							if (dr != null)
							{
								var custCode = dr.CUSTCODE.ToString();
								var response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();


								rowIndex = gvwPreassesData.Rows.Add(dr.CUSTDESC, string.Empty, string.Empty);
								gvwPreassesData.Rows[rowIndex].Cells["gvtPQuestions"].Tag = "N";

								gvwPreassesData.Rows[rowIndex].Tag = dr;

								var fieldType = dr.CUSTRESPTYPE.ToString();

								var custQuestionResp = string.Empty;
								var custQuestionCode = string.Empty;
								if (true) //!Mode.Equals(Consts.Common.Add))
									if (response != null && response.Count > 0)
									{
										if (!dr.CUSTACTIVECUST.Equals("A"))
											gvwPreassesData.Rows[rowIndex].Cells["gvtPQuestions"].Tag = "Y";

										isResponse = true;
										if (fieldType.Equals("D"))
										{
											var custReponseEntity =
												_model.FieldControls.GetCustomResponses(
													"PREASSES", response[0].ACTCODE);

											foreach (var custResp in response)
											{
												var code = custResp.ACTMULTRESP.Trim();
												var custRespEntity =
													custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
												if (custRespEntity != null)
												{
													custQuestionResp += custRespEntity.RespDesc;
													custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
												}
											}

											gvwPreassesData.Rows[rowIndex].Cells[1].Tag = custQuestionCode;
											gvwPreassesData.Rows[rowIndex].Cells[1].Value = custQuestionResp;
										}
										else if (fieldType.Equals("C"))
										{
											custQuestionResp = response[0].ACTALPHARESP;
											gvwPreassesData.Rows[rowIndex].Cells[1].Tag = response[0].ACTALPHARESP;
											gvwPreassesData.Rows[rowIndex].Cells[1].Value = response[0].ACTALPHARESP;
										}
										else if (fieldType.Equals("N"))
										{
											custQuestionResp = response[0].ACTNUMRESP.ToString();
										}
										else if (fieldType.Equals("T"))
										{
											custQuestionResp =
												LookupDataAccess.Getdate(response[0].ACTDATERESP.ToString());
										}
										else
										{
											custQuestionResp = response[0].ACTALPHARESP.ToString();
										}
									}

								if (!dr.CUSTACTIVECUST.Equals("A"))
									gvwPreassesData.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
							}
						}
					}
				}

				gvwPreassesData.Update();
			}
		}

		#endregion

		private void btnDPoints_Click(object sender, EventArgs e)
		{
			using (var dimensionform = new PreassesDimentionForm(BaseForm, string.Empty, string.Empty, null))
			{
				dimensionform.ShowDialog();
			}
		}

		private void Enableoutservicecmb()
		{
			if (propAgencyControlDetails != null) //Added by Sudheer on 12/13/2016
			{
				if (propAgencyControlDetails.State.ToUpper() == "TX")
					cmbOutService.Visible = true;
				else
					cmbOutService.Visible = false;
			}
			else //Added by Sudheer on 12/13/2016
			{
				cmbOutService.Visible = false;
			}
		}

		private void FamilyTypeWaringMsg(string strFamilyType)
		{
			try
			{
				if (BaseForm.BaseAgencyControlDetails.FTypeSwitch == "Y")
					if (cmbFamilyType.Items.Count > 0)
						if (strFamilyType != string.Empty)
						{
							var inthousehold = CaseMST.NoInhh.ToString() == string.Empty
								? 0
								: Convert.ToInt32(CaseMST.NoInhh);
							var decimalfamilytype =
								((Utilities.ListItem) cmbFamilyType.SelectedItem).ValueDisplayCode == null
									? 0
									: Convert.ToDecimal(((Utilities.ListItem) cmbFamilyType.SelectedItem)
														.ValueDisplayCode);
							if (decimalfamilytype > 0)
							{
								if (decimalfamilytype != inthousehold)
									lblFamilyTypeWarning.Visible = true;
								else
									lblFamilyTypeWarning.Visible = false;
							}
						}
			}
			catch (Exception ex)
			{
			}
		}
	}
}