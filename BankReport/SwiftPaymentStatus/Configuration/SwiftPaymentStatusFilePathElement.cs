using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SwiftPaymentStatus.Configuration
{
    public class SwiftPaymentStatusFilePathElement : ConfigurationElement
    {
        private const string PROPERTY_PATH = "path";
        private const string PROPERTY_BACKUP = "backup";

        [ConfigurationProperty(PROPERTY_PATH, IsRequired = true)]
        public string FilePath
        {
            get
            {
                return this[PROPERTY_PATH] as string;
            }
        }

        [ConfigurationProperty(PROPERTY_BACKUP, IsRequired = true)]
        public string BackupPath
        {
            get
            {
                return this[PROPERTY_BACKUP] as string;
            }
        }
    }
}
