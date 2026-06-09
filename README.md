# Feng.EasyPlc

用于 PLC（可编程逻辑控制器）通信的 .NET 类库，简化工业自动化设备的数据读写操作。

## 快速入门

### 1. 安装包

```bash
dotnet add package Feng.EasyPlc
```

### 2. 创建配置文件

在项目根目录创建 `PlcSystemConfiguration.json` 文件：

```json
{
  "DeviceConfigurations": [
    {
      "Name": "PLC1",
      "IPAddress": "192.168.1.100",
      "Port": 502,
      "Protocol": "HSL:ModbusTcp",
      "TimeOut": 1000,
      "IsEnabled": true
    }
  ],
  "DataPoints": [
    {
      "PlcDeviceName": "PLC1",
      "GroupName": "温度",
      "Name": "传感器温度",
      "Address": "40001"
    },
    {
      "PlcDeviceName": "PLC1",
      "GroupName": "状态",
      "Name": "运行状态",
      "Address": "00001"
    }
  ],
  "AxisConfigurations": [
    {
      "PlcDeviceName": "PLC1",
      "AxisNumber": 1,
      "Name": "X轴",
      "CurrentPositionAddress": "40100",
      "JogAddress": "00010",
      "OriginAddress": "00011",
      "ClearAlarmAddress": "00012"
    }
  ]
}
```

### 3. 基础使用

```csharp
using Feng.EasyPlc.Services;
using NLog;

// 1. 创建 PLC 管理器
var logger = LogManager.GetLogger("PLC");
var plcManager = new PlcManager(logger);

// 2. 连接所有设备
var (success, error) = await plcManager.ConnectAsync();
if (!success)
{
    Console.WriteLine($"连接失败: {error}");
    return;
}

// 3. 读取数据
// 通过配置的数据点名称读取
var temperature = plcManager.ReadInt16("传感器温度");
var isRunning = plcManager.ReadBool("运行状态");

// 4. 写入数据
plcManager.Write("运行状态", true);

// 5. Set/Reset 操作（简化写入 1/0）
plcManager.Set("运行状态");    // 写入 1
plcManager.Reset("运行状态");  // 写入 0

// 6. 等待操作
// 等待地址值变为目标值，超时 5000ms
var waitSuccess = await plcManager.WaitAsync("运行状态", true, 5000);

// 7. 运动轴控制
// 轴点动
plcManager.AxisJogPositive("X轴");     // 正向点动
plcManager.AxisJogNegative("X轴");     // 负向点动
plcManager.AxisJog("X轴", 1);           // 自定义方向

// 获取轴当前位置
var position = plcManager.GetAxisCurrentPosition("X轴");

// 清除报警
plcManager.ClearAlarm("X轴");

// 8. 断开连接
await plcManager.DisconnectAsync();
```

### 4. 直接访问设备

如果需要直接操作设备，可以通过索引器获取：

```csharp
// 通过设备名称获取设备
var device = plcManager["PLC1"];

// 直接读取地址
var value = device.ReadInt16("40001");

// 直接写入地址
device.Write("00001", true);
```

## 依赖项

```xml
<ItemGroup>
    <PackageReference Include="HslCommunication" Version="12.3.0" />
    <PackageReference Include="NLog" Version="6.1.0" />
</ItemGroup>
```

## 版本说明

### 1.0.0.6

- 修改配置文件节点DataPoints为PointConfigurations
