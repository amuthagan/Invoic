using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProcessDesigner.BLL;
using WPF.MDI;
using ProcessDesigner.Common;
using System.Net;
using System.Data;
using ProcessDesigner.UserControls;
using ProcessDesigner.Model;
using System.Collections.ObjectModel;


namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private UserInformation _userInformation;
        private string _frmName;
        string projectName;
        private ProcessDesigner.frmProductCategoryMaster productCategoryMaster;
        ProcessDesigner.frmThreadDetails threadDetails;
        private ActiveUsersBLL bll;
        private FeaturesBLL fbll = null;
        private SpecialCharacteristicsBll scbll = null;
        private ProcessDesigner.frmForm2 auditreport;
        public MainWindow(UserInformation userInformation)
        {
            InitializeComponent();
            _userInformation = userInformation;
            this.Title = "SmartPD " + " 03/Oct/2018" + " Production V5.4/D&D/SFL";
            projectName = "SmartPD - ";
            //_userInformation.StatusBarDetails = this.stMain;
            MainMDI.Container = Container;
            fbll = new FeaturesBLL(userInformation);
            scbll = new SpecialCharacteristicsBll(userInformation);
            MainMDI.Container.Children.CollectionChanged += Children_CollectionChanged;
        }

        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<MdiChild> children = sender as ObservableCollection<MdiChild>;

            if (children.Count == 1)
            {
                bool isexistsep = false;
                foreach (var item in miWindows.Items)
                {
                    Separator sep = item as Separator;
                    if (sep != null)
                    {
                        isexistsep = true;
                        break;
                    }
                }
                if (!isexistsep)
                {
                    Separator spr = new Separator();
                    spr.Name = "spr";
                    miWindows.Items.Add(spr);
                }
            }
            else if (children.Count == 0)
            {
                foreach (var item in miWindows.Items)
                {
                    Separator sep = item as Separator;
                    if (sep != null)
                    {
                        miWindows.Items.Remove(sep);                       
                        break;
                    }
                }
            }


            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = children[e.NewStartingIndex].Title.Replace(projectName, "");
                        menuItem.Tag = children[e.NewStartingIndex];
                        children[e.NewStartingIndex].GotFocus += Child_GotFocus;
                        menuItem.IsCheckable = true;
                        menuItem.Click += menuItem_Click;
                        miWindows.Items.Add(menuItem);
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var item in miWindows.Items)
                        {
                            MenuItem miitem = item as MenuItem;
                            if (miitem != null && miitem.Tag.IsNotNullOrEmpty() && (MdiChild)miitem.Tag == (MdiChild)e.OldItems[0])
                            {
                                miWindows.Items.Remove(miitem);
                                StatusMessage.setStatus("Ready");
                                break;
                            }
                        }
                        break;
                    }
            }
        }

        void Child_GotFocus(object sender, RoutedEventArgs e)
        {
            foreach (var item in miWindows.Items)
            {
                MenuItem miitem = item as MenuItem;
                if (miitem != null && miitem.Tag.IsNotNullOrEmpty())
                {
                    if ((MdiChild)miitem.Tag == (MdiChild)sender && ((MdiChild)sender).Focused)
                    {
                        miitem.IsChecked = true;
                    }
                    else
                    {
                        miitem.IsChecked = false;
                    }
                }
            }
        }


        void menuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            MdiChild child = menuItem.Tag as MdiChild;
            child.Focus();
        }


        public bool IsFormMinimized(string formtitle)
        {
            try
            {
                int containerCount = 0;
                string containertitle = string.Empty;
                containerCount = Container.Children.Count;
                if (containerCount == 0) return false;
                for (int i = 0; i < containerCount; i++)
                {
                    containertitle = Container.Children[i].Title.ToString().ToUpper();
                    int retval = 0;
                    retval = (containertitle.IndexOf(formtitle.ToString().ToUpper(), 0));
                    if (retval > 0)
                    {
                        if (Container.Children[i].WindowState == WindowState.Minimized)
                        {
                            Container.Children[i].WindowState = WindowState.Normal;
                            return true;
                        }
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            //StatusMessage.setStatus(_userInformation, "Ready", DateTime.Now.ToString());

            MenuItem mi = sender as MenuItem;

            //if (mi.Name != "miExit" || mi.Name != "miTileHorizontally" || mi.Name != "miTileVertically" || mi.Name != "miTileCascade") Progress.Start();
            switch (mi.Name)
            {
                case "miExit":
                case "miTileHorizontally":
                case "miTileVertically":
                case "miTileCascade":
                    break;
                default:
                    Progress.Start();
                    break;
            }
            switch (mi.Name)
            {
                case "miPendingPartStatus":

                    MdiChild mdiChildPendingPartStatus = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Pending Parts Status Report"
                    };

                    ProcessDesigner.frmPendingPartStatus objRptPendingPartStatus = new ProcessDesigner.frmPendingPartStatus(_userInformation, mdiChildPendingPartStatus, false, mdiChildPendingPartStatus.Title);
                    mdiChildPendingPartStatus.Content = objRptPendingPartStatus;
                    mdiChildPendingPartStatus.Height = objRptPendingPartStatus.Height + 40;
                    mdiChildPendingPartStatus.Width = objRptPendingPartStatus.Width + 20;
                    Container.Children.Add(mdiChildPendingPartStatus);
                    break;

                case "miOqa":

                    MdiChild mdiChildmiOQA = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Operator Quality Assurance Chart"
                    };
                    ProcessDesigner.frmOperatorQualityAssurance objRptOQA = new ProcessDesigner.frmOperatorQualityAssurance(_userInformation, mdiChildmiOQA, -9999, OperationMode.AddNew, mdiChildmiOQA.Title);
                    mdiChildmiOQA.Content = objRptOQA;
                    mdiChildmiOQA.Height = objRptOQA.Height + 40;
                    mdiChildmiOQA.Width = objRptOQA.Width + 20;
                    Container.Children.Add(mdiChildmiOQA);

                    break;

                case "miMISCustomerPartNos":

                    MdiChild mdiChildMISCustomerPartNos = new MdiChild
                                        {
                                            MaximizeBox = false,
                                            MinimizeBox = false,
                                            Resizable = false,
                                            Title = ApplicationTitle + "Customer Partno Wise Report"
                                        };
                    ProcessDesigner.frmRptCustpartNo objRptCustpartNo = new ProcessDesigner.frmRptCustpartNo(_userInformation, mdiChildMISCustomerPartNos, null, null, null, false, mdiChildMISCustomerPartNos.Title);
                    mdiChildMISCustomerPartNos.Content = objRptCustpartNo;
                    mdiChildMISCustomerPartNos.Height = objRptCustpartNo.Height + 40;
                    mdiChildMISCustomerPartNos.Width = objRptCustpartNo.Width + 20;
                    Container.Children.Add(mdiChildMISCustomerPartNos);

                    break;
                case "miMISFeatureWise":

                    MdiChild mdiChildMISFeatureWise = new MdiChild
                                        {
                                            MaximizeBox = false,
                                            MinimizeBox = false,
                                            Resizable = false,
                                            Title = ApplicationTitle + "Feature Wise Report"
                                        };
                    ProcessDesigner.frmRptFeatureWise objRptMISFeatureWise = new ProcessDesigner.frmRptFeatureWise(_userInformation, mdiChildMISFeatureWise, null, null, null, null, null, false, mdiChildMISFeatureWise.Title);
                    mdiChildMISFeatureWise.Content = objRptMISFeatureWise;
                    mdiChildMISFeatureWise.Height = objRptMISFeatureWise.Height + 40;
                    mdiChildMISFeatureWise.Width = objRptMISFeatureWise.Width + 20;
                    Container.Children.Add(mdiChildMISFeatureWise);
                    break;
                case "miMISMFMReport":

                    MdiChild mdiChildMISMFM = new MdiChild
                                        {
                                            MaximizeBox = false,
                                            MinimizeBox = false,
                                            Resizable = false,
                                            Title = ApplicationTitle + "MFM Report"
                                        };
                    ProcessDesigner.frmrptMFM objRptMFM = new ProcessDesigner.frmrptMFM(_userInformation, mdiChildMISMFM, null, null, null, false, mdiChildMISMFM.Title);
                    mdiChildMISMFM.Content = objRptMFM;
                    mdiChildMISMFM.Height = objRptMFM.Height + 40;
                    mdiChildMISMFM.Width = objRptMFM.Width + 20;
                    Container.Children.Add(mdiChildMISMFM);

                    break;
                case "miMISPartNos":

                    MdiChild mdiChildPartNos = new MdiChild
                                        {
                                            MaximizeBox = false,
                                            MinimizeBox = false,
                                            Resizable = false,
                                            Title = ApplicationTitle + "Product Information Report"
                                        };
                    ProcessDesigner.frmrptPartNos objRptPartNos = new ProcessDesigner.frmrptPartNos(_userInformation, mdiChildPartNos, null, null, null, null, false, mdiChildPartNos.Title);
                    mdiChildPartNos.Content = objRptPartNos;
                    mdiChildPartNos.Height = objRptPartNos.Height + 40;
                    mdiChildPartNos.Width = objRptPartNos.Width + 20;
                    Container.Children.Add(mdiChildPartNos);

                    break;
                case "miOTPMChart":

                    MdiChild mdiChildOTPMChart = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "OTPM Charts - Development Lead Time"
                    };
                    ProcessDesigner.frmOTPMCharts objRptOTPMChart = new ProcessDesigner.frmOTPMCharts(_userInformation, mdiChildOTPMChart, null, null, null, false, mdiChildOTPMChart.Title);
                    mdiChildOTPMChart.Content = objRptOTPMChart;
                    mdiChildOTPMChart.Height = objRptOTPMChart.Height + 40;
                    mdiChildOTPMChart.Width = objRptOTPMChart.Width + 20;
                    Container.Children.Add(mdiChildOTPMChart);

                    break;
                case "miHardnessConversion":

                    ProcessDesigner.frmHardnessConversion hardnessConversion = new ProcessDesigner.frmHardnessConversion();
                    hardnessConversion.Owner = this;
                    hardnessConversion.ShowInTaskbar = false;
                    hardnessConversion.Show();
                    break;
                case "miThreadDetails":
                    if (threadDetails == null)
                    {
                        threadDetails = new ProcessDesigner.frmThreadDetails();
                    }
                    threadDetails.Owner = this;
                    threadDetails.ShowInTaskbar = false;
                    threadDetails.Show();
                    break;
                case "miChemicalComposition":
                    ProcessDesigner.frmChemicalComposition chemicalComposition = new ProcessDesigner.frmChemicalComposition();
                    chemicalComposition.Owner = this;
                    chemicalComposition.ShowInTaskbar = false;
                    chemicalComposition.Show();
                    break;
                case "miSurfaceFinish":
                    ProcessDesigner.frmSurfaceFinish surfaceFinish = new ProcessDesigner.frmSurfaceFinish();
                    surfaceFinish.Owner = this;
                    surfaceFinish.ShowInTaskbar = false;
                    surfaceFinish.Show();
                    break;
                case "miLibrarySearch":
                    ProcessDesigner.frmLibrarySearch librarySearch = new ProcessDesigner.frmLibrarySearch();
                    librarySearch.Owner = this;
                    librarySearch.ShowInTaskbar = false;
                    librarySearch.Show();
                    break;
                case "miSapHalb":
                    MdiChild mdiChildHalb = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Halb"
                    };
                    ProcessDesigner.frmSapHalb ofinishHalb = new ProcessDesigner.frmSapHalb(_userInformation, mdiChildHalb);
                    mdiChildHalb.Content = ofinishHalb;
                    mdiChildHalb.Height = ofinishHalb.Height + 40;
                    mdiChildHalb.Width = ofinishHalb.Width + 20;
                    Container.Children.Add(mdiChildHalb);

                    break;
                case "miSapFert":
                    MdiChild mdiChildFert = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Fert"
                    };
                    ProcessDesigner.frmSapFert ofinishFert = new ProcessDesigner.frmSapFert(_userInformation, mdiChildFert);
                    mdiChildFert.Content = ofinishFert;
                    mdiChildFert.Height = ofinishFert.Height + 40;
                    mdiChildFert.Width = ofinishFert.Width + 20;
                    Container.Children.Add(mdiChildFert);

                    break;
                case "miSapBom":
                    MdiChild mdiChildBom = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "BOM"
                    };
                    ProcessDesigner.frmSapBom ofinishBOM = new ProcessDesigner.frmSapBom(_userInformation, mdiChildBom);
                    mdiChildBom.Content = ofinishBOM;
                    mdiChildBom.Height = ofinishBOM.Height + 40;
                    mdiChildBom.Width = ofinishBOM.Width + 20;
                    Container.Children.Add(mdiChildBom);
                    break;
                case "miSapRouting":
                    MdiChild mdiChildRouting = new MdiChild
                   {
                       MaximizeBox = false,
                       MinimizeBox = false,
                       Resizable = false,
                       Title = ApplicationTitle + "Routing"
                   };
                    ProcessDesigner.frmSapRouting ofinishRouting = new ProcessDesigner.frmSapRouting(_userInformation, mdiChildRouting);
                    mdiChildRouting.Content = ofinishRouting;
                    mdiChildRouting.Height = ofinishRouting.Height + 40;
                    mdiChildRouting.Width = ofinishRouting.Width + 20;
                    Container.Children.Add(mdiChildRouting);
                    break;

                case "miSapProductVersion":
                    MdiChild mdiChildproductVersion = new MdiChild
                   {
                       MaximizeBox = false,
                       MinimizeBox = false,
                       Resizable = false,
                       Title = ApplicationTitle + "ProductionVersion"
                   };
                    ProcessDesigner.frmSapProductVersion productVersion = new ProcessDesigner.frmSapProductVersion(_userInformation, mdiChildproductVersion);
                    mdiChildproductVersion.Content = productVersion;
                    mdiChildproductVersion.Height = productVersion.Height + 40;
                    mdiChildproductVersion.Width = productVersion.Width + 20;
                    Container.Children.Add(mdiChildproductVersion);
                    break;

                case "miSapImport":
                    MdiChild mdiChildSapImport = new MdiChild
                   {
                       MaximizeBox = false,
                       MinimizeBox = false,
                       Resizable = false,
                       Title = ApplicationTitle + "Master Import"
                   };
                    ProcessDesigner.frmSapImport sapImport = new ProcessDesigner.frmSapImport(_userInformation, mdiChildSapImport);
                    mdiChildSapImport.Content = sapImport;
                    mdiChildSapImport.Height = sapImport.Height + 40;
                    mdiChildSapImport.Width = sapImport.Width + 20;
                    Container.Children.Add(mdiChildSapImport);

                    break;
                case "miSecurity":
                    MdiChild mdiChildsecurity = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "User And Role Management"
                    };
                    ProcessDesigner.frmSecurity security = new ProcessDesigner.frmSecurity(_userInformation, mdiChildsecurity);
                    mdiChildsecurity.Content = security;
                    mdiChildsecurity.Height = security.Height + 40;
                    mdiChildsecurity.Width = security.Width + 20;
                    Container.Children.Add(mdiChildsecurity);

                    break;
                case "miDevelopmentReport":
                    MdiChild mdidevelopmentReport = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Development Report"
                    };

                    ProcessDesigner.frmDevelopmentReport developmentReport = new ProcessDesigner.frmDevelopmentReport(_userInformation, mdidevelopmentReport);
                    mdidevelopmentReport.Content = developmentReport;
                    mdidevelopmentReport.Width = developmentReport.Width + 20;
                    mdidevelopmentReport.Height = developmentReport.Height + 40;
                    Container.Children.Add(mdidevelopmentReport);

                    break;
                case "miManufacturingReport":
                    MdiChild manufacReportChild = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Manufacturing Report"
                    };

                    ProcessDesigner.frmManufacturingReport manufacReport = new ProcessDesigner.frmManufacturingReport(_userInformation, manufacReportChild);
                    manufacReportChild.Content = manufacReport;
                    manufacReportChild.Width = manufacReport.Width + 20;
                    manufacReportChild.Height = manufacReport.Height + 40;
                    Container.Children.Add(manufacReportChild);

                    break;
                // Masters

                case "miOperationMaster":
                    this._frmName = "OPERMASTER";
                    MdiChild mdiChildoper = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Operation Master"
                    };
                    ProcessDesigner.frmOperationMain operationMaster = new ProcessDesigner.frmOperationMain(_frmName, mdiChildoper);
                    mdiChildoper.Content = operationMaster;
                    mdiChildoper.Height = operationMaster.Height + 40;
                    mdiChildoper.Width = operationMaster.Width + 20;
                    Container.Children.Add(mdiChildoper);
                    break;
                case "miFinishMast":
                    this._frmName = "FINISHMASTER";
                    MdiChild mdiChildfinish = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Base Coat Finish Master"
                    };
                    ProcessDesigner.frmFinishMaster ofinishMaster = new ProcessDesigner.frmFinishMaster(_frmName, mdiChildfinish);
                    mdiChildfinish.Content = ofinishMaster;
                    mdiChildfinish.Height = ofinishMaster.Height + 40;
                    mdiChildfinish.Width = ofinishMaster.Width + 20;
                    Container.Children.Add(mdiChildfinish);

                    break;
                case "miUnitMaster":
                    this._frmName = "UNITMASTER";
                    MdiChild mdiChildunit = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Unit Master"
                    };
                    ProcessDesigner.frmOperationMaster ounitMaster = new ProcessDesigner.frmOperationMaster(_frmName, mdiChildunit);
                    mdiChildunit.Content = ounitMaster;
                    mdiChildunit.Height = ounitMaster.Height + 40;
                    mdiChildunit.Width = ounitMaster.Width + 20;
                    Container.Children.Add(mdiChildunit);

                    break;
                case "miCoatingMast":
                    this._frmName = "COATMASTER";
                    MdiChild mdiChildcoat = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Top Coating Master"
                    };
                    ProcessDesigner.frmCoatingMaster ocoatMaster = new ProcessDesigner.frmCoatingMaster(_frmName, mdiChildcoat);
                    mdiChildcoat.Content = ocoatMaster;
                    mdiChildcoat.Height = ocoatMaster.Height + 40;
                    mdiChildcoat.Width = ocoatMaster.Width + 20;
                    Container.Children.Add(mdiChildcoat);

                    break;
                case "miCustomer":
                    this._frmName = "CUSTOMER";
                    MdiChild mdiChildcus = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Customer Master"
                    };
                    ProcessDesigner.frmOperationMaster ocustMaster = new ProcessDesigner.frmOperationMaster(_frmName, mdiChildcus);
                    mdiChildcus.Content = ocustMaster;
                    mdiChildcus.Height = ocustMaster.Height + 40;
                    mdiChildcus.Width = ocustMaster.Width + 20;
                    Container.Children.Add(mdiChildcus);

                    break;
                case "miLocationMaster":
                    MdiChild mdiLocMaster = new MdiChild
                   {
                       MaximizeBox = false,
                       MinimizeBox = false,
                       Resizable = false,
                       Title = ApplicationTitle + "Location Master"
                   };
                    ProcessDesigner.frmLocationMasterRange oLocmaster = new ProcessDesigner.frmLocationMasterRange(mdiLocMaster);
                    mdiLocMaster.Content = oLocmaster;
                    mdiLocMaster.Height = oLocmaster.Height + 40;
                    mdiLocMaster.Width = oLocmaster.Width + 20;
                    Container.Children.Add(mdiLocMaster);

                    break;

                //ProcessDesigner.hotkeys hotekey = new ProcessDesigner.hotkeys();
                //Container.Children.Add(new MdiChild {
                //    Title = "Cost Center Master",
                //    Content = hotekey,
                //    Height = hotekey.Height + 40,
                //    Width = hotekey.Width + 20,
                //    MaximizeBox = false,
                //    MinimizeBox = false
                //});
                //   MdiChild hotekey = new MdiChild;
                //   hotekey.Owner = this;
                // hotekey.ShowInTaskbar = false;
                // hotekey.Show();


                case "miCCMaster":
                    MdiChild mdiCCMaster = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Cost Centre Master"
                    };
                    ProcessDesigner.frmCCMaster ccMaster = new ProcessDesigner.frmCCMaster(_userInformation, mdiCCMaster);
                    mdiCCMaster.Content = ccMaster;
                    mdiCCMaster.Width = ccMaster.Width + 20;
                    mdiCCMaster.Height = ccMaster.Height + 40;
                    Container.Children.Add(mdiCCMaster);
                    break;
                case "miProductCategoryMaster":
                    //if (productCategoryMaster == null)
                    //{
                    MdiChild pcMaster = new MdiChild();
                    pcMaster.Title = ApplicationTitle + "Product Category Master";
                    productCategoryMaster = new ProcessDesigner.frmProductCategoryMaster(pcMaster, _userInformation);
                    pcMaster.Content = productCategoryMaster;
                    pcMaster.Height = productCategoryMaster.Height + 40;
                    pcMaster.Width = productCategoryMaster.Width + 20;
                    pcMaster.MinimizeBox = false;
                    pcMaster.MaximizeBox = false;
                    pcMaster.Resizable = false;
                    Container.Children.Add(pcMaster);
                    //}                   
                    break;
                case "miToolAdmin":
                    MdiChild tooladmin = new MdiChild();
                    tooladmin.Title = ApplicationTitle + "Tool Administration";
                    ProcessDesigner.frmToolAdmin toolAdm = new ProcessDesigner.frmToolAdmin(tooladmin, _userInformation);
                    tooladmin.Content = toolAdm;
                    tooladmin.Height = toolAdm.Height + 45;
                    tooladmin.Width = toolAdm.Width + 25;
                    tooladmin.MinimizeBox = false;
                    tooladmin.MaximizeBox = false;
                    tooladmin.Resizable = false;
                    Container.Children.Add(tooladmin);
                    //}                   
                    break;
                case "miUpdateFeature":
                    MdiChild featureMdi = new MdiChild();
                    featureMdi.Title = ApplicationTitle + "Features Update";
                    ProcessDesigner.frmFeatureUpdationMaster featureU = new ProcessDesigner.frmFeatureUpdationMaster(_userInformation, featureMdi);
                    featureMdi.Content = featureU;
                    featureMdi.Height = featureU.Height + 40;
                    featureMdi.Width = featureU.Width + 20;
                    featureMdi.MinimizeBox = false;
                    featureMdi.MaximizeBox = false;
                    featureMdi.Resizable = false;
                    Container.Children.Add(featureMdi);
                    break;
                case "miExhibit":
                    MdiChild exhibitMdi = new MdiChild();
                    exhibitMdi.Title = ApplicationTitle + "Exhibit Master";
                    ProcessDesigner.frmExhibit exhi = new ProcessDesigner.frmExhibit(_userInformation, exhibitMdi);
                    exhibitMdi.Content = exhi;
                    exhibitMdi.Height = exhi.Height + 40;
                    exhibitMdi.Width = exhi.Width + 20;
                    exhibitMdi.MinimizeBox = false;
                    exhibitMdi.MaximizeBox = false;
                    exhibitMdi.Resizable = false;
                    Container.Children.Add(exhibitMdi);

                    break;

                case "miFeatureUpdationMaster":
                    MdiChild mdiFeatureMast = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Characteristic Master"
                    };

                    ProcessDesigner.frmFeatureMaster featureMaster = new ProcessDesigner.frmFeatureMaster(_userInformation, mdiFeatureMast);
                    mdiFeatureMast.Content = featureMaster;
                    mdiFeatureMast.Width = featureMaster.Width + 20;
                    mdiFeatureMast.Height = featureMaster.Height + 40;
                    Container.Children.Add(mdiFeatureMast);

                    break;

                case "miAuditReportAll":
                    //MdiChild mdiAuditReportAll = new MdiChild
                    //{
                    //    MaximizeBox = false,
                    //    MinimizeBox = false,
                    //    Resizable = false
                    //};
                    if (auditreport == null || PresentationSource.FromVisual(auditreport) == null)
                    {
                        auditreport = new ProcessDesigner.frmForm2(_userInformation);
                        auditreport.Title = ApplicationTitle + "Audit Report";
                        auditreport.Owner = this;
                        auditreport.ShowInTaskbar = true;
                        auditreport.Show();
                    }
                    break;

                case "miCharacteristicMaster":
                    MdiChild mdiCrossLinkingMast = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Cross Linking of Characteristics"
                    };

                    ProcessDesigner.frmCharacteristicMaster crossMaster = new ProcessDesigner.frmCharacteristicMaster(_userInformation, mdiCrossLinkingMast);
                    mdiCrossLinkingMast.Content = crossMaster;
                    mdiCrossLinkingMast.Width = crossMaster.Width + 20;
                    mdiCrossLinkingMast.Height = crossMaster.Height + 40;
                    Container.Children.Add(mdiCrossLinkingMast);

                    break;

                case "miCPMMaster":
                    MdiChild mdicpmMaster = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "Control Plan Member Master"
                    };

                    ProcessDesigner.frmCPMMaster cpmMaster = new ProcessDesigner.frmCPMMaster(_userInformation, mdicpmMaster);
                    mdicpmMaster.Content = cpmMaster;
                    mdicpmMaster.Width = cpmMaster.Width + 20;
                    mdicpmMaster.Height = cpmMaster.Height + 40;
                    Container.Children.Add(mdicpmMaster);

                    break;
                case "miPSWMaster":
                    MdiChild mdipswMaster = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "PSW Title"
                    };

                    ProcessDesigner.frmPSWTitleMaster pswMaster = new ProcessDesigner.frmPSWTitleMaster(_userInformation, mdipswMaster);
                    mdipswMaster.Content = pswMaster;
                    mdipswMaster.Width = pswMaster.Width + 20;
                    mdipswMaster.Height = pswMaster.Height + 40;
                    Container.Children.Add(mdipswMaster);

                    break;
                case "miRawMaterial":
                    MdiChild mdiChild = new MdiChild
                    {
                        Title = ApplicationTitle + "Raw Material",
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false
                    };

                    ProcessDesigner.frmRawMaterial rawMaterial = new ProcessDesigner.frmRawMaterial(_userInformation, mdiChild);
                    mdiChild.Content = rawMaterial;
                    mdiChild.Height = rawMaterial.Height + 40;
                    mdiChild.Width = rawMaterial.Width + 20;
                    Container.Children.Add(mdiChild);

                    break;
                case "miPartNumberConfiguration":
                    MdiChild partNumberConfigChild = new MdiChild
                    {
                        Title = ApplicationTitle + "Part Number Configuration",
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false
                    };

                    ProcessDesigner.frmPartNumberConfig partNumberConfig = new ProcessDesigner.frmPartNumberConfig(_userInformation, partNumberConfigChild);
                    partNumberConfigChild.Content = partNumberConfig;
                    partNumberConfigChild.Height = partNumberConfig.Height + 40;
                    partNumberConfigChild.Width = partNumberConfig.Width + 20;
                    Container.Children.Add(partNumberConfigChild);

                    break;
                case "miCFTMeet":
                    MdiChild frmCFTChild = new MdiChild
                    {
                        Title = ApplicationTitle + "CFT Meeting",
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false
                    };
                    ProcessDesigner.frmCFTMeet cftMeet = new ProcessDesigner.frmCFTMeet(_userInformation, frmCFTChild);
                    frmCFTChild.Content = cftMeet;
                    frmCFTChild.Height = cftMeet.Height + 40;
                    frmCFTChild.Width = cftMeet.Width + 20;
                    Container.Children.Add(frmCFTChild);
                    break;

                case "miEcnPcnCFT":
                    MdiChild frmEcnCFTChild = new MdiChild
                    {
                        Title = ApplicationTitle + "ECN-CFT Meeting",
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false
                    };
                    ProcessDesigner.frmECNCFTMeet ecnCftMeet = new ProcessDesigner.frmECNCFTMeet(_userInformation, frmEcnCFTChild);
                    frmEcnCFTChild.Content = ecnCftMeet;
                    frmEcnCFTChild.Height = ecnCftMeet.Height + 40;
                    frmEcnCFTChild.Width = ecnCftMeet.Width + 20;
                    Container.Children.Add(frmEcnCFTChild);
                    break;

                case "miFRCS":
                    MdiChild frmFRCSChild = new MdiChild
                                        {
                                            Title = ApplicationTitle + "Feasibility Report and Cost Sheet",
                                            MaximizeBox = false,
                                            MinimizeBox = false,
                                            Resizable = false
                                        };

                    ProcessDesigner.frmFRCS frcs = new ProcessDesigner.frmFRCS(_userInformation, frmFRCSChild, -99999, OperationMode.AddNew);
                    frmFRCSChild.Content = frcs;
                    frmFRCSChild.Height = frcs.Height + 40;
                    frmFRCSChild.Width = frcs.Width + 20;
                    Container.Children.Add(frmFRCSChild);
                    break;

                case "miControlPlanPI":
                    MdiChild frmPccsChild = new MdiChild
                    {
                        Title = ApplicationTitle + "Control Plan",
                        MaximizeBox = true,
                        MinimizeBox = true,
                        Resizable = false
                    };

                    ProcessDesigner.frmPCCS frpcc = new ProcessDesigner.frmPCCS(_userInformation, frmPccsChild);
                    frmPccsChild.Content = frpcc;
                    frmPccsChild.Height = frpcc.Height + 23;
                    frmPccsChild.Width = frpcc.Width + 20;
                    Container.Children.Add(frmPccsChild);
                    break;

                case "miToolSchedule":
                    MdiChild frmToolSchedule = new MdiChild
                    {
                        Title = ApplicationTitle + "Tool Schedule",
                        MaximizeBox = true,
                        MinimizeBox = true,
                        Resizable = false
                    };

                    ProcessDesigner.frmToolSchedule_new frtoolschedule = new ProcessDesigner.frmToolSchedule_new(_userInformation, frmToolSchedule);
                    frmToolSchedule.Content = frtoolschedule;
                    frmToolSchedule.Height = frtoolschedule.Height + 23;
                    frmToolSchedule.Width = frtoolschedule.Width + 20;
                    Container.Children.Add(frmToolSchedule);
                    frmToolSchedule.Position = new Point(frmToolSchedule.Position.X, frmToolSchedule.Position.Y + 50);
                    Container.InvalidateSize();
                    frmToolSchedule.Position = new Point(frmToolSchedule.Position.X, frmToolSchedule.Position.Y - 50);
                    break;
                case "miTollInfo":
                    MdiChild frmToolsInfo = new MdiChild
                   {
                       Title = ApplicationTitle + "Tools Information",
                       MaximizeBox = false,
                       MinimizeBox = false,
                       Resizable = false
                   };

                    ProcessDesigner.frmToolsInfo toolsinfo = new ProcessDesigner.frmToolsInfo(_userInformation, frmToolsInfo);
                    frmToolsInfo.Content = toolsinfo;
                    frmToolsInfo.Height = toolsinfo.Height + 23;
                    frmToolsInfo.Width = toolsinfo.Width + 20;
                    Container.Children.Add(frmToolsInfo);
                    break;

                case "miRequestDevelopment":

                    MdiChild frmFRCRequest = new MdiChild
                    {
                        Title = ApplicationTitle + "Request for Product Development",
                         MaximizeBox = false,
                         MinimizeBox = false,
                         Resizable = false
                    };
                    ProcessDesigner.frmRPD frmRpd = new ProcessDesigner.frmRPD(frmFRCRequest);
                    frmFRCRequest.Content = frmRpd;
                    frmFRCRequest.Height = frmRpd.Height + 40;
                    frmFRCRequest.Width = frmRpd.Width + 20;
                    Container.Children.Add(frmFRCRequest);
                    if (IsFormMinimized("Request for Product Development") == true)
                    {
                        frmFRCRequest.WindowState = WindowState.Normal;
                    }

                    break;

                case "miLog":
                    MdiChild logchild = new MdiChild();
                    logchild.Title = ApplicationTitle + "Log View";
                    ProcessDesigner.frmLogView log = new ProcessDesigner.frmLogView(_userInformation, logchild);
                    logchild.Content = log;
                    logchild.Height = log.Height + 40;
                    logchild.Width = log.Width + 20;
                    logchild.Resizable = false;
                    logchild.MinimizeBox = false;
                    logchild.MaximizeBox = false;
                    Container.Children.Add(logchild);
                    break;
                //Product Information
                case "miProductInformation":
                    MdiChild frmProductInformationChild = new MdiChild
                                        {
                                            Title = ApplicationTitle + "Product Master",
                       
                                             MaximizeBox = false,
                                             MinimizeBox = false,
                                             Resizable = false,
                                        };

                    ProcessDesigner.frmProductInformation productInformation = new ProcessDesigner.frmProductInformation(_userInformation,
                        frmProductInformationChild, -99999, OperationMode.Edit);
                    frmProductInformationChild.Content = productInformation;
                    frmProductInformationChild.Height = productInformation.Height + 50;
                    frmProductInformationChild.Width = productInformation.Width + 20;
                    Container.Children.Add(frmProductInformationChild);
                    break;

                case "miChangePassword":
                    ProcessDesigner.frmChangePassword changePassword = new ProcessDesigner.frmChangePassword(_userInformation);
                    changePassword.Owner = this;
                    changePassword.ShowInTaskbar = false;
                    Progress.End();
                    changePassword.ShowDialog();
                    break;

                case "miProcessSheet":
                    MdiChild psheet = new MdiChild();
                    psheet.Title = ApplicationTitle + "Process Sheet";
                    ProcessDesigner.frmProcessSheet processsheet = new ProcessDesigner.frmProcessSheet(psheet, _userInformation);
                    psheet.Content = processsheet;
                    psheet.Height = processsheet.Height + 40;
                    psheet.Width = processsheet.Width + 20;
                    //PSheet.WindowState = WindowState.Maximized;                    
                    psheet.Resizable = false;
                    psheet.MinimizeBox = true;
                    psheet.MaximizeBox = true;
                    Container.Children.Add(psheet);
                    break;

                case "miDDPerformance":

                    MdiChild mdiDDperformance = new MdiChild();
                    mdiDDperformance.Title = ApplicationTitle + "D&D Performance Details";
                    ProcessDesigner.frmDDPerformance dDPerformance = new ProcessDesigner.frmDDPerformance(_userInformation, mdiDDperformance);
                    mdiDDperformance.Content = dDPerformance;
                    mdiDDperformance.Height = dDPerformance.Height + 40;
                    mdiDDperformance.Width = dDPerformance.Width + 20;
                    mdiDDperformance.MinimizeBox = false;
                    mdiDDperformance.MaximizeBox = false;
                    mdiDDperformance.Resizable = false;
                    Container.Children.Add(mdiDDperformance);
                    break;

                case "miDrawings":
                    MdiChild drwMaster = new MdiChild();
                    drwMaster.Title = ApplicationTitle + "Drawings";
                    ProcessDesigner.frmDrawings drawings = new ProcessDesigner.frmDrawings(drwMaster, _userInformation);
                    drwMaster.Content = drawings;
                    drwMaster.Height = drawings.Height + 40;
                    drwMaster.Width = drawings.Width + 20;
                    drwMaster.MinimizeBox = false;
                    drwMaster.MaximizeBox = false;
                    drwMaster.Resizable = false;
                    Container.Children.Add(drwMaster);
                    drwMaster.Position = new Point(drwMaster.Position.X, drwMaster.Position.Y + 200);
                    Container.InvalidateSize();
                    drwMaster.Position = new Point(drwMaster.Position.X, drwMaster.Position.Y - 200);
                    break;
                case "miMFMPlanDetails":

                    MdiChild childMFPlanDetails = new MdiChild();
                    childMFPlanDetails.Title = ApplicationTitle + "MFM Plan";
                    ProcessDesigner.frmMfmPlanDetails mfplandetails = new ProcessDesigner.frmMfmPlanDetails(childMFPlanDetails, _userInformation);
                    childMFPlanDetails.Content = mfplandetails;
                    childMFPlanDetails.Height = mfplandetails.Height + 40;
                    childMFPlanDetails.Width = mfplandetails.Width + 20;
                    childMFPlanDetails.Resizable = false;
                    childMFPlanDetails.MinimizeBox = false;
                    childMFPlanDetails.MaximizeBox = false;
                    Container.Children.Add(childMFPlanDetails);
                    break;
                case "miTileHorizontally":
                    Container.MdiLayout = MdiLayout.TileHorizontal;
                    break;

                case "miTileVertically":
                    Container.MdiLayout = MdiLayout.TileVertical;
                    break;

                case "miTileCascade":
                    Container.MdiLayout = MdiLayout.Cascade;
                    break;

                case "miCostSheetSearch":
                    MdiChild mdiCostSheetSearch = new MdiChild();
                    ProcessDesigner.frmCostSheetSearch costSheetSearch = new ProcessDesigner.frmCostSheetSearch(_userInformation, mdiCostSheetSearch);
                    mdiCostSheetSearch.Title = ApplicationTitle + "Cost Sheet Search";
                    mdiCostSheetSearch.Content = costSheetSearch;
                    mdiCostSheetSearch.Height = costSheetSearch.Height + 40;
                    mdiCostSheetSearch.Width = costSheetSearch.Width + 20;
                    mdiCostSheetSearch.MinimizeBox = false;
                    mdiCostSheetSearch.MaximizeBox = false;
                    mdiCostSheetSearch.Resizable = false;
                    Container.Children.Add(mdiCostSheetSearch);
                    break;
                case "miProductSearch":
                    MdiChild mdiProductSearch = new MdiChild();
                    ProcessDesigner.frmProductSearch productSearch = new frmProductSearch(_userInformation, mdiProductSearch);
                    mdiProductSearch.Title = ApplicationTitle + "Product Search";
                    mdiProductSearch.Content = productSearch;
                    mdiProductSearch.Height = productSearch.Height + 40;
                    mdiProductSearch.Width = productSearch.Width + 20;
                    mdiProductSearch.MinimizeBox = false;
                    mdiProductSearch.MaximizeBox = false;
                    mdiProductSearch.Resizable = false;
                    Container.Children.Add(mdiProductSearch);
                    break;

                case "miExit":
                    this.Close();
                    string hostname = Dns.GetHostName();
                    string ip = Dns.GetHostByName(hostname).AddressList[0].ToString();
                    string username = _userInformation.UserName;
                    bll = new ActiveUsersBLL(_userInformation);
                    bll.LogOut(username, ip, hostname);
                    break;
                case "miAllFeature":
                    DataTable feature;
                    feature = fbll.GetAllFeature();
                    if (feature == null || feature.Rows.Count == 0)
                    {
                        Progress.End();
                        ShowInformationMessage(PDMsg.NoRecordsPrint);
                    }
                    else
                    {
                        frmReportViewer rv = new frmReportViewer(feature, "AllFeature");
                        Progress.End();
                        rv.ShowDialog();
                    }
                    break;

                case "miQCP":
                    MdiChild qcpchild = new MdiChild();
                    qcpchild.Title = ApplicationTitle + "QCP";
                    ProcessDesigner.frmQcp qcp = new ProcessDesigner.frmQcp(_userInformation, qcpchild);
                    qcpchild.Content = qcp;
                    qcpchild.Height = qcp.Height + 40;
                    qcpchild.Width = qcp.Width + 20;
                    qcpchild.Resizable = false;
                    qcpchild.MinimizeBox = false;
                    qcpchild.MaximizeBox = false;
                    Container.Children.Add(qcpchild);
                    break;

                case "miSpecialCharacteristicsReport":
                    DataTable specialcharactertics;
                    specialcharactertics = scbll.GetSpecialCharacteristics();
                    if (specialcharactertics == null || specialcharactertics.Rows.Count == 0)
                    {
                        Progress.End();
                        ShowInformationMessage(PDMsg.NoRecordsPrint);
                        return;
                    }
                    else
                    {
                        frmReportViewer rv = new frmReportViewer(specialcharactertics, "SpecialCharacteristics");
                        Progress.End();
                        rv.ShowDialog();
                    }
                    break;
                case "miSpecialCharacteristicsExcel":
                    Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                    sfd.DefaultExt = ".xlsx";
                    sfd.Filter = "Excel File (*.xls, *.xlsx)|*.xls;*.xlsx";
                    sfd.ShowDialog();
                    if (sfd.FileName != "")
                    {
                        DataTable dt;
                        dt = scbll.GetSpecialCharacteristics();
                        dt.ExportToExcel(sfd.FileName);
                        ShowInformationMessage("Exported to Excel File Succesfully");
                    }
                    break;
                case "miTFCNPD":

                    MdiChild tfcchild = new MdiChild();
                    tfcchild.Title = ApplicationTitle + "TFC Report";
                    ProcessDesigner.frmTfc tfc = new ProcessDesigner.frmTfc(_userInformation, tfcchild);
                    tfcchild.Content = tfc;
                    tfcchild.Height = tfc.Height + 40;
                    tfcchild.Width = tfc.Width + 20;
                    tfcchild.Resizable = false;
                    tfcchild.MinimizeBox = false;
                    tfcchild.MaximizeBox = false;
                    Container.Children.Add(tfcchild);
                    break;
                case "miTFCECN":

                    MdiChild tfcchildecn = new MdiChild();
                    tfcchildecn.Title = ApplicationTitle + "Team Feasibility Commitment ECN-CFT Meeting";
                    ProcessDesigner.frmTfcECN tfcecn = new ProcessDesigner.frmTfcECN(_userInformation, tfcchildecn);
                    tfcchildecn.Content = tfcecn;
                    tfcchildecn.Height = tfcecn.Height + 40;
                    tfcchildecn.Width = tfcecn.Width + 20;
                    tfcchildecn.Resizable = false;
                    tfcchildecn.MinimizeBox = false;
                    tfcchildecn.MaximizeBox = false;
                    Container.Children.Add(tfcchildecn);
                    break;
                case "miTFCPCR":

                    MdiChild tfcchildpcr = new MdiChild();
                    tfcchildpcr.Title = ApplicationTitle + "Team Feasibility Commitment PCR";
                    ProcessDesigner.frmTfcPCR tfcpcr = new ProcessDesigner.frmTfcPCR(_userInformation, tfcchildpcr);
                    tfcchildpcr.Content = tfcpcr;
                    tfcchildpcr.Height = tfcpcr.Height + 40;
                    tfcchildpcr.Width = tfcpcr.Width + 20;
                    tfcchildpcr.Resizable = false;
                    tfcchildpcr.MinimizeBox = false;
                    tfcchildpcr.MaximizeBox = false;
                    Container.Children.Add(tfcchildpcr);
                    break;

                case "miApplicationMaster":

                    MdiChild appchild = new MdiChild();
                    appchild.Title = ApplicationTitle + "PSW Application Master";
                    ProcessDesigner.frmApplication app = new ProcessDesigner.frmApplication(_userInformation, appchild);
                    appchild.Content = app;
                    appchild.Height = app.Height + 40;
                    appchild.Width = app.Width + 20;
                    appchild.Resizable = false;
                    appchild.MinimizeBox = false;
                    appchild.MaximizeBox = false;
                    Container.Children.Add(appchild);
                    break;

                case "miCategoryMaster":

                    MdiChild categorychild = new MdiChild();
                    categorychild.Title = ApplicationTitle + "Category Master";
                    ProcessDesigner.frmCategory cate = new ProcessDesigner.frmCategory(_userInformation, categorychild);
                    categorychild.Content = cate;
                    categorychild.Height = cate.Height + 40;
                    categorychild.Width = cate.Width + 20;
                    categorychild.Resizable = false;
                    categorychild.MinimizeBox = false;
                    categorychild.MaximizeBox = false;
                    Container.Children.Add(categorychild);
                    break;
                case "miMachineBooking":
                    MdiChild machinechild = new MdiChild();
                    machinechild.Title = ApplicationTitle + "Machine Booking for Development for the Year";
                    ProcessDesigner.frmMachineBooking machine = new ProcessDesigner.frmMachineBooking(_userInformation, machinechild);
                    machinechild.Content = machine;
                    machinechild.Height = machine.Height + 40;
                    machinechild.Width = machine.Width + 20;
                    machinechild.Resizable = false;
                    machinechild.MinimizeBox = false;
                    machinechild.MaximizeBox = false;
                    Container.Children.Add(machinechild);
                    break;

                case "miAPQP":
                    MdiChild mdiAPQP = new MdiChild
                    {
                        MaximizeBox = false,
                        MinimizeBox = false,
                        Resizable = false,
                        Title = ApplicationTitle + "APQP"
                    };
                    ProcessDesigner.frmAPQP aPQP = new ProcessDesigner.frmAPQP(_userInformation, mdiAPQP);
                    mdiAPQP.Content = aPQP;
                    mdiAPQP.Height = aPQP.Height + 40;
                    mdiAPQP.Width = aPQP.Width + 20;
                    Container.Children.Add(mdiAPQP);
                    break;
                //case "miTesting":
                //    ProcessDesigner.Testing testing = new ProcessDesigner.Testing(_userInformation);
                //    testing.Owner = this;
                //    testing.ShowInTaskbar = false;
                //    Progress.End();
                //    testing.Show();
                //    break;
                case "miBaseCoatFinish":
                    FinishMasterBll finishBasebll = new FinishMasterBll(_userInformation);
                      
                    DataTable dtBaseCoatFinish;
                    dtBaseCoatFinish = finishBasebll.GetBaseCoating();
                    dtBaseCoatFinish.WriteXmlSchema(@"D:\SFLProcessDesigner\03.Coding\ProcessDesigner\ProcessDesigner\XML\BaseCoatFinishXML.xml");
                    if (dtBaseCoatFinish == null || dtBaseCoatFinish.Rows.Count == 0)
                    {
                        Progress.End();
                        ShowInformationMessage(PDMsg.NoRecordsPrint);
                        return;
                    }
                    else
                    {
                        frmReportViewer rv = new frmReportViewer(dtBaseCoatFinish, "BaseCoatFinish");
                        Progress.End();
                        rv.ShowDialog();
                    }
                    break;

                case "miTopCoatFinish":
                    Coatingmaster coatingMasterbll = new Coatingmaster(_userInformation);

                    DataTable dtTopCoatFinish;
                    dtTopCoatFinish = coatingMasterbll.GetTopCoatingMaster();
                    dtTopCoatFinish.WriteXmlSchema(@"D:\SFLProcessDesigner\03.Coding\ProcessDesigner\ProcessDesigner\XML\TopCoatFinishXML.xml");
                    if (dtTopCoatFinish == null || dtTopCoatFinish.Rows.Count == 0)
                    {
                        Progress.End();
                        ShowInformationMessage(PDMsg.NoRecordsPrint);
                        return;
                    }
                    else
                    {
                        frmReportViewer rv = new frmReportViewer(dtTopCoatFinish, "TopCoatFinish");
                        Progress.End();
                        rv.ShowDialog();
                    }
                    break;
            }
            Progress.End();
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        public string ApplicationTitle = "SmartPD - ";
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                string hostname = Dns.GetHostName();
                string ip = Dns.GetHostByName(hostname).AddressList[0].ToString();
                string username = _userInformation.UserName;
                bll = new ActiveUsersBLL(_userInformation);
                bll.LogOut(username, ip, hostname);
                SecurityUsersBll secuserbll = new SecurityUsersBll(_userInformation);
                if (secuserbll.CheckIsAdminAvailable() == false)
                {
                    MessageBox.Show("Please select ADMIN rights for any one administrator user", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }



}
