using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Data;
using System.Windows.Input;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    class PermissionViewModel : ViewModelBase
    {
        bool bHeaderClicked = false;
        bool bChange = false;
        private bool _closed = false;
        bool execCheckAll = true;
        private SEC_ROLE_OBJECT_PERMISSION _secRoleObjectPermission;
        private PermissionDet _permissionDet;
        //private DataTable _permission;
        private List<SEC_ROLE_OBJECT_PERMISSION> _permission;
        private string _roleName;
        private ICommand _onSaveCommand;
        private ICommand _onCancelCommand;
        private ICommand _onCheckChangeCommandModify;
        private ICommand _onCheckChangeCommandDelete;
        private ICommand _onCheckChangeCommandView;
        private ICommand _onCheckChangeCommandPrint;
        private ICommand _onCheckChangeCommandAdd;
        private ICommand _onCheckChangeCommandShow;


        private ICommand _onCheckChangeCommandModifyAll;
        private ICommand _onUnCheckChangeCommandModifyAll;

        private ICommand _onCheckChangeCommandAddAll;
        private ICommand _onUnCheckChangeCommandAddAll;

        private ICommand _onCheckChangeCommandViewAll;
        private ICommand _onUnCheckChangeCommandViewAll;

        private ICommand _onCheckChangeCommandDeleteAll;
        private ICommand _onUnCheckChangeCommandDeleteAll;

        private ICommand _onCheckChangeCommandPrintAll;
        private ICommand _onUnCheckChangeCommandPrintAll;

        private ICommand _onCheckChangeCommandShowAll;
        private ICommand _onUnCheckChangeCommandShowAll;







        private SEC_ROLE_OBJECT_PERMISSION _selectedRow;

        private Boolean _checkedShow;
        private Boolean _checkedAdd;
        private Boolean _checkedModify;
        private Boolean _checkedView;
        private Boolean _checkedDelete;
        private Boolean _checkedPrint;

        private System.Windows.Controls.CheckBox chkSelectAllShow;
        private System.Windows.Controls.CheckBox chkSelectAllAdd;
        private System.Windows.Controls.CheckBox chkSelectAllModify;
        private System.Windows.Controls.CheckBox chkSelectAllView;
        private System.Windows.Controls.CheckBox chkSelectAllDelete;
        private System.Windows.Controls.CheckBox chkSelectAllPrint;

        private System.Windows.Controls.CheckBox chkAll;





        public PermissionViewModel(UserInformation userInformation, string roleName)
        {
            _secRoleObjectPermission = new SEC_ROLE_OBJECT_PERMISSION();
            _permissionDet = new PermissionDet(userInformation);
            Permission = _permissionDet.GetRolePermissionList(roleName);
            _roleName = roleName;

            foreach (SEC_ROLE_OBJECT_PERMISSION perm in Permission)
            {
                if (!_permissionDet.IsPrintDisable(perm.OBJECT_NAME))
                {
                    perm.PERM_PRINT = false;
                    perm.PERM_VIEW = false;
                }
            }

            //var checkedcnt = Permission.Where(perm => perm.PERM_PRINT == true);
            //var cnt = Permission.Where(perm => _permissionDet.IsPrintDisable(perm.OBJECT_NAME));
            //chkSelectAllPrint.IsChecked = (checkedcnt.Count() == cnt.Count() ? true : false);


        }

        public Action CloseAction { get; set; }

        public SEC_ROLE_OBJECT_PERMISSION SecRoleObjectPermission
        {
            get
            {
                return _secRoleObjectPermission;
            }
            set
            {
                _secRoleObjectPermission = value;
                NotifyPropertyChanged("SecRoleObjectPermission");
            }
        }

        public List<SEC_ROLE_OBJECT_PERMISSION> Permission
        {
            get
            {
                return _permission;
            }
            set
            {
                _permission = value;
                NotifyPropertyChanged("Permission");
            }
        }

        public string Role_Name
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;
                NotifyPropertyChanged("Role_Name");
            }
        }

        public SEC_ROLE_OBJECT_PERMISSION SelectedRow
        {
            get
            {
                return _selectedRow;
            }
            set
            {
                _selectedRow = value;
                CheckSelectAll();
                NotifyPropertyChanged("SelectedRow");
            }
        }

        public Boolean CheckedShow
        {
            get
            {
                return _checkedShow;
            }
            set
            {
                _checkedShow = value;
                CheckSelectAll();
                NotifyPropertyChanged("CheckedShow");
            }
        }

        public Boolean CheckedAdd
        {
            get
            {
                return _checkedAdd;
            }
            set
            {
                _checkedAdd = value;
                CheckSelectAll();
                NotifyPropertyChanged("CheckedAdd");
            }
        }

        public Boolean CheckedModify
        {
            get
            {
                return _checkedModify;
            }
            set
            {
                _checkedModify = value;
                CheckSelectAll();
                NotifyPropertyChanged("CheckedModify");
            }
        }

        public Boolean CheckedView
        {
            get
            {
                return _checkedView;
            }
            set
            {
                _checkedView = value;
                CheckSelectAll();
                NotifyPropertyChanged("CheckedView");
            }
        }

        public Boolean CheckedDelete
        {
            get
            {
                return _checkedDelete;
            }
            set
            {
                _checkedDelete = value;
                CheckSelectAll();
                NotifyPropertyChanged("CheckedDelete");
            }
        }

        public Boolean CheckedPrint
        {
            get
            {
                return _checkedPrint;
            }
            set
            {
                _checkedPrint = value;
                CheckSelectAll();
                NotifyPropertyChanged("CheckedPrint");
            }
        }

        /// <summary>
        /// This is a relay command to save the rights for the selected role
        /// </summary>
        public ICommand OnSaveCommand
        {
            get
            {
                if (_onSaveCommand == null)
                {
                    _onSaveCommand = new RelayCommand(param => this.SavePermission(), null);
                }
                return _onSaveCommand;
            }
        }

        /// <summary>
        /// Relay command for modify check box
        /// </summary>
        public ICommand OnCheckChangeCommandModify
        {
            get
            {
                if (_onCheckChangeCommandModify == null)
                {
                    _onCheckChangeCommandModify = new RelayCommand(param => this.ModifyChecked(), null);
                }
                return _onCheckChangeCommandModify;
            }
        }

        /// <summary>
        /// Relay command for delete check box
        /// </summary>
        public ICommand OnCheckChangeCommandDelete
        {
            get
            {
                if (_onCheckChangeCommandDelete == null)
                {
                    _onCheckChangeCommandDelete = new RelayCommand(param => this.DeleteChecked(), null);
                }
                return _onCheckChangeCommandDelete;
            }
        }

        /// <summary>
        /// relay command for view check box
        /// </summary>
        public ICommand OnCheckChangeCommandView
        {
            get
            {
                if (_onCheckChangeCommandView == null)
                {
                    _onCheckChangeCommandView = new RelayCommand(param => this.ViewChecked(), null);
                }
                return _onCheckChangeCommandView;
            }
        }

        /// <summary>
        /// relay command for print check box
        /// </summary>
        public ICommand OnCheckChangeCommandPrint
        {
            get
            {
                if (_onCheckChangeCommandPrint == null)
                {
                    _onCheckChangeCommandPrint = new RelayCommand(param => this.PrintChecked(), null);
                }
                return _onCheckChangeCommandPrint;
            }
        }

        /// <summary>
        /// relay command for print check box
        /// </summary>
        public ICommand OnCheckChangeCommandAddAll
        {
            get
            {
                if (_onCheckChangeCommandAddAll == null)
                {
                    _onCheckChangeCommandAddAll = new RelayCommand(param => this.SelectAll(true, "ADD"), null);
                }
                return _onCheckChangeCommandAddAll;
            }
        }

        /// <summary>
        /// relay command for print check box
        /// </summary>
        public ICommand OnUnCheckChangeCommandAddAll
        {
            get
            {
                if (_onUnCheckChangeCommandAddAll == null)
                {
                    _onUnCheckChangeCommandAddAll = new RelayCommand(param => this.SelectAll(false, "ADD"), null);
                }
                return _onUnCheckChangeCommandAddAll;
            }
        }


        public ICommand OnCheckChangeCommandViewAll
        {
            get
            {
                if (_onCheckChangeCommandViewAll == null)
                {
                    _onCheckChangeCommandViewAll = new RelayCommand(param => this.SelectAll(true, "VIEW"), null);
                }
                return _onCheckChangeCommandViewAll;
            }
        }

        public ICommand OnUnCheckChangeCommandViewAll
        {
            get
            {
                if (_onUnCheckChangeCommandViewAll == null)
                {
                    _onUnCheckChangeCommandViewAll = new RelayCommand(param => this.SelectAll(false, "VIEW"), null);
                }
                return _onUnCheckChangeCommandViewAll;
            }
        }

        public ICommand OnCheckChangeCommandDeleteAll
        {
            get
            {
                if (_onCheckChangeCommandDeleteAll == null)
                {
                    _onCheckChangeCommandDeleteAll = new RelayCommand(param => this.SelectAll(true, "DELETE"), null);
                }
                return _onCheckChangeCommandDeleteAll;
            }
        }

        public ICommand OnUnCheckChangeCommandDeleteAll
        {
            get
            {
                if (_onUnCheckChangeCommandDeleteAll == null)
                {
                    _onUnCheckChangeCommandDeleteAll = new RelayCommand(param => this.SelectAll(false, "DELETE"), null);
                }
                return _onUnCheckChangeCommandDeleteAll;
            }
        }

        public ICommand OnCheckChangeCommandModifyAll
        {
            get
            {
                if (_onCheckChangeCommandModifyAll == null)
                {
                    _onCheckChangeCommandModifyAll = new RelayCommand(param => this.SelectAll(true, "EDIT"), null);
                }
                return _onCheckChangeCommandModifyAll;
            }
        }

        public ICommand OnUnCheckChangeCommandModifyAll
        {
            get
            {
                if (_onUnCheckChangeCommandModifyAll == null)
                {
                    _onUnCheckChangeCommandModifyAll = new RelayCommand(param => this.SelectAll(false, "EDIT"), null);
                }
                return _onUnCheckChangeCommandModifyAll;
            }
        }

        public ICommand OnCheckChangeCommandPrintAll
        {
            get
            {
                if (_onCheckChangeCommandPrintAll == null)
                {
                    _onCheckChangeCommandPrintAll = new RelayCommand(param => this.SelectAll(true, "PRINT"), null);
                }
                return _onCheckChangeCommandPrintAll;
            }
        }

        public ICommand OnUnCheckChangeCommandPrintAll
        {
            get
            {
                if (_onUnCheckChangeCommandPrintAll == null)
                {
                    _onUnCheckChangeCommandPrintAll = new RelayCommand(param => this.SelectAll(false, "PRINT"), null);
                }
                return _onUnCheckChangeCommandPrintAll;
            }
        }

        public ICommand OnCheckChangeCommandShowAll
        {
            get
            {
                if (_onCheckChangeCommandShowAll == null)
                {
                    _onCheckChangeCommandShowAll = new RelayCommand(param => this.SelectAll(true, "SHOW"), null);
                }
                return _onCheckChangeCommandShowAll;
            }
        }

        public ICommand OnUnCheckChangeCommandShowAll
        {
            get
            {
                if (_onUnCheckChangeCommandShowAll == null)
                {
                    _onUnCheckChangeCommandShowAll = new RelayCommand(param => this.SelectAll(false, "SHOW"), null);
                }
                return _onUnCheckChangeCommandShowAll;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OnCheckChangeCommandAdd
        {
            get
            {
                if (_onCheckChangeCommandAdd == null)
                {
                    _onCheckChangeCommandAdd = new RelayCommand(param => this.CheckAddClicked(), null);
                }
                return _onCheckChangeCommandAdd;
            }
        }

        public ICommand OnCheckChangeCommandShow
        {
            get
            {
                if (_onCheckChangeCommandShow == null)
                {
                    _onCheckChangeCommandShow = new RelayCommand(param => this.CheckShowClicked(), null);
                }
                return _onCheckChangeCommandShow;
            }
        }


        public ICommand OnCancelCommand
        {
            get
            {
                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _onCancelCommand;
            }
        }


        public void ModifyChecked()
        {
            try
            {
                if (SelectedRow != null)
                {
                    CheckModifyClicked();
                    SelectedRow.PERM_DELETE = SelectedRow.PERM_EDIT;
                    SelectedRow.PERM_VIEW = SelectedRow.PERM_EDIT;

                    NotifyPropertyChanged("SelectedRow");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void DeleteChecked()
        {
            try
            {
                if (SelectedRow != null)
                {
                    CheckDeleteClicked();
                    SelectedRow.PERM_EDIT = SelectedRow.PERM_DELETE;
                    NotifyPropertyChanged("SelectedRow");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void ViewChecked()
        {
            try
            {
                if (SelectedRow != null)
                {
                    CheckViewClicked();
                    SelectedRow.PERM_PRINT = SelectedRow.PERM_VIEW;
                    NotifyPropertyChanged("SelectedRow");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void PrintChecked()
        {
            try
            {
                if (SelectedRow != null)
                {
                    CheckPrintClicked();
                    if (!_permissionDet.IsPrintDisable(SelectedRow.OBJECT_NAME))
                    {
                        SelectedRow.PERM_PRINT = false;
                    }
                    SelectedRow.PERM_VIEW = SelectedRow.PERM_PRINT;

                    NotifyPropertyChanged("SelectedRow");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// Save the permission for each role
        /// </summary>

        private void SavePermission()
        {
            try
            {
                if (_permissionDet.SavePermission(Permission, Role_Name))
                {
                    System.Windows.Forms.MessageBox.Show("All Permission have been saved successfully!", "SmartPD", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    if (_closed == false)
                    {
                        _closed = true;
                        CloseAction();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Cancel()
        {
            try
            {
                _closed = false;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    _closed = true;
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void CheckSelectAll()
        {
            try
            {
                execCheckAll = false;
                if (_selectedRow != null)
                {
                    if (_selectedRow.PERM_ADD == true && _selectedRow.PERM_DELETE == true && _selectedRow.PERM_EDIT == true && _selectedRow.PERM_PRINT == true && _selectedRow.PERM_SHOW == true && _selectedRow.PERM_VIEW == true)
                    {
                        chkAll.IsChecked = true;
                    }
                    else
                    {
                        chkAll.IsChecked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                execCheckAll = true;
                throw ex.LogException();
            }
            execCheckAll = true;
        }

        private void SelectAll(bool check, string colname)
        {
            if (bChange == true) return;
            try
            {
                bHeaderClicked = true;
                foreach (SEC_ROLE_OBJECT_PERMISSION perm in Permission)
                {
                    if (colname == "EDIT" || colname == "DELETE")
                    {
                        perm.PERM_DELETE = check;
                        perm.PERM_EDIT = check;
                        perm.PERM_VIEW = check;
                        perm.PERM_PRINT = check;
                        if (colname == "EDIT")
                        {
                            if (!_permissionDet.IsPrintDisable(perm.OBJECT_NAME))
                            {
                                perm.PERM_PRINT = false;
                            }
                            perm.PERM_VIEW = perm.PERM_PRINT;
                        }

                    }
                    else if (colname == "VIEW" || colname == "PRINT")
                    {
                        perm.PERM_VIEW = check;
                        perm.PERM_PRINT = check;
                        CheckedShow = check;
                        CheckedPrint = check;
                        if (!_permissionDet.IsPrintDisable(perm.OBJECT_NAME))
                        {
                            perm.PERM_PRINT = false;
                        }
                        perm.PERM_VIEW = perm.PERM_PRINT;
                    }
                    else if (colname == "ADD")
                    {
                        perm.PERM_ADD = check;
                    }
                    else if (colname == "SHOW")
                    {
                        perm.PERM_SHOW = check;
                    }
                }
                if (colname == "EDIT" || colname == "DELETE")
                {
                    chkSelectAllDelete.IsChecked = check;
                    chkSelectAllModify.IsChecked = check;
                    chkSelectAllView.IsChecked = check;
                    chkSelectAllPrint.IsChecked = check;
                }
                else if (colname == "VIEW" || colname == "PRINT")
                {
                    chkSelectAllView.IsChecked = check;
                    chkSelectAllPrint.IsChecked = check;
                }

                NotifyPropertyChanged("Permission");
            }
            catch (Exception ex)
            {
                bHeaderClicked = false;
                throw ex.LogException();
            }
            bHeaderClicked = false;
        }

        public void LoadMethod(object sender, EventArgs e)
        {
            try
            {
                //_lstRoles = (System.Windows.Controls.ListBox)((System.Windows.Window)sender).FindName("lstRoles");
                //_lstUserRoles = (System.Windows.Controls.ListBox)((System.Windows.Window)sender).FindName("lstUserRoles");
                //SelectListItem();
                System.Windows.Controls.DataGrid dgview = null;

                dgview = ((System.Windows.Controls.DataGrid)((System.Windows.Window)sender).FindName("grdPermission"));

                chkAll = ((System.Windows.Controls.CheckBox)((System.Windows.Window)sender).FindName("chkAll"));

                if (dgview != null)
                {
                    chkSelectAllShow = (System.Windows.Controls.CheckBox)dgview.FindName("chkSelectAllShow");
                    chkSelectAllAdd = (System.Windows.Controls.CheckBox)dgview.FindName("chkSelectAllAdd");
                    chkSelectAllView = (System.Windows.Controls.CheckBox)dgview.FindName("chkSelectAllView");
                    chkSelectAllDelete = (System.Windows.Controls.CheckBox)dgview.FindName("chkSelectAllDelete");
                    chkSelectAllPrint = (System.Windows.Controls.CheckBox)dgview.FindName("chkSelectAllPrint");
                    chkSelectAllModify = (System.Windows.Controls.CheckBox)dgview.FindName("chkSelectAllModify");
                    if (dgview.Items.Count > 0)
                    {
                        dgview.SelectedIndex = 0;
                    }
                }
                HeaderCheck();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CheckAll(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (SelectedRow != null)
                {
                    if (execCheckAll == true)
                    {
                        SelectedRow.PERM_ADD = ((System.Windows.Controls.CheckBox)sender).IsChecked;
                        SelectedRow.PERM_DELETE = SelectedRow.PERM_ADD;
                        SelectedRow.PERM_EDIT = SelectedRow.PERM_ADD;
                        SelectedRow.PERM_PRINT = SelectedRow.PERM_ADD;
                        SelectedRow.PERM_VIEW = SelectedRow.PERM_ADD;
                        SelectedRow.PERM_SHOW = SelectedRow.PERM_ADD;
                        if (!_permissionDet.IsPrintDisable(SelectedRow.OBJECT_NAME))
                        {
                            SelectedRow.PERM_PRINT = false;
                        }
                        SelectedRow.PERM_VIEW = SelectedRow.PERM_PRINT;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void HeaderCheck()
        {
            var cntperm = Permission.Where(perm => perm.PERM_ADD == true);
            if (cntperm.Count() == Permission.Count())
            {
                chkSelectAllAdd.IsChecked = true;
            }

            cntperm = Permission.Where(perm => perm.PERM_DELETE == true);
            if (cntperm.Count() == Permission.Count())
            {
                chkSelectAllDelete.IsChecked = true;
            }

            cntperm = Permission.Where(perm => perm.PERM_EDIT == true);
            if (cntperm.Count() == Permission.Count())
            {
                chkSelectAllModify.IsChecked = true;
            }

            cntperm = Permission.Where(perm => perm.PERM_PRINT == true);
            var checkedcnt = Permission.Where(perm => perm.PERM_PRINT == true);
            var cnt = Permission.Where(perm => _permissionDet.IsPrintDisable(perm.OBJECT_NAME));
            chkSelectAllPrint.IsChecked = (checkedcnt.Count() == cnt.Count() ? true : false);
            //if (cntperm.Count() == Permission.Count())
            //{
            //    chkSelectAllPrint.IsChecked = true;
            //}

            cntperm = Permission.Where(perm => perm.PERM_SHOW == true);
            if (cntperm.Count() == Permission.Count())
            {
                chkSelectAllShow.IsChecked = true;
            }

            cntperm = Permission.Where(perm => perm.PERM_VIEW == true);
            checkedcnt = Permission.Where(perm => perm.PERM_PRINT == true);
            cnt = Permission.Where(perm => _permissionDet.IsPrintDisable(perm.OBJECT_NAME));
            chkSelectAllView.IsChecked = (checkedcnt.Count() == cnt.Count() ? true : false);
            //if (cntperm.Count() == Permission.Count())
            //{
            //    chkSelectAllView.IsChecked = true;
            //}
        }


        private void CheckAddClicked()
        {
            bChange = true;
            try
            {
                if (bHeaderClicked == true)
                {
                    bChange = false;
                    CheckSelectAll();
                    return;
                }
                if (SelectedRow != null)
                {
                    var cntperm = Permission.Where(perm => perm.PERM_ADD == true);
                    if (cntperm.Count() == Permission.Count())
                    {
                        chkSelectAllAdd.IsChecked = true;
                    }
                    else
                    {
                        chkSelectAllAdd.IsChecked = false;
                    }
                    CheckSelectAll();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            bChange = false;
        }


        private void CheckShowClicked()
        {
            bChange = true;
            try
            {
                if (bHeaderClicked == true)
                {
                    bChange = false;
                    CheckSelectAll();
                    return;
                }
                if (SelectedRow != null)
                {
                    var cntperm = Permission.Where(perm => perm.PERM_SHOW == true);
                    chkSelectAllShow.IsChecked = (cntperm.Count() == Permission.Count() ? true : false);
                    CheckSelectAll();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            bChange = false;
        }

        private void CheckModifyClicked()
        {
            bChange = true;
            try
            {
                if (bHeaderClicked == true)
                {
                    bChange = false;
                    CheckSelectAll();
                    return;
                }
                if (SelectedRow != null)
                {
                    var cntperm = Permission.Where(perm => perm.PERM_EDIT == true);
                    chkSelectAllModify.IsChecked = (cntperm.Count() == Permission.Count() ? true : false);
                    CheckSelectAll();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            bChange = false;
        }

        private void CheckViewClicked()
        {
            bChange = true;
            try
            {
                if (bHeaderClicked == true)
                {
                    bChange = false;
                    CheckSelectAll();
                    return;
                }
                if (SelectedRow != null)
                {
                    var checkedcnt = Permission.Where(perm => perm.PERM_VIEW == true);
                    var cnt = Permission.Where(perm => _permissionDet.IsPrintDisable(perm.OBJECT_NAME));
                    chkSelectAllView.IsChecked = (checkedcnt.Count() == cnt.Count() ? true : false);
                    CheckSelectAll();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            bChange = false;
        }

        private void CheckDeleteClicked()
        {
            bChange = true;
            try
            {
                if (bHeaderClicked == true)
                {
                    bChange = false;
                    CheckSelectAll();
                    return;
                }
                if (SelectedRow != null)
                {
                    var cntperm = Permission.Where(perm => perm.PERM_DELETE == true);
                    chkSelectAllDelete.IsChecked = (cntperm.Count() == Permission.Count() ? true : false);
                    CheckSelectAll();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            bChange = false;
        }

        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!_closed)
                {

                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void CheckPrintClicked()
        {
            bChange = true;
            try
            {
                if (bHeaderClicked == true)
                {
                    bChange = false;
                    CheckSelectAll();
                    return;
                }
                if (SelectedRow != null)
                {
                    var checkedcnt = Permission.Where(perm => perm.PERM_PRINT == true);
                    var cnt = Permission.Where(perm => _permissionDet.IsPrintDisable(perm.OBJECT_NAME));
                    chkSelectAllPrint.IsChecked = (checkedcnt.Count() == cnt.Count() ? true : false);
                    CheckSelectAll();

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            bChange = false;
        }
    }
}
