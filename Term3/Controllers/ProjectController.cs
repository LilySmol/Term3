using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Web.Mvc;
using TErm.Models;
using Term3.Helpers.Integration;

namespace Term3.Controllers
{
    public class ProjectController : Controller
    {
        private static int userId = 0;        
        private ProjectModel project = new ProjectModel();
        static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());

        // GET: Project
        public ActionResult Projects(int userID)
        {
            userId = userID;
            fillComboBox();
            return View(project);
        }

        [HttpPost]
        public ActionResult Projects(ProjectModel projectModel)
        { 
            if (projectModel.name == null) //обновить проекты и задачи пользователя
            {
                ServerRequests serverRequests = new ServerRequests();
                serverRequests.baseUrl = resource.GetString("baseUrl");
                serverRequests.updateProjects(userId);
                fillComboBox();
                return View(project);
            }     
            return RedirectToAction("Issues", "Issue", new { userID = userId, projectTitle = projectModel.name });
        }

        private void fillComboBox()
        {
            ServerRequests serverRequests = new ServerRequests();
            serverRequests.baseUrl = resource.GetString("baseUrl");
            List<ProjectModel> projects = serverRequests.getProjects(userId);
            project.projectsList = new List<SelectListItem>();
            foreach (ProjectModel projectObject in projects)
            {
                project.projectsList.Add(new SelectListItem { Text = projectObject.name, Value = projectObject.name });
            }
        }
    }
}