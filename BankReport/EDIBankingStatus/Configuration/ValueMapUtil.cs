using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdiBankingStatus
{
    public class ValueMapUtil
    {
        public List<ValueMap> ListBankMap { get; private set; }
        public List<ValueMap> ListReportMap { get; private set; }

        public ValueMapUtil()
        {
            ListBankMap = new List<ValueMap>();
            ListReportMap = new List<ValueMap>();
            Initialize();
        }

        private void Initialize()
        {
            var bankSection = BankMapSectionHandler.Instance;

            if (bankSection != null && bankSection.BankMapCollection != null)
            {
                foreach (Map m in bankSection.BankMapCollection)
                {
                    ListBankMap.Add(new ValueMap {Identifier = m.Identifier, Value = m.Value });
                }
            }

            var reportSection = ReportMapSectionHandler.Instance;
            if (reportSection != null && reportSection.ReportMapCollection != null)
            {
                foreach (Map m in reportSection.ReportMapCollection)
                {
                    ListReportMap.Add(new ValueMap { Identifier = m.Identifier, Value = m.Value });
                }
            }
        }
    }

    public class ValueMap
    {
        public string Identifier { get; set; }

        public string Value { get; set; }
    }
}
