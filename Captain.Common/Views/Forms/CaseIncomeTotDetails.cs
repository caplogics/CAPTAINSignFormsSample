#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseIncomeTotDetails : Form
    {
        private CaptainModel _model = null;
        public CaseIncomeTotDetails(BaseForm baseForm, string strIncometype, string strInterval, string strIncometypedesc, string strIntervaldesc, string strFamilSeq, string strFactor, ProgramDefinitionEntity programDefinitiondata)
        {
            InitializeComponent();
            _model = new CaptainModel();
            programDefinitionList = programDefinitiondata;
            BaseForm = baseForm;
            lblIntervalType.Text = strIncometypedesc;
            GetCaseIncomeDetails(strFamilSeq, strIncometype, strInterval, strIncometypedesc, strIntervaldesc,strFactor);
        }
        public BaseForm BaseForm { get; set; }
        public ProgramDefinitionEntity programDefinitionList { get; set; }
        void GetCaseIncomeDetails(string strFamilySeq, string strIncometype, string strInterval, string strIncometypedesc, string strIntervaldesc,string strFactor)
        {
            AgencyControlEntity propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");

            List<CommonEntity> commonInterval = _model.lookupDataAccess.GetIncomeInterval(string.Empty, propAgencyControlDetails.State);
            commonInterval = commonInterval.FindAll(u => u.Code == strInterval);
            string strDesc = string.Empty;
            if (commonInterval.Count>0)
                strDesc = commonInterval[0].Desc.ToString();
            List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, strFamilySeq);
            caseIncomeList = caseIncomeList.FindAll(u => u.Interval == strInterval && u.Type == strIncometype);
            decimal decTotalAmount = 0;
            foreach (CaseIncomeEntity item in caseIncomeList)
            {
                if (item.Val1 != string.Empty)
                {
                    //if (Convert.ToDecimal(item.Val1) > 0)
                    //{
                        decTotalAmount = Convert.ToDecimal(item.Val1) + decTotalAmount;
                        gvwIncomeDetails.Rows.Add(strDesc,LookupDataAccess.Getdate(item.Date1), item.Val1);
                    //}
                    if (item.Exclude == "Y")
                        gvtAmount.DefaultCellStyle.ForeColor = Color.Red;
                }
                if (item.Val2 != string.Empty)
                {
                    //if (Convert.ToDecimal(item.Val2) > 0)
                    //{
                        decTotalAmount = Convert.ToDecimal(item.Val2) + decTotalAmount;
                        gvwIncomeDetails.Rows.Add("", LookupDataAccess.Getdate(item.Date2), item.Val2);
                   // }
                    if (item.Exclude == "Y")
                        gvtAmount.DefaultCellStyle.ForeColor = Color.Red;
                }
                if (item.Val3 != string.Empty)
                {
                    //if (Convert.ToDecimal(item.Val3) > 0)
                    //{
                        decTotalAmount = Convert.ToDecimal(item.Val3) + decTotalAmount;
                        gvwIncomeDetails.Rows.Add("", LookupDataAccess.Getdate(item.Date3), item.Val3);
                   // }
                    if (item.Exclude == "Y")
                        gvtAmount.DefaultCellStyle.ForeColor = Color.Red;
                }
                if (item.Val4 != string.Empty)
                {
                    //if (Convert.ToDecimal(item.Val4) > 0)
                    //{
                        decTotalAmount = Convert.ToDecimal(item.Val4) + decTotalAmount;
                        gvwIncomeDetails.Rows.Add("", LookupDataAccess.Getdate(item.Date4), item.Val4);
                    //}
                    if (item.Exclude == "Y")
                        gvtAmount.DefaultCellStyle.ForeColor = Color.Red;
                }
                if (item.Val5 != string.Empty)
                {
                    //if (Convert.ToDecimal(item.Val5) > 0)
                    //{
                        decTotalAmount = Convert.ToDecimal(item.Val5) + decTotalAmount;
                        gvwIncomeDetails.Rows.Add("", LookupDataAccess.Getdate(item.Date5), item.Val5);
                   // }
                    if (item.Exclude == "Y")
                        gvtAmount.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            if (programDefinitionList.IncomeWeek == "Y")
            {
                if (strInterval == "W" || strInterval == "E")
                {                   
                    if (decTotalAmount > 0)
                    {
                        int introwcout = gvwIncomeDetails.Rows.Count;
                        if (strInterval == "E")
                            introwcout = 4;
                        decimal decavg = decTotalAmount / introwcout;
                        double dec30days = Convert.ToDouble(decavg) * 4.33;
                        double decfactor = 1;
                        if (strFactor != string.Empty)
                            decfactor = Convert.ToDouble(strFactor);
                        double decanuual = dec30days * decfactor;
                        if (strInterval == "E")
                        {
                            lbl1.Text = decTotalAmount.ToString("0.00##") + "/" + introwcout + " = " + decavg.ToString("0.00##") + " (average None income)";
                        }
                        else
                        {
                            lbl1.Text = decTotalAmount.ToString("0.00##") + "/" + introwcout + " = " + decavg.ToString("0.00##") + " (average weekly income)";
                        }
                        
                        lbl2.Text = decavg.ToString("0.00##") + " * 4.33 = " + dec30days.ToString("0.00##") + " (30 days income)";
                        lbl3.Text = dec30days.ToString("0.00##") + " * " + decfactor.ToString() + " = " + decanuual.ToString("0.00##") + " (annualized)";
                    }
                }
                else if (strInterval == "B")
                {                    
                    if (decTotalAmount > 0)
                    {
                        decimal decavg = decTotalAmount / gvwIncomeDetails.Rows.Count;
                        double dec30days = Convert.ToDouble(decavg) * 2.16;
                        double decfactor = 1;
                        if (strFactor != string.Empty)
                            decfactor = Convert.ToDouble(strFactor);
                        double decanuual = dec30days * decfactor;
                        lbl1.Text = decTotalAmount.ToString("0.00##") + "/" + gvwIncomeDetails.Rows.Count + " = " + decavg.ToString("0.00##") + " (average BiWeekly income)";
                        lbl2.Text = decavg.ToString("0.00##") + " * 2.16 = " + dec30days.ToString("0.00##") + " (30 days income)";
                        lbl3.Text = dec30days.ToString("0.00##") + " * " + decfactor.ToString() + " = " + decanuual.ToString("0.00##") + " (annualized)";
                    }
                }
                else if (strInterval == "S")
                {
                    if (decTotalAmount > 0)
                    {
                        decimal decavg = decTotalAmount / gvwIncomeDetails.Rows.Count;
                        double dec30days = Convert.ToDouble(decavg) * 2;
                        double decfactor = 1;
                        if (strFactor != string.Empty)
                            decfactor = Convert.ToDouble(strFactor);
                        double decanuual = dec30days * decfactor;
                        lbl1.Text = decTotalAmount.ToString("0.00##") + "/" + gvwIncomeDetails.Rows.Count + " = " + decavg.ToString("0.00##") + " (average Semi Monthly income)";
                        lbl2.Text = decavg.ToString("0.00##") + " * 2 = " + dec30days.ToString("0.00##") + " (30 days income)";
                        lbl3.Text = dec30days.ToString("0.00##") + " * " + decfactor.ToString() + " = " + decanuual.ToString("0.00##") + " (annualized)";
                    }
                }
                else if (strInterval == "M")
                {
                    if (decTotalAmount > 0)
                    {
                        decimal decavg = decTotalAmount / gvwIncomeDetails.Rows.Count;
                        double dec30days = Convert.ToDouble(decavg);
                        double decfactor = 1;
                        if (strFactor != string.Empty)
                            decfactor = Convert.ToDouble(strFactor);
                        double decanuual = dec30days * decfactor;
                        lbl1.Text = decTotalAmount.ToString("0.00##") + "/" + gvwIncomeDetails.Rows.Count + " = " + decavg.ToString("0.00##") + " (average Monthly income)";
                        lbl2.Text = "";//decavg.ToString("0.00##") + " * 2 = " + dec30days.ToString("0.00##") + " (30 days income)";
                        lbl3.Text = dec30days.ToString("0.00##") + " * " + decfactor.ToString() + " = " + decanuual.ToString("0.00##") + " (annualized)";
                    }
                }
                else
                {
                   // decTotalAmount = Math.Round(decTotalAmount, 2);
                    double decfactor = 1;
                    if (strFactor != string.Empty)
                        decfactor = Convert.ToDouble(strFactor);
                    double decanuual = Convert.ToDouble(decTotalAmount) * decfactor;
                    lbl1.Text = "";
                    lbl2.Text = "";
                    //lbl3.Text = decTotalAmount.ToString("0.00##") + " * " + decfactor.ToString() + " = " + decanuual.ToString("0.00##") + " (annualized)";
                    lbl3.Text = LookupDataAccess.decimal2value(decTotalAmount.ToString()) + " * " + decfactor.ToString() + " = " + LookupDataAccess.decimal2value(decanuual.ToString()) + " (annualized)";               
                }
            }
            else
            {
               // decTotalAmount = Math.Round(decTotalAmount, 2);
                double decfactor = 1;
                if (strFactor != string.Empty)
                    decfactor = Convert.ToDouble(strFactor);
                double decanuual = Convert.ToDouble(decTotalAmount) * decfactor;
                lbl1.Text = "";
                lbl2.Text = "";
                lbl3.Text = LookupDataAccess.decimal2value(decTotalAmount.ToString()) + " * " + decfactor.ToString() + " = " + LookupDataAccess.decimal2value(decanuual.ToString()) + " (annualized)";               
            }
            txtTotal.Text = LookupDataAccess.decimal2value(decTotalAmount.ToString()); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}