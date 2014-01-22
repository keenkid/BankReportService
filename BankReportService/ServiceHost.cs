using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReportMonitor;

namespace BankReportService
{
    public class ServiceHost : MarshalByRefObject, IServiceHost
    {
        List<AbstractMonitor> _monitorList = null;

        public ServiceHost()
        {
            InitializeMonitor();
        }

        private void InitializeMonitor()
        {
            _monitorList = new List<AbstractMonitor>();
            _monitorList.AddRange(from tp in typeof(AbstractMonitor).Assembly.GetTypes()
                                  where typeof(AbstractMonitor).IsAssignableFrom(tp) && !tp.IsAbstract
                                  select Activator.CreateInstance(tp) as AbstractMonitor);
        }

        public void OnStart(string[] args)
        {
            if (null != _monitorList && 0 < _monitorList.Count)
            {
                _monitorList.ForEach(monitor => monitor.LaunchMonitor());
            }
        }

        public void OnStop()
        {
            if (null != _monitorList && 0 < _monitorList.Count)
            {
                _monitorList.ForEach(monitor => monitor.DisposeMonitor());
            }
        }
    }
}
