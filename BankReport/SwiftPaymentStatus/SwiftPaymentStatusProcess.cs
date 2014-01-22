using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using BankReportMonitor;
using BankReport.Common;

namespace SwiftPaymentStatus
{
    [Monitor(Description = "SWIFT pain.002 parser")]
    public class SwiftPaymentStatusProcess : IProcess
    {
        public void Handle(object state)
        {
            string fileName = state as string;

            DBHelper helper = new DBHelper();

            SwiftPain002 pain002 = new SwiftPain002(fileName);

            if (pain002.IsValidSwiftPain002)
            {
                helper.ListPaymentStauts = pain002.ListPaymentStatus;
                helper.ExecuteUpdate();
            }
        }
    }
}
