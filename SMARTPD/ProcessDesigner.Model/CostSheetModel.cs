using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class CostSheetModel : ViewModelBase
    {


        private string _part_no;
        public string PART_NO
        {
            get { return _part_no; }
            set
            {
                _part_no = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        private string _part_desc;
        public string PART_DESC
        {
            get { return _part_desc; }
            set
            {
                _part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }


        private Nullable<int> _route_no;
        public Nullable<int> ROUTE_NO
        {
            get { return _route_no; }
            set
            {
                _route_no = value;
                NotifyPropertyChanged("ROUTE_NO");
            }
        }

        private string _wire_rod_cd;
        public string WIRE_ROD_CD
        {
            get { return _wire_rod_cd; }
            set
            {
                _wire_rod_cd = value;
                NotifyPropertyChanged("WIRE_ROD_CD");
            }
        }

        private string _customer;
        public string CUSTOMER
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyPropertyChanged("CUSTOMER");
            }
        }

        private Nullable<decimal> _rmcost = 0;
        public Nullable<decimal> RMCOST
        {
            get { return _rmcost; }
            set
            {
                _rmcost = (value != null) ? value : 0;
                NotifyPropertyChanged("RMCOST");

                TOTAL = RMCOST + COST;

                if (FINISH_WT != null && FINISH_WT != 0)
                {
                    REAL = TOTAL * 1000 / FINISH_WT;
                }
                else
                {
                    REAL = 0;
                }
            }
        }

        private Nullable<decimal> _rmmast = 0;
        public Nullable<decimal> RMMAST
        {
            get { return _rmmast; }
            set
            {
                _rmmast = (value != null) ? value : 0;
                NotifyPropertyChanged("RMMAST");
            }
        }

        private Nullable<decimal> _total = 0;
        public Nullable<decimal> TOTAL
        {
            get { return _total; }
            set
            {
                _total = (value != null) ? value : 0;
                NotifyPropertyChanged("TOTAL");
            }
        }

        private Nullable<decimal> _cost = 0;
        public Nullable<decimal> COST
        {
            get { return _cost; }
            set
            {
                _cost = (value != null) ? value : 0;
                NotifyPropertyChanged("COST");

                TOTAL = RMCOST + COST;

                if (FINISH_WT != null && FINISH_WT != 0)
                {
                    REAL = TOTAL * 1000 / FINISH_WT;
                }
                else
                {
                    REAL = 0;
                }
            }
        }

        private Nullable<decimal> _real = 0;
        public Nullable<decimal> REAL
        {
            get { return _real; }
            set
            {
                _real = (value != null) ? value : 0;
                NotifyPropertyChanged("REAL");
            }
        }



        private Nullable<decimal> _finish_wt = 0;
        public Nullable<decimal> FINISH_WT
        {
            get { return _finish_wt; }
            set
            {
                _finish_wt = (value != null) ? value : 0;
                NotifyPropertyChanged("FINISH_WT");
            }
        }

        private Nullable<decimal> _cheese_wt = 0;
        public Nullable<decimal> CHEESE_WT
        {
            get { return _cheese_wt; }
            set
            {
                _cheese_wt = (value != null) ? value : 0;
                NotifyPropertyChanged("CHEESE_WT");
            }
        }

        private string _location;
        public string LOCATION
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyPropertyChanged("LOCATION");
            }
        }

        private string _custcode;
        public string CUSTCODE
        {
            get { return _custcode; }
            set
            {
                _custcode = value;
                NotifyPropertyChanged("CUSTCODE");
            }
        }

        private string _finish_desc;
        public string FINISH_DESC
        {
            get { return _finish_desc; }
            set
            {
                _finish_desc = value;
                NotifyPropertyChanged("FINISH_DESC");
            }
        }

        private bool _export = false;
        public bool EXPORT
        {
            get { return _export; }
            set
            {
                _export = value;
                NotifyPropertyChanged("EXPORT");
            }
        }

        private bool _exportisclicked = false;
        public bool ExportIsClicked
        {
            get { return _exportisclicked; }
            set
            {
                _exportisclicked = value;
                NotifyPropertyChanged("ExportIsClicked");
            }
        }

        private DataView _dvProcessMain = null;
        public DataView DVProcessMain
        {
            get { return _dvProcessMain; }
            set
            {
                _dvProcessMain = value;
                NotifyPropertyChanged("DVProcessMain");
            }
        }

        private DataView _dvProductMaster = null;
        public DataView DVProductMaster
        {
            get { return _dvProductMaster; }
            set
            {
                _dvProductMaster = value;
                NotifyPropertyChanged("DVProductMaster");
            }
        }

        private DataView _dvCostSheet = null;
        public DataView DVCostSheet
        {
            get { return _dvCostSheet; }
            set
            {
                _dvCostSheet = value;
                NotifyPropertyChanged("DVCostSheet");
            }
        }

        private DataView _dvrmbasic = null;
        public DataView DVRMBasic
        {
            get { return _dvrmbasic; }
            set
            {
                _dvrmbasic = value;
                NotifyPropertyChanged("DVRMBasic");
            }
        }
    }
}
