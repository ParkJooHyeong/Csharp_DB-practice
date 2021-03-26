using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prac
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComd = new SqlCommand();
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            sbPanel1.Text= "DB File Name";
            sbPanel2.DropDownItems.Clear();
            sbPanel2.Text = "Table List";
            sbPanel3.Text = "Initialized";
            sqlConn.Close();
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            StreamReader sr = new StreamReader(openFileDialog.FileName);
            string buf = sr.ReadLine();
            string[] sArr = buf.Split(',');


        }
    }
}
