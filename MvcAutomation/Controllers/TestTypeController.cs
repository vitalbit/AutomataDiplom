﻿using BLL.Interface.Entities;
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
        public ActionResult GetFiles(string testType)
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Scripts/TestsFolder/" + testType + "/Input/"));
            List<string> fileNames = new List<string>();
            foreach (FileInfo fi in di.GetFiles())
            {
                fileNames.Add(fi.Name);
            }
            return Json(new { testFiles = fileNames }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult CreateType(string testType, string jsFile, string cssFile, string dllFile, string resolveType)
        {
            var tempPath = Server.MapPath("~/Temp/");
            var path = Server.MapPath("~/Scripts/TestsFolder/");

            DirectoryInfo di = new DirectoryInfo(path);
            di = di.CreateSubdirectory(testType);
            FileInfo jsfi = new FileInfo(tempPath + jsFile);
            jsfi.CopyTo(di.FullName + "\\" + jsFile, true);
            FileInfo cssfi = new FileInfo(tempPath + cssFile);
            cssfi.CopyTo(di.FullName + "\\" + cssFile, true);
            FileInfo dllfi = new FileInfo(tempPath + dllFile);
            dllfi.CopyTo(di.FullName + "\\" + dllFile, true);
            di.CreateSubdirectory("Input");

            TestTypeEntity test = new TestTypeEntity()
            {
                ModuleName = testType,
                CssFileName = cssFile,
                JsFileName = jsFile,
                DllFileName = dllFile,
                ResolveDllType = resolveType
            };
            testService.CreateTestType(test);
            return Json(new { message = "Тип теста добавлен" });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddFiles(string testType)
        {
            if (Request.Files.Count > 0)
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    file.SaveAs(Server.MapPath("~/Scripts/TestsFolder/" + testType + "/Input/" + file.FileName));
                }
            }
            return Json(new { message = "Test files has been added" });
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

        [HttpPost]
        public JsonResult AddDllFile()
        {
            string path = Server.MapPath("~/Temp/");
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (var dllfile in di.GetFiles("*.dll"))
            {
                System.IO.File.Delete(dllfile.FullName);
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