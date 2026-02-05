using System.Text;
using System.Threading.Tasks;
using Feng.EasyPlc.Contracts;
using Feng.EasyPlc.Models;
using HslCommunication;
using HslCommunication.ModBus;
using Newtonsoft.Json.Linq;
using NLog;

namespace Feng.EasyPlc.Protocol
{
    public class ModbusTcp : IPlcDevice
    {
        private readonly ModbusTcpNet _modbus;
        private readonly ILogger? _logger;

        public string PlcName { get; init; }

        public string PlcIpAddress { get; init; }

        public int PlcPort { get; init; }

        public ModbusTcp(PlcConfiguration configuration, ILogger? logger = null)
        {
            this.PlcName = configuration.Name ?? throw new ArgumentNullException(nameof(configuration.Name));
            this.PlcIpAddress = configuration.IPAddress ?? throw new ArgumentNullException(nameof(configuration.IPAddress));
            this.PlcPort = configuration.Port;
            this._modbus = new ModbusTcpNet(this.PlcIpAddress, this.PlcPort);
            _logger = logger;
        }

        public bool Connect()
        {
            var re = this._modbus.ConnectServer();
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error($"连接PLC失败： PLCIP《{this.PlcIpAddress}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }

        public async Task<bool> ConnectAsync()
        {
            var re = await this._modbus.ConnectServerAsync();
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error($"连接PLC失败： PLCIP《{this.PlcIpAddress}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }

        public bool DisConnect()
        {
            var re = this._modbus.ConnectClose();
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error($"断开PLC失败： PLCIP《{this.PlcIpAddress}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }

        public async Task<bool> DisConnectAsync()
        {
            var re = await this._modbus.ConnectCloseAsync();
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error($"断开PLC失败： PLCIP《{this.PlcIpAddress}》 错误码《{re.ToMessageShowString()}》");
            return false;
        }


        #region Read

        public short? ReadInt16(string address)
        {
            var re = this._modbus.ReadInt16(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public short[]? ReadInt16(string address, int length)
        {
            var re = this._modbus.ReadInt16(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<short?> ReadInt16Async(string address)
        {
            var re = await this._modbus.ReadInt16Async(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<short[]?> ReadInt16Async(string address, int length)
        {
            var re = await this._modbus.ReadInt16Async(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public ushort? ReadUInt16(string address)
        {
            var re = this._modbus.ReadUInt16(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public ushort[]? ReadUInt16(string address, int length)
        {
            var re = this._modbus.ReadUInt16(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<ushort?> ReadUInt16Async(string address)
        {
            var re = await this._modbus.ReadUInt16Async(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt16》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<ushort[]?> ReadUInt16Async(string address, int length)
        {
            var re = await this._modbus.ReadUInt16Async(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int16》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public int? ReadInt32(string address)
        {
            var re = this._modbus.ReadInt32(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public int[]? ReadInt32(string address, int length)
        {
            var re = this._modbus.ReadInt32(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<int?> ReadInt32Async(string address)
        {
            var re = await this._modbus.ReadInt32Async(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<int[]?> ReadInt32Async(string address, int length)
        {
            var re = await this._modbus.ReadInt32Async(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public uint? ReadUInt32(string address)
        {
            var re = this._modbus.ReadUInt32(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public uint[]? ReadUInt32(string address, int length)
        {
            var re = this._modbus.ReadUInt32(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<uint?> ReadUInt32Async(string address)
        {
            var re = await this._modbus.ReadUInt32Async(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<uint[]?> ReadUInt32Async(string address, int length)
        {
            var re = await this._modbus.ReadUInt32Async(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt32》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public long? ReadInt64(string address)
        {
            var re = this._modbus.ReadInt64(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public long[]? ReadInt64(string address, int length)
        {
            var re = this._modbus.ReadInt64(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<long?> ReadInt64Async(string address)
        {
            var re = await this._modbus.ReadInt64Async(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<long[]?> ReadInt64Async(string address, int length)
        {
            var re = await this._modbus.ReadInt64Async(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Int64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public ulong? ReadUInt64(string address)
        {
            var re = this._modbus.ReadUInt64(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public ulong[]? ReadUInt64(string address, int length)
        {
            var re = this._modbus.ReadUInt64(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<ulong?> ReadUInt64Async(string address)
        {
            var re = await this._modbus.ReadUInt64Async(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<ulong[]?> ReadUInt64Async(string address, int length)
        {
            var re = await this._modbus.ReadUInt64Async(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《UInt64》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public bool? ReadBool(string address)
        {
            var re = this._modbus.ReadBool(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public bool[]? ReadBool(string address, int length)
        {
            var re = this._modbus.ReadBool(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<bool?> ReadBoolAsync(string address)
        {
            var re = await this._modbus.ReadBoolAsync(address);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("读取PLC失败： PLCIP《{ip}》 读取地址《{addr}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<bool[]?> ReadBoolAsync(string address, int length)
        {
            var re = await this._modbus.ReadBoolAsync(address, Convert.ToUInt16(length));
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《Bool》 错误码《{error}》", this.PlcIpAddress, address, length, re.ToMessageShowString());
                return null;
            }
        }

        public string? ReadString(string address, ushort length, Encoding encoding)
        {
            var re = this._modbus.ReadString(address, length, encoding);
            if (re.IsSuccess)
            {
                return re.Content;
            }
            else
            {
                _logger?.Error("批量读取PLC失败： PLCIP《{ip}》 读取起始地址《{addr}》 读取长度《{length}》 读取类型《String》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, length, encoding.EncodingName, re.ToMessageShowString());
                return null;
            }
        }

        public async Task<string?> ReadStringAsync(string address, ushort length, Encoding encoding)
        {
            var re = await this._modbus.ReadStringAsync(address, length, encoding);
            if (re.IsSuccess)
            {
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

        public bool Write(string address, short value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, short[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, ushort value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, ushort[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, int value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, int[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, uint value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, uint[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, long value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, long[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, ulong value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, ulong[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, bool value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, bool[] value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, string value)
        {
            var re = this._modbus.Write(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, Encoding.ASCII.EncodingName, re.ToMessageShowString());
            return false;
        }

        public bool Write(string address, string value, Encoding encoding)
        {
            var re = this._modbus.Write(address, value, encoding);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, encoding.EncodingName, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, short value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, short[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, ushort value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, ushort[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, int value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, int[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, uint value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, uint[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, long value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, long[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, ulong value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, ulong[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, bool value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, bool[] value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("批量写入PLC失败： PLCIP《{ip}》 写入起始地址《{addr}》 写入长度《{length}》 写入类型《type》 写入值《{@value}》 错误码《{error}》", this.PlcIpAddress, address, value.Length, value.GetType().Name, value, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, string value)
        {
            var re = await this._modbus.WriteAsync(address, value);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, Encoding.ASCII.EncodingName, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WriteAsync(string address, string value, Encoding encoding)
        {
            var re = await this._modbus.WriteAsync(address, value, encoding);
            if (re.IsSuccess)
            {
                return true;
            }
            _logger?.Error("写入PLC失败： PLCIP《{ip}》 写入地址《{addr}》 写入类型《{type}》 写入值《{value}》 字符编码《{encoding}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, encoding.EncodingName, re.ToMessageShowString());
            return false;
        }

        #endregion

        #region Wait

        public bool Wait(string address, short value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public bool Wait(string address, ushort value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public bool Wait(string address, int value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public bool Wait(string address, uint value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public bool Wait(string address, long value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public bool Wait(string address, ulong value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public bool Wait(string address, bool value, int timeOut = -1, int readInterval = 100)
        {
            var re = this._modbus.Wait(address, value, readInterval, timeOut);
            if (re.IsSuccess) return true;
            _logger?.Error("等待PLC失败： PLCIP《{ip}》 地址《{addr}》 等待类型《{type}》 等待值《{value}》 超时时间《{timeOut}》 轮询间隔《{readInterval}》 错误码《{error}》", this.PlcIpAddress, address, value.GetType().Name, value, timeOut, readInterval, re.ToMessageShowString());
            return false;
        }

        public async Task<bool> WaitAsync(string address, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<short>? re = null;
            while (DateTime.Now <= time || timeOut <= 0)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadInt16Async(address);
                if (re.IsSuccess && re.Content == value)
                {
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

        public async Task<bool> WaitAsync(string address, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<ushort>? re = null;
            while (DateTime.Now <= time || timeOut <= 0)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadUInt16Async(address);
                if (re.IsSuccess && re.Content == value)
                {
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

        public async Task<bool> WaitAsync(string address, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<int>? re = null;
            while (DateTime.Now <= time || timeOut == -1)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadInt32Async(address);
                if (re.IsSuccess && re.Content == value)
                {
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

        public async Task<bool> WaitAsync(string address, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<uint>? re = null;
            while (DateTime.Now <= time || timeOut == -1)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadUInt32Async(address);
                if (re.IsSuccess && re.Content == value)
                {
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

        public async Task<bool> WaitAsync(string address, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<long>? re = null;
            while (DateTime.Now <= time || timeOut == -1)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadInt64Async(address);
                if (re.IsSuccess && re.Content == value)
                {
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

        public async Task<bool> WaitAsync(string address, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<ulong>? re = null;
            while (DateTime.Now <= time || timeOut == -1)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadUInt64Async(address);
                if (re.IsSuccess && re.Content == value)
                {
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

        public async Task<bool> WaitAsync(string address, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
        {
            var time = DateTime.Now.AddMilliseconds(timeOut);
            OperateResult<bool>? re = null;
            while (DateTime.Now <= time || timeOut == -1)
            {
                token.ThrowIfCancellationRequested();
                re = await this._modbus.ReadBoolAsync(address);
                if (re.IsSuccess && re.Content == value)
                {
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
}
