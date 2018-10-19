using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
    public class CPGModel : ViewModelBase
    {
        private string _partNo = "";
        private string _featureDesc = "";
        private decimal _seqNo;
        private decimal _routeNo;
        private decimal? _operCode;
        private string _operDesc = "";
        private DataView _dtCmbSplChar;
        private bool? _selectAll = false;
        private DataView _dtAvailableCharacteristcsDetails;
        private DataView _dtSplCharacteristcsDetails;
        private DataView _dtGrd3CharacteristcsDetails;
        private DataView _dtGrd3MeasuringTechniquesDetails;
        private DataView _dtGrd3SelectedMeasuringTechniquesDetails;
        private DataView _dtGrd4CharacteristcsDetails;
        private DataView _dtPccsDetails;

        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }

        public string FeatureDesc
        {
            get { return _featureDesc; }
            set
            {
                _featureDesc = value;
                NotifyPropertyChanged("FeatureDesc");
            }
        }

        public decimal SeqNo
        {
            get { return _seqNo; }
            set
            {
                _seqNo = value;
                NotifyPropertyChanged("SeqNo");
            }
        }
        public DataView SplChar
        {
            get { return _dtCmbSplChar; }
            set
            {
                _dtCmbSplChar = value;
                NotifyPropertyChanged("SplChar");
            }
        }
        public decimal RouteNo
        {
            get { return _routeNo; }
            set
            {
                _routeNo = value;
                NotifyPropertyChanged("RouteNo");
            }
        }

        public decimal? OperCode
        {
            get { return _operCode; }
            set
            {
                _operCode = value;
                NotifyPropertyChanged("OperCode");
            }
        }
        public string OperDesc
        {
            get { return _operDesc; }
            set
            {
                _operDesc = value;
                NotifyPropertyChanged("OperDesc");
            }
        }

        public bool? SelectAll
        {
            get { return _selectAll; }
            set
            {
                _selectAll = value;
                NotifyPropertyChanged("SelectAll");
            }
        }
        public DataView Grd4CharacteristcsDetails
        {
            get { return _dtGrd4CharacteristcsDetails; }
            set
            {
                _dtGrd4CharacteristcsDetails = value;
                NotifyPropertyChanged("Grd4CharacteristcsDetails");
            }
        }

        public DataView PccsDetails
        {
            get { return _dtPccsDetails; }
            set
            {
                _dtPccsDetails = value;
                NotifyPropertyChanged("PccsDetails");
            }
        }

        public DataView Grd3SelectedMeasuringTechniquesDetails
        {
            get { return _dtGrd3SelectedMeasuringTechniquesDetails; }
            set
            {
                _dtGrd3SelectedMeasuringTechniquesDetails = value;
                NotifyPropertyChanged("Grd3SelectedMeasuringTechniquesDetails");
            }
        }

        public DataView Grd3MeasuringTechniquesDetails
        {
            get { return _dtGrd3MeasuringTechniquesDetails; }
            set
            {
                _dtGrd3MeasuringTechniquesDetails = value;
                NotifyPropertyChanged("Grd3MeasuringTechniquesDetails");
            }
        }

        public DataView Grd3CharacteristcsDetails
        {
            get { return _dtGrd3CharacteristcsDetails; }
            set
            {
                _dtGrd3CharacteristcsDetails = value;
                NotifyPropertyChanged("Grd3CharacteristcsDetails");
            }
        }

        public DataView SplCharacteristcsDetails
        {
            get { return _dtSplCharacteristcsDetails; }
            set
            {
                _dtSplCharacteristcsDetails = value;
                NotifyPropertyChanged("SplCharacteristcsDetails");
            }
        }

        public DataView AvailableCharacteristcsDetails
        {
            get { return _dtAvailableCharacteristcsDetails; }
            set
            {
                _dtAvailableCharacteristcsDetails = value;
                NotifyPropertyChanged("AvailableCharacteristcsDetails");
            }
        }
    }
}
