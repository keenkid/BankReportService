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
    [Monitor(Description = "HSBC MT940 Response")]
    public class HsbcMT940 : MT940, IProcess
    {
        public void Handle(object state)
        {
            ReportName = state as string;

            ParseReport(ReportName);
        }

        /// <summary>
        /// The formatting options available from HSBC are as follows:
        /// 999999999999
        /// 999-99999-999
        /// 999999999999 ACCOUNT NAME
        /// 999-999999-999 ACCOUNT NAME
        /// </summary>
        protected override void ParseAccountIdentification()
        {
            IEnumerable<string> accountIdentifications = GetTagContent(TAG_ACCOUNT_IDENTIFICATION);

            //one element in the container at most
            foreach (string acctNoRaw in accountIdentifications)
            {
                AccountIdentification = HSBCAccountIdentification(acctNoRaw);
            }
        }

        private string HSBCAccountIdentification(string acctNoRaw)
        {
            Regex reg = new Regex(@"^\d+-?\d+-?\d+");
            return reg.Match(acctNoRaw).Value.Replace("-", "");
        }

        protected override void AccountBalanceHeadInfo()
        {
            bankBalance.BankCode = "52";
            bankBalance.TransCode = "1001";
        }

        protected override void AccountStatementHeadInfo()
        {
            bankStatement.BankCode = "52";
            bankStatement.TransCode = "1003";
        }

        protected override void ParseTransactionTypeCode()
        {
            //find the first match string
            transactionTypeCode = new Regex(@"[SNF]{1}[\w]{3}").Match(statementLine).Value.Substring(1);
            bankStatement.Abstract = TransactionTypeMap.GetDescription(transactionTypeCode);
        }

        protected override void ParseSupplementaryDetails()
        {
            string detail = new Regex(@"\|.*\|:").Match(statementLine).Value;
            if (!string.IsNullOrEmpty(detail))
            {
                detail = detail.Substring(1);
                detail = detail.Substring(0, detail.Length - 2);
                if (transactionTypeCode == "TRF")
                {
                    bankStatement.CptyAcctName = detail;
                }
                else
                {
                    bankStatement.Remark = detail;
                }
            }
        }

        protected override void ParseAdditionalInformation()
        {
            string bankName = new Regex(@"/BBK/.*\|?").Match(statementLine).Value;
            if (!string.IsNullOrEmpty(bankName))
            {
                bankName = bankName.Substring(5);
                if (bankName.EndsWith("|"))
                {
                    bankName = bankName.Substring(0, bankName.Length - 1);
                }
                int vertical = bankName.IndexOf("|");
                if (-1 < vertical)
                {
                    bankName = bankName.Substring(0, vertical);
                }
                bankStatement.CptyBankName = bankName.Trim();
            }
            AddInformation();
            //put tag 86 information to remark
            string[] remarks = statementLine.Split(new string[] { TAG_ADDITIONAL_INFO }, StringSplitOptions.None);
            if (remarks.Length > 1)
            {
                bankStatement.Remark = remarks[1];
            }
        }

        private void AddInformation()
        {
            string cptyBankName = new Regex(@"/\|?BI/.*?/", RegexOptions.IgnoreCase).Match(statementLine).Value;
            if (!string.IsNullOrEmpty(cptyBankName))
            {
                cptyBankName = cptyBankName.Substring(4);
                cptyBankName = cptyBankName.Replace("|", string.Empty).Replace("/", string.Empty).Trim();
                bankStatement.CptyBankName = cptyBankName;
            }

            string cptyAcctNo = new Regex(@"/\|?BA/.*?/", RegexOptions.IgnoreCase).Match(statementLine).Value;
            if (!string.IsNullOrEmpty(cptyAcctNo))
            {
                cptyAcctNo = cptyAcctNo.Substring(4);
                cptyAcctNo = cptyAcctNo.Replace("|", string.Empty).Replace("/", string.Empty).Trim();
                bankStatement.CptyAcctNo = cptyAcctNo;
            }

            string cptyAcctName = new Regex(@"/\|?BN/.*?/", RegexOptions.IgnoreCase).Match(statementLine).Value;
            if (!string.IsNullOrEmpty(cptyAcctName))
            {
                cptyAcctName = cptyAcctName.Substring(4);
                cptyAcctName = cptyAcctName.Replace("|", string.Empty).Replace("/", string.Empty).Trim();
                bankStatement.CptyAcctName = cptyAcctName;
            }
        }
    }
}
