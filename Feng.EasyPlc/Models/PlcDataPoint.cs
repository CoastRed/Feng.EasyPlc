namespace Feng.EasyPlc.Models;

public class PlcDataPoint
{
    /// <summary>
    /// PLC名称
    /// </summary>
    public string PlcDeviceName { get; set; } = string.Empty;
    /// <summary>
    /// 要操作的PLC地址的名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 要操作的PLC地址
    /// </summary>
    public string Address { get; set; } = string.Empty;

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public override string ToString()
    {
        return $"{Name} ({PlcDeviceName}:{Address})";
    }
}
