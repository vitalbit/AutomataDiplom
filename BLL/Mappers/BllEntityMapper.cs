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
                    UniversityInfoId = user.UniversityInfoId,
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
                    UniversityInfoId = user.UniversityInfoId,
                    RoleId = user.RoleId,
                    Role = user.Role.ToBllRole(),
                    UniversityInfo = user.UniversityInfo.ToBllUniversityInfo()
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
                    ModuleName = testType.ModuleName,
                    CssFileName = testType.CssFileName,
                    JsFileName = testType.JsFileName,
                    DllFileName = testType.DllFileName,
                    ResolveDllType = testType.ResolveDllType
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
                    ModuleName = testType.ModuleName,
                    CssFileName = testType.CssFileName,
                    JsFileName = testType.JsFileName,
                    DllFileName = testType.DllFileName,
                    ResolveDllType = testType.ResolveDllType
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

        public static DalUniversityInfo ToDalUniversityInfo(this UniversityInfoEntity universityInfo)
        {
            if (universityInfo != null)
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
            return null;
        }

        public static UniversityInfoEntity ToBllUniversityInfo(this DalUniversityInfo universityInfo)
        {
            if (universityInfo != null)
            {
                return new UniversityInfoEntity()
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
            return null;
        }
    }
}
