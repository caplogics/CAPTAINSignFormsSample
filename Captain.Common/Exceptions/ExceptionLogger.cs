/************************************************************************************
 * Class Name   : ExceptionLogger
 * Author       : Applabs
 * Created Date : 
 * Version      : 1.0.0
 * Description  : This class file used to log the exceptions
 *************************************************************************************/

#region Using

using System;
using System.Configuration;
using System.Diagnostics;
using Captain.Common.Utilities;
using Wisej.Web;

#endregion

namespace Captain.Common.Exceptions
{
    public class ExceptionLogger
    {
        /// <summary>
        /// Logs an exception to a log file.
        /// </summary>
        /// <param name="trace">The stack trace.</param>
        /// <param name="exception">The Exception exor.</param>
        /// <param name="severityLevel">The Exception exor level.</param>
        public static void LogException(StackFrame trace, Exception exception, ExceptionSeverityLevel severityLevel)
        {
            if (CommonFunctions.GetConfigValue("LoggingEnabled", "false").ToLower().Equals("true"))
            {
                LoggingUsedInApplication(trace, exception, severityLevel);
            }
        }

        /// <summary>
        /// Displays a frienly message to the user.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="faultCode"></param>
        /// <param name="severityLevel"></param>
        public static void DisplayMessageToUser(string userMessage, QuantumFaults faultType, ExceptionSeverityLevel severityLevel)
        {
            ExceptionLoggingUtility exceptionLoggingMechanisms = new ExceptionLoggingUtility();
            exceptionLoggingMechanisms.ShowFriendlyMessageToUser(userMessage, faultType, severityLevel);
        }

        /// <summary>
        /// Logs an exception into a log file and displays a frienly message to the users.
        /// </summary>
        /// <param name="trace">The stack trace.</param>
        /// <param name="exception">The Exception exor.</param>
        /// <param name="userMessage">The user friendly error message.</param>
        /// <param name="severityLevel">The Exception exor level.</param>
        public static void LogAndDisplayMessageToUser(StackFrame trace, Exception exception, QuantumFaults faultType, ExceptionSeverityLevel severityLevel)
        {
            if (CommonFunctions.GetConfigValue("LoggingEnabled", "false").ToLower().Equals("true"))
            {
                LoggingUsedInApplication(trace, exception, severityLevel);
            }
            DisplayMessageToUser(exception.Message, faultType, severityLevel);
        }

        /// <summary>
        /// Logs an exception into a log file.
        /// </summary>
        /// <param name="trace">The stack trace.</param>
        /// <param name="exception">The Exception exor.</param>
        /// <param name="severityLevel">The Exception exor level.</param>
        private static void LoggingUsedInApplication(StackFrame trace, Exception exception, ExceptionSeverityLevel severityLevel)
        {
            string logMechanism = "";//ConfigurationManager.AppSettings["ExceptionLogging"];
            ExceptionLoggingUtility exceptionLoggingMechanisms = new ExceptionLoggingUtility();
            if (logMechanism == "log4Net")
            {
                exceptionLoggingMechanisms.WriteToLogFile(trace, exception.Message, severityLevel);
            }
            else if (logMechanism == "XMLFiles")
            {
                exceptionLoggingMechanisms.LogExceptionToXML(trace, exception.Message, severityLevel);
            }
            else if(logMechanism == "CaptainLogger")
            {
                CaptainLogger log = new CaptainLogger();
                string message = DateTime.Now.Date.ToString() + " - " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                message += " >>  Method: " + trace.GetMethod().Name + ", Type: " + trace.GetFileName() + ", Line No: " + trace.GetFileLineNumber().ToString() + " [" + exception.Message + "] (" + severityLevel + ")";
                log.LogMessage(message);
            }
            else
            {
                exceptionLoggingMechanisms.WriteExceptionToTextFile(exception.Message, severityLevel);
            }
        }
    }
}
