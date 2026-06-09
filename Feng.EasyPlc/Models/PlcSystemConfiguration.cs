namespace Feng.EasyPlc.Models;

public class PlcSystemConfiguration
{
    public List<PlcDeviceConfiguration> DeviceConfigurations { get; set; } = new List<PlcDeviceConfiguration>();
    public List<PlcPointConfiguration> PointConfigurations { get; set; } = new List<PlcPointConfiguration>();
    public List<PlcAxisConfiguration> AxisConfigurations { get; set; } = new List<PlcAxisConfiguration>();
}
