using ShopASP.Models;
using ShopASP.Models.Utility;
using ShopASP.Models.ViewModel;
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
                i++;
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
                i++;
            }

            return employees;
        }

        private float GetTotalBill()
        {
            var bills = db.bills.ToList();
            float total = 0;
            foreach(var bill in bills)
            {
                total += DbInteract.GetTotalOfCart(bill.cart_id);
            }
            return total;
        }

        //public List<Product> GetAllProduct()
        //{
        //    var productFromDb = (from a in db.products
        //                         join b in db.product_details
        //                          on a.product_id equals b.product_id
        //                         join c in db.product_imgs
        //                          on a.product_id equals c.product_id
        //                         join d in db.colors
        //                          on c.color_id equals d.color_id
        //                         select new
        //                         {
        //                             a.product_id,
        //                             a.product_quantum,
        //                             a.product_price,
        //                             b.product_decrible,
        //                             b.product_tag,
        //                             c.color_id,
        //                             d.color_name,
        //                             d.color_hex,
        //                             b.product_name,
        //                             c.product_img_path
        //                         }).ToList();
        //    Product product;
        //    List<Product> products = new List<Product>();
        //    for (int i = 0; i < productFromDb.Count; i++)
        //    {
        //        product = new Product();
        //        product.Id = productFromDb[i].product_id;
        //        product.Price = (float)productFromDb[i].product_price;
        //        product.Quantum = (int)productFromDb[i].product_quantum;
        //        product.Tag = productFromDb[i].product_tag;
        //        product.Name = productFromDb[i].product_name;
        //        product.Describle = productFromDb[i].product_decrible;

        //        product.ImagePaths = new ProductImg(productFromDb[i].product_id, productFromDb[i].product_img_path, productFromDb[i].color_id);
        //        product.Colors = new Color(productFromDb[i].color_id, productFromDb[i].color_name, productFromDb[i].color_hex);

        //        products.Add(product);
        //    }

        //    return products;
        //}

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Customers = db.customers.ToList();
            ViewBag.Employees = db.employees.ToList();
            ViewBag.Products = db.products.Where(m => m.product_quantum >= 0).ToList();
            ViewBag.Bills = db.bills.ToList();
            ViewBag.Total = GetTotalBill();
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

        public ActionResult CreateNewEmployee()
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
        public ActionResult CreateNewEmployee(EmployeeViewModels form)
        {
            if (!form.Password.Equals(form.RetypePassword))
            {
                ViewData["errorPass"] = "Mật khẩu gõ lại phải trùng khớp";
            }
            else
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
                    newEmployee.employee_password = Utility.ComputeSha256Hash(form.Password);

                    db.employees.InsertOnSubmit(newEmployee);
                    db.SubmitChanges();
                    newEmployee.employee_id = db.employees.OrderByDescending(u => u.employee_id).FirstOrDefault().employee_id;


                    employee_img employee_Img = new employee_img();
                    employee_Img.employee_id = newEmployee.employee_id;
                    employee_Img.employee_img_path = Utility.CorrectPath(path);

                    file.SaveAs(path);
                    db.ExecuteQuery<employee_img>("insert into employee_img values ({0}, {1})", newEmployee.employee_id, employee_Img.employee_img_path);
                    //db.customer_imgs.InsertOnSubmit(customer_Img);
                    db.SubmitChanges();

                    return RedirectToAction("Employee", "Admin");
                }
            }
            return View();

        }
            
        //return RedirectToAction("Index", "Home");
    }
}