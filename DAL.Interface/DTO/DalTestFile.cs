using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalTestFile : IDalEntity
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public IEnumerable<DalTest> Tests { get; set; }
    }
}
