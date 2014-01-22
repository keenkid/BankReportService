using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BankResponseParser.Configuration;
using BankReport.OutputEntity;

namespace BankResponseParser
{
    public class BuildBankResponse
    {
        private List<AbstractBankResponse> _labr;

        public BuildBankResponse(AbstractBankResponse abr)
            : this(new List<AbstractBankResponse>() { abr })
        {

        }

        public BuildBankResponse(List<AbstractBankResponse> labr)
        {
            _labr = labr;

            InitializeContent();
        }

        public string BalanceContent
        {
            private set;
            get;
        }

        public string StatementContent
        {
            private set;
            get;
        }

        private void InitializeContent()
        {
            StringBuilder balance = new StringBuilder();
            StringBuilder statement = new StringBuilder();

            _labr.ForEach(response => 
            {
                if (response is BankBalance)
                {
                    balance.Append(response);
                }
                if (response is BankStatement)
                {
                    statement.Append(response);
                }
            });

            BalanceContent = balance.ToString();

            StatementContent = statement.ToString();
        }

        public void SaveAsFile()
        {
            string sp = BankResponseSaveHandleSection.Instance.SaveParameter.SavePath;
        }
    }
}
