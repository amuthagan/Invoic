using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace ProcessDesigner.Model
{
    public class RoleUserSecurityModel : SEC_USER_MASTER, INotifyPropertyChanged
    {
        public DataView Users { get; set; }
        public DataView Roles { get; set; }
        private DataRowView _selectedRole;
        private DataRowView _selectedUser;

        /// <summary>
        /// Gets or sets the selected role.
        /// </summary>
        /// <value>The selected role.</value>
        public DataRowView SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                _selectedRole = value;
                NotifyPropertyChanged("SelectedRole");
            }
        }

        /// <summary>
        /// Gets or sets the selected user
        /// </summary>
        public DataRowView SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged("SelectedUser");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = null;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
