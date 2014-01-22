using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BankReport.Common;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    public abstract class Finsta : AbstractBankReport
    {
        private const string SPLITOR = "LIN+";

        private const string REPORT_IDENTIFIER = "FINSTA";

        protected IEnumerable<string> listFinstaString = null;

        protected string finstaString = string.Empty;

        protected List<BankBalance> listBankBalance = null;

        protected BankBalance bankBalance = null;

        public override void ParseReport(string reportName)
        {
            ReportName = reportName;

            LoadFinstaContent();

            ValidationAssert();

            BuildFinstaStringList();

            BuildBankBalance();
        }

        private void LoadFinstaContent()
        {
            try
            {
                using (StreamReader sr = new StreamReader(ReportName))
                {
                    string reportLine = string.Empty;
                    StringBuilder builder = new StringBuilder();

                    while ((reportLine = sr.ReadLine()) != null)
                    {
                        builder.Append(reportLine.Trim());
                    }
                    ReportContent = builder.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Load {0} file {1} error:\n{2}", REPORT_IDENTIFIER, ReportName, ex.Message));
            }
        }

        private void ValidationAssert()
        {
            if (!ReportContent.Contains(REPORT_IDENTIFIER))
            {
                ShouldBeAnotherTry = false;
                //may be MT940
                if (!ReportContent.Contains(":20:") && !ReportContent.Contains(":25:")
                && (!ReportContent.Contains(":60M:") || !ReportContent.Contains(":60F:"))
                && (!ReportContent.Contains(":62M:") || !ReportContent.Contains(":62F:")))
                {
                    ShouldBeAnotherTry = true;
                }
                //maybe MT942
                if (!ReportContent.Contains(":20:") && !ReportContent.Contains(":25:")
                    && !ReportContent.Contains(":34F:") && !ReportContent.Contains(":13D:"))
                {
                    ShouldBeAnotherTry = true;
                }
                throw new InvalidBankReportException(ReportName, BankReportType.FINSTA);
            }
        }

        private void BuildFinstaStringList()
        {
            string[] finstaStringArray = ReportContent.SplitItemWithSplitor(SPLITOR);

            listFinstaString = from finsta in finstaStringArray where finsta.Contains(SPLITOR) select finsta;
        }

        private void BuildBankBalance()
        {
            listBankBalance = new List<BankBalance>();

            foreach (string str in listFinstaString)
            {
                finstaString = str;

                bankBalance = new BankBalance();

                SetBankCode4Response();

                SetAccountBalance4Response();

                listBankBalance.Add(bankBalance);
            }

            ListBankBalance = listBankBalance;
        }

        protected abstract void SetBankCode4Response();

        protected abstract void SetAccountBalance4Response();
    }
}
