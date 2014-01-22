using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EdiBankingStatus
{
    public class ReportMapSectionHandler : ConfigurationSection
    {
        private static ReportMapSectionHandler instance = null;

        private const string SECTION_NAME = "ReportMap";

        private ReportMapSectionHandler() { }

        public static ReportMapSectionHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    var config = ConfigurationManager.OpenExeConfiguration(typeof(ReportMapSectionHandler).Assembly.Location);
                    instance = config.GetSection(SECTION_NAME) as ReportMapSectionHandler;
                }
                return instance;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public MapCollection ReportMapCollection
        {
            get { return this[""] as MapCollection; }
        }
    }
}
