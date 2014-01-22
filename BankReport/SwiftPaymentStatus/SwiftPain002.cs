using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using BankReport.Common;
using BankReport.OutputEntity;

namespace SwiftPaymentStatus
{
    public class SwiftPain002
    {
        private const string PAIN002_WHOLE = "CstmrPmtStsRpt";
        private const string PAIN002_FILE_LEVEL_TAG = "OrgnlGrpInfAndSts";
        private const string PAIN002_TRAN_LEVEL_TAG = "OrgnlPmtInfAndSts";

        private const string ORIGINAL_MESSAGEID = "OrgnlMsgId";
        private const string ORIGINAL_MESSAGE_STATUS = "GrpSts";
        private const string ORIGINAL_REFID = "OrgnlEndToEndId";
        private const string ORIGINAL_PAYMENT_STATUS = "TxSts";

        private const string STATUS_REASON_INFORMATION = "StsRsnInf";
        private const string STATUS_REASON = "Rsn";
        private const string STATUS_REASON_CODE = "Cd";
        private const string STATUS_READON_DESC = "AddtlInf";

        private string _fileName = string.Empty;
        private string _content = string.Empty;
        private List<PaymentStatus> _listPaymentStatus;
        private Utility _utility;

        public SwiftPain002() { }

        public SwiftPain002(string fileName)
        {
            _fileName = fileName;
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string FileContent
        {
            get
            {
                if (!string.IsNullOrEmpty(_content))
                {
                    return _content;
                }
                if (File.Exists(_fileName))
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(_fileName, true))
                        {
                            _content = sr.ReadToEnd();
                            _content = Regex.Replace(_content, @" ?xmlns.+?>", m => ">");
                            return _content;
                        }
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
        }

        public bool IsValidSwiftPain002
        {
            get
            {
                return Validate();
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FileContent))
            {
                return false;
            }
            try
            {
                XDocument xdoc = XDocument.Parse(FileContent);
                var elmt = xdoc.Descendants("CstmrPmtStsRpt");
                if (null != elmt && 0 < elmt.Count())
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public PaymentStatusLevel ReportLevel
        {
            get
            {
                if (string.IsNullOrEmpty(FileContent))
                {
                    return PaymentStatusLevel.Invalid;
                }
                try
                {
                    XDocument xdoc = XDocument.Parse(FileContent);
                    var fileLevelElmt = xdoc.Descendants(PAIN002_FILE_LEVEL_TAG);
                    var transLevelElmt = xdoc.Descendants(PAIN002_TRAN_LEVEL_TAG);

                    if (null != transLevelElmt && 0 < transLevelElmt.Count() &&
                        null != fileLevelElmt && 0 < fileLevelElmt.Count())
                    {
                        return PaymentStatusLevel.Transaction;
                    }
                    if (null != fileLevelElmt && 0 < fileLevelElmt.Count())
                    {
                        return PaymentStatusLevel.File;
                    }
                    return PaymentStatusLevel.Invalid;
                }
                catch
                {
                    return PaymentStatusLevel.Invalid;
                }
            }
        }

        public List<PaymentStatus> ListPaymentStatus
        {
            get
            {
                return GetPaymentStatus();
            }
        }

        private List<PaymentStatus> GetPaymentStatus()
        {
            try
            {
                _utility = new Utility();
                if (ReportLevel == PaymentStatusLevel.File)
                {
                    return GetFileLevelPaymentStatus();
                }
                else if (ReportLevel == PaymentStatusLevel.Transaction)
                {
                    return GetTransactionLevelPaymentStatus();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private List<PaymentStatus> GetFileLevelPaymentStatus()
        {
            try
            {
                XDocument xdoc = XDocument.Parse(FileContent);
                var fileLevelDoc = xdoc.Descendants(PAIN002_FILE_LEVEL_TAG).First();
                var msgID = fileLevelDoc.Descendants(ORIGINAL_MESSAGEID);
                var msgStatus = fileLevelDoc.Descendants(ORIGINAL_MESSAGE_STATUS);

                _listPaymentStatus = new List<PaymentStatus>();

                if (null != msgID && 0 < msgID.Count() &&
                    null != msgStatus && 0 < msgStatus.Count())
                {
                    var PaymentStatus = new PaymentStatus()
                    {
                        MessageID = msgID.First().Value,
                        Level = PaymentStatusLevel.File,
                        BankStatusCode = msgStatus.First().Value
                    };                    
                    _utility.GetOurStatus(PaymentStatus);

                    GetReasonCodeAndDesc(fileLevelDoc, PaymentStatus);

                    _listPaymentStatus.Add(PaymentStatus);

                    return _listPaymentStatus;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private List<PaymentStatus> GetTransactionLevelPaymentStatus()
        {
            try
            {
                XDocument xdoc = XDocument.Parse(FileContent);
                var trans = xdoc.Descendants(PAIN002_TRAN_LEVEL_TAG).ToList();
                _listPaymentStatus = new List<PaymentStatus>();

                if (null != trans && 0 < trans.Count())
                {
                    trans.ForEach(transLevelDoc =>
                    {
                        var refID = transLevelDoc.Descendants(ORIGINAL_REFID);
                        var paymentStatus = transLevelDoc.Descendants(ORIGINAL_PAYMENT_STATUS);

                        if (null != refID && 0 < refID.Count() &&
                            null != paymentStatus && 0 < paymentStatus.Count())
                        {
                            var PaymentStatus = new PaymentStatus()
                            {
                                RefID = refID.First().Value,
                                Level = PaymentStatusLevel.Transaction,
                                BankStatusCode = paymentStatus.First().Value
                            };
                            _utility.GetOurStatus(PaymentStatus);

                            GetReasonCodeAndDesc(transLevelDoc, PaymentStatus);

                            _listPaymentStatus.Add(PaymentStatus);
                        }
                    });
                    return _listPaymentStatus;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private void GetReasonCodeAndDesc(XElement elmt, PaymentStatus status)
        {
            try
            {
                var rsnInf = elmt.Descendants(STATUS_REASON_INFORMATION);
                if (null != rsnInf && 0 < rsnInf.Count())
                {
                    var rsn = rsnInf.First().Descendants(STATUS_REASON);
                    if (null != rsn && 0 < rsn.Count())
                    {
                        var rsnCd = rsn.First().Descendants(STATUS_REASON_CODE);
                        if (null != rsnCd && 0 < rsnCd.Count())
                        {
                            status.BankReasonCode = rsnCd.First().Value;
                            TransactionReasonCode.FillTransactionRejectReason(status);
                        }
                    }
                    //
                    var rsnDesc = rsnInf.First().Descendants(STATUS_READON_DESC).ToList();
                    if (null != rsnDesc && 0 < rsnDesc.Count())
                    {
                        StringBuilder tmp = new StringBuilder();
                        rsnDesc.ForEach(rd => tmp.Append(rd.Value));
                        status.BankStatusDesc = tmp.ToString();
                    }
                }
            }
            catch
            {

            }
        }
    }
}
