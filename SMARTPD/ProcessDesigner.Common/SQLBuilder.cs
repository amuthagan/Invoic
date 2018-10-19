using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace ProcessDesigner.Common
{
    public class SQLBuilder
    {
        public StringBuilder SQL = new StringBuilder();
        public List<DbParameter> Parameters = new List<DbParameter>();
    }
}
