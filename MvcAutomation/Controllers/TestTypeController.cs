using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAutomation.Controllers
{
    public class TestTypeController : Controller
    {
        private readonly ITestService testService;

        public TestTypeController(ITestService testService)
        {
            this.testService = testService;
        }

        //
        // GET: /TestType/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetTypes()
        {
            List<TestTypeEntity> testTypes = testService.GetAllTestTypes().ToList();
            return Json(new { testTypes = testTypes }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetFiles()
        {
            List<TestFileEntity> testFiles = testService.GetAllTestFiles().ToList();
            return Json(new { testFiles = testFiles }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult CreateType(string testType)
        {
            TestTypeEntity test = new TestTypeEntity()
            {
                ModuleName = testType
            };
            testService.CreateTestType(test);
            return Json(new { message = "Тип теста добавлен" });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddFile()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                byte[] fileByte = new byte[file.ContentLength];
                file.InputStream.Read(fileByte, 0, file.ContentLength);
                TestFileEntity test = new TestFileEntity()
                {
                    Content = fileByte,
                    FileName = Path.GetFileName(file.FileName)
                };
                testService.CreateTestFile(test);
            }
            return Json(new { message = "Test file has been added" });
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            base.Dispose(disposing);
        }
	}
}