using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReportService
{
    public interface IServiceHost
    {
        void OnStart(string[] args);

        void OnStop();
    }
}
