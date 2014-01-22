using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using BankReport.Common;

namespace BankReportMonitor.Configuration
{
    public class WatchHandleSection : ConfigurationSection
    {
        private const string SECTION_NAME = "watch";

        private static WatchHandleSection _instance = null;

        protected WatchHandleSection() { }

        public static WatchHandleSection Instance
        {
            get
            {
                if (null == _instance)
                {
                    var config = typeof(WatchHandleSection).GetAssemblyConfiguration();
                    if (null != config)
                    {
                        _instance = config.GetSection(SECTION_NAME) as WatchHandleSection;
                    }                    
                }
                return _instance;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public WatchDirectoryCollection WatchDirCollection
        {
            get
            {
                return this[""] as WatchDirectoryCollection;
            }
        }
    }
}
