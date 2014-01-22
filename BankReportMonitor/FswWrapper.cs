using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BankReportMonitor.Configuration;
using System.Reflection;

namespace BankReportMonitor
{
    public class FswWrapper : IFswWrapper
    {
        Utility _util = null;

        List<FileSystemWatcher> _watcheres = null;

        public FswWrapper()
        {
            _util = new Utility();
        }

        public void LuanchWatcher()
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

        public void DisposeWatcher()
        {
            if (null != _watcheres && 0 < _watcheres.Count)
            {
                _watcheres.ForEach(fsw => fsw.Dispose());
            }
        }

        private void FswHandler(object sender, FileSystemEventArgs e)
        {
            try
            {
                Watcher watcher = _util.GetWatcherByFileName(e.FullPath);
                watcher.CurrentFile = e.FullPath;

                ConsumerTimeWorker worker = new ConsumerTimeWorker(ParseReport);
                worker.BeginInvoke(watcher, AfterParse, watcher);
            }
            catch
            {

            }
        }

        private void ParseReport(Watcher watcher)
        {
            Type tp = Type.GetType(watcher.ProcessType);

            if (typeof(IProcess).IsAssignableFrom(tp))
            {
                var process = Activator.CreateInstance(tp) as IProcess;
                process.Handle(watcher.CurrentFile);
            }
        }

        private void AfterParse(IAsyncResult result)
        {
            try
            {
                Watcher watcher = result.AsyncState as Watcher;

                FileInfo file = new FileInfo(watcher.CurrentFile);

                switch (watcher.LastAction)
                {
                    case "0":
                        break;
                    case "1":
                        file.Delete();
                        break;
                    case "2":
                        {
                            string destFilename = Path.Combine(watcher.LastPath, file.Name);
                            File.Delete(destFilename);
                            file.MoveTo(destFilename);
                        }
                        break;
                }
                file = null;
            }
            catch
            {

            }
        }
    }
}
