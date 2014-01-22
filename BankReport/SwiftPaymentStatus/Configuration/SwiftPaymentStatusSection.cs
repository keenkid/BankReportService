using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using BankReport.Common;

namespace SwiftPaymentStatus.Configuration
{
    public class SwiftPaymentStatusSection : ConfigurationSection
    {
        private const string SECTION_NAME = "swiftPaymentStatusHandleSection";        
        private const string ELEMENT_SWIFTPAYMENTSTATUS = "swiftPaymentStatus";
        private static SwiftPaymentStatusSection _instance;

        private SwiftPaymentStatusSection()
        {
        }

        public static SwiftPaymentStatusSection Instance
        {
            get
            {
                if (null == _instance)
                {
                    var config = typeof(SwiftPaymentStatusSection).GetAssemblyConfiguration();
                    _instance = config.GetSection(SECTION_NAME) as SwiftPaymentStatusSection;
                }

                return _instance;
            }
        }

        [ConfigurationProperty(ELEMENT_SWIFTPAYMENTSTATUS, IsKey = false, IsRequired = true)]
        public SwiftPaymentStatusElement PaymentStatus
        {
            get
            {
                return this[ELEMENT_SWIFTPAYMENTSTATUS] as SwiftPaymentStatusElement;
            }
        }
    }
}
