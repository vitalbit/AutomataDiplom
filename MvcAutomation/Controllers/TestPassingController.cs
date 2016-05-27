using BLL.Interface.Entities;
using BLL.Interface.Services;
using MvcAutomation.DllModulesResolver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestEndpoints;

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

        private class DescriptionReg
        {
            public string Description { get; set; }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CurrentFile(int testId, string dllFile, string resolveType)
        {
            TestEntity test = testService.GetTestById(testId);
            List<TestFileEntity> testFiles = new List<TestFileEntity>(test.TestFiles);
            StreamReader sr = new StreamReader(new MemoryStream(testFiles[0].Content));

            ITransform transform = ModuleResolver.GetTransformDll(Server.MapPath("~/Scripts/TestsFolder/" + dllFile), resolveType);
            string description = transform.TransformFileToClient(sr.ReadToEnd());
            sr.Close();
            DescriptionReg descr = new DescriptionReg() { Description = description };
            return Json(descr);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CurrentFileWithoutTransform(int testId)
        {
            TestEntity test = testService.GetTestById(testId);
            List<TestFileEntity> testFiles = new List<TestFileEntity>(test.TestFiles);
            StreamReader sr = new StreamReader(new MemoryStream(testFiles[0].Content));
            DescriptionReg descr = new JavaScriptSerializer().Deserialize<DescriptionReg>(sr.ReadToEnd());
            return Json(descr);
        }

        [HttpGet]
        [Authorize]
        public void Image(string input, string resolveDll, string resolveType)
        {
            resolveDll = Server.MapPath("~/Scripts/TestsFolder/" + resolveDll);
            IImageTestEndpoints endpoint = ModuleResolver.GetImageDll(resolveDll, resolveType);
            Bitmap bitmap = endpoint.GetImage(input);

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