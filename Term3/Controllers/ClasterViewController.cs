using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TErm.Models;

namespace Term3.Controllers
{
    public class ClasterViewController : Controller
    {
        // GET: ClasterView
        public ActionResult Clasters()
        {
            List<ClasteringListModel> clastersList = new List<ClasteringListModel>();
            clastersList.Add(new ClasteringListModel("Авторизация по email", 14, new List<string>() { "Авторизация по логину и паролю", "Авторизация по токену", "Авторизация по номеру телефона" }));
            clastersList.Add(new ClasteringListModel("Добавление в БД пользователя", 10, new List<string>() { "Добавление в БД пользователя", "Добавление в БД покупок", "Добавление модератора" }));
            clastersList.Add(new ClasteringListModel("Фильтр по покупкам", 11, new List<string>() { "Редактирование списка покупок", "Добавить магазин на карту", "Добавить настройки системы", "Настройка темы приложения" }));
            return View(clastersList);
        }
    }
}