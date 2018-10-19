using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;

namespace ProcessDesigner
{

    public class StatusMessage
    {
        public static void setStatus(UserInformation _userInformation, string message = "", string currentStatus = "")
        {
            Status statusMsg = new Status();

            statusMsg.UserName = _userInformation.UserName;
            if (message != "")
            {
                statusMsg.StatusMessage = message;
            }

            if (currentStatus != "")
            {
                statusMsg.CurrentStatus = currentStatus;
            }

            //_userInformation.StatusBarDetails.DataContext = statusMsg;
        }
    }

    class Status
    {
        public string StatusMessage { get; set; }
        public string UserName { get; set; }
        public string CurrentStatus { get; set; }
    }
}
