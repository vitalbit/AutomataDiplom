using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestTypeId { get; set; }
        public ICollection<TestFile> TestFiles { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public TestType TestType { get; set; }

        public Test()
        {
            this.TestFiles = new HashSet<TestFile>();
            this.Answers = new HashSet<Answer>();
        }
    }
}
