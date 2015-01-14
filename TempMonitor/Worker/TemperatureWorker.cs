using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Diagnostics;
using System.Management;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.WMI;
using System.Windows.Forms;


namespace TempMonitor
{
    class TemperatureWorker
    {
        public int CoreOne, CoreTwo, CoreThree, CoreFour;
        private UpdateVisitor _updateVisitor;
        private Computer _computer;
        private WmiProvider _wmiProvider;
        private IHardware _hardware;
        public TemperatureWorker(int duration)
        {
            _updateVisitor = new UpdateVisitor();
            _computer = new Computer();
            _computer.CPUEnabled = true;
            _wmiProvider = new WmiProvider(_computer);
            _computer.Open();
            _hardware = _computer.Hardware[0];
            _hardware.Accept(_updateVisitor);
            //Timer TimerInt = new Timer();
            //TimerInt.Interval = duration;
            //TimerInt.Tick += new EventHandler(tw_tick);
            //TimerInt.Start();
        }
        public void UpdateTemperatures()
        {
            Update();
            Debug.WriteLine("tw_tick");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\OpenHardwareMonitor", "SELECT * FROM Sensor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/0")
                    CoreOne = Convert.ToInt32(queryObj["Value"]);
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/1")
                    CoreTwo = Convert.ToInt32(queryObj["Value"]);
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/2")
                    CoreThree = Convert.ToInt32(queryObj["Value"]);
                if (queryObj["identifier"].ToString() == "/intelcpu/0/temperature/3") 
                    CoreFour = Convert.ToInt32(queryObj["Value"]);
            }
        }
        private void Update()
        {   
            _updateVisitor.VisitHardware(_hardware);
            _wmiProvider.Update();
        }
    }
}
