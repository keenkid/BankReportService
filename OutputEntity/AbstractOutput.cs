using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReport.OutputEntity
{
    public abstract class AbstractOutput
    {
        public abstract string BankCode { get; set; }

        public abstract string TransCode { get; set; }
    }
}
