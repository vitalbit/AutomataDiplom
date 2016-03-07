using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class AnswerEntity
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public int Mark { get; set; }
        public DateTime TestEndTime { get; set; }
        public int TestId { get; set; }
        public int UserId { get; set; }
    }
}
