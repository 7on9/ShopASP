using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Product
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public double Price { set; get; }
        public string ImagePath { set; get; }
    }
}