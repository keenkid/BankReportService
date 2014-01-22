using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankReportMonitor;
using System.IO;
using EdiBankingStatus;
using System.Text.RegularExpressions;
using BankResponseParser;
using BankReport.Common;
using BankReport.OutputEntity;

namespace EDIReportTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MonitorTest(args);
            //BankResponseTest();

            BankBalance bal = new BankBalance() 
            {
                BankCode = "51",
                TransCode = "1001",
                Balance = "10001.01",
                BalanceDate = "20140121",
                CurrType = "USD",
                UsableBalance = "10001.02"
            };
            var val = bal.ToString();
        }

        private static void MonitorTest(string[] args)
        {
            ServiceHostManager manager = new ServiceHostManager();

            manager.OnStart(args);

            Console.ReadKey(true);

            manager.OnStop();
        }

        private static void BankResponseTest()
        {
            List<AbstractBankResponse> lbb = new List<AbstractBankResponse>();            
            lbb.Add(new BankBalance());            
            lbb.Add(new BankStatement());

            BuildBankResponse response = new BuildBankResponse(lbb);
            
            var content = response.BalanceContent;
            content = response.StatementContent;
            response.SaveAsFile();
        }
    }
}
