using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class AgyTabEntity
    {
        #region Constructors

        public AgyTabEntity()
        {

            agytype = string.Empty;
            agycode = string.Empty;
            agy1 = string.Empty;
            agy2 = string.Empty;
            agy3 = string.Empty;
            agy4 = string.Empty;
            agy5 = string.Empty;
            agy6 = string.Empty;
            agy7 = string.Empty;
            agy8 = string.Empty;
            agy9 = string.Empty;
            agya1 = string.Empty;
            agya2 = string.Empty;
            agya3 = string.Empty;
            agya4 = string.Empty;
            agydesc = string.Empty;
            agyactive = string.Empty;
            agydefault = string.Empty;
            agyhierarchy = string.Empty;
            agylstcdate = string.Empty;
            agylstcoperator = string.Empty;
            agyadddate = string.Empty;
            agyaddoperator = string.Empty;
            agyRowtype = string.Empty;
        }

        public AgyTabEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                agytype = row["AGY_TYPE"].ToString().Trim();
                agycode = row["AGY_CODE"].ToString().Trim();
                agy1 = row["AGY_1"].ToString().Trim();
                agy2 = row["AGY_2"].ToString().Trim();
                agy3 = row["AGY_3"].ToString().Trim();
                agy4 = row["AGY_4"].ToString().Trim();
                agy5 = row["AGY_5"].ToString().Trim();
                agy6 = row["AGY_6"].ToString().Trim();
                agy7 = row["AGY_7"].ToString().Trim();
                agy8 = row["AGY_8"].ToString().Trim();
                agy9 = row["AGY_9"].ToString().Trim();
                agya1 = row["AGY_A1"].ToString().Trim();
                agya2 = row["AGY_A2"].ToString().Trim();
                agya3 = row["AGY_A3"].ToString().Trim();
                agya4 = row["AGY_A4"].ToString().Trim();
                agydesc = row["AGY_DESC"].ToString().Trim();
                agyactive = row["AGY_ACTIVE"].ToString().Trim();
                agydefault = row["AGY_DEFAULT"].ToString().Trim();
                agyhierarchy = row["AGY_HIERARCHY"].ToString().Trim();
                agylstcdate = row["AGY_DATE_LSTC"].ToString().Trim();
                agylstcoperator = row["AGY_LSTC_OPERATOR"].ToString().Trim();
                agyadddate = row["AGY_DATE_ADD"].ToString().Trim();
                agyaddoperator = row["AGY_ADD_OPERATOR"].ToString().Trim();
            }

        }

        public AgyTabEntity(DataRow AgyTabControl, string type)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                if (type == "00004")
                {
                    agycode = row["Code"].ToString().Trim();
                    agydesc = row["LookUpDesc"].ToString().Trim();
                    agydefault = row["AGY_DEFAULT"].ToString().Trim();
                    agy1 = row["EXT"].ToString().Trim();
                    agyactive = row["Active"].ToString().Trim();
                    agyhierarchy = row["Hierarchy"].ToString().Trim();
                }
                else
                {
                    
                    agycode = row["Code"].ToString().Trim();
                    agydesc = row["LookUpDesc"].ToString().Trim();
                    agydefault = row["AGY_DEFAULT"].ToString().Trim();
                }
            }

        }

        #endregion

        #region Properties

        public string agytype { get; set; }
        public string agycode { get; set; }
        public string agy1 { get; set; }
        public string agy2 { get; set; }
        public string agy3 { get; set; }
        public string agy4 { get; set; }
        public string agy5 { get; set; }
        public string agy6 { get; set; }
        public string agy7 { get; set; }
        public string agy8 { get; set; }
        public string agy9 { get; set; }
        public string agya1 { get; set; }
        public string agya2 { get; set; }
        public string agya3 { get; set; }
        public string agya4 { get; set; }
        public string agydesc { get; set; }
        public string agyactive { get; set; }
        public string agydefault { get; set; }
        public string agyhierarchy { get; set; }
        public string agylstcdate { get; set; }
        public string agylstcoperator { get; set; }
        public string agyadddate { get; set; }
        public string agyaddoperator { get; set; }
        public string agyRowtype { get; set; }

        #endregion

    }


    public class AgyCommonFillEntity
    {
        #region Constructors

        public AgyCommonFillEntity()
        {
            Code = string.Empty;
            Desc = string.Empty;
            Active = string.Empty;
            Deflt = string.Empty;
            Ext = string.Empty;
            Ext_1 = string.Empty;
            Hier = string.Empty;
            Can_Add = false;
        }

        public AgyCommonFillEntity(DataRow AgyTabControl, string SelHierarchy)
        {
            int HieCount = 0;
            int Index = 0;
            string CompareHie; 

            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                Code = row["Code"].ToString().Trim();
                Desc = row["Code_Desc"].ToString().Trim();
                Active = row["Active"].ToString().Trim();
                Deflt = row["Deflt"].ToString().Trim();
                Ext = row["Ext"].ToString().Trim();
                Ext_1 = row["Ext_1"].ToString().Trim();
                Hier = row["Hierarchy"].ToString().Trim();
                Can_Add = false;

				HieCount = (Hier.Length)/6;

                if (Hier.Substring(0, 6) != "******" && HieCount != 1)
                {
                    for (int i = 0; i < HieCount; i++)
                    {
                        CompareHie = null;
                        CompareHie = Hier.Substring(Index, 6);
                        if (CompareHie == SelHierarchy)
                        {
                            Can_Add = true; break;
                        }
                        Index += 6;
                    }
                }
                else
                    Can_Add = true;
            }

        }

        public AgyCommonFillEntity(string Appno, string ServPlan, string actdate,string Estatus,string Efund)
        {

            Code = Appno;
            Desc = actdate;
            Estatus = Active;
            Efund = Hier;
            Deflt = ServPlan;
    
        }


        #endregion

        #region Properties

        public string Code { get; set; }
        public string Desc { get; set; }
        public string Active { get; set; }
        public string Deflt { get; set; }
        public string Ext { get; set; }
        public string Ext_1 { get; set; }
        public string Hier { get; set; }
        public bool Can_Add { get; set; } 

        #endregion
    }

    public class CTXMLTAGSEntity
    { 

          #region Constructors

        public CTXMLTAGSEntity()
        {
            CTXMLTAG_CATG = string.Empty;
            CTXMLTAG_SCATG = string.Empty;
            CTXMLTAG_DESC = string.Empty;
            CTXMLTAG_AGYTYPE = string.Empty;
         
        }

        public CTXMLTAGSEntity(DataRow CtxmlTagsControl)
        {          

            if (CtxmlTagsControl != null)
            {
                DataRow row = CtxmlTagsControl;
                CTXMLTAG_CATG = row["CTXMLTAG_CATG"].ToString().Trim();
                CTXMLTAG_SCATG = row["CTXMLTAG_SCATG"].ToString().Trim();
                CTXMLTAG_DESC = row["CTXMLTAG_DESC"].ToString().Trim();
                CTXMLTAG_AGYTYPE = row["CTXMLTAG_AGYTYPE"].ToString().Trim();
             
            }

        }

     


        #endregion

        #region Properties

        public string CTXMLTAG_CATG { get; set; }
        public string CTXMLTAG_SCATG { get; set; }
        public string CTXMLTAG_DESC { get; set; }
        public string CTXMLTAG_AGYTYPE { get; set; }

        #endregion
    
    }

    public class CTXMLASOCEntity
    { 
    
          #region Constructors

        public CTXMLASOCEntity()
        {
            CTXMLASOC_CATG = string.Empty;
            CTXMLASOC_SCATG = string.Empty;
            CTXMLASOC_DESC = string.Empty;
            CTXMLASOC_DEFAULT = string.Empty;
            CTXMLASOC_CODES = string.Empty;
            CTXMLASOC_LSTC_OPERATOR = string.Empty;
            CTXMLASOC_DATE_LSTC = string.Empty;
         
        }

        public CTXMLASOCEntity(DataRow CtxmlTagsControl)
        {          

            if (CtxmlTagsControl != null)
            {
                DataRow row = CtxmlTagsControl;
                CTXMLASOC_CATG = row["CTXMLASOC_CATG"].ToString().Trim();
                CTXMLASOC_SCATG = row["CTXMLASOC_SCATG"].ToString().Trim();
                CTXMLASOC_DESC = row["CTXMLASOC_DESC"].ToString().Trim();
                CTXMLASOC_DEFAULT = row["CTXMLASOC_DEFAULT"].ToString().Trim();
                CTXMLASOC_CODES = row["CTXMLASOC_CODES"].ToString();
                CTXMLASOC_LSTC_OPERATOR = row["CTXMLASOC_LSTC_OPERATOR"].ToString().Trim();
                CTXMLASOC_DATE_LSTC = row["CTXMLASOC_DATE_LSTC"].ToString().Trim();

             
            }

        }

     


        #endregion

        #region Properties

        public string CTXMLASOC_CATG { get; set; }
        public string CTXMLASOC_SCATG { get; set; }
        public string CTXMLASOC_DESC { get; set; }
        public string CTXMLASOC_DEFAULT { get; set; }
        public string CTXMLASOC_CODES { get; set; }
        public string CTXMLASOC_LSTC_OPERATOR { get; set; }
        public string CTXMLASOC_DATE_LSTC { get; set; }


        #endregion

    }
}
