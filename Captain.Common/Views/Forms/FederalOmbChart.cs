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
    public partial class FederalOmbChart : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;

        #region Properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity privilege { get; set; }
        public List<MasterPovertyEntity> masterpovertyEntity { get; set; }
        #endregion
        public FederalOmbChart(BaseForm baseForm, PrivilegeEntity privilegeEntity, List<MasterPovertyEntity> MasterPoverty, string strType,string strHeader)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            privilege = privilegeEntity;
            this.Text = privilege.Program + " - " + strHeader;
            masterpovertyEntity = MasterPoverty;
            gridHeaderText(strType);
            lblSDate.Text = LookupDataAccess.Getdate(masterpovertyEntity[0].GdlStartDate);
            lblEDate.Text = LookupDataAccess.Getdate(masterpovertyEntity[0].GdlEndDate);
            int intMasterPovertyRows = 0;
            foreach (var master in masterpovertyEntity)
            {
                if (master.Gdl3Value != "")
                {
                    if (Convert.ToDecimal((master.Gdl3Value)) == 0)
                        master.Gdl3Value = "  ";                
                }
                if (master.Gdl4Value != "")
                {
                    if (Convert.ToDecimal((master.Gdl4Value)) == 0)
                        master.Gdl4Value = "  ";
                }
                if (master.Gdl5Value != "")
                {
                    if (Convert.ToDecimal((master.Gdl5Value)) == 0)
                        master.Gdl3Value = "  ";
                }
                if (master.Gdl5Value != "")
                {
                    if (Convert.ToDecimal((master.Gdl5Value)) == 0)
                        master.Gdl5Value = "  ";
                }
                if (master.Gdl6Value != "")
                {
                    if (Convert.ToDecimal((master.Gdl6Value)) == 0)
                        master.Gdl6Value = "  ";
                }

                if (strType == "FED")
                {
                    dataGridFed.Rows.Add(master.Gdl1Value, master.Gdl2Value, master.Gdl3Value, master.Gdl4Value, master.Gdl5Value, master.Gdl6Value);
                }
                else
                { 
                    if(intMasterPovertyRows != 0)
                    {
                    dataGridFed.Rows.Add(master.GdlNoHouseHolds,master.Gdl1Value, master.Gdl2Value, master.Gdl3Value, master.Gdl4Value, master.Gdl5Value, master.Gdl6Value);
                    }
                    intMasterPovertyRows++;
                }
            }



        }

        private void gridHeaderText(string strType)
        {
            dataGridFed.Columns[0].HeaderText = " ";
            dataGridFed.Columns[1].HeaderText = " ";
            dataGridFed.Columns[2].HeaderText = " ";
            dataGridFed.Columns[3].HeaderText = " ";
            dataGridFed.Columns[4].HeaderText = " ";
            dataGridFed.Columns[5].HeaderText = " ";
            dataGridFed.Columns[6].HeaderText = " ";
            if (strType == "FED")
            {
                dataGridFed.Columns[0].HeaderText = "Pov.Base";
                dataGridFed.Columns[1].HeaderText = "Increment";
            }
            else
            {
                


                dataGridFed.Columns[0].HeaderText = "Fam Size";
                if (Convert.ToString(masterpovertyEntity[0].Gdl1Value) != "")
                    dataGridFed.Columns[1].HeaderText = Convert.ToString(masterpovertyEntity[0].Gdl1Value) + "%";
                if (Convert.ToString(masterpovertyEntity[0].Gdl2Value) != "")
                    dataGridFed.Columns[2].HeaderText = Convert.ToString(masterpovertyEntity[0].Gdl2Value) + "%";
                if (Convert.ToString(masterpovertyEntity[0].Gdl3Value) != "")
                    dataGridFed.Columns[3].HeaderText = Convert.ToString(masterpovertyEntity[0].Gdl3Value) + "%";
                if (Convert.ToString(masterpovertyEntity[0].Gdl4Value) != "")
                    dataGridFed.Columns[4].HeaderText = Convert.ToString(masterpovertyEntity[0].Gdl4Value) + "%";
                if (Convert.ToString(masterpovertyEntity[0].Gdl5Value) != "")
                    dataGridFed.Columns[5].HeaderText = Convert.ToString(masterpovertyEntity[0].Gdl5Value) + "%";
                if (Convert.ToString(masterpovertyEntity[0].Gdl6Value) != "")
                    dataGridFed.Columns[6].HeaderText = Convert.ToString(masterpovertyEntity[0].Gdl6Value) + "%";
              
            }
        }
    }
}