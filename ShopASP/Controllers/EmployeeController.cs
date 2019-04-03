﻿using ShopASP.Models;
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

        // GET: Employee
        public ActionResult Index()
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            return View();
        }

        // GET: /Account/Login

        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
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
            }

            return customers;
        }

        [HttpPost]
        public ActionResult CreateNewProduct(ProductViewModels form)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("Login", "Employee");
            }
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
                product_Img.product_img_path = path;

                file.SaveAs(path);
                db.ExecuteQuery<product_detail>("Insert into product_detail " +
                    "values({0}, {1}, {2}, {3})",
                    newProduct.product_id, form.Name, form.Tag, form.Decrible);
                db.ExecuteQuery<product_img>("insert into product_img values ({0}, {1})", newProduct.product_id, path);
                //db.customer_imgs.InsertOnSubmit(customer_Img);
                db.SubmitChanges();

                return RedirectToAction("Index", "Employee");
            }
            return View();

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", new { Controller = "Employee" });
        }
    }
}