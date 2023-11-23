using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCaseGui.Core.Application.Models
{
    public class FilterEntry
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
        public FilterEntry(string name, DateTime time, string value)
        {
            Name = name;
            Time = time;
            Value = value;
        }
    }
}
