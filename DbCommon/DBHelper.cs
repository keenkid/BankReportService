using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BankReport.Common;
using BankReport.OutputEntity;

namespace BankReport.DbCommon
{
    public class DBHelper
    {
        private List<PaymentStatus> _listPaymentStatus;

        public DBHelper() { }

        public List<PaymentStatus> ListPaymentStauts
        {
            set { _listPaymentStatus = value; }
        }

        public bool ExecuteUpdate()
        {
            string sql = BuildUpdatePaymentSqlScript();

            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            try
            {
                sql = string.Format("BEGIN\n{0}\nEND;", sql, Environment.NewLine);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string BuildUpdatePaymentSqlScript()
        {
            //if (null == _executor || null == _listPaymentStatus || 0 == _listPaymentStatus.Count)
            //{
            //    return string.Empty;
            //}

            //if (0 == string.Compare(_executor.ProviderName, "system.data.sqlclient", true))
            //{
            //    return BuildSqlServerScript();
            //}
            //else if (0 == string.Compare(_executor.ProviderName, "system.data.oracleclient", true))
            //{
            //    return BuildOracleScript();
            //}

            return string.Empty;
        }

        private string BuildOracleScript()
        {
            if (null == _listPaymentStatus || 0 == _listPaymentStatus.Count)
            {
                return string.Empty;
            }

            StringBuilder sqlBuilder = new StringBuilder();
            foreach (var PaymentStatus in _listPaymentStatus)
            {
                if (PaymentStatusLevel.File == PaymentStatus.Level)
                {
                    if (!string.IsNullOrEmpty(PaymentStatus.MessageID))
                    {
                        sqlBuilder.Append("UPDATE SGPAYMENT SET UPDATETIME=SYSDATE,QUERYCOUNT=QUERYCOUNT+1,ADDITIONAL3='FILE',RESULTCODE='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.OurStatusCode));
                        sqlBuilder.Append("',RESULTCOMMENTS='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankStatusDesc).SubStr(240));
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankReasonCode));
                        sqlBuilder.Append("' WHERE ADDITIONAL2='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.MessageID));
                        sqlBuilder.Append("' AND ADDITIONAL3 <> 'TRAN';");
                    }
                }
                if (PaymentStatusLevel.Transaction == PaymentStatus.Level)
                {
                    if (!string.IsNullOrEmpty(PaymentStatus.RefID))
                    {
                        sqlBuilder.Append("UPDATE SGPAYMENT SET UPDATETIME=SYSDATE,QUERYCOUNT=QUERYCOUNT+1,ADDITIONAL3='TRAN',RESULTCODE='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.OurStatusCode));
                        sqlBuilder.Append("',RESULTCOMMENTS='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankStatusDesc).SubStr(240));
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankReasonCode));
                        sqlBuilder.Append("' WHERE OURREF='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.RefID));
                        sqlBuilder.Append("';");                        
                    }
                }
            }

            return sqlBuilder.ToString();
        }

        private string BuildSqlServerScript()
        {
            if (null == _listPaymentStatus || 0 == _listPaymentStatus.Count)
            {
                return string.Empty;
            }

            StringBuilder sqlBuilder = new StringBuilder();
            foreach (var PaymentStatus in _listPaymentStatus)
            {
                if (PaymentStatusLevel.File == PaymentStatus.Level)
                {
                    if (!string.IsNullOrEmpty(PaymentStatus.MessageID))
                    {
                        sqlBuilder.Append("UPDATE SGPAYMENT SET UPDATETIME=GETDATE(),QUERYCOUNT=QUERYCOUNT+1,ADDITIONAL3='FILE',RESULTCODE='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.OurStatusCode));
                        sqlBuilder.Append("',RESULTCOMMENTS='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankStatusDesc).SubStr(240));
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankReasonCode));
                        sqlBuilder.Append("' WHERE ADDITIONAL2='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.MessageID));
                        sqlBuilder.Append("' AND ADDITIONAL3 <> 'TRAN';");
                    }
                }
                if (PaymentStatusLevel.Transaction == PaymentStatus.Level)
                {
                    if (!string.IsNullOrEmpty(PaymentStatus.RefID))
                    {
                        sqlBuilder.Append("UPDATE SGPAYMENT SET UPDATETIME=GETDATE(),QUERYCOUNT=QUERYCOUNT+1,ADDITIONAL3='TRAN',RESULTCODE='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.OurStatusCode));
                        sqlBuilder.Append("',RESULTCOMMENTS='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankStatusDesc).SubStr(240));
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.BankReasonCode));
                        sqlBuilder.Append("' WHERE OURREF='");
                        sqlBuilder.Append(CorrectFieldValue(PaymentStatus.RefID));
                        sqlBuilder.Append("';");
                    }
                }
            }

            return sqlBuilder.ToString();
        }

        private string CorrectFieldValue(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                return string.Empty;
            }
            try
            {
                return Regex.Replace(fieldValue, @"'|INSERT|DELETE|UPDATE|SELECT|TRUNCATE|DROP",
                    m => string.Empty, RegexOptions.IgnoreCase);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
