using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class TestFile : IORMEntity
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public TestFile()
        {
            this.Tests = new HashSet<Test>();
        }
    }
}
