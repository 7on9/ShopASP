using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Color
    {
        public Color(int id, string name, string hex)
        {
            Id = id;
            Name = name;
            Hex = hex;
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public string Hex { set; get; }
    }
}