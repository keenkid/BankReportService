using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BankReport.Common;
using BankReportMonitor;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    [Monitor(Description = "SCB CSV Response")]
    public class ScbCsv : Csv, IProcess
    {
        private IEnumerable<string> listCsvHeader = null;

        private IEnumerable<string> listCsvDetail = null;

        private string AccountIdentifier = string.Empty;

        private string entryDate = string.Empty;
        
        public void Handle(object state)
        {
            ReportName = state as string;
            ParseReport(ReportName);
        }

        protected override void BuildCsvStringList()
        {
            string[] csvStringArray = ReportContent.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            listCsvHeader = from csvLine in csvStringArray where csvLine.StartsWith("\"H\"") select csvLine;

            listCsvDetail = from csvLine in csvStringArray where csvLine.StartsWith("\"I\"") select csvLine;
        }

        protected override void ValidationAssert()
        {
            if (!ReportContent.StartsWith("\"H\""))
            {
                throw new InvalidBankReportException(ReportName, BankReportType.CSV);
            }
            if (listCsvHeader.Count() == 0)
            {
                throw new InvalidBankReportException(ReportName, BankReportType.CSV);
            }
        }

        /// <summary>
        /// H1: Record Type
        /// H2: Company Name
        /// H3: Account Number
        /// H4: Currency Number
        /// H5: Account Type
        /// H6: Transaction Date
        /// H7: Opening Ledger Balance
        /// H8: Opening Availiable Balance
        /// H9: Interim Closing Ledger Balance
        /// H10: Interim Closing Availiable Balance
        /// H11: Statement Number
        /// </summary>
        protected override void BuildBankBalance()
        {
            listBankBalance = new List<BankBalance>();

            foreach (var header in listCsvHeader)
            {
                string[] headerArray = header.Split(new string[] { "," }, StringSplitOptions.None);
                if (headerArray.Length != 11)
                {
                    throw new InvalidBankReportException(ReportName, BankReportType.CSV);
                }
                bankBalance = new BankBalance();

                SetBankCode4Response(bankBalance);

                SetBankBalanceInfomation(headerArray);

                listBankBalance.Add(bankBalance);
            }

            ListBankBalance = listBankBalance;
        }
                
        protected override void BuildBankStatement()
        {
            listBankStatement = new List<BankStatement>();

            foreach (var detail in listCsvDetail)
            {
                string[] detailArray = detail.Split(new string[] { "," }, StringSplitOptions.None);

                //字段数不足12的，认为是一个无效的statement，不予处理
                if (detailArray.Length != 12)
                {
                    continue;
                }

                bankStatement = new BankStatement();

                SetBankCode4Response(bankStatement);

                SetBankStatementInformation(detailArray);

                listBankStatement.Add(bankStatement);
            }

            ListBankStatement = listBankStatement;
        }

        private void SetBankCode4Response(AbstractBankResponse response)
        {
            response.BankCode = "53";
            if (response is BankBalance)
            {
                response.TransCode = "1001";
            }
            else if (response is BankStatement)
            {
                response.TransCode = "1003";
            }
        }

        private void SetBankBalanceInfomation(string[] balanceInfoArr)
        {
            bankBalance.AcctNo = RemoveStringQuotes(balanceInfoArr[2].Trim());
            AccountIdentifier = bankBalance.AcctNo;
            bankBalance.CurrType = RemoveStringQuotes(balanceInfoArr[3].Trim());
            bankBalance.BalanceDate = DateFormat(RemoveStringQuotes(balanceInfoArr[5].Trim()));
            entryDate = bankBalance.BalanceDate;
            bankBalance.Balance = AmountFormat(RemoveStringQuotes(balanceInfoArr[8].Trim()));
            bankBalance.UsableBalance = AmountFormat(RemoveStringQuotes(balanceInfoArr[9].Trim()));
        }

        private void SetBankStatementInformation(string[] statementInfoArr)
        {
            bankStatement.BankRefID = RemoveStringQuotes(statementInfoArr[1].Trim());
            bankStatement.AcctNo = AccountIdentifier;
            bankStatement.EntryDate = entryDate;
            bankStatement.ValueDate = DateFormat(RemoveStringQuotes(statementInfoArr[2].Trim()));
            string dcFlag = RemoveStringQuotes(statementInfoArr[3].Trim());
            if (dcFlag == "D")
            {
                bankStatement.Amount = "-" + AmountFormat(RemoveStringQuotes(statementInfoArr[4]));
            }
            bankStatement.RefID = RemoveStringQuotes(statementInfoArr[5]);
            bankStatement.Remark = RemoveStringQuotes(statementInfoArr[9]);
            bankStatement.Abstract = RemoveStringQuotes(statementInfoArr[10]);
        }

        private string RemoveStringQuotes(string val)
        {
            Regex regex = new Regex("\"");

            return regex.Replace(val, m =>
            {
                return string.Empty;
            });
        }

        /// <summary>
        /// transaction date
        /// </summary>
        /// <param name="rawDateString">DD/MM/YYYY format only</param>
        /// <returns>YYYYMMDD format</returns>
        private string DateFormat(string rawDateString)
        {
            if (!rawDateString.Contains(@"/"))
            {
                return rawDateString;
            }
            string[] arr = rawDateString.Split(new string[] { "/" }, StringSplitOptions.None);
            if (arr.Length != 3)
            {
                return rawDateString;
            }
            return arr[2] + arr[1] + arr[0];
        }

        private string AmountFormat(string amount)
        {
            Regex regex = new Regex(@",");

            return regex.Replace(amount, m =>
                {
                    return ".";
                });
        }
    }
}
