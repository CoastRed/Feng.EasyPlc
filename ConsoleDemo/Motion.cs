using Feng.EasyPlc.Models;
using Feng.EasyPlc.Protocol;
using Feng.EasyPlc.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDemo;

public class Motion : PlcManager
{
    public Motion(ILogger logger) : base(logger)
    {
        
    }

    protected override void InitPLC(List<PlcDeviceConfiguration> configurations)
    {
        foreach (PlcDeviceConfiguration configuration in configurations)
        {
            if(configuration.Protocol == "HSL:ModbusTcp")
            {
                ConfigurationDeviceMap.Add(configuration, new HslPlcDevice(configuration, null, configuration =>
                {
                    return new HslCommunication.Profinet.Siemens.SiemensS7Net(HslCommunication.Profinet.Siemens.SiemensPLCS.S1200, configuration.IPAddress);
                }));
            }
            else
            {
                throw new Exception("Not support protocol");
            }
        }
    }

}
