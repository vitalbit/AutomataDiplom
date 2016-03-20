using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int? CourseId { get; set; }
        public int? GroupId { get; set; }
        public int? SpecialityId { get; set; }
        public int? FacultyId { get; set; }
        public int? RoleId { get; set; }
        public CourseEntity Course { get; set; }
        public GroupEntity Group { get; set; }
        public SpecialityEntity Speciality { get; set; }
        public FacultyEntity Faculty { get; set; }
        public RoleEntity Role { get; set; }
    }
}
