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
    public partial class User : Form
    {
        //SQLiteConnection connection = new SQLiteConnection(@"Data Source=..\\..\\Database\\dvInv.db;Version=3;");
        public User()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        public void Clear()
        {
            txtUserName.Clear();
            txtFullName.Clear();
            txtPass.Clear();
            txtRepass.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRepass.Text)
                {
                    MessageBox.Show("Passwords are not matching!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    
                    if (string.IsNullOrEmpty(txtUserName.Text) ||
                        string.IsNullOrEmpty(txtFullName.Text) ||
                        string.IsNullOrEmpty(txtPass.Text) ||
                        string.IsNullOrEmpty(txtRepass.Text) ||
                        string.IsNullOrEmpty(txtPhone.Text) ||
                        string.IsNullOrEmpty(txtEmail.Text))
                    {
                        MessageBox.Show("Please fill out all the required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    string path = Environment.CurrentDirectory;

                    string databasePath = path + "\\Database\\dbInv.db";
                    string connectionString = $"Data Source={databasePath}; Version = 3;";

                    // Checkbox value (1 if checked, 0 if unchecked)
                    int isCheckboxChecked = isAdmin.Checked ? 1 : 0;

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "INSERT INTO tbUsertest(username, password, fullname, phone, email, admin) VALUES (@username, @password, @fullname, @phone, @email, @admin)";
                            command.Parameters.AddWithValue("@username", txtUserName.Text);
                            command.Parameters.AddWithValue("@fullname", txtFullName.Text);
                            command.Parameters.AddWithValue("@password", txtPass.Text);
                            command.Parameters.AddWithValue("@phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@email", txtEmail.Text);
                            command.Parameters.AddWithValue("@admin", isCheckboxChecked);

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRepass.Text)
                {
                    MessageBox.Show("Passwords are not matching!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to update this user?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string path = Environment.CurrentDirectory;

                    string databasePath = path + "\\Database\\dbInv.db";
                    string connectionString = $"Data Source={databasePath}; Version = 3;";

                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE tbUsertest SET fullname = @fullname, password=@password, phone=@phone, email=@email WHERE username LIKE '" + txtUserName.Text + "' ";
                            command.Parameters.AddWithValue("@username", txtUserName.Text);
                            command.Parameters.AddWithValue("@fullname", txtFullName.Text);
                            command.Parameters.AddWithValue("@password", txtPass.Text);
                            command.Parameters.AddWithValue("@phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@email", txtEmail.Text);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("User has been successfully updated.");
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
