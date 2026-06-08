namespace Feng.EasyPlc.Models;

public class PlcAxisConfiguration
{
    public string PlcDeviceName { get; set; } = string.Empty;
    public int AxisNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CurrentPositionAddress { get; set; }
    public string? JogAddress { get; set; }
    public string? OriginAddress { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public override string ToString()
    {
        return $"{Name} (轴{AxisNumber}, PLC:{PlcDeviceName})";
    }
}
