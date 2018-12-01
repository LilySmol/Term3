using System.Web.Mvc;
using TErm.Models;
using System.Resources;
using System.Reflection;
using Term3.Helpers.Integration;

namespace TErm.Controllers
{
    public class HomeController : Controller
    {
        static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());

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
            string privateToken = person.Token;
            string name = person.Username;
            ServerRequests serverRequests = new ServerRequests();
            serverRequests.baseUrl = resource.GetString("baseUrl");
            int userId = serverRequests.addUser(privateToken, name);
            if (userId == 0)            
            {
                return View();
            }       
            return RedirectToAction("Projects", "Project", new { userID = userId});
        }   
    }
}