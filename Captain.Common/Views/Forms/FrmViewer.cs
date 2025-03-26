//#define VWG           // UNCOMMENT this line if compiling for VisualWebGUI

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.Text;
#if VWG
using SystemWeb = System.Web.UI;
using System.Web;
using Wisej.Web;
#else
using Wisej.Web;
#endif

namespace Captain.Common.Views.Forms
{
#if VWG
    public partial class FrmViewer : Form //, IGatewayComponent
#else
    public partial class FrmViewer : Form
#endif
    {
        /// <summary>
        /// Provides the gateway for viewing files in a Web form
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// 
        /// </para>
        /// </remarks>
        /// 

#if VWG
        // Get the MIME type from a file
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

        private HtmlBox htmlBox1;
#endif

        private string _reportName;

        public FrmViewer()
        {
            try
            {


                InitializeComponent();
#if VWG
                this.Closing += new CancelEventHandler(frmIsClosing);
#else
            this.FormClosing += new FormClosingEventHandler(frmIsClosing);
#endif
            }
            catch (Exception ex)
            {

                AlertBox.Show(ex.Message,MessageBoxIcon.Error);
            }
        }

        public FrmViewer(string ReportName)
        {
            try
            {


                InitializeComponent();
                _reportName = ReportName;
#if VWG
                this.Closing += new CancelEventHandler(frmIsClosing);
#else
            this.FormClosing += new FormClosingEventHandler(frmIsClosing);
#endif
            }
            catch (Exception ex)
            {

                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }
        }

        private void frmViewer_Load(object sender, EventArgs e)
        {

            try
            {
                // Create HtmlBox for file viewing in browser
                //this.htmlBox1 = new Wisej.Web.HtmlPanel();
                //this.htmlBox1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom)
                //| Wisej.Web.AnchorStyles.Left)
                //| Wisej.Web.AnchorStyles.Right)));
                //this.htmlBox1.ContentType = "text/html";
                //this.htmlBox1.Expires = -1;
                //this.htmlBox1.Html = "";
                //this.htmlBox1.Location = new System.Drawing.Point(12, 4);
                //this.htmlBox1.Name = "htmlBox1";
                //this.htmlBox1.Path = "";
                //this.htmlBox1.Resource = null;
                //this.htmlBox1.Size = new System.Drawing.Size(750, 460);
                //this.htmlBox1.TabIndex = 1;
                //this.htmlBox1.Url = "";
                //// Add it to form
                //this.Controls.Add(this.htmlBox1);

                //// Show the report

                //htmlBox1.Resource = new GatewayResourceHandle(new GatewayReference(this, "FileContent"));
                //htmlBox1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

#if VWG
            
            try
            {
                // Create HtmlBox for file viewing in browser
                //this.htmlBox1 = new Wisej.Web.HtmlPanel();
                //this.htmlBox1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom)
                //| Wisej.Web.AnchorStyles.Left)
                //| Wisej.Web.AnchorStyles.Right)));
                //this.htmlBox1.ContentType = "text/html";
                //this.htmlBox1.Expires = -1;
                //this.htmlBox1.Html = "";
                //this.htmlBox1.Location = new System.Drawing.Point(12, 4);
                //this.htmlBox1.Name = "htmlBox1";
                //this.htmlBox1.Path = "";
                //this.htmlBox1.Resource = null;
                //this.htmlBox1.Size = new System.Drawing.Size(750, 460);
                //this.htmlBox1.TabIndex = 1;
                //this.htmlBox1.Url = "";
                //// Add it to form
                //this.Controls.Add(this.htmlBox1);

                //// Show the report

                //htmlBox1.Resource = new GatewayResourceHandle(new GatewayReference(this, "FileContent"));
                //htmlBox1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#endif
        }
        private void FrmViewer_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmIsClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        #region "Web Reporting Gateway"
#if VWG
        //void IGatewayComponent.ProcessRequest(IContext objContext, string strAction)
        //{
        //    try
        //    {
        //        // Try to get the gateway handler
        //        IGatewayHandler objGatewayHandler = ProcessGatewayRequest(objContext.HttpContext, strAction);

        //        if (objGatewayHandler != null)
        //        {
        //            objGatewayHandler.ProcessGatewayRequest(objContext, this);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
           
        //}

        //protected override IGatewayHandler ProcessGatewayRequest(HttpContext objHttpContext, string strAction)
        //{
        //    IGatewayHandler objGH = null;

        //    if (!String.IsNullOrEmpty(strAction) && strAction.Equals("FileContent"))
        //    {
        //        if (objHttpContext != null && objHttpContext.Response != null)
        //        {
        //            // Disable cache.
        //            objHttpContext.Response.Expires = -1;

        //            string strFileName = _reportName;
        //            string strContentType = getMimeFromFile(_reportName);

        //            if (strContentType == "unknown/unknown")
        //            {
        //                MessageBox.Show("Unknown MIME type for file " + _reportName);
        //                return null;
        //            }

        //            objHttpContext.Response.ContentType = strContentType;


        //            // Set Headers for the Pdf attachment
        //            objHttpContext.Response.AddHeader("Content-Disposition", string.Format("inline; filename={0};", Path.GetFileName(strFileName)));

        //            // objHttpContext.Response.AddHeader("Content-Disposition", string.Format("inline; filename={0};", Path.GetFileName(strFileName + "#toolbar=0&navpanes=0")));

        //            if (File.Exists(strFileName))
        //            {
        //                // Write Pdf body.
        //                objHttpContext.Response.WriteFile(strFileName);
        //            }
        //            else
        //            {
        //                MessageBox.Show("File does not exist: " + strFileName);
        //                // Create an html writer.
        //                SystemWeb.HtmlTextWriter objHtmlWriter = new SystemWeb.HtmlTextWriter(objHttpContext.Response.Output);

        //                if (objHtmlWriter != null)
        //                {
        //                    // Write empty body.
        //                    objHtmlWriter.Write("<p> File was not found or contained an invalid mime type - see application log </p> ");
        //                    // Flush html writer.
        //                    objHtmlWriter.Flush();
        //                }
        //            }
        //        }
        //    }
        //    return objGH;
        //}

        public static string getMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                //throw new FileNotFoundException(filename + " not found");
                MessageBox.Show("File not found " + filename);
                return "unknown/unknown";
            }

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                return "unknown/unknown";
            }
        }

        private void FrmViewer_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
#endif
        #endregion
    }
}
