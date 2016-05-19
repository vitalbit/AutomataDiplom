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
            else if (entity is Course)
                return (entity as Course).ToDalCourse();
            else if (entity is Faculty)
                return (entity as Faculty).ToDalFaculty();
            else if (entity is Group)
                return (entity as Group).ToDalGroup();
            else if (entity is Role)
                return (entity as Role).ToDalRole();
            else if (entity is Speciality)
                return (entity as Speciality).ToDalSpeciality();
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
            else if (entity is DalCourse)
                return (entity as DalCourse).ToOrmCourse();
            else if (entity is DalFaculty)
                return (entity as DalFaculty).ToOrmFaculty();
            else if (entity is DalGroup)
                return (entity as DalGroup).ToOrmGroup();
            else if (entity is DalRole)
                return (entity as DalRole).ToOrmRole();
            else if (entity is DalSpeciality)
                return (entity as DalSpeciality).ToOrmSpeciality();
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
                CourseId = user.CourseId,
                GroupId = user.GroupId,
                SpecialityId = user.SpecialityId,
                FacultyId = user.FacultyId,
                RoleId = user.RoleId,
                Course = (DalCourse)user.Course.ToDal(),
                Faculty = (DalFaculty)user.Faculty.ToDal(),
                Group = (DalGroup)user.Group.ToDal(),
                Role = (DalRole)user.Role.ToDal(),
                Speciality = (DalSpeciality)user.Speciality.ToDal()
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
                CourseId = user.CourseId,
                GroupId = user.GroupId,
                SpecialityId = user.SpecialityId,
                FacultyId = user.FacultyId,
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

        public static DalCourse ToDalCourse(this Course course)
        {
            return new DalCourse()
            {
                Id = course.Id,
                Name = course.Name
            };
        }

        public static Course ToOrmCourse(this DalCourse course)
        {
            return new Course()
            {
                Id = course.Id,
                Name = course.Name
            };
        }

        public static DalFaculty ToDalFaculty(this Faculty faculty)
        {
            return new DalFaculty()
            {
                Id = faculty.Id,
                Name = faculty.Name
            };
        }

        public static Faculty ToOrmFaculty(this DalFaculty faculty)
        {
            return new Faculty()
            {
                Id = faculty.Id,
                Name = faculty.Name
            };
        }

        public static DalGroup ToDalGroup(this Group group)
        {
            return new DalGroup()
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public static Group ToOrmGroup(this DalGroup group)
        {
            return new Group()
            {
                Id = group.Id,
                Name = group.Name
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

        public static DalSpeciality ToDalSpeciality(this Speciality speciality)
        {
            return new DalSpeciality()
            {
                Id = speciality.Id,
                Name = speciality.Name
            };
        }

        public static Speciality ToOrmSpeciality(this DalSpeciality speciality)
        {
            return new Speciality()
            {
                Id = speciality.Id,
                Name = speciality.Name
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
                DllFileName = testType.DllFileName
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
                DllFileName = testType.DllFileName
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
            ormUser.CourseId = dalUser.CourseId;
            ormUser.Email = dalUser.Email;
            ormUser.FacultyId = dalUser.FacultyId;
            ormUser.FirstName = dalUser.FirstName;
            ormUser.GroupId = dalUser.GroupId;
            ormUser.LastName = dalUser.LastName;
            ormUser.Password = dalUser.Password;
            ormUser.RoleId = dalUser.RoleId;
            ormUser.SpecialityId = dalUser.SpecialityId;
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
