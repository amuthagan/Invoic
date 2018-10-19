using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;



namespace ProcessDesigner.ExceptionHandler
{

    public static class LogWriter
    {
        private static string filename = "ExceptionLog.log";
        private static string m_exePath = string.Empty;
        private string filePath = string.Empty;
        private static string folderName = "Exception";
        static bool fileExsists = false;

        private static string GetAllFilesInDirectory()
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
                // string str = "";
                foreach (FileInfo file in files)
                {
                    filename = file.Name;
                    if (File.ReadAllBytes(m_exePath + "\\" + filename).Length >= 100 * 1024 * 1024)
                    {
                        string filenamebase = "ErrorLog";
                        if (filename.Contains("-"))
                        {
                            int filelength = filename.Length - 4;


                            string[] strVal = filename.Split('.');
                            string[] str1 = strVal[0].Split('-');
                            int lognumber = Int32.Parse(str1[1].ToString());



                            //  int lognumber = Int32.Parse(filename.Substring(filename.LastIndexOf("-") + 1, filename.Length - 4)); //Get old number, Can cause exception if the last digits aren't numbers
                            lognumber++; //Increment lognumber by 1
                            filename = filenamebase + "-" + lognumber + ".log";
                            return filename;
                        }
                        else
                        {
                            filename = filenamebase + "-1.log"; //Override filename
                        }

                        System.IO.FileStream fs = System.IO.File.Create(m_exePath + "\\" + filename);
                        fs.Close();
                        return filename;
                    }
                    return filename;
                }
                return filename;
            }
            catch (Exception)
            {

            }
            return filename;
        }

        private static string getFormatedException(Exception exception)
        {
            string exTracedMessage = null;
            if (exception == null) return "";

            try
            {
                exTracedMessage = "Date & Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss FFFF") + String.Format("\r\nErrorDescription:{0}", exception.Message);
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

            }
            catch (Exception)
            {
            }
            return exTracedMessage;
        }

        public static void Write(Exception exception)
        {
            filename = GetAllFilesInDirectory();
            try
            {
                if (filename == null || filename.Trim().Length == 0) return;
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + filename))
                {

                    Log(getFormatedException(exception), w);
                }
            }
            catch (Exception)
            {

            }
        }

        public string checkFileExsists(string m_exePath, string filename)
        {
            try
            {
                bool isFileExsists = false;

                isFileExsists = File.Exists(m_exePath + "\\" + filename);

                if (isFileExsists == false)
                {
                    System.IO.FileStream fs = System.IO.File.Create(m_exePath + "\\" + filename);
                    fs.Close();
                    return filename;

                }
                else
                {
                    if (File.ReadAllBytes(filename).Length >= 1 * 1024)
                    {
                        string filenamebase = "ErrorLog";
                        if (filename.Contains("-"))
                        {
                            string[] str = filename.Split('.');
                            string[] str1 = str[0].Split('-');
                            int lognumber = Int32.Parse(str[1]);

                            // int lognumber = Int32.Parse(filename.Substring(filename.LastIndexOf("-") + 1, filename.Length - 4)); //Get old number, Can cause exception if the last digits aren't numbers
                            lognumber++; //Increment lognumber by 1
                            filename = filenamebase + "-" + lognumber + ".log";
                        }
                        else
                        {
                            filename = filenamebase + "-1.log"; //Override filename
                        }

                    }
                    System.IO.FileStream fs = System.IO.File.Create(m_exePath + "\\" + filename);
                    fs.Close();
                    return filename;
                }
            }
            catch
            {
            }

            return filename;
        }

        public static void Log(string logMessage, TextWriter txtWriter)
        {
            if (logMessage == null || logMessage.Trim().Length == 0) return;

            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception)
            {
            }
        }
    }
}

