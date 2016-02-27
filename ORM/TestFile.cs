using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class TestFile
    {
        public int id { get; set; }
        public byte[] Content { get; set; }
        public ICollection<Test> Tests { get; set; }
        public TestFile()
        {
            this.Tests = new HashSet<Test>();
        }
    }
}
