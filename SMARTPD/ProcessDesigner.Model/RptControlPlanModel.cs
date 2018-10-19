using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class RptControlPlanModel
    {
        private DataView _rptControlPlan;
        public DataView RptControlPlan
        {
            get { return _rptControlPlan; }
            set
            {
                _rptControlPlan = value;
            }
        }
        private string _partNo = "";
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
            }
        }

        private string _processNo = "";
        public string ProcessNo
        {
            get { return _processNo; }
            set
            {
                _processNo = value;

            }
        }

        private string _controlPlanType = "";
        public string ControlPlanType
        {
            get { return _controlPlanType; }
            set
            {
                _controlPlanType = value;

            }
        }

        private string _controlPlanNo = "";
        public string ControlPlanNo
        {
            get { return _controlPlanNo; }
            set
            {
                _controlPlanNo = value;
            }
        }

        private string _fax = "";
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;

            }
        }
        private string _phone = "";
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;

            }
        }

        private string _ctm1 = "";
        public string Ctm1
        {
            get { return _ctm1; }
            set
            {
                _ctm1 = value;

            }
        }
        private string _ctm2 = "";
        public string Ctm2
        {
            get { return _ctm2; }
            set
            {
                _ctm2 = value;

            }
        }
        private string _ctm3 = "";
        public string Ctm3
        {
            get { return _ctm3; }
            set
            {
                _ctm3 = value;

            }
        }
        private string _ctm4 = "";
        public string Ctm4
        {
            get { return _ctm4; }
            set
            {
                _ctm4 = value;

            }
        }
        private string _ctm5 = "";
        public string Ctm5
        {
            get { return _ctm5; }
            set
            {
                _ctm5 = value;

            }
        }
        private string _ctm6 = "";
        public string Ctm6
        {
            get { return _ctm6; }
            set
            {
                _ctm6 = value;

            }
        }
        private string _ctm7 = "";
        public string Ctm7
        {
            get { return _ctm7; }
            set
            {
                _ctm7 = value;

            }
        }

        private string _supplierPartNo = "";
        public string SupplierPartNo
        {
            get { return _supplierPartNo; }
            set
            {
                _supplierPartNo = value;

            }
        }

        private string _customerPartNo = "";
        public string CustomerPartNo
        {
            get { return _customerPartNo; }
            set
            {
                _customerPartNo = value;

            }
        }
        private string _partNameDesc = "";
        public string PartNameDesc
        {
            get { return _partNameDesc; }
            set
            {
                _partNameDesc = value;

            }
        }

        private string _supplierCode = "";
        public string SupplierCode
        {
            get { return _supplierCode; }
            set
            {
                _supplierCode = value;

            }
        }

        private string _supplierLocation = "";
        public string SupplierLocation
        {
            get { return _supplierLocation; }
            set
            {
                _supplierLocation = value;

            }
        }


        private string _preparedBy = "";
        public string PreparedBy
        {
            get { return _preparedBy; }
            set
            {
                _preparedBy = value;

            }
        }

        private string _approvedBy = "";
        public string ApprovedBy
        {
            get { return _approvedBy; }
            set
            {
                _approvedBy = value;

            }
        }

        private string _dateOrg;
        public string DateOrg
        {
            get { return _dateOrg; }
            set
            {
                _dateOrg = value;

            }
        }
        private string _dateRev;
        public string DateRev
        {
            get { return _dateRev; }
            set
            {
                _dateRev = value;

            }
        }

        private string _supplierApprDate;
        public string SupplierApprDate
        {
            get { return _supplierApprDate; }
            set
            {
                _supplierApprDate = value;

            }
        }
        private string _otherApprDate;
        public string OtherApprDate
        {
            get { return _otherApprDate; }
            set
            {
                _otherApprDate = value;

            }
        }

        private string _contactPerson = "";
        public string ContactPerson
        {
            get { return _contactPerson; }
            set
            {
                _contactPerson = value;

            }
        }

        private string _customerEnggApprov = "";
        public string CustomerEnggApprov
        {
            get { return _customerEnggApprov; }
            set
            {
                _customerEnggApprov = value;

            }
        }
        private string _customerQualityApprov = "";
        public string CustomerQualityApprov
        {
            get { return _customerQualityApprov; }
            set
            {
                _customerQualityApprov = value;

            }
        }
        private string _otherApprovalcustomer = "";
        public string OtherApprovalcustomer
        {
            get { return _otherApprovalcustomer; }
            set
            {
                _otherApprovalcustomer = value;

            }
        }
        /// <summary>
        /// /DataTable
        /// </summary>
        private string _partProcessNo = "";
        public string PartProcessNo
        {
            get { return _partProcessNo; }
            set
            {
                _partProcessNo = value;

            }
        }
        private string _processNameOperationDesc = "";
        public string ProcessNameOperationDesc
        {
            get { return _processNameOperationDesc; }
            set
            {
                _processNameOperationDesc = value;

            }
        }
        private string _machineDevice = "";
        public string MachineDevice
        {
            get { return _machineDevice; }
            set
            {
                _machineDevice = value;

            }
        }

        private string _isrNo = "";
        public string IsrNo
        {
            get { return _isrNo; }
            set
            {
                _isrNo = value;

            }
        }
        private string _product = "";
        public string Product
        {
            get { return _product; }
            set
            {
                _product = value;

            }
        }
        private string _process = "";
        public string Process
        {
            get { return _process; }
            set
            {
                _process = value;

            }
        }

        private string _splChar = "";
        public string SplChar
        {
            get { return _splChar; }
            set
            {
                _splChar = value;

            }
        }

        private string _processSpec = "";
        public string ProcessSpec
        {
            get { return _processSpec; }
            set
            {
                _processSpec = value;

            }
        }

        private string _gauges_Used = "";
        public string Gauges_Used
        {
            get { return _gauges_Used; }
            set
            {
                _gauges_Used = value;

            }
        }

        private string _sampleSize = "";
        public string SampleSize
        {
            get { return _sampleSize; }
            set
            {
                _sampleSize = value;

            }
        }
        private string _sampleFreq = "";
        public string SampleFreq
        {
            get { return _sampleFreq; }
            set
            {
                _sampleFreq = value;

            }
        }

        private string _controlMethod = "";
        public string ControlMethod
        {
            get { return _controlMethod; }
            set
            {
                _controlMethod = value;

            }
        }
        private string _reactionPlan = "";
        public string ReactionPlan
        {
            get { return _reactionPlan; }
            set
            {
                _reactionPlan = value;

            }
        }

    }

}
