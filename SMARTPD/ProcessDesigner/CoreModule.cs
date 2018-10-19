using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner
{
    [Module(ModuleName = "CoreModule")]
    [ModuleExport(typeof(CoreModule))]
    public class CoreModule : IModule
    {
        private IRegionManager _regionManager;

        [ImportingConstructor]
        public CoreModule(IRegionManager regionManager)
        {
            this._regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
            _regionManager.RegisterViewWithRegion("ExhibitRegion", typeof(frmExhibit));
        }

        #endregion
    }
}
