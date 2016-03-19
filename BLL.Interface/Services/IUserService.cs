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
        IEnumerable<UserEntity> GetAllUserEntities();
        void CreateUser(UserEntity user);
        void UpdateUser(UserEntity user);
        IEnumerable<CourseEntity> GetAllCourseEntities();
        void CreateCourse(CourseEntity course);
        IEnumerable<FacultyEntity> GetAllFacultyEntities();
        void CreateFaculty(FacultyEntity faculty);
        IEnumerable<GroupEntity> GetAllGroupEntities();
        void CreateGroup(GroupEntity group);
        IEnumerable<RoleEntity> GetAllRoleEntities();
        void CreateRole(RoleEntity role);
        IEnumerable<SpecialityEntity> GetAllSpecialityEntities();
        void CreateSpeciality(SpecialityEntity speciality);
    }
}
