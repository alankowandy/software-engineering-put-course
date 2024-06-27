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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadDash();
        }

        public void LoadDash()
        {

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            dgvDashboard.Rows.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("Select box.product_id, sum(quantity), unit, products.min from Box " +
                                                        "INNER JOIN Products Where Box.product_id = Products.id Group by box.product_id", connection))

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvDashboard.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), "out", "in", reader[3].ToString(), "EOD");
                    }

                    for (int i = 0; i < dgvDashboard.RowCount; i++)
                    {
                        if (Int32.Parse(dgvDashboard.Rows[i].Cells[1].Value.ToString()) < Int32.Parse(dgvDashboard.Rows[i].Cells[5].Value.ToString()))
                        {
                            dgvDashboard.Rows[i].Cells[1].Style.BackColor = Color.Red;
                            dgvDashboard.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                        }
                    }
                }
            }
        }
    }

    
}
