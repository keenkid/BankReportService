using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace BankResponseParser.Configuration
{
    public sealed class TransactionTypeMap
    {       
        public static string GetDescription(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return string.Empty;
                }

                string desc = string.Empty;
                XDocument xdoc = XDocument.Parse(Res.TransactionType);
                try
                {
                    desc = (from elmt in xdoc.Descendants("transType")
                            where string.Equals(elmt.Attribute("code").Value, code, StringComparison.CurrentCultureIgnoreCase)
                            select elmt).FirstOrDefault().Attribute("desc").Value;
                }
                catch
                {
                    desc = code;
                }

                return PurifyDesc(desc);
            }
            catch
            {
                return code;
            }
        }

        private static string PurifyDesc(string desc)
        {
            if (string.IsNullOrEmpty(desc))
            {
                return string.Empty;
            }
            desc = desc.Replace("<", "");
            desc = desc.Replace(">", "");
            return desc;
        }
    }
}
