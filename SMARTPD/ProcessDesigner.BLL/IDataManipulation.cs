using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public interface IDataManipulation
    {
        bool Insert<T>(List<T> entities);
        bool Update<T>(List<T> entities);
        bool Delete<T>(List<T> entities);
    }
}
