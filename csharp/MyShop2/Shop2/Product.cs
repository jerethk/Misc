using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB2
{
    class Product
    {
        public string productCode { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public decimal price { get; set; }
        public string imageFile { get; set; }
        public string category { get; set; }
        public Bitmap imageBitmap { get; set; }

        public int stock { get; set; }

    }
}
