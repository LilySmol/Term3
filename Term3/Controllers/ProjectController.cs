using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TErm.Helpers.DataBase;
using TErm.Models;

namespace Term3.Controllers
{
    public class ProjectController : Controller
    {
        private static int userId = 0;

        // GET: Project
        public ActionResult Projects(int userID)
        {
            userId = userID;
            ProjectModel project = new ProjectModel();
            DataTable projectsTable = new DataTable();
            if (userID != 0) //пользователь есть в базе данных
            {
                projectsTable = DataBaseRequest.getProjects(userID);
            }
            project.projectsList = new List<SelectListItem>();
            for (int i = 0; i < projectsTable.Rows.Count; i++)
            {
                project.projectsList.Add(new SelectListItem { Text = projectsTable.Rows[0]["name"].ToString(), Value = projectsTable.Rows[0]["name"].ToString() });
            }
            //project.projectsList.Add(new SelectListItem {Text = "p1", Value = "sfs"});
            //project.projectsList.Add(new SelectListItem { Text = "p2", Value = "2sfs" });
            //project.projectsList.Add(new SelectListItem { Text = "p3", Value = "3sfsf" });
            return View(project);
        }

        [HttpPost]
        public ActionResult Submit(ProjectModel project)
        {
            string projectName = project.name;
            //return RedirectToAction("Issues", "Issue", new { projectID = projectId });
            DataTable issues = DataBaseRequest.getIssues(userId, project.name);
            return View();
        }
    }
}