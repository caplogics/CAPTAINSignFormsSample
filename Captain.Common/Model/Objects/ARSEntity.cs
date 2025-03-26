using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class ARSCUSTEntity
    {
        #region Constructors

        public ARSCUSTEntity()
        {
            ARS_AGENCY = string.Empty;
            ARS_DEPT = string.Empty;
            ARS_PROGRAM = string.Empty;
            ARS_APP_NO = string.Empty;
            ARS_CUST_TYPE = string.Empty;
            ARS_NAME = string.Empty;
            ARS_INDEXBY = string.Empty;
            ARS_NAME_2 = string.Empty;
            ARS_INDEXBY2 = string.Empty;
            ARS_STREET = string.Empty;
            ARS_CITY = string.Empty;
            ARS_STATE = string.Empty;
            ARS_ZIP = string.Empty;
            ARS_ZIP_PLUS4 = string.Empty;
            ARS_TELEPHONE = string.Empty;
            ARS_ACTIVE = string.Empty;
            ARS_C_NAME = string.Empty;
            ARS_C_TELEPHONE = string.Empty;                              
            ARS_FREQ = string.Empty;
            ARS_TERMS = string.Empty;            
            ARS_BILLTO = string.Empty;
            ARS_CONTRACT = string.Empty;
            ARS_LOCATION = string.Empty;
            ARS_PROGRAM_ACCOUNT = string.Empty;
            ARS_EXPENSE_CODE = string.Empty;
            ARS_CLASS_IDENTIFIER = string.Empty;          
            ARS_PRINT_INV = string.Empty;
            ARS_PRINT_STMT = string.Empty;
            ARS_DATE_ADD = string.Empty;
            ARS_ADD_OPERATOR = string.Empty;
            ARS_DATE_LSTC = string.Empty;
            ARS_LSTC_OPERATOR = string.Empty;

        }

        public ARSCUSTEntity(bool Initalize)
        {
            if (Initalize)
            {
                Rec_Type = "I";
                ARS_AGENCY = null;
                ARS_DEPT = null;
                ARS_PROGRAM = null;
                ARS_APP_NO = null;
                ARS_CUST_TYPE = null;
                ARS_NAME = null;
                ARS_INDEXBY = null;
                ARS_NAME_2 = null;
                ARS_INDEXBY2 = null;
                ARS_STREET = null;
                ARS_CITY = null;
                ARS_STATE = null;
                ARS_ZIP = null;
                ARS_ZIP_PLUS4 = null;
                ARS_TELEPHONE = null;
                ARS_ACTIVE = null;
                ARS_C_NAME = null;
                ARS_C_TELEPHONE = null;              
                ARS_FREQ = null;
                ARS_TERMS = null;                
                ARS_BILLTO = null;
                ARS_CONTRACT = null;
                ARS_LOCATION = null;
                ARS_PROGRAM_ACCOUNT = null;
                ARS_EXPENSE_CODE = null;
                ARS_CLASS_IDENTIFIER = null;                
                ARS_PRINT_INV = null;
                ARS_PRINT_STMT = null;
                ARS_DATE_ADD = null;
                ARS_ADD_OPERATOR = null;
                ARS_DATE_LSTC = null;
                ARS_LSTC_OPERATOR = null;

            }
        }

        public ARSCUSTEntity(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";
                ARS_AGENCY = Row["ARS_AGENCY"].ToString().Trim();
                ARS_DEPT = Row["ARS_DEPT"].ToString().Trim();
                ARS_PROGRAM = Row["ARS_PROGRAM"].ToString().Trim();
                ARS_APP_NO = Row["ARS_APP_NO"].ToString().Trim();
                ARS_CUST_TYPE = Row["ARS_CUST_TYPE"].ToString().Trim();
                ARS_NAME = Row["ARS_NAME"].ToString().Trim();
                ARS_INDEXBY = Row["ARS_INDEXBY"].ToString().Trim();
                ARS_NAME_2 = Row["ARS_NAME_2"].ToString().Trim();
                ARS_INDEXBY2 = Row["ARS_INDEXBY2"].ToString().Trim();
                ARS_STREET = Row["ARS_STREET"].ToString().Trim();
                ARS_CITY = Row["ARS_CITY"].ToString().Trim();
                ARS_STATE = Row["ARS_STATE"].ToString().Trim();
                ARS_ZIP = Row["ARS_ZIP"].ToString().Trim();
                ARS_ZIP_PLUS4 = Row["ARS_ZIP_PLUS4"].ToString().Trim();
                ARS_TELEPHONE = Row["ARS_TELEPHONE"].ToString().Trim();
                ARS_ACTIVE = Row["ARS_ACTIVE"].ToString().Trim();
                ARS_C_NAME = Row["ARS_C_NAME"].ToString().Trim();
                ARS_C_TELEPHONE = Row["ARS_C_TELEPHONE"].ToString().Trim();
                ARS_TERMS = Row["ARS_TERMS"].ToString().Trim();
                ARS_BILLTO = Row["ARS_BILLTO"].ToString().Trim();
                ARS_CONTRACT = Row["ARS_CONTRACT"].ToString().Trim();
                ARS_LOCATION = Row["ARS_LOCATION"].ToString().Trim();
                ARS_PROGRAM_ACCOUNT = Row["ARS_PROGRAM_ACCOUNT"].ToString().Trim();
                ARS_EXPENSE_CODE = Row["ARS_EXPENSE_CODE"].ToString().Trim();
                ARS_CLASS_IDENTIFIER = Row["ARS_CLASS_IDENTIFIER"].ToString().Trim();
                ARS_PRINT_INV = Row["ARS_PRINT_INV"].ToString().Trim();
                ARS_PRINT_STMT = Row["ARS_PRINT_STMT"].ToString().Trim();
                ARS_DATE_ADD = Row["ARS_DATE_ADD"].ToString().Trim();
                ARS_ADD_OPERATOR = Row["ARS_ADD_OPERATOR"].ToString().Trim();
                ARS_DATE_LSTC = Row["ARS_DATE_LSTC"].ToString().Trim();
                ARS_LSTC_OPERATOR = Row["ARS_LSTC_OPERATOR"].ToString().Trim();
            }
        }


        public ARSCUSTEntity(DataRow Row,string strTable)
        {
            if (Row != null)
            {
                Rec_Type = "U";
                ARS_AGENCY = Row["ARS_AGENCY"].ToString().Trim();
                ARS_DEPT = Row["ARS_DEPT"].ToString().Trim();
                ARS_PROGRAM = Row["ARS_PROGRAM"].ToString().Trim();
                ARS_APP_NO = Row["ARS_APP_NO"].ToString().Trim();
                ARS_CUST_TYPE = Row["ARS_CUST_TYPE"].ToString().Trim();              
                ARS_INDEXBY = Row["ARS_INDEXBY"].ToString().Trim();
                ARS_NAME = Row["ARS_NAME"].ToString().Trim();
                ARS_INDEXBY2 = Row["ARS_INDEXBY2"].ToString().Trim();
                //ARS_STREET = Row["ARS_STREET"].ToString().Trim();
                //ARS_CITY = Row["ARS_CITY"].ToString().Trim();
                //ARS_STATE = Row["ARS_STATE"].ToString().Trim();
                //ARS_ZIP = Row["ARS_ZIP"].ToString().Trim();
                //ARS_ZIP_PLUS4 = Row["ARS_ZIP_PLUS4"].ToString().Trim();
                //ARS_TELEPHONE = Row["ARS_TELEPHONE"].ToString().Trim();
                ARS_ACTIVE = Row["ARS_ACTIVE"].ToString().Trim();
                //ARS_C_NAME = Row["ARS_C_NAME"].ToString().Trim();
                //ARS_C_TELEPHONE = Row["ARS_C_TELEPHONE"].ToString().Trim();
                ARS_TERMS = Row["ARS_TERMS"].ToString().Trim();
                ARS_BILLTO = Row["ARS_BILLTO"].ToString().Trim();
                ARS_CONTRACT = Row["ARS_CONTRACT"].ToString().Trim();
                ARS_LOCATION = Row["ARS_LOCATION"].ToString().Trim();
                ARS_PROGRAM_ACCOUNT = Row["ARS_PROGRAM_ACCOUNT"].ToString().Trim();
                ARS_EXPENSE_CODE = Row["ARS_EXPENSE_CODE"].ToString().Trim();
                ARS_CLASS_IDENTIFIER = Row["ARS_CLASS_IDENTIFIER"].ToString().Trim();
                ARS_PRINT_INV = Row["ARS_PRINT_INV"].ToString().Trim();
                ARS_PRINT_STMT = Row["ARS_PRINT_STMT"].ToString().Trim();
                ARS_DATE_ADD = Row["ARS_DATE_ADD"].ToString().Trim();
                ARS_ADD_OPERATOR = Row["ARS_ADD_OPERATOR"].ToString().Trim();
                ARS_DATE_LSTC = Row["ARS_DATE_LSTC"].ToString().Trim();
                ARS_LSTC_OPERATOR = Row["ARS_LSTC_OPERATOR"].ToString().Trim();

                ARS_STREET = string.Empty;
                ARS_CITY = string.Empty;
                ARS_STATE = string.Empty;
                ARS_ZIP = string.Empty;
                ARS_ZIP_PLUS4 = string.Empty;
                ARS_TELEPHONE = string.Empty;
                ARS_C_NAME = string.Empty;
                ARS_C_TELEPHONE = string.Empty;

            }
        }

        public ARSCUSTEntity(DataRow Row, string Strtable,string Rep)
        {
            if (Row != null)
            {
                ARS_AGENCY = Row["ARS_AGENCY"].ToString().Trim();
                ARS_DEPT = Row["ARS_DEPT"].ToString().Trim();
                ARS_PROGRAM = Row["ARS_PROGRAM"].ToString().Trim();
                MST_YEAR = Row["MST_YEAR"].ToString().Trim();
                ARS_APP_NO = Row["ARS_APP_NO"].ToString().Trim();
                ARS_CUST_TYPE = Row["ARS_CUST_TYPE"].ToString().Trim();
                ARS_NAME = Row["ARS_NAME"].ToString().Trim();
                ARS_INDEXBY = Row["ARS_INDEXBY"].ToString().Trim();
                ARS_NAME_2 = Row["ARS_NAME_2"].ToString().Trim();
                ARS_INDEXBY2 = Row["ARS_INDEXBY2"].ToString().Trim();
                ARS_STREET = Row["ARS_STREET"].ToString().Trim();
                ARS_CITY = Row["ARS_CITY"].ToString().Trim();
                ARS_STATE = Row["ARS_STATE"].ToString().Trim();
                ARS_ZIP = Row["ARS_ZIP"].ToString().Trim();
                ARS_ZIP_PLUS4 = Row["ARS_ZIP_PLUS4"].ToString().Trim();
                ARS_TELEPHONE = Row["ARS_TELEPHONE"].ToString().Trim();
                ARS_ACTIVE = Row["ARS_ACTIVE"].ToString().Trim();
                ARS_TERMS = Row["ARS_TERMS"].ToString().Trim();
                ARS_BILLTO = Row["ARS_BILLTO"].ToString().Trim();
                ARS_CONTRACT = Row["ARS_CONTRACT"].ToString().Trim();
                ARS_LOCATION = Row["ARS_LOCATION"].ToString().Trim();
                ARS_PROGRAM_ACCOUNT = Row["ARS_PROGRAM_ACCOUNT"].ToString().Trim();
                ARS_EXPENSE_CODE = Row["ARS_EXPENSE_CODE"].ToString().Trim();
                ARS_CLASS_IDENTIFIER = Row["ARS_CLASS_IDENTIFIER"].ToString().Trim();
                ARS_PRINT_INV = Row["ARS_PRINT_INV"].ToString().Trim();
                ARS_PRINT_STMT = Row["ARS_PRINT_STMT"].ToString().Trim();

            }
        }


 

        #endregion

        #region Properties

        public string ARS_AGENCY { get; set; }
        public string ARS_DEPT { get; set; }
        public string ARS_PROGRAM { get; set; }       
        public string ARS_APP_NO { get; set; }
        public string ARS_CUST_TYPE { get; set; }
        public string ARS_NAME { get; set; }
        public string ARS_INDEXBY { get; set; }
        public string ARS_NAME_2 { get; set; }
        public string ARS_INDEXBY2 { get; set; }
        public string ARS_STREET { get; set; }
        public string ARS_CITY { get; set; }
        public string ARS_STATE { get; set; }
        public string ARS_ZIP { get; set; }
        public string ARS_ZIP_PLUS4 { get; set; }
        public string ARS_TELEPHONE { get; set; }
        public string ARS_ACTIVE { get; set; }
        public string ARS_C_NAME { get; set; }
        public string ARS_C_TELEPHONE { get; set; }       
        public string ARS_FREQ { get; set; }
        public string ARS_TERMS { get; set; }        
        public string ARS_BILLTO { get; set; }
        public string ARS_CONTRACT { get; set; }
        public string ARS_LOCATION { get; set; }
        public string ARS_PROGRAM_ACCOUNT { get; set; }
        public string ARS_EXPENSE_CODE { get; set; }
        public string ARS_CLASS_IDENTIFIER { get; set; }        
        public string ARS_PRINT_INV { get; set; }
        public string ARS_PRINT_STMT { get; set; }
        public string ARS_DATE_ADD { get; set; }
        public string ARS_ADD_OPERATOR { get; set; }
        public string ARS_DATE_LSTC { get; set; }
        public string ARS_LSTC_OPERATOR { get; set; }
        public string ARS_KEY { get; set; }
        public string Mode { get; set; }
        public string Ars_Cust_TypeDesc { get; set; }

        public string MST_YEAR { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }


    public class ARSMASTEntity
    {
        #region Constructors

        public ARSMASTEntity()
        {
            ARM_AGENCY = string.Empty;
            ARM_DEPT = string.Empty;
            ARM_PROGRAM = string.Empty;
            ARM_APP_NO = string.Empty;
            ARM_CUST_TYPE = string.Empty;
            ARM_SOURCE = string.Empty;
            ARM_INVOICE = string.Empty;
            ARM_STATUS = string.Empty;
            ARM_TYPE = string.Empty;
            ARM_INVOICE_DATE = string.Empty;
            ARM_DUE_DATE = string.Empty;
            ARM_COMMENTS = string.Empty;
            ARM_AUTO = string.Empty;
            ARM_BILLED = string.Empty;
            ARM_RECEIPT = string.Empty;
            ARM_ADJUSTMENTS = string.Empty;
            ARM_DISC_TAKEN = string.Empty;
            ARM_BALANCE = string.Empty;
            ARM_SITE = string.Empty;
            ARM_ROOM = string.Empty;
            ARM_AMPM = string.Empty;
            ARM_FUNDING_SOURCE = string.Empty;
            ARM_CLOSED_DATE = string.Empty;
            ARM_DATE_LSTC = string.Empty;
            ARM_LSTC_OPERATOR = string.Empty;
            ARM_DATE_ADD = string.Empty;
            ARM_ADD_OPERATOR = string.Empty;

            ARM_INVF_DATE = string.Empty;
            ARM_INVT_DATE = string.Empty;

        }

        public ARSMASTEntity(bool Initialize)
        {
            if (Initialize)
            {
                ARM_AGENCY = null;
                ARM_DEPT = null;
                ARM_PROGRAM = null;
                ARM_APP_NO = null;
                ARM_CUST_TYPE = null;
                ARM_SOURCE = null;
                ARM_INVOICE = null;
                ARM_STATUS = null;
                ARM_TYPE = null;
                ARM_INVOICE_DATE = null;
                ARM_DUE_DATE = null;
                ARM_COMMENTS = null;
                ARM_AUTO = null;
                ARM_BILLED = null;
                ARM_RECEIPT = null;
                ARM_ADJUSTMENTS = null;
                ARM_DISC_TAKEN = null;
                ARM_BALANCE = null;
                ARM_SITE = null;
                ARM_ROOM = null;
                ARM_AMPM = null;
                ARM_FUNDING_SOURCE = null;
                ARM_CLOSED_DATE = null;
                ARM_DATE_LSTC = null;
                ARM_LSTC_OPERATOR = null;
                ARM_DATE_ADD = null;
                ARM_ADD_OPERATOR = null;

                ARM_INVF_DATE = null;
                ARM_INVT_DATE = null;

            }
        }

        public ARSMASTEntity(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";

                ARM_AGENCY = Row["ARM_AGENCY"].ToString().Trim();
                ARM_DEPT = Row["ARM_DEPT"].ToString().Trim();
                ARM_PROGRAM = Row["ARM_PROGRAM"].ToString().Trim();
                ARM_APP_NO = Row["ARM_APP_NO"].ToString().Trim();
                ARM_CUST_TYPE = Row["ARM_CUST_TYPE"].ToString().Trim();
                ARM_SOURCE = Row["ARM_SOURCE"].ToString().Trim();
                ARM_INVOICE = Row["ARM_INVOICE"].ToString().Trim();
                ARM_STATUS = Row["ARM_STATUS"].ToString().Trim();
                ARM_TYPE = Row["ARM_TYPE"].ToString().Trim();
                ARM_INVOICE_DATE = Row["ARM_INVOICE_DATE"].ToString().Trim();
                ARM_DUE_DATE = Row["ARM_DUE_DATE"].ToString().Trim();
                ARM_COMMENTS = Row["ARM_COMMENTS"].ToString().Trim();
                ARM_AUTO = Row["ARM_AUTO"].ToString().Trim();
                ARM_BILLED = Row["ARM_BILLED"].ToString().Trim();
                ARM_RECEIPT = Row["ARM_RECEIPT"].ToString().Trim();
                ARM_ADJUSTMENTS = Row["ARM_ADJUSTMENTS"].ToString().Trim();
                ARM_DISC_TAKEN = Row["ARM_DISC_TAKEN"].ToString().Trim();
                ARM_BALANCE = Row["ARM_BALANCE"].ToString().Trim();
                ARM_SITE = Row["ARM_SITE"].ToString().Trim();
                ARM_ROOM = Row["ARM_ROOM"].ToString().Trim();
                ARM_AMPM = Row["ARM_AMPM"].ToString().Trim();
                ARM_FUNDING_SOURCE = Row["ARM_FUNDING_SOURCE"].ToString().Trim();
                ARM_CLOSED_DATE = Row["ARM_CLOSED_DATE"].ToString().Trim();
                ARM_DATE_LSTC = Row["ARM_DATE_LSTC"].ToString().Trim();
                ARM_LSTC_OPERATOR = Row["ARM_LSTC_OPERATOR"].ToString().Trim();
                ARM_DATE_ADD = Row["ARM_DATE_ADD"].ToString().Trim();
                ARM_ADD_OPERATOR = Row["ARM_ADD_OPERATOR"].ToString().Trim();

                ARM_INVF_DATE = Row["ARM_INVF_DATE"].ToString().Trim();
                ARM_INVT_DATE = Row["ARM_INVT_DATE"].ToString().Trim();
            }
        }

        public ARSMASTEntity(DataRow Row,string strTable,string RepName)
        {
            if (Row != null)
            {
                ARM_AGENCY = Row["ARM_AGENCY"].ToString().Trim();
                ARM_DEPT = Row["ARM_DEPT"].ToString().Trim();
                ARM_PROGRAM = Row["ARM_PROGRAM"].ToString().Trim();
                ARM_APP_NO = Row["ARM_APP_NO"].ToString().Trim();
                ARM_CUST_TYPE = Row["ARM_CUST_TYPE"].ToString().Trim();
                ARM_SOURCE = Row["ARM_SOURCE"].ToString().Trim();
                ARM_INVOICE = Row["ARM_INVOICE"].ToString().Trim();
                ARM_STATUS = Row["ARM_STATUS"].ToString().Trim();
                ARM_TYPE = Row["ARM_TYPE"].ToString().Trim();
                ARM_INVOICE_DATE = Row["ARM_INVOICE_DATE"].ToString().Trim();
                //ARM_AUTO = Row["ARM_AUTO"].ToString().Trim();
                ARM_BILLED = Row["ARM_BILLED"].ToString().Trim();
                //ARM_RECEIPT = Row["ARM_RECEIPT"].ToString().Trim();
                //ARM_ADJUSTMENTS = Row["ARM_ADJUSTMENTS"].ToString().Trim();
                //ARM_DISC_TAKEN = Row["ARM_DISC_TAKEN"].ToString().Trim();
                ARM_BALANCE = Row["ARM_BALANCE"].ToString().Trim();
                //ARM_SITE = Row["ARM_SITE"].ToString().Trim();
                //ARM_ROOM = Row["ARM_ROOM"].ToString().Trim();
                //ARM_AMPM = Row["ARM_AMPM"].ToString().Trim();
                
                //ARM_DATE_LSTC = Row["ARM_DATE_LSTC"].ToString().Trim();
                //ARM_LSTC_OPERATOR = Row["ARM_LSTC_OPERATOR"].ToString().Trim();
                //ARM_DATE_ADD = Row["ARM_DATE_ADD"].ToString().Trim();
                //ARM_ADD_OPERATOR = Row["ARM_ADD_OPERATOR"].ToString().Trim();
                ARM_DUE_DATE = Row["ARM_DUE_DATE"].ToString().Trim();
                
                ART_DESCRIPTION = Row["ART_DESCRIPTION"].ToString().Trim();

                if (RepName == "ARSB2160")
                {
                    
                    ARM_COMMENTS = Row["ARM_COMMENTS"].ToString().Trim();
                    ARM_FUNDING_SOURCE = Row["ARM_FUNDING_SOURCE"].ToString().Trim();
                    ARM_CLOSED_DATE = Row["ARM_CLOSED_DATE"].ToString().Trim();
                }
                else
                {
                    ART_CONTRACT = Row["ART_CONTRACT"].ToString().Trim();
                    ART_DATE = Row["ART_DATE"].ToString().Trim();
                    //ART_SEQ = Row["ART_SEQ"].ToString().Trim();
                    //ART_DISC_TAKEN = Row["ART_DISC_TAKEN"].ToString().Trim();
                }
                
                
            }
        }

        public ARSMASTEntity(DataRow Row, string strTable)
        {
            if (Row != null)
            {
                ARM_AGENCY = Row["ARM_AGENCY"].ToString().Trim();
                ARM_DEPT = Row["ARM_DEPT"].ToString().Trim();
                ARM_PROGRAM = Row["ARM_PROGRAM"].ToString().Trim();
                ARM_APP_NO = Row["ARM_APP_NO"].ToString().Trim();
                ARM_CUST_TYPE = Row["ARM_CUST_TYPE"].ToString().Trim();
                ARM_SOURCE = Row["ARM_SOURCE"].ToString().Trim();
                ARM_INVOICE = Row["ARM_INVOICE"].ToString().Trim();
                ARM_STATUS = Row["ARM_STATUS"].ToString().Trim();
                ARM_TYPE = Row["ARM_TYPE"].ToString().Trim();
                ARM_INVOICE_DATE = Row["ARM_INVOICE_DATE"].ToString().Trim();
                //ARM_AUTO = Row["ARM_AUTO"].ToString().Trim();
                ARM_BILLED = Row["ARM_BILLED"].ToString().Trim();
                //ARM_RECEIPT = Row["ARM_RECEIPT"].ToString().Trim();
                //ARM_ADJUSTMENTS = Row["ARM_ADJUSTMENTS"].ToString().Trim();
                //ARM_DISC_TAKEN = Row["ARM_DISC_TAKEN"].ToString().Trim();
                ARM_BALANCE = Row["ARM_BALANCE"].ToString().Trim();
                //ARM_SITE = Row["ARM_SITE"].ToString().Trim();
                //ARM_ROOM = Row["ARM_ROOM"].ToString().Trim();
                //ARM_AMPM = Row["ARM_AMPM"].ToString().Trim();

                //ARM_DATE_LSTC = Row["ARM_DATE_LSTC"].ToString().Trim();
                //ARM_LSTC_OPERATOR = Row["ARM_LSTC_OPERATOR"].ToString().Trim();
                //ARM_DATE_ADD = Row["ARM_DATE_ADD"].ToString().Trim();
                //ARM_ADD_OPERATOR = Row["ARM_ADD_OPERATOR"].ToString().Trim();
                ARM_DUE_DATE = Row["ARM_DUE_DATE"].ToString().Trim();
                ARM_COMMENTS = Row["ARM_COMMENTS"].ToString().Trim();
                //ART_DESCRIPTION = Row["ART_DESCRIPTION"].ToString().Trim();

                if (strTable == "ARS00125")
                {
                    ARM_AUTO = Row["ARM_AUTO"].ToString().Trim();
                    ARM_RECEIPT = Row["ARM_RECEIPT"].ToString().Trim();
                    ARM_ADJUSTMENTS = Row["ARM_ADJUSTMENTS"].ToString().Trim();
                    ARM_DUE_DATE = Row["ARM_DUE_DATE"].ToString().Trim();
                    ARM_DISC_TAKEN = Row["ARM_DISC_TAKEN"].ToString().Trim();
                    ARM_CLOSED_DATE = Row["ARM_CLOSED_DATE"].ToString().Trim();
                    ARM_DATE_LSTC = Row["ARM_DATE_LSTC"].ToString().Trim();
                    ARM_LSTC_OPERATOR = Row["ARM_LSTC_OPERATOR"].ToString().Trim();
                    ARM_DATE_ADD = Row["ARM_DATE_ADD"].ToString().Trim();
                    ARM_ADD_OPERATOR = Row["ARM_ADD_OPERATOR"].ToString().Trim();
                }

                //if (RepName == "ARSB2160")
                //{
                //    ARM_DUE_DATE = Row["ARM_DUE_DATE"].ToString().Trim();
                //    ARM_COMMENTS = Row["ARM_COMMENTS"].ToString().Trim();
                //    ARM_FUNDING_SOURCE = Row["ARM_FUNDING_SOURCE"].ToString().Trim();
                //    ARM_CLOSED_DATE = Row["ARM_CLOSED_DATE"].ToString().Trim();
                //}
                //else
                //{
                //    ART_CONTRACT = Row["ART_CONTRACT"].ToString().Trim();
                //    ART_DATE = Row["ART_DATE"].ToString().Trim();
                //}


            }
        }

        #endregion


        #region Properties

        public string ARM_AGENCY { get; set; }
        public string ARM_DEPT { get; set; }
        public string ARM_PROGRAM { get; set; }
        public string ARM_APP_NO { get; set; }
        public string ARM_CUST_TYPE { get; set; }
        public string ARM_SOURCE { get; set; }
        public string ARM_INVOICE { get; set; }
        public string ARM_STATUS { get; set; }
        public string ARM_TYPE { get; set; }
        public string ARM_INVOICE_DATE { get; set; }
        public string ARM_DUE_DATE { get; set; }
        public string ARM_COMMENTS { get; set; }
        public string ARM_AUTO { get; set; }
        public string ARM_BILLED { get; set; }
        public string ARM_RECEIPT { get; set; }
        public string ARM_ADJUSTMENTS { get; set; }
        public string ARM_DISC_TAKEN { get; set; }
        public string ARM_BALANCE { get; set; }
        public string ARM_SITE { get; set; }
        public string ARM_ROOM { get; set; }
        public string ARM_AMPM { get; set; }
        public string ARM_FUNDING_SOURCE { get; set; }
        public string ARM_CLOSED_DATE { get; set; }
        public string ARM_DATE_LSTC { get; set; }
        public string ARM_LSTC_OPERATOR { get; set; }
        public string ARM_DATE_ADD { get; set; }
        public string ARM_ADD_OPERATOR { get; set; }
        public string ARM_INVF_DATE { get; set; }
        public string ARM_INVT_DATE { get; set; }
        public string Mode { get; set; }
        public string Rec_Type { get; set; }

        public string ART_DATE { get; set; }
        public string ART_CONTRACT { get; set; }
        public string ART_DESCRIPTION { get; set; }
        public string ART_SEQ { get; set; }
        public string ART_DISC_TAKEN { get; set; }


        #endregion
    }
    
    public class ARSTRANEntity
    {
      #region Construnts

        public ARSTRANEntity()
        {

            art_agency = string.Empty;
            art_dept = string.Empty;
            art_program = string.Empty;
            art_app_no = string.Empty;
            art_Cust_type = string.Empty;
            art_source = string.Empty;
            art_invoice = string.Empty;
            art_Item = string.Empty;
            art_seq = string.Empty;
            art_date = string.Empty;
            art_contract = string.Empty;
            art_location = string.Empty;
            art_program_account = string.Empty;
            art_expense_code = string.Empty;
            art_class_identifier = string.Empty;
            art_inv_amt = string.Empty;
            art_open_amt = string.Empty;
            art_closed_amt = string.Empty;
            art_disc_taken = string.Empty;
            art_description = string.Empty;
            art_pmt_source = string.Empty;
            art_check = string.Empty;
            art1_entry = string.Empty;
            art_pmt_type = string.Empty;
            art1_type = string.Empty;
            art_bill_rate = string.Empty;
            art_units = string.Empty;
            art_date_lstc = string.Empty;
            art_lstc_operator = string.Empty;
            art_date_add = string.Empty;
            art_add_operator = string.Empty;
            art_selct_openamt = string.Empty;
        }
        public ARSTRANEntity(bool Initialize)
        {
            if (Initialize)
            {
                art_agency = null;
                art_dept = null;
                art_program = null;
                art_app_no = null;
                art_Cust_type = null;
                art_source = null;
                art_invoice = null;
                art_Item = null;
                art_seq = null;
                art_date = null;
                art_contract = null;
                art_location = null;
                art_program_account = null;
                art_expense_code = null;
                art_class_identifier = null;
                art_inv_amt = null;
                art_open_amt = null;
                art_closed_amt = null;
                art_disc_taken = null;
                art_description = null;
                art_pmt_source = null;
                art_check = null;
                art1_entry = null;
                art_pmt_type = null;
                art1_type = null;
                art_bill_rate = null;
                art_units = null;
                art_date_lstc = null;
                art_lstc_operator = null;
                art_date_add = null;
                art_add_operator = null;
                art_selct_openamt = null;
            }
        }

        public ARSTRANEntity(DataRow drrow)
        {
            if (drrow!=null)
            {
                art_agency = drrow["ART_AGENCY"].ToString().Trim();
                art_dept = drrow["ART_DEPT"].ToString().Trim();
                art_program = drrow["ART_PROGRAM"].ToString().Trim();
                art_app_no = drrow["ART_APP_NO"].ToString().Trim();
                art_Cust_type = drrow["ART_CUST_TYPE"].ToString().Trim();
                art_source = drrow["ART_SOURCE"].ToString().Trim();
                art_invoice = drrow["ART_INVOICE"].ToString().Trim();
                art_Item = drrow["ART_ITEM"].ToString().Trim();
                art_seq = drrow["ART_SEQ"].ToString().Trim();
                art_date = drrow["ART_DATE"].ToString().Trim();
                art_contract = drrow["ART_CONTRACT"].ToString().Trim();
                art_location = drrow["ART_LOCATION"].ToString().Trim();
                art_program_account = drrow["ART_PROGRAM_ACCOUNT"].ToString().Trim();
                art_expense_code = drrow["ART_EXPENSE_CODE"].ToString().Trim();
                art_class_identifier = drrow["ART_CLASS_IDENTIFIER"].ToString().Trim();
                art_inv_amt = drrow["ART_INV_AMT"].ToString().Trim();
                art_open_amt = drrow["ART_OPEN_AMT"].ToString().Trim();
                art_closed_amt = drrow["ART_CLOSED_AMT"].ToString().Trim();
                art_disc_taken = drrow["ART_DISC_TAKEN"].ToString().Trim();
                art_description = drrow["ART_DESCRIPTION"].ToString().Trim();
                art_pmt_source = drrow["ART_PMT_SOURCE"].ToString().Trim();
                art_check = drrow["ART_CHECK"].ToString().Trim();
                art1_entry = drrow["ART1_ENTRY"].ToString().Trim();
                art_pmt_type = drrow["ART_PMT_TYPE"].ToString().Trim();
                art1_type = drrow["ART_TYPE"].ToString().Trim();
                art_bill_rate = drrow["ART_BILL_RATE"].ToString().Trim();
                art_units = drrow["ART_UNITS"].ToString().Trim();
                art_date_lstc = drrow["ART_DATE_LSTC"].ToString().Trim();
                art_lstc_operator = drrow["ART_LSTC_OPERATOR"].ToString().Trim();
                art_date_add = drrow["ART_DATE_ADD"].ToString().Trim();
                art_add_operator = drrow["ART_ADD_OPERATOR"].ToString().Trim();
                art_selct_openamt = string.Empty;
            }
        }

		 
	#endregion

      #region Properties
        
        public string art_agency { get; set; }
        public string art_dept { get; set; }
        public string art_program { get; set; }
        public string art_app_no { get; set; }
        public string art_Cust_type { get; set; }
        public string art_source { get; set; }
        public string art_invoice { get; set; }
        public string art_Item { get; set; }
        public string art_seq { get; set; }
        public string art_date { get; set; }
        public string art_contract { get; set; }
        public string art_location { get; set; }
        public string art_program_account { get; set; }
        public string art_expense_code { get; set; }
        public string art_class_identifier { get; set; }
        public string art_inv_amt { get; set; }
        public string art_open_amt { get; set; }
        public string art_closed_amt { get; set; }
        public string art_disc_taken { get; set; }
        public string art_description { get; set; }
        public string art_pmt_source { get; set; }
        public string art_check { get; set; }
        public string art1_entry { get; set; }
        public string art_pmt_type { get; set; }
        public string art1_type { get; set; }
        public string art_bill_rate { get; set; }
        public string art_units { get; set; }
        public string art_date_lstc { get; set; }
        public string art_lstc_operator { get; set; }
        public string art_date_add { get; set; }
        public string art_add_operator { get; set; }
        public string art_selct_openamt { get; set; }
        public string Mode { get; set; }
	  #endregion
    }


    public class ARSNUMBEntity
    {
        #region Construnts

        public ARSNUMBEntity()
        {
            arsnumb_code = string.Empty;
            arsnumb_no = string.Empty;


        }
        public ARSNUMBEntity(bool Initialize)
        {
            if (Initialize)
            {
                arsnumb_code = null;
                arsnumb_no = null;
            }
        }

        public ARSNUMBEntity(DataRow drrow)
        {
            if (drrow != null)
            {
                arsnumb_code = drrow["arsnumb_code"].ToString().Trim();
                arsnumb_no = drrow["arsnumb_no"].ToString().Trim();            
            }
        }


        #endregion

        #region Properties
            public string arsnumb_code { get; set; }
            public string arsnumb_no { get; set; }

        #endregion
    }

    public class ARSB2120Entity
    {
        #region Constructors

        public ARSB2120Entity()
        {
            Customer_No = string.Empty;
            Parent_Name = string.Empty;
            Child_Name = string.Empty;
            Fund = string.Empty;
            Class = string.Empty;
            Full_Num = string.Empty;
            DayRate = string.Empty;
            Category = string.Empty;
            Present = string.Empty;
            Absent = string.Empty;
            Total_Bill = string.Empty;
            Parent_Invoice = string.Empty;
            
        }

        public ARSB2120Entity(bool Initalize)
        {
            if (Initalize)
            {
                Customer_No = null;
                Parent_Name = null;
                Child_Name = null;
                Fund = null;
                Class = null;
                Full_Num = null;
                DayRate = null;
                Category = null;
                Present = null;
                Absent = null;
                Total_Bill = null;
                Parent_Invoice = null;
                

            }
        }

        public ARSB2120Entity(string _Customer, string _ParentName, string _ChildName, string _Fund, string _Class, string _FullNum, string _DayRate, string _Category, string _Present, string _Apsent, string _TotalBill, string _Parent_Invoice)
        {
            Customer_No = _Customer; Parent_Name = _ParentName; Child_Name = _ChildName; Fund = _Fund; Class = _Class; Full_Num = _FullNum;DayRate=_DayRate;
            Category = _Category; Present = _Present; Absent = _Apsent; Total_Bill = _TotalBill; Parent_Invoice = _Parent_Invoice;
        }

     
        #endregion

        #region Properties

        public string Customer_No { get; set; }
        public string Parent_Name { get; set; }
        public string Child_Name { get; set; }
        public string Fund { get; set; }
        public string Class { get; set; }
        public string Full_Num { get; set; }
        public string DayRate { get; set; }
        public string Category { get; set; }
        public string Present { get; set; }
        public string Absent { get; set; }
        public string Total_Bill { get; set; }
        public string Parent_Invoice { get; set; }
        
        #endregion

    }


    public class UpdateARSNUMBEntity
    {
        #region Construnts

        public UpdateARSNUMBEntity()
        {
            arsnumb_code = string.Empty;
            arsnumb_no = string.Empty;
            ATTN_AGENCY = string.Empty;
            ATTN_DEPT = string.Empty;
            ATTN_PROG = string.Empty;
            ATTN_YEAR = string.Empty;
            ATTN_APP_NO = string.Empty;
            ATTN_SITE = string.Empty;
            ATTN_ROOM = string.Empty;
            ATTN_AMPM = string.Empty;
            ATTN_FUNDING_SOURCE = string.Empty;
            ATTN_PARENT_INVOICE = string.Empty;
            ATTN_LSTC_OPERATOR = string.Empty;
            Report= string.Empty;


        }
        public UpdateARSNUMBEntity(bool Initialize)
        {
            if (Initialize)
            {
                arsnumb_code = null;
                arsnumb_no = null;
                ATTN_AGENCY = null;
                ATTN_DEPT = null;
                ATTN_PROG = null;
                ATTN_YEAR = null;
                ATTN_APP_NO = null;
                ATTN_SITE = null;
                ATTN_ROOM = null;
                ATTN_AMPM = null;
                ATTN_FUNDING_SOURCE = null;
                ATTN_PARENT_INVOICE = null;
                ATTN_LSTC_OPERATOR = null;
                Report = null;
            }
        }

        public UpdateARSNUMBEntity(DataRow drrow)
        {
            if (drrow != null)
            {
                arsnumb_code = drrow["arsnumb_code"].ToString().Trim();
                arsnumb_no = drrow["arsnumb_no"].ToString().Trim();
                ATTN_AGENCY = drrow["ATTN_AGENCY"].ToString().Trim();
                ATTN_DEPT = drrow["ATTN_DEPT"].ToString().Trim();
                ATTN_PROG = drrow["ATTN_PROGRAM"].ToString().Trim();
                ATTN_YEAR = drrow["ATTN_YEAR"].ToString().Trim();
                ATTN_APP_NO = drrow["ATTN_APP_NO"].ToString().Trim();
                ATTN_SITE = drrow["ATTN_SITE"].ToString().Trim();
                ATTN_ROOM = drrow["ATTN_ROOM"].ToString().Trim();
                ATTN_AMPM = drrow["ATTN_AMPM"].ToString().Trim();
                ATTN_FUNDING_SOURCE = drrow["ATTN_FUNDING_SOURCE"].ToString().Trim();
                ATTN_PARENT_INVOICE = drrow["ATTN_PARENT_INVOICE"].ToString().Trim();
                ATTN_LSTC_OPERATOR = drrow["ATTN_LSTC_OPERATOR"].ToString().Trim();
	        }
        }


        #endregion

        #region Properties
        public string arsnumb_code { get; set; }
        public string arsnumb_no { get; set; }
        public string ATTN_AGENCY { get; set; }
        public string ATTN_DEPT { get; set; }
        public string ATTN_PROG { get; set; }
        public string ATTN_YEAR { get; set; }
        public string ATTN_APP_NO { get; set; }
        public string ATTN_SITE { get; set; }
        public string ATTN_ROOM { get; set; }
        public string ATTN_AMPM { get; set; }
        public string ATTN_FUNDING_SOURCE { get; set; }
        public string ATTN_PARENT_INVOICE { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ATTN_LSTC_OPERATOR { get; set; }
        public string Report { get; set; }

        #endregion
    }



    public class ARSCHECKEntity
    {
        #region Constructors

        public ARSCHECKEntity()
        {
            ARC_AGENCY = string.Empty;
            ARC_DEPT = string.Empty;
            ARC_PROGRAM = string.Empty;
            ARC_APP_NO = string.Empty;
            ARC_CUST_TYPE = string.Empty;
            ARC_SOURCE = string.Empty;
            ARC_CHECK = string.Empty;
            ARC_AUTO = string.Empty;            
            ARC_STATUS = string.Empty;
            ARC_TYPE = string.Empty;
            ARC_CHECK_DATE = string.Empty;
            ARC_DUE_DATE = string.Empty;
            ARC_COMMENTS = string.Empty;         
            ARC_BILLED = string.Empty;
            ARC_RECEIPT = string.Empty;
            ARC_ADJUSTMENTS = string.Empty;
            ARC_DISC_TAKEN = string.Empty;
            ARC_BALANCE = string.Empty;
            ARC_CLOSED_DATE = string.Empty;
            ARC_DATE_LSTC = string.Empty;
            ARC_LSTC_OPERATOR = string.Empty;
            ARC_DATE_ADD = string.Empty;
            ARC_ADD_OPERATOR = string.Empty;
        }

        public ARSCHECKEntity(bool Initialize)
        {
            if (Initialize)
            {
                ARC_AGENCY = null;
                ARC_DEPT = null;
                ARC_PROGRAM = null;
                ARC_APP_NO = null;
                ARC_CUST_TYPE = null;
                ARC_SOURCE = null;
                ARC_CHECK = null;
                ARC_STATUS = null;
                ARC_TYPE = null;
                ARC_CHECK_DATE = null;
                ARC_AUTO = null;                
                ARC_DUE_DATE = null;
                ARC_COMMENTS = null;                
                ARC_BILLED = null;
                ARC_RECEIPT = null;
                ARC_ADJUSTMENTS = null;
                ARC_DISC_TAKEN = null;
                ARC_BALANCE = null;               
                ARC_CLOSED_DATE = null;
                ARC_DATE_LSTC = null;
                ARC_LSTC_OPERATOR = null;
                ARC_DATE_ADD = null;
                ARC_ADD_OPERATOR = null;
            }
        }

        public ARSCHECKEntity(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";

                ARC_AGENCY = Row["ARC_AGENCY"].ToString().Trim();
                ARC_DEPT = Row["ARC_DEPT"].ToString().Trim();
                ARC_PROGRAM = Row["ARC_PROGRAM"].ToString().Trim();
                ARC_APP_NO = Row["ARC_APP_NO"].ToString().Trim();
                ARC_CUST_TYPE = Row["ARC_CUST_TYPE"].ToString().Trim();
                ARC_SOURCE = Row["ARC_SOURCE"].ToString().Trim();
                ARC_CHECK = Row["ARC_CHECK"].ToString().Trim();
                ARC_STATUS = Row["ARC_STATUS"].ToString().Trim();
                ARC_TYPE = Row["ARC_TYPE"].ToString().Trim();
                ARC_CHECK_DATE = Row["ARC_CHECK_DATE"].ToString().Trim();
                ARC_AUTO = Row["ARC_AUTO"].ToString();               
                ARC_DUE_DATE = Row["ARC_DUE_DATE"].ToString().Trim();
                ARC_COMMENTS = Row["ARC_COMMENTS"].ToString().Trim();               
                ARC_BILLED = Row["ARC_BILLED"].ToString().Trim();
                ARC_RECEIPT = Row["ARC_RECEIPT"].ToString().Trim();
                ARC_ADJUSTMENTS = Row["ARC_ADJUSTMENTS"].ToString().Trim();
                ARC_DISC_TAKEN = Row["ARC_DISC_TAKEN"].ToString().Trim();
                ARC_BALANCE = Row["ARC_BALANCE"].ToString().Trim();
                ARC_CLOSED_DATE = Row["ARC_CLOSED_DATE"].ToString().Trim();
                ARC_DATE_LSTC = Row["ARC_DATE_LSTC"].ToString().Trim();
                ARC_LSTC_OPERATOR = Row["ARC_LSTC_OPERATOR"].ToString().Trim();
                ARC_DATE_ADD = Row["ARC_DATE_ADD"].ToString().Trim();
                ARC_ADD_OPERATOR = Row["ARC_ADD_OPERATOR"].ToString().Trim();
            }
        }

        public ARSCHECKEntity(DataRow Row, string strTable, string RepName)
        {
            if (Row != null)
            {
                ARC_AGENCY = Row["ARC_AGENCY"].ToString().Trim();
                ARC_DEPT = Row["ARC_DEPT"].ToString().Trim();
                ARC_PROGRAM = Row["ARC_PROGRAM"].ToString().Trim();
                ARC_APP_NO = Row["ARC_APP_NO"].ToString().Trim();
                ARC_CUST_TYPE = Row["ARC_CUST_TYPE"].ToString().Trim();
                ARC_SOURCE = Row["ARC_SOURCE"].ToString().Trim();
                ARC_CHECK = Row["ARC_INVOICE"].ToString().Trim();
                ARC_STATUS = Row["ARC_STATUS"].ToString().Trim();
                ARC_TYPE = Row["ARC_TYPE"].ToString().Trim();
                ARC_CHECK_DATE = Row["ARC_INVOICE_DATE"].ToString().Trim();
                //ARC_AUTO = Row["ARC_AUTO"].ToString().Trim();
                ARC_BILLED = Row["ARC_BILLED"].ToString().Trim();
                //ARC_RECEIPT = Row["ARC_RECEIPT"].ToString().Trim();
                //ARC_ADJUSTMENTS = Row["ARC_ADJUSTMENTS"].ToString().Trim();
                //ARC_DISC_TAKEN = Row["ARC_DISC_TAKEN"].ToString().Trim();
                ARC_BALANCE = Row["ARC_BALANCE"].ToString().Trim();
                //ARC_SITE = Row["ARC_SITE"].ToString().Trim();
                //ARC_ROOM = Row["ARC_ROOM"].ToString().Trim();
                //ARC_AMPM = Row["ARC_AMPM"].ToString().Trim();

                //ARC_DATE_LSTC = Row["ARC_DATE_LSTC"].ToString().Trim();
                //ARC_LSTC_OPERATOR = Row["ARC_LSTC_OPERATOR"].ToString().Trim();
                //ARC_DATE_ADD = Row["ARC_DATE_ADD"].ToString().Trim();
                //ARC_ADD_OPERATOR = Row["ARC_ADD_OPERATOR"].ToString().Trim();


                ART_DESCRIPTION = Row["ART_DESCRIPTION"].ToString().Trim();

               


            }
        }

        #endregion


        #region Properties

        public string ARC_AGENCY { get; set; }
        public string ARC_DEPT { get; set; }
        public string ARC_PROGRAM { get; set; }
        public string ARC_APP_NO { get; set; }
        public string ARC_CUST_TYPE { get; set; }
        public string ARC_SOURCE { get; set; }
        public string ARC_CHECK { get; set; }
        public string ARC_STATUS { get; set; }
        public string ARC_TYPE { get; set; }
        public string ARC_AUTO { get; set; }
        public string ARC_CHECK_DATE { get; set; }
        public string ARC_DUE_DATE { get; set; }
        public string ARC_COMMENTS { get; set; }        
        public string ARC_BILLED { get; set; }
        public string ARC_RECEIPT { get; set; }
        public string ARC_ADJUSTMENTS { get; set; }
        public string ARC_DISC_TAKEN { get; set; }
        public string ARC_BALANCE { get; set; }
        public string ARC_CLOSED_DATE { get; set; }
        public string ARC_DATE_LSTC { get; set; }
        public string ARC_LSTC_OPERATOR { get; set; }
        public string ARC_DATE_ADD { get; set; }
        public string ARC_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        public string Rec_Type { get; set; }

        public string ART_DATE { get; set; }
        public string ART_CONTRACT { get; set; }
        public string ART_DESCRIPTION { get; set; }


        #endregion
    }


    public class ARSCHKDETEntity
    {
        #region Construnts

        public ARSCHKDETEntity()
        {

            ARD_Agency = string.Empty;
            ARD_Dept = string.Empty;
            ARD_Program = string.Empty;
            ARD_App_no = string.Empty;
            ARD_Cust_type = string.Empty;
            ARD_Source = string.Empty;
            ARD_Invoice = string.Empty;
            ARD_Item = string.Empty;
            ARD_Seq = string.Empty;
            ARD_Date = string.Empty;
            ARD_Contract = string.Empty;
            ARD_Location = string.Empty;
            ARD_Program_account = string.Empty;
            ARD_Expense_code = string.Empty;
            ARD_Class_identifier = string.Empty;
            ARD_Inv_amt = string.Empty;
            ARD_Open_amt = string.Empty;
            ARD_Closed_amt = string.Empty;
            ARD_Disc_taken = string.Empty;
            ARD_Description = string.Empty;
            ARD_Pmt_source = string.Empty;
            ARD_Check = string.Empty;
            ARD1_ITEM = string.Empty;
            ARD_Pmt_type = string.Empty;
            ARD1_Type = string.Empty;
            ARD_Bill_rate = string.Empty;
            ARD_Units = string.Empty;
            ARD_Date_lstc = string.Empty;
            ARD_Lstc_operator = string.Empty;
            ARD_Date_add = string.Empty;
            ARD_Add_operator = string.Empty;
            ARD_Select_Openamt = string.Empty;
            ARD_XmlDetails = string.Empty;
            ARD_Auto = string.Empty;
        }
        public ARSCHKDETEntity(bool Initialize)
        {
            if (Initialize)
            {
                ARD_Agency = null;
                ARD_Dept = null;
                ARD_Program = null;
                ARD_App_no = null;
                ARD_Cust_type = null;
                ARD_Source = null;
                ARD_Invoice = null;
                ARD_Item = null;
                ARD_Seq = null;
                ARD_Date = null;
                ARD_Contract = null;
                ARD_Location = null;
                ARD_Program_account = null;
                ARD_Expense_code = null;
                ARD_Class_identifier = null;
                ARD_Inv_amt = null;
                ARD_Open_amt = null;
                ARD_Closed_amt = null;
                ARD_Disc_taken = null;
                ARD_Description = null;
                ARD_Pmt_source = null;
                ARD_Check = null;
                ARD1_ITEM = null;
                ARD_Pmt_type = null;
                ARD1_Type = null;
                ARD_Bill_rate = null;
                ARD_Units = null;
                ARD_Date_lstc = null;
                ARD_Lstc_operator = null;
                ARD_Date_add = null;
                ARD_Add_operator = null;
                ARD_Select_Openamt = null;
                ARD_Auto = null;
            }
        }

        public ARSCHKDETEntity(DataRow drrow)
        {
            if (drrow != null)
            {
                ARD_Agency = drrow["ARD_AGENCY"].ToString().Trim();
                ARD_Dept = drrow["ARD_DEPT"].ToString().Trim();
                ARD_Program = drrow["ARD_PROGRAM"].ToString().Trim();
                ARD_App_no = drrow["ARD_APP_NO"].ToString().Trim();
                ARD_Cust_type = drrow["ARD_CUST_TYPE"].ToString().Trim();
                ARD_Source = drrow["ARD_SOURCE"].ToString().Trim();
                ARD_Invoice = drrow["ARD_INVOICE"].ToString().Trim();
                ARD_Item = drrow["ARD_ITEM"].ToString().Trim();
                ARD_Seq = drrow["ARD_SEQ"].ToString().Trim();
                ARD_Date = drrow["ARD_DATE"].ToString().Trim();
                ARD_Contract = drrow["ARD_CONTRACT"].ToString().Trim();
                ARD_Location = drrow["ARD_LOCATION"].ToString().Trim();
                ARD_Program_account = drrow["ARD_PROGRAM_ACCOUNT"].ToString().Trim();
                ARD_Expense_code = drrow["ARD_EXPENSE_CODE"].ToString().Trim();
                ARD_Class_identifier = drrow["ARD_CLASS_IDENTIFIER"].ToString().Trim();
                ARD_Inv_amt = drrow["ARD_INV_AMT"].ToString().Trim();
                ARD_Open_amt = drrow["ARD_OPEN_AMT"].ToString().Trim();
                ARD_Closed_amt = drrow["ARD_CLOSED_AMT"].ToString().Trim();
                ARD_Disc_taken = drrow["ARD_DISC_TAKEN"].ToString().Trim();
                ARD_Description = drrow["ARD_DESCRIPTION"].ToString().Trim();
                ARD_Pmt_source = drrow["ARD_PMT_SOURCE"].ToString().Trim();
                ARD_Check = drrow["ARD_CHECK"].ToString().Trim();
                ARD1_ITEM = drrow["ARD1_ITEM"].ToString().Trim();
                ARD_Pmt_type = drrow["ARD_PMT_TYPE"].ToString().Trim();
                ARD1_Type = drrow["ARD_TYPE"].ToString().Trim();
                ARD_Bill_rate = drrow["ARD_BILL_RATE"].ToString().Trim();
                ARD_Units = drrow["ARD_UNITS"].ToString().Trim();
                ARD_Date_lstc = drrow["ARD_DATE_LSTC"].ToString().Trim();
                ARD_Lstc_operator = drrow["ARD_LSTC_OPERATOR"].ToString().Trim();
                ARD_Date_add = drrow["ARD_DATE_ADD"].ToString().Trim();
                ARD_Add_operator = drrow["ARD_ADD_OPERATOR"].ToString().Trim();
                ARD_Select_Openamt = string.Empty;
                ARD_XmlDetails = string.Empty;
                ARD_Auto = string.Empty;
            }
        }


        #endregion

        #region Properties

        public string ARD_Agency { get; set; }
        public string ARD_Dept { get; set; }
        public string ARD_Program { get; set; }
        public string ARD_App_no { get; set; }
        public string ARD_Cust_type { get; set; }
        public string ARD_Source { get; set; }
        public string ARD_Invoice { get; set; }
        public string ARD_Item { get; set; }
        public string ARD_Seq { get; set; }
        public string ARD_Date { get; set; }
        public string ARD_Contract { get; set; }
        public string ARD_Location { get; set; }
        public string ARD_Program_account { get; set; }
        public string ARD_Expense_code { get; set; }
        public string ARD_Class_identifier { get; set; }
        public string ARD_Inv_amt { get; set; }
        public string ARD_Open_amt { get; set; }
        public string ARD_Closed_amt { get; set; }
        public string ARD_Disc_taken { get; set; }
        public string ARD_Description { get; set; }
        public string ARD_Pmt_source { get; set; }
        public string ARD_Check { get; set; }
        public string ARD1_ITEM { get; set; }
        public string ARD_Pmt_type { get; set; }
        public string ARD1_Type { get; set; }
        public string ARD_Bill_rate { get; set; }
        public string ARD_Units { get; set; }
        public string ARD_Date_lstc { get; set; }
        public string ARD_Lstc_operator { get; set; }
        public string ARD_Date_add { get; set; }
        public string ARD_Add_operator { get; set; }
        public string Mode { get; set; }
        public string ARD_Select_Openamt { get; set; }
        public string ARD_XmlDetails { get; set; }
        public string ARD_Auto { get; set; }
        #endregion
    }


}
