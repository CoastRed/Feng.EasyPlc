using Feng.EasyPlc.Contracts;
using Feng.EasyPlc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feng.EasyPlc.Services;

public interface IPlcDeviceManager
{
    IPlcDevice this[PlcAxisConfiguration axisConfiguration] { get; }
    IPlcDevice this[PlcPointConfiguration pointConfiguration] { get; }
    IPlcDevice this[string plcName] { get; }

    Dictionary<PlcDeviceConfiguration, IPlcDevice> ConfigurationDeviceMap { get; }
    PlcSystemConfiguration PlcSystemConfig { get; }
    List<PlcAxisConfiguration> PlcAxisConfigurations { get; }
    List<PlcDeviceConfiguration> PlcDeviceConfigurations { get; }
    List<PlcPointConfiguration> PlcPointConfigurations { get; }
    

    PlcAxisConfiguration GetPlcAxisConfiguration(int axisNumber);
    PlcAxisConfiguration GetPlcAxisConfiguration(string name);
    PlcPointConfiguration GetPlcPointConfiguration(string name);
    
    PlcSystemConfiguration LoadPlcSystemConfiguration();

    void InitPLC();
}
