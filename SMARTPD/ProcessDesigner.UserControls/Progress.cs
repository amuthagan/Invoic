using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessDesigner.UserControls
{
    public class Progress
    {
        private static Thread newWindowThread = null;

        private static string processingtext = "Loading";
        public static string ProcessingText
        {
            get { return processingtext; }
            set
            {
                processingtext = value;
                //if (pw != null) pw.txtProcess.Text = value;
            }
        }

        public static void Start()
        {
            try
            {
                newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
                newWindowThread.SetApartmentState(ApartmentState.STA);
                newWindowThread.IsBackground = true;
                newWindowThread.Start();
            }
            catch (Exception)
            {

            }
        }

        private static ProcessingWindow pw = null;
        private static void ThreadStartingPoint()
        {
            try
            {
                pw = null;
                pw = new ProcessingWindow();
                pw.txtProcess.Text = ProcessingText;
                pw.ShowInTaskbar = false;
                pw.ShowDialog();
                System.Windows.Threading.Dispatcher.Run();
            }
            catch (Exception)
            {

            }
        }

        public static void End()
        {
            try
            {
                Thread.Sleep(10);
                newWindowThread.Abort();
                newWindowThread = null;
                processingtext = "Loading";
                pw = null;
            }
            catch (Exception)
            {

            }
        }
    }
}
