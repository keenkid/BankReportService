using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace BankReport.OutputEntity
{
    [Serializable]
    public class BankBalance : AbstractBankResponse
    {
        [XmlTag(Tag = "acctNo", Level = XmlLevel.Body)]
        public string AcctNo { get; set; }

        [XmlTag(Tag = "currType", Level = XmlLevel.Body)]
        public string CurrType { get; set; }

        [XmlTag(Tag = "balanceDate", Level = XmlLevel.Body)]
        public string BalanceDate { get; set; }

        [XmlTag(Tag = "balance", Level = XmlLevel.Body)]
        public string Balance { get; set; }

        [XmlTag(Tag = "useableBalance", Level = XmlLevel.Body)]
        public string UsableBalance { get; set; }
    }
}
