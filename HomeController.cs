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
       
        public ActionResult TimeTable(string coursebuilding,string coursetitle, string end, string start)
        {
            if(coursebuilding != null)
            {
                FullCalender calender = new FullCalender()
                {
                    Building = coursebuilding,
                    CourseTitle = coursetitle,
                    End = Convert.ToDateTime(end),
                    Start = Convert.ToDateTime(start)
                };
                _db.FullCalenders.Add(calender);
                _db.SaveChanges();
            }
           
            
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PlasuIndex()
        {
            return View();
        }

        public ActionResult GroupChat()
        {
            return View();
        }


        public ActionResult ApplicantDashBoard()
        {
            return View();
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult Calender()
        {
            ViewBag.Message = "Your contact page.";
            return PartialView();
        }

        public ActionResult MyNewDashboard()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> DashBoard()
        {
            int totalMaleStudent = await _db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Male"));
            int totalFemaleStudent = await _db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Female"));
            int active = await _db.Students.AsNoTracking().CountAsync(s => s.Active.Equals(true));
            int graduatedStudent = await _db.Students.AsNoTracking().CountAsync(s => s.IsGraduated.Equals(true));
            int totalStudent = await _db.Students.AsNoTracking().CountAsync();
            int totalStaff = await _db.Staffs.AsNoTracking().CountAsync();

            double val1 = totalMaleStudent * 100;
            double val2 = totalFemaleStudent * 100;

            double boysPercentage = Math.Round(val1 / totalStudent, 2);
            double femalePercentage = Math.Round(val2 / totalStudent, 2);

            ViewBag.MaleStudent = totalMaleStudent;
            ViewBag.Femalestudent = totalFemaleStudent;
            ViewBag.TotalStudent = totalStudent;
            ViewBag.TotalStaff = totalStaff;
            ViewBag.BoysPercentage = boysPercentage;
            ViewBag.FemalePercentage = femalePercentage;
            ViewBag.ActiveStudent = active;
            ViewBag.GraduatedStudent = graduatedStudent;
            ViewBag.Faculty = await _db.Faculties.AsNoTracking().CountAsync();
            ViewBag.Department = await _db.Departments.AsNoTracking().CountAsync();
            ViewBag.Programme = await _db.Programmes.AsNoTracking().CountAsync();

            ViewBag.Semester = new SelectList(_db.Semesters.AsNoTracking(), "SemesterName", "SemesterName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");

            var userActivity = await _query.UserActivityStatistic();
            ViewBag.OnlineUser = userActivity.Item1;
            ViewBag.OnlineUserPercentage = userActivity.Item2;
            ViewBag.AllUsers = userActivity.Item3;

            return View();
        }

        [RecurringAuthorize]
        public ActionResult SchoolSetUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SchoolSetUp(SetUpVm model)
        {
            string _FileName = String.Empty;
            if (model.File?.ContentLength > 0)
            {
                _FileName = Path.GetFileName(model.File.FileName);
                string _path = HostingEnvironment.MapPath("~/Content/Images/") + _FileName;
                var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Images/"));
                if (directory.Exists == false)
                {
                    directory.Create();
                }
                model.File.SaveAs(_path);
            }

            //ViewBag.Message = "File upload failed!!";
            //return View(model);

            Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            //Edit
            if (objAppsettings != null)
            {
                objAppsettings.Settings["SchoolName"].Value = model.SchoolName;
                objAppsettings.Settings["SchoolTheme"].Value = model.SchoolTheme.ToString();
                if (!String.IsNullOrEmpty(_FileName))
                {
                    objAppsettings.Settings["SchoolImage"].Value = _FileName;
                }
                objConfig.Save();
            }
            return View("Index");
        }

        public PartialViewResult NewCalender()
        {
            return PartialView();
        }

        public string Init()
        {
            bool rslt = Utils.InitialiseDiary();
            return rslt.ToString();
        }

        public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            DiaryEvent.UpdateDiaryEvent(id, NewEventStart, NewEventEnd);
        }

        public bool SaveEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration)
        {
            return DiaryEvent.CreateNewEvent(Title, NewEventDate, NewEventTime, NewEventDuration);
        }

        public JsonResult GetDiarySummary(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiaryEvents(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAllAppointmentsInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                color = e.StatusColor,
                                className = e.ClassName,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEvents()
        {
            using (SchoolDbContext dc = new SchoolDbContext())
            {
                var events = dc.AppointmentDiary.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult StudentPortalGuide()
        {
            return View();
        }
    }
}