using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Inventory
{
    public partial class OrderDetail : Form
    {
        private string ID;

        public OrderDetail(string orderId)
        {
            InitializeComponent();
            this.ID = orderId;
            LoadOrderComp();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadOrderComp()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dataGridView1.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM OrderDetail WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                        }
                    }
                }
                    
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            string colName = dataGridView1.Columns[e.ColumnIndex].Name;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (colName == "Delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this component?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var command = new SQLiteCommand("DELETE FROM OrderDetail WHERE IDC LIKE '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Component has been successfully deleted!");
                        }
                        LoadOrderComp();
                    }
                }
            }
        }
    }
    
}
