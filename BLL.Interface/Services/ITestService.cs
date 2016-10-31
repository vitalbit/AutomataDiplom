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
        IEnumerable<AnswerEntity> GetAllAnswers(int start);
        IEnumerable<AnswerEntity> GetAllAnswers(string search);
        IEnumerable<AnswerEntity> GetUserAnswers(string email, int start);
        IEnumerable<AnswerEntity> GetUserAnswers(string email, string search);
        AnswerEntity GetAnswerById(int id);
        TestEntity GetTestById(int id);
        TestTypeEntity GetTypeById(int id);
        void CreateTestType(TestTypeEntity testType);
        void CreateTest(TestEntity test);
        void CreateAnswer(AnswerEntity answer);
    }
}
