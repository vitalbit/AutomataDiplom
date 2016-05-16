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
        public JsonResult GetType(int id)
        {
            TestTypeEntity test = testService.GetTypeById(id);
            return Json(new { testType = test }, JsonRequestBehavior.AllowGet);
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
        public ActionResult CreateType(string testType, string jsFile, string cssFile)
        {
            var tempPath = Server.MapPath("~/Temp/");
            var path = Server.MapPath("~/Scripts/TestsFolder/");

            DirectoryInfo di = new DirectoryInfo(path);
            di = di.CreateSubdirectory(testType);
            FileInfo jsfi = new FileInfo(tempPath + jsFile);
            jsfi.CopyTo(di.FullName + "\\" + jsFile);
            FileInfo cssfi = new FileInfo(tempPath + cssFile);
            cssfi.CopyTo(di.FullName + "\\" + cssFile);

            TestTypeEntity test = new TestTypeEntity()
            {
                ModuleName = testType,
                CssFileName = cssFile,
                JsFileName = jsFile
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult AddJsFile()
        {
            string path = Server.MapPath("~/Temp/");
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (var jsfile in di.GetFiles("*.js"))
            {
                System.IO.File.Delete(jsfile.FullName);
            }

            HttpPostedFileBase file = Request.Files[0];
            file.SaveAs(path + file.FileName);
            return Json("Файл загружен");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult AddCssFile()
        {
            string path = Server.MapPath("~/Temp/");
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (var cssfile in di.GetFiles("*.css"))
            {
                System.IO.File.Delete(cssfile.FullName);
            }

            HttpPostedFileBase file = Request.Files[0];
            file.SaveAs(path + file.FileName);
            return Json("Файл загружен");
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            base.Dispose(disposing);
        }
	}
}