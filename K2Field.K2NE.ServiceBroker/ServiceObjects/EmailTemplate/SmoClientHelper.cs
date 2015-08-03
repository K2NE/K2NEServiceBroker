using System;
using SourceCode.SmartObjects.Client;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.EmailTemplate
{
    public class SmoClientHelper : SmartObjectClientServer, IDisposable
    {
        public SmoClientHelper(string con)
        {
            CreateConnection();
            Connection.Open(con);
        }
        
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
