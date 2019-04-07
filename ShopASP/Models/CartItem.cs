using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class CartItem
    {
        public CartItem(Product product, int quantity, int color, int size)
        {
            Product = product;
            Quantity = quantity;
            Color = color;
            Size = size;
        }

        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
    }
}