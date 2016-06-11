using DAL.Interface.DTO;
using DAL.Interface.Repository;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        private GenericRepository<DalAnswer, Answer> answerRepository;
        private GenericRepository<DalRole, Role> roleRepository;
        private GenericRepository<DalUniversityInfo, UniversityInfo> universityInfoRepository;
        private GenericRepository<DalTest, Test> testRepository;
        private GenericRepository<DalTestFile, TestFile> testFileRepository;
        private GenericRepository<DalTestType, TestType> testTypeRepository;
        private GenericRepository<DalUser, User> userRepository;
        private GenericRepository<DalMaterial, Material> materialRepository;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context
        {
            get { return context; }
        }

        public void Commit()
        {
            if (context != null)
                context.SaveChanges();
        }

        public GenericRepository<DalAnswer, Answer> AnswerRepository
        {
            get
            {
                if (this.answerRepository == null)
                    this.answerRepository = new GenericRepository<DalAnswer, Answer>(context);
                return answerRepository;
            }
        }

        public GenericRepository<DalRole, Role> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                    this.roleRepository = new GenericRepository<DalRole, Role>(context);
                return roleRepository;
            }
        }

        public GenericRepository<DalTest, Test> TestRepository
        {
            get
            {
                if (this.testRepository == null)
                    this.testRepository = new GenericRepository<DalTest, Test>(context);
                return testRepository;
            }
        }

        public GenericRepository<DalTestFile, TestFile> TestFileRepository
        {
            get
            {
                if (this.testFileRepository == null)
                    this.testFileRepository = new GenericRepository<DalTestFile, TestFile>(context);
                return testFileRepository;
            }
        }

        public GenericRepository<DalTestType, TestType> TestTypeRepository
        {
            get
            {
                if (this.testTypeRepository == null)
                    this.testTypeRepository = new GenericRepository<DalTestType, TestType>(context);
                return testTypeRepository;
            }
        }

        public GenericRepository<DalUser, User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                    this.userRepository = new GenericRepository<DalUser, User>(context);
                return userRepository;
            }
        }

        public GenericRepository<DalMaterial, Material> MaterialRepository
        {
            get
            {
                if (this.materialRepository == null)
                    this.materialRepository = new GenericRepository<DalMaterial, Material>(context);
                return materialRepository;
            }
        }

        public GenericRepository<DalUniversityInfo, UniversityInfo> UniversityInfoRepository
        {
            get
            {
                if (this.universityInfoRepository == null)
                    this.universityInfoRepository = new GenericRepository<DalUniversityInfo, UniversityInfo>(context);
                return universityInfoRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (context != null)
            {
                context.Dispose();
            }
        }

        IRepository<DalAnswer> IUnitOfWork.AnswerRepository
        {
            get { return AnswerRepository; }
        }

        IRepository<DalRole> IUnitOfWork.RoleRepository
        {
            get { return RoleRepository; }
        }

        IRepository<DalTest> IUnitOfWork.TestRepository
        {
            get { return TestRepository; }
        }

        IRepository<DalTestFile> IUnitOfWork.TestFileRepository
        {
            get { return TestFileRepository; }
        }

        IRepository<DalTestType> IUnitOfWork.TestTypeRepository
        {
            get { return TestTypeRepository; }
        }

        IRepository<DalUser> IUnitOfWork.UserRepository
        {
            get { return UserRepository; }
        }

        IRepository<DalMaterial> IUnitOfWork.MaterialRepository
        {
            get { return MaterialRepository; }
        }

        IRepository<DalUniversityInfo> IUnitOfWork.UniversityInfoRepository
        {
            get { return UniversityInfoRepository; }
        }
    }
}
