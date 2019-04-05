using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopASP.Models.ViewModel
{
    public class CustomerViewModels
    {
        [Display(Name = "Mã")]
        public int ID { set; get; }
        [Display(Name = "Tên")]
        public string Name { set; get; }
        [Required, Display(Name = "Địa chỉ")]
        public string Address { set; get; }
        [Required, Display(Name = "Giới tính")]
        public bool Gender { set; get; }
        [Required, Display(Name = "Ngày tháng năm sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string Dob { set; get; }
        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }
        [Required, Display(Name = "Số điện thoại")]
        public string Phone { set; get; }
        [Display(Name = "Email")]
        public string Email { set; get; }
        [Display(Name = "Hình đại diện")]
        public HttpPostedFileBase ImagePath { set; get; }
    }
}