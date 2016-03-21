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
            if (user != null)
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
            return null;
        }

        public static UserEntity ToBllUser(this DalUser user)
        {
            if (user != null)
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
                    RoleId = user.RoleId,
                    Course = user.Course.ToBllCourse(),
                    Faculty = user.Faculty.ToBllFaculty(),
                    Group = user.Group.ToBllGroup(),
                    Role = user.Role.ToBllRole(),
                    Speciality = user.Speciality.ToBllSpeciality()
                };
            }
            return null;
        }

        public static DalAnswer ToDalAnswer(this AnswerEntity answer)
        {
            if (answer != null)
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
            return null;
        }

        public static AnswerEntity ToBllAnswer(this DalAnswer answer)
        {
            if (answer != null)
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
            return null;
        }

        public static DalCourse ToDalCourse(this CourseEntity course)
        {
            if (course != null)
            {
                return new DalCourse()
                {
                    Id = course.Id,
                    Name = course.Name
                };
            }
            return null;
        }

        public static CourseEntity ToBllCourse(this DalCourse course)
        {
            if (course != null)
            {
                return new CourseEntity()
                {
                    Id = course.Id,
                    Name = course.Name
                };
            }
            return null;
        }

        public static DalFaculty ToDalFaculty(this FacultyEntity faculty)
        {
            if (faculty != null)
            {
                return new DalFaculty()
                {
                    Id = faculty.Id,
                    Name = faculty.Name
                };
            }
            return null;
        }

        public static FacultyEntity ToBllFaculty(this DalFaculty faculty)
        {
            if (faculty != null)
            {
                return new FacultyEntity()
                {
                    Id = faculty.Id,
                    Name = faculty.Name
                };
            }
            return null;
        }

        public static DalGroup ToDalGroup(this GroupEntity group)
        {
            if (group != null)
            {
                return new DalGroup()
                {
                    Id = group.Id,
                    Name = group.Name
                };
            }
            return null;
        }

        public static GroupEntity ToBllGroup(this DalGroup group)
        {
            if (group != null)
            {
                return new GroupEntity()
                {
                    Id = group.Id,
                    Name = group.Name
                };
            }
            return null;
        }

        public static DalRole ToDalRole(this RoleEntity role)
        {
            if (role != null)
            {
                return new DalRole()
                {
                    Id = role.Id,
                    Name = role.Name
                };
            }
            return null;
        }

        public static RoleEntity ToBllRole(this DalRole role)
        {
            if (role != null)
            {
                return new RoleEntity()
                {
                    Id = role.Id,
                    Name = role.Name
                };
            }
            return null;
        }

        public static DalSpeciality ToDalSpeciality(this SpecialityEntity speciality)
        {
            if (speciality != null)
            {
                return new DalSpeciality()
                {
                    Id = speciality.Id,
                    Name = speciality.Name
                };
            }
            return null;
        }

        public static SpecialityEntity ToBllSpeciality(this DalSpeciality speciality)
        {
            if (speciality != null)
            {
                return new SpecialityEntity()
                {
                    Id = speciality.Id,
                    Name = speciality.Name
                };
            }
            return null;
        }

        public static DalTest ToDalTest(this TestEntity test)
        {
            if (test != null)
            {
                return new DalTest()
                {
                    Id = test.Id,
                    Name = test.Name,
                    TestTypeId = test.TestTypeId,
                    TestFiles = test.TestFiles != null ? test.TestFiles.Select(ent => ent.ToDalTestFile()) : new List<DalTestFile>()
                };
            }
            return null;
        }

        public static TestEntity ToBllTest(this DalTest test)
        {
            if (test != null)
            {
                return new TestEntity()
                {
                    Id = test.Id,
                    Name = test.Name,
                    TestTypeId = test.TestTypeId,
                    TestFiles = test.TestFiles != null ? test.TestFiles.Select(ent => ent.ToBllTestFile()) : new List<TestFileEntity>()
                };
            }
            return null;
        }

        public static DalTestFile ToDalTestFile(this TestFileEntity testFile)
        {
            if (testFile != null)
            {
                return new DalTestFile()
                {
                    Id = testFile.Id,
                    Content = testFile.Content,
                    FileName = testFile.FileName
                };
            }
            return null;
        }

        public static TestFileEntity ToBllTestFile(this DalTestFile testFile)
        {
            if (testFile != null)
            {
                return new TestFileEntity()
                {
                    Content = testFile.Content,
                    Id = testFile.Id,
                    FileName = testFile.FileName
                };
            }
            return null;
        }

        public static DalTestType ToDalTestType(this TestTypeEntity testType)
        {
            if (testType != null)
            {
                return new DalTestType()
                {
                    Id = testType.Id,
                    ModuleName = testType.ModuleName
                };
            }
            return null;
        }

        public static TestTypeEntity ToBllTestType(this DalTestType testType)
        {
            if (testType != null)
            {
                return new TestTypeEntity()
                {
                    Id = testType.Id,
                    ModuleName = testType.ModuleName
                };
            }
            return null;
        }

        public static DalMaterial ToDalMaterial(this MaterialEntity material)
        {
            if (material != null)
            {
                return new DalMaterial()
                {
                    Content = material.Content,
                    Description = material.Description,
                    FileName = material.FileName,
                    Id = material.Id
                };
            }
            return null;
        }

        public static MaterialEntity ToBllMaterial(this DalMaterial material)
        {
            if (material != null)
            {
                return new MaterialEntity()
                {
                    Content = material.Content,
                    Description = material.Description,
                    FileName = material.FileName,
                    Id = material.Id
                };
            }
            return null;
        }
    }
}
