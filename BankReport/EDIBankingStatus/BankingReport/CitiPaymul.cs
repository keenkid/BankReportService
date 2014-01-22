using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{  
    public sealed class CitiPaymul : Paymul
    {
        private const string BANK_NAME = "CITI";

        protected override void FetchEdiReference()
        {
            string regexStr = @"BGM\+452\+[A-Z0-9]+'";
            MessageID = ReportContent.EDIFACTElementValue(regexStr, 8);
        }

        protected override void SetBankName()
        {
            statusInfo.BankCode = BANK_NAME;
        }

        protected override void SetCustomReference()
        {
            //RFF+PQ:SR249181'
            string regexStr = @"RFF\+PQ:[A-Z0-9]+'";
            statusInfo.RefID = singleSection.EDIFACTElementValue(regexStr, 7);
        }
    }
}
