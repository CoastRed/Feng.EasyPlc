using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feng.EasyPlc.Contracts;
using Newtonsoft.Json.Linq;

namespace Feng.EasyPlc.Extensions
{
    public static class PlcDeviceExtensions
    {
        extension(IPlcDevice plcDevice)
        {
            #region Read

            public bool ReadInt16(string address, out short? value)
            {
                value = plcDevice.ReadInt16(address);
                return value.HasValue;
            }

            public bool ReadInt16(string address, out int? value)
            {
                value = plcDevice.ReadInt16(address);
                return value.HasValue;
            }

            public bool ReadInt16(string address, int length, out short[]? value)
            {
                value = plcDevice.ReadInt16(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public bool ReadInt16(string address, int length, out int[]? value)
            {
                var arr = plcDevice.ReadInt16(address, Convert.ToUInt16(length));
                if (arr is null)
                {
                    value = null;
                    return false;
                }
                else
                {
                    value = Array.ConvertAll(arr, i => Convert.ToInt32(i));
                    return true;
                }
            }

            public bool ReadUInt16(string address, out ushort? value)
            {
                value = plcDevice.ReadUInt16(address);
                return value.HasValue;
            }

            public bool ReadUInt16(string address, out uint? value)
            {
                value = plcDevice.ReadUInt16(address);
                return value.HasValue;
            }

            public bool ReadUInt16(string address, int length, out ushort[]? value)
            {
                value = plcDevice.ReadUInt16(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public bool ReadUInt16(string address, int length, out uint[]? value)
            {
                var arr = plcDevice.ReadUInt16(address, Convert.ToUInt16(length));
                if (arr is null)
                {
                    value = null;
                    return false;
                }
                else
                {
                    value = Array.ConvertAll(arr, i => Convert.ToUInt32(i));
                    return true;
                }
            }

            public bool ReadInt32(string address, out int? value)
            {
                value = plcDevice.ReadInt32(address);
                return value.HasValue;
            }

            public bool ReadInt32(string address, int length, out int[]? value)
            {
                value = plcDevice.ReadInt32(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public bool ReadUInt32(string address, out uint? value)
            {
                value = plcDevice.ReadUInt32(address);
                return value.HasValue;
            }

            public bool ReadUInt32(string address, int length, out uint[]? value)
            {
                value = plcDevice.ReadUInt32(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public bool ReadInt64(string address, out long? value)
            {
                value = plcDevice.ReadInt64(address);
                return value.HasValue;
            }

            public bool ReadInt64(string address, int length, out long[]? value)
            {
                value = plcDevice.ReadInt64(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public bool ReadUInt64(string address, out ulong? value)
            {
                value = plcDevice.ReadUInt64(address);
                return value.HasValue;
            }

            public bool ReadUInt64(string address, int length, out ulong[]? value)
            {
                value = plcDevice.ReadUInt64(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public bool ReadBool(string address, out bool? value)
            {
                value = plcDevice.ReadBool(address);
                return value.HasValue;
            }

            public bool ReadBool(string address, int length, out bool[]? value)
            {
                value = plcDevice.ReadBool(address, Convert.ToUInt16(length));
                return value is not null;
            }

            public string? ReadString(string address, int length)
            {
                return plcDevice.ReadString(address, Convert.ToUInt16(length), Encoding.ASCII);
            }

            public bool ReadString(string address, int length, out string? value)
            {
                value = plcDevice.ReadString(address, Convert.ToUInt16(length), Encoding.ASCII);
                return value is not null;
            }

            public bool ReadString(string address, ushort length, Encoding encoding, out string? value)
            {
                value = plcDevice.ReadString(address, Convert.ToUInt16(length), encoding);
                return value is not null;
            }

            public async Task<string?> ReadStringAsync(string address, int length)
            {
                return await plcDevice.ReadStringAsync(address, Convert.ToUInt16(length), Encoding.ASCII);
            }

            #endregion

            #region Write

            public bool WriteInt16(string address, int value)
            {
                return plcDevice.Write(address, (short)value);
            }

            public bool WriteInt16(string address, int[] value)
            {
                return plcDevice.Write(address, Array.ConvertAll(value, i => Convert.ToInt16(i)));
            }

            public async Task<bool> WriteInt16Async(string address, int value)
            {
                return await plcDevice.WriteAsync(address, (short)value);
            }

            public async Task<bool> WriteInt16Async(string address, int[] value)
            {
                return await plcDevice.WriteAsync(address, Array.ConvertAll(value, i => Convert.ToInt16(i)));
            }

            public bool WriteInt64(string address, int value)
            {
                return plcDevice.Write(address, (long)value);
            }

            public bool WriteInt64(string address, int[] value)
            {
                return plcDevice.Write(address, Array.ConvertAll(value, i => Convert.ToInt64(i)));
            }

            public async Task<bool> WriteInt64Async(string address, int value)
            {
                return await plcDevice.WriteAsync(address, (long)value);
            }

            public async Task<bool> WriteInt64Async(string address, int[] value)
            {
                return await plcDevice.WriteAsync(address, Array.ConvertAll(value, i => Convert.ToInt64(i)));
            }

            #endregion

            #region Wait

            public bool WaitInt16(string address, int value, int timeOut = -1, int readInterval = 100)
            {
                return plcDevice.Wait(address, Convert.ToInt16(value), timeOut, readInterval);
            }

            public async Task<bool> WaitInt16Async(string address, int value, int timeOut = -1, int readInterval = 100)
            {
                return await plcDevice.WaitAsync(address, Convert.ToInt16(value), timeOut, readInterval);
            }

            public async Task<bool> WaitInt16Async(string address, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default)
            {
                return await plcDevice.WaitAsync(address, Convert.ToInt16(value), timeOut, readInterval, token);
            }

            #endregion
        }
    }
}
