using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.Workflow.Client;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using MethodType = SourceCode.SmartObjects.Services.ServiceSDK.Types.MethodType;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.EmailTemplate
{
    public class EmailTemplateSO : ServiceObjectBase
    {
        public EmailTemplateSO(K2NEServiceBroker worklistAPI) : base(worklistAPI) { }

        public override string ServiceFolder
        {
            get
            {
                return "Email Template";
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("EmailTemplate", "ServiceObject that provides email template functionality.");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.EmailTemplate.EmailBody, SoType.Memo, "EmailBody"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.EmailTemplate.EmailSubject, SoType.Text, "EmailBody"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.EmailTemplate.TemplateLanguage, SoType.Text, "TemplateLanguage"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.EmailTemplate.TemplateName, SoType.Text, "TemplateName"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.EmailTemplate.EmailTemplateId, SoType.Number, "Email Template Id"));
            

            Method mGetEmailTemplate = Helper.CreateMethod(Constants.Methods.EmailTemplate.GetEmailTemplate, "Get Email Template by name.", MethodType.Read);
            mGetEmailTemplate.InputProperties.Add(Constants.SOProperties.EmailTemplate.TemplateName);
            mGetEmailTemplate.InputProperties.Add(Constants.SOProperties.EmailTemplate.TemplateLanguage);
            mGetEmailTemplate.Validation.RequiredProperties.Add(Constants.SOProperties.EmailTemplate.TemplateName);
            mGetEmailTemplate.Validation.RequiredProperties.Add(Constants.SOProperties.EmailTemplate.TemplateLanguage);
            mGetEmailTemplate.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.TemplateName);
            mGetEmailTemplate.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.TemplateLanguage);
            mGetEmailTemplate.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.EmailBody);
            mGetEmailTemplate.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.EmailSubject);
            mGetEmailTemplate.MethodParameters.Add(Helper.CreateParameter(Constants.SOProperties.EmailTemplate.ProcessInstanceId, SoType.Number, false, "Process Instance Id"));
            foreach (var param in GetCustomParamaters(EmailTemplateCustomParameters))
            {
                mGetEmailTemplate.MethodParameters.Add(param);
            }
            so.Methods.Add(mGetEmailTemplate);

            Method mListEmailTemplates = Helper.CreateMethod(Constants.Methods.EmailTemplate.ListEmailTemplates, "Lists Email Templats.", MethodType.List);
            mListEmailTemplates.InputProperties.Add(Constants.SOProperties.EmailTemplate.TemplateName);
            mListEmailTemplates.InputProperties.Add(Constants.SOProperties.EmailTemplate.TemplateLanguage);
            mListEmailTemplates.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.TemplateName);
            mListEmailTemplates.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.TemplateLanguage);
            mListEmailTemplates.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.EmailBody);
            mListEmailTemplates.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.EmailSubject);
            mListEmailTemplates.ReturnProperties.Add(Constants.SOProperties.EmailTemplate.EmailTemplateId);
            mListEmailTemplates.MethodParameters.Add(Helper.CreateParameter(Constants.SOProperties.EmailTemplate.ProcessInstanceId, SoType.Number, false, "Process Instance Id"));
            foreach (var param in GetCustomParamaters(EmailTemplateCustomParameters))
            {
                mListEmailTemplates.MethodParameters.Add(param);
            }
            so.Methods.Add(mListEmailTemplates);

            return new List<ServiceObject>() {so};
        }
        
        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.EmailTemplate.GetEmailTemplate:
                    GetEmailTemplate();
                    break;
                case Constants.Methods.EmailTemplate.ListEmailTemplates:
                    ListEmailTemplates();
                    break;
            }
        }
        private void GetEmailTemplate()
        {
            Dictionary<string, string> placeHolders = new Dictionary<string, string>();
            string name = GetStringProperty(Constants.SOProperties.EmailTemplate.TemplateName, true);
            string language = GetStringProperty(Constants.SOProperties.EmailTemplate.TemplateLanguage, true);
            int procId = GetIntParameter(Constants.SOProperties.EmailTemplate.ProcessInstanceId);
            //adding placeholders from the workflow
            if (procId > 0)
            {
                foreach (var wfProp in GetWorkflowProperties(procId))
                {
                    placeHolders.Add(wfProp.Key, wfProp.Value);
                }
            }
            //adding custom input parameters for change
            foreach (var param in GetCustomParamaters(EmailTemplateCustomParameters))
            {
                placeHolders.Add(param.Name, GetStringParameter(param.Name));
            }
            string newSubject, newBody;
            Template t;
            using (SmoClientHelper smoSrv = new SmoClientHelper(BaseAPIConnectionString))
            {
                t = new Template(name, language, 0, smoSrv);
            }
            newSubject = ReplacePlaceholders(t.Subject, placeHolders);
            newBody = ReplacePlaceholders(t.Body, placeHolders);
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            DataRow dRow = results.NewRow();
            dRow[Constants.SOProperties.EmailTemplate.EmailBody] = newBody;
            dRow[Constants.SOProperties.EmailTemplate.EmailSubject] = newSubject;
            dRow[Constants.SOProperties.EmailTemplate.TemplateLanguage] = language;
            dRow[Constants.SOProperties.EmailTemplate.TemplateName] = name;
            results.Rows.Add(dRow);
        }

        private void ListEmailTemplates()
        {
            Dictionary<string, string> placeHolders = new Dictionary<string, string>();
            string name = GetStringProperty(Constants.SOProperties.EmailTemplate.TemplateName);
            string language = GetStringProperty(Constants.SOProperties.EmailTemplate.TemplateLanguage);
            string sqlFilter = Convert.ToString(ServiceBroker.Service.ServiceObjects[0].Methods[0].SqlFilter);
            int procId = GetIntParameter(Constants.SOProperties.EmailTemplate.ProcessInstanceId);
            //adding placeholders from the workflow
            if (procId > 0)
            {
                foreach (var wfProp in GetWorkflowProperties(procId))
                {
                    placeHolders.Add(wfProp.Key, wfProp.Value);
                }
            }
            //adding custom input parameters for change
            foreach (var param in GetCustomParamaters(EmailTemplateCustomParameters))
            {
                placeHolders.Add(param.Name, GetStringParameter(param.Name));
            }
            //Building a query to get a list of Templates
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append(
                "SELECT [Id], [EmailSubject], [EmailBody], [TemplateLanguage], [TemplateName] FROM K2Field_K2NE_SMO_EmailTemplate WHERE [Id] IS NOT NULL");
            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(language) || String.IsNullOrEmpty(sqlFilter))
            {
                if (!String.IsNullOrEmpty(name))
                {
                    sqlQuery.AppendFormat(" AND [TemplateName] = '{0}'", name);
                }
                if (!String.IsNullOrEmpty(language))
                {
                    sqlQuery.AppendFormat(" AND [TemplateLanguage] = '{0}'", language);
                }
                if (!String.IsNullOrEmpty(sqlFilter))
                {
                    sqlQuery.AppendFormat(" AND {0}", sqlFilter);
                }
            }
            List<Template> tList = new List<Template>();
            using (SmoClientHelper smoSrv = new SmoClientHelper(BaseAPIConnectionString))
            {
                DataTable dTable = smoSrv.ExecuteSQLQueryDataTable(sqlQuery.ToString());

                foreach (DataRow dRow in dTable.Rows)
                {
                    int tId;
                    int.TryParse(Convert.ToString(dRow[Constants.SOProperties.EmailTemplate.EmailTemplateId]), out tId);
                    string tName = Convert.ToString(dRow[Constants.SOProperties.EmailTemplate.TemplateName]);
                    string tLanguage = Convert.ToString(dRow[Constants.SOProperties.EmailTemplate.TemplateLanguage]);
                    string tSubject = Convert.ToString(dRow[Constants.SOProperties.EmailTemplate.EmailSubject]);
                    string tBody = Convert.ToString(dRow[Constants.SOProperties.EmailTemplate.EmailBody]);

                    Template t = new Template(tId, tName, tLanguage, tSubject, tBody, 0, smoSrv);
                    tList.Add(t);
                }
            }
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            foreach (Template t in tList)
            {
                DataRow row = results.NewRow();
                row[Constants.SOProperties.EmailTemplate.EmailTemplateId] = t.Id;
                row[Constants.SOProperties.EmailTemplate.TemplateName] = t.Name;
                row[Constants.SOProperties.EmailTemplate.TemplateLanguage] = t.Language;
                row[Constants.SOProperties.EmailTemplate.EmailSubject] = ReplacePlaceholders(t.Subject, placeHolders);
                row[Constants.SOProperties.EmailTemplate.EmailBody] = ReplacePlaceholders(t.Body, placeHolders);
                results.Rows.Add(row);
            }
        }

        #region Helper Methods
        /// <summary>
        /// Returns the collection of parameters, which are created by splitting a delimited string
        /// </summary>
        /// <param name="customParameters"></param>
        /// <returns></returns>
        private static MethodParameters GetCustomParamaters(string customParameters)
        {
            MethodParameters paramCollection = new MethodParameters();
            if (String.IsNullOrEmpty(customParameters)) return paramCollection;
            foreach (var param in customParameters.Split(';'))
            {
                paramCollection.Add(Helper.CreateParameter(param, SoType.Text, false, param));
            }
            return paramCollection;
        }
        /// <summary>
        /// Returns the values for workflow specific placeholders
        /// </summary>
        /// <param name="processInstanceId">Process Instance Id</param>
        /// <returns></returns>
        private Dictionary<string, string> GetWorkflowProperties(int processInstanceId)
        {
            Dictionary<string, string > _workflowProperties = new Dictionary<string, string>();
            using (Connection k2Con = new Connection())
            {
                k2Con.Open(K2ClientConnectionSetup);

                ProcessInstance proc = k2Con.OpenProcessInstance(processInstanceId);
                if (proc == null) return _workflowProperties;

                _workflowProperties.Add(Constants.SOProperties.EmailTemplate.ProcessFolio, proc.Folio);
                _workflowProperties.Add(Constants.SOProperties.EmailTemplate.ProcessName, proc.Name);
                _workflowProperties.Add(Constants.SOProperties.EmailTemplate.ProcessStartDate,
                    proc.StartDate.ToString("yyyy-MM-dd HH:mm"));
                _workflowProperties.Add(Constants.SOProperties.EmailTemplate.ProcessOriginator,
                    proc.Originator.DisplayName);

                foreach (DataField dField in proc.DataFields)
                {
                    _workflowProperties.Add("DataField." + dField.Name, Convert.ToString(dField.Value));
                }
                k2Con.Close();
            }
            return _workflowProperties;
        }
        /// <summary>
        /// Replaces all placeholders inside the string with their values
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="pHolders"></param>
        /// <returns></returns>
        private string ReplacePlaceholders(string searchString, Dictionary<string, string> pHolders)
        {
            if (String.IsNullOrEmpty(searchString)) return String.Empty;
            foreach (var p in pHolders)
            {
                string pHolder = "%" + p.Key + "%";
                searchString = searchString.Replace(pHolder, p.Value);
            }
            return searchString;
        }
        #endregion
    }
}
