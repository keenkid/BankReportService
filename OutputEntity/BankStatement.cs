using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using BankReport.Common;

namespace BankReport.OutputEntity
{
    [Serializable]
    public class BankStatement : AbstractBankResponse
    {
        [XmlTag(Tag = "refID", Level = XmlLevel.Body)]
        public string RefID { get; set; }

        [XmlTag(Tag = "bankRefID", Level = XmlLevel.Body)]
        public string BankRefID { get; set; }

        [XmlTag(Tag = "valueDate", Level = XmlLevel.Body)]
        public string ValueDate { get; set; }

        [XmlTag(Tag = "entryDate", Level = XmlLevel.Body)]
        public string EntryDate { get; set; }

        [XmlTag(Tag = "acctNo", Level = XmlLevel.Body)]
        public string AcctNo { get; set; }

        [XmlTag(Tag = "acctName", Level = XmlLevel.Body)]
        public string AcctName { get; set; }

        [XmlTag(Tag = "currType", Level = XmlLevel.Body)]
        public string CurrType { get; set; }

        [XmlTag(Tag = "amount", Level = XmlLevel.Body)]
        public string Amount { get; set; }

        [XmlTag(Tag = "balance", Level = XmlLevel.Body)]
        public string Balance { get; set; }

        [XmlTag(Tag = "cptyAcctNo", Level = XmlLevel.Body)]
        public string CptyAcctNo { get; set; }

        [XmlTag(Tag = "cptyAcctName", Level = XmlLevel.Body)]
        public string CptyAcctName { get; set; }

        [XmlTag(Tag = "cptyBankName", Level = XmlLevel.Body)]
        public string CptyBankName { get; set; }

        [XmlTag(Tag = "remark", Level = XmlLevel.Body)]
        public string Remark { get; set; }

        [XmlTag(Tag = "abstract", Level = XmlLevel.Body)]
        public string Abstract { get; set; }
    }
}
