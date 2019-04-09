using ShopASP.Models;
using ShopASP.Models.Utility;
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
                                select new
                                {
                                    a.product_id,
                                    a.product_price,
                                    b.product_name,
                                    c.product_img_path,
                                    c.color_id
                                }).ToList();
            List<Product> products = new List<Product>();
            int i = 0;
            foreach (var product in listProducts)
            {
                products.Add(new Product());
                products[i].Id = product.product_id;
                products[i].Name = product.product_name;
                products[i].ImagePaths = new ProductImg(product.product_id, product.product_img_path, product.color_id);
                products[i].Price = (float)product.product_price;
                i++;
            }
            return products;
            //return dbShopASP.products.ToList();
        }

        public ActionResult Index()
        {
            ViewBag.Top5 = DbInteract.GetTopProducts(5);
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