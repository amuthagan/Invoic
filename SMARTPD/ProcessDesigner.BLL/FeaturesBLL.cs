using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class FeaturesBLL : Essential
    {
        public FeaturesBLL(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataTable GetAllFeature()
        {
            string result = "select distinct FEATURE"
                + " from PCCS"
                + " order by FEATURE";
            var getfeature = ToDataTable(DB.ExecuteQuery<AllFeatures>(result).ToList());
            return getfeature;
        }
    }
}
