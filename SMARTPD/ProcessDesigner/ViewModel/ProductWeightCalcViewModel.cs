using ProcessDesigner.Common;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;

namespace ProcessDesigner.ViewModel
{
    public class ProductWeightCalcViewModel : ViewModelBase  
    {
        private ProductWeightCalc bllPrdWeight;
        public ProductWeightCalcViewModel(UserInformation userInformation,string entityPrimarykey)
        {

        }
    }
}
