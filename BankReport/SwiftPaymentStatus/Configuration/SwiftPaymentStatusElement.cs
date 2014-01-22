using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SwiftPaymentStatus.Configuration
{
    public class SwiftPaymentStatusElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public StatusElementCollection StatusCollection
        {
            get
            {
                return this[""] as StatusElementCollection;
            }
        }
    }
}
