using BLL.Interface.Entities;
using BLL.Interface.Services;
using InputPostfixRegex;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcAutomation.Controllers
{
    public class TestPassingController : Controller
    {
        private readonly ITestService testService;

        public TestPassingController(ITestService testService)
        {
            this.testService = testService;
        }

        //
        // GET: /TestPassing/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CurrentFile(int testId)
        {
            TestEntity test = testService.GetTestById(testId);
            List<TestFileEntity> testFiles = new List<TestFileEntity>(test.TestFiles);
            StreamReader sr = new StreamReader(new MemoryStream(testFiles[0].Content));

            return Json(sr.ReadToEnd());
        }

        [HttpGet]
        [Authorize]
        public void ImageForPolish(string polish)
        {
            ExprNameArray expression;
            expression.name = "";
            expression.shortName = "";
            expression.arrPolish = polish.Split(' ');

            Bitmap bitmap = TreeChart.DrawBitmap(expression);
            
            Response.ContentType = "image/bmp";
            bitmap.Save(Response.OutputStream, ImageFormat.Bmp);
        }

        protected override void Dispose(bool disposing)
        {
            testService.Dispose();
            base.Dispose(disposing);
        }
	}
}