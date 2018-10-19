using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Reflection;

namespace ProcessDesigner.BLL
{
    public class FeatureUpdateBll : Essential
    {
        public FeatureUpdateBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public List<PCCS> getFeatureUpdateDetails()
        {
            //_FEATURE
            var getFeatureUpdates = (from e in DB.PCCS
                                     where e.FEATURE != ""
                                     select e).ToList();
            return getFeatureUpdates;
        }

        public DataView GetFeatureUpdateMaster()
        {
            //DataTable dataValue;

            ////using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\TestFolder\WriteLines2.txt", true))
            ////{
            ////    file.WriteLine("Time Start");
            ////    file.WriteLine("Time Second:" + System.DateTime.Now.Second);
            ////    file.WriteLine("Time Millisecond:" + System.DateTime.Now.Millisecond);
            //    //dataValue = ToDataTable((from c in DB.PCCS.AsEnumerable()
            //    //                         orderby c.FEATURE ascending
            //    //                         select new { c.FEATURE }).Distinct().ToList());
            ////    file.WriteLine("Time Second:" + System.DateTime.Now.Second);
            ////    file.WriteLine("Time Millisecond:" + System.DateTime.Now.Millisecond);
            ////    file.WriteLine("Time End");

            ////}
            System.Data.DataTable getFeatureUpdates = (from e in DB.PCCS
                                                       where e.FEATURE != ""
                                                       orderby e.FEATURE ascending
                                                       select new { FEATURE = e.FEATURE }).Distinct().ToList().ToDataTable();
            return getFeatureUpdates.DefaultView;
        }

        public bool FeatureAddDuplicate(string newFeature)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from a in DB.PCCS
                                  where a.FEATURE == newFeature
                                  select new { a.FEATURE }).ToList());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                        return false;
                }
                else
                {
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }
        public bool updateFeatureUpdateMaster(string existingFeatureUpdate, string newFeature)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                newFeature = newFeature.Replace("'", "''");
                existingFeatureUpdate = existingFeatureUpdate.Replace("'", "''");
                query.Append("UPDATE PCCS set Feature='" + newFeature + "'  WHERE Feature='" + existingFeatureUpdate + "'");
                IEnumerable<int> result = DB.ExecuteQuery<int>(query.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
