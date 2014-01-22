using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BankReportMonitor
{
    public interface IFswWrapper
    {
        void LuanchWatcher();

        void DisposeWatcher();
    }
}
