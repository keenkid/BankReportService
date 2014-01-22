using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using System.Threading;

using BankReportMonitor;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    [Monitor(Description = "EDI Banking Status")]
    public class ReportProcess : IProcess
    {
        private string reportName = string.Empty;

        private string reportContent = string.Empty;

        private string bankName = string.Empty;

        private string reportType = string.Empty;

        private List<PaymentStatus> listStatusInfo = null;

        public void Handle(object state)
        {
            reportName = state as string;

            listStatusInfo = new List<PaymentStatus>();

            Logger.sysLog.InfoFormat("Start to parse report [{0}]", reportName);

            try
            {
                ValidateReport();

                Logger.sysLog.InfoFormat("This [{0}] report from [{1}]", reportType, bankName);

                AbstractBankingReport report = ReportFactory.CreateBankingReport(bankName, reportType);
                report.ReportName = reportName;
                report.ReportContent = reportContent.Trim();
                report.ParseReport();

                listStatusInfo.AddRange(report.ListPaymentStatus);

                LogPaymentStatusInfo(report.ListPaymentStatus);
            }
            catch (Exception ex)
            {
                Logger.sysLog.ErrorFormat("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            Logger.sysLog.InfoFormat("Parse report [{0}] end.", reportName);

            RecordEdiPaymentStatusInfo();
        }

        private void ValidateReport()
        {
            try
            {
                if (!File.Exists(reportName))
                {
                    throw new Exception(string.Format("The report [{0}] not exists.", reportName));
                }
                int i = 0;
                while (true && i < 10)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(reportName))
                        {
                            reportContent = reader.ReadToEnd().Trim().ToUpper();
                            break;
                        }
                    }
                    catch
                    {
                        //wait for copy
                        Thread.Sleep(500);
                        i++;
                    }
                }
                if (i == 10)
                {
                    Logger.sysLog.Error("This EDI report is too large to parse.");
                    throw new Exception("This EDI report is too large to parse.");
                }
                ValueMapUtil util = new ValueMapUtil();
                bankName = (from map in util.ListBankMap where new Regex(map.Identifier).IsMatch(reportContent) select map.Value).First();
                reportType = (from map in util.ListReportMap where new Regex(map.Identifier).IsMatch(reportContent) select map.Value).First();
            }
            catch
            {
                throw new Exception("The report is not a valide EDI report.");
            }
        }

        private void RecordEdiPaymentStatusInfo()
        {
            try
            {
                if (listStatusInfo == null || listStatusInfo.Count == 0)
                {
                    Logger.sysLog.InfoFormat("There is no banking status information exists in report [{0}]", reportName);
                    return;
                }

                int affectedLine = DbDataAdapterHelper.UpdatePaymentStatusTable(listStatusInfo);
                Logger.sysLog.InfoFormat("{0} row(s) affected.", affectedLine);
            }
            catch (Exception ex)
            {
                Logger.sysLog.ErrorFormat("Database operation encounters error:\r\n{0}", ex.Message);
            }
        }

        private void LogPaymentStatusInfo(List<PaymentStatus> status)
        {
            StringBuilder builder = new StringBuilder();

            status.ForEach(statuInfo => builder.Append(statuInfo));

            Logger.sysLog.InfoFormat("Fetch information from [{0}]:\r\n{1}", reportName, Regex.Replace(builder.ToString(), @";\[", ";\r\n["));
        }
    }
}