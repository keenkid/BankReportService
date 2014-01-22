using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SwiftPaymentStatus.Configuration
{
    public class StatusElement : ConfigurationElement
    {
        private const string PROPERTY_LEVEL = "level";
        private const string PROPERTY_CODE = "code";
        private const string PROPERTY_RESULT = "result";
        private const string PROPERTY_BANK = "bank";
        private const string PROPERTY_DESC = "desc";

        [ConfigurationProperty(PROPERTY_LEVEL, IsKey = false, IsRequired = true)]
        public string StatusLevel
        {
            get
            {
                return this[PROPERTY_LEVEL] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_CODE, IsKey = false, IsRequired = true)]
        public string StatusCode
        {
            get
            {
                return this[PROPERTY_CODE] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_RESULT, IsKey = false, IsRequired = true)]
        public string StatusResult
        {
            get
            {
                return this[PROPERTY_RESULT] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_BANK, IsKey = false, IsRequired = false)]
        public string StatusBank
        {
            get
            {
                return this[PROPERTY_BANK] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_DESC, IsKey = false, IsRequired = true)]
        public string StatusDesc
        {
            get
            {
                return this[PROPERTY_DESC] as string;
            }
        }
    }
}
