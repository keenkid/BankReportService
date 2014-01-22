using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public abstract class AbstractBankingReport
    {
        public string ReportName { protected get; set; }

        public string ReportContent { protected get; set; }
        
        protected IEnumerable<string> loopSection = null;

        protected string singleSection = string.Empty;

        public List<PaymentStatus> ListPaymentStatus { get; protected set; }

        protected PaymentStatus statusInfo = null;

        protected string MessageID { get; set; }
        
        public void ParseReport()
        {
            PrintReport();

            Parsing();
        }

        private void PrintReport()
        {
            Logger.sysLog.Info(string.Format("This report content is:\r\n{0}", 
                new Regex(@"[^?]'\b").Replace(ReportContent, m => { return "'\r\n"; })));
        }

        protected abstract void Parsing();
    }
}
