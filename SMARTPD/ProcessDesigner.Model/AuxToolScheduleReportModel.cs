using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class AuxToolScheduleReportModel
    {
        private string _pART_NO;
        public string PART_NO
        {
            get
            {
                return _pART_NO;
            }
            set
            {
                _pART_NO = value;
            }
        }

        private string _cC_CODE;
        public string CC_CODE
        {
            get
            {
                return _cC_CODE;
            }
            set
            {
                _cC_CODE = value;
            }
        }

        private string _tOOL_CODE;
        public string TOOL_CODE
        {
            get
            {
                return _tOOL_CODE;
            }
            set
            {
                _tOOL_CODE = value;
            }
        }

        private string _tOOL_DESC;
        public string TOOL_DESC
        {
            get
            {
                return _tOOL_DESC;
            }
            set
            {
                _tOOL_DESC = value;
            }
        }

        private string _mADE_FOR;
        public string MADE_FOR
        {
            get
            {
                return _mADE_FOR;
            }
            set
            {
                _mADE_FOR = value;
            }
        }

        private string _tEMPLATE_CD;
        public string TEMPLATE_CD
        {
            get
            {
                return _tEMPLATE_CD;
            }
            set
            {
                _tEMPLATE_CD = value;
            }
        }

        private string _iSSUE_NO;
        public string ISSUE_NO
        {
            get
            {
                return _iSSUE_NO;
            }
            set
            {
                _iSSUE_NO = value;
            }
        }

        private string _aLTER;
        public string ALTER
        {
            get
            {
                return _aLTER;
            }
            set
            {
                _aLTER = value;
            }
        }

        private string _iSSUE_DATE;
        public string ISSUE_DATE
        {
            get
            {
                return _iSSUE_DATE;
            }
            set
            {
                _iSSUE_DATE = value;
            }
        }

        private string _iNITIAL;
        public string INITIAL
        {
            get
            {
                return _iNITIAL;
            }
            set
            {
                _iNITIAL = value;
            }
        }

        private string _cOMPILEDBY;
        public string COMPILEDBY
        {
            get
            {
                return _cOMPILEDBY;
            }
            set
            {
                _cOMPILEDBY = value;
            }
        }

        private string _aPPROVEDBY;
        public string APPROVEDBY
        {
            get
            {
                return _aPPROVEDBY;
            }
            set
            {
                _aPPROVEDBY = value;
            }
        }

    }
}
