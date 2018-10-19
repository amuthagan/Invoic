using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessDesigner.ViewModel
{
    [Export]
    public class TapmacViewModel : BindableBase  
    {
        private DDTAPPING_MAC _ddtappingmac;
        private readonly ICommand cancelTapmacCommand;
        private readonly ICommand saveTapmacCommand;
        private UserInformation userinformation;
        private Tapmachine tapmac;
        public Action CloseAction { get; set; }
        [ImportingConstructor]
        public TapmacViewModel()
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            tapmac = new Tapmachine();
            this.cancelTapmacCommand = new DelegateCommand(this.CancelTapmac);
            this.saveTapmacCommand = new DelegateCommand(this.SaveTapmac);
        }
        public DDTAPPING_MAC ddtappingmac
        {
            get
            {
                return this._ddtappingmac;
            }
            set
            {
                SetProperty(ref this._ddtappingmac, value);
            }
        }
        public ICommand CancelTapmacCommand { get { return this.cancelTapmacCommand; } }
        public ICommand SaveTapmacCommand { get { return this.saveTapmacCommand; } }
        private void CancelTapmac()
        {
            CloseAction();
        }
        private void SaveTapmac()
        {
            try
            {
                tapmac.saveTapmachine(ddtappingmac);               
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
