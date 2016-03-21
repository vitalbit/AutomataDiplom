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
    public class MaterialController : Controller
    {
        private readonly IMaterialService materialService;

        public MaterialController(IMaterialService materialService)
        {
            this.materialService = materialService;
        }

        //
        // GET: /Material/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public JsonResult Add()
        {
            for (int i = 0; i != Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                byte[] fileByte = new byte[file.ContentLength];
                file.InputStream.Read(fileByte, 0, file.ContentLength);
                MaterialEntity material = new MaterialEntity()
                {
                    Content = fileByte,
                    FileName = Path.GetFileName(file.FileName),
                    Description = ""
                };
                materialService.CreateMaterial(material);
            }
            return Json("Material has been added");
        }

        [HttpGet]
        [Authorize]
        public FileResult GetMaterial(int materialId)
        {
            MaterialEntity material = materialService.GetMaterialById(materialId);
            return File(material.Content, "application/text", material.FileName);
        }

        private class MaterialDisplay
        {
            public int Id { get; set; }
            public string FileName { get; set; }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetMaterials(int start)
        {
            List<MaterialDisplay> materials = new List<MaterialDisplay>();
            foreach (MaterialEntity material in materialService.GetAllMaterial(start))
            {
                materials.Add(new MaterialDisplay()
                {
                    FileName = material.FileName,
                    Id = material.Id
                });
            }
            return Json(new { materials = materials });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchMaterial(string search)
        {
            List<MaterialDisplay> materials = new List<MaterialDisplay>();
            foreach (MaterialEntity material in materialService.GetAllMaterial(search))
            {
                materials.Add(new MaterialDisplay()
                {
                    FileName = material.FileName,
                    Id = material.Id
                });
            }
            return Json(new { materials = materials });
        }

        protected override void Dispose(bool disposing)
        {
            materialService.Dispose();
            base.Dispose(disposing);
        }
	}
}