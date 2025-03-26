/************************************************************************************
 * Class Name   : CaptainLogger
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : This class file used to log the exception messages
 *************************************************************************************/

#region Using

using System;
using System.IO;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Exceptions
{
    public class CaptainLogger
    {
        #region Variables


        #endregion

        #region Constructor

        public CaptainLogger()
        { 
            //
        }

        #endregion

        #region Public Methods

        public void LogMessage(string msg)
        {
            // open file for writting
            string fileName = GetFileName();
            string todayDir = GetCurrentDirectory() + fileName.Split('\\')[0] + "\\" + fileName.Split('\\')[1];
            if (!Directory.Exists(todayDir))
            {
                Directory.CreateDirectory(todayDir);
            }

            string file = GetCurrentDirectory() + GetFileName();
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }
            object syncObj = new Object();
            lock (syncObj)
            {
                using (StreamWriter sw = File.AppendText(file))
                {
                    StreamWriter.Synchronized(sw);
                    sw.WriteLine(msg);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To Create Directory for storing Exception logs.
        /// </summary>
        /// <returns></returns>
        private string GetCurrentDirectory()
        {
            string currentDirectory = CommonFunctions.GetConfigValue("ExceptionLogPath", Consts.Common.DefaultExceptionLogsPath);
            return string.Concat(currentDirectory, "\\", "ErrorLogs\\");
        }
        
        private void CreateFile(string file)
        {
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }
        }

        private string GetFileName()
        {
            string fileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMMM") + "\\" + System.DateTime.Today.ToString("dd-MMM") + ".txt";
            return fileName;
        }

        #endregion
    }
}