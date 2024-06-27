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
    public partial class StorageList : Form
    {
        public StorageList()
        {
            InitializeComponent();
            loadTree();
        }

        public void loadTree()
        {
            string path = Environment.CurrentDirectory;

            string databasePath = path + "\\Database\\dbInv.db";
            string connectionString = $"Data Source={databasePath}; Version = 3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    List<string> tmp = new List<string>();

                    command.CommandText = "Select id from products Order By id ASC";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tmp.Add(reader[0].ToString());
                            Tree.Nodes.Add(reader[0].ToString());
                        }
                    }

                    command.CommandText = "Select products.id, box.shelf_id, box.quantity, products.unit, products.max_in_box from products inner join Box on products.id = Box.product_id Order By Box.shelf_id ASC";
                    
                    using (var reader = command.ExecuteReader())
                    { 
                        
                        while (reader.Read())
                        {
                            for(int i = 0; i < tmp.Count(); i++)
                                if (tmp[i] == reader[0].ToString())
                                    Tree.Nodes[i].Nodes.Add("Lokalizacja: " + reader[1].ToString() + "  Ilość: " + reader[2].ToString() + " " +reader[3].ToString() + "  Maksymalna ilość w kontenerze: " + reader[4].ToString());
                        }

                    }
                }
            }
        }
    }
}
