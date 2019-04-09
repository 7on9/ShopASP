using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopASP.Models.ViewModel
{
    public class ProductViewModels
    {
        public ProductViewModels()
        {

        }

        public ProductViewModels(int id, int quantum, float price, string name, string decrible, string tag, int productColor)
        {
            Id = id;
            Quantum = quantum;
            Price = price;
            Name = name;
            Decrible = decrible;
            Tag = tag;
            ProductColor = productColor;
        }

        [Display (Name = "Mã sản phẩm")]
        public int Id { set; get; }

        [Required, Display(Name = "Số lượng")]
        public int Quantum { set; get; }

        [Required, Display(Name = "Giá")]
        public float Price { set; get; }

        [Display(Name = "Tên")]
        public string Name { set; get; }

        [Required, Display(Name = "Mô tả sản phẩm")]
        public string Decrible { set; get; }

        [Required, Display(Name = "Tag")]
        public string Tag { set; get; }

        [Required, Display(Name = "Màu sản phẩm")]
        public int ProductColor { set; get; }

        [Required, Display(Name = "Hình ảnh sản phẩm")]
        public HttpPostedFileBase ImagePath { set; get; }
    }
}