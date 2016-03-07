using BLL.Interface.Entities;
using DAL.Interface.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappers
{
    public static class BllEntityMapper
    {
        public static DalUser ToDalUser(this UserEntity user)
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
                RoleId = user.RoleId
            };
        }

        public static UserEntity ToBllUser(this DalUser user)
        {
            return new UserEntity()
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

        public static DalAnswer ToDalAnswer(this AnswerEntity answer)
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

        public static AnswerEntity ToBllAnswer(this DalAnswer answer)
        {
            return new AnswerEntity()
            {
                Id = answer.Id,
                TestId = answer.TestId,
                UserId = answer.UserId,
                Mark = answer.Mark,
                Content = answer.Content,
                TestEndTime = answer.TestEndTime
            };
        }

        public static DalCourse ToDalCourse(this CourseEntity course)
        {
            return new DalCourse()
            {
                Id = course.Id,
                Number = course.Number
            };
        }

        public static CourseEntity ToBllCourse(this DalCourse course)
        {
            return new CourseEntity()
            {
                Id = course.Id,
                Number = course.Number
            };
        }

        public static DalFaculty ToDalFaculty(this FacultyEntity faculty)
        {
            return new DalFaculty()
            {
                Id = faculty.Id,
                Name = faculty.Name
            };
        }

        public static FacultyEntity ToBllFaculty(this DalFaculty faculty)
        {
            return new FacultyEntity()
            {
                Id = faculty.Id,
                Name = faculty.Name
            };
        }

        public static DalGroup ToDalGroup(this GroupEntity group)
        {
            return new DalGroup()
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public static GroupEntity ToBllGroup(this DalGroup group)
        {
            return new GroupEntity()
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public static DalRole ToDalRole(this RoleEntity role)
        {
            return new DalRole()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static RoleEntity ToBllRole(this DalRole role)
        {
            return new RoleEntity()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static DalSpeciality ToDalSpeciality(this SpecialityEntity speciality)
        {
            return new DalSpeciality()
            {
                Id = speciality.Id,
                Name = speciality.Name
            };
        }

        public static SpecialityEntity ToBllSpeciality(this DalSpeciality speciality)
        {
            return new SpecialityEntity()
            {
                Id = speciality.Id,
                Name = speciality.Name
            };
        }

        public static DalTest ToDalTest(this TestEntity test)
        {
            return new DalTest()
            {
                Id = test.Id,
                Name = test.Name,
                TestTypeId = test.TestTypeId
            };
        }

        public static TestEntity ToBllTest(this DalTest test)
        {
            return new TestEntity()
            {
                Id = test.Id,
                Name = test.Name,
                TestTypeId = test.TestTypeId
            };
        }

        public static DalTestFile ToDalTestFile(this TestFileEntity testFile)
        {
            return new DalTestFile()
            {
                Id = testFile.Id,
                Content = testFile.Content
            };
        }

        public static TestFileEntity ToBllTestFile(this DalTestFile testFile)
        {
            return new TestFileEntity()
            {
                Content = testFile.Content,
                Id = testFile.Id
            };
        }

        public static DalTestType ToDalTestType(this TestTypeEntity testType)
        {
            return new DalTestType()
            {
                Id = testType.Id,
                ModuleName = testType.ModuleName
            };
        }

        public static TestTypeEntity ToBllTestType(this DalTestType testType)
        {
            return new TestTypeEntity()
            {
                Id = testType.Id,
                ModuleName = testType.ModuleName
            };
        }
    }
}
