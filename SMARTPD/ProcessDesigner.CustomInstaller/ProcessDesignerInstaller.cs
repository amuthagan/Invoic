using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Forms;
using System.Configuration;

namespace ProcessDesigner.CustomInstaller
{
    [RunInstaller(true)]
    public partial class ProcessDesignerInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// When overridden in a derived class, performs the installation.
        /// </summary>
        /// <param name="stateSaver">An <see cref="T:System.Collections.IDictionary" /> used to save information needed to perform a commit, rollback, or uninstall operation.</param>
        /// <exception cref="InstallException">Missing config File</exception>
        //[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string connString = string.Empty;
            string AppConfConnStr = string.Empty;
            
            
            // Database Server Name
            string databaseServerName = this.Context.Parameters["DatabaseServerName"];

            // Database Name 
            string databaseName = this.Context.Parameters["DatabaseName"];

            // User Name with Access Rights.
            string userName = this.Context.Parameters["UserName"];

            // Password with Access Rights.
            string password = this.Context.Parameters["Password"];

            connString = @"Data Source=" + databaseServerName + ";Initial Catalog=" + databaseName + ";User ID=" + userName + ";Password=" + password;

            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string Filelocation = asm.Location;
            string DirectoryPath = Path.GetDirectoryName(Filelocation);

            string FILE_NAME = string.Concat(DirectoryPath, @"\ProcessDesigner.exe.Config"); //the application configuration file name

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(FILE_NAME);

            XmlNode node = xmldoc.DocumentElement;

            foreach (XmlNode node1 in node.ChildNodes)
            {
                foreach (XmlNode node2 in node1.ChildNodes)
                {
                    if (node1.Name == "connectionStrings" && node2.Name == "add")
                    {
                        if (node2.Attributes.GetNamedItem("name").Value == "ENGGDB")
                        {
                            node2.Attributes.GetNamedItem("connectionString").Value = connString;
                        }
                    }
                }
            }

            xmldoc.Save(FILE_NAME);
        }

        //[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        //[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        //[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }



    }
}
