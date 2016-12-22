using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace K2Field.K2NE.ServiceBroker.Helpers.PowerShell
{
    [DataContract]
    public class PowerShellVariablesDC
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "value")]
        public object Value { get; set; }

        public PowerShellVariablesDC(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
