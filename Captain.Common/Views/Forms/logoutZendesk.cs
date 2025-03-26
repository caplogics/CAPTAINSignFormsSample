using Captain.Common.Utilities;
using System;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class logoutZendesk : Form
    {
        public logoutZendesk(string _type)
        {
            InitializeComponent();
            string zendeskUrl = "";
            if (_type == "LOGOUT")
                zendeskUrl = "https://capsystems.zendesk.com/access/logout?return_to=https%3A%2F%2Fcapsystems.zendesk.com%2Fhc%2Fen-us";
            //else if (_type == "LOGIN")
            //{
            //    zendeskUrl = CommonFunctions.ZendeskLogin();
            //}

            // Handle the DocumentCompleted event to close the form after the page loads
            webBrowser1.DocumentCompleted += WebBrowser_DocumentCompleted;
            // Load the URL
            webBrowser1.Navigate(zendeskUrl);
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Close the form after the page is loaded
            this.Close();
        }

    }
}
