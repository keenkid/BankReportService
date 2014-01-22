using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReport.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlTagAttribute : Attribute
    {
        public XmlTagAttribute()
        {

        }

        public XmlTagAttribute(string tag, XmlLevel level)
        {
            Tag = tag;
            Level = level;
        }

        public string Tag { get; set; }

        public XmlLevel Level { get; set; }
    }
}
