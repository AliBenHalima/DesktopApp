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
    public partial class Form13 : Form
    {
        Form2 frm2;
        public Form13(Form2 f2)

        {
            frm2 = f2;
            InitializeComponent();
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String MAC = textBox1.Text.ToString();
            if (MAC == "") 
            {

                MessageBox.Show("Please Insert Missing Data");
                return;
            }
            frm2.Delete(MAC);
        }
    }
}
