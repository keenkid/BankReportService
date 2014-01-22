using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class HsbcBansta : Bansta
    {
        private const string BANK_NAME = "HSBC";

        protected override void SetCustomReference()
        {
            string regexStr = @"RFF\+CR:[A-Z0-9]{1,35}[:|']";
            statusInfo.RefID = singleSection.EDIFACTElementValue(regexStr, 7);
        }

        protected override void SetBankReference()
        {
            string regexStr = @"RFF\+AEK:[A-Z0-9]{1,35}[:|']";
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
                string regexStr = @"GIS\+[A-Z0-9]{1,3}'";                
                statusInfo.BankStatusCode = singleSection.EDIFACTElementValue(regexStr, 4);
                BankingStatusXmlQuery.StatusCodeAndDescriptionQuery(statusInfo);
            }
            catch
            {
                throw;
            }
        }
    }
}
