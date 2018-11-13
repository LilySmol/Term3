using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TErm.Models;
using System.Resources;
using System.Reflection;
using TErm.Helpers.Integration;
using TErm.Helpers.Clustering;
using NLog;
using TErm.Helpers.DataBase;
using Term3.Helpers.DataBase;
using Term3.Helpers.Integration;

namespace TErm.Controllers
{
    public class HomeController : Controller
    {       
        public ActionResult Index()
        {
            return View();
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

        [HttpPost]
        public ActionResult Index(UserModel person)
        {
            int userId = 0;
            string privateToken = person.Token;
            string name = person.Username;
            ServerRequests serverRequests = new ServerRequests();
            userId = serverRequests.addUser(person.Token, person.Username);
            if (userId == 0)            
            {
                return View();
            }       
            return RedirectToAction("Projects", "Project", new { userID = userId});
        }   
    }
}