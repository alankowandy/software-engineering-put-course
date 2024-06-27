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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void Clear()
        {
            txtName.Clear();
            txtPass.Clear();
        }

        private void pictureClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            //string databasePath = "C:\\Users\\Alan\\source\\repos\\PPprojectss\\POSales\\POSales\\Database\\dbInv.db";
            //string connectionString = $"Data Source={databasePath}; Version = 3;";

            //Zakomentownay kod poniżej blokuje baze danych po zalogowaniu

            //try
            //{
            //    using (var connection = new SQLiteConnection(connectionString))
            //    {
            //        using (var command = new SQLiteCommand("SELECT * FROM tbUsertest WHERE username=@username AND password=@password", connection))
            //        {
            //            command.Parameters.AddWithValue("@username", txtName.Text);
            //            command.Parameters.AddWithValue("@password", txtPass.Text);

            //            connection.Open();

            //            using (var reader = command.ExecuteReader())
            //            {
            //                if (reader.Read())
            //                {
            //                    reader.Close();
            //                    //MessageBox.Show("Welcome " + reader["fullname"].ToString() + " | ", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    //this.Hide();

            //                    main main = new main();
            //                    //connection.Close();
            //                    //this.Hide();
            //                    main.ShowDialog();
            //                    this.Close();
            //                }
            //                else
            //                {
            //                    MessageBox.Show("Invalid username or password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand("SELECT * FROM tbUsertest WHERE username=@username AND password=@password", connection))
                            {
                                command.Parameters.AddWithValue("@username", txtName.Text);
                                command.Parameters.AddWithValue("@password", txtPass.Text);


                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        // Pobranie fullname i wartości admin aby przesłać je do maina
                                        string fullname = reader.GetString(reader.GetOrdinal("fullname"));
                                        int isAdmin = reader.GetInt32(reader.GetOrdinal("admin"));
     
                                        reader.Close();

                                        transaction.Commit();

                                        this.Hide();
                                        
                                        using (var main = new main(txtName.Text, fullname, isAdmin))
                                        {
                                            main.ShowDialog();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid username or password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Clear();
                                        return;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an exception
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Clear();
            this.Show();
            return;
        }
    }
}
