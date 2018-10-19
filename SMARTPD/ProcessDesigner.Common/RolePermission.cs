using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Common
{
    public class RolePermission
    {
        public string SourceName { get; set; }
        public bool AddNew { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Print { get; set; }
        public bool View { get; set; }
        public bool Save { get; set; }
        public bool Close { get; set; }
        public bool Copy { get; set; }
        public bool Search { get; set; }
        public bool StandartNotes { get; set; }
        public bool CostSheetSearch { get; set; }
        public bool ProductSearch { get; set; }
        public bool SimilarPartNumber { get; set; }
        public bool ShowRelated { get; set; }
        public bool ReleaseDocument { get; set; }
        public bool UpdateOrderProcessing { get; set; }
        public bool CreateCIReference { get; set; }

    }
}
