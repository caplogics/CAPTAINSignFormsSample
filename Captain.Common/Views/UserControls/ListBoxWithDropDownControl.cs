/************************************************************************************
* Class Name    : CustomListBox
* Author        : 
* Created Date  : 
* Version       : 
* Description   : This is CustomListView Control , which is used displays the TableType Attributes
************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Wisej.Web;
using Captain.Common.Controllers;
using Captain.Common.Exceptions;
using Captain.Common.Handlers;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Resources;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.UserControls.Base;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class ListBoxWithDropDownControl : AttributesBaseUserControl
    {
        public ListBoxWithDropDownControl()
        {
            InitializeComponent();

            try
            {
                RemovedItems = new List<ListItem>();
            }
            catch
            {
            }
        }

        #region Public Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListBox InternalListBox
        {
            get
            {
                return internalListBox;
            }
        }

        public int TextBoxWidth
        {
            set
            {
                comboBox.Width = value;
            }
        }

        public bool PanelHeader
        {
            set
            {
                panelHeader.Visible = value;
            }
        }

        public override bool IsModified
        {
            get
            {
                return base.IsModified;
            }
            set
            {
                base.IsModified = value;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ListItem> RemovedItems { get; set; }

        #endregion

        #region Public Methods

        public void RebuildList()
        {
            //foreach ()
            //{
            //    if (isNotCleared)
            //    {
            //        comboBox.Items.Clear();
            //        isNotCleared = false;
            //    }

            //    ListItem listItem = new ListItem();
            //    listItem.ID = attributeValueLookupValueType.attributeValueLookupValuesId;
            //    listItem.Text = attributeValueLookupValueType.loopkupValue;
            //    listItem.Value = "I";
            //    listItem.ValueDisplayCode = attributeValueLookupValueType.loopkupValueDisplayCode;

            //    attributeLookupValueList.Add(listItem);
            //    comboBox.Items.Add(listItem);
            //}
        }

        public void BindDropDown(string attributeName)
        {

        }

        public override void SetToolTip()
        {
            string toolTipValue = Consts.Common.EmptyValue;
            controlToolTip.SetToolTip(InternalListBox, toolTipValue);
        }

        public override void SetEditable(bool allowEdit)
        {
            PanelHeader = allowEdit;
            Update();
        }

        #endregion

        #region Event Handlers

        private void OnAddItem(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                ListItem listItem = comboBox.SelectedItem as ListItem;
                var items = (from item in internalListBox.Items.Cast<ListItem>().ToList()
                             select item).ToList();

                if (items.Find(i => i.Text.Equals(listItem.Text)) == null)
                {
                    listItem.Value = "Insert";
                    internalListBox.Items.Add(listItem);
                    IsModified = true;
                }
            }
        }

        private void OnDeleteItem(object sender, EventArgs e)
        {
            if (internalListBox.SelectedItem != null)
            {
                ListItem item = internalListBox.SelectedItem as ListItem;
                internalListBox.Items.Remove(internalListBox.SelectedItem);
                if (!item.ID.Equals(string.Empty) && item.Value.ToString() == "Update")
                {
                    item.Value = "Delete";
                    RemovedItems.Add(item);
                    IsModified = true;
                }

            }
        }

        #endregion
    }
}
