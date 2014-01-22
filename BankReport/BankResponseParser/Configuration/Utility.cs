using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankResponseParser.Configuration;
using Common;

namespace BankResponseParser.Configuration
{
    public class BankResponseConfig
    {
        public string BankCode { get; set; }

        public string BankReportType { get; set; }

        public string BankName { get; set; }

        public bool NeedBalance { get; set; }

        public bool NeedStatement { get; set; }

        public bool NeedBackup { get; set; }

        public string ReportPath { get; set; }

        public string BackupPath { get; set; }

        public string BalanceResponseName { get; set; }

        public string StatementResponseName { get; set; }
    }

    public class Utility
    {
        public List<BankResponseConfig> ListBankReport { get; private set; }

        public int WaitInterval { get; private set; }

        public Utility()
        {
            ListBankReport = new List<BankResponseConfig>();
            WaitInterval = 0;
            Initialize();
        }

        public void Initialize()
        {
            var section = BankResponseParserSection.Instance;

            if (section != null && section.Report.BECollection != null)
            {
                foreach (BankBasicConfigElement element in section.Report.BECollection)
                {
                    ListBankReport.Add(new BankResponseConfig()
                    {
                        BankCode = element.BankCode,
                        BankReportType = element.ReportType,
                        BankName = element.BankName,
                        NeedBalance = element.NeedBalance == "1" ? true : false,
                        NeedStatement = element.NeedStatement == "1" ? true : false,
                        NeedBackup = element.NeedBackup == "1" ? true : false,
                        ReportPath = element.ReportPath,
                        BackupPath = element.BackupPath,
                        BalanceResponseName = element.BalanceResponseName,
                        StatementResponseName = element.StatementResponseName
                    });
                }
            }

            //if (section != null && section.Repeat != null)
            //{
            //    WaitInterval = section.Repeat.Interval;
            //}
        }

        private BankReportEnum GetReportType(string reportType)
        {
            switch (reportType)
            {
                case "MT940":
                    return BankReportEnum.MT940;
                case "MT942":
                    return BankReportEnum.MT942;
                case "FINSTA":
                    return BankReportEnum.FINSTA;
                default:
                    return BankReportEnum.Other;
            }
        }
    }
}
