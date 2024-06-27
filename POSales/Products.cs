using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            LoadDashboard();
        }

        public void LoadDashboard()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";
            dgvProducts.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Products", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvProducts.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProduct addProduct = new AddProduct(this);
            addProduct.ShowDialog();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            string colName = dgvProducts.Columns[e.ColumnIndex].Name;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (colName == "Del")
                {
                    if (MessageBox.Show("Are you sure you want to delete this component?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var command = new SQLiteCommand("DELETE FROM Products WHERE ID LIKE '" + dgvProducts.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            //MessageBox.Show("Component has been successfully deleted!");
                        }

                        using (var command = new SQLiteCommand("DELETE FROM Products WHERE ID LIKE '" + dgvProducts.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Order has been successfully deleted!");
                        }

                        LoadDashboard();
                    }
                }
            }
        }
    }
}