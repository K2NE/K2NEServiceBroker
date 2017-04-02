using System;
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
        private static string _defaultWorkflowServerConnectionString;
        private static int _workflowServerPort;
        private readonly string _sessionConnectionString;
        private ConnectionSetup _sessionWorkflowConnectionSetup;

        public K2Connection(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            // These values are static because they won't change on the server.
            // This code will only execute once after K2HostServer starts
            if (string.IsNullOrEmpty(_defaultWorkflowServerConnectionString) ||
                _workflowServerPort == 0)
            {
                _defaultWorkflowServerConnectionString = ConfigurationManager.ConnectionStrings["WorkflowServer"].ConnectionString;

                var workflowConnectionSetup = new ConnectionSetup();
                workflowConnectionSetup.ParseConnectionString(_defaultWorkflowServerConnectionString);

                _workflowServerPort = int.Parse(workflowConnectionSetup.ConnectionParameters[SourceCode.Workflow.Client.ConnectionSetup.ParamKeys.Port]);
            }

            SessionManager = serverMarshaling.GetSessionManagerContext();

            var sessionCookie = SessionManager.CurrentSessionCookie;
            _sessionConnectionString = K2NEServiceBroker.SecurityManager.GetSessionConnectionString(sessionCookie);
        }

        public string SessionConnectionString
        {
            get
            {
                return _sessionConnectionString;
            }
        }

        public ISessionManager SessionManager { get; }

        public string UserName { get; set; }

        private ConnectionSetup SessionWorkflowConnectionSetup
        {
            get
            {
                if (_sessionWorkflowConnectionSetup == null &&
                    !string.IsNullOrEmpty(this.SessionConnectionString) &&
                    _workflowServerPort != 0)
                {
                    _sessionWorkflowConnectionSetup = new ConnectionSetup();
                    _sessionWorkflowConnectionSetup.ParseConnectionString(this.SessionConnectionString);
                    _sessionWorkflowConnectionSetup.ConnectionParameters[SourceCode.Workflow.Client.ConnectionSetup.ParamKeys.Port] = _workflowServerPort.ToString();
                    _sessionWorkflowConnectionSetup.ConnectionParameters.Remove(SourceCode.Workflow.Client.ConnectionSetup.ParamKeys.ConnectionString);
                }

                return _sessionWorkflowConnectionSetup;
            }
        }

        public T GetConnection<T>() where T : BaseAPI, new()
        {
            var server = new T();
            server.CreateConnection();
            server.Connection.Open(this.SessionConnectionString);

            //if we should execute as the current user, always make sure we apply the user's credential to the session.
            //Think of PTA, our connection string might be integrated but the connection will be created as the Service Account.
            server.Connection.ImpersonateSessionUser(this);

            return server;
        }

        public Connection GetWorkflowClientConnection()
        {
            var connection = new Connection();

            try
            {
                connection.Open(this.SessionWorkflowConnectionSetup);

                if (!UserName.Equals(connection?.User?.FQN, StringComparison.InvariantCultureIgnoreCase))
                {
                    connection.ImpersonateUser(UserName);
                }

                return connection;
            }
            catch
            {
                connection?.Dispose();
                throw;
            }
        }
    }
}