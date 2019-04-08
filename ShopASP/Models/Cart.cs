using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Cart
    {
        public int Id { set; get; }
        public Customer Customer { set; get; }
        public List<CartItem> Product { set; get; }
        public string TimeCreate { get; set; }
        public bool Status { set; get; }
    }
}