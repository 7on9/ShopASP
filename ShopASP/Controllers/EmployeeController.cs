using ShopASP.Models;
using ShopASP.Models.Utility;
using ShopASP.Models.ViewModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopASP.Controllers
{
    public class EmployeeController : Controller
    {
        dbShopASPDataContext db = new dbShopASPDataContext();
        private List<Cart> waitingCarts = DbInteract.GetAllWaitingCart();
        // GET: Employee
        public ActionResult Index()
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            ViewBag.Carts = waitingCarts;
            return View();
        }

        // GET: /Employee/Login

        public ActionResult Login()
        {
            return View();
        }

        // POST: /Employee/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeViewModels model)
        {
            if (model.Email == null || model.Password == null)
            {
                ViewData["Error"] = "Hãy điền đầy đủ các trường";
            }
            else
            {
                var employee = db
                .employees
                .SingleOrDefault
                (
                    n => (n.employee_email == model.Email.ToLower() &&
                    n.employee_password == Utility.ComputeSha256Hash(model.Password))
                );
                if (employee != null)
                {
                    Session["employee"] = employee;
                    return RedirectToAction("Index", "Employee");
                }
                ViewData["Error"] = "Đăng nhập thất bại";
            }
            return View(model);
        }

        private List<Customer> GetAllCustomer()
        {
            var listAllCustomer = (from a in db.customers
                                   select new
                                   {
                                       a.customer_id,
                                       a.customer_name,
                                       a.customer_gender,
                                       a.customer_address,
                                       a.customer_email,
                                       a.customer_phone,
                                       a.customer_dob
                                   }).ToList();
            List<Customer> customers = new List<Customer>();
            int i = 0;
            foreach (var customer in listAllCustomer)
            {
                customers.Add(new Customer());
                customers[i].ID = customer.customer_id;
                customers[i].Name = customer.customer_name;
                customers[i].Gender = (bool)customer.customer_gender;
                customers[i].Address = customer.customer_address;
                customers[i].Email = customer.customer_email;
                customers[i].Phone = customer.customer_phone;
                customers[i].Dob = customer.customer_dob;
                i++;
            }

            return customers;
        }

        private List<Cart> GetFullDetailOfCart(List<Cart> listCart)
        {
            List<Cart> carts = new List<Cart>();
            carts.AddRange(listCart);
            foreach (var cart in carts)
            {
                var products = (from a in db.cart_details
                                where a.cart_id.Equals(cart.Id)
                                select new
                                {
                                    a.cart_id,
                                    a.product_id,
                                    a.quantum,
                                    a.color_id,
                                    a.size_id
                                }).ToList();
                cart.Product = new List<CartItem>();
                foreach (var product in products)
                {
                    cart.Product.Add(
                        new CartItem(DbInteract.GetProduct(product.product_id), 
                        product.quantum, 
                        product.color_id, 
                        product.size_id));
                }
            }

            return carts;
        }

        public List<Product> GetAllProduct()
        {
            var productFromDb = (from a in db.products
                                 join b in db.product_details
                                  on a.product_id equals b.product_id
                                 join c in db.product_imgs
                                  on a.product_id equals c.product_id
                                 join d in db.colors
                                  on c.color_id equals d.color_id
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
            Product product;
            List<Product> products = new List<Product>();
            for(int i = 0; i < productFromDb.Count; i++)
            {
                product = new Product();
                product.Id = productFromDb[i].product_id;
                product.Price = (float)productFromDb[i].product_price;
                product.Quantum = (int)productFromDb[i].product_quantum;
                product.Tag = productFromDb[i].product_tag;
                product.Name = productFromDb[i].product_name;
                product.Describle = productFromDb[i].product_decrible;

                product.ImagePaths = new ProductImg(productFromDb[i].product_id, productFromDb[i].product_img_path, productFromDb[i].color_id);
                product.Colors = new Color(productFromDb[i].color_id, productFromDb[i].color_name, productFromDb[i].color_hex);

                products.Add(product);
            }
            
            return products;
        }

        // GET 
        public ActionResult CreateNewProduct()
        {
            ViewBag.Color = db.colors.ToList();
            return View();
        }

        // GET 
        public ActionResult Cart()
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            ViewBag.Carts = waitingCarts;
            return View();
        }

        public ActionResult CartInfor(int id)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            ViewBag.Carts = waitingCarts;
            return View(DbInteract.GetFullDetailOfCart(id));
        }

        [HttpPost]
        public ActionResult CreateNewProduct(ProductViewModels form)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            ViewBag.Carts = waitingCarts;
            employee thisAccount = (employee)Session["employee"];
            HttpPostedFileBase file = form.ImagePath;
            if (file != null && file.ContentLength > 0)
            {
                string extend = Path.GetExtension(file.FileName);
                string fileName = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    .TotalSeconds
                    .ToString() + extend;
                string path = Path.Combine(Server.MapPath(Utility.PATH_IMG_PRODUCTS), fileName);

                product newProduct = new product();

                newProduct.product_price = form.Price;
                newProduct.product_quantum = form.Quantum;
                db.products.InsertOnSubmit(newProduct);
                db.SubmitChanges();
                newProduct.product_id = db.products.OrderByDescending(u => u.product_id).FirstOrDefault().product_id;

                product_detail product_Detail = new product_detail();

                product_img product_Img = new product_img();
                product_Img.product_id = newProduct.product_id;
                product_Img.product_img_path = Utility.CorrectPath(path);

                file.SaveAs(path);
                db.ExecuteQuery<product_detail>("Insert into product_detail " +
                    "values({0}, {1}, {2}, {3})",
                    newProduct.product_id, form.Name, form.Tag, form.Decrible);
                db.ExecuteQuery<product_img>("insert into product_img values ({0}, {1}, {2})", newProduct.product_id, product_Img.product_img_path, form.ProductColor);
                //db.customer_imgs.InsertOnSubmit(customer_Img);
                db.SubmitChanges();

                return RedirectToAction("Product", "Employee");
            }
            return View();

        }

        public ActionResult Product()
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            ViewBag.Carts = waitingCarts;
            return View(GetAllProduct());
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", new { Controller = "Employee" });
        }
    }
}