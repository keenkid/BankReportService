using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReportMonitor
{    
    public interface IProcess
    {
        void Handle(object state);
    }
}
