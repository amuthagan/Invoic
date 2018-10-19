using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner
{
    
  public class cStatusBar
    {
     
    public static ProcessDesigner.MainWindow MW = new ProcessDesigner.MainWindow();
     
        
      public static void SetStatus(string Status)
    {
        MW.tbStatus.Text = Status;
    }
       
    }
}
