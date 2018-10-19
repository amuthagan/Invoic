using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using ProcessDesigner.Common;
using System.Net.NetworkInformation;
using System.Data.SqlClient;
using ProcessDesigner.Model;
using System.Reflection;

namespace ProcessDesigner.DAL
{
    public class DataAccessLayer
    {
        ProcessSheetArrayModel model = new ProcessSheetArrayModel();
        private bool isTransactionEsatablished = false;
        private string providerName { get; set; }

        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        private string connectionString { get; set; }

        private DbTransaction transaction = null;
        public DbConnection connection { get; set; }
        private DbProviderFactory providerFactory { get; set; }

        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        private string DataSource
        {
            get
            {
                if (connection.IsNotNullOrEmpty())
                    return connection.DataSource;
                else return null;
            }
        }

        private ConnectionState State
        {
            get
            {
                if (connection.IsNotNullOrEmpty())
                    return connection.State;
                else return ConnectionState.Closed;
            }
        }

        //public SFLPD SFLPDDatabase { get; set; }

        public string DBParameterPrefix { get; set; }

        /// <summary>
        /// Gets the time to wait while establishing a connection before terminating the attempt and generating an error.
        /// </summary>
        public int? ConnectionTimeout
        {
            get
            {
                if (connection.IsNotNullOrEmpty())
                    return connection.ConnectionTimeout;
                else return null;
            }
        }

        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string  efore the connection is opened.
        /// </summary>
        public string Database
        {
            get
            {
                if (connection.IsNotNullOrEmpty())
                    return connection.Database;
                else return null;
            }
        }

        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected.
        /// </summary>
        public string ServerVersion
        {
            get
            {
                if (connection.IsNotNullOrEmpty())
                    return connection.ServerVersion;
                else return null;
            }
        }

        /// <summary>
        /// Gets datatime that represents the datetime of the server to which the object is connected.
        /// </summary>
        public DateTime ServerDateTime
        {
            get
            {
                DateTime dateTime = new DateTime();

                if (providerName == "Oracle.DataAccess.Client" || providerName == "System.Data.OracleClient")
                {
                    DataTable dt = GetDataTable(new StringBuilder().AppendLine("Select TO_Date(sysdate, 'DD/MON/YYYY HH24:MI:SS')"));
                    if (dt.IsNotNullOrEmpty() && dt.Rows.Count > 0)
                    {
                        dateTime = (DateTime)dt.Rows[0][0];
                    }
                }
                else if (providerName == "System.Data.SqlClient")
                {
                    DataTable dt = GetDataTable(new StringBuilder().AppendLine("Select GetDate()"));
                    if (dt.IsNotNullOrEmpty() && dt.Rows.Count > 0)
                    {
                        dateTime = (DateTime)dt.Rows[0][0];
                    }
                }
                else if (providerName == "System.Data.Odbc" || providerName == "System.Data.OleDb")
                {
                }

                return dateTime;
            }
        }

        /// <summary>
        /// Used to Initialize Data Access Layer
        /// </summary>
        /// <param name="connectionStringName">Name of the ConnectionString as per config file</param>
        public DataAccessLayer(string connectionStringName)
        {
            this.providerName = GetProviderNameByConnectionStringName(connectionStringName);
            this.connectionString = GetConnectionStringByProviderName(this.providerName);
            InitializeDataAccessLayer(providerName, connectionString);
        }

        /// <summary>
        /// Used to Initialize Data Access Layer
        /// </summary>
        /// <param name="providerName">Name of the Provider</param>
        /// <param name="connectionString">ConnectionString of the Provider</param>
        public DataAccessLayer(string providerName, string connectionString)
        {
            InitializeDataAccessLayer(providerName, connectionString);
        }

        /// <summary>
        /// Used to Initialize Data Access Layer
        /// </summary>
        /// <param name="providerName">Name of the Provider</param>
        /// <param name="connectionString">ConnectionString of the Provider</param>
        private void InitializeDataAccessLayer(string providerName, string connectionString)
        {
            try
            {
                if (String.IsNullOrEmpty(providerName))
                    throw new ArgumentNullException("Provider Name", "Provider Name should not be empty");

                ////To Do - Check Provider is installed in the Current Machine

                if (String.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException("Connection String", "Connection String should not be empty");

                this.providerName = providerName;
                this.connectionString = Cipher.Decrypt(connectionString, true);

                providerFactory = DbProviderFactories.GetFactory(providerName);

                connection = providerFactory.CreateConnection();
                connection.ConnectionString = this.connectionString;

                ////Assign Parameter prefix for specified provider
                if (!DBParameterPrefix.IsNotNullOrEmpty())
                {
                    if (providerName == "Oracle.DataAccess.Client")
                    {
                        DBParameterPrefix = ":";
                    }
                    else if (providerName == "System.Data.SqlClient" || providerName == "System.Data.OracleClient" ||
                             providerName == "System.Data.Odbc" || providerName == "System.Data.OleDb")
                    {
                        DBParameterPrefix = "@";
                    }
                    else
                    {
                        //For OLEDB, ODBC databases
                        DBParameterPrefix = "?";
                    }
                }
                //OpenPDDatabase();
                DateTime dt = ServerDateTime;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Used to get connection string for the specified provider name & ConnectionString name
        /// </summary>
        /// <param name="providerName">Name of the provider like 'Oracle.DataAccess.Client','System.Data.SqlClient','System.Data.OracleClient','System.Data.Odbc','System.Data.OleDb...</param>
        /// <param name="connectionStringName">Name of the ConnectionString Name as per config file</param>
        /// <returns>Connection String</returns>
        private string GetConnectionStringByProvider(string providerName, string connectionStringName)
        {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if ((cs.ProviderName == providerName && !connectionStringName.IsNotNullOrEmpty()) ||
                    (cs.ProviderName == providerName && connectionStringName.IsNotNullOrEmpty() &&
                        cs.Name == connectionStringName))
                        returnValue = cs.ConnectionString;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Used to get the name of provider by using name of connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the ConnectionString Name as per config file</param>
        /// <returns>Name of the provider</returns>
        private string GetProviderNameByConnectionStringName(string connectionStringName)
        {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (connectionStringName.IsNotNullOrEmpty() && cs.Name == connectionStringName)
                    {
                        returnValue = cs.ProviderName;
                        break;
                    }

                }
            }
            return returnValue;
        }

        /// <summary>
        /// Used to get connection string for the specified provider name
        /// </summary>
        /// <param name="providerName">Name of the provider like 'Oracle.DataAccess.Client','System.Data.SqlClient','System.Data.OracleClient','System.Data.Odbc','System.Data.OleDb...
        /// </param>
        /// <returns>Connection String</returns>
        private string GetConnectionStringByProviderName(string providerName)
        {
            string returnValue = null;

            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                    {
                        returnValue = cs.ConnectionString;
                        break;
                    }

                }
            }
            return returnValue;
        }

        ///// <summary>
        ///// Used to connect database
        ///// </summary>
        ///// <returns></returns>
        //public ConnectionState OpenPDDatabase()
        //{
        //    ConnectionState conState = ConnectionState.Closed;
        //    try
        //    {
        //        if (String.IsNullOrEmpty(connectionString)) throw new Exception("Connection String should not be empty");
        //        SFLPDDatabase = new SFLPD(connection);
        //        conState = ConnectionState.Open;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //    return conState;
        //}

        private string getServerIPAddress(string connectionString)
        {
            string hostAddress = "";
            try
            {
                DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                builder.ConnectionString = connectionString;

                foreach (string key in builder.Keys)
                {
                    if (hostAddress.IsNotNullOrEmpty()) break;
                    switch (key.ToValueAsString().ToUpper())
                    {
                        case "DATA SOURCE":
                            string dataSource = builder[key].ToValueAsString();
                            char[] delimiterChars = { '\\' };

                            foreach (string itm in dataSource.Split(delimiterChars))
                            {
                                System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
                                System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();

                                // Use the default Ttl value which is 128, // but change the fragmentation behavior.
                                options.DontFragment = true;

                                // Create a buffer of 32 bytes of data to be transmitted. 
                                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                                byte[] buffer = Encoding.ASCII.GetBytes(data);
                                int timeout = 120;
                                System.Net.IPAddress[] ipAddress = null;
                                try
                                {
                                    ipAddress = System.Net.Dns.GetHostAddresses(itm);
                                }
                                catch (System.Net.Sockets.SocketException)
                                {
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                }

                                if (!ipAddress.IsNotNullOrEmpty() || ipAddress.Length == 0) break;

                                try
                                {

                                    System.Net.NetworkInformation.PingReply reply = pingSender.Send(ipAddress[0], timeout, buffer, options);
                                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                                    {
                                        hostAddress = reply.Address.ToString();
                                    }
                                }
                                catch (PingException ex)
                                {
                                    ex.LogException();
                                    hostAddress = "";
                                    break;
                                }
                                catch (Exception e)
                                {
                                    hostAddress = "";
                                    throw e.LogException();
                                }
                                if (hostAddress.IsNotNullOrEmpty()) break;
                            }

                            break;
                    }
                }
                //Console.WriteLine("{0}={1}", key, builder[key]);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return hostAddress;
        }

        private static void PingCompletedCallback(object sender, System.Net.NetworkInformation.PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user. 
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");

                // Let the main thread resume.  
                // UserToken is the AutoResetEvent object that the main thread  
                // is waiting for.
                ((System.Threading.AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user. 
            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());

                // Let the main thread resume. 
                ((System.Threading.AutoResetEvent)e.UserState).Set();
            }

            System.Net.NetworkInformation.PingReply reply = e.Reply;

            DisplayReply(reply);

            // Let the main thread resume.
            ((System.Threading.AutoResetEvent)e.UserState).Set();
        }

        public static bool PingServer(string ipaddress)
        {
            bool isPingComplete = false;
            try
            {
                if (!ipaddress.IsNotNullOrEmpty()) return isPingComplete;
                //throw new ArgumentException("Ping needs a host or IP Address.");

                string who = ipaddress;
                System.Threading.AutoResetEvent waiter = new System.Threading.AutoResetEvent(false);

                System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();

                // When the PingCompleted event is raised, 
                // the PingCompletedCallback method is called.
                pingSender.PingCompleted += new System.Net.NetworkInformation.PingCompletedEventHandler(PingCompletedCallback);

                // Create a buffer of 32 bytes of data to be transmitted. 
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                // Wait 12 seconds for a reply. 
                int timeout = 1000;

                // Set options for transmission: 
                // The data can go through 64 gateways or routers 
                // before it is destroyed, and the data packet 
                // cannot be fragmented.
                System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions(64, true);

                Console.WriteLine("Time to live: {0}", options.Ttl);
                Console.WriteLine("Don't fragment: {0}", options.DontFragment);

                // Send the ping asynchronously. 
                // Use the waiter as the user token. 
                // When the callback completes, it can wake up this thread.
                pingSender.SendAsync(who, timeout, buffer, options, waiter);

                // Prevent this example application from ending. 
                // A real application should do something useful 
                // when possible.
                waiter.WaitOne();
                Console.WriteLine("Ping example completed.");
                isPingComplete = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return isPingComplete;
        }

        public static void DisplayReply(System.Net.NetworkInformation.PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
        }

        /// <summary>
        /// Used to connect database
        /// </summary>
        /// <returns></returns>
        private ConnectionState Open()
        {
            ConnectionState conState = ConnectionState.Closed;
            try
            {
                if (String.IsNullOrEmpty(connectionString)) throw new Exception("Connection String should not be empty");
                //if (!getServerIPAddress(connectionString).IsNotNullOrEmpty()) throw new Exception("Unable to Connect Server...\r\nTry again later");

                if (connection == null) connection = providerFactory.CreateConnection();
                if (connection.State == ConnectionState.Open)
                {
                    conState = connection.State;
                }
                else
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    conState = connection.State;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return conState;
        }

        /// <summary>
        /// Helps to copy one parameter into another 
        /// </summary>
        /// <param name="dbParameter">Source of the Parameter</param>
        /// <returns>Copy of Source Parameter</returns>
        private DbParameter copyParameter(DbParameter dbParameter)
        {
            DbParameter dbParameterCopy = null;
            try
            {
                dbParameterCopy = providerFactory.CreateParameter();
                dbParameterCopy.DbType = dbParameter.DbType;
                dbParameterCopy.Direction = dbParameter.Direction;
                dbParameterCopy.IsNullable = dbParameter.IsNullable;
                dbParameterCopy.ParameterName = dbParameter.ParameterName.StartsWith(DBParameterPrefix) ? dbParameter.ParameterName : DBParameterPrefix + dbParameter.ParameterName;
                dbParameterCopy.Size = dbParameter.Size;
                dbParameterCopy.SourceColumn = dbParameter.SourceColumn;
                dbParameterCopy.SourceColumnNullMapping = dbParameter.SourceColumnNullMapping;
                dbParameterCopy.SourceVersion = dbParameter.SourceVersion;
                dbParameterCopy.Value = dbParameter.Value;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dbParameterCopy;
        }

        /// <summary>
        /// Used to get Datasource information of the provider
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ProviderFactoryDataSources()
        {
            DataTable dtResult = null;
            try
            {
                if (providerFactory.CanCreateDataSourceEnumerator)
                {
                    dtResult = providerFactory.CreateDataSourceEnumerator().GetDataSources();
                    dtResult.TableName = "DatabasesServerInformation";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dtResult;
        }

        /// <summary>
        /// Used to get all installed providers in current system
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable InstalledProviders()
        {
            DataTable dtResult = null;
            try
            {
                if (providerFactory.CanCreateDataSourceEnumerator)
                {
                    dtResult = DbProviderFactories.GetFactoryClasses();
                    dtResult.TableName = "InstalledProviders";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dtResult;
        }

        /// <summary>
        /// Used to get provider object information
        /// </summary>
        /// <param name="dbObjects">Name of the object like 'Tables','Views','Procedures','Functions','Triggers','Packages'...</param>
        /// <returns>DataSet</returns>
        public DataSet ObjectsInformation(params string[] dbObjects)
        {
            DataSet dsResult = null;

            try
            {
                if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");
                dsResult = new DataSet();
                if (dbObjects.Length > 0)
                {
                    foreach (string dbObj in dbObjects)
                    {
                        if (dbObj.IsNotNullOrEmpty())
                        {
                            DataTable dt = connection.GetSchema(dbObj.ToValueAsString());
                            dt.TableName = dbObj.ToValueAsString();
                            dsResult.Tables.Add(dt);
                        }
                    }
                }
                else
                {
                    DataTable dt = connection.GetSchema();
                    dt.TableName = "SchemaInformation";
                    dsResult.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return dsResult;

        }

        /// <summary>
        /// Used to close the connected database connection
        /// </summary>
        /// <returns></returns>
        public ConnectionState Close()
        {
            try
            {
                if (connection == null) return ConnectionState.Closed;
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                isTransactionEsatablished = false;
            }
            return connection.State;
        }

        /// <summary>
        /// Used to get GetDataSet list of given query
        /// </summary>
        /// <param name="sqlBuilderList">List of SQLBuilder</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(List<SQLBuilder> sqlBuilderList)
        {
            DataSet ds = new DataSet();
            DbCommand cmd = null;
            try
            {
                if (!sqlBuilderList.IsNotNullOrEmpty()) throw new Exception("Query should not be empty");

                if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");

                int iCount = 0;
                foreach (SQLBuilder sqlbuilder in sqlBuilderList)
                {
                    DataTable dt = new DataTable("DataTable" + iCount.ToString());

                    cmd = connection.CreateCommand();
                    cmd.CommandText = sqlbuilder.SQL.ToValueAsString();

                    cmd.Parameters.Clear();
                    if (sqlbuilder.Parameters.IsNotNullOrEmpty())
                    {
                        foreach (DbParameter dbParameter in sqlbuilder.Parameters)
                        {
                            DbParameter dbParameterCopy = copyParameter(dbParameter);
                            if (dbParameterCopy.IsNotNullOrEmpty()) cmd.Parameters.Add(dbParameterCopy);
                        }
                    }

                    DbDataAdapter ad = providerFactory.CreateDataAdapter();
                    ad.SelectCommand = cmd;
                    ad.Fill(dt);
                    ds.Tables.Add(dt);
                    iCount++;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sqlBuilderList.IsNotNullOrEmpty()) sqlBuilderList.Clear();
                sqlBuilderList = null;
                if (cmd.Parameters.IsNotNullOrEmpty()) cmd.Parameters.Clear();
                cmd = null;
                if (!isTransactionEsatablished == true) Close();

            }

            return ds;
        }

        /// <summary>
        /// Used to get GetDataSet of a given query
        /// </summary>
        /// <param name="sql">>Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(List<StringBuilder> sqlList, List<DbParameter> lstParameter = null)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SQLBuilder> sqlBuilderList = null;

                sqlBuilderList = sqlList.AsEnumerable()
                        .Select(row =>
                                new SQLBuilder()
                                {
                                    SQL = row,
                                    Parameters = lstParameter
                                }).ToList();

                ds = GetDataSet(sqlBuilderList);

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sqlList.IsNotNullOrEmpty()) sqlList.Clear();
                sqlList = null;
                if (lstParameter.IsNotNullOrEmpty())
                    lstParameter.Clear();
                lstParameter = null;

            }

            return ds;
        }

        /// <summary>
        /// Used to get GetDataSet list of given query and assings table name as key
        /// </summary>
        /// <param name="sqlBuilderDictionary">List of SQLBuilder</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(Dictionary<string, SQLBuilder> sqlBuilderDictionary)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SQLBuilder> sqlBuilderList = null;

                sqlBuilderList = sqlBuilderDictionary.AsEnumerable()
                        .Select(row =>
                                new SQLBuilder()
                                {
                                    SQL = row.Value.SQL,
                                    Parameters = row.Value.Parameters
                                }).ToList();

                ds = GetDataSet(sqlBuilderList);

                if (ds.IsNotNullOrEmpty())
                {
                    int iCount = 0;
                    foreach (KeyValuePair<string, SQLBuilder> keyValuePair in sqlBuilderDictionary)
                    {
                        if (keyValuePair.Value != null && !String.IsNullOrEmpty(keyValuePair.Value.ToString()) && !String.IsNullOrEmpty(keyValuePair.Key.ToString()) && ds.Tables[iCount] != null)
                            ds.Tables[iCount].TableName = keyValuePair.Key.ToString();
                        iCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sqlBuilderDictionary.IsNotNullOrEmpty()) sqlBuilderDictionary.Clear();
                sqlBuilderDictionary = null;
            }

            return ds;
        }

        /// <summary>
        /// Used to get GetDataSet of a given query and assings table name as key
        /// </summary>
        /// <param name="sql">>Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(Dictionary<string, StringBuilder> sqlDictionary, List<DbParameter> lstParameter = null)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SQLBuilder> sqlBuilderList = null;

                sqlBuilderList = sqlDictionary.AsEnumerable()
                        .Select(row =>
                                new SQLBuilder()
                                {
                                    SQL = row.Value,
                                    Parameters = lstParameter
                                }).ToList();

                ds = GetDataSet(sqlBuilderList);

                if (ds.IsNotNullOrEmpty())
                {
                    int iCount = 0;
                    foreach (KeyValuePair<string, StringBuilder> keyValuePair in sqlDictionary)
                    {
                        if (keyValuePair.Value != null && !String.IsNullOrEmpty(keyValuePair.Value.ToString()) && !String.IsNullOrEmpty(keyValuePair.Key.ToString()) && ds.Tables[iCount] != null)
                            ds.Tables[iCount].TableName = keyValuePair.Key.ToString();
                        iCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sqlDictionary.IsNotNullOrEmpty()) sqlDictionary.Clear();
                sqlDictionary = null;
            }

            return ds;
        }

        /// <summary>
        /// Used to get GetDataSet of a given query
        /// </summary>
        /// <param name="sql">>Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(StringBuilder sbSQL, List<DbParameter> lstParameter = null)
        {

            DataSet ds = new DataSet();
            try
            {
                List<SQLBuilder> sqlList = new List<SQLBuilder>();
                sqlList.Add(new SQLBuilder() { SQL = sbSQL, Parameters = lstParameter });
                ds = GetDataSet(sqlList);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
                sbSQL = null;
            }

            return ds;
        }

        /// <summary>
        /// Used to get DataTable of a given query
        /// </summary>
        /// <param name="sql">>Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(StringBuilder sbSQL, List<DbParameter> lstParameter = null)
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                List<SQLBuilder> sqlList = new List<SQLBuilder>();
                sqlList.Add(new SQLBuilder() { SQL = sbSQL, Parameters = lstParameter });
                ds = GetDataSet(sqlList);
                if (ds.IsNotNullOrEmpty())
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
                sbSQL = null;
            }

            if (!ds.IsNotNullOrEmpty()) return null;
            return dt;
        }

        /// <summary>
        /// Used to get DataTable of a given query
        /// </summary>
        /// <param name="sql">>Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sql, List<DbParameter> lstParameter = null)
        {
            StringBuilder sbSQL = new StringBuilder().Append(sql);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                List<SQLBuilder> sqlList = new List<SQLBuilder>();
                sqlList.Add(new SQLBuilder() { SQL = sbSQL, Parameters = lstParameter });
                ds = GetDataSet(sqlList);
                if (ds.IsNotNullOrEmpty())
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
                sbSQL = null;
                sql = null;
            }

            if (!ds.IsNotNullOrEmpty()) return null;
            return ds.Tables[0];
        }

        /// <summary>
        /// Used to create transaction
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    Open();
                }
                transaction = connection.BeginTransaction();
                isTransactionEsatablished = true;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        /// <summary>
        /// Used to create transaction
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (transaction == null) throw new Exception("Transaction should be initialized");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                ex.LogException();
                try
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            finally
            {
                isTransactionEsatablished = false;
            }
        }

        public DbParameter CreateParameter()
        {
            return providerFactory.CreateParameter();
        }

        /// <summary>
        /// Used to create transaction
        /// </summary>
        public void RollBackTransaction()
        {
            try
            {
                if (transaction == null) throw new Exception("Transaction should be initialized");
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                isTransactionEsatablished = false;
            }
        }

        /// <summary>
        /// Used to get scalar value query
        /// </summary>
        /// <param name="sbSQL">Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>object</returns>
        public object GetScalarValue(StringBuilder sbSQL, List<DbParameter> lstParameter = null)
        {
            object scalarVal = null;
            DbCommand cmdScal = null;
            try
            {
                if (!sbSQL.IsNotNullOrEmpty()) return scalarVal;
                if (Open() != ConnectionState.Open) return scalarVal;
                cmdScal = connection.CreateCommand();
                if (!sbSQL.ToValueAsString().IsNotNullOrEmpty()) return scalarVal;
                cmdScal.CommandText = sbSQL.ToValueAsString();
                if (lstParameter != null)
                {
                    foreach (DbParameter oracleParameter in lstParameter)
                    {
                        cmdScal.Parameters.Add(oracleParameter);
                    }
                }
                scalarVal = cmdScal.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                cmdScal.Dispose();
                if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
                sbSQL = null;
                if (!isTransactionEsatablished == true) Close();

            }

            return scalarVal;
        }

        /// <summary>
        /// Used to executed the stored procedure
        /// </summary>
        /// <param name="sql">Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>Boolean, True - Successful execution, False - Failure execution</returns>
        public bool ExecuteStoredProcedure(string sql, List<DbParameter> lstParameter = null)
        {
            bool isExecuted = false;
            DbCommand cmd = null;
            try
            {
                if (String.IsNullOrEmpty(sql)) return isExecuted;
                if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");
                cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (lstParameter != null)
                {
                    foreach (DbParameter oracleParameter in lstParameter)
                    {
                        cmd.Parameters.Add(oracleParameter);
                    }
                }
                cmd.ExecuteNonQuery();
                isExecuted = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (!isTransactionEsatablished == true) Close();
            }
            return isExecuted;
        }

        /// <summary>
        /// Used to Executed the Non-Queries
        /// </summary>
        /// <param name="sqlBuilder">List of SQLBuilder</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteNonQuery(List<SQLBuilder> sqlBuilder)
        {
            DataTable executedResult = null;
            DbCommand cmd = null;
            try
            {
                if (!sqlBuilder.IsNotNullOrEmpty()) return executedResult;
                if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");

                executedResult = new DataTable();
                executedResult.Columns.Add(new DataColumn("AutoNumber", System.Type.GetType("System.Int32")));
                executedResult.Columns.Add(new DataColumn("ExecutedCount", System.Type.GetType("System.Int32")));

                Int32 autoNumber = 0;
                foreach (SQLBuilder sql in sqlBuilder)
                {
                    if (sql.ToValueAsString().IsNotNullOrEmpty())
                    {
                        Int32 count = -1;

                        DataRow dataRow = executedResult.Rows.Add();
                        dataRow["AutoNumber"] = autoNumber++;
                        dataRow["ExecutedCount"] = DBNull.Value;

                        cmd = connection.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql.ToValueAsString();
                        if (sql.Parameters.IsNotNullOrEmpty())
                        {
                            cmd.Parameters.Clear();
                            foreach (DbParameter dbParameter in sql.Parameters)
                            {
                                DbParameter dbParameterCopy = copyParameter(dbParameter);
                                if (dbParameterCopy.IsNotNullOrEmpty()) cmd.Parameters.Add(dbParameterCopy);
                            }
                        }
                        count = cmd.ExecuteNonQuery();
                        dataRow["ExecutedCount"] = count;
                        //dataRow.AcceptChanges();
                        executedResult.Rows.Add(dataRow);
                        executedResult.AcceptChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (!isTransactionEsatablished == true) Close();
            }
            return executedResult;
        }

        /// <summary>
        /// Used to Executed the Non-Queries
        /// </summary>
        /// <param name="sql">Database Query</param>
        /// <param name="lstParameter">List of Parameter</param>
        /// <returns>Boolean</returns>
        public Int32? ExecuteNonQuery(string sql, List<DbParameter> lstParameter = null)
        {
            int changedRecordCount = -1;
            try
            {
                SQLBuilder sqlBuilder = new SQLBuilder();
                sqlBuilder.SQL = new StringBuilder().Append(sql);
                sqlBuilder.Parameters = lstParameter;
                DataTable dtResult = ExecuteNonQuery(new List<SQLBuilder> { sqlBuilder });
                if (!dtResult.IsNotNullOrEmpty() || dtResult.Rows.Count == 0) return changedRecordCount;

                Int32 parseResult;
                Int32.TryParse(dtResult.Rows[0]["ExecutedCount"].ToValueAsString(), out parseResult);
                changedRecordCount = parseResult;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                if (!isTransactionEsatablished == true) Close();
            }
            return changedRecordCount;
        }

        public int Renumber_Processsheet(string partno, int routeno)
        {
            int result = 0;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("SMARTPD_RENUMBER_PROCESSSHEET", conn);  //original
                    //SqlCommand cmd = new SqlCommand("SMARTPD_RENUMBER_PROCESSSHEET_06092017NANDHINI", conn);  //new

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure

                    cmd.Parameters.Add(new SqlParameter("@partNo", partno));
                    cmd.Parameters.Add(new SqlParameter("@routeNo", routeno));

                    // execute the command
                    result = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex.LogException();
            }
            return result;

        }

//new update function by nandhu
        public int UpdateSeqNo(string partno, int routeno)
        {
            int result = 0;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("UPDATE_SEQNO", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure

                    cmd.Parameters.Add(new SqlParameter("@partNo", partno));
                    cmd.Parameters.Add(new SqlParameter("@routeNo", routeno));

                    // execute the command
                    result = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex.LogException();
            }
            return result;

        }


        //public int SeqNoUpdate(string partno, int routeno, DataTable dt)
        //{
        //    SqlConnection conn = null;
        //    SqlDataReader reader = null;
        //    int result = 0;
        //    try
        //    {

        //        using (conn = new SqlConnection(this.connectionString))
        //        {
        //            conn.Open();
                    
        //            // 1.  create a command object identifying the stored procedure
        //            SqlCommand cmd = new SqlCommand("SMARTPD_RENUMBER_PROCESSSHEET_FINAL", conn);

        //            // 2. set the command object so it knows to execute a stored procedure
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            // 3. add parameter to command, which will be passed to the stored procedure
        //            cmd.Parameters.Add(new SqlParameter("@partNo", partno));
        //            cmd.Parameters.Add(new SqlParameter("@routeNo", routeno));
                  

        //           // dt = new DataTable(typeof(ProcessSheetArrayModel).Name);

        //            SqlParameter tvparam = cmd.Parameters.AddWithValue("@PROCESSSHEET_ARRAY", dt);
        //            reader = cmd.ExecuteReader();
                   
        //            cmd.Dispose();
        //            conn.Close();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        conn.Close();
        //        throw ex.LogException();
               
        //    }
        //}

        public DataTable Get_Ecn_Pcn_Details(string partno)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("SMARTPD_ECN_PCN_SHEET", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@partNo", partno));

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    cmd.Dispose();
                    conn.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex.LogException();
                return null;
            }
        }

        public decimal Get_Ecn_Pcn_Sel_Sno(string partno, string screentype)
        {
            SqlConnection conn = null;
            string requestedBy = "";
            decimal sno = 0;
            
            try
            {
                using (conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    requestedBy = (screentype == "MPS" ? "SFL" : "CUSTOMER");
                    string sql = "select sno as SNO, product_change_no from DD_PCN where requsted_by='" + requestedBy + "' and part_no = '" + partno + "'and product_change_no in (select max(product_change_no) from DD_PCN where requsted_by='" + requestedBy + "' and part_no = '" + partno + "')";
                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    if (dt != null)
                    {
                        sno = DBNull.Value.Equals(dt.Rows[0]["SNO"]) == true ? 0 : Convert.ToDecimal(dt.Rows[0]["SNO"]);
                    }
                    else
                        sno = 0;

                    cmd.Dispose();
                    conn.Close();
                    return sno;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex.LogException();
                return sno;
            }
        }
        public DataTable Get_PartNo(int idpk)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    string sql = "";
                    if (idpk == 0)
                        sql = "select PART_NO,PART_DESC,IDPK from PRD_MAST where delete_flag = 0 or delete_flag is null";
                    else
                        sql = "select PART_NO,PART_DESC,IDPK from PRD_MAST where delete_flag = 0 or delete_flag is null and IDPK = " + idpk;
                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    cmd.Dispose();
                    conn.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex.LogException();
                return null;
            }
        }

        public DataTable GetToolFamilyCode()
        {
            SqlConnection conn = null;

            try
            {
                using (conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    string sql = "select FAMILY_CD,FAMILY_NAME from TOOL_FAMILY order by FAMILY_CD asc";
                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    cmd.Dispose();
                    conn.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex.LogException();
                return null;
            }
        }
    }

    

    //public class DataAccessLayer
    //{

    //    OracleConnection connection = null;
    //    OracleTransaction transaction = null;

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="connectionStringName"></param>
    //    public DataAccessLayer(string connectionStringName)
    //    {
    //        _connectionStringName = connectionStringName;

    //        if (String.IsNullOrEmpty(_connectionStringName))
    //            _connectionStringName = ConfigurationManager.ConnectionStrings[0].Name;

    //        if (String.IsNullOrEmpty(_connectionStringName))
    //            throw new Exception("Connection String Name should not be empty");

    //        if (ConfigurationManager.ConnectionStrings.Count == 0)
    //            throw new Exception("Connection String should not be empty, Check your app.config file");

    //        if (String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
    //            throw new Exception("Connection String should not be empty");

    //        ConnectionString = decryptConnectionString;

    //        if (connection == null)
    //            connection = new OracleConnection();

    //        connection.ConnectionString = ConnectionString;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    private string _connectionString = "";
    //    private string ConnectionString
    //    {
    //        get { return _connectionString; }
    //        set { _connectionString = value; }
    //    }

    //    private string _connectionStringName = "";
    //    private string ConnectionStringName
    //    {
    //        get { return _connectionStringName; }
    //        set { _connectionStringName = value; }
    //    }

    //    private bool isTransactionEsatablished = false;

    //    public void BeginTransaction()
    //    {
    //        try
    //        {
    //            if (connection == null || connection.State != ConnectionState.Open)
    //            {
    //                Open();
    //            }
    //            transaction = connection.BeginTransaction();
    //            isTransactionEsatablished = true;

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }

    //    }

    //    public void CommitTransaction()
    //    {
    //        try
    //        {
    //            if (transaction == null) throw new Exception("Transaction should be initialized");
    //            transaction.Commit();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            isTransactionEsatablished = false;
    //        }
    //    }

    //    public void RollBackTransaction()
    //    {
    //        try
    //        {
    //            if (transaction == null) throw new Exception("Transaction should be initialized");
    //            transaction.Rollback();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            isTransactionEsatablished = false;
    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    private string decryptConnectionString
    //    {
    //        get
    //        {
    //            if (String.IsNullOrEmpty(ConnectionStringName))
    //                throw new Exception("Connection String Name should not be empty");

    //            if (ConfigurationManager.ConnectionStrings.Count == 0)
    //                throw new Exception("Connection String should not be empty, Check your app.config file");

    //            if (String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString))
    //                throw new Exception("Connection String should not be empty");

    //            return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    private ConnectionState Open()
    //    {
    //        ConnectionState conState = ConnectionState.Closed;
    //        try
    //        {
    //            if (String.IsNullOrEmpty(ConnectionString)) throw new Exception("Connection String should not be empty");
    //            if (connection == null) connection = new OracleConnection();
    //            if (connection.State == ConnectionState.Open)
    //            {
    //                conState = connection.State;
    //            }
    //            else
    //            {
    //                connection.ConnectionString = ConnectionString;
    //                connection.Open();
    //                conState = connection.State;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }

    //        return conState;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public ConnectionState Close()
    //    {
    //        try
    //        {
    //            if (connection == null) return ConnectionState.Closed;
    //            if (connection.State != ConnectionState.Closed) connection.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            isTransactionEsatablished = false;
    //        }
    //        return connection.State;
    //    }

    //    private Exception _dbException = null;
    //    private Exception DBException
    //    {
    //        get { _exception = _dbException; return _dbException; }
    //        set { _dbException = value; }
    //    }

    //    private Exception _exception = null;
    //    public Exception GetException
    //    {
    //        get { return _exception; }
    //        set { _exception = value; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sqlList"></param>
    //    /// <param name="oracleParameterCollection"></param> 
    //    /// <returns></returns>
    //    public DataSet GetDataSet(List<StringBuilder> sqlList, List<OracleParameter> lstParameter = null)
    //    {
    //        DataSet ds = new DataSet();
    //        OracleCommand cmd = null;
    //        try
    //        {
    //            if (!sqlList.IsNotNullOrEmpty()) throw new Exception("Query should not be empty");

    //            if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");

    //            int iCount = 0;
    //            foreach (StringBuilder sb in sqlList)
    //            {
    //                DataTable dt = new DataTable("DataTable" + iCount.ToString());
    //                cmd = new OracleCommand(sb.ToString(), connection);
    //                cmd.Parameters.Clear();
    //                if (lstParameter != null)
    //                {
    //                    foreach (OracleParameter oracleParameter in lstParameter)
    //                    {
    //                        OracleParameter orclParam = oracleParameter.Clone() as OracleParameter;
    //                        if (orclParam != null)
    //                        {
    //                            orclParam.ParameterName = oracleParameter.ParameterName.StartsWith(":") ? oracleParameter.ParameterName : ":" + oracleParameter.ParameterName;

    //                            cmd.Parameters.Add(orclParam);
    //                        }
    //                        //orclParam.ParameterName = oracleParameter.ParameterName.StartsWith(":") ? oracleParameter.ParameterName : ":" + oracleParameter.ParameterName;
    //                        //orclParam.OracleDbType = oracleParameter.OracleDbType;
    //                        //orclParam.Value = oracleParameter.Value;
    //                        //orclParam.Direction = oracleParameter.Direction;


    //                    }
    //                }
    //                OracleDataAdapter ad = new OracleDataAdapter(cmd);
    //                ad.Fill(dt);
    //                ds.Tables.Add(dt);
    //                iCount++;
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (sqlList.IsNotNullOrEmpty()) sqlList.Clear();
    //            sqlList = null;
    //            if (cmd.Parameters.IsNotNullOrEmpty()) cmd.Parameters.Clear();
    //            cmd = null;
    //            if (lstParameter.IsNotNullOrEmpty())
    //                lstParameter.Clear();
    //            lstParameter = null;
    //            if (!isTransactionEsatablished == true) Close();

    //        }

    //        return ds;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sqlList"></param>
    //    /// <param name="oracleParameterCollection"></param> 
    //    /// <returns></returns>
    //    public DataSet GetDataSet(Dictionary<string, StringBuilder> sqlDictionary, List<OracleParameter> lstParameter = null)
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            List<StringBuilder> sqlList = new List<StringBuilder>();
    //            foreach (KeyValuePair<string, StringBuilder> keyValuePair in sqlDictionary)
    //            {
    //                if (keyValuePair.Value != null && !String.IsNullOrEmpty(keyValuePair.Value.ToString()))
    //                    sqlList.Add(keyValuePair.Value);
    //            }
    //            ds = GetDataSet(sqlList, lstParameter);

    //            if (ds.IsNotNullOrEmpty())
    //            {
    //                int iCount = 0;
    //                foreach (KeyValuePair<string, StringBuilder> keyValuePair in sqlDictionary)
    //                {
    //                    if (keyValuePair.Value != null && !String.IsNullOrEmpty(keyValuePair.Value.ToString()) && !String.IsNullOrEmpty(keyValuePair.Key.ToString()) && ds.Tables[iCount] != null)
    //                        ds.Tables[iCount].TableName = keyValuePair.Key.ToString();
    //                    iCount++;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (sqlDictionary.IsNotNullOrEmpty()) sqlDictionary.Clear();
    //            sqlDictionary = null;
    //        }

    //        return ds;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sbSQL"></param>
    //    /// <param name="oracleParameterCollection"></param>
    //    /// <returns></returns>
    //    public DataSet GetDataSet(StringBuilder sbSQL, List<OracleParameter> lstParameter = null)
    //    {

    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            List<StringBuilder> sqlList = new List<StringBuilder>();
    //            sqlList.Add(sbSQL);
    //            ds = GetDataSet(sqlList, lstParameter);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
    //            sbSQL = null;
    //        }

    //        return ds;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sbSQL"></param>
    //    /// <param name="oracleParameterCollection"></param>
    //    /// <returns></returns>
    //    public DataTable GetDataTable(StringBuilder sbSQL, List<OracleParameter> lstParameter = null)
    //    {

    //        DataTable dt = new DataTable();
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            if (!sbSQL.IsNotNullOrEmpty()) throw new Exception("Query should not be empty");
    //            List<StringBuilder> sbList = new List<StringBuilder> { sbSQL };
    //            ds = GetDataSet(sbList, lstParameter);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
    //            sbSQL = null;
    //        }

    //        if (!ds.IsNotNullOrEmpty()) return null;
    //        return ds.Tables[0];
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sql"></param>
    //    /// <param name="oracleParameterCollection"></param>
    //    /// <returns></returns>
    //    public DataTable GetDataTable(string sql, List<OracleParameter> lstParameter = null)
    //    {
    //        StringBuilder sbSQL = new StringBuilder().Append(sql);
    //        DataTable dt = new DataTable();
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            if (String.IsNullOrEmpty(sql)) throw new Exception("Query should not be empty");
    //            List<StringBuilder> sbList = new List<StringBuilder> { sbSQL };

    //            ds = GetDataSet(sbList, lstParameter);
    //            if (ds == null || ds.Tables.Count == 0) return null;
    //            dt = ds.Tables[0];

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
    //            sbSQL = null;
    //            sql = null;
    //        }

    //        if (!ds.IsNotNullOrEmpty()) return null;
    //        return ds.Tables[0];
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sbSQL"></param>
    //    /// <returns></returns>
    //    public string GetScalarValue(StringBuilder sbSQL, List<OracleParameter> lstParameter = null)
    //    {
    //        string scalarVal = null;
    //        OracleCommand cmdScal = null;
    //        try
    //        {
    //            if (!sbSQL.IsNotNullOrEmpty()) return scalarVal;
    //            if (Open() != ConnectionState.Open) return scalarVal;
    //            cmdScal = new OracleCommand(sbSQL.ToString(), connection);
    //            if (lstParameter != null)
    //            {
    //                foreach (OracleParameter oracleParameter in lstParameter)
    //                {
    //                    cmdScal.Parameters.Add(oracleParameter);
    //                }
    //            }
    //            scalarVal = (string)cmdScal.ExecuteScalar();

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            cmdScal.Dispose();
    //            if (sbSQL.IsNotNullOrEmpty()) sbSQL.Clear();
    //            sbSQL = null;
    //            if (!isTransactionEsatablished == true) Close();

    //        }

    //        return scalarVal;
    //    }

    //    public Int32 ExecuteSP(string sql, List<OracleParameter> lstParameter = null)
    //    {
    //        Int32 count = 1;
    //        OracleCommand cmd = null;
    //        try
    //        {
    //            if (String.IsNullOrEmpty(sql)) return -1;
    //            if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");
    //            cmd = new OracleCommand(sql, connection);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            if (lstParameter != null)
    //            {
    //                foreach (OracleParameter oracleParameter in lstParameter)
    //                {
    //                    cmd.Parameters.Add(oracleParameter);
    //                }
    //            }
    //            cmd.ExecuteNonQuery();
    //            return count;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (!isTransactionEsatablished == true) Close();
    //        }
    //    }

    //    public Int32 ExecuteNonQuery(string sql, List<OracleParameter> lstParameter = null)
    //    {
    //        Int32 count = 1;
    //        OracleCommand cmd = null;
    //        try
    //        {
    //            if (String.IsNullOrEmpty(sql)) return -1;
    //            if (Open() != ConnectionState.Open) throw new Exception("Connection should not be established");
    //            cmd = new OracleCommand(sql, connection);
    //            cmd.CommandType = CommandType.Text;
    //            foreach (OracleParameter oracleParameter in lstParameter)
    //            {
    //                cmd.Parameters.Add(oracleParameter);
    //            }
    //            cmd.ExecuteNonQuery();
    //            return count;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex.LogException();
    //        }
    //        finally
    //        {
    //            if (!isTransactionEsatablished == true) Close();
    //        }
    //    }

    //}

}
