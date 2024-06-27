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
using System.Reflection;

namespace Inventory
{
    public partial class UserList : Form
    {
        public UserList()
        {
            InitializeComponent();
            LoadUser();
        }

        public void LoadUser()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            int i = 0;
            dgvUsers.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM tbUsertest", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        i++;
                        dgvUsers.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User userModule = new User();
            userModule.btnSave.Enabled = true;
            userModule.btnUpdate.Enabled = false;
            userModule.ShowDialog();
            LoadUser();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            string colName = dgvUsers.Columns[e.ColumnIndex].Name;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (colName == "Edit")
                {
                    User userModule = new User();
                    userModule.txtUserName.Text = dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString();
                    userModule.txtFullName.Text = dgvUsers.Rows[e.RowIndex].Cells[2].Value.ToString();
                    userModule.txtPass.Text = dgvUsers.Rows[e.RowIndex].Cells[3].Value.ToString();
                    userModule.txtPhone.Text = dgvUsers.Rows[e.RowIndex].Cells[4].Value.ToString();
                    userModule.txtEmail.Text = dgvUsers.Rows[e.RowIndex].Cells[5].Value.ToString();

                    userModule.btnSave.Enabled = false;
                    userModule.btnUpdate.Enabled = true;
                    userModule.txtUserName.Enabled = false;
                    userModule.ShowDialog();
                    LoadUser();
                }
                else if (colName == "Delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this user?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var command = new SQLiteCommand("DELETE FROM tbUsertest WHERE username LIKE '" + dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", connection))
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("User has been successfully deleted!");
                        }
                        LoadUser();
                    }
                }
            }
        }
    }
}
