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
    public partial class Form2TransactionViewer : Form
    {

        public Form2TransactionViewer()
        {
            InitializeComponent();
        }

        private void Form2TransactionViewer_Load(object sender, EventArgs e)
        {
            List<Transaction> allTransactions = DBAccess.queryAllTransactions();

            transactionTable.DataSource = new BindingSource(allTransactions, "");
            
            /*
            var transactionQuery = from transaction in myshopDB.Transactions
                                   select new
                                   {
                                       transaction.InvoiceNo,
                                       transaction.Date,
                                       transaction.Amount
                                   };

            transactionTable.DataSource = transactionQuery.ToList(); */
        }

        private void Form2TransactionViewer_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int invoiceNumberToFind;
            
            if (Int32.TryParse(tbInvoiceNo.Text, out invoiceNumberToFind) && invoiceNumberToFind >= 0)
            {
                Transaction transaction = DBAccess.queryTransaction(invoiceNumberToFind);
                
                if (transaction != null)
                {
                    tbDate.Text = transaction.date.ToString();
                    tbCost.Text = transaction.amount.ToString();
                    tbCustomer.Text = transaction.customer.ToString();

                    listBoxItems.DataSource = new BindingSource(transaction.items, "");
                    listBoxItems.DisplayMember = "product";
                }
                else
                {
                    MessageBox.Show("Invoice not found");
                }


                /*
                   var myQuery = from transactions in myshopDB.Transactions
                                 where transactions.InvoiceNo == invoiceNumberToFind
                                 select new
                                 {
                                     transactions.Date,
                                     transactions.Amount,
                                     transactions.InvoiceItems,
                                     customerName = transactions.CustomerNavigation.Lastname
                                 };

                   var myList = myQuery.ToList();

                   if (myList.Count == 1)  // should be no more than 1 because invoice no. is unique
                   {
                       var itemQuery = from item in myList[0].InvoiceItems
                                       join product in myshopDB.Products on item.Product equals product.ProductCode
                                       select $"{item.ProductNavigation.Description} {item.ProductNavigation.Price}";

                       listBoxItems.DataSource = itemQuery.ToList();
                   } */
            }
        }

        private void transactionTable_SelectionChanged(object sender, EventArgs e)
        {
            if (transactionTable.SelectedRows.Count > 0)
            {
                tbInvoiceNo.Text = transactionTable.SelectedRows[0].Cells["InvoiceNo"].Value.ToString();
            }
        }

    }
}
