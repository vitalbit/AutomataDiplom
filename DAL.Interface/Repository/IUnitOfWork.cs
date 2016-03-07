using DAL.Interface.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<DalAnswer> AnswerRepository { get; }
        IRepository<DalCourse> CourseRepository { get; }
        IRepository<DalFaculty> FacultyRepository { get; }
        IRepository<DalGroup> GroupRepository { get; }
        IRepository<DalRole> RoleRepository { get; }
        IRepository<DalSpeciality> SpecialityRepository { get; }
        IRepository<DalTest> TestRepository { get; }
        IRepository<DalTestFile> TestFileRepository { get; }
        IRepository<DalTestType> TestTypeRepository { get; }
        IRepository<DalUser> UserRepository { get; }
        DbContext Context { get; }
        void Commit();
    }
}
