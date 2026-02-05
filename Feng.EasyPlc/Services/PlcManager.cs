using System.Text;
using Feng.EasyPlc.Contracts;
using Feng.EasyPlc.Extensions;
using Feng.EasyPlc.Models;
using Newtonsoft.Json;
using NLog;

namespace Feng.EasyPlc.Services
{
    public class PlcManager
    {

        private readonly Dictionary<PlcConfiguration, IPlcDevice> _configurationDeviceMap = new Dictionary<PlcConfiguration, IPlcDevice>();
        private readonly ILogger? _logger;

        private PlcSystemConfiguration PlcSystemConfig { get; set; } = new PlcSystemConfiguration();
        private List<PlcDataPoint> PlcDataPoints => this.PlcSystemConfig.DataPoints;
        private List<PlcAxisConfiguration> PlcAxisConfigurations => this.PlcSystemConfig.AxisConfigurations;

        public IPlcDevice this[string plcName] => _configurationDeviceMap.First(x => x.Key.Name == plcName).Value;
        public IPlcDevice this[PlcDataPoint point] => _configurationDeviceMap.First(x => x.Key.Name == point.PlcDeviceName).Value;
        public IPlcDevice this[PlcAxisConfiguration axisConfiguration] => _configurationDeviceMap.First(x => x.Key.Name == axisConfiguration.PlcDeviceName).Value;

        public PlcManager(ILogger? logger)
        {
            _logger = logger;
            this.PlcSystemConfig = this.LoadPlcSystemConfiguration();
            this.InitPLC();
        }

        public PlcManager(List<PlcConfiguration> config, ILogger? logger = null) : this(logger)
        {
            this.PlcSystemConfig.DeviceConfigurations = config ?? throw new ArgumentNullException(nameof(config));
        }

        public virtual void InitPLC()
        {
            if (this.PlcSystemConfig.DeviceConfigurations == null)
            {
                throw new ArgumentException("Plc configuration is null.");
            }
            if (this.PlcSystemConfig.DeviceConfigurations.Any(s => string.IsNullOrWhiteSpace(s.Name) || string.IsNullOrWhiteSpace(s.IPAddress)))
            {
                throw new ArgumentException("PLC configuration must have a valid Name and IP.");
            }
            foreach (var plcConfig in this.PlcSystemConfig.DeviceConfigurations)
            {
                if (plcConfig.IsEnabled)
                {
                    switch (plcConfig.Protocol)
                    {
                        case Models.PlcProtocol.ModbusTcp:
                            _configurationDeviceMap.Add(plcConfig, new Protocol.ModbusTcp(plcConfig));
                            break;
                        default:
                            throw new NotSupportedException($"Protocol {plcConfig.Protocol} is not supported.");
                    }
                }
                else
                {
                    //_plcs.Add(plcConfig, new MockPLC());
                }
            }
        }

        public virtual PlcSystemConfiguration LoadPlcSystemConfiguration()
        {
            //string filePath = "PlcSystemConfiguration.json";

            //IConfiguration configuration = new ConfigurationBuilder()
            //    .SetBasePath(AppContext.BaseDirectory)
            //    .AddJsonFile(filePath, optional: false, reloadOnChange: true)
            //    .Build();
            //return configuration.Get<PlcSystemConfiguration>() ?? new PlcSystemConfiguration();

            string json = File.ReadAllText("PlcSystemConfiguration.json");
            return JsonConvert.DeserializeObject<PlcSystemConfiguration>(json) ?? new PlcSystemConfiguration();

        }

        public virtual PlcDataPoint GetPlcDataPoint(string name)
        {
            return this.PlcDataPoints.Single(s => s.Name == name) ?? throw new ArgumentNullException(nameof(name), $"PlcDataPoint with name {name} not found.");
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
            foreach (var plc in _configurationDeviceMap.Values)
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
            foreach (var plc in _configurationDeviceMap.Values)
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
            foreach (var plc in _configurationDeviceMap.Values)
            {
                plc.DisConnect();
            }
            return true;
        }

        public virtual async Task<bool> DisconnectAsync()
        {
            foreach (var plc in _configurationDeviceMap.Values)
            {
                await plc.DisConnectAsync();
            }
            return true;
        }



        #region Read


        public virtual short? ReadInt16(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadInt16(dataPoint);
        }

        public virtual short? ReadInt16(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadInt16(dataPoint.Address);
        }

        public virtual async Task<short?> ReadInt16Async(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadInt16Async(dataPoint);
        }

        public virtual async Task<short?> ReadInt16Async(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadInt16Async(dataPoint.Address);
        }




        public virtual ushort? ReadUInt16(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadUInt16(dataPoint);
        }

        public virtual ushort? ReadUInt16(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadUInt16(dataPoint.Address);
        }

        public virtual async Task<ushort?> ReadUInt16Async(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadUInt16Async(dataPoint);
        }

        public virtual async Task<ushort?> ReadUInt16Async(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadUInt16Async(dataPoint.Address);
        }



        public virtual int? ReadInt32(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadInt32(dataPoint);
        }

        public virtual int? ReadInt32(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadInt32(dataPoint.Address);
        }

        public virtual async Task<int?> ReadInt32Async(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadInt32Async(dataPoint);
        }

        public virtual async Task<int?> ReadInt32Async(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadInt32Async(dataPoint.Address);
        }



        public virtual uint? ReadUInt32(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadUInt32(dataPoint);
        }

        public virtual uint? ReadUInt32(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadUInt32(dataPoint.Address);
        }

        public virtual async Task<uint?> ReadUInt32Async(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadUInt32Async(dataPoint);
        }

        public virtual async Task<uint?> ReadUInt32Async(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadUInt32Async(dataPoint.Address);
        }



        public virtual long? ReadInt64(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadInt64(dataPoint);
        }

        public virtual long? ReadInt64(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadInt64(dataPoint.Address);
        }

        public virtual async Task<long?> ReadInt64Async(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadInt64Async(dataPoint);
        }

        public virtual async Task<long?> ReadInt64Async(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadInt64Async(dataPoint.Address);
        }



        public virtual ulong? ReadUInt64(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadUInt64(dataPoint);
        }

        public virtual ulong? ReadUInt64(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadUInt64(dataPoint.Address);
        }

        public virtual async Task<ulong?> ReadUInt64Async(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadUInt64Async(dataPoint);
        }

        public virtual async Task<ulong?> ReadUInt64Async(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadUInt64Async(dataPoint.Address);
        }


        public virtual bool? ReadBool(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadBool(dataPoint);
        }

        public virtual bool? ReadBool(PlcDataPoint dataPoint)
        {
            return this[dataPoint].ReadBool(dataPoint.Address);
        }

        public virtual async Task<bool?> ReadBoolAsync(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadBoolAsync(dataPoint);
        }

        public virtual async Task<bool?> ReadBoolAsync(PlcDataPoint dataPoint)
        {
            return await this[dataPoint].ReadBoolAsync(dataPoint.Address);
        }



        public virtual string? ReadString(string name, ushort length)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadString(dataPoint, length);
        }

        public virtual string? ReadString(string name, ushort length, Encoding encoding)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.ReadString(dataPoint, length, encoding);
        }

        public virtual string? ReadString(PlcDataPoint dataPoint, ushort length)
        {
            IPlcDevice s = this[dataPoint];

            return this[dataPoint].ReadString(dataPoint.Address, length);
        }

        public virtual string? ReadString(PlcDataPoint dataPoint, ushort length, Encoding encoding)
        {
            return this[dataPoint].ReadString(dataPoint.Address, length, encoding);
        }

        public virtual async Task<string?> ReadStringAsync(string name, ushort length)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadStringAsync(dataPoint, length);
        }

        public virtual async Task<string?> ReadStringAsync(PlcDataPoint dataPoint, ushort length)
        {
            return await this[dataPoint].ReadStringAsync(dataPoint.Address, length);
        }

        public virtual async Task<string?> ReadStringAsync(string name, ushort length, Encoding encoding)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.ReadStringAsync(dataPoint, length, encoding);
        }

        public virtual async Task<string?> ReadStringAsync(PlcDataPoint dataPoint, ushort length, Encoding encoding)
        {
            return await this[dataPoint].ReadStringAsync(dataPoint.Address, length, encoding);
        }

        #endregion


        #region Write

        public virtual bool WriteInt16(string name, int value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].WriteInt16(dataPoint.Address, value);
        }

        public virtual bool WriteInt16(PlcDataPoint dataPoint, int value)
        {
            return this[dataPoint].WriteInt16(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteInt16Async(string name, int value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteInt16Async(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteInt16Async(PlcDataPoint dataPoint, int value)
        {
            return await this[dataPoint].WriteInt16Async(dataPoint.Address, value);
        }

        public virtual bool Write(string name, short value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, short value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, ushort value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, ushort value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, int value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, int value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, uint value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, uint value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, long value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, long value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, ulong value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, ulong value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, bool value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, bool value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(string name, string value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual bool Write(PlcDataPoint dataPoint, string value)
        {
            return this[dataPoint].Write(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, short value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, short value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, ushort value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, ushort value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, int value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, int value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, uint value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, uint value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, long value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, long value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, ulong value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, ulong value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, bool value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, bool value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(string name, string value)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        public virtual async Task<bool> WriteAsync(PlcDataPoint dataPoint, string value)
        {
            return await this[dataPoint].WriteAsync(dataPoint.Address, value);
        }

        #endregion


        #region Wait


        public bool WaitInt16(string name, int value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.WaitInt16(dataPoint, value, timeOut, readInterval);
        }
        public bool WaitInt16(PlcDataPoint dataPoint, int value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, Convert.ToInt16(value), timeOut, readInterval);
        }

        public virtual async Task<bool> WaitInt16Async(string name, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitInt16Async(string name, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, Convert.ToInt16(value), timeOut, readInterval, token);
        }

        public bool Wait(string name, short value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, short value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(string name, ushort value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, ushort value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(string name, int value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, int value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(string name, uint value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, uint value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(string name, long value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, long value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(string name, ulong value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, ulong value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(string name, bool value, int timeOut = -1, int readInterval = 100)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public bool Wait(PlcDataPoint dataPoint, bool value, int timeOut = -1, int readInterval = 100)
        {
            return this[dataPoint].Wait(dataPoint.Address, value, timeOut, readInterval);
        }

        public virtual async Task<bool> WaitAsync(string name, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(string name, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(string name, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(string name, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(string name, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(string name, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(string name, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        public virtual async Task<bool> WaitAsync(PlcDataPoint dataPoint, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            return await this[dataPoint].WaitAsync(dataPoint.Address, value, timeOut, readInterval, token);
        }

        #endregion


        #region Set-Reset

        public virtual bool Set(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.WriteInt16(dataPoint, 1);
        }

        public virtual bool Set(PlcDataPoint dataPoint)
        {
            return this.WriteInt16(dataPoint, 1);
        }

        public virtual async Task<bool> SetAsync(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.WriteInt16Async(dataPoint, 1);
        }

        public virtual async Task<bool> SetAsync(PlcDataPoint dataPoint)
        {
            return await this.WriteInt16Async(dataPoint, 1);
        }

        public virtual bool Reset(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return this.WriteInt16(dataPoint, 0);
        }

        public virtual bool Reset(PlcDataPoint dataPoint)
        {
            return this.WriteInt16(dataPoint, 0);
        }

        public virtual async Task<bool> ResetAsync(string name)
        {
            var dataPoint = this.GetPlcDataPoint(name);
            return await this.WriteInt16Async(dataPoint, 0);
        }

        public virtual async Task<bool> ResetAsync(PlcDataPoint dataPoint)
        {
            return await this.WriteInt16Async(dataPoint, 0);
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

        #endregion

    }
}
