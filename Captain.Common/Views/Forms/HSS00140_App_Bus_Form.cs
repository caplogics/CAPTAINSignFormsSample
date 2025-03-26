/************************************************************************
 * Conversion On    :   12/19/2022      * Converted By     :   Kranthi
 * Modified On      :   12/19/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00140_App_Bus_Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public HSS00140_App_Bus_Form(BaseForm baseForm, string sel_App)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Lbl_App_NO.Text = Search_App = sel_App;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text = "Bus Route";
            Fill_App_Details_Grid();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Search_App { get; set; }

        public string Year { get; set; }

        public string App_No { get; set; }



        public PrivilegeEntity Privileges { get; set; }


        #endregion


        private void Fill_App_Details_Grid()
        {
            int rowIndex = 0;
            List<BUSCEntity> App_list;
            BUSCEntity SearchEntity = new BUSCEntity(true);
            SearchEntity.BUSC_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity.BUSC_DEPT = BaseForm.BaseDept.Trim();
            SearchEntity.BUSC_PROG = BaseForm.BaseProg.Trim(); //SearchEntity.BUSC_NUMBER = Number.Trim();
            //SearchEntity.BUSC_ROUTE = Route_Id.Trim();
            SearchEntity.BUSC_YEAR = BaseForm.BaseYear; SearchEntity.BUSC_CHILD = Search_App.Trim();
            App_list = _model.SPAdminData.Browse_ChldBUSC(SearchEntity, "Browse");

            List<ChldBMEntity> BusList = new List<ChldBMEntity>();
            ChldBMEntity SearchEntity1 = new ChldBMEntity(true);
            SearchEntity1.ChldBMAgency = BaseForm.BaseAgency.Trim();
            SearchEntity1.chldBMDept = BaseForm.BaseDept.Trim();
            SearchEntity1.ChldBMProg = BaseForm.BaseProg.Trim();
            SearchEntity1.Sort = "C";
            BusList = _model.SPAdminData.Browse_ChldBM(SearchEntity1, "Browse");

            List<BusRTEntity> Route_list = new List<BusRTEntity>();
            string Baseyear = "    ";
            BusRTEntity SearchEntity2 = new BusRTEntity(true);
            SearchEntity2.BUSRT_AGENCY = BaseForm.BaseAgency.Trim(); SearchEntity2.BUSRT_DEPT = BaseForm.BaseDept.Trim();
            SearchEntity2.BUSRT_PROGRAM = BaseForm.BaseProg.Trim();
            //SearchEntity2.BUSRT_NUMBER = Number.Trim(); SearchEntity2.BUSRT_ROUTE = Route_Id.Trim();
            if (string.IsNullOrEmpty(BaseForm.BaseYear.Trim())) Baseyear = "    "; else Baseyear = BaseForm.BaseYear.Trim();
            SearchEntity2.BUSRT_YEAR = Baseyear;
            Route_list = _model.SPAdminData.Browse_ChldBUSR(SearchEntity2, "Browse");

            string Bus_Name = "", Bus_ID = "", Area_Served = "" ;
            foreach (BUSCEntity ent in App_list)
            {
                Bus_Name = Bus_ID = Area_Served = "";
                foreach (ChldBMEntity Entity in BusList)
                {
                    if (Entity.ChldBMNumber.ToString().Trim() == ent.BUSC_NUMBER.Trim())
                    {
                        Bus_Name = Entity.Desc.ToString().Trim();
                        Bus_ID = Entity.ChldBMNumber.ToString().Trim();
                        break;
                    }
                }

                foreach (BusRTEntity Entity in Route_list)
                {
                    if (Entity.BUSRT_NUMBER.ToString().Trim() == ent.BUSC_NUMBER.Trim() &&
                        Entity.BUSRT_ROUTE.ToString().Trim() == ent.BUSC_ROUTE.Trim())
                    {
                        Area_Served = Entity.BUSRT_AREA_SERVED.ToString().Trim();
                        break;
                    }
                }

                rowIndex = App_Details_Grid.Rows.Add(Bus_ID + " - " + Bus_Name, ent.BUSC_ROUTE + " - " + Area_Served, Get_Time_Format(ent.BUSC_PICKUP.Trim()), Get_Time_Format(ent.BUSC_HOME.Trim()));
            }

        }





        private string Get_Time_Format(string strTime)
        {
            string Time = strTime, AM_PM = "AM", Ret_Time = "";
            if (!string.IsNullOrEmpty(strTime.Trim()))
            {
                //Time = "0000".Substring(0, 4 - Time.Length) + Time;

                //char[] delimiters = new char[] { ':', ' ' };
                string[] spltTime = Time.Split(':');

                string Disp_Hours = spltTime[0];
                string Disp_Min = spltTime[1];
                string Disp_Sec = spltTime[2];
                int Compare_Hours = int.Parse(Disp_Hours);
                if (Compare_Hours > 11)
                {
                    AM_PM = "PM";
                    if (Compare_Hours > 12)
                        Compare_Hours -= 12;
                }


                Disp_Hours = Compare_Hours.ToString();
                Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;
                Ret_Time = Disp_Hours + ":" + Disp_Min + ":" + Disp_Sec + " " + AM_PM;
            }

            return Ret_Time;

        }

    }
}