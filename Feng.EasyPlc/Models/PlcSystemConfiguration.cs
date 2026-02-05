using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feng.EasyPlc.Models
{
    public class PlcSystemConfiguration
    {
        public List<PlcConfiguration> DeviceConfigurations { get; set; } = new List<PlcConfiguration>();
        public List<PlcDataPoint> DataPoints { get; set; } = new List<PlcDataPoint>();
        public List<PlcAxisConfiguration> AxisConfigurations { get; set; } = new List<PlcAxisConfiguration>();
    }
}
