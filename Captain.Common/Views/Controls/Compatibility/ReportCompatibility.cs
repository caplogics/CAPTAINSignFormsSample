using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
namespace Captain.Common.Views.Controls.Compatibility
{
    public class ReportCompatibility
    {
        #region Properties

        public string ReportPath
        {
            get
            {
                return _reportPath;
            }

            set
            {
                _reportPath = value;
            }
        }

        public string ReportFile
        {
            get
            {
                return _reportFile;
            }

            set
            {
                _reportFile = value;
            }
        }

        public string ReportDataFileXML
        {
            get
            {
                return _reportDataFileXML;
            }

            set
            {
                _reportDataFileXML = value;
            }
        }

        public string ReportPathFileXML
        {
            get
            {
                return _reportPathFileXML;
            }

            set
            {
                _reportPathFileXML = value;
            }
        }

        #endregion

        #region Methods

        public Stream ExportPDF(ReportDataSource reportDataSource)
        {
            const string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string mimeType;
            string[] streams;
            Warning[] warnings;


            _reportPath = Path.Combine(this._reportPath, this._reportFile);

            LocalReport report = new LocalReport();
            report.ReportPath = _reportPath;

            report.DataSources.Add(reportDataSource);

            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";

            //Render the report

            byte[] bytes =
             report.Render(reportType,
                            deviceInfo,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warnings);
            return new MemoryStream();
        }

        #endregion

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        #region Variables

        private IList<Stream> m_streams;
        private string _reportPath;
        private string _reportFile;
        private string _reportPathFileXML;
        private string _reportDataFileXML;

        #endregion
    }
}
