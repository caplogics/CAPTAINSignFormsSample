using DevExpress.XtraReports;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;

namespace Captain.Common.Views.Controls.Compatibility
{
    //[Serializable]
    internal class LocalReportHelper : MarshalByRefObject, ISponsor, IDisposable
    {
        private LocalReport _report;
        ILease _lease;

		public override object InitializeLifetimeService()
		{
			_lease = (ILease)base.InitializeLifetimeService();
			_lease.Register(this);
			return _lease;
		}

		public TimeSpan Renewal(ILease lease)
		{
			Debug.WriteLine("RemotingSponsor.Renewal called");
			return this._lease != null ? lease.InitialLeaseTime : TimeSpan.Zero;
		}


		public void Dispose()
		{
			if (_lease != null)
			{
				_lease.Unregister(this);
				_lease = null;
			}

            if (_report != null)
            {
                _report.Dispose();
                _report = null;
            }
        }

        /// <summary>
        /// Creates a new instance of the report helper.
        /// </summary>
        /// <param name="reportPath">The report path</param>
        /// <returns></returns>
        public static LocalReportHelper Create(string reportPath)
        {
			var instance = (LocalReportHelper)RSAppDomain.CreateInstanceAndUnwrap(typeof(LocalReportHelper).Assembly.FullName, typeof(LocalReportHelper).FullName);
			instance.LoadReport(reportPath);

			return instance;
		}

        /// <summary>
        /// Creates a new report instance with a summary table.
        /// </summary>
        /// <param name="reportPath">The report path</param>
        /// <param name="summary">The summary report DataTable</param>
        public static LocalReportHelper CreateWithSummaryReport(string reportPath, DataTable summary)
        {
            var instance = (LocalReportHelper)RSAppDomain.CreateInstanceAndUnwrap(typeof(LocalReportHelper).Assembly.FullName, typeof(LocalReportHelper).FullName);
            instance.LoadReportWithSummaryTable(reportPath, summary);

            return instance;
        }


        public event SubreportProcessingEventHandler SubreportProcessing
        {
            add { _report.SubreportProcessing += value; }
            remove { _report.SubreportProcessing -= value; }
        }

        public string GetReportPath()
        {
            return _report.ReportPath;
        }

        private void LoadReport(string reportPath)
        {
            if (_report == null)
                _report = new LocalReport();

            _report.ReportPath = reportPath;
        }

        // Loads the report, and attaches to the subreport processing event.
        private void LoadReportWithSummaryTable(string reportPath, DataTable table)
        {
            if (_report == null)
                _report = new LocalReport();

            Summary_table = table;
            _report.SubreportProcessing += ReportOnSubreportProcessing;
            _report.ReportPath = reportPath;
        }


        int ireport = 0;


        // Same login found in CASB2012_AdhocRDLCForm.cs file, moved here to make it simpler to manage across AppDomains.
        private void ReportOnSubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (Summary_table != null)
            {
                ReportDataSource datasource = new ReportDataSource("ZipCodeDataset", Summary_table);
                ireport = ireport + 1;
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource("ZipCodeDataset", Summary_table)); // objDataSource.Tables["GetCASESNPadpyn"]));
            }
        }

        public void AddDataSource(string name, object data)
        {
            _report.DataSources.Add(new ReportDataSource(name, data));
        }

		public byte[] Render(string format)
		{
			return _report.Render(format);
		}

		private static AppDomain RSAppDomain
		{
			get
			{
				if (_rsAppDomain == null)
				{
					lock (typeof(LocalReportHelper))
					{
						if (_rsAppDomain == null)
						{
							var currentSetup = AppDomain.CurrentDomain.SetupInformation;
							var info = new AppDomainSetup()
							{
								ApplicationBase = currentSetup.ApplicationBase + "bin\\",
								LoaderOptimization = currentSetup.LoaderOptimization,
							};
							info.SetCompatibilitySwitches(new[] { "NetFx40_LegacySecurityPolicy" });
							_rsAppDomain = AppDomain.CreateDomain("RSDomain", null, info);
						}
					}
				}
				return _rsAppDomain;
			}
		}

        public DataTable Summary_table { get; private set; }

        private static AppDomain _rsAppDomain;
    }
}
