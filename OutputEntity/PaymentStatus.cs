using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReport.Common;

namespace BankReport.OutputEntity
{
    [Serializable]
    public class PaymentStatus : AbstractOutput
    {
        public override string BankCode { get; set; }

        public override string TransCode { get; set; }

        public string MessageID { get; set; }

        public string RefID { get; set; }

        public string BankRef { get; set; }

        public string LineSpan { get; set; }

        public BankReportType Source { get; set; }

        public PaymentStatusLevel Level { get; set; }

        public string OurStatusCode { get; set; }

        public string BankStatusCode { get; set; }

        public string BankReasonCode { get; set; }

        public string BankStatusDesc { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("[MessageID='{0}']", MessageID));
            builder.Append(string.Format("[RefID='{0}']", RefID));
            builder.Append(string.Format("[BankRef='{0}']", BankRef));
            builder.Append(string.Format("[Level='{0}']", Level));
            builder.Append(string.Format("[OurStatusCode='{0}']", OurStatusCode));
            builder.Append(string.Format("[BankStatusCode='{0}']", BankStatusCode));
            builder.Append(string.Format("[BankReasonCode='{0}']", BankReasonCode));
            builder.Append(string.Format("[BankStatusDesc='{0}']", BankStatusDesc));
            builder.Append(";");
            return builder.ToString();
        }
    }
}
