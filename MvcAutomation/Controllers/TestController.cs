using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAutomation.Controllers
{
    public class TestController : Controller
    {
        private readonly ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        //
        // GET: /Test/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult Create(int typeId, int fileId, string testName)
        {
            TestFileEntity testFile = testService.GetFileById(fileId);
            List<TestFileEntity> fileList = new List<TestFileEntity>();
            fileList.Add(testFile);
            TestEntity test = new TestEntity()
            {
                Name = testName,
                TestFiles = fileList,
                TestTypeId = typeId
            };
            testService.CreateTest(test);
            return Json(new { message = "Тест создан" });
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            base.Dispose(disposing);
        }
	}
}