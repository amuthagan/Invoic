using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Windows;
using System.Windows.Forms;
using System;
using ProcessDesigner.Common;
using System.Reflection;
using System.IO;
using System.Windows.Input;
using System.Collections.Generic;
using CrystalDecisions.Shared;
using ProcessDesigner.Model;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner
{
    /// <summary>
    /// Interaction logic for frmReportViewer.xaml
    /// </summary>
    public partial class frmReportViewer : Window
    {
        public bool ReadyToShowReport = false;
        public string ReportPath = "";
        ExportFormatType exportFormatType = ExportFormatType.NoFormat;
        public frmReportViewer(DataSet dtSearchReport, string formName, ExportFormatType exportFormatType = ExportFormatType.NoFormat, Dictionary<string, string> dictFormula = null)
        {
            InitializeComponent();
            reportView.Owner = Window.GetWindow(this);
            string reportPath = GetReportPath();
            ReadyToShowReport = false;
            this.exportFormatType = exportFormatType;
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "RPD":
                    this.Title = ApplicationTitle + " - " + "Request for product development";
                    reportPath = reportPath + "rptRPD.rpt";
                    ReadyToShowReport = ShowRPDReport(dtSearchReport, reportPath);
                    break;
                case "FRCS":
                    this.Title = ApplicationTitle + " - " + "Product Feasibility and Cost Report";
                    reportPath = reportPath + "rptCostSheet.rpt";
                    ReadyToShowReport = ShowFRCSReport(dtSearchReport, reportPath);
                    break;
                case "CostSheet":
                    reportPath = reportPath + "AccCostSheet.rpt";
                    ReadyToShowReport = ShowCostSheetReport(dtSearchReport, reportPath);
                    break;
                case "CUSTOMER_PARTNO_WISE_REPORT":
                    reportPath = reportPath + "rptCustPartNo.rpt";
                    ReadyToShowReport = BindReport(dtSearchReport, reportPath);
                    break;
                case "FlowChart":
                    reportPath = reportPath + "rptFlowChart.rpt";
                    ReadyToShowReport = ShowFlowChartReport(dtSearchReport, reportPath, exportFormatType, dictFormula);
                    break;
                case "PerformanceReport":
                    reportPath = reportPath + "rptPerformanceList.rpt";
                    ReadyToShowReport = ShowPerformanceReport(dtSearchReport, reportPath, dictFormula);
                    break;
                case "PerformanceReport1":
                    reportPath = reportPath + "rptPerformanceList1.rpt";
                    ReadyToShowReport = ShowPerformanceReport(dtSearchReport, reportPath, dictFormula);
                    break;
                case "OPERATOR_QUALITY_ASSURANCE_CHART":
                    reportPath = reportPath + "RptOQA.rpt";
                    ReadyToShowReport = ShowOQAReport(dtSearchReport, reportPath, dictFormula);
                    break;
                case "PENDING_PARTS_STATUS":
                    reportPath = reportPath + "rptPendingStatusReport.rpt";
                    ReadyToShowReport = ShowReport(dtSearchReport, reportPath);
                    break;
                case "APQPReport":
                    reportPath = reportPath + "APQP.rpt";
                    ReadyToShowReport = ShowAPQPReport(dtSearchReport, reportPath, dictFormula);
                    break;
                case "CFTMeet":                   
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                  //  System.Windows.Controls.Button button = reportView.FindName("btnPrint");
                    //button.Click += button_Click;
                    reportPath = reportPath + "CFTMeeting.rpt";
                    ReadyToShowReport = ShowReportCFTMeet(dtSearchReport, reportPath);
                    
                    break;
                case "ECNCFTMeet":
                    reportPath = reportPath + "RptECNCFTMeet.rpt";
                    ReadyToShowReport = ShowReportCFTMeet(dtSearchReport, reportPath);
                    break;
                case "ManufacturePrint":
                    reportPath = reportPath + "ManufacturingReport.rpt";
                    ReadyToShowReport = ShowManufactureReport(dtSearchReport, reportPath);
                    break;
                case "TOOL_DRWG_ISSUES_DIMENSION":
                    reportPath = reportPath + "rptToolDrawing.rpt";
                    ReadyToShowReport = ShowToolDrawingIssueDimensionalReport(dtSearchReport, reportPath);
                    break;
            }

        }
        void button_Click(object sender, System.EventArgs e)
        {
           
        }

        public frmReportViewer(DataSet dtSearchReport, string formName, Dictionary<string, string> dictFormula)
        {
            InitializeComponent();
            reportView.Owner = Window.GetWindow(this);
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "OPERATOR_QUALITY_ASSURANCE_CHART":
                    reportPath = reportPath + "RptOQA.rpt";
                    ReadyToShowReport = ShowOQAReport(dtSearchReport, reportPath, dictFormula);
                    break;
            }

        }

        public string GetReportPath()
        {
            string reportPathNew = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //  System.Windows.MessageBox.Show(reportPathNew);
            if (Assembly.GetExecutingAssembly().IsDebug() || reportPathNew.Contains("\\bin\\Debug"))
            {
                DirectoryInfo d = new DirectoryInfo(reportPathNew);
                reportPathNew = d.Parent.Parent.FullName;
            }
            ReportPath = reportPathNew + "\\Reports\\";
            //  ReportPath = "E:\\TFS\\SFL\\SFLProcessDesigner\\03.Coding\\ProcessDesigner\\ProcessDesigner\\Reports\\";
            return ReportPath;


        }

        public frmReportViewer(DataTable dtSearchReport, string formName)
        {
            InitializeComponent();
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "CostSheetSearch":
                    this.Title = ApplicationTitle + " - " + "Cost Sheet Search List";
                    reportPath = reportPath + "rptCostSearchlist.rpt";
                    ReadyToShowReport = GetCostSheetSearch(dtSearchReport, reportPath);
                    break;
                case "ProductSearch":
                    this.Title = ApplicationTitle + " - " + "Product Search List";
                    reportPath = reportPath + "rptProductSearch.rpt";
                    ReadyToShowReport = GetProductSearch(dtSearchReport, reportPath);
                    break;
                case "ISIR":
                    reportPath = reportPath + "ISIR.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "PartSubmissionWarrant":
                    reportPath = reportPath + "PartSubmissionWarrant.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "ProcessSheet":
                    reportPath = reportPath + "rptProcessSheet.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;

                case "GetCheckList":
                    reportPath = reportPath + "rptCheckList.rpt";
                    ReadyToShowReport = ShowDimensionalReport(dtSearchReport, reportPath);
                    break;
                case "Inspection":
                    reportPath = reportPath + "rptInitialSampleInspection.rpt";
                    ReadyToShowReport = ShowDimensionalReport(dtSearchReport, reportPath);
                    break;
                case "ToolSchedule":
                    reportPath = reportPath + "rptToolschedule.rpt";
                    ReadyToShowReport = ShowToolScheduleReport(dtSearchReport, reportPath);
                    break;
                case "AuxToolSchedule":
                    reportPath = reportPath + "rptAuxillaryToolSchedule.rpt";
                    ReadyToShowReport = ShowAuxToolScheduleReport(dtSearchReport, reportPath);
                    break;
                case "ControlPlanPrint":
                    reportPath = reportPath + "rptControlPlan.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "AllFeature":
                    reportPath = reportPath + "rptFeatures.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "QCP":
                    reportPath = reportPath + "rptQcp.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "SpecialCharacteristics":
                    reportPath = reportPath + "rptCustom.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "TFC":
                    reportPath = reportPath + "rptTfc.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "AuditReport":
                    this.Title = ApplicationTitle + " - " + "Master List of Customer drawing Vs M Part No.";
                    reportPath = reportPath + "rptAuditReport.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "TFC-ECN":
                    reportPath = reportPath + "rptTfcECN.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "TFC-PCR":
                    reportPath = reportPath + "rptTfcPCR.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "BaseCoatFinish":
                    reportPath = reportPath + "rptBaseCoatFinish.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;
                case "TopCoatFinish":
                    reportPath = reportPath + "rptTopCoatFinish.rpt";
                    ReadyToShowReport = ShowReportCommon(dtSearchReport, reportPath);
                    break;


            }

        }

        //new export excel
        //public frmReportViewer(DataTable dsReport)
        //{
        //    InitializeComponent();
        //    string reportPath = GetReportPath();
        //    this.Title = ApplicationTitle + " - " + "Report Viewer";
               
        //            reportPath = reportPath + "rptControlPlan.rpt";
        //            ReadyToShowReport = ExportToExcel(dsReport, reportPath);
                   
            
        //}

        public frmReportViewer(DataSet dsSearchReport, string formName, string subheader)
        {
            InitializeComponent();
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "FEATURE_WISE_REPORT":
                    if (exportFormatType == ExportFormatType.NoFormat)
                        reportPath = reportPath + "FeatureWise.rpt";
                    else
                        reportPath = reportPath + "FeatureWiseExport.rpt";
                    ReadyToShowReport = BindReport(dsSearchReport, reportPath, exportFormatType, subheader);
                    break;
            }

        }

        public frmReportViewer(DataTable dtSearchReport, string formName, string subheader)
        {
            InitializeComponent();
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "Dimensional":
                    reportPath = reportPath + "rptDimensionalResults.rpt";
                    ReadyToShowReport = ShowProductPccsReport(dtSearchReport, reportPath, subheader);
                    break;
                case "Material":
                    reportPath = reportPath + "rptMaterialResults.rpt";
                    ReadyToShowReport = ShowProductPccsReport(dtSearchReport, reportPath, subheader);
                    break;
                case "Perfomance":
                    reportPath = reportPath + "rptPerfomanceResults.rpt";
                    ReadyToShowReport = ShowProductPccsReport(dtSearchReport, reportPath, subheader);
                    break;
                case "PSW":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptPSW.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "PSWApp":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptPSWApp.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "LeadTime":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptLeadTime.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "AwaitingDoc":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptAwaitingdocumentation.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "AwaitingTools":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptAwaitingTools.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "AwaitingForging":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptAwaitingTools.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "AwaitingSecondary":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptAwaitingTools.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "AwaitingPPAP":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptAwaitingTools.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "FirstTimeRight":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptPSWApproved.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "CustomerComp":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptPSWApproved.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;
                case "NoOfShifts":
                    this.Title = ApplicationTitle + " - " + subheader;
                    reportPath = reportPath + "rptToolsandShifts.rpt";
                    ReadyToShowReport = ShowMFMDevReport(dtSearchReport, reportPath, subheader);
                    break;

            }

        }

        public frmReportViewer(DataTable dtDevRptMain, DataTable dtDevRptDesignAssum, DataTable dtDevRptLog, DataTable dtDevRptShortclouser, string formName)
        {
            InitializeComponent();
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "DevelopmentPrint":
                    reportPath = reportPath + "DevelopmentReport.rpt";
                    ReadyToShowReport = ShowDevRptReport(dtDevRptMain, dtDevRptDesignAssum, dtDevRptLog, dtDevRptShortclouser, reportPath);
                    break;
            }

        }

        public frmReportViewer(List<object> lstSearchReport, string formName)
        {
            InitializeComponent();
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
            }

        }

        public frmReportViewer(DataTable dtSearchReport, string formName, string typeECNPCN, string formatNo, string revisionNo)
        {
            InitializeComponent();
            string reportPath = GetReportPath();
            this.Title = ApplicationTitle + " - " + "Report Viewer";
            switch (formName)
            {
                case "ECNPCN":
                    reportPath = reportPath + "ECNPCNSheet.rpt";
                    ReadyToShowReport = ShowECNPCNReport(dtSearchReport, reportPath, typeECNPCN, formatNo, revisionNo);
                    break;
            }

        }


        private bool GetCostSheetSearch(DataTable dtSearchReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;
                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtSearchReport != null)
                {
                    report.SetDataSource(from c in dtSearchReport.AsEnumerable()
                                         select new
                                         {
                                             CI_REFERENCE = c.Field<string>("ci_reference"),
                                             PROD_DESC = c.Field<string>("prod_desc"),
                                             CUST_NAME = c.Field<string>("cust_name"),
                                             CUST_DWG_NO = c.Field<string>("cust_dwg_no"),
                                             PART_NO = c.Field<string>("part_no")
                                         });
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool GetProductSearch(DataTable dtSearchReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;
                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtSearchReport != null && dtSearchReport.Rows.Count > 0)
                {
                    report.SetDataSource(dtSearchReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                throw ex.LogException();
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            return bReturnValue;
        }

        private bool ShowRPDReport(DataSet dtSearchReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtSearchReport.IsNotNullOrEmpty() && dtSearchReport.Tables.IsNotNullOrEmpty() && dtSearchReport.Tables.Count > 0)
                {
                    report.SetDataSource(dtSearchReport.Tables[0]);
                    report.Subreports["rpdRPD_Sub.rpt"].SetDataSource(dtSearchReport.Tables[1]);
                    report.SetParameterValue("PART_NO", dtSearchReport.Tables[0].Rows[0]["PART_NO"].ToString());
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;

        }

        public string ApplicationTitle = "SmartPD";
        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private bool fileExists(string fileName)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                ShowInformationMessage("Report File '" + fileName + "' does not exists!");
            }
            return fileInfo.Exists;
        }
        private bool ShowFRCSReport(DataSet dsReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;

        }

        private bool ShowCostSheetReport(DataSet dsReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;

        }



        private bool ShowReportCommon(DataTable dsReport, string reportPath)
        {
           
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;
                
                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport != null && dsReport.Rows.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
               
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }


        //for export excel
        //private bool ExportToExcel(DataTable dsReport, string reportPath)
        //{
        //    //new 
        //    ExportOptions crExportOptions;
        //    //end
        //    bool bReturnValue = false;
        //    try
        //    {
        //        if (!fileExists(reportPath)) return bReturnValue;

        //        ReportDocument report = new ReportDocument();
        //        report.Load(reportPath);
        //        if (dsReport != null && dsReport.Rows.Count > 0)
        //        {
        //            report.SetDataSource(dsReport);
        //        }
        //        reportView.ViewerCore.ReportSource = report;
        //        //new
        //        DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
        //        ExcelFormatOptions crFormatTypeOptions = new ExcelFormatOptions();
        //        //crDiskFileDestinationOptions.DiskFileName = "D:\\" + reportPath + ".xls";
        //        crDiskFileDestinationOptions.DiskFileName = "D:\\ControlPlan.xls";
        //        crExportOptions = report.ExportOptions;
        //        crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        crExportOptions.ExportFormatType = ExportFormatType.Excel;
        //        crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
        //        crExportOptions.FormatOptions = crFormatTypeOptions;
        //        report.Export();
        //        //end
        //        bReturnValue = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //    return bReturnValue;
        //}

        private bool ShowMFMDevReport(DataTable dtReport, string reportPath, string subheader)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    report.SetDataSource(dtReport);
                }

                report.SetParameterValue("SubHeading", subheader);

                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }


        private bool ShowDevRptReport(DataTable dtDevRptMain, DataTable dtDevRptDesignAssum, DataTable dtDevRptLog, DataTable dtDevRptShortclouser, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtDevRptMain != null && dtDevRptMain.Rows.Count > 0)
                {
                    report.SetDataSource(dtDevRptMain);
                    report.Subreports[0].SetDataSource(dtDevRptDesignAssum);
                    report.Subreports[1].SetDataSource(dtDevRptLog);
                    report.Subreports[2].SetDataSource(dtDevRptShortclouser);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowManufactureReport(DataSet dsRptDetails, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsRptDetails != null && dsRptDetails.Tables.Count > 0)
                {
                    report.SetDataSource(dsRptDetails.Tables["MainReport"]);
                    report.Subreports["ManuFactDesign.rpt"].SetDataSource(dsRptDetails.Tables["Design"]);
                    report.Subreports["ManuFactDifficulties.rpt"].SetDataSource(dsRptDetails.Tables["Difficulties"]);
                    report.Subreports["ManuFactPreQual.rpt"].SetDataSource(dsRptDetails.Tables["PreQual"]);
                    report.Subreports["ManuFactProcess.rpt"].SetDataSource(dsRptDetails.Tables["Process"]);
                    report.Subreports["ManuFactOutput.rpt"].SetDataSource(dsRptDetails.Tables["Output"]);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowDimensionalReport(DataTable dsReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport != null && dsReport.Rows.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowECNPCNReport(DataTable dsReport, string reportPath, string typeECNPCN, string formatNo, string revisionNo)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport != null && dsReport.Rows.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }

                CrystalDecisions.CrystalReports.Engine.TextObject txtReportHeader;
                CrystalDecisions.CrystalReports.Engine.TextObject txtDesc;
                CrystalDecisions.CrystalReports.Engine.TextObject txtFormatNo;
                CrystalDecisions.CrystalReports.Engine.TextObject txtRevisionNo;
                CrystalDecisions.CrystalReports.Engine.TextObject textRefernceNo;

                txtReportHeader = report.ReportDefinition.ReportObjects["txtHeaderText"] as TextObject;
                txtDesc = report.ReportDefinition.ReportObjects["txtDesc"] as TextObject;
                txtFormatNo = report.ReportDefinition.ReportObjects["txtFormatNo"] as TextObject;
                txtRevisionNo = report.ReportDefinition.ReportObjects["txtRevisionNo"] as TextObject;
                textRefernceNo = report.ReportDefinition.ReportObjects["textRefernceNo"] as TextObject;

                txtFormatNo.Text = formatNo;
                txtRevisionNo.Text = revisionNo;

                report.SetParameterValue("ScreenName", typeECNPCN);

                if (typeECNPCN == "ECN")
                {
                    txtReportHeader.Text = "Engineering Change Note (ECN)";
                    txtDesc.Text = "ECN Sheet No";
                    textRefernceNo.Text = "ECN REFERENCE No.";
                }
                else
                {
                    txtReportHeader.Text = "Process Change Note (PCN)";
                    txtDesc.Text = "PCN Sheet No";
                    textRefernceNo.Text = "PCN REFERENCE No.";
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowToolScheduleReport(DataTable dtReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    report.SetDataSource(dtReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowProductPccsReport(DataTable dtReport, string reportPath, string partname)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    report.SetDataSource(dtReport);
                }
                ((TextObject)(report.ReportDefinition.ReportObjects["Text20"])).Text = partname;
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowAuxToolScheduleReport(DataTable dtReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    report.SetDataSource(dtReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowFlowChartReport(DataSet dsReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;

        }

        private bool BindReport(DataSet dsReport, string reportPath, ExportFormatType exportFormatType = ExportFormatType.NoFormat, string subheading = "")
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    reportDocument.SetDataSource(dsReport);
                }
                if (reportPath.Contains("FeatureWise.rpt")) reportDocument.SetParameterValue("SubHeading", subheading);
                reportView.ViewerCore.ReportSource = reportDocument;

                if (exportFormatType != ExportFormatType.NoFormat)
                {
                    switch (exportFormatType)
                    {
                        case ExportFormatType.Excel:
                        case ExportFormatType.ExcelRecord:
                        case ExportFormatType.ExcelWorkbook:
                            string fileName = "";
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.AddExtension = true;
                            saveFileDialog.CheckPathExists = true;
                            saveFileDialog.DefaultExt = "xls";
                            saveFileDialog.Filter = "Excel Worksheets|*.xls";
                            System.IO.DirectoryInfo di = new DirectoryInfo(GetReportPath() + "\\ExportToExcel");
                            if (!di.Exists)
                            {
                                try
                                {
                                    di.Create();
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                }
                            }

                            saveFileDialog.InitialDirectory = di.FullName;
                            saveFileDialog.Title = "Export to " + Enum.GetName(typeof(ExportFormatType), exportFormatType).ToString();
                            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return bReturnValue;
                            fileName = saveFileDialog.FileName;

                            if (!fileName.IsNotNullOrEmpty()) return bReturnValue;

                            //ExportOptions exportOpts = new ExportOptions();
                            //ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                            //DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                            //exportOpts = reportDocument.ExportOptions;

                            //// Set the excel format options.
                            //excelFormatOpts.ExcelUseConstantColumnWidth = true;
                            //exportOpts.ExportFormatType = exportFormatType;
                            //exportOpts.FormatOptions = excelFormatOpts;

                            //// Set the disk file options and export.
                            //exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                            //diskOpts.DiskFileName = fileName;
                            //exportOpts.DestinationOptions = diskOpts;

                            reportDocument.ExportToDisk(exportFormatType, fileName);
                            break;
                    }
                }
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                //ex.ShowAndLogException();
            }
            return bReturnValue;

        }

        private bool ShowFlowChartReport(DataSet dsReport, string reportPath, ExportFormatType exportFormatType = ExportFormatType.NoFormat, Dictionary<string, string> dictFormula = null)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                if (dictFormula != null)
                {
                    if (dictFormula.ContainsKey("RouteNo"))
                    {
                        report.DataDefinition.FormulaFields["RouteNo"].Text = "'" + dictFormula["RouteNo"] + "'";
                    }
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowPerformanceReport(DataSet dsReport, string reportPath, Dictionary<string, string> dictFormula = null)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                if (dictFormula != null)
                {
                    try
                    {
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblColumn0"])).Text = dictFormula["lblColumn0"];
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblColumn1"])).Text = dictFormula["lblColumn1"];
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblColumn2"])).Text = dictFormula["lblColumn2"];
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblColumn3"])).Text = dictFormula["lblColumn3"];
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblColumn4"])).Text = dictFormula["lblColumn4"];
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblColumn5"])).Text = dictFormula["lblColumn5"];
                        ((TextObject)(report.ReportDefinition.ReportObjects["lblHeader"])).Text = dictFormula["lblHeader"];
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }
                    //report.DataDefinition.FormulaFields["RouteNo"].Text = "'" + dictFormula["RouteNo"] + "'";
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowOQAReport(DataSet dsReport, string reportPath, Dictionary<string, string> dictFormulas)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }

                if (dictFormulas != null)
                {
                    ((TextObject)(report.ReportDefinition.ReportObjects["lblEX_NO"])).Text = dictFormulas["EX_NO"];
                    //((TextObject)(report.ReportDefinition.ReportObjects["lblREVISION_NO"])).Text = dictFormulas["REVISION_NO"];
                    ((TextObject)(report.ReportDefinition.ReportObjects["lblPRV_OPERATION_DESC"])).Text = dictFormulas["PRV_OPERATION_DESC"];
                }

                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private bool ShowReport(DataSet dsReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;

        }
        private bool ShowReportCFTMeet(DataSet dsReport, string reportPath, Dictionary<string, string> dictFormula = null)
        {
            bool bReturnValue = false;
            DataTable dtSpl, dtMid, dtTime, dtExihibit = new DataTable();
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (!dictFormula.IsNotNullOrEmpty())
                {
                    dtSpl = dsReport.Tables[2].Copy();
                    dtMid = dsReport.Tables[1].Copy();
                    dtTime = dsReport.Tables[3].Copy();

                    if (dsReport.Tables.Count == 5 && reportPath.Contains("CFTMeeting.rpt"))
                    {
                        dtExihibit = dsReport.Tables[4].Copy();
                        if (dtExihibit.IsNotNullOrEmpty())
                        {
                            ((TextObject)(report.ReportDefinition.ReportObjects["txtExihibit"])).Text = dtExihibit.Rows[0]["EX_NO"].ToString();
                            ((TextObject)(report.ReportDefinition.ReportObjects["txtRevision"])).Text = dtExihibit.Rows[0]["REVISION_NO"].ToString();
                        }

                    }
                    string rowNum = "";
                    try
                    {
                        for (int i = 0; i < dtSpl.Rows.Count; i++)
                        {
                            if (i < 13)
                            {
                                rowNum = String.Format("{0:00.##}", i + 1);
                                ((TextObject)(report.ReportDefinition.ReportObjects["txtF" + rowNum])).Text = dtSpl.Rows[i][0].ToString();
                                ((TextObject)(report.ReportDefinition.ReportObjects["txtSS" + rowNum])).Text = dtSpl.Rows[i][1].ToString();
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {

                        for (int i = 0; i < dtMid.Rows.Count; i++)
                        {
                            if (i < 24)
                            {
                                rowNum = String.Format("{0:00.##}", i + 1);
                                ((TextObject)(report.ReportDefinition.ReportObjects["txtMM" + rowNum])).Text = dtMid.Rows[i][0].ToString();
                                ((TextObject)(report.ReportDefinition.ReportObjects["txtMS" + rowNum])).Text = dtMid.Rows[i][2].ToString();
                                ((TextObject)(report.ReportDefinition.ReportObjects["txtMC" + rowNum])).Text = dtMid.Rows[i][1].ToString();
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }
                    int rptTimerowNum = 0;
                    try
                    {

                        for (int i = 0; i < dtTime.Rows.Count; i++)
                        {
                            if (i <= 17 && i != 12 && i != 14)
                            {
                                rowNum = String.Format("{0:00.##}", i + 1);
                                if (rowNum.ToDecimalValue() > 3 && rowNum.ToDecimalValue() <= 17)
                                {
                                    // ToString().ToDateAsString("DD/MM/YYYY")
                                    //if (dsReport.Tables.Count == 5 && reportPath.Contains("CFTMeeting.rpt"))
                                    //{
                                    rptTimerowNum = rptTimerowNum + 1;
                                    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIS" + String.Format("{0:00.##}", rptTimerowNum)])).Text = (dtTime.Rows[i][2].ToValueAsString().Trim().Length > 0) ? Convert.ToDateTime(dtTime.Rows[i][2]).ToString("dd/MMM/yyyy") : "";
                                    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIE" + String.Format("{0:00.##}", rptTimerowNum)])).Text = (dtTime.Rows[i][3].ToValueAsString().Trim().Length > 0) ? Convert.ToDateTime(dtTime.Rows[i][3]).ToString("dd/MMM/yyyy") : "";
                                    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIR" + String.Format("{0:00.##}", rptTimerowNum)])).Text = (dtTime.Rows[i][4].ToValueAsString().Trim().Length > 0) ? Convert.ToDateTime(dtTime.Rows[i][4]).ToString("dd/MMM/yyyy") : "";
                                    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIA" + String.Format("{0:00.##}", rptTimerowNum)])).Text = (dtTime.Rows[i][5].ToValueAsString().Trim().Length > 0) ? Convert.ToDateTime(dtTime.Rows[i][5]).ToString("dd/MMM/yyyy") : "";

                                    //}
                                    //else
                                    //{
                                    //    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIS" + String.Format("{0:00.##}", rowNum)])).Text = (dtTime.Rows[i][2].ToValueAsString().Trim().Length > 0) ? dtTime.Rows[i][2].ToDateAsString() : "";
                                    //    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIE" + String.Format("{0:00.##}", rowNum)])).Text = (dtTime.Rows[i][3].ToValueAsString().Trim().Length > 0) ? dtTime.Rows[i][3].ToDateAsString() : "";
                                    //    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIR" + String.Format("{0:00.##}", rowNum)])).Text = (dtTime.Rows[i][4].ToValueAsString().Trim().Length > 0) ? dtTime.Rows[i][4].ToDateAsString() : "";
                                    //    ((TextObject)(report.ReportDefinition.ReportObjects["txtTIIIA" + String.Format("{0:00.##}", rowNum)])).Text = (dtTime.Rows[i][5].ToValueAsString().Trim().Length > 0) ? dtTime.Rows[i][5].ToDateAsString() : "";
                                    //}
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }

                }

                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;

        }
        private bool ShowAPQPReport(DataSet dsReport, string reportPath, Dictionary<string, string> dictFormula)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty())
                {
                    //report.SetDataSource(dsReport);

                    foreach (DataRow item in dsReport.Tables[0].Rows)
                    {
                        if (item["HIDDEN_SL_NO"].IsNotNullOrEmpty())
                        {

                            ((TextObject)(report.ReportDefinition.ReportObjects["comment" + item["HIDDEN_SL_NO"].ToString()])).Text = item["COMMENT"].ToString();

                            if (item["COMPLETION_DATE"].IsNotNullOrEmpty())
                            {
                                ((TextObject)(report.ReportDefinition.ReportObjects["comp_date" + item["HIDDEN_SL_NO"].ToString()])).Text = Convert.ToDateTime(item["COMPLETION_DATE"]).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                ((TextObject)(report.ReportDefinition.ReportObjects["comp_date" + item["HIDDEN_SL_NO"].ToString()])).Text = "";
                            }


                            if (item["DUE_DATE"].IsNotNullOrEmpty())
                            {
                                ((TextObject)(report.ReportDefinition.ReportObjects["due_date" + item["HIDDEN_SL_NO"].ToString()])).Text = Convert.ToDateTime(item["DUE_DATE"]).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                ((TextObject)(report.ReportDefinition.ReportObjects["due_date" + item["HIDDEN_SL_NO"].ToString()])).Text = "";
                            }

                            report.SetParameterValue("status" + item["HIDDEN_SL_NO"].ToString(), item["STATUS"].ToString());
                        }
                    }
                }
                if (dictFormula != null)
                {
                    if (dictFormula.ContainsKey("PartNo"))
                    {
                        report.DataDefinition.FormulaFields["PartNo"].Text = "'" + dictFormula["PartNo"] + "'";
                    }
                }

                //report.DataDefinition.FormulaFields["StatusNRow1"].Text = "chr(254)";
                //((TextObject)(report.ReportDefinition.ReportObjects["txtStatus1"])).Text = "chr(264)";
                //report.DataDefinition.FormulaFields["StatusNRow11"].Text = "chr(254)";
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private void reportView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.PageDown:
                case Key.Right:
                    reportView.ViewerCore.ShowNextPage();
                    break;
                case Key.PageUp:
                case Key.Left:
                    reportView.ViewerCore.ShowPreviousPage();
                    break;
            }
        }

        private bool ShowToolDrawingIssueDimensionalReport(DataSet dsReport, string reportPath)
        {
            bool bReturnValue = false;
            try
            {
                if (!fileExists(reportPath)) return bReturnValue;

                ReportDocument report = new ReportDocument();
                report.Load(reportPath);
                if (dsReport.IsNotNullOrEmpty() && dsReport.Tables.IsNotNullOrEmpty() && dsReport.Tables.Count > 0)
                {
                    report.SetDataSource(dsReport);
                }
                reportView.ViewerCore.ReportSource = report;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }


    }
}
