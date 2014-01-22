using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class CitiBansta : Bansta
    {
        private const string BANK_NAME = "CITI";

        protected override void SetCustomReference()
        {
            string regexStr = @"RFF\+AIJ:[A-Z0-9]{1,35}'";
            statusInfo.RefID = singleSection.EDIFACTElementValue(regexStr, 8);
        }

        protected override void SetBankReference()
        {
            string regexStr = @"RFF\+AIK:[A-Z0-9]{1,35}'";
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
                string regexStr = @"GIS\+\w{1,3}:";
                statusInfo.BankStatusCode = singleSection.EDIFACTElementValue(regexStr, 4);
                regexStr = @"FTX\+ACB\+{3}.*?'";
                string reasonDesc = singleSection.EDIFACTElementValue(regexStr, 10);
                BankingStatusXmlQuery.StatusCodeAndDescriptionQuery(statusInfo);
                statusInfo.BankStatusDesc = string.Format("{0} {1}", statusInfo.BankStatusDesc, reasonDesc.Replace(":", string.Empty));
            }
            catch
            {
                throw;
            }
        }
    }
}
