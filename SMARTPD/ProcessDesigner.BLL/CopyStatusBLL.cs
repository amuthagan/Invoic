using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
namespace ProcessDesigner.BLL
{
    public class CopyStatusBLL : Essential
    {
        public CopyStatusBLL(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;

        }

        public DataTable GetRecordCount(string varColumnName, string varTableName, string varColumnValue, string varFilterstring = "")
        {
            try
            {
                varColumnName = varColumnName.ToUpper();
                varTableName = varTableName.ToUpper();
                varColumnValue = varColumnValue.ToUpper();
                varFilterstring = varFilterstring.ToUpper();
                string getQuery;

                DataTable dataValue;

                if (varFilterstring.Trim() == "")
                {
                    getQuery = "select * From " + varTableName + " where " + varColumnName + " = '" + varColumnValue + "'";

                }
                else
                {
                    getQuery = "select *  From " + varTableName + " where " + varColumnName + " = '" + varColumnValue + "' and " + varFilterstring + "";
                }


                return dataValue = ToDataTable(DB.ExecuteQuery<CopyStatusModel>(getQuery).ToList());


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }
        string x;
        public string CopyProductData(string tblName, string oldPartNo = "", string newPartNo = "", string oldRouteNo = "", string newRouteNo = "", string oldSeqNo = "", string newSeqNo = "", string oldCC = "", string newCC = "", string oldSH = "", string newSH = "")
        {
            try
            {
                switch (tblName)
                {
                    case "PCCS":
                        x = CopyProcessSheet("PCCS", oldPartNo, newPartNo, oldRouteNo, newRouteNo, oldSeqNo, newSeqNo);
                        break;

                }
                return "0";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public string CopyProcessSheet(string tblName, string oldPartNo = "", string newPartNo = "", string oldRouteNo = "", string newRouteNo = "", string oldSeqNo = "", string newSeqNo = "")
        {
            try
            {
                return "0";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DataView GetPartNoDetails(DataView partNumberDetails)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.PRD_MAST
                                  where ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select new { o.PART_NO, o.PART_DESC }).ToList());
                if (dt != null)
                {
                    partNumberDetails = dt.DefaultView;
                }
                else
                {
                    partNumberDetails = null;
                }

                return partNumberDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetRouteNoDetailsByPartNo(DataView routeNoDetails, string partNo)
        {
            try
            {
                DataTable dt = new DataTable();
                if (partNo.IsNotNullOrEmpty())
                {
                    routeNoDetails = null;
                    dt = ToDataTable((from o in DB.PROCESS_MAIN
                                      where o.PART_NO == partNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.ROUTE_NO }).ToList());

                }
                if (dt != null)
                {
                    routeNoDetails = dt.DefaultView;

                }
                else
                {
                    routeNoDetails = null;
                }


                return routeNoDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public DataView GetSequenceNoDetailsByPartNoRouteNo(DataView seqNoDetails, string partNo, decimal routeNo)
        {
            try
            {
                DataTable dt = new DataTable();
                if (partNo.IsNotNullOrEmpty())
                {
                    dt = ToDataTable((from o in DB.PROCESS_SHEET
                                      where o.PART_NO == partNo && o.ROUTE_NO == routeNo && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                      select new { o.SEQ_NO, o.OPN_DESC }).ToList());

                }
                if (dt != null)
                {
                    seqNoDetails = dt.DefaultView;
                }
                else
                {
                    seqNoDetails = null;
                }


                return seqNoDetails;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }



}



//Public Function RecordCount(varColumnName As String, varTableName As String, varColumnValue As String, Optional varFilterstring As String) As Integer
//On Error GoTo ErrorTrap

//Dim rs As ADODB.Recordset

//    Set rs = New ADODB.Recordset

//    If varFilterstring = "" Then
//        Sql = " select count(" & varColumnName & ")  From " & varTableName & " where " & varColumnName & " = '" & sqlEncode(varColumnValue) & "'"
//    Else
//        Sql = " select count(" & varColumnName & ")  From " & varTableName & " where " & varColumnName & " = '" & sqlEncode(varColumnValue) & "' and " & varFilterstring & ""
//    End If

//    With rs
//        .ActiveConnection = gvarcnn
//        .CursorLocation = adUseClient
//        .LockType = adLockReadOnly
//        .CursorType = adOpenForwardOnly
//        .Source = Sql
//        .Open
//        Set .ActiveConnection = Nothing
//    End With

//        RecordCount = rs.Fields(0)

//        Set rs = Nothing

//Exit Function
//ErrorTrap:
//    MsgBox Err.Description, vbInformation, "Process Designer"
//End Function