using ShopASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopASP.Controllers
{
    public class AdminController : Controller
    {
        dbShopASPDataContext dbShopASP = new dbShopASPDataContext();

        private List<Customer> GetAllCustomer()
        {
            var listAllCustomer = (from a in dbShopASP.customers
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
            var listAllEmployee = (from a in dbShopASP.employees
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
            return View();
        }

        public ActionResult Customer()
        {
            return View(GetAllCustomer());
        }

        public ActionResult Employee()
        {
            return View(GetAllEmployee());
        }

        public ActionResult CreateNewAccount()
        {
            return View();
        }
    }
}