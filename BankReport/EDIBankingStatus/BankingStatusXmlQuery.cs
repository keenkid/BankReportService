using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EdiBankingStatus
{
    public static class BankingStatusXmlQuery
    {
        private const string NODE_STATUS = "status";

        private const string ATTRIBUTE_CODE = "code";

        private const string ATTRIBUTE_DESC = "desc";

        private const string ATTRIBUTE_RESULT = "result";

        private const string ATTRIBUTE_SOURCE = "source";

        private const string ATTRIBUTE_BANK = "bank";

        private static string GetXmlFileFullPath(string xmlFileName)
        {
            if (string.IsNullOrEmpty(xmlFileName))
            {
                return string.Empty;
            }
            try
            {
                string xmlFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), xmlFileName);
                if (!File.Exists(xmlFilePath))
                {
                    throw new Exception(string.Format("There is no indicated XML FILE [{0}]", xmlFilePath));
                }
                return xmlFilePath;
            }
            catch
            {
                throw;
            }
        }

        public static void ActionCodeDescritionQuery(PaymentStatus statusInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(statusInfo.BankStatusCode))
                {
                    throw new Exception("This EDI report is not a valid report.");
                }

                XDocument xmlDoc = XDocument.Parse(Res.ActionCode);

                var rslt = (from item in xmlDoc.Descendants(NODE_STATUS)
                            where item.Attribute(ATTRIBUTE_CODE).Value == statusInfo.BankStatusCode
                            select new { Description = item.Attribute(ATTRIBUTE_DESC).Value, StatusCode = item.Attribute(ATTRIBUTE_RESULT).Value }
                    ).First();

                statusInfo.OurStatusCode = rslt.StatusCode.SqlInjectionPrevent();
                statusInfo.BankStatusDesc = rslt.Description.SqlInjectionPrevent();
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    throw new Exception(string.Format("Please contact your supplier, \r\nThere is no XML NODE for Bank:{0},Report:{1} and bank status code:{2}.",
                        statusInfo.BankCode, statusInfo.Source, statusInfo.BankStatusCode));
                }
                throw;
            }
        }

        public static void StatusCodeAndDescriptionQuery(PaymentStatus statusInfo)
        {
            try
            {
                if (statusInfo == null || string.IsNullOrEmpty(statusInfo.BankStatusCode))
                {
                    throw new Exception("This EDI report is not a valid report.");
                }

                XDocument xmlDoc = XDocument.Parse(Res.BankingStatusCodeList);

                var rslt = (
                    from item in xmlDoc.Descendants(NODE_STATUS)
                    where 1 == 1
                    && string.Compare(item.Attribute(ATTRIBUTE_BANK).Value, statusInfo.BankCode, true) == 0
                    && string.Compare(item.Attribute(ATTRIBUTE_CODE).Value, statusInfo.BankStatusCode, true) == 0
                    && item.Attribute(ATTRIBUTE_SOURCE).Value.ToUpper().Contains(statusInfo.Source.ToString().ToUpper())
                    select
                    new { Description = item.Attribute(ATTRIBUTE_DESC).Value, StatusCode = item.Attribute(ATTRIBUTE_RESULT).Value }
                    ).First();

                statusInfo.OurStatusCode = rslt.StatusCode.SqlInjectionPrevent();
                statusInfo.BankStatusDesc = rslt.Description.SqlInjectionPrevent();
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    throw new Exception(string.Format("Please contact your supplier, \r\nThere is no XML NODE for Bank:{0},Report:{1} and bank status code:{2}.",
                        statusInfo.BankCode, statusInfo.Source, statusInfo.BankStatusCode));
                }
                throw;
            }
        }
    }
}
