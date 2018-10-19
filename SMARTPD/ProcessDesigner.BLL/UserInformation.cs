using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.DAL;

namespace ProcessDesigner.BLL
{
    public class UserInformation
    {
        public string UserName { get; set; }
        public DataAccessLayer Dal { get; set; }
       
        public SFLPD_UAT SFLPDDatabase { get; set; }
        public string UserRole { get; set; }
        public string Version { get; set; }
    }
}
