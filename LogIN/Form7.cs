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
    public partial class Form7 : Form
    {
        
        public Form7()
        {
            
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        { if (textBox1.Text != "" && textBox2.Text != "")
            {
                Form2.ShutdownLegacy(Form2.DataGrid_IP, ShutdownFlags.LegacyForcedShutdown, textBox1.Text, textBox2.Text, "No Comments");
                this.Close();
            }
            else
            {
                MessageBox.Show("Enter les Infomations d'authetification !");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }
}
