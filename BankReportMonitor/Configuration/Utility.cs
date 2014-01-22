using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using BankReport.Common;

namespace BankReportMonitor.Configuration
{
    public class Utility
    {
        private List<Watcher> _collection = null;

        public Utility()
        {            
            Initialize();
            InitializeTimer();
        }

        private void Initialize()
        {
            _collection = new List<Configuration.Watcher>();
            if (null != WatchHandleSection.Instance && null != WatchHandleSection.Instance.WatchDirCollection)
            {
                foreach (WatchDirectory item in WatchHandleSection.Instance.WatchDirCollection)
                {
                    _collection.Add(new Watcher()
                    {
                        DirName = item.DirName,
                        DirPath = item.DirPath,
                        LastAction = item.LastAction,
                        LastPath = item.LastPath,
                        ProcessType = item.ProcessType
                    });
                }
            }            
        }

        private void InitializeTimer()
        {
            int elapse = 60;
            var config = typeof(Utility).GetAssemblyConfiguration();
            if (null == config)
            {
                ElapseSecond = 60;
                return;
            }
            if (Int32.TryParse(config.AppSettings.Settings["elapse"].Value, out elapse))
            {
                ElapseSecond = elapse;
            }
            else
            {
                ElapseSecond = 60;
            }
        }

        public List<Watcher> WatcherCollection
        {
            get { return _collection; }
            set { _collection = value; }
        }

        public int ElapseSecond
        {
            get;
            set;
        }

        public Watcher GetWatcherByFileName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            foreach (Watcher watcher in WatcherCollection)
            {
                if (filename.StartsWith(watcher.DirPath, StringComparison.CurrentCultureIgnoreCase))
                {
                    return watcher;
                }
            }
            return null;
        }
    }
}
