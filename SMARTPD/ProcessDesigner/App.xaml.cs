using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Data.Common;
using System.Security.Permissions;
using ProcessDesigner.ExceptionHandler;
using ProcessDesigner.Common;
using System.Resources;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {

                ResourceManager myManager = new ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                string connectionStringName = myManager.GetString("ConnectionString");
                string encryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                string providerName = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
                string connectionString = ProcessDesigner.DAL.Cipher.Decrypt(encryptedConnectionString, true);

                if (AppException.Initialize(providerName, connectionString) != "Successfully Initialized")
                {

                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }
        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            e.Handled = true;
            MessageBox.Show(e.Exception.Message);
            //if (e.Exception.Message.Contains("Violation of UNIQUE KEY constraint") == false)
            //{
            //    e.Exception.LogException();
            //    ShowInformationMessage(PDMsg.DBNotConnected);
            //    //Application.Current.Shutdown(0);
            //}
            //else
            //{

            //}
            e.Exception.LogException();

            //AppException(e.Exception);
            //if (MessageBox.Show("Do you want to close the application\r\n\r\nUnhandled Exception:" + e.Exception.Message, "Process Designer", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //{
            //    Application.Current.Shutdown(0);
            //}
        }

        private void ApplicationException(Exception exception)
        {
            if (exception == null) return;
            if (exception as DbException != null)
            {
                GotDbException(exception as DbException);
            }

            try
            {
                string exTracedMessage = "Date & Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss FFFF") + String.Format("\r\nErrorDescription:{0}", exception.Message);
                StackTrace st = new StackTrace(exception, true);
                int maxTraceCount = st.FrameCount > 5 ? 5 : st.FrameCount;
                for (int i = 0; i < maxTraceCount; i++)
                {
                    StackFrame sf = st.GetFrame(i);
                    string exceptionFormat = "";
                    if (sf.GetFileLineNumber() != 0 && sf.GetFileColumnNumber() != 0)
                        exceptionFormat = String.Format("\r\nFile:{0}\r\nMethod:{1}\r\nLine Number:{2}, Column Number:{3}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber(), sf.GetFileColumnNumber());
                    else if (sf.GetFileLineNumber() != 0)
                        exceptionFormat = String.Format("\r\nFile:{0}\r\nMethod:{1}\r\nLine Number:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber());

                    if (!String.IsNullOrEmpty(exceptionFormat))
                    {
                        exTracedMessage += exceptionFormat;
                        Console.WriteLine();
                        Console.WriteLine(exceptionFormat);
                        exTracedMessage += "\r\n" + (i < maxTraceCount ? "\t" : "");
                    }

                }
                exTracedMessage += "\r\n";

                //FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, sf.GetFileName());
                //if (fileIOPermission.IsUnrestricted())
                //{
                //}

                MessageBox.Show(exception.Message);
                exception = null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void GotDbException(DbException exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}
