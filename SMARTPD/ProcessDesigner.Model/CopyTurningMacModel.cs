using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ProcessDesigner.Model
{
    public class CopyTurningMacModel : ViewModelBase
    {
        private string _cost_cent_code;
        private string _machine_type;
        private string _workholder_type;
        private Nullable<decimal> _max_mandrel_len;
        private Nullable<decimal> _min_prod_dia;
        private Nullable<decimal> _max_prod_dia;
        private Nullable<decimal> _max_prod_len;
        private Nullable<decimal> _min_prod_len;
        private Nullable<decimal> _spindle_speed;
        private string _feed_restrictions;
        private Nullable<decimal> _turret_stations;
        private string _coolant;

        public string COST_CENT_CODE
        {
            get
            {
                return _cost_cent_code;
            }
            set
            {
                _cost_cent_code = value;
                NotifyPropertyChanged("COST_CENT_CODE");
            }
        }
        public string MACHINE_TYPE
        {
            get
            {
                return _machine_type;
            }
            set
            {
                _machine_type = value;
                NotifyPropertyChanged("MACHINE_TYPE");
            }
        }
        public string WORKHOLDER_TYPE
        {
            get
            {
                return _workholder_type;
            }
            set
            {
                _workholder_type = value;
                NotifyPropertyChanged("WORKHOLDER_TYPE");
            }
        }
        public Nullable<decimal> MAX_MANDREL_LEN
        {
            get
            {
                return _max_mandrel_len;
            }
            set
            {
                _max_mandrel_len = value;
                NotifyPropertyChanged("MAX_MANDREL_LEN");
            }
        }
        public Nullable<decimal> MIN_PROD_DIA
        {
            get
            {
                return _min_prod_dia;
            }
            set
            {
                _min_prod_dia = value;
                NotifyPropertyChanged("MIN_PROD_DIA");
            }
        }
        public Nullable<decimal> MAX_PROD_DIA
        {
            get
            {
                return _max_prod_dia;
            }
            set
            {
                _max_prod_dia = value;
                NotifyPropertyChanged("MAX_PROD_DIA");
            }
        }
        public Nullable<decimal> MAX_PROD_LEN
        {
            get
            {
                return _max_prod_len;
            }
            set
            {
                _max_prod_len = value;
                NotifyPropertyChanged("MAX_PROD_LEN");
            }
        }
        public Nullable<decimal> MIN_PROD_LEN
        {
            get
            {
                return _min_prod_len;
            }
            set
            {
                _min_prod_len = value;
                NotifyPropertyChanged("MIN_PROD_LEN");
            }
        }
        public Nullable<decimal> SPINDLE_SPEED
        {
            get
            {
                return _spindle_speed;
            }
            set
            {
                _spindle_speed = value;
                NotifyPropertyChanged("SPINDLE_SPEED");
            }
        }
        public string FEED_RESTRICTIONS
        {
            get
            {
                return _feed_restrictions;
            }
            set
            {
                _feed_restrictions = value;
                NotifyPropertyChanged("FEED_RESTRICTIONS");
            }
        }
        public Nullable<decimal> TURRET_STATIONS
        {
            get
            {
                return _turret_stations;
            }
            set
            {
                _turret_stations = value;
                NotifyPropertyChanged("TURRET_STATIONS");
            }
        }
        public string COOLANT
        {
            get
            {
                return _coolant;
            }
            set
            {
                _coolant = value;
                NotifyPropertyChanged("COOLANT");
            }
        }
    }
}
