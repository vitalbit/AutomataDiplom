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
        public string CssFileName { get; set; }
        public string JsFileName { get; set; }
        public string DllFileName { get; set; }
        public string ResolveDllType { get; set; }
        public virtual ICollection<Test> Tests { get; set; }

        public TestType()
        {
            this.Tests = new HashSet<Test>();
        }
    }
}
