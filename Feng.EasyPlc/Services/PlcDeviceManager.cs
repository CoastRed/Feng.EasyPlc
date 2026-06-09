using Feng.EasyPlc.Contracts;
using Feng.EasyPlc.Models;
using Newtonsoft.Json;
using NLog;

namespace Feng.EasyPlc.Services;

public abstract class PlcDeviceManager: IPlcDeviceManager
{
    protected readonly ILogger? _logger;

    public Dictionary<PlcDeviceConfiguration, IPlcDevice> ConfigurationDeviceMap { get; protected set; } = new Dictionary<PlcDeviceConfiguration, IPlcDevice>();
    public PlcSystemConfiguration PlcSystemConfig { get; protected set; } = new PlcSystemConfiguration();

    public List<PlcDeviceConfiguration> PlcDeviceConfigurations => this.PlcSystemConfig.DeviceConfigurations;
    public List<PlcPointConfiguration> PlcPointConfigurations => this.PlcSystemConfig.PointConfigurations;
    public List<PlcAxisConfiguration> PlcAxisConfigurations => this.PlcSystemConfig.AxisConfigurations;

    #region 索引器

    public virtual IPlcDevice this[string plcName] => ConfigurationDeviceMap.First(x => x.Key.Name == plcName).Value;
    public virtual IPlcDevice this[PlcPointConfiguration pointConfiguration] => ConfigurationDeviceMap.First(x => x.Key.Name == pointConfiguration.PlcDeviceName).Value;
    public virtual IPlcDevice this[PlcAxisConfiguration axisConfiguration] => ConfigurationDeviceMap.First(x => x.Key.Name == axisConfiguration.PlcDeviceName).Value;

    #endregion

    protected PlcDeviceManager(ILogger? logger)
    {
        _logger = logger;
        this.PlcSystemConfig = this.LoadPlcSystemConfiguration();
        InitPLC();
        
    }

    public PlcDeviceManager(PlcSystemConfiguration config, ILogger? logger = null)
    {
        _logger = logger;
        this.PlcSystemConfig = config ?? throw new ArgumentNullException(nameof(config));
        InitPLC();
    }

    public virtual PlcSystemConfiguration LoadPlcSystemConfiguration()
    {
        string json = File.ReadAllText("PlcSystemConfiguration.json");
        var configuration = JsonConvert.DeserializeObject<PlcSystemConfiguration>(json) ?? new PlcSystemConfiguration();
        if (configuration == null)
        {
            throw new ArgumentException("Plc configuration is null.");
        }
        if (configuration.DeviceConfigurations.Any(s => string.IsNullOrWhiteSpace(s.Name) || string.IsNullOrWhiteSpace(s.IPAddress)))
        {
            throw new ArgumentException("PLC configuration must have a valid Name and IP.");
        }
        return configuration;

    }

    public virtual PlcPointConfiguration GetPlcPointConfiguration(string name)
    {
        return this.PlcPointConfigurations.Single(s => s.Name == name) ?? throw new ArgumentNullException(nameof(name), $"PlcPointConfiguration with name {name} not found.");
    }

    public virtual PlcAxisConfiguration GetPlcAxisConfiguration(string name)
    {
        return this.PlcAxisConfigurations.Single(s => s.Name == name) ?? throw new ArgumentNullException(nameof(name), $"AxisConfiguration with name {name} not found.");
    }

    public virtual PlcAxisConfiguration GetPlcAxisConfiguration(int axisNumber)
    {
        return this.PlcAxisConfigurations.Single(s => s.AxisNumber == axisNumber) ?? throw new ArgumentNullException(nameof(axisNumber), $"AxisConfiguration with AxisNumber {axisNumber} not found.");
    }


    public abstract void InitPLC();

}
