/* File Name : DrawingModel.cs
 * Description : Created for Drawing variables.
 * Created By  : Ezhil Maran
 * Created Date: 09/03/2016
 * Modified By  :
 * Modified Date:
 * Modified History :
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace ProcessDesigner.Model
{
    public class DrawingModel1 : ViewModelBase
    {
        private string _partNo;
        private string _partdesc;
        private decimal _dwgType;
        private string _dwgTypeCd;
        private string _dwgTypeDesc;
        private decimal _pageNo;
        private string _prddwg;
        private decimal _issueNo;
        private DateTime _issueDate;
        private string _strIssueDate;
        private string _issueAlter;
        private MemoryStream _photo;
        private bool _photoChanged;
        public DataRowView SelectedItem { get; set; }
        private DataView _dVType;
        private string _compiledBy;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Part No should not be empty")]
        public string PART_NO
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        public string PART_DESC
        {
            get { return _partdesc; }
            set
            {
                _partdesc = value;
                NotifyPropertyChanged("PART_DESC");
            }

        }

        public string DWG_TYPE_CD
        {
            get { return _dwgTypeCd; }
            set
            {
                _dwgTypeCd = value;
                NotifyPropertyChanged("DWG_TYPE_CD");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Drawing Type should not be empty")]
        public string DWG_TYPE_DESC
        {
            get { return _dwgTypeDesc; }
            set
            {
                _dwgTypeDesc = value;
                NotifyPropertyChanged("DWG_TYPE_DESC");
            }
        }

        public decimal DWG_TYPE
        {
            get { return _dwgType; }
            set
            {
                _dwgType = value;
                NotifyPropertyChanged("DWG_TYPE");
            }
        }

        public decimal PAGE_NO
        {
            get { return _pageNo; }
            set
            {
                _pageNo = value;
                NotifyPropertyChanged("PAGE_NO");
            }
        }

        public string PRD_DWG
        {
            get { return _prddwg; }
            set
            {
                _prddwg = value;
                NotifyPropertyChanged("PRD_DWG");
            }
        }

        public Decimal ISSUE_NO
        {
            get { return _issueNo; }
            set
            {
                _issueNo = value;
                NotifyPropertyChanged("ISSUE_NO");
            }
        }


        public DateTime ISSUE_DATE
        {
            get
            {
                return _issueDate;
            }
            set
            {
                _issueDate = value;
                NotifyPropertyChanged("ISSUE_DATE");
            }
        }

        public string StrISSUE_DATE
        {
            get
            {
                if (_issueDate != null)
                {
                    _strIssueDate = Convert.ToString(Convert.ToDateTime(_issueDate.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy"));
                }
                return _strIssueDate;
            }
            set
            {
                _strIssueDate = value;
                NotifyPropertyChanged("StrISSUE_DATE");
            }
        }

        public string ISSUE_ALTER
        {
            get { return _issueAlter; }
            set
            {
                _issueAlter = value;
                NotifyPropertyChanged("ISSUE_ALTER");
            }
        }

        public MemoryStream Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                NotifyPropertyChanged("Photo");
            }
        }

        public bool PhotoChanged
        {
            get { return _photoChanged; }
            set
            {
                _photoChanged = value;
                NotifyPropertyChanged("PhotoChanged");
            }
        }


        private byte[] _prodDrwImage;

        public byte[] PROD_DRW_IMAGE
        {
            get { return _prodDrwImage; }
            set
            {
                _prodDrwImage = value;
                NotifyPropertyChanged("PROD_DRW_IMAGE");
            }
        }

        private byte[] _imageByte;

        public byte[] ImageByte
        {
            get { return _imageByte; }
            set
            {
                _imageByte = value;
                NotifyPropertyChanged("ImageByte");
            }
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }
        private string _mode;
        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        public DataView DVType
        {
            get { return _dVType; }
            set
            {
                _dVType = value;
                NotifyPropertyChanged("DVType");
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string COMPILED_BY
        {
            get { return _compiledBy; }
            set
            {
                _compiledBy = value;
            }
        }
        private string _locCode = "";
        public string LOC_CODE
        {
            get { return _locCode; }
            set
            {
                _locCode = value;
                NotifyPropertyChanged("LOC_CODE");
            }
        }


    }


}
