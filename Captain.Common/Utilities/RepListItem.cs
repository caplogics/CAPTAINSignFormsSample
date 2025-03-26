/************************************************************************************
* Class Name    : RepListItem
* Author        : 
* Created Date  : 
* Version       : 
* Description   : This class used to define the listitem properties
*************************************************************************************/

using System;
using System.ComponentModel;

namespace Captain.Common.Utilities
{
    [Serializable()]
    public class RepListItem : IComparable<RepListItem>
    {
        public RepListItem()
        {
            Text = string.Empty;
            Value = string.Empty;
            ValueDisplayCode = string.Empty;
            ID = string.Empty;
            IsInstanceItem = false;
        }

        public RepListItem(string text)
        {
            Text = text;
            Value = text;
            ID = text;
            IsInstanceItem = false;
        }

        public RepListItem(string text, object value)
        {
            Text = text;
            Value = value;
            ID = string.Empty;
            IsInstanceItem = false;
        }

        public RepListItem(string text, object value, string id)
        {
            Text = text;
            Value = value;
            ID = id;
            IsInstanceItem = false;
        }

        public RepListItem(string text, object value, string id, bool isInstanceItem)
        {
            Text = text;
            Value = value;
            ID = id;
            IsInstanceItem = isInstanceItem;
        }

        public RepListItem(string text, object value, string id, string valueDisplayCode)
        {
            Text = text;
            Value = value;
            ID = id;
            ValueDisplayCode = valueDisplayCode;
        }

        public string ID { get; set; }

        public string Text { get; set; }

        [Browsable(false)]
        public object Value { get; set; }

        public string ValueDisplayCode { get; set; }

        public bool IsInstanceItem { get; set; }

        public string ToolTip { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public string GetValues()
        {
            return Text + Consts.Common.Tilda + ID + Consts.Common.Tilda + Value.ToString();
        }

        public int CompareTo(RepListItem other)
        {
            return Text.CompareTo(other.Text);
        }
    }
}
