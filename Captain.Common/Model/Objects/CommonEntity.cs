using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.DataProcessing.InMemoryDataProcessor;

namespace Captain.Common.Model.Objects
{
    public class CommonEntity
    {
        #region Constructors

        public CommonEntity()
        {

            Code = string.Empty;
            Desc = string.Empty;
            Active = string.Empty;
            Default = string.Empty;
            Hierarchy = string.Empty;
            ListHierarchy = new List<string>();
            Extension = string.Empty;
        }

        public CommonEntity(string code, string desc)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            InActiveflag = false;
        }

        public CommonEntity(DataRow row)
        {
            Desc    = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();
            ListHierarchy = new List<string>();
            if(!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count; )
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
        }

       
        public CommonEntity(CommonEntity Entity)
        {
            Desc = Entity.Desc;
            Code = Entity.Code;
            Active = Entity.Active;
            Default = Entity.Default;
            Hierarchy = Entity.Hierarchy;
            Extension = Entity.Extension;
        }

        public CommonEntity(DataRow row,string strTable)
        {
            if (strTable == "Funds")
            {
                Desc = row["LookUpDesc"].ToString().Trim();
                Code = row["Code"].ToString().Trim();
                Active = row["Active"].ToString().Trim();
                Default = row["AGY_DEFAULT"].ToString().Trim();
                Hierarchy = row["Hierarchy"].ToString().Trim();
                ListHierarchy = new List<string>();
                if (!Hierarchy.Equals(string.Empty))
                {
                    int nextIndex = 0;
                    for (int count = 0; Hierarchy.Length > count; )
                    {
                        ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                        count = count + 6;
                        nextIndex = count;
                    }
                }
                Extension = row["EXT"].ToString().Trim();
            }
            else if (strTable == "SPANISH")
            {
                Desc = row["AGYS_DESC"].ToString().Trim();
                Code = row["AGYS_CODE"].ToString().Trim();
                Active = row["AGYS_Active"].ToString().Trim();
                Default = row["AGYS_DEFAULT"].ToString().Trim();
                Hierarchy = row["AGYS_Hierarchy"].ToString().Trim();
                AgyCode = row["AGYS_TYPE"].ToString().Trim();
                SDesc = row["AGYS_SDESC"].ToString().Trim();
            }
            else if (strTable == "AGYHEADERS")
            {
                Desc = row["AGY_DESC"].ToString().Trim();
                Code = row["AGY_TYPE"].ToString().Trim();
                
            }
            else
            {
                Desc = row["AGYS_DESC"].ToString().Trim();
                Code = row["AGYS_CODE"].ToString().Trim();
                Active = row["AGYS_Active"].ToString().Trim();
                Default = row["AGYS_DEFAULT"].ToString().Trim();
                Hierarchy = row["AGYS_Hierarchy"].ToString().Trim();
                AgyCode = row["AGYS_TYPE"].ToString().Trim();
                Sort = row["SORT"].ToString().Trim();
                ListHierarchy = new List<string>();
                if (!Hierarchy.Equals(string.Empty))
                {
                    int nextIndex = 0;
                    for (int count = 0; Hierarchy.Length > count; )
                    {
                        ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                        count = count + 6;
                        nextIndex = count;
                    }
                }

            }
        }

        public CommonEntity(string code, string desc,string hierachy)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
        }

        public CommonEntity(string code, string desc, string hierachy,string extension)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
            Extension = extension.Trim();           
        }

        public CommonEntity(string code, string desc, string hierachy, string extension, string defaults, string active,string Rep)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
            Extension = extension.Trim();
            Default = defaults.Trim();
            Active = active.Trim();
            AgyCode = Rep.Trim();
            ListHierarchy = new List<string>();
            if(!string.IsNullOrEmpty(hierachy.Trim()))
            {
                if(hierachy.Length>5)
                {
                    if (!Hierarchy.Equals(string.Empty))
                    {
                        int nextIndex = 0;
                        if(!Hierarchy.Contains("."))
                        {
                            for (int count = 0; Hierarchy.Length > count;)
                            {
                                ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                                count = count + 6;
                                nextIndex = count;
                            }
                        }
                        
                    }
                }
            }
            
        }
        public CommonEntity(string code, string desc, string hierachy, string extension,string defaults,string active)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
            Extension = extension.Trim();
            Default = defaults.Trim();
            Active = active.Trim();
            ListHierarchy = new List<string>();
            if (!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count; )
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
        }

        public CommonEntity(string code, string desc, string hierachy, string extension,  string year, string Agency,string Dept,string Prog)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
            Extension = extension.Trim();            
            Pyear = year.Trim();
            PAgency = Agency.Trim();
            PDept = Dept.Trim();
            Pprog = Prog.Trim();
        }

        public CommonEntity(string code, string desc, string hierachy, string extension, string year, string Agency, string Dept, string Prog,string active)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
            Extension = extension.Trim();
            Pyear = year.Trim();
            PAgency = Agency.Trim();
            PDept = Dept.Trim();
            Pprog = Prog.Trim();
            Active = active.Trim();

        }

        public CommonEntity(string code, string desc, string hierachy, string extension, string year, string Agency, string Dept, string Prog,string active,string sdesc)
        {
            Code = code.Trim();
            Desc = desc.Trim();
            Hierarchy = hierachy.Trim();
            Extension = extension.Trim();
            Pyear = year.Trim();
            PAgency = Agency.Trim();
            PDept = Dept.Trim();
            Pprog = Prog.Trim();
            Active = active.Trim();
            SDesc = sdesc.Trim();
        }

        #endregion

        #region Properties

        public string Code { get; set; }
        public string Desc { get; set; }
        public string Active { get; set; }
        public string Default { get; set; }
        public string Hierarchy { get; set; }        
        public List<string> ListHierarchy { get; set; }
        public bool InActiveflag { get; set; }
        public string Extension { get; set; }
        public string AgyCode { get; set; }
        public string PAgency { get; set; }
        public string PDept { get; set; }
        public string Pprog { get; set; }
        public string Pyear { get; set; }
        public string SDesc { get; set; }

        public string Sort { get; set; }

        #endregion

    }
}
