using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProcessDesigner.Model
{
    public class RptDimensionalModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DataView _rPDModelSearchDetails;
        public DataView RPDModelSearchDetails
        {
            get { return _rPDModelSearchDetails; }
            set
            {
                _rPDModelSearchDetails = value;
                NotifyPropertyChanged("RPDModelSearchDetails");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string PART_NO
        { get; set; }

        public string ISR_NO
        { get; set; }

        public string SPEC_MIN
        { get; set; }

        public string SPEC_MAX
        { get; set; }

        public string FEATURE
        { get; set; }

        public string EX_NO
        { get; set; }

        public string REVISION_NO
        { get; set; }

        public string CUST_STD_DATE
        { get; set; }

        public string CUST_DWG_NO
        { get; set; }

        public string PROD_DESC
        { get; set; }

        public string CUST_DWG_NO_ISSUE
        { get; set; }

        public string ROUTE_NO
        { get; set; }

        public string CUST_CODE
        { get; set; }

        public string CUST_NAME
        { get; set; }

        public string FR_CS_DATE
        { get; set; }

        public string LOCATION
        { get; set; }

        public string ISBLANK
        { get; set; }
    }
}
