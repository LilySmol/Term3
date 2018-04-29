using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using TErm.Models;

namespace TErm.Helpers.DataBase
{
    public class DataBaseHelper
    {
        private string connection;

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="userID">id пользователя</param>
        public void insertOrUpdateDataForUser(int userID)
        {

        }

        /// <summary>
        /// Добавляет данные пользователя в базу
        /// </summary>
        /// <param name="user"></param>
        public void insertDataForUser(UserModel user)
        {
            SQLiteConnection sqliteCon = new SQLiteConnection(connection);
        }

    }
}