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
    public partial class OrderComponentDetails : Form
    {
        public OrderComponentDetails()
        {
            InitializeComponent();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LoadOrder(string orderId)
        {
            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvOrder.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Convert the string order ID to an integer
                if (int.TryParse(orderId, out int parsedOrderId))
                {
                    // Modify the SQL query based on your database schema
                    // I assume your table structure has columns like ID, ProductName, Quantity, and Price
                    using (var command = new SQLiteCommand("SELECT * FROM OrderComponentDetails WHERE ID = @ID", connection))
                    {
                        command.Parameters.AddWithValue("@ID", parsedOrderId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgvOrder.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString());
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the conversion fails
                    MessageBox.Show("Invalid Order ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
