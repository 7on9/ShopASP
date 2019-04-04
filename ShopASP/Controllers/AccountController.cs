using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ShopASP.Models;
using ShopASP.Models.Utility;
using ShopASP.Models.ViewModel;

namespace ShopASP.Controllers
{
    public class AccountController : Controller
    {
        private dbShopASPDataContext db = new dbShopASPDataContext();

        public AccountController()
        {
        }

        // GET: /Account/Login

        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            var customer = db
                .customers
                .SingleOrDefault
                (
                    n => (n.customer_email == model.Email.ToLower() &&
                    n.customer_password == Utility.ComputeSha256Hash(model.Password))
                );
            if (customer != null)
            {
                Session["account"] = customer;
                return RedirectToAction("Index", "Home");
            }
            ViewData["Error"] = "Đăng nhập thất bại";
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", new { Controller = "Home" });
        }

        public ActionResult EditInfor()
        {
            if(Session == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditInfor(CustomerViewModels form)
        { 
            customer thisCustomer = (customer)Session["account"];
            HttpPostedFileBase file = form.ImagePath;
            if(file != null && file.ContentLength > 0 )
            {
                string extend = Path.GetExtension(file.FileName);
                string fileName = Utility.ComputeSha256Hash((thisCustomer.customer_email)) + extend;
                string path = Path.Combine(Server.MapPath(Utility.PATH_IMG_PRODUCTS), fileName);
                
                var customer = db
                .customers
                .SingleOrDefault
                (
                    n => (n.customer_email == thisCustomer.customer_email)
                );
                if (customer != null)
                {
                    customer.customer_dob = form.Dob;
                    customer.customer_gender = form.Gender;
                    customer.customer_phone = form.Phone;
                    customer.last_update = DateTime.Now;
                    customer.customer_address = form.Address;

                    customer_img customer_Img = new customer_img();
                    customer_Img.customer_id = customer.customer_id;
                    customer_Img.customer_img_path = Utility.CorrectPath(path);

                    file.SaveAs(path);
                    db.ExecuteQuery<customer_img>("insert into customer_img values ({0}, {1})",customer.customer_id, customer_Img.customer_img_path);
                    //db.customer_imgs.InsertOnSubmit(customer_Img);
                    db.SubmitChanges();

                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
            //return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register

        public ActionResult Register()
        {
            if (Session != null)
            {
                Session.Clear();
            }
            return View();
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection form, customer customer)
        {
            bool valid = true;
            var name = form["Name"];
            var email = form["Email"];
            var pass = form["Password"];
            var reTypePass = form["ReTypePassword"];

            if (String.IsNullOrEmpty(name))
            {
                valid = false;
                ViewData["ErrorName"] = "Trường bắt buộc";
            }
            if (String.IsNullOrEmpty(email))
            {
                valid = false;
                ViewData["ErrorEmail"] = "Trường bắt buộc";

            }
            else
            {
                var res = (from a in db.customers
                           where a.customer_email == email
                           select new
                           {
                               a.customer_email
                           }).Count();
                if (res > 0)
                {
                    valid = false;
                    ViewData["ErrorEmail"] = "Email đã được sử dụng";
                }
            }
            if (String.IsNullOrEmpty(pass))
            {
                valid = false;
                ViewData["ErrorPassword"] = "Trường bắt buộc";
            }
            if (String.IsNullOrEmpty(reTypePass))
            {
                valid = false;
                ViewData["ErrorReTypePassword"] = "Trường bắt buộc";
            }
            else
            {
                if (!reTypePass.Equals(pass))
                {
                    ViewData["ErrorReTypePassword"] = "Mật khẩu nhập lại không đúng";
                }
            }
            if (valid)
            {
                customer.customer_name = name;
                customer.customer_password = Utility.ComputeSha256Hash(pass);
                customer.customer_email = email.ToLower();
                customer.customer_level = 0;
                DateTime now = DateTime.Now;
                customer.time_create = now;
                customer.last_update = now;
                db.customers.InsertOnSubmit(customer);
                db.SubmitChanges();
                Session["account"] = customer;
                return RedirectToAction("EditInfor");

            }
            return this.Register();
        }
    }     
}