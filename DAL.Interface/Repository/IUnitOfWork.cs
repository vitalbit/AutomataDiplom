using DAL.Interface.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<DalAnswer> AnswerRepository { get; }
        IRepository<DalRole> RoleRepository { get; }
        IRepository<DalTest> TestRepository { get; }
        IRepository<DalTestFile> TestFileRepository { get; }
        IRepository<DalTestType> TestTypeRepository { get; }
        IRepository<DalUser> UserRepository { get; }
        IRepository<DalMaterial> MaterialRepository { get; }
        IRepository<DalUniversityInfo> UniversityInfoRepository { get; }
        DbContext Context { get; }
        void Commit();
    }
}
