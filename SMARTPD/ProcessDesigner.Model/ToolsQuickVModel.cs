using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ToolsQuickVModel : ViewModelBase
    {
        public System.IO.MemoryStream picture { get; set; }
        public bool ImageChanged { get; set; }
        public string FileType { get; set; }
        public string MimeType { get; set; }

        private string _family_cd = "";
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

        private string _tool_cd = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tool code is required.")]
        public string TOOL_CD
        {
            get { return _tool_cd; }
            set
            {
                _tool_cd = value;
                NotifyPropertyChanged("TOOL_CD");
            }
        }

        private DataRowView _selectedFamily;
        public DataRowView SelectedFamily
        {
            get { return this._selectedFamily; }
            set
            {
                this._selectedFamily = value;
                NotifyPropertyChanged("SelectedFamily");
            }
        }

        private DataRowView _toolIssuesSelectedRow;
        public DataRowView ToolIssuesSelectedRow
        {
            get { return this._toolIssuesSelectedRow; }
            set
            {
                this._toolIssuesSelectedRow = value;
                NotifyPropertyChanged("ToolIssuesSelectedRow");
            }
        }

        private DataView dvToolFamily = null;
        public DataView DVToolFamily
        {
            get { return dvToolFamily; }
            set
            {
                dvToolFamily = value;
                NotifyPropertyChanged("DVToolFamily");
            }
        }

        private DataView dvToolParameter = null;
        public DataView DVToolParameter
        {
            get { return dvToolParameter; }
            set
            {
                dvToolParameter = value;
                NotifyPropertyChanged("DVToolParameter");
            }
        }

        private TOOL_DIMENSION toolDimension = null;
        public TOOL_DIMENSION ToolDimension
        {
            get { return toolDimension; }
            set
            {
                toolDimension = value;
                NotifyPropertyChanged("ToolDimension");
            }
        }

        private DataView dvToolIssue = null;
        public DataView DVToolIssue
        {
            get { return dvToolIssue; }
            set
            {
                dvToolIssue = value;
                NotifyPropertyChanged("DVToolIssue");
            }
        }

        private DataView dvRevisionToolIssue = null;
        public DataView DVRevisionToolIssue
        {
            get { return dvRevisionToolIssue; }
            set
            {
                dvRevisionToolIssue = value;
                NotifyPropertyChanged("DVRevisionToolIssue");
            }
        }

        private string _file_Name;
        public string File_Name
        {
            get { return _file_Name; }
            set
            {
                _file_Name = value;
                NotifyPropertyChanged("File_Name");
            }
        }

        private string _displayFile_Name = "";
        public string DisplayFile_Name
        {
            get { return _displayFile_Name; }
            set
            {
                _displayFile_Name = value;
                NotifyPropertyChanged("DisplayFile_Name");
            }
        }

        private bool _showRevisions = false;
        public bool ShowRevisions
        {
            get { return _showRevisions; }
            set
            {
                _showRevisions = value;
                NotifyPropertyChanged("ShowRevisions");
            }
        }
    }
}
