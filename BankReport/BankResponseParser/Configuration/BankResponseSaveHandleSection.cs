using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BankReport.Common;

namespace BankResponseParser.Configuration
{
    public class BankResponseSaveHandleSection : ConfigurationSection
    {
        private const string SECTION_NAME = "BankResponseSaveHandleSection";
        private const string ELEMENT_SAVE = "save";

        private static BankResponseSaveHandleSection _instance = null;

        private BankResponseSaveHandleSection() { }

        public static BankResponseSaveHandleSection Instance
        {
            get
            {
                if (null == _instance)
                {
                    var config = typeof(BankResponseSaveHandleSection).GetAssemblyConfiguration();
                    _instance = config.GetSection(SECTION_NAME) as BankResponseSaveHandleSection;
                }
                return _instance;
            }
        }

        [ConfigurationProperty(ELEMENT_SAVE, IsRequired = true)]
        public SaveBankResponseParameter SaveParameter
        {
            get
            {
                return this[ELEMENT_SAVE] as SaveBankResponseParameter;
            }
            set
            {
                this[ELEMENT_SAVE] = value;
            }
        }
    }

    public class SaveBankResponseParameter : ConfigurationElement
    {
        private const string PROPERTY_BALANCE_NAME_PREFIX = "balance";

        private const string PROPERTY_STATEMENT_NAME_PREFIX = "statement";

        private const string PROPERTY_NAME_PATTERN = "namePattern";

        private const string PROPERTY_SAVEPATH = "savePath";

        [ConfigurationProperty(PROPERTY_BALANCE_NAME_PREFIX, IsRequired = true)]
        public string BalanceNamePrefix
        {
            set
            {
                this[PROPERTY_BALANCE_NAME_PREFIX] = value;
            }
            get
            {
                return this[PROPERTY_BALANCE_NAME_PREFIX] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_STATEMENT_NAME_PREFIX, IsRequired = true)]
        public string StatementNamePrefix
        {
            set
            {
                this[PROPERTY_STATEMENT_NAME_PREFIX] = value;
            }
            get
            {
                return this[PROPERTY_STATEMENT_NAME_PREFIX] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_NAME_PATTERN, IsRequired = true)]
        public string NamePattern
        {
            get
            {
                return this[PROPERTY_NAME_PATTERN] as string;
            }
            set
            {
                this[PROPERTY_NAME_PATTERN] = value;
            }
        }

        [ConfigurationProperty(PROPERTY_SAVEPATH, IsRequired = true)]
        public string SavePath
        {
            get
            {
                return this[PROPERTY_SAVEPATH] as string;
            }
            set
            {
                this[PROPERTY_SAVEPATH] = value;
            }
        }
    }
}
