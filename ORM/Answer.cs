using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class Answer
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public DateTime TestEndTime { get; set; }
        public int TestId { get; set; }
        public int UserId { get; set; }
        public virtual Test Test { get; set; }
        public virtual User User { get; set; }

        public Answer()
        {
            this.TestEndTime = DateTime.Now;
        }

        public bool IsTimeEnd
        {
            get { return DateTime.Now >= this.TestEndTime; }
        }
    }
}
