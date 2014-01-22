using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReport.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MonitorAttribute : Attribute
    {
        public MonitorAttribute() 
        {
            SetAsMonitor = true;
        }

        public bool SetAsMonitor
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
