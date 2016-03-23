using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface ITestService : IDisposable
    {
        IEnumerable<TestTypeEntity> GetAllTestTypes();
        IEnumerable<TestEntity> GetAllTests();
        IEnumerable<TestEntity> GetAllTests(int start);
        IEnumerable<TestEntity> GetAllTests(string search);
        IEnumerable<TestFileEntity> GetAllTestFiles();
        TestEntity GetTestById(int id);
        TestFileEntity GetFileById(int id);
        TestTypeEntity GetTypeById(int id);
        void CreateTestType(TestTypeEntity testType);
        void CreateTest(TestEntity test);
        void CreateTestFile(TestFileEntity testFile);
    }
}
