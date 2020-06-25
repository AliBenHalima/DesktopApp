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
using System.Management;
using System.Runtime.Serialization;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace LogIN
{
   
    public partial class Form8 : Form
    {
        Form2 f2;
        private readonly Form2 frm2;
        MySqlConnection con = new MySqlConnection("Server = localhost; Database=testschema;Uid=root;Pwd=solo;");

        public Form8(Form2 f8)
        {
            frm2 = f8;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
                try
                {
                    con.Open();
                    // MySqlCommand cmd = new MySqlCommand("Select * from data where MAC='" + textBox1.Text.ToString() + "'", con);
                    MySqlDataAdapter adap = new MySqlDataAdapter("Select * from data where MAC = '" + textBox1.Text.ToString() + "'", con);
                    DataTable dt = new DataTable(); //DataSet oDataSet = new DataSet();
                    adap.Fill(dt);
                    //MySqlDataReader dr = cmd.ExecuteReader();
                    // DataTable dt = new DataTable();
                    //   dt.Load(reader);
                    //  if (reader.HasRows)
                    if (dt.Rows.Count != 0)
                    {
                    
                    frm2.dataGridView1.DataSource = null;
                    frm2.dataGridView1.Rows.Clear();
                    frm2.dataGridView1.DataSource = dt;
                   
                    con.Close();
                        this.Close();


                    }
                    else
                    {
                        MessageBox.Show("MAC Address not found");
                        con.Close();

                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }

        
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
