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
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace LogIN
{
    public partial class Form1 : Form
    {
        MySqlConnection con = new MySqlConnection("Server = localhost; Database=testschema;Uid=root;Pwd=solo;");
        string network = "255.255.255.255";
        int udpPort = 9;
        int ttl = 128;
        string mac;
        Form2 f2;
        public Form1()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            con.Open();
            MySqlCommand cmd = new MySqlCommand("Select * FROM userpass where USERNAME='" + textBox1.Text + "' AND PASSWORD='" + textBox2.Text + "'", con);
            MySqlDataReader dr = cmd.ExecuteReader();
            // //  DataTable dt = new DataTable();
            //  dt.Load(dr);
            if (dr.Read())
            {
                if (dr.GetString(0) == textBox1.Text && dr.GetString(1) == textBox2.Text)
                {
                    //  if (dr.GetString(0) == dr.GetString(1))

                    this.Hide();
                     f2 = new Form2();
                    f2.Show();
                    con.Close();

                }


            }
            else
            {
                MessageBox.Show("Verifier User et Password");
                Init();
                dr.Close();
                con.Close();
            }



            dr.Close();
            con.Close();
        }

        public void Init()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            //  { if (textBox2.Text != "")
            textBox2.PasswordChar = default(char); //return '0/'



        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.PasswordChar = default(char); //'0'
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            f2 = new Form2();
            f2.Show();
        }
        /*   public static void WakeUp(string mac, string network, int udpPort , int ttl )
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

          for (int p = 0; p < packet.Length; p++)
          { MessageBox.Show(packet[p].ToString()); }


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
  private static byte[] GetMac(string mac)  // 01 05 09 C2 C1 B1
  {
      byte[] m = default(byte[]);
      String macAddress = mac;                    // Our device MAC address
      macAddress = Regex.Replace(mac, "[-|:]", "");
      for (int j = 0; j < 16; j++)
      {
          for (int k = 0; k < macAddress.Length; k ++)
          {
              String s = macAddress.Substring((k*2), 2);
              m[k] = byte.Parse(s, NumberStyles.HexNumber);
              //payloadIndex++;
          }


          /*
          int i = default(int);
          byte[] m = new byte[6];
          string s;
          s = mac.Replace(" ", "");
          s = s.Replace(":", "");
          s = s.Replace("-", "");
          for (i = 0; (i <= 5); i++)
          {
              m[i] = ("&H" + s.Substring((i * 2), 2));
          }

      }

      return m;
}

  */



    }
}