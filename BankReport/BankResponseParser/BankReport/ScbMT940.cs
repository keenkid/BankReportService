using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using BankResponseParser.Configuration;
using BankReport.Common;
using BankReportMonitor;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    [Monitor(Description = "SCB MT940 Response")]
    public class ScbMT940 : MT940, IProcess
    {
        public void Handle(object state)
        {
            ReportName = state as string;
            ParseReport(ReportName);
        }

        protected override void AccountBalanceHeadInfo()
        {
            bankBalance.BankCode = "53";
            bankBalance.TransCode = "1001";
        }

        protected override void AccountStatementHeadInfo()
        {
            bankStatement.BankCode = "53";
            bankStatement.TransCode = "1003";
        }

        /// <summary>
        /// parse transaction type code, BAI code for SCB MT940
        /// </summary>
        protected override void ParseTransactionTypeCode()
        {
            Regex regex = new Regex(@"[,.]{1}\d{0,2}N\d{3}");
            string typeCode = regex.Match(statementLine).Value;
            if (typeCode != string.Empty)
            {
                transactionTypeCode = typeCode.Substring(typeCode.IndexOf("N") + 1);
                bankStatement.Abstract = TransactionTypeMap.GetDescription(transactionTypeCode);
            }
        }

        /// <summary>
        /// SCB put custom reference number here
        /// <b>re-write BankStatement.RefID property</b>
        /// </summary>
        protected override void ParseSupplementaryDetails()
        {
            string[] refIDs = statementLine.Split(new string[] { SPLITOR }, StringSplitOptions.RemoveEmptyEntries);
            if (refIDs.Length > 1)
            {
                string refID = refIDs[1];
                if (!refID.Contains(TAG_ADDITIONAL_INFO))
                {
                    refID = refID.Replace("-", string.Empty).Replace(SPACE, string.Empty);
                    bankStatement.RefID = refID;
                }
            }
        }

        /// <summary>
        /// tag :86: for additional information
        /// store in Remark property
        /// </summary>
        protected override void ParseAdditionalInformation()
        {
            string[] remarks = statementLine.Split(new string[] { TAG_ADDITIONAL_INFO }, StringSplitOptions.None);
            if (remarks.Length > 1)
            {
                bankStatement.Remark = remarks[1];
            }
        }
    }
}
