using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ToolScheduleModel : ViewModelBase
    {

        private string _partNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part No is required")]
        public string PartNo
        {
            get
            {
                return _partNo.ToUpper();
            }
            set
            {
                _partNo = value.ToUpper();
                NotifyPropertyChanged("PartNo");
            }
        }

        private string _routeNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Route No is required")]
        public string RouteNo
        {
            get
            {
                return _routeNo;
            }
            set
            {
                _routeNo = value;
                NotifyPropertyChanged("RouteNo");
            }
        }

        private string _seqNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Seq No is required")]
        public string SeqNo
        {
            get
            {
                return _seqNo;
            }
            set
            {
                _seqNo = value;
                NotifyPropertyChanged("SeqNo");
            }
        }

        private string _cCSno;
        [Required(AllowEmptyStrings = false, ErrorMessage = "CC No is required")]
        public string CCSno
        {
            get
            {
                return _cCSno;
            }
            set
            {
                _cCSno = value;
                NotifyPropertyChanged("CCSno");
            }
        }

        private string _cCCode;
        public string CCCode
        {
            get
            {
                return _cCCode;
            }
            set
            {
                _cCCode = value;
                NotifyPropertyChanged("CCCode");
            }
        }


        private string _subHeadingNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sub Heading is required")]
        public string SubHeadingNo
        {
            get
            {
                return _subHeadingNo;
            }
            set
            {
                _subHeadingNo = value;
                NotifyPropertyChanged("SubHeadingNo");
            }
        }



        private string _partDescription;
        public string PartDescription
        {
            get
            {
                return _partDescription;
            }
            set
            {
                _partDescription = value;
            }
        }

        private string _topNote;
        public string TopNote
        {
            get
            {
                return _topNote;
            }
            set
            {
                _topNote = value;
                NotifyPropertyChanged("TopNote");
            }
        }

        private string _botNote;
        public string BotNote
        {
            get
            {
                return _botNote;
            }
            set
            {
                _botNote = value;
                NotifyPropertyChanged("BotNote");
            }
        }

        private string _subHeading = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sub Heading is required")]
        public string SubHeading
        {
            get
            {
                return _subHeading;
            }
            set
            {
                _subHeading = value;
            }
        }

        //new by nandhini
        private System.Guid _rOWID;
        public System.Guid ROWID
        {
            get
            {
                return _rOWID;
            }
            set
            {
                _rOWID = value;
                NotifyPropertyChanged("ROWID");
            }
        }
        //end new
        /*
        PartNoCombo - PART_NO
ProcessNoCombo - ROUTE_NO
SequenceNoCombo - SEQ_NO
CostCentreCombo - CC_SNO
SubHeadingCombo - SUB_HEADING_NO
             */
    }
}
