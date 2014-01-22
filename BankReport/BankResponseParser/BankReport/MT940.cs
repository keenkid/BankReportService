using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BankResponseParser.Configuration;
using BankReport.Common;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    public abstract class MT940 : AbstractBankReport
    {
        #region Data members

        protected const string MT940_END_FLAG = "-";

        protected const string MT940_TAG_START_FLAG = ":";

        protected const string SPLITOR = "|";

        protected const string SPACE = " ";

        protected const string TAG_TRANSACTION_REF = ":20:";

        protected const string TAG_ACCOUNT_IDENTIFICATION = ":25:";

        protected const string TAG_FIRST_OPENING_BALANCE = ":60F:";
        protected const string TAG_INTER_OPENING_BALANCE = ":60M:";

        protected const string TAG_STATEMENT_LINE = ":61:";

        protected const string TAG_ADDITIONAL_INFO = ":86:";

        protected const string TAG_FIRST_CLOSING_BALANCE = ":62F:";
        protected const string TAG_INTER_CLOSING_BALANCE = ":62M:";

        protected const string TAG_AVAILABLE_BALANCE = ":64:";

        /// <summary>
        /// 一个string是一个完整的MT940
        /// </summary>
        protected IEnumerable<string> listMt940 = null;
        /// <summary>
        /// 一个string是MT940中的一个segment
        /// </summary>
        protected List<string> listSegment = null;
        /// <summary>
        /// 
        /// </summary>
        protected List<List<string>> listMt940ListSegment = null;

        protected IEnumerable<string> listStatementLine = null;
        protected string statementLine = string.Empty;
        protected string transactionTypeCode = string.Empty;

        protected BankBalance bankBalance = null;
        protected List<BankBalance> listBankBalance = null;

        protected BankStatement bankStatement = null;
        protected List<BankStatement> listBankStatement = null;

        protected string AccountIdentification { get; set; }
        protected string AccountCurrency { get; set; }

        #endregion

        /// <summary>
        /// parse MT940 report
        /// </summary>
        /// <param name="reportName">report file name</param>
        public override void ParseReport(string reportName)
        {
            try
            {
                ReportName = reportName;

                LoadMt940Content();

                ValidationAssert();

                BuildSingleMt940List();

                RefineSingleMt940List();

                ParseMT940();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadMt940Content()
        {
            try
            {
                //MT940 based on line break, do not use ReadLine() method here
                using (StreamReader sr = new StreamReader(ReportName))
                {
                    ReportContent = sr.ReadToEnd().Trim().ToUpper();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Load this MT940 {0} content error:{1}\n", ReportName, ex.Message));
            }
        }

        /// <summary>
        /// validation
        /// </summary>
        private void ValidationAssert()
        {
            if (!ReportContent.Contains(TAG_TRANSACTION_REF)
                && !ReportContent.Contains(TAG_ACCOUNT_IDENTIFICATION)
                && (!ReportContent.Contains(TAG_FIRST_OPENING_BALANCE) || !ReportContent.Contains(TAG_INTER_OPENING_BALANCE))
                && (!ReportContent.Contains(TAG_FIRST_CLOSING_BALANCE) || !ReportContent.Contains(TAG_INTER_CLOSING_BALANCE)))
            {
                ShouldBeAnotherTry = false;
                //maybe FINSTA
                if (ReportContent.Contains("FINSTA"))
                {
                    ShouldBeAnotherTry = true;
                }
                //maybe MT942
                if (!ReportContent.Contains(TAG_TRANSACTION_REF)
                    && !ReportContent.Contains(TAG_ACCOUNT_IDENTIFICATION)
                    && !ReportContent.Contains(":34F:") && !ReportContent.Contains(":13D:"))
                {
                    ShouldBeAnotherTry = true;
                }
                throw new InvalidBankReportException(ReportName, BankReportType.MT940);
            }
        }

        private void BuildSingleMt940List()
        {
            string[] singleMt940Array = ReportContent.SplitItemWithSplitor(TAG_TRANSACTION_REF);

            listMt940 = from str in singleMt940Array where str.Contains(TAG_TRANSACTION_REF) select str;
        }

        private void RefineSingleMt940List()
        {
            listMt940ListSegment = new List<List<string>>();

            foreach (string singleMt940 in listMt940)
            {
                string[] mt940SegmentArray = singleMt940.Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);

                bool MT940Start = false;
                listSegment = new List<string>();

                foreach (string segment in mt940SegmentArray)
                {
                    if (!segment.StartsWith(TAG_TRANSACTION_REF) && !MT940Start)
                    {
                        continue;
                    }
                    else if (!MT940Start)
                    {
                        MT940Start = true;
                    }
                    BuildListSegment(segment.Trim());

                    //useless infomation after tag :64:
                    if (segment.StartsWith(TAG_AVAILABLE_BALANCE) || segment.StartsWith(MT940_END_FLAG))
                    {
                        break;
                    }
                }
                listMt940ListSegment.Add(listSegment);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        private void BuildListSegment(string segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                return;
            }

            //将tag :61:的第二行sub field及可能出现的tag :86:均放在tag :61:行
            if (!segment.StartsWith(MT940_TAG_START_FLAG) || segment.StartsWith(TAG_ADDITIONAL_INFO))
            {
                int idx = listSegment.Count - 1;
                string formerContent = listSegment[idx];

                StringBuilder builder = new StringBuilder();
                builder.Append(formerContent);
                builder.Append(SPLITOR);
                builder.Append(segment);

                listSegment.RemoveAt(idx);
                listSegment.Add(builder.ToString());
            }
            else
            {
                listSegment.Add(segment);
            }
        }

        /// <summary>
        /// parse mt940 report
        /// </summary>
        public void ParseMT940()
        {
            listBankBalance = new List<BankBalance>();
            listBankStatement = new List<BankStatement>();

            foreach (List<string> singleMt940 in listMt940ListSegment)
            {
                listSegment = singleMt940;

                try
                {
                    ParseAccountIdentification();

                    ParseAccountBalance();

                    ParseAccountStatements();
                }
                catch
                {
                    throw;
                }
            }
            ListBankBalance = listBankBalance;
            ListBankStatement = listBankStatement;
        }

        /// <summary>
        /// tag :25: account Identification
        /// this information will be used by BankBalance and BankStatement
        /// </summary>
        protected virtual void ParseAccountIdentification()
        {
            IEnumerable<string> accountIdentification = GetTagContent(TAG_ACCOUNT_IDENTIFICATION);
            string acctNo = string.Empty;
            //one element in the container at most
            foreach (string identification in accountIdentification)
            {
                acctNo = identification;
            }
            AccountIdentification = acctNo;
        }

        #region Balance

        private void ParseAccountBalance()
        {
            bankBalance = new BankBalance();

            bankBalance.AcctNo = AccountIdentification;

            AccountBalanceHeadInfo();

            AccountClosingBalance();

            AccountAvaliableBalance();

            listBankBalance.Add(bankBalance);
        }

        protected abstract void AccountBalanceHeadInfo();

        /// <summary>
        /// tag :62F: closing balance
        /// </summary>
        private void AccountClosingBalance()
        {
            IEnumerable<string> closingBalances = GetTagContent(TAG_FIRST_CLOSING_BALANCE);
            //one element in the container at most
            foreach (string closingBal in closingBalances)
            {
                string cdFlag = closingBal.Substring(0, 1);

                string balanceDate = closingBal.Substring(1, 6);
                bankBalance.BalanceDate = DateTime.Today.Year.ToString().Substring(0, 2) + balanceDate;

                AccountCurrency = closingBal.Substring(7, 3);
                bankBalance.CurrType = AccountCurrency;

                string amount = closingBal.Substring(10);
                if (cdFlag == "D")
                {
                    amount = "-" + amount;
                }
                bankBalance.Balance = GetAmountValue(amount);
            }
        }

        /// <summary>
        /// tag :64: avaliable balace
        /// </summary>
        private void AccountAvaliableBalance()
        {
            IEnumerable<string> avaliableBalances = GetTagContent(TAG_FIRST_CLOSING_BALANCE);
            //one element in the container at most
            foreach (string avaliableBal in avaliableBalances)
            {
                string cdFlag = avaliableBal.Substring(0, 1);
                string amount = avaliableBal.Substring(10);
                if (cdFlag == "D")
                {
                    amount = "-" + amount;
                }
                bankBalance.UsableBalance = GetAmountValue(amount);
            }
        }

        #endregion

        #region Statement

        private void ParseAccountStatements()
        {
            GetStatementLine();

            foreach (string str in listStatementLine)
            {
                statementLine = str;
                bankStatement = new BankStatement();

                bankStatement.AcctNo = AccountIdentification;
                bankStatement.CurrType = AccountCurrency;

                /***************************************
                 * following part method sequence is very important
                 * do not change it if no necessary.
                 * ***************************************/
                //head information
                AccountStatementHeadInfo();
                //value date and entry date
                ParseValueDateAndEntryDate();
                //transaction amount
                ParseAmount();
                //remark and abstract
                ParseTransactionTypeCode();
                //transaction reference
                ParseReference();
                //supplementary details
                ParseSupplementaryDetails();
                //tag :86: part
                ParseAdditionalInformation();

                listBankStatement.Add(bankStatement);
            }
        }

        private void GetStatementLine()
        {
            listStatementLine = GetTagContent(TAG_STATEMENT_LINE);
        }

        protected abstract void AccountStatementHeadInfo();

        /// <summary>
        /// fetch value date and amount information
        /// </summary>
        /// <returns></returns>
        private void ParseValueDateAndEntryDate()
        {
            string valueDate = statementLine.Substring(0, 6);
            string entryDate = statementLine.Substring(6, 4);

            if (valueDate.Substring(2) == entryDate)
            {
                valueDate = "20" + valueDate;
                entryDate = valueDate.Substring(0, 4) + entryDate;
            }
            else
            {
                string valueDate_M = valueDate.Substring(2, 2);
                string entryDate_M = entryDate.Substring(0, 2);
                //跨年
                if (valueDate_M == "12" && entryDate_M == "01")
                {
                    valueDate = "20" + valueDate;
                    entryDate = (Convert.ToInt32(valueDate.Substring(0, 4)) + 1).ToString() + entryDate;
                }
                else
                {
                    valueDate = "20" + valueDate;
                    entryDate = valueDate.Substring(0, 4) + entryDate;
                }
            }

            //hope MT940 format never change
            bankStatement.ValueDate = valueDate;
            bankStatement.EntryDate = entryDate;
        }

        /// <summary>
        /// fetch transaction amount
        /// </summary>
        protected virtual void ParseAmount()
        {
            string cdFlag = statementLine.Substring(10, 1);
            //amount max length
            string amount = statementLine.Substring(12, 15);
            //reverse
            if (cdFlag.StartsWith("R"))
            {
                cdFlag = statementLine.Substring(10, 2);
                amount = statementLine.Substring(13, 15);
            }
            if (cdFlag == "D" || cdFlag == "RC")
            {
                amount = "-" + amount;
            }

            bankStatement.Amount = GetAmountValue(amount);
        }

        /// <summary>
        /// refID and bankRefID,generally in tag :61:
        /// </summary>
        protected virtual void ParseReference()
        {
            string[] rawArray = new Regex(@"\d*[,.]{1}\d{0,2}[SNF]{1}[\w]{3}").Split(statementLine);
            if (rawArray.Length < 2)
            {
                return;
            }

            string referenceRaw = rawArray[1];
            string[] referenceArray = referenceRaw.Split(new string[] { SPLITOR }, StringSplitOptions.None);
            string referencePart = referenceArray[0];

            string[] ourRefAndBankRef = referencePart.Split(new string[] { "//" }, StringSplitOptions.None);

            bankStatement.RefID = ourRefAndBankRef[0].Trim();
            if (bankStatement.RefID.Contains(SPACE))
            {
                string[] temps = bankStatement.RefID.Split(new string[] { SPACE }, StringSplitOptions.RemoveEmptyEntries);
                if (temps.Length > 0)
                {
                    bankStatement.RefID = temps[0];
                }
            }
            if (bankStatement.RefID.Length <= 6)
            {
                bankStatement.RefID = DateTime.Now.ToString("ffffffmmssHHddMMyyyy") + transactionTypeCode;
            }

            if (ourRefAndBankRef.Length == 2 && !string.IsNullOrEmpty(ourRefAndBankRef[1].Trim()))
            {
                bankStatement.BankRefID = ourRefAndBankRef[1].Trim() + transactionTypeCode;
            }
            else
            {
                bankStatement.BankRefID = DateTime.Now.ToString("ffffffmmssHHddMMyyyy") + transactionTypeCode;
            }

            if (-1 < bankStatement.BankRefID.IndexOf("NONREF", StringComparison.InvariantCultureIgnoreCase))
            {
                bankStatement.BankRefID = DateTime.Now.ToString("ffffffmmssHHddMMyyyy") + transactionTypeCode;
            }
        }

        protected abstract void ParseTransactionTypeCode();

        protected abstract void ParseSupplementaryDetails();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void ParseAdditionalInformation();

        #endregion

        /// <summary>
        /// get tag content
        /// </summary>
        /// <param name="tagID">tag ID pre-indicate</param>
        /// <returns>IEnumerable&lt;string&gt;</returns>
        protected IEnumerable<string> GetTagContent(string tagID)
        {
            if (string.IsNullOrEmpty(tagID))
            {
                yield break;
            }

            IEnumerable<string> contents = from content in listSegment where content.Contains(tagID) select content;

            foreach (string str in contents)
            {
                //将tag部分删除
                yield return str.Substring(tagID.Length);
            }
        }

        /// <summary>
        /// get amount string from a amount start string
        /// </summary>
        /// <param name="amountRaw">amount string with alphabet character</param>
        /// <returns>pure amount value</returns>
        protected string GetAmountValue(string amountRaw)
        {
            try
            {
                //replace possible comma
                amountRaw = amountRaw.Replace(",", ".");
                Regex reg = new Regex(@"^-?\d*\.?\d{0,2}");
                return string.Format("{0:F2}", Convert.ToDecimal(reg.Match(amountRaw).Value));
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
