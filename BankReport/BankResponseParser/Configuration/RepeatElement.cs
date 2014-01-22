using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BankResponseParser.Configuration
{
    public class RepeatElement : ConfigurationElement
    {
        private const string PROPERTY_NAME_INTERVAL = "interval";

        [ConfigurationProperty(PROPERTY_NAME_INTERVAL, IsRequired = false, DefaultValue = 10)]
        public int Interval
        {
            get { return (int)this[PROPERTY_NAME_INTERVAL]; }
        }
    }
}
