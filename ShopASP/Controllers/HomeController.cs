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

        public ActionResult Index()
        {
            ViewBag.Top5 = DbInteract.GetTopProducts(5);
            return View(DbInteract.GetAllProducts());
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