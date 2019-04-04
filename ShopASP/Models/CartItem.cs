using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class CartItem
    {
        public CartItem(Product product, int quantity)
        {
            Product = product;
            quantity = quantity;
        }

        public Product Product { get; set; }
        public int quantity { get; set; }
        
    }
}