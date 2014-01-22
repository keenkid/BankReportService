using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwiftPaymentStatus.Configuration;
using BankReport.Common;
using BankReport.OutputEntity;

namespace SwiftPaymentStatus
{
    public class SwiftPaymentStatus
    {
        public string StatusLevel
        {
            get;
            set;
        }

        public string StatusCode
        {
            get;
            set;
        }

        public string StatusBank
        {
            get;
            set;
        }

        public string StatusResult
        {
            get;
            set;
        }

        public string StatusDesc
        {
            get;
            set;
        }
    }

    public class Utility
    {
        private string _filePath = string.Empty;
        private string _backupPath = string.Empty;
        private List<SwiftPaymentStatus> _swiftPaymentStatusList;

        public Utility()
        {
            _swiftPaymentStatusList = new List<SwiftPaymentStatus>();

            Initialize();
        }

        public List<SwiftPaymentStatus> SwiftPaymentStatusList
        {
            get { return _swiftPaymentStatusList; }
        }

        private void Initialize()
        {
            var section = SwiftPaymentStatusSection.Instance;

            if (null != section &&
                null != section.PaymentStatus &&
                null != section.PaymentStatus.StatusCollection &&
                0 < section.PaymentStatus.StatusCollection.Count)
            {
                foreach (StatusElement status in section.PaymentStatus.StatusCollection)
                {
                    _swiftPaymentStatusList.Add(new SwiftPaymentStatus()
                    {
                        StatusBank = status.StatusBank,
                        StatusCode = status.StatusCode,
                        StatusLevel = status.StatusLevel,
                        StatusResult = status.StatusResult,
                        StatusDesc = status.StatusDesc
                    });
                }
            }
        }

        public void GetOurStatus(PaymentStatus status)
        {
            if (null == status || 
                string.IsNullOrEmpty(status.BankStatusCode) ||
                status.Level == PaymentStatusLevel.Invalid)
            {
                return;
            }
            string level = (status.Level == PaymentStatusLevel.File) ? "FILE" : "TRAN";
            foreach (SwiftPaymentStatus util in _swiftPaymentStatusList)
            {
                if (0 == string.Compare(util.StatusLevel, level, true) &&
                    0 == string.Compare(util.StatusCode, status.BankStatusCode, true))
                {
                    status.OurStatusCode = util.StatusResult;
                    status.BankStatusDesc = util.StatusDesc;
                    break;
                }
            }
        }
    }
}
