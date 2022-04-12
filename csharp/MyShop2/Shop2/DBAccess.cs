using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopDB2
{
    class DBAccess
    {
        private const string connString = "server=localhost; user=root; pwd=password; database=myshop";
        private const string imageDirectory = "images/";

        public static List<Product> queryAllProducts()
        {
            List<Product> result = new List<Product>();

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from products;";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product nextProduct = new Product();
                    nextProduct.productCode = reader.GetString("product_code");
                    nextProduct.description = reader.GetString("description");
                    nextProduct.quantity = reader.GetString("quantity");
                    nextProduct.price = reader.GetDecimal("price");
                    nextProduct.category = reader.GetString("category");
                    nextProduct.imageFile = imageDirectory + reader.GetString("image");

                    // load image (if exists)
                    if (File.Exists(nextProduct.imageFile))
                    {
                        nextProduct.imageBitmap = new System.Drawing.Bitmap(nextProduct.imageFile);
                    }

                    result.Add(nextProduct);
                }

                reader.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show($"SQL Exception {e.ToString()} occurred.");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }

        public static List<Product> queryProductsByCategory(string category)
        {
            List<Product> result = new List<Product>();

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * from products WHERE category = @cat;";
                cmd.Parameters.AddWithValue("@cat", category);
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product nextProduct = new Product();
                    nextProduct.productCode = reader.GetString("product_code");
                    nextProduct.description = reader.GetString("description");
                    nextProduct.quantity = reader.GetString("quantity");
                    nextProduct.price = reader.GetDecimal("price");
                    nextProduct.category = reader.GetString("category");
                    nextProduct.imageFile = imageDirectory + reader.GetString("image");

                    // load image (if exists)
                    if (File.Exists(nextProduct.imageFile))
                    {
                        nextProduct.imageBitmap = new System.Drawing.Bitmap(nextProduct.imageFile);
                    }

                    result.Add(nextProduct);
                }

                reader.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show($"SQL Exception {e.ToString()} occurred.");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }
        
        public static int queryStock(string product_code)
        {
            int result = -1;

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT stock FROM product_stocks WHERE product_code = @code;";
                cmd.Parameters.AddWithValue("@code", product_code);
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = reader.GetInt32("stock");
                }
            }
            catch (MySqlException e)
            {
                MessageBox.Show($"SQL Exception {e.ToString()} occurred.");
                return -1;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }

        public static bool checkOut(int customer, DateTime date, Purchase purchase, out int invoiceId)
        {
            // generate correct date format
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            string monthString;
            string dayString;

            if (month < 10)
            {
                monthString = $"0{month}";
            }
            else monthString = month.ToString();

            if (day < 10)
            {
                dayString = $"0{day}";
            }
            else dayString = day.ToString();

            string dateString = $"{year}{monthString}{dayString}";

            // Create connection object
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            MySqlTransaction checkoutTransaction = null;

            try
            {
                conn.Open();
                checkoutTransaction = conn.BeginTransaction();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = checkoutTransaction;
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = conn;
                cmd2.Transaction = checkoutTransaction;

                // Step 1: Insert into transactions table
                cmd.CommandText = $"INSERT INTO transactions (date, customer, amount) VALUES ({dateString}, {customer}, {purchase.GetTotalPrice()});";
                cmd.ExecuteNonQuery();

                // get auto-incremented ID
                cmd.CommandText = "SELECT last_insert_id()";
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int id = reader.GetInt32(0);
                reader.Close();

                foreach (Product product in purchase.GetProductList())
                {
                    // Step 2: Insert into invoice_items table
                    cmd.Parameters.Clear();
                    cmd.CommandText = $"INSERT INTO invoice_items (invoice, product) VALUES (@invoiceid, @productcode);";
                    cmd.Parameters.AddWithValue("@invoiceid", id);
                    cmd.Parameters.AddWithValue("@productcode", product.productCode);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    // Step 3: Get then update stock
                    int stock = -1;
                    cmd.CommandText = "SELECT stock FROM product_stocks WHERE product_code = @productcode";
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        stock = reader.GetInt32("stock");
                        reader.Close();

                        cmd.CommandText = "UPDATE product_stocks SET stock = @newstock WHERE product_code = @productcode";
                        cmd.Parameters.AddWithValue("@newstock", stock - 1);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                }

                checkoutTransaction.Commit();
                invoiceId = id; // sent back to caller via reference 
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()}");
                invoiceId = -1;
                if (checkoutTransaction != null) checkoutTransaction.Rollback();
                return false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return true;
        }

        public static List<Transaction> queryAllTransactions()
        {
            List<Transaction> result = new List<Transaction>();

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM transactions;";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Transaction nextTransaction = new Transaction();
                    nextTransaction.invoiceNo = reader.GetInt32("invoice_no");
                    nextTransaction.customer = reader.GetInt32("customer");
                    nextTransaction.date = reader.GetDateTime("date");
                    nextTransaction.amount = reader.GetDecimal("amount");

                    result.Add(nextTransaction);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()} occurred.");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }

        public static Transaction queryTransaction(int id)
        {
            Transaction result = null;

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM transactions " +
                    "INNER JOIN invoice_items ON transactions.invoice_no = invoice_items.invoice ";
                cmd.CommandText += $" WHERE transactions.invoice_no = @invoiceno";
                cmd.Parameters.AddWithValue("@invoiceno", id);
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Transaction();
                    result.customer = reader.GetInt32("customer");
                    result.date = reader.GetDateTime("date");
                    result.amount = reader.GetDecimal("amount");

                    // Add first item in result set
                    InvoiceItem firstInvoiceItem = new InvoiceItem();
                    firstInvoiceItem.invoice = id;
                    firstInvoiceItem.product = reader.GetString("product");
                    firstInvoiceItem.recordId = reader.GetInt32("record_id");
                    result.items.Add(firstInvoiceItem);
                }

                // Add subsequent items
                while (reader.Read())
                {
                    InvoiceItem nextInvoiceItem = new InvoiceItem();
                    nextInvoiceItem.invoice = id;
                    nextInvoiceItem.product = reader.GetString("product");
                    nextInvoiceItem.recordId = reader.GetInt32("record_id");
                    result.items.Add(nextInvoiceItem);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()} occurred.");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }

        public static List<Customer> queryAllCustomers()
        {
            List<Customer> result = new List<Customer>();
            
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM customers";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer nextCustomer = new Customer();
                    nextCustomer.customerId = reader.GetInt32("customer_id");
                    nextCustomer.firstName = reader.GetString("firstname");
                    nextCustomer.lastName = reader.GetString("lastname");
                    nextCustomer.email = reader.GetString("email");
                    nextCustomer.balance = reader.GetDecimal("balance");
                    nextCustomer.address = reader.GetString("address");
                    nextCustomer.state = reader.GetString("state");
                    nextCustomer.postcode = reader.GetString("postcode");
                    nextCustomer.phone = reader.GetString("phone");
                    result.Add(nextCustomer);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()} occurred.");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }

        public static Customer queryCustomersById(int id)
        {
            Customer result = null;

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM customers WHERE customer_id = @custid";
                cmd.Parameters.AddWithValue("@custid", id);
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Customer();
                    result.customerId = reader.GetInt32("customer_id");
                    result.firstName = reader.GetString("firstname");
                    result.lastName = reader.GetString("lastname");
                    result.email = reader.GetString("email");
                    result.balance = reader.GetDecimal("balance");
                    result.address = reader.GetString("address");
                    result.state = reader.GetString("state");
                    result.postcode = reader.GetString("postcode");
                    result.phone = reader.GetString("phone");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()} occurred.");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return result;
        }

        public static bool addNewCustomer(Customer newCustomer)
        {
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO customers (firstname, lastname, address, state, postcode, phone, email) " +
                    "VALUES (@fn, @ln, @ad, @st, @pc, @ph, @em)";

                cmd.Parameters.AddWithValue("@fn", newCustomer.firstName);
                cmd.Parameters.AddWithValue("@ln", newCustomer.lastName);
                cmd.Parameters.AddWithValue("@ad", newCustomer.address);
                cmd.Parameters.AddWithValue("@st", newCustomer.state);
                cmd.Parameters.AddWithValue("@pc", newCustomer.postcode);
                cmd.Parameters.AddWithValue("@ph", newCustomer.phone);
                cmd.Parameters.AddWithValue("@em", newCustomer.email);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()} occurred.");
                return false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return true;
        }

        public static bool updateCustomer(Customer customer)
        {
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connString;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE customers SET " +
                    "firstname = @fn, " +
                    "lastname = @ln, " +
                    "address = @ad, " +
                    "state = @st, " +
                    "postcode = @pc, " +
                    "phone = @ph, " +
                    "email = @em " +
                    "WHERE customer_id = @id";

                cmd.Parameters.AddWithValue("@id", customer.customerId);
                cmd.Parameters.AddWithValue("@fn", customer.firstName);
                cmd.Parameters.AddWithValue("@ln", customer.lastName);
                cmd.Parameters.AddWithValue("@ad", customer.address);
                cmd.Parameters.AddWithValue("@st", customer.state);
                cmd.Parameters.AddWithValue("@pc", customer.postcode);
                cmd.Parameters.AddWithValue("@ph", customer.phone);
                cmd.Parameters.AddWithValue("@em", customer.email);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show($"EXCEPTION {e.ToString()} occurred.");
                return false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }

            return true;
        }
    }
}