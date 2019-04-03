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
    public class AdminController : Controller
    {
        dbShopASPDataContext db = new dbShopASPDataContext();

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

        private List<Employee> GetAllEmployee()
        {
            var listAllEmployee = (from a in db.employees
                                   select new
                                   {
                                       a.employee_id,
                                       a.employee_name,
                                       a.employee_gender,
                                       a.employee_address,
                                       a.employee_email,
                                       a.employee_phone,
                                       a.employee_dob
                                   }).ToList();
            List<Employee> employees = new List<Employee>();
            int i = 0;
            foreach (var employee in listAllEmployee)
            {
                employees.Add(new Employee());
                employees[i].ID = employee.employee_id;
                employees[i].Name = employee.employee_name;
                employees[i].Gender = (bool)employee.employee_gender;
                employees[i].Address = employee.employee_address;
                employees[i].Email = employee.employee_email;
                employees[i].Phone = employee.employee_phone;
                employees[i].Dob = employee.employee_dob;
            }

            return employees;
        }
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        public ActionResult Customer()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(GetAllCustomer());
        }

        public ActionResult Employee()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(GetAllEmployee());
        }

        public ActionResult CreateNewAccount()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        // GET: /Admin/Login

        public ActionResult Login()
        {
            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminViewModels model)
        {
            account account = new account();
            account = db
                .accounts
                .SingleOrDefault
                (
                    n => (n.account_username == model.Username.ToLower() &&
                    n.account_password == Utility.ComputeSha256Hash(model.Password))
                );
            if (account != null)
            {
                Session["admin"] = account;
                return RedirectToAction("Index", "Admin");
            }
            ViewData["Error"] = "Đăng nhập thất bại";
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", new { Controller = "Admin" });
        }

        [HttpPost]
        public ActionResult CreateNewAccount(EmployeeViewModels form)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            account thisAccount = (account)Session["admin"];
            HttpPostedFileBase file = form.ImagePath;
            if (file != null && file.ContentLength > 0)
            {
                string extend = Path.GetExtension(file.FileName);
                string fileName = Utility.ComputeSha256Hash((thisAccount.account_username)) + extend;
                string path = Path.Combine(Server.MapPath(Utility.PATH_IMG_PRODUCTS), fileName);

                employee newEmployee = new employee();

                newEmployee.employee_name = form.Name;
                newEmployee.employee_email = form.Email;
                newEmployee.employee_dob = form.Dob;
                newEmployee.employee_gender = form.Gender;
                newEmployee.employee_phone = form.Phone;
                newEmployee.employee_address = form.Address;

                db.employees.InsertOnSubmit(newEmployee);
                db.SubmitChanges();
                newEmployee.employee_id = db.employees.OrderByDescending(u => u.employee_id).FirstOrDefault().employee_id;


                employee_img employee_Img = new employee_img();
                employee_Img.employee_id = newEmployee.employee_id;
                employee_Img.employee_img_path = path;

                file.SaveAs(path);
                db.ExecuteQuery<employee_img>("insert into employee_img values ({0}, {1})", newEmployee.employee_id, path);
                //db.customer_imgs.InsertOnSubmit(customer_Img);
                db.SubmitChanges();

                return RedirectToAction("Index", "Admin");
            }
            return View();

        }
            
        //return RedirectToAction("Index", "Home");
    }
}