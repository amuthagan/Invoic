using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System.Data;
using System.Windows;
using ProcessDesigner.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Drawing;
using System.IO;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class ECNPCNViewModel : ViewModelBase
    {
        UserInformation _userInformation;
        WPF.MDI.MdiChild _mdiChild;
        ECNPCNBll _eCNPCNBll;


        public ECNPCNViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string ecnOrPcn, Nullable<DateTime> startdate, Nullable<DateTime> enddate)
        {
            try
            {
                ECNOrPCN = ecnOrPcn;
                _userInformation = userInformation;
                _mdiChild = mdiChild;
                this.refreshCommand = new DelegateCommand(this.Refresh);
                _eCNPCNBll = new ECNPCNBll(userInformation);
                StartDate = startdate;
                EndDate = enddate;
                Refresh();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private Nullable<DateTime> _startDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required")]
        public Nullable<DateTime> StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }

        private Nullable<DateTime> _endDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is required")]
        public Nullable<DateTime> EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }

        private string _eCNOrPCN;
        public string ECNOrPCN
        {
            get
            {
                return _eCNOrPCN;
            }
            set
            {
                _eCNOrPCN = value;
                NotifyPropertyChanged("ECNOrPCN");
            }
        }

        private DataView _eCNPCNResult;
        public DataView ECNPCNResult
        {
            get
            {
                return _eCNPCNResult;
            }
            set
            {
                _eCNPCNResult = value;
                NotifyPropertyChanged("ECNPCNResult");
            }
        }

        private string _headerDetails;
        public string HeaderDetails
        {
            get { return _headerDetails; }
            set
            {
                _headerDetails = value;
                NotifyPropertyChanged("HeaderDetails");
            }
        }


        private readonly ICommand refreshCommand;
        public ICommand RefreshCommand { get { return this.refreshCommand; } }
        private void Refresh()
        {
            string startDate;
            string endDate;
            Int64 cnt = 0;
            try
            {
                if (StartDate.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Start Date"));
                    return;
                }

                if (StartDate.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("End Date"));
                    return;
                }

                if (StartDate > EndDate)
                {
                    ShowInformationMessage("Start Date is Greater than End Date,Please Check it.!");
                    return;
                }
                startDate = Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy");
                endDate = Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy");

                if (ECNOrPCN == "PCN")
                {
                    ECNPCNResult = _eCNPCNBll.GetPCNDetails(startDate, endDate);
                }
                else
                {
                    ECNPCNResult = _eCNPCNBll.GetECNDetails(startDate, endDate);
                }
                cnt = ECNPCNResult.Count;
                HeaderDetails = ECNOrPCN + " Details - " + cnt.ToString() + (cnt > 0 ? " Entries" : " Entry") + " found ";
                NotifyPropertyChanged("HeaderDetails");

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

    }
}
