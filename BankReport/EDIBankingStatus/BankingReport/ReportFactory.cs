using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace EdiBankingStatus
{
    class ReportFactory
    {
        private static IEnumerable<string> asmNameArray = null;

        private static List<Type> bankingReportList = null;

        private static Dictionary<string, Type> cacheReportList = new Dictionary<string, Type>();

        public static AbstractBankingReport CreateBankingReport(string bankName, string reportTypeStr)
        {
            try
            {
                LoadAssemblies();

                LoadBankingReportClass();

                Type tp = GetBankingReportType(bankName, reportTypeStr);

                return Activator.CreateInstance(tp) as AbstractBankingReport;
            }
            catch
            {
                throw;
            }
        }

        private static void LoadAssemblies()
        {
            try
            {
                if (asmNameArray == null)
                {
                    string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                    asmNameArray = from name in Directory.GetFiles(currentFolder) where name.EndsWith(".dll") || name.EndsWith(".exe") select name;

                    if (asmNameArray.Count() == 0)
                    {
                        throw new Exception(string.Format("Current folder [{0}] does not exist any process program. From [{1}]", currentFolder, MethodInfo.GetCurrentMethod().Name));
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static void LoadBankingReportClass()
        {
            if (bankingReportList == null)
            {
                bankingReportList = new List<Type>();

                try
                {
                    asmNameArray.ToList().ForEach(asmName =>
                        {
                            bankingReportList.AddRange(
                                from tp in Assembly.LoadFrom(asmName).GetTypes() 
                                where !tp.IsAbstract && typeof(AbstractBankingReport).IsAssignableFrom(tp) 
                                select tp);
                        });
                }
                catch
                {
                    throw;
                }
            }
        }

        private static Type GetBankingReportType(string bankName, string reportTypeStr)
        {
            try
            {
                return cacheReportList[bankName + reportTypeStr];
            }
            catch
            {
                var report = (
                        from t in bankingReportList
                        where
                        ((BankAttribute)t.GetCustomAttributes(typeof(BankAttribute), false)[0]).BankName == bankName
                        &&
                        ((ReportTypeAttribute)t.GetCustomAttributes(typeof(ReportTypeAttribute), false)[0]).ReportName == reportTypeStr
                        select t).First();
                cacheReportList.Add(bankName + reportTypeStr, report);
                return report;
            }
        }
    }
}
