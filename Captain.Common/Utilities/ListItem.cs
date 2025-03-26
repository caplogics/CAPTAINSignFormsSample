/************************************************************************************
* Class Name    : ListItem
* Author        : 
* Created Date  : 
* Version       : 
* Description   : This class used to define the listitem properties
*************************************************************************************/

using System;
using System.ComponentModel;
using System.Drawing;

namespace Captain.Common.Utilities
{
    [Serializable()]
    public class ListItem : IComparable<ListItem>
    {
        public ListItem()
        {
            Text = string.Empty;
            Value = string.Empty;
            ValueDisplayCode = string.Empty;
            ID = string.Empty;
            IsInstanceItem = false;
        }

        public ListItem(string text)
        {
            Text = text.Trim();
            Value = text.Trim();
            ID = text.Trim();
            IsInstanceItem = false;
        }

        public ListItem(string text, object value)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = string.Empty;            
            IsInstanceItem = false;
        }

        public ListItem(string text, object value, string id,Color favoriteColor)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            FavoriteColor = favoriteColor;
            IsInstanceItem = false;
        }

        public ListItem(string text, object value, string id, Color favoriteColor,string defaultvalue)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            FavoriteColor = favoriteColor;
            IsInstanceItem = false;
            DefaultValue = defaultvalue.Trim();
        }

        public ListItem(string text, object value, string id, Color favoriteColor, string defaultvalue,string valueDisplaycode)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            FavoriteColor = favoriteColor;
            IsInstanceItem = false;
            DefaultValue = defaultvalue.Trim();
            ValueDisplayCode = valueDisplaycode.Trim();
        }

        public ListItem(string text, object value, string id, string code)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            ValueDisplayCode = code.Trim();          
        }

        public ListItem(string text, object value, string id, string code,string screenCode,string screenType)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            ValueDisplayCode = code.Trim();
            ScreenCode = screenCode.Trim();
            ScreenType = screenType.Trim();
        }
        public ListItem(string text, object value, string id, string code, string screenCode, string screenType, Color favoriteColor)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            ValueDisplayCode = code.Trim();
            ScreenCode = screenCode.Trim();
            ScreenType = screenType.Trim();
            FavoriteColor = favoriteColor;
        }

        public ListItem(string text, object value, string id, bool isInstanceItem)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            IsInstanceItem = isInstanceItem;
        }

        public ListItem(string text, object value, string id, string code, string screenCode, string screenType, string stramount,string strdetails)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            ValueDisplayCode = code.Trim();
            ScreenCode = screenCode.Trim();
            ScreenType = screenType.Trim();
            
            Amount = stramount;
            Details = strdetails;
        }
        public ListItem(string text, object value, string id, string code, string screenCode, string screenType, string stramount, string strdetails, string strCategory)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            ValueDisplayCode = code.Trim();
            ScreenCode = screenCode.Trim();
            ScreenType = screenType.Trim();
            Amount = stramount;
            Details = strdetails;
            Category = strCategory;
        }

        public ListItem(string text, object value, string id, string code, string screenCode, string screenType, string stramount, string strdetails, string strCategory, string strHAmount, string strAdjAmount, string strCumAMount)
        {
            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            ValueDisplayCode = code.Trim();
            ScreenCode = screenCode.Trim();
            ScreenType = screenType.Trim();
            Amount = stramount;
            Details = strdetails;
            Category = strCategory;
            HAmount = strHAmount;
            AdjAmount = strAdjAmount;
            CumAMount = strCumAMount;
        }
        public ListItem(string text, object value, string id, string strp1monthly, string strp1Adjust, string strp1Cumulative, string strp2monthly, string strp2Adjust, string strp2Cumulative, string strp3monthly,
            string strp3Adjust, string strp3Cumulative, string strP1Amount, string strP1AdjAmount, string strP1CumAMount, string strP2Amount, string strP2AdjAmount, string strP2CumAMount, string strP3Amount,
            string strP3AdjAmount, string strP3CumAMount)  // Brought update - Vikash 01032023 for Funding Source Report
        {

            Text = text.Trim();
            Value = value.ToString().Trim();
            ID = id.Trim();
            P1Monthly = strp1monthly;
            P1Adjust = strp1Adjust;
            P1Cummulative = strp1Cumulative;

            P2Monthly = strp2monthly;
            P2Adjust = strp2Adjust;
            P2Cummulative = strp2Cumulative;

            P3Monthly = strp3monthly;
            P3Adjust = strp3Adjust;
            P3Cummulative = strp3Cumulative;

            P1Amount = strP1Amount;
            P1AdjAmount = strP1AdjAmount;
            P1CumAMount = strP1CumAMount;
            P2Amount = strP2Amount;
            P2AdjAmount = strP2AdjAmount;
            P2CumAMount = strP2CumAMount;
            P3Amount = strP3Amount;
            P3AdjAmount = strP3AdjAmount;
            P3CumAMount = strP3CumAMount;
        }

        public string ID { get; set; }

        public string Text { get; set; }

        [Browsable(false)]
        public object Value { get; set; }

        public string ValueDisplayCode { get; set; }

        public string ScreenType { get; set; }

        public string ScreenCode{ get; set; }

        public bool IsInstanceItem { get; set; }

        public Color FavoriteColor { get; set; }

        public string DefaultValue { get; set; }

        public string ToolTip { get; set; }

        public string Details { get; set; }

        public string Amount { get; set; }

        public string Category { get; set; } //Added by Vikash 01032023 for Funding Source Report
        public override string ToString()
        {
            return Text;
        }

        public string GetValues()
        {
            return Text + Consts.Common.Tilda + ID + Consts.Common.Tilda + Value.ToString();
        }

        public int CompareTo(ListItem other)
        {
            return Text.CompareTo(other.Text);
        }
        public string HAmount { get; set; }
        public string AdjAmount { get; set; }
        public string CumAMount { get; set; }

        public string P1Monthly { get; set; }
        public string P1Adjust { get; set; }
        public string P1Cummulative { get; set; }

        public string P2Monthly { get; set; }
        public string P2Adjust { get; set; }
        public string P2Cummulative { get; set; }

        public string P3Monthly { get; set; }
        public string P3Adjust { get; set; }
        public string P3Cummulative { get; set; }

        public string P1Amount { get; set; }
        public string P1AdjAmount { get; set; }
        public string P1CumAMount { get; set; }
        public string P2Amount { get; set; }
        public string P2AdjAmount { get; set; }
        public string P2CumAMount { get; set; }
        public string P3Amount { get; set; }
        public string P3AdjAmount { get; set; }
        public string P3CumAMount { get; set; }
    }
}
