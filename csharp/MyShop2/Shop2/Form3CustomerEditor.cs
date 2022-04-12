using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopDB2
{
    public partial class Form3CustomerEditor : Form
    {
        private List<Customer> customerList;
        private int mode = 0;                   // mode 1 = add, mode 2 = edit
        private int activeRecordId;             // stores Id of customer currently being edited

        public Form3CustomerEditor()
        {
            InitializeComponent();
        }

        private void Form3CustomerEditor_Load(object sender, EventArgs e)
        {
            customerList = DBAccess.queryAllCustomers();
            customerTable.DataSource = new BindingSource(customerList, "");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.mode = 1;
            
            panelInput.Enabled = true;
            tbFirstname.Text = "";
            tbLastname.Text = "";
            tbAddress.Text = "";
            tbPostcode.Text = "2000";
            tbPhone.Text = "";
            tbEmail.Text = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.mode = 2;
            Customer retrievedCustomer = null;

            if (customerTable.SelectedRows.Count == 1)
            {
                int id = (int)customerTable.SelectedRows[0].Cells["customerId"].Value;

                retrievedCustomer = DBAccess.queryCustomersById(id);
            }
            else
            {
                MessageBox.Show("Please select a record to edit.");
            }

            if (retrievedCustomer != null)
            {
                panelInput.Enabled = true;
                this.activeRecordId = retrievedCustomer.customerId;
                tbFirstname.Text = retrievedCustomer.firstName;
                tbLastname.Text = retrievedCustomer.lastName;
                tbAddress.Text = retrievedCustomer.address;
                tbPostcode.Text = retrievedCustomer.postcode;
                tbPhone.Text = retrievedCustomer.phone;
                tbEmail.Text = retrievedCustomer.email;
                cbState.Text = retrievedCustomer.state;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelInput.Enabled = false;
            this.mode = 0;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string validationError = "";

            if (!validateInput(ref validationError))
            {
                MessageBox.Show($"Incorrect {validationError} field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (this.mode == 1) // add 
                {
                    Customer newCustomer = new Customer();
                    newCustomer.firstName = tbFirstname.Text;
                    newCustomer.lastName = tbLastname.Text;
                    newCustomer.address = tbAddress.Text;
                    newCustomer.state = cbState.Text;
                    newCustomer.postcode = tbPostcode.Text;
                    newCustomer.phone = tbPhone.Text;
                    newCustomer.email = tbEmail.Text;

                    if (DBAccess.addNewCustomer(newCustomer))
                    {
                        MessageBox.Show("Successfully added");
                    }
                    else MessageBox.Show("Failed");
                }
                else if (this.mode == 2) // update 
                {
                    Customer revisedCustomer = new Customer();
                    revisedCustomer.customerId = this.activeRecordId;
                    revisedCustomer.firstName = tbFirstname.Text;
                    revisedCustomer.lastName = tbLastname.Text;
                    revisedCustomer.address = tbAddress.Text;
                    revisedCustomer.state = cbState.Text;
                    revisedCustomer.postcode = tbPostcode.Text;
                    revisedCustomer.phone = tbPhone.Text;
                    revisedCustomer.email = tbEmail.Text;

                    if (DBAccess.updateCustomer(revisedCustomer))
                    {
                        MessageBox.Show("Successfully updated");
                    }
                    else MessageBox.Show("Failed");
                }

                this.mode = 0;
                panelInput.Enabled = false;
                customerList = DBAccess.queryAllCustomers();
                customerTable.DataSource = new BindingSource(customerList, "");
            }
        }

        private bool validateInput(ref string error)
        {
            string namePattern = @"^[-A-Za-z]+$";
            string addressPattern = @"^[- A-Za-z0-9]+$";
            string postcodePattern = @"^\d{4}$";
            string statePattern = @"^[A-Z]{2,3}$";
            string phonePattern = @"^0[0-9]{9}$";
            string emailPattern = @"^[-A-Za-z0-9]+@[-A-Za-z0-9]+.[.-A-Za-z0-9]+$";

            if (!Regex.IsMatch(tbFirstname.Text.Trim(), namePattern))
            {
                error = "first name";
                return false;
            }

            if (!Regex.IsMatch(tbLastname.Text.Trim(), namePattern))
            {
                error = "last name";
                return false;
            }

            if (!Regex.IsMatch(tbAddress.Text.Trim(), addressPattern))
            {
                error = "address";
                return false;
            }

            if (!Regex.IsMatch(cbState.Text, statePattern))
            {
                error = "state";
                return false;
            }

            if (!Regex.IsMatch(tbPostcode.Text, postcodePattern))
            {
                error = "postcode";
                return false;
            }

            if (!Regex.IsMatch(tbPhone.Text, phonePattern))
            {
                error = "phone";
                return false;
            }

            if (!Regex.IsMatch(tbEmail.Text, emailPattern))
            {
                error = "email";
                return false;
            }

            return true;
        }
    }
}
