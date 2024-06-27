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
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OrderModuleForm orderModule = new OrderModuleForm();
            orderModule.ShowDialog();
            LoadOrders();
        }

        public void LoadOrders()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvOrders.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Orders", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvOrders.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
                    }
                }
            }
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            string colName = dgvOrders.Columns[e.ColumnIndex].Name;



            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (colName == "Edit")
                {
                    OrderModuleForm order = new OrderModuleForm();
                    order.txtID.Text = dgvOrders.Rows[e.RowIndex].Cells[0].Value.ToString();
                    order.txtCustID.Text = dgvOrders.Rows[e.RowIndex].Cells[2].Value.ToString();
                    order.txtCustName.Text = dgvOrders.Rows[e.RowIndex].Cells[3].Value.ToString();
                    order.label10.Visible = false;
                    order.dateOrder.Visible = false;
                    order.label14.Visible = true;
                    order.listStatus.Enabled = true;
                    order.listStatus.Text = dgvOrders.Rows[e.RowIndex].Cells[5].Value.ToString();
                    //order.txtPrice.Text = dgvOrders.Rows[e.RowIndex].Cells[2].Value.ToString();
                    //order.txtDesc.Text = dgvOrders.Rows[e.RowIndex].Cells[3].Value.ToString();

                    //order.LoadOrder(order.txtID.Text);

                    //order.txtID.Enabled = false;
                    order.btnSave.Enabled = false;
                    order.btnUpdate.Enabled = true;
                    //order.txtName.Enabled = false;
                    order.ShowDialog();
                    LoadOrders();
                }
                else if (colName == "Delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this component?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var command = new SQLiteCommand("DELETE FROM Order WHERE ID LIKE '" + dgvOrders.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            //MessageBox.Show("Component has been successfully deleted!");
                        }

                        using (var command = new SQLiteCommand("DELETE FROM OrderDetail WHERE ID LIKE '" + dgvOrders.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Order has been successfully deleted!");
                        }
                        LoadOrders();

                    }
                }
                else if (colName == "Details")
                {
                    OrderDetail orderDetail = new OrderDetail(dgvOrders.Rows[e.RowIndex].Cells[0].Value.ToString());
                    //orderDetail.LoadOrder(dgvOrders.Rows[e.RowIndex].Cells[0].Value.ToString());
                    orderDetail.ShowDialog();
                    LoadOrders();
                }
            }
        }
    }
}
