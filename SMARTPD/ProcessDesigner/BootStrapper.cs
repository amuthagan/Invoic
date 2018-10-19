using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.MefExtensions;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.Reflection;
using System.IO;
namespace ProcessDesigner
{
   public class BootStrapper : MefBootstrapper
   {
       protected override System.Windows.DependencyObject CreateShell()
       {
           return this.Container.GetExportedValue<frmLogin>();
       }

       protected override void InitializeShell()
       {

           base.InitializeShell();
         
           Application.Current.MainWindow  = (Window)this.Shell;
           Application.Current.MainWindow.ShowDialog();
       }

       protected override void ConfigureAggregateCatalog()
       {

           base.ConfigureAggregateCatalog();
           var excuteAssembly = Assembly.GetExecutingAssembly();
           var callingAssembly = Assembly.GetCallingAssembly();
           var entryAssembly = Assembly.GetEntryAssembly();
           //this.AggregateCatalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".exe"));
           this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(excuteAssembly));
           //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(CoreModule).Assembly));       
       }

    }
}
