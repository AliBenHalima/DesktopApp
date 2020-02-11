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
    public partial class Form12 : Form
    {
        Form2 f2;
        private readonly Form2 frm2;
        public Form12(Form2 f12)
        {
            frm2 =f12;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            frm2.Ping1(textBox1.Text.ToString());
            textBox1.Clear();
            textBox1.Focus();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
