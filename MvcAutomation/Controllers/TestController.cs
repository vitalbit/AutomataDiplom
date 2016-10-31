using BLL.Interface.Entities;
using BLL.Interface.Services;
using MvcAutomation.DllModulesResolver;
using MvcAutomation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestEndpoints;

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
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int typeId, string testName)
        {
            TestEntity test = new TestEntity()
            {
                Name = testName,
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
                    TestName = test.Name,
                    DllFilePath = testType.ModuleName + '/' + testType.DllFileName,
                    ResolveDllType = testType.ResolveDllType,
                    TestFileNumber = this.GetRandomFileNumber(test.Id)
                });
        }

        private class ResolveInformation
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public string DllFilePath { get; set; }
            public string ResolveDllType { get; set; }
            public int TestFileNumber { get; set; }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CompareResultsSimple(string result)
        {
            ResolveInformation inf = new JavaScriptSerializer().Deserialize<ResolveInformation>(result);
            inf.DllFilePath = Server.MapPath("~/Scripts/TestsFolder/" + inf.DllFilePath);
            ITestEndpoints testModule = ModuleResolver.GetAppDll(inf.DllFilePath, inf.ResolveDllType);
            string gradeResult = testModule.Grade(result);
            TestEntity test = testService.GetTestById(inf.Id);
            AnswerEntity answerEnt = new AnswerEntity()
            {
                Content = System.Text.Encoding.Default.GetBytes(result),
                Mark = Double.Parse(gradeResult),
                TestId = test.Id,
                UserId = userService.GetUserByEmail(User.Identity.Name).Id,
                TestEndTime = DateTime.Now
            };
            testService.CreateAnswer(answerEnt);

            return Json(new { Mark = gradeResult });
        }

        [HttpPost]
        [Authorize]
        public ActionResult CompareResults(string result)
        {
            ResolveInformation inf = new JavaScriptSerializer().Deserialize<ResolveInformation>(result);
            inf.DllFilePath = Server.MapPath("~/Scripts/TestsFolder/" + inf.DllFilePath);
            ITransform transform = ModuleResolver.GetTransformDll(inf.DllFilePath, inf.ResolveDllType);
            string studentFile = transform.TransformFileFromClient(result);
            string studentDirPath = Server.MapPath("~/ProjectsExpressions");
            DirectoryInfo projectExpr = new DirectoryInfo(studentDirPath);
            DirectoryInfo studentName = new DirectoryInfo(studentDirPath + "/" + User.Identity.Name);
            DirectoryInfo testDir = new DirectoryInfo(studentDirPath + "/" + User.Identity.Name + "/" + inf.Description);

            if (!projectExpr.GetDirectories().Contains(studentName))
                studentName.Create();
            if (studentName.GetDirectories().Contains(testDir))
                testDir.Delete();
            testDir.Create();

            FileInfo studentFileInfo = new FileInfo(testDir.FullName + "/" + "Input.rgl");

            using (StreamWriter writer = new StreamWriter(studentFileInfo.Create()))
            {
                writer.Write(studentFile);
            }

            ITestEndpoints testModule = ModuleResolver.GetAppDll(inf.DllFilePath, inf.ResolveDllType);
            string gradeResult = testModule.Grade(testDir.FullName);
            TestEntity test = testService.GetTestById(inf.Id);

            if (gradeResult == "0")
            {
                string automataFile = transform.TransformFileFromClient2(result);
                StringBuilder automataFileSb = new StringBuilder(automataFile);
                automataFileSb.AppendLine("Private Partition");
                automataFileSb.AppendLine();
                FileInfo partitionFile = new FileInfo(testDir + "/" + inf.Description + "/" + inf.Description + "_Partition.txt");
                string partition = "";
                using (StreamReader reader = partitionFile.OpenText())
                {
                    partition = reader.ReadToEnd();
                }
                partition = partition.Replace("\n", "");
                string[] lines = partition.Split(Environment.NewLine.ToCharArray()).Skip(3).ToArray();
                int k = 0;
                while (k != lines.Length && lines[k] != "*****")
                {
                    automataFileSb.AppendLine(lines[k++]);
                }
                automataFileSb.AppendLine("*****");

                DirectoryInfo compareDir = testDir.CreateSubdirectory("Automata");
                FileInfo studentAnswer = new FileInfo(compareDir.FullName + "/student_file.txt");
                using (StreamWriter studentWriter = new StreamWriter(studentAnswer.Create()))
                {
                    studentWriter.Write(automataFileSb.ToString());
                }

                FileInfo rightAnswer = new FileInfo(compareDir.FullName + "/answer_file.txt");
                FileInfo[] files = this.GetTestFiles(inf.Id);
                using (FileStream fs = files[inf.TestFileNumber].OpenRead())
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        using (FileStream rightAnswerFs = rightAnswer.Create())
                        {
                            using (StreamWriter answerWriter = new StreamWriter(rightAnswerFs))
                            {
                                answerWriter.Write(sr.ReadToEnd());
                            }
                        }
                    }
                }

                gradeResult = testModule.Grade(studentAnswer.FullName, rightAnswer.FullName);
                if (gradeResult == "True")
                    gradeResult = "0";
            }

            if (gradeResult == "0")
            {
                gradeResult = "10";
            }
            else
            {
                gradeResult = "0";
            }
            AnswerEntity answerEnt = new AnswerEntity()
            {
                Content = System.Text.Encoding.Default.GetBytes(result),
                Mark = Double.Parse(gradeResult),
                TestId = test.Id,
                UserId = userService.GetUserByEmail(User.Identity.Name).Id,
                TestEndTime = DateTime.Now
            };
            testService.CreateAnswer(answerEnt);
            
            return Json(new { Mark = gradeResult });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAnswers(int start)
        {
            IEnumerable<AnswerEntity> answers = testService.GetAllAnswers(start);
            return Json(new { results = this.ToModelResult(answers) });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                    Course = user.UniversityInfo != null ? user.UniversityInfo.Course : "",
                    Faculty = user.UniversityInfo != null ? user.UniversityInfo.Faculty : "",
                    FirstName = user.FirstName,
                    Group = user.UniversityInfo != null ? user.UniversityInfo.Group : "",
                    LastName = user.LastName,
                    Mark = answer.Mark,
                    Speciality = user.UniversityInfo != null ? user.UniversityInfo.Speciality : "",
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? testId)
        {
            return View(testId);
        }

        private int GetRandomFileNumber(int testId)
        {
            FileInfo[] files = this.GetTestFiles(testId);
            return new Random().Next(files.Length);
        }

        private FileInfo[] GetTestFiles(int testId)
        {
            TestEntity test = testService.GetTestById(testId);
            TestTypeEntity testType = testService.GetTypeById(test.TestTypeId);
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Scripts/TestsFolder/" + testType.ModuleName + "/Input/"));
            return di.GetFiles();
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}