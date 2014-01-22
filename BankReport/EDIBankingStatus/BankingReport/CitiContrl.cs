using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class CitiContrl : Contrl
    {
        private const string BANK_NAME = "CITI";

        protected override void FetchActionCode()
        {
            string regexStr = @"UN\+\d['|\+]";
            actionCode = ReportContent.EDIFACTElementValue(regexStr, 3);
            if (string.IsNullOrEmpty(actionCode))
            {
                actionCode = "7";
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
                statusInfo.BankStatusDesc = string.Format("Line {0}:{1}", statusInfo.LineSpan, statusInfo.BankStatusDesc);                
            }
            catch
            {
                throw;
            }
        }
    }
}
