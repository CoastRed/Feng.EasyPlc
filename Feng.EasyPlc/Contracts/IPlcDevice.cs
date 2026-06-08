using System.Text;

namespace Feng.EasyPlc.Contracts;

public interface IPlcDevice
{
    string PlcName { get; init; }
    string PlcIpAddress { get; init; }
    int PlcPort { get; init; }

    bool Connect();
    Task<bool> ConnectAsync();
    bool DisConnect();
    Task<bool> DisConnectAsync();

    #region Read

    short? ReadInt16(string address);
    short[]? ReadInt16(string address, int length);
    Task<short?> ReadInt16Async(string address);
    Task<short[]?> ReadInt16Async(string address, int length);
    ushort? ReadUInt16(string address);
    ushort[]? ReadUInt16(string address, int length);
    Task<ushort?> ReadUInt16Async(string address);
    Task<ushort[]?> ReadUInt16Async(string address, int length);

    int? ReadInt32(string address);
    int[]? ReadInt32(string address, int length);
    Task<int?> ReadInt32Async(string address);
    Task<int[]?> ReadInt32Async(string address, int length);
    uint? ReadUInt32(string address);
    uint[]? ReadUInt32(string address, int length);
    Task<uint?> ReadUInt32Async(string address);
    Task<uint[]?> ReadUInt32Async(string address, int length);

    long? ReadInt64(string address);
    long[]? ReadInt64(string address, int length);
    Task<long?> ReadInt64Async(string address);
    Task<long[]?> ReadInt64Async(string address, int length);
    ulong? ReadUInt64(string address);
    ulong[]? ReadUInt64(string address, int length);
    Task<ulong?> ReadUInt64Async(string address);
    Task<ulong[]?> ReadUInt64Async(string address, int length);

    bool? ReadBool(string address);
    bool[]? ReadBool(string address, int length);
    Task<bool?> ReadBoolAsync(string address);
    Task<bool[]?> ReadBoolAsync(string address, int length);

    string? ReadString(string address, ushort length, Encoding encoding);
    Task<string?> ReadStringAsync(string address, ushort length, Encoding encoding);

    #endregion

    #region Write

    bool Write(string address, short value);
    bool Write(string address, short[] value);
    bool Write(string address, ushort value);
    bool Write(string address, ushort[] value);
    bool Write(string address, int value);
    bool Write(string address, int[] value);
    bool Write(string address, uint value);
    bool Write(string address, uint[] value);
    bool Write(string address, long value);
    bool Write(string address, long[] value);
    bool Write(string address, ulong value);
    bool Write(string address, ulong[] value);
    bool Write(string address, bool value);
    bool Write(string address, bool[] value);
    bool Write(string address, string value);
    bool Write(string address, string value, Encoding encoding);
    Task<bool> WriteAsync(string address, short value);
    Task<bool> WriteAsync(string address, short[] value);
    Task<bool> WriteAsync(string address, ushort value);
    Task<bool> WriteAsync(string address, ushort[] value);
    Task<bool> WriteAsync(string address, int value);
    Task<bool> WriteAsync(string address, int[] value);
    Task<bool> WriteAsync(string address, uint value);
    Task<bool> WriteAsync(string address, uint[] value);
    Task<bool> WriteAsync(string address, long value);
    Task<bool> WriteAsync(string address, long[] value);
    Task<bool> WriteAsync(string address, ulong value);
    Task<bool> WriteAsync(string address, ulong[] value);
    Task<bool> WriteAsync(string address, bool value);
    Task<bool> WriteAsync(string address, bool[] value);
    Task<bool> WriteAsync(string address, string value);
    Task<bool> WriteAsync(string address, string value, Encoding encoding);

    #endregion

    #region Wait

    bool Wait(string address, short value, int timeOut = -1, int readInterval = 100);
    bool Wait(string address, ushort value, int timeOut = -1, int readInterval = 100);
    bool Wait(string address, int value, int timeOut = -1, int readInterval = 100);
    bool Wait(string address, uint value, int timeOut = -1, int readInterval = 100);
    bool Wait(string address, long value, int timeOut = -1, int readInterval = 100);
    bool Wait(string address, ulong value, int timeOut = -1, int readInterval = 100);
    bool Wait(string address, bool value, int timeOut = -1, int readInterval = 100);
    Task<bool> WaitAsync(string address, short value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);
    Task<bool> WaitAsync(string address, ushort value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);
    Task<bool> WaitAsync(string address, int value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);
    Task<bool> WaitAsync(string address, uint value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);
    Task<bool> WaitAsync(string address, long value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);
    Task<bool> WaitAsync(string address, ulong value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);
    Task<bool> WaitAsync(string address, bool value, int timeOut = -1, int readInterval = 100, CancellationToken token = default);

    #endregion
}