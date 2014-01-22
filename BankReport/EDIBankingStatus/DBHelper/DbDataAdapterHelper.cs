using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using Oracle.DataAccess.Client;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public static class DbDataAdapterHelper
    {
        private const string SOURCE_NAME = "currentDB";

        private static Configuration config = ConfigurationManager.OpenExeConfiguration(typeof(DbDataAdapterHelper).Assembly.Location);

        private static string csname = config.AppSettings.Settings[SOURCE_NAME].Value;

        private static ConnectionStringSettings settings = config.ConnectionStrings.ConnectionStrings[csname];

        private static readonly string connectionStringName = settings.Name;

        private static PaymentStatus statusInfo = null;

        private static readonly string providerName = settings.ProviderName;

        private static readonly string connectionString = settings.ConnectionString;

        public static int UpdatePaymentStatusTable(List<PaymentStatus> listPaymentStatus)
        {
            if (listPaymentStatus == null || listPaymentStatus.Count == 0)
            {
                return 0;
            }
            int count = 0;
            foreach (PaymentStatus status in listPaymentStatus)
            {
                try
                {
                    statusInfo = status;

                    DbDataAdapter adapter = BuildDbDataAdapter();

                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    UpdateDataTable(dt);

                    count += adapter.Update(dt);
                }
                catch (Exception ex)
                {
                    Logger.sysLog.ErrorFormat("Database encounters error:\r\n{0}\r\nWhen operate data:\r\n{1}", ex.Message, status.ToString());
                }
            }
            return count;
        }

        private static void UpdateDataTable(DataTable dt)
        {
            if (statusInfo.Source == BankReportType.PAYMUL)
            {
                DataRow newRow = dt.NewRow();
                newRow["FileReference"] = statusInfo.MessageID;
                newRow["CustomReference"] = statusInfo.RefID;
                newRow["BankReference"] = statusInfo.BankRef;
                newRow["Bank"] = statusInfo.BankCode;
                newRow["LineSpan"] = statusInfo.LineSpan;
                newRow["Source"] = statusInfo.Source;
                newRow["InsertDate"] = DateTime.Now;
                newRow["UpdateDate"] = DateTime.Now;
                newRow["OurStatusCode"] = statusInfo.OurStatusCode;
                newRow["BankStatusCode"] = statusInfo.BankStatusCode;
                newRow["BankStatusDesc"] = statusInfo.BankStatusDesc.SubStr(255);
                dt.Rows.Add(newRow);
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["UpdateDate"] = DateTime.Now;
                    row["Source"] = statusInfo.Source;
                    row["OurStatusCode"] = statusInfo.OurStatusCode;
                    row["BankStatusCode"] = statusInfo.BankStatusCode;
                    row["BankStatusDesc"] = statusInfo.BankStatusDesc.SubStr(255);
                }
            }
        }

        private static DbDataAdapter BuildDbDataAdapter()
        {
            try
            {
                switch (providerName)
                {
                    case ConstantFields.DB_PROVIDER_SQLSERVER:
                        return BuildSqlDataAdapter();
                    case ConstantFields.DB_PROVIDER_ORACLE:
                        return BuildOracleDataAdapter();
                    default:
                        throw new Exception("Not supported database.");
                }
            }
            catch
            {
                throw;
            }
        }

        private static SqlDataAdapter BuildSqlDataAdapter()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                SqlDataAdapter adapter = null;
                if (statusInfo.Source == BankReportType.PAYMUL)
                {
                    adapter = new SqlDataAdapter(SelectScript4Insert(), conn);
                }
                else
                {
                    string sql = SelectScript4Update();
                    if (string.IsNullOrEmpty(sql) || !Regex.IsMatch(sql, "select", RegexOptions.IgnoreCase))
                    {
                        throw new Exception("Invalid Query statement.");
                    }
                    adapter = new SqlDataAdapter(sql, conn);
                }
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
                return adapter;
            }
            catch
            {
                throw;
            }
        }

        private static OracleDataAdapter BuildOracleDataAdapter()
        {
            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleDataAdapter adapter = null;
                if (statusInfo.Source == BankReportType.PAYMUL)
                {
                    adapter = new OracleDataAdapter(SelectScript4Insert(), conn);
                }
                else
                {
                    string sql = SelectScript4Update();
                    if (string.IsNullOrEmpty(sql) || !Regex.IsMatch(sql, "select", RegexOptions.IgnoreCase))
                    {
                        throw new Exception("Invalid Query statement.");
                    }
                    adapter = new OracleDataAdapter(SelectScript4Update(), conn);
                }
                OracleCommandBuilder cmdBuilder = new OracleCommandBuilder(adapter);
                return adapter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string SelectScript4Insert()
        {
            return "SELECT * FROM SGEDIBankingStatus WHERE 1 = 2";
        }

        private static string SelectScript4Update()
        {
            if (statusInfo == null || (string.IsNullOrEmpty(statusInfo.MessageID) && string.IsNullOrEmpty(statusInfo.RefID)))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM SGEDIBankingStatus WHERE 1 = 1");
            if (!string.IsNullOrEmpty(statusInfo.MessageID))
            {
                builder.Append(" AND FileReference='");
                builder.Append(statusInfo.MessageID);
                builder.Append("'");
            }
            if (!string.IsNullOrEmpty(statusInfo.RefID))
            {
                builder.Append(" AND CustomReference='");
                builder.Append(statusInfo.RefID);
                builder.Append("'");
            }
            if (!string.IsNullOrEmpty(statusInfo.LineSpan))
            {
                switch (providerName)
                {
                    case ConstantFields.DB_PROVIDER_SQLSERVER:
                        builder.Append(string.Format(" AND CONVERT(int,SUBSTRING(LineSpan,1,CHARINDEX('-',LineSpan,1)-1))<={0}", statusInfo.LineSpan));
                        builder.Append(string.Format(" AND CONVERT(int,SUBSTRING(LineSpan,CHARINDEX('-',LineSpan,1)+1,LEN(LineSpan)))>={0}", statusInfo.LineSpan));
                        break;
                    case ConstantFields.DB_PROVIDER_ORACLE:
                        builder.Append(string.Format(@" AND to_number(REGEXP_SUBSTR(LINESPAN,'^\d*[^-]'))<={0}", statusInfo.LineSpan));
                        builder.Append(string.Format(@" AND to_number(REGEXP_SUBSTR(LINESPAN,'[^-]\d*$'))>={0}", statusInfo.LineSpan));
                        break;
                    default:
                        throw new Exception("Not supported database.");
                }
            }
            //contrl can not update bansta
            if (statusInfo.Source == BankReportType.CONTRL)
            {
                builder.Append(string.Format(" AND Source IN ('{0}','{1}')", Enum.GetName(typeof(BankReportType), BankReportType.PAYMUL), Enum.GetName(typeof(BankReportType), BankReportType.CONTRL)));
            }

            return builder.ToString();
        }
    }
}
