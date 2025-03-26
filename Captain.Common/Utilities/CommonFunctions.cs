/****************************************************************************************************
 * Class Name    : CommonFunctions
 * Author        : Chitti
 * Created Date  : 
 * Version       : 1.0.0
 * Description   : This class has common functions,which used in across the application
 ****************************************************************************************************/

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Wisej.Web;
using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Parameters;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Utilities;
using System.Runtime.InteropServices;
using Captain.Common.Exceptions;
using Captain.DatabaseLayer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Collections.Specialized;

using System.Drawing;
using Aspose.Cells;
using System.Security.Claims;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Data.SqlClient;

#endregion

namespace Captain.Common.Utilities
{
    public static class CommonFunctions
    {
        #region Variables

        private static object _serialization = new object();

        #endregion

        #region Private Methods

        /// <summary>
        /// GetUserNameFormatted
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="templateString"></param>
        /// <returns></returns>
        private static string GetUserNameFormatted(UserEntity userEntity, string templateString)
        {
            string userTemplate = templateString.ToLower();

            userTemplate = userTemplate.Replace(Consts.Parameters.FirstName, userEntity.FirstName);
            userTemplate = userTemplate.Replace(Consts.Parameters.LastName, userEntity.LastName);
            userTemplate = userTemplate.Replace(Consts.Parameters.MiddleName, userEntity.MI);
            //userTemplate = userTemplate.Replace(Consts.Parameters.Title, userEntity.);
            //userTemplate = userTemplate.Replace(Consts.Parameters.Email, userEntity.Email);
            userTemplate = userTemplate.Replace(Consts.Parameters.UserName, userEntity.UserName);
            userTemplate = userTemplate.Replace(Consts.Parameters.UserID, userEntity.UserID);

            return userTemplate.Trim();
        }

        private static string IncrementCharacter(string currentString)
        {
            char[] allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] givenCharacters = currentString.ToCharArray();
            int index = currentString.Length - 1;
            char lastChar = givenCharacters[index];
            for (int i = 0; i < allowedChars.Length; i++)
            {
                if (allowedChars[i] == lastChar)
                {
                    if (i == allowedChars.Length - 1)
                    {
                        givenCharacters[index] = allowedChars[0];
                        if (givenCharacters.Length > 1)
                        {
                            string stepUpPreceedingCharacters = IncrementCharacter(new string(givenCharacters.Take(givenCharacters.Length - 1).ToArray()));
                            givenCharacters = (stepUpPreceedingCharacters + givenCharacters[index]).ToCharArray();
                        }
                        else
                        {
                            givenCharacters = new char[] { allowedChars[0], givenCharacters[index] };
                        }
                    }
                    else
                    {
                        givenCharacters[index] = allowedChars[i + 1];
                    }
                    break;
                }
            }
            return new string(givenCharacters);
        }

        #endregion

        #region Public Static Methods



        public static bool CheckDateFormat(string strDate, string Format)
        {
            bool flag = true;
            try
            {
                if(strDate == "1/1/0001")
                {
                    AlertBox.Show("Invalid Date Format", MessageBoxIcon.Warning);
                    flag = false;
                }
                //DateTime dateValue;
                //if (DateTime.TryParseExact(strDate, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                //    return true;
                //else
                //{
                //    AlertBox.Show("Invalid Date Format", MessageBoxIcon.Warning);
                //    return false;
                //}
                return flag;
            }
            catch
            {
                AlertBox.Show("Invalid Date Format", MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>
        /// Creates a timer and initializes it.
        /// </summary>
        /// <param name="interval">The interval of the timer.</param>
        /// <param name="eventHandler">The ticker event handler.</param>
        /// <param name="container">The container for the timer.</param>
        /// <returns>Returns the timer object.</returns>
        public static Timer AddTimedEvent(int interval, EventHandler eventHandler, IContainer container)
        {
            try
            {
                Timer timedEvent = new Timer(container);
                timedEvent.Interval = interval;
                timedEvent.Tick += eventHandler;
                timedEvent.Enabled = true;
                timedEvent.Start();

                return timedEvent;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a date value and formats using the specified format.
        /// </summary>
        /// <param name="dateValue">The date value to format.</param>
        /// <param name="format">The format to use.</param>
        /// <returns>Returns a formatted date value if it is a valid date. Otherwise, returns an empty string.</returns>
        public static string ChangeDateFormat(string dateValue, string currentFormat, string newFormat)
        {
            if (string.IsNullOrEmpty(dateValue)) { return string.Empty; }
            string value = string.Empty;
            DateTime dateTime;

            bool success = DateTime.TryParseExact(dateValue, currentFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            if (!success) { success = DateTime.TryParse(dateValue, out dateTime); }
            if (success) { value = dateTime.ToString(newFormat); }

            return value;
        }

        /// <summary> By: Yeswanth 
        /// Gets a string and returns Datatable.
        /// </summary>
        /// <param XML_To_Table>XML string .</param>
        /// <returns>Returns Table after converting is the input string is valid XML data, else returns an empty string.</returns>
        public static DataTable Convert_XMLstring_To_Datatable(string XML_To_Table)
        {

            DataTable Return_Table = new DataTable();

            try
            {
                if (!string.IsNullOrEmpty(XML_To_Table))
                {
                    DataSet dataSet = new DataSet();
                    StringReader stringReader = new StringReader(XML_To_Table);
                    dataSet.ReadXml(stringReader);
                    Return_Table = dataSet.Tables[0];
                }
            }
            catch (Exception) { }

            return Return_Table;
        }


        /// <summary>
        /// Clears temporary files uploaded.
        /// </summary>
        /// <param name="globalGuid"></param>
        /// <returns></returns>
        public static bool ClearTempFolder(string globalGuid, string userID)
        {
            try
            {
                string directory = CommonFunctions.GetUserFolder(globalGuid, userID);
                Directory.Delete(directory, true);
            }
            catch (Exception)
            {
            }
            return true;
        }

        /// <summary>
        /// Clears temporary files uploaded.
        /// </summary>
        /// <param name="globalGuid"></param>
        /// <returns></returns>
        public static bool ClearTempFolder(string globalGuid, string userID, string fileName)
        {
            try
            {
                string directoryPath = CommonFunctions.GetUserFolder(globalGuid, userID);
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.Exists)
                {
                    FileInfo[] fileInfoArray = directoryInfo.GetFiles(fileName, SearchOption.AllDirectories);
                    foreach (FileInfo fileInfo in fileInfoArray)
                    {
                        if (fileInfo != null)
                        {
                            fileInfo.Delete();
                            DirectoryInfo fileDirectoryInfo = fileInfo.Directory;
                            if (fileDirectoryInfo.GetFiles().Length + fileDirectoryInfo.GetDirectories().Length == 0)
                            {
                                fileDirectoryInfo.Delete();
                            }
                        }
                    }
                    if (Directory.Exists(directoryInfo.FullName) && directoryInfo.GetFiles().Length + directoryInfo.GetDirectories().Length == 0)
                    {
                        directoryInfo.Delete();
                    }
                }

            }
            catch (Exception)
            {
            }
            return true;
        }

        /// <summary>
        /// Clears the users cache.
        /// </summary>
        /// <param name="userID">The user id.</param>
        /// <param name="startsWith">The beginning of the cache key name.</param>
        public static void ClearUserCache(string userID, string startsWith)
        {
            //Loop through cached values
            foreach (DictionaryEntry dictionaryEntry in HttpRuntime.Cache)
            {
                if (dictionaryEntry.Key.ToString().StartsWith(startsWith) && dictionaryEntry.Key.ToString().EndsWith(Consts.Common.Underscore + userID))
                {
                    HttpRuntime.Cache.Remove(dictionaryEntry.Key.ToString());
                }
            }
        }

        /// <summary>
        /// Creates a folder struction from a file path.
        /// </summary>
        /// <param name="filePath">The full path to the file from which the structure will be created.</param>
        /// <returns>True if successfull.</returns>
        public static bool CreateFolderStructure(string filePath)
        {
            try
            {
                if (filePath != null)
                {
                    string path = Path.GetDirectoryName(filePath);
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string data, bool isFileName)
        {
            if (string.IsNullOrEmpty(data)) { return default(T); }

            T objectOut = default(T);

            try
            {
                lock (_serialization)
                {
                    string attributeXml = string.Empty;
                    string xmlString = string.Empty;

                    if (isFileName)
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(data);
                        xmlString = xmlDocument.OuterXml;
                    }
                    else
                    {
                        xmlString = data;
                    }

                    using (StringReader read = new StringReader(xmlString))
                    {
                        Type outType = typeof(T);

                        XmlSerializer serializer = new XmlSerializer(outType);
                        using (XmlReader reader = new XmlTextReader(read))
                        {
                            objectOut = (T)serializer.Deserialize(reader);
                            reader.Close();
                        }

                        read.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //LogExceptionWithDisplay(new StackFrame(true), ex, Captain<string>.Session[Consts.SessionVariables.UserID], ExceptionSeverityLevel.Error, new string[0]);
            }

            return objectOut;
        }

        /// <summary>
        /// Formats date string to DD-MMM-YYYY format
        /// </summary>
        /// <param name="srcDate"></param>
        /// <returns></returns>
        public static string FormatDateString(string srcDate)
        {
            try
            {
                if (srcDate != string.Empty && srcDate.Trim().Length > 0)
                {
                    return Convert.ToDateTime(srcDate).ToString("");
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get a path and makes sure that it is valid by removing invalid characters.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetValidPath(string path)
        {
            string regexSearch = string.Format(Consts.Parameters.Zero + Consts.Parameters.One, new string(Path.GetInvalidFileNameChars()), new string(Path.GetInvalidPathChars()));
            Regex r = new Regex(string.Format(Consts.Common.OpenBracket + Consts.Parameters.Zero + Consts.Common.CloseBracket, Regex.Escape(regexSearch)));

            return r.Replace(path, string.Empty);
        }

        /// <summary>
        /// Generates a folder path from a file path.
        /// </summary>
        /// <param name="filePath">The full qualified file path.</param>
        /// <returns>True if the folder structure is created. Otherwise, returns false.</returns>
        /// <example>bool success = GenerageFolderStructure("C:\Test\TestCases\testcase.doc")</example>
        public static bool GenerateFolderStructure(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    string path = Path.GetDirectoryName(filePath);
                    Directory.CreateDirectory(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="isDropDown"></param>
        /// <returns></returns>
        public static CellType GetColumnType(string columnType, bool isDropDown)
        {
            CellType cellType = CellType.TextBox;

            switch (columnType.ToLower())
            {
                case Consts.DataTypes.Boolean:
                    cellType = CellType.CheckBox;
                    break;
                case Consts.DataTypes.UserType:
                    cellType = CellType.TextBox;
                    break;
                case Consts.DataTypes.String:
                    cellType = isDropDown ? CellType.ComboBox : CellType.TextBox;
                    break;
                case Consts.DataTypes.Percent:
                case Consts.DataTypes.Number:
                    cellType = CellType.TextBox;
                    break;
                case Consts.DataTypes.DateTime:
                    cellType = CellType.DateTimePicker;
                    break;
                case Consts.DataTypes.Date:
                    cellType = CellType.DatePicker;
                    break;
                case Consts.DataTypes.Time:
                    cellType = CellType.TextBox;
                    break;
            }

            return cellType;
        }

        /// <summary>
        /// Returns config value.
        /// </summary>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static T GetConfigValue<T>(string valueName, T defaultValue)
        {
            try
            {
                object returnValue = null;

                bool isKeyExist = WebConfigurationManager.AppSettings.AllKeys.Cast<string>().ToList().Find(k => k.Equals(valueName)) != null;
                if (!isKeyExist) { return defaultValue; }

                switch (typeof(T).Name.ToLower())
                {
                    case "datetime":
                        returnValue = DateTime.Parse(WebConfigurationManager.AppSettings[valueName]);
                        break;
                    case "int32":
                        returnValue = int.Parse(WebConfigurationManager.AppSettings[valueName]);
                        break;
                    case "int64":
                        returnValue = long.Parse(WebConfigurationManager.AppSettings[valueName]);
                        break;
                    case "boolean":
                        returnValue = bool.Parse(WebConfigurationManager.AppSettings[valueName]);
                        break;
                    default:
                        returnValue = WebConfigurationManager.AppSettings[valueName];
                        break;
                }

                return null == returnValue ? defaultValue : (T)returnValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Used to get a confirmation message when deleting a node.
        /// </summary>
        /// <param name="isObjectHasActiveTask"></param>
        /// <param name="hasChildrenForAnyRow"></param>
        /// <param name="isDocument"></param>
        /// <returns></returns>
        public static string GetNodeDeleteConfirmationMessage(bool isObjectHasActiveTask, bool hasChildrenForAnyRow, bool isDocument)
        {
            string message = string.Empty;

            if (isObjectHasActiveTask)
            {
                if (hasChildrenForAnyRow && !isDocument)
                {
                    //message = Consts.Messages.ChildItemsAndActiveTasksCompleteOrCancelBeforeDeleting.GetMessage();
                }
                else if (isDocument && !hasChildrenForAnyRow)
                {
                    // message = Consts.Messages.ItemHasActiveTasksCompleteOrCancelBeforeDeleting.GetMessage();
                }
                else
                {
                    //message = Consts.Messages.AreYouSureYouWantToDeleteItemAndActiveTasks.GetMessage();
                }
            }
            else
            {
                if (hasChildrenForAnyRow)
                {
                    // message = Consts.Messages.ChildItemsAreYouSureYouWantToDelete.GetMessage();
                }
                else
                {
                    // message = Consts.Messages.AreYouSureYouWantToDelete.GetMessage();
                }
            }

            return message;
        }

        /// <summary>
        /// Gets the
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static XmlDocument GetEmbeddedXmlDocument(string embededXmlResource)
        {
            Assembly compounds = Assembly.GetExecutingAssembly();
            using (Stream readStream = compounds.GetManifestResourceStream(embededXmlResource))
            {
                XmlTextReader xmlTextReader = new XmlTextReader(readStream);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlTextReader);

                return xmlDocument;
            }
        }

        /// <summary>
        /// Gets the description for an enumeration which has been assigned an DescriptionAttribute tag
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="defaultDescription"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object enumValue, string defaultDescription)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return defaultDescription;
        }

        /// <summary>
        /// Get the language from the browser.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static CultureInfo GetBrowserCulture(HttpContext httpContext)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(Consts.Common.DefaultLanguage);

            string[] languages = httpContext.Request.UserLanguages;

            if (languages != null && languages.Length != 0)
            {
                try
                {
                    string language = languages[0].ToLowerInvariant().Trim();
                    cultureInfo = CultureInfo.CreateSpecificCulture(language);
                }
                catch (ArgumentException)
                {
                }
            }

            return cultureInfo;
        }

        /// <summary>
        /// Gets the content of a file as base64
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileContent(string fileName)
        {
            string content = string.Empty;

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, Convert.ToInt32(fileStream.Length));
                content = Convert.ToBase64String(fileBytes, Base64FormattingOptions.None);
            }

            return content;
        }

        /// <summary>
        /// Gets dates in the future.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static string GetFutureDates(string dates)
        {
            try
            {
                string strFutureDates = string.Empty;
                if (dates == "")
                {
                    System.DateTime today = System.DateTime.Now;
                    strFutureDates = FormatDateString(today.AddDays(GetRandomUsers()).ToString());
                }
                else
                {
                    strFutureDates = FormatDateString(dates);
                }

                return strFutureDates;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshInterval"></param>
        /// <returns></returns>
        public static int GetIntervalInMilliseconds(int refreshInterval)
        {
            int interval = 0;

            switch (refreshInterval)
            {
                case 1: //10 Seconds
                    interval = 1000 * 10;
                    break;
                case 2: //20 Seconds
                    interval = 1000 * 20;
                    break;
                case 3: //30 Seconds
                    interval = 1000 * 30;
                    break;
                case 4: //40 Seconds
                    interval = 1000 * 40;
                    break;
                case 5: //50 Seconds
                    interval = 1000 * 50;
                    break;
                case 6: //1 Minute
                    interval = 1000 * 60;
                    break;
                case 7: //2 Minute
                    interval = 2000 * 60;
                    break;
                case 8: //3 Minute
                    interval = 3000 * 60;
                    break;
                case 9: //4 Minute
                    interval = 4000 * 60;
                    break;
                case 10: //5 Minute
                    interval = 5000 * 60;
                    break;
            }

            return interval;
        }

        /// <summary>
        /// Gets an open no content tab.
        /// </summary>
        /// <param name="ContentTabs"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public static TabPage GetNoContentTab(TabControl ContentTabs)
        {
            TabPage results = null;

            foreach (TabPage workspaceTab in ContentTabs.TabPages)
            {
                //if (workspaceTab.Controls[0] is NoContentControl)
                //{
                //    results = workspaceTab;
                //    break;
                //}
            }

            return results;
        }

        /// <summary>
        /// Gets an open tab.
        /// </summary>
        /// <param name="ContentTabs"></param>
        /// <param name="tagClass"></param>
        /// <returns></returns>
        public static TabPage GetOpenTab(TabControl ContentTabs, TagClass tagClass)
        {


            TabPage results = null;

            foreach (TabPage workspaceTab in ContentTabs.TabPages)
            {
                TagClass wsTagClass = workspaceTab.Tag as TagClass;

                //This will be added back when versions is implemented in the download/upload control - Alex
                //&& wsTagClass.MajorVersion == tagClass.MajorVersion && wsTagClass.MinorVersion == tagClass.MinorVersion
                if (wsTagClass.ObjectID == tagClass.ObjectID)
                {
                    results = workspaceTab;
                    break;
                }
            }

            return results;
        }

        /// <summary>
        /// Returns CTDPath by concatenating the name of the parent nodes.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="ctdPath"></param>
        private static void GetParentNodes(XmlNode xmlNode, ref string ctdPath)
        {
            try
            {
                if (xmlNode.ParentNode != null)
                {
                    ctdPath = ctdPath + xmlNode.ParentNode.Name + Consts.Common.Slash;
                    GetParentNodes(xmlNode.ParentNode, ref ctdPath);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Geenrates a Random Number within the specified Range.
        /// </summary>
        /// <returns></returns>
        public static int GetRandomUsers()
        {
            try
            {
                // STUBS REMOVAL  :  Removed  Acced DB code from ?GetRandomUsers? method
                System.Random intRandomGenerator = new System.Random();
                return intRandomGenerator.Next(1, 9);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a select one item for a combobox
        /// </summary>
        /// <returns></returns>
        public static ListItem GetSelectOneItem()
        {
            return new ListItem() { Text = Consts.Common.SelectOne, ID = string.Empty, Value = string.Empty };
        }

        /// <summary>
        /// Gets a Undefined item for a combobox
        /// </summary>
        /// <returns></returns>
        public static ListItem GetUndefinedItem()
        {
            return new ListItem() { Text = Consts.Common.UnDefined, ID = string.Empty, Value = string.Empty };
        }

        /// <summary>
        /// Gets a all item for a combobox
        /// </summary>
        /// <returns></returns>
        public static ListItem GetAllItem()
        {
            return new ListItem() { Text = Consts.Common.All, ID = string.Empty, Value = Consts.Common.Zero };
        }

        /// <summary>
        /// Gets a clear value item for a combobox
        /// </summary>
        /// <returns></returns>
        public static ListItem GetClearValueItem()
        {
            return new ListItem() { Text = Consts.Common.ClearValue, ID = string.Empty, Value = string.Empty };
        }

        /// <summary>
        /// Gets a list of selected taxonomy nodes.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="list"></param>
        /// <param name="parentNodeID"></param>
        public static void GetSelectedSpecificNodes(TreeNodeCollection nodes, List<TagClass> list, string parentNodeID)
        {
            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode.Checked)
                {
                    TagClass tagClass = (TagClass)treeNode.Tag;
                    tagClass.ParentNodeID = parentNodeID;
                    list.Add(tagClass);
                    if (treeNode.Nodes.Count > 0) //Was treeNode.HasNodes
                    {
                        GetSelectedSpecificNodes(treeNode.Nodes, list, tagClass.NodeID);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a server path for a file
        /// </summary>
        /// <param name="globalGuid"></param>
        /// <param name="filesName"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetServerFile(string globalGuid, string filesName, string userID)
        {
            string serverFile = CommonFunctions.GetUserFolder(globalGuid, userID) + filesName;

            string upDownRootPath = CommonFunctions.GetConfigValue(Consts.Common.UpDownRootPath, Consts.Common.DefaultUpDownRootPath);
            string upDownRootDrive = CommonFunctions.GetConfigValue(Consts.Common.UpDownRootDrive, Consts.Common.DefaultUpDownRootPath);

            serverFile = serverFile.Replace(upDownRootPath, upDownRootDrive);

            return serverFile;
        }

        /// <summary>
        /// Genareates a tooltip based on a TagClass
        /// </summary>
        /// <param name="tagClass">The TagClass object.</param>
        /// <returns></returns>
        public static string GetMenuToolTip(TagClass tagClass)
        {
            StringBuilder toolTip = new StringBuilder();

#if DEBUG
            toolTip.Append("Node Type: ");
#endif
            return toolTip.ToString();
        }

        /// <summary>
        /// Genareates a tooltip based on a TagClass
        /// </summary>
        /// <param name="tagClass">The TagClass object.</param>
        /// <returns></returns>
        public static string GetTagClassToolTip(TagClass tagClass)
        {
            StringBuilder toolTip = new StringBuilder();

#if DEBUG
            toolTip.Append("Name: ");

#endif

            return toolTip.ToString();
        }

        /// <summary>
        /// Gets a fully qualified file path.
        /// </summary>
        /// <param name="fileGuid">The file guid. This is used for uploading and will be appended to the path.</param>
        /// <param name="userID">This is the user's id. This allows to get the users file folder.</param>
        /// <returns>Retuns a folder path string.</returns>
        public static string GetUserFolder(string fileGuid, string userID)
        {
            string userFolder = string.Empty;
            string upDownRootPath = CommonFunctions.GetConfigValue(Consts.Common.UpDownRootPath, Consts.Common.DefaultUpDownRootPath);

            userFolder = userID.Equals(string.Empty) ? upDownRootPath : Path.Combine(upDownRootPath, userID);

            userFolder = fileGuid.Trim().Equals(string.Empty) ? userFolder : Path.Combine(userFolder, fileGuid);

            if (userFolder.LastIndexOf(Consts.Common.BackSlash) == userFolder.Length - 1)
            {
                return userFolder;
            }
            else
            {
                return userFolder + Consts.Common.BackSlash;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_xmlDocument"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public static XmlAttribute GetXmlNodeAttribute(XmlDocument _xmlDocument, string attributeName, string attributeValue)
        {
            XmlAttribute xmlAttr = null;
            try
            {
                xmlAttr = (XmlAttribute)_xmlDocument.CreateNode(XmlNodeType.Attribute, attributeName, string.Empty);
                xmlAttr.Value = attributeValue;
            }
            catch (Exception ex)
            {
                StackFrame stackframe = new StackFrame(1, true);
                //               ExceptionLogger.LogException(stackframe, ex, ExceptionSeverityLevel.Low);
            }
            return xmlAttr;
        }

        /// <summary>
        /// Gets a string value from an XmlNode given the path of the node.
        /// </summary>
        /// <param name="xmlNode">The XmlNode that contains the value.</param>
        /// <param name="xPath">The path to the single node.</param>
        /// <returns>A string value.</returns>
        public static string GetXmlValue(XmlNode xmlNode, string xPath)
        {
            string xmlString = string.Empty;
            try
            {
                if (xmlNode.SelectSingleNode(xPath) != null)
                {
                    xmlString = xmlNode.SelectSingleNode(xPath).InnerText;
                    return xmlString;
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the value from a report option.
        /// </summary>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <param name="reportOptions"></param>
        /// <returns></returns>
        public static string GetReportOptions(string optionName, string reportOptions)
        {
            string reptOptionvalue = reportOptions.Replace(Consts.Common.LessThan + optionName + Consts.Common.GreaterThan, string.Empty);
            return reptOptionvalue.Replace(Consts.Common.LessThan + Consts.Common.Slash + optionName + Consts.Common.GreaterThan, string.Empty).Trim();
        }

        /// <summary>
        /// Returns true if the value is length of 4 and all are digits.
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool Is4DigitInteger(string theValue)
        {
            bool returnValue;
            try
            {
                if (theValue.Length == 4)
                {
                    int dummyValue;
                    returnValue = Int32.TryParse(theValue, out dummyValue);
                }

                else
                {
                    returnValue = false;
                }
                return returnValue;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Gives whether the text is alphanumeric or not
        /// </summary>
        /// <param name="matchText"></param>   
        public static bool IsAlphaNumeric(string matchText)
        {
            try
            {
                System.Text.RegularExpressions.Match match = Regex.Match(matchText, "", RegexOptions.IgnoreCase);
                return match.Success;
            }
            catch (Exception ex)
            {
                // ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
                return false;
            }
        }

        /// <summary>
        /// Checks for numbers only
        /// </summary>
        /// <param name="matchText"></param>
        /// <returns></returns>
        public static bool IsNumeric(string matchText)
        {
            try
            {
                System.Text.RegularExpressions.Match match = Regex.Match(matchText, Consts.StaticVars.DecimalString, RegexOptions.IgnoreCase);
                return match.Success;
            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
                return false;
            }
        }

        /// <summary>
        /// Gives whether the text is alphanumeric or not
        /// </summary>
        /// <param name="matchText"></param>   
        public static bool IsExtendedAlphaNumeric(string matchText)
        {
            try
            {
                System.Text.RegularExpressions.Match match = Regex.Match(matchText, Consts.StaticVars.AlphaNumericString, RegexOptions.IgnoreCase);
                return match.Success;
            }
            catch (Exception ex)
            {
                // ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
                return false;
            }
        }

        /// <summary>
        /// Gives whether the text is alphanumeric or not
        /// </summary>
        /// <param name="matchText"></param>   
        public static bool IsAlpha(string matchText)
        {
            try
            {
                System.Text.RegularExpressions.Match match = Regex.Match(matchText, Consts.StaticVars.AlphaString, RegexOptions.IgnoreCase);
                return match.Success;
            }
            catch (Exception ex)
            {
                // ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
                return false;
            }
        }

        public static bool IsDecimalValid(string matchText)
        {
            bool boolvalid = true;
            try
            {
                Convert.ToDecimal(matchText);
            }
            catch (Exception ex)
            {
                boolvalid = false;
            }
            return boolvalid;
        }
        /// <summary>
        /// Returns true if the selected treeview node is an application; returns false if the node is not initialized or is not a application
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool IsApplicationNode(TreeNode node)
        {
            bool answer = false;
            try
            {
                if (!(node == null))
                {
                    // Get the tag class from the node
                    if (!(node.Tag == null))
                    {
                        TagClass tagClass = (TagClass)node.Tag;
                    }
                }

                return answer;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the mode is debug.
        /// </summary>
        /// <returns></returns>
        public static bool CheckDebugMode()
        {
            try
            {
                return GetConfigValue(Consts.Common.IsDebugMode, false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns true if the value is integer.
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool IsInteger(string theValue)
        {
            bool returnValue = false;
            try
            {
                int dummyValue;
                returnValue = Int32.TryParse(theValue, out dummyValue);
                return returnValue;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determins if a tab is open.
        /// </summary>
        /// <param name="ContentTabs"></param>
        /// <param name="tagClass"></param>
        /// <returns></returns>
        public static bool IsTabOpen(TabControl ContentTabs, TagClass tagClass)
        {
            return GetOpenTab(ContentTabs, tagClass) != null;
        }

        /// <summary>
        /// Gives whether the email is valid (or) not
        /// </summary>
        /// <param name="emailID"></param>      
        public static bool IsValidEmail(string emailID)
        {
            try
            {
                System.Text.RegularExpressions.Match match = Regex.Match(emailID.Trim(), Consts.StaticVars.EmailValidatingString, RegexOptions.IgnoreCase);
                return match.Success;
            }
            catch (Exception ex)
            {
                StackFrame stackFrame = new StackFrame();
                // ExceptionLogger.LogAndDisplayMessageToUser(stackFrame, ex, QuantumFaults.None, ExceptionSeverityLevel.High);
                return false;
            }
        }

        /// <summary>
        /// Function to get first 100 chars appended with ...
        /// </summary>
        /// <param name="originalValue">The original string</param>
        /// <returns>The modified string</returns>
        public static string LimitStringToHundredCharacters(string originalValue)
        {
            return LimitStringToNCharacters(originalValue, 100);
        }

        /// <summary>
        /// Function to get first N chars appended with ...
        /// </summary>
        /// <param name="originalValue">The original string</param>
        /// <param name="maxCharacters">The maximum characters to get from the string.</param>
        /// <returns>The modified string</returns>
        public static string LimitStringToNCharacters(string originalValue, int maxCharacters)
        {
            string resultValue = string.Empty;
            if (originalValue.Length <= maxCharacters)
            {
                resultValue = originalValue;
            }
            else
            {
                resultValue = originalValue.Substring(0, maxCharacters);
                resultValue = resultValue + Consts.Common.ThreeDots;
            }
            return resultValue;
        }

        /// <summary>
        /// Log errors to log file using log4net
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="userID"></param>
        /// <param name="errorMessage"></param>
        public static void LogToFile(StackFrame frame, string[] parameters, string userID, string errorMessage)
        {
            ExceptionLoggingUtility logError = new ExceptionLoggingUtility();
            logError.WriteToLogFile(frame, parameters, errorMessage, userID, ExceptionSeverityLevel.Error);
            logError = null;
        }

        /// <summary>
        /// Logs the service calls to a trace.log file using log4net.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <param name="userID"></param>
        public static void LogToFile(StackFrame frame, string[] parameters, string userID, string[] response)
        {
            ExceptionLoggingUtility logMessage = new ExceptionLoggingUtility();
            logMessage.DebugToLogFile(frame, parameters, response, userID);
            logMessage = null;
        }

        /// <summary>
        /// Logs the service calls to a trace.log file using log4net.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="userID"></param>
        public static void LogToFile<TRequest>(StackFrame frame, TRequest request, string response, string userID)
        {
            ExceptionLoggingUtility logMessage = new ExceptionLoggingUtility();

            logMessage.DebugToLogFile(frame, request, response, userID);
            logMessage = null;
        }

        /// <summary>
        /// Log errors to trace.txt file under viewpoint\logs
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logName"></param>
        public static void LogToFile(string message, string logName)
        {
            if (GetConfigValue(Consts.Common.IsDebugMode, false))
            {
                lock (Consts.CacheKeys.Locker)
                {
                    using (StreamWriter sw = File.AppendText(@"C:\Captain\Logs\" + logName + ".log"))
                    {
                        sw.WriteLine(message);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Reads from one stream, writes it to the other stream and closes both streams.
        /// </summary>
        /// <param name="readStream">The source stream.</param>
        /// <param name="writeStream">The target stream. </param>
        /// <remarks>This method is used to get resource stream and save it to a file stream. Once done it will close both streams.</remarks>
        public static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            try
            {
                int length = 256;
                Byte[] buffer = new Byte[length];
                int bytesRead = readStream.Read(buffer, 0, length);
                // write the required bytes
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = readStream.Read(buffer, 0, length);
                }
                readStream.Close();
                writeStream.Close();
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        public static string SerializeObject<T>(T serializableObject)
        {
            string xmlString = string.Empty;

            if (serializableObject == null) { return xmlString; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlString = xmlDocument.OuterXml;
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //LogExceptionWithDisplay(new StackFrame(true), ex, Captain<string>.Session[Consts.SessionVariables.UserID], ExceptionSeverityLevel.Error, new string[0]);
            }

            return xmlString;
        }

        /// <summary>
        /// Sets the cache user and system cache expiration timeout.
        /// </summary>
        public static void SetCacheExpiration()
        {
            GlobalVariables.CacheExpiration = GetConfigValue(Consts.Common.CacheExpiration, 1);
            GlobalVariables.SystemCacheExpiration = GetConfigValue(Consts.Common.SystemCacheExpiration, 24);
        }

        /// <summary>
        /// Set properties for a DataGridView column.
        /// </summary>
        /// <param name="column">DataGridView column.</param>
        /// <param name="headerText">Column header text.</param>
        /// <param name="padding">Column padding.</param>
        /// <param name="visible">Indicates if column should be displayed.</param>
        public static void SetGridColumn(DataGridViewColumn column, string headerText, Nullable<Padding> padding, bool visible)
        {
            if (padding.HasValue)
            {
                DataGridViewCellStyle dgvCellStyle = new DataGridViewCellStyle();
                dgvCellStyle.Padding = padding.Value;
                column.DefaultCellStyle = dgvCellStyle;
            }
            column.HeaderText = headerText;
            column.ToolTipText = headerText;
            column.Visible = visible;
        }

        /// <summary>
        /// Create a new node or sets the value of an existing payload node.
        /// </summary>
        /// <param name="payload">The existing payload.</param>
        /// <param name="payloadNodeType">The type of node to add to the payload.</param>
        /// <param name="nodeName">The name of the node.</param>
        /// <param name="nodeValue">The value of the node.</param>
        /// <returns></returns>
        public static List<XmlNode> SetPayloadNode(XmlNode[] payload, PayloadNodeTypes payloadNodeType, string nodeName, string nodeValue)
        {
            List<XmlNode> payloadList = new List<XmlNode>(payload);
            XmlNode xmlNode = payloadList.Find(n => n.Name.Equals(nodeName));

            if (xmlNode == null)
            {
                if (payloadNodeType == PayloadNodeTypes.Simple)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    string nodeXml = Consts.Common.TaskNodeTemplate;
                    nodeXml = string.Format(nodeXml, nodeName, nodeValue);
                    xmlDocument.LoadXml(nodeXml);

                    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                    namespaceManager.AddNamespace(Consts.Common.TaskPrefix, Consts.Common.TaskNamespace);
                    xmlNode = xmlDocument.SelectSingleNode(Consts.Common.TaskPrefix + Consts.Common.Colon + nodeName, namespaceManager);
                    xmlNode.InnerText = nodeValue;
                }
                payloadList.Add(xmlNode);
            }
            else
            {
                xmlNode.InnerText = nodeValue;
            }

            return payloadList;
        }

        /// <summary>
        /// Checks if a node can be expanded.
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public static bool IsExpandable(TreeNode treeNode)
        {
            bool isExpandable = false;

            for (int nodeIndex = 0; nodeIndex < treeNode.Nodes.Count; nodeIndex++)
            {
                TreeNode node = treeNode.Nodes[nodeIndex];
                isExpandable = node.Nodes.Count > 0 && !node.IsExpanded;
                if (isExpandable)
                    break;
                else if (node.Nodes.Count > 0)
                    isExpandable = IsExpandable(node);
            }

            return isExpandable;
        }

        /// <summary>
        /// Grid view cell tool tip added.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="addOperator"></param>
        /// <param name="addDate"></param>
        /// <param name="updateOperator"></param>
        /// <param name="updateDate"></param>
        /// <param name="datagridview"></param>
        public static void setTooltip(int rowIndex, string addOperator, string addDate, string updateOperator, string updateDate, DataGridView datagridview)
        {
            string toolTipText = Consts.Common.AddedBy + addOperator.Trim() + Consts.Common.On + addDate.ToString() + Consts.Common.NewLine;
            string modifiedBy = string.Empty;
            if (!updateOperator.ToString().Trim().Equals(string.Empty))
                modifiedBy = updateOperator.ToString().Trim() + Consts.Common.On + updateDate.ToString();
            toolTipText += Consts.Common.ModifiedBy + modifiedBy;

            foreach (DataGridViewCell cell in datagridview.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }



        /// <summary>
        /// Grid view cell tool tip added.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="addOperator"></param>
        /// <param name="addDate"></param>
        /// <param name="updateOperator"></param>
        /// <param name="updateDate"></param>
        /// <param name="datagridview"></param>
        public static void setTooltip(int rowIndex, string addOperator, string addDate, string updateOperator, string updateDate, DataGridView datagridview, string strApplicantNo)
        {
            string toolTipText = "Applicant No : " + strApplicantNo;
            string modifiedBy = string.Empty;
            string addedby = string.Empty;
            //+ Consts.Common.NewLine + Consts.Common.AddedBy + addOperator.Trim() + Consts.Common.On + addDate.ToString() + Consts.Common.NewLine
            if (!addOperator.ToString().Trim().Equals(string.Empty))
            {
                addedby = Consts.Common.NewLine + Consts.Common.AddedBy + addOperator.Trim() + Consts.Common.On + addDate.ToString();
                toolTipText += addedby;
            }
            if (!updateOperator.ToString().Trim().Equals(string.Empty))
            {
                modifiedBy = updateOperator.ToString().Trim() + Consts.Common.On + updateDate.ToString();
                toolTipText += Consts.Common.NewLine + Consts.Common.ModifiedBy + modifiedBy;
            }
            foreach (DataGridViewCell cell in datagridview.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        public static void setTooltip(int rowIndex, string updateOperator, string updateDate, DataGridView datagridview)
        {
            string toolTipText = string.Empty;
            string modifiedBy = string.Empty;
            if (!updateOperator.ToString().Trim().Equals(string.Empty))
            {
                modifiedBy = updateOperator.ToString().Trim() + Consts.Common.On + updateDate.ToString();
                toolTipText += Consts.Common.NewLine + Consts.Common.ModifiedBy + modifiedBy;
            }
            foreach (DataGridViewCell cell in datagridview.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }



        public static string GetHierachyFormat(string strAgency, string strDept, string strProgram, string strYear, string HIEREPRSNTN)
        {
            string strName = string.Empty;

            switch (HIEREPRSNTN)
            {
                case "01":
                    strName = strAgency;
                    break;
                case "02":
                    strName = strAgency + Consts.Common.TabSpace + strDept;
                    break;
                case "03":
                    strName = strAgency + Consts.Common.TabSpace + strProgram;
                    break;
                case "04":
                    strName = strAgency + Consts.Common.TabSpace + Consts.Common.TabSpace + Consts.Common.TabSpace + strDept + Consts.Common.TabSpace + Consts.Common.TabSpace + Consts.Common.TabSpace + strProgram;
                    break;
                case "05":
                    strName = strAgency + Consts.Common.TabSpace + strProgram + Consts.Common.TabSpace + strDept;
                    break;
                case "06":
                    strName = strDept;
                    break;
                case "07":
                    strName = strDept + Consts.Common.TabSpace + strAgency;
                    break;
                case "08":
                    strName = strDept + Consts.Common.TabSpace + strProgram;
                    break;
                case "09":
                    strName = strDept + Consts.Common.TabSpace + strProgram + Consts.Common.TabSpace + strAgency;
                    break;
                case "10":
                    strName = strDept + Consts.Common.TabSpace + strAgency + Consts.Common.TabSpace + strProgram;
                    break;
                case "11":
                    strName = strProgram;
                    break;
                case "12":
                    strName = strProgram + Consts.Common.TabSpace + strAgency;
                    break;
                case "13":
                    strName = strProgram + Consts.Common.TabSpace + strDept;
                    break;
                case "14":
                    strName = strProgram + Consts.Common.TabSpace + strAgency + Consts.Common.TabSpace + strDept;
                    break;
                case "15":
                    strName = strProgram + Consts.Common.TabSpace + strDept + Consts.Common.TabSpace + strAgency;
                    break;


            }
            return strName + Consts.Common.TabSpace + Consts.Common.TabSpace + Consts.Common.TabSpace + Consts.Common.TabSpace + Consts.Common.TabSpace + strYear;
        }

        public static string GetHTMLHierachyFormat(string strAgency, string strDept, string strProgram, string strYear, string HIEREPRSNTN)
        {
            string strName = string.Empty;

            switch (HIEREPRSNTN)
            {
                case "01":
                    strName = strAgency;
                    break;
                case "02":
                    strName = strAgency + Consts.Common.HTMLTabSpace + strDept;
                    break;
                case "03":
                    strName = strAgency + Consts.Common.HTMLTabSpace + strProgram;
                    break;
                case "04":
                    strName = strAgency + Consts.Common.HTMLTabSpace + strDept + Consts.Common.HTMLTabSpace + strProgram;
                    break;
                case "05":
                    strName = strAgency + Consts.Common.HTMLTabSpace + strProgram + Consts.Common.HTMLTabSpace + strDept;
                    break;
                case "06":
                    strName = strDept;
                    break;
                case "07":
                    strName = strDept + Consts.Common.HTMLTabSpace + strAgency;
                    break;
                case "08":
                    strName = strDept + Consts.Common.HTMLTabSpace + strProgram;
                    break;
                case "09":
                    strName = strDept + Consts.Common.HTMLTabSpace + strProgram + Consts.Common.HTMLTabSpace + strAgency;
                    break;
                case "10":
                    strName = strDept + Consts.Common.HTMLTabSpace + strAgency + Consts.Common.HTMLTabSpace + strProgram;
                    break;
                case "11":
                    strName = strProgram;
                    break;
                case "12":
                    strName = strProgram + Consts.Common.HTMLTabSpace + strAgency;
                    break;
                case "13":
                    strName = strProgram + Consts.Common.HTMLTabSpace + strDept;
                    break;
                case "14":
                    strName = strProgram + Consts.Common.HTMLTabSpace + strAgency + Consts.Common.HTMLTabSpace + strDept;
                    break;
                case "15":
                    strName = strProgram + Consts.Common.HTMLTabSpace + strDept + Consts.Common.HTMLTabSpace + strAgency;
                    break;


            }
            return strName + Consts.Common.HTMLTabSpace + Consts.Common.HTMLTabSpace + Consts.Common.HTMLTabSpace + strYear;
        }

        public static void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (Convert.ToString(li.Value).Trim().Equals(value.Trim()) || Convert.ToString(li.Text).Trim().Equals(value.Trim()))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }
        public static string FollowupIndicatior(string strFollowupdate)
        {

            string strType = string.Empty;
            if (strFollowupdate != string.Empty)
            {
                if (Convert.ToDateTime(strFollowupdate) <= DateTime.Now.Date)
                {
                    strType = "R";
                }
                else
                {
                    if (Convert.ToDateTime(strFollowupdate) > DateTime.Now.Date && Convert.ToDateTime(strFollowupdate) <= DateTime.Now.Date.AddDays(7))
                    {
                        strType = "Y";
                    }
                    else
                    {
                        strType = "B";
                    }
                }
            }
            return strType;
        }
        public static HierarchyEntity GetHierachyNameFormat(string Agency, string Dept, string Program)
        {
            CaptainModel _model = new CaptainModel();
            return _model.HierarchyAndPrograms.GetCaseHierarchyName(Agency, Dept, Program);

        }

        public static void MessageBoxDisplay(string strMsg)
        {
            //MessageBox.Show(strMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            AlertBox.Show(strMsg, MessageBoxIcon.Warning);
        }

        public static string CalculationYear(DateTime dtstartDate, DateTime dtIntakeDate)
        {
            int intDiffInYears = 0;
            string strYears = "0";
            if (dtIntakeDate.Year > dtstartDate.Year)
            {
                if (dtIntakeDate.Month >= dtstartDate.Month)
                {
                    if (dtIntakeDate.Day >= dtstartDate.Day)
                    {   // this is a best case, easy subtraction situation
                        intDiffInYears = dtIntakeDate.Year - dtstartDate.Year;
                        strYears = intDiffInYears.ToString();
                    }
                    else
                    {
                        if (dtIntakeDate.Month == dtstartDate.Month)
                        {
                            intDiffInYears = dtIntakeDate.Year - dtstartDate.Year - 1;
                            strYears = intDiffInYears.ToString();

                        }
                        else
                        {
                            intDiffInYears = dtIntakeDate.Year - dtstartDate.Year;
                            strYears = intDiffInYears.ToString();
                        }
                    }
                }
                else
                {
                    intDiffInYears = dtIntakeDate.Year - dtstartDate.Year - 1;
                    strYears = intDiffInYears.ToString();
                }
            }
            //else
            //{
            //    if (dtIntakeDate.Year > dtstartDate.Year)
            //    {
            //        intDiffInYears = dtIntakeDate.Year - dtstartDate.Year - 1;
            //        strYears = intDiffInYears.ToString();
            //    }

            //}

            return strYears;

        }
        /// <summary>
        ///   Agytabs Table Filter AgyType and Hierchy wise
        /// </summary>
        /// <param name="commonAgyList"> BaseAgyTabsEntity</param>
        /// <param name="AgyCode">AgyCode</param>
        /// <param name="Agency">Agency</param>
        /// <param name="Dept">Dept</param>
        /// <param name="Prog">Program</param>
        /// <returns></returns>
        public static List<CommonEntity> AgyTabsFilterCode(List<CommonEntity> commonAgyList, string AgyCode, string Agency, string Dept, string Prog)
        {
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = commonAgyList.FindAll(u => u.AgyCode == AgyCode);
            if (_AgytabsFilter.Count > 0)
            {
                _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")).ToList();
                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }
            return _AgytabsFilter;
        }


        /// <summary>
        ///   Agytabs Table Filter AgyType and Hierchy wise
        /// </summary>
        /// <param name="commonAgyList"> BaseAgyTabsEntity</param>
        /// <param name="AgyCode">AgyCode</param>
        /// <param name="Agency">Agency</param>
        /// <param name="Dept">Dept</param>
        /// <param name="Prog">Program</param>
        ///  <param name="Mode">Mode</param>
        /// <returns></returns>
        public static List<CommonEntity> AgyTabsFilterCode(List<CommonEntity> commonAgyList, string AgyCode, string Agency, string Dept, string Prog, string Mode)
        {
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = commonAgyList.FindAll(u => u.AgyCode == AgyCode);
            if (_AgytabsFilter.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                if (_AgytabsFilter.Count > 0)
                {
                    if (_AgytabsFilter[0].Sort == "C")
                        _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Code).ToList();
                    else
                        _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
                }
            }
            return _AgytabsFilter;
        }

        public static List<CommonEntity> AgyTabsFilterCodeDescription(List<CommonEntity> commonAgyList, string AgyCode, string Agency, string Dept, string Prog, string Mode)   // Brought update - Vikash 01032023 for Funding Source Report
        {
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = commonAgyList.FindAll(u => u.AgyCode == AgyCode);
            if (_AgytabsFilter.Count > 0)
            {



                _AgytabsFilter = _AgytabsFilter.OrderBy(u => u.Desc).ToList();
            }
            return _AgytabsFilter;
        }


        /// <summary>
        ///   Agytabs Table Filter AgyType and Hierchy wise
        /// </summary>
        /// <param name="commonAgyList"> BaseAgyTabsEntity</param>
        /// <param name="AgyCode">AgyCode</param>
        /// <param name="Agency">Agency</param>
        /// <param name="Dept">Dept</param>
        /// <param name="Prog">Program</param>
        ///  <param name="Mode">Mode</param>
        /// <returns></returns>
        public static List<CommonEntity> AgyTabsFilterCodeStatus(List<CommonEntity> commonAgyList, string AgyCode, string Agency, string Dept, string Prog, string Mode)
        {
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = commonAgyList.FindAll(u => u.AgyCode == AgyCode);
            if (_AgytabsFilter.Count > 0)
            {

                if (Mode.ToUpper() == "A")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.Active.ToString().ToUpper() == "Y").ToList();//(u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******"))
                }
                else if (Mode.ToUpper() == "I")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.Active.ToString().ToUpper() == "N").ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }
            return _AgytabsFilter;
        }



        /// <summary>
        ///   Agytabs Table Filter AgyType and Hierchy wise
        /// </summary>
        /// <param name="commonAgyList"> BaseAgyTabsEntity</param>
        /// <param name="AgyCode">AgyCode</param>
        /// <param name="Agency">Agency</param>
        /// <param name="Dept">Dept</param>
        /// <param name="Prog">Program</param>
        ///  <param name="Mode">Mode</param>
        /// <returns></returns>
        public static List<CommonEntity> AgyTabsFilterOrderbyCode(List<CommonEntity> commonAgyList, string AgyCode, string Agency, string Dept, string Prog, string Mode)
        {
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = commonAgyList.FindAll(u => u.AgyCode == AgyCode);
            if (_AgytabsFilter.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Code).ToList();
            }
            return _AgytabsFilter;
        }

        /// <summary>
        ///   Agytabs Table Filter AgyType and Hierchy wise
        /// </summary>
        /// <param name="commonAgyList"> BaseAgyTabsEntity</param>
        /// <param name="AgyCode">AgyCode</param>
        /// <param name="Agency">Agency</param>
        /// <param name="Dept">Dept</param>
        /// <param name="Prog">Program</param>
        ///  <param name="Mode">Mode</param>
        /// <returns></returns>
        public static List<CommonEntity> AgyTabsDecisionCodeFilters(List<CommonEntity> commonAgyList, string AgyCode, string Agency, string Dept, string Prog, string Mode)
        {
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = commonAgyList;
            if (_AgytabsFilter.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();//u.Extension.ToString() != ""
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(Agency + Dept + Prog) || u.ListHierarchy.Contains(Agency + Dept + "**") || u.ListHierarchy.Contains(Agency + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Code).ToList();
            }
            return _AgytabsFilter;
        }


        public static string GetLIMPQuesResp(string strcode)
        {
            string strDesc = string.Empty;
            if (strcode == "Y")
                strDesc = "Yes";
            if (strcode == "N")
                strDesc = "No";
            if (strcode == "U")
                strDesc = "Not Applicable";

            return strDesc;
        }

        public static List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues, string Mode, string Agency, string Depart, string Program)
        {
            string HIE = Agency + Depart + Program;
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            //_AgytabsFilter = LookupValues;
            if (LookupValues.Count > 0)
            {
                int i = 0; bool Can_Continue = true;
                foreach (CommonEntity Entity in LookupValues)
                {
                    string Temp = Entity.Hierarchy.ToString().Trim();
                    Can_Continue = true; i = 0;
                    if (!string.IsNullOrEmpty(Temp.Trim()))
                    {
                        for (i = 0; Can_Continue;)
                        {
                            string TempCode = Temp.Substring(i, 6);

                            if (HIE == "******")
                                _AgytabsFilter.Add(Entity);
                            else if (Depart + Program == "****")
                            {
                                if (TempCode.Substring(0, 2).ToString().Trim() == Agency)
                                    _AgytabsFilter.Add(Entity);
                            }
                            else if (Program == "**")
                            {
                                if (TempCode.Substring(0, 4).ToString().Trim() == Agency + Depart)
                                    _AgytabsFilter.Add(Entity);
                            }
                            else
                            {
                                if (TempCode == HIE)
                                    _AgytabsFilter.Add(Entity);
                                else if (TempCode.Contains("**"))
                                {
                                    if (TempCode.Substring(4, 2).ToString() == "**")
                                    {
                                        if (TempCode.Substring(0, 4).ToString().Trim() == Agency + Depart)
                                            _AgytabsFilter.Add(Entity);
                                    }
                                    else if (TempCode.Substring(2, 4).ToString() == "****")
                                    {
                                        if (TempCode.Substring(0, 2).ToString().Trim() == Agency)
                                            _AgytabsFilter.Add(Entity);
                                    }
                                    else if (TempCode == "******")
                                        _AgytabsFilter.Add(Entity);
                                }
                            }


                            i += 6;
                            if (i >= Temp.Length)
                                Can_Continue = false;
                        }

                    }
                }
                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            return _AgytabsFilter;
        }

        public static string GetPhoneNo(string phnno)
        {
            try
            {
                if (phnno != string.Empty && phnno.Trim().Length > 0)
                {
                    MaskedTextBox mskPhn = new MaskedTextBox();
                    mskPhn.Mask = "000-000-0000";
                    mskPhn.Text = phnno;

                    return mskPhn.Text.Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }



        #endregion

        #region Newly Implemented Common Functions
        /***********************************************************************/
        /*   JWT- Token for Gitbook URLs
         *   Kranthi on 11/23/2022 
        /***********************************************************************/
        private static string GenerateTokenKey(string _ModuleID)
        {
            string RestokenKey = "";
            string idtoken = "";
            if (_ModuleID == "01")
                idtoken = "acce26b9-b309-4d50-99a7-4f51455b535d";   // Admin Module
            if (_ModuleID == "03")
                idtoken = "50b01a0a-ad56-41e7-9065-cddcb9ed0f88";   // Case Management
            if (_ModuleID == "07")
                idtoken = "7da54d0f-f475-435f-bba3-970f8de7ea25";   // CEAP Module
            if (_ModuleID == "CMN")
                idtoken = "b8a212a4-e367-4b30-9393-f626fb5b692b";   // Common Screens in all Modules
            try
            {
                if (idtoken != "")
                {
                    //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(idtoken));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        null,
                      //_config["Jwt:Issuer"],
                      //_config["Jwt:Issuer"],
                      null,
                      null,
                      expires: DateTime.Now.AddMinutes(60),
                      signingCredentials: credentials);

                    RestokenKey = new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            catch
            {
            }
            return RestokenKey;
        }

        /*********************************************************************
         * Create GitBook URLs based on Form Name and Subpages Number
         * Kranthi on 11/23/2022
         ******************************************************************/
        public static string BuildHelpURLS(string FormName, int PageNumber, string _ModuleID)
        {
            string resURL = "";
            //string _moduleURL = "";
            //if (_ModuleID == "01")
            //    _moduleURL = "case - management-module";
            //if (_ModuleID == "01")
            //    _moduleURL = "case - management-module";
            //if (_ModuleID == "01")
            //    _moduleURL = "case - management-module";

            string gitbookSpaceURL = "https://capsystems-1.gitbook.io/";  //"https://capsystems-1.gitbook.io/captain-help-1/";
            string PageURL = "";

            try
            {
                switch (FormName)
                {
                    #region ADMINISTRATION
                    #region SCREENS
                    case "ADMN0005": //Assign User Privilages

                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/assign-user-accounts-and-privileges";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/assign-user-accounts-and-privileges/add-edit-new-use";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/assign-user-accounts-and-privileges/assign-user-privileges";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/assign-user-accounts-and-privileges/copy-a-template-user";

                        break;
                    case "ADMN0006": //Agency Table Codes
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/agency-table-codes";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/agency-table-codes/add-edit-table-code-entry";
                        break;
                    case "FIXSSNUM": //ClientID Auidt and Review
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/client-id-audit-and-review";
                        break;
                    case "ADMN0022": //Contacts and Service Custom Questions
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/contacts-and-services-custom-questions";
                        break;
                    case "ADMN0024": //Service Posting Date Control
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/service-posting-date-control";
                        break;
                    case "ADMN0009": //Hierarchy Definition
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/hierarchy-definition";
                        break;
                    case "ADMN0010": //Master Poverty Guidelines 
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/federal-poverty-chart";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/hud";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/cmi";
                        if (PageNumber == 4) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/smi";
                        break;
                    case "ADMN0012": //Agency Control file Maintanence
                        if (PageNumber == 0) PageURL = "";
                        break;
                    case "ADMN0013": //Zipcode
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/zip-code-file-maintenace";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/zip-code-file-maintenace/add-edit-zip-code";
                        break;
                    case "ADMNCONT":    //Incomplete Intake Control
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/incomplete-intake-controls";
                        break;
                    case "CASE0008":    //Field Control Maintenance
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/fields-control-maintenance";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/fields-control-maintenance/add-edit-fields-control-in-a-specific-hierarchy";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/fields-control-maintenance/custom-fields-definition";
                        break;
                    case "CASE0009": //Eligibilty Criteria
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/eligibility-criteria";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/eligibility-criteria/add-a-question-to-a-group";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/eligibility-criteria/group-definitions";
                        break;
                    case "CASE0011": // Agency Referral Database Screen
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/agency-referral-database";
                        break;
                    case "ADMN0015":    //Site and room maintenance
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/site-and-room-maintenance";
                        break;
                    case "ADMN0020":    //SP Admin Screen
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/service-plan-admin-screen";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/service-plan-admin-screen/service-plan-admin";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/service-plan-admin-screen/add-service-outcome-to-service-plan";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/service-plan-admin-screen/validate-a-service-plan";
                        break;

                    case "TMS00009":   //Vendor Maintenance
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/vendor-maintenance";
                        break;
                    case "ADMN0016":    //Master Table Maintenance
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/service-table";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/outcome-table";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/outcome-indicators";
                        if (PageNumber == 4) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/individual-household-characteristics";
                        if (PageNumber == 5) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/services";
                        if (PageNumber == 6) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/ranking-categories";
                        break;
                    case "CASE0007":    //Program Definition
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/program-definition";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/program-definition/program-information";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/program-definition/poverty-guidelines";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/program-definition/program-switches";
                        if (PageNumber == 4) PageURL = "system-administration-module/system-administration/screens/program-definition/income-and-relation";
                        if (PageNumber == 5) PageURL = "system-administration-module/system-administration/screens/program-definition/hs-ar-controls";
                        break;
                    case "FIXFAMID":    //HouseholdID Audit and Review
                        if (PageNumber == 0) PageURL = "";
                        break;

                    case "MAT00001":    //Matrix/Scales Definitions
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-matrix";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-benchmarks";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-scales";
                        if (PageNumber == 4) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-outcomes";
                        if (PageNumber == 5) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-questions";
                        if (PageNumber == 6) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-reasons";
                        break;
                    case "MAT00002":    //Matrix/Scales Score Sheets
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/matrix-scales-score-sheet";
                        break;
                    #endregion
                    #region REPORTS
                    case "ADMNB001": // Report :: Master Tables List
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/reports/master-table-lists";
                        break;
                    case "ADMNB002":    // Report:: User tree structure
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/reports/user-tree-structure";
                        break;
                    case "ADMNB005":    // Report:: Image Types Report
                        if (PageNumber == 0) PageURL = "";
                        break;
                    #endregion
                    #endregion

                    #region CASEMANAGEMENT
                    #region SCREENS
                    case "ADVMMSEARCH":
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-search-feature/search-using-advanced-search";
                        break;

                    case "APPT0001":    // Appointment Template
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/appointment-template";
                        break;
                    case "APPT0002": //Appointment Schedule
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/appointments-schedule";
                        break;
                    case "APPT0003":    // Appointment-Reserve Schedule
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/appointment-reserve-schedule";
                        break;
                   
                    case "CASE2001":        // Client intake
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake";
                        if (PageNumber == 1) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-applicant/enter-applicant-details";
                        if (PageNumber == 2) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-applicant/enter-address-and-intake-details";
                        if (PageNumber == 3) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-applicant/complete-custom-questions";
                        if (PageNumber == 4) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-applicant/housing-and-asset-inquiry";
                        if (PageNumber == 5) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-applicant/employment-information";
                        if (PageNumber == 6) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-applicant/complete-pre-assessment";
                        if (PageNumber == 7) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-household-member";
                        if (PageNumber == 8) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-household-member/complete-custom-questions";
                        if (PageNumber == 9) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-household-member/enter-employment-information";
                        break;
                    case "CASE2003":        // Client intake
                        _ModuleID = "CMN";
                        if (PageNumber == 2) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/income-verification";
                        break;
                    
                    case "CASINCOM":
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/income-details";
                        break;

                    case "CASE0028":    // Client Followup
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-follow-up-solution/client-follow-up-on-tool";
                        break;
                    case "CASE0027":    // Client Followup Search tool
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-follow-up-solution";
                        if (PageNumber == 1) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-follow-up-solution/client-follow-up-on-search-tool";
                        break;
                    case "MAT00003":    // Matrix/Scales Assessments
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/matrix-scales-assessments";
                        break;
                    case "PIP00000":    // Public Intake Portal Hub
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-screens/public-intake-portal-hub";
                        break;


                    #endregion
                    #region REPORTS
                    case "APPTB001": //Appointment Schedule Report
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-reports/appointment-schedule-report";
                        break;
                    case "CASB0007":    // Funnel Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/funnel-report";
                        break;
                    case "CASB0008":    // Customer Intake Quality Control
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/customer-intake-quality-control";
                        break;
                    case "CASB0012":    // Adhoc Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/ad-hoc-report";
                        break;
                    case "CASB0013":    // Agency Wide Activity Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/agency-wide-service-report";
                        break;
                    case "CASB0530":    // Ranking/Risk Assessment Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/ranking-risk-assessment";
                        break;
                    case "MATB1002":    // Matrix Scale Measures Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/matrix-scales-measures-report";
                        break;
                    case "PIPB0001":    //PIP Registration Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/pip-registration-report";
                        break;
                    case "PIPB0002":    //PIP Intake Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/pip-intake-report";
                        break;
                    case "RNGB0004":    //ROMA Individual/Household Characteristics
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/roma-individual-household-characteristics";
                        break;
                    case "RNGB0005":    // Program Service And Outcome Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/program-service-and-outcomes-report";
                        break;
                    case "RNGB0014":    //ROMA Outcome Indicators
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/roma-outcome-indicators";
                        break;
                    case "RNGS0014":    //ROMA Services
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/roma-services-report";
                        break;
                    #endregion
                    #endregion

                    #region CEAP Program
                    #region Screens
                    case "CASE0016":    // Benifit Maintenance & Usage Posting
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/screens/benefit-maintenance-and-usage-posting";
                        break;
                    case "CASE0026":    // Budget Maintenance
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/screens/budget-maintenance";
                        break;
                    case "CASE0021":    // Service Payment Adjustment
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/screens/service-payment-adjustment";
                        break;
                    #endregion
                    #region Reports
                    case "TMSB0034":    // Funding Source Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/reports/funding-source-report";
                        break;
                    case "CEAPB002":    // Performance Measures Data
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/reports/performance-measures-data";
                        break;
                    case "CASB0019":    // Pledge Sheet/Bundling
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/reports/pledge-sheet-bundling";
                        break;
                    case "CASB0020":    // Request for Payment Process
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/reports/request-for-payment";
                        break;
                    case "CASB0021":    // Ser/Pay Adjustment Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/reports/ser-pay-adjustment-report";
                        break;
                    case "CASB0017":    // Usage Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "ceap-module/ceap-module/reports/usage-report";
                        break;
                    #endregion
                    #endregion


                    default:
                        if (PageNumber == 0) PageURL = "";
                        break;
                }

                if (PageURL != "")
                    resURL = gitbookSpaceURL + PageURL + "?jwt_token=" + GenerateTokenKey(_ModuleID);
            }
            catch
            {

            }

            return resURL;

        }


        /*******************************************************************************/
        /** Zen Desk URL Callings *****/
        public static string CreateZenHelps(string FormName, int PageNumber, string _ModuleID, string _AgyShortName, string _state)
        {
            string resURL = "";

            string ZendeskURL = "https://capsystems.zendesk.com/hc/en-us/articles/";
            string PageURL = "";
            try
            {
                switch (FormName)
                {

                    #region ADMINISTRATION
                    #region SCREENS
                    case "ADMN0005": //Assign User Privilages

                        if (PageNumber == 0) PageURL = "28207613369364-Assign-User-Accounts-Privileges";
                        if (PageNumber == 1) PageURL = "28208567884820-Add-Edit-New-User";
                        if (PageNumber == 2) PageURL = "28209240662676--Assign-User-Privileges";
                        if (PageNumber == 3) PageURL = "28208798410772-Copy-a-Template-User";

                        break;
                    case "ADMN0006": //Agency Table Codes
                        if (PageNumber == 0) PageURL = "28209551021460-Agency-Table-Codes";
                        if (PageNumber == 1) PageURL = "28209669725972-Agency-Table-Codes-Add-Edit-Entry";
                        break;
                    case "FIXSSNUM": //ClientID Auidt and Review
                        if (PageNumber == 0) PageURL = "28273599069972-Client-ID-Audit-Review";
                        break;
                    case "ADMN0022": //Contacts and Service Custom Questions
                        if (PageNumber == 0) PageURL = "28273827760532--Contacts-and-Services-Custom-Questions";
                        break;
                    case "ADMN0024": //Service Posting Date Control
                        if (PageNumber == 0) PageURL = "system-administration-module/system-administration/screens/service-posting-date-control";
                        break;
                    case "ADMN0009": //Hierarchy Definition
                        if (PageNumber == 0) PageURL = "28285028676244-Hierarchy-Definition";
                        break;
                    case "ADMN0010": //Master Poverty Guidelines 
                        if (PageNumber == 0) PageURL = "28285232875412-Master-Poverty-Guidelines";
                        if (PageNumber == 1) PageURL = "28285317299860-Federal-Poverty-Chart";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/hud";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/cmi";
                        if (PageNumber == 4) PageURL = "system-administration-module/system-administration/screens/master-poverty-guidelines/smi";
                        break;
                    case "ADMN0012": //Agency Control file Maintanence
                        if (PageNumber == 0) PageURL = "";
                        break;
                    case "ADMN0013": //Zipcode
                        if (PageNumber == 0) PageURL = "28380378150932-ZIP-Code-File-Maintenance";
                        if (PageNumber == 1) PageURL = "28381449427220-ZIP-Code-Add-Edit-Delete";
                        break;
                    case "ADMNCONT":    //Incomplete Intake Control
                        if (PageNumber == 0) PageURL = "28285134699924-Incomplete-Intake-Controls";
                        break;
                    case "CASE0008":    //Field Control Maintenance
                        if (PageNumber == 0) PageURL = "28284463983764-Fields-Control-Maintenance";
                        if (PageNumber == 1) PageURL = "28284818189972-Add-Edit-Fields-Control-in-a-Specific-Hierarchy";
                        if (PageNumber == 2) PageURL = "30327365203476-Custom-Fields-Definition-Add-Edit-Delete";
                        break;
                    case "CASE0009": //Eligibilty Criteria
                        if (PageNumber == 0) PageURL = "28283499959700-Eligibility-Criteria";
                        if (PageNumber == 1) PageURL = "28284037617300--Add-a-Question-to-a-Group";
                        if (PageNumber == 2) PageURL = "28283768190996-Group-Definitions";
                        break;
                    case "CASE0011": // Agency Referral Database Screen
                        if (PageNumber == 0) PageURL = "28209916837780-Agency-Referral-Database";//"system-administration-module/system-administration/screens/agency-referral-database";
                        break;
                    case "ADMN0015":    //Site and room maintenance
                        if (PageNumber == 0) PageURL = "28337883780116-Site-Room-Maintenance";
                        break;
                    case "ADMN0020":    //SP Admin Screen
                        if (PageNumber == 0) PageURL = "28338089015572-Service-Plan-Admin-Screen";// "28386230993556-Service-Plan-Admin";
                        if (PageNumber == 1) PageURL = "28338194231188-Add-Service-Outcome-to-Service-Plan"; //PageURL = "28338089015572-Service-Plan-Admin-Screen";
                        if (PageNumber == 2) PageURL = "28338194231188-Add-Service-Outcome-to-Service-Plan";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/service-plan-admin-screen/validate-a-service-plan";
                        break;

                    case "TMS00009":   //Vendor Maintenance
                        if (PageNumber == 0) PageURL = "28379844594324--Vendor-Maintenance";
                        break;
                    case "ADMN0016":    //Master Table Maintenance
                        if (PageNumber == 0) PageURL = "29560560725012-Master-Table-Maintenance";
                        if (PageNumber == 1) PageURL = "28334720414228-Service-Table";
                        if (PageNumber == 2) PageURL = "28373293299860-Outcome-Table";
                        if (PageNumber == 3) PageURL = "29579708745620--Outcome-Indicators";// "system-administration-module/system-administration/screens/master-table-maintenance/outcome-indicators";
                        if (PageNumber == 4) PageURL = "28334814618900-Individual-Household-Characteristics";
                        if (PageNumber == 5) PageURL = "system-administration-module/system-administration/screens/master-table-maintenance/services";
                        if (PageNumber == 6) PageURL = "28213725917332-Ranking-Categories";
                        break;
                    case "CASE0007":    //Program Definition
                        if (PageNumber == 0) PageURL = "28335945342228-Program-Definition";
                        if (PageNumber == 1) PageURL = "28373753441044-Program-Information";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/program-definition/poverty-guidelines";
                        if (PageNumber == 3) PageURL = "28336321488148-Program-Switches";
                        if (PageNumber == 4) PageURL = "28336377991956-Income-Relation";
                        if (PageNumber == 5) PageURL = "28336466960276-Head-Start-Accounts-Receivable-Controls";

                        if (PageNumber == 6) PageURL = "28336263055764-Program-Definition-Tab";
                        break;
                    case "FIXFAMID":    //HouseholdID Audit and Review
                        if (PageNumber == 0) PageURL = "";
                        break;

                    case "MAT00001":    //Matrix/Scales Definitions
                        if (PageNumber == 0) PageURL = "28335118912404-Matrix-Scales-Definitions";
                        if (PageNumber == 1) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-matrix";
                        if (PageNumber == 2) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-benchmarks";
                        if (PageNumber == 3) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-scales";
                        if (PageNumber == 4) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-outcomes";
                        if (PageNumber == 5) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-questions";
                        if (PageNumber == 6) PageURL = "system-administration-module/system-administration/screens/matrix-scales-definitions/add-edit-reasons";
                        break;
                    case "MAT00002":    //Matrix/Scales Score Sheets
                        if (PageNumber == 0) PageURL = "28335303599892-Matrix-Scales-Score-Sheet";
                        break;
                    #endregion
                    #region REPORTS
                    case "ADMNB001": // Report :: Master Tables List
                        if (PageNumber == 0) PageURL = "28391563004820--Master-Table-List";
                        break;
                    case "ADMNB002":    // Report:: User tree structure
                        if (PageNumber == 0) PageURL = "28282995931412-User-Tree-Structure";
                        break;
                    case "ADMNB005":    // Report:: Image Types Report
                        if (PageNumber == 0) PageURL = "";
                        break;
                    case "ADMN0030":    // Report:: Image Types Report
                        if (PageNumber == 0) PageURL = "29423689510676--Target-Numbers-Enter-Service-Plan-Target-Entry";
                        if (PageNumber == 1) PageURL = "29422178795412-Target-Numbers-Enter-Program-Level";

                        break;
                    #endregion
                    #endregion

                    #region HeadStart /Early Childhood Educ.
                    #region Screens
                    case "HSS00137": // Screen :: Attendence by posting screen
                        if (PageNumber == 0) PageURL = "28277585562900-Attendance-Posting-by-Site";
                        break;
                    case "HSS00138": // Screen :: Bus master / Route Maintenance screen
                        if (PageNumber == 0) PageURL = "28553704810644-Bus-Master-Maintenance";
                        if (PageNumber == 1) PageURL = "28553704810644-Bus-Master-Maintenance";
                        break;
                    case "HSS00140": // Screen :: Bus Client Placement
                        if (PageNumber == 0) PageURL = "28530526881812-Bus-Client-Placement";
                        break;
                    case "HSS00134": // Screen :: Client Tracking
                        if (PageNumber == 0)
                            PageURL = "28284771347860-Client-Tracking";
                        break;
                    case "ENRLHIST": // Screen :: Enroll History
                        if (PageNumber == 0)
                            PageURL = "28274157078932--Enroll-History";
                        break;
                    //case "CASE0010": // Screen :: Enrollment/Withdrawals
                    //    if (PageNumber == 0)
                    //        PageURL = "28589753482772--Enrollment-Withdrawals";
                    //    break;
                    case "CASE2330": // Screen :: Medical/Emergency
                        if (PageNumber == 0)
                            PageURL = "28554893115028-Medical-Emergency";
                        break;
                    case "HSS00430": // Screen :: Non-Custodial Parent
                        if (PageNumber == 0)
                            PageURL = "28911080633108-Non-Custodial-Parent";
                        break;
                    case "PIR00000": // Screen :: PIR Control
                        if (PageNumber == 0)
                            PageURL = "28601315084948-PIR-Control";
                        break;
                    case "PIR30001": // Screen :: PIR Logic Association 2.0
                        if (PageNumber == 0)
                            PageURL = "28602704090132-PIR-Logic-Association";
                        break;
                    case "HSS00135": // Screen :: Site Schedule
                        if (PageNumber == 0)
                            PageURL = "28555679936916-Site-Schedule";
                        if (PageNumber == 1)
                            PageURL = "28555679936916-Site-Schedule";
                        break;
                    case "STFMST10": // Screen :: Staff Master Maintenance
                        if (PageNumber == 0)
                            PageURL = "28702342092180-Staff-Master-Maintenance";
                        break;
                    case "HSS00001": // Screen :: Staff to Client Associations
                        if (PageNumber == 0)
                            PageURL = "28702812271636-Staff-to-Client-Associations";
                        break;
                    case "HSS00133": // Screen :: Track Master Maintenance
                        if (PageNumber == 0)
                            PageURL = "28567203117588-Track-Master-Maintenance";
                        break;

                    #endregion
                    #region Reports

                    case "HSSB0108": // Report :: Attendence Sheet Report
                        if (PageNumber == 0) PageURL = "28605762882324-Attendance-Sheets";
                        break;
                    case "HSSB0111": // Report :: Average Daily Attendence Report
                        if (PageNumber == 0) PageURL = "28606089351060-Average-Daily-Attendance";
                        break;
                    case "HSSB0118": // Report :: Bus Lists Report
                        if (PageNumber == 0) PageURL = "28606907100308-Bus-Lists";
                        break;
                    case "HSSB0100": // Report :: Child Lists/Counts Report
                        if (PageNumber == 0) PageURL = "28698055412628-Child-Lists-Counts";
                        break;
                    case "HSSB0106": // Report :: Client track list Report
                        if (PageNumber == 0) PageURL = "28699815854228-Child-Track-List";
                        break;
                    case "HSSB0109": // Report :: Detail Attendance
                        if (PageNumber == 0)
                            PageURL = "28697258983828-Detail-Attendance";
                        break;
                    case "HSSB0104": // Report :: Enrollment Summary
                        if (PageNumber == 0)
                            PageURL = "28696258310292-Enrollment-Summary";
                        break;
                    case "HSSB0123": // Report :: Growth Charts Plus Interface
                        if (PageNumber == 0)
                            PageURL = "28637707734420-Growth-Charts-Plus-Interface";
                        break;
                    case "HSSB0124": // Report :: Next Year's Preparation
                        if (PageNumber == 0)
                            PageURL = "28630681490964-Next-Year-s-Preparation";
                        break;
                    case "HSSB1026": // Report :: PIR Counting Tool 2.0
                        if (PageNumber == 0)
                            PageURL = "28628513551892-PIR-Counting-Tool";
                        break;
                    case "HSSB0103": // Report :: Select Child Tracking Tasks
                        if (PageNumber == 0)
                            PageURL = "28626644935700-Select-Child-Tracking-Tasks";
                        break;
                    case "HSSB0114": // Report :: Site Track Tasks To Address
                        if (PageNumber == 0)
                            PageURL = "28625228780180--Site-Track-Tasks-To-Address";
                        break;
                    case "HSSB0102": // Report :: Track File Lists
                        if (PageNumber == 0)
                            PageURL = "28624702253204-Track-File-Lists";
                        break;
                    case "HSSB0112": // Report :: USDA Meal Reimbursement
                        if (PageNumber == 0)
                            PageURL = "28623706284180-USDA-Meal-Reimbursement";
                        break;
                    case "HSSB0115": // Report :: Waiting List Report
                        if (PageNumber == 0)
                            PageURL = "28624223176596-Waiting-List";
                        break;

                    #endregion
                    #endregion

                    #region CASEMANAGEMENT
                    #region SCREENS
                    case "COPYDRAG":
                        if (PageNumber == 0) PageURL = "26856216845460-Copy-Drag-This-Client";
                        break;
                    case "ADVMMSEARCH":
                        if (PageNumber == 0) PageURL = "26853163740948-Search-Feature-Advanced-Search";
                        break;
                    case "CASE0010":
                        if (PageNumber == 0) PageURL = "28699602267668-Program-Enrollment"; // for module 3 program enrollment
                        if (PageNumber == 1) PageURL = "28589753482772--Enrollment-Withdrawals"; // for module 2 program enrollment

                        break;
                    case "CASE2001":        // Client intake
                        if (PageNumber == 0) PageURL = "26493446562836-Client-Intake";
                        if (PageNumber == 1) PageURL = "26566360374292-Applicant-Details";
                        if (PageNumber == 2) PageURL = "26577115361940-Address-Intake-Details";
                        if (PageNumber == 3) PageURL = "26597143152916-Custom-Questions";
                        if (PageNumber == 4) PageURL = "26597654579476-Housing-Asset-Inquiry";
                        if (PageNumber == 5) PageURL = "26597972650772-Employment-Information";
                        if (PageNumber == 6) PageURL = "26598110363156-Pre-Assessment";
                        if (PageNumber == 7) PageURL = "captain-common-screens-and-reports/common-captain-screens/client-intake/add-edit-household-member";
                        if (PageNumber == 8) PageURL = "26597143152916-Custom-Questions";
                        if (PageNumber == 9) PageURL = "26597972650772-Employment-Information";
                        break;
                    case "CASE2003":        // Client intake --- Income verification
                        _ModuleID = "CMN";
                        if (PageNumber == 2) PageURL = "26704764767380-Income-Verification";
                        break;

                    case "CASINCOM":
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "26598270157076-Income-Details";
                        break;
                    case "IMGUPLOD":
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "29350759479444-Image-Document-Upload";
                        break;
                    case "CASE2004":        // SIM - Service Integration Matrix
                        if (PageNumber == 0) PageURL = "26889393775124-Service-Integration-Matrix-SIM";
                        break;
                    case "PIP00000":    // Public Intake Portal Hub
                        if (PageNumber == 0) PageURL = "28128240120852-PIP-Hub";
                        break;
                    case "CASE0006":    // Service Posting
                        if (PageNumber == 0) PageURL = "28308404224788-Service-Payment-Adjustment"; 
                        if (PageNumber == 1) PageURL = "29385721622932-Service-Outcome-Bulk-Posting";
                        if (PageNumber == 3) PageURL = "28214481538196-Contact-Details-Add-Edit-Delete";

                        if (_state == "TX")
                        { if (PageNumber == 4) { PageURL = "28278457467924-Add-Edit-Services-CEAP"; } }
                        else
                        {
                            if (PageNumber == 4) { PageURL = "28281587177364-Add-Edit-Services"; }
                        }
                        //    if (PageNumber == 1) PageURL = "captain-common-screens-and-reports/common-captain-screens/service-posting/add-edit-a-service-plan";
                        //if (PageNumber == 2) PageURL = "captain-common-screens-and-reports/common-captain-screens/service-posting/add-edit-a-referral";
                        //if (PageNumber == 4) PageURL = "captain-common-screens-and-reports/common-captain-screens/service-posting/add-edit-a-service-plan/add-edit-services-ceap";
                        //if (PageNumber == 5) PageURL = "captain-common-screens-and-reports/common-captain-screens/service-posting/add-edit-a-service-plan/add-edit-services-non-ceap";
                        //if (PageNumber == 6) PageURL = "captain-common-screens-and-reports/common-captain-screens/service-posting/add-edit-a-service-plan/add-edit-outcomes";
                        break;
                    case "MAT00003":    // Matrix/Scales Assessments
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "26889667868436-Matrix-Scales-Assessments";
                        break;
                    case "CASE0012":    // Inkind Service Maintenance
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "28710657612180-Inkind-Service-Maintenance";
                        break;
                    case "CASE0028":    // Client Followup
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "28303933058964-Client-Follow-up-On-Tool";
                        break;
                    case "CASE0027":    // Client Followup Search tool
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "28303698460820-Client-Follow-up-Solution";
                        if (PageNumber == 1) PageURL = "28304060515860-Client-Follow-Up-On-Search-Tool";
                        break;
                    case "TMS00100":    // Client Followup
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "28308632796308-Appointments-Template";
                        break;

                    #endregion
                    #region REPORTS
                    case "APPTB001": //Appointment Schedule Report
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "captain-common-screens-and-reports/common-captain-reports/appointment-schedule-report";
                        break;
                    case "CASB0007":    // Funnel Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28280248992916-Funnel-Report";
                        break;
                    case "CASB0008":    // Customer Intake Quality Control
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28280450010772-Customer-Intake-Quality-Control";
                        break;
                    case "CASB0012":    // Adhoc Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28214221826196-Ad-hoc-Report";
                        break;
                    case "CASB0013":    // Agency Wide Activity Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28204684202388--Agency-Wide-Service-Report";
                        break;
                    case "CASB0530":    // Ranking/Risk Assessment Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28302087109652-Ranking-Risk-Assessment";
                        break;
                    case "MATB1002":    // Matrix Scale Measures Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28279376035988-Matrix-Scales-Measures-Report";
                        break;
                    case "PIPB0001":    //PIP Registration Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/pip-registration-report";
                        break;
                    case "PIPB0002":    //PIP Intake Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "case-management-module/case-management-module/reports/pip-intake-report";
                        break;
                    case "RNGB0004":    //ROMA Individual/Household Characteristics
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28302283634708-ROMA-Individual-Household-Characteristics-Report";
                        break;
                    case "RNGB0005":    // Program Service And Outcome Report
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28301763402516-Program-Service-Outcomes-Report";
                        break;
                    case "RNGB0014":    //ROMA Outcome Indicators
                        _ModuleID = "03";
                        if (PageNumber == 0) PageURL = "28302785899540-ROMA-Outcome-Indicators-Report";// "case-management-module/case-management-module/reports/roma-outcome-indicators";
                        break;
                    case "RNGS0014":    //ROMA Services
                        if (PageNumber == 0) PageURL = "28302917435668-ROMA-Services-Report"; //"28302917435668-ROMA-Services-Report";
                        break;
                    #endregion
                    #endregion


                    #region CEAP Program
                    #region Screens
                    case "CASE0016":    // Benifit Maintenance & Usage Posting
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28308140201364-Benefit-Maintenance-Usage-Posting";
                        break;
                    case "CASE0026":    // Budget Maintenance
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28308237747348-Budget-Maintenance";
                        break;
                    case "CASE0021":    // Service Payment Adjustment
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28308404224788-Service-Payment-Adjustment";
                        break;
                    #endregion
                    #region Reports
                    case "TMSB0034":    // Funding Source Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28307685136660-Funding-Source-Report";
                        break;
                    case "CEAPB002":    // Performance Measures Data
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28307843676180-Performance-Measures-Data";
                        break;
                    case "CASB0019":    // Pledge Sheet/Bundling
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28308067262356-Pledge-Sheet-Bundling";
                        break;
                    case "CASB0020":    // Request for Payment Process
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28334474985108-Request-for-Payment";
                        break;
                    case "CASB0021":    // Ser/Pay Adjustment Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28334555099284-Ser-Pay-Adjustment-Report";
                        break;
                    case "CASB0017":    // Usage Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28307204871828-Usage-Report";
                        break;
                    #endregion
                    #endregion
                    case "APPT0002": //Appointment Schedule
                        _ModuleID = "CMN";
                        if (PageNumber == 0) PageURL = "28206896793492-Appointments-Schedule";
                        break;
                    case "TMSB0110":    // Appointment Schedule Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28277262020628-Appointments-Schedule-Report";
                        break;
                    case "TMS00120":    // Appointment Schedule Report
                        _ModuleID = "07";
                        if (PageNumber == 0) PageURL = "28308737767316-Appointments-Reserve-Schedule";
                        break;
                    case "UNIV":
                        if (PageNumber == 0) { PageURL = "https://capsystems.zendesk.com/hc/en-us"; ZendeskURL = ""; }
                        break;
                }

                //if (_zenTokenKey == "")
                _zenTokenKey = GenerateZendeskTokenKey();

                if (PageURL != "")
                    resURL = GetZendeskLoginUrl(_zenTokenKey, ZendeskURL + PageURL);
            }
            catch
            {

            }
            return resURL;
        }
        public static string GetZendeskLoginUrl(string jwtToken, string returnTo)
        {
            return $"https://capsystems.zendesk.com/access/jwt?jwt={jwtToken}&return_to={Uri.EscapeDataString(returnTo)}";
        }

        //********//
        /** Zendesk JWT - Token Generation *******/
        public static string _zenTokenKey = "";
        private static string GenerateZendeskTokenKey()
        {
            string RestokenKey = "";
            string SecretKey = "jTKFgexkas2ZqRAYM0nJlhdsMAreZC8uWFztU8ERqPl3TKEH";
            string name = "CAPSystems Zendesk"; //"Jen Goodman"; 
            string email = "capsystems.zendesk@gmail.com";// "jgoodman@littledixie.org";
            try
            {
                if (SecretKey != "")
                {
                    TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                    int timestamp = (int)t.TotalSeconds;

                    var payload = new Dictionary<string, object>() {
                { "iat", timestamp },
                { "jti", System.Guid.NewGuid().ToString() },
                { "name", name },
                { "email", email }
            };

                    string token = JWT.JsonWebToken.Encode(payload, SecretKey, JWT.JwtHashAlgorithm.HS256);
                    RestokenKey = token;
                }
            }
            catch
            {
            }


            return RestokenKey;
        }

        public static string ZendeskLogin(string Url)
        {
            _zenTokenKey = GenerateZendeskTokenKey();
            string ResUrl = GetZendeskLoginUrl(_zenTokenKey, Url);
            return ResUrl;
        }

        /* Gridview Filter and Select the row on user key press*/

        public static void KeyPress(DataGridView dataGrid, int cellIndex, char keyChar)
        {
            #region Wisej Code
            //if (Char.IsLetterOrDigit(keyChar))
            //{
            //    var search = keyChar.ToString();
            //    var searchColumn = dataGrid.Columns[cellIndex];

            //    var row = dataGrid.Rows.FirstOrDefault(r =>
            //    {
            //        return r[searchColumn].Value.ToString().StartsWith(search, StringComparison.CurrentCultureIgnoreCase);
            //    });


            //    if (row != null)
            //    {
            //        dataGrid.ClearSelection();

            //        row.Selected = true;
            //        dataGrid.ScrollRowIntoView(row);
            //    }
            //}
            #endregion;

            #region Kranthi's Code

            if (Char.IsLetterOrDigit(keyChar))
            {

                List<DataGridViewRow> SelectedgvRows = (from c in dataGrid.Rows.Cast<DataGridViewRow>().ToList()
                                                        where ((c.Cells[cellIndex]).Value.ToString().ToUpper().StartsWith(keyChar.ToString().ToUpper()))
                                                        select c).ToList();
                if (SelectedgvRows.Count > 0)
                {
                    int i = 0;
                    foreach (DataGridViewRow dgvRow in SelectedgvRows)
                    {

                        if (i == 1)
                        {
                            dataGrid.ClearSelection();
                            dgvRow.Selected = true;
                            dataGrid.ScrollRowIntoView(dgvRow);
                            i = 0;
                            return;
                        }
                        if (dgvRow.Selected == true)
                            i = 1;
                    }

                    dataGrid.ClearSelection();
                    foreach (DataGridViewRow dgvRow in SelectedgvRows)
                    {
                        dgvRow.Selected = true;
                        dataGrid.ScrollRowIntoView(dgvRow);
                        break;
                    }
                }
                else
                {
                    AlertBox.Show("Key not found!", MessageBoxIcon.Warning, null, System.Drawing.ContentAlignment.BottomRight);
                }
            }

            #endregion
        }


        /* Search a string value in Datagridview by navigating Previous and Next */
        public static void NavigateStringSearch(DataGridView dataGrid, int cellIndex, string strSearchKey, string searchType)
        {
            if (strSearchKey != "")
            {

                List<DataGridViewRow> SelectedgvRows = (from c in dataGrid.Rows.Cast<DataGridViewRow>().ToList()
                                                        where ((c.Cells[cellIndex]).Value.ToString().ToUpper().Contains(strSearchKey.ToString().ToUpper()))
                                                        select c).ToList();
                if (SelectedgvRows.Count > 0)
                {
                    int i = 0; int currentIndex = 0;
                    //if (searchType == "N")
                    //{
                    foreach (DataGridViewRow dgvRow in SelectedgvRows)
                    {

                        if (i == 1)
                        {
                            dataGrid.ClearSelection();

                            if (searchType == "N")
                                dgvRow.Selected = true;
                            else if (searchType == "P")
                            {
                                currentIndex = dgvRow.ClientIndex;
                                if ((currentIndex - 1 - 1) >= 0)
                                    SelectedgvRows[currentIndex - 1 - 1].Selected = true;
                                else
                                    SelectedgvRows[0].Selected = true;
                            }

                            dataGrid.ScrollRowIntoView(dgvRow);
                            i = 0;
                            return;
                        }
                        if (dgvRow.Selected == true)
                            i = 1;
                    }
                    //}
                    //if (searchType == "P")
                    //{
                    //    dataGrid.ClearSelection();

                    //    currentIndex= dataGrid.SelectedRows[0]

                    //    if ((currentIndex - 1) >= 0)
                    //        SelectedgvRows[currentIndex - 1].Selected = true;
                    //    else
                    //        SelectedgvRows[0].Selected = true;
                    //    //for (int x = SelectedgvRows.Count; x > 0; x--)
                    //    //{
                    //    //    dataGrid.ClearSelection();
                    //    //}
                    //}

                    dataGrid.ClearSelection();
                    foreach (DataGridViewRow dgvRow in SelectedgvRows)
                    {
                        dgvRow.Selected = true;
                        dataGrid.ScrollRowIntoView(dgvRow);
                        break;
                    }
                }
                else
                {
                    AlertBox.Show("Key not found!", MessageBoxIcon.Warning, null, System.Drawing.ContentAlignment.BottomRight);
                }
            }

        }
        #endregion


        public static int BuildHIEGrid(List<HierarchyEntity> oSPHierachies, DataGridView grdViewControl)
        {
            int _rowIndex = 0;
            try
            {
                foreach (HierarchyEntity spHIE in oSPHierachies)
                {
                    string _spAgy = (spHIE.Agency == "" ? "**" : spHIE.Agency);
                    string _spDept = (spHIE.Dept == "" ? "**" : spHIE.Dept);
                    string _spProg = (spHIE.Prog == "" ? "**" : spHIE.Prog);

                    string _spcode = _spAgy + "-" + _spDept + "-" + _spProg;
                    string _spHIEName = spHIE.HirarchyName.ToString();

                    _rowIndex = grdViewControl.Rows.Add("", "", _spcode, _spHIEName);
                    grdViewControl.Rows[_rowIndex].Tag = spHIE;
                    if (spHIE.UsedFlag.Equals("Y"))
                    {
                        grdViewControl.Rows[_rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex) { }

            return _rowIndex;

        }

        public static int BuildSerHIEGrid(List<HierarchyEntity> oSPHierachies, DataGridView grdViewControl)
        {
            int _rowIndex = 0;
            try
            {
                foreach (HierarchyEntity spHIE in oSPHierachies)
                {
                    string _spAgy = (spHIE.Agency == "" ? "**" : spHIE.Agency);
                    string _spDept = (spHIE.Dept == "" ? "**" : spHIE.Dept);
                    string _spProg = (spHIE.Prog == "" ? "**" : spHIE.Prog);

                    string _spcode = _spAgy + "-" + _spDept + "-" + _spProg;
                    string _spHIEName = spHIE.HirarchyName.ToString();

                    _rowIndex = grdViewControl.Rows.Add("", "", _spcode, _spHIEName);
                    grdViewControl.Rows[_rowIndex].Tag = spHIE;
                    setTooltip(_rowIndex, spHIE, grdViewControl);

                    if (spHIE.UsedFlag.Equals("Y"))
                    {
                        grdViewControl.Rows[_rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex) { }

            return _rowIndex;

        }
        private static void setTooltip(int rowIndex, HierarchyEntity spHIE, DataGridView gvSerHie)
        {

            string toolTipText = "Added By    : " + spHIE.AddOperator.ToString().Trim() + " on " + spHIE.DateAdd.ToString() + "\n";
            string modifiedBy = string.Empty;
            if (!spHIE.LSTCOperator.ToString().Trim().Equals(string.Empty))
                modifiedBy = spHIE.LSTCOperator.ToString().Trim() + " on " + spHIE.DateLSTC.ToString();
            toolTipText += "Modified By : " + modifiedBy;
            foreach (DataGridViewCell cell in gvSerHie.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        public static string GetAlphanumericCode()
        {
            string result = "";
            try
            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                result = new string(
                    Enumerable.Repeat(chars, 25)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray()
                );
            }
            catch { }

            return result;
        }

        public static string getDBName(string strcon)
        {
            string DBName = string.Empty;
            try
            {
                //string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString.ToString();
                var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(strcon);

                string dsrc = connectionString.DataSource;
                DBName = connectionString.InitialCatalog;
            }
            catch { }


            return DBName;
        }

        public static void CnvtCntrl2Copy(Object WisejCntrl)
        {

            var CntrlType = WisejCntrl.GetType().FullName;

            if (CntrlType == "Wisej.Web.MaskedTextBox")
            {
                MaskedTextBox txtcnrtl = WisejCntrl as MaskedTextBox;
                txtcnrtl.ReadOnly = true;
                txtcnrtl.Enabled = true;
                txtcnrtl.SelectOnEnter = true;
            }
            if (CntrlType == "Wisej.Web.TextBox")
            {
                TextBox txtcnrtl = WisejCntrl as TextBox;
                txtcnrtl.ReadOnly = true;
                txtcnrtl.Enabled = true;
                txtcnrtl.SelectOnEnter = true;
            }


        }
        public static string generateColor()
        {
            var random = new Random();

            int minValue = 127;

            int red = random.Next(minValue, 256);
            int green = random.Next(minValue, 256);
            int blue = random.Next(minValue, 256);

            int color = (red << 16) | (green << 8) | blue;

            return String.Format("#{0:X6}", color);

            //string color = String.Format("#{0:X6}", random.Next(0x1000000));
            //return color;
        }

        #region OTP SECTION
        //OTP SECTION
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomTokenNumber(int length)
        {
            var strTokenNumber = RandomString(length);
            return strTokenNumber;
        }


        public static string SendTwilloSMSService(string MsgText, string toMobileNo)
        {
            string _isOTPSent = "N";
            try
            {
                if (toMobileNo != "")
                {
                    // Find your Account SID and Auth Token at twilio.com/console
                    // and set the environment variables. See http://twil.io/secure
                    string accountSid = "ACa05625950fa41c32b6e0ff41150915d6";// Environment.GetEnvironmentVariable("ACa05625950fa41c32b6e0ff41150915d6");
                    string authToken = "3bfff993ac12e189fb631f2a7652d83f"; // Environment.GetEnvironmentVariable("3bfff993ac12e189fb631f2a7652d83f");

                    TwilioClient.Init(accountSid, authToken);

                    if (toMobileNo == "7075380141" || toMobileNo == "9849899214" || toMobileNo == "9963481181" || toMobileNo == "7075249766")
                        toMobileNo = "+91" + toMobileNo;
                    else
                        toMobileNo = "+1" + toMobileNo;

                    var message = MessageResource.Create(
                        body: MsgText,                                                 //"Join Earth's mightiest heroes. Like Kevin Bacon.",
                        from: new Twilio.Types.PhoneNumber("+12057517533"),  // Trail version Twillo Phone number provided by the Twillo Website                   //+15017122661
                        to: new Twilio.Types.PhoneNumber(toMobileNo)                        //+15558675310
                    );
                    _isOTPSent = "Y";
                }
            }
            catch (Exception ex)
            {
                _isOTPSent = "N";
            }
            return _isOTPSent;
        }

        public static string SendTextGridSMSService(string MsgText, string toMobileNo)
        {
            string _isOTPSent = "N";
            try
            {
                if (toMobileNo != "")
                {
                    // Textgrid
                    const string accountSid = "aUS5GO7UX98eIwuElE77Qw==";
                    const string authToken = "d3e258866ab946b69f404f7c04ac4492";


                    const string twilioPhoneNumber = "+19782380126";
                    string recipientPhoneNumber = toMobileNo; //"+917075380141";

                    string messageBody = MsgText;


                    var url = $"https://api.textgrid.com/2010-04-01/Accounts/{accountSid}/Messages.json";

                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(accountSid, authToken);

                        var values = new NameValueCollection
            {
                { "From", twilioPhoneNumber },
                { "To", recipientPhoneNumber },
                { "Body", messageBody }
            };

                        try
                        {
                            byte[] response = client.UploadValues(url, "POST", values);
                            string strJson = System.Text.Encoding.UTF8.GetString(response);
                            dynamic jsonArray = JsonConvert.DeserializeObject(strJson);
                            //MessageBox.Show("SMS Sent Successfully!");
                        }
                        catch (WebException ex)
                        {
                            //MessageBox.Show($"Error sending SMS: {ex.Message}");
                        }
                    }
                    _isOTPSent = "Y";
                }
            }
            catch (Exception ex)
            {
                _isOTPSent = "N";
            }

            return _isOTPSent;
        }

        public static string SendOTPEmailService(string emailID, string TokenNumber, string _AgencyName, string _officeAddr)
        {
            string _isemailSent = "Y";
            try
            {
                var model = new CaptainModel();
                var dtMailConfig = model.UserProfileAccess.GetEMailSetting("LOGINOTP");

                if (dtMailConfig.Rows.Count > 0)
                {
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString(), _AgencyName);
                    mailMessage.Subject = dtMailConfig.Rows[0]["MAIL_SUBJECT"].ToString();

                    var body = dtMailConfig.Rows[0]["MAIL_CONTENT"].ToString() + TokenNumber;
                    body = body + "<br/><br/><br/><br/><br/><br/>" +
                        _AgencyName + "<br/>" + _officeAddr;
                    //dtMailConfig.Rows[0]["MAIL_SENDER_NAME"].ToString() + "<br/>" + 
                    //dtMailConfig.Rows[0]["MAIL_SENDER_ADDR"].ToString();
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.To.Add(emailID);


                    var smtp = new SmtpClient();
                    smtp.Host = dtMailConfig.Rows[0]["MAIL_HOST"].ToString();
                    smtp.EnableSsl = true;
                    var NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString();
                    NetworkCred.Password = dtMailConfig.Rows[0]["MAIL_PASSWORD"].ToString();
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(dtMailConfig.Rows[0]["MAIL_PORT"].ToString());
                    smtp.Send(mailMessage);
                    _isemailSent = "Y";
                }
            }
            catch (Exception ex)
            {
                _isemailSent = "N";
                AlertBox.Show("Failed to send OTP. Please contact Administrator", MessageBoxIcon.Warning);
            }

            return _isemailSent;
        }

        public static string MaskEmailfunction(string strEmail)
        {
            string strMainmaskemail = string.Empty;
            try
            {
                var stremailat = strEmail.Split('@');
                if (stremailat.Length > 0)
                {
                    var email = stremailat[0].ToString();
                    var strmaskemail = string.Empty;
                    if (email.Length > 0)
                        for (var i = 0; i < email.Length; i++)
                            if (i == 0)
                                strmaskemail = strmaskemail + email[0].ToString();
                            else if (i == email.Length - 1)
                                strmaskemail = strmaskemail + email[email.Length - 1].ToString();
                            else
                                strmaskemail = strmaskemail + "*";
                    //var maskedEmail = string.Format("{0}****{1}", email[0],
                    //email.Substring(email.Length - 1));
                    var email2 = stremailat[1].ToString();
                    var strmaskemail2 = string.Empty;
                    if (email2.Length > 0)
                    {
                        var strarremail = email2.Split('.');
                        for (var i = 0; i < strarremail[0].Length; i++)
                            if (i == 0)
                                strmaskemail2 = strmaskemail2 + strarremail[0][0].ToString();
                            else if (i == strarremail[0].Length - 1)
                                strmaskemail2 = strmaskemail2 + strarremail[0][strarremail[0].Length - 1].ToString();
                            else
                                strmaskemail2 = strmaskemail2 + "*";
                        strmaskemail2 = strmaskemail2 + "." + strarremail[1];
                    }


                    //var maskedEmail2 = string.Format("{0}****{1}", email2[0],
                    //email2.Substring(email2.IndexOf('.') - 1));
                    strMainmaskemail = strmaskemail + "@" + strmaskemail2;
                }
            }
            catch (Exception ex)
            {
            }

            return strMainmaskemail;
        }

        public static string MaskMobilefunction(string mobile)
        {
            int startIndex = 2;
            string mask = "*****";

            if (string.IsNullOrEmpty(mobile))
                return string.Empty;

            string result = mobile;
            int starLengh = mask.Length;


            if (mobile.Length >= startIndex)
            {
                result = mobile.Insert(startIndex, mask);
                if (result.Length >= (startIndex + starLengh * 2))
                    result = result.Remove((startIndex + starLengh), starLengh);
                else
                    result = result.Remove((startIndex + starLengh), result.Length - (startIndex + starLengh));

            }

            return result;
        }

        public static string OPTinMessage(string OPTinDate, string jsonParms)
        {

            string strRes = "";
            if (OPTinDate == "")
            {
                //height: auto;margin: 0 auto;padding: 10px; for center
                strRes = "<br/><div style='text-align:Left; width:100%'> <div style='text-align:Left; width:55%; color:#021e5e; " +
                        "font-size:16px;font-family: calibri;position: relative; padding-top:10px;'>By opting into our communications, " +
                        "you will be able to receive your CAPTAIN MFA code via your cell number that is linked to your user account. Remember, you have the freedom to opt-out at any " +
                        "time by notifying your System Administrator.</div></div><div style='text-align:left;font-size:17px;font-family: calibri; padding-top:5px;'>";
                string _lnkText = "";
                //if (OPTinDate == "")
                    _lnkText = "Accept to get TEXT Messages";
                //else
                //    _lnkText = "Decline to get TEXT Messages";

                strRes += "<a href='" + Application.Uri.AbsoluteUri + "/optin.aspx?idparms=" + WM_EncryptUrl(jsonParms.ToString()) + "' target='blank'>" + _lnkText + "</a>";
                strRes += "</div><br/><br/>";
            }
            else
            {
                //height: auto;margin: 0 auto;padding: 10px;
                strRes = "<br/><div style='text-align:left; width:100%'> <div style='text-align:left; width:55%; color:#021e5e; " +
                        "font-size:16px;font-family: calibri;position: relative; padding-top:10px;'>" +
                        "You opted into our communications and receiving your CAPTAIN MFA code via your cell number that is linked to your user account. " +
                        "You have the freedom to opt-out at any time by notifying your System Administrator." +
                        "</div></div><br/><br/>";

            }

            return strRes;
        }
        public static string WM_EncryptUrl(string ourl)
        {
            string result = string.Empty;

            try
            {
                string _encrypturl = HttpUtility.UrlEncode(Encrypt(ourl.ToString()));
                result = _encrypturl;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        //public static List<SignParams> WM_DecryptUrl(string ourl)
        //{
        //    List<SignParams> result = new List<SignParams>();
        //    try
        //    {
        //        string _decrypturl = Decrypt(HttpUtility.UrlDecode(ourl));
        //        JavaScriptSerializer json = new JavaScriptSerializer();
        //        List<SignParams> objArray = json.Deserialize<List<SignParams>>(_decrypturl);
        //        result = objArray;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return result;
        //}

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static bool UpdateOPTDate(string _strUserID, string _optStatus)
        {
            bool result = false;
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString;
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                string sql = "";
                if (_optStatus == "A")
                    sql = "UPDATE PASSWORD SET PWR_OPT_IN=GETDATE() WHERE PWR_EMPLOYEE_NO=@USERID";
                else
                    sql = "UPDATE PASSWORD SET PWR_OPT_IN=NULL WHERE PWR_EMPLOYEE_NO=@USERID";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@USERID", _strUserID);
                int _res = cmd.ExecuteNonQuery();
                connection.Close();
                if (_res > 0)
                    result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        #endregion

        #region Department Background Colors 

        // The below Codes were referred from this link - https://www.computerhope.com/cgi-bin/htmlcolor.pl

        public static Color Get_Dept_Back_Color(string Dept)
        {
            Color background_Color = Color.White;

            switch (Dept)
            {
                case "**":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFACD");//("#966F33");
                    break;
                case "00":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#eff0f0");//("#686A6C");
                    break;
                case "01":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#f5eae9");//("#0000FF"); 
                    break;
                case "02":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#ADD8E6");
                    break;
                case "03":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#effad4");//("#FFFF33");
                    break;
                case "04":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#ddf9dd");//("#90EE90");
                    break;
                case "05":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#ccf2ff");//("#00BFFF");
                    break;
                case "06":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#e5ffff");//("#00FFFF");
                    break;
                case "07":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#f9f9fe");//("#E3E4FA");//("#000000");
                    break;
                case "08":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#f4cce3");//("#736AFF");//("#0AFFFF");
                    break;
                case "09":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#fff6e5");//("#FFA500");
                    break;
                case "10":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#e5e5e5");//("#808080");
                    break;
                case "11":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#e5e5f3");//("#00008B");
                    break;
                case "12":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#93917C");
                    break;
                case "13":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#f1f0ff");//("#454545");
                    break;
                case "14":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFCCCB");
                    break;
                case "15":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#660000");
                    break;
                case "16":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#006400");
                    break;
                case "17":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FF00FF");
                    break;
                case "18":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
                    break;
                case "19":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#9D00FF");
                    break;
                case "20":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                    break;
                case "21":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#C0C0C0");
                    break;
                case "22":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFC0CB");
                    break;
                case "23":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#800080");
                    break;
                case "24":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#008000");
                    break;
                case "25":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#f7f2f7");//("#00FF00");
                    break;
                case "26":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#E1D9D1");
                    break;
                case "27":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F70D1A");
                    break;
                case "28":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#728FCE");
                    break;
                case "29":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FDBD01");
                    break;
                case "30":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#A52A2A");
                    break;
                case "31":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFD700");
                    break;
                case "32":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#228B22");
                    break;
                case "33":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFFF4");
                    break;
                case "34":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#666362");
                    break;
                case "35":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#1E90FF");
                    break;
                case "36":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    break;
                case "37":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#357EC7");
                    break;
                case "38":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#BCC6CC");
                    break;
                case "39":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F8F0E3");
                    break;
                case "40":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#800000");
                    break;
                case "41":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFDD0");
                    break;
                case "42":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#DADBDD");
                    break;
                case "43":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#2B65EC");
                    break;
                case "44":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#36013F");
                    break;
                case "45":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#8E7618");
                    break;
                case "46":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFFF7");
                    break;
                case "47":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#22CE83");
                    break;
                case "48":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#98AFC7");
                    break;
                case "49":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FD1C03");
                    break;
                case "50":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#e5e5ff");//("#E6C7C2");//("#EB5406");
                    break;
                case "51":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#edfcfa");//("#040720");
                    break;
                case "52":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#34282C");
                    break;
                case "53":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#A9A9A9");
                    break;
                case "54":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#ADDFFF");
                    break;
                case "55":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#3B3131");
                    break;
                case "56":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#990000");
                    break;
                case "57":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#848B79");
                    break;
                case "58":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#7E3517");
                    break;
                case "59":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#8A865D");
                    break;
                case "60":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#ccf2ff");//("#00BFFF");
                    break;
                case "61":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#A70D2A");
                    break;
                case "62":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#AA6C39");
                    break;
                case "63":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#000080");
                    break;
                case "64":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F535AA");
                    break;
                case "65":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#CD5C5C");
                    break;
                case "66":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#4B5320");
                    break;
                case "67":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F67280");
                    break;
                case "68":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#82CAFF");
                    break;
                case "69":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FDD017");
                    break;
                case "70":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FBE7A1");
                    break;
                case "71":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#7FFFD4");
                    break;
                case "72":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#1569C7");
                    break;
                case "73":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#B6B6B4");
                    break;
                case "74":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#0020C2");
                    break;
                case "75":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FBF6D9");
                    break;
                case "76":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FF8C00");
                    break;
                case "77":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#E42217");
                    break;
                case "78":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#8A9A5B");
                    break;
                case "79":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#2E1A47");
                    break;
                case "80":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#008080");
                    break;
                case "81":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F75D59");
                    break;
                case "82":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#550A35");
                    break;
                case "83":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#0002FF");
                    break;
                case "84":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#C6DEFF");
                    break;
                case "85":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFCE44");
                    break;
                case "86":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#4169E1");
                    break;
                case "87":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#C9C0BB");
                    break;
                case "88":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#566D7E");
                    break;
                case "89":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#0000A0");
                    break;
                case "90":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#A8A9AD");
                    break;
                case "91":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#728C00");
                    break;
                case "92":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFFF0");
                    break;
                case "93":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F62217");
                    break;
                case "94":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#7F38EC");
                    break;
                case "95":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#C68E17");
                    break;
                case "96":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#F5F5DC");
                    break;
                case "97":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#9F8C76");
                    break;
                case "98":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    break;
                case "99":
                    background_Color = System.Drawing.ColorTranslator.FromHtml("#d3d9d0");//("#254117");
                    break;
            }

            return background_Color;
        }

        #endregion


       
    }
}

