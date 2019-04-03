using ShopASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopASP.Controllers
{
    public class ShopController : Controller
    {
        dbShopASPDataContext db = new dbShopASPDataContext();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product(int id)
        {
           var productFromDb = (from a in db.products
                           join b in db.product_details
                            on a.product_id equals b.product_id
                           join c in db.product_imgs
                            on a.product_id equals c.product_id
                           join d in db.colors
                            on c.color_id equals d.color_id
                           where a.product_id == id
                           select new
                           {
                               a.product_id,
                               a.product_quantum,
                               a.product_price,
                               b.product_decrible,
                               b.product_tag,
                               c.color_id,
                               d.color_name,
                               d.color_hex,
                               b.product_name,
                               c.product_img_path
                           }).ToList();
            Product product = new Product();

            product.Id = productFromDb[0].product_id;
            product.Price =(float) productFromDb[0].product_price;
            product.Quantum = (int)productFromDb[0].product_quantum;
            product.Tag = productFromDb[0].product_tag;
            product.Name = productFromDb[0].product_name;
            product.Describle = productFromDb[0].product_decrible;
            product.ImagePaths = new List<ProductImg>();
            product.Colors = new List<Color>();

            foreach (var i in productFromDb)
            { 
                product.ImagePaths.Add(new ProductImg(i.product_id, i.product_img_path, i.color_id));
                product.Colors.Add(new Color(i.color_id, i.color_name, i.color_hex));
            }

            return View(product);
        }
    }
}