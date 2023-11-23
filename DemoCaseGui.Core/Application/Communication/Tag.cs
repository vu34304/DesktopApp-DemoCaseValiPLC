namespace DemoCaseGui.Core.Application.Communication;
public class Tag
{
    public string name { get; set; }
    public string dbname { get; set; }
    public object? value { get; set; }
    public string address { get; set; }
    public DateTime timestamp { get; set; } = DateTime.Now;

    public Tag(string _name, string _dbname, object? _value, string _address, DateTime _timestamp)
    {
        name = _name;
        dbname = _dbname;
        value = _value;
        address = _address;
        timestamp = _timestamp;
    }
}
