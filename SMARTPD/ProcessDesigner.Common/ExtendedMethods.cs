
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using ProcessDesigner.ExceptionHandler;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ProcessDesigner.Common
{
    public static partial class ExtendedMethods
    {
        public static string FormatEscapeChars(this string s)
        {
            string result = s;
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    result = s.Replace("'", "''");
                }
            }
            catch { }
            return result;
        }

        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    System.ComponentModel.TypeConverter conv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

        public static bool IsEqual(this DataTable firstDataTable, DataTable secondDataTable)
        {
            if ((!firstDataTable.IsNotNullOrEmpty() || !secondDataTable.IsNotNullOrEmpty()) || firstDataTable.Rows.Count != secondDataTable.Rows.Count)
                return false;

            foreach (DataColumn col in firstDataTable.Columns)
            {
                for (int row = 0; row < firstDataTable.Rows.Count; row++)
                {

                    if (Convert.ToString(firstDataTable.Rows[row][col.ColumnName]).Trim() != Convert.ToString(secondDataTable.Rows[row][col.ColumnName]).Trim())
                    {
                        Console.WriteLine(firstDataTable.Rows[row][col.ColumnName] + ":" + secondDataTable.Rows[row][col.ColumnName]);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsEqual(this DataView firstDataView, DataView secondDataView)
        {
            if ((!firstDataView.IsNotNullOrEmpty() || !secondDataView.IsNotNullOrEmpty()) || firstDataView.Count != secondDataView.Count)
                return false;

            foreach (DataColumn col in firstDataView.Table.Columns)
            {
                for (int row = 0; row < firstDataView.Count; row++)
                {

                    if (Convert.ToString(firstDataView[row][col.ColumnName]) != Convert.ToString(secondDataView[row][col.ColumnName]))
                    {
                        Console.WriteLine(firstDataView[row][col.ColumnName] + ":" + secondDataView[row][col.ColumnName]);
                        return false;
                    }
                }
            }
            return true;
        }

        //public static DataTable Compare(this DataTable first, DataTable second)
        //{
        //    first.TableName = first.TableName + "_FirstTable";
        //    second.TableName = second.TableName + "_SecondTable";

        //    //Create Empty Table
        //    DataTable table = new DataTable("Differences");

        //    try
        //    {
        //        //Must use a Dataset to make use of a DataRelation object
        //        using (DataSet ds = new DataSet())
        //        {
        //            //Add tables
        //            ds.Tables.AddRange(new DataTable[] { first.Copy(), second.Copy() });

        //            //Get Columns for DataRelation
        //            DataColumn[] firstcolumns = new DataColumn[ds.Tables[0].Columns.Count];

        //            for (int i = 0; i < firstcolumns.Length; i++)
        //            {
        //                firstcolumns[i] = ds.Tables[0].Columns[i];
        //            }

        //            DataColumn[] secondcolumns = new DataColumn[ds.Tables[1].Columns.Count];

        //            for (int i = 0; i < secondcolumns.Length; i++)
        //            {
        //                secondcolumns[i] = ds.Tables[1].Columns[i];
        //            }

        //            //Create DataRelation
        //            DataRelation r = new DataRelation(string.Empty, firstcolumns, secondcolumns, false);

        //            ds.Relations.Add(r);

        //            //Create columns for return table
        //            for (int i = 0; i < first.Columns.Count; i++)
        //            {
        //                table.Columns.Add(first.Columns[i].ColumnName, first.Columns[i].DataType);
        //            }

        //            //If First Row not in Second, Add to return table.
        //            table.BeginLoadData();

        //            foreach (DataRow parentrow in ds.Tables[0].Rows)
        //            {
        //                DataRow[] childrows = parentrow.GetChildRows(r);
        //                if (childrows == null || childrows.Length == 0)
        //                    table.LoadDataRow(parentrow.ItemArray, true);
        //            }

        //            table.EndLoadData();

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return table.Rows.Count > 0 ? table : null;
        //}

        public static bool IsNumericType(this Type type)
        {

            HashSet<Type> numericTypes = new HashSet<Type>
            {
                typeof(byte),       typeof(sbyte),
                typeof(UInt16),     typeof(Int16),  
                typeof(UInt32),     typeof(Int32),   
                typeof(UInt64),     typeof(Int64),  
                typeof(UInt64),     typeof(Int64),  
                typeof(short),      typeof(ushort),
                typeof(Decimal),    typeof(Double),  
                typeof(decimal),    typeof(Single),  
                };

            return numericTypes.Contains(type) ||
                   numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        public static string TrimWhiteSpace(this string data)
        {
            string sReturnValue = data;
            try
            {
                sReturnValue = System.Text.RegularExpressions.Regex.Replace(data, @"\s+", " ");
            }
            catch (Exception ex)
            {
                sReturnValue = data;
                ex.LogException();
            }
            return sReturnValue;
        }

        public static bool IsRelease(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            if (attributes == null || attributes.Length == 0)
                return true;

            var d = (DebuggableAttribute)attributes[0];
            if ((d.DebuggingFlags & DebuggableAttribute.DebuggingModes.Default) == DebuggableAttribute.DebuggingModes.None)
                return true;

            return false;
        }

        public static bool IsDebug(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            if (attributes == null || attributes.Length == 0)
                return true;

            var d = (DebuggableAttribute)attributes[0];
            if (d.IsJITTrackingEnabled) return true;
            return false;
        }

        public static object GetFieldValue(this object paramType, string propertyName)
        {
            object oReturnValue = null;
            try
            {
                PropertyInfo propertyInfo = paramType.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)
                {
                    oReturnValue = propertyInfo.GetValue(paramType, null);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return oReturnValue;
        }

        public static bool SetFieldValue(this object paramType, string propertyName, object propertyValue)
        {
            bool bReturnValue = false;
            try
            {
                PropertyInfo propertyInfo = paramType.GetType().GetProperty(propertyName);
                if (propertyInfo.IsNotNullOrEmpty())
                {
                    bool isNullable = propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                    if (propertyInfo != null && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanWrite)
                    {
                        if (!isNullable)
                        {
                            propertyInfo.SetValue(paramType, (object)propertyValue.ToValueAsString(), null);
                        }
                        else
                        {
                            object convertedValue = null;
                            convertedValue = System.Convert.ChangeType(propertyValue, Nullable.GetUnderlyingType(propertyInfo.PropertyType));
                            propertyInfo.SetValue(paramType, convertedValue, null);
                        }
                    }
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        public static DataRowView hasValue(this DataView dataView, string fieldName, string fieldValue)
        {
            DataRowView selectedRow = null;
            try
            {
                if (!dataView.IsNotNullOrEmpty() || !fieldName.IsNotNullOrEmpty()) return selectedRow;

                dataView.RowFilter = null;
                if (dataView.Count == 0) return selectedRow;

                dataView.RowFilter = fieldName + " = '" + fieldValue + "'";
                if (dataView.Count > 0)
                {
                    selectedRow = dataView[0];
                }

                dataView.RowFilter = null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return selectedRow;
        }

        public static string GetRowFilter(this DataView dataView, string searchValue, string operatorName = "IN", string relationalOperator = "OR")
        {
            if (!dataView.IsNotNullOrEmpty() || !searchValue.IsNotNullOrEmpty()) return string.Empty;
            if (!searchValue.IsNotNullOrEmpty()) searchValue = string.Empty;
            if (!relationalOperator.IsNotNullOrEmpty()) relationalOperator = "OR";
            if (!operatorName.IsNotNullOrEmpty()) operatorName = "IN";

            string[] columnNames = dataView.Table.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

            string sReturnValue = "";

            foreach (DataColumn dataColumn in dataView.Table.Columns)
            {
                string prefix = dataColumn.DataType.Name == "String" ? "'" : "";
                string suffix = dataColumn.DataType.Name == "String" ? "'" : "";

                switch (operatorName.ToUpper())
                {
                    case "IN":
                        sReturnValue += dataColumn.ColumnName + " IN(" + prefix + searchValue + suffix + ") ";
                        break;
                    case "LIKE":
                        //if (dataColumn.DataType.ToString() == "String")
                        sReturnValue += dataColumn.ColumnName + " LIKE " + prefix + searchValue + "%" + suffix;
                        break;
                    default: return null;
                }

                if (dataColumn.GetHashCode() != dataView.Table.Columns[dataView.Table.Columns.Count - 1].GetHashCode())
                {
                    sReturnValue += " " + relationalOperator + " ";
                }
            }
            return sReturnValue;
        }

        public static string ColumnNames(this DataView dataView)
        {
            if (!dataView.IsNotNullOrEmpty()) return string.Empty;

            string[] columnNames = dataView.Table.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            return string.Join(",", columnNames);

        }

        public static bool IsNumeric(this string val, System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.Number)
        {
            if (val.ToValueAsString().Length == 0) return true;
            Double result;
            return Double.TryParse(val, numberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        public static bool HasNonEmptyCells(this DataView dataView)
        {
            bool isFound = false;
            if (!dataView.IsNotNullOrEmpty()) return isFound;

            foreach (DataRowView rowView in dataView)
            {
                foreach (DataColumn col in dataView.Table.Columns)
                {
                    if (rowView[col.ColumnName].IsNotNullOrEmpty() && rowView[col.ColumnName].ToValueAsString().Trim().Length > 0)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;
            }
            return isFound;
        }

        public static bool HasNonEmptyCells(this DataTable dataTable)
        {
            bool isFound = false;
            if (!dataTable.IsNotNullOrEmpty()) return isFound;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (dataRow[col.ColumnName].IsNotNullOrEmpty() && dataRow[col.ColumnName].ToValueAsString().Trim().Length > 0)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;
            }
            return isFound;
        }

        public static byte[] ToObservable<T>(this List<T> lstT)
        {
            return null;
            //return new ObservableCollection<T>(lstT);

            //observableCollection = new ObservableCollection<ForgingMachineTypes>();
            //           foreach (ForgingMachineTypes item in lstEntity)
            //           {
            //               observableCollection.Add(item);
            //           }
        }



        /// <summary>
        /// utilty function to convert string to byte[] .
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static T DeepCopy<T>(this T obj)
        {
            if (obj == null)
                return obj;
            return (T)ProcessDeepCopy(obj);
        }

        private static object ProcessDeepCopy(object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(ProcessDeepCopy(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {
                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    try
                    {
                        object fieldValue = field.GetValue(obj);
                        if (fieldValue == null)
                            continue;
                        field.SetValue(toret, ProcessDeepCopy(fieldValue));
                    }
                    catch (MissingMethodException)
                    {

                    }
                    catch (ArgumentNullException)
                    {

                    }
                    catch (Exception ex)
                    {

                        throw ex.LogException();
                    }
                }
                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
        }


        public static Guid ToGuidValue(this string guidValue)
        {
            Guid returnlValue;
            try
            {
                Guid.TryParse(guidValue, out returnlValue);
                if (!returnlValue.IsNotNullOrEmpty())
                {
                    return Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnlValue;

        }

        public static DateTime? ToDateTimeValue(this string dateTimeValue)
        {
            DateTime returnlValue;
            try
            {
                DateTime dateValue;

                if (dateTimeValue == null || dateTimeValue.ToString().Trim().Length == 0)
                    return null;
                //else if (DateTime.TryParse(dateTimeValue.ToValueAsString(), out dateValue) == true)
                //{
                //    //return dateValue.Day.ToValueAsString() + "/" + dateValue.Month.ToValueAsString() + "/" + dateValue.Year.ToValueAsString();
                //    return dateValue;
                //}
                else
                {

                    string[] cusformat = new string[] { System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern };

                    if (DateTime.TryParseExact(dateTimeValue.ToValueAsString(), cusformat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateValue))
                        return dateValue;
                    else if (DateTime.TryParse(dateTimeValue.ToValueAsString(), out dateValue) == true)
                        return dateValue;
                    else
                    {
                        char[] whitespace = new char[] { ' ', '\t' };
                        char[] dateSeparator = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator.ToCharArray();

                        char[] separator = whitespace.Concat(dateSeparator).ToArray();

                        string[] dtValues = dateTimeValue.Split(separator);
                        string[] dtformats = cusformat[0].Split(separator);

                        string syear = "";
                        string smonth = "";
                        string sday = "";
                        if (dtformats.Length > 0)
                        {
                            for (int index = 0; index < dtformats.Length; index++)
                            {
                                if (dtformats[index].Contains("y"))
                                    syear = dtValues[index];

                                if (dtformats[index].Contains("M") && dtformats[index].Length <= 2)
                                    smonth = dtValues[index];
                                else if (dtformats[index].Contains("M") && dtformats[index].Length > 2)
                                {

                                }

                                if (dtformats[index].Contains("d"))
                                    sday = dtValues[index];

                            }
                        }
                        //string syear = dateTimeValue.Substring(cusformat[0].IndexOf("y"), (cusformat[0].LastIndexOf("y") - cusformat[0].IndexOf("y")) + 1);
                        //string smonth = dateTimeValue.Substring(cusformat[0].IndexOf("M"), (cusformat[0].LastIndexOf("M") - cusformat[0].IndexOf("M")) + 1);
                        //string sday = dateTimeValue.Substring(cusformat[0].IndexOf("d"), (cusformat[0].LastIndexOf("d") - cusformat[0].IndexOf("d")) + 1);

                        int year;
                        int month;
                        int day;

                        if (!int.TryParse(syear, out year))
                        {
                            return null;
                        }

                        if (!int.TryParse(smonth, out month) || smonth.ToIntValue() > 12)
                        {
                            return null;
                        }

                        if (!int.TryParse(sday, out day) || sday.ToIntValue() > 31 || sday.ToIntValue() <= 0)
                        {
                            return null;
                        }

                        DateTime newDateTime = new DateTime(year, month, day);

                        string longDateString = newDateTime.ToLongDateString();
                        return Convert.ToDateTime(longDateString);
                    }

                }

                //string longDateString = Convert.ToDateTime(dateTimeValue).ToLongDateString();
                //if (dateTimeValue == null || dateTimeValue.Equals(DBNull.Value)) return null;
                //DateTime.TryParse(dateTimeValue, out returnlValue);
                //if (!returnlValue.IsNotNullOrEmpty())
                //{
                //    return null;
                //}
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnlValue;

        }

        public static decimal ToDecimalValue(this string decimalValue)
        {
            decimal returnlValue;
            try
            {
                decimal.TryParse(decimalValue, out returnlValue);
                if (!returnlValue.IsNotNullOrEmpty() && !returnlValue.Equals(DBNull.Value))
                {
                    returnlValue = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnlValue;

        }

        public static double ToDoubleValue(this string doubleValue)
        {
            double returnlValue;
            try
            {
                double.TryParse(doubleValue, out returnlValue);
                if (!returnlValue.IsNotNullOrEmpty())
                {
                    returnlValue = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return returnlValue;

        }

        public static int ToIntValue(this string intValue)
        {
            int intVal;
            try
            {
                int.TryParse(intValue, out intVal);
                if (!intVal.IsNotNullOrEmpty())
                {
                    intVal = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return intVal;

        }

        public static string ToDoubleAsString(this string doubleValue, int roundDecimals = 2)
        {
            string returnString = string.Empty;
            try
            {
                double doubleVal;
                double.TryParse(doubleValue, out doubleVal);
                if (!doubleVal.IsNotNullOrEmpty())
                {
                    doubleVal = 0.0;
                }
                doubleVal = Math.Round(doubleVal, roundDecimals);
                returnString = doubleVal.ToValueAsString();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return (returnString);

        }

        public static string ToValueAsString(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return "";
            else
                return Convert.ToString(obj);
        }

        //public static bool ToBoolean(this string obj)
        //{
        //    if (obj == null || obj.Trim().Length == 0 || obj.Trim().ToUpper() == "N" || obj.Trim().ToUpper() == "NO")
        //        return false;
        //    else
        //        return true;
        //}

        public static bool ToBooleanAsString(this object obj)
        {
            if (obj == null || obj.ToString().Trim().Length == 0 || obj.ToString().Trim().ToUpper() == "N" || obj.ToString().Trim().ToUpper() == "NO"
                || obj.ToString().Trim().ToUpper() == "0" ||
                obj.ToString().Trim().ToUpper() == "FALSE" || obj == DBNull.Value)
                return false;
            else
                return true;
        }

        public static string FromBooleanAsString(this bool? obj, bool isSingleChar = false)
        {
            string sBoolShort = "Y";
            string sBoolLong = "YES";
            if (obj == null || obj == false)
            {
                sBoolShort = "N";
                sBoolLong = "NO";
            }
            return isSingleChar ? sBoolShort : sBoolLong;

        }

        public static string ToFormattedDateAsString(this object date, string format = "dd/MM/yyyy")
        {

            try
            {

                if (date == null || date.ToString().Trim().Length == 0)
                    return "";

                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "{0:" + format + "}";
                ci.DateTimeFormat.DateSeparator = "/";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;

                string formatedDate = "";
                Nullable<DateTime> dateValue;
                dateValue = date as Nullable<DateTime>;

                formatedDate = String.Format("{0:" + format + "}", dateValue);

                return formatedDate;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return "";

            }

        }

        public static string ToDateAsString(this object obj, string format = "DD/MM/YYYY")
        {
            DateTime dateValue;

            if (obj == null || obj.ToString().Trim().Length == 0)
                return "";
            else if (DateTime.TryParse(obj.ToValueAsString(), out dateValue) == true)
            {
                //return dateValue.Day.ToValueAsString() + "/" + dateValue.Month.ToValueAsString() + "/" + dateValue.Year.ToValueAsString();
                return dateValue.ToShortDateString();
            }
            else
            {

                string[] cusformat = new string[] { "dd/MM/yyyy" };

                if (DateTime.TryParseExact(obj.ToValueAsString(), cusformat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateValue))
                    return dateValue.ToShortDateString();
                else
                {
                    obj.ToValueAsString().ToDateTimeValue().ToValueAsString();
                    string syear = obj.GetFieldValue("Year").ToValueAsString();
                    string smonth = obj.GetFieldValue("Month").ToValueAsString();
                    string sday = obj.GetFieldValue("Day").ToValueAsString();

                    int year;
                    int month;
                    int day;

                    if (!int.TryParse(syear, out year))
                    {
                        return null;
                    }

                    if (!int.TryParse(smonth, out month) || smonth.ToIntValue() > 12)
                    {
                        return null;
                    }

                    if (!int.TryParse(sday, out day) || sday.ToIntValue() > 31 || sday.ToIntValue() <= 0)
                    {
                        return null;
                    }

                    DateTime newDateTime = new DateTime(year, month, day);

                    return newDateTime.ToFormattedDateAsString();

                    //Console.WriteLine("Invalid");
                }
                //DateTime userDate = Convert.ToDateTime("MM/dd/yyyy");


                //Regex stringDatePattern = new Regex("*[0-9][0-9][/][0-9][0-9][/][0-9][0-9[0-9][0-9]]*");
                //bool a = stringDatePattern.IsMatch(obj.ToValueAsString());


                //Boolean hasDate = false;
                //DateTime dateTime = new DateTime();
                //String[] inputText = obj.ToValueAsString().Split(' '); //split on a whitespace

                //foreach (String text in inputText)
                //{
                //    //Use the Parse() method
                //    try
                //    {
                //        dateTime = DateTime.Parse(text);
                //        hasDate = true;
                //        break; //no need to execute/loop further if you have your date
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                ////after breaking from the foreach loop, you can check if hasDate=true
                ////if it is, then your user entered a date and you can retrieve it from the dateTime 

                //if (hasDate)
                //{
                //    //user entered a date, get it from dateTime
                //}
                //else
                //{
                //    //user didn't enter any date
                //}



                //string[] cusformat = new string[] { "yyyy-MM-dd HH:mm:ss" };
                //string value = obj.ToValueAsString();
                //DateTime datetime;

                //if (DateTime.TryParseExact(value, cusformat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out datetime))
                //    Console.WriteLine("Valid  : " + datetime);
                //else
                //    Console.WriteLine("Invalid");


                //return userDate.ToShortDateString();
            }
        }

        public static string ToString(this OperationMode operationMode)
        {
            try
            {
                return ((OperationMode)operationMode).ToString();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }
        /// <summary>
        /// Helps to find DataSet has table(s) or null
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <returns>True - NonEmpty, False - Empty</returns>
        public static bool IsNotNullOrEmpty(this DataSet dataSet)
        {
            bool hasNotNullOrEmpty = false;
            try
            {
                if (dataSet != null && dataSet.Tables.Count != 0) hasNotNullOrEmpty = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return hasNotNullOrEmpty;
        }

        /// <summary>
        /// Helps to find DataTable is null
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <returns>True - NonEmpty, False - Empty</returns>
        public static bool IsNotNullOrEmpty(this DataTable dataTable)
        {
            bool hasNotNullOrEmpty = false;
            try
            {
                if (dataTable != null) hasNotNullOrEmpty = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return hasNotNullOrEmpty;
        }

        /// <summary>
        /// Helps to find object is null
        /// </summary>
        /// <param name="dataTable">object</param>
        /// <returns>True - NonEmpty, False - Empty</returns>
        public static bool IsNotNullOrEmpty(this object obj)
        {
            bool hasNotNullOrEmpty = false;
            try
            {
                if (obj != null && obj != DBNull.Value)
                {

                    switch (obj.GetType().ToString())
                    {
                        default: hasNotNullOrEmpty = true; break;
                        case "System.String":
                            hasNotNullOrEmpty = (obj as string != null && obj.ToString().Trim().Length != 0) ? true : false; break;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return hasNotNullOrEmpty;
        }

        /// <summary>
        /// Helps to find object is null
        /// </summary>
        /// <param name="dataTable">object</param>
        /// <returns>True - NonEmpty, False - Empty</returns>
        public static bool IsNotNullOrEmpty(this List<StringBuilder> list)
        {
            bool hasNotNullOrEmpty = false;
            try
            {
                if (list != null) hasNotNullOrEmpty = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return hasNotNullOrEmpty;
        }

        /// <summary>
        /// Helps to encrypt the string like password....
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Return encrypted string </returns>
        public static string EncryptPassword(this string password)
        {
            try
            {
                string functionReturnValue = null;
                int nLen = 0;
                string sencr = null;
                string snew = null;
                int nCtr = 0;
                long lngascii = 0;
                if (password.Length != 0)
                {
                    if (password.Length < 16)
                    {
                        password = password.Trim() + "#";
                        if (password.Length < 16)
                        {
                            nLen = 16;
                            for (nCtr = 1; nCtr <= nLen; nCtr++)
                            {

                                password = password + (char)nCtr;
                            }
                        }
                    }
                    else
                    {
                        password = password.Substring(1, 16);
                    }

                    nCtr = 1;
                    foreach (char c in password)
                    {
                        lngascii = c + nCtr;
                        if (lngascii < 40 || lngascii > 123)
                            lngascii = lngascii + 40 + nCtr;

                        if (lngascii == 275)
                        {
                            sencr = sencr + "lo" + "G";
                        }
                        else
                        {
                            sencr = sencr + "lo" + (char)lngascii;
                        }
                        nCtr++;
                    }

                    sencr = sencr.Substring(0, 30);
                    snew = Reverse(sencr);

                    functionReturnValue = snew.Trim();
                }
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Helps to revers the string
        /// </summary>
        /// <param name="Password"></param>
        /// <returns>Return reverse string </returns>
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                //Setting column names as Property names
                //object obj = null;
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    //inserting property values to datatable rows
                    try
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                    catch (TargetParameterCountException)
                    {

                    }
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ToDataTableWithType<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                //Setting column names as Property names
                DataColumn column;
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    column = new DataColumn(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    column = new DataColumn(prop.Name, prop.PropertyType);
                }
                column.AllowDBNull = true;
                dataTable.Columns.Add(column);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    try
                    {
                        //inserting property values to datatable rows
                        values[i] = props[i].GetValue(item, null);
                    }
                    catch (System.Reflection.TargetParameterCountException)
                    {
                        values[i] = null;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static Exception LogException(this Exception ex)
        {
            AppException.Log(ex); return ex;
        }

        public static void ShowAndLogException(this Exception ex)
        {
            AppException.ShowAndLog(ex);
        }


    }
}
