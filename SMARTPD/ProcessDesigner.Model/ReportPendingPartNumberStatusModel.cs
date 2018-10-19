using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ReportPendingPartNumberStatusModel : ViewModelBase
    {
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
                _grid_title = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("GRID_TITLE");
            }
        }

        private Nullable<DateTime> _doc_rel_dt_plan;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Planned Document Release Date")]
        public Nullable<DateTime> DOC_REL_DT_PLAN
        {
            get { return _doc_rel_dt_plan; }
            set
            {
                _doc_rel_dt_plan = value;
                NotifyPropertyChanged("DOC_REL_DT_PLAN");
            }
        }

    }
}
