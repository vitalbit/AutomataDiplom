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
        private readonly IRepository<DalCourse> courseRepository;
        private readonly IRepository<DalFaculty> facultyRepository;
        private readonly IRepository<DalGroup> groupRepository;
        private readonly IRepository<DalRole> roleRepository;
        private readonly IRepository<DalSpeciality> specialityRepository;

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.userRepository = uow.UserRepository;
            this.courseRepository = uow.CourseRepository;
            this.facultyRepository = uow.FacultyRepository;
            this.groupRepository = uow.GroupRepository;
            this.roleRepository = uow.RoleRepository;
            this.specialityRepository = uow.SpecialityRepository;
        }

        public IEnumerable<UserEntity> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(answ => answ.ToBllUser());
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

        public IEnumerable<CourseEntity> GetAllCourseEntities()
        {
            return courseRepository.GetAll().Select(answ => answ.ToBllCourse());
        }

        public void CreateCourse(CourseEntity course)
        {
            courseRepository.Create(course.ToDalCourse());
            uow.Commit();
        }

        public IEnumerable<FacultyEntity> GetAllFacultyEntities()
        {
            return facultyRepository.GetAll().Select(answ => answ.ToBllFaculty());
        }

        public void CreateFaculty(FacultyEntity faculty)
        {
            facultyRepository.Create(faculty.ToDalFaculty());
            uow.Commit();
        }

        public IEnumerable<GroupEntity> GetAllGroupEntities()
        {
            return groupRepository.GetAll().Select(answ => answ.ToBllGroup());
        }

        public void CreateGroup(GroupEntity group)
        {
            groupRepository.Create(group.ToDalGroup());
            uow.Commit();
        }

        public IEnumerable<RoleEntity> GetAllRoleEntities()
        {
            return roleRepository.GetAll().Select(answ => answ.ToBllRole());
        }

        public void CreateRole(RoleEntity role)
        {
            roleRepository.Create(role.ToDalRole());
            uow.Commit();
        }

        public IEnumerable<SpecialityEntity> GetAllSpecialityEntities()
        {
            return specialityRepository.GetAll().Select(answ => answ.ToBllSpeciality());
        }

        public void CreateSpeciality(SpecialityEntity speciality)
        {
            specialityRepository.Create(speciality.ToDalSpeciality());
            uow.Commit();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
