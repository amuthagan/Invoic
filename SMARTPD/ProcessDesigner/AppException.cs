using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace ProcessDesigner
{
    public class AppException : ApplicationException
    {
        public AppException() : base() { }
        public AppException(string message)
            : base(message)
        {
            if (String.IsNullOrEmpty(message)) return;

            MessageBox.Show(message);
        }
        public AppException(Exception exception)
            : base(exception.Message, exception.InnerException)
        {
            if (exception == null) return;
            string exTracedMessage = "";
            StackTrace st = new StackTrace(exception, true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                string exceptionFormat = String.Format("File:{0},Method:{1},Line Number:{2}, Column Number:", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber(), sf.GetFileColumnNumber());
                exTracedMessage += exceptionFormat;
                Console.WriteLine();
                Console.WriteLine(exceptionFormat);
                exTracedMessage += "\r\n" + (i < st.FrameCount ? "\t" : "");
            }
            MessageBox.Show(exception.Message);
            exception = null;
        }

    }
}
