using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Wisej.Web;
using Captain.Common.Exceptions;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Specialized;

namespace Captain.Common.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Gets a typed object from a Tag property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject"></param>
        /// <returns></returns>
        public static T GetTag<T>(this object anyObject)
        {
            T returnType = default(T);

            returnType = anyObject.GetPropertyValue<T>(Consts.PropertyNames.Tag);

            return returnType;
        }

        /// <summary>
        /// Gets a typed object from a Tag property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject"></param>
        /// <returns></returns>
        public static ListItem GetListItem(this object anyObject)
        {
            if (anyObject is ListItem)
            {
                return anyObject as ListItem;
            }
            return new ListItem();
        }

        /// <summary>
        /// Extentions to Converting XML to XDocument.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Breaks a string into a list of words.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public static List<string> Words(this string sentence)
        {
            return sentence.Split(new char[] { ' ' }).ToList<string>();
        }

        /// <summary>
        /// Flattens a menu item with children menu items.
        /// </summary>
        /// <param name="menuItem">The menu item to flatten</param>
        /// <returns></returns>
        public static List<MenuItem> Flatten(this MenuItem menuItem)
        {
            List<MenuItem> menuList = new List<MenuItem>();

            Action<MenuItem> print = null;
            print = m =>
            {
                menuList.Add(m);
                m.MenuItems.Cast<MenuItem>().ToList().ForEach(print);
            };
            print(menuItem);

            return menuList;
        }

        /// <summary>
        /// Flattens a menu item collection.
        /// </summary>
        /// <param name="menuItemCollection">The collection to flatten.</param>
        /// <returns></returns>
        public static List<MenuItem> Flatten(Menu.MenuItemCollection menuItemCollection) //Was Flatten(this MenuItemCollection menuItemCollection)
        {
            List<MenuItem> menuItems = menuItemCollection.Cast<MenuItem>().ToList();
            List<MenuItem> menuItemList = new List<MenuItem>();

            for (int menuIndex = 0; menuIndex < menuItems.Count; menuIndex++)
            {
                MenuItem menuItem = menuItems[menuIndex];
                menuItemList.AddRange(menuItem.Flatten());
            }

            return menuItemList;
        }

        /// <summary>
        /// Replaces a tagclass in a tagclass list with anotherone.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tagClass"></param>
        public static void AddTagClass(this List<TagClass> list, TagClass tagClass)
        {
            RemoveTagClass(list, tagClass.ObjectID);

            list.Add(tagClass);
        }

        /// <summary>
        /// Removes a tagclas base on an object id from a tagclass list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tagClass"></param>
        public static void RemoveTagClass(this List<TagClass> list, string objectID)
        {
            TagClass existingTagClass = list.Find(t => t.ObjectID.Equals(objectID));

            if (existingTagClass != null)
            {
                list.Remove(existingTagClass);
            }
        }

        /// <summary>
        /// Removes a spinner item from a list.
        /// </summary>
        /// <param name="spinnerList"></param>
        /// <param name="objectID"></param>
        public static SpinnerIconItem RemoveSpinner(this List<SpinnerIconItem> spinnerList, string objectID)
        {
            SpinnerIconItem item = spinnerList.Find(s => s.ObjectID.Equals(objectID));
            if (item != null) { spinnerList.Remove(item); }
            return item;
        }

        /// <summary>
        /// Removes a tagclas base on an object id from a tagclass list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tagClass"></param>
        public static TagClass GetItem(this List<TagClass> list, string objectID)
        {
            TagClass tagClass = list.Find(t => t.ObjectID.Equals(objectID));

            if (tagClass != null) { return tagClass; }

            return new TagClass();
        }

        /// <summary>
        /// Check is the fileName string has a period in its name. By windows rules, any file with a period in it has a file extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool HasFileExtension(this string fileName)
        {
            int periodIndex = fileName.LastIndexOf(Consts.Common.Period);
            return periodIndex != -1 && fileName.Substring(periodIndex).Length > 1;
        }

        /// <summary>
        /// Gets the extension of a file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExtension(this string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf(Consts.Common.Period));
        }

        /// <summary>
        /// Gets a TagClass from a Tag property
        /// </summary>
        /// <param name="typeObject"></param>
        /// <returns></returns>
        public static TagClass GetTagClass(this object typeObject)
        {
            TagClass tagClass = typeObject.GetPropertyValue<TagClass>(Consts.Common.Tag);
            return tagClass;
        }

        /// <summary>
        /// Gets a TagClass from a Tag property
        /// </summary>
        /// <param name="typeObject"></param>
        /// <param name="userID"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static TagClass GetTagClass(this object typeObject, string userID, string locale)
        {
            TagClass tagClass = typeObject.GetPropertyValue<TagClass>(Consts.Common.Tag);
            if (tagClass == null)
            {
                tagClass = new TagClass();
            }
            return tagClass;
        }

        /// <summary>
        /// Encodes an html string.
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string htmlString)
        {
            return System.Web.HttpUtility.HtmlEncode(htmlString);
        }

        /// <summary>
        /// Decodes an encoded html string
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string htmlString)
        {
            return System.Web.HttpUtility.HtmlDecode(htmlString);
        }

        /// <summary>
        /// Encrypts a string in .NET format.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string EncryptMessage(this string message)
        {
            EncryptionManager encryptionManager = new EncryptionManager();
            return encryptionManager.EncryptMessage(message, Consts.Security.EncryptionKey, Consts.Security.EncryptionIV);
        }

        /// <summary>
        /// Decrypts an .NET format encrypted string.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string DecryptMessage(this string message)
        {
            EncryptionManager encryptionManager = new EncryptionManager();
            return encryptionManager.DecryptMessage(message, Consts.Security.EncryptionKey, Consts.Security.EncryptionIV);
        }

        /// <summary>
        /// Encrypts a string in java format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncryptData(this string data)
        {
            EncryptionManager encryptionManager = new EncryptionManager();
            return encryptionManager.EncryptData(data);
        }

        /// <summary>
        /// Decrypt an encrypted string in java format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DecryptData(this string data)
        {
            EncryptionManager encryptionManager = new EncryptionManager();
            return encryptionManager.DecryptData(data);
        }

        /// <summary>
        /// Gets a message from the messages resource.
        /// </summary>
        /// <param name="messageName"></param>
        /// <returns></returns>
        public static string GetMessage(this string messageName)
        {
            return Captain.Common.Resources.CaptainResourceManager.GetMessageString(messageName);
        }

        /// <summary>
        /// Gets a message from the messages resource.
        /// </summary>
        /// <param name="messageName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetMessage(this string messageName, params object[] parameters)
        {
            string message = Captain.Common.Resources.CaptainResourceManager.GetMessageString(messageName);

            if (parameters != null && parameters.Length > 0)
            {
                message = string.Format(message, parameters);
            }

            return message;
        }

        /// <summary>
        /// Gets a control name from the controls resource.
        /// </summary>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static string GetControlName(this string controlName)
        {
            return Captain.Common.Resources.CaptainResourceManager.GetControlString(controlName);
        }

        /// <summary>
        /// Gets a property value from an object.
        /// </summary>
        /// <param name="propertyContainer">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object propertyContainer, string propertyName)
        {
            return GetPropertyValue<T>(propertyContainer, propertyName, null);
        }

        /// <summary>
        /// Gets a property value from an object.
        /// </summary>
        /// <param name="propertyContainer">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object propertyContainer, string propertyName, object[] index)
        {
            T value = default(T);

            Type type = propertyContainer.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.CanRead)
            {
                if ((T)propertyInfo.GetValue(propertyContainer, index) != null)
                {
                    value = (T)propertyInfo.GetValue(propertyContainer, index);
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a property value from an object.
        /// </summary>
        /// <param name="propertyContainer">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object propertyContainer, string propertyName, object[] index)
        {
            object value = null;

            Type type = propertyContainer.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.CanRead)
            {
                if (propertyInfo.GetValue(propertyContainer, index) != null)
                {
                    value = propertyInfo.GetValue(propertyContainer, index).ToString();
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a property value from an object.
        /// </summary>
        /// <param name="propertyContainer">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="format">The format to get for the property</param>
        /// <returns></returns>
        public static string GetPropertyValueAsString(this object propertyContainer, string propertyName, string format)
        {
            string value = string.Empty;

            object propertyValue = propertyContainer.GetPropertyValue(propertyName, null);
            if (propertyValue != null)
            {
                if (propertyValue == typeof(DateTime))
                {
                    value = propertyValue.Format<DateTime>(format);
                }
                else if (propertyValue == typeof(long))
                {
                    value = propertyValue.Format<long>(format);
                }
                else if (propertyValue == typeof(int))
                {
                    value = propertyValue.Format<int>(format);
                }
                else if (propertyValue == typeof(decimal))
                {
                    value = propertyValue.Format<decimal>(format);
                }
                else if (propertyValue == typeof(double))
                {
                    value = propertyValue.Format<double>(format);
                }
                else if (propertyValue == typeof(float))
                {
                    value = propertyValue.Format<float>(format);
                }
                else
                {
                    value = propertyValue.ToString();
                }
            }

            return value;
        }

        /// <summary>
        /// Formats a DateTime value
        /// </summary>
        /// <param name="dateTime">The DateTime to be formatted.</param>
        /// <param name="format">The format to be used.</param>
        /// <returns></returns>
        public static string Format<T>(this object valueType, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                T value = (T)valueType;
                return string.Format(format, value);
            }
            else
            {
                string returnValue = string.Empty;
                returnValue = string.Format(valueType.ToString(), args);
                return returnValue;
            }
        }

        /// <summary>
        /// Gets a property value from an object.
        /// </summary>
        /// <param name="propertyContainer">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object propertyContainer, string propertyName, object value)
        {
            SetPropertyValue(propertyContainer, propertyName, value, null);
        }

        /// <summary>
        /// Gets a property value from an object.
        /// </summary>
        /// <param name="propertyContainer">The object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static void SetPropertyValue(this object propertyContainer, string propertyName, object value, object[] index)
        {
            Type type = propertyContainer.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                try
                {
                    propertyInfo.SetValue(propertyContainer, value, index);
                }
                catch { }
            }
        }

        /// <summary>
        /// Displays a friendly message.
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="parameters"></param>
        public static void DisplayNonResourcedFirendlyMessage(this string message, ExceptionSeverityLevel severityLevel, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                message = string.Format(message, parameters);
            }

            ExceptionLogger.DisplayMessageToUser(message, QuantumFaults.None, severityLevel);
        }

        /// <summary>
        /// Displays a friendly message.
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="parameters"></param>
        public static void DisplayFirendlyMessage(this string messageTemplate, ExceptionSeverityLevel severityLevel, params object[] parameters)
        {
            string message = messageTemplate.GetMessage();

            if (parameters != null && parameters.Length > 0)
            {
                message = string.Format(message, parameters);
            }

            ExceptionLogger.DisplayMessageToUser(message, QuantumFaults.None, severityLevel);
        }

        /// <summary>
        /// Converts a string into "Title Case" format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            else
            {
                return Regex.Replace(data.ToLower(), Consts.Common.TitleCaseRegEx, m => m.Groups[1].Value.ToUpper());
            }
        }

     }
}
