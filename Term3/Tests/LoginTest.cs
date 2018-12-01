//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Resources;
//using System.Web;
//using TErm.Controllers;
//using TErm.Helpers.DataBase;
//using TErm.Helpers.Integration;
//using TErm.Models;
//using Term3.Controllers;
//using Term3.Helpers.DataBase;

//namespace Term3.Tests
//{
//    [TestFixture]
//    public class LoginTest
//    {
//        GitLabParser gitLabParser = new GitLabParser();
//        ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());
//        string userName;
//        string token;

//        [SetUp]
//        public void beforeTest()
//        {
//            gitLabParser.baseUrl = resource.GetString("baseUrl");
//        }

//        [Test, Timeout(100000)]
//        [Property("Test", "T-1")]
//        public void correctUsernameToken()
//        {
//            userName = "LilySmol";
//            token = "GG8RjMH3TyguYqP6FBxu";            
//            List<ProjectModel> projectList = gitLabParser.getProjectsListByPrivateToken(token, userName);
//            Assert.True(projectList != null);
//        }

//        [Test, Timeout(100000)]
//        [Property("Test", "T-2")]
//        public void incorrectUsernameToken()
//        {
//            userName = "incorrectUser";
//            token = "incorrectPassword";
//            List<ProjectModel> projectList = gitLabParser.getProjectsListByPrivateToken(token, userName);
//            Assert.True(projectList == null);
//        }

//        [Test, Timeout(100000)]
//        [Property("Test", "T-3")]
//        public void emptyUsernameToken()
//        {
//            userName = "";
//            token = "";
//            List<ProjectModel> projectList = gitLabParser.getProjectsListByPrivateToken(token, userName);
//            Assert.True(projectList == null);
//        }
//    }

//    [TestFixture]
//    public class DataBaseTest
//    {
//        ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());

//        [Test, Timeout(100000)]
//        [Property("Test", "Т-10")]
//        public void insertUser()
//        {
//            string name = resource.GetString("dbInsertUser");
//            string token = resource.GetString("dbInsertToken");
//            DataBaseRequest.insertUser(name, token);
//        }

//        [Test, Timeout(100000)]
//        [Property("Test", "Т-11")]
//        public void insertProject()
//        {
//            int projectId = Convert.ToInt32(resource.GetString("dbProjectId"));
//            int userId = Convert.ToInt32(resource.GetString("dbUserId"));
//            addProject(projectId, userId);
//        }

//        [Test, Timeout(100000)]
//        [Property("Test", "Т-12")]
//        public void deleteProject()
//        {
//            int projectId = Convert.ToInt32(resource.GetString("dbProjectId"));
//            int userId = Convert.ToInt32(resource.GetString("dbUserId"));
//            addProject(projectId, userId);
//            DataBaseRequest.deleteProject(projectId);
//        }

//        private void addProject(int projectId, int userId)
//        {
//            string name = "Project";
//            string description = "description";
//            DataBaseRequest.insertProject(projectId, name, description, userId);
//        }
//    }
//}