using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using ProcessDesigner.DAL;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Reflection;
using ProcessDesigner.Model;

namespace ProcessDesigner.Common
{
    public abstract class Essential
    {

        private UserInformation _userInformation;
        protected UserInformation userInformation
        {
            get
            {
                return _userInformation;
            }
            set
            {
                _userInformation = value;
                userName = _userInformation.UserName;
                userRole = _userInformation.UserRole;

                if (_userInformation != null && _userInformation.Dal != null)
                {
                    Dal = _userInformation.Dal;
                    DBParameterPrefix = Dal.DBParameterPrefix;

                    _userInformation.SFLPDDatabase = new SFLPD_UAT(Dal.connection);

                    //if (_userInformation.IsNotNullOrEmpty())
                    DB = _userInformation.SFLPDDatabase;
                    DB.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);

                    serverDateTime = Dal.ServerDateTime;
                    serverDate = Dal.ServerDateTime.Date;
                    serverDateTime = Dal.ServerDateTime;

                }
            }
        }
        private bool _isDefaultSubmitRequired = true;
        public bool IsDefaultSubmitRequired
        {
            get { return _isDefaultSubmitRequired; }
            set
            {
                _isDefaultSubmitRequired = value;
            }
        }

        private SFLPD_UAT _db;
        public SFLPD_UAT DB

        //private SFLPD_DevTest _db;
        //public SFLPD_DevTest DB
        {
            get
            {
                if (_userInformation.IsNotNullOrEmpty())
                    _db = _userInformation.SFLPDDatabase;

                System.Data.Linq.ChangeSet cs = _db.GetChangeSet();
                try
                {
                    //if (cs.Inserts.Count > 0 || cs.Updates.Count > 0 || cs.Deletes.Count > 0)
                    if (IsDefaultSubmitRequired)
                    {
                        _db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
                    }
                        
                }
                catch (InvalidOperationException ex)
                {
                    throw ex.LogException(); //when databased Closed Connection
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex.LogException();
                }
                catch (System.Data.Linq.ChangeConflictException)
                {

                    if (cs.Inserts.Count > 0)
                    {
                        _db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, cs.Inserts);
                    }
                    if (cs.Updates.Count > 0)
                    {
                        _db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, cs.Updates);
                    }
                    if (cs.Deletes.Count > 0)
                    {
                        _db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, cs.Deletes);
                    }

                }
                return _db;
            }
            set
            {
                _db = value;
            }

        }

        protected string userName { get; set; }
        protected string userRole { get; set; }

        protected DateTime serverDate { get; set; }
        protected DateTime serverDateTime { get; set; }

        public DataAccessLayer Dal = null;
        protected string DBParameterPrefix { get; set; }
        protected DataSet dsResult = new DataSet();
        protected DataTable dtResult = new DataTable();
        protected StringBuilder sbSQL = new StringBuilder();
        protected List<StringBuilder> sqlList = new List<StringBuilder>();
        protected Dictionary<string, StringBuilder> sqlDictionary = new Dictionary<string, StringBuilder>();
        protected string sql = null;
        protected List<DbParameter> lstDbParameter = new List<DbParameter>();
        protected string tableName = null;
        protected string whereClause = null;
        protected List<string> columnNames = null;

        #region Generate Oracle Parameter
        public ParameterDirection DBOutputParameterDirection() { return ParameterDirection.Output; }

        public ParameterDirection DBInputOutputParameterDirection() { return ParameterDirection.InputOutput; }

        public ParameterDirection DBInputParameterDirection() { return ParameterDirection.Input; }

        public ParameterDirection DBReturnValueParameterDirection() { return ParameterDirection.ReturnValue; }

        public DbParameter DBParameter()
        {
            DbParameter parameter = Dal.CreateParameter();
            return parameter;
        }

        public DbParameter DBParameter(string parameterName, object obj)
        {
            DbParameter parameter = Dal.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = obj;
            return parameter;
        }

        //public DbParameter DBParameter(string parameterName, DbType oraType)
        //{
        //    return new DbParameter(parameterName, oraType);
        //}

        //public DbParameter DBParameter(string parameterName, DbType type, int size)
        //{
        //    return new DbParameter(parameterName, type, size);
        //}

        public DbParameter DBParameter(string parameterName, DbType type, ParameterDirection direction)
        {
            DbParameter parameter = Dal.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = type;
            parameter.Direction = direction;
            return parameter;
        }

        //public DbParameter DBParameter(string parameterName, DbType type, int size, string srcColumn)
        //{
        //    return new DbParameter(parameterName, type, size, srcColumn);
        //}

        public DbParameter DBParameter(string parameterName, DbType type, object obj, ParameterDirection direction)
        {
            DbParameter parameter = Dal.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = type;
            parameter.Value = obj;
            parameter.Direction = direction;
            return parameter;
        }

        public DbParameter DBParameter(string parameterName, DbType type, int size, object obj, ParameterDirection direction)
        {
            DbParameter parameter = Dal.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = type;
            parameter.Size = size;
            parameter.Value = obj;
            parameter.Direction = direction;
            return parameter;
        }

        public DbParameter DBParameter(string parameterName, DbType type, object obj)
        {
            DbParameter parameter = Dal.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = type;
            parameter.Value = obj;
            return parameter;
        }

        //public DbParameter DBParameter(string parameterName, DbType oraType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object obj)
        //{
        //    return new DbParameter(parameterName, oraType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, obj);
        //}
        #endregion

        /// <summary>
        /// Helps to generate next number of the table
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="whereClause">Optional if want to generate next number based on contition</param>
        /// <param name="defaultIfNull">optional if specify the default value of the column it returns null value</param>
        /// <param name="lstDbParameter">optional, specify the bindiing variables if you used in the where clause</param>
        /// <returns>string</returns>
        public string GenerateNextNumber(string tableName, string columnName, string whereClause = null, string defaultIfNull = null,
            List<DbParameter> lstDbParameter = null)
        {
            object nextNumber = defaultIfNull;
            try
            {
                if (String.IsNullOrEmpty(tableName)) throw new Exception("Table Name should not be empty");
                if (String.IsNullOrEmpty(columnName)) throw new Exception("Column Name should not be empty");

                tableName = tableName.Trim();
                tableName = (tableName.ToUpper().IndexOf("DBO.") > 0 ? tableName : "DBO." + tableName);

                columnName = columnName.Trim();

                string columnSQL = "MAX( " + columnName + " ) + 1";
                if (String.IsNullOrEmpty(defaultIfNull))
                {
                    //columnSQL = "NVL( " + columnSQL + " , " + defaultIfNull + " )";
                }

                sbSQL.Append("SELECT " + columnSQL + " AS NextNumber ");
                sbSQL.Append("    FROM " + tableName);
                if (!String.IsNullOrEmpty(whereClause))
                {
                    sbSQL.Append(" " + (whereClause.ToUpper().IndexOf("WHERE") > 0 ? whereClause : "WHERE " + whereClause));
                }
                nextNumber = Dal.GetScalarValue(sbSQL, lstDbParameter);

                if (!nextNumber.IsNotNullOrEmpty())
                    nextNumber = defaultIfNull;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {

            }
            return nextNumber.ToValueAsString();
        }

        /// <summary>
        /// Helps to get data of a table
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="whereClause">Optional if want to generate next number based on contition</param>
        /// <param name="lstDbParameter">optional, specify the bindiing variables if you used in the where clause</param>
        /// <param name="columnNames">optional, specify the list of columns</param>
        /// <returns></returns>
        public DataTable GetTableData(string tableName, string whereClause = null, List<DbParameter> lstDbParameter = null, List<string> columnNames = null)
        {
            DataTable dataTable = new DataTable();
            StringBuilder columnSQL = null;
            try
            {
                if (String.IsNullOrEmpty(tableName)) throw new Exception("Table Name should not be empty");
                if (!columnNames.IsNotNullOrEmpty() || columnNames.Count == 0) columnNames.Add("*");

                tableName = tableName.Trim();
                //tableName = (tableName.ToUpper().IndexOf("DBO.") > 0 ? tableName : "DBO." + tableName);

                columnSQL = new StringBuilder("");
                foreach (string colName in columnNames)
                {
                    columnSQL.AppendLine(colName);
                    if (colName.GetHashCode() != columnNames[columnNames.Count - 1].GetHashCode())
                    {
                        columnSQL.Append(",");
                    }
                }

                sbSQL.Append("SELECT ");
                sbSQL.Append(columnSQL.ToString());
                sbSQL.Append("    FROM " + tableName);
                if (!String.IsNullOrEmpty(whereClause))
                {
                    sbSQL.Append(" " + (whereClause.ToUpper().IndexOf("WHERE") > 0 ? whereClause : "WHERE " + whereClause));
                }

                dataTable = Dal.GetDataTable(sbSQL, lstDbParameter);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (columnSQL.IsNotNullOrEmpty()) columnSQL.Clear();
                columnSQL = null;
            }
            return dataTable;
        }

        public DataTable GetAllPartCodeAndDescription()
        {
            DataTable dataTable = new DataTable();
            List<string> columnNames = null;
            try
            {
                string tableName = "prd_mast";
                columnNames = new List<string>();
                columnNames.Add("part_no");
                columnNames.Add("part_desc");
                dataTable = GetTableData(tableName, null, null, columnNames);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (columnNames.IsNotNullOrEmpty()) columnNames.Clear();
                columnNames = null;
            }
            return dataTable;
        }

        public string ToDbParameter(string dbParameterName)
        {
            dbParameterName = dbParameterName.IndexOf(Dal.DBParameterPrefix) > 0 ? dbParameterName : Dal.DBParameterPrefix + dbParameterName;
            return dbParameterName;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ToDataTableWithType<T>(List<T> items)
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
                    //inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// Used to get description of tables
        /// </summary>
        /// <param name="tableNames">List of table Names</param>
        /// <returns>List of V_TABLE_DESCRIPTION</returns>
        public List<V_TABLE_DESCRIPTION> GetTableColumnsSize(params string[] tableNames)
        {
            List<V_TABLE_DESCRIPTION> tables = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return tables;
                if (tableNames.Length != 0)
                {
                    string[] _tableNames = (from row in tableNames.AsEnumerable()
                                            select row.ToUpper()).ToArray<string>();

                    tables = (from row in DB.V_TABLE_DESCRIPTION
                              where _tableNames.Contains(row.TABLE_NAME.ToUpper()) == true
                              select row).ToList<V_TABLE_DESCRIPTION>();
                }
                else
                {
                    tables = (from row in DB.V_TABLE_DESCRIPTION
                              select row).ToList<V_TABLE_DESCRIPTION>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return tables;

        }

        /// <summary>
        /// Used to get description of the table
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnNames">List of Columns</param>
        /// <returns>List of V_TABLE_DESCRIPTION object</returns>
        public List<V_TABLE_DESCRIPTION> GetColumnsSize(string tableName, params string[] columnNames)
        {

            List<V_TABLE_DESCRIPTION> columns = null;
            try
            {
                if (!DB.IsNotNullOrEmpty() || !tableName.IsNotNullOrEmpty()) return columns;
                if (columnNames.Length != 0)
                {

                    columns = (from row in DB.V_TABLE_DESCRIPTION
                               where row.TABLE_NAME == tableName && columnNames.Contains(row.COLUMN_NAME) == true
                               select row).ToList<V_TABLE_DESCRIPTION>();
                }
                else
                {
                    columns = (from row in DB.V_TABLE_DESCRIPTION
                               where row.TABLE_NAME == tableName
                               select row).ToList<V_TABLE_DESCRIPTION>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return columns;

        }

        /// <summary>
        /// Used to set rights of the Action Buttones
        /// </summary>
        /// <param name="sourcename">Name of Form</param>
        /// <returns>RolePermission</returns>
        public RolePermission GetUserRights(string sourcename)
        {
            List<SEC_ROLE_OBJECT_PERMISSION> lstrolePermission = null;
            RolePermission permission = null;
            try
            {
                if (sourcename.IsNotNullOrEmpty())
                {
                    lstrolePermission = (from row in DB.SEC_ROLE_OBJECT_PERMISSION
                                         where ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false) || (row.DELETE_FLAG == null)) &&
                                         row.OBJECT_NAME.ToUpper() == sourcename.ToUpper() &&
                                         row.ROLE_NAME.ToUpper() == userInformation.UserRole.ToUpper()
                                         select row).ToList<SEC_ROLE_OBJECT_PERMISSION>();

                    //if (!rolePermission.IsNotNullOrEmpty())
                    //{
                    //    rolePermission = new SEC_ROLE_OBJECT_PERMISSION();
                    //    rolePermission.PERM_ADD = true;
                    //    rolePermission.PERM_EDIT = true;
                    //    rolePermission.PERM_VIEW = true;
                    //    rolePermission.PERM_DELETE = true;
                    //    rolePermission.PERM_PRINT = true;
                    //}

                    permission = new RolePermission();
                    permission.SourceName = sourcename;
                    permission.AddNew = false;
                    permission.Edit = false;
                    permission.View = false;
                    permission.Delete = false;
                    permission.Print = false;

                    foreach (SEC_ROLE_OBJECT_PERMISSION objPermission in lstrolePermission)
                    {
                        if (objPermission.PERM_ADD.ToBooleanAsString() == true) permission.AddNew = objPermission.PERM_ADD.ToBooleanAsString();
                        if (objPermission.PERM_EDIT.ToBooleanAsString() == true) permission.Edit = objPermission.PERM_EDIT.ToBooleanAsString();
                        if (objPermission.PERM_VIEW.ToBooleanAsString() == true) permission.View = objPermission.PERM_VIEW.ToBooleanAsString();
                        if (objPermission.PERM_DELETE.ToBooleanAsString() == true) permission.Delete = objPermission.PERM_DELETE.ToBooleanAsString();
                        if (objPermission.PERM_PRINT.ToBooleanAsString() == true) permission.Print = objPermission.PERM_PRINT.ToBooleanAsString();
                        permission.Save = true;
                        permission.Close = true;
                        permission.Copy = true;
                        permission.Search = true;
                        permission.StandartNotes = true;
                        permission.CostSheetSearch = true;
                        permission.ProductSearch = true;
                        permission.SimilarPartNumber = true;
                        permission.ShowRelated = true;
                        permission.ReleaseDocument = true;
                        permission.UpdateOrderProcessing = true;
                        permission.CreateCIReference = true;

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (lstrolePermission.IsNotNullOrEmpty()) lstrolePermission.Clear();
                lstrolePermission = null;                
            }
            return permission;
        }

        public DateTime? ServerDateTime()
        {
            return serverDate;
        }

    }

}
