using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BankResponseParser.Configuration
{
    public class BankBasicConfigElement : ConfigurationElement
    {
        private const string ELEMENT_BANKCODE = "code";
        private const string ELEMENT_TYPE = "type";
        private const string ELEMENT_BANKNAME = "name";
        private const string ELEMENT_BALANCE = "balance";
        private const string ELEMENT_STATEMENT = "statement";
        private const string ELEMENT_NEEDBACKUP = "needBackup";
        private const string ELEMENT_REPORT = "report";
        private const string ELEMENT_BACKUP = "backup";
        private const string ELEMENT_BALANCENAME = "balanceFileName";
        private const string ELEMENT_STATEMENTNAME = "statementFileName";

        [ConfigurationProperty(ELEMENT_BANKCODE, IsRequired = true, IsKey = true)]
        public string BankCode
        {
            get { return this[ELEMENT_BANKCODE] as string; }
        }

        [ConfigurationProperty(ELEMENT_TYPE, IsRequired = true, IsKey = true)]
        public string ReportType
        {
            get { return this[ELEMENT_TYPE] as string; }
        }

        [ConfigurationProperty(ELEMENT_BANKNAME, IsRequired = true, IsKey = false)]
        public string BankName
        {
            get { return this[ELEMENT_BANKNAME] as string; }
        }

        [ConfigurationProperty(ELEMENT_BALANCE, IsRequired = true, IsKey = false)]
        public string NeedBalance
        {
            get
            {
                return this[ELEMENT_BALANCE] as string;
            }
        }

        [ConfigurationProperty(ELEMENT_STATEMENT, IsRequired = true, IsKey = false)]
        public string NeedStatement
        {
            get
            {
                return this[ELEMENT_STATEMENT] as string;
            }
        }

        [ConfigurationProperty(ELEMENT_NEEDBACKUP, IsRequired = true, IsKey = false)]
        public string NeedBackup
        {
            get { return this[ELEMENT_NEEDBACKUP] as string; }
        }

        [ConfigurationProperty(ELEMENT_REPORT, IsRequired = true, IsKey = false)]
        public string ReportPath
        {
            get { return this[ELEMENT_REPORT] as string; }
        }

        [ConfigurationProperty(ELEMENT_BACKUP, IsRequired = true, IsKey = false)]
        public string BackupPath
        {
            get { return this[ELEMENT_BACKUP] as string; }
        }

        [ConfigurationProperty(ELEMENT_BALANCENAME, IsRequired = true, IsKey = false)]
        public string BalanceResponseName
        {
            get { return this[ELEMENT_BALANCENAME] as string; }
        }

        [ConfigurationProperty(ELEMENT_STATEMENTNAME, IsRequired = true, IsKey = false)]
        public string StatementResponseName
        {
            get { return this[ELEMENT_STATEMENTNAME] as string; }
        }
    }
}
