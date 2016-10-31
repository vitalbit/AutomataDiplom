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
        public ActionResult CurrentFile(int testId, string dllFile, string resolveType, int testFileNumber)
        {
            FileInfo[] files = this.GetTestFiles(testId);
            string description = "";
            using (FileStream fs = files[testFileNumber].OpenRead())
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    ITransform transform = ModuleResolver.GetTransformDll(Server.MapPath("~/Scripts/TestsFolder/" + dllFile), resolveType);
                    description = transform.TransformFileToClient(sr.ReadToEnd());
                }
            }
            DescriptionReg descr = new DescriptionReg() { Description = description };
            return Json(descr);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CurrentFileWithoutTransform(int testId, int testFileNumber)
        {
            DescriptionReg descr = null;
            FileInfo[] files = this.GetTestFiles(testId);
            using (FileStream fs = files[testFileNumber].OpenRead())
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    descr = new JavaScriptSerializer().Deserialize<DescriptionReg>(sr.ReadToEnd());
                }
            }
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
            base.Dispose(disposing);
        }
	}
}