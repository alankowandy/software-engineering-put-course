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
using System.Runtime.InteropServices;

namespace Inventory
{
    
    
    public partial class main : Form
    {
        private Form activeForm = null;
        private string loggedInUsername;
        private string loggedInFullname;
        private readonly int isAdmin;
        
        public main(string username, string fullname, int isAdmin)
        {
            InitializeComponent();
            customizeDesign();
            //ConsoleHelper.CreateConsole(); // wykomentować jeżeli nie chcemy konsoli

            loggedInUsername = username;
            loggedInFullname = fullname;
            this.isAdmin = isAdmin;

            labelUserName.Text = loggedInUsername;
            labelName.Text = loggedInFullname;

            openChildForm(new Dashboard());
        }


        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void customizeDesign()
        {
            panelStock.Visible = false;
            panelOrders.Visible = false;
        }

        private void hideMenuOptions()
        {
            if(panelStock.Visible == true)
            {
                panelStock.Visible = false;
            }
            if(panelOrders.Visible == true)
            {
                panelOrders.Visible = false;
            }
        }

        private void showMenuOptions(Panel Menu)
        {
            if(Menu.Visible == false)
            {
                hideMenuOptions();
                Menu.Visible = true;
            }
            else
            {
                Menu.Visible = false;
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            hideMenuOptions();
            openChildForm(new Dashboard());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            hideMenuOptions();
        }

        private void panelMain_Paint(object sender, EventArgs e)
        {
            hideMenuOptions();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            hideMenuOptions();
            openChildForm(new Products());
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            showMenuOptions(panelStock);
        }

        private void btnStockList_Click(object sender, EventArgs e)
        {
            openChildForm(new StorageList());
        }

        private void btnStorage_Click(object sender, EventArgs e)
        {
            openChildForm(new StockList());
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            showMenuOptions(panelOrders);
        }

        private void btnOrdersList_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderForm());
        }

        private void btnOrdersDetails_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderComponents());
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerList());
        }

        private void btnBANF_Click(object sender, EventArgs e)
        {
            hideMenuOptions();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (isAdmin == 1)
            {
                openChildForm(new UserList());
            }
            else
            {
                MessageBox.Show("You don't have permission to access the User List.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
