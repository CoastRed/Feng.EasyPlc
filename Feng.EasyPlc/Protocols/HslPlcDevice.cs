using Feng.EasyPlc.Contracts;
using Feng.EasyPlc.Models;
using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.ModBus;
using Newtonsoft.Json.Linq;
using NLog;
using System.Text;
using System.Threading.Tasks;

namespace Feng.EasyPlc.Protocol;

public class HslPlcDevice : IPlcDevice
{
    
    private readonly PlcDeviceConfiguration _configuration;
    private readonly ILogger? _logger;

    private IReadWriteDevice _device = null!;
    private DeviceTcpNet? DeviceTcpNet => _device as DeviceTcpNet;

    public string PlcName { get; init; }

    public string PlcIpAddress { get; init; }

    public int PlcPort { get; init; }

    public HslPlcDevice(PlcDeviceConfiguration configuration, ILogger? logger = null, Func<PlcDeviceConfiguration, IReadWriteDevice>? deviceFactory = null)
    {
        _configuration = configuration;
        _logger = logger;
        this.PlcName = configuration.Name ?? throw new ArgumentNullException(nameof(configuration.Name));
        this.PlcIpAddress = configuration.IPAddress ?? throw new ArgumentNullException(nameof(configuration.IPAddress));
        this.PlcPort = configuration.Port;
        if(deviceFactory is not null)
        {
            this._device = deviceFactory(configuration);
        }
        else
        {
            this.InitDevice();
        }

        DeviceTcpNet?.ConnectTimeOut = this._configuration.TimeOut;
        DeviceTcpNet?.ReceiveTimeOut = this._configuration.TimeOut;
        
    }

    public virtual void InitDevice()
    {
        var protocol = _configuration.Protocol.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if(protocol.Length < 2)
        {
            throw new Exception("不支持的协议类型：" + _configuration.Protocol);
        }
        if(protocol[0] == "HSL" && protocol[1] == "ModbusTcp")
        {
            this._device = new ModbusTcpNet(PlcIpAddress, this.PlcPort);
        }
        else
        {
            throw new Exception("不支持的协议类型：" + _configuration.Protocol);
        }
    }

    public virtual bool Connect()
    {
        if(DeviceTcpNet is not null)
        {
            var re = this.DeviceTcpNet.ConnectServer();
            if (re.IsSuccess)
            {
                _logger?.Info($"连接PLC成功： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》");
                return true;
            }
            _logger?.Error($"连接PLC失败： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual async Task<bool> ConnectAsync()
    {
        if (DeviceTcpNet is not null)
        {
            var re = await this.DeviceTcpNet.ConnectServerAsync();
            if (re.IsSuccess)
            {
                _logger?.Info($"连接PLC成功： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》");
                return true;
            }
            _logger?.Error($"连接PLC失败： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual bool DisConnect()
    {
        if (DeviceTcpNet is not null)
        {
            var re = this.DeviceTcpNet.ConnectClose();
            if (re.IsSuccess)
            {
                _logger?.Info($"断开PLC成功： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》");
                return true;
            }
            _logger?.Error($"断开PLC失败： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual async Task<bool> DisConnectAsync()
    {
        if (DeviceTcpNet is not null)
        {
            var re = await this.DeviceTcpNet.ConnectCloseAsync();
            if (re.IsSuccess)
            {
                _logger?.Info($"断开PLC成功： PLCIP《{this.PlcIpAddress}》 PLC端口《{this.PlcPort}》");
                return true;
            }
            _logger?.Error($"断开PLC失败： PLCIP《{this.PlcIpAddress}》  PLC端口《{this.PlcPort}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }
        else
        {
            return true;
        }
    }


    #region Read

    public virtual short? ReadInt16(string address)
    {
        var re = this._device.ReadInt16(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int16》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual short[]? ReadInt16(string address, int length)
    {
        var re = this._device.ReadInt16(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<short?> ReadInt16Async(string address)
    {
        var re = await this._device.ReadInt16Async(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int16》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<short[]?> ReadInt16Async(string address, int length)
    {
        var re = await this._device.ReadInt16Async(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual ushort? ReadUInt16(string address)
    {
        var re = this._device.ReadUInt16(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt16》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual ushort[]? ReadUInt16(string address, int length)
    {
        var re = this._device.ReadUInt16(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt16》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<ushort?> ReadUInt16Async(string address)
    {
        var re = await this._device.ReadUInt16Async(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt16》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<ushort[]?> ReadUInt16Async(string address, int length)
    {
        var re = await this._device.ReadUInt16Async(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt16》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual int? ReadInt32(string address)
    {
        var re = this._device.ReadInt32(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int32》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual int[]? ReadInt32(string address, int length)
    {
        var re = this._device.ReadInt32(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int32》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<int?> ReadInt32Async(string address)
    {
        var re = await this._device.ReadInt32Async(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int32》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<int[]?> ReadInt32Async(string address, int length)
    {
        var re = await this._device.ReadInt32Async(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int32》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual uint? ReadUInt32(string address)
    {
        var re = this._device.ReadUInt32(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt32》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual uint[]? ReadUInt32(string address, int length)
    {
        var re = this._device.ReadUInt32(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt32》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<uint?> ReadUInt32Async(string address)
    {
        var re = await this._device.ReadUInt32Async(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt32》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<uint[]?> ReadUInt32Async(string address, int length)
    {
        var re = await this._device.ReadUInt32Async(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt32》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual long? ReadInt64(string address)
    {
        var re = this._device.ReadInt64(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int64》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual long[]? ReadInt64(string address, int length)
    {
        var re = this._device.ReadInt64(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int64》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<long?> ReadInt64Async(string address)
    {
        var re = await this._device.ReadInt64Async(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int64》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<long[]?> ReadInt64Async(string address, int length)
    {
        var re = await this._device.ReadInt64Async(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int64》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual ulong? ReadUInt64(string address)
    {
        var re = this._device.ReadUInt64(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt64》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual ulong[]? ReadUInt64(string address, int length)
    {
        var re = this._device.ReadUInt64(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt64》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<ulong?> ReadUInt64Async(string address)
    {
        var re = await this._device.ReadUInt64Async(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt64》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<ulong[]?> ReadUInt64Async(string address, int length)
    {
        var re = await this._device.ReadUInt64Async(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt64》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual bool? ReadBool(string address)
    {
        var re = this._device.ReadBool(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Bool》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual bool[]? ReadBool(string address, int length)
    {
        var re = this._device.ReadBool(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Bool》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<bool?> ReadBoolAsync(string address)
    {
        var re = await this._device.ReadBoolAsync(address);
        if (re.IsSuccess)
        {
            _logger?.Trace("读取PLC成功： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Bool》 读取结果《{result}》", this.PlcIpAddress, address, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<bool[]?> ReadBoolAsync(string address, int length)
    {
        var re = await this._device.ReadBoolAsync(address, Convert.ToUInt16(length));
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Bool》 读取结果《{result}》", this.PlcIpAddress, address, length, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
            return null;
        }
    }

    public virtual string? ReadString(string address, ushort length, Encoding encoding)
    {
        var re = this._device.ReadString(address, length, encoding);
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《String》 字符编码《{encoding}》 读取结果《{result}》", this.PlcIpAddress, address, length, encoding, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《String》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, length, encoding.EncodingName, re.ToMessageShowString());
            return null;
        }
    }

    public virtual async Task<string?> ReadStringAsync(string address, ushort length, Encoding encoding)
    {
        var re = await this._device.ReadStringAsync(address, length, encoding);
        if (re.IsSuccess)
        {
            _logger?.Trace("批量读取PLC成功： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《String》 字符编码《{encoding}》 读取结果《{result}》", this.PlcIpAddress, address, length, encoding, re.Content);
            return re.Content;
        }
        else
        {
            _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《String》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, length, encoding.EncodingName, re.ToMessageShowString());
            return null;
        }
    }

    #endregion

    #region Write

    public virtual bool Write(string address, short value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, short[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, ushort value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, ushort[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, int value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, int[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, uint value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, uint[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, long value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, long[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, ulong value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, ulong[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, bool value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, bool[] value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, string value)
    {
        var re = this._device.Write(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》", this.PlcIpAddress, address, value.GetType().Name, value, Encoding.ASCII.EncodingName);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, Encoding.ASCII.EncodingName, re.ToMessageShowString());
        return false;
    }

    public virtual bool Write(string address, string value, Encoding encoding)
    {
        var re = this._device.Write(address, value, encoding);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》", this.PlcIpAddress, address, value.GetType().Name, value, encoding.EncodingName);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, encoding.EncodingName, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, short value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, short[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, ushort value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, ushort[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, int value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, int[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, uint value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, uint[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, long value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, long[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, ulong value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, ulong[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, bool value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, bool[] value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("批量写入PLC成功： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{value}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, string value)
    {
        var re = await this._device.WriteAsync(address, value);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》", this.PlcIpAddress, address, value.GetType().Name, value);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, Encoding.ASCII.EncodingName, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WriteAsync(string address, string value, Encoding encoding)
    {
        var re = await this._device.WriteAsync(address, value, encoding);
        if (re.IsSuccess)
        {
            _logger?.Debug("写入PLC成功： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》", this.PlcIpAddress, address, value.GetType().Name, value, encoding.EncodingName);
            return true;
        }
        _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, encoding.EncodingName, re.ToMessageShowString());
        return false;
    }

    #endregion

    #region Wait

    public virtual bool Wait(string address, short value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual bool Wait(string address, ushort value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual bool Wait(string address, int value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual bool Wait(string address, uint value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual bool Wait(string address, long value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual bool Wait(string address, ulong value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual bool Wait(string address, bool value, int timeOut = -1, int readInterval = 100)
    {
        var re = this._device.Wait(address, value, readInterval, timeOut);
        if (re.IsSuccess)
        {
            _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
            return true;
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<short>? re = null;
        while (DateTime.Now <= time || timeOut <= 0)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadInt16Async(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }

        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<ushort>? re = null;
        while (DateTime.Now <= time || timeOut <= 0)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadUInt16Async(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }

        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<int>? re = null;
        while (DateTime.Now <= time || timeOut == -1)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadInt32Async(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<uint>? re = null;
        while (DateTime.Now <= time || timeOut == -1)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadUInt32Async(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<long>? re = null;
        while (DateTime.Now <= time || timeOut == -1)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadInt64Async(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<ulong>? re = null;
        while (DateTime.Now <= time || timeOut == -1)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadUInt64Async(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    public virtual async Task<bool> WaitAsync(string address, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
    {
        var time = DateTime.Now.AddMilliseconds(timeOut);
        OperateResult<bool>? re = null;
        while (DateTime.Now <= time || timeOut == -1)
        {
            token.ThrowIfCancellationRequested();
            re = await this._device.ReadBoolAsync(address);
            if (re.IsSuccess && re.Content == value)
            {
                _logger?.Debug("等待PLC成功： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval);
                return true;
            }
            else
            {
                await Task.Delay(readInterval, token);
            }
        }
        _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re?.ToMessageShowString());
        return false;
    }

    #endregion

}
