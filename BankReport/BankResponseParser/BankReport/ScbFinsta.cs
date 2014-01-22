using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;
using BankReportMonitor;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    [Monitor(Description = "SCB FINSTA Response")]
    public class ScbFinsta : Finsta, IProcess
    {
        public void Handle(object state)
        {
            ReportName = state as string;

            ParseReport(ReportName);
        }

        protected override void SetBankCode4Response()
        {
            bankBalance.BankCode = "53";
            bankBalance.TransCode = "1001";
        }

        protected override void SetAccountBalance4Response()
        {
            ParseAccountNo();
            ParseCurrency();
            ParseBalance();
            ParseBalanceDate();
        }

        private void ParseAccountNo()
        {
            string regexString = @"FII\+AS\+\w{1,35}:";
            bankBalance.AcctNo = finstaString.EDIFACTElementValue(regexString, 7);
        }

        private void ParseCurrency()
        {
            string regexString = @":[A-Z]{3}\+";
            bankBalance.CurrType = finstaString.EDIFACTElementValue(regexString, 1);
        }

        private void ParseBalance()
        {
            string regexString = @"MOA\+343:[0-9]{1,18}[,.]?[0-9]{0,2}'";
            bankBalance.Balance = finstaString.EDIFACTElementValue(regexString, 8);
        }

        /// <summary>
        /// intra-day balance
        /// </summary>
        private void ParseBalanceDate()
        {
            string regexString = @"MOA\+343:[0-9]{1,18}[,.]?[0-9]{0,2}'?DTM\+171:\d{8}:102";
            string balanceDateRaw = finstaString.EDIFACTElementValue(regexString, 8);
            regexString = @"DTM\+171:\d{8}:";
            bankBalance.BalanceDate = finstaString.EDIFACTElementValue(regexString, 8);
        }
    }
}
