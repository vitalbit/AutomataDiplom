using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcAutomation.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        IUserService userService = (IUserService)(new NinjectDependencyResolver().GetService(typeof(IUserService)));

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            RoleEntity newRole = new RoleEntity() { Name = roleName };
            userService.CreateRole(newRole);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] role = new string[] { };
            UserEntity user = userService.GetAllUserEntities().FirstOrDefault(ent => ent.Email == username);
            if (user != null)
            {
                RoleEntity userRole = userService.GetAllRoleEntities().FirstOrDefault(ent => ent.Id == user.RoleId);
                if (userRole != null)
                    role = new string[] { userRole.Name };
            }
            return role;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            UserEntity user = userService.GetAllUserEntities().FirstOrDefault(ent => ent.Email == username);
            if (user != null)
            {
                RoleEntity role = userService.GetAllRoleEntities().FirstOrDefault(ent => ent.Id == user.RoleId);
                if (role != null && role.Name == roleName)
                    return true;
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}