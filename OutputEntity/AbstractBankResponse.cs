using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BankReport.Common;

namespace BankReport.OutputEntity
{
    [Serializable]
    public abstract class AbstractBankResponse : AbstractOutput
    {
        [XmlTag(Tag = "bankCode", Level = XmlLevel.Head)]
        public override string BankCode { get; set; }

        [XmlTag(Tag = "transCode", Level = XmlLevel.Head)]
        public override string TransCode { get; set; }

        [XmlTag(Tag = "transDate", Level = XmlLevel.Head)]
        public string TransDate { get { return DateTime.Now.ToString("yyyyMMdd"); } }

        [XmlTag(Tag = "transTime", Level = XmlLevel.Head)]
        public string TransTime { get { return DateTime.Now.ToString("HHmmss"); } }

        public override string ToString()
        {
            return this.GetType().GetXmlStructre(this);
        }
    }
}
