using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class User
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
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Course Course { get; set; }
        public virtual Group Group { get; set; }
        public virtual Speciality Speciality { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Role Role { get; set; }
        public User()
        {
            Answers = new HashSet<Answer>();
        }
    }
}
