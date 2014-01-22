using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BankReportMonitor.Configuration
{
    public class WatchDirectory : ConfigurationElement
    {
        private const string PROPERTY_DIRNAME = "name";

        private const string PROPERTY_DIRPATH = "path";

        private const string PROPERTY_LASTACTION = "lastAction";

        private const string PROPERTY_LASTPARTH = "lastPath";

        private const string PROPERTY_TYPE = "type";

        [ConfigurationProperty(PROPERTY_DIRNAME, IsKey = false, IsRequired = true)]
        public string DirName
        {
            get
            {
                return this[PROPERTY_DIRNAME] as string;
            }
            set
            {
                this[PROPERTY_DIRNAME] = value;
            }
        }

        [ConfigurationProperty(PROPERTY_DIRPATH, IsKey = true, IsRequired = true)]
        public string DirPath
        {
            get
            {
                return this[PROPERTY_DIRPATH] as string;
            }
            set
            {
                this[PROPERTY_DIRPATH] = value;
            }
        }

        [ConfigurationProperty(PROPERTY_LASTACTION, IsKey = false, IsRequired = false)]
        public string LastAction
        {
            get
            {
                return this[PROPERTY_LASTACTION] as string;
            }
            set
            {
                this[PROPERTY_LASTACTION] = value;
            }
        }

        [ConfigurationProperty(PROPERTY_LASTPARTH, IsKey = false, IsRequired = false)]
        public string LastPath
        {
            get
            {
                return this[PROPERTY_LASTPARTH] as string;
            }
            set
            {
                this[PROPERTY_LASTPARTH] = value;
            }
        }

        [ConfigurationProperty(PROPERTY_TYPE, IsKey = false, IsRequired = false)]
        public string ProcessType
        {
            get
            {
                return this[PROPERTY_TYPE] as string;
            }
            set
            {
                this[PROPERTY_TYPE] = value;
            }
        }
    }
}
