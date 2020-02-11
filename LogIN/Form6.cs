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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                if (radioButton1.Checked)
                {
                    Form2.ShutdownLegacy(textBox1.Text, ShutdownFlags.LegacyForcedShutdown, textBox2.Text, textBox3.Text, "No Comments");
                    this.Close();
                }
                else if (radioButton2.Checked)
                {
                    Form2.ShutdownLegacy(textBox1.Text, ShutdownFlags.LegacyForcedShutdown, textBox2.Text, textBox3.Text, "No Comments");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Selectionner Un Type de ShutDown !");
                }
            }
            else
            {
                MessageBox.Show("Inserer Les Donnés !");
            }
            }

        private void Button2_Click(object sender, EventArgs e)
        { this.Close(); }   
        
    }
    }

