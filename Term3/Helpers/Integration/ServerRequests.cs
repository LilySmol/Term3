using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web;
using TErm.Helpers.Integration;
using TErm.Models;

namespace Term3.Helpers.Integration
{
    public class ServerRequests: Requests
    {
        private static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());
        private string baseUrl;

        /// <summary>
        /// Добавить пользователя.
        /// </summary>
        public int addUser(string token, string userName)
        {
            baseUrl = resource.GetString("baseUrl");
            var body = new NameValueCollection();
            body["token"] = token;
            body["username"] = userName;
            string response = post(baseUrl + "api/add-user", body);
            if (response.Equals(""))
            {
                return 0;
            }
            UserModel responseObject = JsonConvert.DeserializeObject<UserModel>(response);           
            return responseObject.Id;
        }

        /// <summary>
        /// Получить список проектов пользователя по id.
        /// </summary>
        public List<ProjectModel> getProjects(int userId)
        {
            baseUrl = resource.GetString("baseUrl");
            var body = new NameValueCollection();
            body["id"] = userId.ToString();
            string response = post(baseUrl + "api/get-projects", body);
            if (response.Equals(""))
            {
                return null;
            }
            List<ProjectModel> projectsList = JsonConvert.DeserializeObject<List<ProjectModel>>(response);
            return projectsList;
        }
    }
}