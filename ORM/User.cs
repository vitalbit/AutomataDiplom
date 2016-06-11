using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class User : IORMEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int? UniversityInfoId { get; set; }
        public int? RoleId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual UniversityInfo UniversityInfo { get; set; }
        public virtual Role Role { get; set; }
        public User()
        {
            Answers = new HashSet<Answer>();
        }
    }
}
