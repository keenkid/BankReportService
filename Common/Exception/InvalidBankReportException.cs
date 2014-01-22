using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReport.Common
{
    public class InvalidBankReportException : ApplicationException
    {
        private string _filename = string.Empty;
        BankReportType _reportType;

        public InvalidBankReportException(string filename, BankReportType reportType)
        {
            this._filename = filename;
            this._reportType = reportType;
        }

        public override string Message
        {
            get
            {
                return string.Format("This report {0} is not a valid {1}", _filename, _reportType.ToString());
            }
        }
    }
}
