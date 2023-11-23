namespace DemoCaseGui.Core.Application.Exceptions;
public class MqttConnectionException : Exception
{
    public MqttConnectionException(string? message) : base(message)
    {
    }
}
