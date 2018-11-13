﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Models
{
    public class UserModel
    {
        private int id;
        private string username;
        private string token;
        private static List<ProjectModel> projects;
        private int idProjectForPrognosis;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        public static List<ProjectModel> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
        public int IdProjectForPrognosis
        {
            get { return idProjectForPrognosis; }
            set { idProjectForPrognosis = value; }
        }
    }
}