using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdiBankingStatus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BankAttribute : Attribute
    {
        public BankAttribute(string bankName)
        {
            BankName = bankName;
        }

        public string BankName
        {
            get;
            set;
        }
    }
}
