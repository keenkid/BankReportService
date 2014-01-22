using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReportMonitor.Configuration
{
    [Serializable]
    public class Watcher
    {
        public string DirName { get; set; }

        public string DirPath { get; set; }

        public string LastAction { get; set; }

        public string LastPath { get; set; }

        public string ProcessType { get; set; }

        public string CurrentFile { get; set; }
    }
}
