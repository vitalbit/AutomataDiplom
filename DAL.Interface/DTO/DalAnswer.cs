using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalAnswer : IDalEntity
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public int Mark { get; set; }
        public DateTime TestEndTime { get; set; }
        public int TestId { get; set; }
        public int UserId { get; set; }

        public bool IsTimeEnd
        {
            get { return DateTime.Now >= this.TestEndTime; }
        }
    }
}
