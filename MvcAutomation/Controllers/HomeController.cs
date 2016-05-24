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
        #region Authorization
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
                        return RedirectToAction("HomePage");
                    }
                    else
                        ModelState.AddModelError("", "Неверный пароль!");
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

        [HttpGet]
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

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

        #region ChangePassword
        // http://localhost:8881/Home/ChangePassword
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
        #endregion

        private class UserRole
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Speciaity { get; set; }
            public string Group { get; set; }
            public string Course { get; set; }
            public string Role { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult TenUsersRoles(string start)
        {
            IEnumerable<UserEntity> users = userService.GetAllUserEntities();
            List<RoleEntity> roles = userService.GetAllRoleEntities().ToList();
            List<UserRole> userRole = new List<UserRole>();
            foreach(UserEntity user in users)
            {
                if (user.Role != null)
                {
                    userRole.Add(new UserRole()
                    {
                        Email = user.Email,
                        Course = user.Course != null ? user.Course.Name : null,
                        FirstName = user.FirstName,
                        Group = user.Group != null ? user.Group.Name : null,
                        LastName = user.LastName,
                        Role = user.Role.Name,
                        Speciaity = user.Speciality != null ? user.Speciality.Name : null
                    });
                }
            }
            return Json(new { users = userRole, roles = roles });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult ChangeRole(string email, string newRole)
        {
            UserEntity user = userService.GetUserByEmail(email);
            RoleEntity role = userService.GetRoleByName(newRole);
            user.RoleId = role.Id;
            userService.UpdateUser(user);
            return Json(new { message = "Update Success!" });
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult SearchUser(string search)
        {
            IEnumerable<UserEntity> users = userService.GetAllUserEntities(search);
            IEnumerable<RoleEntity> roles = userService.GetAllRoleEntities();
            List<UserRole> userRole = new List<UserRole>();
            foreach (UserEntity user in users)
            {
                if (user.Role != null)
                {
                    userRole.Add(new UserRole()
                    {
                        Email = user.Email,
                        Course = user.Course != null ? user.Course.Name : null,
                        FirstName = user.FirstName,
                        Group = user.Group != null ? user.Group.Name : null,
                        LastName = user.LastName,
                        Role = user.Role.Name,
                        Speciaity = user.Speciality != null ? user.Speciality.Name : null
                    });
                }
            }
            return Json(new { users = userRole, roles = roles });
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}