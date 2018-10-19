using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class UpdateHistoryBLL : Essential
    {

        public UpdateHistoryBLL(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
        }
        
    }
}
