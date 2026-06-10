# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

Feng.EasyPlc 是一个用于 PLC（可编程逻辑控制器）通信的 .NET 类库，简化工业自动化设备的数据读写操作。项目支持多目标框架（net8.0, net9.0, net10.0）。

## 常用命令

### 构建项目
```bash
dotnet build
```

### 针对特定框架构建
```bash
dotnet build -f net8.0
dotnet build -f net9.0
dotnet build -f net10.0
```

### 打包 NuGet 包
```bash
dotnet pack
```

### 运行代码格式化
```bash
dotnet format
```

## 架构说明

### 核心接口

- **IPlcDevice** (`Contracts/IPlcDevice.cs`): PLC设备的核心接口，定义所有PLC通信操作（连接、读写、等待等）。所有方法都支持同步和异步版本，包括批量读写。
- **IPlcDeviceManager** (`Services/IPlcDeviceManager.cs`): PLC管理器接口，定义设备管理和索引器访问模式。

### 实现类

项目有两种实现模式可供选择：

#### 1. PlcManager（完整实现）
`Services/PlcManager.cs` - 开箱即用的完整实现：
- 所有方法为 `virtual`，可被继承重写
- 通过 `InitPLC(PlcDeviceConfiguration)` 方法根据协议创建设备
- 支持通过继承和重写 `InitPLC` 来扩展协议支持

#### 2. PlcDeviceManager（抽象基类）
`Services/PlcDeviceManager.cs` - 需要继承实现的抽象基类：
- 提供通用功能实现
- 抽象方法 `InitPLC(PlcDeviceConfiguration)` 要求子类实现设备初始化逻辑
- 适用于需要完全自定义初始化流程的场景

### 协议系统

协议实现位于 `Protocols/` 目录：

- **HslPlcDevice** (`Protocols/HslPlcDevice.cs`): 基于 HslCommunication 库的 PLC 设备实现，所有方法为 `virtual`
  - 命名空间：`Feng.EasyPlc.Protocol`（单数）
  - 支持通过构造函数注入 `deviceFactory` 来自定义设备创建
  - `InitDevice()` 虚方法支持子类重写协议解析逻辑

- **MockPlc** (`Protocols/MockPlc.cs`): 用于测试的模拟PLC

协议字符串格式为 `"提供商:协议"`，例如 `"HSL:ModbusTcp"`。

**扩展协议支持**：
1. 继承 `PlcManager` 并重写 `InitPLC(PlcDeviceConfiguration)` 方法
2. 或通过 `HslPlcDevice` 构造函数的 `deviceFactory` 参数注入自定义设备工厂
3. 或继承 `HslPlcDevice` 并重写 `InitDevice()` 方法

### 配置系统

**PlcSystemConfiguration.json** 是项目的核心配置文件，定义三个主要部分：

1. **DeviceConfigurations**: PLC设备配置列表（名称、IP、端口、协议、超时、是否启用、自定义属性）
2. **PointConfigurations**: 数据点配置（将地址映射为可读名称）
3. **AxisConfigurations**: 运动轴配置（轴号、当前位置地址、点动地址、原点回归地址、清除报警地址、自定义属性）

配置文件会在构建时自动复制到输出目录。

### 数据模型

- **PlcDeviceConfiguration**: PLC设备配置，包含 `Properties` 字典用于扩展属性
- **PlcPointConfiguration**: 数据点（名称到地址的映射），包含 `PlcDeviceName` 关联设备
- **PlcAxisConfiguration**: 运动轴配置，包含 `Properties` 字典用于扩展属性
- **PlcSystemConfiguration**: 系统配置根对象

### 索引器访问模式

PlcManager 提供了多种索引器访问方式，用于获取对应的 IPlcDevice 实例：

```csharp
// 通过设备名称
IPlcDevice device = plcManager["设备名称"];

// 通过数据点配置
IPlcDevice device = plcManager[pointConfiguration];

// 通过轴配置
IPlcDevice device = plcManager[axisConfiguration];
```

### 依赖项

- **HslCommunication** (12.3.0): 工业通信库，提供PLC协议实现（需要授权用于生产环境）
- **NLog** (6.1.0): 日志记录
- **Newtonsoft.Json**: JSON 配置反序列化

## 操作模式

### 数据读写

支持两种访问方式：
1. **通过名称**: `plcManager.ReadInt16("数据点名称")` - 使用预配置的 PointConfigurations
2. **直接使用对象**: `plcManager.ReadInt16(pointConfiguration)` - 传入 PlcPointConfiguration 对象

每种数据类型都有同步和异步版本，包括批量读写（如 `ReadInt16(address, length)`）。

### Set-Reset 操作

简化布尔值操作：
- `Set/Reset`: 将数据点写入 1 或 0
- `SetAsync/ResetAsync`: 异步版本

### 运动轴控制

通过轴配置简化运动控制：
- `AxisJogPositive/AxisJogNegative/AxisJog`: 轴点动控制（支持按名称或轴号访问）
- `GetAxisCurrentPosition`: 获取轴当前位置
- `ClearAlarm`: 清除轴报警

### 等待操作

Wait 系列方法用于轮询等待指定地址值变为目标值：
- `timeOut`: 超时时间（毫秒），-1 表示无限等待
- `readInterval`: 轮询间隔（毫秒），默认 100ms
- 异步版本支持 `CancellationToken`

## 扩展性设计

### 添加自定义协议

**方法一：继承 PlcManager**
```csharp
public class MyPlcManager : PlcManager
{
    protected override IPlcDevice InitPLC(PlcDeviceConfiguration configuration)
    {
        if (configuration.Protocol == "Custom:MyProtocol")
        {
            return new MyCustomDevice(configuration, _logger);
        }
        return base.InitPLC(configuration);
    }
}
```

**方法二：使用设备工厂**
```csharp
Func<PlcDeviceConfiguration, IReadWriteDevice> deviceFactory = (config) =>
{
    if (config.Protocol == "HSL:SiemensS71200")
    {
        return new SiemensS7Net(config.IPAddress, 102, SiemensPLCS.S1200);
    }
    return null;
};

var device = new HslPlcDevice(configuration, logger, deviceFactory);
```

### 扩展方法

项目支持通过扩展方法扩展 IPlcDevice 功能（见 `Extensions/PlcDeviceExtensions.cs`）。

## 注意事项

- 项目使用中文错误日志
- 通信失败时读写方法返回 null，写操作返回 false
- HslCommunication 库需要授权用于生产环境
- PlcManager 和 HslPlcDevice 的所有方法都是虚方法，可被继承重写
- 协议字符串格式必须为 `"提供商:协议"`
- 配置节名为 `PointConfigurations`（不是 DataPoints）
