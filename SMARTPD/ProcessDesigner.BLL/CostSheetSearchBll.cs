using ProcessDesigner.Common;
using System;
using System.Linq;
using ProcessDesigner.Model;
using System.Data;
using System.Collections.Generic;
using System.Globalization;

namespace ProcessDesigner.BLL
{
    public class CostSheetSearchBll : Essential
    {
        public CostSheetSearchBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }

        public DataView GetCostSheetSearchDetails(CostSheetSearchModel costsheetsearch)
        {
            //ddci_info i,ddcust_mast c,prd_ciref    
            //select ci_reference as CIReference,prod_desc as ProductDesc,cust_name as CustomerName,cust_dwg_no as CustDwgNo,part_no as PartNo,responsibility as Resp, loc_code as Location   
            //from ddci_info i,ddcust_mast c,prd_ciref  p 
            //where i.ci_reference=p.ci_ref (+)  and i.cust_code = c.cust_code (+)  and part_no ='W01330' 
            //and prod_desc like '%ASDF%' and cust_name like '%ASDF%'  and cust_dwg_no like '%ASDF%' 
            //and pending=1 and ci_reference not like 'O%' and  ci_reference not like 'U%' and ci_reference not like 'X%'  and allot_part_no=1  order by ci_reference 
            //1
            //select ci_reference as CIReference,prod_desc as ProductDesc,cust_name as CustomerName,cust_dwg_no as CustDwgNo,part_no as PartNo,responsibility as Resp, loc_code as Location   from ddci_info i,ddcust_mast c,prd_ciref  p where i.ci_reference=p.ci_ref (+)  and i.cust_code = c.cust_code (+)  and pending=0 order by ci_reference 
            //select ci_reference as CIReference,prod_desc as ProductDesc,cust_name as CustomerName,cust_dwg_no as CustDwgNo,part_no as PartNo,responsibility as Resp, loc_code as Location   from ddci_info i,ddcust_mast c,prd_ciref  p where i.ci_reference=p.ci_ref (+)  and i.cust_code = c.cust_code (+)  and pending=0 and ci_reference not like 'O%' and  ci_reference not like 'U%' and ci_reference not like 'X%'  order by ci_reference 
            //select ci_reference as CIReference,prod_desc as ProductDesc,cust_name as CustomerName,cust_dwg_no as CustDwgNo,part_no as PartNo,responsibility as Resp, loc_code as Location   from ddci_info i,ddcust_mast c,prd_ciref  p where i.ci_reference=p.ci_ref (+)  and i.cust_code = c.cust_code (+)  and pending=0 and ci_reference  like 'X%'  order by ci_reference 
            //select ci_reference as CIReference,prod_desc as ProductDesc,cust_name as CustomerName,cust_dwg_no as CustDwgNo,part_no as PartNo,responsibility as Resp, loc_code as Location   from ddci_info i,ddcust_mast c,prd_ciref  p where i.ci_reference=p.ci_ref (+)  and i.cust_code = c.cust_code (+)  and pending=0 and allot_part_no=1  order by ci_reference 
            //select ci_reference as CIReference,prod_desc as ProductDesc,cust_name as CustomerName,cust_dwg_no as CustDwgNo,part_no as PartNo,responsibility as Resp, loc_code as Location   from ddci_info i,ddcust_mast c,prd_ciref  p where i.ci_reference=p.ci_ref (+)  and i.cust_code = c.cust_code (+)  and pending=1 order by ci_reference 

            //
            try
            {
                //dataValue = ToDataTable((from c in DB.DDOPER_MAST.AsEnumerable()
                //                         where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null)) && !c.OPER_DESC.StartsWith("FORG")
                //                         orderby c.OPER_CODE ascending
                //                         select new { c.OPER_CODE, c.OPER_DESC }).ToList());
                //List<DDCI_INFO> featureMastNew = (from cost in DB.DDCI_INFO
                //                                  select cost).ToList();

                //return featureMastNew;
                string subQuery = string.Empty;

                subQuery = " where 1 = 1  ";

                if (costsheetsearch.ChkShowPending)
                {
                    subQuery += " and pending = 1 ";
                }
                else
                {
                    subQuery += " and pending = 0 ";
                }

                if (!string.IsNullOrEmpty(costsheetsearch.PROD_DESC))
                {
                    subQuery += " AND UPPER(prod_desc) like '%" + sqlEncode(costsheetsearch.PROD_DESC.ToValueAsString().Trim().ToUpper()) + "%'";
                }

                if (!string.IsNullOrEmpty(costsheetsearch.PART_NO))
                {
                    subQuery += " AND UPPER(part_no) = '" + sqlEncode(costsheetsearch.PART_NO.ToUpper().Trim()) + "'";
                }

                if (!string.IsNullOrEmpty(costsheetsearch.CUST_NAME))
                {
                    subQuery += " AND UPPER(cust_name) like '%" + sqlEncode(costsheetsearch.CUST_NAME.ToUpper()) + "%'";
                }

                if (!string.IsNullOrEmpty(costsheetsearch.CUST_DWG_NO))
                {
                    subQuery += " AND UPPER(cust_dwg_no) like '%" + sqlEncode(costsheetsearch.CUST_DWG_NO.ToUpper()) + "%'";
                }

                if (costsheetsearch.ChkShowDomesticOnly)
                {
                    subQuery += " AND ci_reference not like 'O%' and  ci_reference not like 'U%' and ci_reference not like 'X%'";
                }

                if (costsheetsearch.ChkShowExpertOnly)
                {
                    subQuery += " AND ci_reference  like 'X%'";
                }

                //Right Click - Pending Allot partnumber - Condition should be Allot_part_no = 0  - 17/12/2015
                if (costsheetsearch.ChkPendingPartNoAllocation)
                {
                    subQuery += " AND (allot_part_no=0 or allot_part_no is null) ";
                }
                else
                {
                    //subQuery += " AND (allot_part_no=1) ";
                }


                //if (costsheetsearch.ChkShowAll == true && (costsheetsearch.PROD_DESC.IsNotNullOrEmpty() || costsheetsearch.PART_NO.IsNotNullOrEmpty() || costsheetsearch.CUST_NAME.IsNotNullOrEmpty() || costsheetsearch.CUST_DWG_NO.IsNotNullOrEmpty()))
                //{
                //    // subQuery = "";
                //    //CostSheetSearch.PROD_DESC = string.Empty;
                //    //CostSheetSearch.PART_NO = string.Empty;
                //    //CostSheetSearch.CUST_NAME = string.Empty;
                //    //CostSheetSearch.CUST_DWG_NO = string.Empty;
                //}
                //else 
               
                //original
                //if (costsheetsearch.ChkShowAll == true)
                //{
                //    subQuery = " where 1 = 1 and pending = 0 ";
                //}
                //end
                //new
                if (costsheetsearch.ChkShowAll == true && (string.IsNullOrEmpty(costsheetsearch.PROD_DESC) && string.IsNullOrEmpty(costsheetsearch.PART_NO) && string.IsNullOrEmpty(costsheetsearch.CUST_NAME) && string.IsNullOrEmpty(costsheetsearch.CUST_DWG_NO)))
                {
                    subQuery = " where 1 = 1 and pending = 0 ";
                }
                //else if (costsheetsearch.ChkShowAll == true && !string.IsNullOrEmpty(costsheetsearch.CUST_DWG_NO))
                else 
                {
                    subQuery += "";
                }
                //new end


                subQuery += " order by ci_reference ";


                string getquery = " select ci_reference,prod_desc,cust_name,cust_dwg_no ,part_no ,responsibility, loc_code as location,ddi.idpk as CI_INFO_PK "
                                 + " from ddci_info ddi "
                                 + " Left join ddcust_mast ddc on ddc.CUST_CODE = ddi.CUST_CODE "
                                 + " Left join prd_ciref prd on prd.CI_REF = ddi.CI_REFERENCE " + subQuery;
                var getcollection = ToDataTable(DB.ExecuteQuery<CostSheetSearchModel>(getquery).ToList());

                return getcollection.DefaultView;

                //return ToDataTable((from ddct in DB.DDCI_INFO
                //                    join ddcust in DB.DDCUST_MAST on ddct.CUST_CODE equals ddcust.CUST_CODE
                //                    select new
                //                    {
                //                        CI_REFERENCE = ddct.CI_REFERENCE,
                //                        PROD_DESC = ddct.PROD_DESC,
                //                        CUST_DWG_NO = ddct.CUST_DWG_NO,
                //                        CUST_NAME = ddcust.CUST_NAME

                //                    }).ToList()).DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetCustNameDetails(CostSheetSearchModel costsheetModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DDCUST_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null)) && (o.CUST_NAME != null && o.CUST_NAME != "")
                                  select new { o.CUST_NAME }).ToList());
                if (dt != null)
                {
                    costsheetModel.CustName = dt.DefaultView;
                }
                else
                {
                    costsheetModel.CustName = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetCustDwg(CostSheetSearchModel costsheetModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DDCI_INFO
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null)) && (o.CUST_DWG_NO != null && o.CUST_DWG_NO != "")
                                  select new { o.CUST_DWG_NO }).ToList());
                if (dt != null)
                {
                    costsheetModel.CustDwgNo = dt.DefaultView;
                }
                else
                {
                    costsheetModel.CustDwgNo = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetPartNumberDetails()
        {
            //ddci_info i,ddcust_mast c,prd_ciref    ,prd_mast         
            try
            {

                DataTable dataValue;
                dataValue = ToDataTable((from prt in DB.PRD_MAST
                                         select new
                                         {
                                             PART_NO = prt.PART_NO,
                                             PART_DESC = prt.PART_DESC

                                         }).ToList());
                return dataValue.DefaultView;


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public string CopyCostSheet(string varOldCIref, string varNewCIref)
        {

            List<DDCOST_PROCESS_DATA> lstddcost_process_data = new List<DDCOST_PROCESS_DATA>();
            List<DDCOST_PROCESS_DATA> lstoldddcost_process_data = new List<DDCOST_PROCESS_DATA>();



            try
            {

                varNewCIref = varNewCIref.Trim().ToUpper();
                if (varNewCIref.Trim() == "") { return ""; }
                if (varNewCIref.Substring(0, 1) == "N" ||
                   varNewCIref.Substring(0, 1) == "E" ||
                   varNewCIref.Substring(0, 1) == "S" ||
                   varNewCIref.Substring(0, 1) == "W" ||
                   varNewCIref.Substring(0, 1) == "X" ||
                   varNewCIref.Substring(0, 1) == "O" ||
                   varNewCIref.Substring(0, 1) == "U" ||
                   varNewCIref.Substring(0, 1) == "P" ||
                   varNewCIref.Substring(0, 1) == "I")
                {
                    if (varNewCIref.Substring(1, 2).Trim().IsNumeric() == false)
                    {
                        return "Enter Valid Year";
                    }
                    if (varNewCIref.Substring(3, 2).Trim().IsNumeric() == false && varNewCIref.Substring(3, 2).ToValueAsString().ToDecimalValue() > 12)
                    {
                        return "Enter Valid Month";
                    }
                    else if (varNewCIref.Substring(5, 2).Trim().IsNumeric() == false && varNewCIref.Substring(5, 2).ToValueAsString().ToDecimalValue() > 31)
                    {
                        return "Enter Valid Day ";
                    }
                    if (varNewCIref.Substring(7, 3).Trim().IsNumeric() == false)
                    {
                        return "Enter Valid Number ";
                    }
                }
                else
                {
                    return "Enter CI Reference in the Valid Format";
                }





                #region "Insert into ddcost_process_data"
                #endregion

                #region "Insert into ddshape_details"
                #endregion



                /*
                                If rsOld.Fields(j).name <> "CUST_DWG_NO" And _
                    rsOld.Fields(j).name <> "CUST_DWG_NO_ISSUE" And _
                    rsOld.Fields(j).name <> "FR_CS_DATE" And _
                    rsOld.Fields(j).name <> "ENQU_RECD_ON" Then
                         .Fields(j).Value = rsOld.Fields(j).Value
                Else
                    If rsOld.Fields(j).name = "FR_CS_DATE" Then
                        .Fields(j) = Format$(GetServerDate, "dd-mmm-yyyy")
                    ElseIf rsOld.Fields(j).name = "ENQU_RECD_ON" Then
                        .Fields(j) = Format$(CDate(Mid$(varNewCIref, 6, 2) & "/" _
                            & Mid$(varNewCIref, 4, 2) & "/" & Mid$(varNewCIref, 2, 2)), "dd-mmm-yyyy")
                    End If
                End If
                */


                return "";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            //varNewCIref = UCase(varNewCIref.Substring(0, 1)) & Mid(varNewCIref, 2);
            //On Error GoTo ErrorTrap


            //Dim OldSql As String
            //Dim NewSql As String

            //Dim rsOld As ADODB.Recordset
            //Dim rsNew As ADODB.Recordset
            /*
        

        
        //varNewCIref = InputBox("Enter the New CI Reference Number : " & Chr(13) & "  ( Format like  X 02 07 01 001   )   ", "New CI Reference", DefVal)
        if (varNewCIref == "") return false;


           varNewCIref = UCase(Mid(varNewCIref, 1, 1)) & Mid(varNewCIref, 2)
             
        Select Case Mid(varNewCIref, 1, 1)
                
               Case "N", "E", "S", "W", "X", "O", "U", "P", "I"
                        If Not IsNumeric(Mid(varNewCIref, 2, 2)) Then
                                MsgBox "Enter Valid Year", vbInformation, "Process Designer"
                                Exit Sub
                        End If
                        
                        If Not IsNumeric(Mid(varNewCIref, 4, 2)) And Mid(varNewCIref, 4, 2) > 12 Then
                                MsgBox "Enter Valid Month", vbInformation, "Process Designer"
                                Exit Sub
                        End If
                        
                        
                        If Not IsNumeric(Mid(varNewCIref, 6, 2)) And Mid(varNewCIref, 6, 2) > 31 Then
                                MsgBox "Enter Valid Day ", vbInformation, "Process Designer"
                                Exit Sub
                        End If
                        
                        If Not IsNumeric(Mid(varNewCIref, 8, 3)) Then
                                MsgBox "Enter Valid Number ", vbInformation, "Process Designer"
                                Exit Sub
                        End If
                        
               Case Else
                        MsgBox "Enter CI Reference in the Valid Format", vbInformation, "Process Designer"
                        Exit Sub
        End Select
        
        If varNewCIref = "" Then
            Screen.MousePointer = 0
            Exit Sub
        End If
    'Check Ci Reference already exist in Table
    
           If RecordCount("ci_reference", "ddci_info", varNewCIref) >= 1 Then
            MsgBox "CI Reference No " & varNewCIref & " already exists.Enter another CIReference No", vbInformation, "Process Designer"
            Exit Sub
           End If
    
    For i = 1 To 3
        
       Select Case i
        
              Case 1
                    
                    OldSql = "select * from ddci_info where ci_reference ='" & varOldCIref & "'"
        
                    NewSql = "select * from ddci_info where ci_reference ='" & varNewCIref & "'"


              Case 2
              
                    OldSql = "select * from ddcost_process_data where ci_reference ='" & varOldCIref & "'"
                    
                    NewSql = "select * from ddcost_process_data where ci_reference ='" & varNewCIref & "'"
                    
                    
              Case 3
              
                    OldSql = "select * from ddshape_details where ci_reference ='" & varOldCIref & "'"
                    
                    NewSql = "select * from ddshape_details where ci_reference ='" & varNewCIref & "'"
              
        End Select
        
        Set rsOld = New ADODB.Recordset
            rsOld.CursorLocation = adUseClient
            
            rsOld.Open OldSql, gvarcnn, adOpenDynamic, adLockBatchOptimistic
            
            
        If rsOld.RecordCount = 0 Then
            Select Case i
                    Case 2
                            MsgBox "No Process Data to Copy", vbInformation, "Process Designer"
                    Case 3
                            MsgBox "No Weight Calculation Data to Copy", vbInformation, "Process Designer"
            End Select
            GoTo 10
        End If
                        
                        
        Set rsNew = New ADODB.Recordset
            rsNew.CursorLocation = adUseClient
                    
            rsNew.Open NewSql, gvarcnn, adOpenDynamic, adLockBatchOptimistic
                        
        With rsNew
            If .RecordCount = 0 Then
                Select Case i
                    Case 1
                        .AddNew
                            .Fields(0).Value = varNewCIref
                            For j = 1 To rsOld.Fields.Count - 1
                                If rsOld.Fields(j).name <> "CUST_DWG_NO" And _
                                    rsOld.Fields(j).name <> "CUST_DWG_NO_ISSUE" And _
                                    rsOld.Fields(j).name <> "FR_CS_DATE" And _
                                    rsOld.Fields(j).name <> "ENQU_RECD_ON" Then
                                         .Fields(j).Value = rsOld.Fields(j).Value
                                Else
                                    If rsOld.Fields(j).name = "FR_CS_DATE" Then
                                        .Fields(j) = Format$(GetServerDate, "dd-mmm-yyyy")
                                    ElseIf rsOld.Fields(j).name = "ENQU_RECD_ON" Then
                                        .Fields(j) = Format$(CDate(Mid$(varNewCIref, 6, 2) & "/" _
                                            & Mid$(varNewCIref, 4, 2) & "/" & Mid$(varNewCIref, 2, 2)), "dd-mmm-yyyy")
                                    End If
                                End If
                            Next j
                        .UpdateBatch
                    Case 2, 3
                        For k = 1 To rsOld.RecordCount
                            .AddNew
                                .Fields(0).Value = varNewCIref
                                For j = 1 To rsOld.Fields.Count - 1
                                    .Fields(j) = rsOld.Fields(j).Value
                                Next j
                            .UpdateBatch
                            rsOld.MoveNext
                        Next k
                        Data_Copied = 1
                End Select
                    
            ElseIf .RecordCount > 0 Then
                Select Case i
                    Case 1
                        Message = CStr(.RecordCount) & " entry already exists for CI Reference: " & CStr(varNewCIref) & ". Copy only Process?"
                        yorn = MsgBox(Message, vbCritical + vbDefaultButton2 + vbYesNo, "Warning")
                        If yorn = 6 Then
                            GoTo 10
                        Else
                            Screen.MousePointer = 0
                            Exit Sub
                        End If
                    Case 2
                        Message = "Delete existing entries before adding new ones?"
                        yorn = MsgBox(Message, vbInformation + vbDefaultButton2 + vbYesNo, "Warning")
                        If yorn = 6 Then
                            Sql = "DELETE FROM DDCOST_PROCESS_DATA WHERE CI_REFERENCE = '" _
                                & varNewCIref & "'"
                            gvarcnn.Execute Sql, 64
                            For k = 1 To rsOld.RecordCount
                                .AddNew
                                    .Fields(0).Value = varNewCIref
                                    For j = 1 To rsOld.Fields.Count - 1
                                        .Fields(j) = rsOld.Fields(j).Value
                                    Next j
                                .UpdateBatch
                                rsOld.MoveNext
                            Next k
                            Data_Copied = 1
                        Else
                            Screen.MousePointer = 0
                            Exit Sub
                        End If
                    Case 3
                        Screen.MousePointer = 0
                        Exit Sub
                End Select
            End If
            
            
        End With
                                  
10:
    Next i

    If Data_Copied = 1 Then
          MsgBox "CostSheet has been successfully Copied", vbInformation, "Process Designer"
    End If
Exit Sub
ErrorTrap:
    MsgBox Err.Description, vbInformation, "Process Designer"
        */
        }

        public int FindDDCIInfo(string varCIref)
        {
            List<DDCI_INFO> lstDDCI = new List<DDCI_INFO>();
            try
            {
                lstDDCI = (from row in DB.DDCI_INFO
                           where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                           && row.CI_REFERENCE.Trim() == varCIref.Trim()
                           select row).ToList<DDCI_INFO>();

                if (lstDDCI.Count > 0)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return 0;
        }

        public bool InsertDDCIInfo(string varOldCIref, string varNewCIref)
        {
            List<DDCI_INFO> lstOldDDCI = new List<DDCI_INFO>();
            DDCI_INFO ddciinfonew = new DDCI_INFO();
            DDCI_INFO ddciinfoold = new DDCI_INFO();
            CultureInfo provider = CultureInfo.InvariantCulture;


            try
            {
                lstOldDDCI = (from row in DB.DDCI_INFO
                              where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                              && row.CI_REFERENCE.Trim() == varOldCIref.Trim()
                              select row).ToList<DDCI_INFO>();


                if (lstOldDDCI.Count > 0)
                {
                    ddciinfonew = new DDCI_INFO();
                    ddciinfoold = lstOldDDCI[0];
                    ddciinfonew = ddciinfoold.DeepCopy();
                    ddciinfonew.CI_REFERENCE = varNewCIref;
                    ddciinfonew.IDPK = 0;
                    ddciinfonew.CUST_DWG_NO = "";
                    ddciinfonew.FR_CS_DATE = null;
                    ddciinfonew.CUST_DWG_NO_ISSUE = "";
                    ddciinfonew.ENQU_RECD_ON = null;
                    ddciinfonew.FR_CS_DATE = serverDate;
                    ddciinfonew.ROWID = Guid.NewGuid();
                    ddciinfonew.ENQU_RECD_ON = DateTime.ParseExact(varNewCIref.Substring(5, 2) + "/" +
                                                varNewCIref.Substring(3, 2) + "/" +
                                                varNewCIref.Substring(1, 2), "dd/MM/yy", provider);
                    ddciinfonew.UPDATED_BY = null;
                    ddciinfonew.UPDATED_DATE = null;
                    ddciinfonew.ENTERED_BY = userInformation.UserName;
                    ddciinfonew.ENTERED_DATE = serverDateTime;
                    DB.DDCI_INFO.InsertOnSubmit(ddciinfonew);
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return true;
        }

        public int FindDDCost_Process_Data(string varCIref)
        {
            List<DDCOST_PROCESS_DATA> lstentity = new List<DDCOST_PROCESS_DATA>();
            try
            {
                lstentity = (from row in DB.DDCOST_PROCESS_DATA
                             where
                             row.CI_REFERENCE.Trim() == varCIref.Trim()
                             select row).ToList<DDCOST_PROCESS_DATA>();

                if (lstentity.Count > 0)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return 0;
        }

        public int InsertDDCost_Process_Data(string varOldCIref, string varNewCIRef)
        {
            List<DDCOST_PROCESS_DATA> lstddcost_process_data = new List<DDCOST_PROCESS_DATA>();
            List<DDCOST_PROCESS_DATA> lstoldddcost_process_data = new List<DDCOST_PROCESS_DATA>();
            List<DDCI_INFO> lstddciinfo = new List<DDCI_INFO>();
            DDCOST_PROCESS_DATA ddcost_process_datanew = new DDCOST_PROCESS_DATA();

            try
            {
                lstddciinfo = (from row in DB.DDCI_INFO
                               where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                               && row.CI_REFERENCE.Trim() == varNewCIRef.Trim()
                               select row).ToList<DDCI_INFO>();

                lstddcost_process_data = (from row in DB.DDCOST_PROCESS_DATA
                                          where
                                          row.CI_REFERENCE.Trim() == varNewCIRef.Trim()
                                          select row).ToList<DDCOST_PROCESS_DATA>();

                lstoldddcost_process_data = (from row in DB.DDCOST_PROCESS_DATA
                                             where
                                             row.CI_REFERENCE.Trim() == varOldCIref.Trim()
                                             select row).ToList<DDCOST_PROCESS_DATA>();
                if (lstddciinfo.Count > 0)
                {
                    foreach (DDCOST_PROCESS_DATA ddcost_process_dataold in lstoldddcost_process_data)
                    {
                        ddcost_process_datanew = new DDCOST_PROCESS_DATA();
                        ddcost_process_datanew.CI_REFERENCE = varNewCIRef;
                        ddcost_process_datanew.CI_INFO_FK = lstddciinfo[0].IDPK;
                        ddcost_process_datanew.COST_CENT_CODE = ddcost_process_dataold.COST_CENT_CODE;
                        ddcost_process_datanew.FIX_COST = ddcost_process_dataold.FIX_COST;
                        ddcost_process_datanew.IDPK = 0;
                        ddcost_process_datanew.OPERATION = ddcost_process_dataold.OPERATION;
                        ddcost_process_datanew.OPERATION_NO = ddcost_process_dataold.OPERATION_NO;
                        ddcost_process_datanew.OUTPUT = ddcost_process_dataold.OUTPUT;
                        ddcost_process_datanew.ROWID = Guid.NewGuid();
                        ddcost_process_datanew.SNO = ddcost_process_dataold.SNO;
                        ddcost_process_datanew.SPL_COST = ddcost_process_dataold.SPL_COST;
                        ddcost_process_datanew.TOTAL_COST = ddcost_process_dataold.TOTAL_COST;
                        ddcost_process_datanew.UNIT_OF_MEASURE = ddcost_process_dataold.UNIT_OF_MEASURE;
                        ddcost_process_datanew.VAR_COST = ddcost_process_dataold.VAR_COST;
                        DB.DDCOST_PROCESS_DATA.InsertOnSubmit(ddcost_process_datanew);
                    }
                }
                DB.SubmitChanges();
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return 0;
        }


        public int FindDDShape_Details(string varCIref)
        {
            List<DDSHAPE_DETAILS> lstentity = new List<DDSHAPE_DETAILS>();
            try
            {
                lstentity = (from row in DB.DDSHAPE_DETAILS
                             where
                             row.CI_REFERENCE.Trim() == varCIref.Trim()
                             select row).ToList<DDSHAPE_DETAILS>();

                if (lstentity.Count > 0)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return 0;
        }

        public bool InsertDDShape_Details(string varOldCIref, string varNewCIRef)
        {
            List<DDSHAPE_DETAILS> lstddshape_details = new List<DDSHAPE_DETAILS>();
            List<DDSHAPE_DETAILS> lstoldddshape_details = new List<DDSHAPE_DETAILS>();

            DDSHAPE_DETAILS ddshape_detailsnew = new DDSHAPE_DETAILS();
            DDSHAPE_DETAILS ddshape_detailsold = new DDSHAPE_DETAILS();
            try
            {
                lstoldddshape_details = (from row in DB.DDSHAPE_DETAILS
                                         where
                                         row.CI_REFERENCE.Trim() == varOldCIref.Trim()
                                         select row).ToList<DDSHAPE_DETAILS>();

                ddshape_detailsnew.ROWID = Guid.NewGuid();
                if (lstoldddshape_details.Count > 0)
                {
                    ddshape_detailsnew = new DDSHAPE_DETAILS();
                    ddshape_detailsold = lstddshape_details[0];
                    ddshape_detailsnew = ddshape_detailsold.DeepCopy();
                    ddshape_detailsnew.IDPK = 0;
                    ddshape_detailsnew.CI_REFERENCE = varNewCIRef;
                    DB.DDSHAPE_DETAILS.InsertOnSubmit(ddshape_detailsnew);
                    DB.SubmitChanges();
                }
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return true;
        }

        private string sqlEncode(string sqlValue)
        {
            return sqlValue.Replace("'", "''").Trim();
        }



    }
}
