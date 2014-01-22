using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorConfiguration
{
    [Serializable]
    public class DropDownListData
    {
        public const string VALUEMEMBER_NAME = "ValueMember";
        public const string DISPLAYMEMBER_NAME = "DisplayMember";

        public string ValueMember
        {
            get;
            set;
        }

        public string DisplayMember
        {
            get;
            set;
        }
    }
}
