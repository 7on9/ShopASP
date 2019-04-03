using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Color
    {
        public Color(int id, string name)
        {
            Id = id;
            Name = name; 
        }
        public int Id { set; get; }
        public string Name { set; get; }
    }
}