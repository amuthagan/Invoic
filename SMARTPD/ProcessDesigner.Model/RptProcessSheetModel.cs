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
    public class RptProcessSheetModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string PART_NO
        { get; set; }

        public string PART_DESC
        { get; set; }

        public string cust_name
        { get; set; }

        public string cust_dwg_no
        { get; set; }

        public string QUALITY
        { get; set; }

        public string WIRE_ROD_CD
        { get; set; }

        public string wire_size_max
        { get; set; }

        public string wire_size_min
        { get; set; }

        public string CHEESE_WT
        { get; set; }

        public string FINISH_WT
        { get; set; }

        public string seq_no
        { get; set; }

        public string OPN_DESC
        { get; set; }

        public string OPN_CD
        { get; set; }

        public string CC_CODE
        { get; set; }

        public string SETUPTIME
        { get; set; }

        public string OUTPUT
        { get; set; }

        public string EFFY
        { get; set; }

        public string ISSUE_NO
        { get; set; }

        public string ISSUE_DATE
        { get; set; }

        public string ISSUE_ALTER
        { get; set; }

        public string COMPILED_BY
        { get; set; }

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


    }
}
