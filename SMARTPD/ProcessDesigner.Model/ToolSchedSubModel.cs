using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ToolSchedSubModel : ViewModelBase
    {
        private string _pART_NO;
        private decimal _rOUTE_NO;
        private decimal _sEQ_NO;
        private decimal _cC_SNO;
        private decimal _sUB_HEADING_NO;
        private string _sNO;
        private string _tOOL_CODE;
        private string _tOOL_CODE_END;
        private string _tOOL_DESC;
        private string _cATEGORY;
        private System.Nullable<decimal> _qTY;
        private string _rEMARKS;
        private System.Guid _rOWID;
        private int _iDPK;
        private int _delete;
        //new by nandhini
        private string _sUB_HEADING;     
        public string SUB__HEADING
        {
            get
            {
                return _sUB_HEADING;
            }
            set
            {
                _sUB_HEADING = value;
                NotifyPropertyChanged("SUB_HEADING");

            }
        }
        //end new nandhini
        

        public string PART_NO
        {
            get
            {
                return _pART_NO;
            }
            set
            {
                _pART_NO = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        public decimal ROUTE_NO
        {
            get
            {
                return _rOUTE_NO;
            }
            set
            {
                _rOUTE_NO = value;
                NotifyPropertyChanged("ROUTE_NO");
            }
        }

        public decimal SEQ_NO
        {
            get
            {
                return _sEQ_NO;
            }
            set
            {
                _sEQ_NO = value;
                NotifyPropertyChanged("SEQ_NO");
            }
        }

        public decimal CC_SNO
        {
            get
            {
                return _cC_SNO;
            }
            set
            {
                _cC_SNO = value;
                NotifyPropertyChanged("CC_SNO");
            }
        }

        public decimal SUB_HEADING_NO
        {
            get
            {
                return _sUB_HEADING_NO;
            }
            set
            {
                _sUB_HEADING_NO = value;
                NotifyPropertyChanged("SUB_HEADING_NO");
            }
        }

        public string SNO
        {
            get
            {
                return _sNO;
            }
            set
            {
                _sNO = value;
                NotifyPropertyChanged("SNO");
            }
        }

        public string TOOL_CODE
        {
            get
            {
                return _tOOL_CODE;
            }
            set
            {
                _tOOL_CODE = value;
                NotifyPropertyChanged("TOOL_CODE");
            }
        }

        public string TOOL_CODE_END
        {
            get
            {
                return _tOOL_CODE_END;
            }
            set
            {
                _tOOL_CODE_END = value;
                NotifyPropertyChanged("TOOL_CODE_END");
            }
        }

        public string TOOL_DESC
        {
            get
            {
                return _tOOL_DESC;
            }
            set
            {
                _tOOL_DESC = value;
                NotifyPropertyChanged("TOOL_DESC");
            }
        }

        public string CATEGORY
        {
            get
            {
                return _cATEGORY;
            }
            set
            {
                _cATEGORY = value;
                NotifyPropertyChanged("CATEGORY");
            }
        }

        public System.Nullable<decimal> QTY
        {
            get
            {
                return _qTY;
            }
            set
            {
                _qTY = value;
                NotifyPropertyChanged("QTY");
            }
        }

        public string REMARKS
        {
            get
            {
                return _rEMARKS;
            }
            set
            {
                _rEMARKS = value;
                NotifyPropertyChanged("REMARKS");
            }
        }
        public event PropertyChangingEventHandler PropertyChanging;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
     
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ROWID", DbType = "UniqueIdentifier NOT NULL")]      
        public System.Guid ROWID
        {
            get
            {
                return this._rOWID;
            }
            set
            {
                if ((this._rOWID != value))
                {
                    this.SendPropertyChanging();
                    this._rOWID = value;
                    this.SendPropertyChanged("ROWID");
                }
            }
        }
        //public System.Guid ROWID
        //{
        //    get
        //    {
        //        return _rOWID;
        //    }
        //    set
        //    {
        //        _rOWID = value;
        //        NotifyPropertyChanged("ROWID");
        //    }
        //}

        public int IDPK
        {
            get
            {
                return _iDPK;
            }
            set
            {
                _iDPK = value;
                NotifyPropertyChanged("IDPK");
            }
        }

        public int Delete
        {
            get
            {
                return _delete;
            }
            set
            {
                _delete = value;
                NotifyPropertyChanged("Delete");
            }
        }



    }
}
