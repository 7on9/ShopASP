using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Customer
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public bool Gender { set; get; }
        public string Dob { set; get; }
        public string Password { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string ImagePath { set; get; }
    }
}