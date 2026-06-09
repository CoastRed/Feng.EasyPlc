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

### 核心接口与实现

项目采用接口驱动的分层架构：

- **IPlcDevice** (`Contracts/IPlcDevice.cs`): PLC设备的核心接口，定义了所有PLC通信操作（连接、读写、等待等）。所有方法都支持同步和异步版本。
- **IPlcDeviceManager** (`Services/IPlcDeviceManager.cs`): PLC管理器接口，定义设备管理和索引器访问模式。

### 实现类层次

- **PlcManager** (`Services/PlcManager.cs`): PLC管理器的完整实现，提供设备管理、连接管理和数据访问接口。所有方法为 `virtual`，可被继承重写。
- **PlcDeviceManager** (`Services/PlcDeviceManager.cs`): PLC管理器的抽象基类，提供通用功能实现。
- **HslPlcDevice** (`Protocols/HslPlcDevice.cs`): 基于 HslCommunication 库的 PLC 设备实现，所有方法为 `virtual`，支持通过构造函数注入设备工厂函数。

### 协议系统

协议实现位于 `Protocols/` 目录：

- **HslPlcDevice**: 基于 HslCommunication 库的通用实现，支持通过协议字符串动态创建设备
- **MockPlc**: 用于测试的模拟PLC

协议字符串格式为 `"提供商:协议"`，例如 `"HSL:ModbusTcp"`。

添加新协议时：
1. 在 `PlcDeviceConfiguration.Protocol` 中使用 `"Provider:Protocol"` 格式
2. 在 `HslPlcDevice.InitDevice()` 中添加协议解析和设备创建逻辑
3. 或者通过 `HslPlcDevice` 构造函数的 `deviceFactory` 参数注入自定义设备创建逻辑

### 配置系统

**PlcSystemConfiguration.json** 是项目的核心配置文件，定义三个主要部分：

1. **DeviceConfigurations**: PLC设备配置列表（名称、IP、端口、协议、超时、是否启用、自定义属性）
2. **DataPoints**: 数据点配置（将地址映射为可读名称，注意：未来可能重命名为 PointConfigurations）
3. **AxisConfigurations**: 运动轴配置（轴号、当前位置地址、点动地址、原点回归地址、清除报警地址、自定义属性）

配置文件会在构建时自动复制到输出目录。

### 数据模型

- **PlcDeviceConfiguration**: PLC设备配置，包含 `Properties` 字典用于扩展属性
- **PlcPointConfiguration**: 数据点（名称到地址的映射）
- **PlcAxisConfiguration**: 运动轴配置，包含 `Properties` 字典用于扩展属性
- **PlcSystemConfiguration**: 系统配置根对象

### 索引器访问模式

PlcManager 提供了多种索引器访问方式，用于获取对应的 IPlcDevice 实例：

```csharp
// 通过设备名称
IPlcDevice device = plcManager["设备名称"];

// 通过数据点
IPlcDevice device = plcManager[dataPoint];

// 通过轴配置
IPlcDevice device = plcManager[axisConfiguration];
```

### 依赖项

- **HslCommunication** (12.3.0): 工业通信库，提供PLC协议实现
- **NLog** (6.1.0): 日志记录
- **Newtonsoft.Json**: JSON 配置反序列化

## 操作模式

### 数据读写

支持两种访问方式：
1. **通过名称**: `plcManager.ReadInt16("数据点名称")` - 使用预配置的 DataPoints
2. **直接使用对象**: `plcManager.ReadInt16(pointConfiguration)` - 传入 PlcPointConfiguration 对象

每种数据类型都有同步和异步版本。

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

## 扩展

项目支持通过扩展方法扩展 IPlcDevice 功能（见 `Extensions/PlcDeviceExtensions.cs`）。

## 注意事项

- 项目使用中文错误日志
- 通信失败时读写方法返回 null，写操作返回 false
- HslPlcDevice 所有方法都是虚方法，可被继承重写
- PlcManager 所有方法都是虚方法，可被继承重写
- 协议字符串格式必须为 `"提供商:协议"`
- 配置节 "DataPoints" 未来可能重命名为 "PointConfigurations"
