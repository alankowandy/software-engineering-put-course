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
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtName.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this customer?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (string.IsNullOrEmpty(txtName.Text))  
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
                            command.CommandText = "INSERT INTO tbCustomers(name) VALUES (@name)";
                            command.Parameters.AddWithValue("@name", txtName.Text);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("User has been successfully saved.");
                        Clear();
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
    }
}
