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
    public class ProcessSheetModel : ViewModelBase
    {
        private string _part_no;
        private string _part_desc;
        private DataView _dvProductMaster = null;
        private DataView _dvProcessMain = null;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number is required.")]
        public string PART_NO
        {
            get { return _part_no; }
            set
            {
                _part_no = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        private DataView _dtUnitDetails;
        public DataView UnitDetails
        {
            get { return _dtUnitDetails; }
            set
            {
                _dtUnitDetails = value;
                NotifyPropertyChanged("UnitDetails");
            }
        }

        public string PART_DESC
        {
            get { return _part_desc; }
            set
            {
                _part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }

        public DataView DVProductMaster
        {
            get { return _dvProductMaster; }
            set
            {
                _dvProductMaster = value;
                NotifyPropertyChanged("DVProductMaster");
            }
        }

        private Nullable<int> _route_no;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Process Number is required.")]
        public Nullable<int> ROUTE_NO
        {
            get { return _route_no; }
            set
            {
                _route_no = value;
                NotifyPropertyChanged("ROUTE_NO");
            }
        }


        public DataView DVProcessMain
        {
            get { return _dvProcessMain; }
            set
            {
                _dvProcessMain = value;
                NotifyPropertyChanged("DVProcessMain");
            }
        }

        private DataView _dvProcessMainDetails = null;
        public DataView DVProcessMainDetails
        {
            get { return _dvProcessMainDetails; }
            set
            {
                _dvProcessMainDetails = value;
                NotifyPropertyChanged("DVProcessMainDetails");
            }
        }


        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                NotifyPropertyChanged("ActionMode");
            }
        }

        private bool _current_proc = false;
        public bool CURRENT_PROC
        {
            get { return _current_proc; }
            set
            {
                _current_proc = value;
                NotifyPropertyChanged("CURRENT_PROC");
            }
        }

        private string _ajax_cd = null;
        public string AJAX_CD
        {
            get { return _ajax_cd; }
            set
            {
                _ajax_cd = value;
                NotifyPropertyChanged("AJAX_CD");
            }
        }

        private string _tko_cd = null;
        public string TKO_CD
        {
            get { return _tko_cd; }
            set
            {
                _tko_cd = value;
                NotifyPropertyChanged("TKO_CD");
            }
        }

        private string _rm_cd = null;
        public string RM_CD
        {
            get { return _rm_cd; }
            set
            {
                _rm_cd = value;
                NotifyPropertyChanged("RM_CD");
            }
        }

        private string _alt_rm_cd = null;
        public string ALT_RM_CD
        {
            get { return _alt_rm_cd; }
            set
            {
                _alt_rm_cd = value;
                NotifyPropertyChanged("ALT_RM_CD");
            }
        }

        private string _alt_rm_cd1 = null;
        public string ALT_RM_CD1
        {
            get { return _alt_rm_cd1; }
            set
            {
                _alt_rm_cd1 = value;
                NotifyPropertyChanged("ALT_RM_CD1");
            }
        }

        private string _wire_rod_cd = null;
        public string WIRE_ROD_CD
        {
            get { return _wire_rod_cd; }
            set
            {
                _wire_rod_cd = value;
                NotifyPropertyChanged("WIRE_ROD_CD");
            }
        }

        private string _alt_wire_rod_cd = null;
        public string ALT_WIRE_ROD_CD
        {
            get { return _alt_wire_rod_cd; }
            set
            {
                _alt_wire_rod_cd = value;
                NotifyPropertyChanged("ALT_WIRE_ROD_CD");
            }
        }

        private string _alt_wire_rod_cd1 = null;
        public string ALT_WIRE_ROD_CD1
        {
            get { return _alt_wire_rod_cd1; }
            set
            {
                _alt_wire_rod_cd1 = value;
                NotifyPropertyChanged("ALT_WIRE_ROD_CD1");
            }
        }

        private Nullable<decimal> _cheese_wt = null;
        public Nullable<decimal> CHEESE_WT
        {
            get { return _cheese_wt; }
            set
            {
                _cheese_wt = value;
                NotifyPropertyChanged("CHEESE_WT");
            }
        }

        private Nullable<decimal> _cheese_wt_est = null;
        public Nullable<decimal> CHEESE_WT_EST
        {
            get { return _cheese_wt_est; }
            set
            {
                _cheese_wt_est = value;
                NotifyPropertyChanged("CHEESE_WT_EST");
            }
        }

        private Nullable<decimal> _rm_wt = null;
        public Nullable<decimal> RM_WT
        {
            get { return _rm_wt; }
            set
            {
                _rm_wt = value;
                NotifyPropertyChanged("RM_WT");
            }
        }

        private string _ci_reference = null;
        public string CI_REFERENCE
        {
            get { return _ci_reference; }
            set
            {
                _ci_reference = value;
                NotifyPropertyChanged("CI_REFERENCE");
            }
        }

        private DataView _dvprocesssheet = null;
        public DataView DVProcessSheet
        {
            get { return _dvprocesssheet; }
            set
            {
                _dvprocesssheet = value;
                NotifyPropertyChanged("DVProcessSheet");
            }
        }

        private DataView _dvprocessissue = null;
        public DataView DVProcessIssue
        {
            get { return _dvprocessissue; }
            set
            {
                _dvprocessissue = value;
                NotifyPropertyChanged("DVProcessIssue");
            }
        }

        private DataView _dvprocesscc = null;
        public DataView DVProcessCC
        {
            get { return _dvprocesscc; }
            set
            {
                _dvprocesscc = value;
                NotifyPropertyChanged("DVProcessCC");
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

        private DataView _dvaltrmbasic1 = null;
        public DataView DVAltRMBasic1
        {
            get { return _dvaltrmbasic1; }
            set
            {
                _dvaltrmbasic1 = value;
                NotifyPropertyChanged("DVAltRMBasic1");
            }
        }

        private DataView _dvaltrmbasic2 = null;
        public DataView DVAltRMBasic2
        {
            get { return _dvaltrmbasic2; }
            set
            {
                _dvaltrmbasic2 = value;
                NotifyPropertyChanged("DVAltRMBasic2");
            }
        }

        public DataView AllDVWire { get; set; }

        private DataView _dvwire = null;
        public DataView DVWire
        {
            get { return _dvwire; }
            set
            {
                _dvwire = value;
                NotifyPropertyChanged("DVWire");
            }
        }

        private DataView _dvaltwire1 = null;
        public DataView DVAltWire1
        {
            get { return _dvaltwire1; }
            set
            {
                _dvaltwire1 = value;
                NotifyPropertyChanged("DVAltWire1");
            }
        }

        private DataView _dvaltwire2 = null;
        public DataView DVAltWire2
        {
            get { return _dvaltwire2; }
            set
            {
                _dvaltwire2 = value;
                NotifyPropertyChanged("DVAltWire2");
            }
        }

        private DataView _dvoperationcode = null;
        public DataView DVOperationCode
        {
            get { return _dvoperationcode; }
            set
            {
                _dvoperationcode = value;
                NotifyPropertyChanged("DVOperationCode");
            }
        }

        private DataView _dvtransport = null;
        public DataView DVTransport
        {
            get { return _dvtransport; }
            set
            {
                _dvtransport = value;
                NotifyPropertyChanged("DVTransport");
            }
        }

        private DataView _dvfmearisk = null;
        public DataView DVFMEArisk
        {
            get { return _dvfmearisk; }
            set
            {
                _dvfmearisk = value;
                NotifyPropertyChanged("DVFMEArisk");
            }
        }

        private DataView _dvccmaster = null;
        public DataView DVCCMaster
        {
            get { return _dvccmaster; }
            set
            {
                _dvccmaster = value;
                NotifyPropertyChanged("DVCCMaster");
            }
        }

       
    }
}
