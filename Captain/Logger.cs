using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wisej.Web;

namespace Captain
{
    [Serializable()]
    public class Logger
    {

        public static void Log(string strMessage)
        {
            Logger.Log(String.Empty, strMessage);
        }

        public static void Log(string strMessageType, string strMessage)
        {
            Logger.Log(log4net.Core.Level.Info, ((Type)(null)), strMessageType, strMessage);
        }

        private static void Log(log4net.Core.Level level, string strMessageType, string strMessage)
        {
            Logger.Log(level, ((Type)(null)), strMessageType, strMessage);
        }

        public static void Log(Type T, string strMessageType, string strMessage)
        {
            Logger.Log(log4net.Core.Level.Info, T, strMessageType, strMessage);
        }

        public static void Log(Type T, string strMessageType, Exception ex)
        {
            string strMessage = ("Type = "
                        + (ex.GetType().FullName + Environment.NewLine));
            Exception nex = ex;
            while (nex != null)
            {
                strMessage = (strMessage
                            + (nex.Message
                            + (Environment.NewLine
                            + (nex.StackTrace + Environment.NewLine))));
                if (nex.Data != null)
                {
                    strMessage = (strMessage + ("Data: " + Environment.NewLine));
                    foreach (KeyValuePair<string, object> objData in nex.Data)
                    {
                        strMessage = (strMessage
                                    + (objData.Key + (": "
                                    + (objData.Value.ToString() + Environment.NewLine))));
                    }
                }

                strMessage = (strMessage + ("Source: "
                            + (nex.Source + Environment.NewLine)));
                strMessage = (strMessage + ("Target: "
                            + (nex.TargetSite.DeclaringType.FullName + (" / "
                            + (nex.TargetSite.MetadataToken.ToString() + Environment.NewLine)))));
                nex = nex.InnerException;
            }

            Logger.Log(log4net.Core.Level.Critical, T, strMessageType, strMessage);
        }

        public static void Log(Type T, Exception ex)
        {
            Logger.Log(T, "Exception", ex);
        }

        public static void Log(log4net.Core.Level level, Type T, string strMessageType, string strMessage)
        {
            System.Array a = LogManager.GetCurrentLoggers();
            if (((a == null)
                        || (a.Length == 0)))
            {
                log4net.Config.XmlConfigurator.Configure();
            }

            ILog logger;
            if ((T == null))
            {
                logger = LogManager.GetLogger("");
            }
            else
            {
                logger = LogManager.GetLogger(T);
            }

            MDC.Set("user", "");
            MDC.Set("urlreferrer", "");
            MDC.Set("user", "");
            MDC.Set("sourceip", "");
            MDC.Set("url", "");
            MDC.Set("remoterequestinfo", "");
            MDC.Set("remotehost", "");

            var url = Application.Url;
            var sourceip = Application.UserHostAddress;

            if (System.Web.HttpContext.Current != null &&  System.Web.HttpContext.Current.Request != null)
            {
                RemoteRequestInfo rri = new RemoteRequestInfo(HttpContext.Current);
                MDC.Set("remoterequestinfo", rri.Serialize());
                MDC.Set("user", Logger.getUserName(System.Web.HttpContext.Current.Request));
                if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                {
                    MDC.Set("urlreferrer", System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
                }

                MDC.Set("sourceip", System.Web.HttpContext.Current.Request.Url.Host);
                MDC.Set("url", System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                //  Detect reverse proxy information and retrieve client's real IP
                string strRemoteHost = Application.ServerVariables["REMOTE_HOST"];
                if (string.IsNullOrEmpty(strRemoteHost))
                {
                    strRemoteHost = String.Empty;
                }

                string strProxiedHost = System.Web.HttpContext.Current.Request.Headers["X-Forwarded-For"];
                string strReverseProxy = String.Empty;
                if (!string.IsNullOrEmpty(strProxiedHost))
                {
                    if (strProxiedHost.Contains(":"))
                    {
                        strProxiedHost = strProxiedHost.Split(':')[0];
                    }

                    strReverseProxy = strRemoteHost;
                    strRemoteHost = (strProxiedHost + (" ("
                                + (strReverseProxy + ")")));
                }

                MDC.Set("remotehost", strRemoteHost);
            }

            MDC.Set("messagetype", strMessageType);
            if (level == log4net.Core.Level.Debug)
                logger.Debug(strMessage);
            else if (level == log4net.Core.Level.Error)
                logger.Error(strMessage);
            else if (level == log4net.Core.Level.Fatal)
                logger.Fatal(strMessage);
            else if (level ==  log4net.Core.Level.Critical)
                logger.Fatal(strMessage);
            else if (level == log4net.Core.Level.Warn)
                logger.Warn(strMessage);
            else
                logger.Info(strMessage);
        }

        // '' <summary>
        // '' There can be various application dependent ways to obtain username, so adjust this to your needs.
        // '' </summary>
        // '' <param name="objRequest"></param>
        // '' <returns></returns>
        private static string getUserName(System.Web.HttpRequest objRequest)
        {
            try
            {
                System.Security.Principal.WindowsIdentity objUser = objRequest.LogonUserIdentity;
                if (((objUser == null)
                            || string.IsNullOrEmpty(objUser.Name)))
                {
                    return String.Empty;
                }

                return objUser.Name;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

        }
    }
}