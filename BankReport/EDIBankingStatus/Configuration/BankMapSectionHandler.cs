using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EdiBankingStatus
{
    public class BankMapSectionHandler : ConfigurationSection
    {
        private static BankMapSectionHandler instance = null;

        private const string SECTION_NAME = "BankMap";

        private BankMapSectionHandler() { }

        public static BankMapSectionHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    var config = ConfigurationManager.OpenExeConfiguration(typeof(BankMapSectionHandler).Assembly.Location);
                    instance = config.GetSection(SECTION_NAME) as BankMapSectionHandler;
                }
                return instance;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public MapCollection BankMapCollection
        {
            get { return this[""] as MapCollection; }
        }
    }

    public class Map : ConfigurationElement
    {
        private const string IDENTIFIER = "identifier";
        private const string VALUE = "value";

        [ConfigurationProperty(IDENTIFIER, IsKey = true, IsRequired = true)]
        public string Identifier { get { return this[IDENTIFIER].ToString(); } }

        [ConfigurationProperty(VALUE, IsKey = false, IsRequired = true)]
        public string Value { get { return this[VALUE].ToString(); } }
    }

    public class MapCollection : ConfigurationElementCollection
    {
        private const string ELEMENT_NAME = "map";

        protected override ConfigurationElement CreateNewElement()
        {
            return new Map();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Map)element).Identifier;
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

        public new Map this[string code]
        {
            get { return (Map)BaseGet(code); }
        }

        public Map this[int idx]
        {
            get { return (Map)BaseGet(idx); }
        }
    }

}
