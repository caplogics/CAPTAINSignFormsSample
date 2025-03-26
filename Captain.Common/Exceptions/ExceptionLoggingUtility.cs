/************************************************************************************
 * Class Name   : ExceptionLoggingUtility
 * Author       : Applabs
 * Created Date : 
 * Version      : 1.0.0
 * Description  : This class file used to log the exceptions into different files
 *************************************************************************************/

#region Using

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Data;

using log4net;
using log4net.Appender;

//using Microsoft.ApplicationBlocks.ExceptionManagement;

using Captain.Common.Utilities;
using log4net.Config;
using Wisej.Web;

#endregion

namespace Captain.Common.Exceptions
{
    public class ExceptionLoggingUtility
    {
        #region Variables

        /// <summary>
        /// To hold Logger instance i.e. Log4Net
        /// </summary>
        private ILog _log;
        /// <summary>
        /// To hold folder structure where Exception log resides.
        /// </summary>
        private string _folder;
        private string _logExtension = CommonFunctions.GetConfigValue(Consts.Common.ExceptionLogExtension, ".log");
        private static int _maxFileSize = 5242880;

        private static Object _thisLock = new Object();

        #endregion Variables

        #region Constructor

        /// <summary>
        /// static constructor.
        /// </summary>
        public ExceptionLoggingUtility()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _folder = DateTime.Now.Year + Consts.Common.BackSlash + DateTime.Now.ToString("MMMM") + Consts.Common.BackSlash + System.DateTime.Today.ToString("dd-MMM");

        }

        #endregion Constructor

        #region Private Methods

        /// <summary>
        /// Gets the path to the error logs
        /// </summary>
        public string ErrorLogsPath
        {
            get
            {
                return CommonFunctions.GetConfigValue(Consts.Common.ExceptionLogsPath, Consts.Common.DefaultExceptionLogsPath);
            }
        }

        /// <summary>
        /// To assign log file path to log4net
        /// </summary>
        public string LogFile
        {
            get { return ((RollingFileAppender)((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Appenders[0]).File; }
            set
            {
                if (((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Appenders.Count > 0)
                {
                    ((RollingFileAppender)((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Appenders[0]).File = value;
                }
            }
        }

        #endregion Private Methods

        #region Log4Net

        /// <summary>
        /// Used to log Exceptions Occured in the application using Log4Net
        /// </summary>
        /// <param name="trace">stack information of exception</param>    
        /// <param name="errorMessage">Exception Occured</param>
        public void WriteToLogFile(StackFrame frame, string errorMessage, ExceptionSeverityLevel severityLevel)
        {
            try
            {
                string methodName = frame.GetMethod().Name;
                int lineNumber = frame.GetFileLineNumber();
                int columnNumber = frame.GetFileColumnNumber();
                string className = frame.GetFileName();
                if (!string.IsNullOrEmpty(className) && className.Length > 0) { className.Substring(className.LastIndexOf('\\') + 1); }

                LogFile = ErrorLogsPath + Consts.Common.Log4Net + Consts.Common.BackSlash + _folder + _logExtension;

                if (_log.IsDebugEnabled)
                {
                    StringBuilder logMessage = new StringBuilder();
                    logMessage.AppendLine();
                    logMessage.AppendLine("****************************************************************************************************************************");
                    logMessage.AppendLine();
                    logMessage.AppendLine("Request");
                    logMessage.AppendLine("¯¯¯¯¯¯¯");
                    logMessage.AppendLine("  Class Name:  " + className);
                    logMessage.AppendLine("  Line No:     " + lineNumber.ToString());
                    logMessage.AppendLine("  Column No:   " + columnNumber.ToString());
                    logMessage.AppendLine("  Method Name: " + methodName);
                    logMessage.AppendLine("  Severity:     " + severityLevel);
                    logMessage.AppendLine();
                    logMessage.AppendLine("Exception");
                    logMessage.AppendLine("¯¯¯¯¯¯¯¯¯");
                    logMessage.AppendLine(errorMessage);
                    logMessage.AppendLine();

                    _log.Debug(logMessage.ToString());
                    logMessage = null;
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToTextFile(ex.Message, ExceptionSeverityLevel.Low);
            }
            finally { _log = null; }
        }

        /// <summary>
        /// Used to log Exceptions Occured in the application and including input parameters using Log4Net
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="errorMessage"></param>
        /// <param name="userID"></param>
        /// <param name="severityLevel"></param>
        public void WriteToLogFile(StackFrame frame, string[] parameters, string errorMessage, string userID, ExceptionSeverityLevel severityLevel)
        {
            try
            {
                if (_log.IsErrorEnabled)
                {
                    string className = frame.GetFileName();
                    if (!string.IsNullOrEmpty(className) && className.Length > 0) { className.Substring(className.LastIndexOf('\\') + 1); }
                    string methodName = string.Concat(Path.GetFileNameWithoutExtension(className), Consts.Common.Period, frame.GetMethod().Name);
                    int lineNumber = frame.GetFileLineNumber();
                    int columnNumber = frame.GetFileColumnNumber();

                    LogFile = ErrorLogsPath + Consts.Common.Log4Net + Consts.Common.BackSlash + _folder + _logExtension;

                    StringBuilder logMessage = new StringBuilder();
                    logMessage.AppendLine();
                    logMessage.AppendLine("****************************************************************************************************************************");
                    logMessage.AppendLine();
                    logMessage.AppendLine("Request");
                    logMessage.AppendLine("¯¯¯¯¯¯¯");
                    logMessage.AppendLine("  Class Name:  " + className);
                    logMessage.AppendLine("  Line No:     " + lineNumber.ToString());
                    logMessage.AppendLine("  Column No:   " + columnNumber.ToString());
                    logMessage.AppendLine("  Method Name: " + methodName);
                    logMessage.AppendLine("  Parameters:  " + string.Join(string.Concat(Consts.Common.Comma, Consts.Common.Space), parameters == null ? new string[0] : parameters));
                    logMessage.AppendLine("  User ID:     " + userID);
                    logMessage.AppendLine();
                    logMessage.AppendLine("Exception");
                    logMessage.AppendLine("¯¯¯¯¯¯¯¯¯");
                    logMessage.AppendLine(errorMessage);
                    logMessage.AppendLine();

                    _log.Error(logMessage.ToString());
                    logMessage = null;
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToTextFile(ex.Message, ExceptionSeverityLevel.Low);
            }
            finally
            {
                _log = null;
            }
        }

        /// <summary>
        /// Used to log request\response of the soap action using Log4Net
        /// </summary>
        /// <param name="trace">stack information of exception</param>    
        /// <param name="errorMessage">Exception Occured</param>
        public void DebugToLogFile(StackFrame frame, string[] parameters, string[] response, string userID)
        {
            try
            {
                if (_log.IsDebugEnabled)
                {
                    string methodName = string.Concat(Path.GetFileNameWithoutExtension(frame.GetFileName()), Consts.Common.Period, frame.GetMethod().Name);
                    LogFile = ErrorLogsPath + Consts.Common.Log4Net + Consts.Common.BackSlash + _folder + _logExtension;

                    StringBuilder logMessage = new StringBuilder();
                    logMessage.AppendLine();
                    logMessage.AppendLine("****************************************************************************************************************************");
                    logMessage.AppendLine();
                    logMessage.AppendLine("Request");
                    logMessage.AppendLine("¯¯¯¯¯¯¯");
                    logMessage.AppendLine("  Method Name: " + methodName);
                    logMessage.AppendLine("  Parameters:  " + string.Join(string.Concat(Consts.Common.Comma, Consts.Common.Space), parameters));
                    logMessage.AppendLine("  User ID:     " + userID);
                    logMessage.AppendLine();
                    logMessage.AppendLine("Response");
                    logMessage.AppendLine("¯¯¯¯¯¯¯¯");
                    logMessage.AppendLine(string.Join(string.Concat(Consts.Common.Comma, Consts.Common.Space), response == null ? new string[0] : response));
                    logMessage.AppendLine();

                    _log.Debug(logMessage.ToString());
                    logMessage = null;
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToTextFile(ex.Message, ExceptionSeverityLevel.Low);
            }
            finally { _log = null; }
        }

        /// <summary>
        /// Used to log request\response of the soap action using Log4Net
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="userID"></param>
        public void DebugToLogFile<TRequest>(StackFrame frame, TRequest request, string response, string userID)
        {
            try
            {
                if (_log.IsDebugEnabled)
                {
                    string methodName = frame.GetMethod().Name;
                    LogFile = ErrorLogsPath + Consts.Common.Log4Net + Consts.Common.BackSlash + _folder + _logExtension;

                    StringBuilder logMessage = new StringBuilder();
                    logMessage.AppendLine();
                    logMessage.AppendLine("****************************************************************************************************************************");
                    logMessage.AppendLine();
                    logMessage.AppendLine("Request");
                    logMessage.AppendLine("¯¯¯¯¯¯¯");
                    logMessage.AppendLine("  Method Name: " + methodName);
                    logMessage.AppendLine("  User ID:     " + userID);
                    logMessage.AppendLine("  Request Details:  " + CommonFunctions.SerializeObject(request));
                    logMessage.AppendLine();
                    logMessage.AppendLine("Response");
                    logMessage.AppendLine("¯¯¯¯¯¯¯¯");
                    logMessage.AppendLine(response);
                    logMessage.AppendLine();

                    _log.Debug(logMessage.ToString());
                    logMessage = null;
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToTextFile(ex.Message, ExceptionSeverityLevel.Low);
            }
            finally { _log = null; }
        }

        #endregion Log4Net

        #region Write to XML File

        /// <summary>
        /// Writing to XML File
        /// </summary>
        /// <param name="trace">Trace object to get methodname,classname etc</param>
        /// <param name="errorMessage">Exception occured</param>
        public void LogExceptionToXML(StackFrame frame, string errorMessage, ExceptionSeverityLevel severityLevel)
        {
            string methodName = frame.GetMethod().Name;
            int lineNumber = frame.GetFileLineNumber();
            int columnNumber = frame.GetFileColumnNumber();
            string[] fName = null;
            string className = string.Empty;

            if (frame.GetFileName() != null)
            {
                fName = frame.GetFileName().Split('\\');
                className = fName[fName.Length - 1];
            }
            string path = ErrorLogsPath + "ExceptionXMLFiles\\" + _folder.Split('\\')[0] + "\\" + _folder.Split('\\')[1];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = ErrorLogsPath + "ExceptionXMLFiles\\" + _folder + ".xml";

            StringBuilder sb = new StringBuilder();
            string strOldData = string.Empty;

            XmlTextWriter xml = null;
            try
            {
                WriteExceptionBlock(errorMessage, severityLevel, methodName, lineNumber, columnNumber, className, xml, fileName);
            }
            finally
            {
                if (xml != null)
                {
                    xml.Flush();
                    xml.Close();
                }
            }

            System.IO.FileStream fStream = null;

            try
            {
                string errorDetails = sb.ToString();
                fStream = new System.IO.FileStream(fileName, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fStream);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(errorDetails);
                sw.Close();
                fStream.Close();
            }
            catch (Exception)
            {
                throw new Exception("FileOperationFailed");
            }
        }

        /// <summary>
        /// Writes Exception Block in tags into file
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="severityLevel"></param>
        /// <param name="methodName"></param>
        /// <param name="lineNumber"></param>
        /// <param name="columnNumber"></param>
        /// <param name="className"></param>
        /// <param name="xml"></param>
        private static void WriteExceptionBlock(string errorMessage, ExceptionSeverityLevel severityLevel, string methodName, int lineNumber, int columnNumber, string className, XmlTextWriter xml, String filePath)
        {
            try
            {
                DataSet dsExceptions = new DataSet();
                DataTable dtExceptions = new DataTable();

                dtExceptions.TableName = "Exception";
                dtExceptions.Columns.Add("DateTime");
                dtExceptions.Columns.Add("Class");
                dtExceptions.Columns.Add("Method");
                dtExceptions.Columns.Add("LineNumber");
                dtExceptions.Columns.Add("ColumnNumber");
                dtExceptions.Columns.Add("Error");
                dtExceptions.Columns.Add("FaultCode");
                dtExceptions.Columns.Add("FaultType");
                dtExceptions.Columns.Add("FaultMessage");
                dtExceptions.Columns.Add("SeverityLevel");
                dsExceptions.Tables.Add(dtExceptions);

                //To check whether the Error Log file exist or not
                if (File.Exists(filePath))
                {
                    dsExceptions.ReadXml(filePath);
                }

                DataRow drException = dsExceptions.Tables[0].NewRow();
                drException["DateTime"] = DateTime.Now.ToString();
                drException["Class"] = className;
                drException["Method"] = methodName;
                drException["LineNumber"] = lineNumber;
                drException["ColumnNumber"] = columnNumber;
                if (errorMessage.Contains("quantumFault"))
                {
                    errorMessage = errorMessage.Replace("xmlns:fault=\"http://xmlns.octagonresearch.com/quantum/fault\"", "");
                    errorMessage = errorMessage.Replace("fault:", "");

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(errorMessage);
                    XmlNode xmlNode = xmlDocument.SelectSingleNode("quantumFault");

                    drException["Error"] = CommonFunctions.GetXmlValue(xmlNode, "message");
                    drException["FaultCode"] = CommonFunctions.GetXmlValue(xmlNode, "code");
                    drException["FaultType"] = CommonFunctions.GetXmlValue(xmlNode, "type");
                    drException["FaultMessage"] = CommonFunctions.GetXmlValue(xmlNode, "message");

                    xmlNode = null;
                    xmlDocument = null;
                }
                else
                {
                    drException["Error"] = errorMessage;
                    drException["FaultCode"] = string.Empty;
                    drException["FaultType"] = string.Empty;
                    drException["FaultMessage"] = string.Empty;
                }
                drException["SeverityLevel"] = severityLevel;
                dsExceptions.Tables[0].Rows.Add(drException);
                dsExceptions.DataSetName = "ExceptionLog";
                dsExceptions.WriteXml(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Write to XML File

        #region Writing to text file

        /// <summary>
        /// Log the message to appropriate files.
        /// </summary> 
        /// <param name="messageToLog">message to log</param>
        public void WriteExceptionToTextFile(string messageToLog, ExceptionSeverityLevel severityLevel)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            string inTitle = "In ";
            StackFrame stackframe = new StackFrame(1, true);
            string methodName = stackframe.GetMethod().Name;
            lock (_thisLock)
            {
                try
                {
                    string strDirectory = ErrorLogsPath + "ApplicationException\\" + _folder.Split('\\')[0] + "\\" + _folder.Split('\\')[1];
                    if (!Directory.Exists(strDirectory))
                    {
                        Directory.CreateDirectory(strDirectory);
                    }
                    string fileName = ErrorLogsPath + "ApplicationException\\" + _folder + _logExtension;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        FileInfo fileinformation = new FileInfo(fileName);
                        if (fileinformation.Exists && fileinformation.Length > _maxFileSize)
                        {
                            DirectoryInfo info = new DirectoryInfo(strDirectory);
                            int countFiles = info.GetFiles(Path.GetFileName(fileName)).Length;
                            string tempFileName = string.Concat(strDirectory, countFiles.ToString(CultureInfo.CurrentCulture), _logExtension);
                            File.Copy(fileName, tempFileName);
                            File.Delete(fileName);
                        }

                        using (fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (sw = new StreamWriter(fs))
                            {
                                sw.BaseStream.Seek(0, SeekOrigin.End);
                                sw.WriteLine(string.Concat(inTitle, methodName, DateTime.Now.ToString(CultureInfo.CurrentCulture), " ", messageToLog, " Severity ", severityLevel));

                                sw.Flush();
                                fs.Flush();

                                sw.Close();
                                fs.Close();
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    throw new Exception("FileOperationFailed");
                }
            }
        }

        #endregion Writing to text file

        #region Display Message to User

        /// <summary>
        /// To Show information to user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="faultType">Tye type of fault.</param>
        /// <param name="severityLevel">The fault severity</param>
        public void ShowFriendlyMessageToUser(string message, QuantumFaults faultType, ExceptionSeverityLevel severityLevel)
        {
            string suppressFriendlyMessagesOfTypes = CommonFunctions.GetConfigValue("SuppressFriendlyMessagesOfType", string.Empty);
            string suppressFaultsOfType = string.Empty;

            if (faultType != QuantumFaults.None)
            {
                suppressFaultsOfType = CommonFunctions.GetConfigValue("SuppressFaultsOfType", string.Empty);
            }

            bool shouldDisplayFriendlyMessage = !string.IsNullOrEmpty(message) && !suppressFriendlyMessagesOfTypes.ToLower().Contains(severityLevel.ToString().ToLower()) && !suppressFaultsOfType.ToLower().Contains(faultType.ToString().ToLower());
            if (shouldDisplayFriendlyMessage)
            {
                string foreColor = string.Empty;
                // Wisej: Not used: string gradientStartColor = string.Empty;
                // Wisej: Not used: string gradientEndColor = string.Empty;
                string[] severityColors = CommonFunctions.GetConfigValue(Consts.Common.FriendlyMessageColors, Consts.Common.FriendlyMessageDefaultColors).Split(Consts.Common.Tilda.ToCharArray());
                if (severityColors.Length != 8) { severityColors = Consts.Common.FriendlyMessageDefaultColors.Split(Consts.Common.Tilda.ToCharArray()); }
                string[] colors = null;

                MessageBoxIcon icon = MessageBoxIcon.None;

                switch (severityLevel)
                {
                    case ExceptionSeverityLevel.Critical:
                        colors = severityColors[0].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Hand;
                        break;
                    case ExceptionSeverityLevel.Fatal:
                        colors = severityColors[1].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Stop;
                        break;
                    case ExceptionSeverityLevel.High:
                        colors = severityColors[2].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Stop;
                        break;
                    case ExceptionSeverityLevel.Error:
                        colors = severityColors[3].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Error;
                        break;
                    case ExceptionSeverityLevel.Debug:
                        colors = severityColors[4].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Warning;
                        break;
                    case ExceptionSeverityLevel.Normal:
                        colors = severityColors[5].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Information;
                        break;
                    case ExceptionSeverityLevel.Low:
                        colors = severityColors[6].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Information;
                        break;
                    case ExceptionSeverityLevel.Information:
                        colors = severityColors[7].Split(Consts.Common.Comma.ToCharArray());
                        // Wisej: Not used: gradientStartColor = colors[0];
                        // Wisej: Not used: gradientEndColor = colors[1];
                        // Wisej: Not used: foreColor = colors[2];
                        icon = MessageBoxIcon.Information;
                        break;
                }

                AlertBox.Show(message, icon:icon);
            }
        }

        /// <summary>
        /// Used to log Exceptions Occured in the application using Log4Net and display a friendly message
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="errorMessage"></param>
        /// <param name="faultType"></param>
        /// <param name="userMessage"></param>
        /// <param name="caption"></param>
        /// <param name="severityLevel"></param>
        public void WriteToLogFileAndShowMessage(StackFrame trace, string errorMessage, QuantumFaults faultType, string userMessage, ExceptionSeverityLevel severityLevel)
        {
            WriteToLogFile(trace, errorMessage, severityLevel);
            ShowFriendlyMessageToUser(userMessage, faultType, severityLevel);
        }

        #endregion Display Message to User
    }

    /// <summary>
    /// Severity level of Exception
    /// </summary>
    public enum ExceptionSeverityLevel
    {
        Low,
        Normal,
        High,
        Fatal,
        Critical,
        Error,
        Debug,
        Information
    }
}





