using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class Product
    {
        public int Id { set; get; }
        public int Quantum { set; get; }
        public string Name { set; get; }
        public float Price { set; get; }
        public string Describle { set; get; }
        public string Tag { set; get; }
        public List<ProductImg> ImagePaths { set; get; }
        public List<Color> Colors { set; get; }
    }
}