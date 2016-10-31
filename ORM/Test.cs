using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class Test : IORMEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestTypeId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual TestType TestType { get; set; }

        public Test()
        {
            this.Answers = new HashSet<Answer>();
        }
    }
}
