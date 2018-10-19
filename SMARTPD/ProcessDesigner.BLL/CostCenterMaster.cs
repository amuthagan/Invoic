using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.DAL;
using System.Data;
using System.ComponentModel;

namespace ProcessDesigner.BLL
{

    public class CostCenterMaster : INotifyPropertyChanged
    {

        private string _costCentCode;
        private string _cateCode;
        private string _locCode;
        private string _module;
        private double _efficiency;
        private string _costCentDesc;
        private string _machineName;
        private DataTable _opercode;
        private DataTable _dgOperation;
        private DataTable _unitcode;

        public string CostCentCode
        {
            get { return _costCentCode; }
            set
            {
                _costCentCode = value;
                NotifyPropertyChanged("CostCentCode");
            }
        }

        public string CateCode
        {
            get { return _cateCode; }
            set
            {
                _cateCode = value;
                NotifyPropertyChanged("CateCode");
            }
        }

        public string LocCode
        {
            get { return _locCode; }
            set
            {
                _locCode = value;
                NotifyPropertyChanged("LocCode");
            }
        }

        public string Module
        {
            get { return _module; }
            set
            {
                _module = value;
                NotifyPropertyChanged("Module");
            }
        }

        public double Efficiency
        {
            get { return _efficiency; }
            set
            {
                _efficiency = value;
                NotifyPropertyChanged("Efficiency");
            }
        }


        public string CostCentDesc
        {
            get { return _costCentDesc; }
            set
            {
                _costCentDesc = value;
                NotifyPropertyChanged("CostCentDesc");
            }
        }


        public string MachineName
        {
            get { return _machineName; }
            set
            {
                _machineName = value;
                NotifyPropertyChanged("MachineName");
            }
        }

        public DataTable OperCode
        {
            get { return _opercode; }
            set
            {
                _opercode = value;
                NotifyPropertyChanged("OperCode");
            }
        }

        public DataTable UnitCode
        {
            get { return _unitcode; }
            set
            {
                _unitcode = value;
                NotifyPropertyChanged("UnitCode");
            }
        }


        public DataTable dgOperation
        {
            get { return _dgOperation; }
            set
            {
                _dgOperation = value;
                NotifyPropertyChanged("dgOperation");
            }
        }



        public DataTable Output { get; set; }
        public DataTable Operation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
