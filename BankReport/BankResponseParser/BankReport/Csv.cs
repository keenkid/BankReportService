using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    public abstract class Csv : AbstractBankReport
    {
        private const string REPORT_IDENTIFIER = "CSV";

        protected IEnumerable<string> listCsvString = null;

        protected BankBalance bankBalance = null;
        protected List<BankBalance> listBankBalance = null;        

        protected BankStatement bankStatement = null;
        protected List<BankStatement> listBankStatement = null;

        /// <summary>
        /// parse csv report
        /// </summary>
        /// <param name="reportName"></param>
        public override void ParseReport(string reportName)
        {
            ReportName = reportName;

            LoadCsvContent();

            BuildCsvStringList();

            ValidationAssert();

            BuildBankBalance();

            BuildBankStatement();
        }

        private void LoadCsvContent()
        {
            try
            {
                using (StreamReader sr = new StreamReader(ReportName))
                {
                    ReportContent = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Load {0} file {1} error:\n{2}", REPORT_IDENTIFIER, ReportName, ex.Message));
            }
        }

        protected virtual void BuildCsvStringList()
        {
            string[] csvStringArray = ReportContent.Split(new string[] { "\r","\n","\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            listCsvString = from csvLine in csvStringArray where !string.IsNullOrEmpty(csvLine) select csvLine;
        }

        protected abstract void ValidationAssert();
        
        protected abstract void BuildBankBalance();

        protected abstract void BuildBankStatement();
    }
}
