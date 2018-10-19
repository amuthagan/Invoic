using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Data.Common;

namespace ProcessDesigner
{
    public class AppSystemException : Exception
    {
        public AppSystemException() : base() { }
        public AppSystemException(string message)
            : base(message)
        {
            if (String.IsNullOrEmpty(message)) return;

            MessageBox.Show(message);
        }
        //public AppSystemException(OracleException exception)
        //    : base(exception.Message)
        //{
        //    if (exception == null) return;
        //    //string exTracedMessage = "";
        //    //StackTrace st = new StackTrace(exception, true);
        //    //for (int i = 0; i < st.FrameCount; i++)
        //    //{
        //    //    StackFrame sf = st.GetFrame(i);
        //    //    string exceptionFormat = String.Format("File:{0},Method:{1},Line Number:{2}, Column Number:", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber(), sf.GetFileColumnNumber());
        //    //    exTracedMessage += exceptionFormat;
        //    //    Console.WriteLine();
        //    //    Console.WriteLine(exceptionFormat);
        //    //    exTracedMessage += "\r\n" + (i < st.FrameCount ? "\t" : "");
        //    //}
        //    MessageBox.Show(exception.Message);
        //}
        public AppSystemException(Exception exception)
            : base(exception.Message)
        {
            if (exception == null) return;
            if (exception as DbException != null)
            {
                GotDbException(exception as DbException);
            }
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
        }

        private void GotDbException(DbException exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}
