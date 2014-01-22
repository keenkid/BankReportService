using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public abstract class Paymul : AbstractBankingReport
    {
        private const string TRANS_SPLIT = "LIN+";

        private Dictionary<int, int> SequenceAndIndex = null;

        protected override void Parsing()
        {
            FetchEdiReference();

            FetchLoopSection();

            InitialSequenceAndIndex();

            DoParse();
        }

        protected virtual void FetchEdiReference()
        {
            string regexStr = @"BGM\+452\+[A-Z0-9]+\+";
            MessageID = ReportContent.EDIFACTElementValue(regexStr, 8);
        }

        protected virtual void FetchLoopSection()
        {
            string[] sectionArray = ReportContent.SplitItemWithSplitor(TRANS_SPLIT, StringSplitOptions.RemoveEmptyEntries);
            loopSection = from paymul in sectionArray where paymul.Contains(TRANS_SPLIT) select paymul;
        }

        private void InitialSequenceAndIndex()
        {
            SequenceAndIndex = new Dictionary<int, int>();
            string[] segmentArray = new Regex(@"[^\?]'").Split(ReportContent);

            int idx = 0;
            for (int i = 0; i < segmentArray.Length; i++)
            {
                if (segmentArray[i].Contains(TRANS_SPLIT))
                {
                    SequenceAndIndex.Add(idx, i);
                    idx++;
                }
            }
        }

        private void DoParse()
        {
            ListPaymentStatus = new List<PaymentStatus>();
            int idx = 0;
            foreach (var section in loopSection)
            {
                singleSection = section;
                statusInfo = new PaymentStatus();

                statusInfo.MessageID = MessageID;
                SetCustomReference();
                statusInfo.BankRef = string.Empty;
                SetBankName();
                SetLineSpan(idx);                
                statusInfo.Source = BankReportType.PAYMUL;
                statusInfo.OurStatusCode = "P";
                statusInfo.BankStatusCode = "P";
                statusInfo.BankStatusDesc = "Payment is sending to bank gateway";

                ListPaymentStatus.Add(statusInfo);
                idx++;
            }
        }

        protected abstract void SetCustomReference();

        private void SetLineSpan(int index)
        {
            int segmentBeginIndex = SequenceAndIndex[index];
            int segmentEndIndex = 999999;
            if (index + 1 < SequenceAndIndex.Count)
            {
                segmentEndIndex = SequenceAndIndex[index + 1] - 1;
            }
            statusInfo.LineSpan = string.Format("{0}-{1}", segmentBeginIndex, segmentEndIndex);
        }

        protected abstract void SetBankName();
    }
}
