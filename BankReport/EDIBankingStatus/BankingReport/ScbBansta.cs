using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class ScbBansta : Bansta
    {
        private const string BANK_NAME = "SCB";

        protected override void SetCustomReference()
        {
            string regexStr = @"RFF\+CR:[A-Z0-9-]{1,35}[:|']";
            statusInfo.RefID = singleSection.EDIFACTElementValue(regexStr, 7);
        }

        protected override void SetBankReference()
        {
            string regexStr = @"RFF\+ACK:[A-Z0-9]{1,35}[:|']";
            statusInfo.BankRef = singleSection.EDIFACTElementValue(regexStr, 8);
        }

        protected override void SetBankName()
        {
            statusInfo.BankCode = BANK_NAME;
        }

        protected override void SetStatusInformation()
        {
            try
            {
                string regexStr = @"SEQ\+[A-Z0-9]{1,6}[\+|']";
                statusInfo.BankStatusCode = singleSection.EDIFACTElementValue(regexStr, 4);
                regexStr = @"FTX\+ACD\+{3}.*?'";
                string additionalDesc = singleSection.EDIFACTElementValue(regexStr, 10);
                BankingStatusXmlQuery.StatusCodeAndDescriptionQuery(statusInfo);
                statusInfo.BankStatusDesc = string.Format("{0} {1}", statusInfo.BankStatusDesc, additionalDesc.Replace(":", string.Empty));
            }
            catch
            {
                throw;
            }
        }
    }
}
