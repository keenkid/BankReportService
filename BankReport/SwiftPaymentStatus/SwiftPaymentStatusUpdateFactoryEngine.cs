using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SunGard.BankInterface.Engineering;
using SunGard.BankInterface.Data;
using SunGard.BankInterface.SwiftPaymentStatusUpdateFactory.Configuration;

namespace SunGard.BankInterface.SwiftPaymentStatusUpdateFactory
{
    class SwiftPaymentStatusUpdateFactoryEngine : AppEventDrivenObject
    {
        private const int DEFAULT_WAIT_SECONDS = 300;

        private AutoResetEvent _block;
        private IAdvancedEngine _engine;
        private IDbClientFactory _executorFactory;
        private int _waitSeconds;
        private FactoryUtility _utility;

        public SwiftPaymentStatusUpdateFactoryEngine(IApp app)
            : base(app, AppEvents.Initialize | AppEvents.PostRun | AppEvents.PreStop)
        {
            app.GetService<ILogProvider>().System.Info("Constructing SWIFT payment status update engine...");
        }

        protected override void InstantiateComponents()
        {
            _block = new AutoResetEvent(false);
            _utility = new FactoryUtility();

            LoadWaitSeconds();

            _engine = base.Application.GetService<IAdvancedEngineFactory>().GetEngine(
                new EngineCreationContext()
                {
                    Name = "SwiftPaymentStatusUpdateFactory",
                    Description = "SWIFT Payment Status Update Factory"
                });
            _engine.Scheduler = SwiftPaymentStatus;
        }

        protected override void OnApplicationInitialize(object sender, AppEventArgs e)
        {
            base.Application.GetService<ILogProvider>().System.Info("Initializing SWIFT payment status update engine...");
            var efp = Application.GetService<IDbClientFactoryProvider>();
            _executorFactory = efp.GetDbClientFactory();
        }

        protected override void OnPostApplicationRun(object sender, AppEventArgs e)
        {
            base.Application.GetService<ILogProvider>().System.Info("Launching SWIFT payment status update engine...");
            _engine.Run();
        }

        protected override void OnPreApplicationStop(object sender, AppEventArgs e)
        {
            base.Application.GetService<ILogProvider>().System.Info("Stopping SWIFT payment status update engine...");
            _engine.Stop(() => _block.Set());
        }

        private void LoadWaitSeconds()
        {
            var section = SwiftPaymentStatusUpdateFactorySection.Instance;

            if (null != section &&
                null != section.Repeat)
            {
                _waitSeconds = section.Repeat.Interval;
            }

            if (0 >= _waitSeconds)
            {
                _waitSeconds = DEFAULT_WAIT_SECONDS;
            }
        }

        private void SwiftPaymentStatus(LaunchTask launch)
        {
            try
            {
                base.Application.GetService<ILogProvider>().Runtime.Info("Start to update SWIFT payment status...");

                var executor = _executorFactory.CreateExecutor();

                if (null != executor)
                {
                    using (executor)
                    {
                        var factory = new SwiftPaymentStatusUpdateFactory(Application, executor, _utility);
                        factory.UpdateSwiftPaymentStatus();
                    }
                }

                _block.WaitOne(_waitSeconds * 1000);
            }
            catch (Exception e)
            {
                base.Application.GetService<ILogProvider>().Runtime.Error(e.Message, e);
            }
        } 
    }
}
