using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdiBankingStatus;
using System.Reflection;
using System.Threading;
using System.Security.Policy;

namespace EDIReportTest
{
    public class ServiceHostManager : MarshalByRefObject
    {
        ServiceHost _host;

        AppDomain _ad;

        public ServiceHostManager()
        {
            _host = new ServiceHost();
        }

        public void OnStart(string[] args)
        {
            ThreadPool.QueueUserWorkItem(CreateAppDomainAndStart, args);
        }

        public void OnStop()
        {
            if (null != _host)
            {
                _host.OnStop();
            }

            if (null != _ad)
            {
                AppDomain.Unload(_ad);
            }
        }

        private void CreateAppDomainAndStart(object state)
        {
            string[] args = state as string[];

            CreateNewAppDomain();

            _host.OnStart(args);
        }

        private void CreateNewAppDomain()
        {
            var ads = GetDomainSetupInformation();

            var evidence = new Evidence(AppDomain.CurrentDomain.Evidence);

            _ad = AppDomain.CreateDomain("BankReportParser AD#2", evidence, ads);

            _ad.DomainUnload += OnDomainUnload;

            Type tp = typeof(ServiceHost);

            _host = _ad.CreateInstanceAndUnwrap(tp.Assembly.FullName, tp.FullName) as ServiceHost;
        }

        void OnDomainUnload(object sender, EventArgs e)
        {
            _ad = null;
            _host = null;
        }

        private AppDomainSetup GetDomainSetupInformation()
        {
            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ads.ApplicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;

            return ads;
        }
    }
}
