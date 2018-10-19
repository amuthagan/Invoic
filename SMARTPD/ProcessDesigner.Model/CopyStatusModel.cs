using System.ComponentModel;
using System.Data;

namespace ProcessDesigner.Model
{
    public class CopyStatusModel : INotifyPropertyChanged
    {
        private DataView _dtCopyStatusModelMaster;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _part_no;
        public string PART_NO
        {
            get
            {
                return _part_no;
            }
            set
            {
                _part_no = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        public DataView CopyStatusModelMasterDetails
        {
            get { return _dtCopyStatusModelMaster; }
            set
            {
                _dtCopyStatusModelMaster = value;
                NotifyPropertyChanged("CopyStatusModelMasterDetails");
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
