using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class HsbcContrl : Contrl
    {
        private const string BANK_NAME = "HSBC";

        protected override void FetchActionCode()
        {
            //string regexStr = @"APACS\+\d['|\+]";
            //actionCode = ReportContent.EDIFACTElementValue(regexStr, 6);
            actionCode = string.Empty;
        }

        protected override void SetBankName()
        {
            statusInfo.BankCode = BANK_NAME;
        }

        protected override void FetchLoopSection()
        {
            string splitor = "UCM+";
            string[] sectionArray = ReportContent.SplitItemWithSplitor(splitor, StringSplitOptions.RemoveEmptyEntries);
            loopSection = from contrl in sectionArray where contrl.Contains(splitor) select contrl;
        }

        protected override void SetCustomReference()
        {
            string regexStr = @"UCM\+[A-Z0-9]+\+";
            statusInfo.RefID = singleSection.EDIFACTElementValue(regexStr, 4);
        }

        protected override void SetLineSpan()
        {
            statusInfo.LineSpan = string.Empty;
        }

        protected override void SetErrorCodeAndDescription()
        {
            try
            {
                string regexStr = @"APACS\+[4|7]\+[A-Z0-9]+'";                
                statusInfo.BankStatusCode = singleSection.EDIFACTElementValue(regexStr, 8);
                BankingStatusXmlQuery.StatusCodeAndDescriptionQuery(statusInfo);
            }
            catch
            {
                throw;
            }
        }
    }
}
