using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class CaseServicesEntity
    {
        #region Constructors

        public CaseServicesEntity()
        {
            Service = string.Empty;
            Desc = string.Empty;
            Fund = string.Empty;
            Seq = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;

            Application = string.Empty;
            Zero = string.Empty;
            Vendor = string.Empty;
            Authorizations = string.Empty;
            Mi = string.Empty;
            Outofpoverty = string.Empty;
            Active = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            DataAdd = string.Empty;
            AddOperator = string.Empty;
        }

        public CaseServicesEntity(bool Intialize)
        {
            if (Intialize)
            {
                Service = null;
                Desc = null;
                Fund = null;
                Seq = null;
                Agency = null;
                Dept = null;
                Program = null;

                Application = null;
                Zero = null;
                Vendor = null;
                Authorizations = null;
                Mi = null;
                Outofpoverty = null;
                Active = null;
                DateLstc = null;
                LstcOperator = null;
                DataAdd = null;
                AddOperator = null;
            }
        }
        public CaseServicesEntity(DataRow row)
        {
            if (row != null)
            {
                Service = row["CAC_SERVICE"].ToString();
                Desc = row["CAC_DESC"].ToString();
                Fund = row["CAC_FUND"].ToString();
                Seq = row["CAC_SEQ"].ToString();
                Agency = row["CAC_AGENCY"].ToString();
                Dept = row["CAC_DEPT"].ToString();
                Program = row["CAC_PROGRAM"].ToString();

                Application = row["CAC_APPLICATION"].ToString();
                Zero = row["CAC_ZERO"].ToString();
                Vendor = row["CAC_VENDOR"].ToString();
                Authorizations = row["CAC_AUTHORIZATIONS"].ToString();
                Mi = row["CAC_MI"].ToString();
                Outofpoverty = row["CAC_OUTOFPOVERTY"].ToString();
                Active = row["CAC_ACTIVE"].ToString();
                DateLstc = row["CAC_DATE_LSTC"].ToString();
                LstcOperator = row["CAC_LSTC_OPERATOR"].ToString();
                DataAdd = row["CAC_DATE_ADD"].ToString();
                AddOperator = row["CAC_ADD_OPERATOR"].ToString();
            }
        }

        #endregion


        #region Properties


        public string Service { get; set; }
        public string Desc { get; set; }
        public string Fund { get; set; }
        public string Seq { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }

        public string Application { get; set; }
        public string Zero { get; set; }
        public string Vendor { get; set; }
        public string Authorizations { get; set; }
        public string Mi { get; set; }
        public string Outofpoverty { get; set; }
        public string Active { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string DataAdd { get; set; }
        public string AddOperator { get; set; }

        #endregion

    }

}
