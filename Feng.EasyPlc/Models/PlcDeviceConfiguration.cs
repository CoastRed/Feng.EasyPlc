namespace Feng.EasyPlc.Models;

public enum PlcProtocol
{
    ModbusTcp,
}

public class PlcDeviceConfiguration
{
    public string Name { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public int TimeOut { get; set; } = 1000;
    public string Protocol { get; set; } = "HSL:ModbusTcp";
    public bool IsEnabled { get; set; } = true;
    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

}
