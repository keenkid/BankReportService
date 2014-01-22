using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReport.Common
{
    public class DbColumnAttribute : Attribute
    {
        public DbColumnAttribute() { }

        public string DbColumnName
        {
            get;
            set;
        }
    }
}
