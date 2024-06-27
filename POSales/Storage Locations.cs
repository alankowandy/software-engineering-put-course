using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Deployment.Internal;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Inventory
{
    public partial class StockList : Form
    {
        string selectedRack;

        TextBox[,] Names = new TextBox[4, 4];
        TextBox[,] Qtys = new TextBox[4, 4];
        TextBox[,] Units = new TextBox[4, 4];

        public StockList()
        {
            InitializeComponent();

            EditA1.Visible = false; EditA2.Visible = false; EditA3.Visible = false; EditA4.Visible = false;
            EditB1.Visible = false; EditB2.Visible = false; EditB3.Visible = false; EditB4.Visible = false;
            EditC1.Visible = false; EditC2.Visible = false; EditC3.Visible = false; EditC4.Visible = false;
            EditD1.Visible = false; EditD2.Visible = false; EditD3.Visible = false; EditD4.Visible = false;

            A1Name.Enabled = false; B1Name.Enabled = false; C1Name.Enabled = false; D1Name.Enabled = false;
            A1Quantity.Enabled = false; B1Quantity.Enabled = false; C1Quantity.Enabled = false; D1Quantity.Enabled = false;
            A1Unit.Enabled = false; B1Unit.Enabled = false; C1Unit.Enabled = false; D1Unit.Enabled = false;

            A2Name.Enabled = false; B2Name.Enabled = false; C2Name.Enabled = false; D2Name.Enabled = false;
            A2Quantity.Enabled = false; B2Quantity.Enabled = false; C2Quantity.Enabled = false; D2Quantity.Enabled = false;
            A2Unit.Enabled = false; B2Unit.Enabled = false; C2Unit.Enabled = false; D2Unit.Enabled = false;

            A3Name.Enabled = false; B3Name.Enabled = false; C3Name.Enabled = false; D3Name.Enabled = false;
            A3Quantity.Enabled = false; B3Quantity.Enabled = false; C3Quantity.Enabled = false; D3Quantity.Enabled = false;
            A3Unit.Enabled = false; B3Unit.Enabled = false; C3Unit.Enabled = false; D3Unit.Enabled = false;

            A4Name.Enabled = false; B4Name.Enabled = false; C4Name.Enabled = false; D4Name.Enabled = false;
            A4Quantity.Enabled = false; B4Quantity.Enabled = false; C4Quantity.Enabled = false; D4Quantity.Enabled = false;
            A4Unit.Enabled = false; B4Unit.Enabled = false; C4Unit.Enabled = false; D4Unit.Enabled = false;

            AcceptA1.Enabled = false; AcceptA2.Enabled = false; AcceptA3.Enabled = false; AcceptA4.Enabled = false;
            AcceptB1.Enabled = false; AcceptB2.Enabled = false; AcceptB3.Enabled = false; AcceptB4.Enabled = false;
            AcceptC1.Enabled = false; AcceptC2.Enabled = false; AcceptC3.Enabled = false; AcceptC4.Enabled = false;
            AcceptD1.Enabled = false; AcceptD2.Enabled = false; AcceptD3.Enabled = false; AcceptD4.Enabled = false;


            AcceptA1.Visible = false; AcceptA2.Visible = false; AcceptA3.Visible = false; AcceptA4.Visible = false;
            AcceptB1.Visible = false; AcceptB2.Visible = false; AcceptB3.Visible = false; AcceptB4.Visible = false;
            AcceptC1.Visible = false; AcceptC2.Visible = false; AcceptC3.Visible = false; AcceptC4.Visible = false;
            AcceptD1.Visible = false; AcceptD2.Visible = false; AcceptD3.Visible = false; AcceptD4.Visible = false;

        }

        private void Rack_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedRack = Rack.SelectedItem.ToString();
            loadStockRack();
        }

        private void loadStockRack()
        {
            ClearTexts();

            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select Box.shelf_id, Box.product_id, Box.quantity, Products.unit from Box Inner Join Products On Box.product_id = products.id where SUBSTRING(shelf_id, 1, 1) = @rack";

                    command.Parameters.AddWithValue("@rack", selectedRack);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();

                            int col = id[2] - '0';
                            col--;
                            int row;

                            if (id[1] == 'A')
                                row = 0;
                            else if (id[1] == 'B')
                                row = 1;
                            else if (id[1] == 'C')
                                row = 2;
                            else
                                row = 3;

                            Names[row, col].Text = reader[1].ToString();
                            Qtys[row, col].Text = reader[2].ToString();
                            Units[row, col].Text = reader[3].ToString();
                        }
                    }
                }
            }
        }

        private void ClearTexts()
        {
            EditA1.Visible = true; EditA2.Visible = true; EditA3.Visible = true; EditA4.Visible = true;
            EditB1.Visible = true; EditB2.Visible = true; EditB3.Visible = true; EditB4.Visible = true;
            EditC1.Visible = true; EditC2.Visible = true; EditC3.Visible = true; EditC4.Visible = true;
            EditD1.Visible = true; EditD2.Visible = true; EditD3.Visible = true; EditD4.Visible = true;

            A1Name.Enabled = false; B1Name.Enabled = false; C1Name.Enabled = false; D1Name.Enabled = false;
            A1Quantity.Enabled = false; B1Quantity.Enabled = false; C1Quantity.Enabled = false; D1Quantity.Enabled = false;
            A1Unit.Enabled = false; B1Unit.Enabled = false; C1Unit.Enabled = false; D1Unit.Enabled = false;

            A2Name.Enabled = false; B2Name.Enabled = false; C2Name.Enabled = false; D2Name.Enabled = false;
            A2Quantity.Enabled = false; B2Quantity.Enabled = false; C2Quantity.Enabled = false; D2Quantity.Enabled = false;
            A2Unit.Enabled = false; B2Unit.Enabled = false; C2Unit.Enabled = false; D2Unit.Enabled = false;

            A3Name.Enabled = false; B3Name.Enabled = false; C3Name.Enabled = false; D3Name.Enabled = false;
            A3Quantity.Enabled = false; B3Quantity.Enabled = false; C3Quantity.Enabled = false; D3Quantity.Enabled = false;
            A3Unit.Enabled = false; B3Unit.Enabled = false; C3Unit.Enabled = false; D3Unit.Enabled = false;

            A4Name.Enabled = false; B4Name.Enabled = false; C4Name.Enabled = false; D4Name.Enabled = false;
            A4Quantity.Enabled = false; B4Quantity.Enabled = false; C4Quantity.Enabled = false; D4Quantity.Enabled = false;
            A4Unit.Enabled = false; B4Unit.Enabled = false; C4Unit.Enabled = false; D4Unit.Enabled = false;

            AcceptA1.Enabled = false; AcceptA2.Enabled = false; AcceptA3.Enabled = false; AcceptA4.Enabled = false;
            AcceptB1.Enabled = false; AcceptB2.Enabled = false; AcceptB3.Enabled = false; AcceptB4.Enabled = false;
            AcceptC1.Enabled = false; AcceptC2.Enabled = false; AcceptC3.Enabled = false; AcceptC4.Enabled = false;
            AcceptD1.Enabled = false; AcceptD2.Enabled = false; AcceptD3.Enabled = false; AcceptD4.Enabled = false;


            AcceptA1.Visible = false; AcceptA2.Visible = false; AcceptA3.Visible = false; AcceptA4.Visible = false;
            AcceptB1.Visible = false; AcceptB2.Visible = false; AcceptB3.Visible = false; AcceptB4.Visible = false;
            AcceptC1.Visible = false; AcceptC2.Visible = false; AcceptC3.Visible = false; AcceptC4.Visible = false;
            AcceptD1.Visible = false; AcceptD2.Visible = false; AcceptD3.Visible = false; AcceptD4.Visible = false;


            A1Name.Clear(); B1Name.Clear(); C1Name.Clear(); D1Name.Clear();
            A1Quantity.Clear(); B1Quantity.Clear(); C1Quantity.Clear(); D1Quantity.Clear();
            A1Unit.Clear(); B1Unit.Clear(); C1Unit.Clear(); D1Unit.Clear();

            A2Name.Clear(); B2Name.Clear(); C2Name.Clear(); D2Name.Clear();
            A2Quantity.Clear(); B2Quantity.Clear(); C2Quantity.Clear(); D2Quantity.Clear();
            A2Unit.Clear(); B2Unit.Clear(); C2Unit.Clear(); D2Unit.Clear();

            A3Name.Clear(); B3Name.Clear(); C3Name.Clear(); D3Name.Clear();
            A3Quantity.Clear(); B3Quantity.Clear(); C3Quantity.Clear(); D3Quantity.Clear();
            A3Unit.Clear(); B3Unit.Clear(); C3Unit.Clear(); D3Unit.Clear();

            A4Name.Clear(); B4Name.Clear(); C4Name.Clear(); D4Name.Clear();
            A4Quantity.Clear(); B4Quantity.Clear(); C4Quantity.Clear(); D4Quantity.Clear();
            A4Unit.Clear(); B4Unit.Clear(); C4Unit.Clear(); D4Unit.Clear();

            Names[0, 0] = A1Name; Names[0, 1] = A2Name; Names[0, 2] = A3Name; Names[0, 3] = A4Name;
            Names[1, 0] = B1Name; Names[1, 1] = B2Name; Names[1, 2] = B3Name; Names[1, 3] = B4Name;
            Names[2, 0] = C1Name; Names[2, 1] = C2Name; Names[2, 2] = C3Name; Names[2, 3] = C4Name;
            Names[3, 0] = D1Name; Names[3, 1] = D2Name; Names[3, 2] = D3Name; Names[3, 3] = D4Name;

            Qtys[0, 0] = A1Quantity; Qtys[0, 1] = A2Quantity; Qtys[0, 2] = A3Quantity; Qtys[0, 3] = A4Quantity;
            Qtys[1, 0] = B1Quantity; Qtys[1, 1] = B2Quantity; Qtys[1, 2] = B3Quantity; Qtys[1, 3] = B4Quantity;
            Qtys[2, 0] = C1Quantity; Qtys[2, 1] = C2Quantity; Qtys[2, 2] = C3Quantity; Qtys[2, 3] = C4Quantity;
            Qtys[3, 0] = D1Quantity; Qtys[3, 1] = D2Quantity; Qtys[3, 2] = D3Quantity; Qtys[3, 3] = D4Quantity;

            Units[0, 0] = A1Unit; Units[0, 1] = A2Unit; Units[0, 2] = A3Unit; Units[0, 3] = A4Unit;
            Units[1, 0] = B1Unit; Units[1, 1] = B2Unit; Units[1, 2] = B3Unit; Units[1, 3] = B4Unit;
            Units[2, 0] = C1Unit; Units[2, 1] = C2Unit; Units[2, 2] = C3Unit; Units[2, 3] = C4Unit;
            Units[3, 0] = D1Unit; Units[3, 1] = D2Unit; Units[3, 2] = D3Unit; Units[3, 3] = D4Unit;
        }

        private void EditA1_Click(object sender, EventArgs e)
        {
            A1Name.Enabled = true;
            A1Quantity.Enabled = true;
            AcceptA1.Enabled = true;
            AcceptA1.Visible = true;
        }

        private void EditA2_Click(object sender, EventArgs e)
        {
            A2Name.Enabled = true;
            A2Quantity.Enabled = true;
            AcceptA2.Enabled = true;
            AcceptA2.Visible = true;
        }

        private void EditA3_Click(object sender, EventArgs e)
        {
            A3Name.Enabled = true;
            A3Quantity.Enabled = true;
            AcceptA3.Enabled = true;
            AcceptA3.Visible = true;
        }

        private void EditA4_Click(object sender, EventArgs e)
        {
            A4Name.Enabled = true;
            A4Quantity.Enabled = true;
            AcceptA4.Enabled = true;
            AcceptA4.Visible = true;
        }

        private void EditB1_Click(object sender, EventArgs e)
        {
            B1Name.Enabled = true;
            B1Quantity.Enabled = true;
            AcceptB1.Enabled = true;
            AcceptB1.Visible = true;
        }

        private void EditB2_Click(object sender, EventArgs e)
        {
            B2Name.Enabled = true;
            B2Quantity.Enabled = true;
            AcceptB2.Enabled = true;
            AcceptB2.Visible = true;
        }

        private void EditB3_Click(object sender, EventArgs e)
        {
            B3Name.Enabled = true;
            B3Quantity.Enabled = true;
            AcceptB3.Enabled = true;
            AcceptB3.Visible = true;
        }

        private void EditB4_Click(object sender, EventArgs e)
        {
            B4Name.Enabled = true;
            B4Quantity.Enabled = true;
            AcceptB4.Enabled = true;
            AcceptB4.Visible = true;
        }

        private void EditC1_Click(object sender, EventArgs e)
        {
            C1Name.Enabled = true;
            C1Quantity.Enabled = true;
            AcceptC1.Enabled = true;
            AcceptC1.Visible = true;
        }

        private void EditC2_Click(object sender, EventArgs e)
        {
            C2Name.Enabled = true;
            C2Quantity.Enabled = true;
            AcceptC2.Enabled = true;
            AcceptC2.Visible = true;
        }

        private void EditC3_Click(object sender, EventArgs e)
        {
            C3Name.Enabled = true;
            C3Quantity.Enabled = true;
            AcceptC3.Enabled = true;
            AcceptC3.Visible = true;
        }

        private void EditC4_Click(object sender, EventArgs e)
        {
            C4Name.Enabled = true;
            C4Quantity.Enabled = true;
            AcceptC4.Enabled = true;
            AcceptC4.Visible = true;
        }

        private void EditD1_Click(object sender, EventArgs e)
        {
            D1Name.Enabled = true;
            D1Quantity.Enabled = true;
            AcceptD1.Enabled = true;
            AcceptD1.Visible = true;
        }

        private void EditD2_Click(object sender, EventArgs e)
        {
            D2Name.Enabled = true;
            D2Quantity.Enabled = true;
            AcceptD2.Enabled = true;
            AcceptD2.Visible = true;
        }

        private void EditD3_Click(object sender, EventArgs e)
        {
            D3Name.Enabled = true;
            D3Quantity.Enabled = true;
            AcceptD3.Enabled = true;
            AcceptD3.Visible = true;
        }

        private void EditD4_Click(object sender, EventArgs e)
        {
            D4Name.Enabled = true;
            D4Quantity.Enabled = true;
            AcceptD4.Enabled = true;
            AcceptD4.Visible = true;
        }

        private void AcceptA1_Click(object sender, EventArgs e)
        {
            saveEdit(A1Name, A1Quantity, "A1");

            A1Name.Enabled = false;
            A1Quantity.Enabled = false;
            AcceptA1.Enabled = false;
            AcceptA1.Visible = false;

            loadStockRack();
        }

        private void AcceptA2_Click(object sender, EventArgs e)
        {
            saveEdit(A2Name, A2Quantity, "A2");

            A2Name.Enabled = false;
            A2Quantity.Enabled = false;
            AcceptA2.Enabled = false;
            AcceptA2.Visible = false;

            loadStockRack();
        }

        private void AcceptA3_Click(object sender, EventArgs e)
        {
            saveEdit(A3Name, A3Quantity, "A3");

            A3Name.Enabled = false;
            A3Quantity.Enabled = false;
            AcceptA3.Enabled = false;
            AcceptA3.Visible = false;

            loadStockRack();
        }

        private void AcceptA4_Click(object sender, EventArgs e)
        {
            saveEdit(A4Name, A4Quantity, "A4");

            A4Name.Enabled = false;
            A4Quantity.Enabled = false;
            AcceptA4.Enabled = false;
            AcceptA4.Visible = false;

            loadStockRack();
        }

        private void AcceptB1_Click(object sender, EventArgs e)
        {
            saveEdit(B1Name, B1Quantity, "B1");

            B1Name.Enabled = false;
            B1Quantity.Enabled = false;
            AcceptB1.Enabled = false;
            AcceptB1.Visible = false;

            loadStockRack();
        }

        private void AcceptB2_Click(object sender, EventArgs e)
        {
            saveEdit(B2Name, B2Quantity, "B2");

            B2Name.Enabled = false;
            B2Quantity.Enabled = false;
            AcceptB2.Enabled = false;
            AcceptB2.Visible = false;

            loadStockRack();
        }

        private void AcceptB3_Click(object sender, EventArgs e)
        {
            saveEdit(B3Name, B3Quantity, "B3");

            B3Name.Enabled = false;
            B3Quantity.Enabled = false;
            AcceptB3.Enabled = false;
            AcceptB3.Visible = false;

            loadStockRack();
        }

        private void AcceptB4_Click(object sender, EventArgs e)
        {
            saveEdit(B4Name, B4Quantity, "B4");

            B4Name.Enabled = false;
            B4Quantity.Enabled = false;
            AcceptB4.Enabled = false;
            AcceptB4.Visible = false;

            loadStockRack();
        }

        private void AcceptC1_Click(object sender, EventArgs e)
        {
            saveEdit(C1Name, C1Quantity, "C1");

            C1Name.Enabled = false;
            C1Quantity.Enabled = false;
            AcceptC1.Enabled = false;
            AcceptC1.Visible = false;

            loadStockRack();
        }

        private void AcceptC2_Click(object sender, EventArgs e)
        {
            saveEdit(C2Name, C2Quantity, "C2");

            C2Name.Enabled = false;
            C2Quantity.Enabled = false;
            AcceptC2.Enabled = false;
            AcceptC2.Visible = false;

            loadStockRack();
        }

        private void AcceptC3_Click(object sender, EventArgs e)
        {
            saveEdit(C3Name, C3Quantity, "C3");

            C3Name.Enabled = false;
            C3Quantity.Enabled = false;
            AcceptC3.Enabled = false;
            AcceptC3.Visible = false;

            loadStockRack();
        }

        private void AcceptC4_Click(object sender, EventArgs e)
        {
            saveEdit(C4Name, C4Quantity, "C4");

            C4Name.Enabled = false;
            C4Quantity.Enabled = false;
            AcceptC4.Enabled = false;
            AcceptC4.Visible = false;

            loadStockRack();
        }

        private void AcceptD1_Click(object sender, EventArgs e)
        {
            saveEdit(D1Name, D1Quantity, "D1");

            D1Name.Enabled = false;
            D1Quantity.Enabled = false;
            AcceptD1.Enabled = false;
            AcceptD1.Visible = false;

            loadStockRack();
        }

        private void AcceptD2_Click(object sender, EventArgs e)
        {
            saveEdit(D2Name, D2Quantity, "D2");

            D2Name.Enabled = false;
            D2Quantity.Enabled = false;
            AcceptD2.Enabled = false;
            AcceptD2.Visible = false;

            loadStockRack();
        }

        private void AcceptD3_Click(object sender, EventArgs e)
        {
            saveEdit(D3Name, D3Quantity, "D3");

            D3Name.Enabled = false;
            D3Quantity.Enabled = false;
            AcceptD3.Enabled = false;
            AcceptD3.Visible = false;

            loadStockRack();
        }

        private void AcceptD4_Click(object sender, EventArgs e)
        {
            saveEdit(D4Name, D4Quantity, "D4");

            D4Name.Enabled = false;
            D4Quantity.Enabled = false;
            AcceptD4.Enabled = false;
            AcceptD4.Visible = false;

            loadStockRack();
        }

        private void saveEdit(TextBox name, TextBox quan, string boxid)
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            bool properid = false;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM products";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == name.Text)
                            {
                                properid = true;
                                break;
                            }
                        }
                    }
                }

                if (properid == true)
                {
                    using (var command = connection.CreateCommand())
                    {
                        string shelfId = selectedRack + boxid;
                        command.CommandText = "INSERT or REPLACE INTO Box(shelf_id, product_id, quantity) VALUES (@shelf_id, @product_id, @quantity)";
                        command.Parameters.AddWithValue("@shelf_id", shelfId);
                        command.Parameters.AddWithValue("@product_id", name.Text);
                        command.Parameters.AddWithValue("@quantity", quan.Text);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product added to container.");
                }
                else
                    MessageBox.Show("Can't find this product in datebase.");
            }
        }
    }
}
