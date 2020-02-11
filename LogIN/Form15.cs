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
    public partial class Form15 : Form
    {
        Form2 frm2;
        public Form15( Form2 f2)
        {
            frm2 = f2;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String Mac = textBox1.Text.ToString();
            String Name = textBox2.Text.ToString();
            String IP = textBox3.Text.ToString();
            
            if ((Mac == "") || (Name == "") ||(IP== ""))
            {

                MessageBox.Show("Please Insert Missing Data");
                return;
            }
            frm2.Insert(Mac, Name, IP);
        }
    }
}
