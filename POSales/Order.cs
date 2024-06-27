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
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
            LoadProducts();
            //LoadOrder(txtID.Text);
            // Check if txtID.Text is already filled out
            if (string.IsNullOrEmpty(txtID.Text))
            {
                txtID.Enabled = false;
                txtID.Text = Convert.ToString(LoadID());
            }
            LoadOrder(txtID.Text);
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Clear()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtDesc.Clear();
        }

        public void LoadProducts()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvProducts.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM List_Of_Products", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvProducts.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
                    }
                }
            }
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

        public int LoadID()
        {
            int newID = 0;

            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT MAX(ID) FROM OrderComponents", connection))
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

        // Check if a product with the provided ID exists in the List_Of_Products table
        private bool ProductExists(int productID)
        {
            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM List_Of_Products WHERE product_id = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", productID);
                    int productCount = Convert.ToInt32(command.ExecuteScalar());

                    return productCount > 0;
                }
            }
        }

        // Retrieve product details based on the provided product ID
        private (int ID, string Name, string Description) GetProductDetails(int productID)
        {
            string path = Environment.CurrentDirectory;
            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT product_id, name, desc FROM List_Of_Products WHERE product_id = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", productID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (
                                ID: Convert.ToInt32(reader["product_id"]),
                                Name: reader["name"].ToString(),
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this order component?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (string.IsNullOrEmpty(txtName.Text) ||
                        string.IsNullOrEmpty(txtID.Text) ||
                        string.IsNullOrEmpty(txtPrice.Text) ||
                        string.IsNullOrEmpty(txtDesc.Text))
                    {
                        MessageBox.Show("Please fill out all the required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            command.CommandText = "INSERT INTO OrderComponents(name, price, desc, ID) VALUES (@name, @price, @desc, @ID)";
                            command.Parameters.AddWithValue("@name", txtName.Text);
                            command.Parameters.AddWithValue("@price", txtPrice.Text);
                            command.Parameters.AddWithValue("@desc", txtDesc.Text);
                            command.Parameters.AddWithValue("@ID", txtID.Text);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Order component has been successfully saved.");
                        Clear();
                        txtID.Text = Convert.ToString(LoadID());
                        LoadOrder(txtID.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtQTY.Text))
                {
                    MessageBox.Show("Please provide ID and quantity of the product.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convert the provided product ID to an integer
                if (!int.TryParse(txtIDP.Text, out int selectedProductID))
                {
                    MessageBox.Show("Invalid product ID. Please provide a number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate if the product ID exists in the List_Of_Products table
                if (!ProductExists(selectedProductID))
                {
                    MessageBox.Show("Product with the provided ID does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        command.CommandText = "INSERT INTO OrderComponentDetails(IDP, name, desc, qty, ID) VALUES (@IDP, @name, @desc, @qty, @ID)";

                        // Retrieve product details based on the provided product ID
                        var productDetails = GetProductDetails(selectedProductID);

                        // Check if the product with the provided IDP already exists in the order for the specific order ID
                        using (var checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM OrderComponentDetails WHERE IDP = @IDP AND ID = @OrderID", connection))
                        {
                            checkCommand.Parameters.AddWithValue("@IDP", selectedProductID);
                            checkCommand.Parameters.AddWithValue("@OrderID", txtID.Text);

                            int existingProductCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (existingProductCount > 0)
                            {
                                MessageBox.Show("Product with the provided ID is already in the order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Parameters for the command
                        command.Parameters.AddWithValue("@IDP", productDetails.ID);
                        command.Parameters.AddWithValue("@name", productDetails.Name);
                        command.Parameters.AddWithValue("@desc", productDetails.Description);
                        command.Parameters.AddWithValue("@qty", txtQTY.Text);
                        command.Parameters.AddWithValue("@ID", txtID.Text); // Assuming txtID is the ID of the order component

                        command.ExecuteNonQuery();
                    }

                    //MessageBox.Show("Order component has been successfully saved.");
                    //LoadOrder();
                    //Clear();
                }
                LoadOrder(txtID.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate if product ID is provided
                if (string.IsNullOrEmpty(txtIDP.Text))
                {
                    MessageBox.Show("Please provide the product ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convert the provided product ID to an integer
                if (!int.TryParse(txtIDP.Text, out int selectedProductID))
                {
                    MessageBox.Show("Invalid product ID. Please provide a number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        command.CommandText = "DELETE FROM OrderComponentDetails WHERE IDP = @IDP";

                        // Parameter for the command
                        command.Parameters.AddWithValue("@IDP", selectedProductID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            //MessageBox.Show("Product has been successfully removed from the order.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadOrder(txtID.Text);
                            //Clear(); // If you have a method to clear the form fields, you can call it here
                        }
                        else
                        {
                            MessageBox.Show("Product with the provided ID does not exist in the order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            try
            {
                string path = Environment.CurrentDirectory;
                string databasePath = path + "\\Database\\dbInv.db";
                string connectionString = $"Data Source={databasePath}; Version = 3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM OrderComponentDetails";

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LoadOrder(txtID.Text);
                            // Clear(); // If you have a method to clear the form fields, you can call it here
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this component?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string path = Environment.CurrentDirectory;

                    string databasePath = path + "\\Database\\dbInv.db";
                    string connectionString = $"Data Source={databasePath}; Version = 3;";

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE OrderComponents SET price=@price, desc=@desc WHERE name LIKE '" + txtName.Text + "' ";
                            command.Parameters.AddWithValue("@price", txtPrice.Text);
                            command.Parameters.AddWithValue("@desc", txtDesc.Text);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Component has been successfully updated.");
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
