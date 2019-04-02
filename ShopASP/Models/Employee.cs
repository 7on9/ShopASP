using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Employee
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public bool Gender { set; get; }
        public String Dob { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string ImagePath { set; get; }
        public byte Role { set; get; }
    }
}