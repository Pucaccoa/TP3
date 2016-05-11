using _420_476_TP3_Pucacco_Alex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _420_476_TP3_Pucacco_Alex.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        static NORTHWNDEntities db = new NORTHWNDEntities();
        [HttpGet]
        public ActionResult Index()
        {
            Session.Abandon();
            return View();

        }
        public ActionResult Index(string Login,string Password)
        {
            var user  = db.Users.Where(u => u.Login.Equals(Login)).Where(u=> u.Password.Equals(Password));
            if(user.FirstOrDefault() != null)
            {
                Session["Connected"] = user.FirstOrDefault();
                Session["Name"] = user.FirstOrDefault().firstname + " " + user.FirstOrDefault().lastname;
                return RedirectToAction("Index", "Products");
            }
            else
            {
                ViewBag.hello = "Authentification Failed";
                return View();
            }
            
        }
        public ActionResult Disconnect()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }



    }
}