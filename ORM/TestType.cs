using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class TestType : IORMEntity
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public virtual ICollection<Test> Tests { get; set; }

        public TestType()
        {
            this.Tests = new HashSet<Test>();
        }
    }
}
