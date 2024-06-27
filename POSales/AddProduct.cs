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
    public partial class AddProduct : Form
    {
        Products dash;
        public AddProduct(Products tmp)
        {
            InitializeComponent();
            dash = tmp;
        }

        public void Clear()
        {
            txtId.Clear();
            txtDesc.Clear();
            txtMin.Clear();
            txtMax.Clear();
            txtUnit.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to add product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (string.IsNullOrEmpty(txtId.Text) ||
                        string.IsNullOrEmpty(txtDesc.Text) ||
                        string.IsNullOrEmpty(txtMin.Text) ||
                        string.IsNullOrEmpty(txtMax.Text) ||
                        string.IsNullOrEmpty(txtUnit.Text))
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
                            command.CommandText = "INSERT INTO Products (id, desc, min, max_in_box, unit) VALUES (@id, @desc, @min, @max_in_box, @unit)";
                            command.Parameters.AddWithValue("@id", txtId.Text);
                            command.Parameters.AddWithValue("@desc", txtDesc.Text);
                            command.Parameters.AddWithValue("@min", txtMin.Text);
                            command.Parameters.AddWithValue("@max_in_box", txtMax.Text);
                            command.Parameters.AddWithValue("@unit", txtUnit.Text);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product has been successfully saved.");
                        Clear();
                        dash.LoadDashboard();
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
                if (MessageBox.Show("Are you sure you want to edit this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (string.IsNullOrEmpty(txtId.Text) ||
                        string.IsNullOrEmpty(txtDesc.Text) ||
                        string.IsNullOrEmpty(txtMin.Text) ||
                        string.IsNullOrEmpty(txtMax.Text) ||
                        string.IsNullOrEmpty(txtUnit.Text))
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
                            command.CommandText = "UPDATE Products SET id = @id, desc=@desc, min=@min, max_in_box=@max_in_box WHERE id LIKE '" + txtId.Text + "' ";
                            command.Parameters.AddWithValue("@id", txtId.Text);
                            command.Parameters.AddWithValue("@desc", txtDesc.Text);
                            command.Parameters.AddWithValue("@min", txtMin.Text);
                            command.Parameters.AddWithValue("@max_in_box", txtMax.Text);
                            command.Parameters.AddWithValue("@unit", txtUnit.Text);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product has been successfully saved.");
                        Clear();
                        dash.LoadDashboard();
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
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
