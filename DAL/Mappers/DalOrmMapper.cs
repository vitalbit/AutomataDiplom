using DAL.Interface.DTO;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappers
{
    public static class DalOrmMapper
    {
        #region Mappers
        public static IDalEntity ToDal(this IORMEntity entity)
        {
            if (entity is Answer)
                return (entity as Answer).ToDalAnswer();
            else if (entity is UniversityInfo)
                return (entity as UniversityInfo).ToDalUniversityInfo();
            else if (entity is Role)
                return (entity as Role).ToDalRole();
            else if (entity is Test)
                return (entity as Test).ToDalTest();
            else if (entity is TestFile)
                return (entity as TestFile).ToDalTestFile();
            else if (entity is TestType)
                return (entity as TestType).ToDalTestType();
            else if (entity is User)
                return (entity as User).ToDalUser();
            else if (entity is Material)
                return (entity as Material).ToDalMaterial();
            else
                return null;
        }

        public static IORMEntity ToOrm(this IDalEntity entity)
        {
            if (entity is DalAnswer)
                return (entity as DalAnswer).ToOrmAnswer();
            else if (entity is DalUniversityInfo)
                return (entity as DalUniversityInfo).ToOrmUniversityInfo();
            else if (entity is DalRole)
                return (entity as DalRole).ToOrmRole();
            else if (entity is DalTest)
                return (entity as DalTest).ToOrmTest();
            else if (entity is DalTestFile)
                return (entity as DalTestFile).ToOrmTestFile();
            else if (entity is DalTestType)
                return (entity as DalTestType).ToOrmTestType();
            else if (entity is DalUser)
                return (entity as DalUser).ToOrmUser();
            else if (entity is DalMaterial)
                return (entity as DalMaterial).ToOrmMaterial();
            else
                return null;
        }

        public static DalUser ToDalUser(this User user)
        {
            return new DalUser()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Email = user.Email,
                UniversityInfoId = user.UniversityInfoId,
                RoleId = user.RoleId,
                UniversityInfo = (DalUniversityInfo)user.UniversityInfo.ToDal(),
                Role = (DalRole)user.Role.ToDal()
            };
        }

        public static User ToOrmUser(this DalUser user)
        {
            return new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Email = user.Email,
                UniversityInfoId = user.UniversityInfoId,
                RoleId = user.RoleId
            };
        }

        public static DalAnswer ToDalAnswer(this Answer answer)
        {
            return new DalAnswer()
            {
                Id = answer.Id,
                TestId = answer.TestId,
                UserId = answer.UserId,
                Mark = answer.Mark,
                Content = answer.Content,
                TestEndTime = answer.TestEndTime
            };
        }

        public static Answer ToOrmAnswer(this DalAnswer answer)
        {
            return new Answer()
            {
                Id = answer.Id,
                TestId = answer.TestId,
                UserId = answer.UserId,
                Mark = answer.Mark,
                Content = answer.Content,
                TestEndTime = answer.TestEndTime
            };
        }

        public static DalRole ToDalRole(this Role role)
        {
            return new DalRole()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static Role ToOrmRole(this DalRole role)
        {
            return new Role()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static DalTest ToDalTest(this Test test)
        {
            return new DalTest()
            {
                Id = test.Id,
                Name = test.Name,
                TestFiles = test.TestFiles != null ? test.TestFiles.Select(ent => ent.ToDalTestFile()) : new List<DalTestFile>(),
                TestTypeId = test.TestTypeId
            };
        }

        public static Test ToOrmTest(this DalTest test)
        {
            return new Test()
            {
                Id = test.Id,
                Name = test.Name,
                TestFiles = test.TestFiles != null ? test.TestFiles.Select(ent => ent.ToOrmTestFile()).ToList() : new List<TestFile>(),
                TestTypeId = test.TestTypeId
            };
        }

        public static DalTestFile ToDalTestFile(this TestFile testFile)
        {
            return new DalTestFile()
            {
                Id = testFile.Id,
                Content = testFile.Content,
                FileName = testFile.FileName,
                Tests = testFile.Tests != null ? testFile.Tests.Select(ent => ent.ToDalTest()) : new List<DalTest>()
            };
        }

        public static TestFile ToOrmTestFile(this DalTestFile testFile)
        {
            return new TestFile()
            {
                Id = testFile.Id,
                Content = testFile.Content,
                FileName = testFile.FileName,
                Tests = testFile.Tests != null ? testFile.Tests.Select(ent => ent.ToOrmTest()).ToList() : new List<Test>()
            };
        }

        public static DalTestType ToDalTestType(this TestType testType)
        {
            return new DalTestType()
            {
                Id = testType.Id,
                ModuleName = testType.ModuleName,
                CssFileName = testType.CssFileName,
                JsFileName = testType.JsFileName,
                DllFileName = testType.DllFileName,
                ResolveDllType = testType.ResolveDllType
            };
        }

        public static TestType ToOrmTestType(this DalTestType testType)
        {
            return new TestType()
            {
                Id = testType.Id,
                ModuleName = testType.ModuleName,
                CssFileName = testType.CssFileName,
                JsFileName = testType.JsFileName,
                DllFileName = testType.DllFileName,
                ResolveDllType = testType.ResolveDllType
            };
        }

        public static DalMaterial ToDalMaterial(this Material material)
        {
            return new DalMaterial()
            {
                Content = material.Content,
                Description = material.Description,
                FileName = material.FileName,
                Id = material.Id
            };
        }

        public static Material ToOrmMaterial(this DalMaterial material)
        {
            return new Material()
            {
                Content = material.Content,
                Description = material.Description,
                FileName = material.FileName,
                Id = material.Id
            };
        }

        public static DalUniversityInfo ToDalUniversityInfo(this UniversityInfo universityInfo)
        {
            return new DalUniversityInfo()
            {
                AdditionalInfo = universityInfo.AdditionalInfo,
                Course = universityInfo.Course,
                Faculty = universityInfo.Faculty,
                Group = universityInfo.Group,
                Id = universityInfo.Id,
                Speciality = universityInfo.Speciality,
                University = universityInfo.University
            };
        }

        public static UniversityInfo ToOrmUniversityInfo(this DalUniversityInfo universityInfo)
        {
            return new UniversityInfo()
            {
                AdditionalInfo = universityInfo.AdditionalInfo,
                Course = universityInfo.Course,
                Faculty = universityInfo.Faculty,
                Group = universityInfo.Group,
                Id = universityInfo.Id,
                Speciality = universityInfo.Speciality,
                University = universityInfo.University
            };
        }
        #endregion
        #region Copy

        public static void CopyToOrm(this IDalEntity dal, IORMEntity orm, DbContext context)
        {
            if (dal is DalUser && orm is User)
                (dal as DalUser).CopyToOrmUser((User)orm, context);
            //else if (dal is DalTest && orm is Test)
            //    (dal as DalTest).CopyToOrmTest((Test)orm, context);
        }

        public static void CopyToOrmUser(this DalUser dalUser, User ormUser, DbContext context)
        {
            ormUser.Email = dalUser.Email;
            ormUser.FirstName = dalUser.FirstName;
            ormUser.LastName = dalUser.LastName;
            ormUser.Password = dalUser.Password;
            ormUser.RoleId = dalUser.RoleId;
            ormUser.UniversityInfoId = dalUser.UniversityInfoId;
        }

        //public static void CopyToOrmTest(this DalTest dalTest, Test ormTest, DbContext context)
        //{
        //    List<Answer> answers = new List<Answer>();
        //    foreach (var answer in dalTest.Answers)
        //    {
        //        Answer answ = context.Set<Answer>().FirstOrDefault(ent => ent.Id == answer.Id);
        //        if (answ != null)
        //            answers.Add(answ);
        //        else
        //            answers.Add(answer.ToOrmAnswer());
        //    }
        //    ormTest.Answers = answers;

        //    List<AttachmentContent> contents = new List<AttachmentContent>();
        //    foreach (var content in dalTest.AttachmentContents)
        //    {
        //        AttachmentContent cont = context.Set<AttachmentContent>().FirstOrDefault(ent => ent.Id == content.Id);
        //        if (cont != null)
        //            contents.Add(cont);
        //        else
        //            contents.Add(content.ToOrmAttachmentContent());
        //    }
        //    ormTest.AttachmentContents = contents;
        //    ormTest.Name = dalTest.Name;
        //    ormTest.TestCount = dalTest.TestCount;
        //    ormTest.TestTime = dalTest.TestTime;
        //}
        #endregion
    }
}
