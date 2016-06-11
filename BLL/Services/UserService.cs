using BLL.Interface.Entities;
using BLL.Interface.Services;
using BLL.Mappers;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalUser> userRepository;
        private readonly IRepository<DalRole> roleRepository;
        private readonly IRepository<DalUniversityInfo> universityInfoRepository;

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.userRepository = uow.UserRepository;
            this.roleRepository = uow.RoleRepository;
            this.universityInfoRepository = uow.UniversityInfoRepository;
        }

        public UserEntity GetUserByEmail(string email)
        {
            DalUser user = userRepository.GetByPredicate(ent => ent.Email == email).FirstOrDefault();
            if (user == null)
                return null;
            else
                return user.ToBllUser();
        }

        public UserEntity GetUserById(int id)
        {
            return userRepository.GetById(id).ToBllUser();
        }

        public IEnumerable<UserEntity> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(answ => answ.ToBllUser());
        }

        public IEnumerable<UserEntity> GetAllUserEntities(int start)
        {
            return userRepository.GetBetween(start, 10).Select(ent => ent.ToBllUser());
        }

        public IEnumerable<UserEntity> GetAllUserEntities(string search)
        {
            return userRepository.GetByPredicate(ent => ent.Email.ToLower().Contains(search.ToLower()) 
                || (!String.IsNullOrEmpty(ent.LastName) && !String.IsNullOrEmpty(ent.FirstName) && (ent.LastName.ToLower() + ' ' + ent.FirstName.ToLower()).Contains(search.ToLower()))
                || (ent.UniversityInfo != null && ent.UniversityInfo.Faculty.ToLower().Contains(search.ToLower()))
                || (ent.UniversityInfo != null && ent.UniversityInfo.Speciality.ToLower().Contains(search.ToLower())))
                .Select(ent => ent.ToBllUser());
        }

        public void CreateUser(UserEntity user)
        {
            userRepository.Create(user.ToDalUser());
            uow.Commit();
        }

        public void UpdateUser(UserEntity user)
        {
            userRepository.Update(user.ToDalUser());
            uow.Commit();
        }

        public IEnumerable<RoleEntity> GetAllRoleEntities()
        {
            return roleRepository.GetAll().Select(answ => answ.ToBllRole());
        }

        public RoleEntity GetRoleByName(string role)
        {
            return roleRepository.GetAll().FirstOrDefault(ent => ent.Name == role).ToBllRole();
        }

        public void CreateRole(RoleEntity role)
        {
            roleRepository.Create(role.ToDalRole());
            uow.Commit();
        }

        public IEnumerable<UniversityInfoEntity> GetAllUniversityInfoEntities()
        {
            return universityInfoRepository.GetAll().Select(info => info.ToBllUniversityInfo());
        }

        public void CreateUniversityInfo(UniversityInfoEntity universityInfo)
        {
            universityInfoRepository.Create(universityInfo.ToDalUniversityInfo());
            uow.Commit();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
