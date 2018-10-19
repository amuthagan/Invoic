﻿using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ReportMISMFMModel : ViewModelBase
    {
        private string _cust_name;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string CUST_NAME
        {
            get { return _cust_name; }
            set
            {
                _cust_name = value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }

        private string _sfl_part_number;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string PART_NO
        {
            get { return _sfl_part_number; }
            set
            {
                _sfl_part_number = value;
                NotifyPropertyChanged("PART_NO");
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

        private string _grid_title;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string GRID_TITLE
        {
            get { return _grid_title; }
            set
            {
                _grid_title = value;
                NotifyPropertyChanged("GRID_TITLE");
            }
        }


    }
}
