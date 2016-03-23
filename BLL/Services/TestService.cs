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
    public class TestService : ITestService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalTest> testRepository;
        private readonly IRepository<DalTestFile> testFileRepository;
        private readonly IRepository<DalTestType> testTypeRepository;

        public TestService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.testRepository = uow.TestRepository;
            this.testFileRepository = uow.TestFileRepository;
            this.testTypeRepository = uow.TestTypeRepository;
        }

        public IEnumerable<TestTypeEntity> GetAllTestTypes()
        {
            return testTypeRepository.GetAll().Select(ent => ent.ToBllTestType());
        }

        public IEnumerable<TestEntity> GetAllTests()
        {
            return testRepository.GetAll().Select(ent => ent.ToBllTest());
        }

        public IEnumerable<TestEntity> GetAllTests(int start)
        {
            return testRepository.GetBetween(start, 10).Select(ent => ent.ToBllTest());
        }

        public IEnumerable<TestEntity> GetAllTests(string search)
        {
            return testRepository.GetByPredicate(ent => ent.Name.Contains(search)).Select(ent => ent.ToBllTest());
        }

        public IEnumerable<TestFileEntity> GetAllTestFiles()
        {
            return testFileRepository.GetAll().Select(ent => ent.ToBllTestFile());
        }

        public TestEntity GetTestById(int id)
        {
            return testRepository.GetById(id).ToBllTest();
        }

        public TestFileEntity GetFileById(int id)
        {
            return testFileRepository.GetById(id).ToBllTestFile();
        }

        public TestTypeEntity GetTypeById(int id)
        {
            return testTypeRepository.GetById(id).ToBllTestType();
        }

        public void CreateTestType(TestTypeEntity testType)
        {
            testTypeRepository.Create(testType.ToDalTestType());
            uow.Commit();
        }

        public void CreateTest(TestEntity test)
        {
            testRepository.Create(test.ToDalTest());
            uow.Commit();
        }

        public void CreateTestFile(TestFileEntity testFile)
        {
            testFileRepository.Create(testFile.ToDalTestFile());
            uow.Commit();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
