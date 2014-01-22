using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    public abstract class AbstractBankReport
    {
        public string ReportName { get; protected set; }

        public string ReportContent { get; protected set; }

        public bool ShouldBeAnotherTry { get; protected set; }

        public List<BankBalance> ListBankBalance { get; protected set; }

        public List<BankStatement> ListBankStatement { get; protected set; }

        public abstract void ParseReport(string reportName);
    }
}
