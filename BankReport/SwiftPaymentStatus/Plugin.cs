using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunGard.BankInterface.SwiftPaymentStatusUpdateFactory
{
    public class Plugin : IPlugin
    {
        public void Initialize(IApp app)
        {
            new SwiftPaymentStatusUpdateFactoryEngine(app);
        }
    }
}
