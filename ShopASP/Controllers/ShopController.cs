using ShopASP.Models;
using ShopASP.Models.Utility;
using ShopASP.Models.ViewModel;
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

        public Product GetProduct(int id)
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
            product.Price = (float)productFromDb[0].product_price;
            product.Quantum = (int)productFromDb[0].product_quantum;
            product.Tag = productFromDb[0].product_tag;
            product.Name = productFromDb[0].product_name;
            product.Describle = productFromDb[0].product_decrible;

            foreach (var i in productFromDb)
            {
                product.ImagePaths = new ProductImg(i.product_id, i.product_img_path, i.color_id);
                product.Colors = new Color(i.color_id, i.color_name, i.color_hex);
            }
            return product;
        }

        public ActionResult Product(int id)
        {
            ViewBag.Color = db.colors.ToList();
            ViewBag.Size = db.sizes.ToList();
            return View(GetProduct(id));
        }

        public ActionResult AddToCart(FormCollection cartItem)
        {
            List<CartItem> carts = Session["cart"] == null ? (new List<CartItem>()) : (List<CartItem>)Session["cart"];
            CartItem item = carts.Find(m => m.Product.Id == int.Parse(cartItem["IdProduct"]));
            if(item != null)
            {
                item.Quantity = int.Parse(cartItem["quantity"]);
            }
            else
            {
                carts.Add(new CartItem(
                    GetProduct(int.Parse(cartItem["IdProduct"])), 
                    int.Parse(cartItem["quantity"]),
                    int.Parse(cartItem["color"]),
                    int.Parse(cartItem["size"])));
                Session["cart"] = carts;
            }
            return RedirectToAction("Cart", "Shop", carts);
        }

        public ActionResult RemoveFromCart(int id)
        {
            List<CartItem> carts = Session["cart"] == null ? (new List<CartItem>()) : (List<CartItem>)Session["cart"];
            CartItem item = carts.Find(m => m.Product.Id == id);
            if (item != null)
            {
                carts.Remove(item);
            }
            return RedirectToAction("Cart", "Shop", carts);
        }

        public ActionResult Cart()
        {
            return View(Session["cart"] == null ? (new List<CartItem>()) : (List<CartItem>)Session["cart"]);
        }

        public ActionResult CheckOut()
        {
            if (Session["customer"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult Thanks(FormCollection form)
        {
            if (Session["customer"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<CartItem> cart = Session["cart"] as List<CartItem>;
            cart = cart == null ? new List<CartItem>() : cart;

            cart newCart = new cart();
            try
            {
                db.ExecuteQuery<cart>("insert into cart values({0}, {1})",
                Utility.getNowToSQLDateTime(),
                0);
            }
            catch (Exception e){}
            
            //0 = chưa xử lý - 1 =  đã xử lý
            db.SubmitChanges();
            int cartId = db.carts.OrderByDescending(u => u.cart_id).FirstOrDefault().cart_id;
            foreach (CartItem item in cart)
            {
                cart_detail cart_Detail = new cart_detail();
                cart_Detail.cart_id = cartId;
                cart_Detail.product_id = item.Product.Id;
                cart_Detail.quantum = item.Quantity;
                cart_Detail.size_id = item.Size;
                cart_Detail.color_id = item.Color;
                db.ExecuteQuery<cart_detail>("insert into cart_detail values({0}, {1}, {2}, {3}, {4})",
                    cart_Detail.cart_id,
                    cart_Detail.product_id,
                    cart_Detail.quantum,
                    cart_Detail.size_id,
                    cart_Detail.color_id);
                db.SubmitChanges();
            }
            
            return View();
        }
    }
}