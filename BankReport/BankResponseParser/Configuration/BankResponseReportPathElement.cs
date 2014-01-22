using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BankResponseParser.Configuration
{
    public class BankResponseReportPathElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public BankBasicConfigElementCollection BECollection
        {
            get { return this[""] as BankBasicConfigElementCollection; }
        }
    }
}
