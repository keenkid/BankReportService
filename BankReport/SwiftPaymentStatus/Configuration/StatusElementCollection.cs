using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SwiftPaymentStatus.Configuration
{
    public class StatusElementCollection : ConfigurationElementCollection
    {
        private const string ELEMENT_NAME = "status";

        protected override string ElementName
        {
            get
            {
                return ELEMENT_NAME;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new StatusElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var elmt = element as StatusElement;

            if (null != elmt)
            {
                return elmt.StatusLevel + elmt.StatusCode + elmt.StatusBank;
            }
            return null;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
    }
}
