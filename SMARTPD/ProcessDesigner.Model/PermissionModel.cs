using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ProcessDesigner.Model
{
   public class PermissionModel : SEC_ROLE_OBJECT_PERMISSION
    {
        public DataTable RolePermission { get; set; }
    }
}
