using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ProcessDesigner
{

    public class StatusMessage
    {
        public static string UserName = "";
        public static StatusBar StatusBarDetails = null;
        public static void setStatus(string message = null, string currentStatus = null)
        {
            Status statusMsg = new Status();

            Status oldstatus = null;

            if (StatusBarDetails != null)
            {
                oldstatus = StatusBarDetails.DataContext as Status;

                statusMsg.UserName = UserName;
                if (message != null)
                {
                    statusMsg.StatusMessage = message;
                }
                else if (oldstatus != null)
                {
                    statusMsg.StatusMessage = oldstatus.StatusMessage;
                }

                if (currentStatus != null)
                {
                    statusMsg.CurrentStatus = currentStatus;
                }
                else if (oldstatus != null)
                {
                    statusMsg.CurrentStatus = oldstatus.CurrentStatus;
                }

                StatusBarDetails.DataContext = statusMsg;
            }
        }
    }

    class Status
    {
        public string StatusMessage { get; set; }
        public string UserName { get; set; }
        public string CurrentStatus { get; set; }
    }
}
