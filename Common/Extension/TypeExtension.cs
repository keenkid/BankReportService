using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Reflection;

namespace BankReport.Common
{
    public static class TypeExtension
    {
        public static Configuration GetAssemblyConfiguration(this Type tp)
        {
            if (null == tp)
            {
                return null;
            }
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(tp.Assembly.Location);
                if (config.HasFile)
                {
                    return config;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static string GetXmlStructre(this Type tp, object obj)
        {
            PropertyInfo[] props = tp.GetProperties();

            StringBuilder builder = new StringBuilder();

            StringBuilder head = new StringBuilder();
            StringBuilder body = new StringBuilder();

            foreach (PropertyInfo pi in props)
            {
                var attr = pi.GetCustomAttributes(typeof(XmlTagAttribute), false).FirstOrDefault() as XmlTagAttribute;
                if (attr.Level == XmlLevel.Head)
                {
                    head.AppendFormat("<{0}>{1}</{0}>", attr.Tag, pi.GetValue(obj, null));
                }
                else if (attr.Level == XmlLevel.Body)
                {
                    body.AppendFormat("<{0}>{1}</{0}>", attr.Tag, pi.GetValue(obj, null));
                }
            }
            builder.AppendFormat("<packet><head>{0}</head><body><list>{1}</list></body></packet>", head, body);

            return builder.ToString();
        }

        public static string GetDescription(this Type tp)
        {
            var attr = tp.GetCustomAttributes(typeof(MonitorAttribute), false).FirstOrDefault();
            if (null != attr)
            {
                //monitor = (attr as MonitorAttribute).SetAsMonitor;
                return (attr as MonitorAttribute).Description;
            }
            else
            {
                //monitor = false;
                return tp.Name;
            }
        }

        /// <exception cref="ArgumentException"/>        
        public static string GetCustomerAttributeProperyValue(this Type tp, string propertyName)
        {
            try
            {
                var attrs = tp.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    var props = attr.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        if (string.Equals(prop.Name, propertyName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return prop.GetValue(attr, null) as string;
                        }
                    }
                }
                throw new ArgumentException(string.Format("No property named [{0}] exist for Type [{1}] custom attribute", propertyName, tp.Name));
            }
            catch
            {
                throw;
            }
        }
    }
}
