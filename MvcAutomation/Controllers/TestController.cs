using BLL.Interface.Entities;
using BLL.Interface.Services;
using MvcAutomation.Models;
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

        [HttpGet]
        [Authorize]
        public ActionResult Index(int testId, string testName)
        {
            TestModel model = new TestModel()
            {
                TestId = testId,
                TestName = testName
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetTests(int start)
        {
            IEnumerable<TestEntity> tests = testService.GetAllTests(start);
            List<TestModel> result = new List<TestModel>();
            foreach(TestEntity test in tests)
            {
                result.Add(new TestModel()
                    {
                        TestId = test.Id,
                        TestName = test.Name
                    });
            }
            return Json(new { tests = result });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchTests(string search)
        {
            IEnumerable<TestEntity> tests = testService.GetAllTests(search);
            List<TestModel> result = new List<TestModel>();
            foreach (TestEntity test in tests)
            {
                result.Add(new TestModel()
                {
                    TestId = test.Id,
                    TestName = test.Name
                });
            }
            return Json(new { tests = result });
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

        [HttpPost]
        [Authorize]
        public ActionResult StartTest(TestModel model)
        {
            TestEntity test = testService.GetTestById(model.TestId);
            TestTypeEntity testType = testService.GetTypeById(test.TestTypeId);
            return View(new StartTestModel()
                {
                    TestId = test.Id,
                    TypeName = testType.ModuleName,
                    TestName = test.Name
                });
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            base.Dispose(disposing);
        }
	}
}