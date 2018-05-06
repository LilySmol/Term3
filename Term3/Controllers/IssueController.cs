using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Term3.Controllers
{
    public class IssueController : Controller
    {
        // GET: Issue
        public ActionResult Issues(int userID, string projectTitle)
        {
            return View();
        }
    }
}