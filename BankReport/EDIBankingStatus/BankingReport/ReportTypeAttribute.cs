using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdiBankingStatus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ReportTypeAttribute : Attribute
    {
        public ReportTypeAttribute(string reportType)
        {
            ReportName = reportType;
        }

        public string ReportName
        {
            set;
            get;
        }
    }
}
