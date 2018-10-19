using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
   public class ToolAdminModel : ViewModelBase
    {
        private string _family_cd = "";
        private string _family_name = "";
        private string _displayFile_Name = "";
        private DataView dvToolFamily = null;
        private DataView dvToolParameter = null;
        private DataView dvPicture = null;
        private DataRowView _selectedFamily;
        private DataRowView _selectedParameter;  
        private string _status = "";
        private OperationMode _mode = OperationMode.None;
        private DataTable dtDeletedRecords = null;
        private string _file_Name;
        public System.IO.MemoryStream picture { get; set; }
        public bool ImageChanged { get; set; }
        public string FileType { get; set; }

        public string MimeType { get; set; }

       [Required(AllowEmptyStrings = false, ErrorMessage = "Family code is required.")]
        public string FAMILY_CD
        {
            get { return _family_cd; }
            set
            {
                _family_cd = value;
                NotifyPropertyChanged("FAMILY_CD");
            }
        }

       [Required(AllowEmptyStrings = false, ErrorMessage = "Family name is required.")]
        public string FAMILY_NAME
        {
            get { return _family_name; }
            set
            {
                _family_name = value;
                NotifyPropertyChanged("FAMILY_NAME");
            }
        }


        public DataRowView SelectedFamily
        {
            get { return this._selectedFamily; }
            set
            {
                this._selectedFamily = value;
                NotifyPropertyChanged("SelectedFamily");
            }
        }

        public DataRowView SelectedParameter
        {
            get { return this._selectedParameter; }
            set
            {
                this._selectedParameter = value;
                NotifyPropertyChanged("SelectedParameter");
            }
        }

        public DataView DVToolFamily
        {
            get { return dvToolFamily; }
            set
            {
                dvToolFamily = value;
                NotifyPropertyChanged("DVToolFamily");
            }
        }

        public DataView DVToolParameter
        {
            get { return dvToolParameter; }
            set
            {
                dvToolParameter = value;
                NotifyPropertyChanged("DVToolParameter");
            }
        }

        public DataView DVPicture
        {
            get { return dvPicture; }
            set
            {
                dvPicture = value;
                NotifyPropertyChanged("DVPicture");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public OperationMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        public string File_Name
        {
            get { return _file_Name; }
            set
            {
                _file_Name = value;
                NotifyPropertyChanged("File_Name");
            }
        }

        public string DisplayFile_Name
        {
            get { return _displayFile_Name; }
            set
            {
                _displayFile_Name = value;
                NotifyPropertyChanged("DisplayFile_Name");
            }
        }

        public DataTable DTDeletedRecords
        {
            get { return dtDeletedRecords; }
            set
            {
                dtDeletedRecords = value;
                NotifyPropertyChanged("DTDeletedRecords");
            }
        }


    }
}
