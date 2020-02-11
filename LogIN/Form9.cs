using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Net;

namespace LogIN
{
    public partial class Form9 : Form
    { SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aligt\Documents\bas1.mdf;Integrated Security = True; Connect Timeout = 30");
        public Form9()
        {
            InitializeComponent();
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {/*
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into [dbo].[Table1] values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')", conn);
                int p = cmd.ExecuteNonQuery();
                if (p != 0)
                {
                    MessageBox.Show("Client bien Ajouté");
                    Form2 f2 = new Form2();
                    f2.DisplayData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erreur D'insertion");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                
            }*/
            }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
