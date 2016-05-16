using BLL.Interface.Entities;
using BLL.Interface.Services;
using GradeSystems;
using MvcAutomation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcAutomation.Controllers
{
    public class TestController : Controller
    {
        private readonly ITestService testService;
        private readonly IUserService userService;

        public TestController(ITestService testService, IUserService userService)
        {
            this.testService = testService;
            this.userService = userService;
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
                    JsFileName = testType.JsFileName,
                    CssFileName = testType.CssFileName,
                    TestName = test.Name
                });
        }

        [HttpPost]
        [Authorize]
        public ActionResult CompareResults(TestResultsModel result)
        {
            TestEntity test = testService.GetTestById(result.Id);
            List<TestFileEntity> files = test.TestFiles.ToList();
            StreamReader sr = new StreamReader(new MemoryStream(files[0].Content));
            TestResultsModel answer = new JavaScriptSerializer().Deserialize<TestResultsModel>(sr.ReadToEnd());
            TableGradeSystem tg = new TableGradeSystem();
            double mark = tg.Grade(result, answer);
            AnswerEntity answerEnt = new AnswerEntity()
            {
                Content = System.Text.Encoding.Default.GetBytes(new JavaScriptSerializer().Serialize(result)),
                Mark = mark,
                TestId = test.Id,
                UserId = userService.GetUserByEmail(User.Identity.Name).Id,
                TestEndTime = DateTime.Now
            };
            testService.CreateAnswer(answerEnt);
            return Json(new { Mark = mark });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult GetAnswers(int start)
        {
            IEnumerable<AnswerEntity> answers = testService.GetAllAnswers(start);
            return Json(new { results = this.ToModelResult(answers) });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult SearchAnswer(string search)
        {
            IEnumerable<AnswerEntity> answers = testService.GetAllAnswers(search);
            return Json(new { results = this.ToModelResult(answers) });
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAnswerFile(int answerId)
        {
            AnswerEntity answer = testService.GetAnswerById(answerId);
            return File(answer.Content, "application/text", answer.TestEndTime.ToString());
        }

        private List<AnswerResultsModel> ToModelResult(IEnumerable<AnswerEntity> answers)
        {
            List<AnswerResultsModel> answerResults = new List<AnswerResultsModel>();
            foreach (AnswerEntity answer in answers)
            {
                UserEntity user = userService.GetUserById(answer.UserId);
                TestEntity test = testService.GetTestById(answer.TestId);
                answerResults.Add(new AnswerResultsModel()
                {
                    Course = user.Course !=null ? user.Course.Name : "",
                    Faculty = user.Faculty != null ? user.Faculty.Name : "",
                    FirstName = user.FirstName,
                    Group = user.Group != null ? user.Group.Name : "",
                    LastName = user.LastName,
                    Mark = answer.Mark,
                    Speciality = user.Speciality != null ? user.Speciality.Name : "",
                    AnswerId = answer.Id,
                    TestName = test.Name
                });
            }
            return answerResults;
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetUserAnswers(int start)
        {
            IEnumerable<AnswerEntity> answers = testService.GetUserAnswers(User.Identity.Name, start);
            return Json(new { results = this.ToModelResult(answers) });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchUserAnswer(string search)
        {
            IEnumerable<AnswerEntity> answers = testService.GetUserAnswers(User.Identity.Name, search);
            return Json(new { results = this.ToModelResult(answers) });
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int testId)
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            userService.Dispose();
            base.Dispose(disposing);
        }
	}
}