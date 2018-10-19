using System.ComponentModel;
using System.Data;

namespace ProcessDesigner.Model
{
    public class GridProductSearchModel : INotifyPropertyChanged
    {
        /*
                                    <DataGridTextColumn Header="Part No" Binding="{Binding PART_NO}"></DataGridTextColumn>
                            <DataGridTextColumn Header="Description" Binding="{Binding DESCRIPTION}"></DataGridTextColumn>
                            <DataGridTextColumn Header="Forging Cost Centre" Binding="{Binding FORGING_COST_CENTER}"></DataGridTextColumn>
                            <DataGridTextColumn Header="Dia Max" Binding="{Binding DIA_MAX}"></DataGridTextColumn>
                            <DataGridTextColumn Header="Dia Min" Binding="{Binding DIA_MIN}"></DataGridTextColumn>
                            <DataGridTextColumn Header="Quality" Binding="{Binding QUALITY}"></DataGridTextColumn>
        */
        public event PropertyChangedEventHandler PropertyChanged;
        private string _part_no = "";
        public string PART_NO
        {
            get { return _part_no; }
            set
            {
                _part_no = value;
                NotifyPropertyChanged("PART_NO");
            }
        }


        private string _part_desc = "";
        public string PART_DESC
        {
            get { return _part_desc; }
            set
            {
                _part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }

        private string _cc_code = "";
        public string CC_CODE
        {
            get { return _cc_code; }
            set
            {
                _cc_code = value;
                NotifyPropertyChanged("CC_CODE");
            }
        }

        private string _wire_size_max = "";
        public string WIRE_SIZE_MAX
        {
            get { return _wire_size_max; }
            set
            {
                _wire_size_max = value;
                NotifyPropertyChanged("WIRE_SIZE_MAX");
            }
        }

        private string _wire_size_min = "";
        public string WIRE_SIZE_MIN
        {
            get { return _wire_size_min; }
            set
            {
                _wire_size_min = value;
                NotifyPropertyChanged("WIRE_SIZE_MIN");
            }
        }

        private string _quality = "";
        public string QUALITY
        {
            get { return _quality; }
            set
            {
                _quality = value;
                NotifyPropertyChanged("QUALITY");
            }
        }

        private string _cust_dwg_no = "";
        public string CUST_DWG_NO
        {
            get { return _cust_dwg_no; }
            set
            {
                _cust_dwg_no = value;
                NotifyPropertyChanged("CUST_DWG_NO");
            }
        }

        private string _cust_name = "";
        public string CUST_NAME
        {
            get { return _cust_name; }
            set
            {
                _cust_name = value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }


        private DataView _dtProductSearchDetails;
        public DataView ProductSearchDetails
        {
            get { return _dtProductSearchDetails; }
            set
            {
                _dtProductSearchDetails = value;
                NotifyPropertyChanged("ProductSearchDetails");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



    }
}
