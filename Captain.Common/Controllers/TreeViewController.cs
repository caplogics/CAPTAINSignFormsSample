/**********************************************************************************************************
 * Class Name   : TreeViewController
 * Author       : Chittibabu Bellamkonda
 * Created Date : 
 * Version      : 
 * Description  : Used to manipulate the treeview for all modules.
 **********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using System.Web;
using Captain.Common.Interfaces;
using Captain.Common.Model.Parameters;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Wisej.Web.Ext.NavigationBar;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Controllers
{
	public class TreeViewController
	{
		#region Variables

		private TagClass _lastDraggedTagClass = null;
		private TagClass _lastDroppedOntoTagClass = null;
		private TreeNode _clipboardSourceNode = null;
		private TreeNode _clipboardTargetNode = null;

		#endregion

		#region Constructor

		public TreeViewController(IBaseForm baseForm)
		{
			BaseForm = baseForm;
			// Model = baseForm.Model;

		}

		#endregion

		#region Properties

		public IBaseForm BaseForm { get; set; }

		public NavigationBar NavigationBar { get; set; }

		public string CurrentView { get; set; }

		public string BusinessModuleID { get; set; }

		public string HIE { get; set; }

		public TagClass ExpectedRootNode { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes the tree.
		/// </summary>
		/// <param name="treeViewParameter"></param>
		/// <returns></returns>
		public bool Initialize(TreeViewControllerParameter treeViewParameter)
		{
			bool results = false;
			NavigationBar = treeViewParameter.NavigationBar;
			BusinessModuleID = treeViewParameter.BusinessModuleID;
			HIE = treeViewParameter.Hierarchy;

			switch (treeViewParameter.TreeType)
			{
				case TreeType.Administration:
					PopulateAdministrationTree(Consts.Applications.Code.Administration);
					break;
				case TreeType.HeadStart:
					PopulateTreeMenu(Consts.Applications.Code.HeadStart);
					break;
				case TreeType.CaseManagement:
					PopulateTreeMenu(Consts.Applications.Code.CaseManagement);
					break;
				case TreeType.EnergyRI:
					PopulateTreeMenu(Consts.Applications.Code.EnergyRI);
					break;
				case TreeType.EnergyCT:
					PopulateTreeMenu(Consts.Applications.Code.EnergyCT);
					break;
				case TreeType.AccountsReceivable:
					PopulateTreeMenu(Consts.Applications.Code.AccountsReceivable);
					break;
				case TreeType.HousingWeatherization:
					PopulateTreeMenu(Consts.Applications.Code.HousingWeatherization);
					break;
				case TreeType.EmergencyAssistance:
					PopulateTreeMenu(Consts.Applications.Code.EmergencyAssistance);
					break;
				case TreeType.DashBoard:
					PopulateTreeMenu(Consts.Applications.Code.DashBoard);
					break;
				case TreeType.HealthyStart:
					PopulateTreeMenu(Consts.Applications.Code.HealthyStart);
					break;
				case TreeType.AgencyPartner:
					PopulateTreeMenu(Consts.Applications.Code.AgencyPartner);
					break;
			}



			return results;
		}

		/// <summary>
		/// Used to get and populate the Screens & Reports.
		/// </summary>
		/// <param name="parentTreeNode"></param>
		/// <param name="parentTagClass"></param>
		/// <param name="hasCheckBox"></param>
		/// <param name="isChecked"></param>
		/// <param name="useCachedValues"></param>
		/// <param name="countryFilterList"></param>
		/// <returns></returns>
		public bool PopulateAdministrationTree(string code)
		{
			string view = string.Empty;
			NavigationBarItem parentNavigationBarItem ;
			CaptainModel model = new CaptainModel();
			List<PrivilegeEntity> userPrivilege = model.UserProfileAccess.GetScreensByUserID(code, BaseForm.UserID, string.Empty);

			List<MenuBranchEntity> menubranhlist = model.UserProfileAccess.GetMenuBranches();
			if (menubranhlist.Count > 0)
			{
				foreach (MenuBranchEntity menuitem in menubranhlist)
				{

					parentNavigationBarItem = new NavigationBarItem();
					//parentNavigationBarItem.Icon = $"resource.wx/Resources.Icons.{Consts.Icons16x16.AddItem}";
					parentNavigationBarItem.Icon = "Resources/Images/ScrRep.png";
					parentNavigationBarItem.Text = menuitem.MemberDesc.Trim();
					parentNavigationBarItem.ImageIndex = 0;
					if (code == "01" && BaseForm.UserID.ToUpper() == "JAKE")
					{
						PrivilegeEntity privilegeAdmn12 = new PrivilegeEntity();
						privilegeAdmn12.UserID = BaseForm.UserID;
						privilegeAdmn12.ModuleCode = "01";
						privilegeAdmn12.Program = "ADMN0012";
						privilegeAdmn12.Hierarchy = "******";
						privilegeAdmn12.AddPriv = "true";
						privilegeAdmn12.ChangePriv = "true";
						privilegeAdmn12.DelPriv = "true";
						privilegeAdmn12.ViewPriv = "true";
						privilegeAdmn12.PrivilegeName = "Agency Control File Maintenance";
						privilegeAdmn12.DateLSTC = string.Empty;
						privilegeAdmn12.LSTCOperator = BaseForm.UserID;
						privilegeAdmn12.DateAdd = string.Empty;
						privilegeAdmn12.AddOperator = BaseForm.UserID;
						privilegeAdmn12.ModuleName = string.Empty;
						privilegeAdmn12.showMenu = "Y";
						privilegeAdmn12.screenType = "SCREEN";
						userPrivilege.Insert(0, privilegeAdmn12);




						privilegeAdmn12 = new PrivilegeEntity();
						privilegeAdmn12.UserID = BaseForm.UserID;
						privilegeAdmn12.ModuleCode = "01";
						privilegeAdmn12.Program = "TRIGPARA";
						privilegeAdmn12.Hierarchy = "******";
						privilegeAdmn12.AddPriv = "true";
						privilegeAdmn12.ChangePriv = "true";
						privilegeAdmn12.DelPriv = "true";
						privilegeAdmn12.ViewPriv = "true";
						privilegeAdmn12.PrivilegeName = "Trigger Parameters";
						privilegeAdmn12.DateLSTC = string.Empty;
						privilegeAdmn12.LSTCOperator = BaseForm.UserID;
						privilegeAdmn12.DateAdd = string.Empty;
						privilegeAdmn12.AddOperator = BaseForm.UserID;
						privilegeAdmn12.ModuleName = string.Empty;
						privilegeAdmn12.showMenu = "Y";
						privilegeAdmn12.screenType = "SCREEN";
						userPrivilege.Insert(0, privilegeAdmn12);



					}
					if (code == "01" && BaseForm.UserID.ToUpper() == "CAPLOGICS")
					{
						PrivilegeEntity privilegeAdmn12 = new PrivilegeEntity();
						privilegeAdmn12.UserID = BaseForm.UserID;
						privilegeAdmn12.ModuleCode = "01";
						privilegeAdmn12.Program = "ADMN0014";
						privilegeAdmn12.Hierarchy = "******";
						privilegeAdmn12.AddPriv = "true";
						privilegeAdmn12.ChangePriv = "true";
						privilegeAdmn12.DelPriv = "true";
						privilegeAdmn12.ViewPriv = "true";
						privilegeAdmn12.PrivilegeName = "CAP LOGICS Settings";
						privilegeAdmn12.DateLSTC = string.Empty;
						privilegeAdmn12.LSTCOperator = BaseForm.UserID;
						privilegeAdmn12.DateAdd = string.Empty;
						privilegeAdmn12.AddOperator = BaseForm.UserID;
						privilegeAdmn12.ModuleName = string.Empty;
						privilegeAdmn12.showMenu = "Y";
						privilegeAdmn12.screenType = "SCREEN";
						userPrivilege.Insert(0, privilegeAdmn12);
					}

					List<PrivilegeEntity> screentypewiselist = userPrivilege.FindAll(u => u.screenType.Trim() == menuitem.MemberCode.Trim());

					if (screentypewiselist != null && screentypewiselist.Count > 0)
					{
						screentypewiselist = screentypewiselist.FindAll(u => u.showMenu.ToString().ToUpper() == "Y");
						screentypewiselist = screentypewiselist.OrderBy(u => u.PrivilegeName).ToList();
						foreach (PrivilegeEntity privilegeEntity in screentypewiselist)
						{
							var childNavigationBarItem = new NavigationBarItem();
							childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
							childNavigationBarItem.ImageIndex = 1;
							childNavigationBarItem.Icon = "Resources/Images/ScrRep.png";
							childNavigationBarItem.Tag = privilegeEntity;
							parentNavigationBarItem.Items.Add(childNavigationBarItem);
						}
						NavigationBar.Items.Add(parentNavigationBarItem);
					}
				}
			}


		   
			//if (userPrivilege != null && userPrivilege.Count > 0)
			//{
			//    userPrivilege = userPrivilege.FindAll(u => u.showMenu.ToString().ToUpper() == "Y");
			//    userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();

			//    foreach (PrivilegeEntity privilegeEntity in userPrivilege)
			//    {
			//        parentNode.Image = new IconResourceHandle(Consts.Icons16x16.AddItem);
			//        TreeNode childNode = new TreeNode(privilegeEntity.PrivilegeName);
			//        childNode.Image = new IconResourceHandle(Consts.Icons16x16.Entry);
			//        childNode.Tag = privilegeEntity;
			//        parentNode.Nodes.Add(childNode);
			//    }
			//    TreeView.Nodes.Add(parentNode);
			//}

			userPrivilege.Clear();
			parentNavigationBarItem = new NavigationBarItem();
			parentNavigationBarItem.Text = "Reports";
			parentNavigationBarItem.Icon = $"resource.wx/Resources.Icons.{Consts.Icons16x16.AddItem}";
			parentNavigationBarItem.ImageIndex = 0;
			userPrivilege = model.UserProfileAccess.GetReportsByUserID(code, BaseForm.UserID);
			if (userPrivilege != null && userPrivilege.Count > 0)
			{
				userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();
				foreach (PrivilegeEntity privilegeEntity in userPrivilege)
				{
					parentNavigationBarItem.ImageIndex = 9;
					var childNavigationBarItem = new NavigationBarItem();
					childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
					childNavigationBarItem.ImageIndex = 1;
					childNavigationBarItem.Tag = privilegeEntity;
					parentNavigationBarItem.Items.Add(childNavigationBarItem);
				}
				NavigationBar.Items.Add(parentNavigationBarItem);
			}

			userPrivilege.Clear();
			parentNavigationBarItem = new NavigationBarItem();
			parentNavigationBarItem.Text = "User Report Maintenance";
			parentNavigationBarItem.ImageIndex = 0;
			userPrivilege = model.UserProfileAccess.GetUserReportMaintenanceByserID(code, BaseForm.UserID);
			if (userPrivilege != null && userPrivilege.Count > 0)
			{
				//if (code != "01")
				//{
				//    userPrivilege = userPrivilege.FindAll(u => u.Hierarchy.Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg));
				//}
				userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();
				foreach (PrivilegeEntity privilegeEntity in userPrivilege)
				{
					var childNavigationBarItem = new NavigationBarItem();
					childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
					childNavigationBarItem.ImageIndex = 1;
					childNavigationBarItem.Tag = privilegeEntity;
					parentNavigationBarItem.Items.Add(childNavigationBarItem);
				}
				NavigationBar.Items.Add(parentNavigationBarItem);
			}

			return true;
		}

		/// <summary>
		/// Used to get and populate the Screens & Reports.
		/// </summary>
		/// <param name="parentTreeNode"></param>
		/// <param name="parentTagClass"></param>
		/// <param name="hasCheckBox"></param>
		/// <param name="isChecked"></param>
		/// <param name="useCachedValues"></param>
		/// <param name="countryFilterList"></param>
		/// <returns></returns>
		public bool PopulateTreeMenu(string code)
		{
			string view = string.Empty;
			NavigationBar.Items.Clear();
			NavigationBarItem parentNavigationBarItem;
			CaptainModel model = new CaptainModel();
			List<PrivilegeEntity> userPrivilege = model.UserProfileAccess.GetScreensByUserID(code, BaseForm.UserID, HIE);

			List<MenuBranchEntity> menubranhlist = model.UserProfileAccess.GetMenuBranches();
			if (menubranhlist.Count > 0)
			{
				foreach (MenuBranchEntity menuitem in menubranhlist)
				{
					
					parentNavigationBarItem = new NavigationBarItem();
					parentNavigationBarItem.Icon = $"resource.wx/Resources.Icons.{Consts.Icons16x16.AddItem}";

					parentNavigationBarItem.Text = menuitem.MemberDesc.Trim();
					parentNavigationBarItem.ImageIndex = 0;

					List<PrivilegeEntity> screentypewiselist = userPrivilege.FindAll(u => u.screenType.Trim() == menuitem.MemberCode.Trim());

					if (screentypewiselist != null && screentypewiselist.Count > 0)
					{
						screentypewiselist = screentypewiselist.FindAll(u => u.showMenu.ToString().ToUpper() == "Y");
						screentypewiselist = screentypewiselist.OrderBy(u => u.PrivilegeName).ToList();
						foreach (PrivilegeEntity privilegeEntity in screentypewiselist)
						{
							var childNode = new SubNavigationTab();
							childNode.Text = privilegeEntity.PrivilegeName;
							childNode.ImageIndex = 1;
							childNode.Tag = privilegeEntity;
							parentNavigationBarItem.Items.Add(childNode);
						}

						parentNavigationBarItem.Expanded = true;
						
						NavigationBar.Items.Add(parentNavigationBarItem);
						NavigationBar.SelectedItem = parentNavigationBarItem;
					}
				}
			}

			userPrivilege.Clear();
			parentNavigationBarItem = new NavigationBarItem();
			parentNavigationBarItem.Text = "Reports";
			parentNavigationBarItem.ImageIndex = 0;
			parentNavigationBarItem.Icon = "Resources/Images/Dashboard.png";
			userPrivilege = model.UserProfileAccess.GetReportsByUserID(code, BaseForm.UserID);
			if (userPrivilege != null && userPrivilege.Count > 0)
			{
				//if (code != "01")
				//{
				//    userPrivilege = userPrivilege.FindAll(u => u.Hierarchy.Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg));
				//}
				userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();
				foreach (PrivilegeEntity privilegeEntity in userPrivilege)
				{
					var childNavigationBarItem = new SubNavigationTab();
					childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
					childNavigationBarItem.ImageIndex = 1;
					childNavigationBarItem.Tag = privilegeEntity;
					parentNavigationBarItem.Items.Add(childNavigationBarItem);
				}
				NavigationBar.Items.Add(parentNavigationBarItem);
			}

			userPrivilege.Clear();
			parentNavigationBarItem = new NavigationBarItem();
			parentNavigationBarItem.Text = "User Report Maintenance";
			parentNavigationBarItem.ImageIndex = 0;
			userPrivilege = model.UserProfileAccess.GetUserReportMaintenanceByserID(code, BaseForm.UserID);
			if (userPrivilege != null && userPrivilege.Count > 0)
			{
				//if (code != "01")
				//{
				//    userPrivilege = userPrivilege.FindAll(u => u.Hierarchy.Equals(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg));
				//}
				userPrivilege = userPrivilege.OrderBy(u => u.PrivilegeName).ToList();
				foreach (PrivilegeEntity privilegeEntity in userPrivilege)
				{
					var childNavigationBarItem = new NavigationBarItem();
					childNavigationBarItem.Text = privilegeEntity.PrivilegeName;
					childNavigationBarItem.ImageIndex = 1;
					childNavigationBarItem.Tag = privilegeEntity;
					parentNavigationBarItem.Items.Add(childNavigationBarItem);
				}
				NavigationBar.Items.Add(parentNavigationBarItem);
			}

			NavigationBar.Update();
			NavigationBar.ResumeLayout();
			return true;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Generates the tree for administration.
		/// </summary>
		/// <returns>Returns the true if successfull. Otherwise, returns false</returns>
		private bool PopulateAdministrationChildrenNodes()
		{
			return true;
		}

		/// <summary>
		/// Generates a tree from RelatedViewNodeType list
		/// </summary>
		/// <param name="parentTreeNode"></param>
		/// <param name="nodeTypeList"></param>
		/// <param name="hasCheckBox"></param>
		/// <param name="isChecked"></param>
		private void PopulateChildrenNodeType(TreeNode parentTreeNode, List<TagClass> nodeTypeList, bool hasCheckBox, bool isChecked)
		{
			StringBuilder timesBuilder = new StringBuilder();
			timesBuilder.AppendLine(DateTime.Now.ToString());
			NavigationBarItemCollection nodes = NavigationBar.Items;


			timesBuilder.AppendLine("End Update: " + DateTime.Now.ToString());
		}

		/// <summary>
		/// Generates the top tree for submission manager.
		/// </summary>
		/// <returns>Returns the true if successfull. Otherwise, returns false</returns>
		private bool PopulateTopTreeNodes()
		{
			bool result = false;

			return result;
		}

		/// <summary>
		/// Moves the current ClipboardNode.
		/// </summary>
		private void MoveClipboardNode()
		{
			if (_clipboardSourceNode == null)
			{
				_clipboardTargetNode = null;
				return;
			}

			if (_clipboardTargetNode == null)
			{
				_clipboardSourceNode = null;
				return;
			}

			BaseForm.RefreshNode(_clipboardSourceNode.Parent);

			_clipboardSourceNode = null;
			_clipboardTargetNode = null;
		}

		#endregion

	}
}
