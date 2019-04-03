using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models
{
    public class ProductImg
    {
        public ProductImg(int id, string imagePath, int colorId)
        {
            Id = id;
            ImagePath = imagePath;
            ColorId = colorId;
        }

        public int Id { set; get; }
        public string ImagePath { set; get; }
        public int ColorId { set; get; }
    }
}