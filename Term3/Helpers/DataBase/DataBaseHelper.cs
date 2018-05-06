using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using TErm.Helpers.DataBase;
using TErm.Helpers.Integration;
using TErm.Models;

namespace Term3.Helpers.DataBase
{
    public class DataBaseHelper
    {
        static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());

        /// <summary>
        /// Обновляет проекты и задачи пользователя
        /// </summary>
        public void update(UserModel user)
        {
           
        }

        public static List<ProjectModel> getProjectsList(string token, string user)
        {
            GitLabParser gitLabParser = new GitLabParser();
            gitLabParser.baseUrl = resource.GetString("baseUrl");
            return gitLabParser.getProjectsListByPrivateToken(token, user);
        }
    }
}