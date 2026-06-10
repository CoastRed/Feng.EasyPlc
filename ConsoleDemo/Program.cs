using ConsoleDemo;
using Feng.EasyPlc.Services;
using HslCommunication.Core.Device;
using Microsoft.Extensions.DependencyInjection;
using NLog;

Console.WriteLine("Hello, World!");
ILogger logger = LogManager.GetCurrentClassLogger();
//PlcManager plcManager = new PlcManager(logger);

//var r = plcManager.WriteInt16("设备初始化", 100);
//Console.WriteLine(r);

var container = new ServiceCollection();
container.AddSingleton<ILogger>(logger);
container.AddSingleton<PlcManager>();
ServiceProvider sp = container.BuildServiceProvider(); 

var plcManager = sp.GetRequiredService<PlcManager>();
var r0 = plcManager.WriteInt16("设备初始化", 100);
Console.WriteLine(r0);


Motion motion = new Motion(logger);

var r = motion.WriteInt16("设备初始化", 200);
Console.WriteLine(r);

var re = HslCommunication.Authorization.SetAuthorizationCode("d8868ab9-4494-4056-98c6-b669e2434e25");
Console.WriteLine(re);

Console.WriteLine("======原生测试======");
//HslCommunication.ModBus.ModbusTcpNet modbusTcpNet = new HslCommunication.ModBus.ModbusTcpNet("127.0.0.1");
DeviceCommunication modbusTcpNet = new HslCommunication.ModBus.ModbusTcpNet("127.0.0.1");

var r1 = modbusTcpNet.Write("16", 2);
Console.WriteLine(r1.IsSuccess);

Console.WriteLine("======EasyPlc测试======");
//var m = new PlcDeviceManager<DeviceCommunication>();
