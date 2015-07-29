using SourceCode.SmartObjects.Services.Service.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.URM
{
    public class URMFilter : FilterBase
    {
        private Dictionary<string, string> filter;
        private string propertyName;


        public string PropName
        {
            get
            {
                return propertyName;
            }
            set
            {
                propertyName = value;
            }
        }

        public URMFilter()
        {
            filter = new Dictionary<string, string>();
        }

        public URMFilter(FilterExp filter)
            : base(filter)
        {
            this.filter = new Dictionary<string, string>();
        }

        public URMFilter(string whereclause)
            : base(whereclause)
        {
            this.UseValueFormat = false;
            this.filter = new Dictionary<string, string>();
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
            this.filter = new Dictionary<string, string>();
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
            if (this.filter.ContainsKey(strArray[0]))
            {
                return;
            }
            this.filter.Add(strArray[0], strArray[1]);
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
                StringBuilder filterExpression = new StringBuilder();
                filterExpression.Append("<filterexp>");
                filterExpression.Append(xmlDocument.FirstChild.InnerXml);
                filterExpression.Append("</filterexp>");
                URMFilter urmFilter = new URMFilter(filterExpression.ToString());

                if (string.Compare(xmlDocument.FirstChild.FirstChild.Name,"and") == 0)
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    foreach (KeyValuePair<string, string> fil in urmFilter.filter)
                    {
                        dictionary.Add(fil.Key, fil.Value);
                        stringBuilder2.Append(fil.Key);
                        stringBuilder2.Append(fil.Value);
                    }

                    if (retList.ContainsKey(stringBuilder2.ToString()))
                    {
                        return;
                    }
                    retList.Add(stringBuilder2.ToString(), dictionary);
                }
                else
                {
                    foreach (KeyValuePair<string, string> fil in urmFilter.filter)
                    {
                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        dictionary.Add(fil.Key, fil.Value);

                        StringBuilder stringBuilder2 = new StringBuilder();
                        stringBuilder2.Append(fil.Key);
                        stringBuilder2.Append(fil.Value);

                        if (!retList.ContainsKey(stringBuilder2.ToString()))
                        {
                            retList.Add(stringBuilder2.ToString(), dictionary);
                        }
                    }
                }
            }
        }
    }
}