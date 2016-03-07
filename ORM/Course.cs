using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class Course : IORMEntity
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public Course()
        {
            this.Users = new HashSet<User>();
        }
    }
}
