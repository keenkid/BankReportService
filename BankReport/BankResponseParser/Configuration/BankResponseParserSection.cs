using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using Common;

namespace BankResponseParser.Configuration
{
    public sealed class BankResponseParserSection : ConfigurationSection
    {
        private static BankResponseParserSection intance = null;

        private const string SECTION_NAME = "BankResponseParser";
        private const string ELEMENT_REPORTBANK = "BankResponseReportPath";
        private const string ELEMENT_REPEAT = "Repeat";

        private BankResponseParserSection() { }

        public static BankResponseParserSection Instance
        {
            get
            {
                if (intance == null)
                {
                    var config = typeof(BankResponseParserSection).GetAssemblyConfiguration();
                    var section = config.GetSection(SECTION_NAME);
                    intance = section as BankResponseParserSection;
                }
                return intance;
            }
        }

        [ConfigurationProperty(ELEMENT_REPORTBANK, IsKey = false, IsRequired = true)]
        public BankResponseReportPathElement Report
        {
            get { return base[ELEMENT_REPORTBANK] as BankResponseReportPathElement; }
        }

        //[ConfigurationProperty(ELEMENT_REPEAT, IsKey = false, IsRequired = false)]
        //public IntervalElement Repeat
        //{
        //    get { return base[ELEMENT_REPEAT] as IntervalElement; }
        //}
    }
}
