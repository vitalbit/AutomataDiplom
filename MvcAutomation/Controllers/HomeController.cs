using BLL.Interface.Entities;
using BLL.Interface.Services;
using MvcAutomation.Models;
using MvcAutomation.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcAutomation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("HomePage");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckEmail(string email)
        {
            UserEntity user = userService.GetUserByEmail(email);
            
            if (user != null && !String.IsNullOrEmpty(user.Password))
                return Json(new { isRegistered = "true", isInDb = "true" });
            else if (user != null && String.IsNullOrEmpty(user.Password))
                return Json(new { isRegistered = "false", isInDb = "true" });
            else
                return Json(new { isRegistered = "false", isInDb = "false" });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Membership.ValidateUser(loginModel.Email, loginModel.Password))
                    {
                        FormsAuthentication.SetAuthCookie(loginModel.Email, true);
                        return View("HomePage");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Ошибка при валидации!");
                }
            }
            else
                ModelState.AddModelError("", "Неверный пароль!");
            return View("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(LoginModel loginModel)
        {
            if (loginModel.Password == loginModel.RepeatPassword && ModelState.IsValid)
            {
                MembershipUser memberUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(loginModel.Email, loginModel.Password, userService.GetAllRoleEntities().FirstOrDefault(ent => ent.Name == "Student").Id);
                if (memberUser != null)
                {
                    FormsAuthentication.SetAuthCookie(memberUser.Email, true);
                    return View("HomePage");
                }
                else
                    ModelState.AddModelError("", "Ошибка при создании пользователя!");
            }
            else
                ModelState.AddModelError("", "Пароли не совпадают!");
            return View("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult HomePageAdmin()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (model.NewPassword == model.RepeatPassword)
            {
                UserEntity user = userService.GetUserByEmail(User.Identity.Name);
                if (((CustomMembershipProvider)Membership.Provider).ChangePassword(user, model.OldPassword, model.NewPassword))
                    return RedirectToAction("HomePage");
                else
                    ModelState.AddModelError("", "Старый пароль введен не верно!");
            }
            else
            {
                ModelState.AddModelError("", "Пароли не совпадают!");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}