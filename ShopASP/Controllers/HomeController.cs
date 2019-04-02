using ShopASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopASP.Controllers
{
    public class HomeController : Controller
    {
        dbShopASPDataContext dbShopASP = new dbShopASPDataContext();

        private List<Product> GetAllProducts()
        {
            var listProducts = (from a in dbShopASP.products
                               join b in dbShopASP.product_details
                               on a.product_id equals b.product_id
                               join c in dbShopASP.product_imgs
                               on a.product_id equals c.product_id
                               select new {
                                   a.product_id,
                                   a.product_price,
                                   b.product_name,
                                   c.product_img_path
                               }).ToList();
            List<Product> products = new List<Product>();
            int i = 0;
            foreach(var product in listProducts)
            {
                products.Add(new Product());
                products[i].ID = product.product_id;
                products[i].Name = product.product_name;
                products[i].ImagePath = product.product_img_path;
                products[i].Price = (double)product.product_price;
                i++;
            }
            return products;
            //return dbShopASP.products.ToList();
        } 

        public ActionResult Index()
        {
            return View(GetAllProducts());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}