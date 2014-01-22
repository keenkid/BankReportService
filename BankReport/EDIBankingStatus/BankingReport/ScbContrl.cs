using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class ScbContrl : Contrl
    {
        private const string BANK_NAME = "SCB";

        protected override void FetchActionCode()
        {
            string regexStr = @"FUN01G\+\d['|\+]";
            actionCode = ReportContent.EDIFACTElementValue(regexStr, 7);
            //some syntax error happen
            if (string.IsNullOrEmpty(actionCode))
            {
                actionCode = "4";
            }
        }

        protected override void FetchLoopSection()
        {
            base.FetchLoopSection();

            //construct error message for syntax error.
            if (null == loopSection || 0 == loopSection.Count())
            {
                string regexStr = @"\+4\+\d+\+UNH'";
                string errorCode = ReportContent.EDIFACTElementValue(regexStr, 3, 5);
                loopSection = new List<string> { string.Format("UCD+{0}'", errorCode) };
            }
        }

        protected override void SetBankName()
        {
            statusInfo.BankCode = BANK_NAME;
        }

        protected override void SetErrorCodeAndDescription()
        {
            try
            {
                string regexStr = @"UCD\+\d+[\+|']";
                statusInfo.BankStatusCode = singleSection.EDIFACTElementValue(regexStr, 4);
                BankingStatusXmlQuery.StatusCodeAndDescriptionQuery(statusInfo);
                if (!string.IsNullOrEmpty(statusInfo.LineSpan))
                {
                    statusInfo.BankStatusDesc = string.Format("Line {0}:{1}", statusInfo.LineSpan, statusInfo.BankStatusDesc);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
