using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.WMI;


namespace TempMon
{
    class TemperatureWorker
    {
        private UpdateVisitor _updateVisitor;
        private Computer _computer;
        private WmiProvider _wmiProvider;
        private IHardware _hardware;
        public TemperatureWorker()
        {          
            _updateVisitor = new UpdateVisitor();
            _computer = new Computer();
            _computer.CPUEnabled = true;
            _wmiProvider = new WmiProvider(_computer);
            _computer.Open();
            _hardware = _computer.Hardware[0];
            //computer.Accept(updateVisitor);
            _hardware.Accept(_updateVisitor);

        }
        #region Temperature Get Function
        public int CoreOne()
        {
            Update();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\OpenHardwareMonitor", "SELECT * FROM Sensor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/0") return Convert.ToInt32(queryObj["Value"]);
            }
            return 0;
        }
        public int CoreTwo()
        {
            Update();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\OpenHardwareMonitor", "SELECT * FROM Sensor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/1") return Convert.ToInt32(queryObj["Value"]);
            }
            return 0;
        }
        public int CoreThree()
        {
            Update();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\OpenHardwareMonitor", "SELECT * FROM Sensor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/2") return Convert.ToInt32(queryObj["Value"]);
            }
            return 0;
        }
        public int CoreFour()
        {
            Update();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\OpenHardwareMonitor", "SELECT * FROM Sensor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/3") return Convert.ToInt32(queryObj["Value"]);
            }
            return 0;
        }
        #endregion
        private void Update()
        {   
            _updateVisitor.VisitHardware(_hardware);
            _wmiProvider.Update();
        }
    }
}
