using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace BankReport.Common
{
    public static class Logger
    {
        public static readonly ILog sysLog = LogManager.GetLogger("BankReportMonitor");
    }
}
