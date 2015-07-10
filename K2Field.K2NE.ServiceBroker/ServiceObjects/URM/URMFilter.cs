using SourceCode.SmartObjects.Services.Service.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.URM
{
    public class URMFilter : FilterBase
    {
        private Dictionary<string, string> _filter;
        private string _propName;
        private int _pos;

        public string PropName
        {
            get
            {
                return _propName;
            }
            set
            {
                _propName = value;
            }
        }

        public URMFilter()
        {
            _filter = new Dictionary<string, string>();
        }

        public URMFilter(FilterExp filter)
            : base(filter)
        {
            this._filter = new Dictionary<string, string>();
        }

        public URMFilter(string whereclause)
            : base(whereclause)
        {
            this.UseValueFormat = false;
            this._filter = new Dictionary<string, string>();
            this.FilterFormat = "{0}";
            this.EqualFormat = "{0}|{1}";
            this.ContainsFormat = "{0}|*{1}*";
            this.OrFormat = "({0} or {1})";
            this.StartsWithFormat = "{0}|{1}*";
            this.PropertyFormat = "{0}";
            this.ValueFormat = "{0}";
            this.EndsWithFormat = "{0}|*{1}";
        }

        public URMFilter(string whereclause, IFormatProvider propertyFormatProvider)
            : base(whereclause)
        {
            this._filter = new Dictionary<string, string>();
            this.FilterFormat = "{0}";
            this.EqualFormat = "{0}|{1}";
            this.ContainsFormat = "{0}|*{1}*";
            this.OrFormat = "({0} or {1})";
            this.StartsWithFormat = "{0}|{1}*";
            this.PropertyFormat = "{0}";
            this.ValueFormat = "{0}";
            this.EndsWithFormat = "{0}|*{1}";
        }

        public override string visit(Contains contains)
        {
            string sToSplit = base.visit(contains);
            this.addtodic(sToSplit);
            return sToSplit;
        }

        public override string visit(EndsWith endswith)
        {
            string sToSplit = base.visit(endswith);
            this.addtodic(sToSplit);
            return sToSplit;
        }

        public override string visit(Equals equals)
        {
            string sToSplit = base.visit(equals);
            this.addtodic(sToSplit);
            return sToSplit;
        }

        public override string visit(StartsWith startswith)
        {
            string sToSplit = base.visit(startswith);
            this.addtodic(sToSplit);
            return sToSplit;
        }

        public Dictionary<string, Dictionary<string, string>> GetFilterCollection()
        {
            Dictionary<string, Dictionary<string, string>> retList = new Dictionary<string, Dictionary<string, string>>();
            this.GetFilters(this.Filter, ref retList);
            if (retList.Count == 0)
                retList.Add("dummy", new Dictionary<string, string>());
            return retList;
        }

        private void addtodic(string sToSplit)
        {
            string[] strArray = sToSplit.Split('|');
            if (this._filter.ContainsKey(strArray[0]))
            {
                return;
            }
            this._filter.Add(strArray[0], strArray[1]);
        }

        private void GetFilters(string xml, ref Dictionary<string, Dictionary<string, string>> retList)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            if (xmlDocument.FirstChild.ChildNodes.Count != 1)
            {
                foreach (XmlNode xmlNode in xmlDocument.FirstChild.ChildNodes)
                {
                    this.GetFilters(xmlNode.OuterXml, ref retList);
                }
            }
            else if (xmlDocument.FirstChild.InnerXml.Contains("<or>"))
            {
                this.GetFilters(xmlDocument.FirstChild.InnerXml, ref retList);
            }
            else
            {
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append("<filterexp>");
                stringBuilder1.Append(xmlDocument.FirstChild.InnerXml);
                stringBuilder1.Append("</filterexp>");
                URMFilter urmFilter = new URMFilter(stringBuilder1.ToString());
                urmFilter.ToString();
                if (xmlDocument.FirstChild.FirstChild.Name == "and")
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    foreach (KeyValuePair<string, string> keyValuePair in urmFilter._filter)
                    {
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                        stringBuilder2.Append(keyValuePair.Key);
                        stringBuilder2.Append(keyValuePair.Value);
                    }
                    if (retList.ContainsKey(stringBuilder2.ToString()))
                        return;
                    retList.Add(stringBuilder2.ToString(), dictionary);
                }
                else
                {
                    foreach (KeyValuePair<string, string> keyValuePair in urmFilter._filter)
                    {
                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                        StringBuilder stringBuilder2 = new StringBuilder();
                        stringBuilder2.Append(keyValuePair.Key);
                        stringBuilder2.Append(keyValuePair.Value);
                        if (!retList.ContainsKey(stringBuilder2.ToString()))
                            retList.Add(stringBuilder2.ToString(), dictionary);
                    }
                }
            }
        }
    }
}