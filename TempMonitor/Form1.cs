using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.WMI;

namespace TempMonitor
{
    public partial class Form1 : Form
    {
        public int MAX = 89;
        Process ControlledProcess;
        Timer TimerInt;
        TemperatureWorker Temperature;
        public Form1()
        {
            InitializeComponent();
            ControlledProcess = new Process();
            Temperature = new TemperatureWorker(1000);      //refresh time = 1000ms
            TimerInt =  new Timer();
            TimerInt.Interval = 1000;
            TimerInt.Tick += new EventHandler(timer_tick);
            TimerInt.Start();

            statusStrip1.Text = "statusStrip1";
            statusStrip1.Items.Add("Core 1:");
            statusStrip1.Items[0].AutoSize = false;
            statusStrip1.Items[0].Size = new Size(statusStrip1.Width / 5, 1);

            statusStrip1.Items.Add("Core 2:");
            statusStrip1.Items[1].AutoSize = false;
            statusStrip1.Items[1].Size = new Size(statusStrip1.Width / 5, 1);

            statusStrip1.Items.Add("Core 3:");
            statusStrip1.Items[2].AutoSize = false;
            statusStrip1.Items[2].Size = new Size(statusStrip1.Width / 5, 1);

            statusStrip1.Items.Add("Core 4:");
            statusStrip1.Items[3].AutoSize = false;
            statusStrip1.Items[3].Size = new Size(statusStrip1.Width / 5, 1);
        }
        void timer_tick(object sender, EventArgs e)
        {
            Temperature.UpdateTemperatures();
            if (this.Visible) UpdateStatusStrip();
            else UpdateNotifyIcon(); 
            if (Temperature.CoreOne > MAX || Temperature.CoreTwo > MAX)
            {
                if (!ControlledProcess.HasExited) ControlledProcess.Kill();           
            }
            if (Temperature.CoreOne > 85 || Temperature.CoreTwo > 85)
            {
                Beep.Play(1500, 500);
            }
        }
        
        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executables (.exe)|*.exe";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;

            // Process input if the user clicked OK.
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                AppPath.Text = ofd.FileName;
            }
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            if (AppPath.Text.Length != 0)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(AppPath.Text);
                ControlledProcess.StartInfo = startInfo;
                ControlledProcess.Start();
            }
        }
        private void settings_Click(object sender, EventArgs e)
        {
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(2000, "Temperature Monitor", " ", ToolTipIcon.Info);
                this.ShowInTaskbar = false;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.ShowBalloonTip(3000);
        }
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }
        void UpdateNotifyIcon()
        {
            notifyIcon1.BalloonTipTitle = "Temperatures";
            notifyIcon1.BalloonTipText = "Core1 = " + Temperature.CoreOne.ToString()
                                       + "\nCore2 = " + Temperature.CoreTwo.ToString()
                                       + "\nCore3 = " + Temperature.CoreThree.ToString()
                                       + "\nCore4 = " + Temperature.CoreFour.ToString();
        }
        void UpdateStatusStrip()
        {
            statusStrip1.Items[0].Text = "Core 1: " + Temperature.CoreOne.ToString();
            statusStrip1.Items[1].Text = "Core 2: " + Temperature.CoreTwo.ToString();
            statusStrip1.Items[2].Text = "Core 3: " + Temperature.CoreThree.ToString();
            statusStrip1.Items[3].Text = "Core 4: " + Temperature.CoreFour.ToString();
        }
    }
}
