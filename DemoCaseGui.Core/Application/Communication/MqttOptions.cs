namespace DemoCaseGui.Core.Application.Communication;
public class MqttOptions
{
    public int CommunicationTimeout { get; set; }
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public int KeepAliveInterval { get; set; }
}
