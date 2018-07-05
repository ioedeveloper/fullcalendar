using SwiftKampus.Models;
using SwiftKampus.Services;
using SwiftKampus.ViewModels;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SwiftKampus.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Timetable()
        {
            //Income income = new Income();
            return View();
        }

        public JsonResult TimetableList()
        {
            Timetable obj = new Timetable();
            return Json(obj.TimetableList(), JsonRequestBehavior.AllowGet);
        }
    }
}