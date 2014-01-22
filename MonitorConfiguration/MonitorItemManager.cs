using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BankReport.Common;

namespace MonitorConfiguration
{
    public class MonitorItemManager
    {
        private const string SECTION_WATCH = "watch";

        private const string ELEMENT_DIR = "dir";

        private const string ATTRIBUTE_NAME = "name";

        private const string ATTRIBUTE_PATH = "path";

        private const string ATTRIBUTE_ACTION = "lastAction";

        private const string ATTRIBUTE_BACKUP = "lastPath";

        private const string ATTRIBUTE_TYPE = "type";

        private string _configFile = string.Empty;

        private XDocument _xdoc = null;

        public MonitorItemManager(string configFile)
        {
            _configFile = configFile;

            Initialize();
        }

        private void Initialize()
        {
            try
            {
                if (null == _xdoc)
                {
                    _xdoc = XDocument.Load(_configFile);
                }
            }
            catch
            {
                _xdoc = null;
            }
        }

        public void AddMonitorItem(MonitorData data)
        {
            if (null == _xdoc)
            {
                return;
            }
            XElement elmt = new XElement(ELEMENT_DIR);
            XAttribute attr = new XAttribute(ATTRIBUTE_NAME, data.Name);
            elmt.Add(attr);
            attr = new XAttribute(ATTRIBUTE_PATH, data.WatchPath);
            elmt.Add(attr);
            attr = new XAttribute(ATTRIBUTE_ACTION, data.NextAction);
            elmt.Add(attr);
            if (!string.IsNullOrEmpty(data.BackupPath))
            {
                attr = new XAttribute(ATTRIBUTE_BACKUP, data.BackupPath);
                elmt.Add(attr);
            }
            attr = new XAttribute(ATTRIBUTE_TYPE, data.ProcessType);
            elmt.Add(attr);

            _xdoc.Descendants(SECTION_WATCH).FirstOrDefault().Add(elmt);
            _xdoc.Save(_configFile);
        }

        public void DeleteMonitorItem(string watchName)
        {
            if (null == _xdoc)
            {
                return;
            }
            var nodes = (from node in _xdoc.Descendants(ELEMENT_DIR)
                         where string.Equals(node.Attribute(ATTRIBUTE_NAME).Value, watchName, StringComparison.CurrentCultureIgnoreCase)
                         select node).FirstOrDefault();
            if (null != nodes)
            {
                nodes.Remove();
            }
            _xdoc.Save(_configFile);
        }

        public List<MonitorData> GetMonitorItems()
        {
            List<MonitorData> datas = new List<MonitorData>();

            if (null == _xdoc)
            {
                return datas;
            }

            _xdoc.Descendants(ELEMENT_DIR).ToList().ForEach(elmt =>
            {
                datas.Add(new MonitorData()
                {
                    Name = elmt.Attribute(ATTRIBUTE_NAME).Value,
                    WatchPath = elmt.Attribute(ATTRIBUTE_PATH).Value,
                    NextAction = elmt.Attribute(ATTRIBUTE_ACTION).Value,
                    BackupPath = (null == elmt.Attribute(ATTRIBUTE_BACKUP)) ? string.Empty : elmt.Attribute(ATTRIBUTE_BACKUP).Value,
                    ProcessType = elmt.Attribute(ATTRIBUTE_TYPE).Value
                });
            });
            return datas;
        }

        public bool CheckMonitorItemExistByName(string val)
        {
            return CheckMonitorItemExisting(ATTRIBUTE_NAME, val);
        }

        public bool CheckMonitorItemExistByPath(string val)
        {
            return CheckMonitorItemExisting(ATTRIBUTE_PATH, val);
        }

        private bool CheckMonitorItemExisting(string name, string val)
        {
            if (null == _xdoc || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(val))
            {
                return false;
            }

            var elmts = (from elmt in _xdoc.Descendants(ELEMENT_DIR)
                         where string.Equals(elmt.Attribute(name).Value.ContentTrim(), val.ContentTrim(), StringComparison.CurrentCultureIgnoreCase)
                         select elmt);

            return elmts.Count() > 0 ? true : false;
        }
    }
}
