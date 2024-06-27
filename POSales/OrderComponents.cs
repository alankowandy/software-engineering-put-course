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
    public partial class OrderComponents : Form
    {
        public OrderComponents()
        {
            InitializeComponent();
            LoadOrderComp();
        }

        public void LoadOrderComp()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvOrderList.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM OrderComponents", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvOrderList.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString());
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.btnSave.Enabled = true;
            order.btnUpdate.Enabled = false;
            order.ShowDialog();
            LoadOrderComp();
        }

        private void dgvOrderList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            string colName = dgvOrderList.Columns[e.ColumnIndex].Name;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (colName == "Edit")
                {
                    Order order = new Order();
                    order.txtID.Text = dgvOrderList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    order.txtName.Text = dgvOrderList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    order.txtPrice.Text = dgvOrderList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    order.txtDesc.Text = dgvOrderList.Rows[e.RowIndex].Cells[3].Value.ToString();

                    order.LoadOrder(order.txtID.Text);

                    order.txtID.Enabled = false;
                    order.btnSave.Enabled = false;
                    order.btnUpdate.Enabled = true;
                    order.txtName.Enabled = false;
                    order.ShowDialog();
                    LoadOrderComp();
                }
                else if (colName == "Delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this component?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var command = new SQLiteCommand("DELETE FROM OrderComponentDetails WHERE ID LIKE '" + dgvOrderList.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            //MessageBox.Show("Component has been successfully deleted!");
                        }

                        using (var command = new SQLiteCommand("DELETE FROM OrderComponents WHERE name LIKE '" + dgvOrderList.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Component has been successfully deleted!");
                        }
                        LoadOrderComp();
                    }
                }
                else if (colName == "Details")
                {
                    OrderComponentDetails orderDetail = new OrderComponentDetails();
                    orderDetail.LoadOrder(dgvOrderList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    orderDetail.ShowDialog();
                    LoadOrderComp();
                }
            }
        }
    }
}
