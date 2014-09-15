using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace TempMon
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            bool ProcessError = true;
            Process processToKill = new Process();
            
                
            while (ProcessError)
            {
                try
                {
                    processToKill = Process.GetProcessesByName("Firefox")[0];
                    ProcessError = false;
                }
                catch (IndexOutOfRangeException e)
                {
                    ProcessError = true;
                }
                Thread.Sleep(5000);
            }

            StreamWriter Log = new StreamWriter(File.Open(Environment.CurrentDirectory + @"\log.txt", FileMode.Append));
            Log.AutoFlush = true;
            TemperatureWorker Temperature = new TemperatureWorker();

            while (true)
            {
                Log.WriteLine("-----------------------------------");
                Log.WriteLine(DateTime.Now);
                Log.WriteLine("[+] Core One Temperature = " + Temperature.CoreOne());
                Log.WriteLine("[+] Core Two Temperature = " + Temperature.CoreTwo());
                if (Temperature.CoreOne() > 91 || Temperature.CoreTwo() > 91)
                {
                    Log.WriteLine("[*] Killing Process: " + processToKill.ProcessName);
                    processToKill.Kill();
                }
                Thread.Sleep(1000);
            }
            Console.ReadLine();
        }
    }
}
