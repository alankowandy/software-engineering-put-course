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
    public partial class CustomerList : Form
    {
        public CustomerList()
        {
            InitializeComponent();
            LoadCustomer();
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            string colName = dgvCustomers.Columns[e.ColumnIndex].Name;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (colName == "Delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this Customer?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var command = new SQLiteCommand("DELETE FROM tbCustomers WHERE name LIKE '" + dgvCustomers.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Customer has been successfully deleted!");
                        }
                        LoadCustomer();
                    }
                }
            }
        }

        public void LoadCustomer()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            int i = 0;
            dgvCustomers.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM tbCustomers", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        i++;
                        dgvCustomers.Rows.Add(i, reader[0].ToString(), reader[1].ToString());
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.ShowDialog();
            LoadCustomer();
        }
    }
}
