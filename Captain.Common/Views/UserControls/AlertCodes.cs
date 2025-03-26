#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class AlertCodes : UserControl
    {
        private CaptainModel _model = null;
        public AlertCodes(BaseForm baseForm, PrivilegeEntity privileges,ProgramDefinitionEntity ProgramDefDep)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();
            CaseMST = _model.CaseMstData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            if (CaseMST != null)
            {
                txtAlertCodes.Text = CaseMST.AlertCodes;
                btnAlertCodes.Enabled = true;               
            }

            if (privileges != null)
            {
                if (Privileges.AddPriv.Equals("true") || Privileges.ChangePriv.Equals("true"))
                {
                    btnAlertCodes.Enabled = true;
                }
                else
                {
                    btnAlertCodes.Enabled = false;
                }
            }

        }

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public CaseMstEntity CaseMST { get; set; }

        private void btnAlertCodes_Click(object sender, EventArgs e)
        {
            if (BaseForm.BaseCaseSnpEntity != null)
            {
                AlertCodeForm objform = new AlertCodeForm(BaseForm, Privileges, txtAlertCodes.Text, CaseMST);
                objform.StartPosition = FormStartPosition.CenterScreen;
                objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
                objform.ShowDialog();
            }
        }

        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            txtAlertCodes.Text = form.propAlertCode;
            
        }

        //private void contextMenu1_Popup(object sender, EventArgs e)
        //{
        //    contextMenu1.MenuItems.Clear();
        //    if (CaseMST != null)
        //    {
        //        List<string> alertCodesList = new List<string>();
        //        string AlertCodes = txtAlertCodes.Text;
        //        if (!string.IsNullOrEmpty(AlertCodes))
        //        {
        //            string[] alertCodes = AlertCodes.Split(' ');
        //            for (int i = 0; i < alertCodes.Length; i++)
        //            {
        //                alertCodesList.Add(alertCodes.GetValue(i).ToString());
        //            }
        //        }
        //        List<CommonEntity> listCodes = _model.lookupDataAccess.GetAlertCodes();
        //        listCodes = filterByHIE(listCodes);
        //        foreach (CommonEntity alertCodes in listCodes)
        //        {
        //            MenuItem menuItem = new MenuItem();
        //            menuItem.Text = alertCodes.Desc;
        //            menuItem.Tag = alertCodes.Code;
        //            if (alertCodesList.Contains(alertCodes.Code))
        //            {
        //                menuItem.Checked = true;
        //            }
        //            contextMenu1.MenuItems.Add(menuItem);

        //        }
        //    }
        //}

        //private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        //{
        //    string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
        //    LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******"));
        //    return LookupValues;
        //}

       

        //private void txtAlertCodes_MenuClick(object objSource, MenuItemEventArgs objArgs)
        //{
        //    //if (Privileges.AddPriv.Equals("true") || Privileges.ChangePriv.Equals("true"))
        //    //{
        //    //    if (objArgs.MenuItem.Tag is string)
        //    //    {
        //    //        string selectedValue = objArgs.MenuItem.Text;
        //    //        string selectedCode = txtAlertCodes.Text.Trim();
        //    //        if (!objArgs.MenuItem.Checked)
        //    //        {
        //    //            if (!selectedCode.Equals(string.Empty)) selectedCode += " ";
        //    //            selectedCode += objArgs.MenuItem.Tag as string;
        //    //        }
        //    //        else
        //    //        {
        //    //            selectedCode = selectedCode.Replace(objArgs.MenuItem.Tag as string, string.Empty);
        //    //        }
        //    //        selectedCode = selectedCode.Replace("  ", " ");
        //    //        selectedCode = selectedCode.TrimStart();
        //    //        // List<string> strALertcode = new List<string>(selectedCode.Split(' '));
        //    //        if (selectedCode.Length <= 11)
        //    //        {
        //    //            //foreach (string item in strALertcode)
        //    //            //{
        //    //            //    txtAlertCodes.Text = item.ToString() + " ";
        //    //            //}

        //    //            // string selectcodeLength = selectedCode.);

        //    //            txtAlertCodes.Text = selectedCode;
        //    //            CaseMstEntity CaseMstEntity = new CaseMstEntity();

        //    //            CaseMstEntity.ApplAgency = CaseMST.ApplAgency;
        //    //            CaseMstEntity.ApplDept = CaseMST.ApplDept;
        //    //            CaseMstEntity.ApplProgram = CaseMST.ApplProgram;
        //    //            CaseMstEntity.ApplYr = CaseMST.ApplYr;
        //    //            CaseMstEntity.ApplNo = CaseMST.ApplNo;
        //    //            CaseMstEntity.LstcOperator4 = BaseForm.UserID;
        //    //            CaseMstEntity.AlertCodes = selectedCode;
        //    //            CaseMstEntity.Mode = "AlertCodes";
        //    //            string strApplNo = string.Empty;
        //    //            string strClientId = string.Empty;
        //    //            string strFamilyId = string.Empty;
        //    //            _model.CaseMstData.InsertUpdateCaseMst(CaseMstEntity, out strApplNo, out strClientId, out strFamilyId);
        //    //        }
        //    //        else
        //    //        {
        //    //            CommonFunctions.MessageBoxDisplay("You Can't Add More Than 6 Alert Codes");
        //    //        }
        //    //    }
        //   // }
        //}


    }
}