﻿using System;
using System.Configuration;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.Workflow.Client;

namespace K2Field.K2NE.ServiceBroker
{
    /// <summary>
    /// K2Connection
    /// </summary>
    internal class K2Connection
    {
        private static string defaultWorkflowServerConnectionString;
        private static int workflowServerPort;
        private readonly string sessionConnectionString;
        private ISessionManager sessionManager;
        private ConnectionSetup sessionWorkflowConnectionSetup;

        public K2Connection(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            // These values are static because they won't change on the server.
            // This code will only execute once after K2HostServer starts
            if (string.IsNullOrEmpty(defaultWorkflowServerConnectionString) || workflowServerPort == 0)
            {
                defaultWorkflowServerConnectionString = ConfigurationManager.ConnectionStrings["WorkflowServer"].ConnectionString;

                ConnectionSetup workflowConnectionSetup = new ConnectionSetup();
                workflowConnectionSetup.ParseConnectionString(defaultWorkflowServerConnectionString);

                workflowServerPort = int.Parse(workflowConnectionSetup.ConnectionParameters[SourceCode.Workflow.Client.ConnectionSetup.ParamKeys.Port]);
            }

            sessionManager = serverMarshaling.GetSessionManagerContext();

            var sessionCookie = SessionManager.CurrentSessionCookie;
            sessionConnectionString = K2NEServiceBroker.SecurityManager.GetSessionConnectionString(sessionCookie);
        }

        public string SessionConnectionString
        {
            get
            {
                return sessionConnectionString;
            }
        }

        public ISessionManager SessionManager
        {
            get
            {
                return sessionManager;
            }
        }

        public string UserName { get; set; }

        private ConnectionSetup SessionWorkflowConnectionSetup
        {
            get
            {
                if (sessionWorkflowConnectionSetup == null && !string.IsNullOrEmpty(this.SessionConnectionString) && workflowServerPort != 0)
                {
                    sessionWorkflowConnectionSetup = new ConnectionSetup();
                    sessionWorkflowConnectionSetup.ParseConnectionString(this.SessionConnectionString);
                    sessionWorkflowConnectionSetup.ConnectionParameters[SourceCode.Workflow.Client.ConnectionSetup.ParamKeys.Port] = workflowServerPort.ToString();
                    sessionWorkflowConnectionSetup.ConnectionParameters.Remove(SourceCode.Workflow.Client.ConnectionSetup.ParamKeys.ConnectionString);
                }

                return sessionWorkflowConnectionSetup;
            }
        }

        public T GetConnection<T>() where T : BaseAPI, new()
        {
            var server = new T();
            server.CreateConnection();
            server.Connection.Open(this.SessionConnectionString);

            return server;
        }

        public Connection GetWorkflowClientConnection()
        {
            Connection connection = new Connection();

            try
            {
                connection.Open(this.SessionWorkflowConnectionSetup);
                connection.User.ThrowIfNull("connection.User");
                return connection;
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    connection.Dispose();
                }

                throw new Exception("Failed to create Connection to K2.", ex);
            }
        }
    }
}