using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Net;
using System.Management;
using System.Runtime.Serialization;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
using System.Management;
using System.Diagnostics;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;



namespace LogIN
{
    public enum ShutdownFlags
    {
        Logoff = 0,
        ForcedLogoff = 4,
        Shutdown = 1,
        ForcedShutdown = 5,
        Reboot = 2,
        ForcedReboot = 6,
        PowerOff = 8,
        ForcedPoweroff = 12,
        Sleep = 100,
        Hibernate = 101,
        LegacyShutdown = 102,
        LegacyForcedShutdown = 103,
        LegacyReboot = 104,
        LegacyForcedReboot = 105
    }
    public enum EventId : int
    {
        WakeUp = 1,
        Shutdown = 2,
        Up = 100,
        Down = 101,
        Error = 1000
    }
    public partial class Form2 : Form
    {
        MySqlConnection con = new MySqlConnection("Server = localhost; Database=testschema;Uid=root;Pwd=solo;");
        public static string network = "255.255.255.255";
        public static int udpPort = 9;
        public static int ttl = 128;
        string mac;
        int index = 0;
        public static string DataGrid_MAC, DataGrid_IP, DataGrid_IP1, DataGrid_MAC1;
        static int upCount = 0;
        static object lockObj = new object();
        const bool resolveNames = true;
        string host;
        private bool DisplayDataCalled = false;
        //  public DataGridView dataGridView1;
        Form5 f5;
        Form6 f6;
        Form7 f7;
        Form12 f12;
        Form13 f13;
        Form14 f14;
        public Form8 f8;
        public EventLog eventLog1;
        int j;



        public Form2()
        {
            eventLog1 = new EventLog();

            InitializeComponent();




        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

            this.eventLog1.EnableRaisingEvents = true;
            this.eventLog1.Log = "System";
            this.eventLog1.Source = "USER32";
            this.eventLog1.SynchronizingObject = this;
            this.eventLog1.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.eventLog1_EntryWritten);
            EventLogEntry entry = e.Entry;
            if (e.Entry.EventID == 1074)
            {
                File.AppendAllText(@"c:\message.txt", entry.Message);
            }
        }
        public DataGridView datagridview1
        {
            get { return this.datagridview1; }
            set { this.datagridview1 = value; }
        }
        public static byte[] GetMac1(string mac)  // 01 05 09 C2 C1 B1
        {
            int index = 0;
            byte[] m = new byte[1024];
            String macAddress;                    // Our device MAC address
            macAddress = Regex.Replace(mac, "[-|:]", "");
            for (int j = 0; j < 16; j++)
            {
                for (int k = 0; k < macAddress.Length; k += 2)
                {

                    String s = macAddress.Substring(k, 2);
                    m[index] = byte.Parse(s, NumberStyles.HexNumber);
                    index++;
                    //payloadIndex++;
                }
            }
        
            return m;

        }
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            GetMac1(DataGrid_MAC); //5C-B9-01-F5-4C-4D
            WakeUp(DataGrid_MAC, network, udpPort, ttl);

        }

        public static string getIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry("127.0.0.1"); //Dns.GetHostName()
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    MessageBox.Show(localIP);
                }
            }
            return localIP;

        }

        public void Pinger()
        {

            string ipBase = getIPAddress();
            string[] ipParts = ipBase.Split('.');
            ipBase = ipParts[0] + "." + ipParts[1] + "." + ipParts[2] + ".";
            for (int i = 1; i < 255; i++)
            {
                string ip = ipBase + i.ToString();

                Ping p = new Ping();
                p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                p.SendAsync(ip, 100, ip);
            }
            Console.ReadLine();

        }

        static void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                if (resolveNames)
                {
                    string name;
                    try
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                        name = hostEntry.HostName;
                    }
                    catch (SocketException ex)
                    {
                        name = "?";
                    }
                    MessageBox.Show(String.Format("{0} ({1}) is up: ({2} ms)", ip, name, e.Reply.RoundtripTime));
                }
                else
                {
                    MessageBox.Show(String.Format("{0} is up: ({1} ms)", ip, e.Reply.RoundtripTime));
                }
                lock (lockObj)
                {
                    upCount++;
                }
            }
            else if (e.Reply == null)
            {
                MessageBox.Show(String.Format("Pinging {0} failed. (Null Reply object?)", ip));
            }
        }












        public static void WakeUp(string mac, string network, int udpPort, int ttl)
        {
            UdpClient client = default(UdpClient);
            IPEndPoint localEndPoint = default(IPEndPoint);
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces(); //GetAllNetworkInterfaces retourne un tableau qui contint une instance de cet classe

            byte[] packet = new byte[102];
            int i = default(int), j = default(int);
            byte[] macBytes = default(byte[]);

            try
            {
                macBytes = Form2.GetMac1(mac);
                // WOL packet contains a 6-bytes header and 16 times a 6-bytes sequence containing the MAC address.
                // packet =  byte(17 * 6)
                // Header of 0xFF 6 times.

                for (i = 0; i <= 5; i++)
                    packet[i] = 255;

                // Body of magic packet contains the MAC address repeated 16 times.

                for (i = 1; i <= 16; i++)
                {
                    for (j = 0; j <= 5; j++)
                        packet[(i * 6) + j] = macBytes[j];
                }
                /*
                for (int p = 0; p < packet.Length; p++)
                { MessageBox.Show(packet[p].ToString()); }
                */

                foreach (NetworkInterface adapter in nics)
                {
                    // Only display informatin for interfaces that support IPv4. 
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                        continue;

                    //  UnicastIPAddressInformationCollection addresses = adapter.GetIPProperties.UnicastAddresses;
                    UnicastIPAddressInformationCollection addresses = adapter.GetIPProperties().UnicastAddresses;

                    foreach (UnicastIPAddressInformation address in addresses)
                    {
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            localEndPoint = new IPEndPoint(IPAddress.Parse(address.Address.ToString()), udpPort);
                            //  Debug.WriteLine("Interface: " + localEndPoint.ToString());

                            try
                            {
                                client = new UdpClient();
                                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                                client.ExclusiveAddressUse = false;
                                client.Client.Bind(localEndPoint);
                                client.Connect(network, udpPort);
                                client.EnableBroadcast = true;
                                client.Ttl = (short)ttl;

                                client.Send(packet, packet.Length);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void AfficherListeDePCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayData();

        }

        public void DisplayData()
        {
            DisplayDataCalled = true;
            con.Open();
            MySqlCommand cmd1 = new MySqlCommand("Select * FROM data ", con);
            MySqlDataReader dr = cmd1.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
          
            con.Close();
        }
        
        private void ActualiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            if (DisplayDataCalled = true)
            { DisplayData(); }
            else
            {
                return;
            }

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                ContextMenu m = new ContextMenu();
                MenuItem item1 = new MenuItem("Wake");
                MenuItem item2 = new MenuItem("ShutDown");
                MenuItem item3 = new MenuItem("Ping");
                MenuItem item4 = new MenuItem("Delete");

                m.MenuItems.Add(item1);
                m.MenuItems.Add(item2);
                m.MenuItems.Add(item3);
                m.MenuItems.Add(item4);
                item1.Click += Onitem1Clicked;
                item2.Click += Onitem2Clicked;
                item3.Click += Onitem3Clicked;
                item4.Click += Onitem4Clicked;


                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                }

                m.Show(dataGridView1, new Point(e.X, e.Y));

            }
        }
        public void Onitem1Clicked(object sender, EventArgs e)
        {
            GetMac1(DataGrid_MAC);
            WakeUp(DataGrid_MAC, network, udpPort, ttl);
           
        }

        public void Onitem2Clicked(object sender, EventArgs e)
        {
            //Shutdown1(DataGrid_IP, 0); // this works !!
            // Shutdown2(DataGrid_IP);
            f7 = new Form7();
            f7.Show();

        }

        public void Onitem3Clicked(object sender, EventArgs e)
        {

            Ping1(DataGrid_IP);

        }
        public void Onitem4Clicked(object sender, EventArgs e)
        {

            Delete(DataGrid_MAC);

        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dataGridView1.CurrentRow.Selected = true;
                DataGrid_MAC = dataGridView1.Rows[e.RowIndex].Cells["MAC"].FormattedValue.ToString();
                DataGrid_IP = dataGridView1.Rows[e.RowIndex].Cells["IP_Address"].FormattedValue.ToString();
                textBox1.Text = DataGrid_MAC;
                textBox2.Text = DataGrid_IP;


            }

            else if (e.RowIndex < 0)
            {
                // RowIndex : the index of the row that contains the CELL 
                // return 's -1  if there is no  Row

                return;
            }


        }
            
            public void Shutdown(string strComputerNameInLan)
        {
            string shutdownString = @"/c shutdown /l \\" + strComputerNameInLan;
            try
            {
                ProcessStartInfo psiOpt = new ProcessStartInfo("cmd.exe", shutdownString);
                psiOpt.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
                psiOpt.RedirectStandardOutput = true;
                psiOpt.UseShellExecute = false;
                psiOpt.CreateNoWindow = true;
                Process procCommand = Process.Start(psiOpt);
                procCommand.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Shutdown2(string ip)
        {
            ManagementScope scope = new ManagementScope("\\\\" + ip + "\\root\\cimv2"); // \\root\\cimv2 where WMI classes are!
            scope.Connect();
            //Query system for Operating System information  
            ObjectQuery oq = new ObjectQuery("SELECT * FROM Win32_OperatingSystem"); //Chercher dans le Table win32...
            ManagementObjectSearcher query = new ManagementObjectSearcher(scope, oq);//prend iun objectQuery object et un management scope object 
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject obj in queryCollection)
            {
                MessageBox.Show("Computer Name : " + obj["csname"] + " ");
                obj.InvokeMethod("ShutDown2", null); //shutdown  
            }
        }
       
        public void Shutdown1(string ShutdownIP, int TimeToShutdown) // works !!!
        {
            Process commandProcess = new Process();
            try
            {
                commandProcess.StartInfo.FileName = "cmd.exe";
                commandProcess.StartInfo.UseShellExecute = false;
                commandProcess.StartInfo.CreateNoWindow = true;
                commandProcess.StartInfo.RedirectStandardError = true;
                commandProcess.StartInfo.RedirectStandardInput = true;
                commandProcess.StartInfo.RedirectStandardOutput = true;
                commandProcess.Start();
                commandProcess.StandardInput.WriteLine("shutdown /s /m " + ShutdownIP + " /t " + TimeToShutdown + " /f ");
                commandProcess.StandardInput.WriteLine("exit");
                for (; !commandProcess.HasExited;)//wait executed  
                {
                    System.Threading.Thread.Sleep(1);
                }
                //error output  
                string tmpout = commandProcess.StandardError.ReadToEnd();
                string tmpout1 = commandProcess.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (commandProcess != null)
                {
                    commandProcess.Dispose();
                    commandProcess = null;
                }
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to Shutdown all Computers ? ", "Shutdown", MessageBoxButtons.YesNo);
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {

                try
                {

                    if (dialogResult == DialogResult.Yes)
                    {
                        dataGridView1.Rows[i].Selected = true;
                        DataGrid_IP1 = dataGridView1.Rows[i].Cells["IP_Address"].Value.ToString();
                        //selectedrowindex = dataDridview1.SelectedCells[0].RowIndex;
                        ShutdownLegacy(DataGrid_IP1, ShutdownFlags.LegacyForcedShutdown,/* userID */ "",/* Password */  "", "");
                        dataGridView1.ClearSelection();
                        //  dataGridView1.Rows[i+1].Selected = true;
                    }

                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
           
        }

        public void Ping1(string host1)
        {
            var ping = new Ping();
            var options = new PingOptions();
            options.DontFragment = true; // Used to control how Ping data packets are transmitted.

            // { DontFragment = true };

            //just need some data. this sends 10 bytes.
            var buffer = Encoding.ASCII.GetBytes(new string('z', 10));
           
            try
            {
                int index = dataGridView1.SelectedRows[j].Index;
                var reply = ping.Send(host1, 60, buffer, options); //60 time-out 
                if (reply == null)
                {
                    MessageBox.Show("Reply was null");

                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Red;

                    return;
                }

                if (reply.Status == IPStatus.Success)
                {
                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Green;

                    MessageBox.Show("Ping was successful.");
                    // dataGridView1.CurrentCellStyle.SelectionBackColor = Color.Green;


                }
                else
                {
                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Red; MessageBox.Show("Ping failed.");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Ping1_WithnNoMessageBox(string host1_)
        {
            var ping = new Ping();
            var options = new PingOptions();
            options.DontFragment = true; // Used to control how Ping data packets are transmitted.

            // { DontFragment = true };

            //just need some data. this sends 10 bytes.
            var buffer = Encoding.ASCII.GetBytes(new string('z', 10));
           
            try
            {
                var reply = ping.Send(host1_, 60, buffer, options); //60 time-out 
                if (reply == null)
                {
                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Red;
                    //  dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.Red;
                    return;
                }

                if (reply.Status == IPStatus.Success)
                {
                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Green;
                    // dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.Green;


                    // dataGridView1.CurrentCellStyle.SelectionBackColor = Color.Green;

                }
                else
                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Red;
                // dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.Red;


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Ping1(DataGrid_IP);

        }

        private void Button1_Click(object sender, EventArgs e)
        { // getIPAddress();
            for (j = 0; j < dataGridView1.RowCount; j++)
            {
                try
                {
                    dataGridView1.Rows[j].Selected = true;
                    DataGrid_MAC1 = dataGridView1.Rows[j].Cells["MAC"].FormattedValue.ToString();
                    GetMac1(DataGrid_MAC1);
                    WakeUp(DataGrid_MAC1, network, udpPort, ttl);
                    dataGridView1.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*  foreach (DataGridViewRow r in dataGridView1.Rows)
              {


                  r.DefaultCellStyle.BackColor = Color.Red;

              }*/
        }

        private void SinglePCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f5 = new Form5();
            f5.Show();
            //  singlePCToolStripMenuItem.Click += Button1_Click;
            /* GetMac1(textBox6.Text); //5C-B9-01-F5-4C-4D
             WakeUp("5C-B9-01-F5-4C-4D", network, udpPort, ttl);*/
        }

        private void SinglePCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            f6 = new Form6();
            f6.Show();
        }

        private void MultipleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f8 = new Form8(this);
            f8.Show();
        }

        private void InsererToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void InsererToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //   Form9 f9 = new Form9();
            //f9.Show();
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///Form10 f10 = new Form10();
            // f10.Show();
        }

        private void ModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f12 = new Form12(this);
            f12.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
             f13 = new Form13(this);
            f13.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {

            // List<string> mylist = new List<string> { };
            // mylist.Add
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                try
                {
                    dataGridView1.Rows[i].Selected = true;
                    DataGrid_IP1 = dataGridView1.Rows[i].Cells["IP Address"].FormattedValue.ToString();
                    //selectedrowindex = dataDridview1.SelectedCells[0].RowIndex;
                    Ping1(DataGrid_IP1);
                    dataGridView1.ClearSelection();
                    //  dataGridView1.Rows[i+1].Selected = true;
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message);
                }

            }
            // dataGridView1.Rows[i].Style.BackColor = Color.Red;

        }

        private void updateToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            f14  = new Form14(this);
            f14.Show();
        }

        public static void ShutdownLegacy(string host, ShutdownFlags flags, string userid = "", string password = "", string message = "WARNINIGGG" +
            "This PC will Shutdown soon")
        {
            long dwDelay = default(long);
            StringBuilder shutdownCommand = new StringBuilder("/c ");
            StringBuilder shutdownCommand1 = new StringBuilder(" /c ");
            Process p = new Process();
            Process p1 = new Process();
            string error = string.Empty;

            try
            {
                dwDelay = 20;

                if ((string.IsNullOrEmpty(host)))
                    return;

                if ((!string.IsNullOrEmpty(userid)))
                    shutdownCommand.AppendFormat(@"net use \\{0}\IPC$ ""{1}"" /User:{2} & ", host, password, userid);

                shutdownCommand.AppendFormat(@"shutdown /m \\{0} /t {1} ", host, dwDelay);


                switch (flags)
                {
                    case var @case when @case == ShutdownFlags.LegacyShutdown:
                        {
                            shutdownCommand.Append(" / s ");

                            break;
                        }

                    case var case1 when case1 == ShutdownFlags.LegacyForcedShutdown:
                        {
                            shutdownCommand.Append(" /s /f ");

                            break;
                        }

                    case var case2 when case2 == ShutdownFlags.LegacyReboot:
                        {
                            shutdownCommand.Append(" /r ");

                            break;
                        }

                    case var case3 when case3 == ShutdownFlags.LegacyForcedReboot:
                        {
                            shutdownCommand.Append(" /r /f ");

                            break;
                        }
                }

                //   if (!string.IsNullOrEmpty(message))
                //    shutdownCommand.AppendFormat(" /c \"{0}\"", message);

                ProcessStartInfo pi = new ProcessStartInfo()
                {
                    Arguments = shutdownCommand.ToString(),
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = false
                };
                p.StartInfo = pi;
                p.Start();

                DialogResult dialogResult = MessageBox.Show(" This Computer will shutdown in 20s , Do you want to Cancel The Shutdown ? ", "Shutdown", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    shutdownCommand1.AppendFormat(@"net use \\{0}\IPC$ ""{1}"" /User:{2} & ", host, password, userid);
                    shutdownCommand1.AppendFormat(@"shutdown /m \\{0} /a", host);

                    ProcessStartInfo pi1 = new ProcessStartInfo()
                    {
                        Arguments = shutdownCommand1.ToString(),
                        FileName = "cmd.exe",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = false
                    };
                    p1.StartInfo = pi1;
                    p1.Start();
                    //   p1.StartInfo = pi1;
                    // p1.Start();

                }
                else
                {
                    // Display process statistics until
                    // the user closes the program.
                    do
                    {
                        if (!p.HasExited)
                        {

                            // Refresh the current process property values.
                            p.Refresh();
                            Debug.WriteLine("");

                            // Display current process statistics.

                            Debug.WriteLine("  Process: " + p.StartInfo.FileName);
                            Debug.WriteLine("Arguments: " + p.StartInfo.Arguments.ToString());
                            Debug.WriteLine("-------------------------------------");
                            if (p.Responding)
                                Debug.WriteLine("Status = Running");
                            else
                                Debug.WriteLine("Status = Not Responding");
                        }

                        error += p.StandardError.ReadToEnd();
                        Debug.WriteLine("error: " + error.ToString());
                    }
                    while (!p.WaitForExit(1000));

                    Debug.WriteLine("");
                    Debug.WriteLine("Process exit code: {0}", p.ExitCode);

                    if ((p.ExitCode != 0))
                        throw new Exception(error);
                }
            }
            finally
            {
                if (!(p == null))
                    p.Close();

            }

        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form15 f15 = new Form15(this);
            f15.Show();
        }

        public void Delete(String MAC)
        {
            con.Open();
            
            MySqlCommand cmd = new MySqlCommand("Delete from data where MAC='"+ MAC + "'", con);
            int result= cmd.ExecuteNonQuery();
            con.Close();
            DisplayData();
        }
        public void Update(String New_Name,String MAC)
        {
            con.Open();

            MySqlCommand cmd = new MySqlCommand("Update data set Name='"+ New_Name + "' where MAC='" + MAC + "'", con);
            int result = cmd.ExecuteNonQuery();
            con.Close();
            DisplayData();
        }
        public void Insert(String MAC,String New_Name,String IPAddress)
        {
            con.Open();

            MySqlCommand cmd = new MySqlCommand("Insert Into data Values('"+ MAC+"','"+ New_Name + "','"+ IPAddress +"')", con);
            int result = cmd.ExecuteNonQuery();
            con.Close();
            DisplayData();
        }




    }
}