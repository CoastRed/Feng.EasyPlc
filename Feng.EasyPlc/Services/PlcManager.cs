using System.Text;
using Feng.EasyPlc.Contracts;
using Feng.EasyPlc.Extensions;
using Feng.EasyPlc.Models;
using Feng.EasyPlc.Protocol;
using Newtonsoft.Json;
using NLog;

namespace Feng.EasyPlc.Services;

public class PlcManager
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

    public PlcManager(ILogger? logger = null)
    {
        _logger = logger;
        this.PlcSystemConfig = this.LoadPlcSystemConfiguration();
        Initialize(PlcDeviceConfigurations);
    }

    public PlcManager(PlcSystemConfiguration config, ILogger? logger = null)
    {
        _logger = logger;
        this.PlcSystemConfig = config ?? throw new ArgumentNullException(nameof(config));
        Initialize(PlcDeviceConfigurations);
    }

    public void Initialize(List<PlcDeviceConfiguration> configurations)
    {
        foreach (var configuration in configurations)
        {
            ConfigurationDeviceMap.Add(configuration, InitPLC(configuration));
        }
    }

    protected virtual IPlcDevice InitPLC(PlcDeviceConfiguration configuration)
    {
        switch (configuration.Protocol)
        {
            case "HSL:ModbusTcp":
                return new HslPlcDevice(configuration, _logger);
            default:
                throw new NotSupportedException($"Protocol {configuration.Protocol} is not Supported.");
        }
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



    public virtual bool Connect(out string error)
    {
        error = string.Empty;
        foreach (var plc in ConfigurationDeviceMap.Values)
        {
            if (plc.Connect() == false)
            {
                error += $"Plc {plc.PlcName} ({plc.PlcIpAddress}) 连接失败";
            }
        }
        return string.IsNullOrEmpty(error);
    }

    public virtual async Task<(bool, string)> ConnectAsync()
    {
        string error = string.Empty;
        foreach (var plc in ConfigurationDeviceMap.Values)
        {
            if ((await plc.ConnectAsync()) == false)
            {
                error += $"Plc {plc.PlcName} ({plc.PlcIpAddress}) 连接失败";
            }
        }
        return (string.IsNullOrEmpty(error), error);
    }

    public virtual bool Disconnect()
    {
        foreach (var plc in ConfigurationDeviceMap.Values)
        {
            plc.DisConnect();
        }
        return true;
    }

    public virtual async Task<bool> DisconnectAsync()
    {
        foreach (var plc in ConfigurationDeviceMap.Values)
        {
            await plc.DisConnectAsync();
        }
        return true;
    }



    #region Read


    public virtual short? ReadInt16(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadInt16(point);
    }

    public virtual short? ReadInt16(PlcPointConfiguration point)
    {
        return this[point].ReadInt16(point.Address);
    }

    public virtual async Task<short?> ReadInt16Async(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadInt16Async(point);
    }

    public virtual async Task<short?> ReadInt16Async(PlcPointConfiguration point)
    {
        return await this[point].ReadInt16Async(point.Address);
    }




    public virtual ushort? ReadUInt16(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadUInt16(point);
    }

    public virtual ushort? ReadUInt16(PlcPointConfiguration point)
    {
        return this[point].ReadUInt16(point.Address);
    }

    public virtual async Task<ushort?> ReadUInt16Async(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadUInt16Async(point);
    }

    public virtual async Task<ushort?> ReadUInt16Async(PlcPointConfiguration point)
    {
        return await this[point].ReadUInt16Async(point.Address);
    }



    public virtual int? ReadInt32(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadInt32(point);
    }

    public virtual int? ReadInt32(PlcPointConfiguration point)
    {
        return this[point].ReadInt32(point.Address);
    }

    public virtual async Task<int?> ReadInt32Async(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadInt32Async(point);
    }

    public virtual async Task<int?> ReadInt32Async(PlcPointConfiguration point)
    {
        return await this[point].ReadInt32Async(point.Address);
    }



    public virtual uint? ReadUInt32(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadUInt32(point);
    }

    public virtual uint? ReadUInt32(PlcPointConfiguration point)
    {
        return this[point].ReadUInt32(point.Address);
    }

    public virtual async Task<uint?> ReadUInt32Async(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadUInt32Async(point);
    }

    public virtual async Task<uint?> ReadUInt32Async(PlcPointConfiguration point)
    {
        return await this[point].ReadUInt32Async(point.Address);
    }



    public virtual long? ReadInt64(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadInt64(point);
    }

    public virtual long? ReadInt64(PlcPointConfiguration point)
    {
        return this[point].ReadInt64(point.Address);
    }

    public virtual async Task<long?> ReadInt64Async(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadInt64Async(point);
    }

    public virtual async Task<long?> ReadInt64Async(PlcPointConfiguration point)
    {
        return await this[point].ReadInt64Async(point.Address);
    }



    public virtual ulong? ReadUInt64(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadUInt64(point);
    }

    public virtual ulong? ReadUInt64(PlcPointConfiguration point)
    {
        return this[point].ReadUInt64(point.Address);
    }

    public virtual async Task<ulong?> ReadUInt64Async(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadUInt64Async(point);
    }

    public virtual async Task<ulong?> ReadUInt64Async(PlcPointConfiguration point)
    {
        return await this[point].ReadUInt64Async(point.Address);
    }


    public virtual bool? ReadBool(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadBool(point);
    }

    public virtual bool? ReadBool(PlcPointConfiguration point)
    {
        return this[point].ReadBool(point.Address);
    }

    public virtual async Task<bool?> ReadBoolAsync(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadBoolAsync(point);
    }

    public virtual async Task<bool?> ReadBoolAsync(PlcPointConfiguration point)
    {
        return await this[point].ReadBoolAsync(point.Address);
    }



    public virtual string? ReadString(string name, ushort length)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadString(point, length);
    }

    public virtual string? ReadString(string name, ushort length, Encoding encoding)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.ReadString(point, length, encoding);
    }

    public virtual string? ReadString(PlcPointConfiguration point, ushort length)
    {
        IPlcDevice s = this[point];

        return this[point].ReadString(point.Address, length);
    }

    public virtual string? ReadString(PlcPointConfiguration point, ushort length, Encoding encoding)
    {
        return this[point].ReadString(point.Address, length, encoding);
    }

    public virtual async Task<string?> ReadStringAsync(string name, ushort length)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadStringAsync(point, length);
    }

    public virtual async Task<string?> ReadStringAsync(PlcPointConfiguration point, ushort length)
    {
        return await this[point].ReadStringAsync(point.Address, length);
    }

    public virtual async Task<string?> ReadStringAsync(string name, ushort length, Encoding encoding)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.ReadStringAsync(point, length, encoding);
    }

    public virtual async Task<string?> ReadStringAsync(PlcPointConfiguration point, ushort length, Encoding encoding)
    {
        return await this[point].ReadStringAsync(point.Address, length, encoding);
    }

    #endregion


    #region Write

    public virtual bool WriteInt16(string name, int value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].WriteInt16(point.Address, value);
    }

    public virtual bool WriteInt16(PlcPointConfiguration point, int value)
    {
        return this[point].WriteInt16(point.Address, value);
    }

    public virtual async Task<bool> WriteInt16Async(string name, int value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteInt16Async(point.Address, value);
    }

    public virtual async Task<bool> WriteInt16Async(PlcPointConfiguration point, int value)
    {
        return await this[point].WriteInt16Async(point.Address, value);
    }

    public virtual bool Write(string name, short value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, short value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, ushort value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, ushort value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, int value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, int value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, uint value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, uint value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, long value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, long value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, ulong value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, ulong value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, bool value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, bool value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(string name, string value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Write(point.Address, value);
    }

    public virtual bool Write(PlcPointConfiguration point, string value)
    {
        return this[point].Write(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, short value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, short value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, ushort value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, ushort value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, int value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, int value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, uint value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, uint value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, long value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, long value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, ulong value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, ulong value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, bool value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, bool value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(string name, string value)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WriteAsync(point.Address, value);
    }

    public virtual async Task<bool> WriteAsync(PlcPointConfiguration point, string value)
    {
        return await this[point].WriteAsync(point.Address, value);
    }

    #endregion


    #region Wait


    public bool WaitInt16(string name, int value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.WaitInt16(point, value, timeOut, readInterval);
    }
    public bool WaitInt16(PlcPointConfiguration point, int value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, Convert.ToInt16(value), timeOut, readInterval);
    }

    public virtual async Task<bool> WaitInt16Async(string name, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitInt16Async(string name, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, Convert.ToInt16(value), timeOut, readInterval, token);
    }

    public bool Wait(string name, short value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, short value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(string name, ushort value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, ushort value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(string name, int value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, int value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(string name, uint value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, uint value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(string name, long value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, long value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(string name, ulong value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, ulong value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(string name, bool value, int timeOut = -1, int readInterval = 100)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public bool Wait(PlcPointConfiguration point, bool value, int timeOut = -1, int readInterval = 100)
    {
        return this[point].Wait(point.Address, value, timeOut, readInterval);
    }

    public virtual async Task<bool> WaitAsync(string name, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(string name, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(string name, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(string name, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(string name, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(string name, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(string name, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    public virtual async Task<bool> WaitAsync(PlcPointConfiguration point, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        return await this[point].WaitAsync(point.Address, value, timeOut, readInterval, token);
    }

    #endregion


    #region Set-Reset

    public virtual bool Set(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.WriteInt16(point, 1);
    }

    public virtual bool Set(PlcPointConfiguration point)
    {
        return this.WriteInt16(point, 1);
    }

    public virtual async Task<bool> SetAsync(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.WriteInt16Async(point, 1);
    }

    public virtual async Task<bool> SetAsync(PlcPointConfiguration point)
    {
        return await this.WriteInt16Async(point, 1);
    }

    public virtual bool Reset(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return this.WriteInt16(point, 0);
    }

    public virtual bool Reset(PlcPointConfiguration point)
    {
        return this.WriteInt16(point, 0);
    }

    public virtual async Task<bool> ResetAsync(string name)
    {
        var point = this.GetPlcPointConfiguration(name);
        return await this.WriteInt16Async(point, 0);
    }

    public virtual async Task<bool> ResetAsync(PlcPointConfiguration point)
    {
        return await this.WriteInt16Async(point, 0);
    }

    #endregion


    #region Axis

    public virtual bool AxisJogPositive(string axisName)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.JogAddress, nameof(axis.JogAddress));
        return this[axis].WriteInt16(axis.JogAddress, 1);
    }

    public virtual bool AxisJogPositive(int axisNumber)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.JogAddress, nameof(axis.JogAddress));
        return this[axis].WriteInt16(axis.JogAddress, 1);
    }

    public virtual bool AxisJogNegative(string axisName)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.JogAddress, nameof(axis.JogAddress));
        return this[axis].WriteInt16(axis.JogAddress, 2);
    }

    public virtual bool AxisJogNegative(int axisNumber)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.JogAddress, nameof(axis.JogAddress));
        return this[axis].WriteInt16(axis.JogAddress, 2);
    }

    public virtual bool AxisJog(string axisName, int direction)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.JogAddress, nameof(axis.JogAddress));
        return this[axis].WriteInt16(axis.JogAddress, direction);
    }

    public virtual bool AxisJog(int axisNumber, int direction)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.JogAddress, nameof(axis.JogAddress));
        return this[axis].WriteInt16(axis.JogAddress, direction);
    }

    public virtual double? GetAxisCurrentPosition(string axisName)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.CurrentPositionAddress, nameof(axis.CurrentPositionAddress));
        return this[axis].ReadInt32(axis.CurrentPositionAddress);
    }

    public virtual double? GetAxisCurrentPosition(int axisNumber)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.CurrentPositionAddress, nameof(axis.CurrentPositionAddress));
        return this[axis].ReadInt32(axis.CurrentPositionAddress);
    }

    public virtual async Task<double?> GetAxisCurrentPositionAsync(string axisName)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.CurrentPositionAddress, nameof(axis.CurrentPositionAddress));
        return await this[axis].ReadInt32Async(axis.CurrentPositionAddress);
    }

    public virtual async Task<double?> GetAxisCurrentPositionAsync(int axisNumber)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.CurrentPositionAddress, nameof(axis.CurrentPositionAddress));
        return await this[axis].ReadInt32Async(axis.CurrentPositionAddress);
    }

    public virtual bool ClearAlarm(string axisName, int value = 1)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.ClearAlarmAddress, nameof(axis.ClearAlarmAddress));
        return this[axis].WriteInt16(axis.ClearAlarmAddress, value);
    }

    public virtual bool ClearAlarm(int axisNumber, int value = 1)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.ClearAlarmAddress, nameof(axis.ClearAlarmAddress));
        return this[axis].WriteInt16(axis.ClearAlarmAddress, value);
    }

    public virtual async Task<bool> ClearAlarmAsync(string axisName, int value = 1)
    {
        var axis = this.GetPlcAxisConfiguration(axisName);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.ClearAlarmAddress, nameof(axis.ClearAlarmAddress));
        return await this[axis].WriteInt16Async(axis.ClearAlarmAddress, value);
    }

    public virtual async Task<bool> ClearAlarmAsync(int axisNumber, int value = 1)
    {
        var axis = this.GetPlcAxisConfiguration(axisNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(axis.ClearAlarmAddress, nameof(axis.ClearAlarmAddress));
        return await this[axis].WriteInt16Async(axis.ClearAlarmAddress, value);
    }

    #endregion

}
