using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public abstract class Contrl : AbstractBankingReport
    {
        private const string SPLITOR = "UCS+";

        protected string actionCode = string.Empty;

        protected override void Parsing()
        {
            FetchEdiReference();

            FetchActionCode();

            DoParse();
        }

        protected virtual void FetchEdiReference()
        {
            string regexStr = @"UCI\+[A-Z0-9]+\+";
            MessageID = ReportContent.EDIFACTElementValue(regexStr, 4);
        }

        protected abstract void FetchActionCode();

        private void DoParse()
        {
            ListPaymentStatus = new List<PaymentStatus>();
            if (actionCode == "7" || actionCode == "8")
            {
                PositiveActionCodeSetting();
            }
            else
            {
                FetchLoopSection();
                NegativeActionCodeSetting();
            }
        }

        protected virtual void FetchLoopSection()
        {
            string[] sectionArray = ReportContent.SplitItemWithSplitor(SPLITOR, StringSplitOptions.RemoveEmptyEntries);
            loopSection = from contrl in sectionArray where contrl.Contains(SPLITOR) select contrl;
        }

        private void PositiveActionCodeSetting()
        {
            statusInfo = new PaymentStatus();

            statusInfo.MessageID = MessageID;

            SetCustomReference();
            SetBankReference();
            SetBankName();
            
            statusInfo.Source = BankReportType.CONTRL;
            statusInfo.BankStatusCode = actionCode;
            BankingStatusXmlQuery.ActionCodeDescritionQuery(statusInfo);

            ListPaymentStatus.Add(statusInfo);
        }

        private void NegativeActionCodeSetting()
        {
            foreach (var section in loopSection)
            {
                try
                {
                    singleSection = section;
                    statusInfo = new PaymentStatus();

                    statusInfo.MessageID = MessageID;
                    SetCustomReference();
                    SetBankReference();
                    SetBankName();
                    SetLineSpan();
                    statusInfo.Source = BankReportType.CONTRL;
                    SetErrorCodeAndDescription();

                    ListPaymentStatus.Add(statusInfo);
                }
                catch
                {

                }
            }
        }

        protected abstract void SetBankName();

        protected virtual void SetCustomReference()
        {
            statusInfo.RefID = string.Empty;
        }

        protected virtual void SetBankReference()
        {
            statusInfo.BankRef = string.Empty;
        }

        protected virtual void SetLineSpan()
        {
            string regexStr = @"UCS\+\d+[\+|']";
            statusInfo.LineSpan = singleSection.EDIFACTElementValue(regexStr, 4);
        }

        protected abstract void SetErrorCodeAndDescription();
    }
}
