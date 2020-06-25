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

    }
}