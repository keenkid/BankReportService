using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data.SqlClient;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public static class DbHelper
    {
        private const string SOURCE_NAME = "currentDB";

        private static Configuration config = ConfigurationManager.OpenExeConfiguration(typeof(DbDataAdapterHelper).Assembly.Location);

        private static ConnectionStringSettings settings = config.ConnectionStrings.ConnectionStrings[SOURCE_NAME];

        private const string BANKING_STATUS_SKELETON_SELECT = "SELECT * FROM SGEDIBankingStatus WHERE 1=2";

        private static readonly string connString = settings.ConnectionString;

        private static List<PaymentStatus> listStatusInfo = null;

        public static string GetDBTypeString()
        {
            try
            {
                string connStringName = ConfigurationManager.AppSettings[SOURCE_NAME];
                if (string.IsNullOrEmpty(connStringName))
                {
                    throw new Exception(string.Format("Indicated database source name [{0}] not exists in appSettings", SOURCE_NAME));
                }
                return ConfigurationManager.ConnectionStrings[connStringName].ProviderName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ExecuteSqlScript(string sqlScript)
        {
            if (string.IsNullOrEmpty(sqlScript))
            {
                return 0;
            }
            try
            {
                DbConnection conn = CreateDbConnection();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlScript;
                conn.Open();
                int affectedLine = cmd.ExecuteNonQuery();
                conn.Close();
                return affectedLine;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DbConnection CreateDbConnection()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[SOURCE_NAME]].ConnectionString;
                string dbType = GetDBTypeString();
                switch (dbType)
                {
                    case ConstantFields.DB_PROVIDER_ORACLE:
                        return new OracleConnection(connStr);
                    case ConstantFields.DB_PROVIDER_SQLSERVER:
                        return new SqlConnection(connStr);
                    default:
                        throw new Exception(string.Format("Not supported database type: {0}", dbType));
                }
            }
            catch
            {
                throw;
            }
        }

        public static int ExecuteDataTableInsert(List<PaymentStatus> listPaymentStatus)
        {
            try
            {
                listStatusInfo = listPaymentStatus;
                if (listStatusInfo == null || listStatusInfo.Count == 0)
                {
                    return 0;
                }
                return UpdateDataTable();
            }
            catch
            {
                throw;
            }
        }

        private static int UpdateDataTable()
        {
            try
            {
                string dbType = GetDBTypeString();

                switch (dbType)
                {
                    case ConstantFields.DB_PROVIDER_ORACLE:
                        return UpdateByOracleDataAdapter();
                    case ConstantFields.DB_PROVIDER_SQLSERVER:
                        return UpdateBySqlDataAdapter();
                    default:
                        throw new Exception(string.Format("Not supported database type: {0}", dbType));
                }
            }
            catch
            {
                throw;
            }
        }

        private static int UpdateBySqlDataAdapter()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(BANKING_STATUS_SKELETON_SELECT, connString);
                adapter.InsertCommand = BuildSqlInsertCommand();
                adapter.InsertCommand.Connection = new SqlConnection(connString);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                BuildUpdateDataTable(dt);

                return adapter.Update(dt);
            }
            catch
            {
                throw;
            }
        }

        private static int UpdateByOracleDataAdapter()
        {
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(BANKING_STATUS_SKELETON_SELECT, connString);
                adapter.InsertCommand = BuildOracleInsertCommand();
                adapter.InsertCommand.Connection = new OracleConnection(connString);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                BuildUpdateDataTable(dt);

                return adapter.Update(dt);
            }
            catch
            {
                throw;
            }
        }

        private static SqlCommand BuildSqlInsertCommand()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO SGEDIBankingStatus");
            sql.Append("(FileReference,CustomReference,BankReference,Bank,LineSpan,");
            sql.Append("Source,OurStatusCode,BankStatusCode,BankStatusDesc)");
            sql.Append(" VALUES(@FileReference,@CustomReference,@BankReference,@Bank,@LineSpan,");
            sql.Append("@Source,@OurStatusCode,@BankStatusCode,@BankStatusDesc)");

            SqlCommand comm = new SqlCommand(sql.ToString());
            SqlParameter param = null;

            param = new SqlParameter("@FileReference", SqlDbType.NVarChar, 14, "FileReference");
            comm.Parameters.Add(param);
            param = new SqlParameter("@CustomReference", SqlDbType.NVarChar, 35, "CustomReference");
            comm.Parameters.Add(param);
            param = new SqlParameter("@BankReference", SqlDbType.NVarChar, 35, "BankReference");
            comm.Parameters.Add(param);
            param = new SqlParameter("@Bank", SqlDbType.NVarChar, 4, "Bank");
            comm.Parameters.Add(param);
            param = new SqlParameter("@LineSpan", SqlDbType.NVarChar, 12, "LineSpan");
            comm.Parameters.Add(param);
            param = new SqlParameter("@Source", SqlDbType.NChar, 6, "Source");
            comm.Parameters.Add(param);
            param = new SqlParameter("@OurStatusCode", SqlDbType.NChar, 1, "OurStatusCode");
            comm.Parameters.Add(param);
            param = new SqlParameter("@BankStatusCode", SqlDbType.NVarChar, 3, "BankStatusCode");
            comm.Parameters.Add(param);
            param = new SqlParameter("@BankStatusDesc", SqlDbType.NVarChar, 255, "BankStatusDesc");
            comm.Parameters.Add(param);
            
            return comm;
        }

        private static OracleCommand BuildOracleInsertCommand()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO SGEDIBankingStatus");
            sql.Append("(FileReference,CustomReference,BankReference,Bank,LineSpan,");
            sql.Append("Source,OurStatusCode,BankStatusCode,BankStatusDesc)");
            sql.Append(" VALUES(:FileReference,:CustomReference,:BankReference,:Bank,:LineSpan,");
            sql.Append(":Source,:OurStatusCode,:BankStatusCode,:BankStatusDesc)");

            OracleCommand comm = new OracleCommand(sql.ToString());
            OracleParameter param = null;

            param = new OracleParameter(":FileReference", OracleDbType.NVarchar2, 14, "FileReference");
            comm.Parameters.Add(param);
            param = new OracleParameter(":CustomReference", OracleDbType.NVarchar2, 35, "RefID");
            comm.Parameters.Add(param);
            param = new OracleParameter(":BankReference", OracleDbType.NVarchar2, 35, "BankReference");
            comm.Parameters.Add(param);
            param = new OracleParameter(":Bank", OracleDbType.NVarchar2, 4, "Bank");
            comm.Parameters.Add(param);
            param = new OracleParameter(":LineSpan", OracleDbType.NVarchar2, 12, "LineSpan");
            comm.Parameters.Add(param);
            param = new OracleParameter(":Source", OracleDbType.NChar, 6, "Source");
            comm.Parameters.Add(param);
            param = new OracleParameter(":OurStatusCode", OracleDbType.NChar, 1, "OurStatusCode");
            comm.Parameters.Add(param);
            param = new OracleParameter(":BankStatusCode", OracleDbType.NVarchar2, 3, "BankStatusCode");
            comm.Parameters.Add(param);
            param = new OracleParameter(":BankStatusDesc", OracleDbType.NVarchar2, 255, "BankStatusDesc");
            comm.Parameters.Add(param);

            return comm;
        }

        private static void BuildUpdateDataTable(DataTable dt)
        {
            foreach (PaymentStatus statusInfo in listStatusInfo)
            {
                DataRow newRow = dt.NewRow();
                newRow["FileReference"] = statusInfo.MessageID;
                newRow["CustomReference"] = statusInfo.RefID;
                newRow["BankReference"] = statusInfo.BankRef;
                newRow["Bank"] = statusInfo.BankCode;
                newRow["LineSpan"] = statusInfo.LineSpan;
                newRow["Source"] = statusInfo.Source;
                newRow["OurStatusCode"] = statusInfo.OurStatusCode;
                newRow["BankStatusCode"] = statusInfo.BankStatusCode;
                newRow["BankStatusDesc"] = statusInfo.BankStatusDesc;
                dt.Rows.Add(newRow);
            }
        }
    }
}
