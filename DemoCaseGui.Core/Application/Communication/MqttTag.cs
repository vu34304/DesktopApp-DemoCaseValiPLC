using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Communication
{
    public class MqttTag
    {
        public string name { get; set; }
        public object? value { get; set; }
        public DateTime timestamp { get; set; } = DateTime.Now;

        public MqttTag(string _name, object? _value, DateTime _timestamp)
        {
            name = _name;
            value = _value;
            timestamp = _timestamp;
        }
    }
}
