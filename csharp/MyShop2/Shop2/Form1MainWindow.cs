using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopDB2
{
    public partial class PurchaseScreen : Form
    {
        private List<Product> productList;
        private List<Customer> customerList;
        private Purchase myPurchase;

        public PurchaseScreen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myPurchase = new Purchase();

            // Set up product category combobox
            string[] productCategories = new string[] {"Fruit & veg", "Dairy", "Food", "Bathroom"};
            foreach (string s in productCategories) cbCategory.Items.Add(s);
            cbCategory.SelectedIndex = 0;

            refreshProducts();
            refreshCustomers();
        }

        private void refreshCustomers()
        {
            // customer combo box
            customerList = DBAccess.queryAllCustomers();

            var customerDisplayList = from cust in customerList
                                      select new
                                      {
                                          cust.customerId,
                                          fullName = $"{cust.firstName} {cust.lastName}"
                                      };

            cbCustomers.DataSource = new BindingSource(customerDisplayList, "");
            cbCustomers.ValueMember = "customerId";
            cbCustomers.DisplayMember = "fullName";

            listBoxPurchaseList.DisplayMember = "Description";
        }

        //  --------------------------------------------------------------------------------

        private void btnShowProducts_Click(object sender, EventArgs e)
        {
            refreshProducts();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshProducts();
        }

        private void refreshProducts()
        {
            string category = "";

            switch (cbCategory.SelectedIndex)
            {
                case 0:
                    category = "fruitveg";
                    break;
                case 1:
                    category = "dairy";
                    break;
                case 2:
                    category = "food";
                    break;
                case 3:
                    category = "bathroom";
                    break;
            }

            this.productList = DBAccess.queryProductsByCategory(category);
            
            foreach (Product p in productList)
            {
                p.stock = DBAccess.queryStock(p.productCode);
            }

            productTable.DataSource = new BindingSource(this.productList, "");
            productTable.Columns["imageFile"].Visible = false;
            productTable.Columns["imageBitmap"].Visible = false;
            productTable.Columns["category"].Visible = false;
            productTable.Columns["productCode"].Visible = false;
        }

        private void productTable_SelectionChanged(object sender, EventArgs e)
        {
            // updates the product image
            if (productTable.SelectedRows.Count == 1 && productTable.SelectedRows[0].Cells["productCode"].Value != null)
            {
                string selectedProductCode = productTable.SelectedRows[0].Cells["productCode"].Value.ToString();

                foreach (var p in productList)
                {
                    if (p.productCode == selectedProductCode) 
                    {
                        displayBox.Image = p.imageBitmap;
                        break;
                    }
                }
            }
        }

        //  --------------------------------------------------------------------------------

        private void btnAddToPurchase_Click(object sender, EventArgs e)
        {
            if (productTable.SelectedRows.Count == 1)
            {
                string selectedProductCode = productTable.SelectedRows[0].Cells["productCode"].Value.ToString();

                foreach (var p in productList)
                {
                    if (p.productCode == selectedProductCode) 
                    {
                        if (p.stock > 0)
                        {
                            myPurchase.AddProduct(p);
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Sorry, out of stock.");
                        }
                    }
                }

                listBoxPurchaseList.DataSource = new BindingSource(myPurchase.GetProductList(), "");
                labelTotalPrice.Text = $"${myPurchase.GetTotalPrice()}";
            }
            else
            {
                MessageBox.Show("No product selected");
            }
        }

        private void btnRemoveFromPurchase_Click(object sender, EventArgs e)
        {
            int itemIndex = listBoxPurchaseList.SelectedIndex;
            
            if (itemIndex >= 0)
            {
                myPurchase.RemoveProduct(itemIndex);

                listBoxPurchaseList.DataSource = myPurchase.GetProductList();
                labelTotalPrice.Text = $"${myPurchase.GetTotalPrice()}";
            }
        }

        private void buttonClearPurchase_Click(object sender, EventArgs e)
        {
            
            DialogResult response = MessageBox.Show("This will clear your cart. Are you sure?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (response == DialogResult.OK)
            {
                newPurchase();
            }
        }

        private void newPurchase()
        {
            myPurchase = new Purchase();

            listBoxPurchaseList.DataSource = myPurchase.GetProductList();
            labelTotalPrice.Text = $"${myPurchase.GetTotalPrice()}";
        }

        //  --------------------------------------------------------------------------------

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            DialogResult response = DialogResult.Cancel;
            if (myPurchase.GetCount() > 0)
            {
                response = MessageBox.Show("Are you ready to checkout?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }

            if (response == DialogResult.OK)
            {
                int invoiceId;
                bool isSuccessful = DBAccess.checkOut((int) cbCustomers.SelectedValue, DateTime.Now, myPurchase, out invoiceId);
                
                if (isSuccessful)
                {
                    createInvoice(invoiceId);
                    newPurchase();
                    refreshProducts();
                }
                else
                {
                    MessageBox.Show("Error occurred. Could not Check Out");
                }
            }
        }

        private void createInvoice(int invoiceNumber)
        {
            tbInvoice.Clear();

            string[] s = new string[myPurchase.GetCount() + 10];

            s[0] = "";
            s[1] = DateTime.Today.ToLongDateString();
            s[2] = $"Invoice number {invoiceNumber}. {myPurchase.GetCount()} Items";

            int line = 4;
            foreach (Product product in myPurchase.GetProductList())
            {
                s[line] = $"{product.description} {product.price}";
                line++;
            }

            s[line + 1] = $"Total: ${myPurchase.GetTotalPrice()}";
            s[line + 2] = "Have a nice day!";
            tbInvoice.Lines = s;
        }

        //  --------------------------------------------------------------------------------

        private void btnViewTransactions_Click(object sender, EventArgs e)
        {
            bool isAlreadyOpen = false;

            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "Form2TransactionViewer")
                {
                    isAlreadyOpen = true;
                }
            }

            if (!isAlreadyOpen)
            {
                Form2TransactionViewer transactionWindow = new Form2TransactionViewer();
                transactionWindow.Show();
            }
        }

        private void btnEditCustomers_Click(object sender, EventArgs e)
        {
            Form3CustomerEditor customerWindow = new Form3CustomerEditor();
            customerWindow.ShowDialog();

            refreshCustomers();
        }

    }

}
