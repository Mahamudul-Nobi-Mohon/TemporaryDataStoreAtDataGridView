using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemporaryDataInsertAtDataGridView
{
    public partial class MyForm : Form
    {
        DataTable dt = new DataTable();
        int indexRows;
        public MyForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int isEdited = 0;

            if (textBoxPrice.Text == "")
            {
                textBoxPrice.Text = "0";
                
            }

            if (dataGridView1.Rows.Count > 0)
            {


                String searchValue = textBoxName.Text;
                int rowIndex = -1;
                if(isEdited == 0)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["Name"].Value != null) // Need to check for null if new row is exposed
                        {
                            rowIndex = -1;
                            if (row.Cells["Name"].Value.ToString().Equals(searchValue))
                            {
                                rowIndex = row.Index;
                                // break;
                                DataGridViewRow newDataRow = dataGridView1.Rows[rowIndex];
                                double sum = Convert.ToDouble(newDataRow.Cells[1].Value) + Convert.ToDouble(textBoxPrice.Text);
                                newDataRow.Cells[0].Value = textBoxName.Text;
                                newDataRow.Cells[1].Value = sum;
                                isEdited = 1;
                                break;
                            }
                           
                        }
                    }
                }
       
            }
            if (isEdited != 1)
            {
                dt.Rows.Add(textBoxName.Text, textBoxPrice.Text);
                this.dataGridView1.DataSource = dt;
                clearAll();
            }

            //else
            // {
            //     dt.Rows.Add(textBoxName.Text, textBoxPrice.Text);
            //     this.dataGridView1.DataSource = dt;
            //     clearAll();
            // }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Name", typeof(string)),
                new DataColumn("Price", typeof(double))});
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxName.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBoxPrice.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1)
            {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(rowIndex);
            clearAll();
            }
            else
            {
                MessageBox.Show("Please select a data. then click Remove button.");
            }
          }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxPrice.Text == "")
            {
                textBoxPrice.Text = "0";
            }
            if (dataGridView1.Rows.Count > 1)
            {
                indexRows = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow newDataRow = dataGridView1.Rows[indexRows];
                newDataRow.Cells[0].Value = textBoxName.Text;
                newDataRow.Cells[1].Value = textBoxPrice.Text;
                clearAll();
            }
            else
            {
                MessageBox.Show("Please select a data. then click update button.");
            }

        }

        private void buttonSaveInDatabase_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=MOHON-PC;Initial Catalog=Sample_30-11-2016;Integrated Security=True");
            con.Open();
            int n = 0;

            foreach (DataRow item in dt.Rows)
            {
               
                string name = dataGridView1.Rows[n].Cells[0].Value.ToString();
               double price =Convert.ToDouble( dataGridView1.Rows[n].Cells[1].Value);

                string query = "INSERT INTO Employee2 (Name,Price) VALUES ('"+ name + "', "+price+")";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.ExecuteNonQuery();

                n++;


                
            }
            con.Close();

            MessageBox.Show("Insert Successfully");
            clearAll();
        }

        private void clearAll()
        {
            textBoxName.Text = string.Empty;
            textBoxPrice.Text = string.Empty;
        }
        
    }
}
