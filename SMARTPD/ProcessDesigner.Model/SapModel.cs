using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class SapModel : ViewModelBase
    {

        private string _partNo = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number is Required")]
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }

        private string _operCode = "";
        public string OperCode
        {
            get { return _operCode; }
            set
            {
                _operCode = value;
                NotifyPropertyChanged("OperCode");
            }
        }
        private string _partDesc = "";
        public string PartDesc
        {
            get { return _partDesc; }
            set
            {
                _partDesc = value;
                NotifyPropertyChanged("PartDesc");
            }
        }
        private string _rohNo = "";
        public string RohNo
        {
            get { return _rohNo; }
            set
            {
                _rohNo = value;
                NotifyPropertyChanged("RohNo");
            }
        }

        private string _noOfOperations = "";
        public string NoOfoperations
        {
            get { return _noOfOperations; }
            set
            {
                _noOfOperations = value;
                NotifyPropertyChanged("NoOfoperations");
            }
        }

        private string _fileName = "";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        private DataView _dtPartnoDetails;
        public DataView PartnoDetails
        {
            get { return _dtPartnoDetails; }
            set
            {
                _dtPartnoDetails = value;
                NotifyPropertyChanged("PartnoDetails");
            }
        }
        private DataView _dtRohDetails;
        public DataView RohDetails
        {
            get { return _dtRohDetails; }
            set
            {
                _dtRohDetails = value;
                NotifyPropertyChanged("RohDetails");
            }
        }

        private DataView _dtHalbDetails;
        public DataView HalbDetails
        {
            get { return _dtHalbDetails; }
            set
            {
                _dtHalbDetails = value;
                NotifyPropertyChanged("HalbDetails");
            }
        }

        private DataView _dtFertMDetails;
        public DataView FertMDetails
        {
            get { return _dtFertMDetails; }
            set
            {
                _dtFertMDetails = value;
                NotifyPropertyChanged("FertMDetails");
            }
        }
        private DataView _dtFertKDetails;
        public DataView FertKDetails
        {
            get { return _dtFertKDetails; }
            set
            {
                _dtFertKDetails = value;
                NotifyPropertyChanged("FertKDetails");
            }
        }
        private DataView _dtFertYDetails;
        public DataView FertYDetails
        {
            get { return _dtFertYDetails; }
            set
            {
                _dtFertYDetails = value;
                NotifyPropertyChanged("FertYDetails");
            }
        }

        private DataView _dtProcessSheetDetails;
        public DataView ProcessSheetDetails
        {
            get { return _dtProcessSheetDetails; }
            set
            {
                _dtProcessSheetDetails = value;
                NotifyPropertyChanged("ProcessSheetDetails");
            }
        }
        private DataView _dtBomDetails;
        public DataView BomDetails
        {
            get { return _dtBomDetails; }
            set
            {
                _dtBomDetails = value;
                NotifyPropertyChanged("BomDetails");
            }
        }

        private DataView _dtRoutingDetails;
        public DataView RoutingDetails
        {
            get { return _dtRoutingDetails; }
            set
            {
                _dtRoutingDetails = value;
                NotifyPropertyChanged("RoutingDetails");
            }
        }

        private DataView _dtProductionVersionDetails;
        public DataView ProductionVersionDetails
        {
            get { return _dtProductionVersionDetails; }
            set
            {
                _dtProductionVersionDetails = value;
                NotifyPropertyChanged("ProductionVersionDetails");
            }
        }
        private int _bomFinishPosition = 0;
        public int FinishPosition
        {
            get { return _bomFinishPosition; }
            set
            {
                _bomFinishPosition = value;
                NotifyPropertyChanged("FinishPosition");
            }
        }
        private int _bomNoOfOperations = 0;
        public int NoOfOperations
        {
            get { return _bomNoOfOperations; }
            set
            {
                _bomNoOfOperations = value;
                NotifyPropertyChanged("NoOfOperations");
            }
        }
        private int _bomWireSize = 0;
        public int WireSize
        {
            get { return _bomWireSize; }
            set
            {
                _bomWireSize = value;
                NotifyPropertyChanged("WireSize");
            }
        }
        private string _bomLocation = "";
        public string Location
        {
            get { return _bomLocation; }
            set
            {
                _bomLocation = value;
                NotifyPropertyChanged("Location");
            }
        }
        private decimal? _bomPlant = 0;
        public decimal? Plant
        {
            get { return _bomPlant; }
            set
            {
                _bomPlant = value;
                NotifyPropertyChanged("Plant");
            }
        }

    }
}
