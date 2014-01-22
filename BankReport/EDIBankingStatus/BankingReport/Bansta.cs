using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public abstract class Bansta : AbstractBankingReport
    {
        private const string SPLITOR = "LIN+";

        protected override void Parsing()
        {
            FetchEdiReference();

            FetchLoopSection();

            DoParse();
        }
                
        protected virtual void FetchEdiReference()
        {
            MessageID = string.Empty;
        }

        protected virtual void FetchLoopSection()
        {
            string[] sectionArray = ReportContent.SplitItemWithSplitor(SPLITOR,StringSplitOptions.RemoveEmptyEntries);
            loopSection = from bansta in sectionArray where bansta.Contains(SPLITOR) select bansta;
        }

        private void DoParse()
        {
            ListPaymentStatus = new List<PaymentStatus>();

            foreach (string section in loopSection)
            {
                try
                {
                    singleSection = section;
                    statusInfo = new PaymentStatus();

                    statusInfo.MessageID = MessageID;
                    SetCustomReference();
                    SetBankReference();
                    SetBankName();
                    statusInfo.Source = BankReportType.BANSTA;
                    SetStatusInformation();

                    ListPaymentStatus.Add(statusInfo);
                }
                catch(Exception ex)
                {
                    Logger.sysLog.ErrorFormat("{0} Custom Reference:{1}", ex.Message, statusInfo.RefID);
                }
            }
        }

        protected abstract void SetCustomReference();

        protected abstract void SetBankReference();

        protected abstract void SetBankName();

        protected abstract void SetStatusInformation();
    }
}
