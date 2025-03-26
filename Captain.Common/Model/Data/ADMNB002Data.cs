using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;


namespace Captain.Common.Model.Data
{
    public class ADMNB002Data
    {

        public ADMNB002Data()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion

        public List<EMPLFUNCEntity> ADMNB002_GetScrPrivbyUserId(string ScrCode, string ModuleCode)
        {
            List<EMPLFUNCEntity> EMPLFUNCprof = new List<EMPLFUNCEntity>();
            try
            {
                DataSet EMPLFUNCData = ADMNB002DB.Rep_ADMNB002_GetScrPrivbyUserId(ScrCode, ModuleCode);
                if (EMPLFUNCData != null && EMPLFUNCData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in EMPLFUNCData.Tables[0].Rows)
                    {
                        EMPLFUNCprof.Add(new EMPLFUNCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return EMPLFUNCprof;
            }

            return EMPLFUNCprof;
        }
        public DataSet ADMNB002_GetDistinctUserId(string Usercode, string ModuleCode)
        {
            DataSet ds = new DataSet();
            try
            {
                DataSet EMPLFUNCData = ADMNB002DB.Rep_ADMNB002_GetScrPrivbyUserId(Usercode, ModuleCode);
                if (EMPLFUNCData.Tables.Count > 0)
                {
                    ds = EMPLFUNCData;
                }
            }
            catch (Exception ex)
            {
                //
                return ds;
            }

            return ds;
        }

        public List<BATCNTLEntity> ADMNB002_GetRepPrivbyUserId(string UserName, string ModuleCode)
        {
            List<BATCNTLEntity> BATCNTLprof = new List<BATCNTLEntity>();
            try
            {
                DataSet BATCNTLCData = ADMNB002DB.ADMNB002_GetRepPrivbyUserId(UserName, ModuleCode);
                if (BATCNTLCData != null && BATCNTLCData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in BATCNTLCData.Tables[0].Rows)
                    {
                        BATCNTLprof.Add(new BATCNTLEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return BATCNTLprof;
            }

            return BATCNTLprof;
        }


    }
}
