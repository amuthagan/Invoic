using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using System.Data;

namespace ProcessDesigner.Model
{
    public class OperatorQualityAssuranceModel : ViewModelBase
    {
        public OperatorQualityAssuranceModel()
            : base()
        {
            InitializeMandatoryFields(this);
        }

        private string _part_number;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number should not be Empty")]
        public string PART_NO
        {
            get { return _part_number; }
            set
            {
                _part_number = value;
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

        private Nullable<DateTime> _today_date;
        public Nullable<DateTime> TODAY_DATE
        {
            get { return _today_date; }
            set
            {
                _today_date = value;
                NotifyPropertyChanged("TODAY_DATE");
            }
        }

        private string _sequence_no;
        public string SEQ_NO
        {
            get { return _sequence_no; }
            set
            {
                _sequence_no = value;
                NotifyPropertyChanged("SEQ_NO");
            }
        }

        private string _shift_no;
        public string SHIFT_NO
        {
            get { return _shift_no; }
            set
            {
                _shift_no = value;
                NotifyPropertyChanged("SHIFT_NO");
            }
        }

        private string _cost_cent_code;
        public string CC_CODE
        {
            get { return _cost_cent_code; }
            set
            {
                _cost_cent_code = value;
                NotifyPropertyChanged("CC_CODE");
            }
        }

        private string _work_order_no;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Work Order Number")]
        public string WORK_ORDER_NO
        {
            get { return _work_order_no; }
            set
            {
                _work_order_no = value;
                NotifyPropertyChanged("WORK_ORDER_NO");
            }
        }

        private string _ccf;
        public string CCF
        {
            get { return _ccf; }
            set
            {
                _ccf = value;
                NotifyPropertyChanged("CCF");
            }
        }

        private string _next_operation_code;
        public string NEXT_OPERATION_CODE
        {
            get { return _next_operation_code; }
            set
            {
                _next_operation_code = value;
                NotifyPropertyChanged("NEXT_OPERATION_CODE");
            }
        }

        private string _wire_dia;
        public string TS_ISSUE_ALTER
        {
            get { return _wire_dia; }
            set
            {
                _wire_dia = value;
                NotifyPropertyChanged("TS_ISSUE_ALTER");
            }
        }

        private string _quantity;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Quantity")]
        public string QUANTITY
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                NotifyPropertyChanged("QUANTITY");
            }
        }

        private bool _is_read_only_quantity = true;
        public bool IS_READ_ONLY_QUANTITY
        {
            get
            {
                return _is_read_only_quantity;
            }
            set
            {
                _is_read_only_quantity = value;
                NotifyPropertyChanged("IS_READ_ONLY_QUANTITY");
            }
        }

        private bool _is_save_enabled = false;
        public bool IS_SAVE_ENABLED
        {
            get
            {
                return _is_save_enabled;
            }
            set
            {
                _is_save_enabled = value;
                NotifyPropertyChanged("IS_SAVE_ENABLED");
            }
        }


        private string _raw_material;
        public string RAW_MATERIAL
        {
            get { return _raw_material; }
            set
            {
                _raw_material = value;
                NotifyPropertyChanged("RAW_MATERIAL");
            }
        }

        private string _machine_name;
        public string MACHINE_NAME
        {
            get { return _machine_name; }
            set
            {
                _machine_name = value;
                NotifyPropertyChanged("MACHINE_NAME");
            }
        }

        private string _operation_code;
        public string OPERATION_CODE
        {
            get { return _operation_code; }
            set
            {
                _operation_code = value;
                NotifyPropertyChanged("OPERATION_CODE");
            }
        }

        private string _operation_desc;
        public string OPERATION_DESC
        {
            get { return _operation_desc; }
            set
            {
                _operation_desc = value;
                NotifyPropertyChanged("OPERATION_DESC");
            }
        }

        private string _next_operation_desc;
        public string NEXT_OPERATION_DESC
        {
            get { return _next_operation_desc; }
            set
            {
                _next_operation_desc = value;
                NotifyPropertyChanged("NEXT_OPERATION_DESC");
            }
        }
        private string _sequence_drawing_issue_no;
        public string SEQUENCE_DRAWING_ISSUE_NO
        {
            get { return _sequence_drawing_issue_no; }
            set
            {
                _sequence_drawing_issue_no = value;
                NotifyPropertyChanged("SEQUENCE_DRAWING_ISSUE_NO");
            }
        }

        private string _route_no;
        public string ROUTE_NO
        {
            get { return _route_no; }
            set
            {
                _route_no = value;
                NotifyPropertyChanged("ROUTE_NO");
            }
        }

        private Nullable<DateTime> _sequence_drawing_issue_date;
        public Nullable<DateTime> SEQUENCE_DRAWING_ISSUE_DATE
        {
            get { return _sequence_drawing_issue_date; }
            set
            {
                _sequence_drawing_issue_date = value;
                NotifyPropertyChanged("SEQUENCE_DRAWING_ISSUE_DATE");
            }
        }

        private DataView _gridData;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public DataView GridData
        {
            get { return _gridData; }
            set
            {
                _gridData = value;
                NotifyPropertyChanged("GridData");
            }
        }

    }
}
