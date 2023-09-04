using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Management;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace MBalancingPC
{
    public partial class SerialCommControl : UserControl
    {
        const int BAUDRATE = 115200;

        SerialPort CurrentPort;

        /// <summary>
        /// Port must be opend
        /// </summary>
        bool NeedToRun = false;

        public bool PortIsConnected = false;

        /// <summary>
        /// Used for GUI blinking
        /// </summary>
        bool ErrorBlinkFlag = false;

        DateTime WatchdogResetTime = DateTime.Now;

        public Action<byte[]> DataReceivedEvent;

        public SerialCommControl()
        {
            InitializeComponent();
        }

        public void Loaded()
        {
            cbSelectPort.Items.Clear();//Очистить список портов

            string tmpCtrName = this.Name;
            string number = Regex.Match(tmpCtrName, @"\d+").Value;

            cbSelectPort.Items.Add("COM9");
            cbSelectPort.SelectedItem = cbSelectPort.Items[0];

            timer1.Enabled = true;
        }

        
        private void CurrentPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdatePortState();
            AutoOpenPort();
            UpdateGUI();
        }

        private void SerialPortReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            while (CurrentPort.IsOpen && (CurrentPort.BytesToRead > 0))
            {
                int bytesToReadCnt = CurrentPort.BytesToRead;
                byte[] rxData = new byte[bytesToReadCnt];
                CurrentPort.Read(rxData, 0, bytesToReadCnt);
                DataReceivedEvent?.Invoke(rxData);
            }
        }

        private void cbSelectPort_DropDown(object sender, EventArgs e)
        {
            cbSelectPort.Items.Clear();

            List<String> port_names = GetSerialPortInfo();
            foreach (string s in port_names)
            {
                cbSelectPort.Items.Add(s);
            }
        }

        public void Start()
        {
            NeedToRun = true;

            if (PortIsConnected)
            {
                CloseCurrentPort();
            }
        }

        //Try to open port if it is needed
        void AutoOpenPort()
        {
            if (NeedToRun && (PortIsConnected == false))
            {
                OpenSerialPort();
            }
        }

        public void Stop()
        {
            NeedToRun = false;
            if (PortIsConnected)
            {
                CloseCurrentPort();
            }
        }

        public void CloseControl()
        {
            if (PortIsConnected)
            {
                CloseCurrentPort();
            }
        }


        void UpdateGUI()
        {
            ErrorBlinkFlag = !ErrorBlinkFlag;
            Color errorColor = Color.Red;
            if (ErrorBlinkFlag)
                errorColor = Color.Coral;

            if (PortIsConnected)
            {
                lblPortState.Text = "OPENED";
                lblPortState.BackColor = Color.LightGray;
            }
            else
            {
                lblPortState.Text = "CLOSED";
                lblPortState.BackColor = errorColor;
            }


            if (NeedToRun)
            {
                lblRun.Text = "RUN";
                lblRun.BackColor = Color.LightGreen;
            }
            else
            {
                lblRun.Text = "STOPPED";
                lblRun.BackColor = Color.LightGray;
            }
        }

        // ************************************************************************************************************

        public void SendPacket(byte[] data)
        {
            try
            {
                if ((CurrentPort != null) && PortIsConnected && CurrentPort.IsOpen)
                {
                    CurrentPort.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
                TryClosePortAsync(CurrentPort, 1500);
                PortIsConnected = false;
            }
        }

        // ************************************************************************************************************

        void CloseCurrentPort()
        {
            string portName = GetSerialName(cbSelectPort.Text);//short variant
            bool result = TryClosePortAsync(CurrentPort, 1500);
            if (result)
            {
                PortIsConnected = false;
                UpdateGUI();
            }
        }

        int OpenSerialPort()
        {
            string portName = GetSerialName(cbSelectPort.Text);//short variant

            try
            {
                // Create object and connect to selected port
                CurrentPort = new SerialPort(portName, BAUDRATE, Parity.None, 8, StopBits.One);
                //CurrentPort.NewLine = "\r\n";
                CurrentPort.DataReceived += SerialPortReceivedHandler;
                CurrentPort.ErrorReceived += CurrentPort_ErrorReceived;

                ThreadedOpenPort();
                //CurrentPort.ReadExisting();

                if (CurrentPort.IsOpen)
                {
                    PortIsConnected = true;
                    return 1;
                }
                PortIsConnected = false;
                return -1;
            }
            catch (Exception ex)
            {
                // Free resource
                PortIsConnected = false;
                return 0;
            } //end of try
        }

        void OpenPortThread()
        {
            try
            {
                CurrentPort.Open();
            }
            catch (ThreadAbortException)
            {
                
            }
            catch (Exception)
            {

            }
        }

        void ThreadedOpenPort()
        {
            Thread t = new Thread(OpenPortThread);
            t.Start();
            if (!t.Join(TimeSpan.FromMilliseconds(5000)))
            {
                t.Abort();
                throw new Exception("More than 5 secs.");
            }
        }


        internal class ProcessConnection
        {

            public static ConnectionOptions ProcessConnectionOptions()
            {
                ConnectionOptions options = new ConnectionOptions();
                options.Impersonation = ImpersonationLevel.Impersonate;
                options.Authentication = AuthenticationLevel.Default;
                options.EnablePrivileges = true;
                return options;
            }

            public static ManagementScope ConnectionScope(string machineName, ConnectionOptions options, string path)
            {
                ManagementScope connectScope = new ManagementScope();
                connectScope.Path = new ManagementPath(@"\\" + machineName + path);
                connectScope.Options = options;
                connectScope.Connect();
                return connectScope;
            }
        }

        /// <summary>
		/// Возвращает список портов, доступных в системе с их названиями
		/// </summary>
		/// <returns>Названия портов</returns>
        public List<String> GetSerialPortInfo()
        {
            List<String> comPortInfoList = new List<String>();

            ConnectionOptions options = ProcessConnection.ProcessConnectionOptions();
            ManagementScope connectionScope = ProcessConnection.ConnectionScope(Environment.MachineName, options, @"\root\CIMV2");

            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
            ManagementObjectSearcher comPortSearcher = new ManagementObjectSearcher(connectionScope, objectQuery);

            using (comPortSearcher)
            {
                string caption = null;
                foreach (ManagementObject obj in comPortSearcher.Get())
                {
                    if (obj != null)
                    {
                        object captionObj = obj["Caption"];
                        if (captionObj != null)
                        {
                            caption = captionObj.ToString();
                            if (caption.Contains("(COM"))
                            {
                                String comPortInfo = "";
                                comPortInfo = caption.Substring(caption.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")",
                                                                                                                                     string.Empty);
                                comPortInfo = comPortInfo + " " + caption;
                                comPortInfoList.Add(comPortInfo);
                            }
                        }
                    }
                }
            }
            return comPortInfoList;
        }

        //"COM1 - xxxxxxx" -> "COM1"
        public String GetSerialName(String info_str)
        {
            String result = "";
            int pos = info_str.IndexOf(' ');
            if (pos < 0) return info_str;
            result = info_str.Substring(0, pos);
            return result;
        }

        void UpdatePortState()
        {
            if (CurrentPort == null)
            {
                PortIsConnected = false;
                return;
            }

            PortIsConnected = CurrentPort.IsOpen;
        }

        private void ClosePort(Object o)
        {
            bool test = TryClosePort((o as TestSerialPortParam).Port);
        }


        private bool TryClosePort(SerialPort port)
        {
            if (port == null)
                return false;

            try
            {
                port.Close();
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool TryClosePortAsync(SerialPort port, int delay)
        {
            TestSerialPortParam param = new TestSerialPortParam(port);

            Thread t = new Thread(new ParameterizedThreadStart(ClosePort));

            t.Start(param);

            bool timeout = true;

            for (int i = 0; i < delay; i++)
            {
                Thread.Sleep(1);
                if (t.IsAlive == false)
                {
                    timeout = false;
                    break;
                }
            }

            if (timeout)
            {
            }

            try
            {
                if (t.IsAlive)
                {
                    return false;//порт не закрыкся
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void cbSelectPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            Start();
        }
    }//end of class

    public class TestSerialPortParam
    {
        internal bool Result;

        internal SerialPort Port;

        internal TestSerialPortParam(SerialPort port)
        {
            Port = port;
            Result = false;
        }
    }
}
