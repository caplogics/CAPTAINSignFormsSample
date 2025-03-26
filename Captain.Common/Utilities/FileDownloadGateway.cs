using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;

namespace Captain.Common.Utilities
{
    //[MetadataTag("CompanionKit.Controls.DataGridView.Features.FileDownloadGateway")] //TODO Wisej.... Find equivelant in Wisej
    public class FileDownloadGateway : Wisej.Web.Control
    {
        private IContainer components;
        private string ContentType;
        public bool DownloadAsAttachment = true;
        public string Filename;
        public LinkParameters linkParameters = new LinkParameters();
        private ContainerControl myContainer;
        private string myFilePath;
        private Stream myStream;
        private string _myString;

        public FileDownloadGateway()
        {
            // Default Link Parameters
            this.linkParameters.Target = "_self";
        }

        private string getContentType(DownloadContentType myContentType)
        {
            switch (myContentType)
            {
                case DownloadContentType.OctetStream:
                    return "application/octet-stream";
                case DownloadContentType.MicrosoftExcel:
                    return "application/x-msexcel";
                case DownloadContentType.MicrosoftWord:
                    return "application/x-msword";
                case DownloadContentType.PlainText:
                    return "text/plain";
            }
            return null;
        }

        /// <summary>
        /// This is the core Function that writes to the HTTP Response Stream
        /// </summary>
        //protected override IGatewayHandler ProcessGatewayRequest(HttpContext objContext, string strAction)
        //{
        //    // Get HTTP Response Object
        //    HttpResponse objResponse = objContext.Response;

        //    // Set the Content-Type
        //    if (this.ContentType != null)
        //    {
        //        objResponse.ContentType = this.ContentType;
        //    }

        //    // Set Filename Header
        //    if (this.DownloadAsAttachment & (this.Filename != null))
        //    {
        //        // add the response headers
        //        objResponse.AddHeader("content-disposition", "attachment; filename=\"" + this.Filename + "\"");
        //    }


        //    // Send File out to HTTP Stream
        //    if (this.myFilePath != null)
        //    {
        //        // Write to Stream using using FileStream
        //        objResponse.WriteFile(this.myFilePath);
        //    }
        //    else if (this._myString != null)
        //    {
        //        // Write to Stream using string
        //        objResponse.Write(this._myString);
        //    }
        //    else
        //    {
        //        // Write to Stream using Byte Array
        //        byte[] myByteArray = this.GetStreamAsByteArray(this.myStream);
        //        objResponse.BinaryWrite(myByteArray);
        //    }

        //    // Close the HTTP Response Stream
        //    objResponse.End();

        //    // Remove Myself from Container
        //    this.myContainer.Controls.Remove(this);
        //    return null;
        //}

        private byte[] GetStreamAsByteArray(Stream stream)
        {
            int streamLength = Convert.ToInt32(stream.Length);
            byte[] fileData = new byte[streamLength + 1];
            stream.Read(fileData, 0, streamLength);
            stream.Close();
            return fileData;
        }

        #region UserControl
        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        #endregion

        public void SetContentType(DownloadContentType argContentType)
        {
            this.ContentType = this.getContentType(argContentType);
        }

        public void SetContentType(string argContentType)
        {
            this.ContentType = argContentType;
        }

        private void StartDownload(ContainerControl argContainer)
        {
            // Set Parent Container
            this.myContainer = argContainer;

            // Add Myself to Container (to get current HTTP Context)
            this.myContainer.Controls.Add(this);

            // Open Link to ourself as a Gateway (using empty Action string) -> Calls Me.GetGatewayHandler()
           // Link.Open(new GatewayReference(this, "empty"), this.linkParameters);

           
        }

        public void StartFileDownload(ContainerControl argContainerFormOrUserControl, string argFilePath)
        {
            this.myStream = null;
            this.myFilePath = argFilePath;
            this._myString = null;
            this.StartDownload(argContainerFormOrUserControl);
        }

        public void StartStreamDownload(ContainerControl argContainerFormOrUserControl, Stream argStream)
        {
            this.myStream = argStream;
            this.myFilePath = null;
            this._myString = null;
            this.StartDownload(argContainerFormOrUserControl);
        }

        public void StartStringDownload(ContainerControl argContainerFormOrUserControl, string argString)
        {
            this.myStream = null;
            this.myFilePath = null;
            this._myString = argString;
            this.StartDownload(argContainerFormOrUserControl);
        }
    }

    public enum DownloadContentType
    {
        OctetStream,
        MicrosoftExcel,
        MicrosoftWord,
        PlainText
    }
}
