using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessDesigner.ExceptionHandler
{
    public class ExceptionHandler
    {
        /// <summary>
        /// Exception policy name configured in Mirosoft Enterprise Library configuration
        /// </summary>
        private const string ERROR_POLICY = "LoggingAndReplacingException";
        private const string LOGGING_POLICY = "LoggingException";
        /// <summary>
        /// Default instance field of Enterprise Library Manager
        /// </summary>
        private static ExceptionHandler _default;
        /// <summary>
        /// Default instance propery of Enterprise Library Manager
        /// </summary>
        public static ExceptionHandler Default
        {
            get
            {
                if (_default == null)
                    _default = new ExceptionHandler();

                return _default;
            }
        }
        /// <summary>
        /// Intializes Enterprise Library Manager - Restricts ELM creation using default instance property
        /// </summary>
        private ExceptionHandler()
        {
            //IConfigurationSource config = ConfigurationSourceFactory.Create();
            //ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);
            //DatabaseProviderFactory dbfactory = new DatabaseProviderFactory(new SystemConfigurationSource(false).GetSection);
            //DatabaseFactory.SetDatabaseProviderFactory(dbfactory, false);
            //LogWriterFactory logWriterFactory = new LogWriterFactory(config);
            //Logger.SetLogWriter(logWriterFactory.Create());
            //ExceptionManager exManager = factory.CreateManager();
            //ExceptionPolicy.SetExceptionManager(factory.CreateManager());
        }
        /// <summary>
        /// Handles exception according to the application configuration
        /// </summary>
        /// <param name="e">Exception object</param>
        public void HandleException(Exception e)
        {
            try
            {
                ExceptionPolicy.HandleException(e, LOGGING_POLICY);
            }
            catch (Exception err)
            {
                Trace.TraceError(string.Format("Error Squared! Error while handling exception: {0}", err.Message));
            }

        }
        /// <summary>
        /// Handles exception according to the application configuration
        /// </summary>
        /// <param name="e">Exception object</param>
        public void HandleException(Exception e, out Exception ex)
        {

            Exception returnex = new Exception();
            try
            {
                bool rethrow = ExceptionPolicy.HandleException(e, ERROR_POLICY, out returnex);
                if (rethrow)
                {
                    MessageBox.Show(returnex.Message);
                }
            }
            catch (Exception err)
            {
                Trace.TraceError(string.Format("Error Squared! Error while handling exception: {0}", err.Message));

            }
            ex = returnex;

        }

    }
}
