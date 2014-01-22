using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using BankReportMonitor;
using System.IO;

namespace BankReportService
{
    public partial class BankReportService : ServiceBase
    {
        private ServiceHostManager _serviceHost = null;

        private static EventLog _logger = null;

        public BankReportService()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            _serviceHost = new ServiceHostManager();

            _logger = this.EventLog;
        }

        protected override void OnStart(string[] args)
        {
            if (null != _logger)
            {
                _logger.WriteEntry("Bank report monitor service start");
            }
            if (null != _serviceHost)
            {
                _serviceHost.OnStart(args);
            }
        }

        protected override void OnStop()
        {
            if (null != _serviceHost)
            {
                _serviceHost.OnStop();
            }
            if (null != _logger)
            {
                _logger.WriteEntry("Bank report monitor service stop");
                _logger.Dispose();
            }
        }
    }
}
