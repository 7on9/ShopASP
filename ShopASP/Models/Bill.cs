using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Bill
    {
        public int Id { set; get; }
        public Cart Cart {set; get;}
    }
}