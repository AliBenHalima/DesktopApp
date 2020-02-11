using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogIN
{
    public partial class Form5 : Form
    {
        public static string mac_f5;
        Form5 f5;
       

        public Form5()
        {
            
            InitializeComponent();
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        public void Button1_Click(object sender, EventArgs e)
        {
            
            mac_f5 = textBox6.Text.ToString();
            Form2.GetMac1(mac_f5); //5C-B9-01-F5-4C-4D
            Form2.WakeUp(mac_f5, Form2.network, Form2.udpPort, Form2.ttl);
            this.Hide();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
