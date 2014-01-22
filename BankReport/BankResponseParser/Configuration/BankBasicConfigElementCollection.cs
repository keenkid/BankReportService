using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BankResponseParser.Configuration
{
    public class BankBasicConfigElementCollection : ConfigurationElementCollection
    {
        private const string ELEMENT_NAME = "Bank";

        protected override ConfigurationElement CreateNewElement()
        {
            return new BankBasicConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BankBasicConfigElement)element).BankCode + ((BankBasicConfigElement)element).ReportType;
        }

        protected override string ElementName
        {
            get
            {
                return ELEMENT_NAME;
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public new BankBasicConfigElement this[string code]
        {
            get { return (BankBasicConfigElement)BaseGet(code); }
        }

        public BankBasicConfigElement this[int index]
        {
            get { return (BankBasicConfigElement)BaseGet(index); }
        }
    }
}
