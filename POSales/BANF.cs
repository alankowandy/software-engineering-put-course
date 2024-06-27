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
    public partial class BANF : Form
    {
        public BANF()
        {
            InitializeComponent();
            LoadBANF();
        }

        private void dgvBANF_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LoadBANF()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvBANF.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM BANF", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvBANF.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    }
                }
            }
        }

        public void calculateBANF()
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM BANF", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvBANF.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    }
                }
            }
        }
    }
}
