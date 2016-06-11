using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAutomation.Controllers
{
    public class DeveloperController : Controller
    {
        [HttpGet]
        public ActionResult AutomataDevelop()
        {
            return View();
        }

        [HttpPost]
        public FileResult AutomataDevelop(string file)
        {
            DirectoryInfo developDir = new DirectoryInfo(Server.MapPath("~/Developing"));
            DirectoryInfo userDevDir = developDir.CreateSubdirectory(User.Identity.Name);
            FileInfo input = new FileInfo(userDevDir + "/Input.rgl");
            using (StreamWriter sr = input.CreateText())
            {
                sr.Write(file);
            }
            ConsoleFrontEnd.Program prog = new ConsoleFrontEnd.Program();
            string[] args = new string[] {"/c", "Input.rgl", "/d", userDevDir.FullName };
            if (prog.Main(args) == "0")
                return File(userDevDir.FullName + "/Cartesian_Automaton.txt", "application/text", "Cartesian_Automaton.txt");
            else
                return File(userDevDir.FullName + "/Input.rgl", "application/text", "Input.rgl");
        }

        [HttpPost]
        public JsonResult GetFile()
        {
            HttpPostedFileBase file = Request.Files[0];
            string fileContent = "";
            using (StreamReader sr = new StreamReader(file.InputStream))
            {
                fileContent = sr.ReadToEnd();
            }
            return Json(new { content = fileContent });
        }
	}
}