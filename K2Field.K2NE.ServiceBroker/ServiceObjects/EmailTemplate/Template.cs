using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using SourceCode.SmartObjects.Client;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.EmailTemplate
{
    /// <summary>
    /// The Class is used to work with EmailTemplates. 
    /// </summary>
    public class Template
    {
        #region Public Properties
        //This hardcoded value is used to avoid endless loop when Templates refer to each other by some mistake!
        private const int maxDepth = 3;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Placeholders;
        public List<Template> TemplatePlaceholders;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lng"></param>
        /// <param name="currentDepth"></param>
        /// <param name="smoSrv"></param>
        public Template(string name, string lng, int currentDepth, SmoClientHelper smoSrv)
        {
            Name = name;
            Language = lng;
            GetSubjectBody(smoSrv);
            currentDepth++;
            if (currentDepth <= maxDepth)
            {
                Placeholders = new List<string>();
                TemplatePlaceholders = new List<Template>();
                GetPlaceHolders(Subject, ref Placeholders);
                GetPlaceHolders(Body, ref Placeholders);
                TemplatePlaceholders = GetTemplatePlaceholders(Placeholders, currentDepth, smoSrv);
                foreach (Template t in TemplatePlaceholders)
                {
                    if (!String.IsNullOrEmpty(t.Subject))
                    {

                        Subject = ReplaceTemplatePlaceholder(Subject, t.Name, t.Subject);
                    }
                    if (!String.IsNullOrEmpty(t.Body))
                    {
                        Body = ReplaceTemplatePlaceholder(Body, t.Name, t.Body);
                    }
                }
            }
        }
        /// <summary>
        /// Constructor when subject and body are already known.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lng"></param>
        /// <param name="subj"></param>
        /// <param name="body"></param>
        /// <param name="currentDepth"></param>
        /// <param name="smoSrv"></param>
        public Template(int id, string name, string lng, string subj, string body, int currentDepth, SmoClientHelper smoSrv)
        {
            Id = id;
            Name = name;
            Language = lng;
            Subject = subj;
            Body = body;
            currentDepth++;
            if (currentDepth <= maxDepth)
            {
                Placeholders = new List<string>();
                TemplatePlaceholders = new List<Template>();
                GetPlaceHolders(Subject, ref Placeholders);
                GetPlaceHolders(Body, ref Placeholders);
                TemplatePlaceholders = GetTemplatePlaceholders(Placeholders, currentDepth, smoSrv);
                foreach (Template t in TemplatePlaceholders)
                {
                    if (!String.IsNullOrEmpty(t.Subject))
                    {

                        Subject = ReplaceTemplatePlaceholder(Subject, t.Name, t.Subject);
                    }
                    if (!String.IsNullOrEmpty(t.Body))
                    {
                        Body = ReplaceTemplatePlaceholder(Body, t.Name, t.Body);
                    }
                }
            }
        }

        ///// <summary>
        ///// Sets the public properties Subject and Body
        ///// </summary>
        ///// <param name="smoServer">SmartObjectClientServer with connection</param>
        private void GetSubjectBody(SmoClientHelper smoServer)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendFormat(
                    "SELECT TOP 1 [Id], [EmailSubject], [EmailBody], [TemplateLanguage], [TemplateName] FROM K2Field_K2NE_SMO_EmailTemplate WHERE [TemplateLanguage] = '{0}' AND [TemplateName] = '{1}'",
                    Language, Name);

            DataTable dTable = smoServer.ExecuteSQLQueryDataTable(sqlQuery.ToString());
            if (dTable.Rows.Count == 0) return;
            Subject = dTable.Rows[0]["EmailSubject"].ToString();
            Body = dTable.Rows[0]["EmailBody"].ToString();
        }
        /// <summary>
        /// Analyzes the string and returns all %placeholders%
        /// </summary>
        /// <param name="searchString">String for search</param>
        /// <param name="pHolders"></param>
        private static void GetPlaceHolders(string searchString, ref List<string>pHolders)
        {
            if (String.IsNullOrEmpty(searchString))return;
            MatchCollection matches = Regex.Matches(searchString, "%[^%]+%");
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    foreach (Capture cap in m.Captures)
                    {
                        string tName = cap.Value.Replace("%", "");
                        if (!pHolders.Contains(tName))
                        {
                            pHolders.Add(tName);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns the list of placeholders, which reference to other templates.
        /// </summary>
        /// <param name="pHolders"></param>
        /// <param name="smoSrv"></param>
        /// <returns></returns>
        private List<Template> GetTemplatePlaceholders(IEnumerable<string> pHolders, int depth, SmoClientHelper smoSrv)
        {
            List<Template> tList = new List<Template>();
            foreach (var pHolder in pHolders)
            {
                if (pHolder.StartsWith("Template."))
                {
                    Template t = new Template(pHolder.Replace("Template.", ""), Language, depth, smoSrv);
                    tList.Add(t);
                }
            }
            return tList;
        }
        /// <summary>
        /// Helper method to replace TemplatePlaceholders with Values
        /// </summary>
        /// <param name="searchString">Text in which we replace</param>
        /// <param name="pHolder">Placeholder which we search</param>
        /// <param name="replaceValue">Value for replacement</param>
        /// <returns></returns>
        private static string ReplaceTemplatePlaceholder(string searchString, string pHolder, string replaceValue)
        {
            pHolder = "%Template." + pHolder + "%";
            return searchString.Replace(pHolder, replaceValue);
        }
    }
}
