using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface IUserService : IDisposable
    {
        UserEntity GetUserByEmail(string email);
        UserEntity GetUserById(int id);
        IEnumerable<UserEntity> GetAllUserEntities();
        IEnumerable<UserEntity> GetAllUserEntities(int start);
        IEnumerable<UserEntity> GetAllUserEntities(string search);
        void CreateUser(UserEntity user);
        void UpdateUser(UserEntity user);
        IEnumerable<RoleEntity> GetAllRoleEntities();
        RoleEntity GetRoleByName(string role);
        void CreateRole(RoleEntity role);
        IEnumerable<UniversityInfoEntity> GetAllUniversityInfoEntities();
        void CreateUniversityInfo(UniversityInfoEntity universityInfo);
    }
}
