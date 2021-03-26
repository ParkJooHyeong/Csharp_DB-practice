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
            sbTables.DropDownItems.Clear();
            sbTables.Text = "Table List";
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
            for(int i = 0; i < sArr.Length; i++)
            {
                dataGridView1.Columns.Add(sArr[i], sArr[i]);

            }
            while (true)
            {
                buf = sr.ReadLine();
                if (buf == null) break;
                sArr = buf.Split(',');
                dataGridView1.Rows.Add(sArr);

            }
            sr.Close();


        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
            string buf = "";
            for(int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                buf += dataGridView1.Columns[i].HeaderText;
                if (i < dataGridView1.ColumnCount - 1) buf += ",";

            }
            sw.Write(buf + "\r\n");

            for(int k = 0; k < dataGridView1.RowCount; k++)
            {
                buf = "";
                for(int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    buf += dataGridView1.Rows[k].Cells[i].Value;
                    if (i < dataGridView1.ColumnCount - 1) buf += ",";
                }
                sw.Write(buf + "\r\n");
            }
            sw.Close();
        }

        string strConn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=;Integrated Security=True;Connect Timeout=30";

        private void openDB_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            try {
                string[] arr = strConn.Split(';');
                strConn = $"{arr[0]};{arr[1]}{openFileDialog.FileName};{arr[2]};{arr[3]}";
                sqlConn.ConnectionString = strConn;
                sqlConn.Open();
                sqlComd.Connection = sqlConn;
                sbPanel1.Text = openFileDialog.SafeFileName;
                sbPanel1.BackColor = Color.Green;

                DataTable dt = sqlConn.GetSchema("Tables");
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    sbTables.DropDownItems.Add(dt.Rows[i].ItemArray[2].ToString());
                }
                sbPanel3.Text = "Success";
                sbPanel3.BackColor = Color.Blue;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                sbPanel3.Text = "Error";
                sbPanel3.BackColor = Color.Red;
            }

        }

        private void sbTables_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            sbTables.Text = e.ClickedItem.Text;
           
        }

        private string GetToken(int i, string src, char del)
        {
            string[] sArr = src.Split(del);
            return sArr[i];
        }

        private void ClearGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void RunSql(string sql)
        {
            try
            {
                string ss = GetToken(0, sql.Trim().ToLower(), ' ');
                sqlComd.CommandText = sql;
                if (ss == "select")
                {
                    ClearGrid();
                    SqlDataReader dr = sqlComd.ExecuteReader();
                    for (int j = 0; j < dr.FieldCount; j++)
                    {
                        dataGridView1.Columns.Add(dr.GetName(j), dr.GetName(j));
                    }

                    for (int i = 0; dr.Read(); i++)
                    {
                        object[] oArr = new object[dr.FieldCount];
                        dr.GetValues(oArr);
                        dataGridView1.Rows.Add(oArr);
                    }
                }
                else
                {
                    sqlComd.ExecuteNonQuery();
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = ".";
        }

        private void updateTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView1.RowCount; i++)
            {
                for(int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].ToolTipText == ".")
                    {
                        string TableName = sbTables.Text;
                        string currentCellHeader = dataGridView1.Columns[j].HeaderText;
                        object currentCellValue = dataGridView1.Rows[i].Cells[j].Value;
                        string idHeader = dataGridView1.Columns[0].HeaderText;
                        object id = dataGridView1.Rows[i].Cells[0].Value;
                      
                        string s = $"update {TableName} set {currentCellHeader}='{currentCellValue}' where {idHeader}={id}";
                    }
                }
            }
        }
    }

}
