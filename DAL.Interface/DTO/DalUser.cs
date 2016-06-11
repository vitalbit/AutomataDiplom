using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalUser : IDalEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int? UniversityInfoId { get; set; }
        public int? RoleId { get; set; }
        public DalUniversityInfo UniversityInfo { get; set; }
        public DalRole Role { get; set; }
    }
}
