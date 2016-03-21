using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAutomation.Controllers
{
    public class StructureOrganizeController : Controller
    {
        private readonly IUserService userService;

        public StructureOrganizeController(IUserService userService)
        {
            this.userService = userService;
        }

        //
        // GET: /StructureOrganize/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult GetData()
        {
            IEnumerable<FacultyEntity> faculties = userService.GetAllFacultyEntities();
            IEnumerable<SpecialityEntity> specialities = userService.GetAllSpecialityEntities();
            IEnumerable<CourseEntity> courses = userService.GetAllCourseEntities();
            IEnumerable<GroupEntity> groups = userService.GetAllGroupEntities();
            return Json(new { faculties = faculties, specialities = specialities, courses = courses, groups = groups }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddFaculty(string faculty)
        {
            FacultyEntity fac = new FacultyEntity() { Name = faculty };
            userService.CreateFaculty(fac);
            return Json(new { faculties = userService.GetAllFacultyEntities() });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddSpeciality(string speciality)
        {
            SpecialityEntity spec = new SpecialityEntity() { Name = speciality };
            userService.CreateSpeciality(spec);
            return Json(new { specialities = userService.GetAllSpecialityEntities() });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddCourse(string course)
        {
            CourseEntity courseEnt = new CourseEntity() { Name = course };
            userService.CreateCourse(courseEnt);
            return Json(new { courses = userService.GetAllCourseEntities() });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddGroup(string group)
        {
            GroupEntity groupEnt = new GroupEntity() { Name = group };
            userService.CreateGroup(groupEnt);
            return Json(new { groups = userService.GetAllGroupEntities() });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddStudent(int? facultyId, int? specialityId, int? courseId, int? groupId, string email, string firstname, string lastname)
        {
            string resultMessage = "Student has been added";
            if (!facultyId.HasValue)
                resultMessage = "Please select faculty";
            else if (!specialityId.HasValue)
                resultMessage = "Please select speciality";
            else if (!courseId.HasValue)
                resultMessage = "Please select course";
            else if (!groupId.HasValue)
                resultMessage = "Please select group";
            else if (String.IsNullOrEmpty(email))
                resultMessage = "Please enter email";
            else if (String.IsNullOrEmpty(firstname))
                resultMessage = "Please enter firstname";
            else if (String.IsNullOrEmpty(lastname))
                resultMessage = "Please enter lastname";
            else
            {
                UserEntity user = new UserEntity()
                {
                    CourseId = courseId.Value,
                    Email = email,
                    FacultyId = facultyId.Value,
                    FirstName = firstname,
                    GroupId = groupId.Value,
                    LastName = lastname,
                    SpecialityId = specialityId.Value
                };
                userService.CreateUser(user);
            }
            return Json(new { message = resultMessage });
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
	}
}