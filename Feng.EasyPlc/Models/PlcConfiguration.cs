using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feng.EasyPlc.Models
{
    public enum PlcProtocol
    {
        ModbusTcp,
    }

    public class PlcConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        public PlcProtocol Protocol { get; set; } = PlcProtocol.ModbusTcp;
        public bool IsEnabled { get; set; } = true;
    }
}
