using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAutomation.Controllers
{
    public class StructureOrganizeController : Controller
    {
        private readonly IUserService userService;

        public StructureOrganizeController(IUserService userService)
        {
            this.userService = userService;
        }

        public class UniversityInfoString
        {
            public int Id { get; set; }
            public string Info { get; set; }
            public string AdditionalInfo { get; set; }
        }

        [HttpGet]
        public ActionResult GetUniversityInfo()
        {
            IEnumerable<UniversityInfoString> infos = userService.GetAllUniversityInfoEntities().
                Select(ent => new UniversityInfoString()
                {
                    Id = ent.Id,
                    Info = ent.ToString(),
                    AdditionalInfo = ent.AdditionalInfo
                });
            return Json(new { universityInfo = infos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult AddUniversityInfo(UniversityInfoEntity info)
        {
            userService.CreateUniversityInfo(info);
            return Json(new { message = "done" });
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
	}
}