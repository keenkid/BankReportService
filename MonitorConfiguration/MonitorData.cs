using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorConfiguration
{
    [Serializable]
    public class MonitorData
    {
        public string Name { get; set; }

        public string WatchPath { get; set; }

        public string NextAction { get; set; }

        public string BackupPath { get; set; }

        public string ProcessType { get; set; }
    }
}
