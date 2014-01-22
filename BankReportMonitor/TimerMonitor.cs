using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using BankReportMonitor.Configuration;
using System.IO;

namespace BankReportMonitor
{
    public class TimerMonitor : AbstractMonitor
    {
        Timer _timer = null;

        public override void LaunchMonitor()
        {
            _timer = new Timer(_util.ElapseSecond);
            //only fire at first elapsed
            _timer.AutoReset = false;
            _timer.Enabled = true;
            _timer.Elapsed += OnElapsed;
            _timer.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            List<string> filenames = new List<string>();

            foreach (Watcher watcher in _util.WatcherCollection)
            {
                filenames.AddRange(Directory.GetFiles(watcher.DirPath));
            }

            HandleEachFile(filenames);
        }

        private void HandleEachFile(List<string> filenames)
        {
            foreach (string filename in filenames)
            {
                Watcher watcher = _util.GetWatcherByFileName(filename);
                if (null == watcher)
                {
                    continue;
                }
                watcher.CurrentFile = filename;

                MonitorHandle(watcher);
            }
        }

        public override void DisposeMonitor()
        {
            if (null != _timer)
            {
                _timer.Stop();
                _timer.Close();
            }
        }
    }
}
