using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using BankReport.OutputEntity;

namespace SwiftPaymentStatus
{
    public static class TransactionReasonCode
    {
        public static void FillTransactionRejectReason(PaymentStatus status)
        {
            if (null == status || string.IsNullOrEmpty(status.BankReasonCode))
            {
                return;
            }

            try
            {
                XDocument xdoc = XDocument.Parse(Res.TransactionRejectReason);
                string desc = (from elmt in xdoc.Descendants("reason")
                               where 0 == string.Compare(elmt.Attribute("code").Value, status.BankReasonCode, true)
                               select elmt.Attribute("desc").Value).First();
                status.BankStatusDesc = desc;
            }
            catch
            {
                //
            }
        }
    }
}
