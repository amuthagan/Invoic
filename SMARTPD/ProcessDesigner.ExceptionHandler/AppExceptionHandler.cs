using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessDesigner.ExceptionHandler
{
    public static class AppException
    {
        public static string Initialize(string providerName, string connectionString)
        {
            string sReturnValue = null;
            try
            {
                IConfigurationSource config = ConfigurationSourceFactory.Create();
                ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);
                DatabaseProviderFactory dbfactory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);
                DatabaseFactory.SetDatabaseProviderFactory(dbfactory, false);
                LogWriterFactory logWriterFactory = new LogWriterFactory(config);
                Logger.SetLogWriter(logWriterFactory.Create());
                ExceptionManager exManager = factory.CreateManager();
                ExceptionPolicy.SetExceptionManager(factory.CreateManager());
                sReturnValue = "Successfully Initialized";
            }
            catch (Exception ex)
            {
                sReturnValue = ex.Message;
            }
            return sReturnValue;
        }

        public static void Log(Exception ex)
        {
            try
            {
                ExceptionHandler.Default.HandleException(ex);
            }
            catch (Exception)
            {

            }

            try
            {
                WriteLog(ex);
            }
            catch (Exception)
            {

            }
        }

        public static void ShowAndLog(Exception ex)
        {
            try
            {
                WriteLog(ex);
            }
            catch (Exception)
            {

            }
            ExceptionHandler.Default.HandleException(ex, out ex);
        }

        private static string getFormatedException(Exception exception)
        {
            string exTracedMessage = null;
            if (exception == null) return "";

            try
            {
                exTracedMessage = "Date & Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss FFFF");
                exTracedMessage += "\r\nDomain Name: " + System.Environment.UserDomainName.ToString();
                exTracedMessage += "\r\nHost Name: " + System.Net.Dns.GetHostName().ToString();
                exTracedMessage += "\r\nIP Address: " + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();
                exTracedMessage += "\r\nOS Version: " + System.Environment.OSVersion.ToString();
                exTracedMessage += String.Format("\r\nDescription:{0}", exception.Message);

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
                //exTracedMessage += "\r\n";

                //FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, sf.GetFileName());
                //if (fileIOPermission.IsUnrestricted())
                //{
                //}

            }
            catch (Exception)
            {
            }
            return exTracedMessage;
        }

        public static void WriteLog(Exception exception)
        {
            string logMessage = getFormatedException(exception);
            if (logMessage == null || logMessage.Trim().Length == 0) return;

            string filename = GetNextFileInDirectory();
            if (filename == null || filename.Trim().Length == 0) return;

            try
            {
                using (StreamWriter txtWriter = File.AppendText(filename))
                {
                    txtWriter.Write(logMessage);
                    txtWriter.Write(exception.ToString());
                    txtWriter.WriteLine("\r\n");
                }
            }
            catch (Exception)
            {

            }
        }

        private static string GetNextFileInDirectory()
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderName = "Exception_Log";
            bool fileExsists = false;
           // string filename = "Exception_" + DateTime.Now.ToString("ddMMyyyy") + ".log";
            string filename = "Exception.log";
            try
            {
                string strFileName = string.Empty;
                m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!Directory.Exists(m_exePath + "\\" + folderName))
                {
                    Directory.CreateDirectory(m_exePath + "\\" + folderName);
                }
                m_exePath = m_exePath + "\\" + folderName;
                DirectoryInfo d = new DirectoryInfo(m_exePath); //Assuming Test is your Folder
                FileInfo[] files = d.GetFiles("*.log"); //Getting Text files
                if (files.Length == 0)
                {
                    if (fileExsists == false)
                    {
                        System.IO.FileStream fs = System.IO.File.Create(m_exePath + "\\" + filename);
                        fs.Close();
                    }
                }

                foreach (FileInfo file in files)
                {
                    filename = file.Name;
                    if (File.ReadAllBytes(m_exePath + "\\" + filename).Length >= 100 * 1024 * 1024)
                    {
                        string filenamebase = "Exception";
                        if (filename.Contains("-"))
                        {
                            int filelength = filename.Length - 4;


                            string[] strVal = filename.Split('.');
                            string[] str1 = strVal[0].Split('-');
                            int lognumber = Int32.Parse(str1[1].ToString());

                            lognumber++; //Increment lognumber by 1
                            filename = filenamebase + "-" + lognumber + ".log";
                            return m_exePath + "\\" + filename;
                        }
                        else
                        {
                            filename = filenamebase + "-1.log"; //Override filename
                        }

                        System.IO.FileStream fs = System.IO.File.Create(m_exePath + "\\" + filename);
                        fs.Close();
                        return m_exePath + "\\" + filename;
                    }
                    return m_exePath + "\\" + filename;
                }
                return m_exePath + "\\" + filename;
            }
            catch (Exception)
            {

            }
            return m_exePath + "\\" + filename;
        }

    }
}
