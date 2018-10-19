using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;



namespace ProcessDesigner.Common
{

    public class LogWriter
    {
        private string filename = "ErrorLog.log";
        private string m_exePath = string.Empty;
        private string filePath = string.Empty;
        private string folderName = "Error";
        bool fileExsists = false;
        public LogWriter()
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }

        public string GetAllFilesInDirectory()
        {
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
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void LogWrite(string logMessage)
        {
            filename = GetAllFilesInDirectory();
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + filename))
                {

                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}

