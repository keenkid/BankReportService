using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReportMonitor.Configuration;
using System.IO;
using BankReport.Common;

namespace BankReportMonitor
{
    public abstract class AbstractMonitor
    {
        protected Utility _util = null;

        public AbstractMonitor()
        {
            _util = new Utility();
        }

        public abstract void LaunchMonitor();

        public abstract void DisposeMonitor();

        protected void MonitorHandle(Watcher watcher)
        {
            try
            {
                ConsumerTimeWorker worker = new ConsumerTimeWorker(Handle);
                worker.BeginInvoke(watcher, AfterHandle, watcher);
            }
            catch
            {

            }
        }

        private void Handle(Watcher watcher)
        {
            object obj = new object();

            lock (obj)
            {
                Type tp = Type.GetType(watcher.ProcessType);

                if (typeof(IProcess).IsAssignableFrom(tp))
                {
                    Logger.sysLog.InfoFormat("Start to Parse report:[{0}]", watcher.CurrentFile);
                    var process = Activator.CreateInstance(tp) as IProcess;
                    process.Handle(watcher.CurrentFile);
                }
            }
        }

        private void AfterHandle(IAsyncResult result)
        {
            try
            {
                Watcher watcher = result.AsyncState as Watcher;

                FileInfo file = new FileInfo(watcher.CurrentFile);

                AfterProcessAction action = (AfterProcessAction)Enum.Parse(typeof(AfterProcessAction), watcher.LastAction, true);
                
                switch (action)
                {
                    case AfterProcessAction.None:
                        break;
                    case AfterProcessAction.Delete:
                        file.Delete();
                        Logger.sysLog.InfoFormat("Bank report [{0}] was deleted", watcher.CurrentFile);
                        break;
                    case AfterProcessAction.Backup:
                        {
                            string destFilename = Path.Combine(watcher.LastPath, file.Name);
                            File.Delete(destFilename);
                            file.MoveTo(destFilename);
                            Logger.sysLog.InfoFormat("Bank report was moved to [{0}]", destFilename);
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
