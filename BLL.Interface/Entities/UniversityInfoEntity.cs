using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class UniversityInfoEntity
    {
        public int Id { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public string Speciality { get; set; }
        public string AdditionalInfo { get; set; }

        public override string ToString()
        {
            return University + ", " + Faculty + ", курс: " + Course + ", группа: " + Group + ", " + Speciality;
        }
    }
}
