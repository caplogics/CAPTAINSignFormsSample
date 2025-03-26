#region Using

using System.Globalization;
using System.Reflection;
using Resources = System.Resources;
using System;
using Captain.Common.Utilities;
using System.Collections.Generic; 

#endregion

namespace Captain.Common.Resources
{
    public enum ResourceTypes
    {
        Control,
        Message
    }

    public static class CaptainResourceManager
    {
        private static System.Resources.ResourceManager _resourceManagerControls = null;
        private static System.Resources.ResourceManager _resourceManagerMessages = null;

        public static CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture(Consts.Common.DefaultLanguage);

        public static CultureInfo Culture;

        public static string GetMessageString(string messageName)
        {
            return GetString(messageName, ResourceTypes.Message);
        }

        public static string GetControlString(string stringName)
        {
            string returnString = stringName;

            returnString = GetString(stringName, ResourceTypes.Control);

            return returnString;
        }

        public static string GetString(string name, ResourceTypes resourceType)
        {
            string returnValue = string.Empty;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            List<string> resources = new List<string>(thisAssembly.GetManifestResourceNames());

            switch (resourceType)
            {
                case ResourceTypes.Control:
                    bool isCultureControlResourceExist = resources.Exists(r => r.Equals(Consts.Common.ControlResource + Culture.Name, StringComparison.CurrentCultureIgnoreCase));

                    if (_resourceManagerControls == null && isCultureControlResourceExist)
                        _resourceManagerControls = new System.Resources.ResourceManager(Consts.Common.ControlResource + Culture.Name, Assembly.GetExecutingAssembly());
                    else
                        _resourceManagerControls = new System.Resources.ResourceManager(Consts.Common.ControlResource + DefaultCulture.Name, Assembly.GetExecutingAssembly());

                    returnValue = _resourceManagerControls.GetString(name, Culture);

                    _resourceManagerControls = null;


                    break;
                case ResourceTypes.Message:
                    bool isCultureMessageResourceExist = resources.Exists(r => r.Equals(Consts.Common.MessagesResource + Culture.Name, StringComparison.CurrentCultureIgnoreCase));

                    if (_resourceManagerMessages == null && isCultureMessageResourceExist)
                        _resourceManagerMessages = new System.Resources.ResourceManager(Consts.Common.MessagesResource + Culture.Name, Assembly.GetExecutingAssembly());
                    else
                        _resourceManagerMessages = new System.Resources.ResourceManager(Consts.Common.MessagesResource + DefaultCulture.Name, Assembly.GetExecutingAssembly());

                    returnValue = _resourceManagerMessages.GetString(name, Culture);

                    _resourceManagerMessages = null;


                    break;
            }

            //if (string.IsNullOrEmpty(returnValue)) { returnValue = Consts.Messages.ResourceBlankNotFound.GetMessage(name); }

            return returnValue;
        }
    }
}
