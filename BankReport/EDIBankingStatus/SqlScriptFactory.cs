using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public static class SqlScriptFactory
    {
        private const string SCRIPT_SQLSERVER_INSERT = "INSERT INTO SGEDIBankingStatus(FileReference,CustomReference,BankReference,Bank,LineSpan,Source,OurStatusCode,BankStatusCode,BankStatusDesc) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";

        private const string SCRIPT_SQLSERVER_UPDATE = "UPDATE SGEDIBankingStatus SET OurStatusCode='{0}',BankStatusCode='{1}',BankStatusDesc='{2}',UpdateDate=GETDATE(),Source='{3}' WHERE 1=1";

        private const string SCRIPT_ORACLE_INSERT = "INSERT INTO SGEDIBankingStatus(FileReference,CustomReference,BankReference,Bank,LineSpan,Source,OurStatusCode,BankStatusCode,BankStatusDesc) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";

        private const string SCRIPT_ORACLE_UPDATE = "UPDATE SGEDIBankingStatus SET OurStatusCode='{0}',BankStatusCode='{1}',BankStatusDesc='{2}',UpdateDate=SYSDATE,Source='{3}' WHERE 1=1";

        private static string dbTypeString = string.Empty;

        private static List<PaymentStatus> ListStatusInfo { get; set; }

        private static PaymentStatus status = null;

        private static StringBuilder sqlBuilder = null;

        public static string CreateSqlScript(List<PaymentStatus> listStatusInfo)
        {
            try
            {
                if (listStatusInfo == null || listStatusInfo.Count == 0)
                {
                    return string.Empty;
                }
                ListStatusInfo = listStatusInfo;
                dbTypeString = DbHelper.GetDBTypeString();

                return BuildSqlScript();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string BuildSqlScript()
        {
            sqlBuilder = new StringBuilder();

            foreach (PaymentStatus statusInfo in ListStatusInfo)
            {
                status = statusInfo;

                if (statusInfo.Source == BankReportType.PAYMUL)
                {
                    BuildInsertScript();
                }
                else
                {
                    BuildUpdateScript();
                }
            }
            Regex regex = new Regex(@";\b");
            string sqlStr = regex.Replace(sqlBuilder.ToString(), m => { return ";\n"; });
            if (string.IsNullOrEmpty(sqlStr))
            {
                return string.Empty;
            }
            return string.Format("BEGIN\n{0}\nEND;", sqlStr);
        }

        private static void BuildInsertScript()
        {
            switch (dbTypeString)
            {
                case ConstantFields.DB_PROVIDER_ORACLE:
                    sqlBuilder.Append(string.Format(SCRIPT_ORACLE_INSERT,
                        status.MessageID, status.RefID, status.BankRef, status.BankCode,
                        status.LineSpan, status.Source, status.OurStatusCode, status.BankStatusCode, status.BankStatusDesc));
                    break;
                case ConstantFields.DB_PROVIDER_SQLSERVER:
                    sqlBuilder.Append(string.Format(SCRIPT_SQLSERVER_INSERT,
                        status.MessageID, status.RefID, status.BankRef, status.BankCode,
                        status.LineSpan, status.Source, status.OurStatusCode, status.BankStatusCode, status.BankStatusDesc));
                    break;
                default:
                    break;
            }
            sqlBuilder.Append(";");
        }

        private static void BuildUpdateScript()
        {
            switch (dbTypeString)
            {
                case ConstantFields.DB_PROVIDER_ORACLE:
                    BuildOracleUpdateScript();
                    break;
                case ConstantFields.DB_PROVIDER_SQLSERVER:
                    BuildSqlServerUpdateScript();
                    break;
                default:
                    break;
            }
        }

        private static void BuildOracleUpdateScript()
        {
            if (!string.IsNullOrEmpty(status.MessageID) || !string.IsNullOrEmpty(status.RefID))
            {
                sqlBuilder.Append(string.Format(SCRIPT_ORACLE_UPDATE, status.OurStatusCode, status.BankStatusCode, status.BankStatusDesc, status.Source));
                if (!string.IsNullOrEmpty(status.MessageID))
                {
                    sqlBuilder.Append(string.Format(" AND FileReference = '{0}'", status.MessageID));
                }
                if (!string.IsNullOrEmpty(status.RefID))
                {
                    sqlBuilder.Append(string.Format(" AND CustomReference='{0}'", status.RefID));
                }
                if (!string.IsNullOrEmpty(status.LineSpan))
                {
                    //oracel 10g at least
                    sqlBuilder.Append(string.Format(@" AND to_number(REGEXP_SUBSTR(LINESPAN,'^\d*[^-]'))<={0}", status.LineSpan));
                    sqlBuilder.Append(string.Format(@" AND to_number(REGEXP_SUBSTR(LINESPAN,'[^-]\d*$'))>={0}", status.LineSpan));
                }
                sqlBuilder.Append(";");
            }
            else
            {
                throw new Exception("Both FileReference and CustomReference are empty, please check.");
            }
        }

        private static void BuildSqlServerUpdateScript()
        {
            if (!string.IsNullOrEmpty(status.MessageID) || !string.IsNullOrEmpty(status.RefID))
            {
                sqlBuilder.Append(string.Format(SCRIPT_SQLSERVER_UPDATE, status.OurStatusCode, status.BankStatusCode, status.BankStatusDesc, status.Source));
                if (!string.IsNullOrEmpty(status.MessageID))
                {
                    sqlBuilder.Append(string.Format(" AND FileReference = '{0}'", status.MessageID));
                }
                if (!string.IsNullOrEmpty(status.RefID))
                {
                    sqlBuilder.Append(string.Format(" AND CustomReference='{0}'", status.RefID));
                }
                if (!string.IsNullOrEmpty(status.LineSpan))
                {
                    sqlBuilder.Append(string.Format(" AND CONVERT(int,SUBSTRING(LineSpan,1,CHARINDEX('-',LineSpan,1)-1))<={0}", status.LineSpan));
                    sqlBuilder.Append(string.Format(" AND CONVERT(int,SUBSTRING(LineSpan,CHARINDEX('-',LineSpan,1)+1,LEN(LineSpan)))>={0}", status.LineSpan));
                }
                sqlBuilder.Append(";");
            }
            else
            {
                throw new Exception("Both FileReference and CustomReference are empty, please check.");
            }
        }
    }
}
