using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Windows.Input;
using ProcessDesigner.BLL;
using System.Windows;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class frmCPGViewModel : ViewModelBase
    {
        private readonly ICommand _onAvailPreviousCommand;
        public ICommand OnAvailPreviousCommand { get { return this._onAvailPreviousCommand; } }
        private readonly ICommand _onAvailNextCommand;
        public ICommand OnAvailNextCommand { get { return this._onAvailNextCommand; } }
        private readonly ICommand _onAvailChkCommand;
        public ICommand OnAvailChkCommand { get { return this._onAvailChkCommand; } }

        private readonly ICommand _onSplPreviousCommand;
        public ICommand OnSplPreviousCommand { get { return this._onSplPreviousCommand; } }
        private readonly ICommand _onSplNextCommand;
        public ICommand OnSplNextCommand { get { return this._onSplNextCommand; } }

        private readonly ICommand _onGrd3PreviousCommand;
        public ICommand OnGrd3PreviousCommand { get { return this._onGrd3PreviousCommand; } }
        private readonly ICommand _onGrd3NextCommand;
        public ICommand OnGrd3NextCommand { get { return this._onGrd3NextCommand; } }

        private readonly ICommand _onGrd4PreviousCommand;
        public ICommand OnGrd4PreviousCommand { get { return this._onGrd4PreviousCommand; } }
        private readonly ICommand _onGrd4NextCommand;
        public ICommand OnGrd4NextCommand { get { return this._onGrd4NextCommand; } }
        private readonly ICommand _onGrd4FinishCommand;
        public ICommand OnGrd4FinishCommand { get { return this._onGrd4FinishCommand; } }

        public Action CloseAction { get; set; }

        private Visibility _visibilityPage1 = Visibility.Visible;
        private Visibility _visibilityPage2 = Visibility.Collapsed;
        private Visibility _visibilityPage3 = Visibility.Collapsed;
        private Visibility _visibilityPage4 = Visibility.Collapsed;
        private Visibility _visibilityPageBtn1 = Visibility.Visible;
        private Visibility _visibilityPageBtn2 = Visibility.Collapsed;
        private Visibility _visibilityPageBtn3 = Visibility.Collapsed;
        private Visibility _visibilityPageBtn4 = Visibility.Collapsed;

        private DataView dgvTempAvail = new DataView();
        private DataView dgvTempSpl = new DataView();
        private DataView dgvTempGrd3Char = new DataView();

        private readonly ICommand _rowEditEndingSplCommand;
        public ICommand RowEditEndingSplCommand { get { return this._rowEditEndingSplCommand; } }

        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }

        public DataRowView SelectedItemSpl { get; set; }
        private CPGModel _cpgModel;
        private CPGBll _cpgbll;
        private DataView dtpccsForm = new DataView();
        private PCCSModel pccsMast = new PCCSModel();
        public frmCPGViewModel(UserInformation userInfo, string partNo, decimal routeNo, decimal seqNo, PCCSModel dtPccsLocal)
        {
            this.selectChangeGrdCommandFeatureDetails = new DelegateCommand<DataRowView>(this.SelectDataRowFeatureDetails);
            this.mouseDoubleClickGrdMeasuringDetails = new DelegateCommand<DataRowView>(this.SelectDataRowMouseDoubleClickMeasuringDetails);
            this.mouseDoubleClickGrdSelectedMeasuringDetails = new DelegateCommand<DataRowView>(this.SelectDataRowMouseDoubleClickSelectedMeasuringDetails);
            this._onAvailPreviousCommand = new DelegateCommand(this.AvailPreviousCommand);
            this._onAvailNextCommand = new DelegateCommand<DataGrid>(this.AvailNextCommand);
            this._onAvailChkCommand = new DelegateCommand<DataGrid>(this.AvailChkCommand);
            this._onSplPreviousCommand = new DelegateCommand<DataGrid>(this.SplPreviousCommand);
            this._onSplNextCommand = new DelegateCommand<DataGrid>(this.SplNextCommand);
            this._onGrd3PreviousCommand = new DelegateCommand<DataGrid>(this.Grd3PreviousCommand);
            this._onGrd3NextCommand = new DelegateCommand<DataGrid>(this.Grd3NextCommand);
            this._onGrd4PreviousCommand = new DelegateCommand<DataGrid>(this.Grd4PreviousCommand);
            this._onGrd4NextCommand = new DelegateCommand(this.Grd4NextCommand);
            this._onGrd4FinishCommand = new DelegateCommand(this.Grd4FinishCommand);
            this._rowEditEndingSplCommand = new DelegateCommand<Object>(this.RowEditEndingSpl);
            this._closeCommand = new DelegateCommand(this.Close);
            _cpgbll = new CPGBll(userInfo);
            _cpgModel = new CPGModel();
            VisibilityPage1 = Visibility.Visible;
            VisibilityPage2 = Visibility.Collapsed;
            VisibilityPage3 = Visibility.Collapsed;
            VisibilityPage4 = Visibility.Collapsed;
            VisibilityPageBtn1 = Visibility.Visible;
            VisibilityPageBtn2 = Visibility.Collapsed;
            VisibilityPageBtn3 = Visibility.Collapsed;
            VisibilityPageBtn4 = Visibility.Collapsed;
            CPGModel.PartNo = partNo;
            CPGModel.RouteNo = routeNo;
            CPGModel.SeqNo = seqNo;
            dtpccsForm = dtPccsLocal.PCCSDetails;
            pccsMast = dtPccsLocal;
            _cpgbll.GetCPGAvailable(CPGModel);

        }

        private void Close()
        {
            try
            {
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
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

        private readonly ICommand selectChangeGrdCommandFeatureDetails;
        public ICommand SelectChangeGrdCommandFeatureDetails { get { return this.selectChangeGrdCommandFeatureDetails; } }
        private void SelectDataRowFeatureDetails(DataRowView selecteditem)
        {
            if (selecteditem.IsNotNullOrEmpty())
            {
                CPGModel.FeatureDesc = selecteditem["FEATURE"].ToString();
                _cpgbll.GetGrid3CharMeasuringTechniquesDetails(CPGModel);
            }

        }
        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
        private readonly ICommand mouseDoubleClickGrdMeasuringDetails;
        public ICommand MouseDoubleClickGrdMeasuringDetails { get { return this.mouseDoubleClickGrdMeasuringDetails; } }
        private void SelectDataRowMouseDoubleClickMeasuringDetails(DataRowView selecteditem)
        {
            if (selecteditem.IsNotNullOrEmpty())
            {
                CPGModel.Grd3SelectedMeasuringTechniquesDetails.RowFilter = "FEATURE='" + selecteditem["FEATURE"] + "'";
                CPGModel.Grd3SelectedMeasuringTechniquesDetails.Table.AcceptChanges();
                if (CPGModel.Grd3SelectedMeasuringTechniquesDetails.Count == 0)
                {
                    if (CPGModel.Grd3SelectedMeasuringTechniquesDetails.Count > 0)
                        selecteditem["SNO"] = CPGModel.Grd3SelectedMeasuringTechniquesDetails.Count + 1;
                    CPGModel.Grd3SelectedMeasuringTechniquesDetails.Table.ImportRow(selecteditem.Row);
                    CPGModel.Grd3MeasuringTechniquesDetails.Table.Rows.Remove(selecteditem.Row);

                }
                else
                {
                    CPGModel.Grd3SelectedMeasuringTechniquesDetails.RowFilter = string.Empty;
                    ShowInformationMessage("Characteristics " + selecteditem["FEATURE"] + " has been already added.");
                }
                CPGModel.Grd3SelectedMeasuringTechniquesDetails.RowFilter = "FEATURE<>''";
                CPGModel.Grd3SelectedMeasuringTechniquesDetails.Table.AcceptChanges();
            }

        }
        private readonly ICommand mouseDoubleClickGrdSelectedMeasuringDetails;
        public ICommand MouseDoubleClickGrdSelectedMeasuringDetails { get { return this.mouseDoubleClickGrdSelectedMeasuringDetails; } }
        private void SelectDataRowMouseDoubleClickSelectedMeasuringDetails(DataRowView selecteditem)
        {

            if (selecteditem.IsNotNullOrEmpty())
            {
                selecteditem.Delete();
            }
        }
        private void RowEditEndingSpl(Object selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    SelectedItemSpl = (DataRowView)selecteditem;
                    if (SelectedItemSpl["FEATURE"].ToString() != "" || SelectedItemSpl["ISR_NO"].ToString() != "" || SelectedItemSpl["SPEC_CHAR"].ToString() != "")
                    {
                        if (CPGModel.SplCharacteristcsDetails[CPGModel.SplCharacteristcsDetails.Count - 1]["FEATURE"].ToString() != "" || CPGModel.SplCharacteristcsDetails[CPGModel.SplCharacteristcsDetails.Count - 1]["ISR_NO"].ToString() != "" || CPGModel.SplCharacteristcsDetails[CPGModel.SplCharacteristcsDetails.Count - 1]["SPEC_CHAR"].ToString() != "")
                        {
                            DataRowView drv = CPGModel.SplCharacteristcsDetails.AddNew();
                            drv.BeginEdit();
                            drv["SNO"] = CPGModel.SplCharacteristcsDetails.Count;
                            drv.EndEdit();
                            NotifyPropertyChanged("CPGModel");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public Visibility VisibilityPage1
        {
            get { return _visibilityPage1; }
            set
            {
                this._visibilityPage1 = value;
                NotifyPropertyChanged("VisibilityPage1");

            }
        }

        public Visibility VisibilityPage2
        {
            get { return _visibilityPage2; }
            set
            {
                this._visibilityPage2 = value;
                NotifyPropertyChanged("VisibilityPage2");

            }
        }
        public Visibility VisibilityPage3
        {
            get { return _visibilityPage3; }
            set
            {
                this._visibilityPage3 = value;
                NotifyPropertyChanged("VisibilityPage3");

            }
        }
        public Visibility VisibilityPage4
        {
            get { return _visibilityPage4; }
            set
            {
                this._visibilityPage4 = value;
                NotifyPropertyChanged("VisibilityPage4");

            }
        }

        public Visibility VisibilityPageBtn1
        {
            get { return _visibilityPageBtn1; }
            set
            {
                this._visibilityPageBtn1 = value;
                NotifyPropertyChanged("VisibilityPageBtn1");

            }
        }

        public Visibility VisibilityPageBtn2
        {
            get { return _visibilityPageBtn2; }
            set
            {
                this._visibilityPageBtn2 = value;
                NotifyPropertyChanged("VisibilityPageBtn2");

            }
        }
        public Visibility VisibilityPageBtn3
        {
            get { return _visibilityPageBtn3; }
            set
            {
                this._visibilityPageBtn3 = value;
                NotifyPropertyChanged("VisibilityPageBtn3");

            }
        }
        public Visibility VisibilityPageBtn4
        {
            get { return _visibilityPageBtn4; }
            set
            {
                this._visibilityPageBtn4 = value;
                NotifyPropertyChanged("VisibilityPageBtn4");

            }
        }

        public CPGModel CPGModel
        {
            get
            {
                return _cpgModel;
            }

            set
            {
                _cpgModel = value;
                NotifyPropertyChanged("CPGModel");
            }
        }

        private void AvailChkCommand(DataGrid dgvAvail)
        {
            if (dgvAvail.ItemsSource.IsNotNullOrEmpty())
            {
                if (CPGModel.SelectAll == true)
                    dgvAvail.SelectAll();
                else
                {
                    dgvAvail.UnselectAll();
                }

            }
        }

        private void AvailNextCommand(DataGrid dgvAvail)
        {
            DataTable dvAvailSelected = new DataTable();
            VisibilityPage1 = Visibility.Collapsed;
            VisibilityPage2 = Visibility.Visible;
            VisibilityPage3 = Visibility.Collapsed;
            VisibilityPage4 = Visibility.Collapsed;
            VisibilityPageBtn1 = Visibility.Collapsed;
            VisibilityPageBtn2 = Visibility.Visible;
            VisibilityPageBtn3 = Visibility.Collapsed;
            VisibilityPageBtn4 = Visibility.Collapsed;
            _cpgbll.GetPccsExistingRecords(CPGModel);
            _cpgbll.GetPccsComboValues(CPGModel);
            if (dgvAvail.ItemsSource.IsNotNullOrEmpty())
            {
                if (dgvAvail.SelectedItems.Count > 0)
                    foreach (DataRowView selectedData in dgvAvail.SelectedItems)
                    {
                        DataRow drow = selectedData.Row;
                        CPGModel.SplCharacteristcsDetails.Table.AcceptChanges();
                        CPGModel.SplCharacteristcsDetails.RowFilter = "FEATURE='" + drow["FEATURE_DESC"].ToString() + "'";
                        if (CPGModel.SplCharacteristcsDetails.Count == 0)
                        {
                            CPGModel.SplCharacteristcsDetails.RowFilter = string.Empty;
                            DataRowView drv = CPGModel.SplCharacteristcsDetails.AddNew();
                            drv.BeginEdit();
                            drv["SNO"] = CPGModel.SplCharacteristcsDetails.Count;
                            drv["FEATURE"] = drow["FEATURE_DESC"];
                            drv["Sample_Size"] = drow["Sample_Size"];
                            drv["Reaction_plan"] = drow["Reaction_plan"];
                            drv["Control_method"] = drow["Control_method"];
                            drv.EndEdit();
                        }
                        CPGModel.SplCharacteristcsDetails.RowFilter = string.Empty;
                        //Insert Duplicate
                        if (dgvTempSpl.IsNotNullOrEmpty())
                        {
                            if (dgvTempSpl.Count > 0)
                            {
                                dgvTempSpl.Table.AcceptChanges();
                                dgvTempSpl.RowFilter = "FEATURE='" + drow["FEATURE_DESC"].ToString() + "'";
                                if (dgvTempSpl.Count == 0)
                                {
                                    dgvTempSpl.RowFilter = string.Empty;
                                    DataRowView drv = dgvTempSpl.AddNew();
                                    drv.BeginEdit();
                                    drv["SNO"] = dgvTempSpl.Count;
                                    drv["FEATURE"] = drow["FEATURE_DESC"];
                                    drv["Sample_Size"] = drow["Sample_Size"];
                                    drv["Reaction_plan"] = drow["Reaction_plan"];
                                    drv["Control_method"] = drow["Control_method"];
                                    drv.EndEdit();
                                }
                                dgvTempSpl.RowFilter = string.Empty;
                            }
                        }


                    }
            }
            if (!dgvTempAvail.IsNotNullOrEmpty() && dgvAvail.ItemsSource.IsNotNullOrEmpty())
            {
                dgvTempAvail = ((DataView)dgvAvail.ItemsSource).Table.Copy().DefaultView;
            }
            else if (dgvTempAvail.IsNotNullOrEmpty() && dgvAvail.ItemsSource.IsNotNullOrEmpty())
            {
                if (dgvTempAvail.Count > 0)
                    dgvAvail.ItemsSource = dgvTempAvail;
            }
            if (dgvTempSpl.IsNotNullOrEmpty())
            {
                if (dgvTempSpl.Count > 0)
                {
                    CPGModel.SplCharacteristcsDetails = dgvTempSpl;
                }
            }
            if (CPGModel.SplCharacteristcsDetails.Count > 0)
            {
                if (CPGModel.SplCharacteristcsDetails[CPGModel.SplCharacteristcsDetails.Count - 1]["FEATURE"].ToString() != "")
                {
                    DataRowView drv = CPGModel.SplCharacteristcsDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = CPGModel.SplCharacteristcsDetails.Count;
                    drv.EndEdit();
                }

            }
        }


        private void AvailPreviousCommand()
        {
            // throw new NotImplementedException();
        }

        private void SplPreviousCommand(DataGrid dgvSpl)
        {

            try
            {
                dgvTempSpl = ((DataView)dgvSpl.ItemsSource).Table.Copy().DefaultView;
                //AvailNextCommand(dgvAvail);
                VisibilityPage1 = Visibility.Visible;
                VisibilityPage2 = Visibility.Collapsed;
                VisibilityPage3 = Visibility.Collapsed;
                VisibilityPage4 = Visibility.Collapsed;
                VisibilityPageBtn1 = Visibility.Visible;
                VisibilityPageBtn2 = Visibility.Collapsed;
                VisibilityPageBtn3 = Visibility.Collapsed;
                VisibilityPageBtn4 = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void SplNextCommand(DataGrid dgvSpl)
        {
            ((DataView)dgvSpl.ItemsSource).Table.AcceptChanges();
            CPGModel.Grd3CharacteristcsDetails = ((DataView)dgvSpl.ItemsSource).Table.DefaultView;
            CPGModel.Grd3CharacteristcsDetails.RowFilter = "Feature<>''";
            if (CPGModel.Grd3CharacteristcsDetails.Count > 0)
                dgvTempGrd3Char = CPGModel.Grd3CharacteristcsDetails;
            if (CPGModel.PccsDetails.IsNotNullOrEmpty() && CPGModel.PccsDetails.Count > 0)
            {
                CPGModel.Grd3SelectedMeasuringTechniquesDetails = dtpccsForm.Table.Copy().DefaultView;
                CPGModel.Grd3SelectedMeasuringTechniquesDetails.RowFilter = "Feature<>''";
            }

            VisibilityPage1 = Visibility.Collapsed;
            VisibilityPage2 = Visibility.Collapsed;
            VisibilityPage3 = Visibility.Visible;
            VisibilityPage4 = Visibility.Collapsed;
            VisibilityPageBtn1 = Visibility.Collapsed;
            VisibilityPageBtn2 = Visibility.Collapsed;
            VisibilityPageBtn3 = Visibility.Visible;
            VisibilityPageBtn4 = Visibility.Collapsed;
        }
        private void Grd3PreviousCommand(DataGrid dgvSpl)
        {
            VisibilityPage1 = Visibility.Collapsed;
            VisibilityPage2 = Visibility.Visible;
            VisibilityPage3 = Visibility.Collapsed;
            VisibilityPage4 = Visibility.Collapsed;
            VisibilityPageBtn1 = Visibility.Collapsed;
            VisibilityPageBtn2 = Visibility.Visible;
            VisibilityPageBtn3 = Visibility.Collapsed;
            VisibilityPageBtn4 = Visibility.Collapsed;
        }
        private void Grd3NextCommand(DataGrid dgv3Selected)
        {
            if ((DataView)dgv3Selected.ItemsSource != null)
            {
                CPGModel.Grd4CharacteristcsDetails = ((DataView)dgv3Selected.ItemsSource).Table.DefaultView;
                if (CPGModel.Grd4CharacteristcsDetails.Count > 0)
                    for (int i = 0; i < CPGModel.Grd4CharacteristcsDetails.Count; i++)
                    {
                        CPGModel.Grd4CharacteristcsDetails[i]["SNO"] = i + 1;
                    }

            }

            VisibilityPage1 = Visibility.Collapsed;
            VisibilityPage2 = Visibility.Collapsed;
            VisibilityPage3 = Visibility.Collapsed;
            VisibilityPage4 = Visibility.Visible;
            VisibilityPageBtn1 = Visibility.Collapsed;
            VisibilityPageBtn2 = Visibility.Collapsed;
            VisibilityPageBtn3 = Visibility.Collapsed;
            VisibilityPageBtn4 = Visibility.Visible;

        }
        private void Grd4PreviousCommand(DataGrid dgv3Selected)
        {
            VisibilityPage1 = Visibility.Collapsed;
            VisibilityPage2 = Visibility.Collapsed;
            VisibilityPage3 = Visibility.Visible;
            VisibilityPage4 = Visibility.Collapsed;
            VisibilityPageBtn1 = Visibility.Collapsed;
            VisibilityPageBtn2 = Visibility.Collapsed;
            VisibilityPageBtn3 = Visibility.Visible;
            VisibilityPageBtn4 = Visibility.Collapsed;
        }
        private void Grd4NextCommand()
        {
            // throw new NotImplementedException();
        }
        private void Grd4FinishCommand()
        {
            pccsMast.PCCSDetails.Table.AcceptChanges();
            //for (int i = 0; i < pccsMast.PCCSDetails.Count; i++)
            //{
            //    pccsMast.PCCSDetails.Delete(0);
            //}
            foreach (DataRowView row in pccsMast.PCCSDetails)
            {
                row.Delete();
            }
            foreach (DataRowView row in CPGModel.Grd4CharacteristcsDetails)
            {

                pccsMast.PCCSDetails.Table.ImportRow(row.Row);
            }
            if (pccsMast.PCCSDetails[pccsMast.PCCSDetails.Count - 1]["FEATURE"].ToString() != "")
            {
                DataRowView drv = pccsMast.PCCSDetails.AddNew();
                drv.BeginEdit();
                drv["SNO"] = pccsMast.PCCSDetails.Count;
                drv.EndEdit();
            }
            ReNumber();

            CloseAction();
        }
        private void ReNumber()
        {
            DataView dvPccs;
            //dvPccs = new DataView(PccsModel.PCCSDetails.ToTable(), "SNO is not null", "SNO Asc", DataViewRowState.CurrentRows);
            pccsMast.PCCSDetails.Table.AcceptChanges();
            dvPccs = pccsMast.PCCSDetails.Table.Copy().DefaultView;

            if (dvPccs.IsNotNullOrEmpty())
            {
                if (dvPccs.Table.Columns.IndexOf("SnoSort") < 0)
                    dvPccs.Table.Columns.Add("SnoSort", typeof(decimal));
                dvPccs.Table.AcceptChanges();
                for (int i = 0; i < dvPccs.Count; i++)
                {
                    dvPccs[i]["SnoSort"] = dvPccs[i]["SNO"];
                }
                dvPccs.Sort = "SnoSort ASC";
                int j = 0;
                foreach (DataRowView item in dvPccs)
                {
                    item["SNO"] = j + 1;
                    item["SnoSort"] = item["SNO"];
                    j++;
                }
                dvPccs.Sort = "SnoSort ASC";
                dvPccs.Table.AcceptChanges();
                pccsMast.PCCSDetails = dvPccs;

            }
        }
    }
}
