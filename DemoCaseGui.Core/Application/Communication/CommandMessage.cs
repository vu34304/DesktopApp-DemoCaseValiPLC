namespace DemoCaseGui.Core.Application.Communication;
public class CommandMessage 
{
    public string name { get; set; }
    public object value { get; set; }
    public DateTime timestamp { get; set; }
    public CommandMessage(string _name, object _value, DateTime _timestamp)
    {
        name = _name;
        value = _value;
        timestamp = _timestamp;
    }
}
