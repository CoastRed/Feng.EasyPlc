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

- **IPlcDevice** (`Contracts/IPlcDevice.cs`): PLC设备的核心接口，定义了所有PLC通信操作（连接、读写、等待等）
- **PlcManager** (`Services/PlcManager.cs`): PLC管理器，负责管理多个PLC设备并提供统一的数据访问接口

### 协议实现

协议实现位于 `Protocols/` 目录：

- **ModbusTcp**: Modbus TCP 协议实现
- **MockPlc**: 用于测试的模拟PLC

添加新协议时需要：
1. 实现 `IPlcDevice` 接口
2. 在 `PlcConfiguration.Protocol` 枚举中添加协议类型
3. 在 `PlcManager.InitPLC()` 中添加协议实例化逻辑

### 配置系统

**PlcSystemConfiguration.json** 是项目的核心配置文件，定义三个主要部分：

1. **DeviceConfigurations**: PLC设备配置列表（名称、IP、端口、协议）
2. **DataPoints**: 数据点配置（将地址映射为可读名称）
3. **AxisConfigurations**: 运动轴配置（用于位置控制、点动等操作）

PlcManager 在初始化时会自动加载此文件，支持通过名称访问配置的设备和数据点，无需直接操作地址字符串。

### 数据模型

- **PlcConfiguration**: PLC设备配置
- **PlcDataPoint**: 数据点（名称到地址的映射）
- **PlcAxisConfiguration**: 运动轴配置（当前位置地址、点动地址、原点回归地址）

### 依赖项

- **HslCommunication** (12.3.0): 工业通信库，提供PLC协议实现
- **NLog** (6.1.0): 日志记录

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

## 操作模式

### 数据读写

支持两种访问方式：
1. **通过名称**: `plcManager.ReadInt16("数据点名称")` - 使用预配置的 DataPoints
2. **直接使用对象**: `plcManager.ReadInt16(dataPoint)` - 传入 PlcDataPoint 对象

### 运动轴控制

通过轴配置简化运动控制：
- `AxisJogPositive/AxisJogNegative`: 轴点动
- `GetAxisCurrentPosition`: 获取轴当前位置

### 等待操作

Wait 系列方法用于轮询等待指定地址值变为目标值：
- `timeOut`: 超时时间（毫秒），-1 表示无限等待
- `readInterval`: 轮询间隔（毫秒），默认 100ms

## 注意事项

- 项目使用中文错误日志
- 通信失败时读写方法返回 null，写操作返回 false
- PlcSystemConfiguration.json 会在构建时自动复制到输出目录
