using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalUniversityInfo : IDalEntity
    {
        public int Id { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public string Speciality { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
