using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BankReportMonitor.Configuration
{
    public class WatchDirectoryCollection : ConfigurationElementCollection
    {
        private const string ELEMENT_NAME = "dir";

        protected override ConfigurationElement CreateNewElement()
        {
            return new WatchDirectory();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as WatchDirectory).DirPath;
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

        public new WatchDirectory this[string path]
        {
            get
            {
                return BaseGet(path) as WatchDirectory;
            }
        }

        public WatchDirectory this[int idx]
        {
            get
            {
                return BaseGet(idx) as WatchDirectory;
            }
        }
    }
}
