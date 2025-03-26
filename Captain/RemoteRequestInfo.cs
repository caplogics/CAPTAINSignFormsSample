using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Captain
{
    [Serializable()]
    public class RemoteRequestInfo
    {

        private string _referrer_url;
        private string _http_user_agent;
        private string _remote_addr;
        private string _http_x_forwarded_for;
        private string _remote_host;
        private string _request_method;
        private string _server_name;
        private string _server_port;
        private string _server_software;


        public RemoteRequestInfo()
        {
            _http_user_agent = "";
            _remote_addr = "";
            _http_x_forwarded_for = "";
            _remote_host = "";
            _request_method = "";
            _server_name = "";
            _server_port = "";
            _server_software = "";
            _referrer_url = "";
        }

        public RemoteRequestInfo(System.Web.HttpContext objContext)
        {
            this.http_x_forwarded_for = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            this.http_user_agent = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["http_user_agent"]);
            this.remote_addr = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["remote_addr"]);
            this.remote_host = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["remote_host"]);
            this.request_method = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["request_method"]);
            this.server_name = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["server_name"]);
            this.server_port = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["server_port"]);
            this.server_software = RemoteRequestInfo.sDefault(objContext.Request.ServerVariables["server_software"]);
            if (objContext.Request.UrlReferrer != null)
            {
                this.referrer_url = RemoteRequestInfo.sDefault(objContext.Request.UrlReferrer.AbsoluteUri);
            }

        }

        public static string pDefault(NameValueCollection nvp, string strProperty)
        {
            if (! String.IsNullOrEmpty(strProperty))
            {
                object obj = nvp[strProperty.ToLower()];
                if ((obj == null))
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }

            }
            else
            {
                return "";
            }

        }

        public static string sDefault(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            else
            {
                return s;
            }

        }

        public string Serialize()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine((string.Format("http_user_agent         : {0}", _http_user_agent) + Environment.NewLine));
            SB.AppendLine(string.Format("remote_addr             : {0}", _remote_addr));
            SB.AppendLine(string.Format("http_x_forwarded_for    : {0}", _http_x_forwarded_for));
            SB.AppendLine(string.Format("remote_host             : {0}", _remote_host));
            SB.AppendLine(string.Format("request_method          : {0}", _request_method));
            SB.AppendLine(string.Format("server_name             : {0}", _server_name));
            SB.AppendLine(string.Format("server_port             : {0}", _server_port));
            SB.AppendLine(string.Format("server_software         : {0}", _server_software));
            SB.AppendLine(string.Format("referrer_url            : {0}", _referrer_url));
            return SB.ToString();
        }

        public string referrer_url
        {
            get
            {
                return _referrer_url;
            }
            set
            {
                _referrer_url = value;
            }
        }

        public string http_user_agent
        {
            get
            {
                return _http_user_agent;
            }
            set
            {
                _http_user_agent = value;
            }
        }

        public string remote_addr
        {
            get
            {
                return _remote_addr;
            }
            set
            {
                _remote_addr = value;
            }
        }

        public string http_x_forwarded_for
        {
            get
            {
                return _http_x_forwarded_for;
            }
            set
            {
                _http_x_forwarded_for = value;
            }
        }

        public string remote_host
        {
            get
            {
                return _remote_host;
            }
            set
            {
                _remote_host = value;
            }
        }

        public string request_method
        {
            get
            {
                return _request_method;
            }
            set
            {
                _request_method = value;
            }
        }

        public string server_name
        {
            get
            {
                return _server_name;
            }
            set
            {
                _server_name = value;
            }
        }

        public string server_port
        {
            get
            {
                return _server_port;
            }
            set
            {
                _server_port = value;
            }
        }

        public string server_software
        {
            get
            {
                return _server_software;
            }
            set
            {
                _server_software = value;
            }
        }
    }
}