using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BankReportMonitor.Configuration;

namespace BankReportMonitor
{
    public class FswMonitor : AbstractMonitor
    {
        List<FileSystemWatcher> _watcheres = null;

        public override void LaunchMonitor()
        {
            _watcheres = new List<FileSystemWatcher>();

            foreach (Watcher dir in _util.WatcherCollection)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();

                watcher.IncludeSubdirectories = false;
                watcher.Path = dir.DirPath;
                watcher.InternalBufferSize = 64 * 1024;
                watcher.NotifyFilter = NotifyFilters.FileName;

                watcher.Created += FswHandler;

                watcher.EnableRaisingEvents = true;

                _watcheres.Add(watcher);
            }
        }

        private void FswHandler(object sender, FileSystemEventArgs e)
        {
            try
            {
                Watcher watcher = _util.GetWatcherByFileName(e.FullPath);
                if (null != watcher)
                {
                    watcher.CurrentFile = e.FullPath;
                    MonitorHandle(watcher);
                }
            }
            catch
            {

            }
        }

        public override void DisposeMonitor()
        {
            if (null != _watcheres && 0 < _watcheres.Count)
            {
                _watcheres.ForEach(fsw => fsw.Dispose());
            }
        }
    }
}
