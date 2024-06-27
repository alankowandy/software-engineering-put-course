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
    public partial class OrderModuleForm : Form
    {
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadOrderComp();
            txtCustID.Enabled = false;
            txtCustName.Enabled = false;
            txtCompID.Enabled = false;
            txtCompName.Enabled = false;
            txtCompPrice.Enabled = false;
            txtCompTotal.Enabled = false;
            listStatus.Enabled = false;
            if (string.IsNullOrEmpty(txtID.Text))
            {
                txtID.Enabled = false;
                txtID.Text = Convert.ToString(LoadID());
            }
            listStatus.Items.Add("New");
            listStatus.Items.Add("Pending");
            listStatus.Items.Add("Finished");
            listStatus.Items.Add("Shipping");
            listStatus.Items.Add("Closed");

            listStatus.Text = "New";
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtCustID.Clear();
            txtCustName.Clear();
            txtCompID.Clear();
            txtCompName.Clear();
            txtIDC.Clear();
            txtSeachCustomer.Clear();
            txtSearchComponent.Clear();
        }

        public void LoadOrderCompSearch()
        {
            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvOrderList.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "SELECT * FROM OrderComponents WHERE id || name || price || desc LIKE '%" + txtSearchComponent.Text + "%'";
                        Console.WriteLine("Query: " + query);  // Debugging statement

                        using (var command = new SQLiteCommand(query, connection, transaction))
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgvOrderList.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString());
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception or log it
                        Console.WriteLine("Error: " + ex.Message);  // Debugging statement
                        transaction.Rollback();
                    }
                }
            }
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

        public void LoadCustomerSearch()
        {
            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            int i = 0;
            dgvCustomers.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbCustomers WHERE id || name LIKE '%" + txtSeachCustomer.Text + "%'";

                using (var command = new SQLiteCommand(query, connection))
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

        private void txtSeachCustomer_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerSearch();
        }

        private void txtSearchComponent_TextChanged(object sender, EventArgs e)
        {
            LoadOrderCompSearch();
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustID.Text = dgvCustomers.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCustName.Text = dgvCustomers.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvOrderList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCompID.Text = dgvOrderList.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCompName.Text = dgvOrderList.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCompPrice.Text = dgvOrderList.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtIDC.Text = dgvOrderList.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void numQty_ValueChanged(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(txtCompPrice.Text) * Convert.ToInt32(numQty.Value);
            if (Convert.ToInt32(numQty.Value) < 0)
            {
                MessageBox.Show("Quantity cannot be less than 0!");
            }
            else
            {
                txtCompTotal.Text = total.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OrderDetail orderDetail = new OrderDetail(txtID.Text);
            orderDetail.ShowDialog();
        }

        public int LoadID()
        {
            int newID = 0;

            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT MAX(ID) FROM Orders", connection))
                {
                    // ExecuteScalar is used to retrieve a single value (in this case, the max ID)
                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // If there are existing records, increment the max ID by 1
                        newID = Convert.ToInt32(result) + 1;
                    }
                    // If no records exist, start with ID = 1 (or any other desired starting value)
                    else
                    {
                        newID = 1;
                    }
                }
            }

            return newID;
        }

        private (int ID, string Name, string Price, string Description) GetProductDetails(int productID)
        {
            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT ID, name, price, desc FROM OrderComponents WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", productID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (
                                ID: Convert.ToInt32(reader["ID"]),
                                Name: reader["name"].ToString(),
                                Price: reader["price"].ToString(),
                                Description: reader["desc"].ToString()
                            );
                        }
                        else
                        {
                            throw new InvalidOperationException("Product not found with the provided ID.");
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Convert the provided product ID to an integer
                if (!int.TryParse(txtIDC.Text, out int selectedProductID))
                {
                    MessageBox.Show("Invalid product ID. Please provide a number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numQty.Value <= 0)
                {
                    MessageBox.Show("Please apply correct quantity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string path = Environment.CurrentDirectory;

                string databasePath = path + "\\Database\\dbInv.db";
                string connectionString = $"Data Source={databasePath}; Version = 3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO OrderDetail(IDC, name, desc, qty, total, ID) VALUES (@IDC, @name, @desc, @qty, @total, @ID)";

                        // Retrieve product details based on the provided product ID
                        var productDetails = GetProductDetails(selectedProductID);

                        // Check if the product with the provided IDP already exists in the order for the specific order ID
                        using (var checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM OrderDetail WHERE IDC = @IDC AND ID = @OrderID", connection))
                        {
                            checkCommand.Parameters.AddWithValue("@IDC", selectedProductID);
                            checkCommand.Parameters.AddWithValue("@OrderID", txtID.Text);

                            int existingProductCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (existingProductCount > 0)
                            {
                                MessageBox.Show("Product with the provided ID is already in the order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Parameters for the command
                        command.Parameters.AddWithValue("@IDC", productDetails.ID);
                        command.Parameters.AddWithValue("@name", productDetails.Name);
                        command.Parameters.AddWithValue("@desc", productDetails.Description);
                        command.Parameters.AddWithValue("@qty", numQty.Value);
                        command.Parameters.AddWithValue("@total", txtCompTotal.Text);
                        command.Parameters.AddWithValue("@ID", txtID.Text); // Assuming txtID is the ID of the order component

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Component added.");
                    txtCompID.Clear();
                    txtCompName.Clear();
                    txtIDC.Clear();
                    txtSeachCustomer.Clear();
                    txtSearchComponent.Clear();
                    numQty.Value = 0;
                    //LoadOrder();
                    //Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            numQty.Value = 0;
            
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("DELETE FROM OrderDetail WHERE ID LIKE '" + LoadID() + "'", connection))
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Order components list cleared.");
                }
                LoadOrderComp();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (string.IsNullOrEmpty(txtID.Text) ||
                        string.IsNullOrEmpty(txtCustID.Text) ||
                        string.IsNullOrEmpty(txtCustName.Text))
                    {
                        MessageBox.Show("Please select Customer and order components.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string path = Environment.CurrentDirectory;
                    string databasePath = path + "\\Database\\dbInv.db";
                    string connectionString = $"Data Source={databasePath}; Version = 3;";

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = new SQLiteCommand("SELECT COUNT(*) FROM OrderDetail WHERE ID = @OrderID", connection))
                        {
                            command.Parameters.AddWithValue("@OrderID", Int32.Parse(txtID.Text));

                            // ExecuteScalar is used to retrieve a single value (in this case, the count of records)
                            var result = command.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                // If the count is 0, show a notification about the absence of data
                                if (Convert.ToInt32(result) == 0)
                                {
                                    MessageBox.Show("No components for the order found! Please add order components.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                // Otherwise, you may perform other actions or leave it as is
                            }
                        }
                    }

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (var totalCommand = new SQLiteCommand("SELECT SUM(total) FROM OrderDetail WHERE ID = @OrderID", connection))
                        {
                            totalCommand.Parameters.AddWithValue("@OrderID", Int32.Parse(txtID.Text));
                            var totalResult = totalCommand.ExecuteScalar();

                            decimal total = totalResult != DBNull.Value ? Convert.ToDecimal(totalResult) : 0;

                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = "INSERT INTO Orders(ID, date, custID, custName, total, status) VALUES (@ID, @date, @custID, @custName, @total, @status)";
                                command.Parameters.AddWithValue("@ID", Int32.Parse(txtID.Text));
                                command.Parameters.AddWithValue("@date", dateOrder.Value);
                                command.Parameters.AddWithValue("@custID", txtCustID.Text);
                                command.Parameters.AddWithValue("@custName", txtCustName.Text);
                                command.Parameters.AddWithValue("@total", total);
                                command.Parameters.AddWithValue("@status", listStatus.Text);

                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Order has been successfully saved.");
                        Clear();
                        txtID.Text = Convert.ToString(LoadID());
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listStatus_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this order?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string path = Environment.CurrentDirectory;

                    string databasePath = path + "\\Database\\dbInv.db";
                    string connectionString = $"Data Source={databasePath}; Version = 3;";

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (var totalCommand = new SQLiteCommand("SELECT SUM(total) FROM OrderDetail WHERE ID = @OrderID", connection))
                        {
                            totalCommand.Parameters.AddWithValue("@OrderID", Int32.Parse(txtID.Text));
                            var totalResult = totalCommand.ExecuteScalar();

                            decimal total = totalResult != DBNull.Value ? Convert.ToDecimal(totalResult) : 0;

                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = "UPDATE Orders SET custID=@custID, custName=@custName, total=@total, status=@status WHERE ID LIKE '" + Int32.Parse(txtID.Text) + "' ";
                                command.Parameters.AddWithValue("@custID", txtCustID.Text);
                                command.Parameters.AddWithValue("@custName", txtCustName.Text);
                                command.Parameters.AddWithValue("@total", total);
                                command.Parameters.AddWithValue("@status", listStatus.Text);

                                command.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Order has been successfully updated.");
                        Clear();
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
