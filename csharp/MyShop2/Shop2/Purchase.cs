using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB2
{
    /// <summary>
    /// This is the class representing purchases.
    /// </summary>
    class Purchase
    {
        private int itemCount;
        private decimal totalPrice;
        private List<Product> productList;

        public Purchase()
        {
            this.itemCount = 0;
            this.totalPrice = 0;
            this.productList = new List<Product>();
        }

        public int AddProduct(Product product)
        {
            // Add product and return size of list
            productList.Add(product);
            itemCount++;

            return itemCount;
        }

        public int RemoveProduct(int index)
        {
            productList.RemoveAt(index);
            itemCount--;

            return itemCount;
        }

        public int GetCount()
        {
            return itemCount;
        }

        private decimal calculateTotalPrice()
        {
            decimal result = 0;

            foreach (Product product in this.productList)
            {
                result += product.price;
            }

            this.totalPrice = result;
            return result;
        }

        public decimal GetTotalPrice()
        {
            return calculateTotalPrice();
        }

        
        public BindingList<Product> GetProductList()
        {
            return new BindingList<Product>(productList);
        }
    }
}
