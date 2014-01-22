using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace EdiBankingStatus
{
    public sealed class ScbPaymul : Paymul
    {
        private const string BANK_NAME = "SCB";

        protected override void SetBankName()
        {
            statusInfo.BankCode = BANK_NAME;
        }

        protected override void SetCustomReference()
        {
            //RFF+AEK:2462402'
            string regexStr = @"RFF\+AEK:[A-Z0-9]+'";
            statusInfo.RefID = singleSection.EDIFACTElementValue(regexStr, 8);
        }
    }
}
